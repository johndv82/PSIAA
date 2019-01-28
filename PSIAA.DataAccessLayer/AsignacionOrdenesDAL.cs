using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PSIAA.DataTransferObject;

namespace PSIAA.DataAccessLayer
{
    public class AsignacionOrdenesDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD)
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la base de datos para obtener el Total de Asignaciones de Ordenes por Contrato.
        /// </summary>
        /// <param name="_categoria">Código de Categoría</param>
        /// <param name="_contrato">Número de Contrato</param>
        /// <param name="_modelo">Modelo de prenda</param>
        /// <param name="_color">Color de prenda</param>
        /// <returns></returns>
        public DataTable SelectTotalAsignacionOrdenes(int _categoria, int _contrato, string _modelo, string _color) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            _sqlParam.Add(new SqlParameter("@categoria", SqlDbType.Int) { Value = _categoria });
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.VarChar) { Value = _contrato });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = _modelo });
            _sqlParam.Add(new SqlParameter("@color", SqlDbType.VarChar) { Value = _color });

            return _trans.ReadingProcedure("PSIAA.AsignacionesOrdenes", _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el último número de órden de asignación ingresado.
        /// </summary>
        /// <returns>Variable de tipo string con el número de orden de asignación</returns>
        public string SelectUltimoNumeroOrden() {
            string query = @"
                select 
                    top 1 Numero_Orden
                from Asignacion_de_ordenes_cab
                order by Numero_Orden desc";

            return _trans.ReadingEscalarQuery(query, null);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener todas las asignaciones con la tarifa y el precio aprobados.
        /// Validando que no hayan sido facturadas.
        /// </summary>
        /// <param name="_codProveedor">Código de Proveedor</param>
        /// <param name="_moneda">Código de Moneda (S/D)</param>
        /// <param name="_fechaAprobPrecio">Fecha de Aprobación de Precio</param>
        /// <returns></returns>
        public DataTable SelectAsignacionOrdenes(string _codProveedor, string _moneda, string _fechaAprobPrecio) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
                    aod.Categoria_Operacion,
	                aod.Numero_Orden as NumeroAsignacion,
	                aod.Orden,
	                aod.Lote,
	                aod.Categoria,
                    ld.modelo,
	                aod.Precio_Unitario,
	                aod.Color,
	                (aod.Cantidad_1 + aod.Cantidad_2 + aod.Cantidad_3
	                +aod.Cantidad_4 + aod.Cantidad_5 + aod.Cantidad_6
	                +aod.Cantidad_7 + aod.Cantidad_8 + aod.Cantidad_9) as Cantidad,
	                sum(aod.Costo_Soles) as Costo_Soles,
	                aod.Tarifa_Dolares as Tarifa_Dolares,
	                sum(coalesce(aod.Costo_Dolares, 0)) as Costo_Dolares,
	                aod.Fecha_de_asignacion,
	                aod.Fecha_de_termino,
	                aod.Usuario_Aprob_Prec
                from Asignacion_de_ordenes_det aod
                left join Doc_pago_taller_asig dpta
                    on dpta.Orden = aod.Orden and dpta.Lote = aod.Lote
                    and dpta.cod_proveedor = @codproveedor 
	                and dpta.Categoria = aod.Categoria
	                and dpta.Nro_Ord_Asig = aod.Numero_Orden
	                and dpta.Cod_Proceso = aod.Proceso
                left join lanzamiento_detalle ld
	                on ld.Orden = aod.Orden
	                and ld.Lote = aod.Lote
                left join Doc_pago_taller_libre dptl 
                    on dptl.Orden = aod.Orden and dptl.Lote = aod.Lote
                    and dptl.cod_proveedor = @codproveedor 
                    and dptl.Categoria = aod.Categoria
                where 
                    aod.Cod_Proveedor = @codproveedor 
                    and dpta.Orden is null and dptl.Orden is null
                    and aod.Precio_Aprobado = 1
                    and aod.Numero_Orden_2 = 0
                    and aod.Terminado = 0x54
                    and aod.Moneda = @moneda
                    and convert(date, aod.Fecha_Aprob_Precio) like '%'+ @fechaAprobacionPrecio +'%'
                group by
	                aod.Categoria_Operacion,
	                aod.Numero_Orden,
	                aod.Orden,
	                aod.Lote,
	                aod.Categoria,
                    ld.Modelo,
	                aod.Precio_Unitario,
                    aod.Tarifa_Dolares,
	                aod.Color,
	                (aod.Cantidad_1 + aod.Cantidad_2 + aod.Cantidad_3
	                +aod.Cantidad_4 + aod.Cantidad_5 + aod.Cantidad_6
	                +aod.Cantidad_7 + aod.Cantidad_8 + aod.Cantidad_9),
	                aod.Fecha_de_asignacion,
	                aod.Fecha_de_termino,
	                aod.Usuario_Aprob_Prec
                order by aod.Fecha_de_asignacion asc";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProveedor });
            _sqlParam.Add(new SqlParameter("@moneda", SqlDbType.VarChar) { Value = _moneda });
            _sqlParam.Add(new SqlParameter("@fechaAprobacionPrecio", SqlDbType.VarChar) { Value = _fechaAprobPrecio });

            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de actualización de Número de Orden2 en la tabla Asignacion_de_ordenes_det.
        /// </summary>
        /// <param name="_docPagoTaller">Objeto de DocumentoPagoTallerDTO</param>
        /// <returns>Variable de tipo int con la cantidad de registros actualizados.</returns>
        public int UpdateNumeroOrden2AsignacionOrdenes(DocumentoPagoTallerDTO _docPagoTaller) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                update Asignacion_de_ordenes_det 
	                set Numero_Orden_2 = @numdocumento 
                where 
	                Cod_Proveedor = @codproveedor 
	                and Numero_Orden = @numorden 
	                and Orden = @orden 
	                and Lote = @lote 
	                and Categoria = @categoria 
	                and Proceso = @proceso";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _docPagoTaller.CodProveedor });
            _sqlParam.Add(new SqlParameter("@numorden", SqlDbType.Float) { Value = _docPagoTaller.NumOrdenAsignacion });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _docPagoTaller.Orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _docPagoTaller.Lote });
            _sqlParam.Add(new SqlParameter("@categoria", SqlDbType.Int) { Value = _docPagoTaller.Categoria });
            _sqlParam.Add(new SqlParameter("@proceso", SqlDbType.Int) { Value = _docPagoTaller.CodProceso });
            _sqlParam.Add(new SqlParameter("@numdocumento", SqlDbType.Int) { Value = _docPagoTaller.NroDocumento });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener los modelos de asignaciones por aprobar.
        /// </summary>
        /// <param name="_codProveedor">Código de Proveedor</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectModelosAsignacionOrdenesPorAprobar(string _codProveedor) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                ld.modelo 
                from Asignacion_de_ordenes_det aod
                inner join lanzamiento_detalle ld 
	                on ld.Orden = aod.Orden 
	                and ld.Lote = aod.lote	
                where aod.Cod_Proveedor = @codproveedor 
	                and year(aod.Fecha_de_asignacion) > 2015
	                and aod.Numero_Orden_2 = 0
                    and aod.Terminado = 0x54
                group by ld.modelo
                order by ld.modelo ";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProveedor });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el detalle de las asignaciones de ordenes por aprobar.
        /// </summary>
        /// <param name="_codProveedor">Código de Proveedor</param>
        /// <param name="_modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectAsignacionOrdenesParaAprobar(string _codProveedor, string _modelo) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                base.Categoria_Operacion,
	                base.Numero_Orden,
	                base.modelo,
	                base.Orden, 
	                base.Lote,
	                sum(base.Cantidad) as Cantidad,
	                base.Moneda,
	                base.TarifaSoles,
	                base.TarifaDolares,
	                sum(base.CostoSoles) as CostoSoles,
	                sum(base.CostoDolares) as CostoDolares,
	                base.Precio_Aprobado,
	                base.Fecha_de_asignacion,
	                base.Usuario_Asignacion,
	                base.Fecha_Aprob_Precio,
	                base.Usuario_Aprob_Prec
                from(
                    select 
	                    aod.Categoria_Operacion,
	                    aod.Numero_Orden,
	                    ld.modelo,
	                    aod.Orden,
	                    aod.lote,
	                    (aod.Cantidad_1+aod.Cantidad_2+aod.Cantidad_3
	                    +aod.Cantidad_4+aod.Cantidad_5+aod.Cantidad_6
	                    +aod.Cantidad_7+aod.Cantidad_8+aod.Cantidad_9) as Cantidad,
	                    coalesce(aod.Moneda, '') as Moneda,
	                    aod.Precio_Unitario as TarifaSoles,
	                    coalesce(aod.Tarifa_Dolares, 0) as TarifaDolares,
	                    sum(aod.Costo_Soles) as CostoSoles,
	                    sum(aod.Costo_Dolares) as CostoDolares,
	                    aod.Precio_Aprobado,
	                    aod.Fecha_de_asignacion,
	                    aod.Usuario_Asignacion,
	                    coalesce(aod.Fecha_Aprob_Precio, '') as Fecha_Aprob_Precio,
	                    coalesce(aod.Usuario_Aprob_Prec, '') as Usuario_Aprob_Prec
                    from Asignacion_de_ordenes_det aod
                    inner join lanzamiento_detalle ld 
	                    on ld.Orden = aod.Orden and ld.Lote = aod.Lote
                    where aod.Cod_Proveedor = @codproveedor
	                    and year(aod.Fecha_de_asignacion) > 2015
	                    and aod.Numero_Orden_2 = 0
                        and aod.Terminado = 0x54
	                    and ld.modelo like '%'+ @modelo +'%'
                    group by 
	                    aod.Categoria_Operacion,
	                    aod.Numero_Orden,
	                    ld.modelo,
	                    aod.Orden,
	                    aod.lote,
	                    (aod.Cantidad_1+aod.Cantidad_2+aod.Cantidad_3
	                    +aod.Cantidad_4+aod.Cantidad_5+aod.Cantidad_6
	                    +aod.Cantidad_7+aod.Cantidad_8+aod.Cantidad_9),
	                    aod.Moneda,
	                    aod.Precio_Unitario,
	                    aod.Tarifa_Dolares,
	                    aod.Precio_Aprobado,
	                    aod.Fecha_de_asignacion,
	                    aod.Usuario_Asignacion,
	                    aod.Fecha_Aprob_Precio,
	                    aod.Usuario_Aprob_Prec
                ) as base
	            group by
	                base.Categoria_Operacion,
	                base.Numero_Orden,
	                base.modelo,
	                base.Orden, 
	                base.Lote,
	                base.Moneda,
	                base.TarifaSoles,
	                base.TarifaDolares,
	                base.Precio_Aprobado,
	                base.Fecha_de_asignacion,
	                base.Usuario_Asignacion,
	                base.Fecha_Aprob_Precio,
	                base.Usuario_Aprob_Prec
	            order by 
		            base.Numero_Orden, 
                    base.Orden, base.Lote";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProveedor });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = _modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el detalle, por proceso, 
        /// de asignacion de ordenes terminadas, validando que no hayan sido facturadas.
        /// </summary>
        /// <param name="_codProv">Código de Proveedor</param>
        /// <param name="_asignacion">Número de Asignación</param>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <param name="_categoria">Código de Categoria</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectDetalleGrupoAsignacionOrdenes(string _codProv, string _asignacion, string _orden, int _lote, int _categoria) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                aod.Cod_Proveedor,
	                aod.Categoria_Operacion,
	                Numero_Orden,
	                aod.Orden,
	                aod.Lote,
	                aod.Categoria,
	                Proceso,
	                coalesce(Costo_Soles, 0) as Costo_Soles,
	                coalesce(Costo_Dolares, 0) as Costo_Dolares
                from Asignacion_de_ordenes_det aod 
                left join Doc_pago_taller_asig dpta
                    on dpta.Orden = aod.Orden and dpta.Lote = aod.Lote
                    and dpta.cod_proveedor = @codproveedor 
	                and dpta.Categoria = aod.Categoria
	                and dpta.Nro_Ord_Asig = aod.Numero_Orden
	                and dpta.Cod_Proceso = aod.Proceso
	                and dpta.Cod_Cat_Oper = aod.Categoria_Operacion
                left join Doc_pago_taller_libre dptl 
                    on dptl.Orden = aod.Orden and dptl.Lote = aod.Lote
                    and dptl.cod_proveedor = @codproveedor 
                    and dptl.Categoria = aod.Categoria
                where 
	                aod.Cod_Proveedor = @codproveedor 
	                and dpta.Orden is null and dptl.Orden is null
	                and aod.Numero_Orden = @asignacion 
	                and aod.Orden = @orden and aod.Lote = @lote 
	                and aod.Categoria = @categoria
                    and aod.Terminado = 0x54";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProv });
            _sqlParam.Add(new SqlParameter("@asignacion", SqlDbType.VarChar) { Value = _asignacion });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _lote });
            _sqlParam.Add(new SqlParameter("@categoria", SqlDbType.Int) { Value = _categoria });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener solo los procesos por asignación de ordenes terminadas.
        /// </summary>
        /// <param name="_codProv">Código de Proveedor</param>
        /// <param name="_modelo">Modelo de Prenda</param>
        /// <param name="_asign">Número de Asignación</param>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectProcesosAsignacion(string _codProv, string _modelo, string _asign, string _orden, int _lote) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            //Son los procesos que se usan para hacer join con PostgreSQL

            string query = @"
                select 
	                aod.Proceso 
                from Asignacion_de_ordenes_det aod
                inner join lanzamiento_detalle ld 
	                on ld.Orden = aod.Orden 
	                and ld.Lote = aod.Lote
                where aod.Cod_Proveedor = @codproveedor 
	                and ld.modelo = @modelo
	                and year(aod.Fecha_de_asignacion) > 2015
	                and aod.Numero_Orden_2 = 0
                    and aod.Numero_Orden = @asignacion
                    and aod.Terminado = 0x54
                    and aod.Orden = @orden
                    and aod.Lote = @lote
                group by aod.Proceso";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProv });
            _sqlParam.Add(new SqlParameter("@asignacion", SqlDbType.VarChar) { Value = _asign });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = _modelo });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _lote });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de actualización de tarifa y tiempo, en la tabla Asignacion_de_ordenes_det.
        /// </summary>
        /// <param name="_codProv">Código de Proveedor</param>
        /// <param name="_catOpe">Código de Categoria de Operación</param>
        /// <param name="_numAsign">Número de Asignación</param>
        /// <param name="_moneda">Código de Moneda (S/D)</param>
        /// <param name="_tarifa">Valor de Tarifa</param>
        /// <param name="_tiempo">Valor de Tiempo</param>
        /// <param name="_proceso">Código de Proceso</param>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <returns>Variable de tipo int con la cantidad de registros actualizados.</returns>
        public int UpdateTarifaTiempoAsignacionOrdenes(string _codProv, int _catOpe, string _numAsign,
                                                string _moneda, double _tarifa, double _tiempo, int _proceso,
                                                string _orden, int _lote) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string queryTarifa= "";

            if (_moneda == "S")
            {
                queryTarifa = @"
                    Precio_Unitario = @tarifasoles ";
                _sqlParam.Add(new SqlParameter("@tarifasoles", SqlDbType.Decimal) { Value = _tarifa });
            }
            else {
                queryTarifa = @"
	                Tarifa_Dolares = @tarifadolares ";
                _sqlParam.Add(new SqlParameter("@tarifadolares", SqlDbType.Decimal) { Value = _tarifa });
            }
            
            string query = @"
                update Asignacion_de_ordenes_det set
                    Tiempo = @tiempo,
                    " + queryTarifa +
                @"where 
	                Cod_Proveedor = @codproveedor
	                and Categoria_Operacion = @categoriaoperacion
	                and Numero_Orden_2 = 0
	                and Numero_Orden = @numeroasignacion
                    and Proceso = @proceso
                    and Orden = @orden
                    and Lote = @lote ";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProv });
            _sqlParam.Add(new SqlParameter("@categoriaoperacion", SqlDbType.Int) { Value = _catOpe });
            _sqlParam.Add(new SqlParameter("@numeroasignacion", SqlDbType.VarChar) { Value = _numAsign });
            _sqlParam.Add(new SqlParameter("@tiempo", SqlDbType.Real) { Value = _tiempo });
            _sqlParam.Add(new SqlParameter("@proceso", SqlDbType.Int) { Value = _proceso });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _lote });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de actualización de precios y recalculo de costos, en la tabla Asignacion_de_ordenes_det.
        /// </summary>
        /// <param name="_codProv">Código de Proveedor</param>
        /// <param name="_catOpe">Código de Categoria de Operación</param>
        /// <param name="_numAsign">Número de Asignación</param>
        /// <param name="_moneda">Código de Moneda (S/D)</param>
        /// <param name="_fechaAprobacion">Fecha de Aprobación de Precio</param>
        /// <param name="_usuarioAprobacion">Usuario quien Aprobó Precio</param>
        /// <param name="_aprobado">Estado de Aprobación (0/1)</param>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <returns>Variable de tipo int con la cantidad de registros actualizados.</returns>
        public int UpdatePrecioAsignacionesOrdenes_RE(string _codProv, int _catOpe, string _numAsign,
                                                string _moneda, string _fechaAprobacion,
                                                string _usuarioAprobacion, int _aprobado, string _orden, int _lote) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string queryUsuario = "";
            string query = "";

            if (_aprobado == 1)
            {
                queryUsuario = @"
                    Fecha_Aprob_Precio = convert(date, @fechaaprobacion),
                    Usuario_Aprob_Prec = @usuarioaprobacion, ";
                _sqlParam.Add(new SqlParameter("@fechaaprobacion", SqlDbType.VarChar) { Value = _fechaAprobacion });
                _sqlParam.Add(new SqlParameter("@usuarioaprobacion", SqlDbType.VarChar) { Value = _usuarioAprobacion });
            }
            else
            {
                queryUsuario = @"
                    Fecha_Aprob_Precio = NULL,
                    Usuario_Aprob_Prec = NULL, ";
            }

            if (_moneda == "S")
            {
                query = @"
                update Asignacion_de_ordenes_det 
	                set Costo_Soles = round(((Tiempo/60) * Precio_Unitario) * (Cantidad_1 + Cantidad_2 + Cantidad_3 + Cantidad_4 + Cantidad_5 +
		                Cantidad_6 + Cantidad_7 + Cantidad_8 + Cantidad_9) , 2), 
                        " + queryUsuario + @"
                        Precio_Aprobado = @aprobado, 
                        Moneda = 'S', 
		                Tarifa_Dolares = 0, Costo_Dolares = 0
                where 
                    Cod_Proveedor = @codproveedor 
                    and Numero_Orden = @numeroasignacion
                    and Orden = @orden
                    and Lote = @lote ";
            }
            else {
                query = @"
                update Asignacion_de_ordenes_det 
	                set Costo_Dolares = round(((Tiempo/60) * Tarifa_Dolares) * (Cantidad_1 + Cantidad_2 + Cantidad_3 + Cantidad_4 + Cantidad_5 +
	                Cantidad_6 + Cantidad_7 + Cantidad_8 + Cantidad_9) , 2), 
                    " + queryUsuario + @"
                    Precio_Aprobado = @aprobado, 
                    Moneda = 'D', 
	                Precio_Unitario = 0, Costo_Soles = 0
                where 
                    Cod_Proveedor = @codproveedor 
                    and Numero_Orden = @numeroasignacion                    
                    and Orden = @orden
                    and Lote = @lote ";
            }

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProv });
            _sqlParam.Add(new SqlParameter("@categoriaoperacion", SqlDbType.Int) { Value = _catOpe });
            _sqlParam.Add(new SqlParameter("@numeroasignacion", SqlDbType.VarChar) { Value = _numAsign });
            _sqlParam.Add(new SqlParameter("@aprobado", SqlDbType.Int) { Value = _aprobado });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _lote });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener los procesos asignados por Orden y Lote.
        /// </summary>
        /// <param name="_codProv">Código de Proveedor</param>
        /// <param name="_nroAsig">Número de Asignación</param>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <param name="_cat">Código de Categoria</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectProcesosAsignacionPorOrdenLote(string _codProv, string _nroAsig, string _orden, 
                                                            int _lote, int _cat) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                aod.Proceso, 
	                aod.Moneda,
	                aod.Tiempo,
	                aod.Precio_Unitario as TarifaSoles,
	                aod.Costo_Soles,
	                aod.Tarifa_Dolares,
	                aod.Costo_Dolares
                from Asignacion_de_ordenes_det aod
                inner join lanzamiento_detalle ld 
                    on ld.Orden = aod.Orden 
                    and ld.Lote = aod.Lote
                where Cod_Proveedor = @codproveedor 
                    and Numero_Orden = @nroasignacion
                    and aod.Orden = @orden 
                    and aod.Lote = @lote 
                    and aod.Categoria = @categoria 
                    and aod.Precio_Aprobado = 1
                    and aod.Numero_Orden_2 = 0
                    and aod.Terminado = 0x54";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProv });
            _sqlParam.Add(new SqlParameter("@nroasignacion", SqlDbType.VarChar) { Value = _nroAsig });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _lote });
            _sqlParam.Add(new SqlParameter("@categoria", SqlDbType.Int) { Value = _cat });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de inserción a la tabla Asignacion_de_ordenes_cab.
        /// </summary>
        /// <param name="asigOrdenCab">Objeto de tipo AsignacionOrdenCabDTO</param>
        /// <returns>Variable de tipo int con la cantidad de registros ingresados.</returns>
        public int InsertAsignacionOrdenCabecera(AsignacionOrdenCabDTO asigOrdenCab) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into Asignacion_de_ordenes_cab values(
	                @catoperacion, 
	                @nroasignacion, 
	                @codproveedor, 
	                convert(date, @fechageneracion), 
	                convert(date, @fechaentrega), 
	                0, @usuario, 
	                convert(date, @fechageneracion),
	                @horageneracion, null, null, @completo
                )";

            _sqlParam.Add(new SqlParameter("@catoperacion", SqlDbType.Int) { Value = asigOrdenCab.CodCatOperacion });
            _sqlParam.Add(new SqlParameter("@nroasignacion", SqlDbType.VarChar) { Value = asigOrdenCab.NroAsignacion });
            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = asigOrdenCab.CodProveedor });
            _sqlParam.Add(new SqlParameter("@fechageneracion", SqlDbType.DateTime) { Value = asigOrdenCab.FechaGeneracion });
            _sqlParam.Add(new SqlParameter("@fechaentrega", SqlDbType.DateTime) { Value = asigOrdenCab.FechaEntrega });
            _sqlParam.Add(new SqlParameter("@usuario", SqlDbType.VarChar) { Value = asigOrdenCab.Usuario });
            _sqlParam.Add(new SqlParameter("@horageneracion", SqlDbType.VarChar) { Value = asigOrdenCab.HoraGeneracion });
            _sqlParam.Add(new SqlParameter("@completo", SqlDbType.SmallInt) { Value = asigOrdenCab.Completo });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de inserción a la tabla Asignacion_de_ordenes_det.
        /// </summary>
        /// <param name="asigOrdenDet">Objeto de tipo AsignacionOrdenDetDTO</param>
        /// <returns>Variable de tipo int con la cantidad de registros ingresados.</returns>
        public int InsertAsignacionOrdenDetalle(AsignacionOrdenDetDTO asigOrdenDet) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into Asignacion_de_ordenes_det values(
	                @catoperacion, 
	                @nroasignacion, 
	                @orden, @lote, 
	                @categoria, 
	                @proceso, 
	                @codproveedor, 0, 0, 
	                convert(date, @fechaasignacion), convert(date, @fechatermino),
	                @activo, @terminado, 
	                @color, @talla1, @talla2, @talla3, @talla4, @talla5, @talla6, @talla7, @talla8, @talla9,
	                @cantidad1, @cantidad2, @cantidad3, @cantidad4, @cantidad5, @cantidad6, @cantidad7, @cantidad8, @cantidad9,
	                convert(date, @fechafinalizacion), 
	                @horaingreso, @usuario, @usuario,
	                null, 0, 0, null, null, null, 0, null, null, null, null
                )";

            _sqlParam.Add(new SqlParameter("@catoperacion", SqlDbType.Int) { Value = asigOrdenDet.CodCatOperacion });
            _sqlParam.Add(new SqlParameter("@nroasignacion", SqlDbType.VarChar) { Value = asigOrdenDet.NroAsignacion });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = asigOrdenDet.Orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = asigOrdenDet.Lote });
            _sqlParam.Add(new SqlParameter("@categoria", SqlDbType.Int) { Value = asigOrdenDet.Categoria });
            _sqlParam.Add(new SqlParameter("@proceso", SqlDbType.Int) { Value = asigOrdenDet.Proceso });
            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = asigOrdenDet.CodProveedor });
            _sqlParam.Add(new SqlParameter("@fechaasignacion", SqlDbType.DateTime) { Value = asigOrdenDet.FechaAsignacion });
            _sqlParam.Add(new SqlParameter("@fechatermino", SqlDbType.DateTime) { Value = asigOrdenDet.FechaTermino });
            _sqlParam.Add(new SqlParameter("@activo", SqlDbType.Int) { Value = asigOrdenDet.Activo });
            _sqlParam.Add(new SqlParameter("@terminado", SqlDbType.Int) { Value = asigOrdenDet.Terminado });
            _sqlParam.Add(new SqlParameter("@color", SqlDbType.VarChar) { Value = asigOrdenDet.Color });
            _sqlParam.Add(new SqlParameter("@talla1", SqlDbType.VarChar) { Value = asigOrdenDet.Tallas[0] });
            _sqlParam.Add(new SqlParameter("@talla2", SqlDbType.VarChar) { Value = asigOrdenDet.Tallas[1] });
            _sqlParam.Add(new SqlParameter("@talla3", SqlDbType.VarChar) { Value = asigOrdenDet.Tallas[2] });
            _sqlParam.Add(new SqlParameter("@talla4", SqlDbType.VarChar) { Value = asigOrdenDet.Tallas[3] });
            _sqlParam.Add(new SqlParameter("@talla5", SqlDbType.VarChar) { Value = asigOrdenDet.Tallas[4] });
            _sqlParam.Add(new SqlParameter("@talla6", SqlDbType.VarChar) { Value = asigOrdenDet.Tallas[5] });
            _sqlParam.Add(new SqlParameter("@talla7", SqlDbType.VarChar) { Value = asigOrdenDet.Tallas[6] });
            _sqlParam.Add(new SqlParameter("@talla8", SqlDbType.VarChar) { Value = asigOrdenDet.Tallas[7] });
            _sqlParam.Add(new SqlParameter("@talla9", SqlDbType.VarChar) { Value = asigOrdenDet.Tallas[8] });
            _sqlParam.Add(new SqlParameter("@cantidad1", SqlDbType.Int) { Value = asigOrdenDet.Cantidades[0] });
            _sqlParam.Add(new SqlParameter("@cantidad2", SqlDbType.Int) { Value = asigOrdenDet.Cantidades[1] });
            _sqlParam.Add(new SqlParameter("@cantidad3", SqlDbType.Int) { Value = asigOrdenDet.Cantidades[2] });
            _sqlParam.Add(new SqlParameter("@cantidad4", SqlDbType.Int) { Value = asigOrdenDet.Cantidades[3] });
            _sqlParam.Add(new SqlParameter("@cantidad5", SqlDbType.Int) { Value = asigOrdenDet.Cantidades[4] });
            _sqlParam.Add(new SqlParameter("@cantidad6", SqlDbType.Int) { Value = asigOrdenDet.Cantidades[5] });
            _sqlParam.Add(new SqlParameter("@cantidad7", SqlDbType.Int) { Value = asigOrdenDet.Cantidades[6] });
            _sqlParam.Add(new SqlParameter("@cantidad8", SqlDbType.Int) { Value = asigOrdenDet.Cantidades[7] });
            _sqlParam.Add(new SqlParameter("@cantidad9", SqlDbType.Int) { Value = asigOrdenDet.Cantidades[8] });
            _sqlParam.Add(new SqlParameter("@fechafinalizacion", SqlDbType.DateTime) { Value = asigOrdenDet.FechaFinalizacion });
            _sqlParam.Add(new SqlParameter("@horaingreso", SqlDbType.VarChar) { Value = asigOrdenDet.HoraIngreso });
            _sqlParam.Add(new SqlParameter("@usuario", SqlDbType.VarChar) { Value = asigOrdenDet.Usuario });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el Centro de Costos por Orden.
        /// </summary>
        /// <param name="orden">Orden de Producción</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectCentroCostosPorOrden(string orden) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                cc.codigo,
	                cc.descripcion
                from centros_costo_modelos ccm
                inner join centro_costos cc on cc.codigo = ccm.centro
                where ccm.Orden = @orden ";
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = orden });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
