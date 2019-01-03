using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.Reports;
using System.Data;

namespace PSIAA.BusinessLogicLayer.Reports
{
    public class FacturacionBLL
    {
        /// <summary>
        /// Variable de instancia a la clase FacturacionDAL.
        /// </summary>
        public FacturacionDAL _facturacionDal = new FacturacionDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Cabecera de Factura.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento</param>
        /// <param name="_serieDoc">Serie de Documento</param>
        /// <param name="_numDoc">Número de Documento</param>
        /// <returns>Contenedor de datos de tipo DataTable con los datos de cabecera.</returns>
        public DataTable ListarCabecera(string _ruc, string _tipoMov, int _serieDoc, int _numDoc) {
            return _facturacionDal.SelectCabeceraFactura(_ruc, _tipoMov, _serieDoc, _numDoc);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Detalle de Factura.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento</param>
        /// <param name="_serieDoc">Serie de Documento</param>
        /// <param name="_numDoc">Número de Documento</param>
        /// <returns>Contenedor de datos de tipo DataTable con el detalle de factura.</returns>
        public DataTable ListarDetalle(string _ruc, string _tipoMov, int _serieDoc, int _numDoc) {
            return _facturacionDal.SelectDetalleFactura(_ruc, _tipoMov, _serieDoc, _numDoc);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Totales por Contrato.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento</param>
        /// <param name="_serieDoc">Serie de Documento</param>
        /// <param name="_numDoc">Número de Documento</param>
        /// <returns>Contenedor de datos de tipo DataTable con los totales.</returns>
        public DataTable ListarTotalesPorContrato(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            return _facturacionDal.SelectTotalPorContratoFactura(_ruc, _tipoMov, _serieDoc, _numDoc);
        }
    }
}
