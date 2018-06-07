using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class LanzamientoCompDTO
    {
        private DateTime _fechaIngreso;
        private DateTime _fechaAtencion;
        private string _horaIngreso;

        public LanzamientoCompDTO() {
            _fechaIngreso = DateTime.Now;
            _horaIngreso = string.Concat(DateTime.Now.ToString("HHmmss"));
        }

        public int NumContrato { get; set; }
        public int NumLanzamiento { get; set; }
        public string Orden { get; set; }
        //contrato (campo BD)
        public char Maquina { get; set; }
        public string Color { get; set; }
        public string Calidad { get; set; }
        public int Secuencia { get; set; }
        public string Modelo { get; set; }
        public string CodProducto { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaIngreso {
            get { return _fechaIngreso;  }
            set { _fechaIngreso = value;  }
        }
        public DateTime FechaAtencion {
            get { return _fechaAtencion; }
            set { _fechaAtencion = value; }
        }
        public string HoraIngreso {
            get { return _horaIngreso; }
            set { _horaIngreso = value;  }
        }
        public string CodProductoSolicitado { get; set; }
        public decimal Kilos { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
