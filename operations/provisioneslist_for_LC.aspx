<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="provisioneslist_for_LC.aspx.cs" Inherits="operations_provisioneslist" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
<fieldset id="Fieldset1">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {
        
    }  

    function mpeSeleccionOnCancel()
    {
        
    }
    </script>
<h3 id="adduser">ACTUALIZAR LIBRO DE COMPRAS</h3>
<table width="650">
<tr><td>
    <table width="650">
    <tr><td colspan="4">
        &nbsp;</td></tr>
    <tr><td>
        <strong>SERIE</strong></td><td>
                <asp:TextBox ID="tb_serie" runat="server" Height="16px" Width="200px"></asp:TextBox>
    </td><td>
            <strong>CORRELATIVO</strong></td><td>
        <asp:TextBox ID="tb_corr_provision" runat="server" Height="16px" Width="200px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="tb_corr_provision_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="tb_corr_provision">
                            </cc1:FilteredTextBoxExtender>
    </td></tr>
    <tr><td align="center" colspan="4" height="50px" valign="middle">
        <strong>MONEDA</strong>&nbsp;&nbsp;&nbsp;&nbsp;
       <asp:DropDownList ID="lb_moneda" runat="server" Font-Bold="True">
                </asp:DropDownList>
        </td></tr>
    <tr><td>
        <strong>TIPO</strong></td><td>
       <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" 
                onselectedindexchanged="lb_tipopersona_SelectedIndexChanged">
                    <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                    <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                </asp:DropDownList>
    </td><td>
            <strong>CODIGO</strong></td><td>
            <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="200px"></asp:TextBox>
    </td></tr>
    <tr><td>
        Nombre</td><td colspan="3">
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                Width="500px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td>
        Direccion</td><td colspan="3">
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" 
                Width="500px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td>
        Telefono</td><td>
                    <asp:TextBox ID="tb_telefono" runat="server" Height="16px" 
                Width="200px" ReadOnly="True"></asp:TextBox>
    </td><td>
            Correo Electronico</td><td>
            <asp:TextBox ID="tb_correoelectronico" runat="server" 
                        Height="16px" Width="200px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td align="center" colspan="4">
        &nbsp;</td></tr>
    <tr><td align="center" colspan="4">
        <asp:Button ID="btn_buscar" runat="server" onclick="btn_buscar_Click1" 
            Text="Buscar" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_nueva" runat="server" onclick="btn_nueva_Click" 
            Text="Nueva Busqueda" />
        </td></tr>
    </table>
</td></tr>
<tr><td><h3 id="H1">Listado de provisiones</h3></td></tr>
<tr><td>&nbsp;<asp:GridView ID="gv_detalle" runat="server" Width="623px" 
        onrowcommand="gv_detalle_RowCommand" onrowcreated="gv_detalle_RowCreated" 
        Font-Size="X-Small">
        <Columns>
            <asp:ButtonField ButtonType="Button" CommandName="Detalle" 
                Text="Actualizar" />
        </Columns>
        </asp:GridView>
        <br />
</td></tr>
<tr><td>

    &nbsp;</td></tr>
<tr><td>
    <!-- ***************************************************************************** -->
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" style="display:none;"
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
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5" 
                    Width="678px">
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
</table>
</fieldset>
</div>
</asp:Content>

