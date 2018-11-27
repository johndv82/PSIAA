using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
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
        //Flag que indica si se asignará o no
        public string Asignacion { get; set; }
    }
}
