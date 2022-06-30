<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pop_cheques.aspx.cs" Inherits="operations_pop_cheques"  Title="AIMAR - BAW"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                <asp:Label ID="lb_total" runat="server" Text="0" Visible="False"></asp:Label>
                <asp:Label ID="lb_facturas" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lb_proveedorID" runat="server" Visible="False"></asp:Label>
                <br />
                Transaccion:
                <asp:DropDownList ID="lb_transaccion" runat="server" Height="22px" 
                    Width="315px">
                </asp:DropDownList>
                <br />
                Banco:
                <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;Cta Bancaria No.:
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" 
                    onselectedindexchanged="lb_cuentas_bancarias_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                Moneda:
                <asp:DropDownList ID="lb_moneda" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_moneda_SelectedIndexChanged" Enabled="False">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                
                Número:                 <asp:TextBox ID="tb_chequeNo" 
                    runat="server" Height="16px" Width="129px"></asp:TextBox>
&nbsp; Fecha:
                <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="128px"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha" Format="MM/dd/yyyy">
                </cc1:CalendarExtender>
&nbsp; Valor:
                <asp:TextBox ID="tb_valor" runat="server" Height="16px" Width="128px" 
                    ReadOnly="True"></asp:TextBox>
                <br />
                Acreditado:                 <asp:TextBox ID="tb_acreditado" runat="server" 
                    Height="16px" Width="457px"></asp:TextBox>
                <br />
                Motivo:<asp:TextBox ID="tb_motivo" runat="server" Height="16px" Width="484px"></asp:TextBox>
                
            </td>
        </tr>
        
        <tr><td align="center">
            <table width="650">
                <thead>
                <tr>
                    <th colspan="6">Detalle de cheque</th>
                </tr>
                </thead>
                <tr><td>
                
                    <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
                        Width="607px" onrowdatabound="gv_detalle_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Cuenta">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Cuenta ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre de la cuenta">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Cuenta Nombre") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="300px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Cuenta de") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cargos">
                                <ItemTemplate>
                                    <asp:TextBox ID="tb_cargos" runat="server"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Abonos">
                                <ItemTemplate>
                                    <asp:TextBox ID="tb_abonos" runat="server"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="75px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                
                </td>
                </tr>
                </table>
        </td></tr>   
        <tr>
            <td align="center">
                <asp:Button ID="bt_guardar" runat="server" Text="Guardar" 
                    onclick="bt_guardar_Click" />
                <asp:Button ID="bt_cancelar" runat="server" Text="Cancelar" />
            </td>
        </tr>
        </table>
      </form>
    </div>
</body>
</html>
