<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="SolicitaBG.aspx.cs" Inherits="Reports_SolicitaER_2" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <table width="650">
    <tr>
        <td>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <table width="300" align="center">
            <thead>
                <tr>
                <td align="center" colspan="2">
                    <b><span style="font-size: small">&nbsp;Balance General</span></b></td>
                </tr>
            </thead>
                <tr>
                <td>Tipo de Reporte</td>
                <td>
<!--                          <asp:ListItem Value="1">Acumulado</asp:ListItem>              -->

                    <asp:DropDownList ID="lb_tipo_reporte" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_tipo_reporte_SelectedIndexChanged">
                        <asp:ListItem Value="0">Seleccione</asp:ListItem>
                        <asp:ListItem Value="2">Acotado</asp:ListItem>
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
                <td colspan="2">
                <asp:Panel ID="Panel1" runat="server" Visible="False">
                    <table align="left" cellpadding="0" cellspacing="0" 
                        style="padding: 0px; margin: 0px; width: 100%">
                        <tr>
                            <td width="110">
                                Fecha de Corte</td>
                            <td>
                                <asp:TextBox ID="tb_fechacorte" runat="server" Height="16px"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="tb_fechacorte_MaskedEditExtender" runat="server" 
                                    Enabled="True" Mask="99/99/9999" MaskType="Date" 
                                    TargetControlID="tb_fechacorte">
                                </cc1:MaskedEditExtender>
                                <cc1:CalendarExtender ID="tb_fechacorte_CalendarExtender" runat="server" 
                                    Enabled="True" Format="MM/dd/yyyy" TargetControlID="tb_fechacorte">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                </td>
                </tr>
                <tr>
                <td colspan="2">
                    <asp:Panel ID="Panel2" runat="server" Visible="False">
                        <table align="left" cellpadding="0" cellspacing="0" 
    style="padding: 0px; margin: 0px; width: 100%">
                            <tr>
                                <td width="110">
                                    Fecha Inicial</td>
                                <td>
                                    <asp:TextBox ID="tb_fechainicial" runat="server" Height="16px"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="tb_fechainicial_MaskedEditExtender" runat="server" 
                                        Enabled="True" Mask="99/99/9999" MaskType="Date" 
                                        TargetControlID="tb_fechainicial">
                                    </cc1:MaskedEditExtender>
                                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server" 
                                        Enabled="True" Format="MM/dd/yyyy" TargetControlID="tb_fechainicial">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Final</td>
                                <td>
                                    <asp:TextBox ID="tb_fechafinal" runat="server" Height="16px"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="tb_fechafinal_MaskedEditExtender" runat="server" 
                                        Enabled="True" Mask="99/99/9999" MaskType="Date" 
                                        TargetControlID="tb_fechafinal">
                                    </cc1:MaskedEditExtender>
                                    <cc1:CalendarExtender ID="tb_fechafinal_CalendarExtender" runat="server" 
                                        Enabled="True" Format="MM/dd/yyyy" TargetControlID="tb_fechafinal">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    </td>
                </tr>
                <tr>
                <td><asp:Label ID="l_contabilidad" runat="server" Text="Tipo Contabilidad"></asp:Label></td>
                <td>
                        <asp:DropDownList ID="lb_contabilidad" runat="server"
                onselectedindexchanged="lb_contabilidad_SelectedIndexChanged" AutoPostBack="true">
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
                    <td><asp:Label ID="l_moneda_consolidado" runat="server" Text="Consolidar moneda?"></asp:Label></td>
                    <td>
                        <asp:CheckBox ID="chk_consolidado" runat="server" />
                    </td>
                </tr>
                <tr>
                   <td style="height: 23px">
                   <asp:Label ID="Label1" runat="server" Text="consolidado regional" Visible="False"></asp:Label>
                   </td>    
                   <td style="height: 23px"><asp:CheckBox ID="regional" runat="server" 
                           Visible="False" /></td>
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

