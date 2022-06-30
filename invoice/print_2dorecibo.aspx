<%@ Page Language="C#" AutoEventWireup="true" CodeFile="print_2dorecibo.aspx.cs" Inherits="invoice_print_2dorecibo"  Title="AIMAR - BAW"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table border="1" width="500">
    <tr><td colspan="2" align="center"><h1>DETALLE DE RECIBO</h1></td></tr>
    <tr>
        <td colspan="2">Nombre del cliente: <%=nombre %></td>
    </tr>
    <tr>
        <td colspan="2">Numero de recibo :<%=recibo %></td>
    </tr>
    <tr>
        <td colspan="2">Fecha de impresion: <%=DateTime.Now.ToShortDateString() %></td>
    </tr>
    <tr>
        <td colspan="2" align="center">DOCUMENTOS PAGADOS CON ESTE RECIBO</td>
    </tr>
    <tr><td colspan="2">
    <table width="500" border="1">
       <%=listado_facturas %>
    </table>
    </td></tr>
    <tr>
        <td>Total Recibo</td><td align="right" width="100"><%=totalrecibo.ToString() %></td>
    </tr>
    <tr>
        <td>Total Pagado</td><td align="right" width="100"><%=totalpagado.ToString()%></td>
    </tr>
    <tr>
        <td>Saldo a favor del cliente</td><td align="right" width="100"><%=saldo.ToString()%></td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
