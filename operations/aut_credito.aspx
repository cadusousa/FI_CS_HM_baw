<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="aut_credito.aspx.cs" Inherits="operations_aut_credito" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<fieldset id="searchresult">
<table width="300" align="center">
<thead>
    <tr><th colspan="2">Credito a autorizar</th></tr>
</thead>
<tbody>
<tr>
    <td>Id solicitud<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </td>
    <td>
        <asp:TextBox ID="tb_idsolicitud" runat="server" ReadOnly="True" Height="16px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Id del cliente</td>
    <td>
        <asp:TextBox ID="tb_idcliente" runat="server" ReadOnly="True" Height="16px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Nombre del cliente</td>
    <td>
        <asp:TextBox ID="tb_nombrecliente" runat="server" ReadOnly="True" Height="16px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Dias solicitados</td>
    <td>
        <asp:TextBox ID="tb_diassolicitados" runat="server" ReadOnly="True" 
            Height="16px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Monto solicitado(Dolares)</td>
    <td>
        <asp:TextBox ID="tb_montosolicitado" runat="server" ReadOnly="True" 
            Height="16px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Dias autorizados</td>
    <td>
        <asp:TextBox ID="tb_diasautorizados" runat="server" Height="16px" MaxLength="3"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="tb_diasautorizados_FilteredTextBoxExtender" 
            runat="server" Enabled="True" FilterType="Numbers" 
            TargetControlID="tb_diasautorizados">
        </cc1:FilteredTextBoxExtender>
    </td>
</tr>
<tr>
    <td>Monto autorizado(Dolares)</td>
    <td>
        <asp:TextBox ID="tb_montoautorizado" runat="server" Height="16px"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="tb_montoautorizado_FilteredTextBoxExtender" 
            runat="server" Enabled="True" TargetControlID="tb_montoautorizado"
            FilterType="Numbers,Custom" 
            ValidChars=".">
        </cc1:FilteredTextBoxExtender>
    </td>
</tr>
<tr>
    <td>Solicitado Por</td>
    <td>
        <asp:TextBox ID="tb_solicitado_por" runat="server" Height="16px" 
            ReadOnly="True"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Cobrador&nbsp;</td>
    <td>
        <asp:TextBox ID="tb_cobrador" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_cobrador"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion"/>
    </td>
</tr>
<tr>
    <td colspan="2" align="center">
        <asp:Button ID="bt_aceptar" runat="server" Text="Guardar" 
            onclick="bt_aceptar_Click" />
    </td>
</tr>

</tbody>
</table>
    <br />
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" style=""
        Width="722px">
            <div><table>
                <tr><td colspan="2" align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                    <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Cobrador" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" Width="281px">
                </asp:GridView> 
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
</fieldset>
</div>
</asp:Content>

