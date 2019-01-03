using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.TuartDB;
using PSIAA.DataAccessLayer;
using PSIAA.DataTransferObject;
using System.Data;
using System.Reflection;

namespace PSIAA.BusinessLogicLayer
{
    public class LanzamientoBLL
    {
        private LanzamientoDAL _lanzamientoDal = new LanzamientoDAL();
        private MedidaPorTallaDAL _medidaPorTallaDal = new MedidaPorTallaDAL();
        private PesosDAL _pesoDal = new PesosDAL();
        private MaquinaBLL _maquinaBll = new MaquinaBLL();
        private CategoriaOperacionDAL _categoriaOperDal = new CategoriaOperacionDAL();
        private AsignacionOrdenesDAL _asignacionOrdenesDal = new AsignacionOrdenesDAL();

        public int[] ListarCantidadesLanzadas(int _contrato, string _modelo, string _color)
        {
            int[] cantidades = new int[9];
            DataTable _dtresult = _lanzamientoDal.SelectCantidadesLanzadas(_contrato, _modelo, _color);
            for (int x = 0; x < _dtresult.Columns.Count; x++)
            {
                cantidades[x] = int.Parse(_dtresult.Rows[0][x] == DBNull.Value ? "0" : _dtresult.Rows[0][x].ToString());
            }
            return cantidades;
        }

        public string CorrelativoAlfabeticoPorModelo(List<ContratoDetalleDTO> modelosAgrupados, string modelo)
        {
            int caracterIni = 65;
            Dictionary<string, char> Modelos = new Dictionary<string, char>();
            modelosAgrupados.OrderBy(x => x.ModeloAA);
            foreach (ContratoDetalleDTO mod in modelosAgrupados)
            {
                Modelos.Add(mod.ModeloAA.Trim(), (char)caracterIni);
                caracterIni++;
            }
            char caracterFinal;
            Modelos.TryGetValue(modelo, out caracterFinal);
            return caracterFinal.ToString();
        }

