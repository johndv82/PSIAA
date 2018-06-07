<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="AlmacenSap.aspx.cs" Inherits="PSIAA.Presentation.View.AlmacenSap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formIngresosAlmacen">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-11">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Almacen de Artículos SAP</h3>
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
                                        <label class="control-label col-md-1" for="txtArticulo">Descripcion: </label>
                                        <div class="col-md-3">
                                            <asp:TextBox runat="server" ID="txtArticulo" type="text" class="form-control input-sm" />
                                        </div>
                                        <label class="control-label col-md-1" for="txtArticulo">Código: </label>
                                        <div class="col-md-2">
                                            <asp:TextBox runat="server" ID="txtCodigo" type="text" class="form-control input-sm" placeholder="Código SAP" />
                                        </div>
                                        <div class="checkbox col-md-2">
                                            <label>
                                                <asp:CheckBox ID="chkStockCero" runat="server" Text="¿Incluir Stock Cero?" />
                                            </label>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnBuscar_Click" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Button ID="btnExportar" runat="server" Text="Exportar a Excel" class="btn btn-success btn-sm" OnClientClick="this.disabled=true" UseSubmitBehavior="False" Visible="False" OnClick="btnExportar_Click" />
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
                                        <asp:GridView ID="gridAlmacenSap" runat="server" Width="100%"
                                            CssClass="table table-striped table-bordered table-hover"
                                            AutoGenerateColumns="False" AllowPaging="true" PageSize="15"
                                            EmptyDataText="No existe ninguna coincidencia" 
                                            OnPageIndexChanging="gridAlmacenSap_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Código SAP" ItemStyle-Width="15%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCodigoSap" runat="server" Text='<%# Eval("CodigoSap") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Descripción del Artículo" ItemStyle-Width="55%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescripcionArticulo" runat="server" Text='<%# Eval("DescripcionArticulo") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Costo Prom." ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCostoPromedio" runat="server" Text='<%# Eval("CostoPromedio") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stock" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Font-Size="Smaller"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="U. Medida" ItemStyle-Width="10%" HeaderStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnidMed" runat="server" Text='<%# Eval("UnidMed") %>' Font-Size="Smaller"></asp:Label>
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
