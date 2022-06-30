<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="viewNC.aspx.cs" Inherits="invoice_viewrcpt" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
    <fieldset id="Fieldset1">
    <h3 id="adduser">NOTA DE CREDITO</h3>

    <table width="650" align="center">
    <tr><td>
        <table width="650" align="left">
        <tbody>
            <tr>
                <td>
                    Tipo de operacion:<asp:DropDownList ID="lb_tipo_transaccion" runat="server" 
                        Enabled="False">
                    </asp:DropDownList>
                    &nbsp;Moneda <asp:DropDownList ID="lb_moneda" runat="server" Enabled="False">
                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br />
                    Serie 
                    NC: <asp:DropDownList ID="lb_serieNC" runat="server">
                    </asp:DropDownList>&nbsp;Correlativo:<asp:TextBox ID="tb_corrNC" runat="server" 
                        Enabled="false" Width="30px" Text="0"></asp:TextBox>
                    <asp:TextBox ID="tb_agenteid" runat="server" Visible="False" Width="30px" 
                        Text="0"></asp:TextBox>
                    <asp:TextBox ID="tb_comix_agente" runat="server" Visible="False" Width="30px" 
                        Text="0"></asp:TextBox>
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
                    <br />
                    Nombre:<asp:TextBox ID="tb_nombre" runat="server" ReadOnly="True" Width="344px" 
                        Height="15px"></asp:TextBox>
                    &nbsp;Nit:&nbsp;
                    <asp:TextBox ID="tb_nit" runat="server" ReadOnly="True" Height="15px" 
                        Width="80px"></asp:TextBox>
                    &nbsp;<br />
    Notas: 
                    <asp:TextBox ID="tb_observaciones" runat="server" Height="15px" 
                        Width="470px"></asp:TextBox>
                    <br />
                    <asp:DropDownList ID="lb_imp_exp" runat="server" Visible="False">
                    </asp:DropDownList>
                    <asp:DropDownList ID="lb_contribuyente" runat="server" Visible="False">
                    </asp:DropDownList>
                    </td>
            </tr>
            <tr>
                <td>
                                        <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                            <tr>
                                                <td>
                                                    HBL</td>
                                                <td>
                    <asp:TextBox ID="tb_hbl" runat="server" Height="15px" Width="250px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    MBL</td>
                                                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" Height="15px" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Routing</td>
                                                <td>
                    <asp:TextBox ID="tb_routing" runat="server" Height="15px" Width="250px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Contendor</td>
                                                <td>
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="15px" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Referencia</td>
                                                <td>
                    <asp:TextBox ID="tb_referencia" runat="server" Height="15px" Width="250px"></asp:TextBox>
                                                </td>
                                                <td>
                              Poliza</td>
                                                <td>
                    <asp:TextBox ID="tb_poliza" runat="server" Height="15px" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
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
                <th colspan="5">Detalle</th>
             </tr>
        </thead>
        <tbody>
            <tr><td>
                <asp:GridView ID="dgw" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="90%" 
                    >
            <Columns>              
                <asp:TemplateField HeaderText="ID" Visible="False">
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

<ItemStyle BackColor="#CCFFFF"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
            
                <%--<asp:GridView ID="dgw" runat="server" PageSize="30" Width="638px">
                        <Columns>
                            <asp:TemplateField HeaderText="Descuento a aplicar">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Height="16px" Width="70px" 
                                        ontextchanged="TextBox1_TextChanged" AutoPostBack="True">0.00</asp:TextBox>
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                        ControlToValidate="TextBox1" ErrorMessage="Error: formato  ###.##" 
                                        SetFocusOnError="True" ValidationExpression="\d+.\d{2}"></asp:RegularExpressionValidator>
                                </ItemTemplate>
                                <ItemStyle Width="70px" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
            </td></tr>--%>
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
                    
                    <asp:Label ID="Label6" runat="server" Text="Sub-Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_subtotal" runat="server" Enabled="False" BackColor="White" 
                        BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 25px">
                    
                    <asp:Label ID="Label7" runat="server" Text="Impuesto:"></asp:Label>
                </td>
                <td style="height: 25px">
                    
                    <asp:TextBox ID="tb_impuesto" runat="server" Enabled="False" BackColor="White" 
                        BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_total" runat="server" Enabled="False" BackColor="White" 
                        BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label2" runat="server" Text="Equivalente"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_totaldolares" runat="server" Enabled="False" 
                        BackColor="White" BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>     </tr>
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
    </table>
       

        <br /><br />
          <div align="center">
            <asp:Button ID="bt_nota_credito_virtual" runat="server" 
                onclick="bt_nota_credito_virtual_Click" Text="Nota Credito Virtual" 
                  Visible="True" />
              &nbsp;&nbsp;
          </div>
</fieldset>
</div>

</asp:Content>

