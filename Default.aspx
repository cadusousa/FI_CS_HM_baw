<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>BAW</title>
<link rel="stylesheet" type="text/css" href="css/theme4.css" />
<link rel="stylesheet" type="text/css" href="css/style.css" />
<style type="text/css">
</style>
<script language="javascript">
   var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
   document.writeln('<link rel="stylesheet" type="text/css" href="css/' + StyleFile + '">');
</script>
</head>
<body>
	<div id="container">
    	<div id="header">
            <h2>BAW - Billing and Accounting Web</h2>
      </div>
        <div id="wrapper">
        <div id="content">
            <div id="box">
                <fieldset id="Fieldset1">
                <h3 id="adduser">INGRESO AL SISTEMA
                    <legend></legend>
                    <form runat="server" id="form" method="post" name="frmCompletar">
                    <table width="400" align="center">
	                    <thead>
	                    </thead>
	                    <tbody>
	                        <tr>
                                <td align="left">
                                    <asp:Label ID="lbUsuario" runat="server">Usuario:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="frmLogin" runat="server" Width="136px" BackColor="White" 
                                        Height="20px"></asp:TextBox>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lbContrasena" runat="server" Text="Contraseña:" Width="64px"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="frmContrasena" runat="server" TextMode="Password" 
                                        Width="136px" BackColor="White" Height="20px" AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="frmLogin"
                                        ErrorMessage="El campo de usuario es requerido" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <br />
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="frmContrasena"
                                        ErrorMessage="La contraseña es requerida" SetFocusOnError="True"></asp:RequiredFieldValidator><br />
                                    <asp:Label ID="lbnotautenticate" runat="server" BackColor="Yellow" 
                                        BorderColor="Yellow" 
                                        Text="Usuario inválido, por favor verifique su usuario y/o contraseña" 
                                        Visible="False" Width="473px" ForeColor="Red"></asp:Label><br />
                                    <%= strContrasenaExpirada %>

                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="cmdLogin" runat="server" OnClick="cmdLogin_Click"
                                        Text="Ingresar" />&nbsp;<br />
                                </td>
                            </tr>
	                    </tbody>
	                </table>
	                </form>
                    </h3>
                </fieldset>    
            </div>
      </div>
      </div>
        <div id="footer">
        <div id="credits">
   		    V.2.8.12</div>
        <div id="styleswitcher">
            <ul>
                <li><a href="javascript: document.cookie='theme='; window.location.reload();" title="Default" id="defswitch">d</a></li>
                <li><a href="javascript: document.cookie='theme=1'; window.location.reload();" title="Blue" id="blueswitch">b</a></li>
                <li><a href="javascript: document.cookie='theme=2'; window.location.reload();" title="Green" id="greenswitch">g</a></li>
                <li><a href="javascript: document.cookie='theme=3'; window.location.reload();" title="Brown" id="brownswitch">b</a></li>
                <li><a href="javascript: document.cookie='theme=4'; window.location.reload();" title="Mix" id="mixswitch">m</a></li>
            </ul>
        </div><br />

        </div>
</div>
        <p>
        </p>
        </body>
</html>