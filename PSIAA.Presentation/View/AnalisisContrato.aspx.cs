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
    public partial class AnalisisContrato : System.Web.UI.Page
    {
        private ContratoBLL _contratoBll = new ContratoBLL();
        private AnalisisContratoBLL _analisisCont = new AnalisisContratoBLL();
        public string usuarioActual = string.Empty;

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
                }
                txtContrato.Focus();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtContrato.Text))
            {
                List<string> _modelos = _contratoBll.ListarModelosContrato(int.Parse(txtContrato.Text));
                _modelos.RemoveAt(0);
                ddlModelo.DataSource = _modelos;
                ddlModelo.DataBind();
                hidContrato.Value = txtContrato.Text;
            }
        }

        private void helper_GroupHeader(string groupName, object[] values, GridViewRow row)
        {
            if (groupName == "Color")
            {
                row.Cells[0].Font.Bold = true;
                row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#B3D5F7");
                row.Cells[0].Text = string.Format("Color:   {0}", values[0].ToString());
            }
        }

        private void helper_Bug(string groupName, object[] values, GridViewRow row)
        {
            if (groupName == null) return;
            row.Cells[0].Text = "TOTAL MATERIAL:";
            row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row.Font.Bold = true;
            if (double.Parse(row.Cells[1].Text) == 100)
            {
                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F5F6CE");
            }
            else
            {
                row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FDACAE");
            }
        }

        protected void btnAnalizar_Click(object sender, EventArgs e)
        {
            //Cargar Materiales
            gridMateriales.DataSource = _analisisCont.ListarMaterialModelo(int.Parse(hidContrato.Value), ddlModelo.SelectedItem.ToString());
            gridMateriales.DataBind();
            //Agrupar Grid de Materiales por Color
            GridViewHelper helper = new GridViewHelper(gridMateriales);
            helper.RegisterGroup("Color", true, false);
            helper.RegisterSummary("Porcentaje", "{0:###.###}", SummaryOperation.Sum, "Color");
            helper.GroupHeader += new GroupEvent(helper_GroupHeader);
            helper.GroupSummary += new GroupEvent(helper_Bug);
            gridMateriales.DataBind();

            //Cargar Medidas
            gridMedidas.DataSource = _analisisCont.ListarMedidasPorModelo(ddlModelo.SelectedItem.ToString());
            gridMedidas.DataBind();

            //Cargar Peso
            lblTalla.Text = _analisisCont.TallaPesoMuestra(ddlModelo.SelectedItem.ToString())[0];
            lblPeso.Text = _analisisCont.TallaPesoMuestra(ddlModelo.SelectedItem.ToString())[1];
        }

        protected void gridMedidas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowType != DataControlRowType.EmptyDataRow))
            {
                if (e.Row.Cells[1].Text == "Error") {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}