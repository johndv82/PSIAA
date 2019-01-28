using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using System.Data;

namespace PSIAA.BusinessLogicLayer.Reports
{
    public class OrdenRequisicionBLL
    {
        /// <summary>
        /// Variable de instancia a la clase LanzamientoDAL.
        /// </summary>
        public LanzamientoDAL _lanzamientoDal = new LanzamientoDAL();
        /// <summary>
        /// Variable de instancia a la clase AsignacionOrdenesDAL.
        /// </summary>
        public AsignacionOrdenesDAL _asigOrdenesDal = new AsignacionOrdenesDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Detalle de Orden de Requisición y lo retorna.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="orden">Orden de Producción</param>
        /// <returns>Contenedor de tipo DataTable con los datos de retorno.</returns>
        public DataTable ListarDetalleOrdenRequision(int contrato, string orden)
        {
            return _lanzamientoDal.SelectDetalleOrdenRequisicion(contrato, orden);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Centro de Costos y lo retorna.
        /// </summary>
        /// <param name="orden">Orden de Producción</param>
        /// <returns>Contenedor de tipo DataTable con los datos de retorno.</returns>
        public DataTable ListarCentroCostos(string orden) {
            return _asigOrdenesDal.SelectCentroCostosPorOrden(orden);
        }
    }
}
