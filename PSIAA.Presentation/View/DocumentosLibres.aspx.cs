using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.DataTransferObject;
using PSIAA.BusinessLogicLayer;
using PSIAA.BusinessLogicLayer.Reports;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Configuration;

namespace PSIAA.Presentation.View
{
    public partial class DocumentosLibres : System.Web.UI.Page
    {
        private DocumentoPagoLibreBLL _docPagoLibreBll = new DocumentoPagoLibreBLL();
        private LiquidacionTallerBLL _liquidTallerBll = new LiquidacionTallerBLL();
        private PagoLibreBLL _pagoLibreBll = new PagoLibreBLL();
        private double[] totalDetalle = { 0, 0, 0 };
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
                    txtCodProveedor.Focus();
                    ddlPeriodos.DataSource = _docPagoLibreBll.ListarYears();
                    ddlPeriodos.DataBind();
                    ddlPeriodos_SelectedIndexChanged(sender, e);
                }
            }
        }

        protected void btnBuscarPorNombre_Click(object sender, EventArgs e)
        {
            txtNombreComercial.Focus();
            DataView dvProveedores = new DataView(_docPagoLibreBll.ListarProveedores());
            Session["ListadoProveedores"] = dvProveedores;
            gridProveedores.DataSource = dvProveedores.ToTable();
            gridProveedores.DataBind();
        }

        protected void gridProveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProveedores.DataSource = ((DataView)Session["ListadoProveedores"]).ToTable();
            gridProveedores.PageIndex = e.NewPageIndex;
            gridProveedores.DataBind();
        }

        protected void gridProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridProveedores.SelectedRow;
            txtCodProveedor.Text = row.Cells[0].Text.Trim();
            hidCodProveedor.Value = txtCodProveedor.Text;
            txtNombreProveedor.Text = ((Button)row.FindControl("btnNombreComercial")).Text.Trim();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CloseModalPorveedores();", true);
            btnBuscar.Visible = true;
            txtNombreProveedor.ForeColor = System.Drawing.Color.Black;
        }

        protected void btnBuscarPorCod_Click(object sender, EventArgs e)
        {
            string respuesta = _docPagoLibreBll.DevolverNombreProveedor(txtCodProveedor.Text);
            if (respuesta == string.Empty)
            {
                txtNombreProveedor.ForeColor = System.Drawing.Color.Red;
                txtNombreProveedor.Text = "ERROR, Proveedor no encontrado";
                btnBuscar.Visible = false;
            }
            else
            {
                txtNombreProveedor.ForeColor = System.Drawing.Color.Black;
                txtNombreProveedor.Text = respuesta;
                hidCodProveedor.Value = txtCodProveedor.Text;
                btnBuscar.Visible = true;
            }
        }

        protected void txtNombreComercial_TextChanged(object sender, EventArgs e)
        {
            DataView dvFiltro = ((DataView)Session["ListadoProveedores"]);
            dvFiltro.RowFilter = "nombre_comercial LIKE '%" + txtNombreComercial.Text + "%'";
            Session["ListadoProveedores"] = dvFiltro;
            gridProveedores.DataSource = dvFiltro.ToTable();
            gridProveedores.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable dtPagosLibres = _liquidTallerBll.ListarLiquidacionesLibres(hidCodProveedor.Value, ddlPeriodos.SelectedValue, ddlMeses.SelectedValue);
            if (dtPagosLibres.Rows.Count > 0)
            {
                gridDocumentosLibres.DataSource = dtPagosLibres;

                //Evaluar si hay datos en la sesion de NroLiquidacion
                if (Session["NroLiquidacion"] != null)
                {
                    if (int.Parse(Session["NroLiquidacion"].ToString()) > 0)
                    {
                        //Mostrar nro de Liquidacion
                        lblRespuesta.Text = "Ultimo N° de Liquidación Generado: " + Session["NroLiquidacion"];
                        lblRespuesta.ForeColor = System.Drawing.Color.Green;
                        lblRespuesta.Visible = true;
                    }
                    else
                    {
                        lblRespuesta.Text = "Hubo un error al registrar los datos";
                        lblRespuesta.ForeColor = System.Drawing.Color.Red;
                        lblRespuesta.Visible = true;
                    }
                    Session["NroLiquidacion"] = null;
                }
            }
            else
            {
                gridDocumentosLibres.DataSource = new DataTable();
            }
            gridDocumentosLibres.DataBind();
            btnNuevo.Visible = true;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ddlOperacionesLibres.DataSource = _docPagoLibreBll.ListarOperacionesLibres();
            ddlOperacionesLibres.DataBind();
            //Crear instancia de la coleccion del DTO de DocPagoLibre
            List<DocumentoPagoLibreDTO> listDocPagoLibre = new List<DocumentoPagoLibreDTO>();
            Session["listDocPagoLibre"] = listDocPagoLibre;
            gridDetalleDocLibre.DataSource = new DataTable();
            gridDetalleDocLibre.DataBind();
            lblRuc.Text = hidCodProveedor.Value;
            //Cargamos numero de semana y fecha
            System.Globalization.Calendar c = CultureInfo.CurrentCulture.Calendar;
            var semana = c.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Sunday).ToString();
            lblSemanaFecha.Text = semana + " - " + DateTime.Now.ToShortDateString();
            txtPrecio.Text = "0.00";
            btnGuardar.Visible = false;
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string codOperacion = ddlOperacionesLibres.SelectedValue;
            List<DocumentoPagoLibreDTO> listDocPagoLibre = Session["listDocPagoLibre"] as List<DocumentoPagoLibreDTO>;
            Session["NroLiquidacion"] = _docPagoLibreBll.GuardarDocumentoPagoLibre(listDocPagoLibre, hidCodProveedor.Value,
                                                                    ddlTipoMov.SelectedValue, ddlMoneda.SelectedValue,
                                                                    usuarioActual);

            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CloseModalNuevoPago();", true);
            btnBuscar_Click(sender, e);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            List<DocumentoPagoLibreDTO> listDocPagoLibre = Session["listDocPagoLibre"] as List<DocumentoPagoLibreDTO>;
            if (_docPagoLibreBll.ExisteOrdenLoteRecepcion(txtOrden.Text.ToUpper(), int.Parse(txtLote.Text))){
                lblInexistenciaOrden.Visible = false;
                var documentoPagoLibre = new DocumentoPagoLibreDTO
                {
                    CodProveedor = hidCodProveedor.Value,
                    //Continuar
                    Orden = txtOrden.Text.ToUpper(),
                    Lote = int.Parse(txtLote.Text),
                    CodOperacion = ddlOperacionesLibres.SelectedValue,
                    DenominacionOper = ddlOperacionesLibres.SelectedItem.Text,
                    Talla = txtTalla.Text.ToUpper(),
                    Prendas = int.Parse(txtPrendas.Text),
                    Tiempo = double.Parse(txtTiempo.Text),
                    Precio = double.Parse(txtPrecio.Text),
                    Observaciones = txtObservaciones.Text.ToUpper()
                };
                listDocPagoLibre.Add(documentoPagoLibre);
                Session["listDocPagoLibre"] = listDocPagoLibre;
                gridDetalleDocLibre.DataSource = listDocPagoLibre;
                gridDetalleDocLibre.DataBind();
                //Limpiar Campos
                txtOrden.Text = string.Empty;
                txtLote.Text = string.Empty;
                txtTalla.Text = string.Empty;
                txtPrendas.Text = string.Empty;
                txtTiempo.Text = string.Empty;
                txtPrecio.Text = string.Empty;
            }else
                lblInexistenciaOrden.Visible = true;
        }

        protected void gridDocumentosLibres_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridDocumentosLibres.SelectedRow;
            string tipoMovimiento = row.Cells[1].Text == "Factura" ? "01" : "02";
            int serie = int.Parse(row.Cells[2].Text);
            int nroLiquidacion = int.Parse(row.Cells[3].Text);

            DataTable _dtCabecera = _pagoLibreBll.ListarCabecera(hidCodProveedor.Value, tipoMovimiento,
                                                    serie, nroLiquidacion);

            DataTable _dtDetalle = _pagoLibreBll.ListarDetalle(hidCodProveedor.Value, tipoMovimiento,
                                                    serie, nroLiquidacion);

            if (_dtCabecera.Rows.Count == 0)
            {
                rptViewPagoLibre.LocalReport.DataSources.Clear();
            }
            else
            {
                ReportDataSource rdsCabecera = new ReportDataSource();
                rdsCabecera.Name = "dsCabeceraPagoLibre";
                rdsCabecera.Value = _dtCabecera;

                ReportDataSource rdsDetalle = new ReportDataSource();
                rdsDetalle.Name = "dsDetallePagoLibre";
                rdsDetalle.Value = _dtDetalle;

                ReportParameter[] parametros = new ReportParameter[1];
                parametros[0] = new ReportParameter("Factura", (tipoMovimiento == "01").ToString());

                rptViewPagoLibre.LocalReport.DataSources.Clear();
                rptViewPagoLibre.LocalReport.DataSources.Add(rdsCabecera);
                rptViewPagoLibre.LocalReport.DataSources.Add(rdsDetalle);
                rptViewPagoLibre.LocalReport.SetParameters(parametros);
                rptViewPagoLibre.LocalReport.Refresh();
            }

            string archivo = "T" + txtCodProveedor.Text + "_" + usuarioActual;

            //Si no existe, creamos el documento
            string nombrepdf = ExportReportToPDF(archivo);
            if (nombrepdf != string.Empty)
            {
                //Cargamos el PDFViewer
                string server = ConfigurationManager.AppSettings["servidor"];
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento('" + nombrepdf + "', '" + server + "');", true);
            }
            else
            {
                //lblMensajeError.Visible = true;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento();", true);
            }
        }

        private string ExportReportToPDF(string reportName)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            try
            {
                byte[] bytes = rptViewPagoLibre.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension,
                                                            out streamids, out warnings);

                string filename = @"C:\inetpub\wwwroot\PSIAA\Reports\Docs\" + reportName + ".pdf";
                using (var fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }

                if (File.Exists(filename))
                    return reportName;
                else
                    return string.Empty;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return string.Empty;
            }
        }

        protected void gridDetalleDocLibre_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowType != DataControlRowType.EmptyDataRow))
            {
                totalDetalle[0] += e.Row.Cells[4].Text == "" ? 0 : double.Parse(e.Row.Cells[4].Text);
                totalDetalle[1] += e.Row.Cells[6].Text == "" ? 0 : double.Parse(e.Row.Cells[6].Text);
                totalDetalle[2] += e.Row.Cells[8].Text == "" ? 0 : double.Parse(e.Row.Cells[8].Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = totalDetalle[0].ToString();
                e.Row.Cells[6].Text = totalDetalle[1].ToString("0.00");
                e.Row.Cells[8].Text = totalDetalle[2].ToString("0.00");

                e.Row.Font.Bold = true;
                e.Row.Font.Size = FontUnit.Medium;
                if (totalDetalle[0] == 0)
                {
                    btnGuardar.Visible = false;
                }
                else
                {
                    btnGuardar.Visible = true;
                }
            }
        }

        protected void gridDetalleDocLibre_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridDetalleDocLibre.SelectedRow;
            List<DocumentoPagoLibreDTO> listDocPagoLibre = Session["listDocPagoLibre"] as List<DocumentoPagoLibreDTO>;

            DocumentoPagoLibreDTO docLibre = (from doc in listDocPagoLibre
                                              where doc.Orden == row.Cells[2].Text
                                              && doc.Lote == int.Parse(row.Cells[3].Text)
                                              && doc.Prendas == int.Parse(row.Cells[4].Text)
                                              && doc.Talla == row.Cells[5].Text
                                              && doc.Tiempo == double.Parse(row.Cells[6].Text)
                                              select doc).FirstOrDefault();

            listDocPagoLibre.Remove(docLibre);
            Session["listDocPagoLibre"] = listDocPagoLibre;
            gridDetalleDocLibre.DataSource = listDocPagoLibre;
            gridDetalleDocLibre.DataBind();
        }

        protected void ddlPeriodos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMeses.DataSource = _docPagoLibreBll.ListarMeses(int.Parse(ddlPeriodos.SelectedValue));
            ddlMeses.DataBind();
        }
    }
}