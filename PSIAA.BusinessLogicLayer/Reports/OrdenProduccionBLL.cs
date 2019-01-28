using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataAccessLayer.TuartDB;
using System.Data;

namespace PSIAA.BusinessLogicLayer.Reports
{
    public class OrdenProduccionBLL
    {
        /// <summary>
        /// Variable de instancia a la clase LanzamientoDAL.
        /// </summary>
        public LanzamientoDAL _lanzamientoDal = new LanzamientoDAL();
        /// <summary>
        /// Variable de instancia a la clase ComponenteModeloBLL.
        /// </summary>
        public ComponenteModeloBLL _modeloComponenteBll = new ComponenteModeloBLL();
        /// <summary>
        /// Variable de instancia a la clase MedidaPorTallaDAL.
        /// </summary>
        public MedidaPorTallaDAL _medidaPorTallaDal = new MedidaPorTallaDAL();
        /// <summary>
        /// Variable de instancia a la clase OperacionModeloDAL.
        /// </summary>
        public OperacionModeloDAL _operacionModeloDal = new OperacionModeloDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de Ordenes Asignadas segun el código de categoria de operaciones:
        /// 400 : Tejido
        /// 500 : Confección
        /// En el caso de que el código de categoria sea 0, se ejecuta el procedimiento de Ordenes Lanzadas.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="categoriaOperacion">Codigo de Categoria de Operaciones</param>
        /// <returns>Contenedor de datos de tipo DataTable con las órdenes.</returns>
        public DataTable ListarOrdenesLanzadasAsignadas(int contrato, int categoriaOperacion)
        {
            if (categoriaOperacion == 0)
            {
                return _lanzamientoDal.SelectOrdenesLanzadas(contrato);
            }
            else {
                return _lanzamientoDal.SelectOrdenesLanzadasAsignadas(contrato, categoriaOperacion);
            }
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Detalle de Ordenes de Producción Lanzadas.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="orden">Orden de Producción</param>
        /// <returns>Contenedor de datos de tipo DataTable con el detalle de Ordenes.</returns>
        public DataTable ListarDetalleOrdenesProduccion(int contrato, string orden)
        {
            return _lanzamientoDal.SelectDetalleOrdenesProduccion(contrato, orden);
        }

        /// <summary>
        /// Ejecuta procedimientos DAL de Combinaciones de Lanzamiento y Componentes por Modelo, y con la ayuda de la función BLL
        /// para armar cabeceras de componentes crea un conjunto (matching) relacionando combinaciones y componentes pre-armado.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="orden">Orden de Producción</param>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de datos de tipo DataTable con el conjunto o matching</returns>
        public DataTable ListarDetalleOrdenProduccion(int contrato, string orden, string modelo) {
            DataTable dtCombinaciones = _lanzamientoDal.SelectCombinacionesPartidaLanzamiento(contrato, orden.Trim(), modelo.Trim());
            DataTable dtComponentesModelo = _modeloComponenteBll.ListarComponentesPorModelo(modelo.Trim());
            DataTable dtComponentes = ArmarComponentesCabecera(dtComponentesModelo);

            var match = (from combi in dtCombinaciones.AsEnumerable()
                         join compo in dtComponentes.AsEnumerable()
                            on combi.Field<string>("modelo").Trim()
                             equals compo.Field<string>("c_codmod").Trim()
                         into outer
                         from compo in outer.DefaultIfEmpty()
                         select new
                         {
                             Modelo = combi.Field<string>("modelo"),
                             ColorPrimario = combi.Field<string>("ColorPrim"),
                             ColorSecundario = combi.Field<string>("ColorSec"),
                             Partida = combi.Field<string>("Partida"),
                             Lote = combi.Field<short>("Lote"),
                             Talla = combi.Field<string>("Talla"),
                             Piezas = combi.Field<int>("Piezas"),
                             TitCompo1 = compo?.Field<string>("componente1"),
                             TitCompo2 = compo?.Field<string>("componente2"),
                             TitCompo3 = compo?.Field<string>("componente3"),
                             TitCompo4 = compo?.Field<string>("componente4"),
                             TitCompo5 = compo?.Field<string>("componente5"),
                             TitCompo6 = compo?.Field<string>("componente6"),
                             TitCompo7 = compo?.Field<string>("componente7"),
                             TitCompo8 = compo?.Field<string>("componente8"),
                             TitCompo9 = compo?.Field<string>("componente9"),
                             TitCompo10 = compo?.Field<string>("componente10")
                         }).ToList();

            return Helper.ToDataTable(match);
        }

        /// <summary>
        /// Genera un nuevo Contenedor de datos en base a los Componentes por Modelo, agrupando el modelo y colocando
        /// cada uno de los componentes en columnas separadas dentro del esquema de máximo 10 componentes.
        /// </summary>
        /// <param name="dtModeloComp">Contenedor de tipo DataTable con los Componentes por Modelo</param>
        /// <returns>Contenedor de tipo DataTable con el conjunto pre-armado</returns>
        private DataTable ArmarComponentesCabecera(DataTable dtModeloComp) {
            DataTable dtComponentes = new DataTable();
            dtComponentes.Columns.Add("c_codmod", typeof(string));
            dtComponentes.Columns.Add("componente1", typeof(string));
            dtComponentes.Columns.Add("componente2", typeof(string));
            dtComponentes.Columns.Add("componente3", typeof(string));
            dtComponentes.Columns.Add("componente4", typeof(string));
            dtComponentes.Columns.Add("componente5", typeof(string));
            dtComponentes.Columns.Add("componente6", typeof(string));
            dtComponentes.Columns.Add("componente7", typeof(string));
            dtComponentes.Columns.Add("componente8", typeof(string));
            dtComponentes.Columns.Add("componente9", typeof(string));
            dtComponentes.Columns.Add("componente10", typeof(string));

            if (dtModeloComp.Rows.Count > 0)
            {
                string[] componentes = new string[10] { "", "", "", "", "", "", "", "", "", "" };
                int indice = 0;
                string modelo = string.Empty;
                foreach (DataRow fila in dtModeloComp.Rows)
                {
                    if (indice == 0)
                    {
                        modelo = fila["c_codmod"].ToString();
                    }
                    componentes[indice] = fila["c_dencom"].ToString();
                    indice = indice + 1;
                }
                dtComponentes.Rows.Add(modelo, componentes[0], componentes[1], componentes[2], componentes[3], componentes[4],
                                    componentes[5], componentes[6], componentes[7], componentes[8], componentes[9]);
            }
            return dtComponentes;
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de pesos y cantidad lanzada, por órden de producción.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="nroOrden">Orden de Producción</param>
        /// <returns>Contenedor de datos de tipo DataTable con los pesos y cantidades lanzadas.</returns>
        public DataTable ListarPesosPorTalla(int contrato, string nroOrden) {
            return _lanzamientoDal.SelectPesoPorTallas(contrato, nroOrden);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Combinacion/Partida por Orden de Produccion y modelo, y el resultado lo filtra
        /// por número de lote, seleccionando solo los campos de ColorSecundario y Partida.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="modelo">Modelo de Prenda</param>
        /// <param name="orden">Orden de Producción</param>
        /// <param name="lote">Número de Lote</param>
        /// <returns>Contenedor de tipo DataTable con los datos de ColorSecundario y Partida.</returns>
        public DataTable ListarColoresPorOrdenLote(int contrato, string modelo, string orden, int lote) {
            DataTable dtCombinaciones = _lanzamientoDal.SelectCombinacionesPartidaLanzamiento(contrato, orden.Trim(), modelo.Trim());

            var colores = (from combi in dtCombinaciones.AsEnumerable()
                           where combi.Field<short>("Lote") == lote
                           select new
                           {
                               Color = combi.Field<string>("ColorSec"),
                               Partida = combi.Field<string>("Partida")
                           }).ToList();
            return Helper.ToDataTable(colores);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de medidas por talla de prenda.
        /// </summary>
        /// <param name="modelo">Modelo de Prenda</param>
        /// <param name="talla">Nombre de Talla</param>
        /// <returns>Contenedor de datos de tipo DataTable con las medidas.</returns>
        public DataTable ListarMedidasPorTalla(string modelo, string talla) {
            return _medidaPorTallaDal.SelectMedidasPorTalla(modelo.Trim(), talla.Trim());
        }

        /// <summary>
        /// Ejecuta un procedimiento BLL de Componentes por Modelo y con el resultado pobla una lista de tipo string
        /// con 7 items predefinidos, cada uno de los primeros 7 componentes.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Lista Genérica de tipo string con los componentes.</returns>
        public List<string> ComponentesPorModelo(string modelo) {
            List<string> componentes = new List<string>() { "","","","","","",""};
            int indice = 0;
            DataTable dtComponentes = _modeloComponenteBll.ListarComponentesPorModelo(modelo.Trim());
            foreach (DataRow fila in dtComponentes.Rows) {
                if (indice < 7) {
                    componentes[indice] = fila["c_dencom"].ToString();
                }
                indice = indice + 1;
            }
            return componentes;
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Código de Operaciones por prenda, y con el resultado se pobla un contenedor de
        /// datos adjuntado un indice número para cada operación.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con las operaciones y su índice correspondiente.</returns>
        public DataTable ListarOperacion(string modelo) {
            DataTable dtOperaciones = _operacionModeloDal.SelectCodigoOperaciones(modelo.Trim());
            DataTable dtOperAgrup = new DataTable();
            int indice = 1;

            dtOperAgrup.Columns.Add("id", typeof(int));
            dtOperAgrup.Columns.Add("nro_fila", typeof(string));
            dtOperAgrup.Columns.Add("operacion", typeof(string));
            foreach (DataRow fila in dtOperaciones.Rows) {
                dtOperAgrup.Rows.Add(indice, Helper.Mascara(indice, "00"), fila["c_codope"]);
                indice = indice + 1;
            }
            return dtOperAgrup;
        }
    }
}
