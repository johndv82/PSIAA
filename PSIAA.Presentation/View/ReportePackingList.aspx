<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="ReportePackingList.aspx.cs" Inherits="PSIAA.Presentation.View.ReportePackingList" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">

    <form runat="server" id="formReportePackingList">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Packing List</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="well well-sm">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label class="control-label">Documento:</label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="input-group">
                                                            <span class="input-group-addon input-sm">N°</span>
                                                            <asp:TextBox ID="txtDocumento" runat="server" class="form-control input-sm" autocomplete="off" MaxLength="6"></asp:TextBox>
                                                            <asp:HiddenField ID="hidDocumento" runat="server" />
                                                            <asp:HiddenField ID="hidUsuario" runat="server" />
                                                            <span class="input-group-btn">
                                                                <asp:Button ID="btnAceptar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnAceptar_Click"/>
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
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <br />
                                                    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger" Text="Ocurrió un Error al Procesar Documento"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive text-center">
                                                <div id="reporte" style="color: white;">
                                                    <rsweb:ReportViewer id="rptViewPackingList" runat="server" font-names="Verdana" font-size="8pt" sizetoreportcontent="True" visible="false">
                                                        <LocalReport ReportPath="../PSIAA.Reports/rptPackingList.rdlc" ShowDetailedSubreportMessages="True">
                                                        </LocalReport>
                                                    </rsweb:ReportViewer>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" style="text-align: center;">
                                        <iframe id="frmPDF" style="width: 100%; height: 600px;"></iframe>
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
        function CargarDocumento(nombreDoc, servidor) {
            var iframe = document.getElementById('frmPDF');
            iframe.src = "http://" + servidor + "/PSIAA/Reports/PackingList/" + nombreDoc + ".pdf";
        }
    </script>
</asp:Content>
