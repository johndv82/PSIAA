using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.Reports
{
    public class PagoLibreDAL
    {
        private Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta un Procedimiento Almacenado en la base de datos para obtener datos de cabecera del documento de Pago Libre.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento (Factura / Recibo)</param>
        /// <param name="_serieDoc">Número de Serie</param>
        /// <param name="_numDoc">Número de Documento (Liquidacion)</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado del Procedimiento Almacenado</returns>
        public DataTable SelectCabeceraPagoLibre(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _ruc });
            _procedureParam.Add(new SqlParameter("@movimiento", SqlDbType.VarChar) { Value = _tipoMov });
            _procedureParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serieDoc });
            _procedureParam.Add(new SqlParameter("@numeroliquid", SqlDbType.Int) { Value = _numDoc });

            return _trans.ReadingProcedure("PSIAA.CabeceraDocumentoLibre", _procedureParam);
        }

        /// <summary>
        /// Ejecuta un Procedimiento Almacenado en la base de datos para obtener el detalle del documento de Pago Libre.
        /// </summary>
        /// <param name="_ruc">Número de Ruc</param>
        /// <param name="_tipoMov">Tipo de Movimiento</param>
        /// <param name="_serieDoc">Serie de Documento</param>
        /// <param name="_numDoc">Número de Documento (Liquidacion)</param>
        /// <returns></returns>
        public DataTable SelectDetallePagoLibre(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _ruc });
            _procedureParam.Add(new SqlParameter("@movimiento", SqlDbType.VarChar) { Value = _tipoMov });
            _procedureParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serieDoc });
            _procedureParam.Add(new SqlParameter("@numeroliquid", SqlDbType.Int) { Value = _numDoc });

            return _trans.ReadingProcedure("PSIAA.DetalleDocumentoLibre", _procedureParam);
        }
    }
}
