using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.ODOO
{
    public class CombinacionColorDAL
    {
        private Transactions _trans = new Transactions();
        public DataTable SelectColorMateriales(string modelo, string combinacion)
        {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();

            string query = @"
                select 
	                dmp.c_codmod,
                    cc.value_color, 
                    ac.percent, 
                    dc.calidad,
                    dt.titulo,
                    coalesce(mmr.tipo_material, '') as tipo_material,
                    coalesce(mmr.denominacion_material, '') as denominacion_material
                from desarrollo_modelo_prenda dmp 
                inner join combinaciones_color cc on cc.modelo_id = dmp.id 
                inner join modeloprenda_material_rel mmr on mmr.modelo_id = dmp.id 
                inner join desarrollo_titulo dt on dt.id = mmr.titulo 
                inner join desarrollo_calidad dc on dc.id = mmr.calidad
                inner join articulo_color ac on ac.modelo_id = dmp.id and ac.name_color = cc.color_x 
                and ac.calidad = dc.id and ac.titulo = dt.id
                where 
	                dmp.c_codmod = @modelo
	                and cc.comb_y = @combinacion ";

            _sqlParam.Add(new NpgsqlParameter("@modelo", NpgsqlDbType.Varchar) { Value = modelo });
            _sqlParam.Add(new NpgsqlParameter("@combinacion", NpgsqlDbType.Varchar) { Value = combinacion });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
