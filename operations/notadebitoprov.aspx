    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="notadebitoprov.aspx.cs" Inherits="invoice_notadebito" Title="AIMAR - BAW"%>

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
<h3 id="adduser">NOTA DE DEBITO</h3>
<div align="center">
<table width="650" align="center">
    <tr><td align="center">
        <table width="650" align="center">
        <tbody>
            <tr>
                <td>
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 99%">
                        <tr>
                            <td align="center" colspan="4">
                                <input id="Button_asociar_documento" type="button" value="Asociar un DOCUMENTO"  onclick="javascript:window.open('searchBL_ND.aspx', null, 'toolbar=no,resizable=no,status=no,width=400,height=600');" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Transaccion</td>
                            <td>
                                <asp:DropDownList ID="lb_trans" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_trans_SelectedIndexChanged"></asp:DropDownList>
                        <asp:Label ID="lbl_blID" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lbl_tipoOperacionID" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_tipo_serie" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_serie_id" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <strong>MONEDA</strong></td>
                            <td>
                                <asp:DropDownList ID="lb_moneda" runat="server" Font-Bold="True"></asp:DropDownList>
                    <asp:Label ID="lb_facid" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                            <asp:Label ID="lbl_tipo_serie_caption" runat="server" 
                                                style="text-align: center" Text="Serie Nota Debito"></asp:Label>
                                        </td>
                            <td>
                                <asp:DropDownList ID="lb_serie_factura" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="lb_serie_factura_SelectedIndexChanged">
                    </asp:DropDownList>
                            </td>
                            <td>
                                Correlativo</td>
                            <td>
                    <asp:TextBox ID="tb_correlativo" runat="server" Enabled="False" Height="16px" 
                        Width="50px">0</asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                                <td align="center" colspan="4">
                                   <table width="650" align="center">
                                    <thead>
                                      <tr>
                                          <th colspan="4">Referenciar Factura</th>
                                      </tr>
                                    </thead>
                                    <tr>
                                    <td style="width: 66px">Serie:</td>
                                    <td style="width: 134px"><asp:TextBox ID="tbSerieFacturaRef" ReadOnly="true" runat="server" Text=""></asp:TextBox></td>
                                    <td style="width: 86px">Correlativo:</td>
                                    <td>
                                        <asp:TextBox ID="tbCorrelativoFacturaRef" ReadOnly="true" runat="server" Text=""></asp:TextBox>
                                        <asp:TextBox ID="tbFechaFacturaRef" ReadOnly="true" runat="server" Text=""></asp:TextBox>
                                        <asp:TextBox ID="tbDocFacturaRef" ReadOnly="true" runat="server" Text=""></asp:TextBox>
                                        <asp:HiddenField ID="hdIdFacturaRef" runat="server" />
                                    </td>
                                    </tr>
                                   </table> 
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
                        Width="82px" ReadOnly="True">0</asp:TextBox>
                                <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
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
                                <asp:DropDownList ID="lb_imp_exp" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                                <asp:Label ID="lbl_hbl_eliminado" runat="server" Visible="False"></asp:Label>
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
                                Nit</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_nit" runat="server" Height="15px" Width="545px" 
                                    ReadOnly="True"></asp:TextBox>
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
                                Telefono</td>
                            <td>
                    <asp:TextBox ID="tb_telefono" runat="server" Height="16px" Width="200px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td>
                                Correo</td>
                            <td>
                                <asp:TextBox ID="tb_correoelectronico" runat="server" 
                        Height="16px" Width="200px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Observaciones</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" Width="545px"></asp:TextBox>
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
                    
                    <asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="185px"></asp:TextBox>
                                <asp:ImageButton ID="btn_borrar_hbl" runat="server" 
                                    ImageUrl="~/img/icons/delete.png" onclick="btn_borrar_hbl_Click" Width="16px" />
                            </td>
                            <td>
                    <asp:Label ID="lb_mbl" runat="server" Text="MBL"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                    <asp:Label ID="lb_routing" runat="server" Text="Routing"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                    <asp:Label ID="lb_contenedor" runat="server" Text="Contenedor"></asp:Label>
                            </td>
                            <td>
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="200px" 
                                            ReadOnly="True" style="text-transform:uppercase;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                    <tr>
                                        <td colspan="2" bgcolor="#D9ECFF">
                                            Documentos del Proveedor</td>
                                    </tr>
                                    <tr>
                                        <td>
                                Factura Referencia</td>
                                        <td>
                    <asp:TextBox ID="tb_referencia" runat="server" Height="16px" Width="200px" MaxLength="28"></asp:TextBox>
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
                <asp:RadioButtonList ID="Rb_Documento" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="TRUE" Selected="True">SI</asp:ListItem>
                    <asp:ListItem Value="FALSE">NO</asp:ListItem>
                </asp:RadioButtonList>
                                        </td>
                                        <td>
                Fecha Libro Compras:</td>
                                        <td>
                <asp:TextBox ID="tb_fecha_libro_compras" runat="server" Height="16px" Width="200px"></asp:TextBox>
                <cc1:MaskedEditExtender ID="tb_fecha_libro_compras_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_libro_compras">
                </cc1:MaskedEditExtender>
                <cc1:CalendarExtender ID="tb_fecha_libro_compras_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_libro_compras" Format="MM/dd/yyyy">
                </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                <asp:RadioButtonList ID="rb_bienserv" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">Bien</asp:ListItem>
                    <asp:ListItem Value="2" Selected="True">Servicio</asp:ListItem>
                </asp:RadioButtonList>
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
    </td></tr>
    <tr><td align="center">
    
        <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="70%" 
            ondatabound="gv_detalle_DataBound" onrowdeleting="gv_detalle_RowDeleting">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" />
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
                <asp:TemplateField HeaderText="Moneda">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo" runat="server" Text='<%# Eval("MONEDATYPE") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="#CCFFFF" HorizontalAlign="Center" 
                        VerticalAlign="Middle" />
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
                    <ItemStyle BackColor="#CCFFFF" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total EQ">
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
    </td></tr>
    <tr><td>
        <table width="300" align="right">
        <thead>
            <tr>
                <th align="left" colspan="2">Totales</tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    <asp:Label ID="Label14" runat="server" Text="Total Equivalente"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_totaldolares" runat="server" Height="16px" ReadOnly="True">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Sub-Total"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_subtotal" runat="server" Height="16px" ReadOnly="True">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Impuesto"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_impuesto" runat="server" Height="16px" ReadOnly="True">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Total"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_total" runat="server" Height="16px" ReadOnly="True">0</asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
        <%--********************************************************--%><asp:Button 
            ID="bt_agregar" runat="server" Text="Agregar Rubro" />
        </td></tr>
    <tr>
                <td>
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
    <tr>
                <td>
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
    <tr>
                <td align="center">
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
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"
                  onclick="bt_Enviar_Click" />&nbsp;
            <input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
            <asp:Button ID="bt_nota_debito_virtual" runat="server" 
                onclick="bt_nota_debito_virtual_Click" Text="Nota Debito Virtual" 
                Visible="False" />
          &nbsp;<asp:Button ID="bt_impresion_debitnote" runat="server" 
                onclick="bt_impresion_debitnote_Click" Text="Impresion Debit Note" 
                Visible="False" />
            <br />
            <asp:Button ID="bt_nd_intrcompany" runat="server" 
                onclick="bt_nd_intrcompany_Click" Text="Nota Debito Intercompany" 
                Visible="False" />
          &nbsp;<asp:Button ID="bt_imprimir" runat="server"
          onclick="bt_imprimir_Click" Text="Imprimir" Visible="False" />
          </div>
    </td></tr>
    </table>
    
    
    <%--********************************************************--%>
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none;">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label1" runat="server" Text="Seleccionar Cuenta" />
        </div></td></tr>
        <tr>
            <td align="left" width="130">Tipo Servicio:</td>
            <td align="left"> 
                <asp:DropDownList ID="lb_servicio" runat="server" 
                    onselectedindexchanged="lb_servicio_SelectedIndexChanged" 
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left"> Rubro:</td>
            <td align="left"> 
                <asp:DropDownList ID="lb_rubro" runat="server">
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
            </td>
        </tr>
        <tr>
            <td align="left">
                Moneda:</td>
            <td align="left">
                <asp:DropDownList ID="lb_moneda2" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                        Text="Aceptar" />
                </td>
                <td>
                    <asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" />
                </td>
            </tr>
        </table>
        </div>
    </asp:Panel>
    <%--********************************************************--%><%--********************************************************--%>    <%--********************************************************--%>
    <asp:Panel ID="pnlhbl" runat="server" CssClass="CajaDialogo" Width="558px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label2" runat="server" Text="Seleccionar Cuenta" />
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_criterio" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="lb_tipo" runat="server">
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
                <asp:GridView ID="dgw1" runat="server" AllowPaging="True" 
                    onpageindexchanging="dgw1_PageIndexChanging" onrowcommand="dgw1_RowCommand" 
                    PageSize="10">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bt_search" runat="server" Text="Buscar" 
                            onclick="bt_search_Click" />
            </td>
            <td><asp:Button ID="Button2" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    
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
                    onpageindexchanging="dgw12_PageIndexChanging" onrowcommand="dgw12_RowCommand" 
                    PageSize="10">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar2" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
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
    
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" style="display:none;"
        Width="800px">
            <div><table>
                <tr><td align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:<asp:TextBox ID="tb_nitb" runat="server" Height="16px" 
                            Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" 
                            Width="293px" /></td>
                </tr>
                <tr>
                    <td>Codigo:<asp:TextBox ID="tb_codigo" runat="server" Height="16px" 
                            Width="293px" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" 
                            onclick="Button4_Click" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label9" runat="server" Text="Seleccionar Agente" />
            </div>
            <div>
                <asp:GridView ID="gv_clientes" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onpageindexchanging="gv_clientes_PageIndexChanging" 
                    onselectedindexchanged="gv_clientes_SelectedIndexChanged" 
                    onpageindexchanged="gv_clientes_PageIndexChanged"
                    PageSize="5">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlFacturas" runat="server" CssClass="CajaDialogo" Width="800px" style=" display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label13" runat="server" Text="Agregar Factura" />
        </div></td></tr>
        <tr>
            <td align="left" width="130">Serie:</td>
            <td align="left"> 
                <asp:DropDownList ID="ddlSerieFacBusqueda" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left"> Correlativo:</td>
            <td align="left"> 
                <asp:TextBox ID="tbCorrelativoFacBusqueda" runat="server" Height="16px" Text=""></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btBuscarFactura" runat="server" Text="Buscar" 
                    onclick="btBuscarFactura_Click" />
            </td>
            <td><asp:Button ID="btnCancelarFacturas" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label25" runat="server" Text="Facturas" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_facturas" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_facturas_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_facturas_PageIndexChanged" 
                onpageindexchanging="gv_facturas_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
            
            
        
     </asp:Panel>
                    
    <%--********************************************************--%>
    <cc1:ModalPopupExtender ID="modalFacturas" runat="server" TargetControlID="tbSerieFacturaRef"
            PopupControlID="pnlFacturas" CancelControlID="btnCancelarFacturas"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="bt_agregar"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <cc1:ModalPopupExtender ID="modalcliente" runat="server" TargetControlID="tbClicod"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    </fieldset>
    </div>
</div>
</asp:Content>


