using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using PSIAA.DataTransferObject;

namespace PSIAA.DataAccessLayer
{
    public class AlmacenDAL
    {
        private Transactions _trans = new Transactions();
        public DataTable SelectAlmacenes90_98() {
            string query = @"
                select 
	                Cod_Almacen as 'Codigo',
	                Descripcion as 'Almacen'
                from Almacenes
                where Cod_Almacen in (90, 93, 95, 98, 112)
                order by Cod_Almacen asc";
            return _trans.ReadingQuery(query, null);
        }

        public string SelectUltimoDocumentoDeHoy() {
            string query = @"
                select top 1
	                numero_documento as 'Documento',
	                fecha_operacion as 'FechaOperacion'
                from detalle_almacen where fecha_operacion = convert(date, getdate())
                group by 
                    numero_documento, 
                    fecha_operacion
                order by numero_documento desc";

            return _trans.ReadingEscalarQuery(query, null);
        }

        public int InsertDetalleAlmacen(AlmacenDTO _almacenDto) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into detalle_almacen(
	                almacen,
	                tipo_movimiento,
	                numero_documento,
	                item_doc,
	                fecha_operacion,
	                ingreso_salida,
	                almacen_origdest,
	                cod_producto,
	                numero,
	                nro_lote,
	                contrato,
	                xxs,xs,s,m,l,xl,xxl,
	                cantidad,
	                peso_bruto,
	                peso_neto,
	                saldo_operacion, costo_soles, costo_dolar, precio_vta_soles,
	                precio_costo, descuento_soles, precio_venta_dolares, porc_descuento,
	                descuento_dolares, tc, Observaciones, costo_adicional_soles, costo_adicional_dolares
                ) values(
	                @almacen,
	                @tipomovimiento,
	                @documento,
	                @item,
	                convert(date, getdate()),
	                @ingresosalida,
	                @almacenorigendestino,
	                @codproducto,
	                @orden,
	                @nrolote,
	                @contrato,
	                @talla1, @talla2, @talla3, @talla4, @talla5, @talla6, @talla7,
	                @cantidad,
	                @pesobruto,
	                @pesoneto,
	                0,0,0,0,0,0,0,0,0,0,'',0,0
                )";
            _sqlParam.Add(new SqlParameter("@almacen", SqlDbType.Int) { Value = _almacenDto.CodAlmacen });
            _sqlParam.Add(new SqlParameter("@tipomovimiento", SqlDbType.VarChar) { Value = _almacenDto.TipoMovimiento });
            _sqlParam.Add(new SqlParameter("@documento", SqlDbType.VarChar) { Value = _almacenDto.NumeroDocumento });
            _sqlParam.Add(new SqlParameter("@item", SqlDbType.Int) { Value = _almacenDto.Item });
            _sqlParam.Add(new SqlParameter("@ingresosalida", SqlDbType.Int) { Value = _almacenDto.IngresoSalida });
            _sqlParam.Add(new SqlParameter("@almacenorigendestino", SqlDbType.Int) { Value = _almacenDto.AlmacenOrigenDestino });
            _sqlParam.Add(new SqlParameter("@codproducto", SqlDbType.VarChar) { Value = _almacenDto.CodProducto });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _almacenDto.Orden.ToUpper() });
            _sqlParam.Add(new SqlParameter("@nrolote", SqlDbType.VarChar) { Value = _almacenDto.NroLote });
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.VarChar) { Value = _almacenDto.Contrato });
            _sqlParam.Add(new SqlParameter("@talla1", SqlDbType.Int) { Value = _almacenDto.Tallas[0] });
            _sqlParam.Add(new SqlParameter("@talla2", SqlDbType.Int) { Value = _almacenDto.Tallas[1] });
            _sqlParam.Add(new SqlParameter("@talla3", SqlDbType.Int) { Value = _almacenDto.Tallas[2] });
            _sqlParam.Add(new SqlParameter("@talla4", SqlDbType.Int) { Value = _almacenDto.Tallas[3] });
            _sqlParam.Add(new SqlParameter("@talla5", SqlDbType.Int) { Value = _almacenDto.Tallas[4] });
            _sqlParam.Add(new SqlParameter("@talla6", SqlDbType.Int) { Value = _almacenDto.Tallas[5] });
            _sqlParam.Add(new SqlParameter("@talla7", SqlDbType.Int) { Value = _almacenDto.Tallas[6] });
            _sqlParam.Add(new SqlParameter("@cantidad", SqlDbType.Int) { Value = _almacenDto.Cantidad });
            _sqlParam.Add(new SqlParameter("@pesobruto", SqlDbType.Int) { Value = _almacenDto.PesoBruto });
            _sqlParam.Add(new SqlParameter("@pesoneto", SqlDbType.Int) { Value = _almacenDto.PesoNeto });

            return _trans.ExecuteQuery(query, _sqlParam);
        }

        public DataTable SelectIngresosProduccion(int _codAlmacen, string _fechaOperacion = "") {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string andFecha = "";
            string andFechaRPC = "";
            if (_fechaOperacion != "") {
                andFecha = " and da.fecha_operacion = convert(date, '" + _fechaOperacion + "')";
                andFechaRPC = " and fecha_ingreso = convert(date, '"+ _fechaOperacion +"')";
            }
            string query = @"
                select 
	                a.Descripcion as 'Almacen',
                    almSS.Almacen_SAP as 'AlmacenSAP',
	                da.tipo_movimiento as 'Tipo', 
	                da.numero_documento as 'Parte',
	                count(item_doc) as 'NroItems', 
	                da.fecha_operacion as 'Fecha',
	                max(rpc.horaIngreso) as 'HoraIngreso',
	                rpc.usuario as 'Usuario'
                from detalle_almacen da 
                inner join Almacenes a on a.Cod_Almacen = da.almacen
                inner join 
                (
	                select 
		                Orden, 
		                Lote, 
		                max(fecha_ingreso) as fechaIngreso, 
		                max(hora_ingreso) as horaIngreso,
		                usuario 
	                from Recepcion_Pto_Control 
	                where almacen = 800  " + andFechaRPC + @"
	                group by Orden, Lote, usuario
                ) rpc 
                on rpc.Orden = da.numero
                inner join Almacen_SIAA_SAP almSS on almSS.Cod_Almacen = da.almacen
                and rpc.Lote = convert(int, da.nro_lote)
                and da.fecha_operacion = convert(date, rpc.fechaIngreso)
                where 
	                da.almacen = @almacen
	                " + andFecha + @"
                group by da.tipo_movimiento, 
	                da.numero_documento, 
	                da.fecha_operacion, 
	                da.almacen,
                    almSS.Almacen_SAP,
	                a.Descripcion,
	                rpc.usuario
                order by da.numero_documento desc";

            _sqlParam.Add(new SqlParameter("@almacen", SqlDbType.Int) { Value = _codAlmacen });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        public DataTable SelectIngresosAlmacen(string _fechaInicial, string _fechaFinal, string _modelo) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = _modelo });
            _sqlParam.Add(new SqlParameter("@fechaini", SqlDbType.VarChar) { Value = _fechaInicial });
            _sqlParam.Add(new SqlParameter("@fechafin", SqlDbType.VarChar) { Value = _fechaFinal });
            return _trans.ReadingProcedure("PSIAA.IngresosAlmacen", _sqlParam);
        }

        public DataTable SelectIngresosProduccion(string nroParte, int almacenSap) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@parte", SqlDbType.VarChar) { Value = nroParte });
            _sqlParam.Add(new SqlParameter("@almacen", SqlDbType.Int) { Value = almacenSap });
            return _trans.ReadingProcedure("dbo.ITSM_INGRESOSPRODUCCION_2", _sqlParam);
        }
    }
}
