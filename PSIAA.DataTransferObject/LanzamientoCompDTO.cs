using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula los campos principales de la tabla Lanzamiento Compuesto.
    /// </summary>
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

        /// <summary>
        /// Propiedad asignada al campo "contrato" de la tabla, que en si representa el código de Maquina.
        /// </summary>
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
