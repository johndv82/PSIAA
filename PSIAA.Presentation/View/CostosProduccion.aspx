<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="CostosProduccion.aspx.cs" Inherits="PSIAA.Presentation.View.CostosProduccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formCostosProd">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <h3 class="panel-title">Costos de Producción por Contrato</h3>
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
                                        <div class="col-md-3">
                                            <div class="input-group">
                                                <span class="input-group-addon input-sm">N° de Contrato</span>
                                                <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm"></asp:TextBox>
                                                <asp:HiddenField ID="hidContrato" runat="server" />
                                                <span class="input-group-btn">
                                                    <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" class="btn btn-default btn-sm" OnClick="btnSeleccionar_Click" />
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <label for="lblCliente">Cliente: </label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblCliente" runat="server" Font-Bold="true" Text="--"></asp:Label>
                                        </div>
                                        <div class="col-md-1">
                                            <label for="lblCerrado">¿Cerrado?: </label>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Label ID="lblCerrado" runat="server" Font-Bold="true" Text="--"></asp:Label>
                                        </div>
                                        <div class="col-md-1">
                                            <label for="cmbModelos">Modelos: </label>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:DropDownList ID="ddlModelo" runat="server" class="form-control input-sm" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <!--FECHAS-->
                                        <div class="col-md-2"></div>
                                        <div class="col-md-2 text-right">
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
                                            <asp:Button ID="btnExportar" runat="server" Text="Exportar a Excel" class="btn btn-success btn-sm" OnClientClick="this.disabled=true" UseSubmitBehavior="False" Visible="false" OnClick="btnExportar_Click" />
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
                            <!--<div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gridCostosProd" runat="server" Width="100%"
                                            CssClass="table table-bordered"
                                            AutoGenerateColumns="False"
                                            EmptyDataText="No existe ninguna coincidencia"
                                            OnRowCreated="gridCostosProd_RowCreated">
                                            <Columns>
                                                <asp:BoundField DataField="Modelo" HeaderText="Modelo" ControlStyle-Width="10%" />
                                                <asp:BoundField DataField="CantSolicitadas" HeaderText="Cant. Solic." ControlStyle-Width="5%" />
                                                <asp:BoundField DataField="CantidadAPT" HeaderText="Terminados" ControlStyle-Width="5%" />
                                                <asp:BoundField DataField="CantidadAPNC" HeaderText="No Conformes" ControlStyle-Width="5%" />
                                                <asp:BoundField DataField="MPKilos" HeaderText="Kilos" ControlStyle-Width="5%" DataFormatString="{0:F2}" />
                                                <asp:BoundField DataField="MPPrecioKilos" HeaderText="Precio Kilos" ControlStyle-Width="5%" DataFormatString="{0:F2}" />
                                                <asp:BoundField DataField="MPMetros" HeaderText="Metros" ControlStyle-Width="5%" DataFormatString="{0:F2}" />
                                                <asp:BoundField DataField="MPPrecioMetros" HeaderText="Precio Metros" ControlStyle-Width="5%" DataFormatString="{0:F2}" />
                                                <asp:BoundField DataField="PrecioInsumos" HeaderText="Precio" ControlStyle-Width="5%" DataFormatString="{0:F2}" />
                                                <asp:BoundField DataField="CostoMOExt" HeaderText="Externa" ControlStyle-Width="10%" />
                                                <asp:BoundField DataField="CostoMOInt" HeaderText="Interna" ControlStyle-Width="5%" />
                                                <asp:BoundField DataField="CostoTotal" HeaderText="Total" ControlStyle-Width="10%" DataFormatString="{0:F2}" />
                                                <asp:BoundField DataField="CostoUnitario" HeaderText="Unitario" ControlStyle-Width="5%" DataFormatString="{0:F2}" />
                                                <asp:BoundField DataField="MontoFact" HeaderText="Facturado" ControlStyle-Width="10%" DataFormatString="{0:F2}" />
                                                <asp:BoundField DataField="Margen" HeaderText="Margen" ControlStyle-Width="10%" DataFormatString="{0:F2}" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>-->
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label runat="server" ID="lblMensajeError" Visible="False">
                                        <div class="alert alert-dismissible alert-danger">
                                            <button type="button" class="close" data-dismiss="alert">&times;</button>
                                            <strong>Error! </strong>La solicitud no devolvio registros.
                                        </div>
                                    </asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label runat="server" ID="lblMensajeOk" Visible="false">
                                        <div class="alert alert-dismissible alert-success">
                                            <button type="button" class="close" data-dismiss="alert">&times;</button>
                                            <strong>Éxito! </strong>Debe Exportar a Excel para visualizar los resultados.
                                        </div>
                                    </asp:Label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                    format: 'DD-MM-YYYY',
                    minDate: '01-01-2015',
                    maxDate: new Date()
                }).data("autoclose", true);
            }
        });
    </script>
</asp:Content>
