<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="confOperaciones.aspx.cs" Inherits="manager_confOperaciones" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
    <h3 id="adduser">Configuración de operaciones</h3>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {
        var stb_cuenta = document.getElementById("ctl00_Contenido_tb_cuenta");
        var stb_cta_nombre= document.getElementById("ctl00_Contenido_tb_cta_nombre");
        stb_cuenta.value = "";
        stb_cta_nombre.value="";
    }  
    </script>
	<table width="100%">
    <tr><td>
        Operación:
        <asp:DropDownList ID="lb_transaccion" runat="server" Height="22px" Width="315px">
        </asp:DropDownList><br />
        &nbsp; Moneda:
        <asp:DropDownList ID="lb_moneda" runat="server" Height="22px" Width="161px">
        </asp:DropDownList>
        &nbsp: Pais:
        <asp:DropDownList ID="lb_pais" runat="server" Height="22px" Width="161px">
        </asp:DropDownList>
        <br />
        Contabilidad:
        <asp:DropDownList ID="lb_tipo_contabilidad" runat="server" Height="22px" Width="161px">
        </asp:DropDownList>
        <br />
        <div align="right">
            <asp:Button ID="bt_buscar_conv" runat="server" Text="Buscar combinacion" 
                onclick="bt_buscar_conv_Click" />
        </div>
        <br /><br />
    </td></tr>
    <tr><td style="background-color:#99FFCC">Cuenta:<asp:TextBox ID="tb_cuenta" runat="server" Height="16px" 
            Width="85px" ReadOnly="True"></asp:TextBox>
        Descripcion:<asp:TextBox ID="tb_cta_nombre" runat="server" Height="16px" 
            Width="220px" ReadOnly="True"></asp:TextBox>
        Cuenta de:
        <asp:DropDownList ID="lb_ctade" runat="server" Height="22px" 
            Width="127px">
            <asp:ListItem>Cargo</asp:ListItem>
            <asp:ListItem>Abono</asp:ListItem>
        </asp:DropDownList>
        <div align="right">
        <asp:Button ID="bt_agregar" runat="server" Text="Agregar" 
                onclick="bt_agregar_Click" />
        </div>
        <br />
    </td></tr>
    <tr><td align="center">
    
        <asp:GridView ID="gv_conf_cuentas" runat="server" 
            onrowdeleting="gv_conf_cuentas_RowDeleting" Width="100%">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" ShowHeader="True" />
            </Columns>
        </asp:GridView>
    
    </td></tr>
    <tr><td align="center">
        <asp:Button ID="bt_guardar" runat="server" onclick="bt_guardar_Click" 
            Text="Grabar" />
    </td></tr>
    </table>
    
    
    
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="533px">
        <div>
        <table>
            <caption>
                &nbsp;&nbsp;Filtrar por:
                <tr>
                    <td>
                        Tipo:<asp:DropDownList ID="lb_clasificacion" runat="server">
                            <asp:ListItem Value="0">Todas</asp:ListItem>
                            <asp:ListItem Selected="True" Value="1">Activo</asp:ListItem>
                            <asp:ListItem Value="2">Pasivo</asp:ListItem>
                            <asp:ListItem Value="3">Venta(Ingresos)</asp:ListItem>
                            <asp:ListItem Value="4">Gastos(Egresos)</asp:ListItem>
                            <asp:ListItem Value="5">Capital</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        Nombre:<asp:TextBox ID="tb_nombre_cta" runat="server" Height="16px" 
                            Width="293px" />
                    </td>
                </tr>
            </caption>
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
    
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="tb_cuenta"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
</div>
</asp:Content>

