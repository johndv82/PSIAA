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
        /// <summary>
        /// Variable de instancia a la clase ContratoDAL.
        /// </summary>
        public readonly ContratoDAL _contratoDal = new ContratoDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de reporte de cabecera contrato, y se evalúa la existencia de datos en su contenedor,
        /// para para retornar la primera Fila de Datos, en caso contrario se retorna nulo.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <returns>Fila de datos de tipo DataRow con la cabecera.</returns>
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

        /// <summary>
        /// Ejecuta un procedimiento DAL de reporte de detalle contrato, y lo retorna.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con el detalle.</returns>
        public DataTable ContratoDetalle(string contrato)
        {
            return _contratoDal.SelectReporteContratoDet(contrato);
        }
    }
}
