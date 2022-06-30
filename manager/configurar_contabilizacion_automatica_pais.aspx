<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="configurar_contabilizacion_automatica_pais.aspx.cs" Inherits="manager_configurar_contabilizacion_automatica_pais" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div align="center">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
        <tr>
            <td align="right" colspan="2">
                <asp:HyperLink ID="HyperLink1" runat="server" 
                    NavigateUrl="~/manager/index_contabilizacion_automatica.aspx">Inicio</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="font-weight: bold; color: #FFFFFF; background-color: #000000">
                Asignar Transacciones</td>
        </tr>
        <tr>
            <td>
                Empresa</td>
            <td>
                <asp:DropDownList ID="drp_empresa" runat="server">
                </asp:DropDownList>
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
                Tipo de Operacion</td>
            <td>
                <asp:DropDownList ID="drp_tipo_operacion" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_visualizar" runat="server" onclick="btn_visualizar_Click" 
                    Text="Visualizar" />
            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_nuevo" runat="server" onclick="btn_nuevo_Click" 
                    Text="Nuevo" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Both" 
                    Width="700px">
                    <asp:GridView ID="gv_transacciones" runat="server" 
    Font-Size="XX-Small" Visible="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TERCEROS">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drp_terceros" runat="server" 
                                    Font-Size="XX-Small">
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Value="True">True</asp:ListItem>
                                        <asp:ListItem>False</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AUTOMATIZAR">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drp_automatizar" runat="server" 
                                    Font-Size="XX-Small">
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem>True</asp:ListItem>
                                        <asp:ListItem>False</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_asignar" runat="server" Text="Asginar" 
                    onclick="btn_asignar_Click" />
            </td>
        </tr>
    </table>
</div>
</asp:Content>

