using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.BusinessLogicLayer.Reports;
using PSIAA.DataTransferObject;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Configuration;

namespace PSIAA.Presentation.View
{
    public partial class Liquidaciones : System.Web.UI.Page
    {
        private DocumentoPagoTallerBLL _docPagoTallerBll = new DocumentoPagoTallerBLL();
        private LiquidacionTallerBLL _liquidTallerBll = new LiquidacionTallerBLL();
        private FacturacionBLL _facturacionBll = new FacturacionBLL();
        private HttpCookie cookie;
        private double[] totalDetalle = { 0, 0, 0 };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cookie = Request.Cookies["Usuario"];
                if (cookie != null)
                {
                    hidUsuario.Value = cookie["Nombre"].ToString();
                    txtCodProveedor.Focus();
                    ddlPeriodos.DataSource = _liquidTallerBll.ListarYears();
                    ddlPeriodos.DataBind();
                }
                else
                {
                    //LOGOUT
                    string user = Request.QueryString["logout"];
                    Session.Remove(user);
                    Session.Abandon();
                    //Destruir Sesiones
                    for (int i = 0; i < Session.Count; i++)
                    {
                        var nombre = Session.Keys[i].ToString();
                        Session.Remove(nombre);
                    }
                    Response.Redirect("default.aspx");
                }
            }
            txtCodProveedor.Focus();
        }

        protected void btnBuscarPorNombre_Click(object sender, EventArgs e)
        {
            txtNombreComercial.Focus();
            DataView dvProveedores = new DataView(_docPagoTallerBll.ListarProveedores());
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
            btnBuscarLiquid.Visible = true;
            txtNombreProveedor.ForeColor = System.Drawing.Color.Black;
        }

        protected void btnBuscarPorCod_Click(object sender, EventArgs e)
        {
            string respuesta = _docPagoTallerBll.DevolverNombreProveedor(txtCodProveedor.Text);
            if (respuesta == string.Empty)
            {
                txtNombreProveedor.ForeColor = System.Drawing.Color.Red;
                txtNombreProveedor.Text = "ERROR, Proveedor no encontrado";
                btnBuscarLiquid.Visible = false;
            }
            else
            {
                txtNombreProveedor.ForeColor = System.Drawing.Color.Black;
                txtNombreProveedor.Text = respuesta;
                hidCodProveedor.Value = txtCodProveedor.Text;
                btnBuscarLiquid.Visible = true;
            }
        }

        protected void btnBuscarLiquid_Click(object sender, EventArgs e)
        {
            gridLiquidaciones.DataSource = _liquidTallerBll.ListarLiquidaciones(hidCodProveedor.Value, int.Parse(ddlPeriodos.SelectedValue));
            gridLiquidaciones.DataBind();
        }

        protected void gridLiquidaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridLiquidaciones.SelectedRow;
            string tipoMovimiento = row.Cells[2].Text == "Factura" ? "01" : "02";
            int serie = int.Parse(row.Cells[3].Text);
            int nroLiquidacion = int.Parse(row.Cells[4].Text);

            //Crear y mostrar PDF
            DataTable _dtCabecera = _facturacionBll.ListarCabecera(txtCodProveedor.Text, tipoMovimiento,
                                                    serie, nroLiquidacion);

            DataTable _dtDetalle = _facturacionBll.ListarDetalle(txtCodProveedor.Text, tipoMovimiento,
                                                                    serie, nroLiquidacion);

            DataTable _dtTotalPorContrato = _facturacionBll.ListarTotalesPorContrato(
                                                        txtCodProveedor.Text, tipoMovimiento,
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

            string archivo = "T" + txtCodProveedor.Text + "_" + hidUsuario.Value.ToString();

            //Si no existe, creamos el documento
            string nombrepdf = ExportReportToPDF(archivo);
            if (nombrepdf != string.Empty)
            {
                //Cargamos el PDFViewer
                string server = ConfigurationManager.AppSettings["servidor"];
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento('" + nombrepdf + "', '"+ server +"');", true);
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
                byte[] bytes = rptViewFactura.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension,
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

        protected void txtNombreComercial_TextChanged(object sender, EventArgs e)
        {
            DataView dvFiltro = ((DataView)Session["ListadoProveedores"]);
            dvFiltro.RowFilter = "nombre_comercial LIKE '%" + txtNombreComercial.Text + "%'";
            Session["ListadoProveedores"] = dvFiltro;
            gridProveedores.DataSource = dvFiltro.ToTable();
            gridProveedores.DataBind();
        }
    }
}