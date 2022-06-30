<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="provisionaBL.aspx.cs" Inherits="operations_provisionaBL" Title="AIMAR - BAW"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">PROVISION DE BL&#39;S</h3>
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
<table width="650">
<tr>
    <td>
        <table width="650" align="left">
        <tbody>
            <tr>
                <td>
                
        <asp:Label ID="lb_mbl" runat="server" Text="MBL"></asp:Label>
        <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="109px" ReadOnly="True"></asp:TextBox>
        <asp:TextBox ID="tb_mbl_id" Visible="false" runat="server" Height="16px" Width="30px"></asp:TextBox>
                    &nbsp;Tipo:<asp:TextBox ID="tb_tipo" runat="server" Height="16px" Width="30px"></asp:TextBox>
                    &nbsp; Moneda:&nbsp;<asp:DropDownList ID="lb_moneda" runat="server"></asp:DropDownList>
                    &nbsp;
                    <asp:DropDownList ID="lb_imp_exp" runat="server">
                    </asp:DropDownList>
                    &nbsp;<asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                    <br />
                    <asp:DropDownList ID="lb_serie_factura" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                <table width="650">
                <thead>
                <tr>
                    <th colspan="6">Detalle de provision           </tr>
                </thead>
                <tr><td>
                    <asp:GridView ID="gv_detalle_costos" runat="server" 
                        Width="630px" AutoGenerateColumns="False" 
                        onrowdeleting="gv_detalle_costos_RowDeleting" 
                        onselectedindexchanged="gv_detalle_costos_SelectedIndexChanged">
                        <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" />
                <asp:TemplateField HeaderText="Codigo" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lb_codigo" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rubro ID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lb_rubro_id" runat="server" Text='<%# Eval("Rubro ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rubro">
                    <ItemTemplate>
                        <asp:Label ID="lb_rubro" runat="server" Text='<%# Eval("Rubro") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo servicio">
                    <ItemTemplate>
                        <asp:Label ID="lb_rubrotype" runat="server" Text='<%# Eval("Tipo servicio") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Moneda">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo" runat="server" Text='<%# Eval("Moneda") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate>
                        <asp:Label ID="lb_total" runat="server" Text='<%# Eval("Valor") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Proveedor">
                    <ItemTemplate>
                        <asp:Label ID="lb_proveedortipo" runat="server" Text='<%# Eval("Tipo Proveedor") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Proveedor ID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lb_proveedorID" runat="server" Text='<%# Eval("Proveedor ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Proveedor">
                    <ItemTemplate>
                        <asp:Label ID="lb_proveedor" runat="server" Text='<%# Eval("Proveedor") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Referencia">
                    <ItemTemplate>
                        <asp:Label ID="lb_referencia" runat="server" Text='<%# Eval("Referencia") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
                    </asp:GridView>
                </td></tr>
                </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                
                    <asp:Button ID="bt_provisionar" runat="server" Text="Provisionar" 
                        onclick="bt_provisionar_Click" />
                
                </td>
            </tr>
        </tbody>
        </table>
    </td>
</tr>
<tr>
<td>
     
    
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="tb_mbl"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <%--********************************************************--%>
    <%--********************************************************--%>
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none">
    
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label1" runat="server" Text="Seleccionar Cuenta" />
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_criterio" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="lb_tipo" runat="server">
                    <asp:ListItem>LCL</asp:ListItem>
                    <asp:ListItem>FCL</asp:ListItem>
                    <asp:ListItem>ALMACENADORA</asp:ListItem>
                    <asp:ListItem>AEREO</asp:ListItem>
                    <asp:ListItem>TERRESTRE T</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="dgw1" runat="server" AllowPaging="True" 
                    onpageindexchanging="dgw1_PageIndexChanging" onrowcommand="dgw1_RowCommand" 
                    PageSize="10">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bt_search" runat="server" Text="Buscar" 
                            onclick="bt_search_Click" />
            </td>
            <td><asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
</td>
</tr>
</table>
</fieldset>
</div>
</asp:Content>

