<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="Solicita_Ventas.aspx.cs" Inherits="Reports_SolicitaER_2" %>

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
                    <b><span style="font-size: small">&nbsp;Ventas</span></b></td>
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
                        <asp:DropDownList ID="lb_contabilidad" runat="server" AutoPostBack="true" 
                            onselectedindexchanged="lb_contabilidad_SelectedIndexChanged">
                        </asp:DropDownList>
                </td>
                </tr>
                <tr>
                    <td>Ver Resultados en</td>
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
                    <td>Serie de Documento</td>
                    <td>
        
        <asp:TextBox ID="tb_serie" runat="server" Height="16px" Width="90px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Incluir</td>
                    <td>
                        <asp:CheckBoxList ID="CB_Documentos" runat="server">
                            <asp:ListItem Value="1">Facturas</asp:ListItem>
                            <asp:ListItem Value="3">Notas de Credito</asp:ListItem>
                            <asp:ListItem Value="4">Notas de Debito</asp:ListItem>
                            <asp:ListItem Value="18">NC Ajuste Contable</asp:ListItem>
                            <asp:ListItem Value="44">ND Proveedor</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td>Grupo SAT</td>
                    <td>
                        <asp:DropDownList ID="lb_grupo" runat="server">
                            <asp:ListItem Value="0">Seleccione</asp:ListItem>
                            <asp:ListItem Value="1">CENTRAL GT</asp:ListItem>
                            <asp:ListItem Value="2">COMBEX</asp:ListItem>
                            <asp:ListItem Value="3">ALSERSA</asp:ListItem>
                            <asp:ListItem Value="4">PUERTO QUETZAL</asp:ListItem>
                            <asp:ListItem Value="5">PUERTO BARRIOS</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Tipo:</td><td>
                    <asp:DropDownList ID="ddl_tipo" runat="server" 
                        Height="25px" Width="143px">
                    <asp:ListItem Value="0">Resumido</asp:ListItem>
                    <asp:ListItem Value="1">Detallado</asp:ListItem>
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

