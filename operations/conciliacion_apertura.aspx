<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="conciliacion_apertura.aspx.cs" Inherits="operations_Ajustes" Title="AIMAR - BAW"  EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">CONCILIACIONES BANCARIAS</h3>
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
<script language="javascript">
        function onCalendarShown() {

            var cal = $find("calendar1");
            //Setting the default mode to month
            cal._switchMode("months", true);
            
            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }
        function onCalendarHidden() 
        {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
              if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild,"click",call);
                    }
                }
            }

        }
        function call(eventElement)
        {
            var target = eventElement.target;
            switch (target.mode) {
            case "month":
                var cal = $find("calendar1");
                cal._visibleDate = target.date;
                cal.set_selectedDate(target.date);
                cal._switchMonth(target.date);
                cal._blur.post(true);
                cal.raiseDateSelectionChanged();
                break;
            }
        }
    </script>
	<table width="99%">
        <tr>
            <td>
                <b>Banco</b>
                <asp:DropDownList ID="lb_bancos" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged" Enabled="False">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp; <b>Cuenta</b>
                <asp:DropDownList ID="lb_banco_cuenta" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_banco_cuenta_SelectedIndexChanged" 
                    Enabled="False">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp; <b>Moneda</b>
                <asp:DropDownList ID="lb_moneda" runat="server" BackColor="White" 
                    Enabled="False">
                </asp:DropDownList>
                <br />
                <br />
                <b>Fecha</b>
                <b>Inicial</b>
                <asp:TextBox ID="tb_fecha_inicial" runat="server" Height="16px" 
                    AutoPostBack="True" ontextchanged="tb_fecha_inicial_TextChanged"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_inicial_CalendarExtender" runat="server" OnClientHidden="onCalendarHidden"  OnClientShown="onCalendarShown" BehaviorID="calendar1" 
                    Enabled="True" TargetControlID="tb_fecha_inicial" Format="MM/dd/yyyy"> 
                </cc1:CalendarExtender>
                &nbsp;&nbsp;&nbsp;&nbsp; <b>Fecha Final</b>
                <asp:TextBox ID="tb_fecha_final" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />

                
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle">
        
                <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                    <tr>
                        <td align="center" bgcolor="#D9ECFF">
                            <b>&nbsp;APERTURA DE SALDOS</b></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table align="center" cellpadding="0" cellspacing="0" style="width: 85%">
                                <tr>
                                    <td>
                                        <b>(+)Creditos Operados</b></td>
                                                            <td>
                                                                <asp:TextBox ID="tb_creditos_operados" runat="server" AutoPostBack="True" 
                                                                    ontextchanged="tb_creditos_operados_TextChanged">0.00</asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="tb_creditos_operados_FilteredTextBoxExtender" 
                                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="."
                                                                    TargetControlID="tb_creditos_operados">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b>(-)Debitos Operados</b></td>
                                    <td>
                                        <asp:TextBox ID="tb_debitos_operados" runat="server" AutoPostBack="True" 
                                            ontextchanged="tb_debitos_operados_TextChanged">0.00</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="tb_debitos_operados_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                            TargetControlID="tb_debitos_operados" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <b>Saldo en Contabilidad</b></td>
                                    <td>
                                        <asp:TextBox ID="tb_saldo_contabilidad" runat="server" ReadOnly="True" 
                                            Font-Bold="True">0.00</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>(+)Debitos en Circulacion</b></td>
                                    <td>
                                        <asp:TextBox ID="tb_debitos_circulacion" runat="server" AutoPostBack="True" 
                                            ontextchanged="tb_debitos_circulacion_TextChanged">0.00</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="tb_debitos_circulacion_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                            TargetControlID="tb_debitos_circulacion" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>(-)Creditos en Circualcion</b></td>
                                    <td>
                                        <asp:TextBox ID="tb_creditos_circulacion" runat="server" AutoPostBack="True" 
                                            ontextchanged="tb_creditos_circulacion_TextChanged">0.00</asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="tb_creditos_circulacion_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                            TargetControlID="tb_creditos_circulacion" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <b>Saldo Final en Circulacion</b></td>
                                    <td style="border-bottom-color: #000000;">
                                        <asp:TextBox ID="tb_saldo_circulacion" runat="server" ReadOnly="True" 
                                            Font-Bold="True">0.00</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <b>Saldo en Banco</b></td>
                                    <td>
                                        <asp:TextBox ID="tb_saldo_banco" runat="server" ReadOnly="True" 
                                            Font-Bold="True">0.00</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Button ID="btn_grabar" runat="server" Text="Grabar" 
                                            onclick="btn_grabar_Click" Enabled="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table> 
</div>
<div align="center" style ="vertical-align:top;">
</div>
</asp:Content>

