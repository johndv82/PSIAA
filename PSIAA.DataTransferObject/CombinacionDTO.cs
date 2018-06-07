using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class CombinacionDTO
    {
        public string Modelo { get; set; }
        public string Color { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Kilos { get; set; }
        public string Producto { get; set; }
        public string Titulo { get; set; }

        public string DescripcionMaterial { get; set; }
    }
}
