using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class MaquinaDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la BD, para obtener todas las Maquinas activas.
        /// </summary>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectMaquinas() {
            string query = @"
                select 
	                c_codmaq as Codigo,
	                c_denmaq as Nombre,
	                i_idmaq as Id,
	                c_linea as Linea,
	                c_abr as Abreviacion,
	                c_galga as Galga,
	                n_capop as Capacidad,
	                n_itmlt as Limite
                from maquina_bac
                where i_est = 1";

            return _trans.ReadingQuery(query, null);
        }
    }
}
