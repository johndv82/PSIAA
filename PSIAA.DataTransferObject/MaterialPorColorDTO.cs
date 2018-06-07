using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class MaterialPorColorDTO
    {
        public int Contrato { get; set; }
        public string Modelo { get; set; }
        public string ColorBase { get; set; }
        public string Color { get; set; }
        public string Calidad { get; set; }
        public decimal Porcentaje { get; set; }
        public string CodProducto { get; set; }
    }
}
