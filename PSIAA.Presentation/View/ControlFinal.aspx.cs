using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.DataTransferObject;

namespace PSIAA.Presentation.View
{
    public partial class ControlFinal : System.Web.UI.Page
    {
        /// <summary>
        /// Variable de llamada a la clase BLL RecepcionControlBLL.
        /// </summary>
        public RecepcionControlBLL _recepcionBll = new RecepcionControlBLL();
        public ContratoBLL _contratoBll = new ContratoBLL();
        public string usuarioActual = string.Empty;

        /// <summary>
        /// Evento de Carga Principal del formulario ControlFinal.aspx
        /// </summary>
        /// <remarks>
        /// En este evento se evalúa la existencia de la sesión del usuario y tambien capturamos su valor en una variable publica,
        /// para su posterior uso.
        /// Se cargan los ingresos recepcionados devuelto por el procedimiento BLL ListarRecepcionControl.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
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

                    gridControlFinal.DataSource = _recepcionBll.ListarRecepcionControl(550);
                    gridControlFinal.DataBind();
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
                if (!string.IsNullOrWhiteSpace(hidOrden.Value) & hidLote.Value != "0")
                {
                    try
                    {
                        IDictionary<string, string> _campo = _recepcionBll.ListarCamposRecepcionControl(hidOrden.Value, int.Parse(hidLote.Value));
                        lblMensajeOk.Visible = false;
                        lblMensajeError.Visible = false;
                        btnGuardar.Visible = true;
                        txtPiezas.Text = _campo["Cantidad"];
                        lblTalla.Text = _campo["Talla"];
                        lblModelo.Text = _campo["Modelo"];
                        lblColor.Text = _campo["Color"];
                    }
                    catch (Exception exc)
                    {
                        lblMensajeError.Visible = true;
                        lblValidacion.Visible = false;
                        btnGuardar.Visible = false;
                        txtPiezas.Text = "0";
                        lblTalla.Text = "XXX";
                        lblModelo.Text = "XXXXXXX";
                        lblColor.Text = "XXXXXXX";
                    }
                }
            }
        }

        /// <summary>
        /// Evento Click del botón btnGuardar.
        /// </summary>
        /// <remarks>
        /// En este evento se invoca al procedimiento de ingresos de recepcion, enviando los parametros de:
        /// Orden, Lote, Piezas y Nombre de Usuario, previamente validando la diferencia de piezas ingresadas con el punto anterior, 
        /// en el caso de que la orden pertenesca a un tipo de contrato: "V", se omite la validación.
        /// Si la acción es exitosa o hay problema con las piezas ingresadas, se muestra un mensaje al usuario.
        /// </remarks>
        /// <param name="sender">Objeto llamador del evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtOrden.Text))
            {
                int _pieza;
                if (int.TryParse(txtPiezas.Text == "" ? "0" : txtPiezas.Text, out _pieza) & hidOrden.Value != string.Empty & hidLote.Value != string.Empty)
                {
                    int _diferencia = _recepcionBll.DiferenciaConPuntoAnterior(hidOrden.Value, int.Parse(hidLote.Value), 550, 530);
                    string tipoContrato = _contratoBll.ObtenerTipoContratoPorOrden(hidOrden.Value).Trim();
                    if(( tipoContrato == "V"))
                    {
                        _diferencia = _pieza;
                    }
                    if ((_diferencia > 0) & (_pieza <= _diferencia) & _pieza > 0)
                    {
                        if (_recepcionBll.IngresarRecepcionControl(hidOrden.Value, int.Parse(hidLote.Value), _pieza, usuarioActual))
                        {
                            gridControlFinal.DataSource = _recepcionBll.ListarRecepcionControl(550);
                            gridControlFinal.DataBind();
                            lblMensajeOk.Visible = true;
                            lblValidacion.Visible = false;
                            txtOrden.Text = string.Empty;
                            txtPiezas.Text = string.Empty;
                            txtOrden.Focus();
                        }
                    }
                    else
                    {
                        lblValidacion.Text = "La cantidad de prendas permitidas es: " + _diferencia.ToString();
                        lblValidacion.Visible = true;
                    }
                }
            }
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
            gridSeguimientoOrden.DataSource = _recepcionBll.ListarSeguimientoRecepcionControl(hidOrden.Value);
            gridSeguimientoOrden.DataBind();
        }
    }
}