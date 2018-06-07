using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class OitwSapDAL
    {
        private TransactionsSAP _transSap = new TransactionsSAP();

        public DataTable SelectOitw(string itemName, int stock, string itemCode) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string filtroCodigoItem;

            if (itemCode != "")
            {
                filtroCodigoItem = "and T0.ItemCode = @codigo";
                _sqlParam.Add(new SqlParameter("@codigo", SqlDbType.VarChar) { Value = itemCode });
            }
            else {
                filtroCodigoItem = string.Empty;
            }

            string query = @"
                select
                    T0.ItemCode as 'CodigoSap', 
	                T1.ItemName as 'DescripcionArticulo', 
	                T0.AvgPrice as 'CostoPromedio', 
	                t0.OnHand as 'Stock', 
                    T1.InvntryUom as 'UnidMed'
                from OITW T0
                inner
                join oitm T1
                on T0.itemCode = T1.ItemCode
                where
                    T1.ItmsGrpCod in ('100','101','102', '106','107', '109', '118', '119', '120','121' )
	                and T0.OnHand > @stock 
                    and T1.ItemName like '%' + @item + '%' " + filtroCodigoItem;

            _sqlParam.Add(new SqlParameter("@item", SqlDbType.VarChar) { Value = itemName });
            _sqlParam.Add(new SqlParameter("@stock", SqlDbType.Int) { Value = stock });

            return _transSap.ReadingQuery(query, _sqlParam);
        }
    }
}
