<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="Facturaciones.aspx.cs" Inherits="PSIAA.Presentation.View.Facturaciones" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formFacturaciones">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h3 class="panel-title">Listado de Facturaciones</h3>
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
                                                    <label class="control-label" for="txtFechaIni">Rango de Facturación:</label>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:TextBox runat="server" ID="txtFechaFacturacion" type="text" class="form-control input-sm" onkeypress="return ValidNum(event);" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button runat="server" class="btn btn-primary btn-sm" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnGuardarExcel" runat="server" Text="Guardar en Excel" class="btn btn-success btn-sm" OnClientClick="this.disabled=true" UseSubmitBehavior="False" Visible="False" OnClick="btnGuardarExcel_Click" />
                                                </div>
                                                <div class="col-md-3"></div>
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
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridFacturaciones" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No existe ninguna coincidencia"
                                                    OnSelectedIndexChanged="gridFacturaciones_SelectedIndexChanged">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" HeaderText="Acción">
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnDetalle" Text="Detalle" class="btn btn-success btn-xs" CommandName="Select" data-target="#modalDocumentoFactura" data-toggle="modal" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="proveedor" HeaderText="Taller" ControlStyle-Width="20%" />
                                                        <asp:BoundField DataField="cod_proveedor" HeaderText="RUC" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="tipo_movimiento" HeaderText="Mov." ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="serie_documento" HeaderText="Serie" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="nro_documento" HeaderText="Nro. Doc." ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="nro_liquidacion" HeaderText="Nro. Liquid." ControlStyle-Width="8%" />
                                                        <asp:BoundField DataField="fecha_documento" HeaderText="Fecha" DataFormatString="{0:d}" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="moneda" HeaderText="Moneda" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="sub_total" HeaderText="Sub Total" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="igv" HeaderText="Igv" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="usuario" HeaderText="Usuario" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="total" HeaderText="Total" DataFormatString="{0:F}" ControlStyle-Width="10%" />
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
        <!---- MODAL DOCUMENTO DE FACTURA --->
        <div class="modal fade" id="modalDocumentoFactura">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Documento de Factura Emitido</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="row table-responsive">
                                    <div id="reporte" style="color: white;">
                                        <rsweb:ReportViewer ID="rptViewFactura" runat="server" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="True">
                                            <LocalReport ReportPath="../PSIAA.Reports/rptFactura.rdlc" ShowDetailedSubreportMessages="True">
                                            </LocalReport>
                                        </rsweb:ReportViewer>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <!----FIN MODAL DOCUMENTO --->
    </form>
    <script type="text/javascript">
        $(function () {
            CallDateTimePicker();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                CallDateTimePicker();
            }

            function CallDateTimePicker() {
                $('#ContentBody_ContentInitBody_txtFechaFacturacion').daterangepicker({
                    minDate: '2016-01-01',
                    maxDate: new Date(),
                    locale: {
                        format: 'YYYY/MM/DD'
                    }
                });
            }
        });

        //Funcion para Validar escritura de numeros
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return ((tecla > 47 && tecla < 58) || tecla == 46);
        }
    </script>
</asp:Content>
