using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.DataTransferObject;
using System.Web.Security;

namespace PSIAA.Presentation.View
{
    public partial class _default : System.Web.UI.Page
    {
        private UsuarioBLL _usuarioBll = new UsuarioBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            txtUsuario.Focus();
            if (!string.IsNullOrEmpty(Request.QueryString["logout"]))
            {
                string user = Request.QueryString["logout"];
                Session.Remove(user);
                
                if (Session.Count > 0) {
                    Session.RemoveAll();
                }
                Session.Abandon();
                Response.Redirect("default.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UsuarioDTO _usuario = _usuarioBll.Login(txtUsuario.Text, txtPassword.Text);
            if (_usuario.Id != 0)
            {
                Session["usuario"] = _usuario;
                Response.Redirect("Inicio.aspx");
            }
            else
            {
                lblMensajeError.Visible = true;
            }
        }
    }
}