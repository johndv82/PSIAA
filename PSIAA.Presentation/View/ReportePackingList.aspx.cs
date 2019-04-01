using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer.SAP;
using PSIAA.DataTransferObject;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Data;
using System.Configuration;

namespace PSIAA.Presentation.View
{
    public partial class ReportePackingList : System.Web.UI.Page
    {
        private readonly PackingListBLL _packingListBll = new PackingListBLL();
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
                txtTipo.Focus();
                lblError.Visible = false;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            int documentoEntry = 0;
            if (!string.IsNullOrWhiteSpace(txtTipo.Text) &&
                !string.IsNullOrWhiteSpace(txtSerie.Text) &&
                !string.IsNullOrWhiteSpace(txtCorrelativo.Text)) {
                documentoEntry = _packingListBll.BuscarDocumentoEntry(txtTipo.Text, txtSerie.Text, txtCorrelativo.Text);
                lblDocEntry.Text = documentoEntry.ToString();
                DataRow drCabecera = _packingListBll.PackingListCabecera(documentoEntry);
                DataTable dtDetalle = MascaraCuadrosBlancoPorPagina(_packingListBll.PackingListDetalle(documentoEntry));
                if (drCabecera != null && dtDetalle.Rows.Count > 0)
                {
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
                    string nombrepdf = ExportReportToPDF("RepPackingList_" + lblDocEntry.Text.ToString() + "_" + usuarioActual);
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

        private DataTable MascaraCuadrosBlancoPorPagina(DataTable dtPackingListDetalle) {
            int nroFilasPagina = 57;
            int nroFilasPorAgregar = 0;
            int nroAgrupados = (from dt in dtPackingListDetalle.AsEnumerable()
                               group dt by new
                               {
                                   talla0 = dt.Field<string>("Talla0"),
                                   talla1 = dt.Field<string>("Talla1"),
                                   talla2 = dt.Field<string>("Talla2"),
                                   talla3 = dt.Field<string>("Talla3"),
                                   talla4 = dt.Field<string>("Talla4"),
                                   talla5 = dt.Field<string>("Talla5"),
                                   talla6 = dt.Field<string>("Talla6"),
                                   talla7 = dt.Field<string>("Talla7"),
                                   talla8 = dt.Field<string>("Talla8")
                               }
                               into grupo
                               select new
                               {
                                   Talla0 = grupo.Key.talla0,
                                   Talla1 = grupo.Key.talla1,
                                   Talla2 = grupo.Key.talla2,
                                   Talla3 = grupo.Key.talla3,
                                   Talla4 = grupo.Key.talla4,
                                   Talla5 = grupo.Key.talla5,
                                   Talla6 = grupo.Key.talla6,
                                   Talla7 = grupo.Key.talla7,
                                   Talla8 = grupo.Key.talla8
                               }).ToList().Count;

            int nroFilasDetalle = dtPackingListDetalle.Rows.Count;

            if ((nroFilasDetalle + nroAgrupados) < nroFilasPagina) {
                nroFilasPorAgregar = nroFilasPagina - (nroFilasDetalle + nroAgrupados);
            }
            else{
                int nroPaginas = (nroFilasDetalle + nroAgrupados) / nroFilasPagina;
                int capacidad = (nroPaginas + 1) * nroFilasPagina;
                nroFilasPorAgregar = capacidad - (nroFilasDetalle + nroAgrupados);
            }

            //Restar uno para el ultimo grupo de Z.
            nroFilasPorAgregar = nroFilasPorAgregar - 1;

            //Agregar Filas en Blanco

            for (int x = 1; x <= nroFilasPorAgregar; x++) {
                DataRow nuevaFila = dtPackingListDetalle.NewRow();
                for (int i = 0; i < nuevaFila.ItemArray.Length; i++)
                {
                    if (dtPackingListDetalle.Columns[i].DataType == Type.GetType("System.Int16") ||
                        dtPackingListDetalle.Columns[i].DataType == Type.GetType("System.Int32"))
                    {
                        nuevaFila[i] = 0;
                    }
                    else if (dtPackingListDetalle.Columns[i].DataType == Type.GetType("System.Decimal"))
                    {
                        nuevaFila[i] = 0.0;
                    }
                    else {
                        nuevaFila[i] = "ZZ";
                    }
                    nuevaFila["Paquete"] = 999;
                    nuevaFila["PesoNeto"] = "00.00";
                    nuevaFila["PesoBruto"] = "00.00";
                }
                dtPackingListDetalle.Rows.Add(nuevaFila);
            }
            return dtPackingListDetalle;
        }
    }
}