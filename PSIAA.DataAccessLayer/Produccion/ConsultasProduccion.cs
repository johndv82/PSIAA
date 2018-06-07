using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.Produccion
{
    public class ConsultasProduccion
    {
        private Transactions _trans = new Transactions();

        public DataTable SelectCostosProduccion(int _contrato, string _fechaIni, string _fechaFin, string _modelo = "") {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            _sqlParam.Add(new SqlParameter("@fechaini", SqlDbType.VarChar) { Value = _fechaIni });
            _sqlParam.Add(new SqlParameter("@fechafin", SqlDbType.VarChar) { Value = _fechaFin });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = _modelo });

            return _trans.ReadingProcedure("PSIAA.CostosProduccion", _sqlParam);
        }

        public DataTable SelectInventarioProductos(int _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });

            return  _trans.ReadingProcedure("PSIAA.InventarioProductos", _sqlParam);
        }
    }
}
