<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="permisos_serie.aspx.cs" Inherits="manager_permisos_serie" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box" align="center">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
        <tr>
            <td colspan="4" height="20" 
                style="color: #FFFFFF; font-weight: bold; background-color: #000000">
                <strong>ASIGNACION DE SERIES POR USUARIO<asp:ScriptManager ID="ScriptManager1" 
                    runat="server">
                </asp:ScriptManager>
                </strong></td>
        </tr>
        <tr>
            <td rowspan="10">
                &nbsp;</td>
            <td colspan="2">
            </td>
            <td rowspan="10">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <b>Usuario</b></td>
            <td>
                <asp:DropDownList ID="drp_usuario" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drp_usuario_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <b>Pais</b></td>
            <td>
                <asp:DropDownList ID="drp_pais" runat="server" AutoPostBack="True" 
                    Enabled="False" onselectedindexchanged="drp_pais_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <b>Sucursal</b></td>
            <td>
                <asp:DropDownList ID="drp_sucursal" runat="server" AutoPostBack="True" 
                    Enabled="False" onselectedindexchanged="drp_sucursal_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <b>Contabilidad</b></td>
            <td>
                <asp:DropDownList ID="drp_contabilidad" runat="server" AutoPostBack="True" 
                    Enabled="False" onselectedindexchanged="drp_contabilidad_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <b>Operacion</b></td>
            <td>
                <asp:DropDownList ID="drp_tipo_operacion" runat="server" AutoPostBack="True" 
                    Enabled="False" 
                    onselectedindexchanged="drp_tipo_operacion_SelectedIndexChanged">
                    <asp:ListItem>Seleccione...</asp:ListItem>
                    <asp:ListItem Value="1">Facturacion</asp:ListItem>
                    <asp:ListItem Value="2">Operaciones</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <b>Tipo</b></td>
            <td>
                <asp:DropDownList ID="drp_tipo_documento" runat="server" Enabled="False" 
                    AutoPostBack="True" 
                    onselectedindexchanged="drp_tipo_documento_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_visualizar" runat="server" Text="Visualizar" 
                    Enabled="False" onclick="btn_visualizar_Click" />
                <asp:Button ID="btn_limpiar" runat="server" Text="Limpiar" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="height: 17px">
                <asp:GridView ID="GridView1" runat="server">
                    <Columns>
                    <asp:TemplateField HeaderText="Asignar">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_asignar" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Restringir">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_restringir" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WL_Clientes">
                        <ItemTemplate>
                                <asp:CheckBox ID="chk_wl_clientes" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WL_Proveedores">
                        <ItemTemplate>
                                <asp:CheckBox ID="chk_wl_proveedores" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="height: 18px">
                <asp:Button ID="btn_asignar" runat="server" Text="Asignar" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                &nbsp;</td>
        </tr>
    </table>
<br />
<br />
    </ContentTemplate>
</asp:UpdatePanel>
</div>
</asp:Content>

