<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="anticipos.aspx.cs" Inherits="operations_anticipos" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
    <h3 id="adduser">
            LIQUIDACION DE ANTICIPOS
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </h3>
    <br />
    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
        <tr>
            <td bgcolor="#D9ECFF" style="font-size: small">
                <strong>DATOS DEL PROVEEDOR</strong></td>
        </tr>
        <tr>
            <td align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td>
                            Serie:
                            <asp:DropDownList ID="drp_serie" runat="server" Enabled="False">
                            </asp:DropDownList>
&nbsp;
                        </td>
                        <td>
                            Correlativo:
                            <asp:TextBox ID="tb_correlativo" runat="server" Height="16px" ReadOnly="True" 
                                Width="50px"></asp:TextBox>
                            <asp:Label ID="lbl_liquidacion_id" runat="server" Text="0" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Transaccion:
                             <asp:DropDownList ID="drp_tipo_transaccion" runat="server" 
                                AutoPostBack="True" 
                                onselectedindexchanged="drp_tipo_transaccion_SelectedIndexChanged">
                            </asp:DropDownList>
&nbsp;Tipo de Persona:
                            <asp:DropDownList ID="drp_tipo_persona" runat="server" Enabled="False">
                                <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                <asp:ListItem Value="2">Agente</asp:ListItem>
                                <asp:ListItem Value="5">Naviera</asp:ListItem>
                                <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                                <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                                <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Codigo: 
                                <asp:TextBox ID="tb_codigo_proveedor" runat="server" Height="16px" 
                        Width="75px" ReadOnly="True">0</asp:TextBox>
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_codigo_proveedor"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
                        </td>
                        <td>
                            Nombre:
                            <asp:TextBox ID="tb_nombre" runat="server" Height="16px" ReadOnly="True" 
                                Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           Observaciones</td>
                        <td>
                            <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" MaxLength="240" 
                                Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#D9ECFF" style="font-size: small">
                <strong>DATOS DEL BANCO</strong></td>
        </tr>
        <tr>
            <td align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td>
                            Banco</td>
                        <td>
                            <asp:DropDownList ID="drp_banco" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="drp_banco_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Cuenta</td>
                        <td>
                            <asp:DropDownList ID="drp_banco_cuenta" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="drp_banco_cuenta_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Moneda</td>
                        <td>
                            <asp:DropDownList ID="drp_moneda" runat="server" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btn_visualizar" runat="server" Text="Visualizar" 
                    onclick="btn_visualizar_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Panel ID="pnl_detalle" runat="server" Visible="False">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
                    <tr>
                        <td bgcolor="#D9ECFF" style="font-size: small">
                            <strong>Cheques Pendientes de Liquidar</strong></td>
                        <td bgcolor="#D9ECFF" style="font-size: small">
                            <strong>Cortes Pendientes de Liquidar</strong></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:GridView ID="gv_documentos" runat="server" AutoGenerateColumns="False" 
                                Font-Size="Small" BackColor="White" BorderStyle="None" GridLines="None" 
                                Width="350px">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_asignar" runat="server" AutoPostBack="True" 
                                                oncheckedchanged="chk_asignar_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ttr_id" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ttr_id" runat="server" Text='<%# Eval("TTR_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ref_id" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ref_id" runat="server" Text='<%# Eval("REF_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_tipo" runat="server" Text='<%# Eval("TIPO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Numero">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_numero" runat="server" Text='<%# Eval("NUMERO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_fecha" runat="server" Text='<%# Eval("FECHA") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_total" runat="server" Text='<%# Eval("TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle Font-Bold="True" 
                                    HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle BackColor="#333333" ForeColor="White" HorizontalAlign="Center" 
                                    VerticalAlign="Middle" />
                                <RowStyle HorizontalAlign="Right" BorderStyle="None" />
                            </asp:GridView>
                        </td>
                        <td valign="top">
                            <asp:GridView ID="gv_cortes" runat="server" AutoGenerateColumns="False" 
                                Font-Size="Small" GridLines="None" Width="350px">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_asignar" runat="server" AutoPostBack="True" 
                                                oncheckedchanged="chk_asignar_CheckedChanged1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ttr_id" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ttr_id" runat="server" Text='<%# Eval("TTR_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ref_id" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ref_id" runat="server" Text='<%# Eval("REF_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_tipo" runat="server" Text='<%# Eval("TIPO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Serie">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_serie" runat="server" Text='<%# Eval("SERIE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Corr">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_correlativo" runat="server" Text='<%# Eval("CORRELATIVO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_fecha" runat="server" Text='<%# Eval("FECHA") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_total" runat="server" Text='<%# Eval("TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#333333" ForeColor="White" HorizontalAlign="Center" 
                                    VerticalAlign="Middle" />
                                <RowStyle HorizontalAlign="Right" />
                            </asp:GridView>
                        </td>
                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" valign="middle" height="100">
                                            <table align="center" cellpadding="0" cellspacing="0" 
                                                style="width: 80%; vertical-align: middle; font-size: small;">
                                                <tr>
                                                    <td width="200">
                                                        <strong>Total Anticipado</strong></td>
                                                    <td align="right" style="width: 100px" width="200">
                                                        <asp:Label ID="lbl_total_cheques_operados" runat="server" Font-Bold="True">0.00</asp:Label>
                                                    </td>
                                                    <td rowspan="3" width="50">
                                                        &nbsp;</td>
                                                    <td width="200">
                                                        <strong>Total a Favor Proveedor</strong></td>
                                                    <td align="right" style="width: 100px" width="200">
                                                        <asp:Label ID="lbl_total_cortes_operados" runat="server" Font-Bold="True">0.00</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 16px; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;">
                                                        <strong>Total a Liquidar</strong></td>
                                                    <td align="right" 
                                                        style="height: 16px; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;">
                                                        <asp:Label ID="lbl_total_cheques_liquidar" runat="server" Font-Bold="True" 
                                                            ForeColor="Red">0.00</asp:Label>
                                                    </td>
                                                    <td style="height: 16px; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;">
                                                        <strong>Total a Liquidar</strong></td>
                                                    <td align="right" 
                                                        style="height: 16px; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;">
                                                        <asp:Label ID="lbl_total_cortes_liquidar" runat="server" Font-Bold="True" 
                                                            ForeColor="Red">0.00</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td bgcolor="#D9ECFF">
                                                        <strong>Disponible</strong></td>
                                                    <td align="right" bgcolor="#D9ECFF">
                                                        <asp:Label ID="lbl_total_cheques_disponibles" runat="server" Font-Bold="True" 
                                                            Font-Italic="False" Text="0.00"></asp:Label>
                                                    </td>
                                                    <td bgcolor="#D9ECFF">
                                                        <strong>Disponible</strong></td>
                                                    <td align="right" bgcolor="#D9ECFF">
                                                        <asp:Label ID="lbl_total_cortes_disponibles" runat="server" Font-Bold="True">0.00</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                    <tr>
                        <td align="center" colspan="2" height="200" valign="middle">
                                        <table align="center" cellpadding="0" cellspacing="0" 
                                            style="width: 90%; font-size: small;">
                                            <tr>
                                                <td bgcolor="#D9ECFF" colspan="4" style="font-size: small">
                                                    <strong>Agregar Deposito Valor Sobrante</strong></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Banco</td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="drp_banco2" runat="server" AutoPostBack="True" 
                                                        onselectedindexchanged="drp_banco2_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Cuenta</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_banco_cuenta2" runat="server" AutoPostBack="True" 
                                                        onselectedindexchanged="drp_banco_cuenta2_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Moneda</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_moneda2" runat="server" Enabled="False">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    No.Referencia</td>
                                                <td>
                                                    <asp:TextBox ID="tb_no_referencia" runat="server" Height="16px" Width="200px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Fecha</td>
                                                <td>
                                                    <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="128px"></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="tb_fecha_MaskedEditExtender" runat="server" 
                                                        Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="tb_fecha">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" 
                                                        Enabled="True" Format="MM/dd/yyyy" TargetControlID="tb_fecha">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Monto</td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="tb_monto" runat="server" Height="16px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="tb_monto_FilteredTextBoxExtender" 
                                                        runat="server" Enabled="True" TargetControlID="tb_monto"
                                                        FilterType="Numbers,Custom" ValidChars="." >
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <asp:Button ID="btn_agregar" runat="server" onclick="btn_agregar_Click" 
                                                        Text="Agregar" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" bgcolor="#D9ECFF" colspan="4" style="font-size: small">
                                                    <strong>Depositos a Liquidar</strong></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:GridView ID="gv_depositos" runat="server" AutoGenerateColumns="False" 
                                                        BackColor="White" BorderStyle="None" Font-Size="Small" GridLines="None" 
                                                        Width="600px" onrowdeleting="gv_depositos_RowDeleting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk_asignar" runat="server" AutoPostBack="True" 
                                                                        oncheckedchanged="chk_asignar_CheckedChanged1" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                                                            <asp:TemplateField HeaderText="ttr_id" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_ttr_id" runat="server" Text='<%# Eval("TTR_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ref_id" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_ref_id" runat="server" Text='<%# Eval("REF_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tipo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_tipo" runat="server" Text='<%# Eval("TIPO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="BancoID" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_bancoid" runat="server" Text='<%# Eval("BANCOID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Banco">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_banco" runat="server" Text='<%# Eval("BANCO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cuenta">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_banco_cuenta" runat="server" Text='<%# Eval("CUENTA") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Referencia">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_referencia" runat="server" Text='<%# Eval("REFERENCIA") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Fecha">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_fecha" runat="server" Text='<%# Eval("FECHA") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_total" runat="server" Text='<%# Eval("TOTAL") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Moneda" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_moneda" runat="server" Text='<%# Eval("MONEDA") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                        <HeaderStyle BackColor="#333333" ForeColor="White" HorizontalAlign="Center" 
                                                            VerticalAlign="Middle" />
                                                        <RowStyle BorderStyle="None" HorizontalAlign="Right" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                        </td>
                    </tr>
                </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
               </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btn_guardar" runat="server" onclick="btn_guardar_Click" 
                    Text="Guardar" />
            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_generar_reporte" runat="server" 
                    onclick="btn_generar_reporte_Click" Text="Reporte" />
            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_imprimir_retencion" runat="server" 
                    Text="Imprimir Retencion" Enabled="false" 
                    onclick="btn_imprimir_retencion_Click" />
&nbsp;</td>
        </tr>
    </table>
</div>
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
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5" 
                    BorderStyle="None">
                    <HeaderStyle BackColor="#0000CC" BorderColor="#0000CC" BorderStyle="None" 
                        ForeColor="White" />
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
</asp:Content>

