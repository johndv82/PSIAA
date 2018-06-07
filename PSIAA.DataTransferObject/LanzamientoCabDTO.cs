using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class LanzamientoCabDTO
    {
        public int NumDocumento { get; set; }
        public int NumLanzamiento { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
    }
}
