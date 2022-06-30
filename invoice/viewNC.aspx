<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="viewNC.aspx.cs" Inherits="invoice_viewrcpt" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
    <fieldset id="Fieldset1">
    <h3 id="adduser">NOTA DE CREDITO</h3>

    <table width="650">
    <tr><td>
        <table width="650" align="left">
        <tbody>
            <tr>
                <td>
                    Tipo de operacion:<asp:DropDownList ID="lb_tipo_transaccion" runat="server" 
                        Enabled="False">
                    </asp:DropDownList>
                    &nbsp;Moneda a facturar<asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br />
                    <asp:Label ID="lbl_tipo_serie_caption" runat="server" Text="Serie:"></asp:Label>
&nbsp;<asp:DropDownList ID="lb_serieNC" runat="server" Width="120px">
                    </asp:DropDownList>&nbsp;Correlativo:<asp:TextBox ID="tb_corrNC" runat="server" 
                        Enabled="false" Width="120px" Text="0"></asp:TextBox>
                    <br />
                   <asp:Label ID="Label1" runat="server" Text="Serie del Documento: "></asp:Label> 
                    <asp:DropDownList ID="lb_serie_factura" runat="server" Width="120px">
                    </asp:DropDownList>&nbsp;Correlativo del Documento:<asp:TextBox ID="lb_facid" 
                        runat="server" Enabled="false" Width="120px" Text="0"></asp:TextBox>
                    <asp:TextBox ID="tb_agenteid" runat="server" Visible="False" Width="30px" 
                        Text="0"></asp:TextBox>
                    <asp:TextBox ID="tb_comix_agente" runat="server" Visible="False" Width="30px" 
                        Text="0"></asp:TextBox>
                    <asp:Label ID="lb_mbl" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lb_routing" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="id_shipper" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_consignee" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lb_tipocobro" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lbl_tipo_serie" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_internal_reference" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_documentGUID" runat="server" Text="0" Visible="False"></asp:Label>
                    <br />
                    Codigo:<asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True"></asp:TextBox>
                    &nbsp; Fecha:<asp:TextBox ID="tb_fecha" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True"></asp:TextBox>
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
                    <br />
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
                                                    MaxLength="80" ReadOnly="True" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                    </td>
            </tr>
            <tr>
                <td>
                                        &nbsp;Naviera:<asp:TextBox ID="tb_naviera" runat="server" Height="15px" Width="85px"></asp:TextBox>
                                        &nbsp; Vapor:&nbsp;
                    <asp:TextBox ID="tb_vapor" runat="server" Height="15px" Width="85px"></asp:TextBox>
                                        &nbsp;Contenedor:
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="15px" Width="85px"></asp:TextBox>
                                        &nbsp;MBL/GUIA:
                    <asp:TextBox ID="tb_bl" runat="server" ReadOnly="True" Height="15px" 
                        Width="85px"></asp:TextBox>
                    <br />
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
                                        &nbsp;<br />
                                        Peso:
                    <asp:TextBox ID="tb_peso" runat="server" Height="15px" Width="40px"></asp:TextBox>
                                        &nbsp;Vol:<asp:TextBox ID="tb_vol" runat="server" Height="15px" Width="40px"></asp:TextBox>
                                        &nbsp; Poliza Aduanal
                                        <asp:TextBox ID="tb_poliza" runat="server" Height="16px"></asp:TextBox>
                                        <br />
                                        &nbsp;Dua Ingreso:                     <asp:TextBox ID="tb_dua_ingreso" runat="server" Height="15px" Width="100px"></asp:TextBox>
                                        &nbsp;Dua Salida:
                    <asp:TextBox ID="tb_dua_salida" runat="server" Height="15px" Width="100px"></asp:TextBox>
                                        &nbsp;Vendedor:
                    <asp:TextBox ID="tb_vendedor1" runat="server" Height="15px" Width="65px"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="tb_vendedor2" runat="server" Width="90px" Height="15px"></asp:TextBox>
                                        <br />
                                        All IN
                                                    <asp:TextBox ID="tb_allin" runat="server" Height="15px" 
                        Width="550px" MaxLength="65" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td>
    <br /><br />
        <table width="650">
        <thead>
            <tr>
                <th colspan="5">Detalle de factura</th>
             </tr>
        </thead>
        <tbody>
            <tr><td>
                <asp:GridView ID="dgw" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="90%" 
                    onrowdatabound="dgw_RowDataBound" >
            <Columns>              
                <asp:TemplateField HeaderText="Descuento a aplicar">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Height="16px" Width="70px" Text='<%# Eval("ABONO") %>' ReadOnly="true"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle Width="70px" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ID">
                    <ItemTemplate>
                        <asp:Label ID="lb_id" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="NOMBRE">
                    <ItemTemplate>
                        <asp:Label ID="lb_rubro" runat="server" Text='<%# Eval("NOMBRE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <asp:Label ID="lb_tipo" runat="server" Text='<%# Eval("TYPE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="TIPO MONEDA" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo" runat="server" Text='<%# Eval("MONEDATYPE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="SUB TOTAL">
                    <ItemTemplate>
                        <asp:Label ID="lb_subtotal" runat="server" Text='<%# Eval("SUBTOTAL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IMPUESTO">
                    <ItemTemplate>
                        <asp:Label ID="lb_impuesto" runat="server" Text='<%# Eval("IMPUESTO") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="TOTAL">
                    <ItemTemplate>
                        <asp:Label ID="lb_total" runat="server" Text='<%# Eval("TOTAL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Equivalente" ItemStyle-BackColor="#CCFFFF">
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comentarios">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_comentario" runat="server" Text='<%# Eval("COMENTARIO") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
            
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
                    &nbsp;&nbsp;<asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tb_total" runat="server" ReadOnly="True"></asp:TextBox>
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
     </td></tr>
     <tr><td>
        <asp:Panel ID="pnl_transmision_electronica" runat="server" Visible="False">
            <table width="650">
                <thead>
                    <tr>
                        <th>
                            Detalle de Transmison</th>
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
    </table>
       

        <br /><br />
          <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Re-Imprimir" 
                  onclick="bt_Enviar_Click" />
            <asp:Button ID="bt_nota_credito_virtual" runat="server" 
                onclick="bt_nota_credito_virtual_Click" Text="Nota Credito Virtual" />
              &nbsp;&nbsp;
          </div>
</fieldset>
</div>

</asp:Content>

