<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="IngresosFaltantesAlmacen.aspx.cs" Inherits="PSIAA.Presentation.View.IngresosFaltantesAlmacen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formIngresosFaltantes">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-11">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Ingresos Faltantes a Almacen - 800</h3>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportar" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="row">
                                <div class="well well-sm">
                                    <div class="row">
                                        <div class="col-md-1">
                                            <label class="control-label">Contrato:</label>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon input-sm">N°</span>
                                                <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm"></asp:TextBox>
                                                <span class="input-group-btn">
                                                    <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" class="btn btn-default btn-sm" OnClick="btnSeleccionar_Click"/>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <span class="control-label">Modelos: </span>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="cmbModelos" runat="server" class="form-control input-sm" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnBuscar_Click"/>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Button ID="btnExportar" runat="server" Text="Exportar a Excel" class="btn btn-success btn-sm" OnClientClick="this.disabled=true" UseSubmitBehavior="False" Visible="False" OnClick="btnExportar_Click"/>
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
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gridIngresosFaltantes" runat="server" Width="100%"
                                            CssClass="table table-striped table-bordered table-hover"
                                            AutoGenerateColumns="False" AllowPaging="true" PageSize="15"
                                            EmptyDataText="No existe ninguna coincidencia"
                                            OnPageIndexChanging="gridIngresosFaltantes_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Contrato" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblContrato" runat="server" Text='<%# Eval("Contrato") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Orden" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrden" runat="server" Text='<%# Eval("Orden") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lote" ItemStyle-Width="5%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLote" runat="server" Text='<%# Eval("Lote") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Talla" ItemStyle-Width="5%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTalla" runat="server" Text='<%# Eval("Talla") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Modelo" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModelo" runat="server" Text='<%# Eval("modelo") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Color" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblColor" runat="server" Text='<%# Eval("Color") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cant. Lanzada" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLanzado" runat="server" Text='<%# Eval("Lanzado") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acab. Confec." ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAcabConfeccion" runat="server" Text='<%# Eval("AcabConfeccion") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ctrl. Final" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCtrlFinal" runat="server" Text='<%# Eval("CtrlFinal") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Almacen" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAlmacen" runat="server" Text='<%# Eval("Almacen") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cant. Faltante" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPorIngresar" runat="server" Text='<%# Eval("PorIngresar") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-11 text-right">
                                    <asp:Label ID="lblNRegistros" runat="server" class="control-label" Font-Bold="true">-</asp:Label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
