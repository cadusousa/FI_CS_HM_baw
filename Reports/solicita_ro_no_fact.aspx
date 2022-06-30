<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="solicita_ro_no_fact.aspx.cs" Inherits="Reports_solicita_ro_no_fact" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
 <table width="650">
    <tr>
        <td>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <table align="center">
            <thead>
                <tr>
                <td align="center" colspan="2">
                    <b><span style="font-size: small">&nbsp;Reporte Routing No Facturados</span></b></td>
                </tr>
            </thead>
                <tr>
                <td>Fecha Inicial</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" runat="server" Height="16px"></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechainicial" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
                <tr>
                <td>Fecha Final&nbsp;</td>
                <td>
                    <asp:TextBox ID="tb_fechafinal" runat="server" Height="16px"></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechafinal_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechafinal" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
                
                <tr>
                    <td>Incluir Carga Ruteada</td>
                    <td>
                        <asp:CheckBox ID="chk_carga_ruteada" runat="server" Checked="true"/>
                    </td>
                </tr>
                <tr>
                    <td>Incluir Cargas FHC</td>
                    <td>
                        <asp:CheckBox ID="chk_cargas_cif" runat="server" Checked="false"/>
                    </td>
                </tr>
                <tr>
                <td> Sistemas</td>
                <td>
                        <asp:CheckBox ID="chk_maritimo" runat="server" Text="Maritimo" />
                        <asp:CheckBox ID="chk_terrestre" runat="server" Text="Terrestre" />
                        <asp:CheckBox ID="chk_aereo" runat="server" Text="Aereo" />
                        <asp:CheckBox ID="chk_wms" runat="server" Text="WMS" />
                        <asp:CheckBox ID="chk_seguros" runat="server" Text="Seguros"/>
                        <asp:CheckBox ID="chk_aduanas" runat="server" Text="Aduanas" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
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

