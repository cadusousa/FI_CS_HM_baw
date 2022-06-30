<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="Ajustes.aspx.cs" Inherits="operations_Ajustes" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">Ajustes Contables
</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
    <script language="javascript" type="text/javascript">   
    function mpecancela_busqueda_rubro()
    {

    } 
    function mpeCuentaOnCancel()
    {

    } 
    function mpeSeleccionOnCancel()
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
    </script>
<br />
        <b>&nbsp;Serie:</b>
        <asp:DropDownList ID="lb_serie" runat="server" AutoPostBack="True" 
            onselectedindexchanged="lb_serie_SelectedIndexChanged">
        </asp:DropDownList>
    &nbsp;<b>&nbsp;&nbsp; Correlativo:</b>
        <asp:TextBox ID="tb_correlativo" runat="server" Height="16px" Width="50px" 
        ReadOnly="True"></asp:TextBox>
        <asp:Label ID="lb_factid" runat="server" Text="0" Visible="False"></asp:Label>
        &nbsp;<b>&nbsp;&nbsp;&nbsp; Moneda:</b>
        <asp:DropDownList ID="lb_moneda" runat="server" BackColor="White" 
        Enabled="False">
        </asp:DropDownList>
    &nbsp;<b>&nbsp;&nbsp;&nbsp; Fecha a Ajustar:</b>
                <asp:TextBox ID="tb_fecha" runat="server" Height="16px" 
        Width="70px" AutoPostBack="True" ontextchanged="tb_fecha_TextChanged"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha">
                </cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="tb_fecha_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha">
                </cc1:MaskedEditExtender>
        <asp:Label ID="lbl_tipo_cambio" runat="server" Visible="False"></asp:Label>
    <br />
    <br />
    <b>&nbsp;Tipo de Ajuste: <asp:DropDownList ID="lb_tipo_ajuste" runat="server" 
        AutoPostBack="True" 
        onselectedindexchanged="lb_tipo_ajuste_SelectedIndexChanged">
    </asp:DropDownList>
                                <asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
    <br />
    <br />
    &nbsp;Observaciones: <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" 
            Width="520px"></asp:TextBox>
    <br />
    <br />
    <asp:Panel ID="Pnl_Entidades" runat="server" Visible="False">
        &nbsp;Tipo de Persona
        <asp:DropDownList ID="lb_tipo_persona" runat="server" BackColor="White" 
            Enabled="False">
            <asp:ListItem Value="3">Clientes</asp:ListItem>
            <asp:ListItem Value="4">Proveedores</asp:ListItem>
            <asp:ListItem Value="2">Agentes</asp:ListItem>
            <asp:ListItem Value="5">Navieras</asp:ListItem>
            <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
            <asp:ListItem Value="10">Intercompany</asp:ListItem>
        </asp:DropDownList>
        &nbsp; Codigo
        <asp:TextBox ID="tb_entidad_codigo" runat="server" Height="16px" Width="50px" 
            ReadOnly="True"></asp:TextBox>
        <br />
        <br />
        &nbsp; Nombre
        <asp:TextBox ID="tb_entidad_nombre" runat="server" Height="16px" Width="300px" 
            ReadOnly="True"></asp:TextBox>
    </asp:Panel>
    <asp:Panel ID="Pnl_Caja_Chica" runat="server" Visible="False">
        &nbsp; Codigo
        <asp:TextBox ID="tb_codigo_caja_chica" runat="server" Height="16px" 
            Width="50px" ReadOnly="True"></asp:TextBox>
        <br />
        <br />
        &nbsp; Proveedor Caja Chica <b>
        <asp:TextBox ID="tb_caja_chica_nombre" runat="server" Height="16px" 
            Width="300px" ReadOnly="True"></asp:TextBox>
        </b>
    </asp:Panel>
    <asp:Panel ID="Pnl_maritimo" runat="server" Visible="False">
        <b>&nbsp;
        <br />
        <asp:Label ID="lb_hbl" runat="server" Text="HBL"></asp:Label>
        <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="103px"></asp:TextBox>
        <cc1:ModalPopupExtender ID="tb_hbl_ModalPopupExtender" runat="server" 
            BackgroundCssClass="FondoAplicacion" CancelControlID="btnCuentaCancelar" 
            DropShadow="True" OnCancelScript="mpeCuentaOnCancel()" PopupControlID="Panel1" 
            TargetControlID="tb_hbl" />
        &nbsp;<asp:Label ID="lb_mbl" runat="server" Text="MBL"></asp:Label>
        <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="109px"></asp:TextBox>
        <cc1:ModalPopupExtender ID="tb_mbl_ModalPopupExtender" runat="server" 
            BackgroundCssClass="FondoAplicacion" CancelControlID="btnCuentaCancelar2" 
            DropShadow="True" OnCancelScript="mpeCuentaOnCancel()" 
            PopupControlID="pnlCuentas2" TargetControlID="tb_mbl" />
        &nbsp;<asp:Label ID="lb_routing" runat="server" Text="Routing"></asp:Label>
        <asp:TextBox ID="tb_routing" runat="server" Height="16px" ReadOnly="True" 
            Width="102px"></asp:TextBox>
        &nbsp;<asp:Label ID="lb_contenedor" runat="server" Text="Contenedor"></asp:Label>
        <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" ReadOnly="True" 
            style="text-transform:uppercase;" Width="102px"></asp:TextBox>
        </b>
    </asp:Panel>
    <asp:Panel ID="Pnl_Bancos" runat="server" Visible="False">
        &nbsp; Banco
        <asp:DropDownList ID="lb_bancos" runat="server" AutoPostBack="True" 
            onselectedindexchanged="lb_bancos_SelectedIndexChanged">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp; Cuenta
        <asp:DropDownList ID="lb_banco_cuenta" runat="server" AutoPostBack="True" 
            onselectedindexchanged="lb_banco_cuenta_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <br />
    </asp:Panel>
    <asp:Panel ID="Pnl_Planilla" runat="server" Visible="False">
        <b>&nbsp;&nbsp;No. Poliza</b>
        <asp:TextBox ID="tb_no_poliza" runat="server" Height="16px"></asp:TextBox>
        &nbsp;
        <asp:Button ID="btn_cargar_planilla" runat="server" 
            Text="Cargar Planilla" onclick="btn_cargar_planilla_Click" />
        <asp:Label ID="lbl_planilla" runat="server" Text="0" Visible="False"></asp:Label>
        <br />
        <table align="center" cellpadding="0" cellspacing="0" style="width: 85%">
            <tr>
                <td align="center" bgcolor="#D9ECFF" height="30">
                    PLANILLA</td>
            </tr>
            <tr>
                <td align="center">
                    <b>
                    <asp:GridView ID="gv_planilla" runat="server" Width="100%">
                    </asp:GridView>
                    </b></td>
            </tr>
            <tr>
                <td align="center">
                    <b>
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                        <tr>
                            <td align="right" width="50%">
                                <b>Total Cargo .:</b></td>
                            <td align="right" width="50%">
                                <asp:Label ID="lbl_cargo2" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style=" border-bottom:solid 1px black; ">
                                <b>Total Abono .:</b></td>
                            <td align="right" style=" border-bottom:solid 1px black; ">
                                <asp:Label ID="lbl_abono2" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" bgcolor="#D9ECFF">
                                <b>Total.:</b></td>
                            <td align="right" bgcolor="#D9ECFF">
                                <asp:Label ID="lbl_total2" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="Btn_guarda_planilla" runat="server" onclick="Btn_guarda_planilla_Click" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" Text="Guardar" />
                    <asp:Button ID="btn_imprimir_planilla" runat="server" Enabled="False" 
                        onclick="btn_imprimir_Click" Text="Imprimir" />
                </td>
            </tr>
        </table>
        <br />
        <br />
    </asp:Panel>
    </b>
    <asp:Panel ID="Pnl_Partida" runat="server" Visible="False">
    <table cellpadding="0" cellspacing="1" style="width: 93%">
        <tr>
            <td width="200">
                <b>Cuenta Contable:</b></td>
            <td>
                <asp:TextBox ID="tb_cuenta" runat="server" Height="16px" 
            Width="85px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <b>Nombre:</b></td>
            <td>
                <asp:TextBox ID="tb_cta_nombre" runat="server" Height="16px" 
            Width="220px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <b>Cuenta de:</b></td>
            <td>
        <asp:DropDownList ID="lb_ctade" runat="server" Height="22px" 
            Width="85px">
            <asp:ListItem>Cargo</asp:ListItem>
            <asp:ListItem>Abono</asp:ListItem>
        </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left">
                <b>Monto:</b></td>
            <td>
                <asp:TextBox ID="tb_monto" runat="server" Height="16px" 
            Width="85px"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="tb_monto_FilteredTextBoxExtender" 
            runat="server" Enabled="True" TargetControlID="tb_monto" FilterType="Numbers,Custom" ValidChars=".">
        </cc1:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" height="50">
        <asp:Button ID="bt_agregar" runat="server" Text="Agregar" 
                onclick="bt_agregar_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" bgcolor="#D9ECFF" height="30">
                <b>PARTIDA DE AJUSTE</b></td>
        </tr>
        <tr>
            <td align="center" colspan="2">
        <asp:GridView ID="gv_conf_cuentas" runat="server" 
            onrowdeleting="gv_conf_cuentas_RowDeleting" Width="100%" Font-Bold="False">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" ShowHeader="True" />
            </Columns>
        </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                    <tr>
                        <td align="right" width="50%">
                            <b>Total Cargo .:</b></td>
                        <td align="right" width="50%">
                            <asp:Label ID="lbl_cargo" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style=" border-bottom:solid 1px black; ">
                            <b>Total Abono .:</b></td>
                        <td align="right" style=" border-bottom:solid 1px black; ">
                            <asp:Label ID="lbl_abono" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" bgcolor="#D9ECFF">
                            <b>Total.:</b></td>
                        <td align="right" bgcolor="#D9ECFF">
                            <asp:Label ID="lbl_total" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                    <asp:Button ID="Btn_guardar" runat="server" onclick="Btn_guardar_Click" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"
                        Text="Guardar" />
                    <asp:Button ID="btn_imprimir" runat="server" Enabled="False" 
                        onclick="btn_imprimir_Click" Text="Imprimir" />
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="Pnl_Externo" runat="server" Visible="False" 
        HorizontalAlign="Center">
    <br/>
        <b>Cargar Ajuste a partir de Excel</b>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:FileUpload ID="FileUpload1" runat="server" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_cargar_ajuste" runat="server" Text="Cargar Ajuste" 
            onclick="btn_cargar_ajuste_Click" />
        <br />
        <br />
        <asp:Label ID="lbl_nota" runat="server" 
            Text="Nota.: el Fomarto de excel debe contener 3 Columnas la primera con el numero de cuenta contable, la segunda con valores numericos del Cargo y la tercera valores numericos del Abono. No deben existir encabezado de columnas ni formulas, dejar la primera fila del Excel en blanco y la hoja del excel con este nombre Hoja1" 
            Font-Size="Smaller" ForeColor="Red"></asp:Label>
        <table align="center" cellpadding="0" cellspacing="0" style="width: 85%">
            <tr>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" bgcolor="#D9ECFF">
                    <b>PARTIDA DE AJUSTE</b></td>
            </tr>
            <tr>
                <td>
                    <b>
                    <asp:GridView ID="gv_partida" runat="server" Width="100%">
                    </asp:GridView>
                    </b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                        <tr>
                            <td align="right" width="50%">
                                <b>Total Cargo .:</b></td>
                            <td align="right" width="50%">
                                <asp:Label ID="lbl_cargo3" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style=" border-bottom:solid 1px black; ">
                                <b>Total Abono .:</b></td>
                            <td align="right" style=" border-bottom:solid 1px black; ">
                                <asp:Label ID="lbl_abono3" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" bgcolor="#D9ECFF">
                                <b>Total.:</b></td>
                            <td align="right" bgcolor="#D9ECFF">
                                <asp:Label ID="lbl_total3" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btn_guardar2" runat="server" onclick="btn_guardar2_Click" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" Text="Guardar" />
                    <asp:Button ID="btn_imprimir2" runat="server" Enabled="False" Text="Imprimir" 
                        onclick="btn_imprimir_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
<asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" style="display:none;"
        Width="700px">
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
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Agente" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="533px" style="display:none;">
        <div>
        <table>
            <caption>
                &nbsp;&nbsp;Filtrar por:
                <tr>
                    <td>
                        Tipo:</td>
                    <td>
                        <asp:DropDownList ID="lb_clasificacion" runat="server">
                            <asp:ListItem Value="0">Todas</asp:ListItem>
                            <asp:ListItem Selected="True" Value="1">Activo</asp:ListItem>
                            <asp:ListItem Value="2">Pasivo</asp:ListItem>
                            <asp:ListItem Value="3">Venta(Ingresos)</asp:ListItem>
                            <asp:ListItem Value="4">Gastos(Egresos)</asp:ListItem>
                            <asp:ListItem Value="5">Capital</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </caption>
            <tr>
                <td>
                    Nombre</td>
                <td>
                    <asp:TextBox ID="tb_nombre_cta" runat="server" Height="16px" Width="293px" />
                </td>
            </tr>
            <tr>
                <td>
                    Cuenta</td>
                <td>
                    <asp:TextBox ID="tb_cuenta_numero" runat="server" Height="16px" Width="293px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="bt_buscar_cta" runat="server" onclick="bt_buscar_cta_Click" 
                        Text="Buscar" />
                </td>
            </tr>
        </table></div>
        <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label1" runat="server" Text="Seleccionar Cuenta" />
        </div>
        <div>
            <asp:GridView ID="gv_cuenta" runat="server" AllowPaging="True" 
                AutoGenerateSelectButton="True" EmptyDataText="No se encontraron referencias" 
                GridLines="None" 
                onpageindexchanging="gv_cuenta_PageIndexChanging" 
                onselectedindexchanged="gv_cuenta_SelectedIndexChanged" Width="480px" 
                PageSize="15" onload="gv_cuenta_Load">
            </asp:GridView>
        </div>
        <div>
            &nbsp;&nbsp;
            <asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" />
        </div>
    </asp:Panel>
    
