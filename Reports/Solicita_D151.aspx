<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="Solicita_D151.aspx.cs" Inherits="Reports_Solicita_D151" %>

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
                    <b><span style="font-size: small">&nbsp;Reporte D151</span></b></td>
                </tr>
            </thead>
              <tr>
                    <td>Tipo Persona</td>
                    <td>
                        <asp:DropDownList ID="lb_tipo_persona" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="lb_tipo_persona_SelectedIndexChanged">
                            <asp:ListItem Value="1">Ventas</asp:ListItem>
                            <asp:ListItem Value="2">Compras</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
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
                <td>Moneda:</td>
                <td>
                    <asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
                    <td><asp:Label ID="l_contabilidad" runat="server" Text="Tipo Contabilidad"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="lb_contabilidad" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Tipo:</td><td><asp:DropDownList ID="ddl_tipo" runat="server">
                    <asp:ListItem Value="1">Detallado</asp:ListItem>
                    <asp:ListItem Value="2">Resumido</asp:ListItem>

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

