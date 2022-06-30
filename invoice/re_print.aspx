<%@ Page Language="C#" AutoEventWireup="true" CodeFile="re_print.aspx.cs" Inherits="invoice_re_print"  Title="AIMAR - BAW"%>

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

<body onLoad="JavaScript:self.focus()">
    <div id="content">
        <form runat="server" id="form" method="post" name="frmCompletar">
        <table align="left">
            <thead>
                <tr><th>Re-Impresion de Documentos</th></tr>
            </thead>
            <tbody>
                <tr><td>&nbsp;
                    <asp:Label ID="Label2" runat="server" Text="Serie: "></asp:Label>
                    <asp:Label ID="lb_serie" runat="server"></asp:Label>
                    <br />
                    &nbsp;
                    <asp:Label ID="Label3" runat="server" Text="Correlativo: "></asp:Label>
                    <asp:Label ID="lb_correlativo" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="Factura ID:" Visible="False"></asp:Label>
                    <asp:TextBox ID="tb_factID" runat="server" Visible="False"></asp:TextBox>
                        <asp:Label ID="lb_tipo" runat="server" Text="0" Visible="False"></asp:Label>
                        <br />
                        </td></tr>
                <tr>
                    <td>Motivo:<br />
                        <asp:TextBox ID="tb_motivo" runat="server" Height="180px" TextMode="MultiLine" 
                            Width="274px"></asp:TextBox>
                        <br />
                    
                    
                    </td>
                </tr> 
                <tr><td align="center">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="tb_motivo" ErrorMessage="RequiredFieldValidator" 
                        SetFocusOnError="True">Es necesario que ingrese el motivo de la reimpresion</asp:RequiredFieldValidator>
                    <br />
                    <asp:Button ID="btn_imprimir" runat="server" Text="Imprimir" 
                        onclick="btn_imprimir_Click" />

                    <asp:Button ID="btn_imprimirSV" runat="server" Text="Imprimir HTML" 
                        onclick="btn_imprimir_ClickSV" />


                </td></tr>       
            </tbody>
        </table>
        </form>
    </div>
</body>
</html>
