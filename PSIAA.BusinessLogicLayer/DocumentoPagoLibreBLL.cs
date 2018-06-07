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
        private DocumentoPagoLibreDAL _docPagoLibreDal = new DocumentoPagoLibreDAL();
        private LiquidacionTallerBLL _liquidTallerBll = new LiquidacionTallerBLL();
        private RecepcionControlDAL _recepcionControlDal = new RecepcionControlDAL();
        private ProveedorDAL _provDal = new ProveedorDAL();

        public DataTable ListarProveedores()
        {
            return _provDal.SelectProveedores();
        }

        public string DevolverNombreProveedor(string codProveedor)
        {
            return _provDal.NombreProveedor(codProveedor);
        }

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

        public bool ExisteOrdenLoteRecepcion(string orden, int lote) {
            return (_recepcionControlDal.SelectExistenciaOrdenLote(orden, lote).Trim() == orden);
        }

        public List<int> ListarYears()
        {
            List<int> years = new List<int>();
            int yearNow = DateTime.Now.Year;
            for (int y = 2015; y <= yearNow; y++)
                years.Add(y);
            return years.OrderByDescending(x => x).ToList();
        }

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
