using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class AsignacionOrdenCabDTO
    {
        #region Atributos
        private DateTime _fechaGeneracion;
        private string _horaGeneracion;

        #endregion

        #region Constructores
        public AsignacionOrdenCabDTO() {
            _fechaGeneracion = DateTime.Now;
            _horaGeneracion = string.Concat(DateTime.Now.ToString("HHmmss"));
        }
        #endregion

        #region Propiedades
        //CodCatOperacion -> NroAsigna
        public int CodCatOperacion { get; set; }
        public string NroAsignacion { get; set; }
        public string CodProveedor { get; set; }
        public DateTime FechaGeneracion {
            get { return _fechaGeneracion; }
        }
        public string HoraGeneracion {
            get { return _horaGeneracion; }
        }

        //Fecha de Retorno
        public DateTime FechaEntrega { get; set; }
        public string Usuario { get; set; }

        //Este campo guarda el valor del chekbox de ¿todas las operaciones?
        public short Completo { get; set; }
        #endregion
    }
}
