using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.BusinessLogicLayer.Produccion;
using System.IO;
using ClosedXML.Excel;
using System.Data;
using PSIAA.DataTransferObject;

namespace PSIAA.Presentation.View
{
    public partial class CostosProduccion : System.Web.UI.Page
    {
        private ContratoBLL _contratoBll = new ContratoBLL();
        private CostosProduccionBLL _costosProdBll = new CostosProduccionBLL();
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
            if (!string.IsNullOrWhiteSpace(txtContrato.Text))
            {
                ddlModelo.DataSource = _contratoBll.ListarModelosContrato(int.Parse(txtContrato.Text));
                ddlModelo.DataBind();
                lblCliente.Text = _contratoBll.ObtenerClienteContrato(int.Parse(txtContrato.Text));
                lblCerrado.Text = _contratoBll.VerificarContratoCerrado(int.Parse(txtContrato.Text));
                hidContrato.Value = txtContrato.Text;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(hidContrato.Value) && !string.IsNullOrWhiteSpace(txtFechaIni.Text) && !string.IsNullOrWhiteSpace(txtFechaIni.Text))
            {
                string modelo = ddlModelo.SelectedIndex == 0 ? "" : ddlModelo.SelectedItem.ToString();
                DataTable dtCostosProd = _costosProdBll.ListarCostosProduccion(int.Parse(txtContrato.Text),
                                                                                txtFechaIni.Text,
                                                                                txtFechaFin.Text, modelo.Trim());
                //gridCostosProd.DataSource = dtCostosProd;
                //gridCostosProd.DataBind();
                Session["dtCostosProd"] = dtCostosProd;
                if (dtCostosProd.Rows.Count > 0)
                {
                    btnExportar.Visible = true;
                    lblMensajeOk.Visible = true;
                    lblMensajeError.Visible = false;
                }
                else
                {
                    btnExportar.Visible = false;
                    lblMensajeError.Visible = true;
                    lblMensajeOk.Visible = false;
                }
            }
        }

