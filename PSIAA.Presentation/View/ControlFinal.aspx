<%@ Page Title="" Language="C#" MasterPageFile="~/Init.master" AutoEventWireup="true" CodeBehind="ControlFinal.aspx.cs" Inherits="PSIAA.Presentation.View.ControlFinal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentInitBody" runat="server">
    <form name="frmRecepcion" runat="server" class="form-horizontal">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-5">
                            <div class="panel panel-default">
                                <div class="panel-heading">Repcepción - Control Final</div>
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class="well well-sm">
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <label for="txtOrden" class="control-label">Orden:</label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox Style="text-transform: uppercase" runat="server" class="form-control input-sm" ID="txtOrden" placeholder="Número de Orden / Lote" autocomplete="off" />
                                                    <asp:HiddenField ID="hidOrden" runat="server"/>
                                                    <asp:HiddenField ID="hidLote" runat="server"/>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button runat="server" class="btn btn-primary btn-sm" ID="btnVerDetalle" Text="Ver Detalle" OnClick="btnVerDetalle_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 col-md-offset-1">
                                                <div class="col-md-6">
                                                    <span class="control-label">Modelo:</span>
                                                    <asp:Label runat="server" class="control-label" ID="lblModelo" Text="XXXXXX" Font-Size="Medium" Font-Bold="true"/>
                                                </div>
                                                <div class="col-md-6">
                                                    <span class="control-label">Talla:</span>
                                                    <asp:Label runat="server" class="control-label" ID="lblTalla" Text="XXX" Font-Size="Medium" Font-Bold="true"/>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-12 col-md-offset-1">
                                                <div class="col-md-6">
                                                    <span class="control-label">Color:</span>
                                                    <asp:Label runat="server" class="control-label" ID="lblColor" Text="XXXXXX" Font-Size="Medium" Font-Bold="true"/>
                                                </div>
                                                <div class="col-md-6">
                                                    <span class="control-label">Cantidad:</span>
                                                    <asp:TextBox runat="server" ID="txtPiezas" Width="60px" onkeypress="return ValidNum(event)" MaxLength="2"/>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-10 col-md-offset-1">
                                                <asp:Label runat="server" ID="lblMensajeError" Visible="False">
                                                <div class="alert alert-dismissible alert-danger">
                                                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                                                    <strong>Error! </strong>Orden Invalida, verifique y vuelva a intentarlo.
                                                </div>
                                                </asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-10 col-sm-offset-1">
                                                <asp:Label runat="server" ID="lblValidacion" Visible="False"/>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-7 col-md-offset-1">
                                                <asp:Button runat="server" class="btn btn-default btn-sm" ID="btnConsultarAvance" Text="Consultar Avance" Visible="true" data-target="#modalSeguimiento" data-toggle="modal" OnClick="btnConsultarAvance_Click" />
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Button runat="server" class="btn btn-success btn-sm" ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-7">
                            <div class="panel panel-primary">
                                <div class="panel-heading">Ordenes Ingresadas</div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <asp:Label runat="server" ID="lblMensajeOk" Visible="False">
                                                    <div class="alert alert-dismissible alert-success">
                                                        <button type="button" class="close" data-dismiss="alert">&times;</button>
                                                        <strong>Éxito! </strong>Órden Ingresada Correctamente.
                                                    </div>
                                            </asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row table-responsive">
                                                <asp:GridView ID="gridControlFinal" runat="server" Width="100%"
                                                    CssClass="table table-bordered"
                                                    AutoGenerateColumns="False"
                                                    EmptyDataText="No hay ninguna orden ingresada hoy.">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Orden" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="WhiteSmoke">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrden" runat="server" Text='<%# Bind("Orden") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Lote" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="WhiteSmoke">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLote" runat="server" Text='<%# Bind("Lote") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Talla" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="WhiteSmoke">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblValorTalla" runat="server" Text='<%# Eval("ValorTalla") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Piezas" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="WhiteSmoke">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblValorPieza" runat="server" Text='<%# Eval("ValorPieza") %>' Font-Size="Smaller" Font-Bold="true"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Usuario" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="WhiteSmoke">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUsuario" runat="server" Text='<%# Eval("Usuario") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fecha" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="WhiteSmoke">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFechaIngreso" runat="server" Text='<%# Eval("FechaIngreso", "{0:d}") %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Hora" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="WhiteSmoke">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHora" runat="server" 
                                                                    Text='<%# Eval("HoraIngreso").ToString().Substring(0,2) + ":" +  Eval("HoraIngreso").ToString().Substring(2,2) + ":" + Eval("HoraIngreso").ToString().Substring(Eval("HoraIngreso").ToString().Length -2,2)%>' 
                                                                    Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="¿Completo?" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="WhiteSmoke">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCompleto" runat="server" Text='<%# Eval("Completo").ToString() == "S" ? "Si" : "No" %>' Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!----MODAL ---->
        <div class="modal fade" id="modalSeguimiento">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Seguimiento por Orden de Producción</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row table-responsive">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gridSeguimientoOrden" runat="server" Width="100%"
                                        CssClass="table table-striped table-bordered table-hover"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay ninguna orden ingresada.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Orden" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrden" runat="server" Text='<%# Eval("Orden") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lote" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLote" runat="server" Text='<%# Eval("Lote") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modelo" ItemStyle-Width="8%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelo" runat="server" Text='<%# Eval("Modelo") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblColor" runat="server" Text='<%# Eval("Color") %>' Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lanz." ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLanzado" runat="server" Text='<%# Eval("300") %>' Font-Size="Smaller" Font-Bold="True"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tejido" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTejido" runat="server" Text='<%# Eval("430") %>' Font-Size="Smaller" Font-Bold="True"
                                                        ForeColor='<%# Eval("430").ToString() != Eval("300").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ctrl. Tejido" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCTejido" runat="server" Text='<%# Eval("440") %>' Font-Size="Smaller" Font-Bold="True"
                                                        ForeColor='<%# Eval("440").ToString() != Eval("430").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lavado" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLavado" runat="server" Text='<%# Eval("450") %>' Font-Size="Smaller" Font-Bold="True"
                                                        ForeColor='<%# Eval("450").ToString() != Eval("440").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ctrl. Lavado" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCLavado" runat="server" Text='<%# Eval("460") %>' Font-Size="Smaller" Font-Bold="True"
                                                        ForeColor='<%# Eval("460").ToString() != Eval("450").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Corte" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCorte" runat="server" Text='<%# Eval("470") %>' Font-Size="Smaller" Font-Bold="True"
                                                        ForeColor='<%# Eval("470").ToString() != Eval("460").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Conf." ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConfeccion" runat="server" Text='<%# Eval("510") %>' Font-Size="Smaller" Font-Bold="True"
                                                        ForeColor='<%# Eval("510").ToString() != Eval("470").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acab. Conf." ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAcabadoConf" runat="server" Text='<%# Eval("530") %>' Font-Size="Smaller" Font-Bold="True"
                                                        ForeColor='<%# Eval("530").ToString() != Eval("510").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ctrl. Final" ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAcabadoFinal" runat="server" Text='<%# Eval("550") %>' Font-Size="Smaller" Font-Bold="True"
                                                        ForeColor='<%# Eval("550").ToString() != Eval("530").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Almac." ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-ForeColor="#696C6E" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAlmacen" runat="server" Text='<%# Eval("800") %>' Font-Size="Smaller" Font-Bold="True"
                                                        ForeColor='<%# Eval("800").ToString() != Eval("550").ToString() ? System.Drawing.Color.Red: System.Drawing.ColorTranslator.FromHtml("#696C6E")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <!---- FIN MODAL --->
    </form>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return ((tecla > 47 && tecla < 58) || tecla == 46);
        }
    </script>
</asp:Content>
