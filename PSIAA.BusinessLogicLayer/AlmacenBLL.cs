using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using System.Data;
using PSIAA.DataTransferObject;

namespace PSIAA.BusinessLogicLayer
{
    public class AlmacenBLL
    {
        private RecepcionControlDAL _recepcionControlDal = new RecepcionControlDAL();
        private AlmacenDAL _almacenDal = new AlmacenDAL();
        private LanzamientoDAL _lanzamientoDal = new LanzamientoDAL();
        private List<RecepcionControlDTO> _listRecepcionControl = new List<RecepcionControlDTO>();
        private List<AlmacenDTO> _listAlmacen = new List<AlmacenDTO>();
        private ListXml _listXml = new ListXml();

        /// <summary>
        /// Devuelve el listado completo de ingresos almacenados en un archivo XML, del servidor.
        /// La conversion es realizada a una lista genérica tipada.
        /// </summary>
        /// <param name="_user">Nombre de usuario que realiza el ingreso</param>
        /// <returns>Lista genérica de Tipo RecepcionControlDTO poblada con todos los ingresos</returns>
        /// 
        public List<RecepcionControlDTO> ListarRecepcionControl(string _user)
        {
            return _listXml.ConvertXmlToListRecepcionControl(_user);
        }

        /// <summary>
        /// Agrega un nuevo elemento al contenido del archivo XML donde es almacenado cada ingreso, y devuelve el contenido completo.
        /// En el primer ingreso siempre se crea dos archivos XML por cada modelo de objeto: RecepcionControlDTO y AlmacenDTO.
        /// En el caso de que el ingreso ya existiera en el contenido del archivo Xml, solo devuelve el contenido completo.
        /// </summary>
        /// <param name="_orden">Orden de Producción de Ingreso</param>
        /// <param name="_lote">Número de Lote</param>
        /// <param name="_codAlmacen">Codigo de Almacén al cual va dirigido el ingreso</param>
        /// <param name="_pieza">Cantidad de piezas a ingresar</param>
        /// <param name="_talla">Talla de la pieza a ingresar</param>
        /// <param name="_user">Nombre de usuario que realiza el ingreso</param>
        /// <param name="duplicado">Parametro de Salida con valor verdadero o falso, en el caso de que el ingreso existiera en 
        /// el contenido del archivo XML</param>
        /// <returns>Lista genérica de tipo RecepcionControlDTO poblada con todos los ingresos</returns>
        /// 
        public List<RecepcionControlDTO> PoblarListasDeIngresoAlmacen(string _orden, int _lote, int _codAlmacen,
                                                                int _pieza, string _talla, string _user, out bool duplicado)
        {
            DataRow drResultLanzamiento = _lanzamientoDal.SelectLanzamientoPorOrden(_orden, _lote).Rows[0];

            object[] TallasPiezas = drResultLanzamiento.ItemArray;
            object[] _tallas = TallasPiezas.Skip(4).Take(9).ToArray();
            object[] _piezas = TallasPiezas.Skip(13).Take(9).ToArray();
            int _cantidad = _piezas.Cast<int>().ToArray().Sum();

            //COMPARAR REGISTROS DUPLICADOS
            _listRecepcionControl = _listXml.ConvertXmlToListRecepcionControl(_user);
            if (_listRecepcionControl.Find(x => (x.Orden == _orden)  && (x.Lote == _lote)) != null)
            {
                duplicado = true;
                return _listRecepcionControl;
            }
            else {
                RecepcionControlDTO _recepcionControlDto = new RecepcionControlDTO()
                {
                    Almacen = 800,
                    Orden = _orden.Trim(),
                    Lote = _lote,
                    Tallas = _tallas.Cast<string>().ToArray(),
                    Piezas = _piezas.Cast<int>().ToArray(),
                    Completo = _cantidad == _pieza ? 'S' : 'N',
                    //Peso = decimal.Parse(_result[2].ToString() == "" ? "0" : _result[2].ToString()),
                    Peso = 0,
                    PiezaDeCambio = _cantidad == _pieza ? 0 : _pieza,
                    Usuario = _user
                };

                //TALLAS PARA ALMACEN
                object[] _piezasAlmacen = _piezas.Take(7).ToArray();

                //LLENAR LISTA OBJETO ALMACEN
                AlmacenDTO _almacenDto = new AlmacenDTO()
                {
                    CodAlmacen = _codAlmacen,
                    TipoMovimiento = "IPRD",
                    //NumeroDocumento = "",
                    //Item = _item,
                    IngresoSalida = 1,
                    AlmacenOrigenDestino = 0,
                    CodProducto = "PT-" + drResultLanzamiento["Modelo"].ToString().Trim() +
                                    "-" + drResultLanzamiento["Color"].ToString().Trim() +
                                    "-" + _talla.Trim() + "/" +
                                    drResultLanzamiento["Contrato"].ToString(),
                    Orden = _orden.Trim(),
                    NroLote = _lote.ToString(),
                    Contrato = drResultLanzamiento["Contrato"].ToString(),
                    Tallas = _piezasAlmacen.Cast<int>().ToArray(),
                    Cantidad = _cantidad == _pieza ? _cantidad : _pieza,
                    PesoBruto = _cantidad == _pieza ? _cantidad : _pieza,
                    PesoNeto = _cantidad == _pieza ? _cantidad : _pieza,
                    TallaDeCambio = _cantidad == _pieza ? 0 : _pieza
                };

                //CONVERTIR XML - LISTA
                _listXml.AgregarAlmacenToXML(_almacenDto, _user);
                _listXml.AgregarRecepcionControlToXML(_recepcionControlDto, _user);
                duplicado = false;
                return _listXml.ConvertXmlToListRecepcionControl(_user);
            }
        }

