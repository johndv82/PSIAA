using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PSIAA.DataAccessLayer;
using PSIAA.DataTransferObject;

namespace PSIAA.BusinessLogicLayer
{
    public class RecepcionControlBLL
    {
        private RecepcionControlDAL _recepcionControlDal = new RecepcionControlDAL();
        private LanzamientoDAL _lanzamientoDal = new LanzamientoDAL();
        private ContratoDAL _contratoDal = new ContratoDAL();

        /// <summary>
        /// Devuelve una lista en base al contenedor de datos que retorna el procedimiento DAL de Ingresos en Recepción Control del día.
        /// </summary>
        /// <param name="almacen">Código de Almacen</param>
        /// <returns>Lista Genérica de tipo RecepcionControlDTO</returns>
        /// 
        public List<RecepcionControlDTO> ListarRecepcionControl(int almacen)
        {
            List<RecepcionControlDTO> _listRecepcionControl = new List<RecepcionControlDTO>();
            foreach (DataRow row in _recepcionControlDal.SelectRecepcionControl(almacen).Rows) {
                object[] _tallas = row.ItemArray.Skip(2).Take(9).ToArray();
                object[] _piezas = row.ItemArray.Skip(11).Take(9).ToArray();
                RecepcionControlDTO _recepcionControlDto = new RecepcionControlDTO
                {
                    Almacen = almacen,
                    Orden = row["Orden"].ToString().Trim(),
                    Lote = int.Parse(row["Lote"].ToString()),
                    Tallas = _tallas.Cast<string>().ToArray(),
                    Piezas = _piezas.Cast<int>().ToArray(),
                    Completo = char.Parse(row["Completo"].ToString()),
                    Usuario = row["Usuario"].ToString(),
                    HoraIngreso = row["Hora"].ToString()
                };
                _listRecepcionControl.Add(_recepcionControlDto);
            }

            return _listRecepcionControl;
        }

