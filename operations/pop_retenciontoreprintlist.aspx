<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pop_retenciontoreprintlist.aspx.cs" Inherits="operations_pop_retenciontoprintlist"  Title="AIMAR - BAW"%>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>

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
<%--<script language="javascript" type="text/javascript">   
    function si()
    {
        
    }  
    </script>--%>
</head>

<body>
    <div id="content">
        <form runat="server" id="form" method="post" name="frmCompletar">
<%--        <asp:ScriptManager ID="ScriptManager1" runat="server" />--%>
        <table align="left">
            <thead>
                <tr><th>Impresión de retenciones</th></tr>
            </thead>
            <tbody>
                <tr><td>
                    <asp:GridView ID="gv_retenciones" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="Este grupo de provisiones no tiene ninguna retencion" Width="90%">
            <Columns>
                <asp:TemplateField HeaderText="Seleccionar">
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_seleccionar" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rerencion ID">
                    <ItemTemplate>
                        <asp:Label ID="lb_id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Serie provision">
                    <ItemTemplate>
                        <asp:Label ID="lb_serieprov" runat="server" Text='<%# Eval("serieprov") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Correlativo provisión">
                    <ItemTemplate>
                        <asp:Label ID="lb_correlativoprov" runat="server" Text='<%# Eval("correlativoprov") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre retención">
                    <ItemTemplate>
                        <asp:Label ID="lb_nombreretencion" runat="server" Text='<%# Eval("nombreretencion") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Valor retencion">
                    <ItemTemplate>
                        <asp:Label ID="lb_valor" runat="server" Text='<%# Eval("valor") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Moneda">
                    <ItemTemplate>
                        <asp:Label ID="lb_moneda" runat="server" Text='<%# Eval("moneda") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Retencion tipo">
                    <ItemTemplate>
                        <asp:Label ID="lb_rettipoid" runat="server" Text='<%# Eval("retenciontipoid") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ID provision">
                    <ItemTemplate>
                        <asp:Label ID="lb_provisionID" runat="server" Text='<%# Eval("provisionid") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
                </td></tr>
                
                <tr><td align="center">
                    Plantillas para utilizar:<%--<asp:TextBox ID="tb_comodin" runat="server"></asp:TextBox>--%>
                    <br />
                    <br />
                    <asp:RadioButtonList ID="rbl_optionretencion" runat="server">
                    </asp:RadioButtonList>
                </td></tr> 
                <tr><td align="center">
                    
                    <asp:Button ID="btn_imprimi" runat="server" onclick="Button1_Click" 
                        Text="Imprimir" />
                    
                </td></tr>
                
                
                
                <tr><td align="center">
                <%--********************************************************--%>
    <%--<asp:Panel ID="pnlConfirmacion" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            Confirmación de impresión</div></td></tr>
        <tr>
            <td align="left" colspan="2">¿Se imprimió correctamente el documento?</td>
            
        </tr>
        <tr>
            <td align="center"><asp:Button ID="btn_si" runat="server" Text="Si" /></td>
            <td align="center"><asp:Button ID="btn_no" runat="server" Text="No" 
                    onclick="btn_no_Click" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    
    <cc1:modalpopupextender ID="Confirmacion_ModalPopupExtender" runat="server" TargetControlID="tb_comodin"
            PopupControlID="pnlConfirmacion" CancelControlID="btn_si"
            OnCancelScript="si()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />--%>
    <%--********************************************************--%>    
                                        
                </td></tr>        
            </tbody>
        </table>
        </form>
    </div>
</body>
</html>

