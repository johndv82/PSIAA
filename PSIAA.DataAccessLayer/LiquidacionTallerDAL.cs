using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataTransferObject;
using System.Data.SqlClient;
using System.Data;

namespace PSIAA.DataAccessLayer
{
    public class LiquidacionTallerDAL
    {
        private Transactions _trans = new Transactions();

        public int InsertLiquidacionTaller(LiquidacionTallerDTO _liquidTallerDto) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into Liquidacion_Talleres(
	                cod_empresa, 
                    nro_de_control, 
                    periodo, 
                    cod_proveedor, 
                    tipo_movimiento,
	                serie_documento, 
                    nro_documento, 
                    fecha_documento, 
                    concepto_compra,
	                c_igv, c_sol, 
                    asiento_generado, 
                    moneda, 
                    fecha_de_cancelacion, 
                    tipo_de_cambio,
	                tipo_de_cambio_aplic, 
                    monto_factura_afecto_soles, 
                    monto_factura_afecto_dolares,
	                tipo_igv, 
                    monto_igv_soles, 
                    monto_igv_dolares, 
                    otros_montos_soles, 
                    otros_montos_dolares,
	                estado, usuario, glosa, 
                    fecha_de_contabilizac, 
                    monto_total_doc_soles, 
                    montototal_doc_dolares,
	                cod_cuenta_total_venta, 
                    voucher, ref_caja, 
                    fecha_creacion, 
                    anno, Semana
                ) values(
                    1, @nrocontrol, @periodo, @codproveedor, @tipomovimiento, @seriedoc, 
                    @numerodoc, convert(date, @fechadoc), @conceptocompra, @cigv, @csol, '', 
                    @moneda, convert(date, @fechacancelacion), 1, '', 
                    @montofacturasoles, @montofacturadolares, @tipoigv, @montoigvsoles, @montoigvdolares, 0, 0, 'P', 
                    @usuario, @glosa, convert(date, @fechacontabilizacion),
                    @montototalsoles, @montototaldolares, '420101', '', '', convert(date, @fechacreacion), @anio, @semana
                )";

