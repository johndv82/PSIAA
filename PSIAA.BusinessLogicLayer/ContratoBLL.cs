using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PSIAA.DataAccessLayer;
using PSIAA.DataTransferObject;

namespace PSIAA.BusinessLogicLayer
{
    public class ContratoBLL
    {
        private ContratoDAL _contratoDal = new ContratoDAL();
        private AsignacionOrdenesDAL _asignacionOrdenesDal = new AsignacionOrdenesDAL();
        private RecepcionControlDAL _recepcionControlDal = new RecepcionControlDAL();
        public DataTable ListarContratos(string _tipoCont = "", string _anio = "", string _contrato = "")
        {
            DataTable _dtContratos = _contratoDal.SelectContrato();

            _tipoCont = _tipoCont == "0" ? "" : _tipoCont;
            _anio = _anio == "<---- TODOS ---->" ? "" : _anio.ToString();
            var filtro = from c in _dtContratos.AsEnumerable()
                         let sAnio = c.Field<Int16>("Anio").ToString()
                         let sNumero = c.Field<Int32>("Numero").ToString()
                         where c.Field<string>("Tipo").StartsWith(_tipoCont)
                         && sAnio.Contains(_anio)
                         && sNumero.Contains(_contrato)
                         select c;

            return filtro.Any() ? filtro.CopyToDataTable() : _dtContratos.Clone();
        }

        public Dictionary<string, string> ListarTiposContrato()
        {
            Dictionary<string, string> _dicLista = new Dictionary<string, string>();
            _dicLista.Add("0", "<---- TODOS ---->");
            foreach (DataRow dr in _contratoDal.SelectTipoContrato().Rows)
            {
                _dicLista.Add(dr["Codigo"].ToString(), dr["TipoContrato"].ToString());
            }
            return _dicLista;
        }

        public List<string> ListarAnios()
        {
            List<string> _lista = new List<string>();
            _lista.Add("<---- TODOS ---->");
            foreach (DataRow dr in _contratoDal.SelectAnios().Rows)
            {
                _lista.Add(dr["Anio"].ToString());
            }
            return _lista;
        }

        public DataTable ListarAvancePorContrato(string _contrato)
        {
            return _contratoDal.SelectAvancePorContrato(_contrato);
        }

