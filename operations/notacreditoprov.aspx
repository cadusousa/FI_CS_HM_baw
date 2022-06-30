<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="notacreditoprov.aspx.cs" Inherits="operations_notacreditoprov" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">NOTA DE CREDITO A PROVISION DE PROVEEDOR<asp:ScriptManager 
        ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
</h3>
<div align="center">
<table width="650" align="center">
    <tr><td>
        <table width="650" align="center">
        <tbody>
            <tr>
                <td>
                    Transaccion</td>
                <td>
                    <asp:DropDownList ID="lb_transaccion" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_transaccion_SelectedIndexChanged" 
                        Enabled="False"></asp:DropDownList>
                </td>
                <td>
                    <strong>MONEDA</strong></td>
                <td>
                    <asp:DropDownList ID="lb_moneda" runat="server" Font-Bold="True"></asp:DropDownList>
                    <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lb_proveedorCCHID" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Serie</td>
                <td>
                    <asp:DropDownList ID="lb_serie_factura" runat="server" 
                        Width="120px" AutoPostBack="True" 
                        onselectedindexchanged="lb_serie_factura_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    Correlativo</td>
                <td>
                    <asp:TextBox ID="tb_NCprov" runat="server" Height="16px" Width="120px" 
                        Enabled="False">0</asp:TextBox>
                    <asp:Label ID="lb_provID" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LimitDays" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lb_fechacreacion" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Tipo de Proveedor</td>
                <td>
                    <asp:DropDownList ID="lb_tipopersona" runat="server"  Enabled="False">
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                    <asp:ListItem Value="10">Intercompany</asp:ListItem>
                </asp:DropDownList>
                </td>
                <td>
                    Codigo</td>
                <td>
                    <asp:TextBox ID="lb_proveedorID" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True"></asp:TextBox>
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
                    <asp:DropDownList ID="lb_imp_exp" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                    
                </td>
            </tr>
            <tr>
                <td>
                    Nombre</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_nombre" runat="server" ReadOnly="True" Width="520px" 
                        Height="15px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Nit</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_nit" runat="server" Height="16px" Width="520px" 
                        ReadOnly="True" />
                </td>
            </tr>
            <tr>
                <td>
                    Observaciones</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" Width="520px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Provision</td>
                <td>
                    <asp:TextBox ID="tb_provID" runat="server" Height="16px" Width="100px"></asp:TextBox>
                </td>
                <td>
                    Referencia</td>
                <td>
                    <asp:TextBox ID="tb_referencia" runat="server" Height="16px" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                    <tr>
                                        <td colspan="4" bgcolor="#D9ECFF">
                                            Libro de Compras</td>
                                    </tr>
                                    <tr>
                                        <td>
                Asignar Libro de Compras:</td>
                                        <td>
                <asp:RadioButtonList ID="Rb_Documento" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="TRUE" Selected="True">SI</asp:ListItem>
                    <asp:ListItem Value="FALSE">NO</asp:ListItem>
                </asp:RadioButtonList>
                                        </td>
                                        <td>
                Fecha Libro Compras:</td>
                                        <td>
                <asp:TextBox ID="tb_fecha_libro_compras" runat="server" Height="16px" Width="128px"></asp:TextBox>
                <cc1:MaskedEditExtender ID="tb_fecha_libro_compras_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_libro_compras">
                </cc1:MaskedEditExtender>
                <cc1:CalendarExtender ID="tb_fecha_libro_compras_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_libro_compras" Format="MM/dd/yyyy">
                </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                </td>
            </tr>
            <tr>
                <td>
                    HBL</td>
                <td>
                    <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="200px" 
                        ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                    MBL</td>
                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="200px" 
                        ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Routing</td>
                <td>
                    <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="200px" 
                        ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                    Contenedor</td>
                <td>
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" 
                        Width="200px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td>
    <!-- ***************************************************************************** -->
    <table width="650">
                <thead>
                <tr>
                    <th colspan="6">DetaDetalle de provision</th>
                </tr>
                </thead>
                <tr><td>
                
                    <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="97%" Font-Size="X-Small">
            <Columns>
                <asp:TemplateField HeaderText="Descuento">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_descuento" runat="server" AutoPostBack="True" Height="16px" 
                            ontextchanged="tb_descuento_TextChanged" Width="69px">0.00</asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="tb_descuento_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" TargetControlID="tb_descuento" FilterType="Numbers,Custom" ValidChars=".">
                        </cc1:FilteredTextBoxExtender>
                        <br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="tb_descuento" ErrorMessage="RegularExpressionValidator" 
                            SetFocusOnError="True" ValidationExpression="\d+.\d{2}">#.##</asp:RegularExpressionValidator>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descuento en Impuesto">
                                <ItemTemplate>
                                    <asp:TextBox ID="tb_descuento_impuesto" runat="server" Height="16px" Width="70px" ReadOnly="true" Enabled="false">0.00</asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="tb_descuento_impuesto_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars="." TargetControlID="tb_descuento_impuesto">
                                    </cc1:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
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
                <asp:TemplateField HeaderText="Tipo Moneda">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo" runat="server" Text='<%# Eval("MONEDATYPE") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="#CCFFFF" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total USD" >
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subtotal">
                    <ItemTemplate>
                        <asp:Label ID="lb_subtotal" runat="server"  Text='<%# Eval("SUBTOTAL") %>'></asp:Label>
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
                    <ItemStyle BackColor="#CCFFFF" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Saldo">
                    <ItemTemplate>
                        <asp:Label ID="lb_saldo" runat="server" Text='<%# Eval("SALDO") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
                
                </td>
                </tr>
                </table>
    <!-- ****************************************************************************************** -->            
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
                    <asp:Label ID="Label1" runat="server" Text="SubTotal:"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tb_subtotal" runat="server" Enabled="False" Height="16px">0</asp:TextBox>
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Impuesto:"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="tb_iva" runat="server" Enabled="False" Height="16px">0</asp:TextBox>
                    <br />
                    <asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tb_total" runat="server" Enabled="False" Height="16px">0</asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
        <%--<input id="bt_agregar" type="button" value="Agregar rubro" 
            onclick="javascript:window.open('additem.aspx', null, 'toolbar=no,resizable=no,status=no,width=400,height=300');"" />--%></td></tr>
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
        <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" 
                  onclick="bt_Enviar_Click" />&nbsp;
            <input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
            <asp:Button ID="bt_imprimir" runat="server" Enabled="False" 
                onclick="bt_imprimir_Click" Text="Imprimir" Visible="False" />
          </div>
    </td></tr>
    </table>
</div>   
</fieldset>
</div>
</asp:Content>


