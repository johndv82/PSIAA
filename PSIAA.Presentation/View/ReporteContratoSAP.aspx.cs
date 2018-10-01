using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer.SAP;
using System.Data;
using System.Configuration;
using System.IO;

namespace PSIAA.Presentation.View
{
    public partial class ReporteContratoSAP : System.Web.UI.Page
    {
        private HttpCookie cookie;
        private readonly ContratoBLL _contratoBll = new ContratoBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
            if (!string.IsNullOrWhiteSpace(txtContrato.Text))
            {
                hidContrato.Value = txtContrato.Text;
                DataRow drCabecera = _contratoBll.ContratoCabecera(hidContrato.Value.ToString());
                DataTable dtDetalle = _contratoBll.ContratoDetalle(hidContrato.Value.ToString());
                if (drCabecera != null && dtDetalle.Rows.Count > 0)
                {
                    ReportDataSource rdsCabecera = new ReportDataSource();
                    rdsCabecera.Name = "dsRepContratoCab";
                    rdsCabecera.Value = drCabecera.Table;

                    ReportDataSource rdsDetalle = new ReportDataSource();
                    rdsDetalle.Name = "dsRepContratoDet";
                    rdsDetalle.Value = dtDetalle;

                    rptViewContrato.LocalReport.DataSources.Clear();
                    rptViewContrato.LocalReport.DataSources.Add(rdsCabecera);
                    rptViewContrato.LocalReport.DataSources.Add(rdsDetalle);
                    rptViewContrato.LocalReport.Refresh();

                    //Si no existe, creamos el documento
                    string nombrepdf = ExportReportToPDF("RepContratoSAP_" + hidContrato.Value.ToString() + "_" + hidUsuario.Value.ToString());
                    if (nombrepdf != string.Empty)
                    {
                        //Cargamos el PDFViewer
                        string server = ConfigurationManager.AppSettings["servidor"];
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento('" + nombrepdf + "', '" + server + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento();", true);
                    }
                }
                else
                {
                    lblError.Visible = true;
                    rptViewContrato.LocalReport.DataSources.Clear();
                }
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