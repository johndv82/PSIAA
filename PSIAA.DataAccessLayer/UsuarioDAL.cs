using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class UsuarioDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión a la BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la BD para obtener los datos principales de la tabla ITSM_Usuarios,
        /// que coincidan con el nombre de usuario y la contraseña.
        /// </summary>
        /// <param name="_nombreUsuario">Nombre de Usuario</param>
        /// <param name="_password">Contraseña de Usuario</param>
        /// <returns>Contenedor de tipo DataTable con el resultado de la consulta.</returns>
        public DataTable SelectUsuario(string _nombreUsuario, string _password)
        {
            string query = @"
                select 
                    IDUsuario, 
                    IDCategoria,
                    Nombre, 
                    Apellidos, 
                    Correo 
                from ITSM_Usuarios
                where 
                    Usuario = @user 
                    and Password = @password
                    and Estado = 1";

            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@user", SqlDbType.VarChar) { Value = _nombreUsuario });
            _sqlParam.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = _password });

             return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la BD para obtener todos los dastos de Página accesibles correspondientes a cada
        /// categoria de un usuario.
        /// </summary>
        /// <param name="_codCategoria">Código de Categoria de Usuario</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectAccesos(int _codCategoria) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            
            string query = @"
                select 
	                p.Pagina,
                    p.Target,
	                p.Nombre,
	                p.Padre
                from ITSM_Accesos a inner join ITSM_Paginas p
	                on p.IDPagina = a.IDPagina
                where a.IDCategoria = @categoria
	                and a.Acceso = 1
                    and p.Estado = 1
                order by p.Padre asc, p.Pagina asc";

            _sqlParam.Add(new SqlParameter("@categoria", SqlDbType.VarChar) { Value = _codCategoria });

            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
