using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PSIAA.DataAccessLayer;

namespace PSIAA.BusinessLogicLayer
{
    public class ClienteBLL
    {
        /// <summary>
        /// Variable de instancia a la clase ClienteDAL.
        /// </summary>
        public ClienteDAL _clienteDal = new ClienteDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Nombres de Clientes, y retorna el resultado.
        /// </summary>
        /// <returns>Contenedor de tipo DataTable con los nombres de clientes</returns>
        public DataTable ListarClientes() {
            return _clienteDal.SelectNombreClientes();
        }
    }
}
