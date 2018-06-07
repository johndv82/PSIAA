using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer.Produccion;

namespace PSIAA.Presentation.View
{
    public partial class InventarioProductos : System.Web.UI.Page
    {
        private InventarioProductosBLL _inventarioProductosBll = new InventarioProductosBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
            }
            txtContrato.Focus();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            hidContrato.Value = txtContrato.Text;
            gridInventarioProductos.DataSource = _inventarioProductosBll.ListarInventarioProductos(int.Parse(hidContrato.Value));
            gridInventarioProductos.DataBind();
        }
    }
}