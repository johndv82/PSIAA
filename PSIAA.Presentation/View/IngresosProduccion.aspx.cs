using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.BusinessLogicLayer.Reports;
using PSIAA.DataTransferObject;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Configuration;

namespace PSIAA.Presentation.View
{
    public partial class IngresosProduccion : System.Web.UI.Page
    {
        private AlmacenBLL _almacenBll = new AlmacenBLL();
        private IngresoProduccionBLL _ingresoProdBll = new IngresoProduccionBLL();
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
                    cmbAlmacenes.DataSource = _almacenBll.ListarAlmacenes();
                    cmbAlmacenes.DataBind();
                }
            }
        }

        protected void cmbAlmacenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNumAlmacen.Text = cmbAlmacenes.SelectedValue.ToString();
        }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            int _codAlmacen = int.Parse(cmbAlmacenes.SelectedValue);
            Session["dtIngresosProduccion"] = _almacenBll.ListarIngresosProduccion(_codAlmacen, txtFechaIngreso.Text);
            gridIngresos.DataSource = Session["dtIngresosProduccion"] as DataTable;
            gridIngresos.DataBind();
        }

        protected void gridIngresos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int _codAlmacen = int.Parse(cmbAlmacenes.SelectedValue);
            gridIngresos.DataSource = Session["dtIngresosProduccion"] as DataTable;
            gridIngresos.PageIndex = e.NewPageIndex;
            gridIngresos.DataBind();
        }

        protected void gridIngresos_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridIngresos.SelectedRow;
            string parte = row.Cells[4].Text;
            int codAlmacenSap = int.Parse(row.Cells[2].Text);

            DataTable _dtDetalle = _ingresoProdBll.DetalleIngresoProduccion(parte, codAlmacenSap);


            ReportDataSource rdsDetalle = new ReportDataSource();
            rdsDetalle.Name = "dsParteIngreso";
            rdsDetalle.Value = _dtDetalle;

            ReportParameter[] parametros = new ReportParameter[1];
            parametros[0] = new ReportParameter("documento", parte);

            rptViewPagoLibre.LocalReport.DataSources.Clear();
            rptViewPagoLibre.LocalReport.DataSources.Add(rdsDetalle);
            rptViewPagoLibre.LocalReport.SetParameters(parametros);
            rptViewPagoLibre.LocalReport.Refresh();

            //Si no existe, creamos el documento
            string nombrepdf = ExportReportToPDF("ParteIngreso_" + parte + "_" + usuarioActual);
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

                string filename = @"C:\inetpub\wwwroot\PSIAA\Reports\Partes\" + reportName + ".pdf";
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