<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="verdetallefactura.aspx.cs" Inherits="invoice_viewrcpt" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box" align="center">
    <fieldset id="Fieldset1">
    <h3 id="adduser">FACTURACION</h3>
    <table width="650" align="center">
    <tr><td>
        <table width="650" align="center">
        <tbody>
            <tr>
                <td id="lb2">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 99%">
                        <tr>
                            <td align="center" colspan="4">
                    <asp:TextBox ID="tb_agenteid" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:TextBox ID="tb_comix_agente" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:Label ID="lb_facid" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lb_mbl" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lb_routing" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="id_shipper" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_consignee" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lbl_tipo_serie" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_internal_reference" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_documentGUID" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo de Operacion</td>
                            <td>
                                <asp:DropDownList ID="lb_tipo_transaccion" runat="server" 
                        BackColor="White" Enabled="False">
                    </asp:DropDownList>
                            </td>
                            <td>
                                Moneda a Facturar</td>
                            <td>
                                <asp:DropDownList ID="lb_moneda" runat="server" Enabled="False">
                    </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                            <asp:Label ID="lbl_tipo_serie_caption" runat="server" 
                                                style="text-align: center" Text="Serie de Factura"></asp:Label>
                                        </td>
                            <td>
                                            <asp:DropDownList ID="lb_serie_factura" runat="server" 
                        BackColor="White" Enabled="False">
                    </asp:DropDownList>
                            </td>
                            <td>
                                Correlativo</td>
                            <td>
                    <asp:TextBox ID="tb_correlativo" runat="server" Enabled="False" Height="16px" 
                        Width="100px" ReadOnly="True">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                            Fecha de Emision</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_fecha_emision" runat="server" ReadOnly="True" Width="500px" 
                        Height="15px" Font-Bold="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                            Tipo de Persona</td>
                            <td>
                                <asp:DropDownList ID="drp_tipopersona" runat="server" AutoPostBack="True" 
                                    Enabled="False">
                                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                                    <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                    <asp:ListItem Value="2">Agente</asp:ListItem>
                                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                                    <asp:ListItem Value="6">Linea Aerea</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                            Codigo de Cliente</td>
                            <td>
                                            <asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="100px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nombre</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_nombre" runat="server" ReadOnly="True" Width="500px" 
                        Height="15px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nit</td>
                            <td>
                    <asp:TextBox ID="tb_nit" runat="server" ReadOnly="True" Height="16px" 
                        Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                Registro No.</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="height: 18px">
                                Razon Social</td>
                            <td colspan="3" style="height: 18px">
                                <asp:TextBox ID="tb_razon" runat="server" Height="15px" Width="500px" 
                        ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo de Contribuyente</td>
                            <td>
                    <asp:DropDownList ID="lb_contribuyente" runat="server" Enabled="False">
                    </asp:DropDownList>
                            </td>
                            <td>
                                Tipo de Operacion</td>
                            <td>
                                <asp:DropDownList ID="lb_imp_exp" runat="server" Enabled="False">
                    </asp:DropDownList>
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
                                Observaciones</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_observaciones" runat="server" Height="15px" 
                        Width="500px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Otras Observaciones</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_observaciones2" runat="server" Height="15px" 
                        Width="500px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Panel ID="pnl_documento_electronico" runat="server" Visible="False">
                                    <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 90%">
                                        <tr>
                                            <td>
                                                Correo para Recibir Factura Electronica</td>
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
                    </table>
                    </td>
            </tr>
            <tr>
                <td>
                                        <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
                                            <tr>
                                                <td>
                                        <asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label>
                                                </td>
                                                <td>
                    <asp:TextBox ID="tb_hbl" runat="server" ReadOnly="True" Height="16px" 
                        Width="250px"></asp:TextBox>
                                                </td>
                                                <td>
        <asp:Label ID="Label3" runat="server" Text="MBL"></asp:Label>
                                                </td>
                                                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" ReadOnly="True" Height="16px" 
                        Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
        <asp:Label ID="Label4" runat="server" Text="Routing"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_orden" runat="server" Height="16px" Width="250px" 
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
        <asp:Label ID="lb_contenedor" runat="server" Text="Contenedor"></asp:Label>
                                                </td>
                                                <td>
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                        <asp:Label ID="lb_tipotranporte" runat="server" Text="Naviera:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_naviera" runat="server" Height="16px" Width="250px" 
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                        <asp:Label ID="lb_transporte" runat="server" Text="Vapor:"></asp:Label>
                                                </td>
                                                <td>
                    <asp:TextBox ID="tb_vapor" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Agente</td>
                                                <td>
                                        <asp:TextBox ID="tb_agente_nombre" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Shipper</td>
                                                <td>
                                        <asp:TextBox ID="tb_shipper" runat="server" Height="16px" 
                        Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Consignee</td>
                                                <td>
                    <asp:TextBox ID="tb_consignee" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Comodity</td>
                                                <td>
                    <asp:TextBox ID="tb_comodity" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Paquetes</td>
                                                <td>
                    <asp:TextBox ID="tb_paquetes1" runat="server" Height="16px" Width="50px" ReadOnly="True"></asp:TextBox>
                                                    <asp:TextBox ID="tb_paquetes2" runat="server" Height="16px" Width="170px" 
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                                        <tr>
                                                            <td>
                                                                Peso</td>
                                                            <td>
                    <asp:TextBox ID="tb_peso" runat="server" Height="16px" Width="50px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                Volumen</td>
                                                            <td>
                                                                <asp:TextBox ID="tb_vol" runat="server" Height="16px" Width="50px" 
                                                                    ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                                                        <tr>
                                                            <td>
                                                                Vendedor:</td>
                                                            <td>
                    <asp:TextBox ID="tb_vendedor1" runat="server" Height="16px" Width="150px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td>
                                        
                                                                <asp:TextBox ID="tb_vendedor2" runat="server" Width="150px" Height="16px" 
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
                    <asp:TextBox ID="tb_poliza_aduanal" runat="server" Height="16px" Width="250px" MaxLength="90" 
                                            ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Regimen Aduanero</td>
                                                <td>
                                        <asp:DropDownList ID="drp_regimen_aduanero" runat="server" Enabled="False">
                                        </asp:DropDownList>
                                        
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Dua de Ingreso</td>
                                                <td>
                                                    <asp:TextBox ID="tb_dua_ingreso" runat="server" Height="16px" Width="250px" 
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Dua de Salida</td>
                                                <td>
                    <asp:TextBox ID="tb_dua_salida" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Recibo de Aduana</td>
                                                <td>
                                                    <asp:TextBox ID="tb_reciboaduanal" runat="server" Height="16px" Width="250px" 
                                                        MaxLength="45" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Valor Aduanero</td>
                                                <td>
                                        <asp:TextBox ID="tb_valor_aduanero" runat="server" Height="16px" Width="250px" 
                                                        MaxLength="45" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Recibo de Agencia</td>
                                                <td>
                                        <asp:TextBox ID="tb_recibo_agencia" runat="server" Height="16px" Width="250px" 
                                                        MaxLength="45" ReadOnly="True"></asp:TextBox>

                                        
                                        
                                        
                                        
                                                </td>
                                                <td>
                                                    Numero de Factura Aduana</td>
                                                <td>
                                        <asp:TextBox ID="tb_no_factura_aduana" runat="server" Height="16px" Width="250px" 
                                                        MaxLength="50" ReadOnly="True"></asp:TextBox>

                                        
                                        
                                        
                                        
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    No. de Embarque</td>
                                                <td>
                                        <asp:TextBox ID="tb_no_embarque" runat="server" Height="16px" Width="250px" 
                                                        MaxLength="50" ReadOnly="True"></asp:TextBox>

                                        
                                        
                                        
                                        
                                                </td>
                                                <td>
                                                    Referencia</td>
                                                <td>
                    <asp:TextBox ID="tb_referencia" runat="server" Height="15px" Width="128px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Certificado de Seguros</td>
                                                <td>
                                                    <asp:TextBox ID="tb_poliza_seguros" runat="server" Height="16px" MaxLength="90" 
                                                        ReadOnly="True" Width="250px"></asp:TextBox>

                                        
                                        
                                        
                                        
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                        
                    <asp:Panel ID="Pnl_Mayan_Logistics" runat="server" Visible="False">
                    &nbsp;Ruta:
                    <asp:DropDownList ID="lb_ruta_pais" runat="server" Enabled="False">
                        <asp:ListItem>GUATEMALA</asp:ListItem>
                        <asp:ListItem>EL SALVADOR</asp:ListItem>
                        <asp:ListItem>HONDURAS</asp:ListItem>
                        <asp:ListItem>NICARGUA</asp:ListItem>
                        <asp:ListItem>COSTA RICA</asp:ListItem>
                        <asp:ListItem>PANAMA</asp:ListItem>
                        <asp:ListItem>BELICE</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <asp:DropDownList ID="lb_ruta" runat="server" Enabled="False">
                        <asp:ListItem>RUTA 1 NORTE</asp:ListItem>
                        <asp:ListItem>RUTA 1 SUR</asp:ListItem>
                        <asp:ListItem>RUTA 2</asp:ListItem>
                        <asp:ListItem>RUTA 3</asp:ListItem>
                        <asp:ListItem>RUTA 4</asp:ListItem>
                    </asp:DropDownList>

                    </asp:Panel>

                                        
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    All IN</td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="tb_allin" runat="server" Height="15px" 
                        Width="550px" MaxLength="65" ReadOnly="True" Enabled="False"></asp:TextBox>

                                        
                                                </td>
                                            </tr>
                                        </table>

                                        
                                        
                                        
                                        
                </td>
            </tr>
        </tbody>
        </table>
                                            <br />
    </td></tr>
    <tr><td align="center">
    
                    <asp:Panel ID="pnl_operacion" runat="server" Visible="False" 
            align="center" Width="400px">
                        <table align="center" style="width: 90%">
                            <tr>
                                <td align="center">
                                    &nbsp;</td>
                                <td align="center">
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>
                    </td></tr>
    <tr><td align="center">
        <table width="650">
            <thead>
                      <tr>
                          <th colspan="5">
                              Detalle de Rubros</th>
                      </tr>
                  </thead>
            <tr>
                <td align="center">
                <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="90%" 
            ondatabound="gv_detalle_DataBound" Enabled="False">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" Visible="False" />
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
                <asp:TemplateField HeaderText="Equivalente USD">
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comentarios">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_comentario" runat="server" Enabled="true" Text='<%# Eval("COMENTARIO") %>' Height="16px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
                </td>
            </tr>
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
                    
                    <asp:Label ID="Label5" runat="server" Text="Sub-Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_subtotal" runat="server" BackColor="White" 
                        BorderStyle="None" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td style="height: 25px">
                    
                    <asp:Label ID="Label9" runat="server" Text="Impuesto:"></asp:Label>
                </td>
                <td style="height: 25px">
                    
                    <asp:TextBox ID="tb_impuesto" runat="server" BackColor="White" 
                        BorderStyle="None" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label10" runat="server" Text="Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_total" runat="server" BackColor="White" BorderStyle="None" 
                        ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label11" runat="server" Text="Equivalente"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_totaldolares" runat="server" BackColor="White" 
                        BorderStyle="None" ReadOnly="True"></asp:TextBox>
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
    <tr><td>
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

        </td></tr>
    </table>


          <div align="center">
              &nbsp;<input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
              <asp:Button ID="btn_re_imprimir" runat="server" onclick="btn_re_imprimir_Click" 
                  Text="ReImprimir" />
            <asp:Button ID="bt_factura_virtual" runat="server" 
                onclick="bt_factura_virtual_Click" Text="Factura Virtual" />

            <asp:Button ID="bt_impresion_invoice" runat="server" Text="Impresion Invoice" 
                  onclick="bt_impresion_invoice_Click" Visible="false" />
          </div>
</fieldset>
</div>

</asp:Content>

