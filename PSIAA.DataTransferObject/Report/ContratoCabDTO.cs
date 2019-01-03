using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject.Report
{
    /// <summary>
    /// Objeto de Transferencia que encapsula la cabecera del Reporte de Contrato.
    /// </summary>
    public class ContratoCabDTO
    {
        public string _msnError;
        public ContratoCabDTO()
        {
        }

        public ContratoCabDTO(string msn)
        {
            //Campo de carga de errores
            _msnError = msn;
        }
        private List<ContratoDetDTO> _detalle = new List<ContratoDetDTO>();
        public string NumContrato { get; set; }
        public string TipoContrato { get; set; }
        public string Fecha { get; set; }
        public string Cliente { get; set; }
        public string Orden { get; set; }
        public string FechaEnvio { get; set; }
        public string Tolerancia { get; set; }
        public int ToleranciaTiempo { get; set; }
        public string HojaL { get; set; }
        public string Moneda { get; set; }
        public string Destino { get; set; }
        public string Observaciones { get; set; }

        /// <summary>
        /// Lista Generica de Tipo ContratoDetDTO que sirve como contenedor del detalle del Contrato.
        /// </summary>
        public List<ContratoDetDTO> Detalle
        {
            get
            {
                return _detalle;
            }
            set
            {
                _detalle = value;
            }
        }
    }
}
