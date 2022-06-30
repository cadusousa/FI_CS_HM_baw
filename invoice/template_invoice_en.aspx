<%@ Page Language="C#" AutoEventWireup="true" CodeFile="template_invoice_en.aspx.cs" Inherits="invoice_template_invoice_en" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            height: 77px;
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
        .style8
        {
            font-size: small;
            font-weight: bold;
            width: 142px;
        }
        .style9
        {
            width: 142px;
        }
        .style10
        {
            font-size: small;
            font-weight: bold;
            width: 142px;
            height: 16px;
        }
        .style11
        {
            height: 16px;
        }
        .style12
        {
            height: 13px;
        }
        .style14
        {
            font-size: small;
            font-weight: bold;
            width: 176px;
        }
        .style15
        {
            width: 176px;
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
                                    <td rowspan="2" width="400px">
                                
                                    <h1>
                                        <asp:Label ID="lb_nombre_agente" runat="server" Text="" Visible="False"></asp:Label>
                                    </h1>
                                    </td>
                                    <td align="left">
                                            
                                    </td>
                                    <td align="right">
                                        <asp:Image ID="Image1" runat="server" Height="50px" ImageUrl="~/img/aimar.jpg" Width="150px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td width="200px" colspan="3">
                                        <asp:Label ID="lbl_direccion_empresa" runat="server" style="font-size: small"></asp:Label>
                                    </td>
                                </tr>
                               <tr>
                                    <td width="200px" colspan="2" class="style12">
                                        <asp:Label ID="lbl_email_empresa" runat="server" style="font-size: small"></asp:Label>
                                    </td>
                                    <td class="style12" align="right">
                                    <asp:Label ID="lbl_tipo_transaccion" runat="server" 
                                            style="font-size: medium; font-weight: 700; font-style: italic"></asp:Label>
                                        </td>
                                </tr>
                                 <tr>
                                 <td width="200px" colspan="2" class="style12">
                                        <asp:Label ID="lb_cedula_juridica" runat="server" style="font-size: x-small" 
                                            Visible="False"></asp:Label> <asp:Label ID="lbl_nit_CR" 
                                            runat="server" Visible="False"></asp:Label>

                                    </td>
                                    <td class="style12">
                                        </td>

                                </tr>
                                   <tr>
                                    <td width="200px" colspan="2">
                                        <asp:Label ID="lbl_direccion_empresa2" runat="server" style="font-size: x-small"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                
                                  <tr>
                                 <td width="200px" colspan="2" class="style12">
                                        <asp:Label ID="lbl_ruc" runat="server" style="font-size: x-small" 
                                            Visible="False"></asp:Label>

                                    </td>
                                    <td class="style12">
                                        </td>

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
                        <td align="right" 
                            style="font-size: medium; font-weight: 700; font-style: italic">
                                        Original</td>
                    </tr>
                    <tr>
                        <td align="right" 
                            style="font-size: medium; font-weight: 700; font-style: italic">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label 
                                            ID="Label1" runat="server" CssClass="style7" Text="No. "></asp:Label>
&nbsp;<asp:Label ID="lbl_serie" runat="server" CssClass="style7"></asp:Label><asp:Label ID="lbl_correlativo" 
                                            runat="server" CssClass="style7"></asp:Label>
                                    </td>
                    </tr>
                    <tr>
                        <td align="left" style="font-size: small; font-weight: 700">
                            <asp:Label ID="lbl_cai" runat="server" Text=""></asp:Label>&nbsp;
                        </td>
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
                                        <asp:Label ID="lb_codigo" runat="server" Text="Codigo"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl_codigo_cliente" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100px" class="style5">
                                       <asp:Label ID="lb_nombre" runat="server" Text="Nombre"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl_nombre_cliente" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100px" class="style5">
                                       <asp:Label ID="lb_nit" runat="server" Text="Nit"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl_nit" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100px" class="style5">
                                        <asp:Label ID="lb_razon" runat="server" Text="Razon"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl_razon" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100px" class="style5"><asp:Label ID="lb_direccion" runat="server" Text="Direccion"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl_direccion" runat="server"></asp:Label>
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
                        <td align="right" 
                            style="font-weight: 700; font-size: small; font-style: italic">
                                                    <span class="style6"><asp:Label ID="lb_cifras" runat="server" Text="Cifras en"></asp:Label>&nbsp;&nbsp;</span><asp:Label ID="lbl_moneda" runat="server"></asp:Label>
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
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" class="style3">
                                <tr>
                                    <td width="400px"  valign="top">
                                    <% if ((user.PaisID == 3 || user.PaisID == 23) && (user.SucursalID == 102 || user.SucursalID == 104) && (user.contaID == 1))
                                       { %>
                                    <table align="center" cellpadding="0" cellspacing="0" class="style4">
                                            <tr>
                                                <td class="style8"><asp:Label ID="Label3" runat="server" Text="Tipo de Cambio"></asp:Label></td>
                                                <td align="right"><asp:Label ID="lbl_tipo_cambio_hn" runat="server" CssClass="style6"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="style8">
                                                    <asp:Label ID="Label2" runat="server" Text="Subtotal Reembolso / Terceros"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"  class="style8"><asp:Label ID="Label4" runat="server" Text="Subtotal Cargos Locales"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="style8"><asp:Label ID="Label6" runat="server" Text="I.S.V. de Cargos Locales"></asp:Label></td>
                                            </tr>
                                          <tr>
                                                <td colspan="2" class="style8"><asp:Label ID="Label8" runat="server" Text="Total a Pagar"></asp:Label></td>
                                            </tr>
                                        </table>
                                    
                                    <% }
                                       else
                                       {%>
                                    &nbsp;
                                    <%} %>
                                    </td>
                                    <td width="300px">
                                        <% if ((user.PaisID == 3 || user.PaisID == 23) && (user.SucursalID == 102 || user.SucursalID == 104) && (user.contaID == 1))
                                           { %>
                                        <table align="center" cellpadding="0" cellspacing="0" class="style4">
                                            <tr>
                                                
                                                <td width="60%" align="right" class="style14"><%= moneda_equivalente %></td>
                                                <td width="40%" align="right" class="style8"><%= moneda %></td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style15">
                                                    <asp:Label ID="lbl_terceros_usd_hn" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                                <td colspan="2" width="100px" align="right">
                                                    <asp:Label ID="lbl_terceros" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style15">
                                                    <asp:Label ID="lbl_locales_usd_hn" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                                <td colspan="2" width="100px" align="right">
                                                    <asp:Label ID="lbl_locales" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style15">
                                                    <asp:Label ID="lbl_impuesto_locales_usd_hn" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                                <td colspan="2" width="100px" align="right">
                                                    <asp:Label ID="lbl_impuesto_locales" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style15">
                                                    <asp:Label ID="lbl_total_usd_hn" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                                <td width="100px" align="right">
                                                    <asp:Label ID="lbl_total_pagar" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                            </tr>
                                          </table>
                                        <%}
                                           else
                                           { %>
                                        <table align="center" cellpadding="0" cellspacing="0" class="style4">
                                            <tr>
                                                <td class="style8">
                                                   <asp:Label ID="lb_subtotal" runat="server" Text="Subtotal"></asp:Label></td>
                                                <td width="100px" align="right">
                                                    <asp:Label ID="lbl_subtotal" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style8">
                                                    <asp:Label ID="lb_impuesto" runat="server" Text="Impuesto"></asp:Label></td>
                                                <td align="right">
                                                    <asp:Label ID="lbl_impuesto" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style10">
                                                    <asp:Label ID="lb_total" runat="server" Text="Total"></asp:Label></td>
                                                <td align="right" class="style11">
                                                    <asp:Label ID="lbl_total" runat="server" CssClass="style6"></asp:Label>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td class="style10">
                                                    <asp:Label ID="lbl_Titulo_ret" runat="server" Text="Retencion" Visible="False"></asp:Label></td>
                                                <td align="right" class="style11">
                                                    <asp:Label ID="lbl_retencion" runat="server" CssClass="style6" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td class="style8">
                                                    <asp:Label ID="lb_anticipos" runat="server" Text="Anticipos" Visible="False"></asp:Label>
                                                  </td>
                                                <td align="right">
                                                    <asp:Label ID="lbl_anticipos" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td class="style8">
                                                    <asp:Label ID="lb_saldo_pagar" runat="server" Text="Saldo a Pagar" 
                                                        Visible="False"></asp:Label>
                                                  </td>
                                                <td align="right">
                                                    <asp:Label ID="lbl_saldo_pagar" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="style6">
                                                <td class="style9">
                                                    <asp:Label ID="lbl_tc" runat="server" Text="TC: " style="font-weight: 700" 
                                                        Visible="False"></asp:Label>
                                                    <asp:Label ID="lbl_equivalente" runat="server" Text="Equivalente" 
                                                        style="font-weight: 700"></asp:Label></td>
                                                <td align="right">
                                                    <asp:Label ID="lbl_total_equivalente" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                     <% } %>

                                    </td>
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
                     <td align="center"><%= factura_beneficio %></td>
                    </tr>
                    <tr>
                        <td align="center"><%= fecha_emision %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%= rango_autorizado %></td>
                    </tr>
                    <tr>
                        <td height="50" valign="bottom">
                            <asp:Label ID="lbl_formas_pago" runat="server" 
                                style="font-size: small; font-weight: 700" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                     <td align="justify"><%= texto_recordatorio_pago %></td>
                    </tr>
                    <tr>
                     <td align="justify"><%= texto_reclamo_documento %></td>
                    </tr>
                    <tr>
                        <td align="right">
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
                            <asp:Label ID="lb_observaciones_hn" runat="server" Text="Label" Visible="False"></asp:Label>
                        </td>
                        
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</form>
</body>
</html>