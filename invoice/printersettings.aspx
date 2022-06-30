<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printersettings.aspx.cs" Inherits="_Default" %>
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
        	<h2>BAW - Billing and Accounting Web</h2>
    <div id="topmenu">
            	<ul>
                	<%--<li class="current"><a href="~/manager/manager.aspx">Administración</a></li>
                    <li><a href="~/invoice/manager.aspx">Perfiles</a></li>--%>
              </ul>
          </div>
      </div>
        <div id="top-panel" 
            style="font-weight: bold; font-style: italic; vertical-align: middle; text-align: right; color: #993400; font-size: 15px; text-transform: uppercase; text-decoration: underline overline;">
            <%
                UsuarioBean usuario = null;
                usuario = (UsuarioBean)Session["usuario"];
                lbl_nombre_sistema.Text = usuario.pais.Nombre_Sistema;
                %>
                <asp:Label ID="lbl_nombre_sistema" runat="server"></asp:Label>
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
                                    <asp:Button ID="btn_next" runat="server" OnClick="btn_next_Click"
                                        Text="Siguiente" />&nbsp;</td>
                            </tr>
	                    </tbody>
	                </table>
	                </form>
                </fieldset>    
            </div>
            <!---<div id="sidebar">
  				<ul>
  				    <li><h3><a href="#" class="user">Usuarios</a></h3>
          				<ul>
                            <li><a href="adduser.aspx" class="useradd">Agregar usuario</a></li>
            				<li><a href="searchuser.aspx" class="search">Buscar usuario</a></li>
                        </ul>
                    </li>
                    <li><h3><a href="#" class="manage_profile">Perfiles</a></h3>
          				<ul>
                            <li><a href="addprofile.aspx" class="addprofile">Crear perfil</a></li>
                            <li><a href="searchprofile.aspx" class="search">Buscar perfil</a></li>                            
                        </ul>
                    </li>
                    <li><h3><a href="#" class="house">Paises</a></h3>
                        <ul>
                        	<li><a href="addpais.aspx" class="addsucursal">Crear Pais</a></li>
                            <li><a href="searchpais.aspx" class="search">Buscar Pais</a></li>
                        </ul>
                    </li>
                    <li><h3><a href="#" class="house">Sucursales</a></h3>
                        <ul>
                        	<li><a href="addsuc.aspx" class="addsucursal">Crear sucursal</a></li>
                            <li><a href="searchsuc.aspx" class="search">Buscar sucursal</a></li>
                        </ul>
                    </li>
                  
				</ul>       
          </div>--->
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