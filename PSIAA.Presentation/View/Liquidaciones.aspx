<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="Liquidaciones.aspx.cs" Inherits="PSIAA.Presentation.View.Liquidaciones" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formLiquidaciones">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h3 class="panel-title">Listado de Liquidaciones</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="well well-sm">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox runat="server" ID="txtCodProveedor" class="form-control input-sm" placeholder="Código de Proveedor" />
                                                        <asp:HiddenField ID="hidCodProveedor" runat="server" />
                                                        <span class="input-group-btn">
                                                            <asp:Button ID="btnBuscarPorCod" runat="server" class="btn btn-info btn-sm" Text="Buscar" OnClick="btnBuscarPorCod_Click" />
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="input-group">
                                                        <asp:TextBox runat="server" ID="txtNombreProveedor" class="form-control input-sm" placeholder="Nombre del Proveedor" ReadOnly="true" />
                                                        <span class="input-group-btn">
                                                            <asp:Button ID="btnBuscarPorNombre" runat="server" class="btn btn-info btn-sm" Text="Buscar" data-target="#modalSelectProveedor" data-toggle="modal" OnClick="btnBuscarPorNombre_Click" />
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <span class="control-label">Periodo:</span>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ddlPeriodos" runat="server" class="form-control input-sm">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnBuscarLiquid" runat="server" class="btn btn-primary btn-sm" Text="Buscar Liquidaciones" Visible="false" OnClick="btnBuscarLiquid_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridLiquidaciones" runat="server" Width="100%"
                                                    CssClass="table table-striped table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No existe ninguna coincidencia"
                                                    OnSelectedIndexChanged="gridLiquidaciones_SelectedIndexChanged">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" HeaderText="Acción">
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnDetalle" Text="Ver PDF" class="btn btn-info btn-xs" CommandName="Select" data-target="#modalDocumentoPagoTaller" data-toggle="modal" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PeriodoMes" HeaderText="Mes" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="TipoMov" HeaderText="Movimiento" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="serie_documento" HeaderText="Serie" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="nro_documento" HeaderText="N° Liquid." ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="concepto" HeaderText="Concepto" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="fecha_documento" HeaderText="Fecha" DataFormatString="{0:d}" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="moneda" HeaderText="Moneda" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="sub_total" HeaderText="Sub Total" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="igv" HeaderText="Igv" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="usuario" HeaderText="Usuario" ControlStyle-Width="5%" />
                                                        <asp:BoundField DataField="total" HeaderText="Total" DataFormatString="{0:F}" ControlStyle-Width="10%" />
                                                        <asp:BoundField DataField="Semana" HeaderText="Sem." ControlStyle-Width="3%" />
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
        <!---- MODAL SELECCION PROVEEDOR - TALLER ---->
        <div class="modal fade" id="modalSelectProveedor">
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Listado de Proveedores</h4>
                            </div>
                            <div class="modal-body">
                                <div class="well well-sm col-sm-12">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <span class="control-label">Nombre Com.: </span>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txtNombreComercial" runat="server" class="form-control input-sm" OnTextChanged="txtNombreComercial_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                                <ProgressTemplate>
                                                    <b>Cargando</b><img src="../Content/Images/load.gif" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </div>
                                </div>
                                <div class="row table-responsive">
                                    <asp:GridView ID="gridProveedores" runat="server" Width="100%"
                                        CssClass="table table-striped table-bordered table-hover"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay registros." AllowPaging="true"
                                        OnPageIndexChanging="gridProveedores_PageIndexChanging"
                                        OnSelectedIndexChanged="gridProveedores_SelectedIndexChanged">
                                        <Columns>
                                            <asp:BoundField DataField="cod_proveedor" HeaderText="Código" ControlStyle-Width="25%" />
                                            <asp:TemplateField HeaderText="Nombre Comercial" ItemStyle-Width="55%" ItemStyle-ForeColor="DarkSlateBlue" ControlStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnNombreComercial" runat="server" Text='<%# Bind("nombre_comercial") %>' Font-Size="Smaller"
                                                        CommandName="Select" CssClass="buttonLabel"></asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ciudad" HeaderText="Ciudad" ControlStyle-Width="20%" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!---- FIN MODAL --->
        <!---- MODAL IMPRESION DE DOCUMENTO TALLER --->
        <div class="modal fade" id="modalDocumentoPagoTaller">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Impresión de Documento de Taller</h4>
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
                                        <rsweb:ReportViewer ID="rptViewFactura" runat="server" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="True" Visible="false">
                                            <LocalReport ReportPath="../PSIAA.Reports/rptFactura.rdlc" ShowDetailedSubreportMessages="True">
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
                </div>
            </div>
        </div>
        <!---- FIN MODAL ---->
    </form>
    <script type="text/javascript">
        function CloseModalPorveedores() {
            $('#modalSelectProveedor').modal('hide');
        }

        function CargarDocumento(nombreDoc, servidor) {
            var iframe = document.getElementById('frmPDF');
            iframe.src = "http://" + servidor + "/PSIAA/Reports/Docs/" + nombreDoc + ".pdf";
        }
    </script>
</asp:Content>
