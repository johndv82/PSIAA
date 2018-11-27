using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.DataTransferObject;

namespace PSIAA.Presentation.View
{
    public partial class Main : System.Web.UI.Page
    {
        private ContratoBLL _contratoBll = new ContratoBLL();
        public string usuarioActual = string.Empty;

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
                    cmbTipoContrato.DataSource = _contratoBll.ListarTiposContrato();
                    cmbTipoContrato.DataBind();

                    cmbAnioEm.DataSource = _contratoBll.ListarAnios();
                    cmbAnioEm.DataBind();

                    CargarGrillaContratos();
                    gridContrato.Columns[3].Visible = false;
                    gridContrato.Columns[5].Visible = false;
                    gridContrato.Columns[10].Visible = false;
                    gridContrato.Columns[12].Visible = false;
                }
            }
        }

        private void CargarGrillaContratos()
        {
            gridContrato.DataSource = _contratoBll.ListarContratos(cmbTipoContrato.SelectedValue,
                                       cmbAnioEm.Text,
                                       txtContrato.Text);
            gridContrato.DataBind();
        }

        protected void gridContrato_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridContrato.DataSource = _contratoBll.ListarContratos(cmbTipoContrato.SelectedValue,
                                       cmbAnioEm.Text,
                                       txtContrato.Text);
            gridContrato.PageIndex = e.NewPageIndex;
            gridContrato.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrillaContratos();
        }

        protected void cmbTipoContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrillaContratos();
        }

        protected void cmbAnioEm_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrillaContratos();
        }
    }
}