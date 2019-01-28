using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class ClienteDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el código y nombre de todos los clientes SIAA.
        /// </summary>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
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
