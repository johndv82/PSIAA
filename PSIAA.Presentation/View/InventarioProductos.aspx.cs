using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer.Produccion;
using PSIAA.DataTransferObject;

namespace PSIAA.Presentation.View
{
    public partial class InventarioProductos : System.Web.UI.Page
    {
        private InventarioProductosBLL _inventarioProductosBll = new InventarioProductosBLL();
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
            hidContrato.Value = txtContrato.Text;
            gridInventarioProductos.DataSource = _inventarioProductosBll.ListarInventarioProductos(int.Parse(hidContrato.Value));
            gridInventarioProductos.DataBind();
        }
    }
}