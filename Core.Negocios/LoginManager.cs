using System;
using System.Collections.Generic;
using System.Data;
using Core.Entidades;
using Core.Servicios;
using Oracle.ManagedDataAccess.Client;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Core.Negocios
{
    public class LoginManager : ILoginManager
    {
        private IDataAccess dataAccess;
        private IParametroSistemaManager psMgr;
        private TipoAmbiente tipo = AmbienteEntity.GetTipoAmbiente();

        public LoginManager(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public MensajeEntity Login(string pUsuario, string pPassword)
        {
            List<OracleParameter> param = new List<OracleParameter>();
            MensajeEntity msg = new MensajeEntity();
            string mensaje = string.Empty;
            string error = string.Empty;

            if (tipo == TipoAmbiente.Produccion || tipo == TipoAmbiente.Pruebas)
            {
                psMgr = new ParametroSistemaManager(dataAccess);

                try
                {
                    ParametroSistemaEntity dato = psMgr.GetParametros("URLOID");
                    string host = dato.Descripcion.Replace("//", "").Split(":")[1];
                    string dn = dato.Cadena;
                    int port = int.Parse(dato.Descripcion.Split(":")[2]);

                    bool autenticado = loginOUD(dn, host, port, pUsuario, pPassword, ref mensaje);

                    if (autenticado == true)
                    {
                        try
                        {
                            param.Add(new OracleParameter() { ParameterName = "p_usuario", Value = pUsuario.ToUpper(), OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
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
                        msg.Mensaje = mensaje;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Login: " + ex.Message);
                }
            }
            else if (tipo == TipoAmbiente.Desarrollo)
            {
                if (pPassword != null)
                {
                    bool isValid = dataAccess.IsValidCredentials(pUsuario.ToUpper(), ref error);
                    if (isValid == true)
                    {
                        try
                        {
                            param.Add(new OracleParameter() { ParameterName = "p_usuario", Value = pUsuario.ToUpper(), OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
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
                        msg.Mensaje = error;
                    }
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

        private Boolean loginOUD(string dn, string host, int port, string user, string pass, ref string mensaje)
        {
            bool autenticado = false;
            try
            {
                dn = "cn=" + user + dn;
                LdapDirectoryIdentifier ldi = new LdapDirectoryIdentifier(host, port);
                LdapConnection ldapConnection = new LdapConnection(ldi);
                ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.SessionOptions.ProtocolVersion = 3;
                NetworkCredential nc = new NetworkCredential(dn, pass);
                ldapConnection.Bind(nc);
                ldapConnection.Dispose();

                autenticado = true;
            }
            catch (LdapException e)
            {
                if (e.ErrorCode == 91)
                    mensaje = "No se puede conectar al servidor OUD. Contacte con su administrador.";
                else if (e.ErrorCode == 49)
                    mensaje = "Usuario y/o Contraseña Incorrecta.";
            }
            return autenticado;
        }
    }
}
