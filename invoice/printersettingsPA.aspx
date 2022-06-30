<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printersettingsPA.aspx.cs" Inherits="_Default" %>
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
        	<h2>AIMAR Administrator</h2>
    <div id="topmenu">
            	<ul>
                	<%--<li class="current"><a href="~/manager/manager.aspx">Administración</a></li>
                    <li><a href="~/invoice/manager.aspx">Perfiles</a></li>--%>
              </ul>
          </div>
      </div>
        <div id="top-panel">
            <!--- <div id="panel">
                <ul>
					<li><a href="adduser.aspx" class="useradd">Agregar usuario</a></li>                        
            		<li><a href="search.aspx" class="search">Buscar</a></li>
                </ul>
            </div> --->
      </div>
        <div id="wrapper">
        <div id="content">
            <div id="box">
                <fieldset id="searchresult">
                    <legend></legend>
                    <form runat="server" id="form" method="post" name="frmCompletar">
                    <table width="500" align="center">
	                    <thead>
	                        <tr><th colspan="2">Configurar&nbsp; Impresion</th></tr>
	                    </thead>
	                    <tbody>
	                        <tr>
                                <td align="right" colspan="2">
                                    &nbsp;</td>
                            </tr>
	                        <tr>
                                <td align="right">
                                    Impresora</td>
                                <td>
                                    &nbsp;
                                    <asp:DropDownList ID="lb_impresora" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                    <asp:Label ID="lb_serie" runat="server"></asp:Label>
                    <asp:Label ID="lb_correlativo" runat="server"></asp:Label>
                    <asp:Label ID="lb_tipo" runat="server" Visible="False"></asp:Label>
                    <asp:TextBox ID="tb_factID" runat="server" Visible="False"></asp:TextBox>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_print" runat="server" OnClick="btn_print_Click"
                                        Text="Imprimir" />&nbsp;
                                    <asp:Button ID="btn_reprint" runat="server" OnClick="btn_reprint_Click" Visible="false"
                                    Text="Re-Imprimir" />
                                    </td>
                            </tr>
	                    </tbody>
	                </table>
	                </form>
                </fieldset>    
            </div>            
      </div>
      </div>
        <div id="footer">
        <div id="credits">
   		Elaborado por MAAT Guatemala C.A.
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
        <p>
&nbsp;&nbsp;&nbsp;
        </p>
</body>
</html>