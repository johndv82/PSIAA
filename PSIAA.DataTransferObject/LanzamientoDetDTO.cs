using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class LanzamientoDetDTO
    {
        #region Atributos

        private string[] _tallas = new string[9];
        private int[] _piezas = new int[9];
        private decimal[] _kilos = new decimal[9];
        private DateTime _fechaIngreso;
        private string _horaIngreso;

        #endregion

        public LanzamientoDetDTO() {
            _fechaIngreso = DateTime.Now;
            _horaIngreso = string.Concat(DateTime.Now.ToString("HHmmss"));
        }

        #region Propiedades
        public int NumDocumento { get; set; }
        public short NumLanzamiento { get; set; }
        public string Orden { get; set; }
        public short Lote { get; set; }
        //Maquina -> contrato(DB)
        public string Maquina { get; set; }
        public string Color { get; set; }
        public string Modelo { get; set; }
        public string CodProducto { get; set; }
        public string[] Tallas {
            get { return LimitarLongitudCaracteresTalla(_tallas); }
            set { _tallas = value;  }
        }
        public int[] Piezas {
            get { return _piezas; }
            set { _piezas = value;  }
        }
        public decimal[] Kilos {
            get { return _kilos;  }
            set { _kilos = value;  }
        }
        public string Usuario { get; set; }
        public DateTime FechaIngreso {
            get { return _fechaIngreso; }
        }
        public string HoraIngreso {
            get { return _horaIngreso;  }
        }
        public decimal KilosTot { get; set; }
        public DateTime FechaSolicitud { get; set; }

        #endregion

        #region Metodos
        public static string[] LimitarLongitudCaracteresTalla(string[] tallas)
        {
            string[] nuevasTallas = new string[9];
            int indice = 0;
            foreach (string talla in tallas)
            {
                string t = string.Empty;
                if (talla.Length > 6)
                    t = talla.Substring(0, 6);
                else
                    t = talla;
                nuevasTallas[indice] = t;
                indice++;
            }
            return nuevasTallas;
        }
        #endregion
    }
}
