<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="recibo_factura.aspx.cs" Inherits="operations_recibo_factura" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">RECEPCION DE FACTURAS</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    
        <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
            <tr>
                <td>
                    Serie de contraseña:</td>
                <td colspan="2">
                    <asp:DropDownList ID="lb_contraseniaserie" runat="server"></asp:DropDownList>
         <asp:TextBox ID="tb_correlativo" runat="server" ReadOnly="True" Height="16px"></asp:TextBox>
                </td>
                <td>
         <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lbl_blID" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_tipoOperacionID" runat="server" Text="8" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_tpiID" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_proveedorID" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
        <asp:Panel ID="pnl_oc" runat="server">
            Orden de Compra:<asp:DropDownList ID="lb_serie_factura" runat="server">
            </asp:DropDownList>
            <asp:TextBox ID="tb_oc_correlativo" runat="server" AutoPostBack="True" 
            Height="16px" ontextchanged="tb_ordencompra_TextChanged" 
    Width="65px"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="tb_oc_correlativo_FilteredTextBoxExtender" 
            runat="server" Enabled="True" FilterType="Numbers" 
            TargetControlID="tb_oc_correlativo">
            </cc1:FilteredTextBoxExtender>
            &nbsp;<br /> Provision:
            <asp:DropDownList ID="lb_serie_provision" runat="server">
            </asp:DropDownList>
            <asp:TextBox ID="tb_provision_correlativo" runat="server" AutoPostBack="True" 
            Height="16px" ontextchanged="tb_provision_correlativo_TextChanged" 
            Width="65px"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="tb_provision_correlativo_FilteredTextBoxExtender" 
            runat="server" Enabled="True" FilterType="Numbers" 
            TargetControlID="tb_provision_correlativo">
            </cc1:FilteredTextBoxExtender>
            &nbsp;<br />
            </asp:Panel>
        <asp:Panel ID="pnl_provision_automatica" runat="server">
            Provision Automatica:
            <asp:DropDownList ID="drp_serie_provision_automatica" runat="server">
            </asp:DropDownList>
            <asp:TextBox ID="tb_provision_automatica_correlativo" runat="server" 
                AutoPostBack="True" Height="16px" 
                ontextchanged="tb_provision_automatica_correlativo_TextChanged" Width="65px"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="tb_provision_automatica_correlativo_FilteredTextBoxExtender" 
                runat="server" Enabled="True" FilterType="Numbers" 
                TargetControlID="tb_provision_automatica_correlativo">
            </cc1:FilteredTextBoxExtender>
        </asp:Panel>
                    <asp:Panel ID="pnl_sat" runat="server" Visible="False">
                        <asp:Label ID="lb_tipo_docto" runat="server" Text="Tipo Documento:" 
                        Visible="False"></asp:Label>
                        <asp:DropDownList ID="ddl_tipo_documento" runat="server" Visible="False">
                        </asp:DropDownList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    Factura Serie:</td>
                <td>
                    <asp:TextBox ID="tb_factura" runat="server" Height="16px" Width="200px"></asp:TextBox>
                </td>
                <td>
                    Factura correlativo:</td>
                <td>
                    <asp:TextBox ID="tb_factura_correlativo" runat="server" 
            Height="16px" Width="200px" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Monto de la factura:</td>
                <td>
                    <asp:TextBox ID="tb_monto_factura" runat="server" Height="16px" Width="200px"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="tb_monto_factura_FilteredTextBoxExtender" 
            runat="server" Enabled="True" TargetControlID="tb_monto_factura" FilterType="Numbers,Custom" ValidChars=".">
        </cc1:FilteredTextBoxExtender>
                    <asp:Label ID="lb_proveedorID" runat="server" Text="0" Visible="False"></asp:Label>
        <asp:Label ID="lb_servicioID" runat="server" Text="0" Visible="False"></asp:Label>
        <asp:Label ID="lb_departamento" runat="server" Text="0" Visible="False"></asp:Label>
        <asp:Label ID="lb_tieID" runat="server" Text="0" Visible="False"></asp:Label>
        <asp:Label ID="lb_moneda" runat="server" Text="0" Visible="False"></asp:Label>
        <asp:Label ID="lb_provisionID" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
                <td>
                    Credito:</td>
                <td>
        <asp:TextBox ID="lb_credito" runat="server" Height="16px" Width="200px" 
            ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Fecha de la factura:</td>
                <td>
        <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="200px" AutoPostBack="True" 
                        ontextchanged="tb_fecha_TextChanged"></asp:TextBox>
        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha">
                </cc1:MaskedEditExtender>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha" Format="MM/dd/yyyy">
                </cc1:CalendarExtender>
                </td>
                <td>
                    Fecha Pago:</td>
                <td>
        <asp:TextBox ID="lb_fechapago" runat="server" Height="16px" Width="200px" 
            ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Nombre:</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_nombre" runat="server" Height="16px" Width="500px" 
            ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Nit:</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_nit" runat="server" Height="16px" Width="500px" 
                        ReadOnly="True" />
                </td>
            </tr>
            <tr>
                <td>
                    Tipo:</td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rb_bienserv" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">Bien</asp:ListItem>
                    <asp:ListItem Value="2" Selected="True">Servicio</asp:ListItem>
                </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    HBL:</td>
                <td>
                    <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="200px" 
            ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                    MBL:</td>
                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="200px" 
            ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Routing:</td>
                <td>
                    <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="200px" 
            ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                    Contenedor:</td>
                <td>
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" 
            Width="200px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
        Observaciones:</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_observacion" runat="server" Height="16px" Width="550px" 
                        MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
        Nombre quien autoriza la orden de compra:</td>
                <td colspan="2">
                    <asp:TextBox ID="tb_personaautoriza" 
            runat="server" Height="16px" Width="350px" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Departamento:</td>
                <td colspan="3">
        <asp:TextBox ID="tb_departamento" 
            runat="server" Height="16px" Width="500px" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Fecha de Recepcion:</td>
                <td colspan="3">
                    <asp:Label ID="lbl_fecha_recibo_factura" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4" height="40" style="background-color: #FFFF99" 
                    valign="middle">
                    &nbsp; Valor:&nbsp; &nbsp;<asp:TextBox ID="tb_valor" 
            runat="server" Height="16px" 
            Width="100px" ReadOnly="True"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="tb_noafecto" Text="0" runat="server" Height="16px" Width="100px" 
            AutoPostBack="True" ontextchanged="tb_noafecto_TextChanged" Visible="false"></asp:TextBox>
        &nbsp;&nbsp;
        <asp:TextBox ID="tb_afecto" runat="server" Height="16px" Width="100px" Text="0" Visible="false"></asp:TextBox>
        &nbsp;&nbsp;
        <asp:TextBox ID="tb_iva" runat="server" Height="16px" Width="100px" Text="0" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
    <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" Visible="false"
            EmptyDataText="No hay rubros que facturar" Width="97%" 
            ondatabound="gv_detalle_DataBound" onrowdeleting="gv_detalle_RowDeleting">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" />
                <asp:TemplateField HeaderText="Codigo">
                    <ItemTemplate>
                        <asp:Label ID="lb_codigo" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rubro">
                    <ItemTemplate>
                        <asp:Label ID="lb_rubro" runat="server" Text='<%# Eval("NOMBRE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <asp:Label ID="lb_tipo" runat="server" Text='<%# Eval("TYPE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Moneda">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo" runat="server" Text='<%# Eval("MONEDATYPE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total USD">
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subtotal">
                    <ItemTemplate>
                        <asp:Label ID="lb_subtotal" runat="server" Text='<%# Eval("SUBTOTAL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Impuesto">
                    <ItemTemplate>
                        <asp:Label ID="lb_impuesto" runat="server" Text='<%# Eval("IMPUESTO") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate>
                        <asp:Label ID="lb_total" runat="server" Text='<%# Eval("TOTAL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
        
        <asp:Button ID="bt_aceptar" runat="server" onclick="bt_aceptar_Click" 
            Text="Aceptar Factura" />
        
        <asp:Button ID="bt_imprimir" runat="server" Enabled="False" 
            onclick="bt_imprimir_Click" Text="Imprimir" />
        
                </td>
            </tr>
        </table>
</div>
</asp:Content>

