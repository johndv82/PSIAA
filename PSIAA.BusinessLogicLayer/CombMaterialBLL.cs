using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.TuartDB;

namespace PSIAA.BusinessLogicLayer
{
    public class CombMaterialBLL
    {
        private CombMaterialDAL _combMaterialDal = new CombMaterialDAL();
        public DataTable ListarCombinacionMaterialTacita(string modelo) {
            return _combMaterialDal.SelectCombinacionMaterial(modelo);
        }
    }
}
