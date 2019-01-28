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
        /// <summary>
        /// Variable de instancia a la clase CombMaterialDAL.
        /// </summary>
        public CombMaterialDAL _combMaterialDal = new CombMaterialDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de caracteristicas de material por modelo, y retorna el resultado.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con las caracteristicas.</returns>
        public DataTable ListarCombinacionMaterialTacita(string modelo) {
            return _combMaterialDal.SelectCombinacionMaterial(modelo);
        }
    }
}
