using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula datos secundarios de elementos a Asignar.
    /// </summary>
    public class AasignarDTO
    {
        public string Modelo { get; set; }
        public string Color { get; set; }
        public DateTime FechaRetorno { get; set; }
        public int CodCatOperacion { get; set; }
        public string DescripcionCatOper { get; set; }
        public string NroAsignacion { get; set; }
        public string CodProveedor { get; set; }
        public string Taller { get; set; }
        public bool TodasOperaciones { get; set; }

        /// <summary>
        /// Flag que indica si se asignará o no
        /// </summary>
        public string Asignacion { get; set; }
    }
}
