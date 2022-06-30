<%@ Page Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="solicitaEC.aspx.cs" Inherits="Reports_EstadoCuentaAgente" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
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
<h3 id="adduser">REPORTE DE SOA</h3>
<table width="650">
<tr><td>
    <table width="650">
    <tr><td>
        Tipo de proveedor:
       <asp:DropDownList ID="lb_tipopersona" runat="server">
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                </asp:DropDownList>
                &nbsp; Moneda:
       <asp:DropDownList ID="lb_moneda" runat="server">
                </asp:DropDownList>
                <br />
        Codigo :<asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px"></asp:TextBox>
                    <br />
                    Nombre : 
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
            Width="377px" ReadOnly="True"></asp:TextBox>
                    <br />
                     Contacto:<asp:TextBox ID="tb_contacto" runat="server" Height="16px" 
                        Width="308px" ReadOnly="True"></asp:TextBox>
                    <br />
                    Direccion:
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" 
            Width="326px" ReadOnly="True"></asp:TextBox>
                    &nbsp;Teléfono:&nbsp;
                    <asp:TextBox ID="tb_telefono" runat="server" Height="16px" 
            Width="96px" ReadOnly="True"></asp:TextBox>
                    <br />
                    Correo electrónico:<asp:TextBox ID="tb_correoelectronico" runat="server" 
                        Height="16px" Width="231px" ReadOnly="True"></asp:TextBox>
        <br />
    </td></tr>
    <tr><td align="center">
        &nbsp;</td></tr>
    </table>
    <table>
    <tr>
    <td>Serie: </td><td>
        <asp:TextBox ID="tb_serie" runat="server"></asp:TextBox></td><td>Correlativo: </td>
        <td><asp:TextBox ID="tb_correlativo" runat="server"></asp:TextBox></td><td>
        <asp:Button ID="bt_buscarbyserie" runat="server" Text="Buscar" 
            onclick="bt_buscarbyserie_Click" /></td>
        
        </tr>
    </table>
</td></tr>
<tr><td><h3 id="H1">SOAs PAGADOS</h3></td></tr>
<tr><td>&nbsp;<asp:GridView ID="gv_detalle" runat="server" Width="623px" 
        onrowcommand="gv_detalle_RowCommand" onrowcreated="gv_detalle_RowCreated">
        <RowStyle HorizontalAlign="Center" />
        <Columns>
            <asp:ButtonField ButtonType="Button" CommandName="Seleccionar" 
                Text="Seleccionar" />
        </Columns>
        </asp:GridView>
        <br />
</td></tr>
<tr><td>
    <!-- ***************************************************************************** -->
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" 
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
    </td>
</tr>
</table>
</fieldset>
</div>
</asp:Content>

