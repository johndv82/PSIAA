using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.SAP
{
    public class ContratoDAL
    {
        private readonly Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta un Procedimiento Almacenado en la base de datos SAP para obtener los datos de cabecera del Reporte de Contrato SAP.
        /// </summary>
        /// <param name="nroContrato">Número de Contrato</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado del procedimiento.</returns>
        public DataTable SelectReporteContratoCab(string nroContrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@nrocontrato", SqlDbType.VarChar) { Value = nroContrato });
            return _trans.ReadingProcedure("REPCONTRATO_CAB", _sqlParam);
        }

        /// <summary>
        /// Ejecuta un Procidimiento Almacenado en la base de datos SAP para obtener el detalle del Reporte de Contrato SAP.
        /// </summary>
        /// <param name="nroContrato">Número de Contrato</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado del procedimiento.</returns>
        public DataTable SelectReporteContratoDet(string nroContrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@nrocontrato", SqlDbType.VarChar) { Value = nroContrato });
            return _trans.ReadingProcedure("REPCONTRATO_DET", _sqlParam);
        }
    }
}
