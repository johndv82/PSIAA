using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataTransferObject;
using PSIAA.DataAccessLayer.TuartDB;
using System.Data;

namespace PSIAA.BusinessLogicLayer
{
    public class AnalisisContratoBLL
    {
        /// <summary>
        /// Variable de instancia a la clase SimulacionMpBLL.
        /// </summary>
        public SimulacionMpBLL _simulacionMpBll = new SimulacionMpBLL();
        /// <summary>
        /// Variable de instancia a la clase ContratoBLL.
        /// </summary>
        public ContratoBLL _contratoBll = new ContratoBLL();
        /// <summary>
        /// Variable de instancia a la clase PesosDAL.
        /// </summary>
        public PesosDAL _pesosDal = new PesosDAL();
        /// <summary>
        /// Variable de instancia a la clase MedidaPorTallaDAL.
        /// </summary>
        public MedidaPorTallaDAL _medidaPorTallaDal = new MedidaPorTallaDAL();

        /// <summary>
        /// Ejecuta procedimientos BLL de productos por combinación de modelo o color unitario segun sea el caso, con los datos obtenidos
        /// se construye un contenedor de materiales de tipo DataTable con los siguientes campos: Color, Material, Porcentaje, Decripción, 
        /// y se pobla dicho contenedor segun el tipo de color de cada modelo del contrato.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de datos de tipo DataTable con los materiales.</returns>
        /// 
        public DataTable ListarMaterialModelo(int contrato, string modelo)
        {
            Dictionary<string, List<string[]>> prodMaterialCombo = new Dictionary<string, List<string[]>>();
            Dictionary<string, string[]> prodMaterialUni = new Dictionary<string, string[]>();

            List<ContratoDetalleDTO> _listContratoDet = _contratoBll.ListarDetalleContrato(contrato, true);
            _listContratoDet = _listContratoDet.Where(x => x.ModeloAA.Trim() == modelo.Trim())
                                .OrderBy(x => x.ModeloAA).ToList()
                                .OrderBy(y => y.CodColor).ToList();

            foreach (var contratoDet in _listContratoDet)
            {
                if (contratoDet.CodColor.Trim().Substring(0, 2) == "C0")
                {
                    prodMaterialCombo.Add(contratoDet.CodColor.Trim(),
                        _simulacionMpBll.ListarProductosCombo(contratoDet.ModeloAA.Trim(), contratoDet.CodColor.Trim()));
                }
                else
                {
                    prodMaterialUni.Add(contratoDet.CodColor.Trim(), _simulacionMpBll.ProductoColorUnitario(contratoDet.ModeloAA.Trim()));
                }
            }

            DataTable dtMateriales = new DataTable();
            dtMateriales.Columns.Add("Color", typeof(string));
            dtMateriales.Columns.Add("Material", typeof(string));
            dtMateriales.Columns.Add("Porcentaje", typeof(double));
            dtMateriales.Columns.Add("Descripcion", typeof(string));

            if (prodMaterialCombo.Count > 0)
            {
                foreach (KeyValuePair<string, List<string[]>> combo in prodMaterialCombo)
                {
                    foreach (var item in combo.Value)
                    {
                        DataRow fila = dtMateriales.NewRow();
                        fila["Color"] = combo.Key.ToString();
                        fila["Material"] = item[0].ToString();
                        fila["Porcentaje"] = double.Parse(item[2].ToString());
                        fila["Descripcion"] = item[1].ToString();
                        dtMateriales.Rows.Add(fila);
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, string[]> color in prodMaterialUni)
                {
                    string colorUnitario = color.Key.ToString();
                    DataRow fila = dtMateriales.NewRow();
                    fila["Color"] = colorUnitario;
                    fila["Material"] = color.Value[0].ToString() + colorUnitario;
                    fila["Porcentaje"] = 100;
                    fila["Descripcion"] = color.Value[1].ToString();
                    dtMateriales.Rows.Add(fila);
                }

            }
            return dtMateriales;
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de medidas por talla, y evalúa la expresión matemática de cada medida, en el caso de que
        /// la expresión tenga un error de escritura, se actualizará dicho campo con la palabra: "Error".
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de datos de tipo DataTable con las medidas de la prenda, por talla de muestra</returns>
        public DataTable ListarMedidasPorModelo(string modelo)
        {
            string talla = TallaPesoMuestra(modelo)[0];
            DataTable _dtResult = _medidaPorTallaDal.SelectMedidasPorTalla(modelo.Trim(), talla);
            foreach (DataRow dr in _dtResult.Rows) {
                try
                {
                    string medida = dr["Medida"].ToString().Replace(' ', '+');
                    double resuelto = 0;
                    resuelto = double.Parse(new DataTable().Compute(medida == "" ? "0" : medida, "").ToString());
                }
                catch (Exception)
                {
                    dr["Medida"] = "Error";
                }
            }
            return _dtResult;
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de muestra de peso por modelo para obtener los valores de talla y peso base del modelo en cuestion.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Arreglo de tipo string con dos valores(1): talla(0) y peso base(1).</returns>
        public string[] TallaPesoMuestra(string modelo) { //[TALLA, PESO]
            string[] tallaPeso = new string[2] { "", "0"};
            DataRow drMuestraPeso = _pesosDal.SelectMuestraPeso(modelo.Trim());
            if (drMuestraPeso != null) {
                tallaPeso[0] = drMuestraPeso["c_tal"].ToString();
                tallaPeso[1] = drMuestraPeso["n_pestej"].ToString();
            }
            return tallaPeso;
        }

        /// <summary>
        /// Ejecuta un procedimiento BLL de Medidas por Modelo, y evalúa cada elemento de la columna: "Medida" si el valor
        /// es: "Error", en el caso de que haya al menos uno, se finaliza el procedimiento.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Variable de tipo bool con la validación del error.</returns>
        public bool ValidarMedidasPorModelo(string modelo) {
            DataTable dtResult = ListarMedidasPorModelo(modelo);
            bool verificacionModelo = true;
            if (dtResult.Rows.Count > 0)
            {
                foreach (DataRow row in dtResult.Rows)
                {
                    if (row["Medida"].ToString() == "Error")
                    {
                        verificacionModelo = false;
                        break;
                    }
                }
            }
            else {
                verificacionModelo = false;
            }
            return verificacionModelo;
        }

        /// <summary>
        /// Ejecuta un procedimiento BLL de Materiales por Modelo, agrupa los registros por color sumando el porcentaje, y evalúa
        /// que el valor del procentaje sea 100, en el caso contrario finaliza el procedimiento.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="modelo">Modelo de Prenda</param>
        /// <returns>Variable de tipo bool con la validación del porcentaje.</returns>
        public bool ValidarMaterialModelo(int contrato, string modelo) {
            DataTable dtMaterialModelo = ListarMaterialModelo(contrato, modelo);
            bool verificacionMaterial = true;
            var coloresAgrupado = (from dtm in dtMaterialModelo.AsEnumerable()
                                  group dtm by new
                                  {
                                      color = dtm.Field<string>("Color")
                                  } into grupo
                                  select new
                                  {
                                      Color = grupo.Key,
                                      Porcentaje = grupo.Sum(x => x.Field<double>("Porcentaje"))
                                  }).ToList();

            DataTable dtMatAgrupado = Helper.ToDataTable(coloresAgrupado);
            foreach (DataRow row in dtMatAgrupado.Rows) {
                if (row["Porcentaje"].ToString() != "100") {
                    verificacionMaterial = false;
                    break;
                }
            }
            return verificacionMaterial;
        }
    }
}
