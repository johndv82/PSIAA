using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.Reports;
using System.Data;

namespace PSIAA.BusinessLogicLayer.Reports
{
    public class PagoLibreBLL
    {
        private PagoLibreDAL _pagoLibreDal = new PagoLibreDAL();
        public DataTable ListarCabecera(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            return _pagoLibreDal.SelectCabeceraPagoLibre(_ruc, _tipoMov, _serieDoc, _numDoc);
        }

        public DataTable ListarDetalle(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            return _pagoLibreDal.SelectDetallePagoLibre(_ruc, _tipoMov, _serieDoc, _numDoc);
        }
    }
}
