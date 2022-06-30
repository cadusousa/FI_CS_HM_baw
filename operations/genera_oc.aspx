<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="genera_oc.aspx.cs" Inherits="operations_GeneraOC" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">ORDENES DE COMPRA</h3>
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

    function mpeSeleccionOnCancel()
    {
        
    }
    function mpecancela_busqueda_rubro() { }
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
<div align="center">
<table width="700" align="center">
    <tr><td align="center">
        <table width="700" align="center">
        <tbody>
            <tr>
                <td>
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 97%">
                        <tr>
                            <td align="center" colspan="4" style="height: 35px">
                        <input id="Button_asociar_documento" type="button" value="Asociar un DOCUMENTO"  onclick="javascript:window.open('searchBL_oc.aspx?id='+ document.getElementById('ctl00_Contenido_lbl_tocid').innerHTML, null, 'toolbar=no,resizable=no,status=no,width=400,height=600');" />
                        <asp:Label ID="lbl_blID" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lbl_tipoOperacionID" runat="server" Text="0" 
                        Visible="False"></asp:Label>
                    <asp:Label ID="lb_suc" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lb_estadodoc" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lb_pais" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lbl_tocid" runat="server" Text="0" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 85%">
                                    <tr>
                                        <td align="center">
                                            <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                                                <tr>
                                                    <td>
                                                        Serie</td>
                                                    <td>
                <asp:DropDownList ID="lb_serie_factura" runat="server" AutoPostBack="True" 
                                                            onselectedindexchanged="lb_serie_factura_SelectedIndexChanged">
                </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        Correlativo</td>
                                                    <td>
                                            <asp:Label ID="lb_corr" runat="server" Text="0"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>MONEDA</strong>&nbsp;</td>
                                                    <td>
                <asp:DropDownList ID="lb_moneda" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_moneda_SelectedIndexChanged" Font-Bold="True">
                </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <strong>CODIGO</strong></td>
                                                    <td>
                                <asp:TextBox ID="tb_proveedorID" runat="server" Height="16px" 
                    Width="70px" ReadOnly="True" BackColor="#D9ECFF"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Panel ID="pnl_provision" runat="server" Visible="False">
                                                <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 70%">
                                                    <tr>
                                                        <td align="center" bgcolor="#D9ECFF" colspan="4">
                                                            <strong>PROVISION</strong></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>SERIE</strong></td>
                                                        <td>
                                                            <asp:Label ID="lbl_provision_serie" runat="server" style="font-weight: 700"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <strong>CORRELATIVO</strong></td>
                                                        <td>
                                                            <asp:Label ID="lbl_provision_correlativo" runat="server" Text="0" 
                                                                style="font-weight: 700"></asp:Label>
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
                                <strong>Departamento</strong></td>
                            <td>
                    <asp:DropDownList ID="lb_departamento_interno" runat="server" Height="23px" 
                        Width="255px">
                    </asp:DropDownList>
                            </td>
                            <td>
                                <strong>Fecha</strong></td>
                            <td>
                    <asp:TextBox ID="tb_fecha" runat="server" Height="16px" ReadOnly="True" 
                        Width="100px"></asp:TextBox>
                <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Nombre</strong></td>
                            <td colspan="3">
                <asp:TextBox ID="tb_proveedornombre" runat="server" Height="16px" Width="500px" 
                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo de Contribuyente</td>
                            <td>
                    <asp:DropDownList ID="lb_contribuyente" runat="server" Width="140px" 
                    ToolTip="Hola">
                    </asp:DropDownList>
                <asp:Label ID="lb_contribuyente2" runat="server"></asp:Label>
                            </td>
                            <td>
                                Tipo de Operacion</td>
                            <td>
                                <asp:DropDownList ID="lb_imp_exp" runat="server">
                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Dirección</td>
                            <td colspan="3">
                <asp:TextBox ID="tb_proveedordireccion" runat="server" Height="16px" 
                    Width="500px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Teléfono</td>
                            <td colspan="3">
                <asp:TextBox ID="tb_proveedortelefono" runat="server" Height="16px" 
                    Width="500px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Correo electrónico</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_proveedoremail" runat="server" 
                    Height="16px" Width="500px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Contacto</td>
                            <td colspan="3">
                <asp:TextBox ID="tb_proveedorcontacto" runat="server" Height="16px" 
                    Width="500px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>No de cotizacion</strong></td>
                            <td>
                                <asp:TextBox ID="tb_cotizacion" runat="server" Height="16px" 
                    Width="130px" MaxLength="30"></asp:TextBox>
                            </td>
                            <td>
                                Prioridad</td>
                            <td>
                                <asp:DropDownList ID="lb_prioridad" runat="server" Height="23px" 
                    Width="133px">
                    <asp:ListItem Value="0">Baja</asp:ListItem>
                    <asp:ListItem Value="1">Media</asp:ListItem>
                    <asp:ListItem Value="2">Alta</asp:ListItem>
                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" bgcolor="#D9ECFF">
                                <strong>Descripción de bienes o servicios</strong></td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                <asp:TextBox ID="tb_descripcion_oc" runat="server" Rows="3" 
                    TextMode="MultiLine" Width="631px" MaxLength="250"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" height="70">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td>
                <asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
                         </td>
                        <td>
                    <asp:Label ID="lb_mbl" runat="server" Text="MBL"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                    <asp:Label ID="lb_routing" runat="server" Text="Routing"></asp:Label>
                        </td>
                        <td>
                        <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>

                    <asp:Label ID="lb_contenedor" runat="server" Text="Contenedor"></asp:Label>
                        </td>
                        <td>

                <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
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
                    <asp:TextBox ID="tb_serie_proveedor" runat="server" Height="16px" Width="150px" MaxLength="49"></asp:TextBox> 
                                        </td>
                                        <td>
                                            Correlativo</td>
                                        <td>
                                            <asp:TextBox ID="tb_correlativo_proveedor" 
                        runat="server" Height="16px" Width="100px" MaxLength="30"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="tb_correlativo_proveedor_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers" 
                        TargetControlID="tb_correlativo_proveedor">
                    </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            Fecha</td>
                                        <td>
                                            <asp:TextBox ID="tb_fecha_proveedor" runat="server" Height="16px" 
                        Width="128px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fecha_proveedor_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date" Enabled="True" 
                        TargetControlID="tb_fecha_proveedor">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fecha_proveedor_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fecha_proveedor" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                <table width="650">
                <thead>
                <tr>
                    <th>Detalle de Rubros</th>
                </tr>
                </thead>
                <tr><td>
                
                    <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="97%" 
            ondatabound="gv_detalle_DataBound" onrowdeleting="gv_detalle_RowDeleting" 
                        onrowdatabound="gv_detalle_RowDataBound">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    ShowDeleteButton="True" />
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
                    <ItemStyle BackColor="#CCFFFF" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subtotal">
                    <ItemTemplate>
                        <asp:Label ID="lb_subtotal" runat="server"  Text='<%# Eval("SUBTOTAL") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Impuesto">
                    <ItemTemplate>
                        <asp:Label ID="lb_impuesto" runat="server" Text='<%# Eval("IMPUESTO") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate>
                        <asp:Label ID="lb_total" runat="server" Text='<%# Eval("TOTAL") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="#CCFFFF" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Equivalente">
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
                
                </td>
                </tr>
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
                    <asp:Label ID="Label2" runat="server" Text="Total Equivalente"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_totaldolares" runat="server" Height="16px" ReadOnly="True" style=" text-align:right"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Sub-Total:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_subtotal" runat="server" Height="16px" ReadOnly="True" style=" text-align:right"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Impuesto:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_impuesto" runat="server" Height="16px" ReadOnly="True" style=" text-align:right"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_total2" runat="server" Height="16px" ReadOnly="True" style=" text-align:right"></asp:TextBox>
                    <asp:TextBox ID="tb_total" 
                    runat="server" Height="16px" Width="30px" ReadOnly="True" Visible="False" style=" text-align:right"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
    </td>
    </tr>
                </table>
                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" bgcolor="#D9ECFF">
                                <strong>Observaciones adicionales</strong></td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                <asp:TextBox ID="tb_observaciones" runat="server" Rows="3" TextMode="MultiLine" 
                    Width="628px" MaxLength="250"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" bgcolor="#D9ECFF">
                                <strong>Nombre del proveedor para la cancelación y términos de pago</strong></td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                <asp:TextBox ID="tb_terminos" runat="server" Rows="3" TextMode="MultiLine" 
                    Width="626px" MaxLength="250"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                    <asp:CheckBox ID="chk_draft" runat="server" 
                        Text="Guardar como &quot;DRAFT&quot;" TextAlign="Left" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                    
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
                            <td align="center" colspan="4">
                <table width="650">
                    <tr>
                        <td>
                            <asp:Button ID="bt_grabar" runat="server" Text="Grabar" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"
                                onclick="bt_grabar_Click" />
                        </td>
                        <td>
                            <asp:Button ID="bt_aceptar" runat="server" Enabled="False" Text="Autorizar" OnClientClick="if (!confirm('¿Desea autorizar la orden de compra?')) { return false; } else { BloquearPantalla(); }" UseSubmitBehavior="false"
                                onclick="bt_aceptar_Click" Visible="False" />
                        </td>
                        <td>
                            <asp:Button ID="tb_cancelar" runat="server" Text="Rechazar" 
                                onclick="tb_cancelar_Click" Visible="False" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btn_imprimir" runat="server" onclick="btn_imprimir_Click" 
                                Text="Imprimir" />
                        </td>
                        <td>Orden solicitada por:<br />
                            <asp:Label ID="lb_solicita" runat="server"></asp:Label>
                        </td>
                        <td>Orden autorizada por:<br /><asp:Label ID="lb_autoriza" runat="server"></asp:Label>
                            </td>
                        <td>&nbsp;</td>
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
    <tr><td>
    <!-- ***************************************************************************** -->
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
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Proveedor" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" onload="gv_proveedor_Load" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5" 
                    Width="692px">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_proveedorID"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <!-- ***************************************************************************** -->
        <%--<asp:Panel ID="Panel_ROS" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            Seleccionar RO
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_buscar_ro" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="drp_tipo_ro" runat="server">
                    <asp:ListItem>RO ADUANAS</asp:ListItem>
                    <asp:ListItem>RO SEGUROS</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gv_ros" runat="server" AllowPaging="True" 
                    PageSize="10" onpageindexchanging="gv_ros_PageIndexChanging" 
                    onrowcommand="gv_ros_RowCommand">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_buscar_ro" runat="server" Text="Buscar" 
                    onclick="btn_buscar_ro_Click" />
            </td>
            <td><asp:Button ID="Button2" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
            <!-- ---------------------------------------------------- -->
                    <cc1:ModalPopupExtender ID="ModalPopup_ROS" runat="server" TargetControlID="tb_routing2"
            PopupControlID="Panel_ROS" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />--%>    <%--********************************************************--%><%--<asp:Panel ID="Panel_ROS" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            Seleccionar RO
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_buscar_ro" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="drp_tipo_ro" runat="server">
                    <asp:ListItem>RO ADUANAS</asp:ListItem>
                    <asp:ListItem>RO SEGUROS</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gv_ros" runat="server" AllowPaging="True" 
                    PageSize="10" onpageindexchanging="gv_ros_PageIndexChanging" 
                    onrowcommand="gv_ros_RowCommand">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_buscar_ro" runat="server" Text="Buscar" 
                    onclick="btn_buscar_ro_Click" />
            </td>
            <td><asp:Button ID="Button2" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
            <!-- ---------------------------------------------------- -->
                    <cc1:ModalPopupExtender ID="ModalPopup_ROS" runat="server" TargetControlID="tb_routing2"
            PopupControlID="Panel_ROS" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />--%><%--********************************************************--%><%--<asp:Panel ID="Panel_ROS" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            Seleccionar RO
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_buscar_ro" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="drp_tipo_ro" runat="server">
                    <asp:ListItem>RO ADUANAS</asp:ListItem>
                    <asp:ListItem>RO SEGUROS</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gv_ros" runat="server" AllowPaging="True" 
                    PageSize="10" onpageindexchanging="gv_ros_PageIndexChanging" 
                    onrowcommand="gv_ros_RowCommand">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_buscar_ro" runat="server" Text="Buscar" 
                    onclick="btn_buscar_ro_Click" />
            </td>
            <td><asp:Button ID="Button2" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
            <!-- ---------------------------------------------------- -->
                    <cc1:ModalPopupExtender ID="ModalPopup_ROS" runat="server" TargetControlID="tb_routing2"
            PopupControlID="Panel_ROS" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />--%>
        <%--********************************************************--%><%--<asp:Panel ID="Panel_ROS" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            Seleccionar RO
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_buscar_ro" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="drp_tipo_ro" runat="server">
                    <asp:ListItem>RO ADUANAS</asp:ListItem>
                    <asp:ListItem>RO SEGUROS</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gv_ros" runat="server" AllowPaging="True" 
                    PageSize="10" onpageindexchanging="gv_ros_PageIndexChanging" 
                    onrowcommand="gv_ros_RowCommand">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_buscar_ro" runat="server" Text="Buscar" 
                    onclick="btn_buscar_ro_Click" />
            </td>
            <td><asp:Button ID="Button2" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
            <!-- ---------------------------------------------------- -->
                    <cc1:ModalPopupExtender ID="ModalPopup_ROS" runat="server" TargetControlID="tb_routing2"
            PopupControlID="Panel_ROS" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />--%>
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
                <asp:DropDownList ID="lb_rubro" runat="server" onload="lb_rubro_Load">
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
            <td>
                <asp:Button ID="buscar_rubro" runat="server" onclick="buscar_rubro_Click" 
                    Text="Aceptar" />
            </td>
            <td><asp:Button ID="cancela_busqueda_rubro" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    
    <cc1:ModalPopupExtender ID="btn_rubro_ModalPopupExtender" runat="server" TargetControlID="bt_agregar"
            PopupControlID="pnlrubros" CancelControlID="cancela_busqueda_rubro"
            OnCancelScript="mpecancela_busqueda_rubro()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
        <%--********************************************************--%>

        <!-- ------------------------------------------------------------------------------------------------------------- !-->
        <%--<asp:Panel ID="Panel_ROS" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            Seleccionar RO
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_buscar_ro" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="drp_tipo_ro" runat="server">
                    <asp:ListItem>RO ADUANAS</asp:ListItem>
                    <asp:ListItem>RO SEGUROS</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gv_ros" runat="server" AllowPaging="True" 
                    PageSize="10" onpageindexchanging="gv_ros_PageIndexChanging" 
                    onrowcommand="gv_ros_RowCommand">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_buscar_ro" runat="server" Text="Buscar" 
                    onclick="btn_buscar_ro_Click" />
            </td>
            <td><asp:Button ID="Button2" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
            <!-- ---------------------------------------------------- -->
                    <cc1:ModalPopupExtender ID="ModalPopup_ROS" runat="server" TargetControlID="tb_routing2"
            PopupControlID="Panel_ROS" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />--%>
    </td></tr>
</table>
</div>
</fieldset>
</div>
</asp:Content>

