using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.DataTransferObject;
using System.IO;
using ClosedXML.Excel;
using System.Data;

namespace PSIAA.Presentation.View
{
    public partial class IngresosFaltantesAlmacen : System.Web.UI.Page
    {
        private ContratoBLL _contratoBll = new ContratoBLL();
        RecepcionControlBLL _recepcionControlBll = new RecepcionControlBLL();
        public string usuarioActual = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] != null)
            {
                usuarioActual = ((UsuarioDTO)Session["usuario"]).User;

                if (!IsPostBack)
                {
                    /*
                     * Diferente a Post y Back
                     * Todo lo que se ejecutará al recargar la pagina
                     * Cuando se acciona un botón llamamos Post
                     * Cuando usamos el botón Atras del Navegador llamamos Back
                     */
                }
                txtContrato.Focus();
            }
        }

        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtContrato.Text)) {
                cmbModelos.DataSource = _contratoBll.ListarModelosContrato(int.Parse(txtContrato.Text));
                cmbModelos.DataBind();
                btnExportar.Visible = false;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string modelo = string.Empty;
            if (cmbModelos.Items.Count > 0) {
                modelo = cmbModelos.SelectedIndex == 0 ? "" : cmbModelos.Text;
                DataTable dtIngresosFaltates = _recepcionControlBll.ListarIngresosFaltantesAlmacen(int.Parse(txtContrato.Text), modelo);
                Session["dtIngresosFaltantes"] = dtIngresosFaltates;
                gridIngresosFaltantes.DataSource = Session["dtIngresosFaltantes"];
                gridIngresosFaltantes.DataBind();
                btnExportar.Visible = true;
                lblNRegistros.Text = "N° de registros devueltos: " + dtIngresosFaltates.Rows.Count;
            }
        }

        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }

        protected void gridIngresosFaltantes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridIngresosFaltantes.DataSource = Session["dtIngresosFaltantes"];
            gridIngresosFaltantes.PageIndex = e.NewPageIndex;
            gridIngresosFaltantes.DataBind();
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Almacén");

            worksheet.Cell("B2").Value = "Ingresos Faltantes a Almacen";
            //CABECERAS
            worksheet.Cell("B3").Value = "Contrato";
            worksheet.Cell("C3").Value = "Orden";
            worksheet.Cell("D3").Value = "Lote";
            worksheet.Cell("E3").Value = "Talla";
            worksheet.Cell("F3").Value = "Modelo";
            worksheet.Cell("G3").Value = "Color";
            worksheet.Cell("H3").Value = "Cant. Lanzada";
            worksheet.Cell("I3").Value = "Acab. Confec.";
            worksheet.Cell("J3").Value = "Ctrl. Final";
            worksheet.Cell("K3").Value = "Almacen";
            worksheet.Cell("L3").Value = "Faltante";

            DataTable dt = (DataTable)Session["dtIngresosFaltantes"];
            worksheet.Cell("B4").InsertData(dt.AsEnumerable());
            int filasTabla = dt.Rows.Count + 3;

            var rngTable = worksheet.Range("B2:L" + filasTabla).AddToNamed("Tabla");

            /** Cabecera */
            var rngHeaders = rngTable.Range("A1:K2");
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.LightCornflowerBlue;

            rngTable.Row(1).Merge();

            /** Bordes */
            rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            worksheet.Columns().AdjustToContents();

            /******* Export *******/
            MemoryStream stream = GetStream(workbook);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode("IngresosFaltantesAlmacen" + DateTime.Now.ToString("yyyyMMdd") + "_" + usuarioActual + ".xlsx"));
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
    }
}