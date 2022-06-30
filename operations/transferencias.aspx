<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="transferencias.aspx.cs" Inherits="operations_pop_cheques" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
    <h3 id="adduser">TRANSFERENCIAS BANCARIAS</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
<script  type="text/javascript">
    function BloquearPantalla() {
        $.blockUI({ message: '<h1>Procesando...</h1>' });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI();
        });
    }
    </script>
    <table align="center" cellpadding="0" cellspacing="0" style="width: 93%">
        <tr>
            <td>
                <strong>Tipo Transaccion</strong></td>
            <td>            
                <asp:DropDownList ID="lb_transaccion" runat="server" Height="22px" 
                    Width="315px" onselectedindexchanged="lb_transaccion_SelectedIndexChanged" 
                    AutoPostBack="True">
                </asp:DropDownList>
                
                </td>
            <td>
                <strong>Tipo de Persona</strong></td>
            <td>
                
                <asp:DropDownList ID="lb_tipopersona" runat="server" Enabled="false">
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                    <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                </asp:DropDownList>
                </td>
        </tr>
        <tr>
            <td align="center" colspan="4" style="height: 18px">
                <table align="center" cellpadding="0" style="width: 35%">
                    <tr>
                        <td>
                            <strong>Codigo</strong></td>
                        <td>
                            <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px" ReadOnly="True">0</asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                Nombre</td>
            <td colspan="3">
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                    Width="377px" ReadOnly="True"></asp:TextBox>
                        <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                    </td>
        </tr>
        <tr>
            <td>
                Direccion</td>
            <td>
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" Width="326px" 
                    ReadOnly="True"></asp:TextBox>
                    </td>
            <td>
                Teléfono</td>
            <td>
                    <asp:TextBox ID="tb_telefono" runat="server" Height="16px" Width="96px" 
                    ReadOnly="True"></asp:TextBox>
                    </td>
        </tr>
        <tr>
            <td>
                Correo</td>
            <td colspan="3">
                <asp:TextBox 
                    ID="tb_correoelectronico" runat="server" 
                        Height="16px" Width="231px" ReadOnly="True"></asp:TextBox>
                <asp:TextBox ID="tb_contacto" 
                    runat="server" Height="16px" 
                        Width="100px" ReadOnly="True" Visible="False"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#99FFCC">
            <td>
                <strong>Banco</strong></td>
            <td>
                <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                </asp:DropDownList>
                </td>
            <td>
                <strong>Cuenta Bancaria</strong></td>
            <td>
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" 
                    onselectedindexchanged="lb_cuentas_bancarias_SelectedIndexChanged" 
                    AutoPostBack="True">
                </asp:DropDownList>
                </td>
        </tr>
        <tr>
            <td bgcolor="#99FFCC">
                <strong>Número</strong></td>
            <td bgcolor="#99FFCC">
            <asp:TextBox ID="tb_chequeNo" 
                    runat="server" Height="16px" Width="129px" MaxLength="49"></asp:TextBox>
                </td>
            <td bgcolor="#99FFCC">
                <strong>Moneda</strong></td>
            <td bgcolor="#99FFCC">
                <asp:DropDownList ID="lb_moneda" runat="server" Enabled="False">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <table align="center" cellpadding="0" style="width: 40%">
                    <tr>
                        <td>
                            <strong>Fecha de Emision</strong></td>
                        <td>
                            <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="128px"></asp:TextBox>
                <cc1:MaskedEditExtender ID="tb_fecha_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha">
                </cc1:MaskedEditExtender>
                <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha" Format="MM/dd/yyyy">
                </cc1:CalendarExtender> 
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                Acreditado</td>
            <td colspan="3">
                <asp:TextBox ID="tb_acreditado" runat="server" 
                    Height="16px" Width="457px"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td>
                Concepto</td>
            <td colspan="3">
                <asp:TextBox ID="tb_motivo" runat="server" Height="16px" Width="484px"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td>
                Referencia</td>
            <td colspan="3">
                <asp:TextBox ID="tb_referencia" runat="server" Height="16px" Width="484px"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td>
                Observaciones</td>
            <td colspan="3">
                <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" Width="484px"></asp:TextBox>

                </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Button ID="btn_buscar_soas" runat="server" onclick="btn_buscar_soas_Click" 
                    Text="Buscar Cortes" Font-Bold="True" Height="30px" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
            <table width="650">
                <thead>
                <tr>
                    <th>Documentos para pagar con esta Transferencia</th>
                </tr>
                </thead>
                <tr><td><asp:GridView ID="gv_cortes" runat="server" Width="623px">
        <Columns>
            <asp:TemplateField HeaderText="Seleccione">
                <ItemTemplate>
                    <asp:CheckBox ID="chk_seleccion" runat="server" AutoPostBack="True" 
                        oncheckedchanged="chk_seleccion_CheckedChanged" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView></td>
                </tr>
                <tr><td>
                    <asp:Label ID="lb_anticipo" runat="server" Text="Monto del anticipo:" 
                        Visible="False"></asp:Label>
                    &nbsp;<asp:TextBox ID="tb_anticipo" runat="server" AutoPostBack="True" Height="16px" 
                        ontextchanged="tb_anticipo_TextChanged" Visible="False" Width="104px" 
                        Text="0.00"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="tb_anticipo" 
                        ErrorMessage="Error, solo debe ser en formato ##.##" SetFocusOnError="True" 
                        ValidationExpression="\d+.\d{2}$">El formato permitido es ##.##</asp:RegularExpressionValidator>
                    </td>
                </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <table align="center" style="width: 55%">
                    <tr>
                        <td>
                            <strong>Cargos Adicionales</strong></td>
                        <td>
                <asp:TextBox ID="tb_cargoadicional" 
                    runat="server" Height="16px" Width="129px"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="tb_cargoadicional_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" TargetControlID="tb_cargoadicional" FilterType="Numbers,Custom" ValidChars=".">
                </cc1:FilteredTextBoxExtender>
                        </td>
                        <td>
                <asp:Button ID="btn_calcular" runat="server" onclick="btn_calcular_Click" 
                    Text="Agregar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr bgcolor="#99FFCC">
            <td align="center" colspan="4" bgcolor="#99FFCC">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 70%">
                    <tr>
                        <td>
            
                            <strong>Total a Transferir</strong></td>
                        <td>
                            <asp:TextBox ID="tb_valor" runat="server" Height="16px" Width="128px" 
                    ReadOnly="True">0</asp:TextBox>
                    <asp:TextBox ID="tb_valor_equivalente" runat="server" Height="16px" Width="128px" Visible=false>0</asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
        Retenciones a aplicar:
            <asp:Label ID="lb_provision_retencion" runat="server" Text='0' Visible="false"/>
            <asp:GridView ID="gv_retenciones" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_id" runat="server" Text='<%# Eval("ret_id") %>' Visible="false"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Provision">
                        <ItemTemplate>
                            <asp:Label ID="lb_prov" runat="server" Text='<%# Eval("ret_prov") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Monto Provision">
                        <ItemTemplate>
                            <asp:Label ID="lb_prov_valor" runat="server" Text='<%# Eval("ret_prov_valor") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre Retencion">
                        <ItemTemplate>
                            <asp:Label ID="lb_nombre" runat="server" Text='<%# Eval("ret_nombre") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Monto Retencion">
                        <ItemTemplate>
                            <asp:Label ID="lb_monto" runat="server" Text='<%# Eval("ret_monto") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No Retencion">
                        <ItemTemplate>
                            <asp:TextBox ID="lb_referencia" runat="server" Text='<%# Eval("ret_referencia") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Provision">
                        <ItemTemplate>
                            <asp:Label ID="lb_prov_fecha" runat="server" Text='<%# Eval("ret_prov_fecha") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TCambio Provision">
                        <ItemTemplate>
                            <asp:Label ID="lb_tcambio" runat="server" Text='<%# Eval("ret_tcambio") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Transaccion">
                        <ItemTemplate>
                            <asp:Label ID="lb_transaccion" runat="server" Text='<%# Eval("ret_transaccion") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Moneda">
                        <ItemTemplate>
                            <asp:Label ID="lb_moneda" runat="server" Text='<%# Eval("ret_moneda") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                         
                </Columns>
            </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
        <table width="650">
          <thead>
              <tr>
                  <th>Poliza de Diario</th>
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
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:Button ID="bt_guardar" runat="server" Text="Guardar" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"
                    onclick="bt_guardar_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_imprimir" runat="server" onclick="btn_imprimir_Click" 
                    Text="Imprimir" Visible="False" />
            </td>
        </tr>
        </table>

        <!-- ***************************************************************************** -->
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" 
        Width="800px" style="display:none">
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
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged">
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

    <br />

    <br />
    <br />
</div>
</asp:Content>
