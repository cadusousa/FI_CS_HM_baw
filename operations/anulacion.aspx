<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="anulacion.aspx.cs" Inherits="operations_anulacion" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">ANULACIÓN DE DOCUMENTOS</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">   
    function mpecancela_busqueda_rubro()
    {

    } 
    function mpeCuentaOnCancel()
    {

    } 
    function mpeSeleccionOnCancel()
    {

    }
    </script>
<table align="center">
    <tr><td>Tipo de documento: </td> <td style="width: 387px">
        <asp:DropDownList ID="lb_tipo_doc" runat="server" AutoPostBack="True" 
            onselectedindexchanged="lb_tipo_doc_SelectedIndexChanged" Width="350px">
            <asp:ListItem Value="1">Factura</asp:ListItem>
            <asp:ListItem Value="2">Recibo</asp:ListItem>
            <asp:ListItem Value="3">Nota de Crédito</asp:ListItem>
            <asp:ListItem Value="4">Nota de Débito</asp:ListItem>
            <asp:ListItem Value="18">NC Ajuste</asp:ListItem>
            <asp:ListItem Value="17">Depositos</asp:ListItem>
            <asp:ListItem Value="6">Cheques</asp:ListItem>
            <asp:ListItem Value="19">Transferencias</asp:ListItem>
            <asp:ListItem Value="15">Ajuste Contable</asp:ListItem>
            <asp:ListItem Value="5">Provisiones</asp:ListItem>
            <asp:ListItem Value="21">NC Bancaria</asp:ListItem>
            <asp:ListItem Value="20">ND Bancaria</asp:ListItem>
            <asp:ListItem Value="12">NC Proveedor</asp:ListItem>
            <asp:ListItem Value="31">NC a ND Proveedor</asp:ListItem>
            <asp:ListItem Value="9">Retencion Proveedor</asp:ListItem>
            <asp:ListItem Value="22">Recibo SOA</asp:ListItem>
            <asp:ListItem Value="23">Corte Proveedor</asp:ListItem>                      
            <asp:ListItem Value="14">Proforma</asp:ListItem>   
            <asp:ListItem Value="28">Deposito Soa</asp:ListItem>
            <asp:ListItem Value="29">Cheque Comision</asp:ListItem>
            <asp:ListItem Value="30">Corte Comision</asp:ListItem>
        </asp:DropDownList>
        </td></tr>
    <tr><td>Moneda:</td> <td style="width: 387px">
       <asp:DropDownList ID="lb_moneda" runat="server">
                </asp:DropDownList>
        </td></tr>
                <tr>
        <td><asp:Label ID="lb_id" runat="server" Text="ID: " Visible="False"></asp:Label></td>
                    <td style="width: 387px">
                    <asp:TextBox ID="tb_id" runat="server" Visible="False" Height="16px"></asp:TextBox></td>           
        </tr>
    <tr><td>Serie del documento: </td><td style="width: 387px">
        <asp:TextBox ID="tb_refid" runat="server" Height="16px" Width="400px"></asp:TextBox>
        </td></tr>
    <tr><td>Número de correlativo: </td><td style="width: 387px">
        <asp:TextBox ID="tb_correlativo" runat="server" Height="16px" Width="400px"></asp:TextBox>
        </td></tr>
    <tr><td>Banco: </td><td style="width: 387px">
        
                                    <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="350px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                </asp:DropDownList>
        
        </td></tr>
    <tr><td>Cuenta Bancaria: </td><td style="width: 387px">
        
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" Width="350px">
                </asp:DropDownList>
        
        </td></tr>
    <tr><td>Codigo:</td>
        <td style="width: 387px"><asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px"></asp:TextBox>
        </td>
    </tr>
    <tr><td>Nombre : </td>
        <td style="width: 387px">
            <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                Width="400px" ReadOnly="True"></asp:TextBox>
        </td>
    </tr>
    <tr><td>Motivo de anulacion: </td><td style="width: 387px">
        <asp:TextBox ID="tb_motivo" runat="server" Height="16px" Width="400px"></asp:TextBox>
        </td></tr>
    <tr><td colspan="2">
    <!-- ***************************************************************************** -->
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" 
        Width="800px" style="display:none">
            <div><table>
                <tr><td align="center">Filtrar por</td></tr>
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
    <asp:TextBox ID="tb_sql" runat="server" Height="16px" Width="293px" Visible="false" />
    </td></tr>
    <tr><td colspan="2" align="center">
        <asp:Button ID="bt_aceptar" runat="server" Text="Buscar" 
            onclick="bt_aceptar_Click" />
        </td></tr>
</table>
<br /><br />
<center>

</center>
        <asp:Label ID="Label1" runat="server" 
            Text=" * El boton de anulacion permanecera deshabilitado cuando un documento este dentro de un periodo cerrado. O sea Serie electronica " 
            Enabled="False"></asp:Label>
<table align="center">
    <tr><td>
        <asp:GridView ID="gv_resultado" runat="server"        
            EmptyDataText="No hay registros que coincida con este criterio de búsqueda" 
            onrowdeleting="gv_resultado_RowDeleting" 
            onrowcreated="gv_resultado_RowCreated" 
            onrowdatabound="gv_resultado_RowDataBound" onselectedindexchanged="gv_resultado_SelectedIndexChanged" 
            >
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Delete" HeaderText="Anular" 
                    ShowHeader="True" Text="Anular" />
            </Columns>
        </asp:GridView>
        </td></tr>
    <tr><td><br /><br /></td></tr>
    <tr><td>
        <asp:GridView ID="gv_detalle" runat="server">
        </asp:GridView>
        </td></tr>
    
</table>
</div>
</asp:Content>