        /// <summary>
        /// Genera un listado de Almacenes con su codigo respectivo.
        /// </summary>
        /// <returns>Colección de Tipo [int, string] poblada con todos los almacenes consultados</returns>
        /// 
        public Dictionary<int, string> ListarAlmacenes()
        {
            Dictionary<int, string> _dicLista = new Dictionary<int, string>();
            _dicLista.Add(0, "<---- SELECCIONAR ---->");
            foreach (DataRow dr in _almacenDal.SelectAlmacenes90_98().Rows)
            {
                _dicLista.Add(int.Parse(dr["Codigo"].ToString()), dr["Almacen"].ToString());
            }
            return _dicLista;
        }

        /// <summary>
        /// Envía los ingresos contenidos en los arhivos XML a la capa de Acceso a Datos para su respectiva inserción en la BD.
        /// Los archivos XML son convetidos en listas genericas de tipo RecepcionControlDTO, AlmacenDTO y modificados previamente a su
        /// registro, añadiendo su numero de parte y respectivo numero de item.
        /// </summary>
        /// <param name="_numParte">Parametro de salida que es devuelta al CALLER, con número de parte que se ingresó.</param>
        /// <param name="_user">Nombre de Usuario que realiza el ingreso de piezas</param>
        /// <returns>Valor verdadero/falso segun el ingreso se haya ejecutado correctamente</returns>
        /// 
        public bool IngresarDetalleAlmacen(out string _numParte, string _user)
        {
            //POBLAR LISTS
            _listRecepcionControl = _listXml.ConvertXmlToListRecepcionControl(_user);
            _listAlmacen = _listXml.ConvertXmlToListAlmacen(_user);

            List<int> _returns = new List<int>();
            _listAlmacen = _listAlmacen.OrderBy(c => c.CodAlmacen).ToList();
            string _nuevoDocumento = "";
            try
            {
                //ACTUALIZAMOS CAMPOS DEL LISTADO DE ALMACENES
                int _item = 1;
                int _codAlmacenAnterior = 0;
                string ultimoDocumento = "";

                ultimoDocumento = _almacenDal.SelectUltimoDocumentoDeHoy();
                if (!string.IsNullOrEmpty(ultimoDocumento))
                {
                    int ultimoDigito = int.Parse(ultimoDocumento.Substring(ultimoDocumento.Length - 2, 2));
                    ultimoDigito++;
                    _nuevoDocumento = ultimoDocumento.Substring(0, ultimoDocumento.Length - 2) + Helper.Mascara(ultimoDigito, "00");
                }
                //SI NO HAY PARTES DE HOY, CREAR UNO NUEVO
                else
                {
                    _nuevoDocumento = string.Concat(DateTime.Now.ToString("yyyyMMdd") + "01");
                }

                foreach (AlmacenDTO _alm in _listAlmacen)
                {
                    if (_item == 1)
                    {
                        _codAlmacenAnterior = _alm.CodAlmacen;
                    }
                    if (_codAlmacenAnterior != _alm.CodAlmacen)
                    {
                        _codAlmacenAnterior = _alm.CodAlmacen;
                        _item = 1;
                    }
                    _alm.NumeroDocumento = _nuevoDocumento;
                    _alm.Item = _item;
                    //INSERTAMOS ALMACEN
                    int fila = _almacenDal.InsertDetalleAlmacen(_alm);
                    _returns.Add(fila);
                    _item++;
                }

                foreach (RecepcionControlDTO _recepcion in _listRecepcionControl)
                {
                    int fila = _recepcionControlDal.InsertRecepcionControl(_recepcion);
                    _returns.Add(fila);
                }

                if (_returns.Count == (_listAlmacen.Count + _listRecepcionControl.Count))
                {
                    _listXml.LimpiarArchivosXml(_user);
                    //Limpiar Listas para evitar duplicidad
                    _listAlmacen.Clear();
                    _listRecepcionControl.Clear();
                    _numParte = _nuevoDocumento;
                    return true;
                }
                else
                {
                    _numParte = "";
                    return false;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message.ToString());
                _numParte = "";
                return false;
            }
        }

