<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="notacreditoND.aspx.cs" Inherits="operations_notacreditoND" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">NOTA DE CREDITO A NOTA DE DEBITO PROVEEDOR
                                </h3>
                                <asp:ScriptManager ID="ScriptManager2" runat="server" >
 <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
<script  type="text/javascript">
    function BloquearPantalla() {
        $.blockUI({ message: '<h1>Generando...</h1>' });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI();
        });
    }
    </script>
<div align="center">
<table width="650"  align="center">
    <tr><td align="center">
        <table width="650" align="center">
        <tbody>
            <tr>
                <td>
                    Transaccion</td>
                <td>
                    <asp:DropDownList ID="lb_transaccion" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_transaccion_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td>
                    <strong>MONEDA</strong></td>
                <td>
                    <asp:DropDownList ID="lb_moneda" runat="server" Font-Bold="True"></asp:DropDownList>
                    <asp:Label ID="lb_provID" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lb_fechacreacion" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LimitDays" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                                            <asp:Label ID="lbl_tipo_serie_caption" runat="server" 
                                                style="text-align: center" 
                        Text="Serie Nota Credito"></asp:Label>
                                        </td>
                <td>
                    <asp:DropDownList ID="lb_serie_factura" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_serie_factura_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lbl_tipo_serie" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_serie_id" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
                <td>
                    Correlativo Nota Credito</td>
                <td>
                    <asp:TextBox ID="tb_NCprov" runat="server" Height="16px" Width="53px" 
                        Enabled="False">0</asp:TextBox>
                    <asp:Label ID="lb_proveedorCCHID" runat="server" Visible="False"></asp:Label>
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
                    <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
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
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" Width="545px" ReadOnly="True"></asp:TextBox>
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
                <td align="center" colspan="4">
                                <asp:Panel ID="pnl_documento_electronico" runat="server">
                                    <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 90%">
                                        <tr>
                                            <td>
                                                Correo para Recibir Documento Electronico</td>
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
                    Nota Debito</td>
                <td>
                    <asp:TextBox ID="tb_provID" runat="server" Height="16px" Width="100px" 
                        ReadOnly="True"></asp:TextBox>
                    <asp:TextBox ID="tb_nota_debito" runat="server" Height="16px" ReadOnly="True" 
                        Width="100px"></asp:TextBox>
                </td>
                <td>
                    Referencia</td>
                <td>
                    <asp:TextBox ID="tb_referencia" runat="server" Height="16px" Width="100px"></asp:TextBox>
                    <asp:TextBox ID="tb_nd_serie" runat="server" Height="16px" Visible="False" 
                        Width="25px"></asp:TextBox>
                    <asp:TextBox ID="tb_nd_correlativo" runat="server" Height="16px" 
                        Visible="False" Width="25px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    HBL</td>
                <td>
                    <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="200px"></asp:TextBox>
                </td>
                <td>
                    MBL</td>
                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Routing</td>
                <td>
                    <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="200px"></asp:TextBox>
                </td>
                <td>
                    Contenedor</td>
                <td>
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" 
                        Width="200px"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td align="center">


        <table width="650">
        <thead>
            <tr>
                <th align="center">Estado de Nota de Debito</th>
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
                                                                        <b>Total Nota Debito</b></td>
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
                &nbsp;<asp:Label ID="lbl_anulacion" runat="server" Font-Bold="True" 
                    Text="Anular Nota de Debito con esta NC"></asp:Label>
            </td></tr>
        </tbody>
        </table>




    <!-- ***************************************************************************** -->
    <asp:Panel ID="pnl_provisiones" runat="server" ScrollBars="Both" Width="720px" 
                    Height="400px">
                    
    <table width="700">
                <thead>
                <tr>
                    <th colspan="6">Detalle de provision</th>
                </tr>
                </thead>
                <tr><td align="center">
                
                <asp:GridView ID="gv_detalle" runat="server" PageSize="30" Width="638px" 
                        Font-Size="X-Small" onrowcreated="gv_detalle_RowCreated">
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
                
                </td>
                </tr>
                </table>
                </asp:Panel>
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
    <tr><td align="center">
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
        <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Generar NC" 
                  onclick="bt_Enviar_Click"   OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" />&nbsp;
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

