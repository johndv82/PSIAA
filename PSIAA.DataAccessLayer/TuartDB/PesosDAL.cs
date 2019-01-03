using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Text;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class PesosDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD)
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el peso de tejido base de la prenda y su respectiva talla.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataRow SelectMuestraPeso(string modelo) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();

            string query = @"
                select 
                    c_tal,
                    n_pestej,
                    n_pesaca
                from Pesos
                where 
                    c_codmod = @modelo
                    and n_pestej != 0.0
                limit 1";

            _sqlParam.Add(new NpgsqlParameter("@modelo", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = modelo });
            DataTable dtResult = _trans.ReadingQuery(query, _sqlParam);
            if (dtResult.Rows.Count == 0) {
                return null;
            } else
                return dtResult.Rows[0];
        }
    }
}
