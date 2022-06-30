<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="showrcpt.aspx.cs" Inherits="invoice_showrcpt" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

    <div id="box" align="center">
        <h3 id="adduser">RECIBO DE CAJA<asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    </h3>
        
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
                                        
                                        <asp:DropDownList ID="lbMoneda" runat="server" Enabled="False">
                    </asp:DropDownList>
                                        
                                    </td>
                                    <td>
                                        <strong>ROE</strong></td>
                                    <td>
                                        
                                        <asp:TextBox ID="tb_roe" runat="server" Height="16px" ReadOnly="True" 
                        Width="71px"></asp:TextBox>
                                        
                                    </td>
                                    <td>
                                        <strong>FECHA</strong></td>
                                    <td>
                                       
                                        <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="88px" 
                                            ReadOnly="True"></asp:TextBox>
                                        <asp:Label ID="rcptID" runat="server" Text="Label" Visible="False"></asp:Label>
                                       
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Serie</td>
                        <td>
                            <asp:TextBox ID="tb_serierecibo" runat="server" Height="16px" 
                        Width="62px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td align="center">
                            Correlativo</td>
                        <td>
                
                            <asp:TextBox ID="tb_correlativo" runat="server" Height="16px" 
                        Width="62px" ReadOnly="True"></asp:TextBox>
                
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Tipo de Persona</td>
                        <td>

                        </td>
                        <td align="center">
                            Codigo</td>
                        <td>

                            <asp:TextBox ID="tb_clientid" runat="server" Height="16px" 
                        Width="87px" ReadOnly="True"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <strong>CLIENTE</strong></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:TextBox 
                        ID="tb_clientname" runat="server" Height="16px" Width="600px" 
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
                        <!--Cheque o Deposito:<asp:TextBox ID="tb_ref_id" runat="server" Height="16px" 
                        Width="105px"></asp:TextBox>
                    &nbsp;Banco:<asp:DropDownList ID="lb_banco" runat="server" ></asp:DropDownList>
                    <br /> -->
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <strong>OBSERVACIONES</strong></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:TextBox ID="tb_nota" runat="server" 
                        Height="15px" Width="600px" MaxLength="295"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" height="25px">
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="ANTICIPO" visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:TextBox 
                        ID="tb_contenedor" runat="server" Height="15px" Width="113px" visible="false"></asp:TextBox>
                        </td>
                        <td align="center">
                        </td>
                        <td align="center">
                    <asp:TextBox ID="tb_hbl" runat="server" Height="15px" Width="112px" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:TextBox 
                        ID="tb_referencia" runat="server" Height="15px" Width="123px" visible="false"></asp:TextBox>
                        </td>
                        <td align="center">
                        </td>
                        <td align="center">
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
                        EmptyDataText="El recibo no tiene facturas pagadas" 
                        onrowcreated="dgw_aplicada_RowCreated" Font-Size="X-Small">
                    <Columns>
                        <asp:TemplateField HeaderText="Recibo ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptid" runat="server" Visible="false" Text='<%# Eval("ReciboID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Recibo Serie  " Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptserie" runat="server" Text='<%# Eval("Recibo_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Recibo Correlativo  " Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptcorr" runat="server" Text='<%# Eval("Recibo_Correlativo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Factura ID  "  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_facid" runat="server"  Visible="false" Text='<%# Eval("Factura_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Factura Serie  ">
                            <ItemTemplate>
                                <asp:Label ID="lb_facserie" runat="server" Text='<%# Eval("Factura_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Factura Correlativo  ">
                            <ItemTemplate>
                                <asp:Label ID="lb_faccorr" runat="server" Text='<%# Eval("Factura_Correlativo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Abono  ">
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
                        <asp:TemplateField HeaderText="  Recibo Serie  " Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptserie2" runat="server" Text='<%# Eval("Recibo_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Recibo Correlativo  " Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptcorr2" runat="server" Text='<%# Eval("Recibo_Correlativo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Nota Debito ID  "  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_facid2" runat="server"  Visible="false" Text='<%# Eval("NotaDebito_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  ND Serie  ">
                            <ItemTemplate>
                                <asp:Label ID="lb_facserie2" runat="server" Text='<%# Eval("Factura_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  ND Correlativo  ">
                            <ItemTemplate>
                                <asp:Label ID="lb_faccorr2" runat="server" Text='<%# Eval("Factura_Correlativo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Abono  ">
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
                            <h3 id="H9">PAGOS RECIBIDOS EN ESTE DOCUMENTO</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                    <asp:GridView ID="gv_pagos" runat="server" Width="604px" Font-Size="X-Small">
                    </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <h3 id="H10">MONTO DISPONIBLE</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4"> 
                    <asp:TextBox ID="tb_monto" runat="server" Height="21px" 
                        AutoPostBack="True" Width="83px" ReadOnly="True" BackColor="LightCyan" 
                                Font-Bold="True">0</asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <h3 id="H11">FACTURAS PENDIENTES DE PAGO</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                    <asp:GridView ID="dgw" runat="server" onrowcreated="dgw_RowCreated" 
                                Font-Size="XX-Small">
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
                                <cc1:FilteredTextBoxExtender ID="tb_descND_FilteredTextBoxExtender" 
                                    runat="server" Enabled="True"  FilterType="Numbers,Custom" ValidChars=".," TargetControlID="tb_descND">
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
                            &nbsp;</td>
                    </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                        <asp:Button ID="bt_Enviar" runat="server" Text="Aplicar Pago" 
                            onclick="bt_Enviar_Click" />
                        <asp:Button ID="btn_imprimir" runat="server" Enabled="False" 
                            onclick="btn_imprimir_Click" Text="Imprimir Detalle" />
                        &nbsp;<asp:Button ID="btn_reimprimir" runat="server" 
                            onclick="btn_reimprimir_Click" Text="ReImprimir" />
                        &nbsp;<asp:Button ID="btn_recibo_virtual" runat="server" Text="Recibo Virtual" 
                        onclick="btn_recibo_virtual_Click" />
                </td>
            </tr>
        </tbody>
        </table>
    </div>

</asp:Content>

