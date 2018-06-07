using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Npgsql;
using System.Data;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class Transactions
    {
        private NpgsqlConnection oConnTuart;
        public Transactions()
        {
            oConnTuart = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ConnectionTuartdb"].ConnectionString);
        }
        public DataTable ReadingQuery(string query, List<NpgsqlParameter> parametros = null)
        {
            DataTable dtRetorno = new DataTable();
            try
            {
                NpgsqlCommand comando = new NpgsqlCommand(query, oConnTuart);
                comando.CommandType = CommandType.Text;
                if (parametros != null)
                    foreach (NpgsqlParameter param in parametros)
                        comando.Parameters.Add(param);

                NpgsqlDataAdapter _sqlDa = new NpgsqlDataAdapter(comando);
                _sqlDa.Fill(dtRetorno);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dtRetorno;
        }

        public string ReadingEscalarQuery(string query, List<NpgsqlParameter> parametros = null)
        {
            string valorRetorno = string.Empty;
            try
            {
                NpgsqlCommand comando = new NpgsqlCommand(query, oConnTuart);
                comando.CommandType = CommandType.Text;
                if (parametros != null)
                    foreach (NpgsqlParameter param in parametros)
                        comando.Parameters.Add(param);

                oConnTuart.Open();
                valorRetorno = comando.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                oConnTuart.Close();
            }
            return valorRetorno;
        }
    }
}
