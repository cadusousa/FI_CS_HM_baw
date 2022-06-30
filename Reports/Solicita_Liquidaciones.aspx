<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="Solicita_Liquidaciones.aspx.cs" Inherits="Reports_Solicita_Liquidaciones" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
    <table align="center" cellpadding="0" cellspacing="0" 
        style="width: 70%; height: 74px;">
        <thead>
                <tr>
                <td align="center" colspan="4" height="25">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <b><span style="font-size: small">Reporte de Liquidaciones</span></b></td>
                </tr>
            </thead>
        <tr>
            <td style="height: 18px">
                Tipo de Persona</td>
            <td style="height: 18px">
                            <asp:DropDownList ID="drp_tipo_persona" runat="server">
                                <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                <asp:ListItem Value="2">Agente</asp:ListItem>
                                <asp:ListItem Value="5">Naviera</asp:ListItem>
                                <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                                <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                            </asp:DropDownList>
            </td>
            <td style="height: 18px">
                Codigo</td>
            <td style="height: 18px">
                                <asp:TextBox ID="tb_codigo_proveedor" runat="server" Height="16px" 
                        Width="75px">0</asp:TextBox>
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_codigo_proveedor"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                Nombre: 
                            <asp:TextBox ID="tb_nombre" runat="server" Height="16px" ReadOnly="True" 
                                Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 17px" colspan="4">
                Banco
                            <asp:DropDownList ID="drp_banco" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="drp_banco_SelectedIndexChanged">
                            </asp:DropDownList>
                </td>
        </tr>
        <tr>
            <td colspan="2">
                Cuenta
                            <asp:DropDownList ID="drp_banco_cuenta" runat="server">
                            </asp:DropDownList>
                </td>
            <td colspan="2">
                Moneda
                            <asp:DropDownList ID="drp_moneda" runat="server" Enabled="False">
                            </asp:DropDownList>
                </td>
        </tr>
        <tr>
            <td colspan="2">
                Fecha Inicial
                    <asp:TextBox ID="tb_fechainicial" runat="server" Height="16px"></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechainicial" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
            <td colspan="2">
                Fecha Final
                    <asp:TextBox ID="tb_fechafinal" runat="server" Height="16px"></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechafinal_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechafinal" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Button ID="btn_generar" runat="server" Text="Generar" 
                    onclick="btn_generar_Click" />
            </td>
        </tr>
    </table>
</div>
<asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" style="display:none;"
        Width="800px">
            <div><table>
                <tr><td align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:<asp:TextBox ID="tb_nitb" runat="server" Height="16px" Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td>Codigo:<asp:TextBox ID="tb_codigo" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table></div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5" 
                    BorderStyle="None">
                    <HeaderStyle BackColor="#0000CC" BorderColor="#0000CC" BorderStyle="None" 
                        ForeColor="White" />
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
</asp:Content>

