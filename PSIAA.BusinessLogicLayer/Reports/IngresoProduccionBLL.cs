using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using System.Data;

namespace PSIAA.BusinessLogicLayer.Reports
{
    public class IngresoProduccionBLL
    {
        /// <summary>
        /// Variable de instancia a la clase AlmacenDAL.
        /// </summary>
        public AlmacenDAL _almacenDal = new AlmacenDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Ingresos a Producción.
        /// </summary>
        /// <param name="nroParte">Número de Parte de Ingreso</param>
        /// <param name="almacenSap">Codigo de Almacén SAP</param>
        /// <returns>Contenedor de datos de tipo DataTable con los ingresos.</returns>
        public DataTable DetalleIngresoProduccion(string nroParte, int almacenSap) {
            return _almacenDal.SelectIngresosProduccion(nroParte, almacenSap);
        }
    }
}
