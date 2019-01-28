using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.SAP;
using System.Data;

namespace PSIAA.BusinessLogicLayer.SAP
{
    public class OitwSapBLL
    {
        /// <summary>
        /// Variable de instancia a la clase OitwSapDAL.
        /// </summary>
        public OitwSapDAL _oitwSapDal = new OitwSapDAL();

        /// <summary>
        /// Evalúa si se incluirá stock cero, para enviar al metodo DAL de artículos Sap, en el parametro stock,  
        /// un valor entero de -1, y en caso contrario un 0.
        /// </summary>
        /// <param name="_nombre">Nombre del Artículo</param>
        /// <param name="_incluirStockCero">Valor booleano para incluir stock cero.</param>
        /// <param name="codigo">Código de Artículo</param>
        /// <returns>Contenedor de tipo DataTable con el retorno de DAL.</returns>
        public DataTable ListarArticulosSap(string _nombre, bool _incluirStockCero, string codigo) {
            int stock = _incluirStockCero ? -1 : 0;
            return _oitwSapDal.SelectOitw(_nombre, stock, codigo);
        }
    }
}
