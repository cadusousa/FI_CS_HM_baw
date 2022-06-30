<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="addfactura.aspx.cs" Inherits="manager_addfactura" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <br />
    <div id="box">
        <h3 id="adduser">DOCUMENTOS<asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </h3>

	        <fieldset id="personal">
                <legend>Información del documento</legend>
                <br />
                <label>Sucursal :</label> 
                <asp:DropDownList ID="lb_suc" runat="server" Enabled="False">
                </asp:DropDownList>
                <br /><br />
                <label>Tipo de documento :</label> 
                <asp:DropDownList ID="lb_tipo_doc" runat="server" Enabled="False">
                    <asp:ListItem Value="1">Factura</asp:ListItem>
                    <asp:ListItem Value="2">Recibo</asp:ListItem>
                    <asp:ListItem Value="3">Nota Credito</asp:ListItem>
                    <asp:ListItem Value="4">Nota Debito</asp:ListItem>
                    <asp:ListItem Value="5">Contraseña de provisiones</asp:ListItem>
                    <asp:ListItem Value="7">Orden de Compra</asp:ListItem>
                    <asp:ListItem Value="8">Contraseña de facturas</asp:ListItem>
                    <asp:ListItem Value="9">Provisiones Agentes</asp:ListItem>
                    <asp:ListItem Value="11">Corte para estados de cuenta proveedores</asp:ListItem>
                    <asp:ListItem Value="12">Nota credito proveedores</asp:ListItem>
                    <asp:ListItem Value="14">Factura Proforma</asp:ListItem>
                    <asp:ListItem Value="15">Ajuste Contable</asp:ListItem>
                    <asp:ListItem Value="18">Ajustes (NC)</asp:ListItem>
                    <asp:ListItem Value="20">Retenciones</asp:ListItem>
                    <asp:ListItem Value="22">Recibo Corte</asp:ListItem>
                    <asp:ListItem Value="24">Liquidaciones</asp:ListItem>
                    <asp:ListItem Value="31">Nota Credito a Nota Debito prov</asp:ListItem>
                    
                </asp:DropDownList>
                <br /><br />
                <label>Serie : </label> 
                <asp:TextBox ID="tb_serie" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                <br /><br />
                <label>Correlativo siguiente : </label>
                <asp:TextBox ID="tb_NoInicial" runat="server" Height="16px" MaxLength="9"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="tb_NoInicial_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" FilterType="Numbers" 
                    TargetControlID="tb_NoInicial">
                </cc1:FilteredTextBoxExtender>
                <br />
                <br />
                Estado:
                <asp:DropDownList ID="drp_estado" runat="server">
                    <asp:ListItem Value="1">Activa</asp:ListItem>
                    <asp:ListItem Value="0">InActiva</asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                <asp:Panel ID="pnl_datos_ocultos" runat="server" Visible="False">
                
                        <asp:TextBox ID="tb_facid" runat="server" Enabled="false" Height="16px"></asp:TextBox>
                
                        <br />
                        <br />
                        Moneda:
                        <asp:DropDownList ID="lb_moneda" runat="server" Height="22px" Visible="False" 
                            Width="129px">
                        </asp:DropDownList>
                        <br />
                        <br />
                        Simbolo de moneda:
                        <asp:TextBox ID="tb_simbolo" runat="server" Height="16px"></asp:TextBox>
                        <br /><br />
                        Tipo Letra:
                        <asp:TextBox ID="tb_font" runat="server" Height="16px"></asp:TextBox>
                        <br /><br />
                        Interlineado(para detalle de factura): 
                        <asp:TextBox ID="tb_interlineado" runat="server" Height="16px"></asp:TextBox>
                        <br /><br />
                        Tamaño de letra:
                        <asp:TextBox ID="tb_size" runat="server" Height="16px"></asp:TextBox>
                        <br /><br />
                        Corrimiento para rubros excentos:
                        <asp:TextBox ID="tb_corrimiento" runat="server" Height="16px"></asp:TextBox>
                        <br />
                        <br />
                        Tipo de Contabilidad:
                        <asp:DropDownList ID="lb_tipo_contabilidad" runat="server" Height="22px" 
                        Width="129px">
                        </asp:DropDownList>
                        <br />
                        <br />
                        Tipo de Operacion:
                        <asp:DropDownList ID="lb_tipo_operacion" runat="server" Height="22px" 
                        Width="129px">
                        <asp:ListItem Value="1">Facturacion</asp:ListItem>
                        <asp:ListItem Value="2">Operaciones</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <br />

                        <asp:Panel ID="Panel3" runat="server" Visible="False">
                        Tipo de Documento:
                        <asp:DropDownList ID="lb_tipo_documento" runat="server">
                            <asp:ListItem Value="0">No Electronico</asp:ListItem>
                            <asp:ListItem Value="1">Electronico</asp:ListItem>
                            <asp:ListItem Value="2">Copia</asp:ListItem>
                        </asp:DropDownList>
                        </asp:Panel>
                        <br />

                        <asp:Panel ID="Panel1" runat="server" Visible="False">
                        Tipo Factura&nbsp;
                        <asp:DropDownList ID="lb_tipo_factura" runat="server">
                        </asp:DropDownList>
                        </asp:Panel>
                        <br />

                        <asp:Panel ID="Panel2" runat="server" Visible="False">
                        Formato Documento&nbsp;
                        <asp:DropDownList ID="lb_formato_documento" runat="server">
                        </asp:DropDownList>
                        </asp:Panel>
                        <br />




                        <asp:Panel ID="Panel4" runat="server" Visible="False">
                        
                        Dispositivo Electronico:
                        <asp:TextBox ID="tb_dispositivo_electronico" runat="server" Height="16px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="tb_dispositivo_electronico_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" 
                            TargetControlID="tb_dispositivo_electronico">
                        </cc1:FilteredTextBoxExtender>
                        <br />
                        <br />

                        </asp:Panel>

                </asp:Panel>



                        
                <asp:Panel ID="Panel5" runat="server" Visible="False">
                        
                        <br /><br />
                        <h3>Seccion Autorizacion Resolucion, Rangos</h3>
                        <br />

                        Numero de Autorizacion :<asp:TextBox ID="tb_numero_autorizacion" runat="server" 
                        Height="16px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="tb_numero_autorizacion_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers" 
                        TargetControlID="tb_numero_autorizacion">
                        </cc1:FilteredTextBoxExtender>
                        <br />
                        <br />
                        Fecha de Resolucion:<asp:TextBox ID="tb_fecha_resolucion" runat="server" 
                        Height="16px" ></asp:TextBox>
                        <cc1:CalendarExtender ID="tb_fecha_resolucion_CalendarExtender" 
                        runat="server" Enabled="True" TargetControlID="tb_fecha_resolucion" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="tb_fecha_resolucion_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                        TargetControlID="tb_fecha_resolucion">
                        </cc1:MaskedEditExtender>
                        <br />
                        <br />

                        Rango Inicial Autorizado:<asp:TextBox ID="tb_rango_inicial" runat="server" 
                            Height="16px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="tb_rango_inicial_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" 
                            TargetControlID="tb_rango_inicial">
                        </cc1:FilteredTextBoxExtender>
                        <br />
                        <br />
                        Rango Final Autorizado:<asp:TextBox ID="tb_rango_final" runat="server" 
                            Height="16px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="tb_rango_final_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" 
                            TargetControlID="tb_rango_final">
                        </cc1:FilteredTextBoxExtender>
                                
                         <br /><br />
                </asp:Panel>

               
            </fieldset>
            <br />
          <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" 
                  onclick="bt_Enviar_Click" />&nbsp;
            <input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
              <asp:Button ID="btn_regresar" runat="server" PostBackUrl="~/invoice/index.aspx" 
                  Text="Inicio" Visible="False" />
          </div>

    </div>
    <asp:Panel ID="pnl_campos" runat="server" Visible="False">
        <div id="box">
            <h3>
                Campos de la facturaa</h3>
            <asp:GridView ID="gv_conf_cuentas" runat="server" Width="400px" 
            AutoGenerateColumns="False" onrowdatabound="gv_conf_cuentas_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Campo">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Campo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Posicion X">
                        <ItemTemplate>
                            <asp:TextBox ID="tb_posx" runat="server" Height="16px" 
                            Text='<%# Eval("PosicionX") %>' Width="120px"></asp:TextBox>
                            <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="tb_posx" ErrorMessage="Solo se permiten números" 
                            SetFocusOnError="True" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Posicion Y">
                        <ItemTemplate>
                            <asp:TextBox ID="tb_posy" runat="server" Text='<%# Eval("PosicionY") %>' 
                            Height="16px" Width="120px"></asp:TextBox>
                            <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                            ControlToValidate="tb_posy" ErrorMessage="Solo se permiten números" 
                            SetFocusOnError="True" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Notas">
                        <ItemTemplate>
                            <asp:TextBox ID="tb_notas" runat="server" Text='<%# Eval("Notas") %>' 
                            Height="16px" Width="120px"></asp:TextBox>
                            <br />
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table width="400" align="center">
                <tr>
                    <td align="center">
                        <asp:Button ID="bt_preview" runat="server" onclick="bt_preview_Click" 
                Text="Previsualizar" />
                    </td>
                </tr>
            </table>
            <!--- finaliza modulo --->
        </div>
    </asp:Panel>
    &nbsp;
        
    </asp:Content>

