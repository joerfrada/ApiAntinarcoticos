using System;
using System.Collections.Generic;
using System.Data;
using Core.Entidades;
using Core.Servicios;
using Core.Utilidades;
using Oracle.ManagedDataAccess.Client;

namespace Core.Negocios
{
    public class ParametroSistemaManager : IParametroSistemaManager
    {
        private IDataAccess dataAccess;

        public ParametroSistemaManager(IDataAccess dataAccess) {
            this.dataAccess = dataAccess;
        }

        public ParametroSistemaEntity GetParametros(string pCodigo)
        {
            List<OracleParameter> param = new List<OracleParameter>();

            try
            {
                param.Add(new OracleParameter() { ParameterName = "p_codigo", Value = pCodigo, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
                param.Add(new OracleParameter() { ParameterName = "c_parametro", OracleDbType = OracleDbType.RefCursor, Direction = ParameterDirection.Output });

                DataSet ds = dataAccess.ExecuteResultDataSet(param, "pkg_bd_parametros.pr_get_parametros");
                DataTable dt = ds.Tables[0];

                ParametroSistemaEntity paramSistema = new ParametroSistemaEntity();
                paramSistema.Descripcion = dt.Rows[0]["descripcion"].ToString();
                paramSistema.Valor = dt.Rows[0]["valor"].ToString();
                paramSistema.Cadena = dt.Rows[0]["cadena"].ToString();

                return paramSistema;
            }
            catch (Exception ex)
            {
                throw new Exception("Error GetParametros: " + ex.Message);
            }
        }
    }
}
