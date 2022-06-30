<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="provisiones.aspx.cs" Inherits="operations_provisiones" Title="AIMAR - BAW" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
<h3 id="adduser">PROVISIONES</h3>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {
        var stb_cuenta = document.getElementById("ctl00_Contenido_tb_cuenta");
        var stb_cta_nombre= document.getElementById("ctl00_Contenido_tb_cta_nombre");
        stb_cuenta.value = "";
        stb_cta_nombre.value="";
    }  

    function mpeSeleccionOnCancel()
    {
        var stb_nit = document.getElementById("ctl00_Contenido_tb_nit");
        var stb_nombre=document.getElementById("ctl00_Contenido_tb_nombre");
        stb_nit.value = "";
        stb_nombre.value="";
        stb_nit.style.backgroundColor = "#FFFFFF";
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
    <br />
    <div align="center">
            <table align="center">
            <tr><td align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td>
                            Transaccion</td>
                        <td height="30px">
                <asp:DropDownList ID="lb_transaccion" runat="server" Height="22px" AutoPostBack="True">
                </asp:DropDownList>
                        </td>
                        <td>
                            <strong>MONEDA</strong></td>
                        <td>
                <asp:DropDownList ID="lb_moneda" runat="server" Height="22px" Width="161px" 
                                Font-Bold="True">
                </asp:DropDownList>
                <asp:Label ID="lbl_ted_id" runat="server" Text="0" Visible="False"></asp:Label>
                            <asp:Label ID="lb_proveedorID" runat="server" Text="0" Visible="False"></asp:Label>
                <asp:Label ID="lb_paiID" runat="server" Text="0" Visible="False"></asp:Label>
                <asp:Label ID="lb_sucID" runat="server" Text="0" Visible="False"></asp:Label>
                <asp:Label ID="lb_provID" runat="server" Text="0" Visible="False"></asp:Label>
                <asp:Label ID="lb_proveedorCCHID" runat="server" Text="0" Visible="False"></asp:Label>
                <asp:Label ID="lbl_ocid" runat="server" Text="0" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" bgcolor="#D9ECFF" colspan="4">
                                Provision</td>
                    </tr>
                    <tr>
                        <td>
                                Serie</td>
                        <td>
                <asp:TextBox ID="tb_serie_provision" runat="server" ReadOnly="True" 
                    Width="80px" Height="16px"></asp:TextBox>
                        </td>
                        <td>
                            Correlativo</td>
                        <td>
                <asp:TextBox ID="tb_correlativo_provision" runat="server" ReadOnly="True" 
                    Width="80px" Height="16px"></asp:TextBox>
                    <asp:Label ID="lb_tpi" runat="server" Text="0" Visible="False"></asp:Label>
                
                        </td>
                    </tr>
                    <tr>
                        <td>
                                Fecha Emision&nbsp;</td>
                        <td>
                            <asp:TextBox ID="tb_fecha_emision" runat="server" Height="16px" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            Fecha Autorizacion</td>
                        <td>
                            <asp:TextBox ID="tb_fecha_autorizacion" runat="server" Height="16px" 
                                Width="200px"></asp:TextBox>
                
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#D9ECFF" colspan="4">
                                Contrasena</td>
                    </tr>
                    <tr>
                        <td style="height: 25px">
                                Serie</td>
                        <td style="height: 25px">
                            <asp:DropDownList ID="lb_serie_factura" runat="server" Enabled="False">
                </asp:DropDownList>
                        </td>
                        <td style="height: 25px">
                            Correlativo</td>
                        <td style="height: 25px">
                <asp:TextBox ID="tb_oc_correlativo" runat="server" 
                    Height="16px" Width="65px" ReadOnly="True" Enabled="False"></asp:TextBox>
                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tipo de Proveedor</td>
                        <td>
                            <asp:DropDownList ID="drp_tipo_persona" runat="server" Enabled="False">
                                    <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                        <asp:ListItem Value="2">Agente</asp:ListItem>
                                        <asp:ListItem Value="5">Naviera</asp:ListItem>
                                        <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                                        <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                                        <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                                    </asp:DropDownList>
                        </td>
                        <td>
                            Codigo</td>
                        <td>
                <asp:TextBox ID="tb_codigo_proveedor" runat="server" 
                    Height="16px" Width="65px" ReadOnly="True"></asp:TextBox>
                
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
                            Tipo de Operacion</td>
                        <td>
                            <asp:DropDownList ID="lb_imp_exp" runat="server">
                    </asp:DropDownList>
                <asp:Label ID="lbl_blID" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_tipoOperacionID" runat="server" Text="8" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Nombre</td>
                        <td colspan="3">
                            <asp:TextBox ID="tb_nombre" runat="server" Height="16px" Width="500px" 
                    ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Nit</td>
                        <td colspan="3">
                            <asp:TextBox ID="tb_nit" runat="server" Height="16px" Width="500px" 
                                ReadOnly="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>ESTADO</strong></td>
                        <td colspan="3" height="25px">
                            <asp:Label ID="lbl_ted_nombre" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
                                    <tr>
                                        <td colspan="4" bgcolor="#D9ECFF">
                                            Documentos del Proveedor</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Serie</td>
                                        <td>
                                            <asp:TextBox ID="tb_factura" runat="server" 
                    Height="16px" Width="150px"></asp:TextBox>
                                        </td>
                                        <td>
                                            Correlativo</td>
                                        <td>
                                            <asp:TextBox ID="tb_factura_correlativo" runat="server" Height="16px" Width="150px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="tb_factura_correlativo_FilteredTextBoxExtender" 
                                                runat="server" Enabled="True" FilterType="Numbers" 
                                                TargetControlID="tb_factura_correlativo">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
                                                <tr>
                                                    <td>
                                                        Fecha</td>
                                                    <td>
                                                        <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="100px" 
                                                            AutoPostBack="True" ontextchanged="tb_fecha_TextChanged"></asp:TextBox>
                                                        <cc1:MaskedEditExtender ID="tb_fecha_MaskedEditExtender" runat="server" 
                                                            Mask="99/99/9999" MaskType="Date" Enabled="True" 
                                                            TargetControlID="tb_fecha">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" 
                                                            Enabled="True" TargetControlID="tb_fecha" Format="MM/dd/yyyy">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        Credito</td>
                                                    <td>
                <asp:TextBox ID="lb_credito" runat="server" Height="16px" Width="100px" 
                    ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Fecha Pago</td>
                                                    <td>
                <asp:TextBox ID="lb_fechapago" runat="server" Height="16px" Width="100px" 
                    ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><asp:Label ID="lb_tipo_docto" runat="server" Text="Tipo Documento:" Visible="False"></asp:Label></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_tipo_documento" runat="server" Visible="False" 
                                                            Enabled="False">
                                                        </asp:DropDownList>
                                                     </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:RadioButtonList ID="rb_bienserv" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">Bien</asp:ListItem>
                    <asp:ListItem Value="2" Selected="True">Servicio</asp:ListItem>
                </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Panel ID="pnl_anulaciones" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 50%">
                                    <tr>
                                        <td align="center" bgcolor="#E6E6E6" colspan="4">
                                            <strong>Documento Anulado</strong></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;<asp:Label ID="lbl_persona_anula" runat="server" Font-Bold="False" 
                                                Font-Underline="True">Por</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_fecha_anulacion" runat="server" Font-Bold="False" 
                                                Font-Underline="True">Fecha</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_hora_anulacion" runat="server" Font-Bold="False" 
                                                Font-Underline="True">Hora</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_motivo_anulacion" runat="server" Font-Bold="False" 
                                                Font-Underline="True">Motivo</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Panel ID="pnl_oc" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 90%">
                                    <tr>
                                        <td align="center" bgcolor="#E6E6E6" colspan="3">
                                            <strong>
                                            <asp:Label ID="lb_or_compra" runat="server" Text="Orden de compra: Serie: " 
                                                Visible="False"></asp:Label>
                                            <asp:Label ID="lb_serieOC" runat="server" Text=" 0 " Visible="False"></asp:Label>
                                            <asp:Label ID="lb_or_compra2" runat="server" Text=" - Correlativo: " 
                                                Visible="False"></asp:Label>
                                            <asp:Label ID="lb_correlativoOC" runat="server" Text="0" Visible="False"></asp:Label>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 16px">
                                            <asp:Label ID="lbl_descripcion_bs" runat="server" Font-Bold="False" 
                                                Font-Underline="True" Text="Descripción de bienes o servicios"></asp:Label>
                                        </td>
                                        <td style="height: 16px">
                                            <asp:Label ID="lbl_observaciones_oc" runat="server" Font-Bold="False" 
                                                Font-Underline="True" Text="Observaciones Orden de Compra"></asp:Label>
                                        </td>
                                        <td style="height: 16px">
                                            <asp:Label ID="lbl_terminos_pago" runat="server" Font-Bold="False" 
                                                Font-Underline="True" Text="Términos de pago"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="D" bgcolor="#D9ECFF" colspan="4">
                            Datos de la Carga</td>
                    </tr>
                    <tr>
                        <td>
                            HBL</td>
                        <td>
                            <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            MBL</td>
                        <td>
                            <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ROUTING</td>
                        <td>
                            <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            CONTENEDOR</td>
                        <td>
                            <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            POLIZA</td>
                        <td>
                <asp:TextBox ID="tb_poliza" runat="server" Height="16px" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            CERTIFICADO DE SEGUROS</td>
                        <td>
                <asp:TextBox ID="tb_poliza_seguros" runat="server" Height="16px" Width="200px" 
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Observaciones</td>
                        <td colspan="3">
                            <asp:TextBox ID="tb_observacion" runat="server" Height="16px" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#D9ECFF" colspan="4">
                            Libro de Compras</td>
                    </tr>
                    <tr>
                        <td>
                            Asignar</td>
                        <td>
                <asp:RadioButtonList ID="Rb_Documento" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="TRUE" Selected="True">SI</asp:ListItem>
                    <asp:ListItem Value="FALSE">NO</asp:ListItem>
                </asp:RadioButtonList>
                        </td>
                        <td>
                            Fecha</td>
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
                    <tr>
                        <td align="left" bgcolor="#D9ECFF" colspan="4" style="height: 18px">
                            Retenciones a aplicar</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                <asp:GridView ID="gv_retenciones" runat="server" AutoGenerateColumns="False">
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
                <asp:CheckBoxList ID="chklist_retencion" runat="server" RepeatColumns="4" 
                    RepeatDirection="Horizontal">
                </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                <asp:Panel ID="Pnl_Mayan_Logistics" runat="server" Visible="False">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                        <tr>
                            <td align="left" bgcolor="#D9ECFF" colspan="2">
                                Ruta</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="lb_ruta_pais" runat="server">
                                    <asp:ListItem>GUATEMALA</asp:ListItem>
                                    <asp:ListItem>EL SALVADOR</asp:ListItem>
                                    <asp:ListItem>HONDURAS</asp:ListItem>
                                    <asp:ListItem>NICARGUA</asp:ListItem>
                                    <asp:ListItem>COSTA RICA</asp:ListItem>
                                    <asp:ListItem>PANAMA</asp:ListItem>
                                    <asp:ListItem>BELICE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="lb_ruta" runat="server">
                                    <asp:ListItem>RUTA 1 NORTE</asp:ListItem>
                                    <asp:ListItem>RUTA 1 SUR</asp:ListItem>
                                    <asp:ListItem>RUTA 2</asp:ListItem>
                                    <asp:ListItem>RUTA 3</asp:ListItem>
                                    <asp:ListItem>RUTA 4</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left" bgcolor="#D9ECFF">
                            Totales</td>
                    </tr>
                    <tr>
                        <td>
                            No Afecto</td>
                        <td>
                <asp:TextBox ID="tb_noafecto" runat="server" Height="16px" Width="200px" 
                    AutoPostBack="True" ontextchanged="tb_noafecto_TextChanged" ReadOnly="True"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="tb_noafecto_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" TargetControlID="tb_noafecto" FilterType="Numbers,Custom" ValidChars=".">
                </cc1:FilteredTextBoxExtender>
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
                    </table>
                </td></tr>
            
            <tr><td align="center">
            <br />
                <table width="650">
                <thead>
                <tr>
                    <th>Detalle de provision</th>
                </tr>
                </thead>
                <tr><td>
                
                    <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="97%" 
                        onrowdeleting="gv_detalle_RowDeleting" ondatabound="gv_detalle_DataBound" 
                        onrowcreated="gv_detalle_RowCreated" 
                        onrowdatabound="gv_detalle_RowDataBound">
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
                <asp:TemplateField HeaderText="Tipo" visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lb_tipo" runat="server" visible="true" Text='<%# Eval("TYPE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Moneda">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo" runat="server" Text='<%# Eval("MONEDATYPE") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="#CCFFFF" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subtotal" visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lb_subtotal" runat="server"  visible="true" Text='<%# Eval("SUBTOTAL") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Impuesto" visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lb_impuesto" runat="server" visible="true" Text='<%# Eval("IMPUESTO") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate>
                        <asp:Label ID="lb_total" runat="server" Text='<%# Eval("TOTAL") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="#CCFFFF" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Equivalente" >
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" visible="true" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
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
                
                    <asp:GridView ID="gv_detalle_anular" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="97%" 
                        onrowdeleting="gv_detalle_RowDeleting" ondatabound="gv_detalle_DataBound" 
                        onrowcreated="gv_detalle_RowCreated" Visible="False">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" />
                <asp:TemplateField HeaderText="Codigo">
                    <ItemTemplate>
                        <asp:Label ID="lb_codigo2" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rubro">
                    <ItemTemplate>
                        <asp:Label ID="lb_rubro2" runat="server" Text='<%# Eval("NOMBRE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo" visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lb_tipo2" runat="server" visible="true" 
                            Text='<%# Eval("TYPE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Moneda">
                    <ItemTemplate>
                        <asp:Label ID="lb_monedatipo2" runat="server" Text='<%# Eval("MONEDATYPE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total USD" visible="false" >
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares2" visible="false" runat="server" 
                            Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subtotal" visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lb_subtotal2" runat="server"  visible="true" 
                            Text='<%# Eval("SUBTOTAL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Impuesto" visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lb_impuesto2" runat="server" visible="true" 
                            Text='<%# Eval("IMPUESTO") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate>
                        <asp:Label ID="lb_total2" runat="server" Text='<%# Eval("TOTAL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DFID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lb_dfid2" runat="server" Text='<%# Eval("DFID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="COSTOID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lb_costoid2" runat="server" Text='<%# Eval("COSTOID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
                    <asp:Label ID="Label1" runat="server" Text="0" Visible="False"></asp:Label>
                
                </td>
                </tr>
                <tr><td>
                
             <asp:Button ID="bt_agregar" runat="server" Text="Agregar Rubro" />
    
            <cc1:ModalPopupExtender ID="btn_rubro_ModalPopupExtender" runat="server" TargetControlID="bt_agregar"
            PopupControlID="pnlrubros" CancelControlID="cancela_busqueda_rubro"
            OnCancelScript="mpecancela_busqueda_rubro()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
            
            
            
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
                <td colspan="2" align="center">
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
            
                </td>
                </tr>
                
                <tr>
                <td>
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
                </table>
            </td></tr>   
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="bt_guardar" runat="server" Text="Autorizar" 
                                onclick="bt_guardar_Click" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"  />
                            </td>
                            <td>
                                <asp:Button ID="bt_cancelar" runat="server" Text="Cancelar" 
                                    onclick="bt_cancelar_Click" />
                            </td>
                             <td>
                                <asp:Button ID="bt_ImprimirOCVirtual" runat="server" Text="Impresion Virtual" 
                                    onclick="bt_ImprimirOC_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
</div>
</asp:Content>