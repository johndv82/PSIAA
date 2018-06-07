using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Npgsql;

namespace PSIAA.DataAccessLayer.ODOO
{
    public class ComponenteModeloDAL
    {
        private Transactions _trans = new Transactions();
        public DataTable SelectComponentesModelo(string modelo) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();
            string query = @"
                select 
	                dc.id as c_codcom,
                    m.modelo_id as c_codmod,
                    dc.descripcion_componente as c_dencom
                from modeloprenda_componente_rel m 
                inner join desarrollo_modelo_prenda d on m.modelo_id = d.id 
                inner join desarrollo_componente dc on dc.id = m.id
                where d.c_codmod = @modelo 
                order by dc.id
                limit 10";
            _sqlParam.Add(new NpgsqlParameter("@modelo", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
