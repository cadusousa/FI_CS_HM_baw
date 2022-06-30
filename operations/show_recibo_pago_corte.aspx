<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="show_recibo_pago_corte.aspx.cs" Inherits="operation_show_recibo_pago_corte" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

    <div id="box">
        <h3 id="adduser">RECIBOS DE CAJA</h3>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
        <br />
        <table align="center" cellpadding="0" cellspacing="0" 
                    style="width: 95%; height: 900px;">
                    <tr>
                        <td align="center" colspan="4">
                            <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                <tr>
                                    <td>
                                        <strong>MONEDA</strong></td>
                                    <td>
                                        <asp:DropDownList ID="lbMoneda" runat="server" Enabled=false>
                    </asp:DropDownList>
                                    </td>
                                    <td>
                                        <strong>ROE</strong></td>
                                    <td>

                                        <asp:TextBox ID="tb_roe" runat="server" Height="16px" ReadOnly="True" 
                        Width="71px"></asp:TextBox>
                                        <asp:Label ID="rcptID" runat="server" Text="Label" Visible="False"></asp:Label>

                                    </td>
                                    <td>
                                        <strong>FECHA</strong></td>
                                    <td>

                                        <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="88px"></asp:TextBox>

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
                            <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" onselectedindexchanged="lb_tipopersona_SelectedIndexChanged" Enabled=false>
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="10">Intercopanys</asp:ListItem>
                </asp:DropDownList>
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
                            <strong>NOMBRE</strong></td>
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
                            <strong>OBSERVACIONES</strong></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:TextBox ID="tb_nota" runat="server" Height="15px" Width="600px" 
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" height="25px">
                            <asp:TextBox 
                        ID="tb_contenedor" runat="server" Height="15px" Width="113px" visible="false"></asp:TextBox>
                    <asp:TextBox ID="tb_hbl" runat="server" Height="15px" Width="112px" visible="false"></asp:TextBox><asp:TextBox 
                        ID="tb_referencia" runat="server" Height="15px" Width="123px" visible="false"></asp:TextBox>
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="ANTICIPO" visible="false"/>
                            <asp:DropDownList ID="lb_banco" runat="server" Visible="False">
                            </asp:DropDownList>
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
                        EmptyDataText="El recibo no tiene cortes pagados" 
                        onrowcreated="dgw_aplicada_RowCreated" Font-Size="X-Small">
                    <Columns>
                        <asp:TemplateField HeaderText="Recibo ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptid" runat="server" Visible="false" Text='<%# Eval("ReciboID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Recibo Serie  ">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptserie" runat="server" Text='<%# Eval("Recibo_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Recibo Correlativo  ">
                            <ItemTemplate>
                                <asp:Label ID="lb_rcptcorr" runat="server" Text='<%# Eval("Recibo_Correlativo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Corte ID  "  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lb_facid" runat="server"  Visible="false" Text='<%# Eval("Factura_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Corte Serie  ">
                            <ItemTemplate>
                                <asp:Label ID="lb_facserie" runat="server" Text='<%# Eval("Factura_Serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="  Corte Correlativo  ">
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
                            <h3 id="H9">PAGOS RECIBIDOS</h3>
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
                            <h3 id="H10">MONTO A APLICAR</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                    <asp:TextBox ID="tb_monto" runat="server" Height="21px" 
                        AutoPostBack="True" Width="83px" ReadOnly="True">0</asp:TextBox>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <h3 id="H11">CORTES PENDIENTES DE PAGO</h3>
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
                                  <asp:TemplateField HeaderText="CARGOS">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cargos0" runat="server" Text='<%# Eval("CARGOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="ABONOS">
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
                        <asp:Button ID="bt_Enviar" runat="server" Text="Aplicar" onclick="bt_Enviar_Click" />
                        <asp:Button ID="btn_imprimir" runat="server" Enabled="False" 
                            onclick="btn_imprimir_Click" Text="Imprimir Detalle Recibo" Visible=false />
                            <asp:Button ID="btn_reimprimir" runat="server" 
                            onclick="btn_reimprimir_Click" Text="ReImprimir Recibo"  Visible=false/>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                        </td>
                    </tr>
                </table>
        <br />
    </div>

</asp:Content>

