using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataTransferObject;
using System.Data.SqlClient;
using System.Data;

namespace PSIAA.DataAccessLayer
{
    public class DocumentoPagoTallerDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de inserción a la tabla Doc_pago_taller_asig.
        /// </summary>
        /// <param name="_docPagoTaller">Objeto de tipo DocumentoPagoTallerDTO</param>
        /// <returns>Variable de tipo int con la cantidad de registros ingresados.</returns>
        public int InsertDocumentoPagoTaller(DocumentoPagoTallerDTO _docPagoTaller) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into Doc_pago_taller_asig(
	                cod_proveedor, 
	                tipo_movimiento, 
	                serie_documento, 
	                nro_documento, 
	                Cod_Cat_Oper, 
	                Nro_Ord_Asig, 
	                Orden, 
                    Lote, 
	                Categoria, 
	                Cod_Proceso
                ) values(
	                @codproveedor, @tipomov, @seriedoc, @nrodoc, @codcatoper, 
                    @nroorden, @orden, @lote, @categoria, @codproceso )";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _docPagoTaller.CodProveedor });
            _sqlParam.Add(new SqlParameter("@tipomov", SqlDbType.VarChar) { Value = _docPagoTaller.TipoDocumento });
            _sqlParam.Add(new SqlParameter("@seriedoc", SqlDbType.Int) { Value = _docPagoTaller.SerieDocumento });
            _sqlParam.Add(new SqlParameter("@nrodoc", SqlDbType.Int) { Value = _docPagoTaller.NroDocumento });
            _sqlParam.Add(new SqlParameter("@codcatoper", SqlDbType.Int) { Value = _docPagoTaller.CategoriaOperacion });
            _sqlParam.Add(new SqlParameter("@nroorden", SqlDbType.Float) { Value = _docPagoTaller.NumOrdenAsignacion });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _docPagoTaller.Orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _docPagoTaller.Lote });
            _sqlParam.Add(new SqlParameter("@categoria", SqlDbType.Int) { Value = _docPagoTaller.Categoria });
            _sqlParam.Add(new SqlParameter("@codproceso", SqlDbType.Int) { Value = _docPagoTaller.CodProceso });
            return _trans.ExecuteQuery(query, _sqlParam);
        }
    }
}
