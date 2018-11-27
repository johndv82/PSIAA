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
        private SimulacionMpBLL _simulacionMpBll = new SimulacionMpBLL();
        private ContratoBLL _contratoBll = new ContratoBLL();
        private PesosDAL _pesosDal = new PesosDAL();
        private MedidaPorTallaDAL _medidaPorTallaDal = new MedidaPorTallaDAL();

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

        public string[] TallaPesoMuestra(string modelo) { //[TALLA, PESO]
            string[] tallaPeso = new string[2] { "", "0"};
            DataRow drMuestraPeso = _pesosDal.SelectMuestraPeso(modelo.Trim());
            if (drMuestraPeso != null) {
                tallaPeso[0] = drMuestraPeso["c_tal"].ToString();
                tallaPeso[1] = drMuestraPeso["n_pestej"].ToString();
            }
            return tallaPeso;
        }

        //Metodos de Validacion, para Calculo de Materia Prima
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
