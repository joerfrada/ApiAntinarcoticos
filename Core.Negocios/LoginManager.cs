using System;
using System.Collections.Generic;
using System.Data;
using Core.Entidades;
using Core.Servicios;
using Oracle.ManagedDataAccess.Client;

namespace Core.Negocios
{
    public class LoginManager : ILoginManager
    {
        private IDataAccess dataAccess;

        public LoginManager(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public MensajeEntity Login(string pUsuario, string pPassword)
        {
            List<OracleParameter> param = new List<OracleParameter>();
            MensajeEntity msg = new MensajeEntity();

            pUsuario = pUsuario.ToUpper();

            if (pPassword != null)
            {
                bool isValid = dataAccess.IsValidCredentials(pUsuario);
                if (isValid == true)
                {
                    try
                    {
                        param.Add(new OracleParameter() { ParameterName = "p_usuario", Value = pUsuario, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                        param.Add(new OracleParameter() { ParameterName = "c_usuario", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });

                        DataSet ds = dataAccess.ExecuteResultDataSet(param, "pq_api_seguridad.pr_login");
                        DataTable dt = ds.Tables[0];

                        UsuarioEntity usuario = new UsuarioEntity();
                        usuario.Usuario_id = Int64.Parse(dt.Rows[0]["usuario_id"].ToString());
                        usuario.Usuario = dt.Rows[0]["usuario"].ToString();
                        usuario.Nombre_completo = dt.Rows[0]["nombre_completo"].ToString();
                        usuario.Email_institucional = dt.Rows[0]["email_institucional"].ToString();
                        usuario.Activo = Convert.ToBoolean(dt.Rows[0]["activo"].ToString());
                        usuario.Menus = this.GetUsuarioMenuId(usuario.Usuario_id);

                        msg.Tipo = TipoMensaje.SUCCESS;
                        msg.Result = usuario;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error Login: " + ex.Message);
                    }
                }
                else
                {
                    msg.Tipo = TipoMensaje.ERROR;
                    msg.Mensaje = "El usuario no existe";
                }
            }

            return msg;
        }

        private List<MenuEntity> GetUsuarioMenuId(Int64 p_usuario_id)
        {
            List<OracleParameter> param = new List<OracleParameter>();

            try
            {
                param.Add(new OracleParameter() { ParameterName = "p_usuario_id", Value = p_usuario_id, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "c_menu", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });

                DataTable dt = dataAccess.ExecuteResultTable(param, "pq_api_seguridad.pr_get_usuarios_menu_id");

                List<MenuEntity> lstDatosTmp = new List<MenuEntity>();

                foreach (DataRow dr in dt.Rows)
                {
                    MenuEntity item = new MenuEntity();
                    item.Menu_id = Int64.Parse(dr["menu_id"].ToString());
                    item.Nombre_menu = dr["nombre_menu"].ToString();
                    item.Tipo_menu = dr["tipo_menu"].ToString();
                    item.Menu_padre_id = Int64.Parse(dr["menu_padre_id"].ToString());
                    item.Cod_modulo = dr["cod_modulo"].ToString();
                    item.Icono = dr["icono"].ToString();
                    item.Ruta = dr["ruta"].ToString();
                    item.Activo = dr["activo"].ToString() == "S" ? true : false;
                    item.Collapsed = true;

                    lstDatosTmp.Add(item);
                }

                List<MenuEntity> lstDatos = lstDatosTmp.Where(g => g.Menu_padre_id == 0).Select(g => new MenuEntity()
                {
                    Menu_id = g.Menu_id,
                    Nombre_menu = g.Nombre_menu,
                    Tipo_menu = g.Tipo_menu,
                    Menu_padre_id = g.Menu_padre_id,
                    Cod_modulo = g.Cod_modulo,
                    Icono = g.Icono,
                    Ruta = g.Ruta,
                    Activo = g.Activo,
                    Collapsed = g.Collapsed,
                    Menus = MenuRecursivo(lstDatosTmp, g.Menu_id)
                }).ToList();

                return lstDatos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error GetUsuarioMenuId: " + ex.Message);
            }
        }

        private List<MenuEntity> MenuRecursivo(List<MenuEntity> menus, Int64 parent_id)
        {
            return menus
                .Where(g => g.Menu_padre_id == parent_id)
                .Select(g => new MenuEntity()
                {
                    Menu_id = g.Menu_id,
                    Nombre_menu = g.Nombre_menu,
                    Tipo_menu = g.Tipo_menu,
                    Menu_padre_id = g.Menu_padre_id,
                    Cod_modulo = g.Cod_modulo,
                    Icono = g.Icono,
                    Ruta = g.Ruta,
                    Activo = g.Activo,
                    Collapsed = g.Collapsed,
                    Menus = MenuRecursivo(menus, g.Menu_id)
                }).ToList();
        }
    }
}
