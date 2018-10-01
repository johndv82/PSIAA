using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.SAP;
using System.Data;

namespace PSIAA.BusinessLogicLayer.SAP
{
    public class ContratoBLL
    {
        private readonly ContratoDAL _contratoDal = new ContratoDAL();
        public DataRow ContratoCabecera(string contrato)
        {
            DataTable dtCab = _contratoDal.SelectReporteContratoCab(contrato);
            if (dtCab.Rows.Count > 0)
            {
                return dtCab.Rows[0];
            }
            else
                return null;
        }

        public DataTable ContratoDetalle(string contrato)
        {
            return _contratoDal.SelectReporteContratoDet(contrato);
        }
    }
}
