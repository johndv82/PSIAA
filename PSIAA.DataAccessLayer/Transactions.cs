using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PSIAA.DataAccessLayer
{
    public class Transactions
    {
        private SqlConnection oConnSia;
        public Transactions()
        {
            oConnSia = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionSIAA"].ConnectionString);
        }
        public DataTable ReadingQuery(string query, List<SqlParameter> parametros = null)
        {
            DataTable dtRetorno = new DataTable();
            try
            {
                SqlCommand comando = new SqlCommand(query, oConnSia);
                comando.CommandType = CommandType.Text;
                comando.CommandTimeout = 360;
                if (parametros != null)
                    foreach (SqlParameter param in parametros)
                        comando.Parameters.Add(param);

                SqlDataAdapter _sqlDa = new SqlDataAdapter(comando);
                _sqlDa.Fill(dtRetorno);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dtRetorno;
        }

        public string ReadingEscalarQuery(string query, List<SqlParameter> parametros = null)
        {
            string valorRetorno = string.Empty;
            try
            {
                SqlCommand comando = new SqlCommand(query, oConnSia);
                comando.CommandType = CommandType.Text;
                if (parametros != null)
                    foreach (SqlParameter param in parametros)
                        comando.Parameters.Add(param);

                oConnSia.Open();
                valorRetorno = comando.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                oConnSia.Close();
            }
            return valorRetorno;
        }

        public int ExecuteQuery(string query, List<SqlParameter> parametros)
        {
            int filasAfectadas = 0;
            SqlTransaction sqlTrans = null;
            try
            {
                oConnSia.Open();
                sqlTrans = oConnSia.BeginTransaction();
                SqlCommand comando = new SqlCommand(query, oConnSia);
                comando.CommandType = CommandType.Text;
                comando.Transaction = sqlTrans;
                if (parametros != null)
                    foreach (SqlParameter param in parametros)
                        comando.Parameters.Add(param);

                filasAfectadas = comando.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message.ToString());
                sqlTrans.Rollback();
            }
            finally {
                oConnSia.Close();
            }
            return filasAfectadas;
        }

        //TRANSACCION PARA REPORTES
        public DataTable ReadingProcedure(string nombreProcedure, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand(nombreProcedure, oConnSia);
            DataTable dtRetorno = new DataTable();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandTimeout = 360;
            if (parametros != null)
                foreach (SqlParameter param in parametros)
                    comando.Parameters.Add(param);
            try
            {
                oConnSia.Open();
                SqlDataReader reader = comando.ExecuteReader();
                dtRetorno.Load(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message.ToString());
                oConnSia.Close();
            }
            finally
            {
                oConnSia.Close();
            }
            return dtRetorno;
        }
    }
}
