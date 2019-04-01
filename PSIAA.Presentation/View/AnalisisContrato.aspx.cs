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
        /// <summary>
        /// Variable de instancia a la clase ContratoBLL
        /// </summary>
        public ContratoBLL _contratoBll = new ContratoBLL();
        /// <summary>
        /// Variable de instancia a la clase AnalisisContratoBLL
        /// </summary>
        public AnalisisContratoBLL _analisisContBll = new AnalisisContratoBLL();
        /// <summary>
        /// Variable publica para almacenar el usuario logueado.
        /// </summary>
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

        /// <summary>
        /// Evento Click del botón btnBuscar.
        /// </summary>
        /// <remarks>
        /// En este evento se ejecuta un procedimiento BLL de Listar Modelos por Contrato y el resultado es cargado en la Lista
        /// Desplegable ddlModelo.
        /// El valor del contrato ingresado es cargado en un control invisible, para su posterior consulta.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
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

        /// <summary>
        /// Evento click del botón btnAnalizar.
        /// </summary>
        /// <remarks>
        /// En este evento se ejecutan los procedimientos BLL para cargar el Listado de Materiales por Modelo, Listado de 
        /// Medidas por Modelo, y valores de Talla/Peso por Modelo, y son cargados en sus respectivas grillas.
        /// La grilla de Materiales es agrupada por Color sumando su Porcentajes.
        /// </remarks>
        /// <param name="sender">Objeto que llama al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
        protected void btnAnalizar_Click(object sender, EventArgs e)
        {
            //Cargar Materiales
            gridMateriales.DataSource = _analisisContBll.ListarMaterialModelo(int.Parse(hidContrato.Value), ddlModelo.SelectedItem.ToString());
            gridMateriales.DataBind();
            //Agrupar Grid de Materiales por Color
            GridViewHelper helper = new GridViewHelper(gridMateriales);
            helper.RegisterGroup("Color", true, false);
            helper.RegisterSummary("Porcentaje", "{0:###.###}", SummaryOperation.Sum, "Color");
            helper.GroupHeader += new GroupEvent(helper_GroupHeader);
            helper.GroupSummary += new GroupEvent(helper_Bug);
            gridMateriales.DataBind();

            //Cargar Medidas
            gridMedidas.DataSource = _analisisContBll.ListarMedidasPorModelo(ddlModelo.SelectedItem.ToString());
            gridMedidas.DataBind();

            //Cargar Peso
            lblTalla.Text = _analisisContBll.TallaPesoMuestra(ddlModelo.SelectedItem.ToString())[0];
            lblPeso.Text = _analisisContBll.TallaPesoMuestra(ddlModelo.SelectedItem.ToString())[1];
        }

        /// <summary>
        /// Evento de Enlace de Fila de Datos de la grilla grdiMedidas.
        /// </summary>
        /// <param name="sender">Objeto que llamada al evento</param>
        /// <param name="e">Argumentos que contienen datos del evento</param>
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