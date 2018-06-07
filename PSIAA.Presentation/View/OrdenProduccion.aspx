<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="OrdenProduccion.aspx.cs" Inherits="PSIAA.Presentation.View.OrdenProduccion" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formOrdenRequisicion">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true" AsyncPostBackTimeout="800"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-11">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Ordenes de Producción Lanzadas y Asignadas</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="well well-sm">
                                            <div class="row">
                                                <div class="col-md-1">
                                                    <label class="control-label">Contrato:</label>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon input-sm">N°</span>
                                                        <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm" autocomplete="off"></asp:TextBox>
                                                        <asp:HiddenField ID="hidContrato" runat="server" />
                                                        <asp:HiddenField ID="hidUsuario" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-2"><span class="control-label">Producción para: </span></div>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ddlCatOperacion" runat="server" class="form-control input-sm">
                                                        <asp:ListItem Value="400" Text="Orden de Tejido"></asp:ListItem>
                                                        <asp:ListItem Value="500" Text="Orden de Confección"></asp:ListItem>
                                                        <asp:ListItem Value="0" Text="Ninguno"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnBuscar_Click" />
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
                                                <div class="col-md-2"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 text-center">
                                            <b>
                                                <asp:CheckBox ID="chkAgregarCopiaOP" runat="server" Text="Agregar copia" Checked="false" Visible="false" /></b><br />
                                            <asp:Button ID="btnGenerarOrdenProduccion" runat="server" Text="Generar Ordenes Produccion" class="btn btn-info btn-sm" Visible="false" OnClick="btnGenerarOrdenProduccion_Click" /><br />
                                            <asp:HyperLink ID="linkOrdenProd" runat="server" Target="_blank"></asp:HyperLink>
                                        </div>
                                        <div class="col-md-4 text-center">
                                            <b>
                                                <asp:CheckBox ID="chkAgregarCopiaTP" runat="server" Text="Agregar copia" Checked="false" Visible="false" /></b><br />
                                            <asp:Button ID="btnGenerarTarjetaProduccion" runat="server" Text="Generar Tarjetas de Producción" class="btn btn-warning btn-sm" Visible="false" OnClick="btnGenerarTarjetaProduccion_Click" /><br />
                                            <asp:HyperLink ID="linkTarjetaProd" runat="server" Target="_blank"></asp:HyperLink>
                                        </div>
                                        <div class="col-md-4 text-center">
                                            <b>
                                                <asp:CheckBox ID="chkAgregarCopiaOR" runat="server" Text="Agregar copia" Checked="false" Visible="false" /></b><br />
                                            <asp:Button ID="btnGenerarOrdenRequisicion" runat="server" Text="Generar Ordenes Requisicion" class="btn btn-info btn-sm" Visible="false" OnClick="btnGenerarOrdenRequisicion_Click" /><br />
                                            <asp:HyperLink ID="linkOrdenReq" runat="server" Target="_blank"></asp:HyperLink>
                                        </div>
                                    </div><br />
                                    <div class="row">
                                        <div class="col-md-12 text-left">
                                            <asp:Label runat="server" ID="lblAvisoSeleccion" Visible="False">
                                            <div class="alert alert-dismissible alert-warning">
                                                <button type="button" class="close" data-dismiss="alert">&times;</button>
                                                <strong>Aviso! </strong>Debe seleccionar al menos un registro.
                                             </div>
                                            </asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridOrdenesLanzadasAsignadas" runat="server" Width="100%"
                                                    CssClass="table table-bordered"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No existe ninguna coincidencia">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSeleccionarTodo" runat="server" onclick="checkAll(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSeleccionar" runat="server" onclick="checkOne(this)" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Orden" HeaderText="Orden" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="modelo" HeaderText="Modelo" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="ModeloSap" HeaderText="Modelo SAP" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="taller" HeaderText="Taller Asignado" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="CantidadLanzada" HeaderText="Cant. Lanz." ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="CantidadAsignada" HeaderText="Cant. Asig." ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="Kilos" HeaderText="Kilos" DataFormatString="{0:F}" ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="usuario" HeaderText="Usuario" ItemStyle-Width="7%" />
                                                        <asp:BoundField DataField="fecha_ingreso" HeaderText="Fec. Ingreso" DataFormatString="{0:d}" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="FechaTermino" HeaderText="Fec. Term." DataFormatString="{0:d}" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="maquina" HeaderText="Maq." DataFormatString="{0:d}" ItemStyle-Width="5%" />
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
        <!---- MODAL IMPRESION DE ORDEN DE REQUISICION --->
        <div class="modal fade" id="modalDetalleOrdenProduccion">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Documento de Orden de Producción</h4>
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
                                <br />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table-responsive text-center">
                                            <div id="reporte" style="color: white;">
                                                <rsweb:ReportViewer ID="rptViewOrdenProduccion" runat="server" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="True" Visible="false">
                                                    <LocalReport ReportPath="../PSIAA.Reports/rptOrdenProduccion.rdlc" ShowDetailedSubreportMessages="True">
                                                    </LocalReport>
                                                </rsweb:ReportViewer>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table-responsive text-center">
                                            <div id="reporteTarjeta" style="color: white;">
                                                <rsweb:ReportViewer ID="rptViewTarjetaProduccion" runat="server" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="True" Visible="false">
                                                    <LocalReport ReportPath="../PSIAA.Reports/rptTarjetaProduccion.rdlc" ShowDetailedSubreportMessages="True">
                                                    </LocalReport>
                                                </rsweb:ReportViewer>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table-responsive text-center">
                                            <div id="reporteOrdenReq" style="color: white;">
                                                <rsweb:ReportViewer ID="rptViewOrdenReq" runat="server" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="True" Visible="false">
                                                    <LocalReport ReportPath="../PSIAA.Reports/rptOrdenRequisicion.rdlc" ShowDetailedSubreportMessages="True">
                                                    </LocalReport>
                                                </rsweb:ReportViewer>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!---- FIN MODAL ---->
    </form>
    <script type="text/javascript">
        function CargarDocumento(nombreDoc, servidor) {
            var iframe = document.getElementById('frmPDF');
            iframe.src = "http://" + servidor + "/PSIAA/Reports/Prod/" + nombreDoc + ".pdf";
        }

        //Funcion para Seleccionar una Fila
        function checkOne(objRef) {
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                row.style.backgroundColor = "#9BE99E";
            }
            else {
                row.style.backgroundColor = "white";
            }
        }

        //Funcion para Seleccionar/Deseleccionar todas las Filas
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                    if (objRef.checked) {
                        row.style.backgroundColor = "#9BE99E";
                        inputList[i].checked = true;
                    }
                    else {
                        row.style.backgroundColor = "white";
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>
</asp:Content>
