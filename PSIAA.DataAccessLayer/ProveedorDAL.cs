using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class ProveedorDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión a la BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la BD, para obtener datos principales de Proveedores activos.
        /// </summary>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectProveedores() {
            string query = @"
                select
	                cod_proveedor,
	                nombre_comercial,
	                direccion,
	                telefono_1,
	                telefono_2,
	                ciudad
                from proveedores
                where 
	                estado = 'A'";

            return _trans.ReadingQuery(query, null);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la BD, para obtener el nombre comercial de un Proveedor.
        /// </summary>
        /// <param name="_codProveedor">Código de Proveedor (RUC)</param>
        /// <returns>Variable de tipo string con el nombre del proveedor.</returns>
        public string NombreProveedor(string _codProveedor) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                nombre_comercial 
                from proveedores 
                where cod_proveedor = @codproveedor
                and estado = 'A'";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _codProveedor });
            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }
    }
}
