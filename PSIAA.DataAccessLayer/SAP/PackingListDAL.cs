using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.SAP
{
    public class PackingListDAL
    {
        private TransactionsSAP _trans = new TransactionsSAP();

        public DataTable SelectPackingListCabecera(int docKey) {
            List<SqlParameter> sqlParam = new List<SqlParameter>();
            sqlParam.Add(new SqlParameter("@DocKey", SqlDbType.Int) { Value = docKey });
            return _trans.ReadingProcedure("REPPACKINGLIST_CAB", sqlParam);
        }

        public DataTable SelectPackingListDetalle(int docKey) {
            List<SqlParameter> sqlParam = new List<SqlParameter>();
            sqlParam.Add(new SqlParameter("@DocKey", SqlDbType.Int) { Value = docKey });
            return _trans.ReadingProcedure("REPPACKINGLIST_DET", sqlParam);
        }

        public int SelectDocumentoEntry(string tipoDoc, string serie, string correlativo) {
            List<SqlParameter> sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                DocEntry 
                from ODLN 
                where
                U_BPP_MDTD = @tipo and
                U_BPP_MDSD = @serie and
                U_BPP_MDCD = @correlativo";
            sqlParam.Add(new SqlParameter("@tipo", SqlDbType.VarChar) { Value = tipoDoc });
            sqlParam.Add(new SqlParameter("@serie", SqlDbType.VarChar) { Value = serie });
            sqlParam.Add(new SqlParameter("@correlativo", SqlDbType.VarChar) { Value = correlativo });
            string rpta =  _trans.ReadingEscalarQuery(query, sqlParam);
            return rpta == "" ? 0 : Convert.ToInt32(rpta);
        }
    }
}
