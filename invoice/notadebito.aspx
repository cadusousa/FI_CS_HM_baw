<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="notadebito.aspx.cs" Inherits="invoice_notadebito" Title="AIMAR - BAW" %>

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

        function VerificarUsuarioCambioCliente() {
            var u = document.getElementById("txtUsuarioCambioDeCliente").value;
            var c = document.getElementById("txtContraseniaCambioDeCliente").value;
            if (u.trim() == "") {
                alert("Ingrese un usuario para poder continuar");
                return false;
            }
            else {
                if (c.trim() == "") {
                    alert("Ingrese una contraseña para continuar");
                    return false;
                }
                else
                    return true;
            }
        }
    </script>
    <script type="text/javascript">
        document.onkeydown = function (event) {

            if (window.event && (window.event.keyCode == 27)) {
                window.event.keyCode = 505;
                alert("Tecla Esc deshabilitada");
            }
            if (window.event.keyCode == 505) {
                return false;
            }
        } 
    </script>
        <script type="text/javascript">
            document.onkeydown = function (event) {
                if (event.keyCode == 37 && event.altKey) {
                    return false;
                }
                if (event.keyCode == 39 && event.altKey) {
                    return false;
                }
                if (event.keyCode == 8 && event.target.localName == "body") {
                    return false;
                }
            } 
    </script>
