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

        /// <summary>
        /// Crea nueva instancia de conexión a la cadena: ConnectionOdoo, del archivo de configuración (WebConfig).
        /// </summary>
        public Transactions()
        {
            oConnOdoo = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ConnectionOdoo"].ConnectionString);
        }

        /// <summary>
        /// Ejecuta una consulta de lectura en formato de cadena, directamente a la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="query">Consulta de formato de cadena</param>
        /// <param name="parametros">Lista Genérica de tipo NpgsqlParameter, no obligatorio</param>
        /// <returns>Contenedor de datos de tipo DataTable, con el retorno de la consulta</returns>
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
