using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer.Reports;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Data;
using System.Configuration;

namespace PSIAA.Presentation.View
{
    public partial class ReportePackingList : System.Web.UI.Page
    {
        private readonly PackingListBLL _packingListBll = new PackingListBLL();
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
            txtDocumento.Focus();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDocumento.Text)) {
                DataRow drCabecera = _packingListBll.PackingListCabecera(Convert.ToInt32(txtDocumento.Text));
                DataTable dtDetalle = _packingListBll.PackingListDetalle(Convert.ToInt32(txtDocumento.Text));
                if (drCabecera != null && dtDetalle.Rows.Count > 0)
                {
                    hidDocumento.Value = txtDocumento.Text;
                    ReportDataSource rdsCabecera = new ReportDataSource();
                    rdsCabecera.Name = "dsPackingListCab";
                    rdsCabecera.Value = drCabecera.Table;

                    ReportDataSource rdsDetalle = new ReportDataSource();
                    rdsDetalle.Name = "dsPackingListDet";
                    rdsDetalle.Value = dtDetalle;

                    rptViewPackingList.LocalReport.DataSources.Clear();
                    rptViewPackingList.LocalReport.DataSources.Add(rdsCabecera);
                    rptViewPackingList.LocalReport.DataSources.Add(rdsDetalle);
                    rptViewPackingList.LocalReport.Refresh();
                    
                    //Si no existe, creamos el documento
                    string nombrepdf = ExportReportToPDF("RepPackingList_" + hidDocumento.Value.ToString() + "_" + hidUsuario.Value.ToString());
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
                else {
                    lblError.Visible = true;
                    rptViewPackingList.LocalReport.DataSources.Clear();
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
                byte[] bytes = rptViewPackingList.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension,
                                                            out streamids, out warnings);

                string filename = @"C:\inetpub\wwwroot\PSIAA\Reports\PackingList\" + reportName + ".pdf";
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