        public List<LanzamientoDetDTO> ListarPreLanzamiento(List<AlanzarDTO> _listAlanzar, string _usuario)
        {
            //TEST PESOS POR PIEZA
            //decimal[] result = PesosPorPieza("L", 10, "S416-105", 3);

            List<MaquinaDTO> _listMaquinas = _maquinaBll.ListarMaquinas();
            List<LanzamientoDetDTO> _listLanzamiento = new List<LanzamientoDetDTO>();

            string _modeloAnterior = "";
            int correlativoOrden = 0;
            int nroLanz = _lanzamientoDal.SelectUltimoNroLanzamiento(_listAlanzar[0].Contrato);
            nroLanz = nroLanz == 0 ? 1 : nroLanz + 1;

            _listAlanzar = _listAlanzar.OrderBy(c => c.Modelo).ToList();

            foreach (AlanzarDTO _alanzar in _listAlanzar)
            {
                if (_modeloAnterior != _alanzar.Modelo)
                {
                    correlativoOrden = _lanzamientoDal.SelectUtimoCorrelativoOrden(_alanzar.Contrato, _alanzar.Modelo) + 1;
                    _modeloAnterior = _alanzar.Modelo;
                }
                else
                {
                    _modeloAnterior = _alanzar.Modelo;
                }

                //VARIABLES
                int contrato = _alanzar.Contrato;
                string maquina = _listMaquinas.Find(x => x.Codigo == _alanzar.Linea.Trim()).Abreviacion;
                int capacidad = _listMaquinas.Find(x => x.Abreviacion == maquina).Capacidad;
                int limite = _listMaquinas.Find(x => x.Abreviacion == maquina).Limite;
                char correlativoAlfa = _alanzar.CorrelativoModelo;

                for (int c = 0; c < _alanzar.Cantidades.Length; c++)
                {
                    //int resto = 111 % 10;
                    if (_alanzar.Cantidades[c] == 0) continue;

                    int resto, cantidadFilasEnteras;

                    if (_alanzar.Cantidades[c] > limite)
                    {
                        resto = _alanzar.Cantidades[c] % limite;
                        cantidadFilasEnteras = _alanzar.Cantidades[c] / limite;
                    }
                    else
                    {
                        resto = 0;
                        cantidadFilasEnteras = 1;
                    }

                    int valorAIngresar = 0;
                    int limiteAlmacenado = 0;
                    int lote = 1;
                    if (_alanzar.Cantidades[c] < limite)
                    {
                        valorAIngresar = _alanzar.Cantidades[c];
                    }
                    else
                    {
                        valorAIngresar = limite;
                    }

                    for (int l = 1; l <= cantidadFilasEnteras; l++)
                    {
                        int[] _nuevasCantidades = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        _nuevasCantidades[c] = valorAIngresar;
                        limiteAlmacenado += valorAIngresar;

                        if (limiteAlmacenado > capacidad)
                        {
                            correlativoOrden++;
                            lote = 1;
                            limiteAlmacenado = valorAIngresar;
                        }

                        LanzamientoDetDTO lanz = new LanzamientoDetDTO();
                        decimal[] _kilosCalculados = PesosPorPieza(_alanzar.Tallas[PosicionEnteroArray(_nuevasCantidades)],
                                                _nuevasCantidades.Sum(),
                                                _alanzar.Modelo,
                                                PosicionEnteroArray(_nuevasCantidades));
                        lanz.NumDocumento = contrato;
                        lanz.NumLanzamiento = (short)nroLanz;
                        lanz.Orden = contrato.ToString().Substring(contrato.ToString().Length - 4, 4) +
                                maquina + correlativoAlfa + Helper.Mascara(correlativoOrden, "00");
                        lanz.Lote = (short)lote;
                        lanz.Maquina = maquina;
                        lanz.Color = _alanzar.Color;
                        lanz.Modelo = _alanzar.Modelo;
                        lanz.CodProducto = _alanzar.Material;
                        lanz.Tallas = _alanzar.Tallas;
                        lanz.Piezas = _nuevasCantidades;
                        lanz.Kilos = _kilosCalculados;
                        lanz.Usuario = _usuario;
                        lanz.KilosTot = _kilosCalculados.Sum();
                        lanz.FechaSolicitud = DateTime.Now;

                        _listLanzamiento.Add(lanz);
                        lote++;

                        if ((resto != 0) && (l == cantidadFilasEnteras))
                        {
                            cantidadFilasEnteras++;
                            valorAIngresar = resto;
                            resto = 0;
                            continue;
                        }
                        else
                        {
                            valorAIngresar = limite;
                        }
                    }
                    correlativoOrden++;
                }
                nroLanz++;
            }
            return _listLanzamiento;
        }

        private int PosicionEnteroArray(int[] arreglo)
        {
            int posicion = 0;
            for (int i = 0; i < arreglo.Length; i++)
            {
                if (!arreglo[i].Equals(0))
                {
                    posicion = i;
                    break;
                }
            }
            return posicion;
        }

        private decimal[] PesosPorPieza(string _talla, int _cantidad, string _modelo, int _posicion)
        {
            decimal[] _pesos = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            decimal _totalArea;
            decimal _totalAreaMuestra;

            CalcularTotalArea(_talla, _modelo, out _totalArea);

            DataRow drMuestraPeso = _pesoDal.SelectMuestraPeso(_modelo.Trim());
            if (drMuestraPeso != null) {
                string _muestraTalla = drMuestraPeso["c_tal"].ToString();
                decimal _muestraPeso = decimal.Parse(drMuestraPeso["n_pestej"].ToString());
                decimal _pesoFinal = 0;

                if (_muestraTalla.Equals(_talla))
                {
                    _pesoFinal = _muestraPeso;
                }
                else
                {
                    CalcularTotalArea(_muestraTalla, _modelo, out _totalAreaMuestra);
                    if (_totalAreaMuestra > 0)
                    {
                        _pesoFinal = (_totalArea * _muestraPeso) / _totalAreaMuestra;
                    }
                    else
                    {
                        _pesoFinal = 0;
                    }
                }
                _pesos[_posicion] = Math.Round(Math.Round(_pesoFinal, 4) * _cantidad, 3);
            }
            return _pesos;
        }

