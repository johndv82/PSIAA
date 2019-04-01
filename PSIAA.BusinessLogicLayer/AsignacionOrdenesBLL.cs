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
        /// <summary>
        /// Variable de instancia a la clase AsignacionOrdenesDAL.
        /// </summary>
        public AsignacionOrdenesDAL _asignacionOrdenesDal = new AsignacionOrdenesDAL();
        /// <summary>
        /// Variable de instancia a la clase OperacionModeloDAL.
        /// </summary>
        public OperacionModeloDAL _operacionModeloDal = new OperacionModeloDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL para listar asignaciones aprobadas para el pago a taller.
        /// </summary>
        /// <param name="_codProveedor">Código de Proveedor</param>
        /// <param name="_moneda">Moneda S/D</param>
        /// <param name="_fechaAprobPre">Fecha de Aprobación de Precio</param>
        /// <returns>Contenedor de tipo DataTable con las asignaciones</returns>
        public DataTable ListarAsignaciones(string _codProveedor, string _moneda, string _fechaAprobPre)
        {
            return _asignacionOrdenesDal.SelectAsignacionOrdenes(_codProveedor, _moneda, _fechaAprobPre);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Total de Asignaciones, y retorna el resultado.
        /// A su vez en base al resultado se extrae el nombre de tallas guardandolo
        /// en un array para que sea retornado en paralelo al regreso de la función.
        /// </summary>
        /// <param name="_categoria">Código de Categoria</param>
        /// <param name="_contrato">Número de Contrato</param>
        /// <param name="_modelo">Modelo de Prenda</param>
        /// <param name="_color">Color</param>
        /// <param name="_tallas">Parametro de salida con un arreglo de Tallas</param>
        /// <returns>Contenedor de tipo DataTable con el retorno DAL.</returns>
        public DataTable ListarAsignacionesPorOrden(int _categoria, int _contrato, string _modelo,
                                                    string _color, out string[] _tallas)
        {
            string[] Tallas = new string[9];
            int indTalla = 0;
            DataTable dtAsignacion = _asignacionOrdenesDal.SelectTotalAsignacionOrdenes(_categoria,
                                                            _contrato, _modelo, _color);
            if (dtAsignacion.Rows.Count > 0)
            {
                for (int x = 0; x < dtAsignacion.Columns.Count; x++)
                {
                    if (x >= 14)
                    {
                        Tallas[indTalla] = dtAsignacion.Rows[0][x].ToString();
                        indTalla++;
                    }
                }
            }
            _tallas = Tallas;
            return dtAsignacion;
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Modelos de Asignaciónes por aprobar.
        /// </summary>
        /// <param name="codProv">Código de Proveedor</param>
        /// <returns>Lista genérica de tipo string con los modelos./returns>
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

        /// <summary>
        /// Ejecuta un procedimiento DAL de asignaciones de ordenes por aprobar, y retorna el resultado.
        /// </summary>
        /// <param name="codProv">Código de Proveedor</param>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con las asignaciones.</returns>
        public DataTable ListarAsignacionesParaAprobar(string codProv, string modelo)
        {
            return _asignacionOrdenesDal.SelectAsignacionOrdenesParaAprobar(codProv, modelo);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de detalle de asignaciones agrupadas, y el resultado lo retorna.
        /// </summary>
        /// <param name="_codProv">Código de Proveedor</param>
        /// <param name="nroAsig">Número de Asignación</param>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <param name="_categoria">Código de Categoria</param>
        /// <returns>Contenedor de tipo DataTable con el detalle de asignaciones.</returns>
        public DataTable ListarDetalleProcesoAsignacionOrdenes(string _codProv, string nroAsig, string _orden, int _lote, int _categoria)
        {
            return _asignacionOrdenesDal.SelectDetalleGrupoAsignacionOrdenes(_codProv, nroAsig, _orden, _lote, _categoria);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de actualización de tarifa/tiempo de asignación por cada proceso aprobado.
        /// </summary>
        /// <param name="codProv">Código de Proveedor</param>
        /// <param name="catOperacion">Categoria de Operación</param>
        /// <param name="numAsignacion">Número de Asignación</param>
        /// <param name="moneda">Moneda S/D</param>
        /// <param name="listProcesoPrecio">Listado de procesos con precio aprobado</param>
        /// <param name="orden">Orden de Producción</param>
        /// <param name="lote">Número de Lote</param>
        /// <returns>Variable de tipo int con la cantidad de actualizaciones.</returns>
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

        /// <summary>
        /// Convierte el valor del parametro: "aprobado" a entero (1 si es verdadero, o 0 si es falso). Para enviarlo y ejecutar
        /// el procedimiento DAL de actualizacion y recalculo de precios y costos en asignaciones.
        /// </summary>
        /// <param name="codProv">Código de Proveedor</param>
        /// <param name="catOperacion">Categoria de Operación</param>
        /// <param name="numAsignacion">Número de Asignación</param>
        /// <param name="moneda">Moneda (S/D)</param>
        /// <param name="fechaAprob">Fecha de Aprobación</param>
        /// <param name="usuarioAprob">Usuario de Aprobación</param>
        /// <param name="aprobado">Aprobación (SI/NO)</param>
        /// <param name="orden">Orden de Producción</param>
        /// <param name="lote">Número de Lote</param>
        /// <returns>Variable de tipo int con la cantidad de actualizaciones.</returns>
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

        private string NroDeOrdenAsignacion()
        {
            string ultimoNumero = _asignacionOrdenesDal.SelectUltimoNumeroOrden();
            long nuevoNumero = long.Parse(ultimoNumero) + 1;
            return nuevoNumero.ToString();
        }

        /// <summary>
        /// Genera un nuevo número de asignación y según las operaciones seleccionadas categoriza y sub-categoriza cada uno de sus procesos
        /// por modelo. Construye un nuevo objeto tipo AsignacionOrdenDetDTO por cada proceso y se dispone a ejecutar el procedimiento DAL
        /// de Insert Asignación Detalle, y otro objeto de tipo AsignacionOrdenDetDTO para el procedimiento de Insert Asignación Cabecera.
        /// </summary>
        /// <param name="listAasignar">Lista genérica de tipo AasignarDTO con los valores a Asignar</param>
        /// <param name="listLanzamientoDet">Lista genérica de tipo LanzamientoDetDTO con los lanzamientos</param>
        /// <param name="user">Nombre de Usuario</param>
        /// <returns>Arreglo de tipo int con dos valores: cantidad de cabeceras ingresadas, y cantidad de detalles ingresados.</returns>
        public int[] IngresarAsignacionOrden(List<AasignarDTO> listAasignar, List<LanzamientoDetDTO> listLanzamientoDet, string user)
        {
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
