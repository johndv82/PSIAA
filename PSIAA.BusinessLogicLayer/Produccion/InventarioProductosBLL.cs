using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.Produccion;
using System.Data;

namespace PSIAA.BusinessLogicLayer.Produccion
{
    public class InventarioProductosBLL
    {
        private ConsultasProduccion _consProduccion = new ConsultasProduccion();
        public DataTable ListarInventarioProductos(int contrato)
        {
            return _consProduccion.SelectInventarioProductos(contrato);
        }
    }
}
