using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula los campos principales de la tabla ITSM_Paginas.
    /// </summary>
    public class PaginaDTO
    {
        public string Pagina { get; set; }
        public string Target { get; set; }
        public string Nombre { get; set; }
        public string Padre { get; set; }
    }
}
