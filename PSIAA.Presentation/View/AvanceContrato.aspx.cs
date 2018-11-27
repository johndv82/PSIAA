using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.DataTransferObject;
using System.Data;
using System.Net;
using System.Web.Services;

namespace PSIAA.Presentation.View
{
    public partial class AvanceContrato : System.Web.UI.Page
    {
        private ContratoBLL _contratoBll = new ContratoBLL();
        private static ClienteBLL _clienteBll = new ClienteBLL();
        private int[] totalAvance = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private string puntoControl;
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
                txtSearch.Focus();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            txtSearch.ID = "txtSearch";
            if (rbnFiltros.SelectedValue == "contrato")
            {
                gridAvanceContrato.DataSource = _contratoBll.ListarAvancePorContrato(txtSearch.Text);
                gridAvanceContrato.DataBind();

                if (gridAvanceContrato.Rows.Count > 0)
                {
                    hidContrato.Value = txtSearch.Text;
                    btnAvanceDetallado.Visible = true;
                }
                ddlContratos.Visible = false;
            }
            else if (rbnFiltros.SelectedValue == "modelo")
            {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text)) {
                    ddlContratos.DataSource = _contratoBll.ListarContratosPorModelo(txtSearch.Text);
                    ddlContratos.DataBind();
                    ddlContratos.Visible = true;
                }
            }
            else if (rbnFiltros.SelectedValue == "cliente") {
                ddlContratos.DataSource = _contratoBll.ListarContratosPorCliente(int.Parse(hidCustomerId.Value));
                ddlContratos.DataBind();
                ddlContratos.Visible = true;
            }
        }

        protected void gridAvanceContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridAvanceContrato.SelectedRow;
            lblModelo.Text = ((Button)row.FindControl("lblModelo")).Text.Trim();
            lblColor.Text = ((Label)row.FindControl("lblColor")).Text.Trim();

            if (puntoControl != "0")
            {
                ddlPuntos.SelectedValue = puntoControl;
                CargarDatosAvance();
            }
            else
            {
                string baseModelo = lblModelo.Text.Substring(0, lblModelo.Text.IndexOf('-'));
                string informacion = leerPaginaWeb(@"http://192.168.0.1/diseno/hojas/" + baseModelo);

                //BUSCAR ARCHIVOS DE MODELO EN CADENA
                List<string> subModelos = new List<string>();
                string cadInicio = ".pdf" + '"' + " id=" + '"';
                string cadFinal = ".pdf" + '"' + ">";
                while (informacion.IndexOf(cadInicio) > 0)
                {
                    int indInicio = informacion.IndexOf(cadInicio) + cadInicio.Length;
                    int indFin = informacion.IndexOf(cadFinal);
                    string docModelo = informacion.Substring(indInicio, indFin - indInicio) + ".pdf";
                    subModelos.Add(docModelo);
                    informacion = informacion.Substring(indFin + cadFinal.Length, informacion.Length - (indFin + cadFinal.Length));
                }
                string radioButton = "";

                if (subModelos.Count > 0)
                {
                    foreach (string nombre in subModelos)
                    {
                        string etiqueta = @"
                        <input type='radio' name='rdbModelo' id='rdbModelo" + subModelos.IndexOf(nombre) + @"' value='" + nombre + @"' onchange='CargarDocumento(this)' />
                        <label for='rdbModelo" + subModelos.IndexOf(nombre) + @"'>" + nombre + @"</label>";
                        radioButton += etiqueta;
                    }
                    idRadios.InnerHtml = radioButton;
                }
                else
                    idRadios.InnerHtml = @"
                    <div class='alert alert-dismissible alert-danger'>
                        <button type = 'button' class='close' data-dismiss='alert'>&times;</button>
                        <strong>Hubo un Error!</strong> No se encontró Hoja de Especificaciones.
                    </div>";
            }
        }

        private string leerPaginaWeb(string laUrl)
        {
            try
            {
                WebRequest request = WebRequest.Create(laUrl);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string res = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return res;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void CargarDatosAvance()
        {
            gridDetalleAvance.DataSource = _contratoBll.FiltrarDetalleAvancePorContrato(hidContrato.Value, int.Parse(puntoControl), lblModelo.Text, lblColor.Text);
            gridDetalleAvance.DataBind();

            if ((ddlPuntos.SelectedValue == "400") || (ddlPuntos.SelectedValue == "500"))
            {
                PoblarGridAsignacionOrdenes();
                gridDetalleAvance.Visible = false;
                gridDetalleAsignaciones.Visible = true;
            }
            else
            {
                gridDetalleAvance.Visible = true;
                gridDetalleAsignaciones.Visible = false;
            }
        }

        private void PoblarGridAsignacionOrdenes()
        {
            string[] tallas;
            int contrato = rbnFiltros.SelectedValue == "contrato" ? int.Parse(txtSearch.Text) : int.Parse(ddlContratos.Text);
            gridDetalleAsignaciones.DataSource = _contratoBll.ListarAsignacionesPorOrden(int.Parse(ddlPuntos.SelectedValue),
                                                                                        contrato, lblModelo.Text,
                                                                                        lblColor.Text, out tallas);
            gridDetalleAsignaciones.DataBind();

            for (int c = 4; c < gridDetalleAsignaciones.Columns.Count; c++)
            {
                gridDetalleAsignaciones.Columns[c].HeaderText = tallas[c - 4];
            }
            gridDetalleAsignaciones.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            puntoControl = ddlPuntos.SelectedValue;
            CargarDatosAvance();
        }

        protected void gridAvanceContrato_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowType != DataControlRowType.EmptyDataRow))
            {
                totalAvance[0] += ((Label)e.Row.FindControl("lblSolicitado")).Text == "" ? 0 : int.Parse(((Label)e.Row.FindControl("lblSolicitado")).Text);
                totalAvance[1] += ((Label)e.Row.FindControl("lblLanzado")).Text == "" ? 0 : int.Parse(((Label)e.Row.FindControl("lblLanzado")).Text);
                totalAvance[2] += ((Button)e.Row.FindControl("lblAsigTejido")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblAsigTejido")).Text);
                totalAvance[3] += ((Button)e.Row.FindControl("lblTejido")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblTejido")).Text);
                totalAvance[4] += ((Button)e.Row.FindControl("lblCtrlTejido")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblCtrlTejido")).Text);
                totalAvance[5] += ((Button)e.Row.FindControl("lblLavado")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblLavado")).Text);
                totalAvance[6] += ((Button)e.Row.FindControl("lblCtrlLavado")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblCtrlLavado")).Text);
                totalAvance[7] += ((Button)e.Row.FindControl("lblCorte")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblCorte")).Text);
                totalAvance[8] += ((Button)e.Row.FindControl("lblAsigConfeccion")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblAsigConfeccion")).Text);
                totalAvance[9] += ((Button)e.Row.FindControl("lblConfeccion")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblConfeccion")).Text);
                totalAvance[10] += ((Button)e.Row.FindControl("lblAcabConfeccion")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblAcabConfeccion")).Text);
                totalAvance[11] += ((Button)e.Row.FindControl("lblAcabFinal")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblAcabFinal")).Text);
                totalAvance[12] += ((Button)e.Row.FindControl("lblAlmacen")).Text == "" ? 0 : int.Parse(((Button)e.Row.FindControl("lblAlmacen")).Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = totalAvance[0].ToString("0,0");
                e.Row.Cells[4].Text = totalAvance[1].ToString("0,0");
                e.Row.Cells[5].Text = totalAvance[2].ToString("0,0");
                e.Row.Cells[6].Text = totalAvance[3].ToString("0,0");
                e.Row.Cells[7].Text = totalAvance[4].ToString("0,0");
                e.Row.Cells[8].Text = totalAvance[5].ToString("0,0");
                e.Row.Cells[9].Text = totalAvance[6].ToString("0,0");
                e.Row.Cells[10].Text = totalAvance[7].ToString("0,0");
                e.Row.Cells[11].Text = totalAvance[8].ToString("0,0");
                e.Row.Cells[12].Text = totalAvance[9].ToString("0,0");
                e.Row.Cells[13].Text = totalAvance[10].ToString("0,0");
                e.Row.Cells[14].Text = totalAvance[11].ToString("0,0");
                e.Row.Cells[15].Text = totalAvance[12].ToString("0,0");
                e.Row.Font.Bold = true;
                e.Row.Font.Size = FontUnit.Medium;
            }
        }

        protected void gridAvanceContrato_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            puntoControl = e.CommandArgument.ToString();
        }

        private void GroupHeaderAvanceTallas(string groupName, object[] values, GridViewRow row)
        {
            row.Font.Bold = true;
            row.Cells[0].Text = "";
            foreach (object valor in values)
                row.Cells.Add(new TableCell() { Text = valor.ToString() });
            row.Cells[0].ColumnSpan = 2;
        }

        private static void GroupHeaderDetalleContrato(string groupName, object[] values, GridViewRow row)
        {
            row.Font.Bold = true;
            row.Cells[0].Text = "";
            foreach (object valor in values)
                row.Cells.Add(new TableCell() { Text = valor.ToString() });

            row.Cells[0].ColumnSpan = 5;
        }

        protected void btnAvanceDetallado_Click(object sender, EventArgs e)
        {
            //CARGAR GRID DE SOLICITADO
            int contrato = 0;
            if (rbnFiltros.SelectedValue == "contrato")
            {
                int.TryParse(txtSearch.Text == "" ? "0" : txtSearch.Text, out contrato);
            }
            else {
                contrato = int.Parse(ddlContratos.Text);
            }

            Session["dtContratoDetalle"] = _contratoBll.ListarDetalleContrato(contrato, false);

            rblPuntosControl.SelectedValue = "300";
            rblPuntosControl_SelectedIndexChanged1(sender, e);
        }

        protected void rblPuntosControl_SelectedIndexChanged1(object sender, EventArgs e)
        {
            string _cliente, _po;
            int contrato = rbnFiltros.SelectedValue == "contrato" ? int.Parse(txtSearch.Text) : int.Parse(ddlContratos.Text);
            DataTable dtAvance = _contratoBll.FiltrarAvanceDetalladoTallasPorPunto(contrato, int.Parse(rblPuntosControl.SelectedValue), out _cliente, out _po);
            if (dtAvance.Rows.Count > 0)
            {
                gridAvanceTallas.DataSource = dtAvance;
                gridAvanceTallas.DataBind();

                GridViewHelper helper = new GridViewHelper(gridAvanceTallas);

                string[] tallas = { "talla1", "talla2", "talla3", "talla4", "talla5", "talla6", "talla7", "talla8", "talla9" };
                helper.RegisterGroup(tallas, true, false);
                helper.GroupHeader += new GroupEvent(GroupHeaderAvanceTallas);

                gridAvanceTallas.DataBind();
            }
            else
            {
                gridAvanceTallas.DataSource = new DataTable();
                gridAvanceTallas.DataBind();

                gridDetalleSolicitadoContrato.DataSource = new DataTable();
                gridDetalleSolicitadoContrato.DataBind();
            }
            lblContrato.Text = hidContrato.Value;
            lblCliente.Text = _cliente;
            lblPo.Text = _po;
            CargarGridDetalleSolicitadoContrato();
        }

        private void CargarGridDetalleSolicitadoContrato()
        {

            List<ContratoDetalleDTO> listContDet = Session["dtContratoDetalle"] as List<ContratoDetalleDTO>;
            gridDetalleSolicitadoContrato.DataSource = listContDet;
            gridDetalleSolicitadoContrato.DataBind();

            GridViewHelper helper = new GridViewHelper(gridDetalleSolicitadoContrato);

            string[] tallas = { "Tallas[0]", "Tallas[1]", "Tallas[2]", "Tallas[3]", "Tallas[4]", "Tallas[5]", "Tallas[6]", "Tallas[7]", "Tallas[8]" };
            helper.RegisterGroup(tallas, true, false);
            helper.GroupHeader += new GroupEvent(GroupHeaderDetalleContrato);

            gridDetalleSolicitadoContrato.DataBind();
        }

        [WebMethod]
        public static string[] GetClientes(string prefijo)
        {
            List<string> customers = new List<string>();
            DataTable dtClientes = _clienteBll.ListarClientes();
            foreach (DataRow row in dtClientes.Rows)
            {
                customers.Add(string.Format("{0}-{1}", row["nombre"].ToString().Trim().ToUpper(), row["cod_cliente"]));
            }
            var rpta = customers.Where(x => x.Contains(prefijo.ToUpper())).ToList();
            var arr =  rpta.ToArray();
            return arr;
        }

        protected void rbnFiltros_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            /*if (rbnFiltros.SelectedValue != "cliente")
            {
                txtSearch.ID = "Otro";
            }*/
        }

        protected void ddlContratos_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridAvanceContrato.DataSource = _contratoBll.ListarAvancePorContrato(ddlContratos.Text.Trim());
            gridAvanceContrato.DataBind();

            if (gridAvanceContrato.Rows.Count > 0)
            {
                hidContrato.Value = txtSearch.Text;
                btnAvanceDetallado.Visible = true;
            }
        }
    }
}