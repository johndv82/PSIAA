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
        private RecepcionControlBLL _recepcionBll = new RecepcionControlBLL();
        private ContratoBLL _contratoBll = new ContratoBLL();
        private HttpCookie cookie;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cookie = Request.Cookies["Usuario"];
                if (cookie != null)
                {
                    lblUser.Text = cookie["Nombre"].ToString();
                    gridControlFinal.DataSource = _recepcionBll.ListarRecepcionControl(550);
                    gridControlFinal.DataBind();
                    txtOrden.Focus();
                }
                else
                {
                    //LOGOUT
                    string user = Request.QueryString["logout"];
                    Session.Remove(user);
                    Session.Abandon();
                    Response.Redirect("default.aspx");
                }
            }
        }

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
                        if (_recepcionBll.IngresarRecepcionControl(hidOrden.Value, int.Parse(hidLote.Value), _pieza, lblUser.Text))
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

        protected void btnConsultarAvance_Click(object sender, EventArgs e)
        {
            gridSeguimientoOrden.DataSource = _recepcionBll.ListarSeguimientoRecepcionControl(hidOrden.Value);
            gridSeguimientoOrden.DataBind();
        }
    }
}