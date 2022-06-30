<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="view_invoice.aspx.cs" Inherits="invoice_viewrcpt" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box" align="center">
    <fieldset id="Fieldset1">
    <h3 id="adduser">NOTA CREDITO A FACTURA</h3>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
            <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
        </Scripts>
        </asp:ScriptManager>
        <script  type="text/javascript">
            function BloquearPantalla() {
                if (document.getElementById('aspnetForm').checkValidity()) {
                    $.blockUI({ message: '<h1>Procesando...</h1>' });
                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    prm.add_endRequest(function () {
                        $.unblockUI();
                    });
                }
            }
        </script>
        <script type="text/javascript">
            document.onkeydown = function () {

                if (window.event && (window.event.keyCode == 27)) {
                    window.event.keyCode = 505;
                    alert("Tecla Esc deshabilitada");
                }
                if (window.event.keyCode == 505) {
                    return false;
                }
            } 
        </script>

    <table width="650" align="center">
    <tr><td>
        <table width="650" align="center">
        <tbody>
            <tr>
                <td>
                    Transaccion</td>
                <td>
                    <asp:DropDownList ID="lb_tipo_transaccion" runat="server" 
                        Enabled="False">
                    </asp:DropDownList>
                    </td>
                <td>
                    <strong>MONEDA</strong></td>
                <td>
                    <asp:DropDownList ID="lb_moneda" runat="server" 
                        Enabled="False" Font-Bold="True">
                    </asp:DropDownList>
                    <asp:TextBox ID="tb_agenteid" runat="server" Visible="False" Width="35px" 
                        Text="0" Height="16px"></asp:TextBox>
                    <asp:TextBox ID="tb_comix_agente" runat="server" Visible="False" Width="35px" 
                        Text="0" Height="16px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    <strong>Nota de Credito -</strong> <asp:Label ID="lbl_tipo_serie_caption" runat="server" Text="Serie:"></asp:Label>
                    </td>
                <td>
                    <asp:DropDownList ID="lb_serieNC" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_serieNC_SelectedIndexChanged" Width="120px">
                    </asp:DropDownList>
                    </td>
                <td>
                    Correlativo Nota Credito</td>
                <td>
                    <asp:TextBox ID="tb_corrNC" runat="server" 
                        Enabled="false" Width="120px" Text="0" Height="16px"></asp:TextBox>
                    <asp:Label ID="lb_mbl" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lb_routing" runat="server" Visible="False"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td>
                    Serie Factura</td>
                <td>
                    <asp:DropDownList ID="lb_serie_factura" runat="server" Width="120px" 
                        Enabled="False">
                    </asp:DropDownList>
                    </td>
                <td>
                    Correlativo Factura</td>
                <td>
                    <asp:TextBox ID="lb_facid" 
                        runat="server" Enabled="false" Width="120px" Text="0" Height="16px"></asp:TextBox>
                    <asp:Label ID="id_shipper" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_consignee" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lb_tipocobro" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lbl_tipo_serie" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_internal_reference" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_serie_id" runat="server" Text="0" Visible="False"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td>
                    Fecha Factura</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_fecha" runat="server" Height="16px" 
                        Width="200px" ReadOnly="True"></asp:TextBox>
                    <asp:TextBox ID="tb_doc" runat="server" Height="16px" 
                        Width="200px" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Tipo de Persona</td>
                <td>
                                <asp:DropDownList ID="drp_tipopersona" runat="server" 
                        Enabled="False">
                                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                                    <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                                </asp:DropDownList>
                    </td>
                <td>
                    Codigo</td>
                <td>
                    <asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True"></asp:TextBox>
                    <asp:Label ID="lbl_NC_ID" runat="server" Visible="False"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td>
                    Tipo de Contribuyente</td>
                <td>
                    <asp:DropDownList ID="lb_contribuyente" runat="server">
                    </asp:DropDownList>
                    </td>
                <td>
                    Tipo de Operacion</td>
                <td>
                    <asp:DropDownList ID="lb_imp_exp" runat="server">
                    </asp:DropDownList>
                    <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LimitDays" runat="server" Visible="False"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td>
                    Nombre</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_nombre" runat="server" ReadOnly="True" Width="545px" 
                        Height="15px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Razon</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_razon" runat="server" Height="15px" Width="545px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Nit</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_nit" runat="server" ReadOnly="True" Height="15px" 
                        Width="545px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                                <asp:Panel ID="pnl_tipo_identificacion_cliente" runat="server" Visible="False">
                                    <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 50%">
                                        <tr>
                                            <td style="font-weight: bold">
                                                Tipo de Identificacion</td>
                                            <td>
                                                <asp:DropDownList ID="drp_tipo_identificacion_cliente" runat="server" 
                                                    Enabled="False">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
            </tr>
            <tr>
                <td>
                    Direccion</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_direccion" 
                        runat="server" Height="15px" Width="545px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Observaciones</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_observaciones" runat="server" Height="15px" 
                        Width="545px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Ref #</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_referencia" runat="server" Height="15px" Width="545px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                                <asp:Panel ID="pnl_documento_electronico" runat="server">
                                    <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 90%">
                                        <tr>
                                            <td>
                                                Correo para Recibir Nota de Credito Electronica</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_correo_documento_electronico" runat="server" 
                                                    style="font-weight: 700"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                        <tr>
                                            <td align="center">
                                                Persona que refiere el correo</td>
                                            <td align="center">
                                                <asp:TextBox ID="tb_referencia_correo" runat="server" Height="16px" 
                                                    MaxLength="80" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                    </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label>
                    </td>
                <td>
                    <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                    </td>
                <td>
        <asp:Label ID="lb_mbl0" runat="server" Text="MBL"></asp:Label>
                    </td>
                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
        <asp:Label ID="lb_routing0" runat="server" Text="Routing"></asp:Label>
                    </td>
                <td>
                    <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                    </td>
                <td>
        <asp:Label ID="lb_contenedor" runat="server" Text="Contenedor"></asp:Label>
                    </td>
                <td>
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td style="height: 18px">
                    Naviera</td>
                <td style="height: 18px">
                    <asp:TextBox 
                                            ID="tb_naviera" runat="server" Height="15px" 
                        Width="250px" ReadOnly="True"></asp:TextBox>
                    </td>
                <td style="height: 18px">
                    Vapor</td>
                <td style="height: 18px">
                    <asp:TextBox ID="tb_vapor" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Shipper</td>
                <td>
                                        <asp:TextBox ID="tb_shipper" runat="server" Height="15px" 
                        Width="250px" ReadOnly="True"></asp:TextBox>
                    </td>
                <td>
                    Consignee</td>
                <td>
                    <asp:TextBox ID="tb_consignee" runat="server" Height="15px" Width="250px" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Comodity</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_comodity" runat="server" 
                                            Height="15px" Width="250px" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Paquetes</td>
                <td>
                    <asp:TextBox ID="tb_paquetes1" runat="server" Height="15px" Width="35px" 
                        ReadOnly="True"></asp:TextBox>
                                        <asp:TextBox ID="tb_paquetes2" runat="server" Height="15px" 
                        Width="80px" ReadOnly="True"></asp:TextBox>
                    </td>
                <td colspan="2">
                                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                                        <tr>
                                                            <td>
                                                                Peso</td>
                                                            <td>
                    <asp:TextBox ID="tb_peso" runat="server" Height="15px" Width="50px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                Volumen</td>
                                                            <td>
                                                                <asp:TextBox ID="tb_vol" runat="server" Height="15px" Width="50px" 
                                                                    ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                    </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                                                        <tr>
                                                            <td>
                                                                Vendedor:</td>
                                                            <td>
                    <asp:TextBox ID="tb_vendedor1" runat="server" Height="15px" Width="150px" ReadOnly="True"></asp:TextBox>
                                        
                                                            </td>
                                                            <td>
                                        
                                                                <asp:TextBox ID="tb_vendedor2" runat="server" Width="150px" Height="15px" 
                                                                    ReadOnly="True"></asp:TextBox>
                                        
                                                            </td>
                                                        </tr>
                                                    </table>
                    </td>
            </tr>
            <tr>
                <td>
                    Poliza Aduanal</td>
                <td>
                                        <asp:TextBox ID="tb_poliza" runat="server" Height="16px" 
                        ReadOnly="True" Width="250px"></asp:TextBox>
                    </td>
                <td>
                    Orden/PO:</td>
                <td>
                    <asp:TextBox ID="tb_orden" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Dua Ingreso</td>
                <td>
                    <asp:TextBox ID="tb_dua_ingreso" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
                <td>
                    Dua Salida</td>
                <td>
                    <asp:TextBox ID="tb_dua_salida" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    ALL IN</td>
                <td colspan="3">
                                                    <asp:TextBox ID="tb_allin" runat="server" Height="15px" 
                        Width="550px" MaxLength="65" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td align="center">
    <br />
        <table width="650">
        <thead>
            <tr>
                <th align="center">Estado de Factura</th>
             </tr>
        </thead>
        <tbody>
            <tr><td align="center">
                <table align="center" cellpadding="0" style="width: 90%" border="0" 
                    cellspacing="0">
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100px">
                                                                        <b>Total Factura</b></td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_total_factura" runat="server" Text="0.00"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <b>Abonos</b></td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_total_abonos" runat="server" Text="0.00"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <b>Saldo</b></td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_saldo" runat="server" Text="0.00"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                &nbsp;<asp:CheckBox ID="chk_anulacion" runat="server" AutoPostBack="True" oncheckedchanged="chk_anulacion_CheckedChanged" />
                &nbsp;<asp:Label ID="lbl_anulacion" runat="server" Font-Bold="True" Text="Anular Factura con esta NC"></asp:Label>
            </td></tr>
        </tbody>
        </table>
                                            <br />
        <table width="650">
        <thead>
            <tr>
                <th colspan="5" align="center">Detalle de factura</th>
             </tr>
        </thead>
        <tbody>
            <tr><td>
                <asp:GridView ID="dgw" runat="server" PageSize="30" Width="638px" 
                    Font-Size="8pt" onrowcreated="dgw_RowCreated">
                        <Columns>
                            <asp:TemplateField HeaderText="Descuento a aplicar">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Height="16px" Width="70px" 
                                        ontextchanged="TextBox1_TextChanged" AutoPostBack="True">0.00</asp:TextBox>
                                        
                                    <cc1:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars="." TargetControlID="TextBox1">
                                    </cc1:FilteredTextBoxExtender>
                                        
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                        ControlToValidate="TextBox1" ErrorMessage="Error: formato  ###.##" 
                                        SetFocusOnError="True" ValidationExpression="\d+.\d{2}"></asp:RegularExpressionValidator>
                                </ItemTemplate>
                                <ItemStyle Width="70px" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descuento en Impuesto">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Height="16px" Width="70px" ReadOnly="true" Enabled="false">0.00</asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox2_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars="." TargetControlID="TextBox2">
                                    </cc1:FilteredTextBoxExtender>
                                        
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                        ControlToValidate="TextBox1" ErrorMessage="Error: formato  ###.##" 
                                        SetFocusOnError="True" ValidationExpression="\d+.\d{2}"></asp:RegularExpressionValidator>
                                    <br />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
            </td></tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td>
        <table width="300" align="right">
        <thead>
            <tr>
                <th align="left" colspan="2">Totales</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="SubTotal:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_subtotal" runat="server" ReadOnly="True" Height="16px" style="text-align:right">0</asp:TextBox>
                    <asp:TextBox ID="tb_factura_subtotal" runat="server" Height="16px" Width="50px" 
                        Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="IVA:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_iva" runat="server" ReadOnly="True" Height="16px" style="text-align:right">0</asp:TextBox>
                    <asp:TextBox ID="tb_factura_impuesto" runat="server" Height="16px" Width="50px" 
                        Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_total" runat="server" ReadOnly="True" Height="16px" style="text-align:right">0</asp:TextBox>
                    <asp:TextBox ID="tb_factura_total" runat="server" Height="16px" Width="50px" 
                        Visible="False"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
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
    </td>
    </tr>
    <tr><td>
        <asp:Panel ID="pnl_transmision_electronica" runat="server" Visible="False">
            <table width="650">
                <thead>
                    <tr>
                        <th>
                            Detalle de Transmision</th>
                    </tr>
                </thead>
                <tr>
                    <td align="center">
                        <asp:TextBox ID="tb_resultado_transmision" runat="server" Height="75px" 
                            ReadOnly="True" TextMode="MultiLine" Width="550px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        </td></tr>
    <tr><td>
    </table>
       

        <br /><br />
          <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Generar Nota Credito" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" 
                  onclick="bt_Enviar_Click"  onkeypress="return event.keyCode!=27" />&nbsp;
            <input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
            <asp:Button ID="bt_Imprimir" runat="server" Text="Imprimir" 
                  onclick="bt_Imprimir_Click" Enabled="False" />
            <asp:Button ID="bt_nota_credito_virtual" runat="server" Enabled="False" 
                onclick="bt_nota_credito_virtual_Click" Text="Nota Credito Virtual" />
              <asp:Button ID="btn_ver_pdf" runat="server" onclick="btn_ver_pdf_Click" 
                  Text="Ver PDF" Visible="False" />
          </div>
</fieldset>
</div>

</asp:Content>

