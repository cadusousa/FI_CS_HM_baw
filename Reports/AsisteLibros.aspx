<%@ Page Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="AsisteLibros.aspx.cs" Inherits="Reports_CompraVenta" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="searchresult">
<asp:ScriptManager ID="ScriptManager1" runat="server" >
 <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
<script language="javascript" type="text/javascript">
    function mpecancela_busqueda_rubro() {

    }
    function mpeCuentaOnCancel() {

    }
    function mpeSeleccionOnCancel() {

    }
</script>
<script  type="text/javascript">
    function BloquearPantalla() {
        $.blockUI({ message: '<h1>Generando...</h1>' });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI();
        });
    }
    </script>
<table width="300" align="center">
<thead>
    <tr><th colspan="2">Asiste Libros</th></tr>
</thead>
<tbody>
<tr>
    <td>Tipo reporte</td>
    <td>
                    <asp:DropDownList ID="lb_reporte" runat="server">
                        <asp:ListItem Value="1">Compras</asp:ListItem>
                        <asp:ListItem Value="2">Ventas</asp:ListItem>
                    </asp:DropDownList>
    </td>
</tr>
                <tr>
                <td><asp:Label ID="l_contabilidad" runat="server" Text="Tipo Contabilidad"></asp:Label></td>
                <td>
                        <asp:DropDownList ID="lb_contabilidad" runat="server">
                        </asp:DropDownList>
                </td>
                </tr>
<tr>
    <td>Moneda</td>
    <td>
                    <asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>
    </td>
</tr>
<tr>
    <td>Empresa</td>
    <td>
                    <asp:DropDownList ID="lb_empresa" runat="server">
                    </asp:DropDownList>
    </td>
</tr>
<tr>
    <td>Fecha Inicial</td>
    <td>
        
        <asp:TextBox ID="tb_fechaini" runat="server" Height="16px" Width="90px"></asp:TextBox>
        <cc1:CalendarExtender ID="tb_fechaini_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fechaini" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
    </td>
</tr>
<tr>
    <td>Fecha Final</td>
    <td>
        
        <asp:TextBox ID="tb_fechafin" runat="server" Height="16px" Width="90px"></asp:TextBox>
        <cc1:CalendarExtender ID="tb_fechafin_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fechafin" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
    </td>
</tr>
                <tr>
                    <td>Estado</td>
                    <td>
                        <asp:CheckBoxList ID="CB_Estados" runat="server">
                            <asp:ListItem Value="1">Activo</asp:ListItem>
                            <asp:ListItem Value="5">Autorizada</asp:ListItem>
                            <asp:ListItem Value="2">Abonado</asp:ListItem>
                            <asp:ListItem Value="3">Anulada</asp:ListItem>
                            <asp:ListItem Value="9">Cortada</asp:ListItem>
                            <asp:ListItem Value="4">Pagada</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
<tr>
    <td colspan="2" align="center">
        
        <asp:Button ID="bt_generar" runat="server" onclick="bt_generar_Click" 
            Text="Generar"   />
        
    </td>
</tr>
</tbody>
</table>
</fieldset>
</div>
</asp:Content>

