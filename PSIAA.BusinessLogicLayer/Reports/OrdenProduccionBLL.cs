using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataAccessLayer.TuartDB;
using System.Data;
using System.Reflection;

namespace PSIAA.BusinessLogicLayer.Reports
{
    public class OrdenProduccionBLL
    {
        private LanzamientoDAL _lanzamientoDal = new LanzamientoDAL();
        private ComponenteModeloBLL _modeloComponenteBll = new ComponenteModeloBLL();
        private DataAccessLayer.TuartDB.MedidaPorTallaDAL _medidaPorTallaDal = new DataAccessLayer.TuartDB.MedidaPorTallaDAL();
        private OperacionModeloDAL _operacionModeloDal = new OperacionModeloDAL();
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

        public DataTable ListarDetalleOrdenesProduccion(int contrato, string orden)
        {
            return _lanzamientoDal.SelectDetalleOrdenesProduccion(contrato, orden);
        }

        public DataTable ListarDetalleOrdenProduccion(int contrato, string orden, string modelo) {
            DataTable dtCombinaciones = _lanzamientoDal.SelectCombinacionesPartidaLanzamiento(contrato, orden.Trim(), modelo.Trim());
            DataTable dtComponentesModelo = _modeloComponenteBll.ListarComponentesPorModelo(modelo.Trim());
            DataTable dtComponentes = ArmarComponentesCabecera(dtComponentesModelo);

            var match = (from compo in dtComponentes.AsEnumerable()
                         join combi in dtCombinaciones.AsEnumerable()
                             on compo.Field<string>("c_codmod").Trim()
                             equals combi.Field<string>("modelo").Trim()
                         into outer
                         from combi in outer.DefaultIfEmpty()
                         select new
                         {
                             Modelo = combi.Field<string>("modelo"),
                             ColorPrimario = combi.Field<string>("ColorPrim"),
                             ColorSecundario = combi.Field<string>("ColorSec"),
                             Partida = combi.Field<string>("Partida"),
                             Lote = combi.Field<short>("Lote"),
                             Talla = combi.Field<string>("Talla"),
                             Piezas = combi.Field<int>("Piezas"),
                             TitCompo1 = compo.Field<string>("componente1"),
                             TitCompo2 = compo.Field<string>("componente2"),
                             TitCompo3 = compo.Field<string>("componente3"),
                             TitCompo4 = compo.Field<string>("componente4"),
                             TitCompo5 = compo.Field<string>("componente5"),
                             TitCompo6 = compo.Field<string>("componente6"),
                             TitCompo7 = compo.Field<string>("componente7"),
                             TitCompo8 = compo.Field<string>("componente8"),
                             TitCompo9 = compo.Field<string>("componente9"),
                             TitCompo10 = compo.Field<string>("componente10")
                         }).ToList();

            return ToDataTable(match);
        }

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

            string[] componentes = new string[10] { "", "", "", "", "", "", "", "", "", ""};
            int indice = 0;
            string modelo = string.Empty;
            foreach (DataRow fila in dtModeloComp.Rows) {
                if (indice == 0) {
                    modelo = fila["c_codmod"].ToString();
                }
                componentes[indice] = fila["c_dencom"].ToString();
                indice = indice + 1;
            }
            dtComponentes.Rows.Add(modelo, componentes[0], componentes[1], componentes[2], componentes[3], componentes[4],
                                componentes[5], componentes[6], componentes[7], componentes[8], componentes[9]);
            return dtComponentes;
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public DataTable ListarPesosPorTalla(int contrato, string nroOrden) {
            return _lanzamientoDal.SelectPesoPorTallas(contrato, nroOrden);
        }

        public DataTable ListarColoresPorOrdenLote(int contrato, string modelo, string orden, int lote) {
            DataTable dtCombinaciones = _lanzamientoDal.SelectCombinacionesPartidaLanzamiento(contrato, orden.Trim(), modelo.Trim());

            var colores = (from combi in dtCombinaciones.AsEnumerable()
                           where combi.Field<short>("Lote") == lote
                           select new
                           {
                               Color = combi.Field<string>("ColorSec"),
                               Partida = combi.Field<string>("Partida")
                           }).ToList();
            return ToDataTable(colores);
        }

        public DataTable ListarMedidasPorTalla(string modelo, string talla) {
            return _medidaPorTallaDal.SelectMedidasPorTalla(modelo.Trim(), talla.Trim());
        }

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

        public DataTable ListarOperacion(string modelo) {
            DataTable dtOperaciones = _operacionModeloDal.SelectCodigoOperaciones(modelo.Trim());
            DataTable dtOperAgrup = new DataTable();
            int indice = 1;

            dtOperAgrup.Columns.Add("id", typeof(int));
            dtOperAgrup.Columns.Add("nro_fila", typeof(string));
            dtOperAgrup.Columns.Add("operacion", typeof(string));
            foreach (DataRow fila in dtOperaciones.Rows) {
                dtOperAgrup.Rows.Add(indice, MascaraNumeroFila(indice), fila["c_codope"]);
                indice = indice + 1;
            }
            return dtOperAgrup;
        }

        private string MascaraNumeroFila(int nroFila)
        {
            string cadena = "00";
            int largoId = nroFila.ToString().Length;
            return cadena.Substring(0, cadena.Length - largoId) + nroFila.ToString();
        }
    }
}
