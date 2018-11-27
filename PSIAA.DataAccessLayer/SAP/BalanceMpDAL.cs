using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.SAP
{
    public class BalanceMpDAL
    {
        private readonly TransactionsSAP _trans = new TransactionsSAP();

        public DataTable SelectRepBalanceMateriaPrima(string nroContrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.VarChar) { Value = nroContrato });
            return _trans.ReadingProcedure("REP_BALANCEMATERIAPRIMA", _sqlParam);
        }
    }
}
