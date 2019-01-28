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
        /// <summary>
        /// Variable de instancia a la clase AsignacionOrdenesDAL.
        /// </summary>
        public AsignacionOrdenesDAL _asigOrdenesDal = new AsignacionOrdenesDAL();
        /// <summary>
        /// Variable de instancia a la clase OperacionModeloDAL.
        /// </summary>
        public OperacionModeloDAL _operacionModeloDal = new OperacionModeloDAL();

        /// <summary>
        /// Ejecuta procedimientos DAL de Procesos Asignados en SIAA y Tiempo de Procesos, para realizar matching entre
        /// resultados, y con la fusión de contenedores se crea una nueva lista de objetos de tipo ProcesoPrecioDTO.
        /// </summary>
        /// <param name="codProveedor">Código de Proveedor</param>
        /// <param name="modelo">Modelo de prenda</param>
        /// <param name="_cantidad">Cantidad de prendas</param>
        /// <param name="_moneda">Código de Moenda S/D</param>
        /// <param name="_asignacion">Número de Asignación</param>
        /// <param name="_tarifa">Tarifa</param>
        /// <param name="orden">Orden de Producción</param>
        /// <param name="lote">Número de Lote</param>
        /// <returns>Lista genérica de tipo ProcesoPrecioDTO con el resultado del matching</returns>
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

        /// <summary>
        /// Ejecuta un procedimiento DAL de descripción por operación, y retorna el resultado.
        /// </summary>
        /// <param name="_codProceso">Código de Proceso</param>
        /// <returns>Variable de tipo string con el valor de descripción.</returns>
        public string DescripcionProcesoJava(int _codProceso) {
            return _operacionModeloDal.SelectDescripcionOperacion(_codProceso);
        }
    }
}
