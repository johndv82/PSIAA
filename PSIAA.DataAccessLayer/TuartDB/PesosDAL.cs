using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Text;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class PesosDAL
    {
        private Transactions _trans = new Transactions();

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
