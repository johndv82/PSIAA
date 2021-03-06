﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula los campos principales de la tabla ITSM_Usuarios.
    /// </summary>
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string User { get; set; }
        public string Correo { get; set; }
    }
}
