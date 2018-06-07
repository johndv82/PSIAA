using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataAccessLayer.TuartDB;
using PSIAA.DataTransferObject;
using System.Data;

namespace PSIAA.BusinessLogicLayer
{
    public class AprobacionPrecioBLL
    {
        private AsignacionOrdenesDAL _asigOrdenesDal = new AsignacionOrdenesDAL();
        private OperacionModeloDAL _operacionModeloDal = new OperacionModeloDAL();

        public List<ProcesoPrecioDTO> ListarPreciosDeProcesos(string codProveedor, string modelo, int _cantidad,
                                                                string _moneda, string _asignacion, double _tarifa,
                                                                string orden, int lote)
        {
            DataTable dtProcesos = _asigOrdenesDal.SelectProcesosAsignacion(codProveedor, modelo, _asignacion, orden, lote);
            DataTable dtProcesosTiempos = _operacionModeloDal.SelectOperacionesTiempo(modelo);

            var list = (from procesos in dtProcesos.AsEnumerable()
                        join tiempos in dtProcesosTiempos.AsEnumerable()
                            on procesos.Field<int>("Proceso")
                            equals tiempos.Field<long>("i_idope")
                        into outer
                        from tiempos in outer.DefaultIfEmpty()
                        select new ProcesoPrecioDTO
                        {
                            Proceso = procesos.Field<int>("Proceso"),
                            NumeroOrden = (tiempos == null) ? 100 : (int)tiempos.Field<long>("i_numord"),
                            CategoriaOperacion = (tiempos == null) ? 0 : (int)tiempos.Field<long>("i_idcatope"),
                            Descripcion = (tiempos == null) ? "(no existe en Java)" : tiempos.Field<string>("descripcion"),
                            Tiempo = (tiempos == null) ? 0 : double.Parse(tiempos.Field<float>("f_tiempope").ToString()),
                            Moneda = _moneda,
                            Cantidad = _cantidad,
                            Tarifa = _tarifa
                        }).OrderBy(x=>x.NumeroOrden).ToList();
            return list as List<ProcesoPrecioDTO>;
        }

        public string DescripcionProcesoJava(int _codProceso) {
            return _operacionModeloDal.SelectDescripcionOperacion(_codProceso);
        }
    }
}
