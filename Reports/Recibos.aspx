<%@ Page Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="Recibos.aspx.cs" Inherits="Reports_EstadoCuentaCliente" Title="AIMAR - BAW" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="searchresult">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <table width="300" align="center">
<thead>
    <tr><th colspan="2">Reporte de Recibos</th></tr>
</thead>
<tbody>
<tr>
    <td>Sucursal</td>
    <td>
                    <asp:DropDownList ID="lb_sucursal" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_sucursal_SelectedIndexChanged">
                    </asp:DropDownList>
                                            </td>
</tr>
<tr>
    <td>Tipo Contabilidad</td>
    <td>

        <asp:DropDownList ID="lb_contabilidad" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_contabilidad_SelectedIndexChanged">
        </asp:DropDownList>

                                            </td>
</tr>
<tr>
    <td>Moneda</td>
    <td>
                    <asp:DropDownList ID="lb_moneda" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_moneda_SelectedIndexChanged">
                    </asp:DropDownList>
    </td>
</tr>
<tr>
    <td>Serie</td>
    <td>
                    <asp:DropDownList ID="lb_serie" runat="server">
                        <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                    </asp:DropDownList>
    </td>
</tr>
<tr>
    <td>Rango</td>
    <td>
                    &nbsp;<asp:TextBox ID="tb_rango_ini" runat="server" Height="16px" Width="50px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="tb_rango_ini_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers" 
                        TargetControlID="tb_rango_ini">
                    </cc1:FilteredTextBoxExtender>
&nbsp; Al&nbsp;
                    <asp:TextBox ID="tb_rango_fin" runat="server" Height="16px" Width="50px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="tb_rango_fin_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers" 
                        TargetControlID="tb_rango_fin">
                    </cc1:FilteredTextBoxExtender>
    </td>
</tr>
<tr>
    <td>Fecha Inicial</td>
    <td>
        
        <asp:TextBox ID="tb_fechaini" runat="server" Height="16px" Width="90px"></asp:TextBox>
        <cc1:MaskedEditExtender ID="tb_fechaini_MaskedEditExtender" runat="server" 
             Mask="99/99/9999" MaskType="Date"  Enabled="True" 
            TargetControlID="tb_fechaini">
        </cc1:MaskedEditExtender>
        <cc1:CalendarExtender ID="tb_fechaini_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fechaini" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
    </td>
</tr>
<tr>
    <td>Fecha Final</td>
    <td>
        
        <asp:TextBox ID="tb_fechafin" runat="server" Height="16px" Width="90px"></asp:TextBox>
        <cc1:MaskedEditExtender ID="tb_fechafin_MaskedEditExtender" runat="server" 
             Mask="99/99/9999" MaskType="Date"  Enabled="True" 
            TargetControlID="tb_fechafin">
        </cc1:MaskedEditExtender>
        <cc1:CalendarExtender ID="tb_fechafin_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fechafin" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
    </td>
</tr>
<tr>
    <td>Pendiente de depositar</td>
    <td>
        <asp:CheckBox ID="chk_pendliq" runat="server" />
    </td>
</tr>
<tr>
    <td colspan="2" align="center">
        
        <asp:Button ID="bt_generar" runat="server" onclick="bt_generar_Click" 
            Text="Generar" />
        
    </td>
</tr>

</tbody>
</table>
</fieldset>
</div>
</asp:Content>

