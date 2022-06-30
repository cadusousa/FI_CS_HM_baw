<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="restricciones.aspx.cs" Inherits="manager_administrar_restricciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
        <tr>
            <td align="center">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:Menu ID="Menu1" runat="server" BorderStyle="Outset" 
                    onmenuitemclick="Menu1_MenuItemClick" Orientation="Horizontal" 
                    Width="400px" Font-Bold="True">
                    <Items>
                        <asp:MenuItem Text="Restricciones" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="Ingresar Restricciones" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="Asignar Restricciones" Value="2"></asp:MenuItem>
                    </Items>
                </asp:Menu>
            </td>
        </tr>
        <tr>
            <td>
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View1" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                    <tr>
                                        <td align="Center" bgcolor="Black" style="color: white; height: 15px;">
                                            <strong>Listado de Restricciones</strong></td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="gv_restricciones2" runat="server" 
                                                onrowcreated="gv_restricciones2_RowCreated" BorderColor="Black">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
                                    <tr>
                                        <td align="Center" bgcolor="#F3F3F3" style="color: black; width: 620px;">
                                            <strong>Ingresar Restricciones</strong></td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 620px">
                                            <table align="center" style="width: 95%">
                                                <tr>
                                                    <td width="200">
                                                        Nombre</td>
                                                    <td>
                                                        <asp:TextBox ID="tb_nombre" runat="server" Height="16px" MaxLength="120"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Descripcion</td>
                                                    <td>
                                                        <asp:TextBox ID="tb_descripcion" runat="server" MaxLength="450" 
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Button ID="btn_ingresar" runat="server" Text="Ingresar" 
                                                            onclick="btn_ingresar_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
                                                            <tr>
                                                                <td bgcolor="Black" style="color: white">
                                                                    <strong>Listado de Restricciones Ingresadas</strong></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:GridView ID="gv_restricciones" runat="server" 
                                                                        onrowcreated="gv_restricciones_RowCreated" 
                                                                        onrowcommand="gv_restricciones_RowCommand" BorderColor="Black">
                                                                        <Columns>
                                                                            <asp:ButtonField Text="Activar/Desactivar" CommandName="Desactivar" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                    <tr>
                                        <td align="Center" bgcolor="#F3F3F3" style="color: black">
                                            <strong>Asignar Restricciones</strong></td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                                <tr>
                                                    <td>
                                                        Pais</td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_paises" runat="server" AutoPostBack="True" 
                                                            onselectedindexchanged="drp_paises_SelectedIndexChanged">
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
                                                        <asp:DropDownList ID="drp_documentos" runat="server" AutoPostBack="True" 
                                                            onselectedindexchanged="drp_documentos_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Sucursal</td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_sucursales" runat="server">
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
                                                        <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                                            <tr>
                                                                <td bgcolor="Black" style="color: white">
                                                                    <strong>Listado de Restricciones Asignadas</strong></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:GridView ID="gv_restricciones3" runat="server" 
                                                                        onrowcreated="gv_restricciones3_RowCreated" Visible="False" 
                                                                        BorderColor="Black">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chk_estado" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Button ID="btn_guardar" runat="server" Text="Asignar" 
                                                            onclick="btn_guardar_Click" />
                                                    </td>
                                                </tr>
                                            </table>
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

