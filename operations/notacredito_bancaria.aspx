<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="notacredito_bancaria.aspx.cs" Inherits="operations_Ajustes" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">NOTA CREDITO BANCARIA</h3>
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
	<table width="99%">
        <tr>
            <td>
                <b>Banco</b>
                <asp:DropDownList ID="lb_bancos" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                </asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp; <b>Cuenta</b>
                <asp:DropDownList ID="lb_banco_cuenta" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_banco_cuenta_SelectedIndexChanged">
                </asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp; <b>Moneda</b>
                <asp:DropDownList ID="lb_moneda" runat="server" BackColor="White" 
                    Enabled="False">
                </asp:DropDownList>
                <br />
                <br />
                <b>No. Referencia</b>
                <asp:TextBox ID="tb_credito_no" runat="server" Height="16px" MaxLength="49"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp; <b>Fecha</b>
                <asp:TextBox ID="tb_fecha" runat="server" Height="16px" 
                        Width="128px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fecha_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date" Enabled="True" 
                        TargetControlID="tb_fecha">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fecha" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
&nbsp;&nbsp;&nbsp;&nbsp; <b>Valor</b>
                <asp:TextBox ID="tb_valor" runat="server" Height="16px" AutoPostBack="True" 
                    ontextchanged="tb_valor_TextChanged">0.00</asp:TextBox>
                <br />
                <br />
                <b>Motivo</b>
                <br />
                <asp:TextBox ID="tb_motivo" runat="server" Height="16px" Width="550px" 
                    MaxLength="200"></asp:TextBox>
                <br />
                <br />
                <b>Descripcion</b>
                <br />
                <asp:TextBox ID="tb_descripcion" runat="server" Height="30px" 
                    TextMode="MultiLine" Width="550px" MaxLength="500"></asp:TextBox>
                <br />
                <br />
                <b>Cuenta Contable</b>
                <asp:TextBox ID="tb_cuenta" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="tb_cuenta"
                    PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
                    OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
                    BackgroundCssClass="FondoAplicacion" />
                
&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tb_cuenta_nombre" runat="server" Height="16px" 
                    ReadOnly="True" Width="300px"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />
                <b>Monto</b>
                &nbsp;<asp:TextBox ID="tb_monto" runat="server" Height="16px">0.00</asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="tb_monto_FilteredTextBoxExtender" 
                    runat="server" Enabled="True" TargetControlID="tb_monto"
                    FilterType="Numbers,Custom" 
                    ValidChars="."> 
                    </cc1:FilteredTextBoxExtender>
                &nbsp;&nbsp;&nbsp; <b>
        <asp:Button ID="bt_agregar" runat="server" Text="Agregar" 
                onclick="bt_agregar_Click" />
                </b>&nbsp;<asp:DropDownList ID="lb_ctade" runat="server" Height="22px" 
            Width="127px" Visible="False">
            <asp:ListItem>Debe</asp:ListItem>
            <asp:ListItem Selected="True">Haber</asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;
                <cc1:FilteredTextBoxExtender ID="tb_valor_FilteredTextBoxExtender" runat="server" 
                TargetControlID="tb_valor" 
                FilterType="Numbers,Custom" 
                ValidChars="." > 
                </cc1:FilteredTextBoxExtender>

                
            </td>
        </tr>
        <tr>
            <td align="center">
        
        <asp:GridView ID="gv_cuentas" runat="server" 
            onrowdeleting="gv_cuentas_RowDeleting" Width="100%" 
                    onrowcreated="gv_cuentas_RowCreated">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" ShowHeader="True" />
            </Columns>
        </asp:GridView>
    
            </td>
        </tr>
        <tr>
            <td align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 70%">
                    <tr>
                        <td>
                            <b>Total Debe</b></td>
                        <td>
                            <asp:TextBox ID="tb_total_debe" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <b>Total Haber</b></td>
                        <td>
                            <asp:TextBox ID="tb_total_haber" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="bt_guardar" runat="server" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false"
                    Text="Grabar" onclick="bt_guardar_Click" />
                <asp:Button ID="btn_nueva" runat="server" Text="Nueva" 
                    onclick="btn_nueva_Click" />
            </td>
        </tr>
        <tr>
            <td>
                    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="533px" style=" display:none; ">
                    <div>
                    <table>
                        <caption>
                            &nbsp;&nbsp;Filtrar por:
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="lb_clasificacion" runat="server" Visible="False">
                                        <asp:ListItem Value="0" Selected="True">Todas</asp:ListItem>
                                        <asp:ListItem Value="1">Activo</asp:ListItem>
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
                                Nombre:</td>
                            <td>
                                <asp:TextBox ID="tb_nombre_cta" runat="server" Height="16px" Width="293px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Cuenta:</td>
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
                    </table>
                    </div>
                    <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                        <asp:Label ID="Label1" runat="server" Text="Seleccionar Cuenta" />
                    </div>
                    <div>
                        <asp:GridView ID="gv_cuenta" runat="server" AllowPaging="True" 
                            AutoGenerateSelectButton="True" EmptyDataText="No se encontraron referencias" 
                            GridLines="None" onload="gv_cuenta_Load" 
                            onpageindexchanging="gv_cuenta_PageIndexChanging" 
                            onselectedindexchanged="gv_cuenta_SelectedIndexChanged" Width="480px" 
                            PageSize="15">
                        </asp:GridView>
                    </div>
                    <div>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" />
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>    
</div>
</asp:Content>

