<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="admin_retencion.aspx.cs" Inherits="manager_admin_retencion" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<h3 id="adduser">Administrador de retencion</h3>
<asp:ScriptManager ID="ScriptManager2" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {
        
    }  

    function mpeSeleccionOnCancel()
    {
        
    }
    function mpecancela_busqueda_rubro() {}
    </script>
    <table width="400">
    <tr><td>
        Busqueda de retencion:<asp:TextBox ID="tb_id_busqueda" runat="server" 
            Height="16px" Width="89px"></asp:TextBox><br /><br /><br />
    </td></tr>
    <tr><td>
        Identificador interno:<asp:TextBox ID="tb_id"  Text="0" ReadOnly runat="server" Height="16px" Width="40px" ></asp:TextBox>
        <br />
        Nombre:<asp:TextBox ID="tb_nombre" runat="server" Height="16px" Width="384px"></asp:TextBox>
        <br />
        Tipo:
        <asp:DropDownList ID="lb_tipo" runat="server">
            <asp:ListItem Value="1">Aplicable base</asp:ListItem>
            <asp:ListItem Value="2">Aplicable al impuesto</asp:ListItem>
            <asp:ListItem Value="3">Aplicable total</asp:ListItem>
            <asp:ListItem Value="4">Aplicable No Afecto</asp:ListItem>
        </asp:DropDownList>
        <br />
        Minimo:
        <asp:TextBox ID="tb_minimo" runat="server" Height="16px" Width="99px">0</asp:TextBox>
        <br />
        Porcentaje (0-100):
        <asp:TextBox ID="tb_porcentaje" runat="server" Height="16px" Width="100px">0</asp:TextBox>
        <br />
        Pais:
        <asp:DropDownList ID="lb_pais" runat="server">
        </asp:DropDownList>
        
    </td></tr>
    <tr><td align="center">
        
    
        <asp:Button ID="bt_guardar" runat="server" onclick="bt_guardar_Click" 
            Text="Guardar" />
        
    
    </td></tr>
    <tr><td>
            <!-- ***************************************************************************** -->
        <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" Width="722px">
            <div>
            <table>
            <tr><td colspan="2" align="center">Filtrar por</td></tr>
            <tr><td>Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
            </tr>
            </table>
            </div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Retencion" />
            </div>
            <div>
                <asp:GridView ID="dgw" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onselectedindexchanged="dgw_SelectedIndexChanged" PageSize="6">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
        <cc1:modalpopupextender ID="mpeSeleccion" runat="server" TargetControlID="tb_id_busqueda"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <!-- ***************************************************************************** -->
    </td></tr>
    </table>
</div>        
</asp:Content>

