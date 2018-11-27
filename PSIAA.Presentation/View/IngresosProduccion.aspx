<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="IngresosProduccion.aspx.cs" Inherits="PSIAA.Presentation.View.IngresosProduccion" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formIngProd">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-10">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Partes por Almacen</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="well well-sm">
                                            <div class="row">
                                                <div class="col-md-1">
                                                    <label class="control-label">Almacen:</label>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Label runat="server" class="label label-info" ID="lblNumAlmacen" Text="0" Font-Size="Small" />
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="cmbAlmacenes" runat="server" class="form-control input-sm" DataTextField="Value" DataValueField="Key" AutoPostBack="True" OnSelectedIndexChanged="cmbAlmacenes_SelectedIndexChanged" />
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <div class="input-group date" id="FechaIngreso">
                                                            <asp:TextBox runat="server" ID="txtFechaIngreso" type="text" class="form-control input-sm" />
                                                            <span class="input-group-addon input-sm">
                                                                <span class="glyphicon glyphicon-calendar"></span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button runat="server" class="btn btn-primary btn-sm" ID="btnProcesar" Text="Procesar" OnClick="btnProcesar_Click" />
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
                                                <asp:GridView ID="gridIngresos" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No hay ningun Ingreso a Producción."
                                                    AllowPaging="True"
                                                    PageSize="50"
                                                    OnPageIndexChanging="gridIngresos_PageIndexChanging"
                                                    OnSelectedIndexChanged="gridIngresos_SelectedIndexChanged">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="Acción">
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnImprimir" Text="Imprimir" class="btn btn-info btn-xs" CommandName="Select" data-target="#modalParte" data-toggle="modal" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Almacen" HeaderText="Almacen" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="AlmacenSAP" HeaderText="Alm. SAP" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo Mov." ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Parte" HeaderText="Nro. Parte" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="NroItems" HeaderText="Nro. Ingresos" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha Ingreso" DataFormatString="{0:d}" ItemStyle-Width="10%" />
                                                        <asp:TemplateField HeaderText="Hora de Ingreso" ItemStyle-Width="10%" ItemStyle-ForeColor="#696C6E" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHora" runat="server" Text='<%# Eval("HoraIngreso").ToString().Substring(0,2) + ":" +  Eval("HoraIngreso").ToString().Substring(2,2) + ":" + Eval("HoraIngreso").ToString().Substring(Eval("HoraIngreso").ToString().Length -2,2) %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" ControlStyle-Width="10%" />
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
        <!---- MODAL PARTE DE INGRESO ---->
        <div class="modal fade" id="modalParte">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Parte de Ingreso</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12 text-center">
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                            <ProgressTemplate>
                                                <b>Cargando</b><img src="../Content/Images/load.gif" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table-responsive text-center">
                                            <div id="reporte" style="color: white;">
                                                <rsweb:ReportViewer ID="rptViewPagoLibre" runat="server" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="True" Visible="false">
                                                    <LocalReport ReportPath="../PSIAA.Reports/rptParteIngreso.rdlc" ShowDetailedSubreportMessages="True">
                                                    </LocalReport>
                                                </rsweb:ReportViewer>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <iframe id="frmPDF" style="width: 850px; height: 400px;"></iframe>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cerrar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!---- FIN MODAL HOJA --->
    </form>
    <script type="text/javascript">
        $(function () {
            CallDateTimePicker();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                CallDateTimePicker();
            }

            function CallDateTimePicker() {
                $('#FechaIngreso').datetimepicker({
                    format: 'YYYY-MM-DD',
                    minDate: '2015-01-01',
                    maxDate: new Date(),
                    defaultDate: 'now'
                }).data("autoclose", true);
            }
        });

        function CargarDocumento(nombreDoc, servidor) {
            var iframe = document.getElementById('frmPDF');
            iframe.src = "http://" + servidor + "/PSIAA/Reports/Partes/" + nombreDoc + ".pdf";
        }
    </script>
</asp:Content>
