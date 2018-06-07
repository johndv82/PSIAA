using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.BusinessLogicLayer.Reports;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using ClosedXML.Excel;

namespace PSIAA.Presentation.View
{
    public partial class Facturaciones : System.Web.UI.Page
    {
        private LiquidacionTallerBLL _liquidacionTallerBll = new LiquidacionTallerBLL();
        private FacturacionBLL _facturacionBll = new FacturacionBLL();
        private DocumentoPagoTallerBLL _docPagoTallerBll = new DocumentoPagoTallerBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFechaFacturacion.Text))
            {
                DataTable dtFacturaciones = _liquidacionTallerBll.ListarFacturacionesPorFecha(txtFechaFacturacion.Text);
                gridFacturaciones.DataSource = dtFacturaciones;
                gridFacturaciones.DataBind();
                if (dtFacturaciones.Rows.Count > 0)
                {
                    Session["dtFacturaciones"] = dtFacturaciones;
                    btnGuardarExcel.Visible = true;
                }
                else
                {
                    Session["dtFacturaciones"] = null;
                    btnGuardarExcel.Visible = false;
                }
            }
        }

        protected void gridFacturaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridFacturaciones.SelectedRow;
            string ruc = row.Cells[2].Text;
            string tipoMovimiento = row.Cells[3].Text == "Factura" ? "01" : "02";
            int serie = 0;
            int nroLiquidacion = int.Parse(row.Cells[6].Text);

            DateTime fechaQuince = new DateTime(2017, 11, 15);
            if (DateTime.Parse(row.Cells[7].Text) < fechaQuince) {
                serie = int.Parse((row.Cells[4].Text) == "E001" ? "101" : row.Cells[4].Text);
            }

            DataTable _dtCabecera = _facturacionBll.ListarCabecera(ruc, tipoMovimiento,
                                                    serie, nroLiquidacion);

            DataTable _dtDetalle = _facturacionBll.ListarDetalle(ruc, tipoMovimiento,
                                                    serie, nroLiquidacion);

            DataTable _dtTotalPorContrato = _facturacionBll.ListarTotalesPorContrato(ruc, tipoMovimiento,
                                                    serie, nroLiquidacion);

            if (_dtCabecera.Rows.Count == 0)
            {
                rptViewFactura.LocalReport.DataSources.Clear();
            }
            else
            {
                ReportDataSource rdsCabecera = new ReportDataSource();
                rdsCabecera.Name = "dsCabeceraFactura";
                rdsCabecera.Value = _dtCabecera;

                ReportDataSource rdsDetalle = new ReportDataSource();
                rdsDetalle.Name = "dsDetalleFactura";
                rdsDetalle.Value = _dtDetalle;

                ReportDataSource rdsTotalPorContrato = new ReportDataSource();
                rdsTotalPorContrato.Name = "dsTotalPorContratoFactura";
                rdsTotalPorContrato.Value = _dtTotalPorContrato;

                ReportParameter[] parametros = new ReportParameter[1];
                parametros[0] = new ReportParameter("Factura", (tipoMovimiento == "01").ToString());

                rptViewFactura.LocalReport.DataSources.Clear();
                rptViewFactura.LocalReport.DataSources.Add(rdsCabecera);
                rptViewFactura.LocalReport.DataSources.Add(rdsDetalle);
                rptViewFactura.LocalReport.DataSources.Add(rdsTotalPorContrato);
                rptViewFactura.LocalReport.SetParameters(parametros);
                rptViewFactura.LocalReport.Refresh();
            }
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
            var worksheet = workbook.Worksheets.Add("Facturaciones");

            //CABECERAS
            worksheet.Cell("B3").Value = "TALLER";
            worksheet.Cell("C3").Value = "RUC";
            worksheet.Cell("D3").Value = "TIPO MOV.";
            worksheet.Cell("E3").Value = "SERIE";
            worksheet.Cell("F3").Value = "NRO DOCUMENTO";
            worksheet.Cell("G3").Value = "NRO LIQUIDACION";
            worksheet.Cell("H3").Value = "FECHA DOC.";
            worksheet.Cell("I3").Value = "MONEDA";
            worksheet.Cell("J3").Value = "SUBTOTAL";
            worksheet.Cell("K3").Value = "IGV";
            worksheet.Cell("L3").Value = "USUARIO";
            worksheet.Cell("M3").Value = "TOTAL";

            DataTable dt = (DataTable)Session["dtFacturaciones"];
            worksheet.Cell("B4").InsertData(dt.AsEnumerable());
            int filasTabla = dt.Rows.Count + 3;

            var rngTable = worksheet.Range("B3:M" + filasTabla).AddToNamed("Tabla");

            /** Cabecera */
            var rngHeaders = rngTable.Range("A1:L1");
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);

            /** Bordes */
            rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            worksheet.Columns().AdjustToContents();

            rngTable.Column(9).Style.NumberFormat.Format = "0.000";
            rngTable.Column(10).Style.NumberFormat.Format = "0.000";
            rngTable.Column(12).Style.NumberFormat.Format = "0.000";

            /******* Export *******/
            MemoryStream stream = GetStream(workbook);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode("Facturaciones_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx"));
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
    }
}