        private void CalcularTotalArea(string _talla, string _modelo, out decimal totalArea)
        {
            DataTable _dtResult = _medidaPorTallaDal.SelectMedidasPorTalla(_modelo.Trim(), _talla.Trim());
            Dictionary<string, decimal> medidas = new Dictionary<string, decimal>();

            foreach (DataRow dr in _dtResult.Rows)
            {
                string medida = dr["Medida"].ToString().Replace(' ', '+');
                try
                {
                    medidas.Add(dr["CodMedida"].ToString(), decimal.Parse(new DataTable().Compute(medida == "" ? "0" : medida, "").ToString()));
                }
                catch (Exception)
                {
                    medidas.Clear();
                    break;
                }
            }
            if (medidas.Count > 0)
            {
                //PRIORIDADES
                string[] cuerpo1 = new string[] { "C", "T", "C^", "" };
                string[] cuerpo2 = new string[] { "O", "O DEL", "T", "A" };
                string[] manga1 = new string[] { "E", "COMB-E", "", "" };
                string[] manga2 = new string[] { "F", "K", "", "" };
                //string[] manga3 = new string[] { "S", "", "", ""};
                //string[] cuello1 = new string[] { "L", "", "" ,""};
                string[] cuello2 = new string[] { "H", "Hint", "", "" };

                //Variables de calculo
                decimal valorCuerpo1 = 1, valorCuerpo2 = 1, valorManga1 = 1, valorManga2 = 1,
                        valorManga3 = 1, valorCuello1 = 1, valorCuello2 = 1;

                for (int p = 0; p < 4; p++)
                {
                    if (medidas.ContainsKey(cuerpo1[p]))
                    {
                        valorCuerpo1 = medidas[cuerpo1[p]];
                        break;
                    }
                }
                for (int p = 0; p < 4; p++)
                {
                    if (medidas.ContainsKey(cuerpo2[p]))
                    {
                        valorCuerpo2 = medidas[cuerpo2[p]];
                        break;
                    }
                }
                //SOLO PARA LA MEDIDA DE LA MANGA 1
                if (medidas.ContainsKey(manga1[0]))
                {
                    valorManga1 = medidas[manga1[0]];
                }
                else
                {
                    //EN CASO DE QUE NO HAYA MEDIDA E
                    decimal Ev = medidas.ContainsKey("EV") ? medidas["EV"] : 1;
                    decimal A = medidas.ContainsKey("A") ? medidas["A"] : 1;
                    decimal H = medidas.ContainsKey("H") ? medidas["H"] : 1;
                    valorManga1 = A != 1 ? (Ev - (A / 2)) : (Ev - (H / 2));
                }
                for (int p = 0; p < 4; p++)
                {
                    if (medidas.ContainsKey(manga2[p]))
                    {
                        valorManga2 = medidas[manga2[p]];
                        break;
                    }
                }
                //Manga3 y Cuello1
                valorManga3 = medidas.ContainsKey("S") ? medidas["S"] : 1;
                valorCuello1 = medidas.ContainsKey("L") ? medidas["L"] : 1;

                for (int p = 0; p < 4; p++)
                {
                    if (medidas.ContainsKey(cuello2[p]))
                    {
                        valorCuello2 = medidas[cuello2[p]];
                        break;
                    }
                }

                decimal areaCuerpo = valorCuerpo1 * valorCuerpo2 * 2;
                //decimal areaManga = valorManga1 * valorManga2 * 4;
                decimal areaManga = (valorManga2 + valorManga3) * (valorManga1 * 2);
                decimal areaCuello = valorCuello1 * valorCuello2;

                totalArea = areaCuerpo + areaManga + areaCuello;
            }
            else
            {
                totalArea = 0;
            }
        }

