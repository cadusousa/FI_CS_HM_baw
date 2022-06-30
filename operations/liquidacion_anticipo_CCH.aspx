<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="liquidacion_anticipo_CCH.aspx.cs" Inherits="operations_liquidacion_anticipo_CCH" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">CHEQUES</h3>
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
        <table>
        <tr><td>
            Transaccion:
                <asp:DropDownList ID="lb_transaccion" runat="server" Height="22px" 
                    Width="315px">
                </asp:DropDownList>
            <br />
            Codigo :<asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                    <br />
                    Nombre : 
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                Width="377px" ReadOnly="True"></asp:TextBox>
                    <br />Banco:                 <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;Cta Bancaria No.:
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" 
                    AutoPostBack="True" 
                onselectedindexchanged="lb_cuentas_bancarias_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                Moneda:
                <asp:DropDownList ID="lb_moneda" runat="server" Enabled="False">
                </asp:DropDownList>
                <br />
            <asp:Button ID="btn_buscar" runat="server" onclick="btn_buscar_Click" 
                Text="Buscar" />
        <br />
        </td></tr>
        <tr><td>
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
    </td></tr>
    <tr><td align="center">
            <table width="650">
                <thead>
                <tr>
                    <th colspan="6">Cheques pendientes de liquidar</th>
                </tr>
                </thead>
                <tr><td><asp:GridView ID="gv_cheques" runat="server" Width="623px">
        <Columns>
            <asp:TemplateField HeaderText="Seleccione">
                <ItemTemplate>
                    <asp:CheckBox ID="chk_cheque" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
                    <br />
                    </td>
                </tr>
                </table>
        </td></tr>
        <tr><td align="center">
            <table width="650">
                <thead>
                <tr>
                    <th colspan="6">Provisiones pendientes de liquidar</th>
                </tr>
                </thead>
                <tr><td><asp:GridView ID="gv_cortes" runat="server" Width="623px">
        <Columns>
            <asp:TemplateField HeaderText="Seleccione">
                <ItemTemplate>
                    <asp:CheckBox ID="chk_seleccion" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
                    <br />
                    </td>
                </tr>
                </table>
        </td></tr>
        <tr><td align="center">
            <table width="650">
                <thead>
                <tr>
                    <th colspan="6">Deposito de valor sobrante</th>
                </tr>
                </thead>
                </table>
        </td></tr>
        <tr>
            <td style="background-color:#99FFCC">                
                Banco:                 
                <asp:DropDownList ID="lb_bancos0" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos0_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;Cta bancaria No:
                <asp:DropDownList ID="lb_cuentas_bancarias0" runat="server" 
                    AutoPostBack="True" 
                onselectedindexchanged="lb_cuentas_bancarias0_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                Moneda:
                <asp:DropDownList ID="lb_moneda0" runat="server" Enabled="False">
                </asp:DropDownList>
                    <br />
                No. boleta:&nbsp;
                    <asp:TextBox ID="tb_boleta" runat="server" Height="16px" Width="121px"></asp:TextBox>
                &nbsp;Fecha: <asp:TextBox ID="tb_fechadoc" runat="server" Height="16px" 
                        Width="128px"></asp:TextBox>
               <cc1:MaskedEditExtender ID="tb_fechadoc_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date" Enabled="True" 
                        TargetControlID="tb_fechadoc">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fechadoc_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechadoc" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>                         
                        <br/>
                Monto:<asp:TextBox ID="tb_sobrante" 
                    runat="server" Height="16px" Width="129px"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="tb_sobrante_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" TargetControlID="tb_sobrante"
                    FilterType="Numbers,Custom" ValidChars="." > 
                </cc1:FilteredTextBoxExtender>
            
            <br />
            </td>
        </tr>
        <tr><td>
        <table width="650">
          <thead>
              <tr>
                  <th colspan="5">Poliza de Diario del Deposito</th>
              </tr>
          </thead>
          <tr>
              <td>
                  <asp:GridView ID="gv_detalle_partida" runat="server" 
                      AutoGenerateColumns="False" EmptyDataText="" Width="90%">
                      <Columns>
                          <asp:TemplateField HeaderText="CUENTA">
                              <ItemTemplate>
                                  <asp:Label ID="lb_cuenta" runat="server" Text='<%# Eval("CUENTA") %>'></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="DESCRIPCION DE CUENTA">
                              <ItemTemplate>
                                  <asp:Label ID="lb_desc_cuenta" runat="server" Text='<%# Eval("DESC_CUENTA") %>'></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="CARGOS">
                              <ItemTemplate>
                                  <asp:Label ID="lb_cargos" runat="server" Text='<%# Eval("CARGOS") %>'></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="ABONOS">
                              <ItemTemplate>
                                  <asp:Label ID="lb_abonos" runat="server" Text='<%# Eval("ABONOS") %>'></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                      </Columns>
                  </asp:GridView>
              </td>
          </tr>
      </table>
        </td></tr>
        <tr>
            <td align="center">
                <asp:Button ID="bt_guardar" runat="server" Text="Guardar" 
                    onclick="bt_guardar_Click" ValidationGroup="a" />
            </td>
        </tr>
        </table>
      </div>
</asp:Content>

