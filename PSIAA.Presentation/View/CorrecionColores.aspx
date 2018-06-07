<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="CorrecionColores.aspx.cs" Inherits="PSIAA.Presentation.View.CorrecionColores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="formCorrecionColores">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <div class="modal fade" id="modalCorrecionColores">
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Corrección de Colores</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <label class="control-label">Contrato:</label>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm" onkeypress="return ValidNum(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" class="btn btn-primary btn-sm" OnClick="btnAceptar_Click" />
                                    </div>
                                    <div class="col-md-3">
                                        <div class="col-md-12 text-center">
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                                <ProgressTemplate>
                                                    <b>Cargando</b><img src="../Content/Images/load.gif" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <asp:Label runat="server" ID="lblMensajeOk" Visible="false">
                                        <div class="alert alert-dismissible alert-success">
                                            <button type="button" class="close" data-dismiss="alert">&times;</button>
                                            <strong>Éxito! </strong>Proceso ejecutado Correctamente.
                                        </div>
                                    </asp:Label>
                                </div>
                                <br />
                                <div class="row">
                                    <asp:Label runat="server" ID="lblError" Visible="False">
                                        <div class="alert alert-dismissible alert-warning">
                                            <button type="button" class="close" data-dismiss="alert">&times;</button>
                                            <strong>Aviso! </strong>El proceso no encontró ninguna Corrección.
                                         </div>
                                    </asp:Label>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" class="btn btn-default btn-sm" OnClick="btnCerrar_Click"/>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $(window).on('load', function () {
            $('#modalCorrecionColores').modal('show');
        });

        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return ((tecla > 47 && tecla < 58));
        }
    </script>
</asp:Content>