<div id="box" align="center">
<fieldset id="Fieldset1">
<h3 id="adduser" align="left">NOTA DEBITO / RECORDATORIO DE PAGO</h3>
<table width="650">
    <tr><td>
        <table width="650" align="left">
        <tbody>
            <tr>
                <td>
                        <asp:Label ID="lbl_blID" runat="server" Text="0" Visible="False"></asp:Label>
                        <asp:Label ID="lbl_tipoOperacionID" runat="server" Text="11" Visible="False"></asp:Label>
                        &nbsp;<asp:Label ID="lbl_whitelist" runat="server" Text="FALSE" Visible="False"></asp:Label>
                    &nbsp;<asp:Label ID="lbl_restricciones" runat="server" Text="FALSE" Visible="False"></asp:Label>
                        &nbsp;&nbsp;<asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tb_agenteid" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:TextBox ID="tb_comix_agente" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:Label ID="lb_facid" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="id_shipper" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_consignee" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_comodities" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lb_requierealias" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lbl_tipo_serie" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_serie_id" runat="server" Text="0" Visible="False"></asp:Label>
                        <br />
                        <table align="center" cellpadding="0" cellspacing="0" style="width: 99%">
                            <tr>
                                <td align="center" colspan="4">
                    <input id="Button2" type="button" value="Cargar a partir de un DOCUMENTO"  
                        onclick="javascript:window.open('searchBL_ND.aspx', null, 'toolbar=no,resizable=no,status=no,width=400,height=600');"/></td>
                            </tr>
                            <tr>
                                <td>
                                    Tipo de operacion</td>
                                <td>
                                    <asp:DropDownList ID="lb_tipo_transaccion" runat="server" 
                        Enabled="False">
                    </asp:DropDownList>
                                </td>
                                <td>
                                    <strong>Moneda</strong></td>
                                <td>
                                    <asp:DropDownList ID="lb_moneda" runat="server" Font-Bold="True" 
                                        Enabled="False">
                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                            <asp:Label ID="lbl_tipo_serie_caption" runat="server" 
                                                style="text-align: center" Text="Serie Nota Debito"></asp:Label>
                                        </td>
                                <td>
                                    <asp:DropDownList ID="lb_serie_factura" runat="server" 
                        AutoPostBack="True" 
                        onselectedindexchanged="lb_serie_factura_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:TextBox ID="tb_correlativo" runat="server" Enabled="False" Height="16px" 
                        Width="50px">0</asp:TextBox>
                                </td>
                                <td>
                                            Codigo de Cliente</td>
                                <td>
                                    <asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True">0</asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td align="center" colspan="4">
                                   <table width="650" align="center">
                                    <thead>
                                      <tr>
                                          <th colspan="4">Referenciar Factura</th>
                                      </tr>
                                    </thead>
                                    <tr>
                                    <td style="width: 66px">Serie:</td>
                                    <td style="width: 134px"><asp:TextBox ID="tbSerieFacturaRef" ReadOnly="true" runat="server" Text=""></asp:TextBox></td>
                                    <td style="width: 86px">Correlativo:</td>
                                    <td>
                                        <asp:TextBox ID="tbCorrelativoFacturaRef" ReadOnly="true" runat="server" Text=""></asp:TextBox>
                                        <asp:TextBox ID="tbFechaFacturaRef" ReadOnly="true" runat="server" Text=""></asp:TextBox>
                                        <asp:TextBox ID="tbDocFacturaRef" ReadOnly="true" runat="server" Text=""></asp:TextBox>
                                        <asp:HiddenField ID="hdIdFacturaRef" runat="server" />
                                    </td>
                                    </tr>
                                   </table> 
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    Nombre</td>
                                <td colspan="3">
                                    <asp:TextBox 
                        ID="tb_nombre" runat="server" Width="500px" 
                        Height="15px" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nit</td>
                                <td colspan="3">
                    <asp:TextBox ID="tb_nit" runat="server" ReadOnly="True" Height="15px" 
                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Panel ID="pnl_tipo_identificacion_cliente" runat="server" Visible="False">
                                        <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 50%">
                                            <tr>
                                                <td style="font-weight: bold">
                                                    Tipo de Identificacion</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_tipo_identificacion_cliente" runat="server" 
                                                        Enabled="False">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                Razon Social</td>
                                <td colspan="3">
                                    <asp:TextBox ID="tb_razon" runat="server" 
                        Height="15px" Width="500px" ReadOnly="True"></asp:TextBox>
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
                                    <asp:DropDownList ID="lb_imp_exp" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_imp_exp_SelectedIndexChanged">
                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tipo de Cliente</td>
                                <td colspan="3">
                                <asp:Button ID="btnInfo" runat="server" OnClientClick="return false;" Text="-"  
                                    BorderStyle="None" BackColor="White"/>
                                <%--********************************************************--%>
        <div id="flyout" style="display:none; overflow: hidden; z-index: 2; background-color: #FFFFFF; border: solid 1px #D0D0D0;"></div>
        <div id="info" runat="server" style="display:none ; width: 250px; z-index: 2; opacity: 0; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0); font-size: 12px; border: solid 1px #CCCCCC; background-color: #FFFFFF; padding: 5px;">
            <div id="btnCloseParent" style="float: right; opacity: 0; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0);">
                <asp:LinkButton id="btnClose" runat="server" OnClientClick="return false;" Text="X" ToolTip="Close"
                    Style="background-color: #666666; color: #FFFFFF; text-align: center; font-weight: bold; text-decoration: none; border: outset thin #FFFFFF; padding: 5px;" />
            </div>
            <div>
                <asp:Panel ID="pnl_credito" runat="server" Visible="False">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                        <tr>
                            <td>
                                <strong>Tiempo Autorizado</strong></td>
                            <td bgcolor="#D9ECFF" align="right">
                                <asp:Label ID="lbl_tiempo_autorizado" runat="server" Text="0"></asp:Label>
                                &nbsp;(Dias)</td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Monto Autorizado</strong></td>
                            <td bgcolor="#D9ECFF" align="right">
                                <asp:Label ID="lbl_monto_autorizado" runat="server" Text="0"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
        <script type="text/javascript" language="javascript">
            function Cover(bottom, top, ignoreSize) {
                var location = Sys.UI.DomElement.getLocation(bottom);
                top.style.position = 'absolute';
                top.style.top = location.y + 'px';
                top.style.left = location.x + 'px';
                if (!ignoreSize) {
                    top.style.height = bottom.offsetHeight + 'px';
                    top.style.width = bottom.offsetWidth + 'px';
                }
            }
        </script>
        <ajaxToolkit:AnimationExtender id="OpenAnimation" runat="server" TargetControlID="btnInfo">
            <Animations>
                <OnClick>
                    <Sequence>
                        <EnableAction Enabled="false" />
                        <StyleAction AnimationTarget="info" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="info" Duration=".2"/>
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                        <Parallel AnimationTarget="info" Duration=".5">
                            <Color PropertyKey="color" StartValue="#666666" EndValue="#FF0000" />
                            <Color PropertyKey="borderColor" StartValue="#666666" EndValue="#FF0000" />
                        </Parallel>
                        <Parallel AnimationTarget="info" Duration=".5">
                            <Color PropertyKey="color" StartValue="#FF0000" EndValue="#666666" />
                            <Color PropertyKey="borderColor" StartValue="#FF0000" EndValue="#666666" />
                            <FadeIn AnimationTarget="btnCloseParent" MaximumOpacity=".9" />
                        </Parallel>
                    </Sequence>
                </OnClick>
            </Animations>
        </ajaxToolkit:AnimationExtender>
        <ajaxToolkit:AnimationExtender id="CloseAnimation" runat="server" TargetControlID="btnClose">
            <Animations>
                <OnClick>
                    <Sequence AnimationTarget="info">
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>
                        <StyleAction Attribute="display" Value="none"/>
                        <StyleAction Attribute="width" Value="230px"/>
                        <StyleAction Attribute="height" Value=""/>
                        <StyleAction Attribute="fontSize" Value="12px"/>
                        <OpacityAction AnimationTarget="btnCloseParent" Opacity="0" />
                        <EnableAction AnimationTarget="btnInfo" Enabled="true" />
                    </Sequence>
                </OnClick>
                <OnMouseOver>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FFFFFF" EndValue="#FF0000" />
                </OnMouseOver>
                <OnMouseOut>

                    <Color Duration=".2" PropertyKey="color" StartValue="#FF0000" EndValue="#FFFFFF" />
                </OnMouseOut>
             </Animations>
        </ajaxToolkit:AnimationExtender>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    Direccion</td>
                                <td colspan="3">
                                    <asp:TextBox ID="tb_direccion" runat="server" 
                        Height="15px" Width="500px" 
                        ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Notas</td>
                                <td colspan="3">
                    <asp:TextBox ID="tb_observaciones" runat="server" Height="15px" Width="500px" 
                        MaxLength="250"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                <asp:Panel ID="pnl_documento_electronico" runat="server">
                                    <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 90%">
                                        <tr>
                                            <td>
                                                Correo para Recibir Documento Electronico</td>
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
                        </table>
                    </td>
            </tr>
            <tr>
                <td>
                                        <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                            <tr>
                                                <td>
                                        <asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
        <asp:Label ID="lb_mbl" runat="server" Text="MBL"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
        <asp:Label ID="lb_routing" runat="server" Text="Routing"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
        <asp:Label ID="lb_contenedor" runat="server" Text="Contenedor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True" style="text-transform:uppercase;"></asp:TextBox>
                                        <cc1:MaskedEditExtender ID="tb_contenedor_MaskedEditExtender" runat="server" 
                                            Mask="AAAA999999C9"  MaskType="None" Filtered="-"
                                            Enabled="True" 
                                            TargetControlID="tb_contenedor">
                                        </cc1:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                        <asp:Label ID="lb_tipotranporte" runat="server" Text="Naviera"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_naviera" runat="server" Height="15px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                        <asp:Label ID="lb_transporte" runat="server" Text="Vapor"></asp:Label>
                                                </td>
                                                <td>
                    <asp:TextBox ID="tb_vapor" runat="server" Height="15px" Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Agente</td>
                                                <td>
                                        <asp:TextBox ID="tb_agente_nombre" runat="server" Height="16px" Width="250px" 
                                            ReadOnly="True"></asp:TextBox>
                                        
                                                </td>
                                                <td>
                                                    Shipper</td>
                                                <td>
                                        <asp:TextBox ID="tb_shipper" runat="server" Height="15px" 
                        Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Consignee</td>
                                                <td>
                    <asp:TextBox ID="tb_consignee" runat="server" Height="15px" Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Comodity</td>
                                                <td>
                                        <asp:TextBox ID="tb_comodity" runat="server" 
                                            Height="15px" Width="250px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Paquetes</td>
                                                <td>
                    <asp:TextBox ID="tb_paquetes1" runat="server" Height="15px" Width="50px" ReadOnly="True"></asp:TextBox>
                                                    <asp:TextBox ID="tb_paquetes2" runat="server" Height="15px" Width="170px" 
                                            ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                                        <tr>
                                                            <td>
                                                                Peso</td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td>
                    <asp:TextBox ID="tb_peso" runat="server" Height="15px" Width="50px" ReadOnly="True"></asp:TextBox>
                                                                Volumen</td>
                                                            <td>
                                                                <asp:TextBox ID="tb_vol" runat="server" Height="15px" Width="50px" 
                                            ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <table align="left" cellpadding="0" cellspacing="0" style="width: 80%">
                                                        <tr>
                                                            <td>
                                                                Vendedor:</td>
                                                            <td>
                    <asp:TextBox ID="tb_vendedor1" runat="server" Height="15px" Width="150px" ReadOnly="True"></asp:TextBox>
                                        
                                                            </td>
                                                            <td>
                                        
                                        <asp:TextBox ID="tb_vendedor2" runat="server" Width="150px" Height="15px" 
                                            ReadOnly="True"></asp:TextBox>
                                        
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Poliza Aduanal</td>
                                                <td>
                                                    <asp:TextBox ID="tb_orden" runat="server" Height="15px" Width="250px" 
                                                        MaxLength="45"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Recibo de aduana</td>
                                                <td>
                                                    <asp:TextBox ID="tb_reciboaduanal" runat="server" 
                                            Height="16px" Width="250px" MaxLength="45"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Dua Ingreso</td>
                                                <td>
                                                    <asp:TextBox ID="tb_dua_ingreso" runat="server" Height="15px" Width="250px" 
                                                        MaxLength="45"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Dua Salida</td>
                                                <td>
                    <asp:TextBox ID="tb_dua_salida" runat="server" Height="15px" Width="250px" MaxLength="45"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    ALL IN</td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="tb_allin" runat="server" Height="15px" 
                        Width="550px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                <asp:RadioButtonList ID="rb_bienserv" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">Bien</asp:ListItem>
                    <asp:ListItem Value="2" Selected="True">Servicio</asp:ListItem>
                </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnl_operacion" runat="server" Visible="False" align="center" Width="400px">
                        <table align="center" style="width: 50%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_sistema" runat="server" Font-Bold="True" Font-Italic="True"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lbl_operacion" runat="server" Font-Bold="True" 
                                        Font-Italic="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td align="center">
        <table width="650" align="center">
            <thead>
                      <tr>
                          <th>
                              Detalle de Rubros</th>
                      </tr>
                  </thead>
            <tr>
                <td align="center">
    
        <asp:GridView ID="gv_detalle" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No hay rubros que facturar" Width="90%" 
            ondatabound="gv_detalle_DataBound" onrowdeleting="gv_detalle_RowDeleting" 
                        onrowcreated="gv_detalle_RowCreated" Font-Size="X-Small">
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
                    <ItemStyle BackColor="#CCFFFF" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Equivalente">
                    <ItemTemplate>
                        <asp:Label ID="lb_totaldolares" runat="server" Text='<%# Eval("TOTALD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comentarios">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_comentario" runat="server" Text='<%# Eval("COMENTARIO") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CARGOID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lb_cargoid" runat="server" Text='<%# Eval("CARGOID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <asp:Label ID="lbl_local_internacional" runat="server" Text='<%# Eval("LOCAL_INTERNACIONAL") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
                </td>
            </tr>
        </table>
        </td></tr>
    <tr><td>
        <table width="310" align="right">
        <thead>
            <tr>
                <th align="left" colspan="2">Totales</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    
                    <asp:Label ID="Label6" runat="server" Text="Sub-Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_subtotal" runat="server" Height="16px" ReadOnly="True" style=" text-align:right"
                        BackColor="White" BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label7" runat="server" Text="Impuesto:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_impuesto" runat="server" Height="16px" ReadOnly="True" style=" text-align:right"
                        BackColor="White" BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="tb_total" runat="server" Height="16px" ReadOnly="True" style=" text-align:right"
                        BackColor="White" BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label2" runat="server" Text="Equivalente"></asp:Label>
                    :</td>
                <td>
                    
                    <asp:TextBox ID="tb_totaldolares" runat="server" Height="16px" ReadOnly="True" style=" text-align:right"
                        BackColor="White" BorderStyle="None">0</asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
            <asp:Button 
            ID="bt_agregar" runat="server" Text="Agregar Rubro" />
        </td></tr>
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
        </td>
        </tr>
        <tr>
        <td>
        <asp:Panel ID="pnl_transmision_electronica" runat="server" Visible="False">
            <table width="650">
                <thead>
                    <tr>
                        <th>
                            Detalle de Transmision</th>
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
        </td>
        </tr>
    <tr><td>
        <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar"  OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" 
                  onclick="bt_Enviar_Click" />&nbsp;
            <asp:Button ID="Cancel" runat="server" Text="Cancel" onclick="Cancel_Click" />&nbsp;
            <asp:Button ID="bt_imprimir" runat="server" Enabled="False" 
                onclick="bt_imprimir_Click" Text="Imprimir" />
          &nbsp;
            <asp:Button ID="bt_nota_debito_virtual" runat="server" Enabled="False" 
                onclick="bt_nota_debito_virtual_Click" Text="Nota Debito Virtual" />
          &nbsp;
            <asp:Button ID="bt_impresion_debit_note" runat="server" Visible="False" 
                Text="Nota Debito Virtual" onclick="bt_impresion_debit_note_Click" />
          </div>
    </td></tr>
    </table>
    
    
    <%--********************************************************--%>
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none;">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label1" runat="server" Text="Agregar Rubro" />
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
                <asp:TextBox ID="tb_monto" runat="server" Width="93px" Height="16px" >0.00</asp:TextBox>
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
            <td align="center" colspan="2">
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Aceptar" />
                <asp:Button ID="btn_siguiente_rubro" runat="server" onclick="Button1_Click" 
                    Text="Siguiente" />
                <asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" />
            </td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="bt_agregar"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <%--********************************************************--%>
    <asp:Panel ID="pnlCliente" runat="server" CssClass="CajaDialogo" Width="533px" style=" display:none;" DefaultButton="Button4">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label3" runat="server" Text="Agregar Cliente" />
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
                OnSelectedIndexChanging="gv_clientes_SelectedIndexChanging"
                AllowPaging="True" onpageindexchanged="gv_clientes_PageIndexChanged" 
                onpageindexchanging="gv_clientes_PageIndexChanging" PageSize="10">
            </asp:GridView>
            <asp:TextBox ID="txtOcultoParaPopUp" runat="server" style="display:none;" />
        </td></tr>
        </table>
        </div>
    </asp:Panel>
    <%--********************************************************--%>
    <asp:Panel ID="pnlShipper" runat="server" CssClass="CajaDialogo" Width="533px" style=" display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label5" runat="server" Text="Criterio de busqueda" />
        </div></td></tr>
        
        <tr>
            <td align="left" width="130">Nombre:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nombre_shipper" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> NIT:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nit_shipper" runat="server" Height="16px"></asp:TextBox>
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
    <asp:Panel ID="pnlConsignee" runat="server" CssClass="CajaDialogo" Width="533px" style=" display:none">
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
                <asp:TextBox ID="tb_nombre_consignee" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> NIT:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_nit_consignee" runat="server" Height="16px"></asp:TextBox>
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
    <asp:Panel ID="pnlComodities" runat="server" CssClass="CajaDialogo" Width="533px" style=" display:none">
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
                <asp:TextBox ID="tb_nombre_comoditie" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> Codigo:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_codigo_comoditie" runat="server" Height="16px"></asp:TextBox>
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
    
    <br />
    

