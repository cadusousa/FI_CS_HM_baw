<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="configurar_intercompany_operativo.aspx.cs" Inherits="manager_configurar_intercompany_operativo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
        <tr>
            <td align="center">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:Menu ID="Menu1" runat="server" BorderStyle="Outset" 
                    onmenuitemclick="Menu1_MenuItemClick" Orientation="Horizontal" 
                    Width="400px" Font-Bold="True">
                    <Items>
                        <asp:MenuItem Text="Configuraciones" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="Ingresar Configuracion" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="Ingresar Transaccion" Value="3"></asp:MenuItem>
                        <asp:MenuItem Text="Eliminar Configuracion" Value="2"></asp:MenuItem>
                    </Items>
                </asp:Menu>
            </td>
        </tr>
        <tr>
            <td align="center">
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View1" runat="server">
                                <asp:GridView ID="gv_configuraciones" runat="server" Font-Size="XX-Small" 
                                    CellPadding="4" ForeColor="#333333" GridLines="None">
                                    <AlternatingRowStyle BackColor="White" />
                                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                    <SortedDescendingHeaderStyle BackColor="#820000" />
                                </asp:GridView>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                                    <tr>
                                        <td align="center" bgcolor="#F3F3F3" colspan="2" 
                                            style="color: black; ">
                                            <strong>Configurar Intercompany Operativo</strong></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Transaccion</td>
                                        <td>
                                            <asp:DropDownList ID="drp_tipo_trasaccion" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Intercompany</td>
                                        <td>
                                            <asp:DropDownList ID="drp_intercompanys" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_intercompanys_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lbl_empresa_id" runat="server" Text="0" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Sistema</td>
                                        <td>
                                            <asp:DropDownList ID="drp_sistema" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_sistema_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo de Documento</td>
                                        <td>
                                            <asp:DropDownList ID="drp_tipo_documento" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_tipo_documento_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo de Contabilidad</td>
                                        <td>
                                            <asp:DropDownList ID="drp_contabilidad" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_contabilidad_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                                <asp:ListItem Value="2">Financiera</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Moneda</td>
                                        <td>
                                            <asp:DropDownList ID="drp_moneda" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_moneda_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Sucursal</td>
                                        <td>
                                            <asp:DropDownList ID="drp_sucursal" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_sucursal_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Operacion</td>
                                        <td>
                                            <asp:DropDownList ID="drp_tipo_operacion" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_tipo_operacion_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione</asp:ListItem>
                                                <asp:ListItem Value="1">Facturacion</asp:ListItem>
                                                <asp:ListItem Value="2">Operaciones</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Serie</td>
                                        <td>
                                            <asp:DropDownList ID="drp_serie" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_serie_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btn_guardar_configuracion" runat="server" Text="Guardar" 
                                                onclick="btn_guardar_configuracion_Click" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btn_limpiar" runat="server" onclick="btn_limpiar_Click" 
                                                Text="Limpiar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                                    <tr>
                                        <td align="center" bgcolor="#F3F3F3" 
                                            style="color: black; width: 620px; font-weight: bold;">
                                            Editar Intercompany Operativo</td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="gv_configuraciones_eliminar" runat="server" CellPadding="4" 
                                                Font-Size="XX-Small" ForeColor="#333333" GridLines="None" 
                                                onrowdeleting="gv_configuraciones_eliminar_RowDeleting">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                                                </Columns>
                                                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View4" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                                    <tr>
                                        <td align="center" bgcolor="#F3F3F3" colspan="3" style="color: black; ">
                                            <strong>Ingresar Transaccion Intercompany Administrativo</strong></td>
                                    </tr>
                                    <tr>
                                        <td width="50">
                                            &nbsp;</td>
                                        <td>
                                            Nombre</td>
                                        <td>
                                            <asp:TextBox ID="tb_transaccion" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            &nbsp;</td>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btn_guardar_transaccion" runat="server" 
                                                onclick="btn_guardar_transaccion_Click" Text="Guardar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
            </td>
        </tr>
    </table>
</div>
</asp:Content>