        protected void gridCostosProd_RowCreated(object sender, GridViewRowEventArgs e)
        {
            /*if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Contrato";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.Lavender;

                HeaderCell = new TableCell();
                HeaderCell.Text = "Productos de Almacen";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.Lavender;

                HeaderCell = new TableCell();
                HeaderCell.Text = "Materia Prima";
                HeaderCell.ColumnSpan = 4;
                HeaderGridRow.Cells.Add(HeaderCell);
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.Lavender;

                HeaderCell = new TableCell();
                HeaderCell.Text = "Avios";
                HeaderCell.ColumnSpan = 1;
                HeaderGridRow.Cells.Add(HeaderCell);
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.Lavender;

                HeaderCell = new TableCell();
                HeaderCell.Text = "Mano de Obra";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.Lavender;

                HeaderCell = new TableCell();
                HeaderCell.Text = "Costos";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.Lavender;

                HeaderCell = new TableCell();
                HeaderCell.Text = "Ventas SAP";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.Lavender;

                gridCostosProd.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }*/
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
            var worksheet = workbook.Worksheets.Add("CostosProducción");

            //Fecha de Reporte
            worksheet.Cell("B2").Value = "Fechas: " + txtFechaIni.Text + " / " + txtFechaFin.Text;
            worksheet.Cell("B2").Style.Font.Bold = true;
            worksheet.Range("B2:E2").Row(1).Merge();

            //CABECERAS PRIMARIAS
            worksheet.Cell("B4").Value = "Contrato";
            worksheet.Range("B4:G4").Row(1).Merge();

            worksheet.Cell("H4").Value = "Almacen";
            worksheet.Range("H4:I4").Row(1).Merge();

            worksheet.Cell("J4").Value = "Materia Prima";
            worksheet.Range("J4:Q4").Row(1).Merge();

            worksheet.Cell("R4").Value = "Desperdicios";
            worksheet.Range("R4:AC4").Row(1).Merge();

            worksheet.Cell("AD4").Value = "Materia Prima Presupuestada por Ingenieria";
            worksheet.Range("AD4:AF4").Row(1).Merge();

            worksheet.Cell("AG4").Value = "Avios";

            worksheet.Cell("AH4").Value = "Cantidades Asigandas a Talleres";
            worksheet.Range("AH4:AM4").Row(1).Merge();

            worksheet.Cell("AN4").Value = "Costos Mano de Obra Directa";
            worksheet.Range("AN4:AU4").Row(1).Merge();

            worksheet.Cell("AV4").Value = "Costos Mano de Obra Indirecta";
            worksheet.Range("AV4:BB4").Row(1).Merge();

            worksheet.Cell("BC4").Value = "TOTAL COSTOS";
            worksheet.Range("BC4:BF4").Row(1).Merge();

            worksheet.Cell("BG4").Value = "TOTAL GASTOS";
            worksheet.Range("BG4:BI4").Row(1).Merge();

            worksheet.Cell("BJ4").Value = "Ventas SAP";
            worksheet.Range("BJ4:BM4").Row(1).Merge();

            //CABECERAS SECUNDARIAS-------------------------------------
            //Contrato
            worksheet.Cell("B5").Value = "Contrato";
            worksheet.Cell("C5").Value = "Cliente";
            worksheet.Cell("D5").Value = "Modelo";
            worksheet.Cell("E5").Value = "Linea";
            worksheet.Cell("F5").Value = "Cant. Solicitadas";
            worksheet.Cell("G5").Value = "Cant. Lanzadas";

            //Productos de Almacen
            worksheet.Cell("H5").Value = "Productos Terminados";
            worksheet.Cell("I5").Value = "Productos No Conformes";

            //Materia Prima
            worksheet.Cell("J5").Value = "Kilos Tela";
            worksheet.Cell("K5").Value = "Precio Kilos Tela";
            worksheet.Cell("L5").Value = "Metros Tela";
            worksheet.Cell("M5").Value = "Precio Metros Tela";
            worksheet.Cell("N5").Value = "Kilos Hilado";
            worksheet.Cell("O5").Value = "Precio Kilos Hilado";
            worksheet.Cell("P5").Value = "Precio Promedio Hilado";
            worksheet.Cell("Q5").Value = "Peso Paneles Recepcionados";

            //Desperdicios Hilado
            worksheet.Cell("R5").Value = "Desperdicios - Almacen 10";
            worksheet.Cell("S5").Value = "Precio de Desperdicios - Almacen 10";
            worksheet.Cell("T5").Value = "Desperdicios Hilado - Almacen 11";
            worksheet.Cell("U5").Value = "Precio de Desperdicios Hilado - Almacen 11";
            //Desperdicios Tela
            worksheet.Cell("V5").Value = "Desperdicios Tela en Kilos - Almacen 10";
            worksheet.Cell("W5").Value = "Precio de Desperdicios Tela en Kilos - Almacen 10";
            worksheet.Cell("X5").Value = "Desperdicios Tela en Metros - Almacen 10";
            worksheet.Cell("Y5").Value = "Precio de Desperdicios Tela en Metros - Almacen 10";
            worksheet.Cell("Z5").Value = "Desperdicios Tela en Kilos - Almacen 11";
            worksheet.Cell("AA5").Value = "Precio de Desperdicios Tela en Kilos - Almacen 11";
            worksheet.Cell("AB5").Value = "Desperdicios Tela en Metros - Almacen 11";
            worksheet.Cell("AC5").Value = "Precio de Desperdicios Tela en Metros - Almacen 11";

            //Materia Prima Presupuestada
            worksheet.Cell("AD5").Value = "Kilos MP Lanzada";
            worksheet.Cell("AE5").Value = "Costo MP Lanzada";
            worksheet.Cell("AF5").Value = "Costo Unitario MP Lanzada";

            //Avios
            worksheet.Cell("AG5").Value = "Precio";

            //Cantidades asignadas a Talleres
            worksheet.Cell("AH5").Value = "Corte/Tejido AA";
            worksheet.Cell("AI5").Value = "Confecion/Acabado AA";
            worksheet.Cell("AJ5").Value = "Acabado Final AA";
            worksheet.Cell("AK5").Value = "Corte/Tejido Tercero";
            worksheet.Cell("AL5").Value = "Confeccion/Acabado Tercero";
            worksheet.Cell("AM5").Value = "Acabado Final Tercero";

            //Costos de Mano de Obra Directa
            worksheet.Cell("AN5").Value = "Costo Tejido";
            worksheet.Cell("AO5").Value = "Costo Lavado";
            worksheet.Cell("AP5").Value = "Costo Confeccion";
            worksheet.Cell("AQ5").Value = "Costo Acabado";
            worksheet.Cell("AR5").Value = "Serv. Externo Tejido";
            worksheet.Cell("AS5").Value = "Serv. Externo Confeccion";
            worksheet.Cell("AT5").Value = "Serv. Externo Acabado Confeccion";
            worksheet.Cell("AU5").Value = "Serv. Externo Acabado Final";

            //Costos de Mano de Obra Indirecta
            worksheet.Cell("AV5").Value = "Costo Ctrl. Calidad";
            worksheet.Cell("AW5").Value = "Tejido Punto";
            worksheet.Cell("AX5").Value = "Corte";
            worksheet.Cell("AY5").Value = "Confeccion";
            worksheet.Cell("AZ5").Value = "Acabado";
            worksheet.Cell("BA5").Value = "Muestras";
            worksheet.Cell("BB5").Value = "Costos Indirectos Adm Prod";

            //Total Costos
            worksheet.Cell("BC5").Value = "Costo MP";
            worksheet.Cell("BD5").Value = "Costo MO";
            worksheet.Cell("BE5").Value = "Costo Avios";
            worksheet.Cell("BF5").Value = "Costos Indirectos";

            //Total Gastos
            worksheet.Cell("BG5").Value = "Gasto Adm.";
            worksheet.Cell("BH5").Value = "Gasto Ventas";
            worksheet.Cell("BI5").Value = "Gasto Financiero";

            //Ventas SAP
            worksheet.Cell("BJ5").Value = "Costo Total";
            worksheet.Cell("BK5").Value = "Facturado";
            worksheet.Cell("BL5").Value = "Unidades Facturadas";
            worksheet.Cell("BM5").Value = "Margen";

            DataTable dt = (DataTable)Session["dtCostosProd"];
            worksheet.Cell("B6").InsertData(dt.AsEnumerable());
            int filasTabla = dt.Rows.Count + 5;

            var rngTable = worksheet.Range("B4:BM" + filasTabla).AddToNamed("Tabla");
            rngTable.Style.Alignment.WrapText = true;

            /** Formato Cabecera Primaria*/
            var rngHeadersPrim = rngTable.Range("A1:BL1");
            rngHeadersPrim.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeadersPrim.Style.Font.Bold = true;
            rngHeadersPrim.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;

            /** Formato Cabecera Secundaria*/
            var rngHeadersSec = rngTable.Range("A2:BL2");
            rngHeadersSec.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeadersSec.Style.Font.Bold = true;
            rngHeadersSec.Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232);
            rngHeadersSec.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            /** Bordes */
            rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            for (int i = 9; i <= 32; i++) {
                rngTable.Column(i).Style.NumberFormat.Format = "0.0000";
            }

            for (int i = 39; i <= 62; i++)
            {
                rngTable.Column(i).Style.NumberFormat.Format = "0.0000";
            }
            rngTable.Column(64).Style.NumberFormat.Format = "0.0000";

            //----Ajustar tamaño a todas las columnas----
            for (int s = 2; s <= 65; s++) {
                worksheet.Column(s).Width = 12;
            }

            /******* Export *******/
            MemoryStream stream = GetStream(workbook);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode("CostosProduccion" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx"));
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
    }
}