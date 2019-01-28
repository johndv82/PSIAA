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
        /// <summary>
        /// Variable de instancia a la clase PagoLibreDAL.
        /// </summary>
        public PagoLibreDAL _pagoLibreDal = new PagoLibreDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Cabecera de Pago Libre y lo retorna.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento</param>
        /// <param name="_serieDoc">Serie de Documento</param>
        /// <param name="_numDoc">Número de Documento</param>
        /// <returns>Contenedor de tipo DataTable con los datos de retorno.</returns>
        public DataTable ListarCabecera(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            return _pagoLibreDal.SelectCabeceraPagoLibre(_ruc, _tipoMov, _serieDoc, _numDoc);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Detalle de Pago Libre, y lo retorna.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento</param>
        /// <param name="_serieDoc">Serie de Documento</param>
        /// <param name="_numDoc">Número de Documento</param>
        /// <returns>Contenedor de tipo DataTable con los datos de retorno.</returns>
        public DataTable ListarDetalle(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            return _pagoLibreDal.SelectDetallePagoLibre(_ruc, _tipoMov, _serieDoc, _numDoc);
        }
    }
}
