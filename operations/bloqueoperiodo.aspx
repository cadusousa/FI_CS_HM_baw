<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="bloqueoperiodo.aspx.cs" Inherits="operations_bloqueoperiodo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<table align="center" width="500px">
<tr>
    
    <td align ="right">PERIODO ACTUAL BLOQUEADO: </td><td>
    <asp:TextBox ID="tb_bloqueo" runat="server"  width="100px" Enabled="False"></asp:TextBox></td>
</tr>
<tr>
    <td align ="right">Pais: </td><td ><asp:TextBox ID="TB_pais" runat="server" 
        width="100px" Enabled="False"></asp:TextBox></td>
</tr>
<tr>
<td align ="right">Nueva Fecha De Bloqueo: </td>
<td>
<asp:TextBox ID="tb_fechacorte" runat="server"  width="100px"></asp:TextBox>
    <cc1:CalendarExtender ID="tb_fechacorte_CalendarExtender" 
        runat="server" Enabled="True" Format="MM/dd/yyyy" TargetControlID="tb_fechacorte">
    </cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="tb_fechacorte_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date" Enabled="True" 
                        TargetControlID="tb_fechacorte">
   </cc1:MaskedEditExtender>


 
</td>
</tr>
<tr>
    <td colspan="2"= align="center">
        <asp:Button ID="Bguardar" runat="server" Text="Guardar" 
            onclick="Bguardar_Click" />
        <br />
        <br />
        <br />
        <asp:GridView ID="gv_resultado" runat="server" 
            onrowdeleting="gv_resultado_RowDeleting">
        <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Delete" HeaderText="Anular" 
                    ShowHeader="True" Text="Anular" />
            </Columns>
        </asp:GridView>
        <br />
    </td>
</tr>
</table>


</asp:Content>

