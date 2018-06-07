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
        private MaquinaDAL _maquinaDal = new MaquinaDAL();
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
