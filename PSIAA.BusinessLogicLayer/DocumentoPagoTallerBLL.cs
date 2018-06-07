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
        private ProveedorDAL _provDal = new ProveedorDAL();
        private DocumentoPagoTallerDAL _docPagoTallerDal = new DocumentoPagoTallerDAL();
        private AsignacionOrdenesDAL _asigOrdenesDal = new AsignacionOrdenesDAL();
        private LiquidacionTallerBLL _liquidTallerBll = new LiquidacionTallerBLL();
        private OperacionModeloDAL _operacionModeloDal = new OperacionModeloDAL();

        public DataTable ListarProveedores()
        {
            return _provDal.SelectProveedores();
        }

        public string DevolverNombreProveedor(string codProveedor) {
            return _provDal.NombreProveedor(codProveedor);
        }

        /*public int NroDocumentoSugerido(string _codProv, int _serie, string _tipoMov) {
            string documento = _docPagoTallerDal.SelectUltimoNumeroDocumentoPagoTaller(_codProv, _serie, _tipoMov);
            return int.Parse(documento == "" ? "0" : documento) + 1;
        }*/

        /*public bool ExisteNumeroDocumento(string _codProv, int _serie, string _tipoMov, int _nroDocumento) {
            var numDocumento = _docPagoTallerDal.SelectNumeroDocumento(_codProv, _serie, _tipoMov, _nroDocumento);
            if (numDocumento == "")
                return false;
            else
                return true;
        }*/

        private int NroLiquidacion()
        {
            string id = _liquidTallerBll.UltimoNroControlLiquidacion();
            string periodo = DateTime.Now.ToString("yyyyMM");
            int _nroControl = int.Parse(id == "" ? "0" : id) + 1;
            return int.Parse((periodo + _nroControl.ToString()).ToString());
        }

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
