using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.ODOO;
using PSIAA.DataAccessLayer.TuartDB;
using System.Data;

namespace PSIAA.BusinessLogicLayer
{
    public class ComponenteModeloBLL
    {
        private ComponenteModeloDAL _compModelo = new ComponenteModeloDAL();
        private ModeloComponenteDAL _modelComponente = new ModeloComponenteDAL();

        public DataTable ListarComponentesPorModelo(string modelo) {
            DataTable dtComponentesOdoo = _compModelo.SelectComponentesModelo(modelo);
            if (dtComponentesOdoo.Rows.Count > 0) {
                return dtComponentesOdoo;
            }
            else{
                return _modelComponente.SelectComponentesModelo(modelo);
            }
        }
    }
}
