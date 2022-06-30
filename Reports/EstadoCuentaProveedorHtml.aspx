<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EstadoCuentaProveedorHtml.aspx.cs" Inherits="Reports_EstadoCuentaProveedorHtml" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="10" cellspacing="0" class="style3" style="font-family:Arial">
            <tr>
                <td width="400px">
                    <asp:Image ID="Image1" runat="server" Height="67px" ImageUrl="~/img/aimar.jpg" Width="200px"/>
                    
                </td>
                <td align="center">
                 <h1 style="font-family:Arial;display:none">ESTADO DE CUENTA</h1>   
                 <h1><asp:Label ID="Label1" runat="server" Text=""></asp:Label></h1>
                </td>
                <td align="left">
                </td>
            </tr>
            <tr>
                <td colspan="3"><b><asp:Label ID="lbl_tipo_persona" runat="server" Text=""></asp:Label>:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_codigo_cliente" runat="server" Text=""></asp:Label></td>
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
                <td>TOTAL</td>
                <td>SERIE</td>
                <td>CORR.</td>
                <td>FECHA</td>
                <td>VALOR</td>
                <td>ABONO</td>
                <td>SALDO</td>
                <td>CONTENEDOR</td>
                <td>HBL</td>
                <td>SERVICIO</td>
                <td>POLIZA</td>
                <td>USUARIO</td>
            </tr>
            <%= html_cortes %>
            <%= html_sincorte %>
            <tr>
                <td colspan="16">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4" align="center" style="border:solid;border-width:2px; border-color:Black; font-size:large; background-color:Black; color:White"><b>Total en Cortes</b></td>
                <td colspan="3" align="center" style="border:solid;border-width:2px; border-color:Black; font-size:large; background-color:Black; color:White"><b>Abonos</b></td>
                <td colspan="3" align="center" style="border:solid;border-width:2px; border-color:Black; font-size:large; background-color:Black; color:White"><b>Total sin Cortes</b></td>
                <td colspan="6">&nbsp;</td>
            </tr>
            <tr>
            <td align="right" colspan="4" style="border:solid;border-width:2px; border-color:Black; font-size:large"><b><asp:Label ID="resumen_cortes" runat="server" Text=""></asp:Label></b></td>
            <td align="right" colspan="3" style="border:solid;border-width:2px; border-color:Black; font-size:large"><b><asp:Label ID="resumen_abonos" runat="server" Text=""></asp:Label></b></td>
            <td align="right" colspan="3" style="border:solid;border-width:2px; border-color:Black; font-size:large"><b><asp:Label ID="resumen_sincortes" runat="server" Text=""></asp:Label></b></td>
            <td colspan="3"></td>
            <td colspan="3" style="border:solid;border-width:2px; border-color:Black; font-size:x-large"><b>SALDO&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_total_saldo_proveedor" runat="server" Text=""></asp:Label></b></td>
            </tr>
        </table>
    </form>
</body>
</html>
