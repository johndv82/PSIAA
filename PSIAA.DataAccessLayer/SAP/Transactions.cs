using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.SAP
{
    public class Transactions
    {
        private SqlConnection oConnSap;

        /// <summary>
        /// Crea nueva instancia de conexión a la cadena: ConnectionSAP, del archivo de configuración (WebConfig).
        /// </summary>
        public Transactions()
        {
            oConnSap = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionSAP"].ConnectionString);
        }

        /// <summary>
        /// Ejecuta una consulta de lectura en formato de cadena, directamente a la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="query">Cadena de Consulta</param>
        /// <param name="parametros">Lista Genérica de tipo SqlParameter</param>
        /// <returns>Contenedor de tipo DataTable con el retorno de la consulta.</returns>
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

        /// <summary>
        /// Ejecuta una consulta de lectura escalar en formato de cadena, directamente a la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="query">Cadena de Consulta</param>
        /// <param name="parametros">Lista Genérica de tipo SqlParameter</param>
        /// <returns>Variable de tipo string con el valor de retorno.</returns>
        public string ReadingEscalarQuery(string query, List<SqlParameter> parametros = null)
        {
            string valorRetorno = string.Empty;
            try
            {
                SqlCommand comando = new SqlCommand(query, oConnSap);
                comando.CommandType = CommandType.Text;
                if (parametros != null)
                    foreach (SqlParameter param in parametros)
                        comando.Parameters.Add(param);

                oConnSap.Open();
                valorRetorno = comando.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                oConnSap.Close();
            }
            return valorRetorno;
        }

        /// <summary>
        /// Ejecuta un comando de tipo: StoredProcedure, directamente en la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="nombreProcedure">Nombre del Procedimiento Almacenado</param>
        /// <param name="parametros">Lista Genérica de tipo SqlParameter</param>
        /// <returns>Contenedor de tipo DataTable con el retorno del procedimiento</returns>
        public DataTable ReadingProcedure(string nombreProcedure, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand(nombreProcedure, oConnSap);
            DataTable dtRetorno = new DataTable();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandTimeout = 360;
            if (parametros != null)
                foreach (SqlParameter param in parametros)
                    comando.Parameters.Add(param);
            try
            {
                oConnSap.Open();
                SqlDataReader reader = comando.ExecuteReader();
                dtRetorno.Load(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message.ToString());
                oConnSap.Close();
            }
            finally
            {
                oConnSap.Close();
            }
            return dtRetorno;
        }
    }
}
