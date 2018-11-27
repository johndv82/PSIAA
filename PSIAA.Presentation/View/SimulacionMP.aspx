<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="SimulacionMP.aspx.cs" Inherits="PSIAA.Presentation.View.SimulacionMP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="fromSimulacionMP">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true" AsyncPostBackTimeout="600"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportar" />
            </Triggers>
            <ContentTemplate>
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-10">
                            <div class="panel panel-default">
                                <div class="panel-heading">Simulación de Materia Prima</div>
                                <div class="panel-body">
                                    <div class="well well-sm">
                                        <div class="row">
                                            <div class="col-md-1">
                                                <span class="control-label">Contrato: </span>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon input-sm">N°</span>
                                                    <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm" autocomplete="off"></asp:TextBox>
                                                    <asp:HiddenField ID="hidContrato" runat="server" />
                                                    <span class="input-group-btn">
                                                        <asp:Button ID="btnBuscar" runat="server" class="btn btn-info btn-sm" Text="Buscar" OnClick="btnBuscar_Click" />
                                                    </span>
                                                </div>
                                                <br />
                                                <span class="control-label">Adicional: </span>
                                                <asp:TextBox ID="txtAdicional" runat="server" class="form-control input-sm" onfocus="ValueDefaultFocus(this);" onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:ListBox ID="lstModelos" runat="server" SelectionMode="Multiple" class="multiselect-ui form-control"></asp:ListBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:RadioButtonList ID="rbnTipoProceso" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow" AutoPostBack="true">
                                                    <asp:ListItem Text="Consulta" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Cálculo"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <br />
                                                <asp:Label ID="lblRptaMaterial" runat="server" Text="" Visible="false"></asp:Label><br />
                                                <asp:Label ID="lblRptaMedidaPeso" runat="server" Text="" Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Button ID="btnProcesar" runat="server" Text="Procesar" class="btn btn-primary btn-sm" Visible="false" OnClick="btnProcesar_Click" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <asp:Label runat="server" ID="lblErrorCalculo" Visible="False">
                                        <div class="alert alert-dismissible alert-danger">
                                            <button type="button" class="close" data-dismiss="alert">&times;</button>
                                            <strong>Error! </strong>No se pudo completar el cálculo.
                                         </div>
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--<div class="row">
                                <div class="progress">
                                    <div id="bar" class="progress-bar progress-bar-striped progress-bar-success" role="progressbar" style="width: 0%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">
                                    </div>
                                </div>
                        </div>-->
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-primary">
                                <div class="panel-heading">Resultado</div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridSimulacionDet" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No existe ningun registro">
                                                    <Columns>
                                                        <asp:BoundField DataField="NroSimulacion" HeaderText="N° Sim." ItemStyle-Width="6%" />
                                                        <asp:BoundField DataField="NombreMaquina" HeaderText="Linea P." ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="Color" HeaderText="Color" ItemStyle-Width="4%" />
                                                        <asp:BoundField DataField="CodProducto" HeaderText="Cod. Material" ItemStyle-Width="18%" />
                                                        <asp:BoundField DataField="DescProducto" HeaderText="Material" ItemStyle-Width="22%" />
                                                        <asp:BoundField DataField="Modelo" HeaderText="Modelo" ItemStyle-Width="8%" />
                                                        <asp:BoundField DataField="KilosSimulado" HeaderText="Kilos" DataFormatString="{0:F3}" ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="PorSeguridad" HeaderText="% Adic." DataFormatString="{0:F}" ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="TotalKilos" HeaderText="Total K." DataFormatString="{0:F3}" ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-Width="5%" />
                                                        <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha Ing." DataFormatString="{0:d}" ItemStyle-Width="7%" />
                                                        <asp:BoundField DataField="HoraIngreso" HeaderText="Hora Ing." ItemStyle-Width="7%" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
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
                                    <div class="row">
                                        <asp:Label runat="server" ID="lblMensajeOk" Visible="false">
                                        <div class="alert alert-dismissible alert-success">
                                            <button type="button" class="close" data-dismiss="alert">&times;</button>
                                            <strong>Éxito! </strong>Se guardaron los cambios correctamente.
                                        </div>
                                        </asp:Label>
                                    </div>
                                    <div class="row">
                                        <asp:Label runat="server" ID="lblErrorGuardarCalculo" Visible="false">
                                        <div class="alert alert-dismissible alert-danger">
                                            <button type="button" class="close" data-dismiss="alert">&times;</button>
                                            <strong>Error! </strong>No se pudo guardar el calculo.
                                        </div>
                                        </asp:Label>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 text-left">
                                            <asp:Button ID="btnExportar" runat="server" Text="Exportar a Excel" class="btn btn-info btn-sm" OnClientClick="this.disabled=true" UseSubmitBehavior="False" Visible="False" OnClick="btnExportar_Click" />
                                        </div>
                                        <div class="col-md-8 text-right">
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Calculo" class="btn btn-success btn-sm" Visible="False" OnClick="btnGuardar_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript">
        function CambiarProgreso(valor) {
            $('#bar').css('width', valor + '%');
        }

        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return ((tecla > 47 && tecla < 58) || tecla == 46);
        }

        function ValueDefaultZero(obj) {
            obj.value = obj.value.trim() == "" ? "0.00" : obj.value;
        }

        function ValueDefaultFocus(obj) {
            obj.value = obj.value.trim() != "0.00" ? obj.value : "";
        }
    </script>
</asp:Content>
