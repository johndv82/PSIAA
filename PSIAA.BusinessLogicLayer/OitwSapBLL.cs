using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using System.Data;

namespace PSIAA.BusinessLogicLayer
{
    public class OitwSapBLL
    {
        private OitwSapDAL _oitwSapDal = new OitwSapDAL();

        public DataTable ListarArticulosSap(string _nombre, bool _incluirSotckCero, string codigo) {
            int stock = _incluirSotckCero ? -1 : 0;
            return _oitwSapDal.SelectOitw(_nombre, stock, codigo);
        }
    }
}
