<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="newrcpt.aspx.cs" Inherits="invoice_newrcpt" Title="AIMAR - BAW" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
<h3 id="adduser">RECIBOS DE CAJA</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {

    } 
    function mpeClienteOnCancel()
    {

    } 
    </script>
    <table width="730" align="left">
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
                                        <asp:DropDownList ID="lbMoneda" runat="server" Enabled="False" Font-Bold="True"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <strong>ROE</strong></td>
                                    <td>
                                        <asp:TextBox ID="tb_roe" runat="server" Height="16px" ReadOnly="True" 
                    Width="71px" Font-Bold="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <strong>FECHA</strong></td>
                                    <td>
                                        <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="88px" 
                    ReadOnly="True" Font-Bold="True"></asp:TextBox>
                <asp:Label ID="lbl_flag" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
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
                
                <asp:Label ID="lbl_tipo_cliente" runat="server" Visible="False"></asp:Label>
                
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Tipo de Persona</td>
                        <td>
                            <asp:TextBox ID="tb_tipo_persona" runat="server" Height="16px" Width="70px" 
                                ReadOnly="True">Cliente</asp:TextBox>
                        </td>
                        <td align="center">
                            Codigo</td>
                        <td>
                            <asp:TextBox ID="tb_clientid" runat="server" Height="16px" Width="87px" ></asp:TextBox>
                <asp:DropDownList ID="lb_tipopago" runat="server" Visible="False"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <strong>CLIENTE</strong></td>
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
                            <asp:TextBox ID="tb_nota" 
                    runat="server" Height="15px" Width="600px" MaxLength="295"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" height="25px">
                <asp:CheckBox ID="chk_anticipo" runat="server" Text="ANTICIPO" 
                    oncheckedchanged="chk_anticipo_CheckedChanged" AutoPostBack="True" visible="true" 
                                Font-Bold="True"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                <asp:Label ID="lb_hbl" runat="server" Text="HBL" visible="false"></asp:Label>
                        </td>
                        <td align="center">
        <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="100px" visible="false"></asp:TextBox>
                        </td>
                        <td align="center">
                            <asp:Label ID="lb_mbl" runat="server" Text="MBL" visible="false"></asp:Label>
                        </td>
                        <td align="center">
        <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="100px" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lb_routing" runat="server" Text="Routing" visible="false"></asp:Label>
                        </td>
                        <td align="center">
        <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="100px" visible="false"></asp:TextBox>
                        </td>
                        <td align="center">
                            <asp:Label ID="lb_contenedor" runat="server" Text="Contenedor" visible="false"></asp:Label>
                        </td>
                        <td align="center">
        <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="100px" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <h3 id="H6">FACTURAS ABONADAS CON ESTE RECIBO</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                    <asp:GridView ID="dgw_aplicada" runat="server" AutoGenerateColumns="False" 
                                EmptyDataText="El recibo no tiene facturas pagadas" Font-Size="X-Small">
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
                        <asp:TemplateField HeaderText="Factura ID"  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_facid" runat="server"  Visible="false" Text='<%# Eval("Factura_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Factura Serie">
                            <ItemTemplate>
                                <asp:Label ID="lb_facserie" runat="server" Text='<%# Eval("Factura_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Factura_Correlativo">
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
                            <h3 id="H7">NOTAS DE DEBITO ABONADAS CON ESTE RECIBO</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                    <asp:GridView ID="gv_nd_abonadas" runat="server"  AutoGenerateColumns="False" 
                                EmptyDataText="El recibo no tiene notas de debito pagadas" Font-Size="X-Small">
                    <Columns>
                        <asp:TemplateField HeaderText="Nota Debito ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptid2" runat="server" Visible="false" Text='<%# Eval("ReciboID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Recibo Serie" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptserie2" runat="server" Text='<%# Eval("Recibo_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Recibo Correlativo" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptcorr2" runat="server" Text='<%# Eval("Recibo_Correlativo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nota Debito ID"  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_facid2" runat="server"  Visible="false" Text='<%# Eval("NotaDebito_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Factura Serie">
                            <ItemTemplate>
                                <asp:Label ID="lb_facserie2" runat="server" Text='<%# Eval("Factura_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Factura_Correlativo">
                            <ItemTemplate>
                                <asp:Label ID="lb_faccorr2" runat="server" Text='<%# Eval("Factura_Correlativo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Abono">
                            <ItemTemplate>
                                <asp:Label ID="lb_abono2" runat="server" Text='<%# Eval("Abono") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Abono">
                            <ItemTemplate>
                                <asp:Label ID="lb_fecha_abono2" runat="server" Text='<%# Eval("Fecha_Abono") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Diferencial" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_diferencial2" runat="server" Text='<%# Eval("Diferencial") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Moneda Diferencial" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_moneda_diferencial2" runat="server" Text='<%# Eval("Moneda_Diferencial") %>'></asp:Label>
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
            Width="150px" AutoPostBack="True" 
                onselectedindexchanged="lb_tipo_pago_SelectedIndexChanged">
            <asp:ListItem Value="Efectivo">Efectivo</asp:ListItem>
            <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
            <asp:ListItem Value="Deposito bancario">Deposito bancario</asp:ListItem>
            <asp:ListItem Value="Transferencia bancaria">Transferencia bancaria</asp:ListItem>
            <asp:ListItem>Retencion IVA</asp:ListItem>
            <asp:ListItem>Retencion ISR</asp:ListItem>
            <asp:ListItem>Retencion CLIENTES</asp:ListItem>
            <asp:ListItem>Compensacion</asp:ListItem>
            <asp:ListItem>Ajuste periodo anterior</asp:ListItem>
            <asp:ListItem>Diferencial Cambiario</asp:ListItem>
        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Monto</td>
                                    <td>
                                        <asp:TextBox ID="tb_montoparcial" runat="server" Height="16px" Width="200px">0</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="tb_montoparcial_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" TargetControlID="tb_montoparcial" FilterType="Numbers,Custom" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                        &nbsp;<br />
                                    </td>
                                    <td>
                                        Moneda</td>
                                    <td>
            <asp:DropDownList ID="lb_moneda" runat="server" Width="60px"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Banco</td>
                                    <td>
                                        <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" Width="150px" 
                AutoPostBack="True" 
                onselectedindexchanged="lb_bancos_SelectedIndexChanged1">
                </asp:DropDownList>
                                    </td>
                                    <td>
                                        Cuenta</td>
                                    <td>
                                        <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" Height="25px" 
                                            Width="200px">
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
                                        <asp:TextBox ID="tb_referencia" 
                runat="server" Height="16px" 
            Width="150px" MaxLength="25"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td colspan="3">
            <asp:Button ID="tb_agregarpago" runat="server" onclick="tb_agregarpago_Click" 
                Text="Agregar" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="tb_montoparcial" 
                ErrorMessage="El campo monto no puede estar vacio" SetFocusOnError="True"></asp:RequiredFieldValidator>
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
                    AutoPostBack="True" Width="83px" Enabled="False" Font-Bold="True">0</asp:TextBox>
        <asp:TextBox ID="tb_monto_equivalente" runat="server" Height="21px" 
                    AutoPostBack="True" Width="83px" Enabled="False" Visible="false" Font-Bold="True">0</asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <h3 id="H11">FACTURAS PENDIENTES DE PAGO</h3>
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
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="TextBox1" ErrorMessage="Error ###.##" 
                                SetFocusOnError="True" ValidationExpression="\d+.\d{2}">
                                        </asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>    
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <h3 id="H12">NOTAS DE DEBITO PENDIENTES DE PAGO</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                <asp:GridView ID="gv_notadebito" runat="server" 
                    onrowcreated="gv_notadebito_RowCreated" Font-Size="XX-Small">
                    <Columns>
                        <asp:TemplateField HeaderText="Abono a aplicar">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_descND" runat="server" Height="16px" Width="70px" Text="0.00" 
                                    AutoPostBack="True" ontextchanged="tb_descND_TextChanged"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" 
                                    runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars=".," TargetControlID="tb_descND">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="tb_descND" ErrorMessage="Error ###.##" 
                                SetFocusOnError="True" ValidationExpression="\d+.\d{2}">
                                        </asp:RegularExpressionValidator>
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
                    <asp:Button ID="btn_recibo_virtual" runat="server" Text="Recibo Virtual" 
                        Enabled="False" onclick="btn_recibo_virtual_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                    &nbsp;</td>
        </tr>
        <tr><td>
            <%--*******************************************************************--%>
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
                    PageSize="10" Enabled="False" Font-Size="X-Small">
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
    
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="tb_hbl"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
            <%--*******************************************************************--%><%--*******************************************************************--%>
    <asp:Panel ID="pnlCliente" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none;">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label3" runat="server" Text="Criterio de busqueda" />
        </div></td></tr>
        <tr>
            <td align="left" width="130">Codigo:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_codigo" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" width="130">Nombre:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nombre_cliente" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> NIT:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nit_cliente" runat="server" Height="16px"></asp:TextBox>
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
    <cc1:ModalPopupExtender ID="modalcliente" runat="server" TargetControlID="tb_clientid"
            PopupControlID="pnlCliente" CancelControlID="Cancel_cliientebtn"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <%--*******************************************************************--%>
        </td>
        </tr>
    </tbody>
    </table>
    
    
</div>

</asp:Content>

