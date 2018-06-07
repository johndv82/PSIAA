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
        private ClienteDAL _clienteDal = new ClienteDAL();

        public DataTable ListarClientes() {
            return _clienteDal.SelectNombreClientes();
        }
    }
}