        //Metodo usado para calculo de materia prima(Abastecimiento)
        public decimal CalcularKilosNecesarios(ContratoDetalleDTO _contratoDetalle, int[] cantAlanzar)
        {
            decimal kilosNecesarios = 0;
            string modelo = _contratoDetalle.ModeloAA;
            for (int x = 0; x < cantAlanzar.Length; x++)
            {
                string talla = _contratoDetalle.Tallas[x];
                int cantidad = cantAlanzar[x];
                if (cantidad != 0)
                {
                    kilosNecesarios += PesosPorPieza(talla, cantidad, modelo, 0).Sum();
                }
            }
            return kilosNecesarios;
        }

        //Metodo usado para calcular el peso base(1 unidad) para Lanzamiento(Planificacion)
        public Dictionary<string, decimal> CalcularPesosBase(ContratoDetalleDTO _contratoDetalle)
        {
            Dictionary<string, decimal> dicKilos = new Dictionary<string, decimal>();
            string modelo = _contratoDetalle.ModeloAA;
            for (int x = 0; x < 9; x++)
            {
                string talla = _contratoDetalle.Tallas[x];
                if (talla != "")
                {
                    dicKilos.Add(talla, PesosPorPieza(talla, 1, modelo, 0).Sum());
                }
            }
            return dicKilos;
        }

        public Dictionary<int, string> ListarCategoriasOperaciones()
        {
            Dictionary<int, string> dicCategoriaOper = new Dictionary<int, string>();
            DataTable dtResult = _categoriaOperDal.SelectCategoriaOperacion();
            dicCategoriaOper.Add(0, "<---- SELECCIONAR ---->");
            foreach (DataRow dr in dtResult.Rows)
            {
                dicCategoriaOper.Add(int.Parse(dr["i_idcatope"].ToString()), dr["i_idcatope"].ToString() + " - " + dr["c_dencatope"].ToString());
            }
            return dicCategoriaOper;
        }

        public DataTable ListarProveedores()
        {
            ProveedorDAL _provDal = new ProveedorDAL();
            return _provDal.SelectProveedores();
        }

        public int IngresarLanzamiento(List<LanzamientoDetDTO> listLanzamientoDet, List<MaterialPorColorDTO> listMaterialPorColor, string _user)
        {
            int nroRegistros = 0;
            foreach (LanzamientoDetDTO det in listLanzamientoDet)
            {
                if (_lanzamientoDal.InsertLanzamientoDet(det) > 0)
                    nroRegistros++;
            }
            List<LanzamientoCabDTO> listLanzamientoCab = listLanzamientoDet
                                                       .GroupBy(x => x.NumLanzamiento)
                                                       .Select(g => new LanzamientoCabDTO
                                                       {
                                                           NumDocumento = g.Max(x => x.NumDocumento),
                                                           NumLanzamiento = g.Key,
                                                           Fecha = g.Max(x => x.FechaIngreso),
                                                           Usuario = g.Max(x => x.Usuario)
                                                       }).ToList();

            foreach (LanzamientoCabDTO cab in listLanzamientoCab)
            {
                _lanzamientoDal.InsertLanzamientoCab(cab);
            }
            //Evaluar si hay colores combinados agregados
            if (listMaterialPorColor.Count > 0)
            {
                //Agrupar LanzamientoDetalle por Orden
                var AgrupadoOrdenLanzDet = from data in listLanzamientoDet.AsEnumerable()
                                           group data by new
                                           {
                                               orden = data.Orden,
                                           } into grupo
                                           select new
                                           {
                                               NumDocumento = grupo.Max(x => x.NumDocumento),
                                               NumLanzamiento = grupo.Max(x => x.NumLanzamiento),
                                               Orden = grupo.Key.orden,
                                               Maquina = grupo.Max(x => x.Maquina),
                                               Color = grupo.Max(x => x.Color),
                                               Modelo = grupo.Max(x => x.Modelo),
                                               CodProducto = grupo.Max(x => x.CodProducto),
                                               FechaIngreso = grupo.Max(x => x.FechaIngreso),
                                               HoraIngreso = grupo.Max(x => x.HoraIngreso),
                                               FechaSolicitud = grupo.Max(x => x.FechaSolicitud)
                                           };
                //Agrupar LanzamientoDetalle por Color y Sumar pesos para Lanzamiento Compuesto
                var AgrupadoColorLanzDet = from ld in listLanzamientoDet.AsEnumerable()
                                           group ld by new
                                           {
                                               color = ld.Color
                                           } into grupo
                                           select new
                                           {
                                               color = grupo.Key.color,
                                               kilos = grupo.Sum(x => x.KilosTot)
                                           };

                foreach (var item in AgrupadoOrdenLanzDet)
                {
                    int secuencia = 1;
                    var listFiltroMaterialPorColor = listMaterialPorColor.Where(x => x.ColorBase == item.Color).ToList();

                    var kilosPorColor = AgrupadoColorLanzDet
                                        .FirstOrDefault(x => x.color == item.Color).kilos.ToString();

                    foreach (MaterialPorColorDTO matCol in listFiltroMaterialPorColor)
                    {
                        decimal calculoKilos = (decimal.Parse(kilosPorColor) / 100) * matCol.Porcentaje;
                        LanzamientoCompDTO lanzComp = new LanzamientoCompDTO()
                        {
                            NumContrato = item.NumDocumento,
                            NumLanzamiento = item.NumLanzamiento,
                            Orden = item.Orden,
                            Maquina = char.Parse(item.Maquina),
                            Color = matCol.Color,
                            Calidad = matCol.Calidad,
                            Secuencia = secuencia,
                            Modelo = item.Modelo,
                            CodProducto = matCol.CodProducto,
                            Usuario = _user,
                            FechaIngreso = item.FechaIngreso,
                            FechaAtencion = item.FechaSolicitud,
                            HoraIngreso = item.HoraIngreso,
                            CodProductoSolicitado = item.CodProducto,
                            Porcentaje = matCol.Porcentaje,
                            Kilos = Math.Round(calculoKilos, 2)
                        };
                        //Insertar Lanzamiento Compuesto en BD
                        _lanzamientoDal.InsertLanzamientoComp(lanzComp);
                        secuencia++;
                    }
                }
            }
            return nroRegistros;
        }

