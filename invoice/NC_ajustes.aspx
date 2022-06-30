<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="NC_ajustes.aspx.cs" Inherits="invoice_viewrcpt" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
    <fieldset id="Fieldset1">
    <h3 id="adduser">AJUSTE CONTABLE<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        </h3>

    <table width="650">
    <tr><td>
        <table width="650" align="left">
        <tbody>
            <tr>
                <td>
                    Tipo de operacion:<asp:DropDownList ID="lb_tipo_transaccion" runat="server" 
                        Enabled="False">
                    </asp:DropDownList>
                    &nbsp;Moneda a facturar<asp:DropDownList ID="lb_moneda" runat="server" 
                        Enabled="False">
                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br />
                    Serie: 
                    <asp:DropDownList ID="lb_serieNC" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_serieNC_SelectedIndexChanged" Width="120px">
                    </asp:DropDownList>&nbsp;Correlativo:<asp:TextBox ID="tb_corrNC" runat="server" 
                        Enabled="false" Width="120px" Text="0" Height="16px"></asp:TextBox>
                    <br />
                    Serie de factura: 
                    <asp:DropDownList ID="lb_serie_factura" runat="server" Width="120px">
                    </asp:DropDownList>&nbsp;Correlativo de factura:<asp:TextBox ID="lb_facid" 
                        runat="server" Enabled="false" Width="120px" Text="0" Height="16px"></asp:TextBox>
                    <asp:TextBox ID="tb_agenteid" runat="server" Visible="False" Width="35px" 
                        Text="0" Height="16px"></asp:TextBox>
                    <asp:TextBox ID="tb_comix_agente" runat="server" Visible="False" Width="35px" 
                        Text="0" Height="16px"></asp:TextBox>
                    <asp:Label ID="lb_mbl" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lb_routing" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="id_shipper" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_consignee" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lb_tipocobro" runat="server" Visible="False" Text="0"></asp:Label>
                    <br />
                    Codigo:<asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True"></asp:TextBox>
                    &nbsp; Fecha:<asp:TextBox ID="tb_fecha" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True"></asp:TextBox>
                    &nbsp;<asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                    <br />
                    Nombre:<asp:TextBox ID="tb_nombre" runat="server" ReadOnly="True" Width="344px" 
                        Height="15px"></asp:TextBox>
                    &nbsp;&nbsp; Nit:&nbsp;
                    <asp:TextBox ID="tb_nit" runat="server" ReadOnly="True" Height="15px" 
                        Width="80px"></asp:TextBox>
                    <br />
                    Razon:  <asp:TextBox ID="tb_razon" runat="server" Height="15px" Width="242px" 
                        ReadOnly="True"></asp:TextBox>
                    &nbsp;<asp:DropDownList ID="lb_imp_exp" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="lb_contribuyente" runat="server">
                    </asp:DropDownList>
                    <br />
                    Dir: <asp:TextBox ID="tb_direccion" runat="server" Height="15px" Width="396px" 
                        ReadOnly="True"></asp:TextBox>
                    <br />
                    Notas: <asp:TextBox ID="tb_observaciones" runat="server" Height="15px" 
                        Width="378px"></asp:TextBox>
                    &nbsp;Ref #:&nbsp;
                    <asp:TextBox ID="tb_referencia" runat="server" Height="15px" Width="128px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                                        &nbsp;<asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label>
                                        :<asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="103px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;&nbsp;
        <asp:Label ID="lb_mbl0" runat="server" Text="MBL"></asp:Label>
                                        :<asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="129px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;&nbsp;<br />
        <asp:Label ID="lb_routing0" runat="server" Text="Routing"></asp:Label>
                                        :<asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="102px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;&nbsp;
        <asp:Label ID="lb_contenedor" runat="server" Text="Contenedor"></asp:Label>
                                        :<asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="102px" 
                                            ReadOnly="True"></asp:TextBox>
                                        <br />
                                        Naviera:<asp:TextBox ID="tb_naviera" runat="server" Height="15px" Width="85px"></asp:TextBox>
                                        &nbsp; Vapor:&nbsp;
                    <asp:TextBox ID="tb_vapor" runat="server" Height="15px" Width="85px"></asp:TextBox>
                                        &nbsp;<br />
                                        &nbsp;Shipper: 
                                        <asp:TextBox ID="tb_shipper" runat="server" Height="15px" 
                        Width="180px" ReadOnly="True"></asp:TextBox>
                                        &nbsp;Orden/PO:
                    <asp:TextBox ID="tb_orden" runat="server" Height="15px" Width="100px"></asp:TextBox>
                                        &nbsp;Consignee:
                    <asp:TextBox ID="tb_consignee" runat="server" Height="15px" Width="100px" ReadOnly="True"></asp:TextBox>
                    <br />
                                        &nbsp;Comodity:
                    <asp:TextBox ID="tb_comodity" runat="server" Height="15px" Width="215px"></asp:TextBox>
                                        &nbsp;Paquetes:
                    <asp:TextBox ID="tb_paquetes1" runat="server" Height="15px" Width="35px"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="tb_paquetes2" runat="server" Height="15px" Width="80px"></asp:TextBox>
                                        &nbsp;Peso:
                    <asp:TextBox ID="tb_peso" runat="server" Height="15px" Width="40px"></asp:TextBox>
                                        &nbsp;Vol:<asp:TextBox ID="tb_vol" runat="server" Height="15px" Width="40px"></asp:TextBox>
                                        &nbsp;<br />
                                        &nbsp;Dua Ingreso:                     <asp:TextBox ID="tb_dua_ingreso" runat="server" Height="15px" Width="100px"></asp:TextBox>
                                        &nbsp;Dua Salida:
                    <asp:TextBox ID="tb_dua_salida" runat="server" Height="15px" Width="100px"></asp:TextBox>
                                        &nbsp;Vendedor:
                    <asp:TextBox ID="tb_vendedor1" runat="server" Height="15px" Width="65px"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="tb_vendedor2" runat="server" Width="90px" Height="15px"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td>
    <br />
        <table width="650">
        <thead>
            <tr>
                <th align="center">Estado de factura</th>
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
                &nbsp;<asp:Label ID="lbl_anulacion" runat="server" Font-Bold="True" Text="Anular Factura con este Ajuste"></asp:Label>
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
                <asp:GridView ID="dgw" runat="server" PageSize="30" Width="638px">
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
                <th align="left">Totales</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="IVA:"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tb_iva" runat="server" ReadOnly="True" Height="16px"></asp:TextBox>
                    <br />
                    &nbsp;&nbsp;<asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tb_total" runat="server" ReadOnly="True" Height="16px"></asp:TextBox>
                    <br />
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
    </table>
       

        <br /><br />
          <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Generar Ajuste Contable" 
                  onclick="bt_Enviar_Click" />&nbsp;
            <input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
            <asp:Button ID="bt_Imprimir" runat="server" Text="Imprimir" 
                  onclick="bt_Imprimir_Click" Enabled="False" />
            <asp:Button ID="bt_factura_virtual" runat="server" Enabled="False" 
                onclick="bt_factura_virtual_Click" Text="NC Ajuste Virtual" />      
          </div>
</fieldset>
</div>

</asp:Content>

