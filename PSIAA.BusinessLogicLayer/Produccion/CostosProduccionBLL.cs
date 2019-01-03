using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.Produccion;
using System.Data;

namespace PSIAA.BusinessLogicLayer.Produccion
{
    public class CostosProduccionBLL
    {
        /// <summary>
        /// Variable de instancia a la clase ConsultasProduccion.
        /// </summary>
        public ConsultasProduccion consProduccion = new ConsultasProduccion();

        /// <summary>
        /// Ejecuta un procedimiento DAL de los costos de producción.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="fechaInicio">Fecha de Inicio de consulta</param>
        /// <param name="fechaFin">Fecha Fin de consulta</param>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de datos de tipo DataTable con los datos de costos de producción.</returns>
        public DataTable ListarCostosProduccion(int contrato, string fechaInicio, string fechaFin, string modelo)
        {
            return consProduccion.SelectCostosProduccion(contrato, fechaInicio, fechaFin, modelo);
        }
    }
}
