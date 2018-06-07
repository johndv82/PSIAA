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
        private Transactions _trans = new Transactions();

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

        public string SelectUltimoNumeroDocumentoPagoTaller(string _codProveedor, int _serie, string _tipoMov) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                top 1 * 
                from (
	                select 
		                nro_documento 
	                from Doc_pago_taller_asig 
	                where cod_proveedor = @codproveedor
                        and serie_documento =  @serie
		                and tipo_movimiento = @tipomovimiento
	                group by nro_documento
                union
	                select 
		                nro_documento 
	                from Doc_pago_taller_libre 
	                where cod_proveedor = @codproveedor
                        and serie_documento = @serie
		                and tipo_movimiento = @tipomovimiento
	                group by nro_documento
                ) as base
                order by base.nro_documento desc";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProveedor });
            _sqlParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serie });
            _sqlParam.Add(new SqlParameter("@tipomovimiento", SqlDbType.VarChar) { Value = _tipoMov });
            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }

        public string SelectNumeroDocumento(string _codProveedor, int _serie, string _tipoMov, int _nroDoc) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                base.nro_documento
                from (
                select 
	                nro_documento
                from Doc_pago_taller_asig 
	                where cod_proveedor = @codproveedor 
	                and tipo_movimiento = @tipomovimiento 
	                and serie_documento = @serie
                group by nro_documento
                union
                select
	                nro_documento
                from Doc_pago_taller_libre
		                where cod_proveedor = @codproveedor 
	                and tipo_movimiento = @tipomovimiento 
	                and serie_documento = @serie
                group by nro_documento
                ) as base
                where base.nro_documento = @documento";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProveedor });
            _sqlParam.Add(new SqlParameter("@tipomovimiento", SqlDbType.VarChar) { Value = _tipoMov });
            _sqlParam.Add(new SqlParameter("@serie", SqlDbType.Int) { Value = _serie });
            _sqlParam.Add(new SqlParameter("@documento", SqlDbType.Int) { Value = _nroDoc });
            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }
    }
}
