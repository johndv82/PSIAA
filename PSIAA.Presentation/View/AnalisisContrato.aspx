<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="AnalisisContrato.aspx.cs" Inherits="PSIAA.Presentation.View.AnalisisContrato" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form runat="server" id="fromSimulacionMP">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true" AsyncPostBackTimeout="600"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="panel panel-success">
                                <div class="panel-heading">Análisis Completo de Contrato</div>
                                <div class="panel-body">
                                    <div class="well well-sm">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <span class="control-label">Contrato: </span>
                                            </div>
                                            <div class="col-md-9">
                                                <div class="input-group">
                                                    <span class="input-group-addon input-sm">N°</span>
                                                    <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm" autocomplete="off"></asp:TextBox>
                                                    <asp:HiddenField ID="hidContrato" runat="server" />
                                                    <asp:HiddenField ID="hidUsuario" runat="server" />
                                                    <span class="input-group-btn">
                                                        <asp:Button ID="btnBuscar" runat="server" class="btn btn-info btn-sm" Text="Buscar" OnClick="btnBuscar_Click" />
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-3">
                                                <span class="control-label">Modelo: </span>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:DropDownList ID="ddlModelo" runat="server" class="form-control input-sm" />
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:Button ID="btnVerContrato" runat="server" class="btn btn-default btn-sm" Visible="false" Text="Ver Contrato" />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Button ID="btnAnalizar" runat="server" class="btn btn-primary btn-sm" Text="Analizar Modelo" OnClick="btnAnalizar_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="panel panel-info">
                                <div class="panel-heading">Peso Base por Modelo</div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="form-group">
                                                    <span class="col-md-3 control-label">Talla:</span>
                                                    <div class="col-md-9">
                                                        <asp:Label ID="lblTalla" runat="server" Font-Bold="true" Text="--"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="form-group">
                                                    <span class="col-md-3 control-label">Peso:</span>
                                                    <div class="col-md-9">
                                                        <asp:Label ID="lblPeso" runat="server" Font-Bold="true" Text="--"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="panel panel-info">
                                <div class="panel-heading">Materiales por Modelo</div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridMateriales" runat="server" Width="100%"
                                                    CssClass="table table-bordered"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No existe ningun registro">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Color" Visible="false" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblColor" runat="server" Text='<%# Bind("Color") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Material" HeaderText="Nombre Material" ItemStyle-Width="35%" />
                                                        <asp:BoundField DataField="Porcentaje" HeaderText="%" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción Material" ItemStyle-Width="45%" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-default">
                                <div class="panel-heading">Medidas por Modelo</div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gridMedidas" runat="server" Width="100%"
                                                    CssClass="table table-bordered"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="Error, no hay medidas establecidas para su Talla Base."
                                                    OnRowDataBound="gridMedidas_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="CodMedida" HeaderText="Cod. Medida" ItemStyle-Width="50%" />
                                                        <asp:BoundField DataField="Medida" HeaderText="Medida" ItemStyle-Width="50%" />
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
