using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.DataTransferObject;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace PSIAA.Presentation.View
{
    public partial class ReporteLiquidaciones : System.Web.UI.Page
    {
        private LiquidacionTallerBLL _liquidTallerBll = new LiquidacionTallerBLL();
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
                    ddlPeriodos.DataSource = _liquidTallerBll.ListarYears();
                    ddlPeriodos.DataBind();
                    ddlPeriodos_SelectedIndexChanged(sender, e);
                }
            }
        }

        protected void ddlPeriodos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSemanas.DataSource = _liquidTallerBll.ListarSemanas(int.Parse(ddlPeriodos.SelectedValue));
            ddlSemanas.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable dtLiquidacionesPorSem = _liquidTallerBll.ListarLiquidacionesPorSemana(int.Parse(ddlPeriodos.SelectedItem.ToString()), int.Parse(ddlSemanas.SelectedItem.ToString()));
            Session["dtLiquidacionesPorSem"] = dtLiquidacionesPorSem;
            if (dtLiquidacionesPorSem.Rows.Count > 0)
            {
                btnGuardarExcel.Visible = true;
                gridLiquidaciones.DataSource = dtLiquidacionesPorSem;
                gridLiquidaciones.DataBind();
            }
            else
            {
                btnGuardarExcel.Visible = false;
                gridLiquidaciones.DataSource = null;
                gridLiquidaciones.DataBind();
            }
        }

        protected void gridLiquidaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridLiquidaciones.DataSource = Session["dtLiquidacionesPorSem"];
            gridLiquidaciones.PageIndex = e.NewPageIndex;
            gridLiquidaciones.DataBind();
        }

        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }

        protected void btnGuardarExcel_Click(object sender, EventArgs e)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Liquidaciones");

            worksheet.Cell("B2").Value = "Reporte de Liquidaciones";
            //CABECERAS
            worksheet.Cell("B3").Value = "TALLER";
            worksheet.Cell("C3").Value = "PERIODO";
            worksheet.Cell("D3").Value = "RUC";
            worksheet.Cell("E3").Value = "MOVIMIENTO";
            worksheet.Cell("F3").Value = "NRO LIQUIDACION";
            worksheet.Cell("G3").Value = "FECHA";
            worksheet.Cell("H3").Value = "CONCEPTO";
            worksheet.Cell("I3").Value = "MONEDA";
            worksheet.Cell("J3").Value = "SUBTOTAL";
            worksheet.Cell("K3").Value = "IGV";
            worksheet.Cell("L3").Value = "USUARIO";
            worksheet.Cell("M3").Value = "GLOSA";
            worksheet.Cell("N3").Value = "TOTAL";
            worksheet.Cell("O3").Value = "SEMANA";

            DataTable dt = (DataTable)Session["dtLiquidacionesPorSem"];
            worksheet.Cell("B4").InsertData(dt.AsEnumerable());
            int filasTabla = dt.Rows.Count + 3;

            var rngTable = worksheet.Range("B2:O" + filasTabla).AddToNamed("Tabla");

            /** Cabecera */
            var rngHeaders = rngTable.Range("A1:N2");
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
            Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode("Reporte_Liquidaciones" + DateTime.Now.ToString("yyyyMMdd") + "_" + usuarioActual + ".xlsx"));
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
    }
}