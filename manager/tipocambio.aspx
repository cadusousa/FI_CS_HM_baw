<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tipocambio.aspx.cs" Inherits="manager_tipocambio"  Title="AIMAR - BAW"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>AIMAR::::</title>
<link rel="stylesheet" type="text/css" href="../css/theme4.css" />
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script language="javascript">
   var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
   document.writeln('<link rel="stylesheet" type="text/css" href="../css/' + StyleFile + '">');
</script>
</head>

<body>
    <div id="content">
        <form runat="server" id="form" method="post" name="frmCompletar">
        <table width="350" align="left">
            <thead>
                <tr><th colspan="2">Configuración de tipo de cambio</th></tr>
            </thead>
            <tbody>

                <tr>
                    <td align="right">Pais: </td>
                    <td>
                    <asp:TextBox ID="tb_id" runat="server" Enabled="False" Visible="false"></asp:TextBox>
                     <asp:TextBox ID="TextBox1" runat="server" Enabled="False" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="tb_pais" runat="server" Enabled="False"></asp:TextBox>
                        <asp:Label ID="lb_pai_id" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">Tipo de cambio: </td>
                    <td>  
                        <asp:TextBox ID="tb_tipo_cambio" runat="server"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td align="center" colspan="2"><b><asp:Label ID="lb_msg" runat="server" Visible="true"></asp:Label></b></td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="cmdLogin" runat="server" OnClick="cmdLogin_Click" Text="Guardar" />
                        &nbsp;&nbsp;
                        <input id="Cancel" type="button" value="Cancelar" onclick="javascript:window.close();" />
                    </td>
                </tr>
            </tbody>
        </table>
        </form>
    </div>
</body>
</html>