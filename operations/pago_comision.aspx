<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="pago_comision.aspx.cs" Inherits="operations_pago_comision" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<h3 id="adduser">Pago Corte Comisiones</h3>
<div style="width:98%;" align="center">
<table align="center" cellpadding="0" cellspacing="0" style=" ">
<tr>
    <td  colspan="1">Vendedor: </td><td style="width: 357px" align="center" colspan= "3">
    <asp:DropDownList ID="ddl_vendedores" runat="server" Height="24px" 
        Width="218px" AutoPostBack="True" onselectedindexchanged="ddl_vendedores_SelectedIndexChanged" 
        >
    </asp:DropDownList></td>
   
</tr>
<tr>
<td  align=center colspan= 4><strong> Agregar Cheque.</strong></td>
</tr>
        <tr bgcolor="#99FFCC">
            <td style="height: 32px">
                <strong>Banco</strong></td>
            <td style="height: 32px">
               <!-- <asp:TextBox ID="tbx_banco" runat="server" Height="16px" Width="330px"></asp:TextBox> -->
                <asp:DropDownList ID="ddl_bancos" runat="server" Height="25px" Width="220px" 
                    AutoPostBack="True" onselectedindexchanged="ddl_bancos_SelectedIndexChanged">
                </asp:DropDownList>
                </td>
            <td style="height: 32px">
                <strong>Cuenta Bancaria</strong></td>
            <td style="height: 32px">
               <!-- <asp:TextBox ID="tbx_cuenta" runat="server"></asp:TextBox>  -->
                <asp:DropDownList ID="ddl_cuenta" runat="server" Width="150px">
            </asp:DropDownList>
                </td>
           
        </tr>
                <tr>
            <td bgcolor="#99FFCC">
                <strong>Número</strong></td>
            <td bgcolor="#99FFCC">
                <asp:TextBox ID="tb_chequeNo" 
                    runat="server" Height="16px" Width="150px" ></asp:TextBox>
                </td>
            <td bgcolor="#99FFCC">
                <strong>Monto</strong></td>
            <td bgcolor="#99FFCC">
                
                <asp:TextBox ID="tbx_monto" runat="server"></asp:TextBox></td>
        </tr>
        <tr bgcolor="#99FFCC">
            <td>
                Acreditado</td>
            <td colspan="3">
                <asp:TextBox ID="tb_acreditado" runat="server" 
                    Height="16px" Width="550px" Enabled="False"></asp:TextBox>
                </td>
        </tr>
        <tr bgcolor="#99FFCC">
            <td>
                Concepto</td>
            <td colspan="3">
                <asp:TextBox ID="tb_motivo" runat="server" Height="16px" Width="550px"></asp:TextBox>
                </td>
        </tr>
        <tr bgcolor="#99FFCC">
            <td>
                Observaciones</td>
            <td colspan="3">
                <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" Width="550px"></asp:TextBox>
                </td>
        </tr>
        <tr><td colspan=4 style="height: 26px" align="center"> 
            <asp:Button ID="btn_agregar_chk" runat="server" Text="Agregar"  Enabled="False"
                onclick="Button1_Click" /> 
            <asp:Button ID="Button1" runat="server" Text="Borrar" 
                onclick="Button1_Click1" /></td>
        
        </tr>
<tr bgcolor="#99CCFF"><td align="center" colspan="4" style="height: 17px"><strong>Cheques</strong></td></tr>
<tr >
<td align="center" colspan="4" > 
<asp:Panel ID="pnl_provisiones" runat="server" ScrollBars="Both" Width="650px" 
                    Height="150px">
<asp:GridView ID="gv_cheques" runat="server" >
      <Columns>
        <asp:TemplateField HeaderText="Seleccionar">
                
            <ItemTemplate>
                <asp:CheckBox ID="chb_cheques" runat="server" />
            </ItemTemplate>
                
        </asp:TemplateField>
    </Columns>
     </asp:GridView>
     </asp:Panel>
     </td>
</tr>
<tr bgcolor="#FF9966"><td align="center" colspan="4"><strong>Cortes Comision</strong></td></tr>
<tr>
<td align="center" colspan="4" >
<asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Width="650px" 
                    Height="150px">
    <asp:Label ID="lbl_notifica" runat="server"
            Text="Label" Enabled="False" Visible="False"></asp:Label>
             <asp:GridView ID="gv_cortes" runat="server" >
    <Columns>
        <asp:TemplateField HeaderText="Seleccionar">
                
            <ItemTemplate>
                <asp:CheckBox ID="chb_cortes" runat="server" />
            </ItemTemplate>
                
        </asp:TemplateField>
    </Columns>
     </asp:GridView>
     </asp:Panel>
     </td>
</tr>
<tr>
<td align=center colspan=4>  <asp:Button ID="btn_pagar" runat="server" Text="Pagar" 
        onclick="btn_pagar_Click" /></td>
</tr>

</table>
</div>
</div>
</asp:Content>

