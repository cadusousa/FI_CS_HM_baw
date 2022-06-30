<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="transferencias_search.aspx.cs" Inherits="operations_cheques_search" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">BUSCAR TRANSFERENCIA</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<table width="650">
    <tr><td>
        <table width="350" align="center">
        <tbody>
            <tr>
                <td>Banco:</td>
                <td>
                    <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" Width="317px" AutoPostBack="True" onselectedindexchanged="lb_bancos_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Cuenta</td>
                <td>
                    <asp:DropDownList ID="lb_cuentas_bancarias" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Número</td>
                <td>
                    <asp:TextBox ID="tb_chequeNo" runat="server" Height="16px" Width="129px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Estado Documento</td>
                <td>                    
                    <asp:DropDownList ID="lb_ted_id" runat="server">
                        <asp:ListItem Value="4">Pagado</asp:ListItem>
                        <asp:ListItem Value="3">Anulado</asp:ListItem>
                        <asp:ListItem Value="14">Anticipada</asp:ListItem>
                    </asp:DropDownList>                    
                </td>
            </tr>
            <tr>
                <td>Tipo Proveedor:</td>
                <td>
                    <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" onselectedindexchanged="lb_tipopersona_SelectedIndexChanged">
                        <asp:ListItem Value="0">Todos</asp:ListItem>
                        <asp:ListItem Value="4">Proveedor</asp:ListItem>
                        <asp:ListItem Value="2">Agente</asp:ListItem>
                        <asp:ListItem Value="5">Naviera</asp:ListItem>
                        <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                        <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                        <asp:ListItem Value="3">Cliente</asp:ListItem>
                        <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Codigo:</td>
                <td>
                    <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" Width="115px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Nombre:</td>
                <td>
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" Width="377px" 
                        Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btn_buscar" runat="server" onclick="btn_buscar_Click" 
                        Text="Buscar" />
                </td>
            </tr>
        </tbody>
        </table>
    </td>
    </tr>
    <tr>
        <td>
            <table align="left">
            <tr>
                <td>
                    <asp:GridView ID="gv_cheques" runat="server" 
                        EmptyDataText="No existen datos que cumplan con estos criterios." 
                        onrowcommand="gv_cheques_RowCommand" PageSize="30" 
                        AutoGenerateColumns="False" onrowcreated="gv_cheques_RowCreated">
                        <EmptyDataRowStyle BackColor="Yellow" ForeColor="#FF3300" />
                        <Columns>
                            <asp:CommandField ButtonType="Button" SelectText="Ver Detalle" 
                                ShowSelectButton="True" />
                            <asp:TemplateField HeaderText="ID">
                                <ItemTemplate>
                                    <asp:Label ID="lb_chequeid" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>    
                            <asp:TemplateField HeaderText="Cuenta">
                                <ItemTemplate>
                                    <asp:Label ID="lb_cuenta" runat="server" Text='<%# Eval("cuenta") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Banco">
                                <ItemTemplate>
                                    <asp:Label ID="lb_banco" runat="server" Text='<%# Eval("banconombre") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Número de Transferencia">
                                <ItemTemplate>
                                    <asp:Label ID="lb_chequeno" runat="server" Text='<%# Eval("chequeno") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemTemplate>
                                    <asp:Label ID="lb_provID" runat="server" Text='<%# Eval("provid") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Moneda">
                                <ItemTemplate>
                                    <asp:Label ID="lb_moneda" runat="server" Text='<%# Eval("moneda") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Monto">
                                <ItemTemplate>
                                    <asp:Label ID="lb_monto" runat="server" Text='<%# Eval("monto") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <!-- ***************************************************************************** -->
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" 
        Width="722px" style="display:none">
            <div><table>
                <tr><td align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:<asp:TextBox ID="tb_nitb" runat="server" Height="16px" Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td>Codigo:<asp:TextBox ID="tb_codigo" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Agente" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_agenteID"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <!-- ***************************************************************************** -->
        </td>
    </tr>
</table>
</div>
</asp:Content>

