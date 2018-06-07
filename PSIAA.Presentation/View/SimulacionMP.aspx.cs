using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.DataTransferObject;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Threading;

namespace PSIAA.Presentation.View
{
    public partial class SimulacionMP : System.Web.UI.Page
    {
        private SimulacionMpBLL _simulacionBll = new SimulacionMpBLL();
        private AnalisisContratoBLL _analisisContBll = new AnalisisContratoBLL();
        private ContratoBLL _contratoBll = new ContratoBLL();
        private HttpCookie cookie;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                cookie = Request.Cookies["Usuario"];
                if (cookie != null)
                {
                    hidUsuario.Value = cookie["Nombre"].ToString();
                    txtContrato.Focus();
                    if ((hidUsuario.Value == "produccion"))
                    {
                        rbnTipoProceso.Items[1].Enabled = false;
                    }
                }
                else
                {
                    //LOGOUT
                    string user = Request.QueryString["logout"];
                    Session.Remove(user);
                    Session.Abandon();
                    //Destruir Sesiones
                    for (int i = 0; i < Session.Count; i++)
                    {
                        var nombre = Session.Keys[i].ToString();
                        Session.Remove(nombre);
                    }
                    Response.Redirect("default.aspx");
                }
                txtAdicional.Text = "0.00";
            }
            lblRptaMaterial.Visible = false;
            lblRptaMedidaPeso.Visible = false;
        }
        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtContrato.Text)) {
                hidContrato.Value = txtContrato.Text;
                #region CapturarModelos
                List<string> modelosSeleccionados = new List<string>();
                foreach (ListItem item in lstModelos.Items)
                {
                    if (item.Selected)
                    {
                        modelosSeleccionados.Add(item.Text.Trim());
                    }
                }
                if (modelosSeleccionados.Count == 0)
                {
                    modelosSeleccionados = lstModelos.Items.Cast<ListItem>().Select(x => x.Text.Trim()).ToList();
                }
                #endregion

                if (rbnTipoProceso.Items[0].Selected)
                {
                    AccionProcesar(modelosSeleccionados);
                }
                else {
                    if (ValidarModelos(modelosSeleccionados))
                    {
                        AccionProcesar(modelosSeleccionados);
                    }
                }
            }
            lblMensajeOk.Visible = false;
            lblErrorCalculo.Visible = false;
            lblErrorGuardarCalculo.Visible = false;
        }

        private void AccionProcesar(List<string> _modelSelect) {
            if (rbnTipoProceso.Items[0].Selected)
            {
                List<SimulacionDetDTO> listsimDetDto = _simulacionBll.ListarSimulacionMpDetalle(int.Parse(txtContrato.Text), _modelSelect);
                if (listsimDetDto.Count > 0)
                {
                    Session["ListSimCalculo"] = listsimDetDto;
                    gridSimulacionDet.DataSource = Session["ListSimCalculo"];
                    gridSimulacionDet.ForeColor = System.Drawing.Color.Black;
                    gridSimulacionDet.DataBind();
                    btnExportar.Visible = true;
                }
                else
                {
                    gridSimulacionDet.DataSource = new DataTable();
                    gridSimulacionDet.ForeColor = System.Drawing.Color.Black;
                    gridSimulacionDet.DataBind();
                    btnExportar.Visible = false;
                }
                btnGuardar.Visible = false;
            }
            else
            {
                List<SimulacionDetDTO> listSimCalculo = _simulacionBll.ListarCalculoMateriaPrima(int.Parse(txtContrato.Text),
                                                        hidUsuario.Value, _modelSelect, decimal.Parse(txtAdicional.Text));
                if (listSimCalculo.Count > 0)
                {
                    Session["ListSimCalculo"] = listSimCalculo;
                    gridSimulacionDet.DataSource = listSimCalculo;
                    gridSimulacionDet.ForeColor = System.Drawing.Color.Blue;
                    gridSimulacionDet.DataBind();
                    btnGuardar.Visible = true;
                    lblErrorCalculo.Visible = false;
                    btnExportar.Visible = true;
                }
                else
                {
                    lblErrorCalculo.Visible = true;
                    gridSimulacionDet.DataSource = new DataTable();
                    gridSimulacionDet.DataBind();
                    btnGuardar.Visible = false;
                    btnExportar.Visible = false;
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            List<SimulacionDetDTO> listSimCalculo = Session["ListSimCalculo"] as List<SimulacionDetDTO>;
            int regIngresados = _simulacionBll.IngresarSimulacionCalculo(listSimCalculo, int.Parse(hidContrato.Value), hidUsuario.Value);
            if (regIngresados > 2)
            {
                lblMensajeOk.Visible = true;
                gridSimulacionDet.DataSource = new DataTable();
                gridSimulacionDet.DataBind();
                btnExportar.Visible = false;
                btnGuardar.Visible = false;
            }
            else {
                lblErrorGuardarCalculo.Visible = true;
                lblMensajeOk.Visible = false;
            }
        }

        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("SimulacionMP");

            worksheet.Cell("A2").Value = "Simulación de Cálculo de Materia Prima";
            //CABECERAS
            worksheet.Cell("A3").Value = "Correlativo";
            worksheet.Cell("B3").Value = "#Simulacion";
            worksheet.Cell("C3").Value = "Maquina";
            worksheet.Cell("D3").Value = "Color";
            worksheet.Cell("E3").Value = "Cod. Producto";
            worksheet.Cell("F3").Value = "Material";
            worksheet.Cell("G3").Value = "Modelo";
            worksheet.Cell("H3").Value = "Kilos";
            worksheet.Cell("I3").Value = "%Segur.";
            worksheet.Cell("J3").Value = "Total Kilo";
            worksheet.Cell("K3").Value = "Kilos Almac";
            worksheet.Cell("L3").Value = "Fecha de Ingreso";
            worksheet.Cell("M3").Value = "Hora Ing.";
            worksheet.Cell("N3").Value = "Usuario";
            worksheet.Cell("O3").Value = "Contrato";

            List<SimulacionDetDTO> lista = (List<SimulacionDetDTO>)Session["ListSimCalculo"];
            string cliente = _contratoBll.ObtenerClienteContrato(int.Parse(hidContrato.Value));
            worksheet.Cell("A4").InsertData(lista.AsEnumerable());
            int filasTabla = lista.Count + 3;

            var rngTable = worksheet.Range("A2:O" + filasTabla).AddToNamed("Tabla");

            /** Cabecera */
            var rngHeaders = rngTable.Range("A1:O2");
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Lavender;

            rngTable.Row(1).Merge();

            /** Bordes */
            rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            worksheet.Columns().AdjustToContents();

            /******* Export *******/
            MemoryStream stream = GetStream(workbook);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode("SimulacionMP" + DateTime.Now.ToString("yyyyMMdd") + "_" + hidContrato.Value + "_" + cliente+ ".xlsx"));
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }

        private bool ValidarModelos(List<string> _modelSelect) {
            bool verificado = true;
            if (_modelSelect.Count > 0)
            {
                foreach (string model in _modelSelect) {
                    //Material
                    if (_analisisContBll.ValidarMaterialModelo(int.Parse(txtContrato.Text), model))
                    {
                        lblRptaMaterial.Text = "Datos de Material Verificado";
                        lblRptaMaterial.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblRptaMaterial.Text = "Conflicto de Materiales (SIAA/TACITA): " + model;
                        lblRptaMaterial.ForeColor = System.Drawing.Color.Red;
                        verificado = false;
                        break;
                    }
                    //Medidas
                    if (_analisisContBll.ValidarMedidasPorModelo(model))
                    {
                        lblRptaMedidaPeso.Text = "Datos de Pesos/Medidas Verificado";
                        lblRptaMedidaPeso.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblRptaMedidaPeso.Text = "Conflicto en Tallas de Medidas y Peso (TACITA): " + model;
                        lblRptaMedidaPeso.ForeColor = System.Drawing.Color.Red;
                        verificado = false;
                        break;
                    }
                }
                lblRptaMaterial.Visible = true;
                lblRptaMedidaPeso.Visible = true;
            }
            return verificado;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<string> _modelos = _contratoBll.ListarModelosContrato(int.Parse(txtContrato.Text));
            if (_modelos.Count > 0) {
                _modelos.RemoveAt(0);
                hidContrato.Value = txtContrato.Text;
                lstModelos.DataSource = _modelos;
                lstModelos.DataBind();
                btnProcesar.Visible = true;
            }
        }
    }
}