<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="contabilizacion_seguros.aspx.cs" Inherits="manager_contabilizacion_seguros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
        <tr>
            <td align="center">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:Menu ID="Menu1" runat="server" BorderStyle="Outset" Orientation="Horizontal" 
                    Width="400px" Font-Bold="True" onmenuitemclick="Menu1_MenuItemClick">
                    <Items>
                        <asp:MenuItem Text="Listado de Configuraciones" Value="0"></asp:MenuItem>
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
                                    CellPadding="4" ForeColor="#333333" GridLines="None" 
                                    onrowcreated="gv_configuraciones_RowCreated">
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
                                            <strong>Configurar Contabilizacion de Seguros</strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td width="50">
                                            &nbsp;</td>
                                        <td>
                                            Empresa que asegura</td>
                                        <td>
                                            <asp:DropDownList ID="drp_empresa_asegura" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_empresa_asegura_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Moneda del Seguro</td>
                                        <td>
                                            <asp:DropDownList ID="drp_moneda_seguro" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_moneda_seguro_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td colspan="2" 
                                            style="border-top-style: solid; border-top-width: 1px; border-top-color: #000000">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Empresa donde se Contabiliza</td>
                                        <td>
                                            <asp:DropDownList ID="drp_empresa_contabiliza" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="drp_empresa_contabiliza_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Transaccion</td>
                                        <td>
                                            <asp:DropDownList ID="drp_transaccion" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_transaccion_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
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
                                            &nbsp;</td>
                                        <td>
                                            Tipo de Servicio</td>
                                        <td>
                                            <asp:DropDownList ID="drp_tipo_servicio" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_tipo_servicio_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                <asp:ListItem Value="6">Seguros</asp:ListItem>
                                                <asp:ListItem Value="14">Terceros</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Contabilidad</td>
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
                                        <td style="height: 25px">
                                            </td>
                                        <td style="height: 25px">
                                            Moneda de Transaccion</td>
                                        <td style="height: 25px">
                                            <asp:DropDownList ID="drp_moneda_transaccion" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="drp_moneda_transaccion_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
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
                                            &nbsp;</td>
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
                                            &nbsp;</td>
                                        <td>
                                            Serie</td>
                                        <td>
                                            <asp:DropDownList ID="drp_serie" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_serie_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            &nbsp;</td>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btn_guardar" runat="server" 
                                                Text="Guardar" onclick="btn_guardar_Click" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btn_nueva" runat="server" onclick="btn_nueva_Click" 
                                                Text="Nueva" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                                    <tr>
                                        <td align="center" bgcolor="#F3F3F3" 
                                            style="color: black; width: 620px; font-weight: bold;">
                                            Eliminar Configuracion de Seguros</td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="gv_configuraciones_eliminar" runat="server" 
                                                Font-Size="XX-Small" 
                                                onrowdeleting="gv_configuraciones_eliminar_RowDeleting" 
                                                onrowcreated="gv_configuraciones_eliminar_RowCreated">
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

