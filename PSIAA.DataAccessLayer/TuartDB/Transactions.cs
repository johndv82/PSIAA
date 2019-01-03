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

        /// <summary>
        /// Crea nueva instancia de conexión a la cadena: ConnectionTuartdb, del archivo de configuración (WebConfig).
        /// </summary>
        public Transactions()
        {
            oConnTuart = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ConnectionTuartdb"].ConnectionString);
        }

        /// <summary>
        /// Ejecuta una consulta de lectura en formato de cadena, directamente en la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="query">Cadena de Consulta</param>
        /// <param name="parametros">Lista Genérica de tipo NpgsqlParameter</param>
        /// <returns>Contenedor de tipo DataTable con el retorno de la consulta</returns>
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

        /// <summary>
        /// Ejecuta una consulta de lectura escalar en formato de cadena, directamente en la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="query">Cadena de Consulta</param>
        /// <param name="parametros">Lista Genérica de tipo NpgsqlParameter</param>
        /// <returns>Variable de tipo string con el valor de retorno.</returns>
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
