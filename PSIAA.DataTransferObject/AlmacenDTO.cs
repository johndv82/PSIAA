using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula los campos principales de la tabla DetalleAlmacen.
    /// </summary>
    public class AlmacenDTO
    {
        /** Atributos */
        private int _tallaDeCambio;
        private string _nroLote;
        private int[] _tallas = new int[7];

        /** Propiedades */
        public int CodAlmacen { get; set; }
        public string TipoMovimiento { get; set; }
        public string NumeroDocumento { get; set; }
        public int Item { get; set; }
        public int IngresoSalida { get; set; }
        public int AlmacenOrigenDestino { get; set; }
        public string CodProducto { get; set; }
        public string Orden { get; set; }
        public string NroLote
        {
            get
            {
                string cadena = "000";
                int largoId = _nroLote.ToString().Length;
                return cadena.Substring(0, cadena.Length - largoId) + _nroLote.ToString();
            }
            set { _nroLote = value; }
        }
        public string Contrato { get; set; }
        public int[] Tallas {
            get
            {
                for (int i = 0; i < _tallas.Length; i++)
                {
                    if (_tallas[i] != 0 && _tallaDeCambio != 0)
                        _tallas[i] = _tallaDeCambio;
                }
                return _tallas;
            }
            set { _tallas = value; }
        }
        public int Cantidad { get; set; }
        public int PesoBruto { get; set; }
        public int PesoNeto { get; set; }

        public int TallaDeCambio { set { _tallaDeCambio = value; } }
    }
}
