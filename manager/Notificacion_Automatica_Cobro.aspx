<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notificacion_Automatica_Cobro.aspx.cs" Inherits="manager_Notificacion_Automatica_Cobro" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 65%;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">

    <table align="center" cellpadding="0" cellspacing="0" class="style1">
        <tr>
            <td align="center">
                <img src="../img/aimar.jpg" alt="" />
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" 
                    style="font-family:Verdana;font-size:12pt;" width="600px">
                    <tr>
                        <td>
                            Estimado Intercompany, <b>NOMBRE DEL INTERCOMPANY DESTINO (Nit. 8230749)</b></td>
                    </tr>
                    <tr>
                        <td style="border-bottom:1px dotted #2525E2;padding-bottom:20px;">
                            <br />
                            <b>INTERCOMPANY ORIGEN (Nit. 12105562) </b>Ha emitido para usted un 
                            documento de cobro. A continuación podrá encontrar mas 
                            información.</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" 
                    style="font-family:Verdana;font-size:12pt;" width="600px">
                    <tr>
                        <td>
                            <b>Información de Cobro</b></td>
                    </tr>
                    <tr>
                        <td style="border-bottom:1px dotted #2525E2;padding-bottom:20px;">
                            <br />
                            <ul>
                                <li>Fecha:</li>
                                <li>Documento:</li>
                                <li>Serie y número:</li>
                                <li>Monto:</li>
                                <li>Master:</li>
                                <li>House:</li>
                                <li>Observaciones:</li>
                            </ul>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
                <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" />
            </td>
        </tr>
    </table>

    </form>

</body>
</html>
