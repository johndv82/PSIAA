using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PSIAA.BusinessLogicLayer.Reports;
using PSIAA.DataTransferObject.Report;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Configuration;

namespace PSIAA.Presentation.View
{
    public partial class ReporteContrato : System.Web.UI.Page
    {
        private ContratoRepBLL _contratoRepBll = new ContratoRepBLL();
        private HttpCookie cookie;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                cookie = Request.Cookies["Usuario"];
                if (cookie != null)
                {
                    hidUsuario.Value = cookie["Nombre"].ToString();
                    lblError.Visible = false;
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
            txtContrato.Focus();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            List<ContratoCabDTO> listaContrato = new List<ContratoCabDTO>();
            lblError.Visible = false;
            listaContrato.Clear();
            ContratoCabDTO _contratoCab = _contratoRepBll.Cabecera(int.Parse(txtContrato.Text));
            if (_contratoCab._msnError == null)
            {
                if (_contratoCab.NumContrato == null)
                {
                    lblError.Visible = true;
                    lblError.Text = "No hay Ninguna Coincidencia para el Numero de Contrato";
                    rptViewContrato.LocalReport.DataSources.Clear();
                }
                else
                {
                    hidContrato.Value = txtContrato.Text;
                    listaContrato.Add(_contratoCab);

                    ReportDataSource rdsCabecera = new ReportDataSource();
                    rdsCabecera.Name = "dsRdlcCabecera";
                    rdsCabecera.Value = listaContrato;

                    ReportDataSource rdsDetalle = new ReportDataSource();
                    rdsDetalle.Name = "dsRdlcDetalle";
                    rdsDetalle.Value = listaContrato[0].Detalle;

                    rptViewContrato.LocalReport.DataSources.Clear();
                    rptViewContrato.LocalReport.DataSources.Add(rdsCabecera);
                    rptViewContrato.LocalReport.DataSources.Add(rdsDetalle);
                    rptViewContrato.LocalReport.Refresh();
                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = _contratoCab._msnError;
                rptViewContrato.LocalReport.DataSources.Clear();
            }

            //Si no existe, creamos el documento
            string nombrepdf = ExportReportToPDF("Contrato_" + hidContrato.Value.ToString() + "_" + hidUsuario.Value.ToString());
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
                byte[] bytes = rptViewContrato.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension,
                                                            out streamids, out warnings);

                string filename = @"C:\inetpub\wwwroot\PSIAA\Reports\Contratos\" + reportName + ".pdf";
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
    }
}