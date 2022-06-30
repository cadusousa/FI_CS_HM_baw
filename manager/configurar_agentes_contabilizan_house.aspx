<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="configurar_agentes_contabilizan_house.aspx.cs" Inherits="manager_configurar_agentes_contabilizan_house" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div align="center">

    <table align="center" cellpadding="0" cellspacing="0" style="width: 85%">
        <tr>
            <td align="right" colspan="2">
                <asp:HyperLink ID="HyperLink1" runat="server" 
                    NavigateUrl="~/manager/index_contabilizacion_automatica.aspx">Inicio</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="font-weight: bold; color: #FFFFFF; background-color: #000000">
                Configurar 
                Grupo de Agentes Contabilizan por House</td>
        </tr>
        <tr>
            <td>
                Empresa</td>
            <td>
                <asp:DropDownList ID="drp_empresa" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drp_empresa_SelectedIndexChanged">
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
                <asp:DropDownList ID="drp_tipo_operacion" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drp_tipo_operacion_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Grupo</td>
            <td>
                <asp:DropDownList ID="drp_grupos" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_guardar" runat="server" Text="Guardar" 
                    onclick="btn_guardar_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_nuevo" runat="server" onclick="btn_nuevo_Click" 
                    Text="Nuevo" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="font-weight: bold; color: #FFFFFF; background-color: #000000">
                Grupo de Agentes que Contabilizan por House</td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gv_agentes_contabilizan_house" runat="server">
                </asp:GridView>
            </td>
        </tr>
    </table>

</div>
</asp:Content>

