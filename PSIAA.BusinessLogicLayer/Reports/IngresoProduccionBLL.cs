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
        private AlmacenDAL _almacenDal = new AlmacenDAL();
        public DataTable DetalleIngresoProduccion(string nroParte, int almacenSap) {
            return _almacenDal.SelectIngresosProduccion(nroParte, almacenSap);
        }
    }
}
