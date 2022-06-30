<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="administracion_tipos_servicio.aspx.cs" Inherits="manager_administracion_tipos_servicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <br />
    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
        <tr>
            <td align="center" bgcolor="#F3F3F3" style="font-weight: bold">
                Administracion de Tipos de Servicio</td>
        </tr>
        <tr>
            <td align="center" valign="middle">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td style="height: 18px">
                            Pais</td>
                        <td style="height: 18px">
                                                        <asp:DropDownList ID="drp_paises" runat="server" AutoPostBack="True" 
                                                            onselectedindexchanged="drp_paises_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                    </tr>
                    <tr>
                        <td style="height: 18px">
                            Contabilidad</td>
                        <td style="height: 18px">
                                                        <asp:DropDownList ID="drp_contabilidad" 
                                runat="server">
                                                            <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                                            <asp:ListItem Value="2">Financiera</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                    </tr>
                    <tr>
                        <td style="height: 18px">
                            Sucursal</td>
                        <td style="height: 18px">
                                                        <asp:DropDownList ID="drp_sucursales" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                    </tr>
                    <tr>
                        <td>
                            Documento</td>
                        <td>
                                                        <asp:DropDownList ID="drp_documentos" 
                                runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btn_visualizar" runat="server" Text="Visualizar" 
                                onclick="btn_visualizar_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                <tr>
                                    <td align="center" bgcolor="Black" style="color: #FFFFFF; font-weight: bold;">
                                        Listado de Tipos de Servicio Asignados</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="gv_tipos_servicio" runat="server" EmptyDataText="...">
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
                            <asp:Button ID="btn_asignar" runat="server" Text="Asignar" 
                                onclick="btn_asignar_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
<br />
</asp:Content>

