<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="modificar.aspx.cs" Inherits="operations_modificar" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">MODIFICAR DOCUMENTOS</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">   
    function mpecancela_busqueda_rubro()
    {

    } 
    function mpeCuentaOnCancel()
    {

    } 
    function mpeSeleccionOnCancel()
    {

    }
    </script>
<table align="center">
    <tr><td>Tipo de documento: </td> <td>
        <asp:DropDownList ID="lb_tipo_doc" runat="server" onselectedindexchanged="lb_tipo_doc_SelectedIndexChanged" 
                                        AutoPostBack="True">
            <asp:ListItem Value="1">Factura</asp:ListItem>
            <asp:ListItem Value="2">Recibo</asp:ListItem>
            <asp:ListItem Value="3">Nota de Crédito</asp:ListItem>
            <asp:ListItem Value="4">Nota de débito</asp:ListItem>
        </asp:DropDownList>
        </td></tr>
        <tr><td>Sucursal: </td> <td>
        <asp:DropDownList ID="lb_sucursal" runat="server" onselectedindexchanged="lb_sucursal_SelectedIndexChanged" 
                                        AutoPostBack="True">
        </asp:DropDownList>
        </td></tr>
        <tr><td>Tipo Contabilidad: </td><td>
            <asp:DropDownList ID="lb_conta_erronea" runat="server" 
                onselectedindexchanged="lb_conta_erronea_SelectedIndexChanged" AutoPostBack="True">
            </asp:DropDownList>
        </td></tr>
    <tr><td>Serie equivocada: </td><td>
        <asp:DropDownList ID="lb_serie_erronea" runat="server" AutoPostBack="True">
        </asp:DropDownList>
        </td></tr>
    <tr><td>Número de correlativo equivocado: </td><td>
        <asp:TextBox ID="tb_correlativo_erroneo" runat="server" Height="16px"></asp:TextBox>
        </td></tr>
        <tr><td>Tipo Contabilidad Correcta: </td><td>
            <asp:DropDownList ID="lb_conta_correcta" runat="server" 
                onselectedindexchanged="lb_conta_correcta_SelectedIndexChanged" AutoPostBack="True">
            </asp:DropDownList>
        </td></tr>
    <tr><td>Serie correcta: </td><td>
        <asp:DropDownList ID="lb_serie_correcta" runat="server" AutoPostBack="True">
        </asp:DropDownList>
        </td></tr>
    <tr><td>Número de correlativo correcto: </td><td>
        <asp:TextBox ID="tb_correlativo_correcto" runat="server" Height="16px"></asp:TextBox>
        </td></tr>
    
    <tr><td colspan="2" align="center">
        <asp:Button ID="bt_aceptar" runat="server" Text="Modificar" 
            onclick="bt_aceptar_Click" />
        </td></tr>
</table>
</div>
</asp:Content>

