using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class MaquinaDTO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public long Id { get; set; }
        public long Linea { get; set; }
        public string Abreviacion { get; set; }
        public string Galga { get; set; }
        public int Capacidad { get; set; }
        public int Limite { get; set; }
    }
}
