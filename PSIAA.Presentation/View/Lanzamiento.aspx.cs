using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSIAA.BusinessLogicLayer;
using PSIAA.DataTransferObject;
using System.Data;

namespace PSIAA.Presentation.View
{
    public partial class Lanzamiento : System.Web.UI.Page
    {
        private ContratoBLL _contratoBll = new ContratoBLL();
        private LanzamientoBLL _lanzamientoBll = new LanzamientoBLL();
        private AsignacionOrdenesBLL _asignacionOrdenBll = new AsignacionOrdenesBLL();
        private HojaCombinacionesBLL _hojaCombinacionesBll = new HojaCombinacionesBLL();
        private HttpCookie cookie;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cookie = Request.Cookies["Usuario"];
                Session["Asignado"] = string.Empty;
                Session["Asignado"] = "Si";
                if (cookie != null)
                {
                    hidUsuario.Value = cookie["Nombre"].ToString();
                    Session["ListadoAlanzar"] = new List<AlanzarDTO>();
                    Session["ListadoAasignar"] = new List<AasignarDTO>();
                    Session["listMaterialPorColor"] = new List<MaterialPorColorDTO>();
                    Session["dicPesosBaseContrato"] = new Dictionary<string, Dictionary<string, decimal>>();
                    Session["Tallas"] = new string[9];
                    txtContrato.Focus();

                    //Cargar Categorias de Operacion
                    ddlCatOperacion.DataSource = _lanzamientoBll.ListarCategoriasOperaciones();
                    ddlCatOperacion.DataBind();
                }
                else
                {
                    //LOGOUT
                    string user = Request.QueryString["logout"];
                    Session.Remove(user);
                    Session.Abandon();
                    //Destruir Sesiones
                    for (int i = 0; i < Session.Count; i++)
                    {
                        var nombre = Session.Keys[i].ToString();
                        Session.Remove(nombre);
                    }
                    Response.Redirect("default.aspx");
                }
            }
            lblMensajeErrorContrato.Visible = false;
            lblmsnError.Visible = false;
            lblmsnRegistrosDuplicados.Visible = false;

