using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class CategoriaOperacionDAL
    {
        private Transactions _trans = new Transactions();

        public DataTable SelectCategoriaOperacion() {
            string query = @"
                select 
	                i_idcatope, 
	                c_dencatope 
                from categoriaoperacion
                where i_est = 1
                order by i_idcatope";
            return _trans.ReadingQuery(query);
        }
    }
}
