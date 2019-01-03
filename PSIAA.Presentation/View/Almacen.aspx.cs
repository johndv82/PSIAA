using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using System.Data;
using System.Drawing;
using PSIAA.DataTransferObject;

namespace PSIAA.Presentation.View
{
    public partial class Almacen : System.Web.UI.Page
    {
        /// <summary>
        /// Variable de llamada a la clase BLL AlmacenBLL.
        /// </summary>
        public AlmacenBLL _almacenBll = new AlmacenBLL();
        /// <summary>
        /// Variable de llamada a la clase BLL RecepcionControlBLL.
        /// </summary>
        public RecepcionControlBLL _recepcionBll = new RecepcionControlBLL();
        /// <summary>
        /// Variable de llamada al helper ListXml.
        /// </summary>
        public ListXml _listXml = new ListXml();
        /// <summary>
        /// Variable publica para almacenar el usuario logueado.
        /// </summary>
        public string usuarioActual = string.Empty;

        /// <summary>
        /// Evento de Carga Principal del formulario Almacen.aspx
        /// </summary>
        /// <remarks>
        /// En este evento se evalúa la existencia de la sesión del usuario y tambien capturamos su valor en una variable publica,
        /// para su posterior uso.
        /// Se cargan los ingresos recepcionados devuelto por el procedimiento BLL ListarRecepcionControl.
        /// Se cargan el listado de almacenes devuelto por el procedimiento BLL ListarAlmacenes.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] != null) {
                usuarioActual = ((UsuarioDTO)Session["usuario"]).User;

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

                    //Cargar Datos de Recepcion Control de Almacen
                    gridControlFinal.DataSource = _almacenBll.ListarRecepcionControl(usuarioActual);
                    gridControlFinal.DataBind();

