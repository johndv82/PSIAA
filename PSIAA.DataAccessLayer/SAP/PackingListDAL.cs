using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.SAP
{
    public class PackingListDAL
    {
        private Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta un Procedimiento Almacenado en la base de datos SAP para obtener los datos de cabecera para el reporte Packing List.
        /// </summary>
        /// <param name="docKey">Documento de Entrada</param>
        /// <returns>Contenedor de datos de tip DataTable con el resultado del procedimiento almacenado.</returns>
        public DataTable SelectPackingListCabecera(int docKey) {
            List<SqlParameter> sqlParam = new List<SqlParameter>();
            sqlParam.Add(new SqlParameter("@DocKey", SqlDbType.Int) { Value = docKey });
            return _trans.ReadingProcedure("REPPACKINGLIST_CAB", sqlParam);
        }

        /// <summary>
        /// Ejecuta un Procedimiento Almacenado en la base de datos SAP para obtener el detalle del reporte de Packing List.
        /// </summary>
        /// <param name="docKey">Documento de Entrada</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado del procedimiento almacenado.</returns>
        public DataTable SelectPackingListDetalle(int docKey) {
            List<SqlParameter> sqlParam = new List<SqlParameter>();
            sqlParam.Add(new SqlParameter("@DocKey", SqlDbType.Int) { Value = docKey });
            return _trans.ReadingProcedure("REPPACKINGLIST_DET", sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos SAP para obtener el documento entrada del reporte Packing List.
        /// </summary>
        /// <param name="tipoDoc">Tipo de Documento</param>
        /// <param name="serie">Número de Serie del Documento</param>
        /// <param name="correlativo">Correlativo del Documento</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado de la consulta.</returns>
        public int SelectDocumentoEntry(string tipoDoc, string serie, string correlativo) {
            List<SqlParameter> sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                DocEntry 
                from ODLN 
                where
                    U_BPP_MDTD = @tipo and
                    U_BPP_MDSD = @serie and
                    U_BPP_MDCD = @correlativo";
            sqlParam.Add(new SqlParameter("@tipo", SqlDbType.VarChar) { Value = tipoDoc });
            sqlParam.Add(new SqlParameter("@serie", SqlDbType.VarChar) { Value = serie });
            sqlParam.Add(new SqlParameter("@correlativo", SqlDbType.VarChar) { Value = correlativo });
            string rpta =  _trans.ReadingEscalarQuery(query, sqlParam);
            return rpta == "" ? 0 : Convert.ToInt32(rpta);
        }
    }
}
