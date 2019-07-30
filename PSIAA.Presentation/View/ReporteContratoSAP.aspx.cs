using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer.SAP;
using PSIAA.DataTransferObject;
using System.Data;
using System.Configuration;
using System.IO;

namespace PSIAA.Presentation.View
{
    public partial class ReporteContratoSAP : System.Web.UI.Page
    {
        private readonly ContratoBLL _contratoBll = new ContratoBLL();
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
                    lblError.Visible = false;
                }
                txtContrato.Focus();
            }
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
                    lblError.Visible = false;
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
                    string nombrepdf = ExportReportToPDF("RepContratoSAP_" + hidContrato.Value.ToString() + "_" + usuarioActual);
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
                    lblError.Text = "No hay Ninguna Coincidencia para el Numero de Contrato";
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