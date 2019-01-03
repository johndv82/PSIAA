using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula los campos principales de la tabla Liquidacion Talleres.
    /// </summary>
    public class LiquidacionTallerDTO
    {
        #region Atributos
        private DateTime _fechaCreacion;
        #endregion

        public LiquidacionTallerDTO()
        {
            _fechaCreacion = DateTime.Now;
        }
        #region Propiedades
        public int NroControl { get; set; }
        public string Periodo
        {
            get
            {
                return _fechaCreacion.ToString("yyyyMM");
            }
        }
        public string CodProveedor { get; set; }
        public string TipoMovimiento { get; set; }
        public int SerieDocumento { get; set; }
        public int NroDocumento { get; set; }
        public DateTime FechaDocumento
        {
            get { return _fechaCreacion; }
        }
        public string ConceptoCompra { get; set; }
        public int CIgv
        {
            get
            {
                return TipoMovimiento == "01" ? 1 : 0;
            }
        }
        public int CSol
        {
            get
            {
                return TipoMovimiento == "01" ? 0 : 1;
            }
        }

        public string Moneda { get; set; }
        public string FechaCancelacion
        {
            get
            {
                return "1901-01-01";
            }
        }
        public double MontoFacturaSoles { get; set; }
        public double MontoFacturaDolares { get; set; }
        public string TipoIgv {
            get {
                return TipoMovimiento == "01" ? "G" : "N";
            }
        }
        public double MontoIgvSoles {
            get {
                double igv = MontoFacturaSoles * 0.18;
                return TipoMovimiento == "01" ? igv : 0;
            }
        }
        public double MontoIgvDolares {
            get {
                double igv = MontoFacturaDolares * 0.18;
                return TipoMovimiento == "01" ? igv : 0;
            }
        }
        public string Usuario { get; set; }
        public string Glosa {
            get {
                return "PAGO SERVICIO DE TALLERES";
            }
        }
        public DateTime FechaContabilizacion {
            get {
                return _fechaCreacion;
            }
        }
        public double MontoTotalSoles {
            get {
                double total = (MontoFacturaSoles * 0.18) + MontoFacturaSoles;
                return TipoMovimiento == "01" ? total : MontoFacturaSoles;
            }
        }
        public double MontoTotalDolares
        {
            get
            {
                double total = (MontoFacturaDolares * 0.18) + MontoFacturaDolares;
                return TipoMovimiento == "01" ? total : MontoFacturaDolares;
            }
        }
        public DateTime FechaCreacion {
            get {
                return _fechaCreacion;
            }
        }
        public int Anio {
            get {
                return DateTime.Now.Year;
            }
        }
        public int Semana {
            get {
                Calendar c = CultureInfo.CurrentCulture.Calendar;
                return c.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            }
        }
        #endregion
    }
}
