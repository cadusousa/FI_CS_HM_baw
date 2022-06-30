<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="genera_comisiones.aspx.cs" Inherits="operations_genera_comisiones" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">Comisiones</h3>

<asp:ScriptManager ID="ScriptManager1" runat="server" >
 <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
<script  type="text/javascript">
    function BloquearPantalla() {
        $.blockUI({ message: '<h1>Generando...</h1>' });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI();
        });
    }
    </script>
<div style="width:100%;" align="center">
<table align="center" cellpadding="0" cellspacing="0" style="width: 93%">
<tr>
                <td style="width: 76px">Fecha Inicial</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" runat="server" Height="16px"></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechainicial" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
                <tr>
                <td style="width: 76px">Fecha Final&nbsp;</td>
                <td>
                    <asp:TextBox ID="tb_fechafinal" runat="server" Height="16px"></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechafinal_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechafinal" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
<tr>
    <td style="width: 76px">Vendedor: </td><td style="width: 357px">
    <asp:DropDownList ID="ddl_vendedores" runat="server" Height="24px" 
        Width="218px" AutoPostBack="True" 
        onselectedindexchanged="ddl_vendedores_SelectedIndexChanged">
    </asp:DropDownList><asp:Button ID="btn_generar_comis" runat="server" 
        Text="Generar" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" onclick="btn_generar_comis_Click" /></td>
    
  
</tr>
<tr><td align="center" colspan="2"><strong>Comisiones</strong></td></tr>
<tr>
<td align="center" colspan="2">
<asp:Panel ID="pnl_provisiones" runat="server" ScrollBars="Both" Width="650px" 
                    Height="550px">
                    
<asp:GridView ID="gv_comisiones" runat="server" BackColor="White" 
        BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
        onrowcreated="gv_comisiones_RowCreated">
    <Columns>
        <asp:TemplateField HeaderText="">
                
            <ItemTemplate>
                <asp:CheckBox ID="CB_comision_selected" runat="server" />
            </ItemTemplate>
                
        </asp:TemplateField>
    </Columns>
    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
    <RowStyle BackColor="White" ForeColor="#330099" />
    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
    <SortedAscendingCellStyle BackColor="#FEFCEB" />
    <SortedAscendingHeaderStyle BackColor="#AF0101" />
    <SortedDescendingCellStyle BackColor="#F6F0C0" />
    <SortedDescendingHeaderStyle BackColor="#7E0000" />
    </asp:GridView>
    </asp:Panel>
</td>
    
</tr>
<tr>
    <td style="width: 76px">Observaciones: </td><td>
    <asp:TextBox ID="tbx_observacion" runat="server" Width="348px"></asp:TextBox></td>
</tr>
<tr>
    <td align="center" colspan="2">
        <asp:Button ID="btn_generar_corte" runat="server" Text="Generar Corte" 
            Enabled="False" onclick="btn_generar_corte_Click" />        
    </td>
</tr>
<tr>
<td align="center" colspan="2"><strong>Cortes</strong></td>
</tr>
<tr>
<td align="center" colspan="2" > <asp:GridView ID="gv_cortes" runat="server" 
        Enabled="False">
     </asp:GridView></td>
</tr>
 <tr>
     
            <td align="center" colspan="4">
                <asp:Button ID="bt_guardar" runat="server" Text="Guardar" 
                    onclick="bt_guardar_Click" ValidationGroup="a" Enabled="False" 
                    Visible="False" />
            </td>
        </tr>
</table>

</div>
</div>
</asp:Content>

