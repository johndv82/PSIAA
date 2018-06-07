using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class SimulacionDetDTO
    {
        #region Atributos
        private decimal _porSeguridad = 0;
        private decimal _kilosSimulado = 0;
        #endregion
        #region Propiedades
        public int Correlativo { get; set; }
        public string NroSimulacion { get; set; }
        public string NombreMaquina { get; set; } 
        public string Color { get; set; }
        public string CodProducto { get; set; }
        public string DescProducto { get; set; }
        public string Modelo { get; set; }
        public decimal KilosSimulado {
            get { return _kilosSimulado; }
            set { _kilosSimulado = value; }
        }
        public decimal PorSeguridad {
            get { return _porSeguridad; }
            set { _porSeguridad = value; }
        }
        public decimal TotalKilos {
            get {
                if (_porSeguridad == 0)
                    return _kilosSimulado;
                else
                    return _kilosSimulado + (_kilosSimulado * (_porSeguridad / 100));
            }
        }
        public string KilosAlmacenados { get { return string.Empty; } }
        public DateTime FechaIngreso { get; set; }
        public string HoraIngreso { get; set; }
        public string Usuario { get; set; }
        public int NumContrato { get; set; }
        #endregion
    }
}
