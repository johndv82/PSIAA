using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.SAP
{
    public class BalanceMpDAL
    {
        private readonly Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta un Procedimiento Almacenado en la base de datos SAP para obtener el Balance de Materia Prima.
        /// </summary>
        /// <param name="nroContrato">Número de Contrato</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado del procedimiento.</returns>
        public DataTable SelectRepBalanceMateriaPrima(string nroContrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.VarChar) { Value = nroContrato });
            return _trans.ReadingProcedure("REP_BALANCEMATERIAPRIMA", _sqlParam);
        }
    }
}