        public AsignacionOrdenDetDTO AsignacionOrdenDet(LanzamientoDetDTO lanzDet, DateTime fechaRetorno,
                                                        int codCatOper, string nroAsignacion,
                                                        string codProveedor, bool todasOperaciones)
        {
            var _asigOrdenDet = new AsignacionOrdenDetDTO()
            {
                CodCatOperacion = codCatOper,
                NroAsignacion = nroAsignacion,
                Orden = lanzDet.Orden,
                Lote = lanzDet.Lote
            };
            return _asigOrdenDet;
        }

        public Dictionary<string, char> ModelosCorrelativoPorMaquina(List<ContratoDetalleDTO> listContrato)
        {
            Dictionary<string, char> modelosCorr = new Dictionary<string, char>();
            var listContAgrupado = listContrato
                                .GroupBy(x => new { x.ModeloAA, x.Linea })
                                .Select(x => new ContratoDetalleDTO { ModeloAA = x.Key.ModeloAA, Linea = x.Key.Linea })
                                .OrderBy(x => x.Linea).ThenBy(x => x.ModeloAA).ToList();

            string[] lineasShima = new string[] { "013", "014", "012", "010", "016" };
            int indice = 0;
            string lineaAnterior = string.Empty;

            foreach (var contratoDet in listContAgrupado)
            {
                var lineaEncontrada = Array.Find(lineasShima, x => x.Equals(contratoDet.Linea.Trim()));
                if (lineaEncontrada == null)
                {
                    if ((lineaAnterior != contratoDet.Linea))
                    {
                        indice = 65;
                    }
                    else
                    {
                        indice = indice + 1;
                    }
                    lineaAnterior = contratoDet.Linea;
                }
                else
                {
                    var lineaAnteriorEncontrada = Array.Find(lineasShima, x => x.Equals(lineaAnterior.Trim()));
                    if (lineaAnteriorEncontrada == null)
                    {
                        indice = 65;
                    }
                    else
                    {
                        indice = indice + 1;
                    }
                    lineaAnterior = contratoDet.Linea;
                }
                modelosCorr.Add(contratoDet.ModeloAA.Trim(), (char)indice);
            }
            return modelosCorr;
        }
    }
}
