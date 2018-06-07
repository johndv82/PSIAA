using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class DocumentoPagoLibreDTO
    {
        #region Atributos
        private double _precio;
        private int _prendas;
        private double _tiempo;
        #endregion

        #region Propiedades
        public string CodProveedor { get; set; }
        public string TipoMovimiento { get; set; }
        public int NroDocumento { get; set; }
        public string Orden { get; set; }
        public int Lote { get; set; }
        public string CodOperacion { get; set; }
        public string DenominacionOper { get; set; }
        public string Talla { get; set; }
        public int Prendas
        {
            get { return _prendas; }
            set { _prendas = value; }
        }
        public double Tiempo {
            get { return _tiempo; }
            set { _tiempo = value; }
        }
        public string Moneda { get; set; }
        public double Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }
        public double Total
        {
            get
            {
                double _total = ((_tiempo / 60) * _precio) * _prendas;
                return _total;
            }
        }
        public string Observaciones { get; set; }
        #endregion
    }
}
