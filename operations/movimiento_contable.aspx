<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="movimiento_contable.aspx.cs" Inherits="operations_movimiento_contable" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">MOVIMIENTO DE CUENTAS</h3>
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
	<table width="99%">
        <tr>
            <td>
                <b>
                <br />
                Cuenta Contable</b>
                <asp:TextBox ID="tb_cuenta" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" 
                    BackgroundCssClass="FondoAplicacion" CancelControlID="btnCuentaCancelar" 
                    DropShadow="True" OnCancelScript="mpeCuentaOnCancel()" 
                    PopupControlID="pnlCuentas" TargetControlID="tb_cuenta" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tb_cuenta_nombre" runat="server" Height="16px" ReadOnly="True" 
                    Width="300px"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />
                <b>Fecha</b>
                <b>Inicial</b>
                <asp:TextBox ID="tb_fecha_inicial" runat="server" Height="16px"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_inicial_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_inicial">
                </cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="tb_fecha_inicial_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_inicial">
                </cc1:MaskedEditExtender>
                &nbsp;&nbsp;&nbsp;&nbsp; <b>Fecha Final</b>
                <asp:TextBox ID="tb_fecha_final" runat="server" Height="16px"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_final_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_final">
                </cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="tb_fecha_final_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_final">
                </cc1:MaskedEditExtender>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />
                <asp:Label ID="l_contabilidad" runat="server" Font-Bold="True" 
                    Text="Tipo Contabilidad"></asp:Label>
                &nbsp;<asp:DropDownList ID="lb_contabilidad" runat="server">
                </asp:DropDownList>
                <br />
                <br />
                <strong>Moneda</strong>
                    <asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="bt_visualizar" runat="server" 
                    Text="VISUALIZAR" onclick="bt_visualizar_Click" />
                <br />
                <br />
                <br />

                
            </td>
        </tr>
    </table>   
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" style=" display:none" 
                    Width="533px">
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
                            onselectedindexchanged="gv_cuenta_SelectedIndexChanged" PageSize="15" 
                            Width="480px">
                        </asp:GridView>
                    </div>
                    <div>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" />
                    </div>
                </asp:Panel>
</div>
    
</asp:Content>

