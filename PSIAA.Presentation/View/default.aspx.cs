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

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UsuarioDTO _usuario = _usuarioBll.Login(txtUsuario.Text, txtPassword.Text);
            if (_usuario.Id != 0)
            {
                //Creacion de Cookie
                HttpCookie cookie = Request.Cookies["Usuario"];
                if (cookie == null)
                    cookie = new HttpCookie("Usuario");

                cookie["Nombre"] = _usuario.User;
                cookie.Expires = DateTime.Now.AddHours(3);
                Response.Cookies.Add(cookie);
                ///
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