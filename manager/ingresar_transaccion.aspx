<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="ingresar_transaccion.aspx.cs" Inherits="manager_ingresar_transaccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div align="center">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 70%">
        <tr>
            <td align="right" colspan="2">
                <asp:HyperLink ID="HyperLink1" runat="server" 
                    NavigateUrl="~/manager/index_contabilizacion_automatica.aspx">Inicio</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="font-weight: bold; color: #FFFFFF; background-color: #000000;">
                Ingresar Transacciones</td>
        </tr>
        <tr>
            <td>
                Transaccion</td>
            <td>
                <asp:DropDownList ID="drp_transaccion" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Tipo de Persona Origen</td>
            <td>
                <asp:DropDownList ID="drp_tpi_origen" runat="server">
                    <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Linea Aerea</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Tipo de Persona Destino</td>
            <td>
                <asp:DropDownList ID="drp_tpi_destino" runat="server">
                    <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Linea Aerea</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_guardar" runat="server" onclick="btn_guardar_Click" 
                    Text="Guardar" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="font-weight: bold; color: #FFFFFF; background-color: #000000;">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gv_transacciones" runat="server" 
                    onrowdeleting="gv_transacciones_RowDeleting" Font-Size="XX-Small">
                    <Columns>
                        <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
</asp:Content>

