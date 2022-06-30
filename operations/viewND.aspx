<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="viewND.aspx.cs" Inherits="invoice_notadebito" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {

    } 
    function mpeClienteOnCancel()
    {

    } 
    </script>
<div id="box" align="center">
<fieldset id="Fieldset1">
    <h3 id="H1" align="left">NOTA DEBITO</h3>
<table width="650">
    <tr><td>
        <table width="630" align="left">
        <tbody>
            <tr>
                <td>
                    Tipo de Operacion</td>
                <td>
                    <asp:DropDownList ID="lb_tipo_transaccion" runat="server" 
                        Enabled="False">
                    </asp:DropDownList>
                    </td>
                <td>
                    Moneda</td>
                <td>
                    <asp:DropDownList ID="lb_moneda" runat="server" Enabled="False">
                    </asp:DropDownList>
                    </td>
            </tr>
            <tr>
                <td>
                    Serie</td>
                <td>
                    <asp:DropDownList ID="lb_serie_factura" runat="server" Enabled="False">
                    </asp:DropDownList>
                    </td>
                <td>
                    Correlativo</td>
                <td>
                    <asp:TextBox ID="tb_correlativo" runat="server" Height="16px" 
                        Width="50px" ReadOnly="True">0</asp:TextBox>
                    <asp:TextBox ID="tb_agenteid" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:TextBox ID="tb_comix_agente" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:Label ID="lb_facid" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="id_shipper" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_consignee" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_comodities" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lb_requierealias" runat="server" Visible="False" Text="0"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td>
                    Codigo&nbsp;</td>
                <td>
                    <asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="200px" ReadOnly="True"></asp:TextBox>
                    </td>
                <td>
                    NIT</td>
                <td>
                    <asp:TextBox ID="tb_nit" runat="server" ReadOnly="True" Height="15px" 
                        Width="200px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Tipo Contribuyente</td>
                <td>
                    <asp:DropDownList ID="lb_contribuyente" runat="server" Enabled="False">
                    </asp:DropDownList>
                    </td>
                <td>
                    Tipo de Operacion</td>
                <td>
                    <asp:DropDownList ID="lb_imp_exp" runat="server" AutoPostBack="True" 
                        Enabled="False">
                    </asp:DropDownList>
                    </td>
            </tr>
            <tr>
                <td>
                    Nombre</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_nombre" runat="server" Width="500px" 
                        Height="15px" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Razon</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_razon" runat="server" Height="15px" Width="500px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Direccion</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_direccion" runat="server" Height="15px" Width="500px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Obsevaciones</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_observaciones" runat="server" Height="15px" Width="500px" 
                        ReadOnly="True"></asp:TextBox>
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
        <asp:Label ID="lb_mbl" runat="server" Text="MBL"></asp:Label>
                    </td>
                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
        <asp:Label ID="lb_routing" runat="server" Text="Routing"></asp:Label>
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
                <td>
                    <asp:Label ID="lb_tipotranporte" runat="server" Text="Naviera:"></asp:Label>
                    </td>
                <td>
                    <asp:TextBox ID="tb_naviera" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
                <td>
                                        <asp:Label ID="lb_transporte" runat="server" Text="Vapor:"></asp:Label>
                    </td>
                <td>
                    <asp:TextBox ID="tb_vapor" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Agente</td>
                <td>
                                        <asp:TextBox ID="tb_agente_nombre" runat="server" 
                        Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                        
    <cc1:ModalPopupExtender ID="modalAgentes" runat="server" TargetControlID="tb_agente_nombre"
            PopupControlID="pnlAgentes" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
                                        
                    </td>
                <td>
                    Shipper</td>
                <td>
                    <asp:TextBox ID="tb_shipper" runat="server" Height="15px" 
                        Width="250px" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Consignee</td>
                <td>
                    <asp:TextBox ID="tb_consignee" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
                <td>
                    Comodity</td>
                <td>
                    <asp:TextBox ID="tb_comodity" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    Paquetes</td>
                <td>
                    <asp:TextBox ID="tb_paquetes1" runat="server" Height="15px" Width="50px" 
                        ReadOnly="True"></asp:TextBox>
                                        <asp:TextBox ID="tb_paquetes2" runat="server" Height="15px" 
                        Width="170px" ReadOnly="True"></asp:TextBox>
                    </td>
                <td colspan="2">
                                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                                        <tr>
                                                            <td>
                                                                Peso</td>
                                                            <td>
                    <asp:TextBox ID="tb_peso" runat="server" Height="15px" Width="40px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                Volumen</td>
                                                            <td>
                                                                <asp:TextBox ID="tb_vol" runat="server" Height="15px" Width="40px" 
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
                    <asp:TextBox ID="tb_orden" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
                <td>
                    Recibo de Aduana</td>
                <td>
                    <asp:TextBox ID="tb_reciboaduanal" runat="server" Height="16px" Width="250px" 
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
                    Certificado de&nbsp; Seguros</td>
                <td>
                    <asp:TextBox ID="tb_poliza_seguros" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
                <td>
                    Factura de Referencia&nbsp;</td>
                <td>
                    <asp:TextBox ID="tb_factura_referencia" runat="server" Height="15px" Width="250px" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    ALL IN</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_allin" runat="server" Height="15px" 
                        Width="500px" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;</td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td align="center">
    
        <table width="650">
            <thead>
                      <tr>
                          <th>
                              Detalle de Rubros</th>
                      </tr>
                  </thead>
            <tr>
                <td align="center">
    
        <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="90%" 
            ondatabound="gv_detalle_DataBound">
            <Columns>
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
                <asp:TemplateField HeaderText="Tipo Moneda" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo" runat="server" Text='<%# Eval("MONEDATYPE") %>'></asp:Label>
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
                <asp:TemplateField HeaderText="Equivalente">
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comentarios" Visible="false">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_comentario" runat="server" Text='<%# Eval("COMENTARIO") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
                </td>
            </tr>
        </table>
    
    </td></tr>
    <tr><td align="right">
        <table width="300" align="right">
        <thead>
            <tr>
                <th align="left" colspan="2">Totales</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    
                    <asp:Label ID="Label6" runat="server" Text="Sub-Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_subtotal" runat="server" Enabled="False" style=" text-align:right">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label7" runat="server" Text="Impuesto:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_impuesto" runat="server" Enabled="False" style=" text-align:right">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_total" runat="server" Enabled="False" style=" text-align:right">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label2" runat="server" Text="Equivalente"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_totaldolares" runat="server" Enabled="False" style=" text-align:right">0</asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>           
    </td></tr>
    <tr><td align="center">
    <table width="650">
                  <thead>
                      <tr>
                          <th colspan="5">
                              Poliza de Diario</th>
                      </tr>
                  </thead>
                  <tr>
                      <td align="center">
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
        <asp:Panel ID="pnl_intercompanys" runat="server" Visible="False">
            <table width="650">
                <thead>
                    <tr>
                        <th>
                            Detalle de Automatizacion</th>
                    </tr>
                </thead>
                <tr>
                    <td align="center">
                        <asp:TextBox ID="tb_resultado_automatizacion" runat="server" Height="75px" 
                            ReadOnly="True" TextMode="MultiLine" Width="550px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </td></tr>
    <tr><td>
        <div align="center">
            &nbsp;<asp:Button ID="bt_imprimir" runat="server" 
                onclick="bt_imprimir_Click" Text="Re-Imprimir"/>
            &nbsp;<asp:Button ID="bt_nota_debito_virtual" runat="server" 
                onclick="bt_nota_debito_virtual_Click" Text="Nota Debito Virtual" />
            <asp:Button ID="bt_impresion_debit_note" runat="server" 
                onclick="bt_impresion_debit_note_Click" Text="Impresion Debit Note" 
                Visible="false" />
            <br />
            <asp:Button ID="bt_nd_intercompany" runat="server" 
                onclick="bt_nd_intercompany_Click" Text="Nota Debito Intercompany" 
                Visible="false" />
          </div>
    </td></tr>
    </table>
</fieldset>        
</div>
</asp:Content>


