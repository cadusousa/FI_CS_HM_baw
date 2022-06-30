<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="Depositos.aspx.cs" Inherits="operations_Depositos" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box" align="center">
<fieldset id="Fieldset1">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {
        
    }  

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
<h3 id="adduser">INGRESAR DEPOSITOS</h3>
<table width="650" align="center">
<tr><td align="center">
        <table cellpadding="0" cellspacing="0" style="width: 95%">
            <tr>
                <td>
                    Transaccion</td>
                <td colspan="3">
                <asp:DropDownList ID="lb_transaccion" runat="server" Height="22px" Width="350px">
                </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Banco</td>
                <td colspan="3">
                    <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="350px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Cuenta Bancaria</td>
                <td colspan="3">
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" 
                    onselectedindexchanged="lb_cuentas_bancarias_SelectedIndexChanged" 
            AutoPostBack="True" Width="350px">
                </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Moneda</strong></td>
                <td>
                    <asp:DropDownList ID="lb_moneda" runat="server" Enabled="False" Width="165px" 
                        BackColor="#66CCFF" Font-Bold="True" ForeColor="Black">
                </asp:DropDownList>
                </td>
                <td>
                    Numero de Referencia</td>
                <td>
                    <asp:TextBox ID="tb_refNo" 
                    runat="server" Height="16px" Width="150px" MaxLength="25"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Fecha</td>
                <td>
        <asp:TextBox ID="tb_fechadoc" runat="server" Height="16px" 
                        Width="150px" AutoPostBack="True" ontextchanged="tb_fechadoc_TextChanged"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fechadoc_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date" Enabled="True" 
                        TargetControlID="tb_fechadoc">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fechadoc_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechadoc" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                <td align="center">
                    Tipo de Cambio</td>
                <td align="center">
                    <asp:TextBox ID="tb_tipo_cambio" runat="server" Height="16px" ReadOnly="True" 
                        Width="70px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td align="center">
                    <strong>Monto a Depositar</strong></td>
                <td>
        <asp:TextBox ID="tb_monto" 
                    runat="server" Height="16px" Width="130px" BackColor="#66CCFF" style=" text-align:right"
                        Font-Bold="True" ForeColor="Black"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="tb_monto_FilteredTextBoxExtender" 
            runat="server" Enabled="True" TargetControlID="tb_monto" FilterType="Numbers,Custom" ValidChars=".">
        </cc1:FilteredTextBoxExtender>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    Observaciones</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" Width="450px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;</td>
            </tr>
        </table>
</td></tr>
<tr><td>
        &nbsp;</td></tr>
<tr><td>
    <table width="650">
    <tr><td>
        <h3 id="H2">Busqueda por Recibo</h3>
    </td></tr>
    <tr><td>
        Serie:   
        <asp:TextBox ID="tb_serie" runat="server" Height="16px" Width="99px"></asp:TextBox>
        Correlativo:
        <asp:TextBox ID="tb_correlativo" runat="server" Height="16px" Width="101px"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="tb_buscar" runat="server" onclick="tb_buscar_Click" 
            Text="Buscar" />
    </td></tr>
    </table>
</td></tr>
<tr><td>
    <table width="650">
    <tr><td>
        <h3 id="H1">Detalle de Recibos</h3>
    </td></tr>
    <tr><td align="center">&nbsp;<asp:GridView ID="gv_cortes" runat="server" Width="623px" 
            onrowcreated="gv_cortes_RowCreated" AllowPaging="True" 
            onpageindexchanging="gv_cortes_PageIndexChanging" PageSize="20" 
            Font-Size="X-Small">
        <Columns>
            <asp:TemplateField HeaderText="SELECCIONE">
                <ItemTemplate>
                    <asp:CheckBox ID="chk_seleccion" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br />
    </td></tr>
    <tr><td>
        <table width="650">
          <thead>
              <tr>
                  <th colspan="5">
                      Poliza de Diario</th>
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
    <tr><td align="center">
                            <table align="center" cellpadding="0" cellspacing="0" 
            style="width: 90%">
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                          <asp:GridView ID="gv_detalle_partida_diferencial" runat="server" 
                              AutoGenerateColumns="False" EmptyDataText="" Width="90%" Visible="False" 
                                Font-Size="X-Small">
                              <Columns>
                                  <asp:TemplateField HeaderText="CUENTA">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cuenta0" runat="server" Text='<%# Eval("CUENTA") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="DESCRIPCION DE CUENTA">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_desc_cuenta0" runat="server" 
                                              Text='<%# Eval("DESC_CUENTA") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="CARGOS">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cargos0" runat="server" Text='<%# Eval("CARGOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="ABONOS">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_abonos0" runat="server" Text='<%# Eval("ABONOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                              </Columns>
                          </asp:GridView>
                                    </td>
                                </tr>
                            </table>
        </td></tr>
    </table>
</td></tr>
<tr><td align="center">
    <asp:Button ID="btn_deposito" runat="server" Text="Ingresar Deposito" 
        OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"
        onclick="btn_deposito_Click" /></td></tr>
</table>
</fieldset>
</div>
</asp:Content>

