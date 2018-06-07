<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="DocumentosLibres.aspx.cs" Inherits="PSIAA.Presentation.View.DocumentosLibres" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formDocumentosLibres">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Listado de Pagos Libres</h3>
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
                                                        <asp:HiddenField ID="hidUsuario" runat="server" />
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
                                                    <span class="control-label">Periodo:</span>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:DropDownList ID="ddlPeriodos" runat="server" class="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodos_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <span class="control-label">Mes:</span>
                                                        </div>
                                                        <div class="col-md-9">
                                                            <asp:DropDownList ID="ddlMeses" runat="server" class="form-control input-sm" DataTextField="Value" DataValueField="Key">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary btn-sm" Text="Buscar" Visible="false" OnClick="btnBuscar_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridDocumentosLibres" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No existe ninguna coincidencia"
                                                    OnSelectedIndexChanged="gridDocumentosLibres_SelectedIndexChanged">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="Acción">
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnImprimir" Text="Ver PDF" class="btn btn-info btn-xs" CommandName="Select" data-target="#modalDocumentoPagoLibre" data-toggle="modal" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="TipoMov" HeaderText="Tipo Mov." ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="serie_documento" HeaderText="Serie" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="nro_documento" HeaderText="N° Liquid." ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="fecha_documento" HeaderText="Fecha Doc." DataFormatString="{0:d}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="moneda" HeaderText="Moneda" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="sub_total" HeaderText="Sub Total" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="igv" HeaderText="Igv" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="usuario" HeaderText="Usuario" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="total" HeaderText="Total" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="Semana" HeaderText="Sem." ControlStyle-Width="5%" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6 text-left">
                                            <asp:Label ID="lblRespuesta" runat="server" Text="" Visible="false"></asp:Label>
                                        </div>
                                        <div class="col-md-6 text-right">
                                            <asp:Button ID="btnNuevo" runat="server" class="btn btn-primary btn-sm" Text="Nuevo Documento" Visible="false" data-target="#modalNuevoPagoLibre" data-toggle="modal" OnClick="btnNuevo_Click" />
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
                                        <div class="col-md-4">
                                            <span class="control-label">Nombre Comercial: </span>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtNombreComercial" runat="server" class="form-control input-sm" OnTextChanged="txtNombreComercial_TextChanged"></asp:TextBox>
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
        <!---- MODAL NUEVO PAGO LIBRE ---->
        <div class="modal fade" id="modalNuevoPagoLibre">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Nuevo Documento de Pago Libre</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-7">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <span class="control-label">Semana/Fecha: </span>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="lblSemanaFecha" runat="server" Font-Bold="true" Text="--"></asp:Label>
                                            </div>
                                            <div class="col-md-2">
                                                <span class="control-label">RUC: </span>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblRuc" runat="server" Font-Bold="true" Text="NNN"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <span class="control-label">Orden: </span>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtOrden" runat="server" class="form-control input-sm" Style="text-transform: uppercase"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <span class="control-label">Lote: </span>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtLote" runat="server" class="form-control input-sm" onkeypress="return ValidNum(event)"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-7">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <span class="control-label">Operacion: </span>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:DropDownList ID="ddlOperacionesLibres" runat="server" class="form-control input-sm" DataTextField="Value" DataValueField="Key" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <span class="control-label">Talla: </span>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtTalla" runat="server" class="form-control input-sm" Style="text-transform: uppercase"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <span class="control-label">Prendas: </span>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtPrendas" runat="server" class="form-control input-sm" onkeypress="return ValidNum(event)"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-7">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <span class="control-label">Observaciones: </span>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="60" TextMode="multiline" Rows="4" Style="resize: none; text-transform: uppercase;" class="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <span class="control-label">Tiempo: </span>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtTiempo" runat="server" class="form-control input-sm" onkeypress="return ValidNum(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <span class="control-label">Tarifa: </span>
                                    </div>
                                    <div class="col-md-2 text-right">
                                        <asp:TextBox ID="txtPrecio" runat="server" class="form-control input-sm" onfocus="ValueDefaultFocus(this);" onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)"></asp:TextBox>
                                        <br />
                                        <asp:Label ID="lblInexistenciaOrden" runat="server" ForeColor="red" Text="Orden no existe" Visible="false"></asp:Label>
                                        <asp:Button ID="btnAgregar" runat="server" class="btn btn-info btn-sm" Text="Agregar" OnClick="btnAgregar_Click" OnClientClick="return ValidarVaciosSerieNumero();" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gridDetalleDocLibre" runat="server" Width="100%"
                                            CssClass="table table-striped table-bordered table-hover"
                                            AutoGenerateColumns="False"
                                            EmptyDataText="No hay ningun registro." ShowFooter="true"
                                            OnSelectedIndexChanged="gridDetalleDocLibre_SelectedIndexChanged"
                                            OnRowDataBound="gridDetalleDocLibre_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" HeaderText="Acción">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btnQuitar" Text="Quitar" class="btn btn-danger btn-xs" CommandName="Select" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DenominacionOper" HeaderText="Operacion" ControlStyle-Width="40%" />
                                                <asp:BoundField DataField="Orden" HeaderText="Orden" ControlStyle-Width="10%" />
                                                <asp:BoundField DataField="Lote" HeaderText="Lote" ControlStyle-Width="3%" />
                                                <asp:BoundField DataField="Prendas" HeaderText="Prendas" ControlStyle-Width="5%" />
                                                <asp:BoundField DataField="Talla" HeaderText="Talla" ControlStyle-Width="10%" />
                                                <asp:BoundField DataField="Tiempo" HeaderText="Tiempo" ControlStyle-Width="5%" />
                                                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-1">
                                        <span class="control-label">Moneda: </span>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlMoneda" runat="server" class="form-control input-sm">
                                            <asp:ListItem Value="S" Text="Soles"></asp:ListItem>
                                            <asp:ListItem Value="D" Text="Dólares"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <span class="control-label">Tipo de Movimiento: </span>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddlTipoMov" runat="server" class="form-control input-sm">
                                            <asp:ListItem Value="01" Text="Factura"></asp:ListItem>
                                            <asp:ListItem Value="02" Text="Recibo por Honorario"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3"></div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="row">
                                    <div class="col-md-6"></div>
                                    <div class="col-md-3">
                                        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cancelar</button>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Button ID="btnGuardar" runat="server" class="btn btn-success btn-sm" Text="Guardar" OnClick="btnGuardar_Click" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!-----FIN MODAL PAGO LIBRE---->
        <!---- MODAL DOCUMENTO DE FACTURA --->
        <div class="modal fade" id="modalDocumentoPagoLibre">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Documento de Pago Libre Emitido</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12 text-center">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <b>Cargando</b><img src="../Content/Images/load.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive text-center">
                                    <div id="reporte" style="color: white;">
                                        <rsweb:ReportViewer ID="rptViewPagoLibre" runat="server" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="True" Visible="false">
                                            <LocalReport ReportPath="../PSIAA.Reports/rptPagoLibre.rdlc" ShowDetailedSubreportMessages="True">
                                            </LocalReport>
                                        </rsweb:ReportViewer>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="text-align: center;">
                            <iframe id="frmPDF" style="width: 850px; height: 400px;"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!----FIN MODAL DOCUMENTO --->
    </form>
    <script type="text/javascript">
        function CloseModalPorveedores() {
            $('#modalSelectProveedor').modal('hide');
        }

        function CloseModalNuevoPago() {
            $('#modalNuevoPagoLibre').modal('hide');
        }

        function ValidarVaciosSerieNumero() {
            var txtOrden = document.getElementById("ContentBody_ContentInitBody_txtOrden");
            var txtLote = document.getElementById("ContentBody_ContentInitBody_txtLote");
            var txtTalla = document.getElementById("ContentBody_ContentInitBody_txtTalla");
            var txtPrendas = document.getElementById("ContentBody_ContentInitBody_txtPrendas");
            var txtTiempo = document.getElementById("ContentBody_ContentInitBody_txtTiempo");
            var txtPrecio = document.getElementById("ContentBody_ContentInitBody_txtPrecio");

            var validados = 6;

            txtOrden.style.borderColor = 'LightGray';
            txtLote.style.borderColor = 'LightGray';
            txtTalla.style.borderColor = 'LightGray';
            txtPrendas.style.borderColor = 'LightGray';
            txtTiempo.style.borderColor = 'LightGray';
            txtPrecio.style.borderColor = 'LightGray';

            if (txtOrden.value == "") {
                txtOrden.style.borderColor = 'Red';
                validados--;
            }
            if (txtLote.value == "") {
                txtLote.style.borderColor = 'Red';
                validados--;
            }
            if (txtTalla.value == "") {
                txtTalla.style.borderColor = 'Red';
                validados--;
            }
            if (txtPrendas.value == "") {
                txtPrendas.style.borderColor = 'Red';
                validados--;
            }
            if (txtTiempo.value == "") {
                txtTiempo.style.borderColor = 'Red';
                validados--;
            }
            if (txtPrecio.value == "") {
                txtPrecio.style.borderColor = 'Red';
                validados--;
            }
            return (validados == 6);
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

        function CargarDocumento(nombreDoc, servidor) {
            var iframe = document.getElementById('frmPDF');
            iframe.src = "http://"+ servidor +"/PSIAA/Reports/Docs/" + nombreDoc + ".pdf";
        }
    </script>
</asp:Content>
