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
        private Transactions _trans = new Transactions();

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

        public string SelectDescripcionOperacion(int _codOperacion) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();
            string query = @"
                select 
                    c_denope || ' - ' || c_comope as descripcion
                from operaciontmp 
                where i_idope = @codoperacion";

            _sqlParam.Add(new NpgsqlParameter("@codoperacion", NpgsqlTypes.NpgsqlDbType.Integer) { Value = _codOperacion });
            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }

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
