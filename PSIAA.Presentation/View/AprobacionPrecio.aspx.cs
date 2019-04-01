using PSIAA.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.DataTransferObject;

namespace PSIAA.Presentation.View
{
    public partial class AprobacionPrecio : System.Web.UI.Page
    {
        /// <summary>
        /// Variable de instancia a la clase DocumentoPagoTallerBLL.
        /// </summary>
        public DocumentoPagoTallerBLL _docPagoTallerBll = new DocumentoPagoTallerBLL();
        /// <summary>
        /// Variable de instancia a la clase AsignacionOrdenesBLL.
        /// </summary>
        public AsignacionOrdenesBLL _asigOrdenesBll = new AsignacionOrdenesBLL();
        /// <summary>
        /// Variable de instancia a la clase AprobacionPrecioBLL.
        /// </summary>
        public AprobacionPrecioBLL _aprobPrecioBll = new AprobacionPrecioBLL();
        private double[] totalDetalle = { 0, 0, 0 };
        /// <summary>
        /// Variable global que almacena el usuario actual logueado.
        /// </summary>
        public string usuarioActual = string.Empty;

        /// <summary>
        /// Evento de Carga Principal del formulario AprobacionPrecio.aspx
        /// </summary>
        /// <remarks>
        /// En este evento se evalúa la existencia de la sesión del usuario y tambien capturamos su valor en una variable publica,
        /// para su posterior uso.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] != null)
            {
                usuarioActual = ((UsuarioDTO)Session["usuario"]).User;
                txtCodProveedor.Focus();
                lblMensajeError.Visible = false;

                if (!IsPostBack)
                {
                    /*
                     * Diferente a Post y Back
                     * Todo lo que se ejecutará al recargar la pagina
                     * Cuando se acciona un botón llamamos Post
                     * Cuando usamos el botón Atras del Navegador llamamos Back
                     */
                    hidOrden.Value = string.Empty;
                    hidLote.Value = "0";
                }
            }
        }

        /// <summary>
        /// Evento Click del botón btnBuscarPorNombre
        /// </summary>
        /// <remarks>
        /// Llama al método BLL de Listar Proveedores y el resultado lo carga en una variable Session 
        /// con el nombre de: ListadoProveedores, y en el source de la grilla gridProveedores.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnBuscarPorNombre_Click(object sender, EventArgs e)
        {
            DataView dvProveedores = new DataView(_docPagoTallerBll.ListarProveedores());
            Session["ListadoProveedores"] = dvProveedores;
            gridProveedores.DataSource = dvProveedores.ToTable();
            gridProveedores.DataBind();
            txtNombreComercial.Focus();
        }

