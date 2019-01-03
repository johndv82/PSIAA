using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class OperacionModeloDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión a la BD)
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener las operaciones completas que tiene una prenda.
        /// </summary>
        /// <param name="_modelo">Modelo de Prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectOperacionesTiempo(string _modelo) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();

            string query = @"
                select 
	                om.i_idope, 
	                ot.i_idcatope,
	                ot.c_denope || ' - ' || ot.c_comope as descripcion,
	                om.f_tiempope,
                    om.i_numord
                from operacionmodelo om
                inner join operaciontmp ot on ot.i_idope = om.i_idope
                where om.c_codmod = @modelo";

            _sqlParam.Add(new NpgsqlParameter("@modelo", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = _modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener la descripción de un proceso.
        /// </summary>
        /// <param name="idProceso">ID de Proceso</param>
        /// <returns>Variable de tipo string con el valor de la descripción.</returns>
        public string SelectDescripcionOperacion(int idProceso) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();
            string query = @"
                select 
                    c_denope || ' - ' || c_comope as descripcion
                from operaciontmp 
                where i_idope = @idoperacion";

            _sqlParam.Add(new NpgsqlParameter("@idoperacion", NpgsqlTypes.NpgsqlDbType.Integer) { Value = idProceso });
            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener primeros 60 códigos de operación por modelo.
        /// </summary>
        /// <param name="modelo">Modelo de Prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta</returns>
        public DataTable SelectCodigoOperaciones(string modelo) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();

            string query = @"
                select 
			        ot.c_codope
                from operacionmodelo om
                inner join operaciontmp ot 
                    on ot.i_idope = om.i_idope
                where om.c_codmod = @modelo
                order by om.i_numord 
                limit 60";

            _sqlParam.Add(new NpgsqlParameter("@modelo", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
