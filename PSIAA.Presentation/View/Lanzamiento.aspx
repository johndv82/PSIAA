<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="Lanzamiento.aspx.cs" Inherits="PSIAA.Presentation.View.Lanzamiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form name="frmLanzamiento" runat="server" class="form-horizontal">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600"></asp:ScriptManager>
        <div class="col-md-11">
            <div class="row">
                <div class="panel panel-primary">
                    <div class="panel-heading">Información de Contrato</div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="well well-sm">
                                        <div class="row">
                                            <div class="col-md-1">
                                                <label class="control-label">Contrato:</label>
                                                <asp:HiddenField ID="hidUsuario" runat="server" />
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon input-sm">N°</span>
                                                    <asp:TextBox ID="txtContrato" runat="server" class="form-control input-sm" autocomplete="off"></asp:TextBox>
                                                    <asp:HiddenField ID="hidContrato" runat="server" />
                                                    <span class="input-group-btn">
                                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-default btn-sm" OnClick="btnBuscar_Click" />
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblRespuesta" runat="server" Text="" Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:ListBox ID="lstModelos" runat="server" SelectionMode="Multiple" class="multiselect-ui form-control" Visible="false" Rows="6"></asp:ListBox>
                                        <br />
                                        <asp:Button ID="btnProcesarModelos" runat="server" class="btn btn-success btn-sm" Text="Procesar Modelos" Visible="false" OnClick="btnProcesarModelos_Click"/>
                                        <br />

                                        <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                            <HeaderTemplate>
                                                <div class="panel panel-primary">
                                                    <div class="panel-heading">Modelos</div>
                                                    <ul class="list-group">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <li class="list-group-item">
                                                    <asp:Button ID="btnModelo" CommandName="Select" Text='<%# Eval("ModeloAA") %>' runat="server"
                                                        CommandArgument='<%# Eval("ModeloAA") %>' class="buttonLabel" />
                                                </li>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </ul>
                                                        </div>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <asp:Label runat="server" ID="lblMensajeErrorContrato">
                                                <div class="alert alert-dismissible alert-danger">
                                                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                                                    <strong>Error! </strong>No existen modelos con ese contrato.
                                                </div>
                                        </asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="form-group">
                                            <div class="well well-md">
                                                <div class="row">
                                                    <div class="col-md-9">
                                                        <asp:RadioButtonList ID="rblColores" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                            RepeatLayout="Flow" CssClass="radioboxlist" OnSelectedIndexChanged="rblColores_SelectedIndexChanged">
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label for="lblModelo">Modelo:</label>
                                                        <asp:Label ID="lblModelo" runat="server" Font-Bold="True">XXXX-XXX</asp:Label><br />
                                                        <label for="lblModeloSap">SAP:</label>
                                                        <asp:Label ID="lblModeloSap" runat="server">XXXX-XXX</asp:Label>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row form-group">
                                                    <label for="txtMaterial" class="col-md-2">Material Alm.:</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtMaterial" runat="server" class="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:Button ID="btnMaterialColor" runat="server" class="btn btn-info btn-xs" Text="+" Visible="false" data-target="#modalMaterialColor" data-toggle="modal" />
                                                    </div>
                                                    <label for="lblCodMeterial" class="col-md-2 text-right">Cod. Material:</label>
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblCodMaterial" runat="server" Text="--" />
                                                    </div>
                                                </div>
                                                <div class="row form-group">
                                                    <label for="lblKgNecesarios" class="col-md-2">Kg. Necesarios:</label>
                                                    <div class="col-md-1">
                                                        <asp:Label ID="lblKgNecesarios" runat="server" />
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:Button ID="btnActualizarKilos" runat="server" class="btn btn-info btn-xs" Text="Actualizar" Visible="false" OnClick="btnActualizarKilos_Click" />
                                                    </div>
                                                    <label for="lblFechaSol" class="col-md-2 text-right">Fecha Solicit.:</label>
                                                    <div class="col-md-1">
                                                        <asp:Label ID="lblFechaSol" runat="server" />
                                                    </div>
                                                    <label for="lblTitulo" class="col-md-2 text-right">Titulo:</label>
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblTitulo" runat="server" Text="---"></asp:Label>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-2">
                                                        <asp:Table ID="tblNombres" runat="server" Width="100%"
                                                            CssClass="table table-striped table-bordered table-hover"
                                                            AutoGenerateColumns="False">
                                                            <asp:TableRow runat="server">
                                                                <asp:TableCell runat="server">Estado</asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow runat="server">
                                                                <asp:TableCell runat="server" Font-Bold="true">Solicitado</asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow runat="server">
                                                                <asp:TableCell runat="server" Font-Bold="true" ForeColor="Blue">Lanzado</asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <div class="row table-responsive">
                                                            <asp:GridView ID="gridCantSolicitadas" runat="server" Width="90%"
                                                                CssClass="table table-striped table-bordered table-hover"
                                                                AutoGenerateColumns="False"
                                                                EmptyDataText="No hay registros.">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="T1" ItemStyle-Width="50px" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="10px" HeaderStyle-BackColor="#ccccff">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCant1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[0]") %>' Font-Size="Smaller"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="T2" ItemStyle-Width="50px" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="10px" HeaderStyle-BackColor="#ccccff">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCant2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[1]") %>' Font-Size="Smaller"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="T3" ItemStyle-Width="50px" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="10px" HeaderStyle-BackColor="#ccccff">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCant3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[2]") %>' Font-Size="Smaller"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="T4" ItemStyle-Width="50px" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="10px" HeaderStyle-BackColor="#ccccff">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCant4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[3]") %>' Font-Size="Smaller"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="T5" ItemStyle-Width="50px" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="10px" HeaderStyle-BackColor="#ccccff">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCant5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[4]") %>' Font-Size="Smaller"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="T6" ItemStyle-Width="50px" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="10px" HeaderStyle-BackColor="#ccccff">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCant6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[5]") %>' Font-Size="Smaller"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="T7" ItemStyle-Width="50px" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="10px" HeaderStyle-BackColor="#ccccff">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCant7" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[6]") %>' Font-Size="Smaller"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="T8" ItemStyle-Width="50px" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="10px" HeaderStyle-BackColor="#ccccff">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCant8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[7]")  %>' Font-Size="Smaller"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="T9" ItemStyle-Width="50px" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="10px" HeaderStyle-BackColor="#ccccff">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCant9" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[8]") %>' Font-Size="Smaller"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-2">
                                                        <asp:Table ID="tblAlanzar" runat="server" Width="100%"
                                                            CssClass="table table-striped table-bordered table-hover"
                                                            AutoGenerateColumns="False">
                                                            <asp:TableRow runat="server">
                                                                <asp:TableCell runat="server" Font-Bold="true" ForeColor="Red">A Lanzar</asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <div class="row table-responsive">
                                                            <asp:Table ID="tblCantidades" runat="server" Width="90%"
                                                                CssClass="table table-striped table-bordered table-hover"
                                                                AutoGenerateColumns="False">
                                                                <asp:TableRow runat="server">
                                                                    <asp:TableCell runat="server" Width="50px">
                                                                        <asp:TextBox ID="txtCant1" runat="server" Font-Size="Smaller" MaxLength="3" Width="45px" Text="0"
                                                                            onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)" onfocus="ValueDefaultFocus(this);" />
                                                                    </asp:TableCell><asp:TableCell runat="server" Width="50px">
                                                                        <asp:TextBox ID="txtCant2" runat="server" Font-Size="Smaller" MaxLength="3" Width="45px" Text="0"
                                                                            onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)" onfocus="ValueDefaultFocus(this);" />
                                                                    </asp:TableCell><asp:TableCell runat="server" Width="50px">
                                                                        <asp:TextBox ID="txtCant3" runat="server" Font-Size="Smaller" MaxLength="3" Width="45px" Text="0"
                                                                            onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)" onfocus="ValueDefaultFocus(this);" />
                                                                    </asp:TableCell><asp:TableCell runat="server" Width="50px">
                                                                        <asp:TextBox ID="txtCant4" runat="server" Font-Size="Smaller" MaxLength="3" Width="45px" Text="0"
                                                                            onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)" onfocus="ValueDefaultFocus(this);" />
                                                                    </asp:TableCell><asp:TableCell runat="server" Width="50px">
                                                                        <asp:TextBox ID="txtCant5" runat="server" Font-Size="Smaller" MaxLength="3" Width="45px" Text="0"
                                                                            onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)" onfocus="ValueDefaultFocus(this);" />
                                                                    </asp:TableCell><asp:TableCell runat="server" Width="50px">
                                                                        <asp:TextBox ID="txtCant6" runat="server" Font-Size="Smaller" MaxLength="3" Width="45px" Text="0"
                                                                            onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)" onfocus="ValueDefaultFocus(this);" />
                                                                    </asp:TableCell><asp:TableCell runat="server" Width="50px">
                                                                        <asp:TextBox ID="txtCant7" runat="server" Font-Size="Smaller" MaxLength="3" Width="45px" Text="0"
                                                                            onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)" onfocus="ValueDefaultFocus(this);" />
                                                                    </asp:TableCell><asp:TableCell runat="server" Width="50px">
                                                                        <asp:TextBox ID="txtCant8" runat="server" Font-Size="Smaller" MaxLength="3" Width="45px" Text="0"
                                                                            onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)" onfocus="ValueDefaultFocus(this);" />
                                                                    </asp:TableCell><asp:TableCell runat="server" Width="50px">
                                                                        <asp:TextBox ID="txtCant9" runat="server" Font-Size="Smaller" MaxLength="3" Width="45px" Text="0"
                                                                            onblur="ValueDefaultZero(this);" onkeypress="return ValidNum(event)" onfocus="ValueDefaultFocus(this);" />
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <fieldset>
                                                            <legend style="font-size: 13pt;">Propiedades de Asignación: &nbsp;&nbsp;
                                                            <div class="btn-group" role="group" aria-label="...">
                                                                <asp:Button ID="btnAsigOn" runat="server" CssClass="btn btn-success btn-sm" Text="Si" OnClick="btnAsigOn_Click" />
                                                                <asp:Button ID="btnAsigOff" runat="server" CssClass="btn btn-default btn-sm" Text="No" OnClick="btnAsigOff_Click" />
                                                            </div>
                                                            </legend>
                                                            <div class="row form-group">
                                                                <div class="col-md-8">
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            <span class="control-label">Cat. Operación:</span>
                                                                        </div>
                                                                        <div class="col-md-1">
                                                                            <asp:Label ID="lblCodCatOpe" runat="server" Text="0" Font-Bold="true" />
                                                                        </div>
                                                                        <div class="col-md-8">
                                                                            <asp:DropDownList ID="ddlCatOperacion" runat="server" class="form-control input-sm" DataTextField="Value" DataValueField="Key" AutoPostBack="True" OnSelectedIndexChanged="ddlProcesos_SelectedIndexChanged"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <span class="control-label">Fecha de Retorno:</span>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="input-group date" id="fechaRetorno">
                                                                        <asp:TextBox runat="server" ID="txtFechaRetorno" class="form-control input-sm" />
                                                                        <span class="input-group-addon input-sm">
                                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row form-group">
                                                                <div class="col-md-8">
                                                                    <div class="row">
                                                                        <div class="col-md-1">
                                                                            <span class="control-label">Taller: </span>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <asp:Label ID="lblCodTaller" runat="server" Text="0" Font-Bold="true" />
                                                                        </div>
                                                                        <div class="col-md-8">
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtProveedor" runat="server" class="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                                                                <span class="input-group-btn">
                                                                                    <asp:Button ID="btnBuscarAsig" runat="server" class="btn btn-info btn-sm" Text="Buscar" data-target="#modalSelectProveedor" data-toggle="modal" OnClick="btnBuscarAsig_Click" />
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="checkbox">
                                                                        <label>
                                                                            <b>
                                                                                <asp:CheckBox ID="chkTodasOperaciones" runat="server" Text="Todas las Operaciones" Checked="true" /></b>
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 text-right">
                                                                    <asp:Button ID="btnAgregar" runat="server" class="btn btn-primary btn-sm" Text="Agregar" Visible="true" OnClick="btnAgregar_Click" />
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-10 col-sm-offset-1">
                                                        <asp:Label runat="server" ID="lblmsnRegistrosDuplicados" Visible="False">
                                                            <div class="alert alert-dismissible alert-danger">
                                                                <button type="button" class="close" data-dismiss="alert">&times;</button>
                                                                <strong>ERROR! </strong>No se puede agregar registros duplicados.
                                                            </div>
                                                        </asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-10 col-sm-offset-1">
                                                        <asp:Label runat="server" ID="lblmsnError" Visible="False">
                                                            <div class="alert alert-dismissible alert-danger">
                                                                <button type="button" class="close" data-dismiss="alert">&times;</button>
                                                                <strong>Ocurrio un Error Inesperado!!!</strong>
                                                            </div>
                                                        </asp:Label>
                                                    </div>
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
        </div>
        <div class="col-md-12">
            <div class="row">
                <div class="panel panel-primary">
                    <div class="panel-heading">A Lanzar</div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="row table-responsive">
                                            <asp:GridView ID="gridAlanzar" runat="server" Width="100%"
                                                CssClass="table table-striped table-bordered table-hover"
                                                AutoGenerateColumns="False"
                                                EmptyDataText="No hay registros."
                                                OnSelectedIndexChanged="gridAlanzar_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="Modelo" HeaderText="Modelo" ItemStyle-Width="8%" />
                                                    <asp:BoundField DataField="Color" HeaderText="Color" ItemStyle-Width="5%" />
                                                    <asp:BoundField DataField="Material" HeaderText="Material" ItemStyle-Width="30%" />
                                                    <asp:BoundField DataField="KilosNecesarios" HeaderText="Kg." ItemStyle-Width="5%" />
                                                    <asp:TemplateField HeaderText="C1" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCant1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[0]") %>' Font-Size="Smaller"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C2" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCant2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[1]") %>' Font-Size="Smaller"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C3" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCant3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[2]") %>' Font-Size="Smaller"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C4" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCant4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[3]") %>' Font-Size="Smaller"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C5" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCant5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[4]") %>' Font-Size="Smaller"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C6" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCant6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[5]") %>' Font-Size="Smaller"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C7" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCant7" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[6]") %>' Font-Size="Smaller"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C8" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCant8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[7]") %>' Font-Size="Smaller"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C9" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCant9" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidades[8]") %>' Font-Size="Smaller"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Asignacion" HeaderText="¿Asignado?" ItemStyle-Width="5%" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" HeaderText="Acción">
                                                        <ItemTemplate>
                                                            <asp:Button runat="server" ID="btnQuitar" Text="Quitar" class="btn btn-danger btn-xs" CommandName="Select" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-11 text-right">
                                        <asp:Button ID="btnPreLanzamiento" runat="server" class="btn btn-primary btn-sm" Text="Pre Lanzamiento" data-target="#modalPreLanzamiento" data-toggle="modal" OnClick="btnPreLanzamiento_Click" />
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modalPreLanzamiento">
            <div class="modal-dialog modal-lg" style="width: 80% !important;">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title">Pre Lanzamiento</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row table-responsive">
                                    <asp:GridView ID="gridPreLanzamiento" runat="server" Width="100%"
                                        CssClass="table table-striped table-bordered table-hover"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay registros.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Modelo" ItemStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelo" runat="server" Text='<%# Eval("Modelo") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblColor" runat="server" Text='<%# Eval("Color") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Orden" ItemStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOrden" runat="server" Text='<%# Eval("Orden") %>' Font-Size="Smaller" Width="75px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lote" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtLote" runat="server" Text='<%# Eval("Lote") %>' Font-Size="Smaller" MaxLength="2" Width="25px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P1" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblp1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Piezas[0]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Piezas[0]")  %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P2" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblp2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Piezas[1]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Piezas[1]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P3" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblp3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Piezas[2]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Piezas[2]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P4" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblp4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Piezas[3]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Piezas[3]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P5" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblp5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Piezas[4]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Piezas[4]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P6" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblp6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Piezas[5]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Piezas[5]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P7" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblp7" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Piezas[6]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Piezas[6]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P8" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblp8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Piezas[7]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Piezas[7]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P9" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblp9" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Piezas[8]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Piezas[8]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K1" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblk1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Kilos[0]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Kilos[0]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K2" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblk2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Kilos[1]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Kilos[1]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K3" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblk3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Kilos[2]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Kilos[2]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K4" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblk4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Kilos[3]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Kilos[3]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K5" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblk5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Kilos[4]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Kilos[4]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K6" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblk6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Kilos[5]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Kilos[5]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K7" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblk7" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Kilos[6]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Kilos[6]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K8" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblk8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Kilos[7]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Kilos[7]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K9" ItemStyle-Width="3%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblk9" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Kilos[8]").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "Kilos[8]") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="row table-responsive">
                                    <asp:GridView ID="gridAsignaciones" runat="server" Width="100%"
                                        CssClass="table table-striped table-bordered table-hover"
                                        AutoGenerateColumns="False" Visible="false"
                                        EmptyDataText="No hay registros.">
                                        <Columns>
                                            <asp:BoundField DataField="Modelo" HeaderText="Modelo" ItemStyle-Width="10%" />
                                            <asp:BoundField DataField="Color" HeaderText="Color" ItemStyle-Width="10%" />
                                            <asp:BoundField DataField="DescripcionCatOper" HeaderText="Categoria Ope." ItemStyle-Width="25%" />
                                            <asp:BoundField DataField="FechaRetorno" HeaderText="Fecha Ret." DataFormatString="{0:d}" ItemStyle-Width="15%" />
                                            <asp:BoundField DataField="Taller" HeaderText="Taller" ItemStyle-Width="40%" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 text-left">
                                        <asp:Label runat="server" ID="lblOrdenLoteRepetido" Visible="False">
                                        <div class="alert alert-dismissible alert-danger">
                                            <button type="button" class="close" data-dismiss="alert">&times;</button>
                                            <strong>Error! </strong>No puede guardar Ordenes con Lotes duplicados.
                                        </div>
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="col-md-2">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <b>Cargando</b><img src="../Content/Images/load.gif" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div class="col-md-10">
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cancelar</button>
                                    <asp:Button ID="btnGuardarLanz" runat="server" CssClass="btn btn-success btn-sm" Text="Guardar Cambios" OnClick="btnGuardarLanz_Click"></asp:Button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!---- MODAL SELECCION PROVEEDOR - TALLER ---->
        <div class="modal fade" id="modalSelectProveedor">
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Listado de Proveedores</h4>
                            </div>
                            <div class="modal-body">
                                <div class="well well-sm col-sm-12">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="input-group">
                                                <span class="input-group-addon input-sm">Nombre del Taller</span>
                                                <asp:TextBox ID="txtNombreComercial" runat="server" class="form-control input-sm" onkeypress="return InvalidarEnter(event);"></asp:TextBox>
                                                <span class="input-group-btn">
                                                    <asp:Button ID="btnFiltrar" runat="server" Text="Buscar" class="btn btn-default btn-sm" OnClick="btnFiltrar_Click" />
                                                </span>
                                            </div>
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
        <!---- MODAL MATERIAL POR COMBINACION DE COLORES ---->
        <div class="modal fade" id="modalMaterialColor">
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">Ingreso de Material por Color</h4>
                            </div>
                            <div class="modal-body">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div class="row table-responsive">
                                            <asp:GridView ID="gridMaterialColor" runat="server" Width="100%"
                                                CssClass="table table-striped table-bordered table-hover"
                                                AutoGenerateColumns="False"
                                                EmptyDataText="No hay registros.">
                                                <Columns>
                                                    <asp:BoundField DataField="Color" HeaderText="Color" ItemStyle-Width="10%" />
                                                    <asp:BoundField DataField="Porcentaje" HeaderText="%" ItemStyle-Width="10%" />
                                                    <asp:BoundField DataField="Kilos" HeaderText="Kilos" ItemStyle-Width="10%" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50%" HeaderText="Material SAP">
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtMaterialSap" Text='<%# Eval("Producto") %>' Width="280px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Titulo" HeaderText="Titulo" ItemStyle-Width="10%" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cancelar</button>
                                <asp:Button ID="btnAceptarColor" runat="server" Text="Aceptar" class="btn btn-primary btn-sm" OnClick="btnAceptarColor_Click" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!---- FIN MODAL --->
    </form>
    <script type="text/javascript">
        $(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                $('#fechaRetorno').datetimepicker({
                    format: 'YYYY-MM-DD',
                    minDate: '2015-01-01'
                }).data("autoclose", true);
            }
        });

        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return ((tecla > 47 && tecla < 58) || tecla == 46);
        }

        function ValueDefaultZero(obj) {
            obj.value = obj.value.trim() == "" ? "0" : obj.value;
            //Validar Asignado - Lanzado
            var idNumero = obj.id.substring(obj.id.length - 1, obj.id.length);
            var labelSolici = document.getElementById("ContentBody_ContentInitBody_gridCantSolicitadas_lblCant" + idNumero + "_0");
            var labelLanzad = document.getElementById("ContentBody_ContentInitBody_gridCantSolicitadas_lblCant" + idNumero + "_1");
            if ((labelSolici != null) && (labelLanzad != null)) {
                var cantidad = parseInt(labelSolici.innerHTML) - parseInt(labelLanzad.innerHTML);
                if (parseInt(obj.value) > cantidad) {
                    obj.style.backgroundColor = 'salmon';
                } else {
                    obj.style.backgroundColor = 'white';
                }
            }
        }

        function ValueDefaultFocus(obj) {
            obj.value = obj.value.trim() != "0" ? obj.value : "";
        }

        function InvalidarEnter(e) {
            if (e.which == 13) {
                return false;
            }
        }

        function CloseModalPorveedores() {
            $('#modalSelectProveedor').modal('hide');
        }

        function CloseModalMaterialColores() {
            $('#modalMaterialColor').modal('hide');
        }
    </script>
</asp:Content>
