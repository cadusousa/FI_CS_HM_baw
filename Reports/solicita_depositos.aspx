<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="solicita_depositos.aspx.cs" Inherits="Reports_solicita_dep_pendientes" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <table width="650">
    <tr>
        <td><asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
            <table width="300" align="center">
            <thead>
                <tr>
                <td align="center" colspan="2">
                    <b>Depositos pendientes de asignar a un recibo
                    </b></td>
                </tr>
            </thead>
                <tr>
                <td>Fecha Inicial</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" runat="server" Height="16px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fechainicial_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                        TargetControlID="tb_fechainicial">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechainicial" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
                <tr>
                <td>Fecha Final&nbsp;</td>
                <td>
                    <asp:TextBox ID="tb_fechafinal" runat="server" Height="16px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fechafinal_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date" Enabled="True" 
                        TargetControlID="tb_fechafinal">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fechafinal_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechafinal" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
                <tr>
                <td> <asp:Label ID="l_contabilidad" runat="server" Text="Tipo Contabilidad" 
                        Visible="False"></asp:Label></td>
                <td>
                        <asp:DropDownList ID="lb_contabilidad" runat="server" Visible="False">
                            
                        </asp:DropDownList>
                </td>
                </tr>
                <tr>
                    
                    <td>Moneda</td>
                    <td>
                    <asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td>
                Estado:
                </td><td>
                    <asp:DropDownList ID="dd_estado" runat="server">
                        <asp:ListItem Value="0">Pendientes de asignar</asp:ListItem>
                        <asp:ListItem Value="1">Asignados</asp:ListItem>
                        <asp:ListItem Value="2">Parcialmente Asignados</asp:ListItem>
                    </asp:DropDownList></td>
                </tr>
                <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btn_generar" runat="server" Text="Generar" 
                        onclick="btn_generar_Click" />
                </td>
            </tr>

            </table>
        </td>
    </tr>
</table>
</asp:Content>

