<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="AprobacionPrecio.aspx.cs" Inherits="PSIAA.Presentation.View.AprobacionPrecio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formAprobacionPrecio">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h3 class="panel-title">Aprobación de Precio para Asignaciones</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="well well-sm">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox runat="server" ID="txtCodProveedor" class="form-control input-sm" placeholder="Código de Proveedor" />
                                                        <asp:HiddenField ID="hidCodProveedor" runat="server" />
                                                        <span class="input-group-btn">
                                                            <asp:Button ID="btnBuscarPorCod" runat="server" class="btn btn-info btn-sm" Text="Buscar" OnClick="btnBuscarPorCod_Click" />
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="input-group">
                                                        <asp:TextBox runat="server" ID="txtNombreProveedor" class="form-control input-sm" placeholder="Nombre del Proveedor" ReadOnly="true" />
                                                        <span class="input-group-btn">
                                                            <asp:Button ID="btnBuscarPorNombre" runat="server" class="btn btn-info btn-sm" Text="Buscar" data-target="#modalSelectProveedor" data-toggle="modal" OnClick="btnBuscarPorNombre_Click" />
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <span class="control-label">Modelos: </span>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ddlModelos" runat="server" class="form-control input-sm" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnBuscar_Click" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:UpdateProgress ID="UpdateProgress" runat="server">
                                                        <ProgressTemplate>
                                                            <div class="overlay" />
                                                            <div class="overlayContent">
                                                                <h2>Cargando...</h2>
                                                                <img src="../Content/Images/cargador.gif" />
                                                            </div>
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridAsignacionesAprobacion" runat="server" Width="100%"
                                                    CssClass="table table-bordered"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No existe ninguna coincidencia"
                                                    OnSelectedIndexChanged="gridAsignacionesAprobacion_SelectedIndexChanged"
                                                    OnRowDataBound="gridAsignacionesAprobacion_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="Categoria_Operacion" HeaderText="Operación" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Numero_Orden" HeaderText="Asignación" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="modelo" HeaderText="Modelo" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Orden" HeaderText="Orden" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Lote" HeaderText="Lote" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Moneda" HeaderText="Moneda" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="TarifaSoles" HeaderText="Tarifa S/." DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="TarifaDolares" HeaderText="Tarifa $." DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="CostoSoles" HeaderText="Costo S/." DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="CostoDolares" HeaderText="Costo $." DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="Precio_Aprobado" HeaderText="Flag" Visible="false" />
                                                        <asp:BoundField DataField="Fecha_de_asignacion" HeaderText="Fecha Asig." DataFormatString="{0:d}" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Fecha_Aprob_Precio" HeaderText="Fecha Aprob." DataFormatString="{0:d}" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Usuario_Aprob_Prec" HeaderText="Usuario Aprob." DataFormatString="{0:d}" ControlStyle-Width="5%" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" HeaderText="Acción">
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnPrecios" Text="Precios" class="btn btn-warning btn-xs" CommandName="Select" data-target="#modalActualizacionPrecio" data-toggle="modal" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!---- MODAL SELECCION PROVEEDOR - TALLER ---->
        <div class="modal fade" id="modalSelectProveedor">
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Listado de Proveedores</h4>
                            </div>
                            <div class="modal-body">
                                <div class="well well-sm col-sm-12">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <span class="control-label">Nombre Com.: </span>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txtNombreComercial" runat="server" class="form-control input-sm" OnTextChanged="txtNombreComercial_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                                <ProgressTemplate>
                                                    <b>Cargando</b><img src="../Content/Images/load.gif" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </div>
                                </div>
                                <div class="row table-responsive">
                                    <asp:GridView ID="gridProveedores" runat="server" Width="100%"
                                        CssClass="table table-striped table-bordered table-hover"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay registros." AllowPaging="true"
                                        OnPageIndexChanging="gridProveedores_PageIndexChanging"
                                        OnSelectedIndexChanged="gridProveedores_SelectedIndexChanged">
                                        <Columns>
                                            <asp:BoundField DataField="cod_proveedor" HeaderText="Código" ControlStyle-Width="25%" />
                                            <asp:TemplateField HeaderText="Nombre Comercial" ItemStyle-Width="55%" ItemStyle-ForeColor="DarkSlateBlue" ControlStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnNombreComercial" runat="server" Text='<%# Bind("nombre_comercial") %>' Font-Size="Smaller"
                                                        CommandName="Select" CssClass="buttonLabel"></asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ciudad" HeaderText="Ciudad" ControlStyle-Width="20%" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!---- FIN MODAL --->
        <!---- MODAL ACTUALIZACION DE PRECIO ---->
        <div class="modal fade" id="modalActualizacionPrecio">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Actualización de Precios</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-1"><span class="control-label">Modelo: </span></div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblModelo" runat="server" Font-Bold="true" Text="MMMM"></asp:Label>
                                        <asp:HiddenField ID="hidCantidad" runat="server" />
                                    </div>
                                    <div class="col-md-2"><span class="control-label">N° Asignación: </span></div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblAsignacion" runat="server" Font-Bold="true" Text="AAAA"></asp:Label>
                                    </div>
                                    <div class="col-md-1"><span class="control-label">Orden/Lt.: </span></div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblOrdenLote" runat="server" Font-Bold="true" Text="OOOO/1"></asp:Label>
                                        <asp:HiddenField ID="hidOrden" runat="server" />
                                        <asp:HiddenField ID="hidLote" runat="server" />
                                        <asp:HiddenField ID="hidCatOpe" runat="server" />
                                    </div>
                                    <div class="col-md-1"><span class="control-label">Prendas: </span></div>
                                    <div class="col-md-1">
                                        <asp:Label ID="lblCantidad" runat="server" Font-Bold="true" Text="PP"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-2"><span class="control-label">Tarifa: </span></div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtTarifa" runat="server" class="form-control input-sm" onfocus="ValueDefaultFocus(this);" onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"><span class="control-label">Moneda: </span></div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlMoneda" runat="server" class="form-control input-sm">
                                            <asp:ListItem Value="S" Text="Soles"></asp:ListItem>
                                            <asp:ListItem Value="D" Text="Dólares"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                            <ProgressTemplate>
                                                <b>Cargando</b><img src="../Content/Images/load.gif" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button runat="server" ID="btnActualizarCostos" Text="Actualizar Costos" class="btn btn-primary btn-sm" OnClick="btnActualizarCostos_Click" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gridPreciosProcesos" runat="server" Width="100%"
                                            CssClass="table table-striped table-bordered table-hover"
                                            AutoGenerateColumns="False"
                                            EmptyDataText="Las Operaciones asignadas no coinciden con los tiempos establecidos."
                                            ShowFooter="true"
                                            OnRowDataBound="gridPreciosProcesos_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="CategoriaOperacion" HeaderText="Cat." ControlStyle-Width="10%" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ControlStyle-Width="50%" />
                                                <asp:BoundField DataField="Tiempo" HeaderText="Tiempo" ControlStyle-Width="10%" />
                                                <asp:BoundField DataField="CostoSoles" HeaderText="Costo S/." DataFormatString="{0:F}" ControlStyle-Width="15%" />
                                                <asp:BoundField DataField="CostoDolares" HeaderText="Costo $." DataFormatString="{0:F}" ControlStyle-Width="15%" />
                                                <asp:BoundField DataField="Proceso" HeaderText="Cod." Visible="false" ControlStyle-Width="10%" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-10 col-sm-offset-1">
                                        <asp:Label runat="server" ID="lblMensajeError" Visible="False">
                                            <div class="alert alert-dismissible alert-danger">
                                                <button type="button" class="close" data-dismiss="alert">&times;</button>
                                                <strong>Hubo un error al guardar los precios.</strong>
                                            </div>
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="row">
                                    <div class="col-md-2">
                                        <span class="control-label">Fecha de Aprob.:</span>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <div class="input-group date" id="FechaAprobacion">
                                                <asp:TextBox runat="server" ID="txtFechaAprob" type="text" class="form-control input-sm" />
                                                <span class="input-group-addon input-sm">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="checkbox">
                                            <label>
                                                <b>
                                                    <asp:CheckBox ID="chkAprobado" runat="server" Text="Aprobado" /></b>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">
                                        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cancelar</button>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" class="btn btn-success btn-sm" OnClick="btnGuardar_Click" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!---- FIN MODAL PRECIOS --->
    </form>
    <script type="text/javascript">
        $(function () {
            //CallDateTimePicker();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                CallDateTimePicker();
            }

            function CallDateTimePicker() {
                var fechaHoy = new Date();
                $('#FechaAprobacion').datetimepicker({
                    format: 'YYYY-MM-DD',
                    minDate: '2017-01-01',
                    maxDate: fechaHoy.setDate(fechaHoy.getDate() + 7),
                    defaultDate: 'now'
                }).data("autoclose", true);
            }
        });

        function CloseModalPorveedores() {
            $('#modalSelectProveedor').modal('hide');
        }

        function CloseModalPrecios() {
            $('#modalActualizacionPrecio').modal('hide');
        }

        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return ((tecla > 47 && tecla < 58) || tecla == 46);
        }

        function ValueDefaultZero(obj) {
            obj.value = obj.value.trim() == "" ? "0.00" : obj.value;
        }

        function ValueDefaultFocus(obj) {
            obj.value = obj.value.trim() != "0.00" ? obj.value : "";
        }
    </script>
</asp:Content>
