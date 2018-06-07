using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class ClienteDAL
    {
        private Transactions _trans = new Transactions();
        public DataTable SelectNombreClientes()
        {
            string query = @"
                select 
                    cod_cliente, 
                    nombre 
                from clientes";
            return _trans.ReadingQuery(query);
        }
    }
}
