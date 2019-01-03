using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula los campos principales de la tabla Lanzamiento Cabecera.
    /// </summary>
    public class LanzamientoCabDTO
    {
        public int NumDocumento { get; set; }
        public int NumLanzamiento { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
    }
}
