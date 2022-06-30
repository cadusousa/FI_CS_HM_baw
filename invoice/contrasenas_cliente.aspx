<%@ Page Title="" Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="contrasenas_cliente.aspx.cs" Inherits="invoice_contrasenas_cliente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">CONTROL DE CONTRASEÑAS</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        function mpeSeleccionOnCancel() {

        }
    </script>
    <script  type="text/javascript">
        function BloquearPantalla() {
            $.blockUI({ message: '<h1>Procesando...</h1>' });
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                $.unblockUI();
            });
        }
    </script>
<div align="center">
<table>
    <tr><td>
        Tipo de Proveedor </td><td align="left">
       <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" Enabled="false">
                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                </asp:DropDownList>
        </td><td>
            Codigo</td><td>
            <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="75px" ReadOnly="True"></asp:TextBox>
        </td>
    </tr>
    <tr><td>
        Nombre</td><td colspan="5">
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td>
        Direccion</td><td colspan="5">
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td>
        No. Contraseña</td><td colspan="5">
                    <asp:TextBox ID="tb_nocontrasena" runat="server" Height="16px" 
                Width="256px" ></asp:TextBox>
    </td></tr>
    <tr><td>
        Fecha de Entrega</td><td colspan="5">
                    <asp:TextBox ID="tb_fecha_pago" runat="server" Height="16px" Width="128px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fecha_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_pago">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_pago" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
    </td></tr>
    <tr>
        <td >Observaciones</td>
        <td colspan="5"><asp:TextBox ID="tb_observaciones" runat="server" Height="16px" Width="520px"></asp:TextBox></td>
    </tr>
    <tr><td colspan="7" align="center">
        <asp:Button ID="btn_buscar" runat="server" onclick="btn_buscar_Click" 
            Text="Buscar Facturas" />
        </td></tr>
    <tr><td colspan="7"><h3 id="adduser">Facturas</h3></td></tr>
    <tr><td colspan="7">&nbsp;<asp:GridView ID="gv_detalle" runat="server" Width="650px" 
            onrowcreated="gv_detalle_RowCreated">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>
    <tr><td colspan="7" align="center">
        <asp:Button ID="bt_generar" runat="server" onclick="bt_generar_Click" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"
            Text="Generar Contraseña" />
        </td></tr>
    <tr><td colspan="7">&nbsp;<br />&nbsp;</td></tr>
    <tr><td colspan="7"></td></tr>
<tr><td colspan="7">&nbsp;
        <br />
</td></tr>
</table>
</div>
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
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_agenteID"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <!-- ***************************************************************************** -->
</div>
</asp:Content>

