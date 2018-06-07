using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class AlanzarDTO
    {
        #region Atributos

        private int[] _cantidades = new int[9];
        private string[] _tallas = new string[9];

        #endregion

        #region Propiedades
        public int Contrato { get; set; }
        public string Modelo { get; set; }
        public char CorrelativoModelo { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public decimal KilosNecesarios { get; set; }
        public int[] Cantidades
        {
            get { return _cantidades; }
            set { _cantidades = value; }
        }

        public string[] Tallas{
            get { return _tallas; }
            set { _tallas = value; }
        }

        public string Linea { get; set; }
        public string Asignacion { get; set; }
        #endregion
    }
}
