using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PSIAA.DataAccessLayer.Reports
{
    public class FacturacionDAL
    {
        private Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta un Procedimiento Almacenado en la base de datos para obtener los datos de cabecera de la factura para el pago a talleres.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento (Factura / Recibo)</param>
        /// <param name="_serieDoc">Número de Serie</param>
        /// <param name="_numDoc">Número de Documento (Liquidación)</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado del Procedimiento Almacenado.</returns>
        public DataTable SelectCabeceraFactura(string _ruc, string _tipoMov, int _serieDoc, int _numDoc) {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _ruc });
            _procedureParam.Add(new SqlParameter("@movimiento", SqlDbType.VarChar) { Value = _tipoMov });
            _procedureParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serieDoc });
            _procedureParam.Add(new SqlParameter("@numerodoc", SqlDbType.Int) { Value = _numDoc });

            return _trans.ReadingProcedure("PSIAA.CabeceraDocumentoTaller", _procedureParam);
        }

        /// <summary>
        /// Ejecuta un Procedimiento Almancenado en la base de datos para obtener el detalle de factura para el pago a talleres.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento (Factura / Recibo)</param>
        /// <param name="_serieDoc">Número de Serie</param>
        /// <param name="_numDoc">Número de Documento (Liquidación)</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado del Procedimiento Almacenado.</returns>
        public DataTable SelectDetalleFactura(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _ruc });
            _procedureParam.Add(new SqlParameter("@movimiento", SqlDbType.VarChar) { Value = _tipoMov });
            _procedureParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serieDoc });
            _procedureParam.Add(new SqlParameter("@numerodoc", SqlDbType.Int) { Value = _numDoc });

            return _trans.ReadingProcedure("PSIAA.DetalleDocumentoTaller", _procedureParam);
        }

        /// <summary>
        /// Ejecuta un Procedimiento Almacenado en  la base de datos para obtener el Total Agrupado por toda la factura.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento (Factura / Recibo)</param>
        /// <param name="_serieDoc">Número de Serie</param>
        /// <param name="_numDoc">Numero de Documento (Liquidacion)</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado del Procedimiento Almacenado</returns>
        public DataTable SelectTotalPorContratoFactura(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _ruc });
            _procedureParam.Add(new SqlParameter("@movimiento", SqlDbType.VarChar) { Value = _tipoMov });
            _procedureParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serieDoc });
            _procedureParam.Add(new SqlParameter("@numerodoc", SqlDbType.Int) { Value = _numDoc });

            return _trans.ReadingProcedure("PSIAA.TotalPorContratoDocumentoTaller", _procedureParam);
        }
    }
}
