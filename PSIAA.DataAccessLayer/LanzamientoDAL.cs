using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using PSIAA.DataTransferObject;

namespace PSIAA.DataAccessLayer
{
    public class LanzamientoDAL
    {
        private Transactions _trans = new Transactions();

        public DataTable SelectLanzamientoPorOrden(string _orden, int _lote)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                modelo as 'Modelo',
	                color as 'Color',
                    Kilos as 'Peso',
	                numero_documento as 'Contrato',
	                talla1, talla2, talla3,
	                talla4, talla5, talla6,
	                talla7, talla8, talla9,
	                piezas1, piezas2, piezas3,
	                piezas4, piezas5, piezas6,
	                piezas7, piezas8, piezas9
                from lanzamiento_detalle 
                where Orden = @orden and Lote = @lote
                order by fecha_ingreso desc";

            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _lote });

            return _trans.ReadingQuery(query, _sqlParam);
        }

        public DataTable SelectCantidadesLanzadas(int _contrato, string _modelo, string _color)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                sum(piezas1) as Cant1,
	                sum(piezas2) as Cant2,
	                sum(piezas3) as Cant3,
	                sum(piezas4) as Cant4,
	                sum(piezas5) as Cant5,
	                sum(piezas6) as Cant6,
	                sum(piezas7) as Cant7,
	                sum(piezas8) as Cant8,
	                sum(piezas9) as Cant9
                from lanzamiento_detalle 
                where 
                    numero_documento = @contrato 
                    and modelo = @modelo 
                    and Color = @color ";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = _modelo });
            _sqlParam.Add(new SqlParameter("@color", SqlDbType.VarChar) { Value = _color });

            return _trans.ReadingQuery(query, _sqlParam);
        }

        public int SelectUtimoCorrelativoOrden(int _contrato, string _modelo)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                case when max(b.CorrelativoOrden) is null then 0 else max(b.CorrelativoOrden) end  as UltimoCorrelativoOrden
                from(
                select 
	                RIGHT(RTRIM(Orden), 2) as CorrelativoOrden 
                from lanzamiento_detalle 
                where 
	                numero_documento = @contrato 
	                and modelo = @modelo
                group by Orden) b";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = _modelo });

            string numero = _trans.ReadingEscalarQuery(query, _sqlParam);
            return int.Parse(numero == "" ? "0" : numero);
        }

        public int SelectUltimoNroLanzamiento(int _contrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                max(No_Lanzamiento) as UltimoNumero 
                from 
	                (select 
		                No_Lanzamiento 
	                from lanzamiento_detalle
	                where numero_documento = @contrato
	                group by No_Lanzamiento) base";
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });

            string numero = _trans.ReadingEscalarQuery(query, _sqlParam);
            return int.Parse(numero == "" ? "0" : numero);
        }

        public int InsertLanzamientoDet(LanzamientoDetDTO _lanzDet)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                insert into 
                lanzamiento_detalle(
	                almacen, tipo_movimiento, 
	                numero_documento, No_Lanzamiento, 
	                Orden, Lote, contrato, Color, 
	                grupo_talla, modelo, Cod_Producto,
	                talla1, talla2, talla3, 
	                talla4, talla5, talla6, 
	                talla7, talla8, talla9,
	                piezas1, piezas2, piezas3, 
	                piezas4, piezas5, piezas6, 
	                piezas7, piezas8, piezas9,
	                kilos1, kilos2, kilos3, 
	                kilos4, kilos5, kilos6, 
	                kilos7, kilos8, kilos9,
	                unidmin, jornal, usuario, 
	                fecha_ingreso, hora_ingreso, 
	                catoper, costohora,
	                observaciones, Kilos, 
	                Fecha_Solicitud)
                values(
	                0, 'LAN', @NumDoc, @NumLanz, @Orden, @Lote, @Maquina, @Color, '', @Modelo, @CodProducto, 
	                @Talla1, @Talla2, @Talla3, @Talla4, @Talla5, @Talla6, @Talla7, @Talla8, @Talla9,
	                @Piezas1, @Piezas2, @Piezas3, @Piezas4, @Piezas5, @Piezas6, @Piezas7, @Piezas8, @Piezas9, 
	                @Kilos1, @Kilos2, @Kilos3, @Kilos4, @Kilos5, @Kilos6, @Kilos7, @Kilos8, @Kilos9,
	                0, 0, @Usuario, convert(date, @FechaIngreso), @HoraIngreso, 0, 0, '', @Kilos, convert(date, @FechaSolicitud)
                )";

            _sqlParam.Add(new SqlParameter("@NumDoc", SqlDbType.Int) { Value = _lanzDet.NumDocumento });
            _sqlParam.Add(new SqlParameter("@NumLanz", SqlDbType.SmallInt) { Value = _lanzDet.NumLanzamiento });
            _sqlParam.Add(new SqlParameter("@Orden", SqlDbType.VarChar) { Value = _lanzDet.Orden });
            _sqlParam.Add(new SqlParameter("@Lote", SqlDbType.SmallInt) { Value = _lanzDet.Lote });
            _sqlParam.Add(new SqlParameter("@Maquina", SqlDbType.VarChar) { Value = _lanzDet.Maquina });
            _sqlParam.Add(new SqlParameter("@Color", SqlDbType.VarChar) { Value = _lanzDet.Color });
            _sqlParam.Add(new SqlParameter("@Modelo", SqlDbType.VarChar) { Value = _lanzDet.Modelo });
            _sqlParam.Add(new SqlParameter("@CodProducto", SqlDbType.VarChar) { Value = _lanzDet.CodProducto });
            _sqlParam.Add(new SqlParameter("@Talla1", SqlDbType.VarChar) { Value = _lanzDet.Tallas[0] });
            _sqlParam.Add(new SqlParameter("@Talla2", SqlDbType.VarChar) { Value = _lanzDet.Tallas[1] });
            _sqlParam.Add(new SqlParameter("@Talla3", SqlDbType.VarChar) { Value = _lanzDet.Tallas[2] });
            _sqlParam.Add(new SqlParameter("@Talla4", SqlDbType.VarChar) { Value = _lanzDet.Tallas[3] });
            _sqlParam.Add(new SqlParameter("@Talla5", SqlDbType.VarChar) { Value = _lanzDet.Tallas[4] });
            _sqlParam.Add(new SqlParameter("@Talla6", SqlDbType.VarChar) { Value = _lanzDet.Tallas[5] });
            _sqlParam.Add(new SqlParameter("@Talla7", SqlDbType.VarChar) { Value = _lanzDet.Tallas[6] });
            _sqlParam.Add(new SqlParameter("@Talla8", SqlDbType.VarChar) { Value = _lanzDet.Tallas[7] });
            _sqlParam.Add(new SqlParameter("@Talla9", SqlDbType.VarChar) { Value = _lanzDet.Tallas[8] });
            _sqlParam.Add(new SqlParameter("@Piezas1", SqlDbType.Int) { Value = _lanzDet.Piezas[0] });
            _sqlParam.Add(new SqlParameter("@Piezas2", SqlDbType.Int) { Value = _lanzDet.Piezas[1] });
            _sqlParam.Add(new SqlParameter("@Piezas3", SqlDbType.Int) { Value = _lanzDet.Piezas[2] });
            _sqlParam.Add(new SqlParameter("@Piezas4", SqlDbType.Int) { Value = _lanzDet.Piezas[3] });
            _sqlParam.Add(new SqlParameter("@Piezas5", SqlDbType.Int) { Value = _lanzDet.Piezas[4] });
            _sqlParam.Add(new SqlParameter("@Piezas6", SqlDbType.Int) { Value = _lanzDet.Piezas[5] });
            _sqlParam.Add(new SqlParameter("@Piezas7", SqlDbType.Int) { Value = _lanzDet.Piezas[6] });
            _sqlParam.Add(new SqlParameter("@Piezas8", SqlDbType.Int) { Value = _lanzDet.Piezas[7] });
            _sqlParam.Add(new SqlParameter("@Piezas9", SqlDbType.Int) { Value = _lanzDet.Piezas[8] });
            _sqlParam.Add(new SqlParameter("@Kilos1", SqlDbType.Decimal) { Value = _lanzDet.Kilos[0] });
            _sqlParam.Add(new SqlParameter("@Kilos2", SqlDbType.Decimal) { Value = _lanzDet.Kilos[1] });
            _sqlParam.Add(new SqlParameter("@Kilos3", SqlDbType.Decimal) { Value = _lanzDet.Kilos[2] });
            _sqlParam.Add(new SqlParameter("@Kilos4", SqlDbType.Decimal) { Value = _lanzDet.Kilos[3] });
            _sqlParam.Add(new SqlParameter("@Kilos5", SqlDbType.Decimal) { Value = _lanzDet.Kilos[4] });
            _sqlParam.Add(new SqlParameter("@Kilos6", SqlDbType.Decimal) { Value = _lanzDet.Kilos[5] });
            _sqlParam.Add(new SqlParameter("@Kilos7", SqlDbType.Decimal) { Value = _lanzDet.Kilos[6] });
            _sqlParam.Add(new SqlParameter("@Kilos8", SqlDbType.Decimal) { Value = _lanzDet.Kilos[7] });
            _sqlParam.Add(new SqlParameter("@Kilos9", SqlDbType.Decimal) { Value = _lanzDet.Kilos[8] });
            _sqlParam.Add(new SqlParameter("@Usuario", SqlDbType.VarChar) { Value = _lanzDet.Usuario });
            _sqlParam.Add(new SqlParameter("@FechaIngreso", SqlDbType.DateTime) { Value = _lanzDet.FechaIngreso });
            _sqlParam.Add(new SqlParameter("@HoraIngreso", SqlDbType.VarChar) { Value = _lanzDet.HoraIngreso });
            _sqlParam.Add(new SqlParameter("@Kilos", SqlDbType.Decimal) { Value = _lanzDet.KilosTot });
            _sqlParam.Add(new SqlParameter("@FechaSolicitud", SqlDbType.DateTime) { Value = _lanzDet.FechaSolicitud });

            return _trans.ExecuteQuery(query, _sqlParam);
        }

        public int InsertLanzamientoCab(LanzamientoCabDTO _lanzCab)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into 
                lanzamiento_cabecera(
	                almacen, tipo_movimiento, numero_documento, No_Lanzamiento, numero_prov,
	                fecha, Fecha_Entrega, taller, tipo_doc_ref, nro_de_serie, nro_docum_ref,
	                items_telacionados, moneda, tc, total_descuento, importe_total, completada, 
	                procesado, anulado, usuario_creacion, fecha_creacion, observaciones
                )
                values(
	                0, 'LAN', @NumDoc, @NumLanz, 0, convert(date, @Fecha), convert(date, @FechaEntrega), 
                    0, 0, 0, 0, 0, '', 0, 0, 0, 1, 0, 0, @Usuario, @FechaCreacion, ''
                )";

            _sqlParam.Add(new SqlParameter("@NumDoc", SqlDbType.Int) { Value = _lanzCab.NumDocumento });
            _sqlParam.Add(new SqlParameter("@NumLanz", SqlDbType.SmallInt) { Value = _lanzCab.NumLanzamiento });
            _sqlParam.Add(new SqlParameter("@Fecha", SqlDbType.DateTime) { Value = _lanzCab.Fecha });
            _sqlParam.Add(new SqlParameter("@FechaEntrega", SqlDbType.DateTime) { Value = _lanzCab.Fecha });
            _sqlParam.Add(new SqlParameter("@Usuario", SqlDbType.VarChar) { Value = _lanzCab.Usuario });
            _sqlParam.Add(new SqlParameter("@FechaCreacion", SqlDbType.Date) { Value = _lanzCab.Fecha });

            return _trans.ExecuteQuery(query, _sqlParam);
        }

        public int InsertLanzamientoComp(LanzamientoCompDTO _lanzComp)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into Lanzamiento_Compuesto(
	                almacen, tipo_movimiento, numero_documento, No_Lanzamiento, Orden, contrato, Color, Calidad, secuencia,
	                grupo_talla, modelo, Cod_Producto, kilos9, unidmin, jornal, usuario, fecha_ingreso, hora_ingreso, 
                    costohora, Cod_Producto_Solicitado, Kilos, Porcentaje, Fecha_Solicitud, Fecha_Atencion
                )
                values(
	                0, 'LAN', @NumContrato, @NumLanzamiento, @Orden, @Maquina, @Color, @Calidad, @Secuencia, 
                    '', @Modelo, @CodProducto, 0, 0, 0, @Usuario, convert(date, @FechaIngreso), @HoraIngreso, 
                    0, @CodProductoSol, @Kilos, @Porcentaje, convert(date, @FechaSolicitud), convert(date, @FechaAtencion)
                )";

            _sqlParam.Add(new SqlParameter("@NumContrato", SqlDbType.Int) { Value = _lanzComp.NumContrato });
            _sqlParam.Add(new SqlParameter("@NumLanzamiento", SqlDbType.SmallInt) { Value = _lanzComp.NumLanzamiento });
            _sqlParam.Add(new SqlParameter("@Orden", SqlDbType.VarChar) { Value = _lanzComp.Orden });
            _sqlParam.Add(new SqlParameter("@Maquina", SqlDbType.Char) { Value = _lanzComp.Maquina });
            _sqlParam.Add(new SqlParameter("@Color", SqlDbType.VarChar) { Value = _lanzComp.Color });
            _sqlParam.Add(new SqlParameter("@Calidad", SqlDbType.VarChar) { Value = _lanzComp.Calidad });
            _sqlParam.Add(new SqlParameter("@Secuencia", SqlDbType.SmallInt) { Value = _lanzComp.Secuencia });
            _sqlParam.Add(new SqlParameter("@Modelo", SqlDbType.VarChar) { Value = _lanzComp.Modelo });
            _sqlParam.Add(new SqlParameter("@CodProducto", SqlDbType.VarChar) { Value = _lanzComp.CodProducto });
            _sqlParam.Add(new SqlParameter("@Usuario", SqlDbType.VarChar) { Value = _lanzComp.Usuario });
            _sqlParam.Add(new SqlParameter("@FechaIngreso", SqlDbType.DateTime) { Value = _lanzComp.FechaIngreso });
            _sqlParam.Add(new SqlParameter("@HoraIngreso", SqlDbType.VarChar) { Value = _lanzComp.HoraIngreso });
            _sqlParam.Add(new SqlParameter("@CodProductoSol", SqlDbType.VarChar) { Value = _lanzComp.CodProductoSolicitado });
            _sqlParam.Add(new SqlParameter("@Kilos", SqlDbType.Float) { Value = _lanzComp.Kilos });
            _sqlParam.Add(new SqlParameter("@Porcentaje", SqlDbType.Real) { Value = _lanzComp.Porcentaje });
            _sqlParam.Add(new SqlParameter("@FechaSolicitud", SqlDbType.DateTime) { Value = _lanzComp.FechaAtencion });
            _sqlParam.Add(new SqlParameter("@FechaAtencion", SqlDbType.DateTime) { Value = _lanzComp.FechaAtencion });

            return _trans.ExecuteQuery(query, _sqlParam);
        }

        public DataTable SelectDetalleOrdenRequisicion(int numContrato, string orden)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                lc.Orden,
	                lc.Cod_Producto,
	                lc.Color,
	                (convert(float, lc.Porcentaje) /100) * totalKilos.kilos as Solicitado,
	                0.0 as Entregado,
	                0.0 as Devuelto
                from Lanzamiento_Compuesto lc 
                left join (
	                select 
		                numero_contrato, 
		                Cod_Modelo_AA, 
		                c_codcol 
	                from contrato_detalle 
	                where numero_contrato = @numcontrato
	                group by 
		                numero_contrato, 
		                Cod_Modelo_AA, 
		                c_codcol
                ) cd 
                    on cd.numero_contrato = lc.numero_documento
                    and cd.Cod_Modelo_AA = lc.modelo
                    and cd.c_codcol = lc.Color
                inner join (
	                select Orden, Sum(Kilos) as Kilos 
	                from Lanzamiento_Compuesto where numero_documento = @numcontrato and Orden = @orden
	                group by Orden
                ) as totalKilos on totalKilos.Orden = lc.Orden
                where lc.numero_documento = @numcontrato and lc.Orden = @orden
                group by 
	                lc.Orden,
	                lc.Cod_Producto,
	                lc.Color,
	                lc.Porcentaje, totalKilos.kilos
                union(
                select 
	                ld.Orden,
	                ld.Cod_Producto,
	                ld.Color,
	                sum(ld.Kilos) as Solicitado,
	                0.0 as Entregado,
	                0.0 as Devuelto
                from lanzamiento_detalle ld 
                left join (
	                select 
		                numero_contrato, 
		                Cod_Modelo_AA, 
		                c_codcol 
	                from contrato_detalle 
	                where numero_contrato = @numcontrato
	                group by 
		                numero_contrato, 
		                Cod_Modelo_AA, 
		                c_codcol
                ) cd 
                    on cd.numero_contrato = ld.numero_documento
                    and cd.Cod_Modelo_AA = ld.modelo
                    and cd.c_codcol = ld.Color
                where ld.numero_documento = @numcontrato and ld.Orden = @orden
                and SUBSTRING(ld.Color, 1, 2) != 'C0'
                group by 
	                ld.Orden,
	                ld.Cod_Producto,
	                ld.Color
                )";

            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = numContrato });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = orden });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        //Para Ordenes de Produccion Lanzadas/Asignadas
        public DataTable SelectOrdenesLanzadasAsignadas(int contrato, int catOperacion)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select
	                ld.Orden,
	                ld.modelo,
	                (mb.ccodigo+'-'+mb.codigo+'-'+mb.diseno) as ModeloSap,
	                max(p.nombre_comercial) as taller,
	                sum(ld.piezas1 + ld.piezas2 + ld.piezas3 
	                +ld.piezas4 + ld.piezas5 + ld.piezas6 
	                +ld.piezas7 + ld.piezas8 + ld.piezas9) as CantidadLanzada,
	                sum(aod.Cantidad) as CantidadAsignada,
	                sum(ld.Kilos) as Kilos,
	                ld.usuario,
	                ld.fecha_ingreso,
	                max(aod.FechaTermino) as FechaTermino,
	                ld.contrato as maquina
                from lanzamiento_detalle ld
                left join modelo_bac mb on mb.c_codmod = ld.modelo
                collate Modern_Spanish_CI_AS
                right join (
	                select 
		                baseAod.Categoria_Operacion,
		                baseAod.Orden,
		                baseAod.Lote,
		                baseAod.Cod_Proveedor,
		                sum(baseAod.Cantidad) as Cantidad,
		                max(baseAod.Fecha_de_termino) as FechaTermino
	                from
	                (
		                select 
			                base.Categoria_Operacion,
			                base.Orden,
			                base.Lote,
			                max(base.Cod_Proveedor) as Cod_Proveedor,
			                sum(base.Cantidad) as Cantidad,
			                max(base.Fecha_de_termino) as Fecha_de_termino
		                from
		                (
		                select
			                aod.Categoria_Operacion, 
			                aod.Numero_Orden, 
			                aod.Orden, 
			                aod.Lote, 
			                aod.Cod_Proveedor, 
			                (aod.Cantidad_1 + aod.Cantidad_2 + aod.Cantidad_3 + aod.Cantidad_4 + 
			                aod.Cantidad_5 + aod.Cantidad_6 + aod.Cantidad_7 + aod.Cantidad_8 + aod.Cantidad_9) as Cantidad,
			                aod.Fecha_de_termino
		                from Asignacion_de_ordenes_det aod
		                where aod.Categoria_Operacion = @catoperacion
		                group by 
			                aod.Categoria_Operacion, 
			                aod.Numero_Orden, 
			                aod.Orden, 
			                aod.Lote, 
			                aod.Cod_Proveedor,
			                (aod.Cantidad_1 + aod.Cantidad_2 + aod.Cantidad_3 + aod.Cantidad_4 + 
			                aod.Cantidad_5 + aod.Cantidad_6 + aod.Cantidad_7 + aod.Cantidad_8 + aod.Cantidad_9),
			                aod.Fecha_de_termino
		                ) base
		                group by
			                base.Categoria_Operacion,
			                base.Orden,
			                base.Lote
	                ) as baseAod
	                group by 
		                baseAod.Categoria_Operacion,
		                baseAod.Orden,
		                baseAod.Lote,
		                baseAod.Cod_Proveedor
                ) as aod on aod.Orden = ld.Orden and aod.Lote = ld.Lote
                left join proveedores p on p.cod_proveedor = aod.Cod_Proveedor
                where 
	                ld.numero_documento = @numcontrato
                group by
	                ld.Orden,
	                ld.modelo,
	                (mb.ccodigo+'-'+mb.codigo+'-'+mb.diseno),
	                ld.usuario,
	                ld.fecha_ingreso,
	                ld.contrato
                order by 
	                ld.Orden desc";

            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = contrato });
            _sqlParam.Add(new SqlParameter("@catoperacion", SqlDbType.Int) { Value = catOperacion });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        //Ordenes de Produccion Lanzadas
        public DataTable SelectOrdenesLanzadas(int contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                ld.Orden,
	                ld.modelo,
	                (mb.ccodigo+'-'+mb.codigo+'-'+mb.diseno) as ModeloSap,
	                '' as taller,
	                sum(ld.piezas1 + ld.piezas2 + ld.piezas3 
	                +ld.piezas4 + ld.piezas5 + ld.piezas6 
	                +ld.piezas7 + ld.piezas8 + ld.piezas9) as CantidadLanzada,
	                0 as CantidadAsignada,
	                sum(ld.Kilos) as Kilos,
	                ld.usuario,
	                ld.fecha_ingreso,
	                '' as FechaTermino,
                    ld.contrato as maquina
                from lanzamiento_detalle ld
                left join modelo_bac mb on mb.c_codmod = ld.modelo
                collate Modern_Spanish_CI_AS
                left join(
	                select
		                aod.Orden, 
		                aod.Lote
	                from Asignacion_de_ordenes_det aod
	                where aod.Categoria_Operacion != 0 and aod.Cod_Proveedor != '' 
	                group by 
		                aod.Orden, 
		                aod.Lote
                ) as baseAod on baseAod.Orden = ld.Orden and baseAod.Lote = ld.Lote
                where 
	                baseAod.Orden is null and baseAod.Lote is null and ld.numero_documento = @contrato
                group by
	                ld.Orden,
	                ld.modelo,
	                (mb.ccodigo+'-'+mb.codigo+'-'+mb.diseno),
	                ld.usuario,
	                ld.fecha_ingreso,
                    ld.contrato
                order by 
	                ld.Orden desc";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = contrato });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        public DataTable SelectDetalleOrdenesProduccion(int contrato, string orden) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                ld.Orden,
	                ld.Lote,
	                ld.Color,
	                ld.Cod_Producto,
	                (ld.piezas1 + ld.piezas2 + ld.piezas3 
	                +ld.piezas4 + ld.piezas5 + ld.piezas6 
	                +ld.piezas7 + ld.piezas8 + ld.piezas9) as Cantidad,
	                case when ld.piezas1 != 0 then ld.talla1
		                when ld.piezas2 != 0 then ld.talla2
		                when ld.piezas3 != 0 then ld.talla3
		                when ld.piezas4 != 0 then ld.talla4
		                when ld.piezas5 != 0 then ld.talla5
		                when ld.piezas6 != 0 then ld.talla6
		                when ld.piezas7 != 0 then ld.talla7
		                when ld.piezas8 != 0 then ld.talla8
	                else ld.talla9 
	                end as Talla,
	                ld.Kilos
                from lanzamiento_detalle ld
                where ld.numero_documento = @contrato and ld.Orden = @orden
                order by ld.Lote ";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = contrato });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = orden });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        public DataTable SelectCombinacionesPartidaLanzamiento(int contrato, string orden, string modelo) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                ld.modelo, 
	                ld.Color as ColorPrim, 
	                lc.Color as ColorSec,
	                SUBSTRING(lc.Cod_Producto, CHARINDEX('-', lc.Cod_Producto, 0) + 1, LEN(lc.Cod_Producto))  as Partida,
	                ld.Lote, 
	                case when ld.piezas1 != 0 then ld.talla1
		                when ld.piezas2 != 0 then ld.talla2
		                when ld.piezas3 != 0 then ld.talla3
		                when ld.piezas4 != 0 then ld.talla4
		                when ld.piezas5 != 0 then ld.talla5
		                when ld.piezas6 != 0 then ld.talla6
		                when ld.piezas7 != 0 then ld.talla7
		                when ld.piezas8 != 0 then ld.talla8
	                else ld.talla9 
	                end as Talla,
	                (ld.piezas1 + ld.piezas2 + ld.piezas3 
	                +ld.piezas4 + ld.piezas5 + ld.piezas6 
	                +ld.piezas7 + ld.piezas8 + ld.piezas9) as Piezas, lc.secuencia
                into #base
                from lanzamiento_detalle ld
                inner join Lanzamiento_Compuesto lc 
	                on lc.Orden = ld.Orden
	                and lc.Cod_Producto_Solicitado = ld.Cod_Producto
                where ld.numero_documento = @contrato and ld.Orden = @orden and ld.modelo = @modelo
                (
                SELECT base.modelo, base.ColorPrim, 
	                STUFF((
		                SELECT 
			                '+' + rtrim(b.ColorSec) AS [text()]
		                FROM #base b
                        WHERE b.modelo = base.modelo
		                group by b.ColorSec, b.secuencia
		                order by b.secuencia
                        FOR XML PATH('')
	                ), 1, 1, '' ) AS ColorSec,
	                STUFF((
		                SELECT 
			                '+' + rtrim(b.Partida) AS [text()]
		                FROM #base b
                        WHERE b.modelo = base.modelo
		                group by b.Partida, b.secuencia
		                order by b.secuencia
                        FOR XML PATH('')
	                ), 1, 1, '' ) AS Partida,
	                base.Lote, base.Talla, base.Piezas
                FROM #base base
                group by 
	                base.modelo,
	                base.ColorPrim,
	                base.Lote, base.Talla, base.Piezas
                ) 
                union
                (select 
	                ld.modelo,
	                ld.Color as ColorPrim,
	                ld.Color as ColorSec,
	                SUBSTRING(ld.Cod_Producto, CHARINDEX('-', ld.Cod_Producto, 0) + 1, LEN(ld.Cod_Producto))  as Partida,
	                ld.Lote,
		                case when ld.piezas1 != 0 then ld.talla1
		                when ld.piezas2 != 0 then ld.talla2
		                when ld.piezas3 != 0 then ld.talla3
		                when ld.piezas4 != 0 then ld.talla4
		                when ld.piezas5 != 0 then ld.talla5
		                when ld.piezas6 != 0 then ld.talla6
		                when ld.piezas7 != 0 then ld.talla7
		                when ld.piezas8 != 0 then ld.talla8
	                else ld.talla9 
	                end as Talla,
	                (ld.piezas1 + ld.piezas2 + ld.piezas3 
	                +ld.piezas4 + ld.piezas5 + ld.piezas6 
	                +ld.piezas7 + ld.piezas8 + ld.piezas9) as Piezas
                from lanzamiento_detalle ld
                where ld.numero_documento = @contrato and ld.Orden = @orden and ld.modelo = @modelo
                and substring(ld.Color, 1, 2) != 'C0'
                )
                drop table #base";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = contrato });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = orden });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        public DataTable SelectPesoPorTallas(int contrato, string orden) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                modelo,
	                case when piezas1 != 0 then talla1
		                when piezas2 != 0 then talla2
		                when piezas3 != 0 then talla3
		                when piezas4 != 0 then talla4
		                when piezas5 != 0 then talla5
		                when piezas6 != 0 then talla6
		                when piezas7 != 0 then talla7
		                when piezas8 != 0 then talla8
	                else talla9 
	                end as Talla,
	                Kilos,
	                (piezas1 + piezas2 + piezas3 + piezas4 + piezas5 + piezas6 + piezas7 + piezas8 + piezas9) as Cantidad
                from lanzamiento_detalle 
                where numero_documento = @contrato and Orden = @orden";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = contrato });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = orden });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
