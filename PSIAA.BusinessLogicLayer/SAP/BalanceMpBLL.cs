using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.SAP;
using System.Data;

namespace PSIAA.BusinessLogicLayer.SAP
{
    public class BalanceMpBLL
    {
        private readonly BalanceMpDAL _balanceMpDal = new BalanceMpDAL();
        public DataTable ReporteBalanceMateriaPrima(string contrato)
        {
            return _balanceMpDal.SelectRepBalanceMateriaPrima(contrato);
        }
    }
}
