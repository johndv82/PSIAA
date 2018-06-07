<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="InventarioProductos.aspx.cs" Inherits="PSIAA.Presentation.View.InventarioProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formInventarioProductos">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-10">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h3 class="panel-title">Inventario de Productos en Proceso</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="well well-sm">
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <label class="control-label">Contrato:</label>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-addon input-sm">N°</span>
                                                            <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm"></asp:TextBox>
                                                            <asp:HiddenField ID="hidContrato" runat="server" />
                                                            <span class="input-group-btn">
                                                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnBuscar_Click"/>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
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
                                                <asp:GridView ID="gridInventarioProductos" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False" 
                                                    EmptyDataText="No existe ninguna coincidencia">
                                                    <Columns>
                                                        <asp:BoundField DataField="modelo" HeaderText="Modelo" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="InventarioTejido" HeaderText="Inv. Tejido" ControlStyle-Width="15%" />
                                                        <asp:BoundField DataField="InventarioCtrlTejido" HeaderText="Inv. Ctrl Tejido" ControlStyle-Width="15%" />
                                                        <asp:BoundField DataField="InventarioLavado" HeaderText="Inv. Lavado" ControlStyle-Width="15%" />
                                                        <asp:BoundField DataField="InventarioCtrlLavado" HeaderText="Inv. Ctrl Lavado" ControlStyle-Width="15%" />
                                                        <asp:BoundField DataField="InventarioConfeccion" HeaderText="Inv. Confeccion" ControlStyle-Width="15%" />
                                                        <asp:BoundField DataField="InventarioAcabadoFinal" HeaderText="Inv. Acabado Final" ControlStyle-Width="15%" />
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
    </form>
</asp:Content>
