<%@ Page Language="C#" AutoEventWireup="true" CodeFile="template_retencion.aspx.cs" Inherits="invoice_template_retencion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Orden de Compra</title>
    <script type="text/javascript">
        function refresh() {
            top.opener.document.location = top.opener.document.location;
            window.close();
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 641px;
        }
        .style2
        {
            width: 98%;
            border-bottom:3px solid #000000;
            border-top:3px solid #000000;
        }
        .style3
        {
            width: 100%;
        }
        .style4
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: small;
        }
        .style5
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: small;
            font-weight: bold;
        }
        .style6
        {
            width: 98%;
        }
        .style7
        {
            color: #FF0000;
            font-family: Arial, Helvetica, sans-serif;
            font-size: large;
            font-weight: bold;
        }
        .style8
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: small;
        }
        .style10
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: small;
            width: 169px;
        }
        
        .style11
        {
            width: 268px;
        }
        
        .style12
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: small;
            height: 19px;
        }
        .style13
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: small;
            font-weight: bold;
            height: 19px;
        }
        .style14
        {
            height: 19px;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table align="center" cellpadding="0" cellspacing="0" class="style1">
        <tr>
            <td colspan="3" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%">
            <div style="width:100%;">
                    <table align="center" cellpadding="0" cellspacing="0" class="style6">
                        <tr>
                            <td width="160px" class="style8" rowspan="3" style="width: 310px">
                                <asp:Image ID="Image1" runat="server" Height="100px" ImageUrl="~/img/aimar.jpg" Width="300px" />
                                
                            </td>
                            <td class="style8" width="200px">
                                &nbsp;</td>
                            <td class="style5">
                                &nbsp;</td>
                            <td class="style8">
                                &nbsp;</td>
                            <td class="style8">
                                &nbsp;</td>
                            <td width="300px" class="style5" style="font-style: italic">
                                <asp:Label ID="lbl_tipo_documento" runat="server" Text=""></asp:Label></td>
                            <td width="300px" class="style7" style="font-style: italic">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style8">
                                &nbsp;</td>
                            <td width="100px" class="style8">
                                &nbsp;</td>
                            <td width="200px" class="style8">
                                &nbsp;</td>
                            <td class="style8" width="50px">
                                &nbsp;</td>
                            <td width="100px" class="style7">
                                <asp:Label ID="lbl_serie" runat="server"></asp:Label>
                            </td>
                            <td width="200px" class="style8">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style8">
                                &nbsp;</td>
                            <td width="100px" class="style8">
                                &nbsp;</td>
                            <td width="200px" class="style8">
                                &nbsp;</td>
                            <td class="style8" width="50px">
                                &nbsp;</td>
                            <td width="100px" class="style8">
                                &nbsp;</td>
                            <td width="200px" class="style8">
                                &nbsp;</td>
                        </tr>
                    </table>
                </div>
            </td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%">
            </td>
            <td width="90%" class="style8">
            <!--Aqui es donde tengo que poner los datos-->
            <%= html_datos %>
            </td>
            <td width="5%">
            </td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%" class="style8">
                <b><%= html_cai_honduras %></b>
            </td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%">
            <div style="width:100%;">
                    <table align="center" cellpadding="0" cellspacing="0" class="style2">
                        <tr>
                            <td width="160px" class="style5">
                                Fecha de Emision</td>
                            <td width="800px" class="style8">
                                <asp:Label ID="lbl_fecha_emision" runat="server"></asp:Label>
                            </td>
                            <td class="style8">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td width="160px" class="style5">
                                Proveedor</td>
                            <td width="800px" class="style8">
                                <asp:Label ID="lbl_proveedor" runat="server"></asp:Label>
                            </td>
                            <td class="style8">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td width="160px" class="style5">
                                Direccion</td>
                            <td width="800px" class="style8">
                                <asp:Label ID="lbl_direccion" runat="server"></asp:Label>
                            </td>
                            <td class="style8">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td width="160px" class="style5">
                                <asp:Label ID="lbl_documento_asociado" runat="server" Text=""></asp:Label></td>
                            <td width="800px" class="style8">
                                <asp:Label ID="lbl_numero_cotizacion" runat="server"></asp:Label>
                            </td>
                            <td class="style8">
                                &nbsp;</td>
                        </tr>
                    </table>
                </div>
            </td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%" class="style8">
                &nbsp;</td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%" class="style8">
                <%= html_descripcion_honduras %>   
            </td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%">
                <!--<table align="center" cellpadding="0" cellspacing="0" class="style10">
                    <tr>
                        <td class="style5" width="160px">
                            Departamento</td>
                        <td class="style8" width="230px">
                            <asp:Label ID="lbl_departamento" runat="server"></asp:Label>
                        </td>
                        <td class="style5" width="130px">
                            Solicitada Por</td>
                        <td class="style8" width="200px">
                            <asp:Label ID="lbl_solicitada_por" runat="server"></asp:Label>
                        </td>
                        <td class="style5" width="130px">
                            Autorizada Por</td>
                        <td class="style8" width="200px">
                            <asp:Label ID="lbl_autorizada_por" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>-->
            </td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%">
                    <table align="center" cellpadding="0" cellspacing="0" class="style2">
                        <tr>
                            <td class="style11">
                                TOTAL FACTURA</td>
                            <td class="style10"  align="right">
                                <asp:Label ID="lbl_total_factura_usd" runat="server" Text=""></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="lbl_total_factura" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style11">
                                VALOR SUJETO A RETENCION</td>
                            <td class="style10"  align="right">
                                <asp:Label ID="lbl_valor_sujeto_usd" runat="server" Text=""></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="lbl_valor_sujeto" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style11">
                                VALOR RETENIDO 12.5%</td>
                            <td class="style10" align="right">
                                <asp:Label ID="lbl_retenido_12_usd" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style8" align="right">
                                <asp:Label ID="lbl_retenido_12_hn" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style11">
                                VALOR RETENIDO 1%</td>
                            <td class="style10" align="right">
                                <asp:Label ID="lbl_retenido_1_usd" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style8" align="right">
                                <asp:Label ID="lbl_retenido_1_hn" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style11">
                                VALOR RETENIDO 25%</td>
                            <td class="style10" align="right">
                                <asp:Label ID="lbl_retenido_25_usd" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style8" align="right">
                                <asp:Label ID="lbl_retenido_25_hn" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style11">
                                VALOR NETO A RECIBIR</td>
                            <td class="style10" align="right">
                                <asp:Label ID="lbl_neto_recibir_usd" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style8" align="right">
                                <asp:Label ID="lbl_neto_recibir_hn" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%" class="style8">
                &nbsp;</td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%">
                    <!--<table align="center" cellpadding="0" cellspacing="0" class="style2">
                        <tr>
                            <td class="style8">
                                &nbsp;</td>
                            <td class="style8">
                                &nbsp;</td>
                            <td class="style5" width="100PX">
                                HBL</td>
                            <td class="style8" width="300px">
                                <asp:Label ID="lbl_hbl" runat="server"></asp:Label>
                            </td>
                            <td class="style5" width="100PX">
                                MBL</td>
                            <td class="style8" width="300px">
                                <asp:Label ID="lbl_mbl" runat="server"></asp:Label>
                            </td>
                            <td class="style8">
                                &nbsp;</td>
                            <td class="style8">
                                &nbsp;</td>
                        </tr>
                    </table>-->
                </td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%" class="style8">
                    Valor de lo retenido en letras dolares: <asp:Label ID="lbl_total_letras_usd" runat="server" Text=""></asp:Label></td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%" class="style8">
            <%= html_detalle_provision%>
                Valor de lo retenido en letras lempiras:  <asp:Label ID="lbl_total_letras" runat="server" Text=""></asp:Label></td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%" class="style8">
                    &nbsp;</td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td width="5%" class="style8" colspan="2">
            Rango de impresión: <b><asp:Label ID="lbl_rango_inicial" runat="server" Text=""></asp:Label></b> hasta <b><asp:Label ID="lbl_rango_final" runat="server" Text=""></asp:Label></b>
            </td>
        </tr>
        <tr>
            <td></td>
            <td width="5%" class="style8" colspan="2">
            Fecha limite de emisión: <asp:Label ID="lbl_fecha_limite_emision" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td width="5%" class="style8">
            </td>
            <td width="5%" class="style8">
            <%= html_pie_pagina %>
            </td>
            <td width="90%" class="style4">
                
            </td>
        </tr>
        <tr>
            <td width="5%" class="style8">
                &nbsp;</td>
            <td width="90%">
                </td>
            <td width="5%" class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3" class="style4">
                &nbsp;</td>
        </tr>
    </table>
</form>
</body>
</html>
