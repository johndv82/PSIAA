using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataTransferObject;
using System.Data;

namespace PSIAA.BusinessLogicLayer
{
    public class MaquinaBLL
    {
        /// <summary>
        /// Variable de instancia a la clase MaquinaDAL.
        /// </summary>
        public MaquinaDAL _maquinaDal = new MaquinaDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Máquinas de Producción, y el resultado lo recorre para acceder a sus datos y 
        /// crear un listado de objetos de tipo MaquinaDTO.
        /// </summary>
        /// <returns>Lista genérica de tipo MaquinaDTO con las maquinas de producción.</returns>
        public List<MaquinaDTO> ListarMaquinas()
        {
            List<MaquinaDTO> _listMaquinas = new List<MaquinaDTO>();

            foreach (DataRow fila in _maquinaDal.SelectMaquinas().Rows)
            {
                MaquinaDTO _maquin = new MaquinaDTO
                {
                    Codigo = fila["Codigo"].ToString(),
                    Nombre = fila["Nombre"].ToString(),
                    Abreviacion = fila["Abreviacion"].ToString(),
                    Capacidad = int.Parse(fila["Capacidad"].ToString()),
                    Limite = int.Parse(fila["Limite"].ToString())
                };
                _listMaquinas.Add(_maquin);
            }
            return _listMaquinas;
        }
    }
}
