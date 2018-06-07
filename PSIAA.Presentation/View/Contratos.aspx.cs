using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;

namespace PSIAA.Presentation.View
{
    public partial class Main : System.Web.UI.Page
    {
        private ContratoBLL _contratoBll = new ContratoBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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