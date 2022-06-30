<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="notacreditoprov_directo.aspx.cs" Inherits="operations_notacreditoprov_directo" Title="AIMAR - BAW"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

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
<div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">NOTA DE CREDITO DIRECTA A PROVEEDOR</h3>
<div align="center">
<table width="650" align="center">
    <tr><td>
        <table width="650" align="center">
        <tbody>
            <tr>
                <td align="center">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
                        <tr>
                            <td align="center">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                    <tr>
                                        <td align="center" colspan="4">
                                <input id="Button_asociar_documento" type="button" value="Asociar un DOCUMENTO"  onclick="javascript:window.open('searchBL_NC.aspx', null, 'toolbar=no,resizable=no,status=no,width=400,height=600');" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Transaccion</td>
                                        <td>
                                            <asp:DropDownList ID="lb_trans" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_trans_SelectedIndexChanged" 
                                    meta:resourcekey="lb_transResource1"></asp:DropDownList>
                        <asp:Label ID="lbl_blID" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lbl_tipoOperacionID" runat="server" Text="0" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <strong>MONEDA</strong></td>
                                        <td>
                                            <asp:DropDownList ID="lb_moneda" runat="server" 
                                    meta:resourcekey="lb_monedaResource1" Font-Bold="True"></asp:DropDownList>
                    <asp:Label ID="lb_facid" runat="server" Visible="False" 
                                    meta:resourcekey="lb_facidResource1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                Serie de Documento</td>
                                        <td>
                                            <asp:DropDownList ID="lb_serie_factura" runat="server" 
                                    meta:resourcekey="lb_serie_facturaResource1" AutoPostBack="True" 
                                                onselectedindexchanged="lb_serie_factura_SelectedIndexChanged">
                    </asp:DropDownList>
                                        </td>
                                        <td>
                                            Correlativo</td>
                                        <td>
                                            <asp:TextBox ID="tb_corr" runat="server" Enabled="False" Height="16px" 
                        Width="59px" meta:resourcekey="tb_corrResource1">0</asp:TextBox>
                                <asp:Label ID="lb_fecha_hora" runat="server" Visible="False" 
                                    meta:resourcekey="lb_fecha_horaResource1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo de Proveedor</td>
                                        <td>
                                <asp:DropDownList ID="lb_tipopersona" runat="server" Enabled="False">
                                    <asp:ListItem Value="0">-</asp:ListItem>
                                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                    <asp:ListItem Value="2">Agente</asp:ListItem>
                                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                                    <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                                </asp:DropDownList>
                                        </td>
                                        <td>
                                            Codigo</td>
                                        <td>
                                            <asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True" meta:resourcekey="tbCliCodResource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo de Contribuyente</td>
                                        <td>
                                            <asp:DropDownList ID="lb_contribuyente" runat="server" 
                                    meta:resourcekey="lb_contribuyenteResource1">
                    </asp:DropDownList>
                                        </td>
                                        <td>
                                            Tipo de Operacion</td>
                                        <td>
                                            <asp:DropDownList ID="lb_imp_exp" runat="server" 
                                    AutoPostBack="True" meta:resourcekey="lb_imp_expResource1">
                    </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Nombre</td>
                                        <td colspan="3">
                                <asp:TextBox ID="tb_nombre" runat="server" ReadOnly="True" Width="545px" 
                        Height="15px" meta:resourcekey="tb_nombreResource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Nit</td>
                                        <td colspan="3">
                                <asp:TextBox ID="tb_nit" runat="server" Height="16px" 
                                    meta:resourcekey="tb_nitResource1" Width="545px" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Direccion</td>
                                        <td colspan="3">
                                <asp:TextBox ID="tb_direccion" runat="server" Height="16px" Width="545px" 
                                    meta:resourcekey="tb_direccionResource1" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Telefono</td>
                                        <td>
                    <asp:TextBox ID="tb_telefono" runat="server" Height="16px" Width="200px" 
                                    meta:resourcekey="tb_telefonoResource1" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            Correo</td>
                                        <td>
                                <asp:TextBox ID="tb_correoelectronico" runat="server" 
                        Height="16px" Width="200px" meta:resourcekey="tb_correoelectronicoResource1" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Observaciones</td>
                                        <td colspan="3">
                                <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" Width="545px" 
                                    meta:resourcekey="tb_observacionesResource1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Referencia</td>
                                        <td colspan="3">
                    <asp:TextBox ID="tb_referencia" runat="server" Height="16px" Width="545px" 
                                    meta:resourcekey="tb_referenciaResource1" MaxLength="49"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
    
                    <asp:Panel ID="pnl_operacion" runat="server" Visible="False" 
            align="center" Width="400px">
                        <table align="center" style="width: 90%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_sistema" runat="server" Font-Bold="True" Font-Italic="True"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lbl_operacion" runat="server" Font-Bold="True" 
                                        Font-Italic="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            HBL</td>
                                        <td>
                                            <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="200px" 
                                                meta:resourcekey="tb_hblResource1" ReadOnly="True"></asp:TextBox>
                                            
                                        </td>
                                        <td>
                                            MBL</td>
                                        <td>
                                            <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="200px" 
                                                meta:resourcekey="tb_mblResource1" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Routing</td>
                                        <td>
                                            <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="200px" 
                                                meta:resourcekey="tb_routingResource1" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            Contenedor</td>
                                        <td>
                                            <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" 
                        Width="200px" meta:resourcekey="tb_contenedorResource1" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                    <tr>
                                        <td colspan="6" bgcolor="#D9ECFF">
                                            Documentos del Proveedor</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Serie</td>
                                        <td>
                    <asp:TextBox ID="tb_documento_serie" runat="server" Height="16px" Width="150px" MaxLength="49"></asp:TextBox> 
                                        </td>
                                        <td>
                                            Correlativo</td>
                                        <td>
                                            <asp:TextBox ID="tb_documento_correlativo" 
                        runat="server" Height="16px" Width="100px" MaxLength="10"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="tb_documento_correlativo_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers" 
                        TargetControlID="tb_documento_correlativo">
                    </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            Fecha</td>
                                        <td>
                                            <asp:TextBox ID="tb_fechadoc" runat="server" Height="16px" 
                        Width="128px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fechadoc_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date" Enabled="True" 
                        TargetControlID="tb_fechadoc">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fechadoc_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechadoc" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
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
                <asp:RadioButtonList ID="Rb_Documento" runat="server" RepeatDirection="Horizontal" 
                                    meta:resourcekey="Rb_DocumentoResource1">
                    <asp:ListItem Value="TRUE" Selected="True" meta:resourcekey="ListItemResource5">SI</asp:ListItem>
                    <asp:ListItem Value="FALSE" meta:resourcekey="ListItemResource6">NO</asp:ListItem>
                </asp:RadioButtonList>
                                        </td>
                                        <td>
                Fecha Libro Compras:</td>
                                        <td>
                <asp:TextBox ID="tb_fecha_libro_compras" runat="server" Height="16px" Width="128px" 
                                    meta:resourcekey="tb_fecha_libro_comprasResource1"></asp:TextBox>
                <cc1:MaskedEditExtender ID="tb_fecha_libro_compras_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_libro_compras" CultureAMPMPlaceholder="" 
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="">
                </cc1:MaskedEditExtender>
                <cc1:CalendarExtender ID="tb_fecha_libro_compras_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_libro_compras" Format="MM/dd/yyyy">
                </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    </table>
                                        </td>
                                    </tr>
                                    </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
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
            EmptyDataText="No hay rubros que facturar" Width="95%" 
            ondatabound="gv_detalle_DataBound" onrowdeleting="gv_detalle_RowDeleting" 
            onrowcreated="gv_detalle_RowCreated" meta:resourcekey="gv_detalleResource1">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" 
                    meta:resourcekey="CommandFieldResource1" />
                <asp:TemplateField HeaderText="Codigo" 
                    meta:resourcekey="TemplateFieldResource5">
                    <ItemTemplate>
                        <asp:Label ID="lb_codigo" runat="server" Text='<%# Eval("ID") %>' 
                            meta:resourcekey="lb_codigoResource1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rubro" meta:resourcekey="TemplateFieldResource6">
                    <ItemTemplate>
                        <asp:Label ID="lb_rubro" runat="server" Text='<%# Eval("NOMBRE") %>' 
                            meta:resourcekey="lb_rubroResource1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo" meta:resourcekey="TemplateFieldResource7">
                    <ItemTemplate>
                        <asp:Label ID="lb_tipo" runat="server" Text='<%# Eval("TYPE") %>' 
                            meta:resourcekey="lb_tipoResource1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Moneda" 
                    meta:resourcekey="TemplateFieldResource8">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo" runat="server" Text='<%# Eval("MONEDATYPE") %>' 
                            meta:resourcekey="lb_monedatipoResource1"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="#CCFFFF" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subtotal" 
                    meta:resourcekey="TemplateFieldResource9">
                    <ItemTemplate>
                        <asp:Label ID="lb_subtotal" runat="server" Text='<%# Eval("SUBTOTAL") %>' 
                            meta:resourcekey="lb_subtotalResource1"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Impuesto" 
                    meta:resourcekey="TemplateFieldResource10">
                    <ItemTemplate>
                        <asp:Label ID="lb_impuesto" runat="server" Text='<%# Eval("IMPUESTO") %>' 
                            meta:resourcekey="lb_impuestoResource1"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total" 
                    meta:resourcekey="TemplateFieldResource11">
                    <ItemTemplate>
                        <asp:Label ID="lb_total" runat="server" Text='<%# Eval("TOTAL") %>' 
                            meta:resourcekey="lb_totalResource1"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" BackColor="#CCFFFF" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total EQ"
                    meta:resourcekey="TemplateFieldResource12">
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>' 
                            meta:resourcekey="lb_totaldolaresResource1"></asp:Label>
                    </ItemTemplate>
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
                </td>
            </tr>
        </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
        </table>
    <tr><td>
    
        &nbsp;</td></tr>
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
                    Sub-Total:</td>
                <td>
                    
                    <asp:TextBox ID="tb_subtotal" runat="server" Enabled="False" BackColor="White" 
                        BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Impuesto:</td>
                <td>
                    
                    <asp:TextBox ID="tb_impuesto" runat="server" Enabled="False" BackColor="White" 
                        BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Total:" 
                        meta:resourcekey="Label8Resource1"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_total" runat="server" Enabled="False" BackColor="White" 
                        BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;Equivalente:&nbsp;
                    </td>
                <td>
                    
                    <asp:TextBox ID="tb_totaldolares" runat="server" Enabled="False" 
                        BackColor="White" BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
        <%--********************************************************--%>
        <%--********************************************************--%> 
        <%--********************************************************--%>
            <asp:Button ID="bt_agregar" runat="server" Text="Agregar Rubro" 
            meta:resourcekey="bt_agregarResource1" />
                
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
                      AutoGenerateColumns="False" Width="90%" 
                      meta:resourcekey="gv_detalle_partidaResource1">
                      <Columns>
                          <asp:TemplateField HeaderText="CUENTA" 
                              meta:resourcekey="TemplateFieldResource1">
                              <ItemTemplate>
                                  <asp:Label ID="lb_cuenta" runat="server" Text='<%# Eval("CUENTA") %>' 
                                      meta:resourcekey="lb_cuentaResource1"></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="DESCRIPCION DE CUENTA" 
                              meta:resourcekey="TemplateFieldResource2">
                              <ItemTemplate>
                                  <asp:Label ID="lb_desc_cuenta" runat="server" Text='<%# Eval("DESC_CUENTA") %>' 
                                      meta:resourcekey="lb_desc_cuentaResource1"></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="CARGOS" 
                              meta:resourcekey="TemplateFieldResource3">
                              <ItemTemplate>
                                  <asp:Label ID="lb_cargos" runat="server" Text='<%# Eval("CARGOS") %>' 
                                      meta:resourcekey="lb_cargosResource1"></asp:Label>
                              </ItemTemplate>
                          </asp:TemplateField>
                          <asp:TemplateField HeaderText="ABONOS" 
                              meta:resourcekey="TemplateFieldResource4">
                              <ItemTemplate>
                                  <asp:Label ID="lb_abonos" runat="server" Text='<%# Eval("ABONOS") %>' 
                                      meta:resourcekey="lb_abonosResource1"></asp:Label>
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
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"
                  onclick="bt_Enviar_Click" meta:resourcekey="bt_EnviarResource1" />&nbsp;
            <input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
            <asp:Button ID="bt_imprimir" runat="server" Enabled="False" 
                onclick="bt_imprimir_Click" Text="Imprimir" Visible="False" 
                meta:resourcekey="bt_imprimirResource1" />
          </div>
    </td></tr>
    </table>
    
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="533px" 
        style="display:none;" meta:resourcekey="pnlCuentasResource1">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label1" runat="server" Text="Seleccionar Cuenta" 
                meta:resourcekey="Label1Resource1" />
        </div></td></tr>
        <tr>
            <td align="left" width="130">Tipo Servicio:</td>
            <td align="left"> 
                <asp:DropDownList ID="lb_servicio" runat="server" 
                    onselectedindexchanged="lb_servicio_SelectedIndexChanged" 
                    AutoPostBack="True" meta:resourcekey="lb_servicioResource1">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left"> Rubro:</td>
            <td align="left"> 
                <asp:DropDownList ID="lb_rubro" runat="server" 
                    meta:resourcekey="lb_rubroResource1">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left"> Total: </td>
            <td align="left">
            <asp:TextBox ID="tb_monto" runat="server" Width="93px" Height="16px">0.00</asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="tb_monto_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" TargetControlID="tb_monto" FilterType="Numbers,Custom" ValidChars=".">
                </cc1:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="tb_monto" ErrorMessage="Error ###.##" SetFocusOnError="True" 
                    ValidationExpression="\d+.\d{2}"></asp:RegularExpressionValidator>
                &nbsp;</td>
        </tr>
            <tr>
                <td align="left">
                    Moneda:</td>
                <td align="left">
                    <asp:DropDownList ID="lb_moneda2" runat="server" 
                        meta:resourcekey="lb_moneda2Resource1">
                    </asp:DropDownList>
                </td>
            </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Aceptar" meta:resourcekey="Button1Resource1" />
                <asp:Button ID="btn_siguiente_rubro" runat="server" onclick="Button1_Click" 
                    Text="Siguiente" />
                <asp:Button ID="btnCuentaCancelar" runat="server" 
                    meta:resourcekey="btnCuentaCancelarResource1" Text="Cancelar" />
            </td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlhbl" runat="server" CssClass="CajaDialogo" Width="558px" 
        style="display:none;" meta:resourcekey="pnlhblResource1">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label2" runat="server" Text="Seleccionar Cuenta" 
                meta:resourcekey="Label2Resource1" />
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_criterio" runat="server" Height="16px" 
                    meta:resourcekey="tb_criterioResource1"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="lb_tipo" runat="server" 
                    meta:resourcekey="lb_tipoResource1">
                    <asp:ListItem meta:resourcekey="ListItemResource7">LCL</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource8">FCL</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource9">ALMACENADORA</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource10">AEREO</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource11">TERRESTRE T</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="dgw1" runat="server" AllowPaging="True"
                onpageindexchanging="dgw1_PageIndexChanging" onrowcommand="dgw1_RowCommand" PageSize="10">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar" HeaderText="Seleccionar" Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
                </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bt_search" runat="server" Text="Buscar" 
                            onclick="bt_search_Click" meta:resourcekey="bt_searchResource1" />
            </td>
            <td><asp:Button ID="Button2" runat="server" Text="Cancelar" 
                    meta:resourcekey="Button2Resource1" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" style="display:none;"
        Width="800px" meta:resourcekey="pnlProveedorResource1">
            <div><table>
                <tr><td align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:<asp:TextBox ID="tb_nitb" runat="server" Height="16px" 
                            Width="131px" meta:resourcekey="tb_nitbResource1" /></td>
                </tr>
                <tr>
                    <td>Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" 
                            Width="293px" meta:resourcekey="tb_nombrebResource1" /></td>
                </tr>
                <tr>
                    <td>Codigo:<asp:TextBox ID="tb_codigo" runat="server" Height="16px" 
                            Width="293px" meta:resourcekey="tb_codigoResource1" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" 
                            onclick="Button4_Click" meta:resourcekey="bt_buscarResource1" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label9" runat="server" Text="Seleccionar Agente" 
                    meta:resourcekey="Label9Resource1" />
            </div>
            <div>
                <asp:GridView ID="gv_clientes" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onpageindexchanging="gv_clientes_PageIndexChanging" 
                    onselectedindexchanged="gv_clientes_SelectedIndexChanged" 
                    onpageindexchanged="gv_clientes_PageIndexChanged"
                    PageSize="5" meta:resourcekey="gv_clientesResource1">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" 
                    meta:resourcekey="btnProveedorCancelarResource1" />
            </div>
        </asp:Panel>
                    
    
    
    <%--********************************************************--%>
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="bt_agregar"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" DynamicServicePath="" 
        Enabled="True" />
    <cc1:ModalPopupExtender ID="modalcliente" runat="server" TargetControlID="tbClicod"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" DynamicServicePath="" 
        Enabled="True" />
    </fieldset>
    <asp:Panel ID="pnlCuentas2" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label12" runat="server" Text="Seleccionar Cuenta" />
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_criterio2" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="lb_tipo2" runat="server">
                    <asp:ListItem>LCL</asp:ListItem>
                    <asp:ListItem>FCL</asp:ListItem>
                    <asp:ListItem>ALMACENADORA</asp:ListItem>
                    <asp:ListItem>AEREO</asp:ListItem>
                    <asp:ListItem>TERRESTRE T</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="dgw12" runat="server" AllowPaging="True" 
                    onpageindexchanging="dgw12_PageIndexChanging" 
            onrowcommand="dgw12_RowCommand" meta:resourcekey="dgw12Resource1">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar2" HeaderText="Seleccionar" 
                            Text="Seleccionar" meta:resourcekey="ButtonFieldResource1" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bt_search2" runat="server" Text="Buscar" 
                            onclick="bt_search2_Click" />
            </td>
            <td><asp:Button ID="btnCuentaCancelar2" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
</div>
</div>
</asp:Content>

