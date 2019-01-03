using System;
using System.Collections.Generic;
using Npgsql;
using System.Text;
using System.Data;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class MedidaPorTallaDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD)
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección para obtener todas las medidas de una prenda por talla.
        /// </summary>
        /// <param name="_modelo">Modelo de prenda</param>
        /// <param name="_talla">Talla de prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectMedidasPorTalla(string _modelo, string _talla) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();

            string query = @"
                select
	                c_codmed as CodMedida, 
                    c_med as Medida
                from medidaxtalla
                where c_codmod = @modelo 
                and c_codtal = @talla ";

            _sqlParam.Add(new NpgsqlParameter("@modelo", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = _modelo });
            _sqlParam.Add(new NpgsqlParameter("@talla", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = _talla });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
