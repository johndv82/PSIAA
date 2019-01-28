using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataTransferObject;
using System.Data;

namespace PSIAA.BusinessLogicLayer
{
    public class DocumentoPagoLibreBLL
    {
        /// <summary>
        /// Variable de instancia a la clase DocumentoPagoLibreDAL.
        /// </summary>
        public DocumentoPagoLibreDAL _docPagoLibreDal = new DocumentoPagoLibreDAL();
        /// <summary>
        /// Variable de instancia a la clase LiquidacionTallerBLL.
        /// </summary>
        public LiquidacionTallerBLL _liquidTallerBll = new LiquidacionTallerBLL();
        /// <summary>
        /// Variable de instancia a la clase RecepcionControlDAL.
        /// </summary>
        public RecepcionControlDAL _recepcionControlDal = new RecepcionControlDAL();
        /// <summary>
        /// Variable de instancia a la clase ProveedorDAL.
        /// </summary>
        public ProveedorDAL _provDal = new ProveedorDAL();

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
        public string DevolverNombreProveedor(string codProveedor)
        {
            return _provDal.NombreProveedor(codProveedor);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Operaciones Libres, y con el resultdo genéra un diccionario para almacenar
        /// la denominación de la operación y su código.
        /// </summary>
        /// <returns>Diccionario de datos de tipo clave/valor, ambos en formato string con el código y las operaciones.</returns>
        public Dictionary<string, string> ListarOperacionesLibres()
        {
            DataTable dtOperLibres = _docPagoLibreDal.SelectOperacionesLibres();
            Dictionary<string, string> listOperLibres = new Dictionary<string, string>();
            foreach (DataRow dr in dtOperLibres.Rows)
            {
                listOperLibres.Add(dr["Cod_operacion"].ToString(), dr["Denominacion"].ToString());
            }
            return listOperLibres;
        }

        private int NroLiquidacion() {
            string id = _liquidTallerBll.UltimoNroControlLiquidacion();
            string periodo = DateTime.Now.ToString("yyyyMM");
            int _nroControl = int.Parse(id == "" ? "0" : id) + 1;
            return int.Parse((periodo + _nroControl.ToString()).ToString());
        }

        /// <summary>
        /// Completa aulgunos datos faltantes del listado de Pagos Libres y se totaliza lo facturado ya sea en soles o dolares,
        /// para la ejecución del metodo DAL de Insert Documento Pago Libre.
        /// Construye un objeto de tipo LiquidacionTallerDTO y se completa con algunos valores faltantes como el monto facturado
        /// obtenido desde lo totalizado de Pagos Libres, para poder ejecutar el metodo BLL de Ingresasar Liquidación Taller.
        /// </summary>
        /// <param name="listDocLibre">Lista genérica de tipo DocumentoPagoLibreDTO, con los pagos libres agregados</param>
        /// <param name="_codProveedor">Código de Proveedor</param>
        /// <param name="_tipoMov">Tipo de Movimiento</param>
        /// <param name="_moneda">Moneda (S/D)</param>
        /// <param name="_usuario">Nombre de Usuario</param>
        /// <returns>Variable de tipo int con el número de liquidación generado e ingresado.</returns>
        public int GuardarDocumentoPagoLibre(List<DocumentoPagoLibreDTO> listDocLibre, string _codProveedor, string _tipoMov, string _moneda, string _usuario) {
            int nroInsertLiquid;
            int nroInsertDocLibre = 0;
            //Campos para Liquidacion
            double _montoFacturacionSoles = 0;
            double _montoFacturacionDolares = 0;
            int _nroLiquidacion = NroLiquidacion();

            foreach (DocumentoPagoLibreDTO docLibre in listDocLibre) {
                docLibre.TipoMovimiento = _tipoMov;
                docLibre.Moneda = _moneda;
                docLibre.NroDocumento = _nroLiquidacion;
                if (_moneda == "S")
                    _montoFacturacionSoles += docLibre.Total;
                else if (_moneda == "D")
                    _montoFacturacionDolares += docLibre.Total;
                else {
                    _montoFacturacionSoles += 0;
                    _montoFacturacionDolares += 0;
                }

                _docPagoLibreDal.InsertDocumentoPagoLibre(docLibre);
                nroInsertDocLibre++;
            }

            //Ingresar Liquidacion
            LiquidacionTallerDTO _liquidTallerDto = new LiquidacionTallerDTO()
            {
                CodProveedor = _codProveedor,
                TipoMovimiento = _tipoMov,
                SerieDocumento = 0,
                NroDocumento = _nroLiquidacion,
                ConceptoCompra = string.Empty,
                Moneda = _moneda,
                MontoFacturaSoles = _montoFacturacionSoles,
                MontoFacturaDolares = _montoFacturacionDolares,
                Usuario = _usuario
            };
            nroInsertLiquid = _liquidTallerBll.IngresarLiquidacionTaller(_liquidTallerDto);
            if ((nroInsertLiquid > 0) && (nroInsertDocLibre > 0))
                return _nroLiquidacion;
            else
                return 0;
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Existencia de Orden y Lote, y el resultado lo compara con la orden recibida.
        /// </summary>
        /// <param name="orden">Orden de Producción</param>
        /// <param name="lote">Número de Lote</param>
        /// <returns>Variable booleano con la comparación (verdadero/falso)</returns>
        public bool ExisteOrdenLoteRecepcion(string orden, int lote) {
            return (_recepcionControlDal.SelectExistenciaOrdenLote(orden, lote).Trim() == orden);
        }

        /// <summary>
        /// Genera un listado de años desde el 2015 hasta la actualidad.
        /// </summary>
        /// <returns>Lista genérica de tipo int con los años generados.</returns>
        public List<int> ListarYears()
        {
            List<int> years = new List<int>();
            int yearNow = DateTime.Now.Year;
            for (int y = 2015; y <= yearNow; y++)
                years.Add(y);
            return years.OrderByDescending(x => x).ToList();
        }

        /// <summary>
        /// Genera un diccionario listando todos los nombres de meses con su respectivo número de mes, de un determinado año.
        /// </summary>
        /// <param name="year">Año</param>
        /// <returns>Diccionario de tipo clave/valor en formato string con los valores de los meses.</returns>
        public Dictionary<string, string> ListarMeses(int year) {
            Dictionary<string, string> meses = new Dictionary<string, string>();
            int yearNow = DateTime.Now.Year;
            int mesNow = (yearNow == year) ? DateTime.Now.Month : 12;
            for (int m = 1; m <= mesNow; m++)
            {
                DateTime dateMes = new DateTime(year, m, 1);
                meses.Add(Helper.Mascara(m, "00"), dateMes.ToString("MMMM"));
            }
            return meses;
        }
    }
}
