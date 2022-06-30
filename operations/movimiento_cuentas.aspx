<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="movimiento_cuentas.aspx.cs" Inherits="operations_Ajustes" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">MOVIMIENTO DE BANCOS</h3>
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
                <b>Cuenta Contable</b>
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
                <asp:Button ID="bt_visualizar" runat="server" onclick="bt_visualizar_Click" 
                    Text="VISUALIZAR" />
                <asp:ImageButton ID="bt_exportar_excel" runat="server" 
                    ImageUrl="~/img/icons/report.png" onclick="bt_exportar_excel_Click" 
                    ToolTip="Exportar a Excel" Width="16px" />
                <br />
                <br />
                <br />

                
            </td>
        </tr>
        <tr>
            <td align="center">
        
                <p>
                    <b>Saldo Inicial</b>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_saldo_inicial" runat="server" Text="0.00"></asp:Label>
                    &nbsp;<asp:Label ID="lbl_saldo_anterior" runat="server" Text="0.00" Visible="False"></asp:Label>
                </p>
                                    </td>
        </tr>
        <tr>
            <td align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 98%">
                    <tr>
                        <td height="200px" valign="top" align="center">
                            <asp:GridView ID="gv_documentos" runat="server" 
                                onrowcreated="gv_documentos_RowCreated">
                                <HeaderStyle BackColor="#D9ECFF" Font-Bold="True" />
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>
                </table>
                <br />
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
            </td>
        </tr>
        <tr>
            <td align="center">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                        <tr>
                            <td align="right" width="50%">
                                <b>Saldo Actual.:</b></td>
                            <td align="right" width="50%">
                                <asp:Label ID="lbl_saldo_actual" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style=" border-bottom:solid 1px black; ">
                                <b>(+)Saldo Inicial.:</b></td>
                            <td align="right" style=" border-bottom:solid 1px black; ">
                                <asp:Label ID="lbl_saldo_inicial2" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
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
            <td align="center">
                &nbsp;</td>
        </tr>
    </table>   
</div>
</asp:Content>
