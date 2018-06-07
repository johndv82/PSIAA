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
        private ConsultasProduccion consProduccion = new ConsultasProduccion();

        public DataTable ListarCostosProduccion(int contrato, string fechaInicio, string fechaFin, string modelo)
        {
            return consProduccion.SelectCostosProduccion(contrato, fechaInicio, fechaFin, modelo);
        }
    }
}
