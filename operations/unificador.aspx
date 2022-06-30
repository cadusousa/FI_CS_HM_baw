<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="unificador.aspx.cs" Inherits="manager_unificador" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">UNIFICADOR DE CUENTAS</h3>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
          <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
          <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
        </Scripts>
    </asp:ScriptManager>
    <script type="text/javascript">
        function BloquearPantalla() {
            $.blockUI({ message: '<h1>Procesando...</h1>' });
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                $.unblockUI();
            });
        }
    </script>
    <table width="700">
    <tr><td>
        
    </td>
    </tr>
    <tr><td>
        <table>
            <tr>
                <td style="width:100px">Tipo de código:</td>
                <td>
                    <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_tipopersona_SelectedIndexChanged" CssClass="formas">
                        <asp:ListItem Value="4">Proveedor</asp:ListItem>
                        <asp:ListItem Value="2">Agente</asp:ListItem>
                        <asp:ListItem Value="5">Naviera</asp:ListItem>
                        <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                        <asp:ListItem Value="3">Cliente</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="text-align:right;width:600px;font-weight:bold"><asp:Label runat="server" ForeColor="Red" ID="lbl_informacion"></asp:Label></td>
            </tr>
        </table>
        <br />
        <fieldset id="personal">
            <legend style="font-weight:bold;font-size:small">Origen</legend>
            <table>
                <tr>
                    <td>Código:</td>
                    <td><asp:TextBox ID="tb_agenteID" runat="server" Width="115px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Nombre:</td>
                    <td><asp:TextBox ID="tb_agentenombre" runat="server" Width="600px" ReadOnly="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Dirección:</td>
                    <td><asp:TextBox ID="tb_direccion" runat="server" Height="16px" Width="600px" ReadOnly="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center"><asp:CheckBox ID="desactivar" runat="server" Text="Desactivar este codigo" visible="true"/></td>
                </tr>
            </table>
         </fieldset>
         <br/><br />
         <fieldset id="personal">
            <legend style="font-weight:bold;font-size:small">Destino</legend>
            <table>
                <tr>
                    <td>Código:</td>
                    <td><asp:TextBox ID="tb_agenteID2" runat="server" Width="115px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Nombre:</td>
                    <td><asp:TextBox ID="tb_agentenombre2" runat="server" Width="600px" ReadOnly="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Dirección:</td>
                    <td><asp:TextBox ID="tb_direccion2" runat="server" Width="600px" ReadOnly="True"></asp:TextBox></td>
                </tr>
            </table>
         </fieldset>
    </td></tr>
    <tr><td>
    <!-- ***************************************************************************** -->
        <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" Width="722px" style="display:none">
            <div><table>
                <tr><td colspan="2" align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:</td>
                    <td><asp:TextBox ID="tb_nitb" runat="server" Height="16px" Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:</td>
                    <td><asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td>Código:</td>
                    <td><asp:TextBox ID="tb_codigo" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center"><asp:Button ID="Button2" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table>
            
            </div>
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
    <!-- ***************************************************************************** -->
        <asp:Panel ID="pnlProveedor2" runat="server" CssClass="CajaDialogo" Width="722px" style="display:none">
            <div><table>
                <tr><td align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:</td>
                    <td><asp:TextBox ID="tb_nitb2" runat="server" Height="16px" Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:</td>
                    <td><asp:TextBox ID="tb_nombreb2" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td>Código:</td>
                    <td><asp:TextBox ID="tb_codigo2" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center"><asp:Button ID="Button3" runat="server" Text="Buscar" onclick="Button3_Click" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label42" runat="server" Text="Seleccionar Agente" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor2" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" onload="gv_proveedor2_Load" 
                    onpageindexchanging="gv_proveedor2_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor2_SelectedIndexChanged" PageSize="5">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar2" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="mpeSeleccion2" runat="server" TargetControlID="tb_agenteID2"
            PopupControlID="pnlProveedor2" CancelControlID="btnProveedorCancelar2"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <!-- ***************************************************************************** -->
    </td></tr>
    <tr><td align="center">
        
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" OnClientClick="if (!confirm('¿Desea unificar?, este proceso puede tardar unos minutos')) { return false; } else { BloquearPantalla(); }"
            Text="Unificar" />
        
    </td></tr>
    </table>
</div>        
</asp:Content>

