<%@ Page Title="" Language="C#" MasterPageFile="~/PSIAA.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="PSIAA.Presentation.View._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBody" runat="server">
    <!-- Cabecera -->
    <div id="top" class="header">
        <div class="vert-text">
            <br />
            <img alt="" class="img-responsive" src="../Content/images/logo.png" /><br />
            <br />
            <br />

            <!--LOGIN-->
            <form class="login-form" id="Form1" method="post" runat="server">
                <div>
                    <br />
                    <p><b>Inicio de Sesión</b></p>
                </div>
                <div class="content">
                    <fieldset>
                        <legend>PSIAA</legend>
                        <div class="form-group">
                            <div class="col-lg-14">
                                <asp:TextBox runat="server" type="text" class="form-control" ID="txtUsuario" placeholder="Usuario"></asp:TextBox>
                                <br />
                                <asp:TextBox runat="server" TextMode="Password" class="form-control" ID="txtPassword" placeholder="Contraseña" />
                                <div class="checkbox col-lg-offset-6">
                                    <label>
                                        <input type="checkbox" disabled="disabled" />
                                        Recordar Clave
                                    </label>
                                </div>
                                <div class="col-lg-offset-8">
                                    <asp:Button ID="btnLogin" runat="server" class="btn btn-primary" Text="Ingresar" OnClick="btnLogin_Click" />
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </form>
            <div id="alertError">
                <asp:Label runat="server" ID="lblMensajeError" Visible="False">
                    <div class="alert alert-dismissible alert-danger">
                        <button type="button" class="close" data-dismiss="alert">&times;</button>
                        <strong>Error! </strong>Credenciales Incorrectas
                    </div>
                </asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
