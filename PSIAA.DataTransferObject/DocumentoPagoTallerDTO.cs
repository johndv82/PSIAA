using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    public class DocumentoPagoTallerDTO
    {
        #region Propiedades
        public string CodProveedor { get; set; }
        public string TipoDocumento { get; set; }
        public int SerieDocumento { get; set; }
        public int NroDocumento { get; set; }
        public int CategoriaOperacion { get; set; }
        public string NumOrdenAsignacion { get; set; }
        public string Orden { get; set; }
        public int Lote { get; set; }
        public int Categoria { get; set; }
        public int CodProceso { get; set; }
        public double MontoFacturacionSoles { get; set; }
        public double MontoFacturacionDolares { get; set; }
        #endregion
    }
}
