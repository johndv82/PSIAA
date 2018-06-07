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

        public DataTable ListarSeguimientoRecepcionControl(string _orden)
        {
            if (!string.IsNullOrEmpty(_orden))
                return _recepcionControlDal.SelectSeguimientoRecepcionControl(_orden);
            else
                return new DataTable();
        }

        public int DiferenciaConPuntoAnterior(string _orden, int _lote, int _puntoActual, int _puntoAnterior) {
            int _diferencia;
            int cantidadPermitida = _recepcionControlDal.SelectCantidadRecepcion(_orden, _lote, _puntoAnterior);
            int cantidadIngresada = _recepcionControlDal.SelectCantidadRecepcion(_orden, _lote, _puntoActual);

            _diferencia = cantidadPermitida - cantidadIngresada;
            return _diferencia;
        }

        public DataTable ListarIngresosFaltantesAlmacen(int _contrato, string _modelo) {
            return _recepcionControlDal.SelectIngresosFaltantesAlmacen(_contrato, _modelo);
        }

        public DataTable ListarIngresosPuntoControl(string _fechaIni, string _fechaFin) {
            return _recepcionControlDal.SelectIngresosPuntoControl(_fechaIni, _fechaFin);
        }
    }
}
