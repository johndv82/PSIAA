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
        private FacturacionDAL _facturacionDal = new FacturacionDAL();
        public DataTable ListarCabecera(string _ruc, string _tipoMov, int _serieDoc, int _numDoc) {
            return _facturacionDal.SelectCabeceraFactura(_ruc, _tipoMov, _serieDoc, _numDoc);
        }

        public DataTable ListarDetalle(string _ruc, string _tipoMov, int _serieDoc, int _numDoc) {
            return _facturacionDal.SelectDetalleFactura(_ruc, _tipoMov, _serieDoc, _numDoc);
        }

        public DataTable ListarTotalesPorContrato(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            return _facturacionDal.SelectTotalPorContratoFactura(_ruc, _tipoMov, _serieDoc, _numDoc);
        }
    }
}
