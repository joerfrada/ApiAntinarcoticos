using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Core.Servicios
{
    public interface IDataAccess
    {
        public void setUsuario(string pUsuario);
        public bool IsValidCredentials(string pUsuario, ref string error);
        public OracleCommand ExecuteProcedure(List<OracleParameter> pParams, String pSqlProc);
        public Task<OracleCommand> ExecuteProcedureAsync(List<OracleParameter> pParams, String pSqlProc);
        public DataTable ExecuteResultTable(List<OracleParameter> pParams, String pSqlProc);
        public DataSet ExecuteResultDataSet(List<OracleParameter> pParams, String pSqlProc);
    }
}
