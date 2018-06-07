using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace PSIAA.Presentation.View
{
    public partial class IngresosPuntosControl : System.Web.UI.Page
    {
        private RecepcionControlBLL _recepcionControlBll = new RecepcionControlBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable dtIngresosPC = _recepcionControlBll.ListarIngresosPuntoControl(txtFechaIni.Text, txtFechaFin.Text);
            if (dtIngresosPC.Rows.Count > 0)
            {
                Session["dtIngresosPC"] = dtIngresosPC;
                gridIngresosPuntoControl.DataSource = dtIngresosPC;
                gridIngresosPuntoControl.DataBind();
                btnGuardarExcel.Visible = true;
                lblNRegistros.Text = "N° de Registros Devueltos: " + dtIngresosPC.Rows.Count;
            }
            else {
                gridIngresosPuntoControl.DataSource = null;
                gridIngresosPuntoControl.DataBind();
                btnGuardarExcel.Visible = false;
                lblNRegistros.Text = string.Empty;
            }
        }

        protected void gridIngresosPuntoControl_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridIngresosPuntoControl.DataSource = Session["dtIngresosPC"] as DataTable;
            gridIngresosPuntoControl.PageIndex = e.NewPageIndex;
            gridIngresosPuntoControl.DataBind();
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
            var worksheet = workbook.Worksheets.Add("IngresosPC");

            worksheet.Cell("B2").Value = "INGRESOS A PUNTOS DE CONTROL";
            //CABECERAS
            worksheet.Cell("B3").Value = "Contrato";
            worksheet.Cell("C3").Value = "Modelo";
            worksheet.Cell("D3").Value = "Orden";
            worksheet.Cell("E3").Value = "Lote";
            worksheet.Cell("F3").Value = "Talla";
            worksheet.Cell("G3").Value = "Color";
            worksheet.Cell("H3").Value = "Cant. Asig.";
            worksheet.Cell("I3").Value = "Punto";
            worksheet.Cell("J3").Value = "Operación";
            worksheet.Cell("K3").Value = "Cod. Taller";
            worksheet.Cell("L3").Value = "Taller";
            worksheet.Cell("M3").Value = "Cant. Ing.";
            worksheet.Cell("N3").Value = "Fecha Ing.";

            DataTable dt = (DataTable)Session["dtIngresosPC"];
            worksheet.Cell("B4").InsertData(dt.AsEnumerable());
            int filasTabla = dt.Rows.Count + 3;

            var rngTable = worksheet.Range("B2:N" + filasTabla).AddToNamed("Tabla");

            /** Cabecera */
            var rngHeaders = rngTable.Range("A1:M2");
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
            Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode("IngresosPuntosControl" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx"));
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
    }
}