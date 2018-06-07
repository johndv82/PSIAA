using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Configuration;
using System.Data;

namespace PSIAA.DataAccessLayer.ODOO
{
    public class Transactions
    {
        private NpgsqlConnection oConnOdoo;
        public Transactions()
        {
            oConnOdoo = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ConnectionOdoo"].ConnectionString);
        }
        public DataTable ReadingQuery(string query, List<NpgsqlParameter> parametros = null)
        {
            DataTable dtRetorno = new DataTable();
            try
            {
                NpgsqlCommand comando = new NpgsqlCommand(query, oConnOdoo);
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
    }
}
