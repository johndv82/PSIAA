using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataAccessLayer.TuartDB;
using PSIAA.DataTransferObject;
using System.Data;
using System.Reflection;

namespace PSIAA.BusinessLogicLayer
{
    public class AsignacionOrdenesBLL
    {
        private AsignacionOrdenesDAL _asignacionOrdenesDal = new AsignacionOrdenesDAL();
        private OperacionModeloDAL _operacionModeloDal = new OperacionModeloDAL();

        public DataTable ListarAsignaciones(string _codProveedor, string _moneda, string _fechaAprobPre)
        {
            return _asignacionOrdenesDal.SelectAsignacionOrdenes(_codProveedor, _moneda, _fechaAprobPre);
        }

        public List<string> ListarGrupoModelos(string codProv)
        {
            List<string> listModelos = new List<string>();
            listModelos.Add("<------TODOS------->");
            foreach (DataRow row in _asignacionOrdenesDal.SelectModelosAsignacionOrdenesPorAprobar(codProv).Rows)
            {
                listModelos.Add(row["modelo"].ToString());
            }
            return listModelos;
        }

        public DataTable ListarAsignacionesParaAprobar(string codProv, string modelo)
        {
            return _asignacionOrdenesDal.SelectAsignacionOrdenesParaAprobar(codProv, modelo);
        }

        public DataTable ListarDetalleProcesoAsignacionOrdenes(string _codProv, string nroAsig, string _orden, int _lote, int _categoria)
        {
            return _asignacionOrdenesDal.SelectDetalleGrupoAsignacionOrdenes(_codProv, nroAsig, _orden, _lote, _categoria);
        }

        public int ActualizarTarifaTiempo(string codProv, int catOperacion, string numAsignacion,
                                    string moneda, List<ProcesoPrecioDTO> listProcesoPrecio, string orden, int lote)
        {
            int actualizaciones = 0;
            foreach (ProcesoPrecioDTO procesoPrecio in listProcesoPrecio)
            {
                if (procesoPrecio.CategoriaOperacion != 0)
                {
                    actualizaciones += _asignacionOrdenesDal.UpdateTarifaTiempoAsignacionOrdenes(codProv, catOperacion, numAsignacion,
                                                                                        moneda, procesoPrecio.Tarifa, procesoPrecio.Tiempo,
                                                                                        procesoPrecio.Proceso, orden, lote);
                }
            }
            return actualizaciones;
        }

        public int RecalcularPrecios(string codProv, int catOperacion, string numAsignacion,
                                    string moneda, string fechaAprob, string usuarioAprob, bool aprobado,
                                    string orden, int lote)
        {
            int _aprob = aprobado ? 1 : 0;
            int actualizaciones = _asignacionOrdenesDal.UpdatePrecioAsignacionesOrdenes_RE(codProv, catOperacion, numAsignacion,
                                                                                        moneda, fechaAprob, usuarioAprob,
                                                                                        _aprob, orden, lote);
            return actualizaciones;
        }

        //Metodo que devuelve el numero de asignacion siguiente
        public string NroDeOrdenAsignacion()
        {
            string ultimoNumero = _asignacionOrdenesDal.SelectUltimoNumeroOrden();
            long nuevoNumero = long.Parse(ultimoNumero) + 1;
            return nuevoNumero.ToString();
        }

        //Metodos para ingresar detalle y cabecera de Asignaciones
        public int[] IngresarAsignacionOrden(List<AasignarDTO> listAasignar, List<LanzamientoDetDTO> listLanzamientoDet, string user)
        {
            //List<AsignacionOrdenDetDTO> _listAsigOrdenDet = new List<AsignacionOrdenDetDTO>();
            string nroAsignacion = NroDeOrdenAsignacion();
            int filasInsertCab = 0;
            int filasInsertDet = 0;

            foreach (var aAsignar in listAasignar)
            {
                if (aAsignar.Asignacion == "Si") {
                    int[] subcategoria;
                    int categoriaOperacion = 0;
                    if (aAsignar.CodCatOperacion == 500)
                    {
                        subcategoria = new int[] { 510, 530, 550 };
                        categoriaOperacion = 500;
                    }
                    else if (aAsignar.CodCatOperacion == 400)
                    {
                        subcategoria = new int[] { 430, 440, 450, 460, 470 };
                        categoriaOperacion = 400;
                    }
                    else
                    {
                        subcategoria = new int[] { aAsignar.CodCatOperacion };
                        categoriaOperacion = int.Parse(aAsignar.CodCatOperacion.ToString().Substring(0, 1) + "00");
                    }

                    foreach (int subcat in subcategoria)
                    {
                        foreach (var lanzDet in listLanzamientoDet)
                        {
                            if ((lanzDet.Modelo == aAsignar.Modelo) && (lanzDet.Color == aAsignar.Color))
                            {
                                foreach (int proceso in ProcesosPorModeloCategoria(lanzDet.Modelo, subcat))
                                {
                                    var asigOrdenDet = new AsignacionOrdenDetDTO()
                                    {
                                        CodCatOperacion = categoriaOperacion,
                                        NroAsignacion = nroAsignacion,
                                        Orden = lanzDet.Orden,
                                        Lote = lanzDet.Lote,
                                        Categoria = subcat,
                                        Proceso = proceso,
                                        CodProveedor = aAsignar.CodProveedor,
                                        FechaTermino = aAsignar.FechaRetorno,
                                        //0X46 = 70
                                        Terminado = 70,
                                        Color = lanzDet.Color,
                                        Tallas = lanzDet.Tallas,
                                        Cantidades = lanzDet.Piezas,
                                        Usuario = user
                                    };
                                    //_listAsigOrdenDet.Add(asigOrdenDet);
                                    int insDet = _asignacionOrdenesDal.InsertAsignacionOrdenDetalle(asigOrdenDet);
                                    filasInsertDet = filasInsertDet + insDet;
                                }
                            }
                        }
                    }
                    //Generar Cabecera de Asignacion de Ordenes
                    var _asigOrdenCab = new AsignacionOrdenCabDTO()
                    {
                        CodCatOperacion = categoriaOperacion,
                        NroAsignacion = nroAsignacion,
                        CodProveedor = aAsignar.CodProveedor,
                        FechaEntrega = aAsignar.FechaRetorno,
                        Completo = aAsignar.TodasOperaciones == true ? (short)1 : (short)0,
                        Usuario = user
                    };
                    int insCab = _asignacionOrdenesDal.InsertAsignacionOrdenCabecera(_asigOrdenCab);
                    filasInsertCab = filasInsertCab + insCab;

                    //Generar siguiente numero de asignacion
                    nroAsignacion = (long.Parse(nroAsignacion) + 1).ToString();
                }
            }
            //DataTable dt = Helper.ToDataTable(_listAsigOrdenDet);
            return new int[] { filasInsertCab, filasInsertDet};
        }

        private int[] ProcesosPorModeloCategoria(string modelo, int categoria)
        {
            DataTable dtOperModelo = _operacionModeloDal.SelectOperacionesTiempo(modelo);
            int[] procesos;
            if (dtOperModelo.Rows.Count > 0)
            {
                procesos = (from c in dtOperModelo.AsEnumerable()
                            where c.Field<long>("i_idcatope").Equals(categoria)
                            select int.Parse(c.Field<long>("i_idope").ToString())).ToArray();
            }
            else
            {
                procesos = new int[0];
            }
            return procesos;
        }
    }
}
