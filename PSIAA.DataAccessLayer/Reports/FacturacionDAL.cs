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

        public DataTable SelectCabeceraFactura(string _ruc, string _tipoMov, int _serieDoc, int _numDoc) {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _ruc });
            _procedureParam.Add(new SqlParameter("@movimiento", SqlDbType.VarChar) { Value = _tipoMov });
            _procedureParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serieDoc });
            _procedureParam.Add(new SqlParameter("@numerodoc", SqlDbType.Int) { Value = _numDoc });

            return _trans.ReadingProcedure("PSIAA.CabeceraDocumentoTaller", _procedureParam);
        }

        public DataTable SelectDetalleFactura(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _ruc });
            _procedureParam.Add(new SqlParameter("@movimiento", SqlDbType.VarChar) { Value = _tipoMov });
            _procedureParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serieDoc });
            _procedureParam.Add(new SqlParameter("@numerodoc", SqlDbType.Int) { Value = _numDoc });

            return _trans.ReadingProcedure("PSIAA.DetalleDocumentoTaller", _procedureParam);
        }

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
