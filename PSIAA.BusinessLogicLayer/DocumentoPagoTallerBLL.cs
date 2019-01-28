using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataAccessLayer.TuartDB;
using PSIAA.DataTransferObject;

namespace PSIAA.BusinessLogicLayer
{
    public class DocumentoPagoTallerBLL
    {
        /// <summary>
        /// Variable de instancia a la clase ProveedorDAL.
        /// </summary>
        public ProveedorDAL _provDal = new ProveedorDAL();
        /// <summary>
        /// Variable de instancia a la clase DocumentoPagoTallerDAL.
        /// </summary>
        public DocumentoPagoTallerDAL _docPagoTallerDal = new DocumentoPagoTallerDAL();
        /// <summary>
        /// Variable de instancia a la clase AsignacionOrdenesDAL.
        /// </summary>
        public AsignacionOrdenesDAL _asigOrdenesDal = new AsignacionOrdenesDAL();
        /// <summary>
        /// Variable de instancia a la clase LiquidacionTallerBLL.
        /// </summary>
        public LiquidacionTallerBLL _liquidTallerBll = new LiquidacionTallerBLL();
        /// <summary>
        /// Variable de instancia a la clase OperacionModeloDAL.
        /// </summary>
        public OperacionModeloDAL _operacionModeloDal = new OperacionModeloDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de proveedores activos, y retorna el resultado.
        /// </summary>
        /// <returns>Contenedor de tipo DataTable con los datos de proveedores.</returns>
        public DataTable ListarProveedores()
        {
            return _provDal.SelectProveedores();
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Nombre de Proveedor por código de proveedor (RUC), y retoran el resultado.
        /// </summary>
        /// <param name="codProveedor">Código de Proveedor</param>
        /// <returns>Variable de tipo string con el nombre del proveedor.</returns>
        public string DevolverNombreProveedor(string codProveedor) {
            return _provDal.NombreProveedor(codProveedor);
        }

        private int NroLiquidacion()
        {
            string id = _liquidTallerBll.UltimoNroControlLiquidacion();
            string periodo = DateTime.Now.ToString("yyyyMM");
            int _nroControl = int.Parse(id == "" ? "0" : id) + 1;
            return int.Parse((periodo + _nroControl.ToString()).ToString());
        }

        /// <summary>
        /// Genera un nuevo número de liquidación y recorre el listado de pagos de taller para enviar cada elemento a un procedimiento
        /// DAL de Insert Documento Pago Taller y Actualizacion de Número de Asignacion de Ordenes.
        /// Construye un objeto de tipo LiquidacionTallerDTO totalizando los pagos de taller y lo envía al procedimiento BLL de
        /// Insert Liquidación Taller.
        /// </summary>
        /// <param name="_listDocPagoTaller">Lista Genérica de tipo DocumentoPagoTallerDTO, con los pagos de taller agregados.</param>
        /// <param name="_moneda">Moneda (S/D)</param>
        /// <param name="_usuario">Nombre de Usuario</param>
        /// <returns>Variable de tipo int con el número de liquidacion ingresado.</returns>
        public int IngresarDocumentoPagoTaller(List<DocumentoPagoTallerDTO> _listDocPagoTaller, string _moneda, string _usuario) {
            int nroInsertsDoc = 0;
            int nroUpdatesAsig = 0;
            int nroInsertLiquid = 0;

            string _tipoMovimiento = string.Empty;
            string _codProveedor = string.Empty;
            int _seriedoc = 0;
            int _nroLiquidacion = NroLiquidacion();
            int _operacion = 0;
            double _montoFacturacionSoles = 0;
            double _montoFacturacionDolares = 0;

            foreach (DocumentoPagoTallerDTO _doc in _listDocPagoTaller) {
                //Insertar Registros de Asignacion a Documentos
                _doc.NroDocumento = _nroLiquidacion;
                if (_docPagoTallerDal.InsertDocumentoPagoTaller(_doc) > 0)
                    nroInsertsDoc++;
                //Actualizar NumeroOrden2 en Tabla AsignacionOrdenesDet
                if (_asigOrdenesDal.UpdateNumeroOrden2AsignacionOrdenes(_doc) > 0)
                    nroUpdatesAsig++;

                if (_listDocPagoTaller.IndexOf(_doc) == 0) {
                    _codProveedor = _doc.CodProveedor;
                    _tipoMovimiento = _doc.TipoDocumento;
                    _operacion = _doc.CategoriaOperacion;
                }
                _montoFacturacionSoles += _doc.MontoFacturacionSoles;
                _montoFacturacionDolares += _doc.MontoFacturacionDolares;
            }
            //Ingresar Liquidacion
            LiquidacionTallerDTO _liquidTallerDto = new LiquidacionTallerDTO()
            {
                CodProveedor = _codProveedor,
                TipoMovimiento = _tipoMovimiento,
                SerieDocumento = _seriedoc,
                NroDocumento = _nroLiquidacion,
                ConceptoCompra = _operacion.ToString(),
                Moneda = _moneda,
                MontoFacturaSoles = _montoFacturacionSoles,
                MontoFacturaDolares = _montoFacturacionDolares,
                Usuario = _usuario
            };
            nroInsertLiquid = _liquidTallerBll.IngresarLiquidacionTaller(_liquidTallerDto);
            if (nroInsertsDoc > 0 && nroUpdatesAsig > 0 && nroInsertLiquid > 0)
            {
                return _nroLiquidacion;
            }
            else
                return 0;
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Procesos Asignados por Orden de Producción SIAA y Operaciones TACITA, con ambos resutado
        /// se realiza un procedimiento de matching y el resultante es convertido y almacenado en un contenedor.
        /// </summary>
        /// <param name="codProveedor">Código de Proveedor</param>
        /// <param name="nroAsignacion">Número de Asignación</param>
        /// <param name="orden">Orden de Producción</param>
        /// <param name="lote">Número de Lote</param>
        /// <param name="categoria">Código de Categoria</param>
        /// <param name="modelo">Modelo de Prenda</param>
        /// <returns>Contenedor de tipo DataTable con el resultante del matching.</returns>
        public DataTable ListarProcesosAsignadosPorOrdenLote(string codProveedor, string nroAsignacion, string orden,
                                                            int lote, int categoria, string modelo) {
            DataTable dtProcesosSia = _asigOrdenesDal.SelectProcesosAsignacionPorOrdenLote(codProveedor, nroAsignacion, orden,
                                                                                           lote, categoria);
            DataTable dtProcesosTiempos = _operacionModeloDal.SelectOperacionesTiempo(modelo);

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("Proceso", typeof(string));
            dtResult.Columns.Add("Moneda", typeof(string));
            dtResult.Columns.Add("Tiempo", typeof(double));
            dtResult.Columns.Add("TarifaSoles", typeof(double));
            dtResult.Columns.Add("CostoSoles", typeof(double));
            dtResult.Columns.Add("TarifaDolares", typeof(double));
            dtResult.Columns.Add("CostoDolares", typeof(double));

            var list = from procesos in dtProcesosSia.AsEnumerable()
                        join tiempos in dtProcesosTiempos.AsEnumerable()
                            on procesos.Field<int>("Proceso")
                            equals tiempos.Field<long>("i_idope")
                        orderby tiempos.Field<long>("i_numord")
                        select dtResult.LoadDataRow(new object[]
                        {
                            tiempos.Field<string>("descripcion"),
                            procesos.Field<string>("Moneda"),
                            double.Parse(procesos.Field<float>("Tiempo").ToString()),
                            double.Parse(procesos.Field<double>("TarifaSoles").ToString()),
                            double.Parse(procesos.Field<double>("Costo_Soles").ToString()),
                            double.Parse(procesos.Field<double>("Tarifa_Dolares").ToString()),
                            double.Parse(procesos.Field<double>("Costo_Dolares").ToString())
                        }, false);
            dtResult =  list.CopyToDataTable();
            return dtResult;
        }
    }
}