                    //Cargar Almacenes
                    cmbAlmacenes.DataSource = _almacenBll.ListarAlmacenes();
                    cmbAlmacenes.DataBind();
                    txtOrden.Focus();
                }
            } 
        }

        /// <summary>
        /// Evento Click del botón btnVerDetalle.
        /// </summary>
        /// En este evento se pobla los controles: txtPiezas, lblTalla, lblModelo y lblColor, llamando al procedimiento BLL 
        /// de Listar Campos de Recepcion Control.
        /// <remarks>
        /// </remarks>
        /// <param name="sender">Objeto llamador del evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnVerDetalle_Click(object sender, EventArgs e)
        {
            lblErrorRegDupli.Visible = false;
            if (!string.IsNullOrWhiteSpace(txtOrden.Text))
            {
                if (txtOrden.Text.IndexOf('/') > 0)
                {
                    hidLote.Value = txtOrden.Text.Split('/')[1].ToString().Trim();
                    hidOrden.Value = txtOrden.Text.Split('/')[0].ToString().Trim();
                }
                else if (txtOrden.Text.IndexOf((char)39) > 0)
                {
                    hidLote.Value = txtOrden.Text.Split((char)39)[1].ToString().Trim();
                    hidOrden.Value = txtOrden.Text.Split((char)39)[0].ToString().Trim();
                }
                else
                {
                    hidLote.Value = "0";
                    hidOrden.Value = string.Empty;
                }
                if (!string.IsNullOrEmpty(hidOrden.Value) & hidLote.Value != "0")
                {
                    try
                    {
                        IDictionary<string, string> _campo = _recepcionBll.ListarCamposRecepcionControl(hidOrden.Value, int.Parse(hidLote.Value));
                        lblMensajeOk.Visible = false;
                        lblMensajeError.Visible = false;
                        btnAgregar.Visible = true;
                        txtPiezas.Text = _campo["Cantidad"];
                        lblTalla.Text = _campo["Talla"];
                        lblModelo.Text = _campo["Modelo"];
                        lblColor.Text = _campo["Color"];
                    }
                    catch (Exception exc)
                    {
                        lblMensajeError.Visible = true;
                        btnAgregar.Visible = false;
                        lblValidacion.Visible = false;
                        txtPiezas.Text = "0";
                        lblTalla.Text = "XXX";
                        lblModelo.Text = "XXXXXXX";
                        lblColor.Text = "XXXXXXX";
                        Console.WriteLine(exc.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Evento Click del botón btnAgregar
        /// </summary>
        /// <remarks>
        /// En este evento se invoca al procedimiento BLL de Poblar Listas Ingresos, enviando los parametros de:
        /// Orden, Lote, Codigo de Almacén, Piezas, Talla y Nombre de Usuario, previamente validando la diferencia 
        /// de piezas ingresadas con el punto anterior, en el caso de que el usuario sea : "muestras", se omite la validación.
        /// Si la acción es exitosa o hay problema con las piezas ingresadas, se muestra un mensaje al usuario.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            int _pieza;

            if (int.TryParse(txtPiezas.Text == "" ? "0" : txtPiezas.Text, out _pieza) & hidOrden.Value != string.Empty & hidLote.Value != string.Empty)
            {
                int _diferencia = _recepcionBll.DiferenciaConPuntoAnterior(hidOrden.Value, int.Parse(hidLote.Value), 800, 550);
                if ((usuarioActual == "muestras"))
                {
                    _diferencia = _pieza;
                }
                if ((_diferencia > 0) & (_pieza <= _diferencia))
                {
                    lblValidacion.Visible = false;
                    int _codigoAlmacen = int.Parse(cmbAlmacenes.SelectedValue);
                    if (_codigoAlmacen != 0 & !string.IsNullOrWhiteSpace(txtOrden.Text) & _pieza > 0)
                    {
                        bool registroDuplicado;
                        gridControlFinal.DataSource = _almacenBll.PoblarListasDeIngresoAlmacen(hidOrden.Value, int.Parse(hidLote.Value), _codigoAlmacen,
                                                                                        _pieza, lblTalla.Text, usuarioActual, out registroDuplicado);
                        gridControlFinal.DataBind();
                        btnGuardarIngreso.Visible = true;
                        lblParte.Visible = false;
                        if (registroDuplicado)
                        {
                            lblErrorRegDupli.Visible = true;
                        }
                        else
                        {
                            txtOrden.Text = string.Empty;
                            txtOrden.Focus();
                            txtPiezas.Text = "0";
                            lblTalla.Text = "XXX";
                            lblModelo.Text = "XXXXXXX";
                            lblColor.Text = "XXXXXXX";
                            btnAgregar.Visible = false;
                        }
                    }
                }
                else
                {
                    lblValidacion.Text = "La cantidad de prendas permitidas es: " + _diferencia.ToString();
                    lblValidacion.Visible = true;
                }
            }
        }

        /// <summary>
        /// Evento Cambio de Seleccion de la lista deplegable cmbAlmacenes
        /// </summary>
        /// <remarks>
        /// Obtiene el valor de la lista desplegable cmbAlmacenes y lo asigna a la etiqueta lblNumAlmacen
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void cmbAlmacenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNumAlmacen.Text = cmbAlmacenes.SelectedValue.ToString();
        }

        /// <summary>
        /// Evento Click del botón btnGuardar
        /// </summary>
        /// <remarks>
        /// En este evento se llama al procedimiento BLL de Ingresar detalle de almacen, retornando en un parametro de salida,
        /// el número de parte. Tambien se pobla un control GridView con el listado de Recepciones de Control de Almacen.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnGuardarIngreso_Click(object sender, EventArgs e)
        {
            string NumeroParte = "";
            if (gridControlFinal.Rows.Count > 0)
            {
                if (_almacenBll.IngresarDetalleAlmacen(out NumeroParte, usuarioActual))
                {
                    lblMensajeOk.Visible = true;
                    lblErrorRegDupli.Visible = false;
                    lblParte.Text = string.Empty;
                    lblParte.Text += "Numero de Parte: " + NumeroParte;
                    lblParte.Visible = true;
                    btnGuardarIngreso.Visible = false;
                    btnLimpiar.Visible = false;
                    gridControlFinal.DataSource = _almacenBll.ListarRecepcionControl(usuarioActual);
                    gridControlFinal.DataBind();

                    //LIMPIAR CAMPOS
                    btnAgregar.Visible = false;
                    txtPiezas.Text = "0";
                    lblTalla.Text = "XXX";
                    lblModelo.Text = "XXXXXXX";
                    lblColor.Text = "XXXXXXX";
                    cmbAlmacenes.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Evento Click del botón btnLimpiar
        /// </summary>
        /// <remarks>
        /// En este evento se llama al procedimiento BLL de Limpiar listas de control, y redirigir la respuesta la página de Almacen.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            _almacenBll.LimpiarListasDeControl(usuarioActual);
            Response.Redirect("Almacen.aspx");
        }

        /// <summary>
        /// Evento Click del botón btnConsultarAvance.
        /// </summary>
        /// <remarks>
        /// En este evento se llama al procedimiento de Listar Seguimiento de Recepcion para cargarlo en un GridView.
        /// </remarks>
        /// <param name="sender">Objeto llamador del evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnConsultarAvance_Click(object sender, EventArgs e)
        {
            gridSeguimientoOrden.DataSource = _almacenBll.ListarSeguimientoRecepcionControl(hidOrden.Value);
            gridSeguimientoOrden.DataBind();
        }

        /// <summary>
        /// Evento de Cambio de Selección del GridView gridControlFinal
        /// </summary>
        /// <remarks>
        /// En este evento se obtienen los valores de Orden de Produción y Npumero de Lote del GridView gridControlFinal, para consultar 
        /// su detalle llamando al procedimiento BLL Detalle de Almacen y mostrar cada uno de sus campos en Etiquetas.
        /// </remarks>
        /// <param name="sender">Objeto llamador del evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void gridControlFinal_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridControlFinal.SelectedRow;
            string Orden = ((Label)row.FindControl("lblOrden")).Text;
            int Lote = int.Parse(((Label)row.FindControl("lblLote")).Text);
            AlmacenDTO _result = _almacenBll.DetalleAlmacen(Orden, Lote, usuarioActual);

            switch (_result.CodAlmacen)
            {
                case 95:
                    lblAlmacen.Text = "ANNTARAH";
                    break;
                case 93:
                    lblAlmacen.Text = "CONTRAMUESTRAS";
                    break;
                case 98:
                    lblAlmacen.Text = "Remate Productos no conformes";
                    break;
                case 90:
                    lblAlmacen.Text = "PROD. TERMINADOS";
                    break;
                case 112:
                    lblAlmacen.Text = "Stock Art Atlas";
                    break;
                default:
                    lblAlmacen.Text = "Otro";
                    break;
            }
            lblCodProducto.Text = _result.CodProducto;
            lblOrden.Text = _result.Orden;
            lblLote.Text = _result.NroLote;
            lblContrato.Text = _result.Contrato;
            lblCantidad.Text = _result.Cantidad.ToString();

            lblOrdenConfirm.Text = _result.Orden;
            lblLoteConfirm.Text = _result.NroLote;
        }

        /// <summary>
        /// Evento Click del botón btnEliminar
        /// </summary>
        /// <remarks>
        /// En este evento se llama a los procedimientos BLL de Eliminar Xml de Almacén y de Recepción, para limpiar la lista
        /// completa de ingresos. Luego se redirige a la página principal de Almacén.
        /// </remarks>
        /// <param name="sender">Objeto llamador del evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            _listXml.EliminarXmlAlmacen(lblOrdenConfirm.Text, lblLoteConfirm.Text, usuarioActual);
            _listXml.EliminarXmlRecepcion(lblOrdenConfirm.Text, int.Parse(lblLoteConfirm.Text), usuarioActual);
            Response.Redirect("Almacen.aspx");
        }
    }
}