        /// <summary>
        /// Ejecuta procedimiento DAL de todos los ingresos de producción en almacén y los retorna en un contenedor de datos.
        /// </summary>
        /// <param name="_codAlmacen">Código de Almacen SIAA</param>
        /// <param name="_fecha">Fecha de Ingreso en formato (Año-Mes-Dia)</param>
        /// <returns>Contenedor de tipo DateTable con los ingresos</returns>
        /// 
        public DataTable ListarIngresosProduccion(int _codAlmacen, string _fecha = "")
        {
            return _almacenDal.SelectIngresosProduccion(_codAlmacen, _fecha);
        }

        /// <summary>
        /// Ejcuta un Helper (ListXml) para limpiar los ingresos realizados por el usuario.
        /// </summary>
        /// <param name="_user">Nombre Usuario que realiza la acción</param>
        /// 
        public void LimpiarListasDeControl(string _user)
        {
            _listXml.LimpiarArchivosXml(_user);
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
        /// Busca el ingreso en el ListadoAlmacenDTO de tipo XML archivado en el servidor, y pobla su objeto de tipo AlmacenDTO.
        /// </summary>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <param name="_user">Nombre de Usuario que realiza la acción</param>
        /// <returns>Objeto de tipo AlmacenDTO con los datos de ingreso.</returns>
        /// 
        public AlmacenDTO DetalleAlmacen(string _orden, int _lote, string _user)
        {
            List<AlmacenDTO> _lista = new List<AlmacenDTO>();
            _lista = _listXml.ConvertXmlToListAlmacen(_user);
            AlmacenDTO _almacen = (from a in _lista
                         where a.Orden == _orden
                         & int.Parse(a.NroLote) == _lote
                         select a).FirstOrDefault();
            return _almacen;
        }

        /// <summary>
        /// Ejecuta procedimiento DAL de los Ingresos a Almacen, y los retorna en un contenedor de datos.
        /// </summary>
        /// <param name="_fechaIni">Fecha de Inicio de consulta</param>
        /// <param name="_fechaFin">Fecha Fin de consulta</param>
        /// <param name="_modelo">Modelo de consulta</param>
        /// <returns>Contenedor de tipo DataTable con los ingresos</returns>
        /// 
        public DataTable ListarIngresosAlmacen(string _fechaIni, string _fechaFin, string _modelo)
        {
            _fechaIni = _fechaIni == "" ? "2015-01-01" : _fechaIni;
            _fechaFin = _fechaFin == "" ? DateTime.Now.ToString("yyyy-MM-dd") : _fechaFin;
            return _almacenDal.SelectIngresosAlmacen(_fechaIni, _fechaFin, _modelo);
        }
    }
}