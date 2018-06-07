using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class ProveedorDAL
    {
        private Transactions _trans = new Transactions();

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
