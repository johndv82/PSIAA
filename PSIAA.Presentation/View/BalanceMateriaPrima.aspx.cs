using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.DataTransferObject;
using System.Data;
using PSIAA.BusinessLogicLayer.SAP;
using System.IO;
using ClosedXML.Excel;

namespace PSIAA.Presentation.View
{
    public partial class BalanceMateriaPrima : System.Web.UI.Page
    {
        public string usuarioActual = string.Empty;
        private readonly BalanceMpBLL _balanceMpBll = new BalanceMpBLL();
        private decimal[] totalAvance = new decimal[] { 0, 0, 0, 0, 0, 0, 0 };

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
                    lblError.Visible = false;
                }
                txtContrato.Focus();
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtContrato.Text)) {
                DataTable dtBalanceMP = _balanceMpBll.ReporteBalanceMateriaPrima(txtContrato.Text);
                if (dtBalanceMP.Rows.Count > 0)
                {
                    gridBalanceMP.DataSource = dtBalanceMP;
                    gridBalanceMP.DataBind();
                    btnGuardarExcel.Visible = true;
                    lblError.Visible = false;
                    Session["dtBalanceMP"] = dtBalanceMP;
                }
                else {
                    lblError.Text = "No hay datos para ese Contrato";
                    lblError.Visible = true;
                }
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
            var worksheet = workbook.Worksheets.Add("BalanceMP");

            //CABECERAS
            worksheet.Cell("B2").Value = "ORDEN";
            worksheet.Cell("C2").Value = "MODELO";
            worksheet.Cell("D2").Value = "PRODUCTO";
            worksheet.Cell("E2").Value = "DESTINO";
            worksheet.Cell("F2").Value = "ENTREGADO";
            worksheet.Cell("G2").Value = "DEVUELTO";
            worksheet.Cell("H2").Value = "ALMACEN 29";
            worksheet.Cell("I2").Value = "ALMACEN 11";
            worksheet.Cell("J2").Value = "ALMACEN 10";
            worksheet.Cell("K2").Value = "UTILIZADO";
            worksheet.Cell("L2").Value = "MATERIA PRIMA";
            worksheet.Cell("M2").Value = "SALDO";
            worksheet.Cell("N2").Value = "COD. TALLER";
            worksheet.Cell("O2").Value = "TALLER";

            DataTable dt = (DataTable)Session["dtBalanceMP"];
            worksheet.Cell("B3").InsertData(dt.AsEnumerable());
            int filasTabla = dt.Rows.Count + 2;

            var rngTable = worksheet.Range("B2:O" + filasTabla);

            var table = rngTable.CreateTable();
            table.ShowTotalsRow = true;
            table.Field("ENTREGADO").TotalsRowFunction = XLTotalsRowFunction.Sum;
            table.Field("DEVUELTO").TotalsRowFunction = XLTotalsRowFunction.Sum;
            table.Field("ALMACEN 29").TotalsRowFunction = XLTotalsRowFunction.Sum;
            table.Field("ALMACEN 11").TotalsRowFunction = XLTotalsRowFunction.Sum;
            table.Field("ALMACEN 10").TotalsRowFunction = XLTotalsRowFunction.Sum;
            table.Field("UTILIZADO").TotalsRowFunction = XLTotalsRowFunction.Sum;
            table.Field("MATERIA PRIMA").TotalsRowFunction = XLTotalsRowFunction.Sum;
            table.Field("SALDO").TotalsRowFunction = XLTotalsRowFunction.Sum;
            table.Field(0).TotalsRowLabel = "TOTAL";

            /** Bordes */
            rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            worksheet.Columns().AdjustToContents();

            /******* Export *******/
            MemoryStream stream = GetStream(workbook);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode("Reporte_BalanceMP" + "-"+ txtContrato.Text+ "-"+ DateTime.Now.ToString("yyyyMMdd") + "_" + usuarioActual + ".xlsx"));
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }

        protected void gridBalanceMP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowType != DataControlRowType.EmptyDataRow))
            {
                totalAvance[0] += e.Row.Cells[4].Text == "" ? 0 : decimal.Parse(e.Row.Cells[4].Text);
                totalAvance[1] += e.Row.Cells[5].Text == "" ? 0 : decimal.Parse(e.Row.Cells[5].Text);
                totalAvance[2] += e.Row.Cells[6].Text == "" ? 0 : decimal.Parse(e.Row.Cells[6].Text);
                totalAvance[3] += e.Row.Cells[7].Text == "" ? 0 : decimal.Parse(e.Row.Cells[7].Text);
                totalAvance[4] += e.Row.Cells[8].Text == "" ? 0 : decimal.Parse(e.Row.Cells[8].Text);
                totalAvance[5] += e.Row.Cells[9].Text == "" ? 0 : decimal.Parse(e.Row.Cells[9].Text);
                totalAvance[6] += e.Row.Cells[10].Text == "" ? 0 : decimal.Parse(e.Row.Cells[10].Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = totalAvance[0].ToString("0.000");
                e.Row.Cells[5].Text = totalAvance[1].ToString("0.000");
                e.Row.Cells[6].Text = totalAvance[2].ToString("0.000");
                e.Row.Cells[7].Text = totalAvance[3].ToString("0.000");
                e.Row.Cells[8].Text = totalAvance[4].ToString("0.000");
                e.Row.Cells[9].Text = totalAvance[5].ToString("0.000");
                e.Row.Cells[10].Text = totalAvance[6].ToString("0.000");
                e.Row.Font.Bold = true;
                e.Row.Font.Size = FontUnit.Medium;
            }
        }
    }
}