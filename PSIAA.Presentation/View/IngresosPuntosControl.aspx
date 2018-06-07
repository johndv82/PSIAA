<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="IngresosPuntosControl.aspx.cs" Inherits="PSIAA.Presentation.View.IngresosPuntosControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formIngresosPuntoControl">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h3 class="panel-title">Ingresos a Puntos de Control</h3>
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
                                                <div class="col-md-2 text-center">
                                                    <label class="control-label" for="txtFechaIni">Fecha Inicio:</label>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <div class="input-group date" id="FechaInicial">
                                                            <asp:TextBox runat="server" ID="txtFechaIni" type="text" class="form-control input-sm" />
                                                            <span class="input-group-addon input-sm">
                                                                <span class="glyphicon glyphicon-calendar"></span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <label class="control-label" for="txtFechaFin">Fecha Fin:</label>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <div class="input-group date" id="FechaFinal">
                                                            <asp:TextBox runat="server" ID="txtFechaFin" type="text" class="form-control input-sm" />
                                                            <span class="input-group-addon input-sm">
                                                                <span class="glyphicon glyphicon-calendar"></span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnBuscar_Click" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnGuardarExcel" runat="server" Text="Guardar en Excel" class="btn btn-success btn-sm" OnClientClick="this.disabled=true" UseSubmitBehavior="False" Visible="False" OnClick="btnGuardarExcel_Click" />
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
                                                <asp:GridView ID="gridIngresosPuntoControl" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False" AllowPaging="true" PageSize="15"
                                                    EmptyDataText="No existe ninguna coincidencia"
                                                    OnPageIndexChanging="gridIngresosPuntoControl_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="contrato" HeaderText="Contrato" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="modelo" HeaderText="Modelo" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="Orden" HeaderText="Orden" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="Lote" HeaderText="Lote" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Talla" HeaderText="Talla" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Color" HeaderText="Color" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="CantidadAsignada" HeaderText="Cant. Asig." ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Punto" HeaderText="Punto" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="Proceso" HeaderText="Operación" ControlStyle-Width="15%" />
                                                        <asp:BoundField DataField="Cod_Proveedor" HeaderText="Cod. Talller" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="Proveedor" HeaderText="Taller" ControlStyle-Width="15%" />
                                                        <asp:BoundField DataField="CantIng" HeaderText="Cant. Ing." ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="FechaIng" HeaderText="Fecha Ing." DataFormatString="{0:d}" ControlStyle-Width="5%" />
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
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $(function () {
            CallDateTimePicker();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                CallDateTimePicker();
            }

            function CallDateTimePicker() {
                $('#FechaInicial, #FechaFinal').datetimepicker({
                    format: 'YYYY-MM-DD',
                    minDate: '2015-01-01',
                    maxDate: new Date()
                }).data("autoclose", true);
            }
        });
    </script>
</asp:Content>
