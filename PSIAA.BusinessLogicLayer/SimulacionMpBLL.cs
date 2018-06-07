using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataAccessLayer.TuartDB;
using PSIAA.DataTransferObject;
using System.Data;
using System.Reflection;

namespace PSIAA.BusinessLogicLayer
{
    public class SimulacionMpBLL
    {
        private SimulacionMpDAL _simulacionDal = new SimulacionMpDAL();
        private ContratoBLL _contratoBll = new ContratoBLL();
        private LanzamientoBLL _lanzamientoBll = new LanzamientoBLL();
        private MaquinaBLL _maquinaBll = new MaquinaBLL();
        private CombMaterialDAL _combMaterial = new CombMaterialDAL();
        private HojaCombinacionesBLL _hojaCombinacionBll = new HojaCombinacionesBLL();

        public List<SimulacionDetDTO> ListarSimulacionMpDetalle(int contrato, List<string> modelos)
        {
            DataTable dtSimulacionDetalle = _simulacionDal.SelectSimulacionDetalleAlternativo(contrato, modelos);
            var _listSimulacion = new List<SimulacionDetDTO>();
            foreach (DataRow fila in dtSimulacionDetalle.Rows)
            {
                var SimulacionDTO = new SimulacionDetDTO()
                {
                    NumContrato = int.Parse(fila["nroContrato"].ToString()),
                    Correlativo = int.Parse(fila["Correlativo"].ToString()),
                    NroSimulacion = fila["codSimulacion"].ToString(),
                    NombreMaquina = fila["Maquina"].ToString(),
                    Color = fila["colorBase"].ToString(),
                    CodProducto = fila["codProducto"].ToString(),
                    DescProducto = fila["Material"].ToString(),
                    Modelo = fila["Modelo"].ToString(),
                    KilosSimulado = decimal.Parse(fila["kilosSimulado"].ToString()),
                    PorSeguridad = decimal.Parse(fila["porAdicional"].ToString()),
                    FechaIngreso = DateTime.Parse(fila["fechaIngreso"].ToString()),
                    HoraIngreso = fila["horaIngreso"].ToString(),
                    Usuario = fila["Usuario"].ToString()
                };
                _listSimulacion.Add(SimulacionDTO);
            }
            return _listSimulacion;
        }

        public List<SimulacionDetDTO> ListarCalculoMateriaPrima(int contrato, string usuario, List<string> modelos, decimal porAdicional)
        {
            List<SimulacionDetDTO> listSimulacionCalculo = new List<SimulacionDetDTO>();
            List<ContratoDetalleDTO> _listContratoDet = _contratoBll.ListarDetalleContrato(contrato, true);
            //Filtrar contrato por modelos seleccionados
            _listContratoDet = _listContratoDet.Where(x => modelos.Contains(x.ModeloAA.Trim())).ToList();

            List<MaquinaDTO> _listMaquinas = _maquinaBll.ListarMaquinas();
            int item = 0;
            string modeloAnterior = string.Empty;

            //Ordenar por Modelo
            _listContratoDet = _listContratoDet.OrderBy(x => x.ModeloAA).ToList();

            //Correlativo de Simulacion
            int _correlativo = int.Parse(_simulacionDal.CorrelativoSimulacion(contrato)) + 1;

            foreach (var contratoDet in _listContratoDet)
            {
                if (_listContratoDet.IndexOf(contratoDet) == 0)
                {
                    modeloAnterior = contratoDet.ModeloAA;
                    item = 1;
                }
                if (modeloAnterior != contratoDet.ModeloAA)
                {
                    item = item + 1;
                    modeloAnterior = contratoDet.ModeloAA;
                }
                decimal kilos = _lanzamientoBll.CalcularKilosNecesarios(contratoDet, contratoDet.Cantidades);
                string maquina = _listMaquinas.Find(x => x.Codigo == contratoDet.Linea.Trim()).Abreviacion;
                string nombreMaquina = _listMaquinas.Find(x => x.Codigo == contratoDet.Linea.Trim()).Nombre;

                if (contratoDet.CodColor.Trim().Substring(0, 2) == "C0")
                {
                    List<string[]> productos = ListarProductosCombo(contratoDet.ModeloAA.Trim(), contratoDet.CodColor.Trim());
                    foreach (string[] prod in productos) {
                        var simulacionDetDesplegado = new SimulacionDetDTO()
                        {
                            NumContrato = contratoDet.Numero,
                            Correlativo = _correlativo,
                            NroSimulacion = contratoDet.Numero.ToString() + maquina + ((char)(item + 64)).ToString(),
                            NombreMaquina = nombreMaquina,
                            Color = contratoDet.CodColor,
                            CodProducto = prod[0],
                            DescProducto = prod[1],
                            Modelo = contratoDet.ModeloAA.Trim(),
                            KilosSimulado = Math.Round(kilos * (decimal.Parse(prod[2]) / 100), 3),
                            PorSeguridad = porAdicional,
                            Usuario = usuario,
                            FechaIngreso = DateTime.Now,
                            HoraIngreso = string.Concat(DateTime.Now.ToString("HH:mm:ss"))
                        };
                        listSimulacionCalculo.Add(simulacionDetDesplegado);
                    }
                }
                else {
                    string[] prodMaterialUni = ProductoColorUnitario(contratoDet.ModeloAA.Trim());
                    if (prodMaterialUni.Length > 1) {
                        var simulacionDet = new SimulacionDetDTO()
                        {
                            NumContrato = contratoDet.Numero,
                            Correlativo = _correlativo,
                            NroSimulacion = contratoDet.Numero.ToString() + maquina + ((char)(item + 64)).ToString(),
                            NombreMaquina = nombreMaquina,
                            Color = contratoDet.CodColor.Trim(),
                            CodProducto = prodMaterialUni[0] + contratoDet.CodColor.Trim(),
                            DescProducto = prodMaterialUni[1],
                            Modelo = contratoDet.ModeloAA.Trim(),
                            KilosSimulado = kilos,
                            PorSeguridad = porAdicional,
                            Usuario = usuario,
                            FechaIngreso = DateTime.Now,
                            HoraIngreso = string.Concat(DateTime.Now.ToString("HH:mm:ss"))
                        };
                        listSimulacionCalculo.Add(simulacionDet);
                    }
                }
            }
            //DataTable dt = Helper.ToDataTable(listSimulacionCalculo);
            return listSimulacionCalculo.OrderBy(x=>x.CodProducto).ToList();
        }

