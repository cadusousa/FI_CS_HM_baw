<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="pagar_corte.aspx.cs" Inherits="operations_pagar_corte" Title="AIMAR - BAW" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
<h3 id="adduser">PAGO DE CORTES (SOAS)</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {

    } 
    function mpeSeleccionOnCancel()
    {

    } 
    function AcceptRcpt(){
        if (confirm('Monto Mayor')) {
        alert('Paso');
        }
    }
    </script>
    <table width="700" align="center">
    <tbody>
        <tr>
            <td align="center">
                <table align="center" cellpadding="0" cellspacing="0" 
                    style="width: 95%; height: 900px;">
                    <tr>
                        <td align="center" colspan="4">
                            <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                <tr>
                                    <td>
                                        <strong>MONEDA</strong></td>
                                    <td>
                                        <asp:DropDownList ID="lbMoneda" runat="server" Enabled="False"></asp:DropDownList>
                <asp:DropDownList ID="lb_tipopago" runat="server" Visible="False"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <strong>ROE</strong></td>
                                    <td>
                                        <asp:TextBox ID="tb_roe" runat="server" Height="16px" ReadOnly="True" 
                    Width="71px"></asp:TextBox>
                <asp:Label ID="lbl_flag" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                                    </td>
                                    <td>
                                        <strong>FECHA</strong></td>
                                    <td>
                                        <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="88px" 
                    ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Serie</td>
                        <td>
                            <asp:DropDownList ID="lb_serie_factura" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="lb_serie_factura_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td align="center">
                            Correlativo</td>
                        <td>
                
                            <asp:TextBox ID="lb_rcpt" runat="server" Height="16px" 
                    Width="40px" Text="0" Enabled="false"></asp:TextBox>
                
                            <asp:Label ID="lb_rcptID" runat="server" Visible="False"></asp:Label>
                
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Tipo de Persona</td>
                        <td>
                            <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" onselectedindexchanged="lb_tipopersona_SelectedIndexChanged">
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="10">Intercompany</asp:ListItem>
                </asp:DropDownList>
                        </td>
                        <td align="center">
                            Codigo</td>
                        <td>
                            <asp:TextBox ID="tb_clientid" runat="server" Height="16px" Width="67px" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <strong>NOMBRE</strong></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:TextBox  ID="tb_clientname" runat="server" Height="16px" Width="600px" 
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <strong>RECIBO A NOMBRE DE</strong></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:TextBox ID="tb_recibonombre" runat="server" 
                    Height="16px" Width="600px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <strong>OBSERVACIONES</strong></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:TextBox ID="tb_nota" runat="server" Height="15px" Width="600px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" height="25px">
                <asp:CheckBox ID="chk_anticipo" runat="server" Text="ANTICIPO" 
                    oncheckedchanged="chk_anticipo_CheckedChanged" AutoPostBack="True" visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <h3 id="H6">CORTES ABONADOS CON ESTE RECIBO</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                    <asp:GridView ID="dgw_aplicada" runat="server" AutoGenerateColumns="False" 
                                EmptyDataText="El recibo no tiene cortes pagadas" Font-Size="X-Small">
                    <Columns>
                        <asp:TemplateField HeaderText="Recibo ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptid" runat="server" Visible="false" Text='<%# Eval("ReciboID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Recibo Serie" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptserie" runat="server" Text='<%# Eval("Recibo_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Recibo Correlativo" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptcorr" runat="server" Text='<%# Eval("Recibo_Correlativo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Corte ID"  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_facid" runat="server"  Visible="false" Text='<%# Eval("Factura_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Corte Serie">
                            <ItemTemplate>
                                <asp:Label ID="lb_facserie" runat="server" Text='<%# Eval("Factura_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Corte_Correlativo">
                            <ItemTemplate>
                                <asp:Label ID="lb_faccorr" runat="server" Text='<%# Eval("Factura_Correlativo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Abono">
                            <ItemTemplate>
                                <asp:Label ID="lb_abono" runat="server" Text='<%# Eval("Abono") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Abono">
                            <ItemTemplate>
                                <asp:Label ID="lb_fecha_abono" runat="server" Text='<%# Eval("Fecha_Abono") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Diferencial" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_diferencial" runat="server" Text='<%# Eval("Diferencial") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Moneda Diferencial" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_moneda_diferencial" runat="server" Text='<%# Eval("Moneda_Diferencial") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <h3 id="H8">AGREGAR NUEVO PAGO</h3>       
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                <tr>
                                    <td>
                                        Tipo de Pago</td>
                                    <td>
                                        <asp:DropDownList 
                ID="lb_tipo_pago" runat="server" Height="26px" 
            Width="127px" AutoPostBack="True" 
                onselectedindexchanged="lb_tipo_pago_SelectedIndexChanged">
            <asp:ListItem Value="Efectivo">Efectivo</asp:ListItem>
            <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
            <asp:ListItem Value="Deposito bancario">Deposito bancario</asp:ListItem>
            <asp:ListItem Value="Transferencia bancaria">Transferencia bancaria</asp:ListItem>
            <asp:ListItem Value="Retencion IVA">Retencion IVA</asp:ListItem>
            <asp:ListItem Value="Retencion ISR">Retencion ISR</asp:ListItem>
            <asp:ListItem Value="Retencion CLIENTES">Retencion CLIENTES</asp:ListItem>
        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Monto</td>
                                    <td>
                                        <asp:TextBox ID="tb_montoparcial" runat="server" Height="16px" 
                                        Width="107px">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="tb_montoparcial_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" TargetControlID="tb_montoparcial" FilterType="Numbers,Custom" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="tb_montoparcial" 
                ErrorMessage="El campo monto no puede estar vacio" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <br />
                                    </td>
                                    <td>
                                        Moneda</td>
                                    <td>
            <asp:DropDownList ID="lb_moneda" runat="server" Width="65px" Enabled="False"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Banco</td>
                                    <td>
                                        <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" Width="100px" 
                AutoPostBack="True" 
                onselectedindexchanged="lb_bancos_SelectedIndexChanged1">
                </asp:DropDownList>
                                    </td>
                                    <td>
                                        Cuenta</td>
                                    <td>
                                        <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" Height="25px" 
                                            Width="100px">
                </asp:DropDownList>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        Referencia</td>
                                    <td>
                                        <asp:TextBox ID="tb_referencia" runat="server" Height="16px" 
            Width="85px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td colspan="3">
            <asp:Button ID="tb_agregarpago" runat="server" onclick="tb_agregarpago_Click" 
                Text="Agregar pago" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <h3 id="H9">PAGOS AGREGADOS</h3>       
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                        <asp:GridView ID="gv_pagos" runat="server" onrowcommand="gv_pagos_RowCommand" 
                            Width="604px" Font-Size="X-Small">
                            <Columns>
                                <asp:ButtonField ButtonType="Button" CommandName="Eliminar" Text="Eliminar" />
                            </Columns>
                        </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <h3 id="H10">MONTO A APLICAR</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
        <asp:TextBox ID="tb_monto" runat="server" Height="21px" 
                    AutoPostBack="True" Width="83px" Enabled="False">0</asp:TextBox>
        <asp:TextBox ID="tb_monto_equivalente" runat="server" Height="21px" 
                    AutoPostBack="True" Width="83px" Enabled="False" Visible="false">0</asp:TextBox>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <h3 id="H11">CORTES PENDIENTES DE PAGO</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                <asp:GridView ID="dgw" runat="server" onrowcreated="dgw_RowCreated" Font-Size="XX-Small">
                    <Columns>
                        <asp:TemplateField HeaderText="Abono a aplicar">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Height="16px" Width="70px" 
                                    AutoPostBack="True" ontextchanged="TextBox1_TextChanged">0.00</asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" 
                                    runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="TextBox1">
                                </cc1:FilteredTextBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>    
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <h3 id="H13">POLIZA DIARIO</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                          <asp:GridView ID="gv_detalle_partida" runat="server" 
                              AutoGenerateColumns="False" EmptyDataText="" Width="90%" Visible="True" 
                                Font-Size="X-Small">
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
                                  <asp:TemplateField HeaderText="DEBE">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cargos" runat="server" Text='<%# Eval("CARGOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="HABER">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_abonos" runat="server" Text='<%# Eval("ABONOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                              </Columns>
                          </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                          <asp:GridView ID="gv_detalle_partida_diferencial" runat="server" 
                              AutoGenerateColumns="False" EmptyDataText="" Width="90%" Visible="False" 
                                Font-Size="X-Small">
                              <Columns>
                                  <asp:TemplateField HeaderText="CUENTA">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cuenta0" runat="server" Text='<%# Eval("CUENTA") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="DESCRIPCION DE CUENTA">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_desc_cuenta0" runat="server" 
                                              Text='<%# Eval("DESC_CUENTA") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="DEBE">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cargos0" runat="server" Text='<%# Eval("CARGOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="HABER">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_abonos0" runat="server" Text='<%# Eval("ABONOS") %>'></asp:Label>
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
                    <asp:Button ID="bt_Enviar" runat="server" Text="Realizar pago" onclick="bt_Enviar_Click" />
                    <asp:Button ID="btn_imprimir" runat="server" onclick="btn_imprimir_Click" 
                        Text="Imprimir" Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                        </td>
                    </tr>
                </table>    
            </td>
        </tr>
        <tr><td>
        <%--********************************************************--%>
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="600px" Visible="false" style="display:none;">
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
                    PageSize="10" Enabled="False">
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
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" Width="722px" style="display:none">
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
                    <td><asp:Button ID="Button2" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table>
            
            </div>
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
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_clientid"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <%--*******************************************************************--%>
        </td>
        </tr>
    </tbody>
    </table>
    
    
</div>

</asp:Content>

