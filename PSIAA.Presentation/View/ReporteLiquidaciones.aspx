<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="ReporteLiquidaciones.aspx.cs" Inherits="PSIAA.Presentation.View.ReporteLiquidaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formIngresosAlmacen">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h3 class="panel-title">Reporte de Liquidaciones</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnGuardarExcel" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="well well-sm">
                                            <div class="row">
                                                <div class="col-md-1">
                                                    <span class="control-label">Periodo:</span>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:HiddenField ID="hidUsuario" runat="server" />
                                                    <asp:DropDownList ID="ddlPeriodos" runat="server" class="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodos_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1">
                                                    <span class="control-label">Semana:</span>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:DropDownList ID="ddlSemanas" runat="server" class="form-control input-sm" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnBuscar_Click" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnGuardarExcel" runat="server" Text="Guardar en Excel" class="btn btn-success btn-sm" OnClientClick="this.disabled=true" UseSubmitBehavior="False" Visible="False" OnClick="btnGuardarExcel_Click"/>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridLiquidaciones" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False" AllowPaging="true" PageSize="15"
                                                    EmptyDataText="No Existe Ningun Registro"
                                                    OnPageIndexChanging="gridLiquidaciones_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="Taller" HeaderText="Taller" ItemStyle-Width="25%" />
                                                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" ItemStyle-Width="7%" />
                                                        <asp:BoundField DataField="Ruc" HeaderText="Ruc" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Movimiento" HeaderText="Tipo Mov." ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Nro_Liquidacion" HeaderText="Nro. Liquid." ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Concepto" HeaderText="Concepto" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="Moneda" HeaderText="Moneda" ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="Subtotal" HeaderText="SubTotal" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Igv" HeaderText="IGV" ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-Width="10%" Visible="false"/>
                                                        <asp:BoundField DataField="Glosa" HeaderText="Glosa" ItemStyle-Width="10%" Visible="false"/>
                                                        <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Semana" HeaderText="Semana" ItemStyle-Width="10%" Visible="false"/>
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
