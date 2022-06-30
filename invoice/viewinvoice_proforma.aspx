<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="viewinvoice_proforma.aspx.cs" Inherits="invoice_invoice" Title="AIMAR - BAW" %>
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
    function mpeClienteOnCancel()
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
    <script type="text/javascript">
        document.onkeydown = function () {

            if (window.event && (window.event.keyCode == 27)) {
                window.event.keyCode = 505;
                alert("Tecla Esc deshabilitada");
            }
            if (window.event.keyCode == 505) {
                return false;
            }
        } 
    </script>
<div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">FACTURACION PROFORMA</h3>
<table width="650">
    <tr><td>
        <table width="700" align="left">
        <tbody>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <br />
                    Tipo de operacion:<asp:DropDownList ID="lb_tipo_transaccion" runat="server">
                    </asp:DropDownList>
                    &nbsp;Moneda a facturar<asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    <asp:Label ID="lbl_blID" runat="server" Text="0" Visible="False"></asp:Label>
                    &nbsp;<asp:Label ID="lbl_tipoOperacionID" runat="server" Text="10" 
                        Visible="False"></asp:Label>
                    <br />
                                            <asp:Label ID="lbl_tipo_serie_caption" runat="server" 
                                                style="text-align: center" Text="Serie de Factura"></asp:Label>
                                        &nbsp;<asp:DropDownList ID="lb_serie_factura2" 
                        runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_serie_factura2_SelectedIndexChanged">
                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;<asp:TextBox ID="tb_correlativo" runat="server" Enabled="False" Height="16px" 
                        Width="50px">0</asp:TextBox>
                    &nbsp;&nbsp;&nbsp; Série de proforma: <asp:DropDownList ID="lb_serie_factura" runat="server">
                    </asp:DropDownList>
                    &nbsp;<asp:TextBox ID="tb_corr_proforma" runat="server" Enabled="False" 
                        Height="16px" Width="65px">0</asp:TextBox>
                    &nbsp;<asp:TextBox ID="tb_agenteid" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:TextBox ID="tb_comix_agente" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:Label ID="lb_facid" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="id_shipper" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_consignee" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_comodities" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lb_requierealias" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lbl_serie_id" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_tipo_serie" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_internal_reference" runat="server" Text="0" Visible="False"></asp:Label>
                    <br />
                    Codigo:<asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True"></asp:TextBox>
                    Nit:&nbsp; 
                    <asp:TextBox ID="tb_nit" runat="server" ReadOnly="True" Height="15px" 
                        Width="80px"></asp:TextBox>
                    <asp:DropDownList ID="lb_imp_exp" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_imp_exp_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:DropDownList ID="lb_contribuyente" runat="server">
                    </asp:DropDownList>
                                <asp:DropDownList ID="lb_tipo_factura" runat="server" Visible="False">
                    </asp:DropDownList>
                    <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                    <br />
                    Nombre:<asp:TextBox 
                        ID="tb_nombre" runat="server" Width="500px" 
                        Height="15px" ReadOnly="True"></asp:TextBox>
                    &nbsp;&nbsp;<br />
                    Razon Social&nbsp;&nbsp;  <asp:TextBox ID="tb_razon" runat="server" 
                        Height="15px" Width="500px"></asp:TextBox>
                    &nbsp;<br />
                   Direccion&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="tb_direccion" runat="server" 
                        Height="15px" Width="500px" 
                        ReadOnly="True"></asp:TextBox>
                    <br />
                    Observaciones&nbsp;<asp:TextBox ID="tb_observaciones" runat="server" Height="15px" Width="500px" 
                        ReadOnly="True"></asp:TextBox>
                    &nbsp;&nbsp;
                    <br />
                    Otras Observaciones
                    <asp:TextBox ID="tb_otras_observaciones" runat="server" Height="15px" 
                        Width="500px" MaxLength="100"></asp:TextBox>
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
                                                    MaxLength="80" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                    </td>
            </tr>
            <tr>
                <td>
                                        <asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label>
                                        :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="tb_hbl" runat="server" 
                                            Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;&nbsp;
        <asp:Label ID="lb_mbl" runat="server" Text="MBL"></asp:Label>
                                        :<asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;&nbsp;<br />
        <asp:Label ID="lb_routing" runat="server" Text="Routing"></asp:Label>
                                        :<asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;&nbsp;<asp:Label ID="lb_contenedor" runat="server" Text="Contenedor"></asp:Label>
                                        :<asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                        <br />
                                        <asp:Label ID="lb_tipotranporte" runat="server" Text="Naviera:"></asp:Label>
                                        &nbsp;<asp:TextBox ID="tb_naviera" runat="server" Height="15px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp; 
                                        <asp:Label ID="lb_transporte" runat="server" Text="Vapor:"></asp:Label>
                                        &nbsp;
                    <asp:TextBox ID="tb_vapor" runat="server" Height="15px" Width="250px" ReadOnly="True"></asp:TextBox>
                                        &nbsp; &nbsp; 
                                        <br />
                                        Agente: 
                                        <asp:TextBox ID="tb_agente_nombre" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp; Shipper:                                         
                                        <asp:TextBox ID="tb_shipper" runat="server" Height="15px" 
                        Width="250px" ReadOnly="True"></asp:TextBox>
                                        &nbsp;<br />
                                        Consignee:
                    <asp:TextBox ID="tb_consignee" runat="server" Height="15px" Width="250px" ReadOnly="True"></asp:TextBox>
                                        &nbsp;Comodity:                      <asp:TextBox ID="tb_comodity" 
                                            runat="server" Height="15px" Width="230px"></asp:TextBox>
                                        <br />
                                        Paquetes:&nbsp;
                    <asp:TextBox ID="tb_paquetes1" runat="server" Height="15px" Width="35px" ReadOnly="True"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="tb_paquetes2" runat="server" Height="15px" Width="100px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;Peso:
                    <asp:TextBox ID="tb_peso" runat="server" Height="15px" Width="80px" ReadOnly="True"></asp:TextBox>
                                        &nbsp;Vol:<asp:TextBox ID="tb_vol" runat="server" Height="15px" Width="100px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;<br />
                                        Vendedor:
                    <asp:TextBox ID="tb_vendedor1" runat="server" Height="15px" Width="100px" ReadOnly="True"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="tb_vendedor2" runat="server" Width="110px" Height="15px" 
                                            ReadOnly="True"></asp:TextBox>
                                        <br />
                                        Dua Ingreso:                     
                                        <asp:TextBox ID="tb_dua_ingreso" runat="server" Height="15px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                        &nbsp;Dua Salida:
                    <asp:TextBox ID="tb_dua_salida" runat="server" Height="15px" Width="250px" ReadOnly="True"></asp:TextBox>
                                        &nbsp;
                                        <br />
                                        Poliza Aduanal:
                    <asp:TextBox ID="tb_orden" runat="server" Height="15px" Width="230px" ReadOnly="True"></asp:TextBox>
                                        &nbsp;Recibo de aduana:<asp:TextBox ID="tb_reciboaduanal" runat="server" 
                                            Height="16px" Width="230px" ReadOnly="True"></asp:TextBox>
                                        <br />
                                        Valor Aduanero
                                        <asp:TextBox ID="tb_valor_aduanero" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                                &nbsp; Regimen Aduanero
                                        <asp:DropDownList ID="drp_regimen_aduanero" runat="server" Enabled="False">
                                        </asp:DropDownList>
                                                &nbsp;<br />
                                        Recibo de Agencia
                                         <asp:TextBox ID="tb_recibo_agencia" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>

                                        
                                        
                                        
                                        
                                        <br />
            ALL IN:                                         <asp:TextBox ID="tb_allin" 
                                            runat="server" Height="15px" 
                        Width="378px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td align="center">
    
        <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="90%" 
            ondatabound="gv_detalle_DataBound" onrowdeleting="gv_detalle_RowDeleting" 
            Enabled="False">
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
                <asp:TemplateField HeaderText="CargoID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lb_cargoid" runat="server" Text='<%# Eval("CARGOID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comentarios">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_comentario" runat="server" Text='<%# Eval("COMENTARIO") %>' Height="16px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
    </td></tr>
    <tr><td>
        <table width="300" align="right">
        <thead>
            <tr>
                <th align="left" colspan="2" style="height: 17px">Totaless</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    
                    <asp:Label ID="Label6" runat="server" Text="Sub-Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_subtotal" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label7" runat="server" Text="Impuesto:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_impuesto" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_total" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label2" runat="server" Text="Equivalente Dolares:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_totaldolares" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
            <asp:Button 
            ID="bt_agregar" runat="server" Text="Agregar Rubro" Enabled="False" 
            Visible="False" />
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
        <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Convertir en factura" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" 
                  onclick="bt_Enviar_Click" />&nbsp;
            <asp:Button ID="bt_imprimir" runat="server" Enabled="False" 
                onclick="bt_imprimir_Click" Text="Imprimir Factura" />
            &nbsp;<input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
            <asp:Button ID="bt_reimprimir_proforma" runat="server" 
                onclick="bt_reimprimir_proforma_Click" Text="Re Imprimir Proforma" />
          &nbsp;
            <asp:Button ID="bt_proforma_virtual" runat="server" 
                onclick="bt_proforma_virtual_Click" Text="Proforma Virtual" />
          </div>
    </td></tr>
    </table>
    
    
    <%--********************************************************--%>
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none">
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
                <asp:TextBox ID="tb_monto" runat="server" Width="93px">0.00</asp:TextBox>
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
            <td>
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Aceptar" />
            </td>
            <td><asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="bt_agregar"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <%--********************************************************--%>
    <asp:Panel ID="pnlCliente" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label3" runat="server" Text="Criterio de busqueda" />
        </div></td></tr>
        <%--<tr>
            <td align="left" width="130">Tipo:</td>
            <td align="left"> 
                
                <asp:DropDownList ID="lb_tipocliente" runat="server">
                    <asp:ListItem Selected="True">Cliente</asp:ListItem>
                    <asp:ListItem>Consignee</asp:ListItem>
                    <asp:ListItem>Shipper</asp:ListItem>
                </asp:DropDownList>
                
            </td>
        </tr>--%>
        <tr>
            <td align="left" width="130">Codigo:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_codigo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" width="130">Nombre:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nombre_cliente" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> NIT:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nit_cliente" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button4" runat="server" onclick="Button4_Click" Text="Aceptar" />
            </td>
            <td><asp:Button ID="Cancel_cliientebtn" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label4" runat="server" Text="Seleccionar" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_clientes" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_clientes_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_clientes_PageIndexChanged" 
                onpageindexchanging="gv_clientes_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
    </asp:Panel>
    <%--********************************************************--%>
    <asp:Panel ID="pnlShipper" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label5" runat="server" Text="Criterio de busqueda" />
        </div></td></tr>
        
        <tr>
            <td align="left" width="130">Nombre:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nombre_shipper" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> NIT:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nit_shipper" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="aceptar_shipperbtn" runat="server" onclick="aceptar_shipperbtn_Click" Text="Aceptar" />
            </td>
            <td><asp:Button ID="cancelar_shipperbtn" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label9" runat="server" Text="Seleccionar" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_shipper" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_shipper_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_shipper_PageIndexChanged" 
                onpageindexchanging="gv_shipper_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
    </asp:Panel>
    <%--********************************************************--%>
    <asp:Panel ID="pnlConsignee" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label10" runat="server" Text="Criterio de busqueda" />
        </div></td></tr>
        <%--<tr>
            <td align="left" width="130">Tipo:</td>
            <td align="left"> 
                
                <asp:DropDownList ID="DropDownList2" runat="server">
                    <asp:ListItem Selected="True">Cliente</asp:ListItem>
                    <asp:ListItem>Consignee</asp:ListItem>
                    <asp:ListItem>Shipper</asp:ListItem>
                </asp:DropDownList>
                
            </td>
        </tr>--%>
        <tr>
            <td align="left" width="130">Nombre:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nombre_consignee" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> NIT:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nit_consignee" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="aceptar_consigneebtn" runat="server" onclick="aceptar_consigneebtn_Click" Text="Aceptar" />
            </td>
            <td><asp:Button ID="cancelar_consigneebtn" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label11" runat="server" Text="Seleccionar" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_consignee" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_consignee_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_consignee_PageIndexChanged" 
                onpageindexchanging="gv_consignee_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
    </asp:Panel>
    <%--********************************************************--%>
    <asp:Panel ID="pnlComodities" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label12" runat="server" Text="Criterio de busqueda" />
        </div></td></tr>
        <%--<tr>
            <td align="left" width="130">Tipo:</td>
            <td align="left"> 
                
                <asp:DropDownList ID="DropDownList2" runat="server">
                    <asp:ListItem Selected="True">Cliente</asp:ListItem>
                    <asp:ListItem>Consignee</asp:ListItem>
                    <asp:ListItem>Shipper</asp:ListItem>
                </asp:DropDownList>
                
            </td>
        </tr>--%>
        <tr>
            <td align="left" width="130">Nombre:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nombre_comoditie" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> Codigo:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_codigo_comoditie" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="aceptar_comoditiesbtn" runat="server" onclick="aceptar_comoditiesbtn_Click" Text="Aceptar" />
            </td>
            <td><asp:Button ID="cancelar_comoditiesbtn" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label14" runat="server" Text="Seleccionar" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_comodities" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_comodities_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_comodities_PageIndexChanged" 
                onpageindexchanging="gv_comodities_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
    </asp:Panel>
    
    <cc1:ModalPopupExtender ID="modalcliente" runat="server" TargetControlID="tbCliCod"
            PopupControlID="pnlCliente" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
     <cc1:ModalPopupExtender ID="modalshipper" runat="server" TargetControlID="tb_shipper"
            PopupControlID="pnlShipper" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
     <cc1:ModalPopupExtender ID="modalconsignee" runat="server" TargetControlID="tb_consignee"
            PopupControlID="pnlConsignee" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
     <cc1:ModalPopupExtender ID="modalcomodities" runat="server" TargetControlID="tb_comodity"
            PopupControlID="pnlComodities" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    </fieldset>
</div>
</asp:Content>

