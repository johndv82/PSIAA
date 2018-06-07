using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using ClosedXML.Excel;
using System.Data;
using System.IO;

namespace PSIAA.Presentation.View
{
    public partial class IngresosAlmacen : System.Web.UI.Page
    {
        private AlmacenBLL _almacenBll = new AlmacenBLL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(txtFechaIni.Text) & !string.IsNullOrWhiteSpace(txtFechaFin.Text)) || !string.IsNullOrWhiteSpace(txtModelo.Text)) {
                DataTable _dtIngAlmacen = _almacenBll.ListarIngresosAlmacen(txtFechaIni.Text, txtFechaFin.Text, txtModelo.Text);
                Session["dtIngAlmacen"] = _dtIngAlmacen;
                if (_dtIngAlmacen.Rows.Count > 0)
                {
                    btnGuardarExcel.Visible = true;
                    gridIngresosAlmacen.DataSource = _dtIngAlmacen;
                    gridIngresosAlmacen.DataBind();
                }
                else {
                    btnGuardarExcel.Visible = false;
                    gridIngresosAlmacen.DataSource = null;
                    gridIngresosAlmacen.DataBind();
                }
            }
        }

        protected void gridIngresosAlmacen_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridIngresosAlmacen.DataSource = Session["dtIngAlmacen"];
            gridIngresosAlmacen.PageIndex = e.NewPageIndex;
            gridIngresosAlmacen.DataBind();
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
            var worksheet = workbook.Worksheets.Add("Ingresos a Almacen");

            worksheet.Cell("B2").Value = "INGRESOS A ALMACEN SIAA";
            //CABECERAS
            worksheet.Cell("B3").Value = "Almacen SIAA";
            worksheet.Cell("C3").Value = "Almacen SAP";
            worksheet.Cell("D3").Value = "Nombre Almacen";
            worksheet.Cell("E3").Value = "Nro. Parte";
            worksheet.Cell("F3").Value = "Cod. Producto";
            worksheet.Cell("G3").Value = "Color";
            worksheet.Cell("H3").Value = "Talla";
            worksheet.Cell("I3").Value = "Orden";
            worksheet.Cell("J3").Value = "Lote";
            worksheet.Cell("K3").Value = "Modelo";
            worksheet.Cell("L3").Value = "Contrato";
            worksheet.Cell("M3").Value = "Tipo Contrato";
            worksheet.Cell("N3").Value = "Cantidad"; //Peso Neto
            worksheet.Cell("O3").Value = "Usuario";
            worksheet.Cell("P3").Value = "Mes";

            DataTable dt = (DataTable)Session["dtIngAlmacen"];
            worksheet.Cell("B4").InsertData(dt.AsEnumerable());
            int filasTabla = dt.Rows.Count + 3;

            var rngTable = worksheet.Range("B2:P" + filasTabla).AddToNamed("Tabla");

            /** Cabecera */
            var rngHeaders = rngTable.Range("A1:O2");
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;

            rngTable.Row(1).Merge();

            /** Pie*/
            //var footer = rngTable.Row(filasTabla - 1);
            //footer.Style.Font.Bold = true;
            //footer.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //footer.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;

            /** Bordes */
            rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            //rngTable.Column(5).Style.NumberFormat.Format = "0.000000";

            worksheet.Columns().AdjustToContents();

            /******* Export *******/
            MemoryStream stream = GetStream(workbook);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode("IngresosAlmacen" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx"));
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
    }
}