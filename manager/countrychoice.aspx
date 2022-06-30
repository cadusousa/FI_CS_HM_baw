<%@ Page Language="C#" AutoEventWireup="true" CodeFile="countrychoice.aspx.cs" Inherits="manager_countrychoice" Title="AIMAR - BAW" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>AIMAR - BAW</title>
<link rel="stylesheet" type="text/css" href="../css/theme4.css" />
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script language="javascript">
   var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
   document.writeln('<link rel="stylesheet" type="text/css" href="../css/' + StyleFile + '">');
</script>
</head>

<body onLoad="javascript:self.focus();">
<form id="form1" runat="server">
<div id="container">
    	<div id="header">
        	<!--<h2>AIMAR Administrator</h2>-->
            <h2>BAW - Billing and Accounting Web</h2>
        </div>
        <!--
        <div id="top-panel">-->
            <!--- <div id="panel">
                <ul>
					<li><a href="adduser.aspx" class="useradd">Agregar usuario</a></li>                        
            		<li><a href="search.aspx" class="search">Buscar</a></li>
                </ul>
            </div> --->
        <!--</div>-->
<div id="wrapper">
<div id="content">
<div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">CONFIGURACION DE SESION
<table width="650">
    <tr><td>
        <table width="400" align="center">
        <tbody>
            <tr>
                <td align="left">Pais:</td>
                <td align="left">
                    <asp:DropDownList ID="lb_pais" runat="server" AutoPostBack="True" Height="28px" 
                        onselectedindexchanged="lb_pais_SelectedIndexChanged" Width="170px">
                    </asp:DropDownList>
                 </td>
            </tr>
            <tr>
                <td align="left">Sucursal:</td>
                <td align="left">
                    <asp:DropDownList ID="lb_sucursal" runat="server" Height="28px" Width="171px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left"><asp:Label ID="l_contabilidad" runat="server" Text="Tipo Contabilidad: "></asp:Label></td>
                <td align="left">
                    <asp:DropDownList ID="lb_contabilidad" runat="server" Height="28px" 
                        Width="170px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="bt_aceptar" runat="server" onclick="bt_aceptar_Click" 
                        Text="Seleccionar" />
                </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
</table>
</h3>
</fieldset>
</div>
</div>
            
      </div>
</form>
</body>
</html>
