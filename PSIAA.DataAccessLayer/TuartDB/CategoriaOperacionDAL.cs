using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class CategoriaOperacionDAL
    {
        /// <summary>
        /// Variable de instancia aa la clase Transactions (Conexión a la BD)
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección la base de datos para obtener la denominación y el ID de las Categorias de Operación.
        /// </summary>
        /// <returns>Contenedor de tipo DataTable con los datos de consulta</returns>
        public DataTable SelectCategoriaOperacion() {
            string query = @"
                select 
	                i_idcatope, 
	                c_dencatope 
                from categoriaoperacion
                where i_est = 1
                order by i_idcatope";
            return _trans.ReadingQuery(query);
        }
    }
}
