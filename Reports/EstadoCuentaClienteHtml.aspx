<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EstadoCuentaClienteHtml.aspx.cs" Inherits="Reports_EstadoCuentaClienteHtml" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="10" cellspacing="0" class="style3" style="font-family:Arial">
            <tr>
                <td width="400px">
                    <asp:Image ID="Image1" runat="server" Height="67px" ImageUrl="~/img/aimar.jpg" Width="200px"  />
                </td>
                <td align="center">
                    <h1 style="font-family:Arial;display:none">ESTADO DE CUENTA</h1>   
                    <h1><asp:Label ID="Label1" runat="server" Text=""></asp:Label></h1>
                </td>
                <td align="left">
                </td>
            </tr>
            <tr>
                <td colspan="3"><b>Cliente:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_codigo_cliente" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td colspan="3"><b>Nombre:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_nombre_cliente" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td>Impresion de saldo pendiente de pago</td>
                <td>
                    <b>Desde:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_fecha_desde" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <b>Hasta:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_fecha_hasta" runat="server" Text=""></asp:Label> 
                </td>
                <td><b>Valores en:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_moneda" runat="server" Text=""></asp:Label></td>
            </tr>
        </table>
        <table cellpadding="5" cellspacing="0" class="style3" style="font-family:Arial; font-size:small">
            <tr style="background-color:Black; color:White">
                <td>TIPO</td>
                <td>SERIE</td>
                <td>CORR.</td>
                <td>FECHA</td>
                <td>VENCE</td>
                <td>TOTAL</td>
                <td>SERIE</td>
                <td>CORR.</td>
                <td>FECHA</td>
                <td>ABONO</td>
                <td>SALDO</td>
                <td>CONTENEDOR</td>
                <td>HBL</td>
                <td>SERVICIO</td>
                <td>POLIZA</td>
                <td>USUARIO</td>
            </tr>
            <%= html_facturas %>
            <%= html_notas_debito %>
            <%= html_recibos %>
            <tr>
                <td colspan="16">&nbsp;</td>
            </tr>
            <tr>
            <td colspan="13"></td>
            <td colspan="3" style="border:solid;border-width:2px; border-color:Black; font-size:x-large"><b>SALDO&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_total_saldo_cliente" runat="server" Text=""></asp:Label></b></td>
            </tr>
        </table>
    </form>
</body>
</html>