        public DataTable FiltrarDetalleAvancePorContrato(string _contrato, int _punto, string _modelo, string _color)
        {
            DataTable _dtDetalleAvanceContrato = _recepcionControlDal.SelectRecepcionPorModeloColor(_contrato);
            try
            {
                DataTable dtFiltroDetalle = new DataTable();
                if (_dtDetalleAvanceContrato.Rows.Count > 0)
                {
                    dtFiltroDetalle = _dtDetalleAvanceContrato.AsEnumerable()
                                            .Where(r => r.Field<int>("almacen") == _punto)
                                            .Where(r => r.Field<string>("modelo").Trim() == _modelo.Trim())
                                            .Where(r => r.Field<string>("Color").Trim() == _color.Trim())
                                            .CopyToDataTable();
                }
                return dtFiltroDetalle;
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

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


        public DataTable FiltrarAvanceDetalladoTallasPorPunto(int _contrato,int _punto, out string _cliente, out string _po)
        {
            DataTable _dtAvanceDetalladoTallas = _recepcionControlDal.SelectAvanceDetalladoPorTallas(_contrato);
            string cliente = "", po = "";
            _cliente = cliente;
            _po = po;

            try
            {
                DataTable dtFiltro = new DataTable();
                if (_dtAvanceDetalladoTallas.Rows.Count > 0)
                {
                    po = _dtAvanceDetalladoTallas.Rows[0]["numero_p_o"].ToString();
                    cliente = _dtAvanceDetalladoTallas.Rows[0]["nombre"].ToString();
                    _cliente = cliente;
                    _po = po;
                    dtFiltro = _dtAvanceDetalladoTallas.AsEnumerable()
                                            .Where(r => r.Field<int>("punto") == _punto)
                                            .OrderBy(t => t.Field<string>("talla1"))
                                            .CopyToDataTable();
                }
                return dtFiltro;
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public List<ContratoDetalleDTO> ListarDetalleContrato(int _contrato, bool _agrupacion)
        {
            DataTable dtReturn = _contratoDal.SelectDetalleContrato(_contrato);
            //Linq to DataTable para agrupar el resultado

            if (_agrupacion)
            {
                var dtAgrupado = from cd in dtReturn.AsEnumerable()
                                 group cd by new
                                 {
                                     numero_contrato = cd.Field<int>("numero_contrato"),
                                     Tipo_Contrato = cd.Field<string>("Tipo_Contrato"),
                                     Cod_Modelo_AA = cd.Field<string>("Cod_Modelo_AA"),
                                     Cod_Modelo_Cliente = cd.Field<string>("Cod_Modelo_Cliente"),
                                     c_codmat = cd.Field<string>("c_codmat"),
                                     Cod_Producto = cd.Field<string>("Cod_Producto"),
                                     c_codcol = cd.Field<string>("c_codcol"),
                                     Cod_Color_Cliente = cd.Field<string>("Cod_Color_Cliente"),
                                     cod_grupo_tallas = cd.Field<string>("cod_grupo_tallas"),
                                     talla1 = cd.Field<string>("talla1"),
                                     talla2 = cd.Field<string>("talla2"),
                                     talla3 = cd.Field<string>("talla3"),
                                     talla4 = cd.Field<string>("talla4"),
                                     talla5 = cd.Field<string>("talla5"),
                                     talla6 = cd.Field<string>("talla6"),
                                     talla7 = cd.Field<string>("talla7"),
                                     talla8 = cd.Field<string>("talla8"),
                                     talla9 = cd.Field<string>("talla9"),
                                     Cod_Proveedor = cd.Field<double>("Cod_Proveedor"),
                                     Cod_proveeedor_2 = cd.Field<double>("Cod_proveedor_2"),
                                     Linea = cd.Field<string>("Linea"),
                                     Galga = cd.Field<string>("Galga"),
                                     Titulo = cd.Field<string>("Titulo")
                                 } into grupo
                                 select new
                                 {
                                     Item = grupo.Max(x => x.Field<short>("Item")),
                                     numero_contrato = grupo.Key.numero_contrato,
                                     Tipo_Contrato = grupo.Key.Tipo_Contrato,
                                     Cod_Modelo_AA = grupo.Key.Cod_Modelo_AA,
                                     Cod_Modelo_Cliente = grupo.Key.Cod_Modelo_Cliente,
                                     c_codmat = grupo.Key.c_codmat,
                                     Cod_Producto = grupo.Key.Cod_Producto,
                                     c_codcol = grupo.Key.c_codcol,
                                     Cod_Color_Cliente = grupo.Key.Cod_Color_Cliente,
                                     cod_grupo_tallas = grupo.Key.cod_grupo_tallas,
                                     talla1 = grupo.Key.talla1 == null ? "" : grupo.Key.talla1,
                                     talla2 = grupo.Key.talla2 == null ? "" : grupo.Key.talla2,
                                     talla3 = grupo.Key.talla3 == null ? "" : grupo.Key.talla3,
                                     talla4 = grupo.Key.talla4 == null ? "" : grupo.Key.talla4,
                                     talla5 = grupo.Key.talla5 == null ? "" : grupo.Key.talla5,
                                     talla6 = grupo.Key.talla6 == null ? "" : grupo.Key.talla6,
                                     talla7 = grupo.Key.talla7 == null ? "" : grupo.Key.talla7,
                                     talla8 = grupo.Key.talla8 == null ? "" : grupo.Key.talla8,
                                     talla9 = grupo.Key.talla9 == null ? "" : grupo.Key.talla9,
                                     C1 = grupo.Sum(x => x.Field<int>("C1")),
                                     C2 = grupo.Sum(x => x.Field<int>("C2")),
                                     C3 = grupo.Sum(x => x.Field<int>("C3")),
                                     C4 = grupo.Sum(x => x.Field<int>("C4")),
                                     C5 = grupo.Sum(x => x.Field<int>("C5")),
                                     C6 = grupo.Sum(x => x.Field<int>("C6")),
                                     C7 = grupo.Sum(x => x.Field<int>("C7")),
                                     C8 = grupo.Sum(x => x.Field<int>("C8")),
                                     C9 = grupo.Sum(x => x.Field<int>("C9")),
                                     Precio_Total = grupo.Sum(x => x.Field<double>("Precio_Total")),
                                     Adicional = grupo.Sum(x => x.Field<Single>("Adicional")),
                                     Cod_Proveedor = grupo.Key.Cod_Proveedor,
                                     Cod_Proveedor_2 = grupo.Key.Cod_proveeedor_2,
                                     Linea = grupo.Key.Linea,
                                     Galga = grupo.Key.Galga,
                                     Titulo = grupo.Key.Titulo,
                                     Cant_Contramuestra = int.Parse(grupo.Sum(x => x.Field<int?>("Cant_Contramuestra")).Value.ToString()),
                                     Peso_Estimado = double.Parse(grupo.Sum(x => x.Field<double?>("Peso_Estimado")).Value.ToString()),
                                     obs1 = grupo.Max(x => x.Field<string>("obs1")).ToString()
                                 };

                dtReturn = Helper.ToDataTable(dtAgrupado.ToList());
            }

            List<ContratoDetalleDTO> listContratoDetalle = new List<ContratoDetalleDTO>();

            foreach (DataRow dr in dtReturn.Rows)
            {

                object[] tallas = dr.ItemArray.Skip(10).Take(9).ToArray();
                object[] cantidades = dr.ItemArray.Skip(19).Take(9).ToArray();

                for (int i = 0; i < tallas.Length; i++)
                {
                    if (tallas[i].Equals(DBNull.Value))
                        tallas[i] = "";
                }

                ContratoDetalleDTO contratoDetalle = new ContratoDetalleDTO()
                {
                    Item = int.Parse(dr["Item"].ToString()),
                    Numero = int.Parse(dr["numero_contrato"].ToString()),
                    Tipo = dr["Tipo_Contrato"].ToString(),
                    ModeloAA = dr["Cod_Modelo_AA"].ToString(),
                    ModeloCliente = dr["Cod_Modelo_Cliente"].ToString(),
                    CodMaterial = dr["c_codmat"].ToString(),
                    CodProducto = dr["Cod_Producto"].ToString(),
                    CodColor = dr["c_codcol"].ToString(),
                    ColorCliente = dr["Cod_Color_Cliente"].ToString(),
                    GrupoTallas = dr["cod_grupo_Tallas"].ToString(),
                    Tallas = tallas.Cast<string>().ToArray(),
                    Cantidades = cantidades.Cast<int>().ToArray(),
                    PrecioTotal = float.Parse(dr["Precio_Total"].ToString()),
                    Adicional = double.Parse(dr["Adicional"].ToString()),
                    CodProveedor = float.Parse(dr["Cod_Proveedor"].ToString()),
                    CodProveedor2 = float.Parse(dr["Cod_Proveedor_2"].ToString()),
                    Linea = dr["Linea"].ToString(),
                    Galga = dr["Galga"].ToString(),
                    Titulo = dr["Titulo"].ToString(),
                    CantContramuestra = dr["Cant_Contramuestra"].Equals(DBNull.Value) ? 0 : int.Parse(dr["Cant_Contramuestra"].ToString()),
                    PesoEstimado = dr["Peso_Estimado"].Equals(DBNull.Value) ? 0 : float.Parse(dr["Peso_Estimado"].ToString()),
                    Observaciones = dr["obs1"].Equals(DBNull.Value) ? "" : dr["Peso_Estimado"].ToString()
                };
                listContratoDetalle.Add(contratoDetalle);
            }
            return listContratoDetalle;
        }

        public List<string> ListarModelosContrato(int _contrato) {
            List<string> listModelos = new List<string>();
            listModelos.Add("<------TODOS------->");
            foreach (DataRow row in _contratoDal.SelectGrupoModelos(_contrato).Rows)
            {
                listModelos.Add(row["Cod_Modelo_AA"].ToString());
            }
            return listModelos;
        }

        public string ObtenerClienteContrato(int _contrato) {
            return _contratoDal.SelectCliente(_contrato);
        }

        public string VerificarContratoCerrado(int _contrato) {
            return _contratoDal.SelectVerificaContratoCerrado(_contrato);
        }

        public DataTable ListarDetalleModeloContrato(int contrato, string modelo) {
            return _contratoDal.SelectDetalleModeloContrato(contrato, modelo);
        }

        public string ObtenerTipoContratoPorOrden(string _orden) {
            return _contratoDal.SelectTipoContrato(_orden);
        }
    }
}
