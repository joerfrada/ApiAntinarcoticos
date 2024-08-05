using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Core.Entidades;
using Core.Servicios;
using System.Security.Cryptography.Xml;

namespace Core.Dao
{
    public class DataAccess : IDataAccess
    {
        private string Usuario { get; set; }
        private string ConnectionString { get; set; }
        private IConfiguration configuration;
        private TipoAmbiente tipo = AmbienteEntity.GetTipoAmbiente();

        public DataAccess(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void setUsuario(string pUsuario)
        {
            this.Usuario = pUsuario;
        }

        public bool IsValidCredentials(string pUsuario)
        {
            if (tipo == TipoAmbiente.Desarrollo)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("DES_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Produccion)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRO_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Pruebas)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRU_XEPDB1").Value;
            }

            try
            {
                using (OracleConnection conn = new OracleConnection(this.ConnectionString))
                {

                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public OracleCommand ExecuteProcedure(List<OracleParameter> pParams, string pSqlProc)
        {
            if (tipo == TipoAmbiente.Desarrollo)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("DES_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Produccion)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRO_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Pruebas)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRU_XEPDB1").Value;
            }

            try
            {
                OracleConnection conn = new OracleConnection(this.ConnectionString);

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = pSqlProc;
                cmd.CommandType = CommandType.StoredProcedure;
                pParams.ForEach(iparam => cmd.Parameters.Add((OracleParameter)iparam));
                cmd.ExecuteNonQuery();

                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception("Error ExecuteProcedure: " + ex.Message);
            }
        }

        public async Task<OracleCommand> ExecuteProcedureAsync(List<OracleParameter> pParams, string pSqlProc)
        {
            if (tipo == TipoAmbiente.Desarrollo)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("DES_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Produccion)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRO_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Pruebas)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRU_XEPDB1").Value;
            }

            try
            {
                OracleConnection conn = new OracleConnection(this.ConnectionString);

                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync();

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = pSqlProc;
                cmd.CommandType = CommandType.StoredProcedure;
                pParams.ForEach(iparam => cmd.Parameters.Add((OracleParameter)iparam));
                await cmd.ExecuteNonQueryAsync();

                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception("Error ExecuteProcedureAsync: " + ex.Message);
            }
        }

        public DataSet ExecuteResultDataSet(List<OracleParameter> pParams, string pSqlProc)
        {
            DataSet ds = null;

            if (tipo == TipoAmbiente.Desarrollo)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("DES_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Produccion)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRO_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Pruebas)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRU_XEPDB1").Value;
            }

            try
            {
                using (OracleConnection conn = new OracleConnection(this.ConnectionString))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = pSqlProc;
                    cmd.CommandType = CommandType.StoredProcedure;

                    pParams.ForEach(iparam => cmd.Parameters.Add((OracleParameter)iparam));
                    cmd.ExecuteNonQuery();

                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataSet _ds = new DataSet();
                    adapter.Fill(_ds);

                    ds = _ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error ExecuteResultDataSet: " + ex.Message);
            }

            return ds;
        }

        public DataTable ExecuteResultTable(List<OracleParameter> pParams, string pSqlProc)
        {
            DataTable dt = null;

            if (tipo == TipoAmbiente.Desarrollo)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("DES_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Produccion)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRO_XEPDB1").Value;
            }
            else if (tipo == TipoAmbiente.Pruebas)
            {
                this.ConnectionString = "User Id=" + this.Usuario + configuration.GetSection("ConnectionStrings").GetSection("PRU_XEPDB1").Value;
            }

            try
            {
                using (OracleConnection conn = new OracleConnection(this.ConnectionString))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = pSqlProc;
                    cmd.CommandType = CommandType.StoredProcedure;

                    pParams.ForEach(iparam => cmd.Parameters.Add((OracleParameter)iparam));

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        dt = new DataTable();
                        dt.Load(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error ExecuteResultDataTable: " + ex.Message);
            }

            return dt;
        }
    }
}
