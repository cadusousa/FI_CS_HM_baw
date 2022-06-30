<%@ Page Language="C#" AutoEventWireup="true" CodeFile="logout.aspx.cs" Inherits="logout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>BAW</title>
<link rel="stylesheet" type="text/css" href="css/theme4.css" />
<link rel="stylesheet" type="text/css" href="css/style.css" />
<script language="javascript">
   var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
   document.writeln('<link rel="stylesheet" type="text/css" href="css/' + StyleFile + '">');
</script>
<script type="text/javascript">
function refresh ()
{
    top.opener.document.location = top.opener.document.location;
    window.close();
}
</script>
</head>

<body>
	<div id="container">
    	<div id="header">
            <h2>BAW - Billing and Accounting Web</h2>
    
        <div id="wrapper">
        <div id="content">
            <div id="box">
                <fieldset id="searchresult">
                    <legend></legend>
                    <form runat="server" id="form" method="post" name="frmCompletar">
                    <table width="400" align="center">
	                    <thead>
	                        <tr><th colspan="2">Salir del sistema</th></tr>
	                    </thead>
	                    <tbody>
	                        <tr>
                                <td align="center">
                                    <asp:Label ID="lbUsuario" runat="server">Su session a sido cerrada con exito</asp:Label></td>
                            </tr>
                            <tr><td>
                                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Aceptar" />
                                </td></tr>
	                    </tbody>
	                </table>
	                </form>
                </fieldset>    
            </div>
      </div>
      </div>
        <div id="footer">
        <div id="credits">
   		Elaborado por AIMAR, S.A.
        </div>
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
</body>
</html>