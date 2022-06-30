<%@ Page Language="C#" AutoEventWireup="true" CodeFile="template_recibo.aspx.cs" Inherits="invoice_template_recibo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript">
        function refresh() {
            top.opener.document.location = top.opener.document.location;
            window.close();
        }
    </script>
    <title>BAW</title>
    <style type="text/css">
        .style1
        {
            width: 700px;
            border: 0px solid #000000;
            font-family: Arial, Helvetica, sans-serif;
            font-size: x-small;
        }
        .style2
        {
            width: 90%;
        }
        .style3
        {
            width: 100%;
        }
        .style4
        {
            width: 100%;
            border-bottom:3px solid #000000;
            border-top:3px solid #000000;
            font-size: small;
        }
        .style5
        {
            font-size: small;
            font-weight: bold;
        }
        .style6
        {
            font-size: small;
        }
        .style7
        {
            color: #FF0000;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <table align="center" cellpadding="0" cellspacing="1" class="style1">
        <tr>
            <td>
                <table align="center" cellpadding="0" cellspacing="0" class="style2">
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center" cellpadding="0" cellspacing="0" class="style3">
                                <tr>
                                    <td rowspan="2" width="200px">
                                        <asp:Image ID="Image1" runat="server" Height="100px" ImageUrl="~/img/aimar.jpg" Width="300px" />
                                        
                                    </td>
                                    <td align="right">
                                        &nbsp;</td>
                                    <td align="right">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:Label ID="lbl_tipo_transaccion" runat="server" 
                                            style="font-size: medium; font-weight: 700; font-style: italic"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="200px" colspan="3">
                                        <asp:Label ID="lbl_direccion_empresa" runat="server" style="font-size: small"></asp:Label>
                                    </td>
                                </tr>
                               
                                 <tr>
                                 <td width="200px" colspan="2">
                                        <asp:Label ID="lb_cedula_juridica" runat="server" style="font-size: small" 
                                            Visible="False"></asp:Label> <asp:Label ID="lbl_nit_CR" 
                                            runat="server" Visible="False"></asp:Label>

                                    </td>
                                    <td>
                                        &nbsp;</td>

                                </tr>
                                   <tr>
                                    <td width="200px" colspan="2">
                                        <asp:Label ID="lbl_direccion_empresa2" runat="server" style="font-size: small"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                  <tr>
                                 <td width="200px" colspan="2">
                                        <asp:Label ID="lbl_ruc" runat="server" style="font-size: small" 
                                            Visible="False"></asp:Label>

                                    </td>
                                    <td>
                                        &nbsp;</td>

                                </tr>
                                <tr>
                                    <td width="200px" align="center" style="width: 0">
                                        <asp:Label ID="lbl_nit_empresa" runat="server" 
                                            style="font-size: small; font-weight: 700"></asp:Label>
                                    </td>
                                    <td width="200px" style="width: 100px">
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left"><asp:Label ID="lbl_tc" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                       <td>
                        <table width="100%"><tr>                    
                        <td class="style5"></td>
                        <td></td>
                        <td align="right" style="font-size: medium; font-weight: 700; font-style: italic">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" CssClass="style7" Text="No. "></asp:Label>&nbsp;<asp:Label ID="lbl_serie" runat="server" CssClass="style7"></asp:Label>
                                        &nbsp;<span class="style7">-</span> <asp:Label ID="lbl_correlativo" runat="server" CssClass="style7"></asp:Label>
                        </td>
                        </tr></table>
                      </td>
                    </tr>
                    <tr>
                        <td align="left" 
                            style="font-size: medium; font-weight: 700; font-style: italic">
                                    &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center" cellpadding="0" cellspacing="0" class="style4">
                                <tr>
                                    <td width="130px" class="style5">
                                        <asp:Label ID="lb_fecha_emision" runat="server" Text="Fecha de Emision"></asp:Label>
                                        </td>
                                    <td>
                                        <table align="center" cellpadding="0" cellspacing="0" class="style3">
                                            <tr>
                                                <td width="100px">
                                                    <asp:Label ID="lbl_fecha_emision" runat="server"></asp:Label>
                                                </td>
                                                <td width="150px" align="right" style="width: 250px">
                                                    <asp:Label ID="lbl_vencimiento" runat="server" 
                                                        style="font-size: small; font-weight: 700"></asp:Label>
                                                </td>
                                                <td width="100px" align="center">
                                                    </span>
                                                    <asp:Label ID="lbl_fecha_vencimiento" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100px" class="style5">
                                       <asp:Label ID="lb_nombre" runat="server" Text="Nombre"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl_nombre_cliente" runat="server"></asp:Label>
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
                        <td>
                            &nbsp;</td>
                    </tr>
                    <%= html_observaciones %>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_tipo_operacion" runat="server" 
                                style="font-weight: 700; font-style: italic; font-size: small"></asp:Label>
                        </td>
                    </tr>
                    <%= html_factura_NC %><%= html_bl %>
                    <tr>
                        <td align="right" 
                            style="font-weight: 700; font-size: small; font-style: italic">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                          <table width="100%">
                          <tr>
                              <td align=left style="font-weight: 700; font-size: small; font-style: italic">
                                 <span class="style6"><asp:Label ID="lbl_pagos_recibidos" runat="server" Text="Pagos recibidos"></asp:Label></span>
                              </td>                          
                              <td align=right style="font-weight: 700; font-size: small; font-style: italic">   
                                 <span class="style6"><asp:Label ID="lb_cifras" runat="server" Text="Cifras en"></asp:Label>&nbsp;&nbsp;</span><asp:Label ID="lbl_moneda" runat="server"></asp:Label>
                              </td>
                          </tr>
                          </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center" cellpadding="0" cellspacing="0" class="style4">
                                <tr>
                                    <td>
                                        <table align="center" cellpadding="0" cellspacing="0" class="style3">
                                            <tr>
                                                <td colspan="2">
                                                    <%= html_Rubros %>    
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!--aqui va todo el detalle de facturas y notas de debito-->
                    <tr>
                        <td>
                          <table width="100%">
                          <tr>
                              <td align=left style="font-weight: 700; font-size: small; font-style: italic">
                                  <span class="style6"><asp:Label ID="lbl_detalle_pagos" runat="server" Text="Detalle de pagos"></asp:Label></span>
                              </td>                          
                              <td align=right style="font-weight: 700; font-size: small; font-style: italic">   
                                 <span class="style6"><asp:Label ID="Label2" runat="server" Text=""></asp:Label>&nbsp;&nbsp;</span><asp:Label ID="Label3" runat="server"></asp:Label>
                              </td>
                          </tr>
                          </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center" cellpadding="0" cellspacing="0" class="style4">
                                <tr>
                                    <td>
                                        <table align="center" cellpadding="0" cellspacing="0" class="style3">
                                            
                                                    <%= listado_facturas%>    
                                                
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!--Fin de detalle de facturas y notas de debito-->
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">
                        <table width="40%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="style5" style="border-top:3px solid #000000"><asp:Label ID="lbl_total_recibo" runat="server" Text="Total Recibo:"></asp:Label></td><td align="right" style="border-top:3px solid #000000"><asp:Label ID="lbl_total" runat="server" CssClass="style6"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="style5"><asp:Label ID="lbl_total_pagado" runat="server" Text="Total Pagado:"></asp:Label></td><td align="right" class="style6"><%=totalpagado.ToString()%></td>
                            </tr>
                            <tr>
                                <td class="style5" style="border-bottom:3px solid #000000"><asp:Label ID="lbl_saldo_favor" runat="server" Text="Saldo a favor:"></asp:Label></td><td align="right" class="style6" style="border-bottom:3px solid #000000"><%=saldo.ToString()%></td>
                            </tr>
                          </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" valign="bottom" align="right">
                            <asp:Label ID="lbl_total_letras" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="50" valign="bottom">
                            <asp:Label ID="lbl_formas_pago" runat="server" 
                                style="font-size: small; font-weight: 700" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        <b>Emitido por:</b>&nbsp;
                        <asp:Label ID="lbl_usuario_crea" runat="server" Text=""></asp:Label>&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_firma_comentario" runat="server" 
                                style="font-size: small; font-weight: 700" Visible="False"></asp:Label>
                            <asp:Label ID="lbl_firma_digital" runat="server" Visible="False" 
                                style="font-size: small"></asp:Label>
                        </td>
                        
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lb_observaciones_hn" runat="server" Text="" Visible="False"></asp:Label>
                        </td>
                        
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
