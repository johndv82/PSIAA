using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class CombMaterialDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD)
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener las caracteristicas del material de una prenda.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de consulta</returns>
        public DataTable SelectCombinacionMaterial(string modelo) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();

            string query = @"
                select 
                    cm.c_codmat,
                    m.c_denmat,
                    m.tipo
                from combmaterial cm
                inner join material m 
                    on m.c_codmat = cm.c_codmat
                where 
                    cm.c_codmod = @modelo";

            _sqlParam.Add(new NpgsqlParameter("@modelo", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
