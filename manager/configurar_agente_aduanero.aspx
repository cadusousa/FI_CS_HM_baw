<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="configurar_agente_aduanero.aspx.cs" Inherits="manager_configurar_agente_aduanero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
<fieldset id="Fieldset1">
<h3 id="adduser" align="left">CONFIGURACION DE PAGOS<asp:ScriptManager 
        ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </h3>
</fieldset>
<br />
    <table align="center" cellpadding="0" cellspacing="0" style="width: 70%">
        <tr>
            <td>
                Empresa Cobra</td>
            <td>
                <asp:DropDownList ID="drp_empresa_cobra" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Empresa Paga</td>
            <td>
                <asp:DropDownList ID="drp_empresa_paga" runat="server">
                </asp:DropDownList>
                <asp:Label ID="lbl_configuracion_id" runat="server" Text="0" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Contabilidad Pago</td>
            <td>
                <asp:DropDownList ID="drp_contabilidad" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drp_contabilidad_SelectedIndexChanged">
                    <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                    <asp:ListItem Value="1">Fiscal</asp:ListItem>
                    <asp:ListItem Value="2">Financiera</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Moneda Pago</td>
            <td>
                <asp:DropDownList ID="drp_moneda" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Empresa Tipo Cambio</td>
            <td>
                <asp:DropDownList ID="drp_empresa_tipo_cambio" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Sucursal Paga</td>
            <td>
                <asp:DropDownList ID="drp_sucursal" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drp_sucursal_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Operacion</td>
            <td>
                <asp:DropDownList ID="drp_operacion" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drp_operacion_SelectedIndexChanged">
                    <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                    <asp:ListItem Value="1">Facturacion</asp:ListItem>
                    <asp:ListItem Value="2">Operaciones</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Serie Paga</td>
            <td>
                <asp:DropDownList ID="drp_serie" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Tipo persona Paga</td>
            <td>
                <asp:DropDownList ID="drp_tipo_persona" runat="server">
                    <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Linea Aerea</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Codigo persona Paga</td>
            <td>
                <asp:TextBox ID="tb_codigo_persona" runat="server"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender ID="tb_codigo_persona_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" FilterType="Numbers" 
                    TargetControlID="tb_codigo_persona">
                </ajaxToolkit:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td>
                Tipo Transaccion de Pago</td>
            <td>
                <asp:DropDownList ID="drp_tipo_transaccion" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Tipo Servicio Paga</td>
            <td>
                <asp:DropDownList ID="drp_tipo_servicio" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drp_tipo_servicio_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Rubro Paga</td>
            <td>
                <asp:DropDownList ID="drp_rubros" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Tipo Operacion</td>
            <td>
                <asp:DropDownList ID="drp_imp_exp_id" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_grabar" runat="server" Text="Grabar" 
                    onclick="btn_grabar_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_limpiar" runat="server" onclick="btn_limpiar_Click" 
                    Text="Limpiar" />
            </td>
        </tr>
        </table>
<br />
</div>
</asp:Content>

