using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class TransactionsSAP
    {
        private SqlConnection oConnSap;
        public TransactionsSAP()
        {
            oConnSap = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionSAP"].ConnectionString);
        }
        public DataTable ReadingQuery(string query, List<SqlParameter> parametros = null)
        {
            DataTable dtRetorno = new DataTable();
            try
            {
                SqlCommand comando = new SqlCommand(query, oConnSap);
                comando.CommandType = CommandType.Text;
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
    }
}
