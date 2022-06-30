<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="configurar_documento_electronico.aspx.cs" Inherits="manager_configurar_documento_electronico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
<fieldset id="Fieldset1">
<h3 id="adduser" align="left">DOCUMENTOS ELECTRONICOS<asp:ScriptManager 
        ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </h3>
</fieldset>
<br />
    <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
        <tr>
            <td align="center">
                <asp:Menu ID="Menu1" runat="server" Font-Bold="False" 
                    onmenuitemclick="Menu1_MenuItemClick" Orientation="Horizontal" 
                    Width="500px" Font-Italic="False">
                    <Items>
                        <asp:MenuItem Text="VER NODO" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="INGRESAR NODO" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="CONFIGURAR NODO" Value="2"></asp:MenuItem>
                        <asp:MenuItem Text="POSICIONAR NODO" Value="3"></asp:MenuItem>
                    </Items>
                    <StaticSelectedStyle Font-Bold="True" Font-Italic="True" 
                        Font-Underline="True" />
                </asp:Menu>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                            <tr>
                                <td>
                                    Pais</td>
                                <td>
                                    <asp:DropDownList ID="drp_pais2" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Documento</td>
                                <td>
                                    <asp:DropDownList ID="drp_documentos2" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contabilidad</td>
                                <td>
                                    <asp:DropDownList ID="drp_contabilidad2" runat="server">
                                        <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                        <asp:ListItem Value="2">Financiera</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_generar_arbol" runat="server" 
                                        onclick="btn_generar_arbol_Click" Text="Visualizar" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    
                                    <asp:TreeView ID="trv_xml" runat="server" ShowLines="True" BorderStyle="None" 
                                        BorderWidth="0px">
                                        <NodeStyle BorderStyle="None" BorderWidth="0px" />
                                    </asp:TreeView>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                            <tr>
                                <td>
                                    Pais</td>
                                <td>
                                    <asp:DropDownList ID="drp_pais" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contabilidad</td>
                                <td>
                                    <asp:DropDownList ID="drp_contabilidad" runat="server">
                                        <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                        <asp:ListItem Value="2">Financiera</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Documento</td>
                                <td>
                                    <asp:DropDownList ID="drp_documentos" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nivel</td>
                                <td>
                                    <asp:DropDownList ID="drp_nivel" runat="server">
                                        <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nodo</td>
                                <td>
                                    <asp:TextBox ID="tb_nombre" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Texto Predeterminado</td>
                                <td>
                                    <asp:TextBox ID="tb_texto_predeterminado" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Puede Repetirse</td>
                                <td>
                                    <asp:CheckBox ID="chk_puede_repetirse" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_ingresar_nodo" runat="server" Text="Guardar" 
                                        onclick="btn_ingresar_nodo_Click" />
                                    <asp:Button ID="btn_nuevo" runat="server" onclick="btn_nuevo_Click" 
                                        Text="Nuevo" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View3" runat="server">
                        <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                            <tr>
                                <td>
                                    Pais</td>
                                <td>
                                    <asp:DropDownList ID="drp_pais3" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contabilidad</td>
                                <td>
                                    <asp:DropDownList ID="drp_contabilidad3" runat="server">
                                        <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                        <asp:ListItem Value="2">Financiera</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Documento</td>
                                <td>
                                    <asp:DropDownList ID="drp_documentos3" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nivel Nodo Hijo</td>
                                <td>
                                    <asp:DropDownList ID="drp_nivel3" runat="server">
                                        <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_visualizar" runat="server" onclick="btn_visualizar_Click" 
                                        Text="Visualizar" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                                        <tr>
                                            <td>
                                                Nodos Hijos</td>
                                            <td>
                                                Nodo Padre</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="drp_nodo_hijo" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_nodo_padre" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_asignar" runat="server" Text="Asignar" 
                                        onclick="btn_asignar_Click" />
                                    <asp:Button ID="btn_nuevo2" runat="server" onclick="btn_nuevo_Click" 
                                        Text="Nuevo" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View4" runat="server">
                        <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                            <tr>
                                <td>
                                    Pais</td>
                                <td>
                                    <asp:DropDownList ID="drp_pais4" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contabilidad</td>
                                <td>
                                    <asp:DropDownList ID="drp_contabilidad4" runat="server">
                                        <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                        <asp:ListItem Value="2">Financiera</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Documento</td>
                                <td>
                                    <asp:DropDownList ID="drp_documentos4" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nivel</td>
                                <td>
                                    <asp:DropDownList ID="drp_nivel4" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="drp_nivel4_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_visualizar2" runat="server" onclick="btn_visualizar2_Click" 
                                        Text="Visualizar" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nodo Padre</td>
                                <td>
                                    <asp:DropDownList ID="drp_nodo_padre2" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="drp_nodo_padre2_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    Nodos Hijos</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" valign="top">
                                    <asp:ListBox ID="lb_nodos_hijos" runat="server" Height="150px" Width="300px"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" valign="top">
                                    <asp:Button ID="btn_up" runat="server" onclick="btn_up_Click" Text="Subir" />
                                    <asp:Button ID="btn_down" runat="server" onclick="btn_down_Click" 
                                        Text="Bajar" />
                                    <asp:Button ID="btn_remove" runat="server" onclick="btn_remove_Click" 
                                        Text="Remover" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_mover_posicion" runat="server" 
                                        onclick="btn_mover_posicion_Click" Text="Guardar" />
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

