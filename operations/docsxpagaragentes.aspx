<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="docsxpagaragentes.aspx.cs" Inherits="operations_docsxpagar" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
<h3 id="adduser">DOCUMENTOS POR CANCELAR</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
    <script language="javascript" type="text/javascript">   
    function mpeSeleccionOnCancel()
    {

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
        Tipo de Proveedor&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td><td align="center" 
            colspan="2">
       <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" 
            onselectedindexchanged="lb_tipopersona_SelectedIndexChanged">
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                    <asp:ListItem Value="10">Intercompany</asp:ListItem>
                </asp:DropDownList>
        </td><td>
            Codigo</td><td>
            <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="75px" ReadOnly="True"></asp:TextBox>
        </td><td>
            <strong>MONEDA</strong></td><td>
       <asp:DropDownList ID="lb_moneda" runat="server" Font-Bold="True" AutoPostBack="True" 
                onselectedindexchanged="lb_moneda_SelectedIndexChanged">
                </asp:DropDownList>
                                <asp:Label ID="lb_fecha_hora" runat="server" 
                Enabled="False" Visible="False"></asp:Label>
        </td></tr>
    <tr><td colspan="2">
        Nombre</td><td colspan="5">
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td colspan="2">
        Direccion</td><td colspan="5">
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td colspan="2">
        Contacto</td><td colspan="5">
                    <asp:TextBox ID="tb_contacto" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td colspan="2">
        Teléfono</td><td colspan="5">
                    <asp:TextBox ID="tb_telefono" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td colspan="2">
        Correo</td><td colspan="5">
                    <asp:TextBox ID="tb_correoelectronico" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td colspan="7" align="center">
        <asp:Button ID="btn_buscar" runat="server" onclick="btn_buscar_Click" 
            Text="Buscar Documentos" />
        </td></tr>
    <tr><td colspan="7"><h3 id="adduser">Cuentas por pagar</h3></td></tr>
    <tr><td colspan="7">&nbsp;<asp:GridView ID="gv_detalle" runat="server" 
            Width="750px" style="table-layout:fixed"
            onrowcreated="gv_detalle_RowCreated" 
            onrowdatabound="gv_detalle_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>
    <tr><td colspan="7">
        &nbsp;</td></tr>
    <tr><td colspan="7"><h3 id="H1">Notas de Credito</h3></td></tr>
    <tr><td colspan="7">&nbsp;<asp:GridView ID="gv_notacredito" runat="server" Width="650px" 
            onrowcreated="gv_notacredito_RowCreated">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>
         <tr><td colspan="7"><h3 id="H4">Notas de Credito a Nota Debito</h3></td></tr>
    <tr><td colspan="7">&nbsp;<asp:GridView ID="gv_notacredito_nd" runat="server" Width="650px" 
            onrowcreated="gv_notacredito_nd_RowCreated">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>
    <tr><td colspan="7">
        &nbsp;</td></tr>
    <tr><td colspan="7"><h3 id="H2">Notas de debito</h3></td></tr>
    <tr><td colspan="7">&nbsp;<asp:GridView ID="gv_notadebito" runat="server" Width="650px" 
            onrowcreated="gv_notadebito_RowCreated">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>
         <tr><td colspan="7"><h3 id="H5"><asp:Label ID="lb_titulo_fac" runat="server" 
                 Text="Facturas Intercompany" Visible="False"></asp:Label></h3></td></tr>
    
         <tr><td colspan="7">&nbsp;<asp:GridView ID="gv_fact_intercompany" runat="server" Width="650px" 
            onrowcreated="gv_factintercompany_RowCreated" Visible="False">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>
    <tr><td colspan="7">
        &nbsp;</td></tr>
    
    <tr><td colspan="7">
        <asp:Button ID="bt_generar" runat="server" onclick="bt_generar_Click" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"
            Text="Generar corte" />
        </td></tr>
    <tr><td colspan="7">&nbsp;<br />&nbsp;</td></tr>
    <tr><td colspan="7"><h3 id="H3">Cortes del proveedor</h3></td></tr>
<tr><td colspan="7">&nbsp;<asp:GridView ID="gv_cortes" runat="server" Width="623px" 
        onrowcommand="gv_cortes_RowCommand" onrowcreated="gv_cortes_RowCreated">
        <Columns>
            <asp:ButtonField ButtonType="Button" CommandName="Detalle" Text="Ver Detalle" />
        </Columns>
        </asp:GridView>
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

