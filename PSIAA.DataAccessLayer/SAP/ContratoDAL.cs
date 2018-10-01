using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.SAP
{
    public class ContratoDAL
    {
        private readonly TransactionsSAP _trans = new TransactionsSAP();

        public DataTable SelectReporteContratoCab(string nroContrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@nrocontrato", SqlDbType.VarChar) { Value = nroContrato });
            return _trans.ReadingProcedure("REPCONTRATO_CAB", _sqlParam);
        }

        public DataTable SelectReporteContratoDet(string nroContrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@nrocontrato", SqlDbType.VarChar) { Value = nroContrato });
            return _trans.ReadingProcedure("REPCONTRATO_DET", _sqlParam);
        }
    }
}
