<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="ReporteContrato.aspx.cs" Inherits="PSIAA.Presentation.View.ReporteContrato" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formGrid">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-success">
                        <div class="panel-heading">
                            <h3 class="panel-title">Reporte General de Contrato</h3>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="well well-sm">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label class="control-label">Contrato:</label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="input-group">
                                                            <span class="input-group-addon input-sm">N°</span>
                                                            <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm" autocomplete="off" MaxLength="6"></asp:TextBox>
                                                            <asp:HiddenField ID="hidContrato" runat="server" />
                                                            <span class="input-group-btn">
                                                                <asp:Button ID="btnAceptar" runat="server" Text="Buscar" class="btn btn-default btn-sm" OnClick="btnAceptar_Click" />
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
                                                    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive text-center">
                                                <div id="reporte" style="color: white;">
                                                    <rsweb:ReportViewer ID="rptViewContrato" runat="server" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="True" Visible="false">
                                                        <LocalReport ReportPath="../PSIAA.Reports/rptContrato.rdlc" ShowDetailedSubreportMessages="True">
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
            iframe.src = "http://" + servidor + "/PSIAA/Reports/Contratos/" + nombreDoc + ".pdf";
        }
    </script>
</asp:Content>
