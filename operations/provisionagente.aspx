<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="provisionagente.aspx.cs" Inherits="operations_provisionagente" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
<fieldset id="Fieldset1">
<h3 id="adduser" align="left">PROVISION A AGENTES</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
<script language="javascript" type="text/javascript">   
    function mpecancela_busqueda_rubro()
    {

    } 
    function mpeCuentaOnCancel()
    {

    } 
    function mpeSeleccionOnCancel()
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
<table width="650" align="center">
    <tr>
        <td>
            <table width="650" align="left">
            <tbody>
            <tr>
                <td>
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                        <tr>
                            <td align="center" colspan="4">
                            <input id="Button_asociar_documento" type="button" value="Asociar un DOCUMENTO"  onclick="javascript:window.open('searchBL_prv.aspx', null, 'toolbar=no,resizable=no,status=no,width=400,height=600');" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Transaccion</td>
                            <td>
                                <asp:DropDownList ID="drp_tipo_transaccion" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="drp_tipo_transaccion_SelectedIndexChanged">
                                </asp:DropDownList>
                        <asp:Label ID="lbl_blID" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lbl_tipoOperacionID" runat="server" Text="0" 
                        Visible="False"></asp:Label>
                            </td>
                            <td>
                                <strong>MONEDA</strong></td>
                            <td>
                                <asp:DropDownList ID="lb_moneda" runat="server" Font-Bold="True">
                    </asp:DropDownList>
                                <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Serie de Documento</td>
                            <td>
                                <asp:DropDownList ID="lb_serie_factura" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="lb_serie_factura_SelectedIndexChanged">
                    </asp:DropDownList>
                            </td>
                            <td>
                                Correlativo</td>
                            <td>
                    <asp:TextBox ID="tb_corr" runat="server" Enabled="False" Height="16px" 
                        Width="70px">0</asp:TextBox>
                                <asp:Label ID="lbl_whitelist" runat="server" Text="FALSE" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo proveedor</td>
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
                                <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px" ReadOnly="True">0</asp:TextBox>
                    
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
                                Tipo Operacion</td>
                            <td>
                    <asp:DropDownList ID="lb_imp_exp" runat="server" Width="150px">
                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label ID="lb_tipo_docto" runat="server" Text="Tipo Documento:" 
                Visible="False"></asp:Label></td><td><asp:DropDownList ID="ddl_tipo_documento" runat="server" Visible="False">
            </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                Nombre</td>
                            <td colspan="3">
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" Width="520px" 
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nit</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_nit" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Direccion</td>
                            <td colspan="3">
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" Width="520px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Telefono</td>
                            <td>
                    <asp:TextBox ID="tb_telefono" runat="server" Height="16px" Width="96px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td>
                                Correo</td>
                            <td>
                                <asp:TextBox ID="tb_correoelectronico" runat="server" 
                        Height="16px" Width="231px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Contacto</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_contacto" runat="server" Height="16px" 
                        Width="520px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Observaciones</td>
                            <td colspan="3">
                    <asp:TextBox ID="tb_observacion" runat="server" Height="16px" Width="520px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
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
                        runat="server" Height="16px" Width="100px" MaxLength="30"></asp:TextBox>
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
                    <asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label>
                            </td>
                            <td>
        <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lb_mbl" runat="server" Text="MBL"></asp:Label>
                            </td>
                            <td>
        <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lb_routing" runat="server" Text="Routing"></asp:Label>
                            </td>
                            <td>
        <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lb_contenedor" runat="server" Text="Contenedor"></asp:Label>
                            </td>
                            <td>
        <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="200px" 
                                            ReadOnly="True" style="text-transform:uppercase;"></asp:TextBox>
                                        <cc1:MaskedEditExtender ID="tb_contenedor_MaskedEditExtender" runat="server" 
                                            Mask="AAAA999999C9"  MaskType="None" Filtered="-"
                                            Enabled="True" 
                                            TargetControlID="tb_contenedor">
                                        </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Poliza Aduanal</td>
                            <td>
                    <asp:TextBox ID="tb_poliza" runat="server" Height="16px" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
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
                <asp:TextBox ID="tb_fecha_libro_compras" runat="server" Height="16px" 
                    Width="128px"></asp:TextBox>
                <cc1:MaskedEditExtender ID="tb_fecha_libro_compras_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_libro_compras">
                </cc1:MaskedEditExtender>
                <cc1:CalendarExtender ID="tb_fecha_libro_compras_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_libro_compras" 
                    Format="MM/dd/yyyy">
                </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                </td>
            </tr>
            </tbody>
            </table>
        </td>
    </tr>
    <tr><td align="center">
    
        <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="90%" 
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
                    <ItemStyle BackColor="#CCFFFF" HorizontalAlign="Center" />
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
                    <ItemStyle BackColor="#CCFFFF" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total EQ">
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
    </td></tr>
    <tr><td align="left"><asp:Button 
            ID="bt_agregar" runat="server" Text="Agregar Rubro" /></td></tr>
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
                    <asp:Label ID="Label6" runat="server" Text="Sub-Total"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_subtotal" runat="server" Height="16px" style=" text-align:right"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Impuesto" style=" text-align:right"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_impuesto" runat="server" Height="16px" style=" text-align:right"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Total"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_total" runat="server" Height="16px" style=" text-align:right"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Total Equivalente"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_totaldolares" runat="server" Height="16px" style=" text-align:right"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;<asp:TextBox ID="tb_afecto" runat="server" Height="16px" Visible=false></asp:TextBox>
                    <asp:TextBox ID="tb_noafecto" runat="server" Height="16px" Visible=false></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
                    
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" style="display:none;"
        Width="800px">
            <div><table>
                <tr><td align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:<asp:TextBox ID="tb_nitb" runat="server" Height="16px" Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td>Codigo:<asp:TextBox ID="tb_codigo" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Agente" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" onload="gv_proveedor_Load" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
                    
    </td>
    </tr>
    
    <tr>
        <td align="center">
                    
                <table width="650">
                  <thead>
                      <tr>
                          <th>
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
    
    <tr>
        <td align="center">
                    
            <asp:Button ID="bt_guardar" runat="server" onclick="bt_guardar_Click" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" 
                Text="Provisionar" ValidationGroup="enviargroup" />
                    
        </td>
    </tr>
    <tr>
        <td>
        <!-- ***************************************************************************** -->
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_agenteID"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <!-- ***************************************************************************** -->
            <%--********************************************************--%>
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label1" runat="server" Text="Seleccionar Cuenta" />
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
            <td><asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    
            <%--********************************************************--%>
    <%--********************************************************--%>
    <asp:Panel ID="pnlrubros" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none;">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label3" runat="server" Text="Seleccionar Cuenta" />
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
            <td align="left">Moneda: </td>
            <td align="left">
                <asp:DropDownList ID="lb_moneda2" runat="server">
                    </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="buscar_rubro" runat="server" onclick="buscar_rubro_Click" 
                    Text="Aceptar" />
                <asp:Button ID="btn_siguiente_rubro" runat="server" 
                    onclick="buscar_rubro_Click" Text="Siguiente" />
                <asp:Button ID="cancela_busqueda_rubro" runat="server" Text="Cancelar" />
            </td>
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
    
    <cc1:ModalPopupExtender ID="btn_rubro_ModalPopupExtender" runat="server" TargetControlID="bt_agregar"
            PopupControlID="pnlrubros" CancelControlID="cancela_busqueda_rubro"
            OnCancelScript="mpecancela_busqueda_rubro()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <%--********************************************************--%>
        </td>
    </tr>
</table>
</fieldset>
</div>
    
</asp:Content>

