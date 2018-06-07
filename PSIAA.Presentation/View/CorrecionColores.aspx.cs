using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;

namespace PSIAA.Presentation.View
{
    public partial class CorrecionColores : System.Web.UI.Page
    {
        private HojaCombinacionesBLL _hojaCombinacionBll = new HojaCombinacionesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
            }
            txtContrato.Focus();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtContrato.Text.Trim())) {
                if (_hojaCombinacionBll.CorregirColores(int.Parse(txtContrato.Text)))
                {
                    lblMensajeOk.Visible = true;
                    lblError.Visible = false;
                }
                else {
                    lblMensajeOk.Visible = false;
                    lblError.Visible = true;
                }
                txtContrato.Text = string.Empty;
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Inicio.aspx");
        }
    }
}