﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSIAA.DataTransferObject
{
    /// <summary>
    /// Objeto de Transferencia que encapsula los campos principales de la tabla Asignacion Ordenes Det
    /// </summary>
    public class AsignacionOrdenDetDTO
    {
        #region Atributos
        private DateTime _fechaAsignacion;
        private string[] _tallas = new string[9];
        private int[] _cantidades = new int[9];
        private string _horaIngreso;
        #endregion

        #region Constructores
        public AsignacionOrdenDetDTO() {
            _fechaAsignacion = DateTime.Now;
            _horaIngreso = string.Concat(DateTime.Now.ToString("HHmmss"));
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Campo Categoria Operacion
        /// </summary>
        public int CodCatOperacion { get; set; }
        public string NroAsignacion { get; set; }
        public string Orden { get; set; }
        public int Lote { get; set; }
        public int Categoria { get; set; }
        public int Proceso { get; set; }
        public string CodProveedor { get; set; }
        public DateTime FechaAsignacion {
            get { return _fechaAsignacion; }
        }

        /// <summary>
        /// Campo de Fecha de Termino
        /// 0x54: Si
        ///     Valor Entero: 84
        /// 0X46 : No
        ///     Valor Entero: 70
        /// </summary>
        public DateTime FechaTermino { get; set; }
        public int Activo { get { return 84; } }
        public int Terminado { get; set; }
        public string Color { get; set; }
        public string[] Tallas {
            get { return LimitarLongitudCaracteresTalla(_tallas); }
            set { _tallas = value; }
        }

        public int[] Cantidades{
            get { return _cantidades; }
            set { _cantidades = value; }
        }

        public DateTime FechaFinalizacion { get { return new DateTime(1901, 01, 01);  } }
        public string HoraIngreso {
            get { return _horaIngreso; }
        }

        public string Usuario { get; set; }
        #endregion

        #region Metodos
        private string[] LimitarLongitudCaracteresTalla(string[] tallas)
        {
            string[] nuevasTallas = new string[9];
            int indice = 0;
            foreach (string talla in tallas)
            {
                string t = string.Empty;
                if (talla.Length > 6)
                    t = talla.Substring(0, 6);
                else
                    t = talla;
                nuevasTallas[indice] = t;
                indice++;
            }
            return nuevasTallas;
        }
        #endregion
    }
}
