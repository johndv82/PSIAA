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
        private LanzamientoDAL _lanzamientoDal = new LanzamientoDAL();
        private AsignacionOrdenesDAL _asigOrdenesDal = new AsignacionOrdenesDAL();

        public DataTable ListarDetalleOrdenRequision(int contrato, string orden)
        {
            return _lanzamientoDal.SelectDetalleOrdenRequisicion(contrato, orden);
        }

        public DataTable ListarCentroCostos(string orden) {
            //return _asigOrdenesDal.SelectCentroCostosPorOrden(orden, catOperacion);
            return _asigOrdenesDal.SelectCentroCostosPorOrden2(orden);
        }
    }
}