            _sqlParam.Add(new SqlParameter("@nrocontrol", SqlDbType.Int) { Value = _liquidTallerDto.NroControl });
            _sqlParam.Add(new SqlParameter("@periodo", SqlDbType.VarChar) { Value = _liquidTallerDto.Periodo });
            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _liquidTallerDto.CodProveedor });
            _sqlParam.Add(new SqlParameter("@tipomovimiento", SqlDbType.VarChar) { Value = _liquidTallerDto.TipoMovimiento });
            _sqlParam.Add(new SqlParameter("@seriedoc", SqlDbType.Int) { Value = _liquidTallerDto.SerieDocumento });
            _sqlParam.Add(new SqlParameter("@numerodoc", SqlDbType.Int) { Value = _liquidTallerDto.NroDocumento });
            _sqlParam.Add(new SqlParameter("@fechadoc", SqlDbType.DateTime) { Value = _liquidTallerDto.FechaDocumento });
            _sqlParam.Add(new SqlParameter("@conceptocompra", SqlDbType.VarChar) { Value = _liquidTallerDto.ConceptoCompra });
            _sqlParam.Add(new SqlParameter("@cigv", SqlDbType.Int) { Value = _liquidTallerDto.CIgv });
            _sqlParam.Add(new SqlParameter("@csol", SqlDbType.Int) { Value = _liquidTallerDto.CSol });
            _sqlParam.Add(new SqlParameter("@moneda", SqlDbType.VarChar) { Value = _liquidTallerDto.Moneda });
            _sqlParam.Add(new SqlParameter("@fechacancelacion", SqlDbType.VarChar) { Value = _liquidTallerDto.FechaCancelacion });
            _sqlParam.Add(new SqlParameter("@montofacturasoles", SqlDbType.Decimal) { Value = _liquidTallerDto.MontoFacturaSoles });
            _sqlParam.Add(new SqlParameter("@montofacturadolares", SqlDbType.Decimal) { Value = _liquidTallerDto.MontoFacturaDolares });
            _sqlParam.Add(new SqlParameter("@tipoigv", SqlDbType.VarChar) { Value = _liquidTallerDto.TipoIgv });
            _sqlParam.Add(new SqlParameter("@montoigvsoles", SqlDbType.Decimal) { Value = _liquidTallerDto.MontoIgvSoles });
            _sqlParam.Add(new SqlParameter("@montoigvdolares", SqlDbType.Decimal) { Value = _liquidTallerDto.MontoIgvDolares });
            _sqlParam.Add(new SqlParameter("@usuario", SqlDbType.VarChar) { Value = _liquidTallerDto.Usuario });
            _sqlParam.Add(new SqlParameter("@glosa", SqlDbType.VarChar) { Value = _liquidTallerDto.Glosa });
            _sqlParam.Add(new SqlParameter("@fechacontabilizacion", SqlDbType.DateTime) { Value = _liquidTallerDto.FechaContabilizacion });
            _sqlParam.Add(new SqlParameter("@montototalsoles", SqlDbType.Decimal) { Value = _liquidTallerDto.MontoTotalSoles });
            _sqlParam.Add(new SqlParameter("@montototaldolares", SqlDbType.Decimal) { Value = _liquidTallerDto.MontoTotalDolares });
            _sqlParam.Add(new SqlParameter("@fechacreacion", SqlDbType.DateTime) { Value = _liquidTallerDto.FechaCreacion });
            _sqlParam.Add(new SqlParameter("@anio", SqlDbType.Int) { Value = _liquidTallerDto.Anio });
            _sqlParam.Add(new SqlParameter("@semana", SqlDbType.Int) { Value = _liquidTallerDto.Semana });

            return _trans.ExecuteQuery(query, _sqlParam);
        }

        public string SelectUltimoNroControlLiquidacionTaller() {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
                    top 1 nro_de_control
                from Liquidacion_Talleres 
                where periodo = @periodo
                order by nro_de_control desc";

            _sqlParam.Add(new SqlParameter("@periodo", SqlDbType.VarChar) { Value = DateTime.Now.ToString("yyyyMM") });
            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }

        public DataTable SelectLiquidacionTalleres(string _codProveedor, int _anio) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                datename(MONTH, SUBSTRING(lt.periodo, 1, 4) + '/' + SUBSTRING(lt.periodo, 5, 2) + '/01') as PeriodoMes,
	                case lt.tipo_movimiento 
		                when '01' then 'Factura'
		                when '02' then 'Recibo por Honorario' else 'Otro' 
	                end as TipoMov,
	                lt.serie_documento,
	                lt.nro_documento,
	                case lt.concepto_compra 
		                when 500 then 'Confección'
		                when 400 then 'Tejido' else 'Otro' 
	                end as concepto,
	                lt.fecha_documento,
	                case lt.moneda
		                when 'S' then 'Soles'
		                when 'D' then 'Dalares' else 'Otro'
	                end as moneda,
	                case lt.moneda
		                when 'S' then lt.monto_factura_afecto_soles
		                when 'D' then lt.monto_factura_afecto_dolares
		                else 0 end as sub_total,
	                case lt.moneda
		                when 'S' then lt.monto_igv_soles
		                when 'D' then lt.monto_igv_dolares
		                else 0 end as igv,
	                lt.usuario,
	                case lt.moneda
		                when 'S' then lt.monto_total_doc_soles
		                when 'D' then lt.montototal_doc_dolares
		                else 0 end as total,
	                lt.Semana
                from Liquidacion_Talleres lt
                right join (
	                select 
		                dpta.cod_proveedor,
		                dpta.tipo_movimiento,
		                dpta.serie_documento,
		                dpta.nro_documento
	                from Doc_pago_taller_asig dpta
	                where dpta.cod_proveedor = @codproveedor 
	                group by 
		                dpta.cod_proveedor,
		                dpta.tipo_movimiento,
		                dpta.serie_documento,
		                dpta.nro_documento
                ) doc on doc.tipo_movimiento = lt.tipo_movimiento
	                and doc.serie_documento = lt.serie_documento
	                and doc.nro_documento = lt.nro_documento
	                and doc.cod_proveedor = lt.cod_proveedor
                where lt.cod_proveedor = @codproveedor 
                and lt.anno = @anio
                order by lt.fecha_documento";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProveedor });
            _sqlParam.Add(new SqlParameter("@anio", SqlDbType.Int) { Value = _anio });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        public DataTable SelectLiquidacionLibreTalleres(string _codProveedor, string _anio, string _mes) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                case doc.tipo_movimiento 
		                when '01' then 'Factura'
		                when '02' then 'Recibo por Honorario' 
                        else 'Otro' end as TipoMov,
                    doc.serie_documento,
	                doc.nro_documento,
	                lt.fecha_documento,
	                case lt.moneda
		                when 'S' then 'Soles'
		                when 'D' then 'Dalares' else 'Otro'
	                end as moneda,
	                lt.fecha_documento,
	                case lt.moneda
		                when 'S' then 'Soles'
		                when 'D' then 'Dalares' else 'Otro'
	                end as moneda,
	                case lt.moneda
		                when 'S' then lt.monto_factura_afecto_soles
		                when 'D' then lt.monto_factura_afecto_dolares
		                else 0 end as sub_total,
	                case lt.moneda
		                when 'S' then lt.monto_igv_soles
		                when 'D' then lt.monto_igv_dolares
		                else 0 end as igv,
	                lt.usuario,
	                case lt.moneda
		                when 'S' then lt.monto_total_doc_soles
		                when 'D' then lt.montototal_doc_dolares
		                else 0 end as total,
	                lt.Semana
                from Liquidacion_Talleres lt
                inner join (
	                select 
		                dptl.cod_proveedor,
		                dptl.tipo_movimiento,
		                dptl.serie_documento,
		                dptl.nro_documento
	                from Doc_pago_taller_libre dptl
	                where dptl.cod_proveedor = @codproveedor 
	                group by 
		                dptl.cod_proveedor,
		                dptl.tipo_movimiento,
		                dptl.serie_documento,
		                dptl.nro_documento
                ) doc on doc.tipo_movimiento = lt.tipo_movimiento
	                and doc.serie_documento = lt.serie_documento
	                and doc.nro_documento = lt.nro_documento
	                and doc.cod_proveedor = lt.cod_proveedor
                where doc.cod_proveedor = @codproveedor  
	                and year(lt.fecha_creacion) = @anio
	                and month(lt.fecha_creacion) = @mes
                order by lt.fecha_documento ";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProveedor });
            _sqlParam.Add(new SqlParameter("@anio", SqlDbType.VarChar) { Value = _anio });
            _sqlParam.Add(new SqlParameter("@mes", SqlDbType.VarChar) { Value = _mes });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        //Reporte para Gerencia
        public DataTable SelectLiquidacionTallerPorFecha(string _fechaIni, string _fechaFin) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                p.nombre_comercial as proveedor,
	                lt.cod_proveedor,
                    case SAP.u_bpp_mdtd
		                when '01' then 'Factura'
		                when '02' then 'Recibo Honorario'
		                else 'Otro' end as tipo_movimiento,
                    SAP.u_bpp_mdsd as serie_documento,
                    SAP.u_bpp_mdcd as nro_documento,
                    lt.nro_documento as nro_liquidacion,
                    lt.fecha_documento,
                    lt.moneda,
	                case lt.moneda
		                when 'S' then round(SAP.[DocTotal]- SAP.[VatSum] + SAP.[WTSum], 2)
		                when 'D' then  round(SAP.[DocTotalFC]- SAP.[VatSumFC] + SAP.[WTSumFC], 2)
		                else 0 end as sub_total,
	                case lt.moneda
		                when 'S' then round(SAP.[VatSum], 2)
		                when 'D' then round(SAP.[VatSumFC], 2)
		                else 0 end as igv,
                    lt.usuario,
	                case lt.moneda
		                when 'S' then round(SAP.[WTSum]+ SAP.[DocTotal], 2)
		                when 'D' then round(SAP.[WTSumFC]+ SAP.[DocTotalFC], 2)
		                else 0 end as total
                from Liquidacion_Talleres lt
                inner join [192.168.0.213].[SBO_ATLAS_PRODUCCION].dbo.[opch] SAP 
                on sap.u_numliq  = (CAST(lt.periodo AS nvarchar(20))  + CAST(lt.nro_de_control AS nvarchar(20)) )
                right join (
                    select 
                        dpta.cod_proveedor,
                        dpta.tipo_movimiento,
                        dpta.serie_documento,
                        dpta.nro_documento
                    from Doc_pago_taller_asig dpta
                    group by 
                        dpta.cod_proveedor,
                        dpta.tipo_movimiento,
                        dpta.serie_documento,
                        dpta.nro_documento
                ) doc on doc.tipo_movimiento = lt.tipo_movimiento
                    and doc.serie_documento = lt.serie_documento
                    and doc.nro_documento = lt.nro_documento
                    and doc.cod_proveedor = lt.cod_proveedor
                inner join proveedores p on p.cod_proveedor = lt.cod_proveedor
                where convert(date, lt.fecha_documento) between @fechaini and @fechafin
                order by lt.fecha_documento";

            _sqlParam.Add(new SqlParameter("@fechaini", SqlDbType.VarChar) { Value = _fechaIni });
            _sqlParam.Add(new SqlParameter("@fechafin", SqlDbType.VarChar) { Value = _fechaFin });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        //Reporte para Ingenieria
        public DataTable SelectLiquidacionesPorSemana(int anio, int semana) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@anio", SqlDbType.Int) { Value = anio });
            _sqlParam.Add(new SqlParameter("@semana", SqlDbType.Int) { Value = semana });
            return _trans.ReadingProcedure("PSIAA.ReportePreLiquidaciones", _sqlParam);
        }
    }
}
