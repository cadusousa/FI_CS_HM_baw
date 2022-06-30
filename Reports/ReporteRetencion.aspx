<%@ Page Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="ReporteRetencion.aspx.cs" Inherits="Reports_ReporteRetencion" Title="AIMAR - BAW" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

    <div id="box">
<fieldset id="searchresult">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">   
    function mpeSeleccionOnCancel()
    {

    } 
    function mpeClienteOnCancel()
    {

    } 
    </script>
<table width="300" align="center">
<thead>
    <tr><th colspan="2">Reporte retencion</th></tr>
</thead>
<tbody>
<tr>
    <td>Tipo de persona</td>
    <td>
       <asp:DropDownList ID="lb_tipopersona" runat="server">
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                </asp:DropDownList>
                                            </td>
</tr>
<tr>
    <td>ID del cliente</td>
    <td>
        <asp:TextBox ID="tb_agenteID" runat="server" ReadOnly="True"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Nombre del cliente</td>
    <td>
        <asp:TextBox ID="tb_agentenombre" runat="server" ReadOnly="True"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Tipo Reteción</td>
    <td>
                    <asp:DropDownList ID="lb_retencion" runat="server">
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
    <td>Fecha Inicial</td>
    <td>
        
        <asp:TextBox ID="tb_fechaini" runat="server" Height="16px" Width="90px"></asp:TextBox>
        <cc1:CalendarExtender ID="tb_fechaini_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fechaini" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
    </td>
</tr>
<tr>
    <td>Fecha Final</td>
    <td>
        
        <asp:TextBox ID="tb_fechafin" runat="server" Height="16px" Width="90px"></asp:TextBox>
        <cc1:CalendarExtender ID="tb_fechafin_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fechafin" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
    </td>
</tr>
<tr>
    <td>Tipo Contabilidad</td>
    <td>

        <asp:DropDownList ID="lb_contabilidad" runat="server">
        </asp:DropDownList>

    </td>
</tr>
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
    &nbsp;</td></tr>
</table>
</fieldset>
</div>

</asp:Content>

