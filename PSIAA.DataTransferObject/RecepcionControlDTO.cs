using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class RecepcionControlDTO
    {
        /** Atributos */
        private string[] _tallas = new string[9];
        private int[] _piezas = new int[9];
        private DateTime _fechaIngreso;
        private string _horaIngreso;
        private int _piezaDeCambio;

        /** Propiedades */
        public RecepcionControlDTO() {
            _fechaIngreso = DateTime.Now;
            _horaIngreso = string.Concat(DateTime.Now.ToString("HHmmss"));
        }
        public int Almacen { get; set; }
        public string Orden { get; set; }
        public int Lote { get; set; }

        public string[] Tallas {
            get { return _tallas;  }
            set { _tallas = value;  }
        }
        public int[] Piezas {
            get {
                for(int i=0; i<_piezas.Length; i++) {
                    if (_piezas[i] != 0 & _piezaDeCambio != 0)
                        _piezas[i] = _piezaDeCambio;
                }
                return _piezas;
            }
            set { _piezas = value;  }
        }
        public string ValorTalla {
            get {
                string _talla = "";
                for(int x = 0; x < _piezas.Length; x++) {
                    if (_piezas[x] != 0)
                        _talla =  _tallas[x].ToString();
                }
                return _talla;
            }
        }
        public int ValorPieza{
            get{
                return _piezas.Sum();
            }
        }
        public string Usuario { get; set; }
        public DateTime FechaIngreso { get { return _fechaIngreso; } }
        public string HoraIngreso { get { return _horaIngreso; } set { _horaIngreso = value; } }
        public decimal Peso { get; set; }
        public string Observaciones { get { return string.Empty;  } }
        public char Completo { get; set; }
        public int PiezaDeCambio { set { _piezaDeCambio = value;  } }
    }
}
