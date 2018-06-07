using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class ProcesoPrecioDTO
    {
        private int _cantidad;
        private double _tarifa = 0;
        public int Proceso { get; set; }
        public int NumeroOrden { get; set; }
        public int CategoriaOperacion { get; set; }
        public string Descripcion { get; set; }
        public double Tiempo { get; set; }
        public string Moneda { get; set; }
        public int Cantidad {
            set { _cantidad = value; }
        }
        public double Tarifa {
            get { return _tarifa; }
            set { _tarifa = value;  }
        }
        public double CostoSoles {
            get {
                double costo = 0;
                if (Moneda == "S") {
                    costo = Math.Round(((Tiempo / 60) * _tarifa) * _cantidad, 3);
                }
                return costo;
            }
        }
        public double CostoDolares
        {
            get
            {
                double costo = 0;
                if (Moneda == "D") {
                    costo = Math.Round(((Tiempo / 60) * _tarifa) * _cantidad, 3);
                }
                return costo;
            }
        }
    }
}
