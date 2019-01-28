<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="GeneracionDocumento.aspx.cs" Inherits="PSIAA.Presentation.View.GeneracionDocumento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formGeneracionDocumento">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Generación de Documento para Facturación</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
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
                                                    <div class="col-md-1"></div>
                                                    <div class="col-md-1">
                                                        <span class="control-label">Tipo Doc.:</span>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:DropDownList runat="server" ID="ddlTipoDocs" CssClass="form-control input-sm">
                                                            <asp:ListItem Value="01">Factura</asp:ListItem>
                                                            <asp:ListItem Value="02">Recibo por Honorario</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        <span class="control-label">Moneda:</span>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlMoneda" runat="server" class="form-control input-sm">
                                                            <asp:ListItem Value="S" Text="Soles"></asp:ListItem>
                                                            <asp:ListItem Value="D" Text="Dólares"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <span class="control-label">Fecha de Aprobación Precio:</span>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <div class="input-group date" id="FechaAprobacion">
                                                                <asp:TextBox runat="server" ID="txtFechaAprobPrecio" type="text" class="form-control input-sm" />
                                                                <span class="input-group-addon input-sm">
                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1"></div>

                                                    <div class="col-md-1">
                                                        <span class="control-label">Fecha Em.:</span>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:Label runat="server" ID="lblFechaEmision" Font-Bold="true" Text="--"></asp:Label>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <span class="control-label">Semana:</span>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:Label runat="server" ID="lblSemana" Font-Bold="true" Text="--"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 text-right">
                                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar Asignaciones" class="btn btn-primary btn-sm" Visible="false" OnClick="btnBuscar_Click" OnClientClick="return ValidarVaciosSerieNumero();" />
                                                    </div>
                                                    <div>
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
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridDetalleAsignaciones" runat="server" Width="100%"
                                                    CssClass="table table-bordered"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No existe ninguna coincidencia"
                                                    OnSelectedIndexChanged="gridDetalleAsignaciones_SelectedIndexChanged">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="3%">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSeleccionarTodo" runat="server" onclick="checkAll(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSeleccionar" runat="server" onclick="checkOne(this)" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Categoria_Operacion" HeaderText="Operación" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="NumeroAsignacion" HeaderText="Asignación" ControlStyle-Width="15%" />
                                                        <asp:BoundField DataField="Orden" HeaderText="Orden" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="Lote" HeaderText="Lote" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Categoria" HeaderText="Categoria" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="modelo" HeaderText="Modelo" ControlStyle-Width="15%" />
                                                        <asp:BoundField DataField="Color" HeaderText="Color" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Precio_Unitario" HeaderText="Tarifa S/." DataFormatString="{0:F}" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Costo_Soles" HeaderText="Costo S/." DataFormatString="{0:F}" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Tarifa_Dolares" HeaderText="Tarifa $" DataFormatString="{0:F}" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Costo_Dolares" HeaderText="Costo $" DataFormatString="{0:F}" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Fecha_de_asignacion" DataFormatString="{0:d}" HeaderText="Fecha Asig." ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Usuario_Aprob_Prec" HeaderText="Usuario Aprob." ControlStyle-Width="5%" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" HeaderText="Acción">
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnDetalle" Text="Detalle" class="btn btn-info btn-xs" CommandName="Select" data-target="#modalProcesosAsignacion" data-toggle="modal" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="well well-sm">
                                                <div class="row">
                                                    <div class="col-md-8">
                                                        <table id="dataTable" class="table table-bordered">
                                                        </table>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <asp:Label ID="lblNRegistros" runat="server" class="control-label" Font-Bold="true">-</asp:Label>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <asp:Label runat="server" ID="lblMensajeError" Visible="False">
                                                            <div class="alert alert-dismissible alert-danger">
                                                                <button type="button" class="close" data-dismiss="alert">&times;</button>
                                                                <strong>Error. Debe seleccionar al menos un registro.</strong>
                                                             </div>
                                                                </asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12 text-right">
                                                                <asp:Button ID="btnGrabar" runat="server" Text="Grabar Documento" class="btn btn-success btn-sm" OnClick="btnGrabar_Click" Visible="false" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblRespuesta" runat="server" Text="" Visible="false"></asp:Label>
                                                    </div>
                                                </div>
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
        <!---- MODAL PROCESOS POR ASIGNACION --->
        <div class="modal fade" id="modalProcesosAsignacion">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Detalle de Liquidación</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="row table-responsive">
                                    <div class="well well-sm">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <span class="control-label">Modelo:</span>
                                                <asp:Label ID="lblModelo" runat="server" Font-Bold="True"></asp:Label>
                                            </div>
                                            <div class="col-md-3">
                                                <span class="control-label">Asignación:</span>
                                                <asp:Label ID="lblAsignacion" runat="server" Font-Bold="True"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <span class="control-label">Orden/Lote:</span>
                                                <asp:Label ID="lblOrdenLote" runat="server" Font-Bold="True"></asp:Label>
                                            </div>
                                            <div class="col-md-2">
                                                <span class="control-label">Categoria:</span>
                                                <asp:Label ID="lblCategoria" runat="server" Font-Bold="True"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:GridView ID="gridProcesosAsignacion" runat="server" Width="100%"
                                        CssClass="table table-striped table-bordered table-hover"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay procesos para esa asignacion.">
                                        <Columns>
                                            <asp:BoundField DataField="Proceso" HeaderText="Proceso" ControlStyle-Width="40%" />
                                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" ControlStyle-Width="10%" />
                                            <asp:BoundField DataField="Tiempo" HeaderText="Tiempo" ControlStyle-Width="10%" />
                                            <asp:BoundField DataField="TarifaSoles" HeaderText="Tarifa S/." DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                            <asp:BoundField DataField="CostoSoles" HeaderText="Costo S/." DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                            <asp:BoundField DataField="TarifaDolares" HeaderText="Tarifa $." DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                            <asp:BoundField DataField="CostoDolares" HeaderText="Costo $." DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <!---- FIN MODAL ---->
    </form>
    <script src="../Scripts/GeneracionDocumento.js"></script>
    <script type="text/javascript">
        //Array para Agrupar
        var dataArray = [];

        var totalSoles = 0;
        var totalDolares = 0;

        dataArray.length = 0;

        $(function () {
            CallDateTimePicker();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                CallDateTimePicker();
                dataArray.length = 0;
                totalSoles = 0;
                totalDolares = 0;
                //Limpiar Checks
                $("input:checkbox").prop('checked', false);
            }

            function CallDateTimePicker() {
                $('#FechaAprobacion').datetimepicker({
                    format: 'YYYY-MM-DD',
                    minDate: '2012-01-01',
                    defaultDate: 'now'
                }).data("autoclose", true);
            }
        });

        function ValidarVaciosSerieNumero() {
            var txtserie = document.getElementById("ContentBody_ContentInitBody_txtSerie");
            var txtnumero = document.getElementById("ContentBody_ContentInitBody_txtNumero");
            var validados = 2;

            txtnumero.style.borderColor = 'LightGray';
            txtserie.style.borderColor = 'LightGray';

            if (txtnumero.value == "") {
                txtnumero.style.borderColor = 'Red';
                validados--;
            }

            if (txtserie.value == "") {
                txtserie.style.borderColor = 'Red';
                validados--;
            }
            return (validados == 2);
        }

        //Funcion para Seleccionar una Fila
        function checkOne(objRef) {
            //Array para capturar fila del gridView
            var fila = [];
            var row = objRef.parentNode.parentNode;

            //Llenar Array con valores del row
            $(row).each(function (index, item) {
                $('td', $(this)).each(function (index, item) {
                    fila[index] = $(item).html();
                });
            });

            var item = {
                Modelo: fila[6].toString().trim(),
                Punto: fila[5],
                Cantidad: parseInt(fila[8]),
                CostoSoles: parseFloat(fila[10]),
                CostoDolares: parseFloat(fila[12])
            };

            if (objRef.checked) {
                row.style.backgroundColor = "#99caf5";
                //Agregar elemento en el array de agrupacion
                dataArray.push(item);
            }
            else {
                row.style.backgroundColor = "white";
                //Quitar elemento en el array de agrupacion
                var posicion = -1;
                for (var i = 0; i < dataArray.length; i++) {
                    if ((dataArray[i].Modelo === item.Modelo)
                        && (dataArray[i].Punto === item.Punto)
                        && (dataArray[i].Cantidad === item.Cantidad)
                        && (dataArray[i].CostoSoles === item.CostoSoles)
                        && (dataArray[i].CostoDolares === item.CostoDolares)) {
                        posicion = i;
                    }
                }
                dataArray.splice(posicion, 1);
            }

            var grupoElementos = GroupByArrayObjects(dataArray);
            ParseJsonToTable(grupoElementos);
        }

        //Funcion para Seleccionar/Deseleccionar todas las Filas
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");

            var fila = [];
            //Limpiar ArrayPrincipal
            dataArray.length = 0;

            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    //Llenar Array con valores del row
                    $(row).each(function (index, item) {
                        $('td', $(this)).each(function (index, item) {
                            fila[index] = $(item).html();
                        });
                    });

                    var item = {
                        Modelo: fila[6].toString().trim(),
                        Punto: fila[5],
                        Cantidad: parseInt(fila[8]),
                        CostoSoles: parseFloat(fila[10]),
                        CostoDolares: parseFloat(fila[12])
                    };

                    if (objRef.checked) {
                        row.style.backgroundColor = "#99caf5";
                        inputList[i].checked = true;
                        //Agregar elemento en el array de agrupacion
                        dataArray.push(item);
                    }
                    else {
                        row.style.backgroundColor = "white";
                        inputList[i].checked = false;
                    }
                }
            }

            var grupoElementos = GroupByArrayObjects(dataArray);
            ParseJsonToTable(grupoElementos);
        }

        function CloseModalPorveedores() {
            $('#modalSelectProveedor').modal('hide');
        }

        //Funcion para Validar escritura de numeros
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return ((tecla > 47 && tecla < 58) || tecla == 46);
        }
    </script>
</asp:Content>
