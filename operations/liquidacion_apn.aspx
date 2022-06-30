<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="liquidacion_apn.aspx.cs" Inherits="operations_liquidacion_apn" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<h3 id="adduser">BUSCAR PROVEEDOR, AGENTE, NAVIERA</h3>
    <table width="80%" align="center">
        <tr>
            <td>Nombre Proveedor</td><td>
            <asp:TextBox ID="tb_nombre_proveedor" runat="server"></asp:TextBox>
            </td>
            <td>Nit Proveedor</td><td>
            <asp:TextBox ID="tb_nit" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Estado Provision</td>
            <td>
                <asp:DropDownList ID="lb_tipopersona" runat="server">
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td><td>
                                    &nbsp;</td>
        </tr>
        <tr>    
            <td colspan="4" align="center">
                <asp:Button ID="btn_buscar" runat="server"  
                    Text="Buscar" />
            </td>
        </tr>
    </table>
</div>
<div id="box">
    <h3 id="H1">Notas de credito</h3>
    <table width="95%" align="center">
    <tr><td align="center">
    </td></tr>
    </table>
</div>
<div id="box">
    <h3 id="H2">Notas de debito</h3>
    <table width="95%" align="center">
    <tr><td align="center">
    </td></tr>
    </table>
</div>
<div id="box">
    <h3 id="H3">Provisiones</h3>
    <table width="95%" align="center">
    <tr><td align="center">
    </td></tr>
    </table>
</div>
</asp:Content>

