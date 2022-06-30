<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="pop_buscarrcpt.aspx.cs" Inherits="invoice_pop_buscarrcpt2" Title="BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<tbody>
<h3 id="adduser">BUSCAR RECIBOS</h3>
    <table width="700" align="center">
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr><td align="left">Codigo Cliente</td><td>
                                                <asp:TextBox ID="tb_codigo_cliente" 
                                                        runat="server" Height="16px" Width="300px"></asp:TextBox></td></tr>
                                            <tr><td align="left">Nombre Cliente</td><td>
                                                <asp:TextBox ID="tb_nombrecliente" 
                                                        runat="server" Height="16px" Width="300px"></asp:TextBox></td></tr>
                            <tr><td align="left">Identificación tributaria</td><td>
                                <asp:TextBox ID="tb_nit" runat="server" 
                                                        Height="16px" Width="300px"></asp:TextBox></td></tr>
                            <tr><td align="left">Serie</td><td> 
                                <asp:DropDownList ID="lb_serie_factura" 
                                    runat="server" Width="150px">
                                    </asp:DropDownList><asp:TextBox ID="tb_corr_factura" runat="server" Width="35px" Visible="false"></asp:TextBox></td></tr>
                            <tr>
                                <td align="left">
                                    No. Recibo</td>
                                <td valign="middle">
                                    <asp:TextBox ID="tb_recibo" runat="server" 
                                        Width="300px" Height="16px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                        ControlToValidate="tb_recibo" ErrorMessage="Solo se permite números" 
                                        SetFocusOnError="True" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                                </td>
                        </tr>
                        <tr>
                            <td align="left">
                                HBL</td>
                            <td>
                                <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                MBL</td>
                            <td>
                                <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Contenedor</td>
                            <td>
                                <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Routing</td>
                            <td>
                                <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Estado Documento</td>
                            <td>
                                
                                <asp:DropDownList ID="lb_ted_id" runat="server" Width="150px">
                                    <asp:ListItem Value="0">Todos</asp:ListItem>
                                    <asp:ListItem Value="1">Activa</asp:ListItem>
                                    <asp:ListItem Value="2">Abonada</asp:ListItem>
                                    <asp:ListItem Value="3">Anulada</asp:ListItem>
                                    <asp:ListItem Value="4">Pagada</asp:ListItem>
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Con saldo pendiente aplicar:</td>
                            <td>
                                <asp:CheckBox ID="chk_pendiente" runat="server" />
                                <asp:Button ID="Button1" runat="server" Text="Buscar" onclick="bt_search_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="dgw1" runat="server" onrowcommand="dgw1_RowCommand" 
                            AllowPaging="True" 
                            EmptyDataText="No se encontraron datos con los criterios ingresados" PageSize="20" 
                            onpageindexchanging="dgw1_PageIndexChanging" 
                            onrowcreated="dgw1_RowCreated">
                        <Columns>
                            <asp:ButtonField HeaderText="Seleccionar" Text="Seleccionar" 
                                    CommandName="Seleccionar" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
    </table>
    </tbody>
  </div>
</asp:Content>

