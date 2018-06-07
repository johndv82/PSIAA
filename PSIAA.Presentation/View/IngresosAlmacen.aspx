<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="IngresosAlmacen.aspx.cs" Inherits="PSIAA.Presentation.View.IngresosAlmacen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formIngresosAlmacen">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Ingresos Detallados a Almacen</h3>
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
                                                    <label class="control-label" for="txtModelo">Modelo:</label>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:TextBox runat="server" ID="txtModelo" class="form-control input-sm" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnBuscar_Click" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnGuardarExcel" runat="server" Text="Guardar en Excel" class="btn btn-success btn-sm" OnClick="btnGuardarExcel_Click" OnClientClick="this.disabled=true" UseSubmitBehavior="False" Visible="False" />
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
                                                <asp:GridView ID="gridIngresosAlmacen" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False" AllowPaging="true" PageSize="15"
                                                    EmptyDataText="No existe ninguna coincidencia" 
                                                    OnPageIndexChanging="gridIngresosAlmacen_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="AlmacenSIAA" HeaderText="Alm. SIAA" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="AlmacenSAP" HeaderText="Alm. SAP" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="NombreAlmacen" HeaderText="Almacen" ItemStyle-Width="15%" />
                                                        <asp:BoundField DataField="Parte" HeaderText="Nro. Parte" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Orden" HeaderText="Orden" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="Lote" HeaderText="Lote" ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="Modelo" HeaderText="Modelo" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="Contrato" HeaderText="Contrato" ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="TipoContrato" HeaderText="Tipo Contrato" ItemStyle-Width="12%" />
                                                        <asp:BoundField DataField="peso_neto" HeaderText="Cant." ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="Mes" HeaderText="Mes" ItemStyle-Width="8%" />
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
