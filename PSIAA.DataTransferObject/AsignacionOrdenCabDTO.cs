using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula los campos principales de la tabla Asignacion Ordenes Cab
    /// </summary>
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
        /// <summary>
        /// Campo de Categoria Operación
        /// </summary>
        public int CodCatOperacion { get; set; }
        public string NroAsignacion { get; set; }
        public string CodProveedor { get; set; }
        public DateTime FechaGeneracion {
            get { return _fechaGeneracion; }
        }
        public string HoraGeneracion {
            get { return _horaGeneracion; }
        }

        /// <summary>
        /// Campo de Fecha de Retorno
        /// </summary>
        public DateTime FechaEntrega { get; set; }
        public string Usuario { get; set; }

        /// <summary>
        /// Este campo guarda el valor del chekbox de: ¿todas las operaciones?
        /// </summary>
        public short Completo { get; set; }
        #endregion
    }
}
