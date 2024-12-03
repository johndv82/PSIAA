<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="BalanceMateriaPrima.aspx.cs" Inherits="PSIAA.Presentation.View.BalanceMateriaPrima" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formBalanceMateriaPrima">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Reporte de Balance de Materia Prima</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnGuardarExcel" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="well well-sm">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label class="control-label">Contrato:</label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="input-group">
                                                            <span class="input-group-addon input-sm">N°</span>
                                                            <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm" autocomplete="off" MaxLength="6"></asp:TextBox>
                                                            <asp:HiddenField ID="hidContrato" runat="server" />
                                                            <span class="input-group-btn">
                                                                <asp:Button ID="btnAceptar" runat="server" Text="Buscar" class="btn btn-default btn-sm" OnClick="btnAceptar_Click" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                                <ProgressTemplate>
                                                    <b>Cargando</b><img src="../Content/Images/load.gif" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <br />
                                                    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Button ID="btnGuardarExcel" runat="server" Text="Guardar en Excel" class="btn btn-success btn-sm" OnClientClick="this.disabled=true" UseSubmitBehavior="False" Visible="False" OnClick="btnGuardarExcel_Click" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridBalanceMP" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No Existe Ningun Registro"
                                                    ShowFooter="true"
                                                    OnRowDataBound="gridBalanceMP_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="Orden" HeaderText="Orden" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="modelo" HeaderText="Modelo" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="CodProducto" HeaderText="Producto" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Destino" HeaderText="Destino" ItemStyle-Width="16%" />
                                                        <asp:BoundField DataField="Entregado" HeaderText="Entregado" DataFormatString="{0:F3}" ItemStyle-Width="6%" />
                                                        <asp:BoundField DataField="Devuelto" HeaderText="Devuelto" DataFormatString="{0:F3}" ItemStyle-Width="6%" />
                                                        <asp:BoundField DataField="Almacen29" HeaderText="Almacen29" DataFormatString="{0:F3}" ItemStyle-Width="6%" />
                                                        <asp:BoundField DataField="Almacen11" HeaderText="Devoluciones Alm. 11" DataFormatString="{0:F3}" ItemStyle-Width="6%" />
                                                        <asp:BoundField DataField="Almacen10" HeaderText="Almacen10" DataFormatString="{0:F3}" ItemStyle-Width="6%" />
                                                        <asp:BoundField DataField="Utilizado" HeaderText="Utilizado" DataFormatString="{0:F3}" ItemStyle-Width="6%" />
                                                        <asp:BoundField DataField="MateriaPrima" HeaderText="M. Prima" DataFormatString="{0:F3}" ItemStyle-Width="6%" />
                                                        <asp:BoundField DataField="Saldo" HeaderText="Saldo" DataFormatString="{0:F3}" ItemStyle-Width="6%" />
                                                        <asp:BoundField DataField="taller" HeaderText="Taller" ItemStyle-Width="16%" />
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
