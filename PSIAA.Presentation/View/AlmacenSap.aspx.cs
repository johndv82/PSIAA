using System;
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
        /// <summary>
        /// Variable de instancia a la clase OitwSapBLL.
        /// </summary>
        public OitwSapBLL _oitwSalBll = new OitwSapBLL();
        /// <summary>
        /// Variable publica para almacenar el usuario logueado.
        /// </summary>
        public string usuarioActual = string.Empty;

        /// <summary>
        /// Evento de carga principal del formulario AlmacenSap.aspx
        /// </summary>
        /// <remarks>
        /// En este evento se evalúa la existencia de la sesión del usuario y tambien capturamos su valor en una variable publica,
        /// para su posterior uso.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
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

        /// <summary>
        /// Evento de Cambio de Indice de Página de la grilla gridAlmacenSap.
        /// </summary>
        /// <remarks>
        /// En este evento se obtiene el Listado de Articulos SAP desde una variable Session, y se carga la grilla de gridAlmacenSap.
        /// Establece un índice de pagina actual en la grilla de gridAlmacenSap.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void gridAlmacenSap_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridAlmacenSap.DataSource = Session["ListaArticulos"] as DataTable;
            gridAlmacenSap.PageIndex = e.NewPageIndex;
            gridAlmacenSap.DataBind();
        }

        /// <summary>
        /// Evento Click del botón btnBuscar.
        /// </summary>
        /// <remarks>
        /// En este evento se ejecuta el procedimiento BLL de Listar Articulos SAP enviando sus parametros requeridos, y el resultado
        /// es cargado en una variable Session para luego usar éste, como fuente de la grilla gridAlmacenSap. En el caso el procedimiento
        /// devuelva vacío no se cargará la grilla.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
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

        private MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }

        /// <summary>
        /// Evento Click del botón btnExportar.
        /// </summary>
        /// <remarks>
        /// En este evento se carga los datos de Session de Articulos SAP en un contenedor para luego, usando la libreria ClosedXML,
        /// exportarlo en un formato Excel(.xlsx), con sus cabeceras respectivas.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
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