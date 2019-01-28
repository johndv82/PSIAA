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

        /// <summary>
        /// Crea nueva instancia de conexión a la cadena: ConnectionSIAA, del archivo de configuración (WebConfig).
        /// </summary>
        public Transactions()
        {
            oConnSia = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionSIAA"].ConnectionString);
        }

        /// <summary>
        /// Ejecuta una consulta de lectura en formato de cadena, directamente a la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="query">Consulta de formato de cadena</param>
        /// <param name="parametros">Parametros en forma de Lista Genérica de tipo NpgsqlParameter, no obligatorio</param>
        /// <returns>Contenedor de datos de tipo DataTable, con el retorno de la consulta</returns>
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

        /// <summary>
        /// Ejecuta una consulta de lectura escalar en formato de cadena, directamente a la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="query">Cadena de Consulta</param>
        /// <param name="parametros">Parametros Lista Genérica de tipo SqlParameter</param>
        /// <returns>Variable de tipo string con el valor de retorno.</returns>
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

        /// <summary>
        /// Ejecuta una consulta inserción, directamente en la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="query">Cadena de Consulta</param>
        /// <param name="parametros">Parametros en forma de Lista Genérica de tipo SqlParameter</param>
        /// <returns>Variable de tipo int con el valor de retorno.</returns>
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

        /// <summary>
        /// Ejecuta un comando de tipo: StoredProcedure, directamente en la base de datos, adjuntando sus parametros.
        /// </summary>
        /// <param name="nombreProcedure">Nombre del Procedimiento Almacenado</param>
        /// <param name="parametros">Parametros en forma Lista Genérica de tipo SqlParameter</param>
        /// <returns>Contenedor de tipo DataTable con el retorno del procedimiento</returns>
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
