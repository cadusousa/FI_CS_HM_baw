<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pop_transferencias.aspx.cs" Inherits="operations_pop_transferencias"  Title="AIMAR - BAW"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>AIMAR::::</title>
<link rel="stylesheet" type="text/css" href="../css/theme4.css" />
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script language="javascript">
   var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
   document.writeln('<link rel="stylesheet" type="text/css" href="../css/' + StyleFile + '">');
</script>
</head>

<body>
    <div id="content">
    <div id="box">
    <h3 id="adduser">CHEQUE VOUCHER</h3>
        <form runat="server" id="form" method="post" name="frmCompletar">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
        <table>
        <tr>
            <td>
                Transaccion:
                <asp:DropDownList ID="lb_transaccion" runat="server" Height="22px" 
                    Width="315px">
                </asp:DropDownList>
                <br />
                Banco:
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" 
                    onselectedindexchanged="lb_cuentas_bancarias_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;Cta Bancaria No.:
                <asp:TextBox ID="tb_cuenta_bancaria" runat="server" Height="16px" Width="128px"></asp:TextBox>
                <br />
                Cuenta:
                <asp:TextBox ID="tb_cuenta_contable" runat="server" Height="16px" Width="127px"></asp:TextBox>
                &nbsp;<asp:TextBox ID="tb_cuenta_nombre" runat="server" Height="16px" 
                    Width="313px"></asp:TextBox>
                &nbsp;Moneda:
                <asp:DropDownList ID="lb_moneda" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                
                Documento
                
                Número:                 <asp:TextBox ID="tb_chequeNo" 
                    runat="server" Height="16px" Width="129px"></asp:TextBox>
&nbsp; Fecha:
                <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="128px"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha">
                </cc1:CalendarExtender>
&nbsp; Valor:
                <asp:TextBox ID="tb_valor" runat="server" Height="16px" Width="128px"></asp:TextBox>
                <br />
                Motivo:<asp:TextBox ID="tb_motivo" runat="server" Height="16px" Width="484px"></asp:TextBox>
                
            </td>
        </tr>
        <tr>
            <td>
                <br />
                Banco:
                <asp:DropDownList ID="lb_cuentas_bancarias2" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" 
                    onselectedindexchanged="lb_cuentas_bancarias2_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;Cta Bancaria No.:
                <asp:TextBox ID="tb_cuenta_bancaria2" runat="server" Height="16px" Width="128px"></asp:TextBox>
                <br />
                Cuenta:
                <asp:TextBox ID="tb_cuenta_contable2" runat="server" Height="16px" Width="127px"></asp:TextBox>
                &nbsp;<asp:TextBox ID="tb_cuenta_nombre2" runat="server" Height="16px" 
                    Width="313px"></asp:TextBox>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="bt_guardar" runat="server" Text="Guardar" onclick="bt_guardar_Click" />
                <asp:Button ID="bt_cancelar" runat="server" Text="Cancelar" />
            </td>
        </tr>
        </table>
      </form>
    </div>
</body>
</html>
