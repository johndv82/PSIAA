﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer.SAP;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using PSIAA.DataTransferObject;

namespace PSIAA.Presentation.View
{
    public partial class AlmacenSap : System.Web.UI.Page
    {
        private OitwSapBLL _oitwSalBll = new OitwSapBLL();
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
            }
        }

        protected void gridAlmacenSap_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridAlmacenSap.DataSource = Session["ListaArticulos"] as DataTable;
            gridAlmacenSap.PageIndex = e.NewPageIndex;
            gridAlmacenSap.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable dtArticulos = _oitwSalBll.ListarArticulosSap(txtArticulo.Text, chkStockCero.Checked, txtCodigo.Text);
            if (dtArticulos.Rows.Count > 0)
            {
                Session["ListaArticulos"] = dtArticulos;
                gridAlmacenSap.DataSource = Session["ListaArticulos"] as DataTable;
                gridAlmacenSap.DataBind();
                lblNRegistros.Text = "N° de registros devueltos: " + dtArticulos.Rows.Count;
                btnExportar.Visible = true;
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
            var worksheet = workbook.Worksheets.Add("Almacén SAP");

            worksheet.Cell("B2").Value = "ALMACEN DE ARTICULOS SAP";
            //CABECERAS
            worksheet.Cell("B3").Value = "Codigo SAP";
            worksheet.Cell("C3").Value = "Descripcion";
            worksheet.Cell("D3").Value = "Costo Promedio";
            worksheet.Cell("E3").Value = "Stock";
            worksheet.Cell("F3").Value = "Unidad de Medida";

            DataTable dt = (DataTable)Session["ListaArticulos"];
            worksheet.Cell("B4").InsertData(dt.AsEnumerable());
            int filasTabla = dt.Rows.Count + 3;

            var rngTable = worksheet.Range("B2:F" + filasTabla).AddToNamed("Tabla");

            /** Cabecera */
            var rngHeaders = rngTable.Range("A1:E2");
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
            Response.AddHeader("content-disposition", "attachment; filename=" + Server.UrlEncode("AlmacenSap" + DateTime.Now.ToString("yyyyMMdd") + "_" + usuarioActual + ".xlsx"));
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
    }
}