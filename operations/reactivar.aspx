<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="reactivar.aspx.cs" Inherits="operations_reactivar" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">REACTIVAR DOCUMENTOS</h3>
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
            <asp:ListItem Value="2">Recibo</asp:ListItem>
            <asp:ListItem Value="17">Depositos</asp:ListItem>
            <asp:ListItem Value="5">Provisiones</asp:ListItem>
        </asp:DropDownList>
        <!--<asp:ListItem Value="1">Factura</asp:ListItem>-->
        <!--<asp:ListItem Value="4">Nota de débito</asp:ListItem>-->
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
    <tr><td>Serie: </td><td>
        <asp:DropDownList ID="lb_serie_erronea" runat="server" AutoPostBack="True">
        </asp:DropDownList>
        </td></tr>
    <tr><td>Banco: </td><td>
            <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                </asp:DropDownList>
        </td></tr>
    <tr><td>Cuenta Bancaria: </td><td>
        
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server">
                </asp:DropDownList>
        </td></tr>            
    <tr><td>Número de correlativo: </td><td>
        <asp:TextBox ID="tb_correlativo" runat="server" Height="16px"></asp:TextBox>
        </td></tr>
    <tr><td colspan="2" align="center">
        <asp:Button ID="bt_aceptar" runat="server" Text="Reactivar" 
            onclick="bt_aceptar_Click" />
        </td></tr>
</table>
</div>
</asp:Content>

