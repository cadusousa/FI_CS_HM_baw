<%@ Page Title="" Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="solicita_contrasenas.aspx.cs" Inherits="invoice_solicita_contrasenas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<tbody>
<h3 id="adduser">REPORTE CONTRASEÑAS</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
    <table width="700" align="center">
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td>Fecha Inicio</td>
                                                <td>
                                                    <asp:TextBox ID="tb_fecha_inicio" runat="server" Height="16px" Width="128px"></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="tb_fecha_inicio_MaskedEditExtender" runat="server" 
                                                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                                                    TargetControlID="tb_fecha_inicio">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:CalendarExtender ID="tb_fecha_inicio_CalendarExtender" runat="server" 
                                                    Enabled="True" TargetControlID="tb_fecha_inicio" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Fecha Fin</td>
                                                <td>    
                                                    <asp:TextBox ID="tb_fecha_fin" runat="server" Height="16px" Width="128px"></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="tb_fecha_fin_MaskedEditExtender1" runat="server" 
                                                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                                                    TargetControlID="tb_fecha_fin">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:CalendarExtender ID="tb_fecha_fin_CalendarExtender1" runat="server" 
                                                    Enabled="True" TargetControlID="tb_fecha_fin" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tipo Persona</td>
                                                <td>
                                                    <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" Enabled="false">
                                                        <asp:ListItem Value="3">Cliente</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr><td align="left">Codigo Cliente</td><td>
                                                <asp:TextBox ID="tb_codigo_cliente" 
                                                        runat="server" Height="16px" Width="300px"></asp:TextBox></td></tr>
                                            <tr><td align="left">Nombre Cliente</td><td>
                                                <asp:TextBox ID="tb_nombrecliente" 
                                                        runat="server" Height="16px" Width="300px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td>No. Contraseña</td>
                                                <td><asp:TextBox ID="tb_nocontrasena" runat="server" Height="16px" Width="256px" ></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td>Ordenar por?</td>
                                                <td>
                                                    <asp:RadioButtonList ID="rbl_ordenar_por" runat="server">
                                                        <asp:ListItem Text ="Fecha Pago" Value="1" />
                                                        <asp:ListItem Text ="Cliente" Value="2" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                            
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="Button1" runat="server" Text="Buscar" onclick="bt_search_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
    </table>
    </tbody>
    <!-- ***************************************************************************** -->
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo"  style="display:none;"
        Width="800px">
            <div><table>
                <tr><td colspan="2" align="center">Filtrar por</td></tr>
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
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Agente" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" onload="gv_proveedor_Load" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_codigo_cliente"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <!-- ***************************************************************************** -->
  </div>
</asp:Content>

