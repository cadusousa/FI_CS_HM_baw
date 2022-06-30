<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addplantilla.aspx.cs" Inherits="manager_addplantilla"  Title="AIMAR - BAW"%>

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
                <tr><th colspan="2">Creación de un campo para la factura</th></tr>
            </thead>
            <tbody>
                <tr>
                    <td align="right">Campo: </td>
                    <td>
                        <asp:TextBox ID="tb_campo" runat="server"></asp:TextBox>
                        <asp:Label ID="lb_fac_id" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lb_suc_id" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">Posición X: </td>
                    <td>
                        <asp:TextBox ID="tb_x" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">Posición Y: </td>
                    <td>
                        <asp:TextBox ID="tb_y" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2"><b><asp:Label ID="lb_msg" runat="server" Visible="true"></asp:Label></b></td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="cmdLogin" runat="server" OnClick="cmdLogin_Click" Text="Guardar" />
                        &nbsp;&nbsp;<input id="Cancel" type="button" value="Cancelar" onclick="javascript:window.close();" />
                        
                    </td>
                </tr>
            </tbody>
        </table>
        </form>
    </div>
</body>
</html>
