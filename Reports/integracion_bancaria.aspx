<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="integracion_bancaria.aspx.cs" Inherits="Reports_integracion_bancaria" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">


<script language="javascript" type="text/javascript">
    function mpeCuentaOnCancel() {

    }
    function mpeClienteOnCancel() {

    } 
    </script>
    <table width="650">
    <tr>
        <td>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <table width="300" align="center">
            <thead>
                <tr>
                <td align="center" colspan="2">
                    <b><span style="font-size: small">&nbsp;Reporte Integracion Bancaria</span></b></td>
                </tr>
            </thead>
                <tr>
                <td>Fecha Inicial</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" runat="server" ></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechainicial" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
                <tr>
                <td>Fecha Final&nbsp;</td>
                <td>
                    <asp:TextBox ID="tb_fechafinal" runat="server" ></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechafinal_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechafinal" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
                <tr><td>Banco: </td><td>
        
                                    <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                </asp:DropDownList>
        
                </td></tr>
                <tr><td>Cuenta Bancaria: </td><td>
        
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server">
                </asp:DropDownList>
        
        </td></tr>
        <tr>
            <td>Moneda</td>
             <td>
                <asp:DropDownList ID="lb_moneda" runat="server">
                 </asp:DropDownList>
                </td>
            </tr>
            <tr>
                 <td>  <asp:Label ID="l_contabilidad" runat="server" Text="Tipo Contabilidad"></asp:Label></td>
                     <td>
                    <asp:DropDownList ID="lb_contabilidad" runat="server">
                    </asp:DropDownList>
                 </td>
                </tr>
                <tr>
                 <td colspan="2" align="center">
                     <asp:Label ID="Label2" runat="server" Text="Cuentas Filtradas" Visible="False"></asp:Label>
                    <asp:GridView ID="gv_bancos" runat="server" Visible="False">
                    </asp:GridView>
                </td>
                </tr>
                <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btn_generar" runat="server" Text="Generar" 
                        onclick="btn_generar_Click" />
                    <asp:Button ID="btn_seleccionar_banco" runat="server" Text="Filtrar..." onclick="btn_seleccionar_banco_Click" 
                         />
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </td>
            </tr>

            </table>
        </td>
    </tr>
</table>
  <asp:Panel ID="pnlBancos" runat="server" CssClass="CajaDialogo" 
        Width="722px" style="display:none">
            <div><table>
                <tr><td colspan="2" align="center">Filtrar por</td></tr>
                <tr>
                    <td>Banco: <asp:DropDownList ID="ddl_bancos_panel" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" 
                            onselectedindexchanged="ddl_bancos_panel_SelectedIndexChanged">
                </asp:DropDownList></td>
                </tr>
                <tr>
                    <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
                
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Cuenta" />
            </div>
            <div>
                <asp:GridView ID="gv_cuentas" runat="server">
                  <Columns>
                    <asp:TemplateField HeaderText="Seleccionar">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </ItemTemplate>
            </asp:TemplateField>
        </Columns>
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnBancoCancelar" runat="server" Text="Cancelar" />
                <td><asp:Button ID="bt_agregar" runat="server" Text="Agregar bancos" onclick="bt_agregar_Click" /></td>
            </div>
        </asp:Panel>
          <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" 
            PopupControlID="pnlBancos" CancelControlID="btnBancoCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" 
            TargetControlID="Label1" />
</asp:Content>

