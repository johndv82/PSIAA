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

        public DataTable SelectCabeceraPagoLibre(string _ruc, string _tipoMov, int _serieDoc, int _numDoc)
        {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _ruc });
            _procedureParam.Add(new SqlParameter("@movimiento", SqlDbType.VarChar) { Value = _tipoMov });
            _procedureParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serieDoc });
            _procedureParam.Add(new SqlParameter("@numeroliquid", SqlDbType.Int) { Value = _numDoc });

            return _trans.ReadingProcedure("PSIAA.CabeceraDocumentoLibre", _procedureParam);
        }

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
