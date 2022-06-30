<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="generar_retenciones.aspx.cs" Inherits="operations_generar_retenciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box" align="center">
<fieldset id="Fieldset1">
<h3 id="adduser" align="left">Generar Retenciones</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">
    function mpecancela_busqueda_rubro() {

    }
    function mpeCuentaOnCancel() {

    }
    function mpeSeleccionOnCancel() {

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
                            <td>
                                Transaccion</td>
                            <td>
                                <asp:DropDownList ID="drp_tipo_transaccion" runat="server">
                                </asp:DropDownList>
                        <asp:Label ID="lbl_blID" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lbl_tipoOperacionID" runat="server" Text="0" 
                        Visible="False"></asp:Label>
                                <asp:Label ID="lbl_prov_id" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                            <td>
                                Moneda</td>
                            <td>
                                <asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>
                                <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Serie de Documento</td>
                            <td>
                                <asp:TextBox ID="lb_serie_factura" runat="server" Enabled="False"></asp:TextBox>
                            </td>
                            <td>
                                Correlativo</td>
                            <td>
                    <asp:TextBox ID="tb_corr" runat="server" Enabled="False" Height="16px" 
                        Width="50px" ReadOnly="True">0</asp:TextBox>
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
                        Width="115px" Enabled="False" ReadOnly="True">0</asp:TextBox>
                    
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo de Contribuyente</td>
                            <td>
                    <asp:DropDownList ID="lb_contribuyente" runat="server" Enabled="False">
                    </asp:DropDownList>
                            </td>
                            <td>
                                Tipo Operacion</td>
                            <td>
                    <asp:DropDownList ID="lb_imp_exp" runat="server" Width="150px" Enabled="False">
                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Label ID="lb_tipo_docto" runat="server" Text="Tipo Documento:" 
                Visible="False"></asp:Label></td><td><asp:DropDownList ID="ddl_tipo_documento" runat="server" 
                                    Visible="False" Enabled="False">
            </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                Nombre</td>
                            <td colspan="3">
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" Width="520px" 
                                    ReadOnly="True" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nit</td>
                            <td colspan="3">
                                <asp:TextBox ID="tb_nit" runat="server" Height="16px" ReadOnly="True" 
                                    Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Direccion</td>
                            <td colspan="3">
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" Width="520px" ReadOnly="True" 
                                    Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Observaciones</td>
                            <td colspan="3">
                    <asp:TextBox ID="tb_observacion" runat="server" Height="16px" Width="520px" Enabled="False" 
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4" align="left" style="text-decoration: underline">
                            <strong>Totales</strong></td>
                        </tr>
                        <tr>
                        <td>
                            No Afecto</td>
                        <td>
                <asp:TextBox ID="tb_noafecto" runat="server" Height="16px" Width="200px" 
                    AutoPostBack="True" ReadOnly="True"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender ID="tb_noafecto_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" TargetControlID="tb_noafecto" 
                                FilterType="Numbers,Custom" ValidChars=".">
                </ajaxToolkit:FilteredTextBoxExtender>
                        </td>
                        <td>
                            Afecto</td>
                        <td>
                <asp:TextBox ID="tb_afecto" runat="server" Height="16px" Width="200px" 
                    ReadOnly="True"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                        <td>
                            IVA</td>
                        <td>
                <asp:TextBox ID="tb_iva" runat="server" Height="16px" Width="200px" 
                    ReadOnly="True"></asp:TextBox>                
                        </td>
                        <td>
                            Valor</td>
                        <td>
                            <asp:TextBox ID="tb_valor" 
                    runat="server" Height="16px" 
                    Width="200px" ReadOnly="True"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td colspan="3">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <table align="center" cellpadding="0" cellspacing="0" 
                                    style="width: 90%; font-weight: 700;">
                                    <tr>
                                        <td colspan="4" bgcolor="#D9ECFF">
                                            <strong>Libro de Compras</strong></td>
                                    </tr>
                                    <tr>
                                        <td>
                Asignar Libro de Compras:</td>
                                        <td>
                <asp:RadioButtonList ID="Rb_Documento" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" 
                                                onselectedindexchanged="Rb_Documento_SelectedIndexChanged">
                    <asp:ListItem Value="TRUE" Selected="True">SI</asp:ListItem>
                    <asp:ListItem Value="FALSE">NO</asp:ListItem>
                </asp:RadioButtonList>
                                        </td>
                                        <td>
                Fecha Libro Compras:</td>
                                        <td>
                <asp:TextBox ID="tb_fecha_libro_compras" runat="server" Height="16px" 
                    Width="128px"></asp:TextBox>
                <ajaxToolkit:MaskedEditExtender ID="tb_fecha_libro_compras_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_libro_compras">
                </ajaxToolkit:MaskedEditExtender>
                <ajaxToolkit:CalendarExtender ID="tb_fecha_libro_compras_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_libro_compras" 
                    Format="MM/dd/yyyy">
                </ajaxToolkit:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <asp:Button ID="btn_actualizar_libro_compras" runat="server" 
                                                Text="Actualizar Libro Compras" 
                                                onclick="btn_actualizar_libro_compras_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4" bgcolor="#D9ECFF">
                                <strong>Retenciones a aplicar</strong></td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                <asp:GridView ID="gv_retenciones" runat="server" AutoGenerateColumns="False" Font-Bold="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Seleccionar">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_retencion" runat="server"/>
                                <asp:Label ID="lb_retencion" runat="server" Text='<%# Eval("ret_id") %>' Visible="false"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre">
                            <ItemTemplate>
                                <asp:Label ID="lb_nombre" runat="server" Text='<%# Eval("ret_nombre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NoDocumento">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_codigo" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Button ID="btn_generar_retenciones" runat="server" 
                                    Text="Aplicar Retenciones" onclick="btn_generar_retenciones_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                    <tr>
                                        <td colspan="6" style="text-decoration: underline">
                                            Documentos del Proveedor</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Serie</td>
                                        <td>
                    <asp:TextBox ID="tb_documento_serie" runat="server" Height="16px" Width="150px" MaxLength="49" 
                                                ReadOnly="True"></asp:TextBox> 
                                        </td>
                                        <td>
                                            Correlativo</td>
                                        <td>
                                            <asp:TextBox ID="tb_documento_correlativo" 
                        runat="server" Height="16px" Width="100px" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            Fecha</td>
                                        <td>
                                            <asp:TextBox ID="tb_fechadoc" runat="server" Height="16px" 
                        Width="128px" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                    <asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label>
                            </td>
                            <td>
        <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="200px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lb_mbl" runat="server" Text="MBL"></asp:Label>
                            </td>
                            <td>
        <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="200px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lb_routing" runat="server" Text="Routing"></asp:Label>
                            </td>
                            <td>
        <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="200px" ReadOnly="True"></asp:TextBox>
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
                                &nbsp;</td>
                        </tr>
                        </table>
                </td>
            </tr>
            </tbody>
            </table>
        </td>
    </tr>
    <tr><td align="center">
        
                <table width="650">
                <thead>
                <tr>
                    <th>Detalle de provision</th>
                </tr>
                </thead>
                <tr><td>
                
                    <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="97%">
            <Columns>
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
                <asp:TemplateField HeaderText="Tipo" visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lb_tipo" runat="server" visible="true" Text='<%# Eval("TYPE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Moneda">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo" runat="server" Text='<%# Eval("MONEDATYPE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Equivalente" >
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" visible="true" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subtotal" visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lb_subtotal" runat="server"  visible="true" Text='<%# Eval("SUBTOTAL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Impuesto" visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lb_impuesto" runat="server" visible="true" Text='<%# Eval("IMPUESTO") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate>
                        <asp:Label ID="lb_total" runat="server" Text='<%# Eval("TOTAL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DFID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lb_dfid" runat="server" Text='<%# Eval("DFID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="COSTOID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lb_costoid" runat="server" Text='<%# Eval("COSTOID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
                
                </td>
                </tr>
                </table>
    
    </td></tr>
    <tr><td>
    <!--
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
                    <asp:TextBox ID="tb_totaldolares" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Sub-Total"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_subtotal" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Impuesto"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_impuesto" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Total"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_total" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>    
        -->
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
    
    </table>
</fieldset>
</div>
</asp:Content>

