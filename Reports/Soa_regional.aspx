<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="Soa_regional.aspx.cs" Inherits="Reports_Soa_regional" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

    <div id="box">
<fieldset id="searchresult">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">
    function mpeCuentaOnCancel() {

    }
    function mpeClienteOnCancel() {

    } 
    </script>
<table width="300" align="center">
<thead>
    <tr><th colspan="2">Soa Regional</th></tr>
</thead>
<tbody>
<tr>
    <td>Tipo</td>
    <td>
       <asp:DropDownList ID="lb_tipopersona" runat="server">
                   <%-- <asp:ListItem Value="3">Cliente</asp:ListItem>--%>
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="10">Intercompany</asp:ListItem>
                    <%--<asp:ListItem Value="8">Caja Chica</asp:ListItem>--%>
                </asp:DropDownList>
                                            </td>
</tr>
<tr>
    <td>ID</td>
    <td>
        <asp:TextBox ID="tb_agenteID" runat="server" ReadOnly="True" Height="16px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Nombre</td>
    <td>
        <asp:TextBox ID="tb_agentenombre" runat="server" ReadOnly="True" Height="16px" 
            Width="217px"></asp:TextBox>
    </td>
</tr>
<%--<tr>
    <td>Moneda</td>
    <td>
                    <asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>
    </td>
</tr>--%>
<tr>
    <td>Fecha Corte</td>
    <td>
        
        <asp:TextBox ID="tb_fechafin" runat="server" Height="16px" Width="90px"></asp:TextBox>
        <cc1:MaskedEditExtender ID="tb_fechafin_MaskedEditExtender" runat="server" 
             Mask="99/99/9999" MaskType="Date"  Enabled="True" 
            TargetControlID="tb_fechafin">
        </cc1:MaskedEditExtender>
        <cc1:CalendarExtender ID="tb_fechafin_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fechafin" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
    </td>
</tr>
<%--<tr>
    <td>Pendiente de liquidar</td>
    <td>
        <asp:CheckBox ID="chk_pendliq" runat="server" Checked="True" />
    </td>
</tr>--%>
<%--<tr>
    <td> <asp:Label ID="l_contabilidad" runat="server" Text="Tipo Contabilidad"></asp:Label></td>
    <td>
        

        <asp:DropDownList ID="lb_contabilidad" runat="server">
        </asp:DropDownList>

    </td>
</tr>--%>
<tr>
    <td colspan="2" align="center">
        
        <asp:Button ID="bt_generar" runat="server" onclick="bt_generar_Click" 
            Text="Generar" />
        
    </td>
</tr>
<tr><td colspan="2">
<%--********************************************************--%>
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" 
        Width="722px" style="display:none">
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
</td>
</tr>
</tbody>
</table>
<table aling="center">
<tr><td>
   <%-- <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" />
    </td></--%>
</table>
</fieldset>
</div>
</asp:Content>

