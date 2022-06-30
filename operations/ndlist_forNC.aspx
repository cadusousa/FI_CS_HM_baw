<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="ndlist_forNC.aspx.cs" Inherits="operations_ndlist_forNC" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="Fieldset1">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">
        function mpeCuentaOnCancel() {

        }

        function mpeSeleccionOnCancel() {

        }
    </script>
<h3 id="adduser">NOTA DE CREDITO A NOTA DE DEBITO PROVEEDOR</h3>
<table width="650">
<tr><td align="center">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
        <tr>
            <td>
                Tipo de Persona</td>
            <td>
       <asp:DropDownList ID="lb_tipopersona" runat="server">
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                    <asp:ListItem Value="10">Intercompany</asp:ListItem>
                </asp:DropDownList>
                </td>
            <td>
                <strong>MONEDA</strong></td>
            <td>
       <asp:DropDownList ID="lb_moneda" runat="server">
                </asp:DropDownList>
                </td>
        </tr>
        <tr>
            <td>
                Codigo</td>
            <td colspan="3">
                <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px"></asp:TextBox>
                    </td>
        </tr>
        <tr>
            <td>
                Nombre</td>
            <td colspan="3">
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                    Width="450px" ReadOnly="True"></asp:TextBox>
                    </td>
        </tr>
        <tr>
            <td>
                Direccion</td>
            <td colspan="3">
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" Width="450px" 
                    ReadOnly="True"></asp:TextBox>
                    </td>
        </tr>
        <tr>
            <td>
                Telefono</td>
            <td colspan="3">
                    <asp:TextBox ID="tb_telefono" runat="server" Height="16px" Width="450px" 
                    ReadOnly="True"></asp:TextBox>
                    <!--
                     Contacto:<asp:TextBox ID="tb_contacto" runat="server" Height="16px" 
                        Width="308px"></asp:TextBox>  
                    <br />
                    -->
                    </td>
        </tr>
        <tr>
            <td>
                Correo Electronico</td>
            <td colspan="3">
                <asp:TextBox ID="tb_correoelectronico" runat="server" 
                        Height="16px" Width="450px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Estado Nota Debito</td>
            <td colspan="3">
       <asp:DropDownList ID="lb_ted" runat="server">
                    <asp:ListItem Value="1">Activo</asp:ListItem>
                    <asp:ListItem Value="9">Cortado</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
        <asp:Button ID="btn_buscar" runat="server" onclick="btn_buscar_Click1" 
            Text="Buscar" />
            </td>
        </tr>
    </table>
</td></tr>
<tr><td>
    &nbsp;</td></tr>
<tr><td><h3 id="H1">Listado de Notas Debito Proveedor</h3></td></tr>
<tr><td>&nbsp;<asp:GridView ID="gv_detalle" runat="server" Width="623px" 
        onrowcommand="gv_detalle_RowCommand" onrowcreated="gv_detalle_RowCreated" 
        Font-Size="X-Small">
        <Columns>
            <asp:ButtonField ButtonType="Button" CommandName="Detalle" 
                Text="Generar NC" />
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

