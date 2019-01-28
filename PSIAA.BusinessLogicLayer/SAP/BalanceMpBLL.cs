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
        /// <summary>
        /// Variable de instancia a la clase BalanceMpDAL.
        /// </summary>
        public readonly BalanceMpDAL _balanceMpDal = new BalanceMpDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Balance de Materia Prima, y lo retorna.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los datos de retorno.</returns>
        public DataTable ReporteBalanceMateriaPrima(string contrato)
        {
            return _balanceMpDal.SelectRepBalanceMateriaPrima(contrato);
        }
    }
}
