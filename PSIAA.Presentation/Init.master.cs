using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.DataTransferObject;
using PSIAA.BusinessLogicLayer;

namespace PSIAA.Presentation
{
    public partial class Init : System.Web.UI.MasterPage
    {
        private UsuarioBLL _usuarioBll = new UsuarioBLL();
        public string etiquetaCompleta = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UsuarioDTO _usuarioLogin = new UsuarioDTO();
                _usuarioLogin = (UsuarioDTO)Session["usuario"];
                if (_usuarioLogin == null)
                {
                    Response.Redirect("default.aspx");
                }
                else
                {
                    lblUsuario.Text = _usuarioLogin.Nombre + " " + _usuarioLogin.Apellidos;

                    //CARGAR ACCESOS
                    List<PaginaDTO> _listadoPaginas = _usuarioBll.ListaAccesos(_usuarioLogin.IdCategoria);
                    string _padre = "";

                    foreach (PaginaDTO _pag in _listadoPaginas)
                    {
                        if (_pag.Padre == "#")
                        {
                            etiquetaCompleta += string.Concat("<li class=\"active\"><a href=\"", _pag.Pagina, "\" target=\"", _pag.Target,"\">", _pag.Nombre, "</a></li>");
                        }
                        else
                        {
                            if (_padre == _pag.Padre)
                            {
                                etiquetaCompleta += string.Concat("<li><a href=\"", _pag.Pagina, "\" target=\"", _pag.Target, "\">", _pag.Nombre, "</a></li>");
                                _padre = _pag.Padre;
                            }
                            else if (_padre == "")
                            {
                                etiquetaCompleta += string.Concat("<li class=\"dropdown active\"><a href = \"#\" data-toggle = \"dropdown\" role = \"button\" aria-expanded = \"true\"><span class=\"caret\"></span>&nbsp;&nbsp;", _pag.Padre, "</a><ul class=\"dropdown-menu\" role=\"menu\">");
                                etiquetaCompleta += string.Concat("<li><a href=\"", _pag.Pagina, "\" target=\"", _pag.Target, "\">", _pag.Nombre, "</a></li>");
                                _padre = _pag.Padre;
                            }
                            else
                            {
                                etiquetaCompleta += "</ul></li>";
                                etiquetaCompleta += string.Concat("<li class=\"dropdown active\"><a href = \"#\" data-toggle = \"dropdown\" role = \"button\" aria-expanded = \"true\"><span class=\"caret\"></span>&nbsp;&nbsp;", _pag.Padre, "</a><ul class=\"dropdown-menu\" role=\"menu\">");
                                etiquetaCompleta += string.Concat("<li><a href=\"", _pag.Pagina, "\" target=\"", _pag.Target, "\">", _pag.Nombre, "</a></li>");
                                _padre = _pag.Padre;
                            }
                        }
                        if (_pag == _listadoPaginas[_listadoPaginas.Count - 1])
                        {
                            etiquetaCompleta += "</ul></li>";
                        }
                    }
                }
            }
        }

        public string Usuario
        {
            get {
                HttpCookie cookie = Request.Cookies["Usuario"];
                return cookie["Nombre"].ToString();
            }
        }
    }
}