using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class MaquinaDAL
    {
        private Transactions _trans = new Transactions();

        public DataTable SelectMaquinas() {
            string query = @"
                select 
	                c_codmaq as Codigo,
	                c_denmaq as Nombre,
	                i_idmaq as Id,
	                c_linea as Linea,
	                c_abr as Abreviacion,
	                c_galga as Galga,
	                n_capop as Capacidad,
	                n_itmlt as Limite
                from maquina_bac
                where i_est = 1";

            return _trans.ReadingQuery(query, null);
        }

        public string SelectDescripcionMaquina(string linea) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                c_denmaq
                from maquina_bac 
                where c_codmaq = @codmaquina";

            _sqlParam.Add(new SqlParameter("@codmaquina", SqlDbType.VarChar) { Value = linea });
            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }
    }
}
