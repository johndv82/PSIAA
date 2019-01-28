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
        /// <summary>
        /// Variable de instancia a la clase ConsultasProduccion.
        /// </summary>
        public ConsultasProduccion _consProduccion = new ConsultasProduccion();

        /// <summary>
        /// Ejecuta un procedimiento DAL de inventario de productos.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <returns>Contenedor de datos de tipo DataTable con los datos de inventario.</returns>
        public DataTable ListarInventarioProductos(int contrato)
        {
            return _consProduccion.SelectInventarioProductos(contrato);
        }
    }
}
