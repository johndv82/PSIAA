using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Npgsql;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class ModeloComponenteDAL
    {
        private Transactions _trans = new Transactions();

        public DataTable SelectComponentesModelo(string modelo) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();

            string query = @"
                select 
                    mc.c_codcom, 
                    mc.c_codmod, 
                    c.c_dencom 
                from modelocomponente mc 
                inner join componente c 
                    on c.c_codcom = mc.c_codcom
                where 
                    mc.c_codmod = @modelo 
                    and c.i_est = 1
                limit 10";

            _sqlParam.Add(new NpgsqlParameter("@modelo", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