            //Evaluar si hay datos en la sesion de FilasAfectadas
            if (Session["NroOrdenesGeneradas"] != null)
            {
                int nroOrdenes = int.Parse(Session["NroOrdenesGeneradas"].ToString());
                if (nroOrdenes > 0)
                {
                    lblRespuesta.Text = "!Lanzamiento ingresado con Exito!";
                    lblRespuesta.ForeColor = System.Drawing.Color.Green;
                    lblRespuesta.Visible = true;
                }
                else
                {
                    lblRespuesta.Text = "Hubo un error al ingresar el Lanzamiento";
                    lblRespuesta.ForeColor = System.Drawing.Color.Red;
                    lblRespuesta.Visible = true;
                }
                Session["NroOrdenesGeneradas"] = null;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Devolver listado de modelos por contrato
            List<string> _modelos = _contratoBll.ListarModelosContrato(int.Parse(txtContrato.Text));
            if (_modelos.Count > 1)
            {
                _modelos.RemoveAt(0);
                hidContrato.Value = txtContrato.Text;
                lstModelos.DataSource = _modelos;
                lstModelos.DataBind();
                lstModelos.Visible = true;
                btnProcesarModelos.Visible = true;
            }
            else {
                lblMensajeErrorContrato.Visible = true;
                lstModelos.Items.Clear();
                lstModelos.Visible = false;
                btnProcesarModelos.Visible = false;
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<ContratoDetalleDTO> _listContratoDetPrin = (List<ContratoDetalleDTO>)Session["ListContratoDetPrin"];
            string modelo = e.CommandArgument.ToString();
            List<ContratoDetalleDTO> _listaContratoDetModelo = _listContratoDetPrin
                                                            .Where(x => x.ModeloAA.Equals(modelo)).ToList();

            Session["ListaContratoDetModelo"] = _listaContratoDetModelo;
            lblModelo.Text = modelo.Trim();
            rblColores.Items.Clear();

            foreach (ContratoDetalleDTO cd in _listaContratoDetModelo.OrderBy(x => x.CodColor))
            {
                rblColores.Items.Add(new ListItem() { Text = "Color: " + cd.CodColor, Value = cd.CodColor });
            }
            Session["listMaterialPorColor"] = new List<MaterialPorColorDTO>();

            Session["Linea"] = _listaContratoDetModelo[0].Linea;
            lblFechaSol.Text = DateTime.Now.ToShortDateString();
            lblCodMaterial.Text = _listaContratoDetModelo[0].CodMaterial;
            lblTitulo.Text = _listaContratoDetModelo[0].Titulo;
            btnAgregar.Visible = true;
            btnActualizarKilos.Visible = true;

            ddlCatOperacion.SelectedIndex = 0;
            ddlProcesos_SelectedIndexChanged(source, e);
            txtProveedor.Text = string.Empty;
            lblCodTaller.Text = "0";

            //COLOCAR TALLAS EN CABECERAS DE GRID
            for (int x = 0; x < gridCantSolicitadas.Columns.Count; x++)
            {
                gridCantSolicitadas.Columns[x].HeaderText = _listaContratoDetModelo[0].Tallas[x];
                Session["Tallas"] = _listaContratoDetModelo[0].Tallas;
            }
            gridCantSolicitadas.DataBind();

            Dictionary<string, Dictionary<string, decimal>> modelPeso = Session["dicPesosBaseContrato"]
                    as Dictionary<string, Dictionary<string, decimal>>;
            Session["dicPesosBase"] = modelPeso[lblModelo.Text];

            rblColores.SelectedIndex = 0;
            rblColores_SelectedIndexChanged(source, e);
        }

        protected void rblColores_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ContratoDetalleDTO> _listaContratoDetModelo = ((List<ContratoDetalleDTO>)Session["ListaContratoDetModelo"])
                                                                .Where(x => x.CodColor.Equals(rblColores.SelectedValue)).ToList();

            ContratoDetalleDTO _lanzado = new ContratoDetalleDTO
            {
                Cantidades = _lanzamientoBll.ListarCantidadesLanzadas(int.Parse(hidContrato.Value), lblModelo.Text, rblColores.SelectedValue)
            };
            _listaContratoDetModelo.Add(_lanzado);

            gridCantSolicitadas.DataSource = _listaContratoDetModelo;
            gridCantSolicitadas.DataBind();

            //RECORRER GRID PARA EVALUAR CANTIDADES FALTANTES POR LANZAR
            int[] _cantidadesLanzadas = new int[9];
            int[] _cantidadesSolicitadas = new int[9];
            int indice = 1;
            foreach (GridViewRow row in gridCantSolicitadas.Rows)
            {
                if (indice == 1)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        _cantidadesSolicitadas[c] += int.Parse(((Label)row.FindControl("lblCant" + (c + 1))).Text);
                    }
                }
                else if (indice == 2)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        _cantidadesLanzadas[c] += int.Parse(((Label)row.FindControl("lblCant" + (c + 1))).Text);
                    }
                    break;
                }
                indice++;
            }

            txtCant1.Text = (int.Parse(_cantidadesSolicitadas[0].ToString()) - int.Parse(_cantidadesLanzadas[0].ToString())).ToString();
            txtCant2.Text = (int.Parse(_cantidadesSolicitadas[1].ToString()) - int.Parse(_cantidadesLanzadas[1].ToString())).ToString();
            txtCant3.Text = (int.Parse(_cantidadesSolicitadas[2].ToString()) - int.Parse(_cantidadesLanzadas[2].ToString())).ToString();
            txtCant4.Text = (int.Parse(_cantidadesSolicitadas[3].ToString()) - int.Parse(_cantidadesLanzadas[3].ToString())).ToString();
            txtCant5.Text = (int.Parse(_cantidadesSolicitadas[4].ToString()) - int.Parse(_cantidadesLanzadas[4].ToString())).ToString();
            txtCant6.Text = (int.Parse(_cantidadesSolicitadas[5].ToString()) - int.Parse(_cantidadesLanzadas[5].ToString())).ToString();
            txtCant7.Text = (int.Parse(_cantidadesSolicitadas[6].ToString()) - int.Parse(_cantidadesLanzadas[6].ToString())).ToString();
            txtCant8.Text = (int.Parse(_cantidadesSolicitadas[7].ToString()) - int.Parse(_cantidadesLanzadas[7].ToString())).ToString();
            txtCant9.Text = (int.Parse(_cantidadesSolicitadas[8].ToString()) - int.Parse(_cantidadesLanzadas[8].ToString())).ToString();

            //Guardar Cantidades Solicitadas en Variable Global, para luego validar
            Session["arrayCantidadesSolicitadas"] = _cantidadesSolicitadas;

            //CALCULAR KILOS NECESARIOS
            int[] cantidadesAlanzar = new int[] { int.Parse(txtCant1.Text), int.Parse(txtCant2.Text), int.Parse(txtCant3.Text),
                                                int.Parse(txtCant4.Text), int.Parse(txtCant5.Text), int.Parse(txtCant6.Text),
                                                int.Parse(txtCant7.Text), int.Parse(txtCant8.Text), int.Parse(txtCant9.Text) };

            Dictionary<string, decimal> pesosBase = Session["dicPesosBase"] as Dictionary<string, decimal>;
            decimal pesoFinal = 0;
            int ind = 0;
            foreach (string talla in _listaContratoDetModelo[0].Tallas)
            {
                if (talla != "")
                {
                    pesoFinal = pesoFinal + (pesosBase[talla] * cantidadesAlanzar[ind]);
                }
                ind++;
            }
            lblKgNecesarios.Text = pesoFinal.ToString();

            //EVALUAR SI EL COLOR TIENE COMBINACION
            string color = rblColores.SelectedValue.ToString();
            if (color.Substring(0, 2) == "C0")
            {
                //Limpiar grid de Combinaciones
                gridMaterialColor.DataSource = new DataTable();
                gridMaterialColor.DataBind();
                string correlativoCol = color.Substring(1, color.Length - 1);
                gridMaterialColor.DataSource = _hojaCombinacionesBll.ListarColoresCombinacion(lblModelo.Text, correlativoCol, decimal.Parse(lblKgNecesarios.Text));
                gridMaterialColor.DataBind();

                btnMaterialColor.Visible = true;
            }
            else
            {
                btnMaterialColor.Visible = false;
            }
            txtMaterial.Text = lblCodMaterial.Text.Trim() + "-HC-" + lblTitulo.Text.Trim() + "-" +
                                rblColores.SelectedValue.ToString().Trim();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            List<AlanzarDTO> _listAlanzar = Session["ListadoAlanzar"] as List<AlanzarDTO>;
            List<AasignarDTO> _listAasignar = Session["ListadoAasignar"] as List<AasignarDTO>;
            int[] cantidadesSolic = Session["arrayCantidadesSolicitadas"] as int[];

            int[] _cantidades = new int[9] { int.Parse(txtCant1.Text), int.Parse(txtCant2.Text), int.Parse(txtCant3.Text),
                                             int.Parse(txtCant4.Text), int.Parse(txtCant5.Text), int.Parse(txtCant6.Text),
                                             int.Parse(txtCant7.Text), int.Parse(txtCant8.Text), int.Parse(txtCant9.Text)};

            lblmsnError.Visible = false;
            lblmsnRegistrosDuplicados.Visible = false;

            if (_cantidades.Sum() > 0)
            {
                try
                {
                    Dictionary<string, char> _modelCorr = Session["modelosCorrelativos"] as Dictionary<string, char>;
                    AlanzarDTO alanzar = new AlanzarDTO
                    {
                        Contrato = int.Parse(hidContrato.Value),
                        Cantidades = _cantidades,
                        Tallas = Session["Tallas"] as string[],
                        Color = rblColores.SelectedValue.Trim(),
                        Modelo = lblModelo.Text,
                        CorrelativoModelo = _modelCorr[lblModelo.Text.Trim()],
                        KilosNecesarios = decimal.Parse(lblKgNecesarios.Text),
                        Material = txtMaterial.Text,
                        Linea = Session["Linea"].ToString(),
                        Asignacion = Session["Asignado"].ToString()
                    };

                    AasignarDTO aasignar = new AasignarDTO()
                    {
                        Modelo = lblModelo.Text,
                        Color = rblColores.SelectedValue.Trim(),
                        CodCatOperacion = int.Parse(lblCodCatOpe.Text),
                        DescripcionCatOper = ddlCatOperacion.SelectedItem.ToString(),
                        FechaRetorno = txtFechaRetorno.Text == "" ? new DateTime(1901, 1, 1) : Convert.ToDateTime(txtFechaRetorno.Text),
                        CodProveedor = lblCodTaller.Text,
                        Taller = txtProveedor.Text,
                        TodasOperaciones = chkTodasOperaciones.Checked,
                        Asignacion = Session["Asignado"].ToString()
                    };

                    if (_listAlanzar.Find(x => (x.Modelo == alanzar.Modelo) && (x.Color == alanzar.Color)) == null)
                    {
                        _listAlanzar.Add(alanzar);
                        _listAasignar.Add(aasignar);
                        Session["ListadoAlanzar"] = _listAlanzar;
                        Session["ListadoAasignar"] = _listAasignar;
                        lblmsnRegistrosDuplicados.Visible = false;
                    }
                    else
                    {
                        lblmsnRegistrosDuplicados.Visible = true;
                    }

                    /*** Poblar Grid de Lanzados ***/
                    gridAlanzar.DataSource = _listAlanzar;
                    gridAlanzar.DataBind();
                }
                catch (Exception exc)
                {
                    lblmsnError.Visible = true;
                    Console.WriteLine(exc.Message);
                }
            }
        }

        protected void btnPreLanzamiento_Click(object sender, EventArgs e)
        {
            List<AlanzarDTO> _alanzar = Session["ListadoAlanzar"] as List<AlanzarDTO>;
            if (_alanzar.Count > 0)
            {
                List<LanzamientoDetDTO> _listLanzDet = _lanzamientoBll.ListarPreLanzamiento(_alanzar, hidUsuario.Value);
                Session["LanzamientoDet"] = _listLanzDet;
                gridPreLanzamiento.DataSource = Session["LanzamientoDet"] as List<LanzamientoDetDTO>;
                gridPreLanzamiento.DataBind();

                List<AasignarDTO> _listAasignar = Session["ListadoAasignar"] as List<AasignarDTO>;
                if (_listAasignar.Count > 0) {
                    gridAsignaciones.DataSource = _listAasignar;
                    gridAsignaciones.DataBind();
                }
                lblOrdenLoteRepetido.Visible = false;
            }
        }

        protected void btnActualizarKilos_Click(object sender, EventArgs e)
        {
            List<ContratoDetalleDTO> _listContratoDetPrin = (List<ContratoDetalleDTO>)Session["ListaContratoDetModelo"];

            //CALCULAR KILOS NECESARIOS
            int[] cantidadesAlanzar = new int[] { txtCant1.Text == "" ? 0 : int.Parse(txtCant1.Text),
                                                txtCant2.Text == "" ? 0 : int.Parse(txtCant2.Text),
                                                txtCant3.Text == "" ? 0 : int.Parse(txtCant3.Text),
                                                txtCant4.Text == "" ? 0 : int.Parse(txtCant4.Text),
                                                txtCant5.Text == "" ? 0 : int.Parse(txtCant5.Text),
                                                txtCant6.Text == "" ? 0 : int.Parse(txtCant6.Text),
                                                txtCant7.Text == "" ? 0 : int.Parse(txtCant7.Text),
                                                txtCant8.Text == "" ? 0 : int.Parse(txtCant8.Text),
                                                txtCant9.Text == "" ? 0 : int.Parse(txtCant9.Text) };

            Dictionary<string, decimal> pesosBase = Session["dicPesosBase"] as Dictionary<string, decimal>;
            decimal pesoFinal = 0;
            int ind = 0;
            foreach (string talla in _listContratoDetPrin[0].Tallas)
            {
                if (talla != "")
                {
                    pesoFinal = pesoFinal + (pesosBase[talla] * cantidadesAlanzar[ind]);
                }
                ind++;
            }
            lblKgNecesarios.Text = pesoFinal.ToString();
            //Actualizar grilla con los materiales y su nueva distribucion de pesos
            string color = rblColores.SelectedValue.ToString();
            if (color.Substring(0, 2) == "C0")
            {
                string correlativoCol = color.Substring(1, color.Length - 1);
                gridMaterialColor.DataSource = _hojaCombinacionesBll.ListarColoresCombinacion(lblModelo.Text, correlativoCol, decimal.Parse(lblKgNecesarios.Text));
                gridMaterialColor.DataBind();
            }
        }

        protected void ddlProcesos_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCodCatOpe.Text = ddlCatOperacion.SelectedValue;
        }

        protected void btnBuscarAsig_Click(object sender, EventArgs e)
        {
            DataView dvProveedores = new DataView(_lanzamientoBll.ListarProveedores());
            Session["ListadoProveedores"] = dvProveedores;
            gridProveedores.DataSource = dvProveedores.ToTable();
            gridProveedores.DataBind();
            txtNombreComercial.Text = string.Empty;
            txtNombreComercial.Focus();
        }

        protected void gridAlanzar_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridAlanzar.SelectedRow;
            string modelo = row.Cells[0].Text;
            string color = row.Cells[1].Text;

            List<AlanzarDTO> _alanzar = Session["ListadoAlanzar"] as List<AlanzarDTO>;
            List<AasignarDTO> _aAsignar = Session["ListadoAasignar"] as List<AasignarDTO>;
            //Eliminar item aLanzar
            AlanzarDTO _alanzarElemento = _alanzar.Find(x => (x.Modelo == modelo) && (x.Color.Trim() == color));
            _alanzar.Remove(_alanzarElemento);
            //Eliminar item aAsignar
            AasignarDTO _aAsignarElemento = _aAsignar.Find(x => (x.Modelo == modelo) && (x.Color.Trim() == color));
            _aAsignar.Remove(_aAsignarElemento);

            Session["ListadoAlanzar"] = _alanzar;
            Session["ListadoAasignar"] = _aAsignar;

            gridAlanzar.DataSource = _alanzar;
            gridAlanzar.DataBind();
        }

        protected void btnGuardarLanz_Click(object sender, EventArgs e)
        {
            List<LanzamientoDetDTO> _listLanzamientoDet = new List<LanzamientoDetDTO>();
            List<MaterialPorColorDTO> _listMaterialPorColor = new List<MaterialPorColorDTO>();
            List<AasignarDTO> _listAasignar = new List<AasignarDTO>();

            _listLanzamientoDet = Session["LanzamientoDet"] as List<LanzamientoDetDTO>;
            _listMaterialPorColor = Session["listMaterialPorColor"] as List<MaterialPorColorDTO>;
            _listAasignar = Session["ListadoAasignar"] as List<AasignarDTO>;

            for (int i = 0; i < _listLanzamientoDet.Count; i++)
            {
                GridViewRow row = gridPreLanzamiento.Rows[i];
                _listLanzamientoDet[i].Orden = ((TextBox)row.FindControl("txtOrden")).Text;
                _listLanzamientoDet[i].Lote = short.Parse(((TextBox)row.FindControl("txtLote")).Text);
            }

            //Evaluar si se repiten Orden/Lote
            int indice = 0;
            string ordenAnterior = string.Empty;
            int loteAnterior = 0;
            bool repetido = false;
            foreach (var fila in _listLanzamientoDet)
            {
                if (indice == 0)
                {
                    ordenAnterior = fila.Orden;
                    loteAnterior = fila.Lote;
                }
                else
                {
                    if (fila.Orden == ordenAnterior && fila.Lote == loteAnterior)
                    {
                        repetido = true;
                        break;
                    }
                    else
                    {
                        ordenAnterior = fila.Orden;
                        loteAnterior = fila.Lote;
                    }
                }
                indice = indice + 1;
            }

            if (!repetido)
            {
                int registros = _lanzamientoBll.IngresarLanzamiento(_listLanzamientoDet, _listMaterialPorColor, hidUsuario.Value);
                var rpta = _asignacionOrdenBll.IngresarAsignacionOrden(_listAasignar, _listLanzamientoDet, hidUsuario.Value);
                Session["NroOrdenesGeneradas"] = registros;
                Response.Redirect("Lanzamiento.aspx");
            }
            else
            {
                //Mensaje de Orden/Lote repetido
                lblOrdenLoteRepetido.Visible = true;
            }
        }

        #region BusquedaProveedores
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            DataView dvFiltro = ((DataView)Session["ListadoProveedores"]);
            dvFiltro.RowFilter = "nombre_comercial LIKE '%" + txtNombreComercial.Text + "%'";
            Session["ListadoProveedores"] = dvFiltro;
            gridProveedores.DataSource = dvFiltro.ToTable();
            gridProveedores.DataBind();
        }

        protected void gridProveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProveedores.DataSource = ((DataView)Session["ListadoProveedores"]).ToTable();
            gridProveedores.PageIndex = e.NewPageIndex;
            gridProveedores.DataBind();
        }

        protected void gridProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridProveedores.SelectedRow;
            lblCodTaller.Text = row.Cells[0].Text.Trim();
            txtProveedor.Text = ((Button)row.FindControl("btnNombreComercial")).Text.Trim();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CloseModalPorveedores();", true);
        }

        #endregion

        protected void btnAsigOn_Click(object sender, EventArgs e)
        {
            Session["Asignado"] = "Si";
            btnAsigOn.CssClass = "btn btn-success btn-sm";
            btnAsigOff.CssClass = "btn btn-default btn-sm";
            //Remover atributos disabled
            ddlCatOperacion.Attributes.Remove("disabled");
            txtFechaRetorno.Attributes.Remove("disabled");
            btnBuscarAsig.Attributes.Remove("disabled");

            ddlCatOperacion.Attributes.Add("enabled", "true");
            txtFechaRetorno.Attributes.Add("enabled", "true");
            btnBuscarAsig.Attributes.Add("enabled", "true");
        }

        protected void btnAsigOff_Click(object sender, EventArgs e)
        {
            Session["Asignado"] = "No";
            btnAsigOn.CssClass = "btn btn-default btn-sm";
            btnAsigOff.CssClass = "btn btn-danger btn-sm";

            //Remover atributos enabled
            ddlCatOperacion.Attributes.Remove("enabled");
            txtFechaRetorno.Attributes.Remove("enabled");
            btnBuscarAsig.Attributes.Remove("enabled");

            ddlCatOperacion.Attributes.Add("disabled", "true");
            txtFechaRetorno.Attributes.Add("disabled", "true");
            btnBuscarAsig.Attributes.Add("disabled", "true");
        }

        protected void btnAceptarColor_Click(object sender, EventArgs e)
        {
            List<MaterialPorColorDTO> _listMaterialPorColor = new List<MaterialPorColorDTO>();
            foreach (GridViewRow fila in gridMaterialColor.Rows)
            {
                string codProducto = ((TextBox)fila.FindControl("txtMaterialSap")).Text;
                MaterialPorColorDTO matColor = new MaterialPorColorDTO()
                {
                    Contrato = int.Parse(hidContrato.Value),
                    Modelo = lblModelo.Text.Trim(),
                    ColorBase = rblColores.SelectedValue.ToString().Trim(),
                    Color = fila.Cells[0].Text.Trim(),
                    Calidad = codProducto.Substring(3, 3),
                    Porcentaje = decimal.Parse(fila.Cells[1].Text),
                    CodProducto = codProducto
                };
                _listMaterialPorColor.Add(matColor);
            }
            if (_listMaterialPorColor.Count > 0)
            {
                Session["listMaterialPorColor"] = _listMaterialPorColor;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "CloseModalMaterialColores();", true);
            }
        }

        protected void btnProcesarModelos_Click(object sender, EventArgs e)
        {
            int contrato = 0;
            int.TryParse(txtContrato.Text == "" ? "0" : txtContrato.Text, out contrato);
            hidContrato.Value = txtContrato.Text;
            List<ContratoDetalleDTO> _listContratoDet = _contratoBll.ListarDetalleContrato(contrato, true);

            //Cargar Correlativos por Modelos de todo el contrato
            Dictionary<string, char> modelosCorrelativos = _lanzamientoBll.ModelosCorrelativoPorMaquina(_listContratoDet);
            Session["modelosCorrelativos"] = modelosCorrelativos;

            //Filtrar por modelos seleccionados
            List<string> modelosSeleccionados = new List<string>();
            foreach (ListItem item in lstModelos.Items)
            {
                if (item.Selected)
                {
                    modelosSeleccionados.Add(item.Text.Trim());
                }
            }
            _listContratoDet = _listContratoDet.Where(x => modelosSeleccionados.Contains(x.ModeloAA.Trim())).ToList();

            List<ContratoDetalleDTO> contratoAgrupado = new List<ContratoDetalleDTO>();

            contratoAgrupado = (from cd in _listContratoDet
                                group cd by new
                                {
                                    Modelo = cd.ModeloAA,
                                    Talla1 = cd.Tallas[0],
                                    Talla2 = cd.Tallas[1],
                                    Talla3 = cd.Tallas[2],
                                    Talla4 = cd.Tallas[3],
                                    Talla5 = cd.Tallas[4],
                                    Talla6 = cd.Tallas[5],
                                    Talla7 = cd.Tallas[6],
                                    Talla8 = cd.Tallas[7],
                                    Talla9 = cd.Tallas[8],
                                    Linea = cd.Linea
                                } into grupo
                                select new ContratoDetalleDTO()
                                {
                                    ModeloAA = grupo.Key.Modelo,
                                    Tallas = new string[] { grupo.Key.Talla1, grupo.Key.Talla2, grupo.Key.Talla3,
                                                   grupo.Key.Talla4, grupo.Key.Talla5, grupo.Key.Talla6,
                                                   grupo.Key.Talla7, grupo.Key.Talla8, grupo.Key.Talla9},
                                    Linea = grupo.Key.Linea
                                }).OrderBy(x => x.ModeloAA).ToList();
            Repeater1.DataSource = contratoAgrupado;
            Repeater1.DataBind();
            Session["ListContratoDetPrin"] = _listContratoDet;

            //Cargar Pesos base de todos los modelos
            Dictionary<string, Dictionary<string, decimal>> modelPeso = new Dictionary<string, Dictionary<string, decimal>>();
            foreach (var contratoDet in contratoAgrupado)
            {
                modelPeso.Add(contratoDet.ModeloAA.Trim(), _lanzamientoBll.CalcularPesosBase(contratoDet));
                Session["dicPesosBaseContrato"] = modelPeso;
            }

            //LIMPIAR CAMPOS
            gridCantSolicitadas.DataSource = null;
            gridCantSolicitadas.DataBind();
            rblColores.Items.Clear();
            lblModelo.Text = "XXXX-XXX";
            txtMaterial.Text = string.Empty;
            lblKgNecesarios.Text = "0.0";
            txtProveedor.Text = string.Empty;
            lblCodCatOpe.Text = "0";
            btnProcesarModelos.Visible = false;
            lstModelos.Visible = false;

            ddlCatOperacion.SelectedIndex = 0;
            ddlProcesos_SelectedIndexChanged(sender, e);
            txtProveedor.Text = string.Empty;
            lblCodTaller.Text = "0";

            gridAlanzar.DataSource = null;
            gridAlanzar.DataBind();
        }
    }
}