        public List<string[]> ListarProductosCombo(string modelo, string combo) {
            List<string[]> codProductos = new List<string[]>();
            string correlativoCol = combo.Substring(1, combo.Length - 1);

            List<CombinacionDTO> listCombinacion = _hojaCombinacionBll.ListarColoresCombinacion(modelo, correlativoCol, 0);
            foreach (CombinacionDTO comb in listCombinacion) {
                codProductos.Add(new string[] { comb.Producto, comb.DescripcionMaterial, comb.Porcentaje.ToString() });
            }
            return codProductos;
        }

        public string[] ProductoColorUnitario(string modelo) {
            string[] partes = modelo.Split('-');
            string _familia = partes[0].Substring(0, 1);
            int _correlativo = int.Parse(partes[0].Substring(1, partes[0].Length - 1));
            DataTable dtProducto = _hojaCombinacionBll.MaterialPorColor(_familia, _correlativo);
            DataTable dtMaterial = _combMaterial.SelectCombinacionMaterial(modelo);
            string[] prodMaterial;

            var prodMaterialJoin = (from prod in dtProducto.AsEnumerable()
                                join mat in dtMaterial.AsEnumerable()
                                on prod.Field<string>("Materiales").ToString().Trim()
                                equals mat.Field<string>("c_codmat")
                                into outer
                                from mat in outer
                                       select new
                                       {
                                           Material = prod.Field<string>("Materiales"),
                                           Titulo = prod.Field<string>("Titulos"),
                                           DescripcionMat = mat.Field<string>("c_denmat"),
                                           TipoMat = mat.Field<string>("tipo")
                                       }).ToList();
            if ((prodMaterialJoin.Count > 0) && (prodMaterialJoin.Count == dtProducto.Rows.Count))
            {
                prodMaterial = new string[2];
                foreach (var item in prodMaterialJoin)
                {
                    prodMaterial = new string[] { item.TipoMat + "-" + item.Material.Trim() + "-HC-" + item.Titulo.Trim() + "-", item.DescripcionMat };
                    break;
                }
            }
            else
                prodMaterial = new string[0];
            return prodMaterial;
        }

        public int IngresarSimulacionCalculo(List<SimulacionDetDTO> listSumCal, int contrato, string usuario) {
            int ingresos = 0;
            int _correlativo = int.Parse(_simulacionDal.CorrelativoSimulacion(contrato)) + 1;

            foreach (var simDet in listSumCal) {
                ingresos = ingresos + _simulacionDal.InsertSimulacionDetalleAlternativo(simDet);
            }
            //Ingresar Cabecera de Simulacion
            ingresos = ingresos + _simulacionDal.InsertSimulacionCabecera(contrato, _correlativo, DateTime.Now, usuario);
            return ingresos;
        }
    }
}
