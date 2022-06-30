<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pop_retenciontoprintlist.aspx.cs" Inherits="operations_pop_retenciontoprintlist"  Title="AIMAR - BAW"%>
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
                <asp:TemplateField HeaderText="Retencion ID">
                    <ItemTemplate>
                        <asp:Label ID="lb_retencionID" runat="server" Text='<%# Eval("retencionID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cheque ID">
                    <ItemTemplate>
                        <asp:Label ID="lb_chequeID" runat="server" Text='<%# Eval("chequeID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Corte ID">
                    <ItemTemplate>
                        <asp:Label ID="lb_corteID" runat="server" Text='<%# Eval("corteID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cheque">
                    <ItemTemplate>
                        <asp:Label ID="lb_cheque" runat="server" Text='<%# Eval("cheque") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Corte">
                    <ItemTemplate>
                        <asp:Label ID="lb_corte" runat="server" Text='<%# Eval("corte") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Provision">
                    <ItemTemplate>
                        <asp:Label ID="lb_provision" runat="server" Text='<%# Eval("provision") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Referencia">
                    <ItemTemplate>
                        <asp:Label ID="lb_referencia" runat="server" Text='<%# Eval("referencia") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Retencion">
                    <ItemTemplate>
                        <asp:Label ID="lb_retencion" runat="server" Text='<%# Eval("retencion") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Monto">
                    <ItemTemplate>
                        <asp:Label ID="lb_monto" runat="server" Text='<%# Eval("monto") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Moneda">
                <ItemTemplate>
                        <asp:Label ID="lb_moneda" runat="server" Text='<%# Eval("moneda") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Proveedor ID">
                <ItemTemplate>
                        <asp:Label ID="lb_proveedorID" runat="server" Text='<%# Eval("proveedorID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Proveedor">
                <ItemTemplate>
                        <asp:Label ID="lb_proveedor" runat="server" Text='<%# Eval("proveedor") %>'></asp:Label>
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

