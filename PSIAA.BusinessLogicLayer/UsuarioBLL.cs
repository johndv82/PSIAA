using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using System.Data;
using PSIAA.DataTransferObject;

namespace PSIAA.BusinessLogicLayer
{
    public class UsuarioBLL
    {
        /// <summary>
        /// Variable de instancia a la clase UsuarioDAL.
        /// </summary>
        public UsuarioDAL _usuarioDal = new UsuarioDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Selección de Usuarios, y se evalúa si el resultado contiene datos, en tal caso se crea una
        /// objeto de tipo UsuarioDTO con la primera coincidencia del resultado, y en caso contrario se devuelve un objeto vacío.
        /// </summary>
        /// <param name="_usuario">Nombre de Usuario</param>
        /// <param name="_password">Clave de Usuario</param>
        /// <returns>Objeto de tipo UsuarioDTO con los datos del usuario.</returns>
        public UsuarioDTO Login(string _usuario, string _password)
        {
            DataTable _dtUsuarios = new DataTable();
            _dtUsuarios = _usuarioDal.SelectUsuario(_usuario, _password);
            if (_dtUsuarios.Rows.Count > 0)
            {
                UsuarioDTO _user = new UsuarioDTO()
                {
                    Id = int.Parse(_dtUsuarios.Rows[0]["IDUsuario"].ToString()),
                    IdCategoria = int.Parse(_dtUsuarios.Rows[0]["IDCategoria"].ToString()),
                    Nombre = _dtUsuarios.Rows[0]["Nombre"].ToString(),
                    Apellidos = _dtUsuarios.Rows[0]["Apellidos"].ToString(),
                    User = _usuario.ToLower(),
                    Correo = _dtUsuarios.Rows[0]["Correo"].ToString(),
                };
                return _user;
            }
            else
                return new UsuarioDTO();
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Accesos por Categoría de Usuario, y el resultado se recorre obteniendo los datos de página
        /// de accesos para poblar una lista de objetos de tipo PaginaDTO.
        /// </summary>
        /// <param name="_codCategoria">Código de Categoria de Usuario</param>
        /// <returns>Lista Genérica de tipo PaginaDTO con las paginas de acceso./returns>
        public List<PaginaDTO> ListaAccesos(int _codCategoria)
        {
            DataTable dtResult = _usuarioDal.SelectAccesos(_codCategoria);
            List<PaginaDTO> _listaPaginas = new List<PaginaDTO>();

            foreach (DataRow fila in dtResult.Rows)
            {
                PaginaDTO _pagina = new PaginaDTO()
                {
                    Pagina = fila["Pagina"].ToString(),
                    Target = fila["Target"].ToString(),
                    Nombre = fila["Nombre"].ToString(),
                    Padre = fila["Padre"].ToString()
                };
                _listaPaginas.Add(_pagina);
            }
            return _listaPaginas;
        }
    }
}
