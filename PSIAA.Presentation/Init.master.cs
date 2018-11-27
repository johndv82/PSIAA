using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
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
            UsuarioDTO _usuarioLogin = (UsuarioDTO)Session["usuario"];
            if (!IsPostBack)
            {
                if (_usuarioLogin == null)
                {
                    Response.Redirect("default.aspx");
                }
                else
                {
                    bool permisoPagina = false;

                    //CARGAR ACCESOS
                    List<PaginaDTO> _listadoPaginas = _usuarioBll.ListaAccesos(_usuarioLogin.IdCategoria);

                    string paginaActual = (new FileInfo(Page.Request.Url.AbsolutePath)).Name;
                    foreach (PaginaDTO pag in _listadoPaginas)
                    {
                        if (paginaActual != "Inicio.aspx")
                        {
                            if (paginaActual == pag.Pagina)
                            {
                                permisoPagina = true;
                                break;
                            }
                        }
                        else
                            permisoPagina = true;
                    }

                    if (permisoPagina)
                    {
                        lblUsuario.Text = _usuarioLogin.User;

                        string _padre = "";

                        foreach (PaginaDTO _pag in _listadoPaginas)
                        {
                            if (_pag.Padre == "#")
                            {
                                etiquetaCompleta += string.Concat("<li class=\"active\"><a href=\"", _pag.Pagina, "\" target=\"", _pag.Target, "\">", _pag.Nombre, "</a></li>");
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
                    else
                    {
                        Response.Redirect("Inicio.aspx");
                    }
                }
            }
            else {
                if (_usuarioLogin != null)
                {
                    lblUsuario.Text = _usuarioLogin.User;
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
    }
}