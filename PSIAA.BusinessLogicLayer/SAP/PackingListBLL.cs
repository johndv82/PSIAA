using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.SAP;
using System.Data;

namespace PSIAA.BusinessLogicLayer.SAP
{
    public class PackingListBLL
    {
        private readonly PackingListDAL _packingListDal = new PackingListDAL();

        public DataRow PackingListCabecera(int docEntry) {
            DataTable dtCab = _packingListDal.SelectPackingListCabecera(docEntry);
            if (dtCab.Rows.Count > 0) {
                return dtCab.Rows[0];
            } else
                return null;
        }

        public DataTable PackingListDetalle(int docEntry) {
            return _packingListDal.SelectPackingListDetalle(docEntry);
        }

        public int BuscarDocumentoEntry(string tipo, string serie, string correlativo) {
            return _packingListDal.SelectDocumentoEntry(tipo, serie, correlativo);
        }
    }
}