        /// <summary>
        /// Devuelve una colección de 4 claves: Talla, Cantidad, Modelo, Color. Pobladas a partir del contenedr de datos que 
        /// retorn el procedimiento DAL de lanzamiento por orden y lote.
        /// </summary>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <returns>Colección de tipo Dictionary [string , string] con datos en clave y valor</returns>
        /// 
        public Dictionary<string, string> ListarCamposRecepcionControl(string _orden, int _lote)
        {
            Dictionary<string, string> _valores = new Dictionary<string, string>();
            DataRow drResultLanzamiento;

            int[] piezas = new int[9];
            string _talla = "";

            try
            {
                drResultLanzamiento = _lanzamientoDal.SelectLanzamientoPorOrden(_orden, _lote).Rows[0];
                for (int col = 13; col < drResultLanzamiento.ItemArray.Length; col++)
                {
                    piezas[col -13] = int.Parse(drResultLanzamiento[col].ToString());
                }

                for (int i = 0; i < piezas.Length; i++)
                {
                    if (piezas[i] != 0)
                        _talla = drResultLanzamiento["talla" + (i + 1).ToString()].ToString();
                }
                _valores.Add("Talla", _talla.Trim());
                _valores.Add("Cantidad", piezas.Sum().ToString());
                _valores.Add("Modelo", drResultLanzamiento["Modelo"].ToString());
                _valores.Add("Color", drResultLanzamiento["Color"].ToString());
                return _valores;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Construye un objeto de tipo RecepcionControlDTO  y lo envia al procedimiento DAL de inserción de datos en la tabla
        /// RecepcionPtoControl. Las tallas y cantidad de piezas son extraidas del metodo DAL de lanzamiento por orden y lote.
        /// </summary>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <param name="_pieza">Cantidad de piezas a ingresar</param>
        /// <param name="_user">Nombre usuario que realiza el ingreso</param>
        /// <returns>Valor verdadero/falso segun el ingreso se haya realizado con éxito</returns>
        /// 
        public bool IngresarRecepcionControl(string _orden, int _lote, int _pieza, string _user)
        {
            object[] TallasPiezas = _lanzamientoDal.SelectLanzamientoPorOrden(_orden, _lote).Rows[0].ItemArray;
            object[] _tallas = TallasPiezas.Skip(4).Take(9).ToArray();
            object[] _piezas = TallasPiezas.Skip(13).Take(9).ToArray();
            int _cantidad = _piezas.Cast<int>().ToArray().Sum();

            RecepcionControlDTO _recepcionControlDto = new RecepcionControlDTO()
            {
                Almacen = 550,
                Orden = _orden.Trim(),
                Lote = int.Parse(_lote.ToString()),
                Tallas = _tallas.Cast<string>().ToArray(),
                Piezas = _piezas.Cast<int>().ToArray(),
                Completo = _cantidad == _pieza ? 'S' : 'N',
                Peso = 0,
                PiezaDeCambio = _cantidad == _pieza ? 0 : _pieza,
                Usuario = _user
            };

            if (_recepcionControlDal.InsertRecepcionControl(_recepcionControlDto) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Ejecuta procedimiento DAL del seguimiento de recepcion por punto de control y los retorna en un contenedor de datos.
        /// En caso de que la Orden sea nula, retorna un contenedor vacio.
        /// </summary>
        /// <param name="_orden">Orden de Producción</param>
        /// <returns>Contenedor de tipo DataTable con el seguimiento</returns>
        /// 
        public DataTable ListarSeguimientoRecepcionControl(string _orden)
        {
            if (!string.IsNullOrEmpty(_orden))
                return _recepcionControlDal.SelectSeguimientoRecepcionControl(_orden);
            else
                return new DataTable();
        }

        /// <summary>
        /// Evalúa la diferencia de piezas que se ingresó en el punto actual, con lo que se ingreso en el punto de control anterior.
        /// </summary>
        /// <example>
        /// Punto: 800
        /// Punto Anterior: 550
        /// Piezas ingresadas = 2
        /// Piezas ingresaadas en el punto anterior: 10
        /// Piezas permitidas = (10 -2) = 8
        /// </example>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <param name="_puntoActual">Punto de Control Actual</param>
        /// <param name="_puntoAnterior">Punto de Control Anterior</param>
        /// <returns>Valor Entero con la cantidad permitida</returns>
        /// 
        public int DiferenciaConPuntoAnterior(string _orden, int _lote, int _puntoActual, int _puntoAnterior) {
            int _diferencia;
            int cantidadPermitida = _recepcionControlDal.SelectCantidadRecepcion(_orden, _lote, _puntoAnterior);
            int cantidadIngresada = _recepcionControlDal.SelectCantidadRecepcion(_orden, _lote, _puntoActual);

            _diferencia = cantidadPermitida - cantidadIngresada;
            return _diferencia;
        }

        /// <summary>
        /// Ejecuta procedimiento DAL de Ingresos Faltantes a Almacen, y lo devuelve en un conteneder de datos.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <param name="_modelo">Modelo de Prenda</param>
        /// <returns>Contenedor de datos de tipo DataTable con los ingresos</returns>
        /// 
        public DataTable ListarIngresosFaltantesAlmacen(int _contrato, string _modelo) {
            return _recepcionControlDal.SelectIngresosFaltantesAlmacen(_contrato, _modelo);
        }

        /// <summary>
        /// Ejecuta procedimiento DAL de Ingresos por Punto de Control, y lo devuelve en un contenedor de datos.
        /// </summary>
        /// <param name="_fechaIni">Fecha Inicio de consulta</param>
        /// <param name="_fechaFin">Fecha Fin de consulta</param>
        /// <returns>Contenedor de datos de tipo DataTable con los ingresos</returns>
        /// 
        public DataTable ListarIngresosPuntoControl(string _fechaIni, string _fechaFin) {
            return _recepcionControlDal.SelectIngresosPuntoControl(_fechaIni, _fechaFin);
        }
    }
}
