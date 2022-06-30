<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="provisioneslist.aspx.cs" Inherits="operations_provisiones_serch" %><%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box" align="center">
<h3 id="adduser">PROVISIONES<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </h3>
    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
        <tr>
            <td align="center" colspan="2" style="padding: 0px; margin: 0px">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td>
                            <strong>Tipo de Proveedor</strong></td>
                        <td>
       <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" 
            onselectedindexchanged="lb_tipopersona_SelectedIndexChanged">
                    <asp:ListItem>Todos</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                    <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                </asp:DropDownList>
                        </td>
                        <td>
                            <strong>Moneda</strong></td>
                        <td>
       <asp:DropDownList ID="lb_moneda" runat="server">
                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                <table align="center" cellpadding="0" cellspacing="1" 
                    style="width: 90%; height: 22px">
                    <tr>
                        <td>
                            <strong>Codigo</strong></td>
                        <td>
                            <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px"></asp:TextBox>
                        </td>
                        <td>
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr bgcolor="#E0E0E0">
            <td>
                <strong>Estado de la Provision</strong></td>
            <td>
       <asp:DropDownList ID="lb_ted" runat="server">
                    <asp:ListItem Value="0">Todos</asp:ListItem>
                    <asp:ListItem Value="1">Activo</asp:ListItem>
                    <asp:ListItem Value="5">Autorizada</asp:ListItem>
                    <asp:ListItem Value="3">Anulada</asp:ListItem>
                    <asp:ListItem Value="9">Cortado</asp:ListItem>
                    <asp:ListItem Value="4">Pagado</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Serie de la Provision</strong></td>
            <td>
                <asp:TextBox ID="tb_serie" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td>
                            <strong>Correlativo de la Provision</strong></td>
                        <td>
        <asp:TextBox ID="tb_corr_provision" runat="server" Height="16px" Width="100px" 
                                ontextchanged="tb_corr_provision_TextChanged" AutoPostBack="True"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="tb_corr_provision_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="tb_corr_provision">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td align="center">
                            <strong>&nbsp;&nbsp; o Rango</strong></td>
                        <td>
                            <asp:TextBox ID="tb_rango_inicial" runat="server" Height="16px" Width="100px" 
                                ontextchanged="tb_rango_inicial_TextChanged" AutoPostBack="True"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="tb_rango_inicial_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="tb_rango_inicial">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td align="center">
                            <strong>A</strong></td>
                        <td align="center">
                            <asp:TextBox ID="tb_rango_final" runat="server" Height="16px" Width="100px" 
                                ontextchanged="tb_rango_final_TextChanged" AutoPostBack="True"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="tb_rango_final_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="tb_rango_final">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr bgcolor="#E0E0E0">
            <td>
                <strong>MBL</strong></td>
            <td>
                <asp:TextBox ID="tb_mbl" runat="server" Height="16px" 
            Width="96px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td>
                            <strong>Serie Orden de Compra</strong></td>
                        <td>
                            <asp:DropDownList ID="lb_seriefactura" runat="server">
        </asp:DropDownList>
                        </td>
                        <td>
                            <strong>Correlativo </strong>
                        </td>
                        <td>
        <asp:TextBox ID="tb_corr_oc" runat="server" Height="16px" Width="100px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="tb_corr_oc_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="tb_corr_oc">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr bgcolor="#E0E0E0">
            <td align="center" colspan="2">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td>
                            <strong>Rango de Fechas</strong></td>
                        <td>
                            <asp:TextBox ID="tb_fecha_inicial" runat="server" Height="16px" Width="100px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="tb_fecha_inicial_MaskedEditExtender" runat="server" 
                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                Mask="99/99/9999" MaskType="Date"
                                TargetControlID="tb_fecha_inicial">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="tb_fecha_inicial_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="tb_fecha_inicial">
                            </cc1:CalendarExtender>
                        </td>
                        <td align="center">
                            <strong>A</strong></td>
                        <td>
                            <asp:TextBox ID="tb_fecha_final" runat="server" Height="16px" Width="100px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="tb_fecha_final_MaskedEditExtender" runat="server" 
                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                Mask="99/99/9999" MaskType="Date"
                                TargetControlID="tb_fecha_final">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="tb_fecha_final_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="tb_fecha_final">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td>
                            <strong>Serie de Factura Proveedor</strong></td>
                        <td>
                            <asp:TextBox ID="tb_serie_proveedor" runat="server" Height="16px" Width="100px"></asp:TextBox>
                        </td>
                        <td>
                            <strong>Correlativo de Factura de Proveedor</strong></td>
                        <td>
                            <asp:TextBox ID="tb_correlativo_proveedor" runat="server" Height="16px" 
                                Width="100px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="tb_correlativo_proveedor_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="tb_correlativo_proveedor">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" bgcolor="#E0E0E0">
                <strong>Certificado de Seguro:</strong></td>
            <td align="left" bgcolor="#E0E0E0">
                <asp:TextBox ID="tb_poliza_seguros" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
        <asp:Button ID="btn_buscar" runat="server" 
            Text="Buscar" onclick="btn_buscar_Click" />
                <asp:Button ID="btn_limpiar" runat="server" onclick="btn_limpiar_Click" 
                    Text="Limpiar" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Panel ID="pnl_provisiones" runat="server" ScrollBars="Both" Width="700px" 
                    Height="600px">
                    <asp:GridView ID="gv_detalle" runat="server" Width="750px" 
        onrowcommand="gv_detalle_RowCommand" 
    onrowcreated="gv_detalle_RowCreated" Font-Size="X-Small">
                        <Columns>
                            <asp:ButtonField ButtonType="Button" CommandName="Detalle" Text="Ver Detalle" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
    </table>
<br />
<cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_agenteID"
                PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
                OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
                BackgroundCssClass="FondoAplicacion">
                <Animations >
                    <OnShown><FadeIn Duration="0.5" Fps="40" /></OnShown>
                    <OnHiding><FadeOut Duration="0.5" Fps="40" /></OnHiding>
                </Animations>
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" DefaultButton="bt_buscar" 
        Width="800px" style="display:none;">
            <div><table>
                <tr><td colspan="2" align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:</td><td><asp:TextBox ID="tb_nitb" runat="server" Height="16px" Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:</td><td><asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td>Codigo:</td><td><asp:TextBox ID="tb_codigo" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="center"><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Agente" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
</div>
</asp:Content>

