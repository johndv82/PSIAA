using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.DataTransferObject;
using System.Data;
using System.Globalization;

namespace PSIAA.Presentation.View
{
    public partial class GeneracionDocumento : System.Web.UI.Page
    {
        private AsignacionOrdenesBLL _asignacionOrdenesBll = new AsignacionOrdenesBLL();
        private DocumentoPagoTallerBLL _docPagoTallerBll = new DocumentoPagoTallerBLL();
        private HttpCookie cookie;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cookie = Request.Cookies["Usuario"];
                if (cookie != null)
                {
                    hidUsuario.Value = cookie["Nombre"].ToString();
                    txtCodProveedor.Focus();
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
            System.Globalization.Calendar c = CultureInfo.CurrentCulture.Calendar;
            var semana = c.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Sunday).ToString();
            lblFechaEmision.Text = DateTime.Now.ToShortDateString();
            lblSemana.Text = semana;
            //Evaluar si hay datos en la sesion de FilasAfectadas
            if (Session["NroLiquidacion"] != null)
            {
                int nroLiquid = int.Parse(Session["NroLiquidacion"].ToString());
                if (nroLiquid > 0)
                {
                    //Mostrar mensaje de registros ingresados
                    lblRespuesta.Text = "Número de Liquidacion Generado: "+ nroLiquid.ToString();
                    lblRespuesta.ForeColor = System.Drawing.Color.Green;
                    lblRespuesta.Visible = true;
                }
                else
                {
                    lblRespuesta.Text = "Hubo un error al ingresar registros de Facturación";
                    lblRespuesta.ForeColor = System.Drawing.Color.Red;
                    lblRespuesta.Visible = true;
                }
                Session["NroLiquidacion"] = null;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(hidCodProveedor.Value) && !string.IsNullOrWhiteSpace(txtFechaAprobPrecio.Text))
            {
                string _proveedor = hidCodProveedor.Value.ToString();

                DataTable dtAsignacionesDet = _asignacionOrdenesBll.ListarAsignaciones(_proveedor, ddlMoneda.SelectedValue, txtFechaAprobPrecio.Text);
                gridDetalleAsignaciones.DataSource = dtAsignacionesDet;
                gridDetalleAsignaciones.DataBind();
                lblNRegistros.Text = "N° de Registros: " + dtAsignacionesDet.Rows.Count.ToString();
            }
            lblMensajeError.Visible = false;
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            int itemsCheckeados = 0;
            List<DocumentoPagoTallerDTO> _listDocPagoTaller = new List<DocumentoPagoTallerDTO>();

            //Recorrer el grid y poblar DTO DocumentoPagoTaller con los items chekeados
            foreach (GridViewRow item in gridDetalleAsignaciones.Rows)
            {
                bool check = ((CheckBox)item.FindControl("chkSeleccionar")).Checked;
                if (check)
                {
                    itemsCheckeados++;
                    //Buscar Detalle/Procesos de Asignacion Agrupada
                    DataTable dtDetalle = _asignacionOrdenesBll.ListarDetalleProcesoAsignacionOrdenes(hidCodProveedor.Value,
                                                                item.Cells[2].Text, item.Cells[3].Text,
                                                                int.Parse(item.Cells[4].Text),
                                                                int.Parse(item.Cells[5].Text));

                    foreach (DataRow row in dtDetalle.Rows)
                    {
                        DocumentoPagoTallerDTO _docTaller = new DocumentoPagoTallerDTO()
                        {
                            //CodProveedor = hidCodProveedor.Value.ToString(),
                            CodProveedor = row["Cod_Proveedor"].ToString(),
                            TipoDocumento = ddlTipoDocs.SelectedValue,
                            SerieDocumento = 0,
                            //NroDocumento -> NroLiquidacion (generado en BLL)
                            //NroDocumento = int.Parse(lblNumeroDoc.Text),
                            CategoriaOperacion = int.Parse(row["Categoria_Operacion"].ToString()),
                            NumOrdenAsignacion = row["Numero_Orden"].ToString(),
                            Orden = row["Orden"].ToString(),
                            Lote = int.Parse(row["Lote"].ToString()),
                            Categoria = int.Parse(row["Categoria"].ToString()),
                            CodProceso = int.Parse(row["Proceso"].ToString()),
                            MontoFacturacionSoles = double.Parse(row["Costo_Soles"].ToString()),
                            MontoFacturacionDolares = double.Parse(row["Costo_Dolares"].ToString())
                        };
                        _listDocPagoTaller.Add(_docTaller);
                    }
                }
            }

            if (itemsCheckeados > 0)
            {
                //Ingresar DocumentoPagoTaller
                int nroLiquidacion = _docPagoTallerBll.IngresarDocumentoPagoTaller(_listDocPagoTaller, ddlMoneda.SelectedValue, hidUsuario.Value);
                Session["NroLiquidacion"] = nroLiquidacion;
                Response.Redirect("GeneracionDocumento.aspx");
            }
            else
            {
                lblMensajeError.Visible = true;
            }
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
            txtNombreProveedor.Text = ((Button)row.FindControl("btnNombreComercial")).Text.Trim();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CloseModalPorveedores();", true);
            btnBuscar.Visible = true;
            btnGrabar.Visible = true;
            lblRespuesta.Visible = false;
            txtNombreProveedor.ForeColor = System.Drawing.Color.Black;
            hidCodProveedor.Value = txtCodProveedor.Text;
        }

        protected void btnBuscarPorCod_Click(object sender, EventArgs e)
        {
            string respuesta = _docPagoTallerBll.DevolverNombreProveedor(txtCodProveedor.Text);
            if (respuesta == string.Empty)
            {
                txtNombreProveedor.ForeColor = System.Drawing.Color.Red;
                txtNombreProveedor.Text = "ERROR, Proveedor no encontrado";
                btnBuscar.Visible = false;
                btnGrabar.Visible = false;
            }
            else
            {
                txtNombreProveedor.ForeColor = System.Drawing.Color.Black;
                txtNombreProveedor.Text = respuesta;
                hidCodProveedor.Value = txtCodProveedor.Text;
                btnBuscar.Visible = true;
                btnGrabar.Visible = true;
            }
            lblRespuesta.Visible = false;
        }

        protected void txtNombreComercial_TextChanged(object sender, EventArgs e)
        {
            DataView dvFiltro = ((DataView)Session["ListadoProveedores"]);
            dvFiltro.RowFilter = "nombre_comercial LIKE '%" + txtNombreComercial.Text + "%'";
            Session["ListadoProveedores"] = dvFiltro;
            gridProveedores.DataSource = dvFiltro.ToTable();
            gridProveedores.DataBind();
        }

        protected void gridDetalleAsignaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridDetalleAsignaciones.SelectedRow;
            string asignacion = row.Cells[2].Text;
            string orden = row.Cells[3].Text.Trim();
            int lote = int.Parse(row.Cells[4].Text);
            int categoria = int.Parse(row.Cells[5].Text);
            string modelo = row.Cells[6].Text.Trim();

            gridProcesosAsignacion.DataSource = _docPagoTallerBll.ListarProcesosAsignadosPorOrdenLote(hidCodProveedor.Value,
                                                                                                        asignacion, orden, lote,
                                                                                                        categoria, modelo);
            gridProcesosAsignacion.DataBind();
            lblAsignacion.Text = asignacion;
            lblOrdenLote.Text = orden + "/" + lote;
            lblCategoria.Text = categoria.ToString();
            lblModelo.Text = modelo;
        }
    }
}