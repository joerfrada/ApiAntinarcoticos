using System;
using System.Collections.Generic;
using System.Data;
using Core.Entidades;
using Core.Servicios;
using Core.Utilidades;
using Oracle.ManagedDataAccess.Client;

namespace Core.Negocios
{
    public class ValorFlexibleManager : IValorFlexibleManager
    {
        private IDataAccess dataAccess;

        public ValorFlexibleManager(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public MensajeEntity CrudTipoValores(TipoValorEntity request, string evento)
        {
            List<OracleParameter> param = new List<OracleParameter>();

            try
            {
                param.Add(new OracleParameter() { ParameterName = "p_evento", Value = evento, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_id_tipo_valor", Value = request.Id_tipo_valor, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.InputOutput });
                param.Add(new OracleParameter() { ParameterName = "p_tipo", Value = request.Tipo, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_descripcion", Value = request.Descripcion.IsNullOrEmpty() ? (object)DBNull.Value : request.Descripcion, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_activo", Value = request.Activo, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_id_tipo_valor_padre", Value = request.Id_tipo_valor_padre == 0 ? (object)DBNull.Value : request.Id_tipo_valor_padre, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_cod", OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Output });
                param.Add(new OracleParameter() { ParameterName = "p_msg", OracleDbType = OracleDbType.Varchar2, Size = 4000, Direction = ParameterDirection.Output });

                OracleCommand resultexec = dataAccess.ExecuteProcedure(param, "pkg_api_param.pr_crud_adm_tipos_valores");

                Int64 p_id = evento == "C" ? Int64.Parse(resultexec.Parameters["p_id_tipo_valor"].Value.ToString()) : 0;
                Int64 p_codigo = Int64.Parse(resultexec.Parameters["p_cod"].Value.ToString());
                string msg = resultexec.Parameters["p_msg"].Value.ToString();

                return new MensajeEntity() { Id = p_id, Tipo = TipoMensaje.SUCCESS, Mensaje = msg };
            }
            catch (Exception ex)
            {
                throw new Exception("Error CrudTipoValores: " + ex.Message);
            }
        }

        public List<TipoValorEntity> GetTipoValores(RequestEntity request, ref long pTotal)
        {
            List<OracleParameter> param = new List<OracleParameter>();

            try
            {
                param.Add(new OracleParameter() { ParameterName = "p_numero_pagina", Value = request.Numero_pagina, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_numero_fila", Value = request.Numero_fila, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_data_text", Value = request.Data_text, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "c_total", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });
                param.Add(new OracleParameter() { ParameterName = "c_tipo", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });

                DataSet ds = dataAccess.ExecuteResultDataSet(param, "pkg_api_param.pr_get_adm_tipos_valores");

                pTotal = Int64.Parse(ds.Tables[0].Rows[0]["total"].ToString());

                DataTable dt = ds.Tables[1];

                List<TipoValorEntity> lstDatos = new List<TipoValorEntity>();

                foreach (DataRow dr in dt.Rows)
                {
                    TipoValorEntity item = new TipoValorEntity();
                    item.Id_tipo_valor = dr["id_tipo_valor"] == DBNull.Value ? 0 : Int64.Parse(dr["id_tipo_valor"].ToString());
                    item.Tipo = dr["tipo"].ToString();
                    item.Descripcion = dr["descripcion"].ToString();
                    item.Activo = dr["activo"] == DBNull.Value ? false : Boolean.Parse(dr["activo"].ToString());
                    item.Id_tipo_valor_padre = dr["id_tipo_valor_padre"] == DBNull.Value ? 0 : Int64.Parse(dr["id_tipo_valor_padre"].ToString());
                    item.Tìpo_valor_padre = dr["tipo_valor_padre"].ToString();
                    item.Usuario_creador = dr["usuario_creador"].ToString();
                    item.Fecha_creacion = DateTime.Parse(dr["fecha_creacion"].ToString());
                    item.Usuario_modificador = dr["usuario_modificador"].ToString();
                    item.Fecha_modificacion = dr["fecha_modificacion"] == (object)DBNull.Value ? null : DateTime.Parse(dr["fecha_modificacion"].ToString());
                    item.Tabla = dr["tabla"].ToString();

                    lstDatos.Add(item);
                }

                return lstDatos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error GetTipoValores: " + ex.Message);
            }
        }

        public MensajeEntity CrudValoresFlexibles(ValorFlexibleEntity request, string evento)
        {
            List<OracleParameter> param = new List<OracleParameter>();

            try            
            {
                param.Add(new OracleParameter() { ParameterName = "p_evento", Value = evento, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_id_valor_flexible", Value = request.Id_valor_flexible, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.InputOutput });
                param.Add(new OracleParameter() { ParameterName = "p_valor", Value = request.Valor.ToUpper(), OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_sigla", Value = request.Sigla.IsNullOrEmpty() ? (object)DBNull.Value : request.Sigla, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_orden", Value = request.Orden == 0 ? (object)DBNull.Value : request.Orden, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_activo", Value = request.Activo, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_id_valor_flexible_padre", Value = request.Id_valor_flexible_padre == 0 ? (object)DBNull.Value : request.Id_valor_flexible_padre, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_atributo1", Value = request.Atributo1.IsNullOrEmpty() ? (object)DBNull.Value : request.Atributo1, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_atributo2", Value = request.Atributo2.IsNullOrEmpty() ? (object)DBNull.Value : request.Atributo2, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_atributo3", Value = request.Atributo3.IsNullOrEmpty() ? (object)DBNull.Value : request.Atributo3, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_atributo4", Value = request.Atributo4.IsNullOrEmpty() ? (object)DBNull.Value : request.Atributo4, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_id_tipo_valor", Value = request.Id_tipo_valor, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_tipo", Value = request.Tipo, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });

                OracleCommand resultexec = dataAccess.ExecuteProcedure(param, "pkg_api_param.pr_crud_adm_valores_flexibles");

                Int64 p_id = evento == "C" ? Int64.Parse(resultexec.Parameters["p_id_valor_flexible"].Value.ToString()) : 0;
                string msg = evento == "C" ? "creado" : "actualizado";

                return new MensajeEntity() { Id = p_id, Tipo = TipoMensaje.SUCCESS, Mensaje = string.Format("El registro ha {0} con éxito.", msg) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error CrudTipoValores: " + ex.Message);
            }
        }

        public List<ValorFlexibleEntity> GetValoresFlexibles(RequestEntity request, ref long pTotal)
        {
            List<OracleParameter> param = new List<OracleParameter>();

            try
            {
                param.Add(new OracleParameter() { ParameterName = "p_numero_pagina", Value = request.Numero_pagina, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_numero_fila", Value = request.Numero_fila, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "p_id_tipo_valor", Value = request.Id, OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.InputOutput });
                param.Add(new OracleParameter() { ParameterName = "p_data_text", Value = request.Data_text, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "c_total", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });
                param.Add(new OracleParameter() { ParameterName = "c_valor", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });

                DataSet ds = dataAccess.ExecuteResultDataSet(param, "pkg_api_param.pr_get_adm_valores_flexibles");

                pTotal = Int64.Parse(ds.Tables[0].Rows[0]["total"].ToString());

                DataTable dt = ds.Tables[1];

                List<ValorFlexibleEntity> lstDatos = new List<ValorFlexibleEntity>();

                foreach (DataRow dr in dt.Rows)
                {
                    ValorFlexibleEntity item = new ValorFlexibleEntity();
                    item.Id_valor_flexible = dr["id_valor_flexible"] == DBNull.Value ? 0 : Int64.Parse(dr["id_valor_flexible"].ToString());
                    item.Valor = dr["valor"].ToString();
                    item.Sigla = dr["sigla"].ToString();
                    item.Orden = Int64.Parse(dr["orden"].ToString());
                    item.Activo = dr["activo"] == DBNull.Value ? false : Boolean.Parse(dr["activo"].ToString());
                    item.Id_valor_flexible_padre = Int64.Parse(dr["id_valor_flexible_padre"].ToString());
                    item.Valor_flexible_padre = dr["valor_flexible_padre"].ToString();
                    item.Atributo1 = dr["atributo1"].ToString();
                    item.Atributo2 = dr["atributo2"].ToString();
                    item.Atributo3 = dr["atributo3"].ToString();
                    item.Atributo4 = dr["atributo4"].ToString();
                    item.Id_tipo_valor = Int64.Parse(dr["id_tipo_valor"].ToString());
                    item.Tipo = dr["tipo"].ToString();
                    item.Usuario_creador = dr["usuario_creador"].ToString();
                    item.Fecha_creacion = DateTime.Parse(dr["fecha_creacion"].ToString());
                    item.Usuario_modificador = dr["usuario_modificador"].ToString();
                    item.Fecha_modificacion = dr["fecha_modificacion"] == (object)DBNull.Value ? null : DateTime.Parse(dr["fecha_modificacion"].ToString());
                    item.Tabla = dr["tabla"].ToString();

                    lstDatos.Add(item);
                }

                return lstDatos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error GetTipoValores: " + ex.Message);
            }
        }
    }
}
