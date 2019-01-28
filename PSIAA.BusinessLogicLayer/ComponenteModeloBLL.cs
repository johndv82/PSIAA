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
        /// <summary>
        /// Variable de instancia a la clase ComponenteModeloDAL.
        /// </summary>
        public ComponenteModeloDAL _compModelo = new ComponenteModeloDAL();
        /// <summary>
        /// Variable de instancia a la clase ModeloComponenteDAL.
        /// </summary>
        private ModeloComponenteDAL _modelComponente = new ModeloComponenteDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Componentes por Modelo ODOO, y lo retorna. En el caso de que el resultado no tenga datos
        /// ejecutamos el procedimiento DAL de Componenetes por Modelo SIAA, y el resultado es retornado.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con los componentes.</returns>
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
