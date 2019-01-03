using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject.Report
{
    /// <summary>
    /// Objeto de Tranferencia que encapsula el detalle del Reporte de Contrato.
    /// </summary>
    public class ContratoDetDTO
    {
        public string ModeloAA { get; set; }
        public string ModeloSAP { get; set; }
        public string ModeloCliente { get; set; }
        public string Descripcion { get; set; }
        public string Material { get; set; }
        public string Titulo { get; set; }
        public string Maquina { get; set; }
        public string Galga { get; set; }
        public string CodColor { get; set; }
        public string Color { get; set; }
        public int CTalla1 { get; set; }
        public int CTalla2 { get; set; }
        public int CTalla3 { get; set; }
        public int CTalla4 { get; set; }
        public int CTalla5 { get; set; }
        public int CTalla6 { get; set; }
        public int CTalla7 { get; set; }
        public int CTalla8 { get; set; }
        public int CTalla9 { get; set; }
        public int CantMuestra { get; set; }
        public string TMuestra { get; set; }
        public int Total { get; set; }

        public string NTalla1 { get; set; }
        public string NTalla2 { get; set; }
        public string NTalla3 { get; set; }
        public string NTalla4 { get; set; }
        public string NTalla5 { get; set; }
        public string NTalla6 { get; set; }
        public string NTalla7 { get; set; }
        public string NTalla8 { get; set; }
        public string NTalla9 { get; set; }
    }
}
