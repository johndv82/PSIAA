<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="Contratos.aspx.cs" Inherits="PSIAA.Presentation.View.Main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">

    <div class="panel panel-primary col-lg-12">
        <div class="panel-heading">
            <h3 class="panel-title">Listado de Contratos</h3>
        </div>
        <div class="panel-body">
            <form runat="server" id="formGrid">
                <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="well well-sm col-lg-4">
                                <div class="row">
                                    <div class="col-lg-4">
                                        <label for="cmbTipoContrato" class="control-label">Tipo de Contrato:</label>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:DropDownList ID="cmbTipoContrato" runat="server" class="form-control input-sm" DataTextField="Value" DataValueField="Key" AutoPostBack="True" OnSelectedIndexChanged="cmbTipoContrato_SelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="well well-sm col-lg-3">
                                <div class="row">
                                    <div class="col-lg-5">
                                        <label for="cmbAnioEm" class="control-label">Año de Emision:</label>
                                    </div>
                                    <div class="col-lg-7">
                                        <asp:DropDownList ID="cmbAnioEm" runat="server" class="form-control input-sm" AutoPostBack="True" OnSelectedIndexChanged="cmbAnioEm_SelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="well well-sm col-lg-4">
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="control-label">Contrato:</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <div class="input-group">
                                            <span class="input-group-addon input-sm">N°</span>
                                            <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-xs" OnClick="btnBuscar_Click" />
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gridContrato" runat="server" Width="100%"
                                        CssClass="table table-striped table-bordered table-hover"
                                        AutoGenerateColumns="False" AllowPaging="true"
                                        OnPageIndexChanging="gridContrato_PageIndexChanging"
                                        EmptyDataText="No existe ninguna coincidencia">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tipo" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTipo" runat="server" Text='<%# Bind("Tipo") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Numero" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNumero" runat="server" Text='<%# Bind("Numero") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NumeroPO" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNumeroPO" runat="server" Text='<%# Bind("NumeroPO") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CodCliente" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCodCliente" runat="server" Text='<%# Bind("CodCliente") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cliente" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCliente" runat="server" Text='<%# Bind("Cliente") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CodClienteConsig" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCodClienteConsig" runat="server" Text='<%# Bind("CodClienteConsig") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ClienteConsig" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClienteConsig" runat="server" Text='<%# Bind("ClienteConsig") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FechaEmision" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFechaEmision" runat="server" Text='<%# Bind("FechaEmision") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FechaSolicitada" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFechaSolicitada" runat="server" Text='<%# Bind("FechaSolicitada") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FechaDespacho" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFechaDespacho" runat="server" Text='<%# Bind("FechaDespacho") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CodTerminoPago" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCodigoTerminoPago" runat="server" Text='<%# Bind("CodTerminoPago") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TerminosPago" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTerminosPago" runat="server" Text='<%# Bind("TerminosPago") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CodTipoTransporte" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCodTipoTransporte" runat="server" Text='<%# Bind("CodTipoTransporte") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TipoTransporte" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTipoTransporte" runat="server" Text='<%# Bind("TipoTransporte") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </form>
        </div>
    </div>
</asp:Content>
