<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="configurar_intercompany_administrativo.aspx.cs" Inherits="manager_configurar_intercompany_administrativo" %>

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
                                        <td align="center" bgcolor="#F3F3F3" colspan="3" 
                                            style="color: black; ">
                                            <strong>Configurar Intercompany Administrativo</strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            Cobro</td>
                                    </tr>
                                    <tr>
                                        <td width="50">
                                            &nbsp;</td>
                                        <td>
                                            Empresa</td>
                                        <td>
                                            <asp:DropDownList ID="drp_empresa_origen" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Contabilidad</td>
                                        <td>
                                            <asp:DropDownList ID="drp_contabilidad_origen" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="drp_contabilidad_origen_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                                <asp:ListItem Value="2">Financiera</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Moneda</td>
                                        <td>
                                            <asp:DropDownList ID="drp_moneda_origen" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_moneda_origen_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Tipo de Operacion</td>
                                        <td>
                                            <asp:DropDownList ID="drp_operacion" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_operacion_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                <asp:ListItem Value="2">Cobro</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Al Intercompany</td>
                                        <td>
                                            <asp:DropDownList ID="drp_intercompanys" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_intercompanys_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            Intercompany Paga en</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Contabilidad</td>
                                        <td>
                                            <asp:DropDownList ID="drp_contabilidad_destino" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="drp_contabilidad_destino_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                                <asp:ListItem Value="2">Financiera</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Moneda</td>
                                        <td>
                                            <asp:DropDownList ID="drp_moneda_destino" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_moneda_destino_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Tipo Documento</td>
                                        <td>
                                            <asp:DropDownList ID="drp_documento_destino" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_documento_destino_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Sucursal</td>
                                        <td>
                                            <asp:DropDownList ID="drp_sucursal_destino" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_sucursal_destino_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Moneda de Impresion</td>
                                        <td>
                                            <asp:DropDownList ID="drp_moneda_impresion" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_moneda_impresion_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            &nbsp;</td>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btn_guardar" runat="server" onclick="btn_guardar_Click" 
                                                Text="Guardar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                                    <tr>
                                        <td align="center" bgcolor="#F3F3F3" 
                                            style="color: black; width: 620px; font-weight: bold;">
                                            Editar Intercompany Administrativo</td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="gv_editar_configuraciones" runat="server" 
                                                Font-Size="XX-Small" onrowdeleting="gv_editar_configuraciones_RowDeleting">
                                                <Columns>
                                                    <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                                                </Columns>
                                                <HeaderStyle BackColor="#CCCCCC" />
                                            </asp:GridView>
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

