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
        private UsuarioDAL _usuarioDal = new UsuarioDAL();
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
