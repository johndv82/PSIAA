using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class PesoDTO
    {
        public string CodigoModelo { get; set; }
        public string Talla { get; set; }
        public decimal PesoTejido { get; set; }
        public decimal PesoAcabado { get; set; }
    }
}
