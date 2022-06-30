<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="Solicita_Retenciones.aspx.cs" Inherits="Reports_SolicitaER_2" %>

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
                    <b><span style="font-size: small">&nbsp;Reporte de Retenciones</span></b></td>
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
                <td><asp:Label ID="l_contabilidad" runat="server" Text="Tipo de Contabilidad"></asp:Label></td>
                <td>
                        <asp:DropDownList ID="lb_contabilidad" runat="server">
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
                    <td>Tipo de Persona&nbsp;</td>
                    <td>
                        <asp:RadioButtonList ID="Rb_Tipo_Persona" runat="server" 
                        RepeatDirection="Horizontal" AutoPostBack="True" 
                            onselectedindexchanged="Rb_Tipo_Persona_SelectedIndexChanged">
                    <asp:ListItem Value="3" Selected="True">Cliente</asp:ListItem>
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>Tipo de Retencion</td>
                    <td>
                        <asp:DropDownList ID="lb_tipo_retencion_clientes" runat="server" 
                            Visible="False">
                            <asp:ListItem Value="0">Todas</asp:ListItem>
                            <asp:ListItem Value="5">Retencion IVA</asp:ListItem>
                            <asp:ListItem Value="6">Retencion ISR</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="lb_tipo_retencion_proveedores" runat="server" 
                            Visible="False">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_personas" runat="server" Text="Personas" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="CB_Personas" runat="server" Visible="False">
                            <asp:ListItem Value="4">Proveedores</asp:ListItem>
                            <asp:ListItem Value="2">Agentes</asp:ListItem>
                            <asp:ListItem Value="5">Navieras</asp:ListItem>
                            <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                            <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                            <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
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

