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
        /// <summary>
        /// Variable de instancia a la clase SimulacionMpDAL.
        /// </summary>
        public SimulacionMpDAL _simulacionDal = new SimulacionMpDAL();
        /// <summary>
        /// Variable de instancia a la clase ContratoBLL.
        /// </summary>
        public ContratoBLL _contratoBll = new ContratoBLL();
        /// <summary>
        /// Variable de instancia a la clase LanzamientoBLL.
        /// </summary>
        public LanzamientoBLL _lanzamientoBll = new LanzamientoBLL();
        /// <summary>
        /// Variable de instancia a la clase MaquinaBLL.
        /// </summary>
        public MaquinaBLL _maquinaBll = new MaquinaBLL();
        /// <summary>
        /// Variable de instancia a la clase CombMaterialDAL.
        /// </summary>
        public CombMaterialDAL _combMaterialDal = new CombMaterialDAL();
        /// <summary>
        /// Variable de instancia a la clase HojaCombinacionesBLL.
        /// </summary>
        public HojaCombinacionesBLL _hojaCombinacionBll = new HojaCombinacionesBLL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Simulación Detalle, y el resultado lo recorre para acceder a sus datos y construir
        /// una lista de tipo SimulacionDetDTO.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="modelos">Lista genérica (string) de modelos de prenda</param>
        /// <returns>Lista genérica de tipo SimulacionDetDTO con el detalle de la simulación.</returns>
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

        /// <summary>
        /// Ejecuta un procedimiento BLL de Detalle de Contrato agrupado para poder calcular los kilos llamando a otro 
        /// procedimiento BLL (Lanzamiento) de Cálculo de kilos por Contrato.
        /// Recorre el Detalle del contrato para evaluar si el color tiene combinación o es color entero, para ambos,
        /// se ejecuta segun sea el caso los procedimientos DAL de Productos por Combinacion y Productos por Color Unitario.
        /// Con ambos resultados de producto se crea un objeto de tipo SimulacionDetDTO, adjuntando datos de máquina de producción
        /// y kilos calculados, y se procede a listarlos. 
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="usuario">Nombre de usuario</param>
        /// <param name="modelos">Lista genércia (string) de modelos de prenda</param>
        /// <param name="porAdicional">Porcentaje Adicional al cálculo</param>
        /// <returns>Lista genérica de tipo SimulacionDetDTO con el Cálculo de Materia Prima.</returns>
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
                decimal kilos = _lanzamientoBll.CalcularKilosPorContrato(contratoDet);
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

        /// <summary>
        /// Ejecuta un procedimiento BLL de Detalles de Combinación por Modelo/Color, el resultado se almacenda en un arreglo
        /// con el siguiente orden, por cada combinación:
        /// (0, Nombre del Producto)
        /// (1, Descripción del Material)
        /// (2, Porcentaje de Color)
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <param name="combo">Combinación de Modelo</param>
        /// <returns>Lista genérica de tipo string[] con el detalle de la combinación.</returns>
        public List<string[]> ListarProductosCombo(string modelo, string combo) {
            List<string[]> codProductos = new List<string[]>();
            string correlativoCol = combo.Substring(1, combo.Length - 1);

            List<CombinacionDTO> listCombinacion = _hojaCombinacionBll.ListarColoresCombinacion(modelo, correlativoCol, 0);
            foreach (CombinacionDTO comb in listCombinacion) {
                codProductos.Add(new string[] { comb.Producto, comb.DescripcionMaterial, comb.Porcentaje.ToString() });
            }
            return codProductos;
        }

        /// <summary>
        /// Ejecuta procedimientos DAL de Materiales por Color, y Descripción de Material por Color, con ambos resultados
        /// se hace un proceso de matching para juntar todos su valores en un arreglo con la siguiente estructura:
        /// (0, Tipo de Material + Nombres del Material + HC + Titulo de Material)
        /// (1, Descripción del Material)
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Arreglo de tipo string con el detalle del producto.</returns>
        public string[] ProductoColorUnitario(string modelo) {
            string[] partes = modelo.Split('-');
            string _familia = partes[0].Substring(0, 1);
            int _correlativo = int.Parse(partes[0].Substring(1, partes[0].Length - 1));
            DataTable dtProducto = _hojaCombinacionBll.MaterialPorColor(_familia, _correlativo);
            DataTable dtMaterial = _combMaterialDal.SelectCombinacionMaterial(modelo);
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

        /// <summary>
        /// Genera el nuevo correlativo de simulación para enviarlo al procedimiento DAL de Insert Simulación Cabecera.
        /// Recorre el Detalle de Simulación para ejecutar por cada item el procedimiento DAL de Insert Simulación Detalle.
        /// Todos los ingresos son almacenados en un contador.
        /// </summary>
        /// <param name="listSumCal">Lista genérica de tipo SimulacionDetDTO con el Detalle Simulado</param>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="usuario">Nombre de Usuario</param>
        /// <returns>Variable de tipo int con la cantidad de ingresos.</returns>
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