<div id="box0">
<fieldset id="Fieldset2">
     <br />
     <asp:Panel ID="pnlFacturas" runat="server" CssClass="CajaDialogo" Width="800px" style=" display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label13" runat="server" Text="Agregar Factura" />
        </div></td></tr>
        <tr>
            <td align="left" width="130">Serie:</td>
            <td align="left"> 
                <asp:DropDownList ID="ddlSerieFacBusqueda" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left"> Correlativo:</td>
            <td align="left"> 
                <asp:TextBox ID="tbCorrelativoFacBusqueda" runat="server" Height="16px" Text=""></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btBuscarFactura" runat="server" Text="Buscar" 
                    onclick="btBuscarFactura_Click" />
            </td>
            <td><asp:Button ID="btnCancelarFacturas" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label25" runat="server" Text="Facturas" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_facturas" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_facturas_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_facturas_PageIndexChanged" 
                onpageindexchanging="gv_facturas_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
            
            
        
     </asp:Panel>

    <asp:Panel ID="pnlNavieras" runat="server" CssClass="CajaDialogo" Width="350px" style=" display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label15" runat="server" Text="Criterio de busqueda" />
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
                <asp:TextBox ID="tb_nombre_navieras" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> Codigo:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_codigo_navieras" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="aceptar_naviera" runat="server" Text="Aceptar" 
                    onclick="aceptar_naviera_Click" />
            </td>
            <td><asp:Button ID="cancelar_naviera" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label16" runat="server" Text="Navieras" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_navieras" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_navieras_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_navieras_PageIndexChanged" 
                onpageindexchanging="gv_navieras_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
    </asp:Panel>
         <asp:Panel ID="pnlUsuarioCambioDeCliente" runat="server" CssClass="CajaDialogo" Width="730px"
        style="display:none;" HorizontalAlign="Center" DefaultButton="btnAceptarCambioDeCliente">
        <div align="center">
            <table align="center">
                <tbody align="center">
                    <tr>
                        <td colspan="2">
                            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                                <asp:Label ID="Label26" runat="server" Text="AUTORIZADO POR..." />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="130">Usuario:</td>
                        <td align="left"><asp:TextBox autocomplete="off" ClientIDMode="Static" ID="txtUsuarioCambioDeCliente" runat="server" Height="16px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left" width="130">Contraseña:</td>
                        <td align="left"><asp:TextBox autocompelete="new-password" ClientIDMode="Static" TextMode="Password" ID="txtContraseniaCambioDeCliente" runat="server" Height="16px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnAceptarCambioDeCliente" runat="server" OnClientClick="javascript:return VerificarUsuarioCambioCliente();" OnClick="btnAceptarCambioDeCliente_Click" Text="Aceptar" />
                        </td>
                        <td>
                            <asp:Button ID="btnCancelarCambioDeCliente" runat="server" Text="Cancelar" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </asp:Panel>
        <cc1:ModalPopupExtender ID="modalFacturas" runat="server" TargetControlID="tbSerieFacturaRef"
            PopupControlID="pnlFacturas" CancelControlID="btnCancelarFacturas"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
        <cc1:ModalPopupExtender ID="modalcliente" runat="server" TargetControlID="tbCliCod"
            PopupControlID="pnlCliente" CancelControlID="Cancel_cliientebtn"
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
     <cc1:ModalPopupExtender ID="modalNavieras" runat="server" TargetControlID="tb_naviera"
            PopupControlID="pnlNavieras" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <cc1:ModalPopupExtender ID="modalAgentes" runat="server" TargetControlID="tb_agente_nombre"
            PopupControlID="pnlAgentes" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <cc1:ModalPopupExtender ID="modalVendedor1" runat="server" TargetControlID="tb_vendedor1"
            PopupControlID="pnlVendedor1" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <cc1:ModalPopupExtender ID="modalVendedor2" runat="server" TargetControlID="tb_vendedor2"
            PopupControlID="pnlVendedor2" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <cc1:ModalPopupExtender ID="modalPaquetes2" runat="server" TargetControlID="tb_paquetes2"
            PopupControlID="pnlPaquetes2" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <cc1:ModalPopupExtender ID="modalUsuarioCambioDeCliente" runat="server" TargetControlID="txtOcultoParaPopUp"
            PopupControlID="pnlUsuarioCambioDeCliente" CancelControlID="btnCancelarCambioDeCliente"
            OnCancelScript="mpeClienteOnCancel()" DropShadow="True" 
            BackgroundCssClass="FondoAplicacion" BehaviorID="modalUsuarioCambioDeCliente" />
            
    </fieldset>
    <br />
    <asp:Panel ID="pnlAgentes" runat="server" CssClass="CajaDialogo" Width="350px" style=" display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label17" runat="server" Text="Criterio de busqueda" />
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
                <asp:TextBox ID="tb_nombre_agentes" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> Codigo:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_codigo_agentes" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="aceptar_agentes" runat="server" Text="Aceptar" 
                    onclick="aceptar_agentes_Click" />
            </td>
            <td><asp:Button ID="cancelar_agentes" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label18" runat="server" Text="Agentes" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_agentes" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_agentes_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_agentes_PageIndexChanged" 
                onpageindexchanging="gv_agentes_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
    </asp:Panel>
    
    <br />
    <asp:Panel ID="pnlVendedor1" runat="server" CssClass="CajaDialogo" 
        Width="350px" style=" display:none ">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label19" runat="server" Text="Criterio de busqueda" />
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
                <asp:TextBox ID="tb_nombre_vendedor1" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> Codigo:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_codigo_vendedor1" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="aceptar_vendedor1" runat="server" Text="Aceptar" 
                    onclick="aceptar_vendedor1_Click" />
            </td>
            <td><asp:Button ID="cancelar_agentes0" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label20" runat="server" Text="Vendedores" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_vendedores1" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_vendedores1_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_vendedores1_PageIndexChanged" 
                onpageindexchanging="gv_vendedores1_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
    </asp:Panel>
    
    <br />
    <asp:Panel ID="pnlVendedor2" runat="server" CssClass="CajaDialogo" 
        Width="350px" style=" display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label21" runat="server" Text="Criterio de busqueda" />
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
                <asp:TextBox ID="tb_nombre_vendedor2" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> Codigo:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_codigo_vendedor2" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="aceptar_vendedor2" runat="server" Text="Aceptar" 
                    onclick="aceptar_vendedor2_Click" />
            </td>
            <td><asp:Button ID="cancelar_agentes1" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label22" runat="server" Text="Vendedores" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_vendedores2" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_vendedores2_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_vendedores2_PageIndexChanged" 
                onpageindexchanging="gv_vendedores2_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
    </asp:Panel>
    
    <asp:Panel ID="pnlPaquetes2" runat="server" CssClass="CajaDialogo" 
        Width="350px" style="display:none">
        <div>
        <table>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label23" runat="server" Text="Criterio de busqueda" />
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
                <asp:TextBox ID="tb_nombre_paquetes2" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left"> Codigo:</td>
            <td align="left"> 
                <asp:TextBox ID="tb_codigo_paquetes2" runat="server" Height="16px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="aceptar_paquetes2" runat="server" Text="Aceptar" 
                    onclick="aceptar_paquetes2_Click" />
            </td>
            <td><asp:Button ID="cancelar_agentes2" runat="server" Text="Cancelar" /></td>
        </tr>
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label24" runat="server" Text="Paquetes" />
        </div></td></tr>
        <tr><td colspan="2">
        
            <asp:GridView ID="gv_paquetes2" runat="server" AutoGenerateSelectButton="True" 
                onselectedindexchanged="gv_paquetes2_SelectedIndexChanged" 
                AllowPaging="True" onpageindexchanged="gv_paquetes2_PageIndexChanged" 
                onpageindexchanging="gv_paquetes2_PageIndexChanging" PageSize="10">
            </asp:GridView>
        
        </td></tr>
        </table>
        </div>
    </asp:Panel>
    
    <br />
    
    <br />
    
</div>
    </fieldset>
</div>
</asp:Content>


