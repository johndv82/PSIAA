<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="AvanceContrato.aspx.cs" Inherits="PSIAA.Presentation.View.AvanceContrato" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formGrid">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Avance de Ingresos por Contrato</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="well well-sm">
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <asp:RadioButtonList ID="rbnFiltros" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                            RepeatLayout="Flow"
                                                            CssClass="radioboxlist" OnSelectedIndexChanged="rbnFiltros_SelectedIndexChanged">
                                                            <asp:ListItem Value="cliente" Text="Cliente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="modelo" Text="Modelo"></asp:ListItem>
                                                            <asp:ListItem Value="contrato" Text="Contrato"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="input-group">
                                                            <span class="input-group-addon input-sm">Nombre:</span>
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control input-sm" autocomplete="off"/>
                                                            <asp:HiddenField ID="hidContrato" runat="server" />
                                                            <span class="input-group-btn">
                                                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnBuscar_Click" />
                                                            </span>
                                                            <asp:HiddenField ID="hidCustomerId" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlContratos" runat="server" class="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlContratos_SelectedIndexChanged" />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Button ID="btnAvanceDetallado" runat="server" Text="Avance Detallado" class="btn btn-success btn-sm" Visible="false" data-target="#modalAvanceTallas" data-toggle="modal" OnClick="btnAvanceDetallado_Click" />
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
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridAvanceContrato" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No existe ninguna coincidencia"
                                                    OnSelectedIndexChanged="gridAvanceContrato_SelectedIndexChanged"
                                                    OnRowDataBound="gridAvanceContrato_RowDataBound" ShowFooter="True"
                                                    OnRowCommand="gridAvanceContrato_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Contrato" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="hiddenContrato" Value='<%# Bind("Contrato") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Modelo" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblModelo" runat="server" Text='<%# Bind("Modelo") %>' Font-Size="Smaller" Font-Bold="true"
                                                                    CommandName="Select" data-target="#modalHojaEspecificaciones" data-toggle="modal" CssClass="buttonLabel" CommandArgument="0"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Color" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblColor" runat="server" Text='<%# Bind("Color") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Solicitado" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSolicitado" runat="server" Text='<%# Bind("Solicitado") %>' Font-Size="Smaller" Font-Bold="True"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Lanzado" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLanzado" runat="server" Text='<%# Bind("Lanzado") %>' Font-Size="Smaller" Font-Bold="True" ForeColor='<%# Eval("Lanzado").ToString() != Eval("Solicitado").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asig. Tejido" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblAsigTejido" runat="server" Text='<%# Bind("AsigTejido") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("AsigTejido").ToString() != Eval("Lanzado").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="400"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tejido" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblTejido" runat="server" Text='<%# Bind("Tejido") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("Tejido").ToString() != Eval("AsigTejido").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="430"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cont. Tejido" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblCtrlTejido" runat="server" Text='<%# Bind("CtrlTejido") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("CtrlTejido").ToString() != Eval("Tejido").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="440"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Lavado" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblLavado" runat="server" Text='<%# Bind("Lavado") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("Lavado").ToString() != Eval("CtrlTejido").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="450"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cont. Lavado" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblCtrlLavado" runat="server" Text='<%# Bind("CtrlLavado") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("CtrlLavado").ToString() != Eval("Lavado").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="460"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Corte" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblCorte" runat="server" Text='<%# Bind("Corte") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("Corte").ToString() != Eval("CtrlLavado").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="470"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asig. Confeccion" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblAsigConfeccion" runat="server" Text='<%# Bind("AsigConfeccion") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("AsigConfeccion").ToString() != Eval("Corte").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="500"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Confeccion" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblConfeccion" runat="server" Text='<%# Bind("Confeccion") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("Confeccion").ToString() != Eval("AsigConfeccion").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="510"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Acab. Confeccion" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblAcabConfeccion" runat="server" Text='<%# Bind("AcabConfeccion") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("AcabConfeccion").ToString() != Eval("Confeccion").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="530"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Acab. Final" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblAcabFinal" runat="server" Text='<%# Bind("AcabFinal") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("AcabFinal").ToString() != Eval("AcabConfeccion").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="550"></asp:Button>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Almacen" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lblAlmacen" runat="server" Text='<%# Bind("Almacen") %>' Font-Size="Smaller" Font-Bold="True"
                                                                    ForeColor='<%# Eval("Almacen").ToString() != Eval("AcabFinal").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'
                                                                    CommandName="Select" data-target="#modalDetalleAvance" data-toggle="modal" CssClass="buttonLabel" CommandArgument="800"></asp:Button>
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
                    <!----MODAL AVANCE ---->
                    <div class="modal fade" id="modalDetalleAvance">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title">Avance por Modelo y Color</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="row table-responsive">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="well well-sm">
                                                    <div class="row">
                                                        <div class="col-md-1">
                                                            <label class="control-label">Punto:</label>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="input-group">
                                                                <span class="input-group-addon input-sm">N°</span>
                                                                <asp:DropDownList runat="server" ID="ddlPuntos" CssClass="form-control input-sm">
                                                                    <asp:ListItem Value="400">Asignacion Tejido</asp:ListItem>
                                                                    <asp:ListItem Value="430">Tejido</asp:ListItem>
                                                                    <asp:ListItem Value="440">Control Tejido</asp:ListItem>
                                                                    <asp:ListItem Value="450">Lavado</asp:ListItem>
                                                                    <asp:ListItem Value="460">Control Lavado</asp:ListItem>
                                                                    <asp:ListItem Value="470">Corte</asp:ListItem>
                                                                    <asp:ListItem Value="500">Asignacion Confeccion</asp:ListItem>
                                                                    <asp:ListItem Value="510">Confeccion</asp:ListItem>
                                                                    <asp:ListItem Value="530">Acabados de Confeccion</asp:ListItem>
                                                                    <asp:ListItem Value="550">Acabado Final</asp:ListItem>
                                                                    <asp:ListItem Value="800">Ingreso Almacen</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:Button runat="server" class="btn btn-primary btn-sm" ID="btnFiltrar" Text="Filtrar" OnClick="btnFiltrar_Click" />
                                                        </div>
                                                        <div class="col-md-5">
                                                            <div class="form-group">
                                                                <span class="col-md-2">Modelo:</span>
                                                                <div class="col-md-5 text-center">
                                                                    <asp:Label ID="lblModelo" runat="server" Font-Bold="True"></asp:Label>
                                                                </div>
                                                                <span class="col-md-2">Color:</span>
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="lblColor" runat="server" Font-Bold="True"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div>
                                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                                                <ProgressTemplate>
                                                                    <img src="../Content/Images/load.gif" />
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!-- GRILLA DE AVANCE POR MODELO-->
                                                <asp:GridView ID="gridDetalleAvance" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No hay información." Visible="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Orden" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrden" runat="server" Text='<%# Bind("Orden") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Lote" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLote" runat="server" Text='<%# Bind("Lote") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTalla" runat="server" Text='<%# Bind("Talla") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cantidad" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad" runat="server" Text='<%# Bind("Cantidad") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Usuario" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUsuario" runat="server" Text='<%# Bind("usuario") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fecha Ing." ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFechaIngreso" runat="server" Text='<%# Eval("FechaIngreso","{0:d}")  %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Hora Ing" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHoraIngreso" runat="server" Text='<%# Bind("HoraIngreso") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Peso" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPeso" runat="server" Text='<%# Bind("Peso") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Taller" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTaller" runat="server" Text='<%# Bind("Taller") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nombre Comer." ItemStyle-Width="8%" HeaderStyle-Width="8%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNombreComercial" runat="server"
                                                                    Text='<%# Eval("NombreComercial").ToString().Trim().Length > 15 ? Eval("NombreComercial").ToString().Trim().Substring(0, 15)+"..." : Eval("NombreComercial").ToString().Trim() %>'
                                                                    Font-Size="Smaller" ToolTip='<%# Bind("NombreComercial") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <!-- FIN GRILLA -->
                                                <!-- GRILLA PARA ASIGNACIONES DE TEJIDO Y CONFECCION -->
                                                <asp:GridView ID="gridDetalleAsignaciones" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No hay información.">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Cod. Taller" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCodTaller" runat="server" Text='<%# Bind("cod_proveedor") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Taller" ItemStyle-Width="3%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTaller" runat="server"
                                                                    Text='<%# Eval("NombreComercial").ToString().Trim().Length > 10 ? Eval("NombreComercial").ToString().Trim().Substring(0, 10)+"..." : Eval("NombreComercial").ToString().Trim() %>'
                                                                    Font-Size="Smaller" ToolTip='<%# Bind("NombreComercial") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fec. Asig." ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFechaAsignacion" runat="server" Text='<%# Eval("FechaAsignacion","{0:d}") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fec. Term." ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFechaTermino" runat="server" Text='<%# Eval("FechaTermino","{0:d}") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCant1" runat="server" Text='<%# Eval("C1").ToString() == "0" ? "" : Eval("C1").ToString()  %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCant2" runat="server" Text='<%# Eval("C2").ToString() == "0" ? "" : Eval("C2").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCant3" runat="server" Text='<%# Eval("C3").ToString() == "0" ? "" : Eval("C3").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCant4" runat="server" Text='<%# Eval("C4").ToString() == "0" ? "" : Eval("C4").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCant5" runat="server" Text='<%# Eval("C5").ToString() == "0" ? "" : Eval("C5").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCant6" runat="server" Text='<%# Eval("C6").ToString() == "0" ? "" : Eval("C6").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCant7" runat="server" Text='<%# Eval("C7").ToString() == "0" ? "" : Eval("C7").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCant8" runat="server" Text='<%# Eval("C8").ToString() == "0" ? "" : Eval("C8").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCant9" runat="server" Text='<%# Eval("C9").ToString() == "0" ? "" : Eval("C9").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <!-- FIN GRILLA ASIGNACIONES -->
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cerrar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!---- FIN MODAL AVANCE --->
                    <!--- MODAL AVANCE POR TALLAS-->
                    <div class="modal fade" id="modalAvanceTallas">
                        <div class="modal-dialog modal-lg" style="width: 80% !important;">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title">Avance Detallado por Tallas</h4>
                                </div>
                                <div class="modal-body" id="modalBody">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <div class="row table-responsive">
                                                <div class="well well-sm">
                                                    <div class="row">
                                                        <div class="col-md-2">
                                                            <span class="control-label">Contrato:</span>
                                                            <asp:Label ID="lblContrato" runat="server" Font-Bold="True"></asp:Label>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <span class="control-label">Número P.O.:</span>
                                                            <asp:Label ID="lblPo" runat="server" Font-Bold="True"></asp:Label>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <span class="control-label">Cliente:</span>
                                                            <asp:Label ID="lblCliente" runat="server" Font-Bold="True"></asp:Label>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <a href="#" id="linkSolicitado" class="btn btn-link" onclick="moverScroll();">Solicitado</a>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-12 text-center">
                                                            <asp:RadioButtonList ID="rblPuntosControl" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                                OnSelectedIndexChanged="rblPuntosControl_SelectedIndexChanged1" RepeatLayout="Flow"
                                                                CssClass="radioboxlist">
                                                                <asp:ListItem Value="300" Text="Lanzamiento"></asp:ListItem>
                                                                <asp:ListItem Value="430" Text="Tejido"></asp:ListItem>
                                                                <asp:ListItem Value="440" Text="Ctrl. Tejido"></asp:ListItem>
                                                                <asp:ListItem Value="450" Text="Lavado"></asp:ListItem>
                                                                <asp:ListItem Value="460" Text="Ctrl. Lavado"></asp:ListItem>
                                                                <asp:ListItem Value="470" Text="Corte"></asp:ListItem>
                                                                <asp:ListItem Value="510" Text="Confeccion"></asp:ListItem>
                                                                <asp:ListItem Value="530" Text="Acabado Conf."></asp:ListItem>
                                                                <asp:ListItem Value="550" Text="Acabado Final"></asp:ListItem>
                                                                <asp:ListItem Value="800" Text="Almacen"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!-- GRILLA PARA AVANCE POR TALLAS -->
                                                <asp:GridView ID="gridAvanceTallas" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="Ningun Dato para Mostrar.">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Modelo" ItemStyle-Width="3%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblModelo" runat="server" Text='<%# Eval("modelo").ToString().Trim() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Color" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblColor" runat="server" Text='<%# Eval("Color")%>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="T1" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTalla1" runat="server" Text='<%# Bind("talla1") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="T2" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTalla2" runat="server" Text='<%# Bind("talla2") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="T3" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTalla3" runat="server" Text='<%# Bind("talla3") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="T4" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTalla4" runat="server" Text='<%# Bind("talla4") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="T5" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTalla5" runat="server" Text='<%# Bind("talla5") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="T6" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTalla6" runat="server" Text='<%# Bind("talla6") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="T7" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTalla7" runat="server" Text='<%# Bind("talla7") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="T8" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTalla8" runat="server" Text='<%# Bind("talla8") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="T9" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbltalla9" runat="server" Text='<%# Bind("talla9") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla 1" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad1" runat="server" Text='<%# Eval("C1").ToString() == "0" ? "" : Eval("C1").ToString()   %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla 2" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad2" runat="server" Text='<%# Eval("C2").ToString() == "0" ? "" : Eval("C2").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla 3" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad3" runat="server" Text='<%# Eval("C3").ToString() == "0" ? "" : Eval("C3").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla 4" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad4" runat="server" Text='<%# Eval("C4").ToString() == "0" ? "" : Eval("C4").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla 5" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad5" runat="server" Text='<%# Eval("C5").ToString() == "0" ? "" : Eval("C5").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla 6" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad6" runat="server" Text='<%# Eval("C6").ToString() == "0" ? "" : Eval("C6").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla 7" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad7" runat="server" Text='<%# Eval("C7").ToString() == "0" ? "" : Eval("C7").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla 8" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad8" runat="server" Text='<%# Eval("C8").ToString() == "0" ? "" : Eval("C8").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla 9" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCantidad9" runat="server" Text='<%# Eval("C9").ToString() == "0" ? "" : Eval("C9").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <!-- FIN GRILLA AVANCE POR TALLAS -->
                                            </div>
                                            <div class="row table-responsive" id="fieldSolicitado">
                                                <fieldset>
                                                    <legend style="color: royalblue">Detalle de Contrato Solicitado</legend>
                                                    <!-- GRILLA DETALLE CONTRATO -->
                                                    <asp:GridView ID="gridDetalleSolicitadoContrato" runat="server" Width="100%"
                                                        CssClass="table table-striped table-bordered table-hover"
                                                        AutoGenerateColumns="False"
                                                        EmptyDataText="Ningun Dato para Mostrar.">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="N°" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Modelo Cl." ItemStyle-Width="3%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModeloCli" runat="server" Text='<%# Eval("ModeloCliente")%>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Color Cl." ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblColorCli" runat="server"
                                                                        Text='<%# Eval("ColorCliente").ToString().Trim().Length > 8 ? Eval("ColorCliente").ToString().Trim().Substring(0, 8)+"..." : Eval("ColorCliente").ToString().Trim() %>'
                                                                        ToolTip='<%# Eval("ColorCliente") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Modelo" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModeloAA" runat="server" Text='<%# Eval("ModeloAA")%>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Color" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblColorAA" runat="server" Text='<%# Eval("CodColor")%>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="T1" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTalla1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tallas[0]") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="T2" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTalla2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tallas[1]") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="T3" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTalla3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tallas[2]") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="T4" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTalla4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tallas[3]") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="T5" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTalla5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tallas[4]") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="T6" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTalla6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tallas[5]") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="T7" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTalla7" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tallas[6]") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="T8" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTalla8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tallas[7]") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="T9" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltalla9" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tallas[8]") %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C1" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCantidad1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[0]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Cantidades[0]").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C2" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCantidad2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[1]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Cantidades[1]").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C3" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCantidad3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[2]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Cantidades[2]").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C4" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCantidad4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[3]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Cantidades[3]").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C5" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCantidad5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[4]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Cantidades[4]").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C6" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCantidad6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[5]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Cantidades[5]").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C7" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCantidad7" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[6]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Cantidades[6]").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C8" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCantidad8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[7]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Cantidades[7]").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C9" ItemStyle-Width="2%" HeaderStyle-Width="2%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCantidad9" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[8]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Cantidades[8]").ToString() %>' Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <!-- FIN GRILLA DETALLE CONTRATO -->
                                                </fieldset>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- FIN MODAL AVANCE POR TALLAS -->
                    <!---- MODAL HOJA DE ESPECIFICACIONES ---->
                    <div class="modal fade" id="modalHojaEspecificaciones">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                            <h4 class="modal-title">Hoja de Especificaciones</h4>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row text-center">
                                                <span id="idRadios" class="radioboxlist" runat="server"></span>
                                            </div>
                                            <br />
                                            <div class="row" style="text-align: center;">
                                                <iframe id="frmPDF" style="width: 850px; height: 400px;"></iframe>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cerrar</button>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <!---- FIN MODAL HOJA --->
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript" src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>
    <link rel="Stylesheet" href="https://twitter.github.io/typeahead.js/css/examples.css" />
    <script type="text/javascript">
        $("#modalDetalleAvance, #modalAvanceTallas").draggable({
            handle: ".modal-header"
        });

        function moverScroll() {
            $('#modalBody').animate({
                scrollTop: $("#fieldSolicitado").offset().top
            }, 1000);
        }

        function CargarDocumento(control) {
            var iframe = document.getElementById('frmPDF');
            var base = control.value.substring(0, control.value.indexOf("-"));
            iframe.src = "http://192.168.0.1/diseno/hojas/" + base + "/" + control.value + "#zoom=72";
        }

        function AbrirModalDetalleSolicitado() {

            $('#modalDetalleContrato').modal('show');
        }

        $(function () {
            Autocomplete();
        });

        var prmInstance = Sys.WebForms.PageRequestManager.getInstance();
        prmInstance.add_endRequest(function () {
            var chkCliente = document.getElementById("ContentBody_ContentInitBody_rbnFiltros_0");
            if (chkCliente.checked) {
                Autocomplete();
            }
        });

        function Autocomplete() {
            $(function () {
                $('[id*=txtSearch]').typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1,
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/View/AvanceContrato.aspx/GetClientes") %>',
                        data: "{ 'prefijo': '" + request + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            items = [];
                            map = {};
                            $.each(data.d, function (i, item) {
                                var id = item.split('-')[1];
                                var name = item.split('-')[0];
                                map[name] = { id: id, name: name };
                                items.push(name);
                            });
                            response(items);
                            $(".dropdown-menu").css("height", "auto").css("font-size", "9pt");
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                updater: function (item) {
                    $('[id*=hidCustomerId]').val(map[item].id);
                    return item;
                }
            });
        });
    }

    </script>
</asp:Content>
