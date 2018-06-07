using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.BusinessLogicLayer.Reports;
using System.Data;
using BarcodeLib;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Configuration;

namespace PSIAA.Presentation.View
{
    public partial class OrdenProduccion : System.Web.UI.Page
    {
        private OrdenProduccionBLL _ordenProduccionBll = new OrdenProduccionBLL();
        private ContratoBLL _contratoBll = new ContratoBLL();
        private OrdenRequisicionBLL _ordenRequiBll = new OrdenRequisicionBLL();
        private HttpCookie cookie;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cookie = Request.Cookies["Usuario"];
                if (cookie != null)
                {
                    hidUsuario.Value = cookie["Nombre"].ToString();
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtContrato.Text))
            {
                DataTable dtResult = _ordenProduccionBll.ListarOrdenesLanzadasAsignadas(int.Parse(txtContrato.Text),
                                                    int.Parse(ddlCatOperacion.SelectedValue));
                if (dtResult.Rows.Count > 0)
                {
                    gridOrdenesLanzadasAsignadas.DataSource = dtResult;
                    gridOrdenesLanzadasAsignadas.DataBind();
                    hidContrato.Value = txtContrato.Text;
                    chkAgregarCopiaOP.Visible = true;
                    chkAgregarCopiaTP.Visible = true;
                    chkAgregarCopiaOR.Visible = true;

                    btnGenerarOrdenProduccion.Visible = true;
                    btnGenerarTarjetaProduccion.Visible = true;
                    btnGenerarOrdenRequisicion.Visible = true;
                }
                else
                {
                    gridOrdenesLanzadasAsignadas.DataSource = new DataTable();
                    gridOrdenesLanzadasAsignadas.DataBind();
                    hidContrato.Value = "0";
                    chkAgregarCopiaOP.Visible = false;
                    chkAgregarCopiaTP.Visible = false;
                    chkAgregarCopiaOR.Visible = false;

                    btnGenerarOrdenProduccion.Visible = false;
                    btnGenerarTarjetaProduccion.Visible = false;
                    btnGenerarOrdenRequisicion.Visible = false;
                }
                chkAgregarCopiaOP.Checked = false;
                chkAgregarCopiaTP.Checked = false;
                chkAgregarCopiaOR.Checked = false;

                linkOrdenProd.Text = string.Empty;
                linkTarjetaProd.Text = string.Empty;
                linkOrdenReq.Text = string.Empty;
            }
        }

        private string GenerarDocumentoOrdenProduccion(int contrato, string modeloSia, string orden, string taller,
                                                    string fechaRetorno, int cantidad, string maquina, string linea,
                                                    string modeloSap, string po, string titulo, string material)
        {
            DataTable dtDetalleOP = _ordenProduccionBll.ListarDetalleOrdenProduccion(contrato, orden, modeloSia);
            DataTable dtPesoTalla = _ordenProduccionBll.ListarPesosPorTalla(contrato, orden);

            string rutaBarCode = GenerarImagenBarcode(orden, "OrdenProduccionCode");

            ReportDataSource rdsDetalle = new ReportDataSource();
            rdsDetalle.Name = "dsDetalleOrdenProduccion";
            rdsDetalle.Value = dtDetalleOP;

            ReportDataSource rdsPesoTalla = new ReportDataSource();
            rdsPesoTalla.Name = "dsPesosPorTalla";
            rdsPesoTalla.Value = dtPesoTalla;

            ReportParameter[] parametros = new ReportParameter[13];
            parametros[0] = new ReportParameter("asignadoa", taller);
            parametros[1] = new ReportParameter("fecharetorno", fechaRetorno);
            parametros[2] = new ReportParameter("numeroorden", orden.Trim());
            parametros[3] = new ReportParameter("contrato", contrato.ToString());
            parametros[4] = new ReportParameter("totalorden", cantidad.ToString());
            parametros[5] = new ReportParameter("maquina", maquina);
            parametros[6] = new ReportParameter("linea", linea);
            parametros[7] = new ReportParameter("codigosap", modeloSap);
            parametros[8] = new ReportParameter("po", po);
            parametros[9] = new ReportParameter("titulo", titulo);
            parametros[10] = new ReportParameter("material", material);
            parametros[11] = new ReportParameter("tipoorden", ddlCatOperacion.SelectedValue.ToString() == "400" ? "TEJIDO" : "CONFECCION");
            parametros[12] = new ReportParameter("barCodeParameter", rutaBarCode);

            rptViewOrdenProduccion.LocalReport.DataSources.Clear();
            rptViewOrdenProduccion.LocalReport.DataSources.Add(rdsDetalle);
            rptViewOrdenProduccion.LocalReport.DataSources.Add(rdsPesoTalla);
            rptViewOrdenProduccion.LocalReport.EnableExternalImages = true;
            rptViewOrdenProduccion.LocalReport.SetParameters(parametros);
            rptViewOrdenProduccion.LocalReport.Refresh();

            //Si no existe, creamos el documento
            string nombrepdf = ExportReportToPDF("OP-" + contrato.ToString() + "-" + orden.Trim() + "_"+ hidUsuario.Value, rptViewOrdenProduccion);
            if (nombrepdf != string.Empty)
            {
                //Cargamos el PDFViewer
                string server = ConfigurationManager.AppSettings["servidor"];
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento('" + nombrepdf + "', '" + server + "');", true);
                nombrepdf = @"C:\inetpub\wwwroot\PSIAA\Reports\Prod\" + nombrepdf + ".pdf";
            }
            else
            {
                //lblMensajeError.Visible = true;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento();", true);
                nombrepdf = string.Empty;
            }
            return nombrepdf;
        }

        private string GenerarDocumentoTarjetaProduccion(int contrato, string modeloSia, string orden, int lote,
                                                        string talla, string modeloSap, string po, int cantidad,
                                                        string titulo, string color)
        {
            string rutaBarCode = GenerarImagenBarcode(orden + "-" + MascaraLote(lote), "TarjetaProduccionCode");

            DataTable dtColores = _ordenProduccionBll.ListarColoresPorOrdenLote(contrato, modeloSia, orden, lote);
            DataTable dtMedidas = _ordenProduccionBll.ListarMedidasPorTalla(modeloSia, talla);
            List<string> lstComponentes = _ordenProduccionBll.ComponentesPorModelo(modeloSia);
            DataTable dtOperaciones = _ordenProduccionBll.ListarOperacion(modeloSia);

            //Generar reporte de Tarjeta de Produccion
            ReportDataSource rdsMedidas = new ReportDataSource();
            rdsMedidas.Name = "dsMedidasPorTalla";
            rdsMedidas.Value = dtMedidas;
            //poner en una variavle globar el source de medida
            Session["SourceMedida"] = rdsMedidas;

            ReportDataSource rdsColores = new ReportDataSource();
            rdsColores.Name = "dsCombinacion";
            rdsColores.Value = dtColores;

            ReportDataSource rdsOperaciones = new ReportDataSource();
            rdsOperaciones.Name = "dsOperaciones";
            rdsOperaciones.Value = dtOperaciones;
            Session["SourceOperaciones"] = rdsOperaciones;

            ReportParameter[] parametros = new ReportParameter[19];
            parametros[0] = new ReportParameter("nroorden", orden);
            parametros[1] = new ReportParameter("codigosap", modeloSap);
            parametros[2] = new ReportParameter("nropo", po);
            parametros[3] = new ReportParameter("contrato", contrato.ToString());
            parametros[4] = new ReportParameter("cliente", "-");
            parametros[5] = new ReportParameter("modelo", modeloSia);
            parametros[6] = new ReportParameter("lote", lote.ToString());
            parametros[7] = new ReportParameter("talla", talla);
            parametros[8] = new ReportParameter("cantidad", cantidad.ToString());
            parametros[9] = new ReportParameter("titulo", titulo);
            for (int i = 0; i <= 6; i++)
            {
                parametros[i + 10] = new ReportParameter("componente" + (i + 1), lstComponentes[i]);
            }
            parametros[17] = new ReportParameter("barCodeParameter", rutaBarCode);
            parametros[18] = new ReportParameter("color", color);

            rptViewTarjetaProduccion.LocalReport.DataSources.Clear();
            rptViewTarjetaProduccion.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
            rptViewTarjetaProduccion.LocalReport.DataSources.Add(rdsColores);
            rptViewTarjetaProduccion.LocalReport.EnableExternalImages = true;
            rptViewTarjetaProduccion.LocalReport.SetParameters(parametros);
            rptViewTarjetaProduccion.LocalReport.Refresh();

            //Si no existe, creamos el documento
            string nombrepdf = ExportReportToPDF("TP-" + contrato.ToString() + "-" + orden.Trim() + "_" + lote.ToString() + "_" + hidUsuario.Value, rptViewTarjetaProduccion);
            if (nombrepdf != string.Empty)
            {
                //Cargamos el PDFViewer
                string server = ConfigurationManager.AppSettings["servidor"];
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento('" + nombrepdf + "', '" + server + "');", true);
                nombrepdf = @"C:\inetpub\wwwroot\PSIAA\Reports\Prod\" + nombrepdf + ".pdf";
            }
            else
            {
                //lblMensajeError.Visible = true;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento();", true);
                nombrepdf = string.Empty;
            }
            return nombrepdf;
        }
        private string GenerarDocumentoOrdenRequisicion(int contrato, string orden, string taller, string fechaEmision,
                                                        string fechaSolicitada, string modelo, int cantidad, string maquina,
                                                        string modeloSap)
        {
            string rutaBarCode = GenerarImagenBarcode(orden.Trim(), "OrdenRequisicionCode");

            //Crear y mostrar PDF
            DataTable _dtDetalle = _ordenRequiBll.ListarDetalleOrdenRequision(contrato, orden);
            DataTable _dtCentroCostos2 = _ordenRequiBll.ListarCentroCostos(orden);

            ReportDataSource rdsDetalle = new ReportDataSource();
            rdsDetalle.Name = "dsRequisicionDetalle";
            rdsDetalle.Value = _dtDetalle;

            ReportDataSource rdsCentroCostos = new ReportDataSource();
            rdsCentroCostos.Name = "dsCentroCostos2";
            rdsCentroCostos.Value = _dtCentroCostos2;

            ReportParameter[] parametros = new ReportParameter[10];
            parametros[0] = new ReportParameter("barCodeParameter", rutaBarCode);
            parametros[1] = new ReportParameter("nombreTaller", taller);
            parametros[2] = new ReportParameter("fechaEmision", fechaEmision);
            parametros[3] = new ReportParameter("fechaSolicitada", fechaSolicitada);
            parametros[4] = new ReportParameter("contrato", contrato.ToString());
            parametros[5] = new ReportParameter("modelo", modelo);
            parametros[6] = new ReportParameter("cantidad", cantidad.ToString());
            parametros[7] = new ReportParameter("ordenProd", orden);
            parametros[8] = new ReportParameter("maquina", maquina);
            parametros[9] = new ReportParameter("modeloSap", modeloSap);

            rptViewOrdenReq.LocalReport.DataSources.Clear();
            rptViewOrdenReq.LocalReport.DataSources.Add(rdsDetalle);
            rptViewOrdenReq.LocalReport.DataSources.Add(rdsCentroCostos);
            rptViewOrdenReq.LocalReport.EnableExternalImages = true;
            rptViewOrdenReq.LocalReport.SetParameters(parametros);
            rptViewOrdenReq.LocalReport.Refresh();

            //Si no existe, creamos el documento
            string nombrepdf = ExportReportToPDF("OR-" + contrato.ToString() + "-" + orden.Trim() + "_" + hidUsuario.Value, rptViewOrdenReq);
            if (nombrepdf != string.Empty)
            {
                //Cargamos el PDFViewer
                string server = ConfigurationManager.AppSettings["servidor"];
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento('" + nombrepdf + "', '" + server + "');", true);
                nombrepdf = @"C:\inetpub\wwwroot\PSIAA\Reports\Prod\" + nombrepdf + ".pdf";
            }
            else
            {
                //lblMensajeError.Visible = true;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CargarDocumento();", true);
                nombrepdf = string.Empty;
            }
            return nombrepdf;
        }

        private string ExportReportToPDF(string reportName, ReportViewer report)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            try
            {
                byte[] bytes = report.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension,
                                                            out streamids, out warnings);

                string filename = @"C:\inetpub\wwwroot\PSIAA\Reports\Prod\" + reportName + ".pdf";
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

        private void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(Session["SourceMedida"] as ReportDataSource);
            e.DataSources.Add(Session["SourceOperaciones"] as ReportDataSource);
        }

        private string MascaraLote(int _nroLote)
        {
            string cadena = "000";
            int largoId = _nroLote.ToString().Length;
            return cadena.Substring(0, cadena.Length - largoId) + _nroLote.ToString();
        }

        private string GenerarImagenBarcode(string valor, string nombreArchivo)
        {
            Barcode codigo = new Barcode();
            codigo.IncludeLabel = true;
            System.Drawing.Image imgBarcode = codigo.Encode(TYPE.CODE39, valor,
                                                            Color.Black, Color.White, 550, 130);

            string rutaImagen = @"C:\inetpub\wwwroot\PSIAA\Reports\BarCodes\" + nombreArchivo + ".png";
            imgBarcode.Save(rutaImagen, ImageFormat.Png);
            if (File.Exists(rutaImagen))
            {
                return rutaImagen;
            }
            else
            {
                return string.Empty;
            }
        }

        protected void btnGenerarOrdenProduccion_Click(object sender, EventArgs e)
        {
            List<string> documentos = new List<string>();
            linkOrdenProd.Text = string.Empty;
            linkOrdenProd.Visible = false;
            foreach (GridViewRow row in gridOrdenesLanzadasAsignadas.Rows)
            {
                bool check = ((CheckBox)row.FindControl("chkSeleccionar")).Checked;
                if (check) {
                    //Cargar datos de grid
                    string orden = row.Cells[1].Text.Trim();
                    string modeloSia = row.Cells[2].Text;
                    string modeloSap = row.Cells[3].Text;
                    string taller = row.Cells[4].Text;
                    int cantidad = int.Parse(row.Cells[5].Text);
                    string fechaRetorno = row.Cells[10].Text;
                    string fechaEmision = DateTime.Now.ToShortDateString();

                    //Cargar Detalle de Modelo de contrato
                    DataRow filaModelo = _contratoBll.ListarDetalleModeloContrato(int.Parse(hidContrato.Value), modeloSia).Rows[0];
                    string maquina = filaModelo["Maquina"].ToString();
                    string nroPo = filaModelo["numero_p_o"].ToString();
                    string material = filaModelo["Material_AA"].ToString();
                    string linea = filaModelo["Linea"].ToString();
                    string titulo = filaModelo["Titulo"].ToString();

                    documentos.Add(GenerarDocumentoOrdenProduccion(int.Parse(hidContrato.Value), modeloSia, orden, taller,
                                                                   fechaRetorno, cantidad, maquina, linea, modeloSap, nroPo,
                                                                   titulo, material));
                }
            }
            if (documentos.Count > 0)
            {
                string docFinal = @"C:\inetpub\wwwroot\PSIAA\Reports\Prod\OrdenProduccion_" + hidContrato.Value + ".pdf";
                documentos.Sort((a, b) => a.CompareTo(b));
                Helpers.PdfMerger.MergeFiles(documentos, docFinal, chkAgregarCopiaOP.Checked);
                if (File.Exists(docFinal))
                {
                    string server = ConfigurationManager.AppSettings["servidor"];
                    linkOrdenProd.NavigateUrl = "http://" + server + "/PSIAA/Reports/Prod/OrdenProduccion_" + hidContrato.Value + ".pdf";
                    linkOrdenProd.Text = "OrdenProduccion_" + hidContrato.Value + "_" + hidUsuario.Value + ".pdf";
                    linkOrdenProd.Visible = true;
                    EliminarDocumentosUnitarios(documentos);
                    lblAvisoSeleccion.Visible = false;
                }
            }
            else
                lblAvisoSeleccion.Visible = true;
        }

        protected void btnGenerarTarjetaProduccion_Click(object sender, EventArgs e)
        {
            List<string> documentos = new List<string>();
            linkTarjetaProd.Text = string.Empty;
            linkTarjetaProd.Visible = false;
            foreach (GridViewRow row in gridOrdenesLanzadasAsignadas.Rows)
            {
                bool check = ((CheckBox)row.FindControl("chkSeleccionar")).Checked;
                if (check) {
                    //Cargar datos de grid
                    string orden = row.Cells[1].Text.Trim();
                    string modeloSia = row.Cells[2].Text;
                    string modeloSap = row.Cells[3].Text;

                    //Cargar Detalle de Modelo de contrato
                    DataRow filaModelo = _contratoBll.ListarDetalleModeloContrato(int.Parse(hidContrato.Value), modeloSia).Rows[0];
                    string nroPo = filaModelo["numero_p_o"].ToString();

                    string titulo = filaModelo["Titulo"].ToString();

                    DataTable dtDetOrdenesProd = _ordenProduccionBll.ListarDetalleOrdenesProduccion(int.Parse(hidContrato.Value), orden);

                    foreach (DataRow dr in dtDetOrdenesProd.Rows)
                    {
                        int lote = int.Parse(dr["Lote"].ToString());
                        string color = dr["Color"].ToString();
                        int cantidad = int.Parse(dr["Cantidad"].ToString());
                        string talla = dr["Talla"].ToString();
                        talla = talla == "ONE SI" ? "ONE SIZE" : talla;
                        documentos.Add(GenerarDocumentoTarjetaProduccion(int.Parse(hidContrato.Value), modeloSia, orden, lote, talla, modeloSap,
                                                                        nroPo, cantidad, titulo, color));
                    }
                } 
            }
            if (documentos.Count > 0)
            {
                string docFinal = @"C:\inetpub\wwwroot\PSIAA\Reports\Prod\TarjetaProduccion_" + hidContrato.Value + ".pdf";
                documentos.Sort((a, b) => a.CompareTo(b));
                Helpers.PdfMerger.MergeFiles(documentos, docFinal, chkAgregarCopiaTP.Checked);
                if (File.Exists(docFinal))
                {
                    string server = ConfigurationManager.AppSettings["servidor"];
                    linkTarjetaProd.NavigateUrl = "http://" + server + "/PSIAA/Reports/Prod/TarjetaProduccion_" + hidContrato.Value + ".pdf";
                    linkTarjetaProd.Text = "TarjetaProduccion_" + hidContrato.Value + "_" + hidUsuario.Value + ".pdf";
                    linkTarjetaProd.Visible = true;
                    EliminarDocumentosUnitarios(documentos);
                    lblAvisoSeleccion.Visible = false;
                }
            }
            else
                lblAvisoSeleccion.Visible = true;
        }

        protected void btnGenerarOrdenRequisicion_Click(object sender, EventArgs e)
        {
            List<string> documentos = new List<string>();
            linkOrdenReq.Text = string.Empty;
            linkOrdenReq.Visible = false;
            foreach (GridViewRow row in gridOrdenesLanzadasAsignadas.Rows)
            {
                bool check = ((CheckBox)row.FindControl("chkSeleccionar")).Checked;
                if (check) {
                    string orden = row.Cells[1].Text.Trim();
                    string modeloSia = row.Cells[2].Text.Trim();
                    string modeloSap = row.Cells[3].Text.Trim();
                    string taller = row.Cells[4].Text;
                    int cantidad = int.Parse(row.Cells[5].Text);
                    string fechaIngreso = row.Cells[9].Text;
                    string fechaTermino = row.Cells[10].Text;
                    string maquina = row.Cells[11].Text;
                    documentos.Add(GenerarDocumentoOrdenRequisicion(int.Parse(hidContrato.Value), orden, taller, fechaIngreso, 
                                                                    fechaTermino, modeloSia, cantidad, maquina, modeloSap));
                }
            }
            if (documentos.Count > 0)
            {
                string docFinal = @"C:\inetpub\wwwroot\PSIAA\Reports\Prod\OrdenRequisicion_" + hidContrato.Value + ".pdf";
                documentos.Sort((a, b) => a.CompareTo(b));
                Helpers.PdfMerger.MergeFiles(documentos, docFinal, chkAgregarCopiaOR.Checked);
                if (File.Exists(docFinal))
                {
                    string server = ConfigurationManager.AppSettings["servidor"];
                    linkOrdenReq.NavigateUrl = "http://" + server + "/PSIAA/Reports/Prod/OrdenRequisicion_" + hidContrato.Value + ".pdf";
                    linkOrdenReq.Text = "OrdenRequisicion_" + hidContrato.Value + "_" + hidUsuario.Value + ".pdf";
                    linkOrdenReq.Visible = true;
                    EliminarDocumentosUnitarios(documentos);
                    lblAvisoSeleccion.Visible = false;
                }
            }
            else
                lblAvisoSeleccion.Visible = true;
        }

        private void EliminarDocumentosUnitarios(List<string> documentos) {
            foreach (string doc in documentos) {
                if (File.Exists(doc))
                    File.Delete(doc);
            }
        }
    }
}