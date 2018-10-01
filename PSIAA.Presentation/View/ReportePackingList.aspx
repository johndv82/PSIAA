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
                                        <div class="col-md-10">
                                            <div class="well well-sm">
                                                <div class="row">
                                                    <div class="col-md-1">
                                                        <label class="control-label">Tipo:</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="input-group">
                                                            <span class="input-group-addon input-sm">N°</span>
                                                            <asp:TextBox ID="txtTipo" runat="server" class="form-control input-sm" autocomplete="off" MaxLength="6"></asp:TextBox>
                                                            <asp:HiddenField ID="hidUsuario" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label">Serie:</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="input-group">
                                                            <span class="input-group-addon input-sm">N°</span>
                                                            <asp:TextBox ID="txtSerie" runat="server" class="form-control input-sm" autocomplete="off" MaxLength="6"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                                            <asp:HiddenField ID="HiddenField2" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label">Correlativo:</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="input-group">
                                                            <span class="input-group-addon input-sm">N°</span>
                                                            <asp:TextBox ID="txtCorrelativo" runat="server" class="form-control input-sm" autocomplete="off" MaxLength="6"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField3" runat="server" />
                                                            <asp:HiddenField ID="HiddenField4" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:Button ID="btnAceptar" runat="server" Text="Buscar" class="btn btn-primary btn-sm" OnClick="btnAceptar_Click" />
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                                            <ProgressTemplate>
                                                                <b>Cargando</b><img src="../Content/Images/load.gif" />
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                    <div class="co-md-1">
                                                        <label class="control-label">Doc.:</label>
                                                        <asp:Label ID="lblDocEntry" runat="server" class="control-label" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2"><br />
                                             <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger" >
                                                 <strong>Error!</strong> Datos Incorrectos
                                             </asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive text-center">
                                                <div id="reporte" style="color: white;">
                                                    <rsweb:ReportViewer ID="rptViewPackingList" runat="server" Font-Names="Verdana" Font-Size="8pt" SizeToReportContent="True" Visible="false">
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
