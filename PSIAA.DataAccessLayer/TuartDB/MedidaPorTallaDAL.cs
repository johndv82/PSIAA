using System;
using System.Collections.Generic;
using Npgsql;
using System.Text;
using System.Data;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class MedidaPorTallaDAL
    {
        private Transactions _trans = new Transactions();
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
