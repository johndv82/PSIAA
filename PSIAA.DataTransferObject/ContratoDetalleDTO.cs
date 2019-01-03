using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula los campos principales de la tabla Contrato Detalle.
    /// </summary>
    public class ContratoDetalleDTO
    {
        #region Propiedades
        public int Item { get; set; }
        public int Numero { get; set; }
        public string Tipo { get; set; }
        public string ModeloAA { get; set; }
        public string ModeloCliente { get; set; }
        public string DescipcionModeloCli { get; set; }
        public string CodMaterial { get; set; }
        public string CodProducto { get; set; }
        public string CodColor { get; set; }
        public string ColorCliente { get; set; }
        public string GrupoTallas { get; set; }
        public string[] Tallas { get; set; }
        public int[] Cantidades { get; set; }
        public float PrecioUnitario { get; set; }
        public float PrecioTotal { get; set; }
        public double Adicional { get; set; }
        public float CodProveedor { get; set; }
        public float CodProveedor2 { get; set; }
        public string Linea { get; set; }
        public string Galga { get; set; }
        public string Titulo { get; set; }
        public int CantContramuestra { get; set; }
        public float PesoEstimado { get; set; }
        public string Observaciones { get; set; }

        #endregion
    }
}