<asp:Panel ID="pnlProveedor2" runat="server" CssClass="CajaDialogo" style="display:none;"
        Width="722px">
            <div><table>
                <tr><td align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:<asp:TextBox ID="tb_nitb2" runat="server" Height="16px" Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:<asp:TextBox ID="tb_nombreb2" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td>Codigo:<asp:TextBox ID="tb_codigo2" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="bt_buscar2" runat="server" Text="Buscar" onclick="bt_buscar2_Click" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label42" runat="server" Text="Seleccionar Proveedor" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor2" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" 
                    onpageindexchanging="gv_proveedor2_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor2_SelectedIndexChanged" PageSize="5" 
                    Width="692px">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar2" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlCuentas2" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label12" runat="server" Text="Seleccionar Cuenta" />
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_criterio2" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="lb_tipo2" runat="server">
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
                <asp:GridView ID="dgw12" runat="server" AllowPaging="True" 
                    onpageindexchanging="dgw12_PageIndexChanging" onrowcommand="dgw12_RowCommand" 
                    PageSize="10">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar2" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bt_search2" runat="server" Text="Buscar" 
                            onclick="bt_search2_Click" />
            </td>
            <td><asp:Button ID="btnCuentaCancelar2" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label2" runat="server" Text="Seleccionar Cuenta" />
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
                    PageSize="10">
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
            <td><asp:Button ID="Button1" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_entidad_codigo"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="tb_cuenta"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />  
    <cc1:ModalPopupExtender ID="mpeSeleccion2" runat="server" TargetControlID="tb_codigo_caja_chica"
            PopupControlID="pnlProveedor2" CancelControlID="btnProveedorCancelar2"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
                
</asp:Content>