        /// <summary>
        /// Evento Cambio de Página de la grilla gridProveedores.
        /// </summary>
        /// <remarks>
        /// Carga el valor de la variable de sesión ListadoProveedores en la grilla gridProveedores, y actualiza su indice de página.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void gridProveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProveedores.DataSource = ((DataView)Session["ListadoProveedores"]).ToTable();
            gridProveedores.PageIndex = e.NewPageIndex;
            gridProveedores.DataBind();
        }

        /// <summary>
        /// Evento Cambio de Selección de la grilla gridProveedores.
        /// </summary>
        /// <remarks>
        /// Obtiene el Código de Proveedor de la fila seleccionada en la grilla, y ejecuta un procedimiento BLL de Listar
        /// Modelos Asignados por Taller, y con el resultado cargar la Lista desplegable ddlModelos.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void gridProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridProveedores.SelectedRow;
            txtCodProveedor.Text = row.Cells[0].Text.Trim();
            hidCodProveedor.Value = txtCodProveedor.Text;
            txtNombreProveedor.Text = ((Button)row.FindControl("btnNombreComercial")).Text.Trim();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CloseModalPorveedores();", true);
            btnBuscar.Visible = true;
            txtNombreProveedor.ForeColor = System.Drawing.Color.Black;
            //Cargar Combo de Modelos
            ddlModelos.DataSource = _asigOrdenesBll.ListarGrupoModelos(hidCodProveedor.Value);
            ddlModelos.DataBind();
        }

        /// <summary>
        /// Evento Click del botón btnBuscarPorCod.
        /// </summary>
        /// <remarks>
        /// En este evento se ejecuta un procedimiento BLL de Nombre de Proveedor por Código, y el resultado, en el caso no sea nulo,
        /// es establecido como texto del control txtNombreProveedor. Luego, con el código de proveedor se ejecuta otro procedimiento BLL
        /// de Listar Modelos Asignados por Taller, y son cargados en la lista desplegable ddlModelos.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnBuscarPorCod_Click(object sender, EventArgs e)
        {
            string respuesta = _docPagoTallerBll.DevolverNombreProveedor(txtCodProveedor.Text);
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
                //Cargar Combo de Modelos
                ddlModelos.DataSource = _asigOrdenesBll.ListarGrupoModelos(hidCodProveedor.Value);
                ddlModelos.DataBind();
            }
        }

        /// <summary>
        /// Evento Click del botón btnBuscar.
        /// </summary>
        /// <remarks>
        /// En este evento se ejecuta un procedimiento BLL de Asignaciones por Aprobar, enviando proveedor y modelo, 
        /// y se carga el resultado en la grilla gridAsignacionesAprobacion
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string modelo = ddlModelos.SelectedIndex == 0 ? "" : ddlModelos.SelectedValue;
            gridAsignacionesAprobacion.DataSource = _asigOrdenesBll.ListarAsignacionesParaAprobar(hidCodProveedor.Value, modelo.Trim());
            gridAsignacionesAprobacion.DataBind();
        }

        /// <summary>
        /// Evento Cambio de Selección de la grilla gridAsignacionesAprobacion.
        /// </summary>
        /// <remarks>
        /// En este evento se obtienen los datos de fila en la grilla gridAsignacionesAprobacion y se poblan las etiquetas
        /// del modal modalActualizacionPrecio.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void gridAsignacionesAprobacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridAsignacionesAprobacion.SelectedRow;

            lblModelo.Text = row.Cells[2].Text.Trim();
            lblAsignacion.Text = row.Cells[1].Text;

            //Orden/Lote
            hidOrden.Value = row.Cells[3].Text.Trim();
            hidLote.Value = row.Cells[4].Text;
            lblOrdenLote.Text = hidOrden.Value + "/" + hidLote.Value;

            //Categoria Operacion
            string catOpe = row.Cells[0].Text;
            hidCatOpe.Value = catOpe;

            //Cantidad
            hidCantidad.Value = row.Cells[5].Text;
            lblCantidad.Text = row.Cells[5].Text;
            string moneda = row.Cells[6].Text;
            ddlMoneda.SelectedIndex = moneda == "Soles" ? 0 : 1;
            txtTarifa.Text = moneda == "Soles" ? row.Cells[7].Text : row.Cells[8].Text;

            chkAprobado.Checked = (row.BackColor == System.Drawing.Color.FromArgb(226, 239, 218));
            btnActualizarCostos_Click(sender, e);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int nroActTarifa = 0;
            int nroActPrecio = 0;
            var _listProcesoPrecio = Session["listProcesoPrecio"] as List<ProcesoPrecioDTO>;
            //Actualizar Tarifa y Tiempo (Java)
            nroActTarifa = _asigOrdenesBll.ActualizarTarifaTiempo(hidCodProveedor.Value, int.Parse(hidCatOpe.Value), lblAsignacion.Text, 
                                                            ddlMoneda.SelectedValue.ToString(), _listProcesoPrecio, hidOrden.Value, int.Parse(hidLote.Value));

            //Calcular y Actualizar Costos
            nroActPrecio = _asigOrdenesBll.RecalcularPrecios(hidCodProveedor.Value, int.Parse(hidCatOpe.Value), lblAsignacion.Text,
                                                            ddlMoneda.SelectedValue.ToString(), txtFechaAprob.Text, usuarioActual,
                                                            chkAprobado.Checked, hidOrden.Value, int.Parse(hidLote.Value));

            if (nroActTarifa > 0 && nroActPrecio > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CloseModalPrecios();", true);
                btnBuscar_Click(sender, e);
            }
            else {
                lblMensajeError.Visible = true;
            }
        }

        protected void btnActualizarCostos_Click(object sender, EventArgs e)
        {
            List<ProcesoPrecioDTO> listProcPre = _aprobPrecioBll.ListarPreciosDeProcesos(hidCodProveedor.Value,
                                                                        lblModelo.Text,
                                                                        int.Parse(hidCantidad.Value.ToString()),
                                                                        ddlMoneda.SelectedValue.ToString(),
                                                                        lblAsignacion.Text,
                                                                        double.Parse(txtTarifa.Text),
                                                                        hidOrden.Value, int.Parse(hidLote.Value));
            gridPreciosProcesos.DataSource = listProcPre;
            gridPreciosProcesos.DataBind();
            if (listProcPre.Count > 0)
            {
                Session["listProcesoPrecio"] = listProcPre;
                //btnGuardar.Visible = true;
            }
            else {
                //btnGuardar.Visible = false;
            }
        }

        protected void gridAsignacionesAprobacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string _estado = DataBinder.Eval(e.Row.DataItem, "Precio_Aprobado").ToString();
                string _moneda = DataBinder.Eval(e.Row.DataItem, "Moneda").ToString();
                string _usuarioAprob = DataBinder.Eval(e.Row.DataItem, "Usuario_Aprob_Prec").ToString();

                e.Row.Cells[6].Text = _moneda == "S" ? "Soles" : "Dolares";
                e.Row.Cells[14].Text = _usuarioAprob == "" ? "" : e.Row.Cells[14].Text;

                if (_estado == "True")
                    e.Row.BackColor = System.Drawing.Color.FromArgb(226, 239, 218); //Verde
                else
                    e.Row.BackColor = System.Drawing.Color.FromArgb(252, 228, 214); //Rojo
            }
        }

        protected void gridPreciosProcesos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowType != DataControlRowType.EmptyDataRow))
            {
                totalDetalle[0] += e.Row.Cells[2].Text == "" ? 0 : double.Parse(e.Row.Cells[2].Text);
                totalDetalle[1] += e.Row.Cells[3].Text == "" ? 0 : double.Parse(e.Row.Cells[3].Text);
                totalDetalle[2] += e.Row.Cells[4].Text == "" ? 0 : double.Parse(e.Row.Cells[4].Text);

                //Evaluar si hay procesos mal asignados
                string catOper = DataBinder.Eval(e.Row.DataItem, "CategoriaOperacion").ToString();
                int codPro = int.Parse(DataBinder.Eval(e.Row.DataItem, "Proceso").ToString());
                if (catOper == "0") {
                    e.Row.ForeColor = System.Drawing.Color.Red; //Letra Roja
                    e.Row.Cells[1].Text = _aprobPrecioBll.DescripcionProcesoJava(codPro) + " " + e.Row.Cells[1].Text;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = totalDetalle[0].ToString("0,0.00");
                e.Row.Cells[3].Text = totalDetalle[1].ToString("0,0.00");
                e.Row.Cells[4].Text = totalDetalle[2].ToString("0,0.00");
                e.Row.Font.Bold = true;
                e.Row.Font.Size = FontUnit.Medium;
                if (totalDetalle[0] == 0)
                {
                    btnGuardar.Visible = false;
                }
                else {
                    btnGuardar.Visible = true;
                }
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