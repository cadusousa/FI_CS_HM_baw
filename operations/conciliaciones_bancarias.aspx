<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="conciliaciones_bancarias.aspx.cs" Inherits="operations_Ajustes" Title="AIMAR - BAW"  EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">CONCILIACIONES BANCARIAS</h3>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
            <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
        </Scripts>
    </asp:ScriptManager>
    <script type="text/javascript">
        function cancelClick() 
        {
            document.aspnetForm.ctl00_Contenido_tb_motivo.value = "";
        }
    </script>
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
    <script  type="text/javascript">
        function BloquearPantalla() {
            $.blockUI({ message: '<h2>Cargando transacciones, por favor espere ......</h2>' });
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                $.unblockUI();
            });
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
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
                <asp:Button ID="bt_visualizar" runat="server" OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" 
                    Text="VISUALIZAR" onclick="bt_visualizar_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />

                
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle">
        
                <asp:Label ID="lbl_circulacion" runat="server" BackColor="Red" Height="10px" 
                    Width="10px" ToolTip="Documentos en Circulacion"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lbl_anulados" runat="server" BackColor="Blue" Height="10px" 
                    Width="10px" ToolTip="Documentos Anulados"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lbl_contabilidad" runat="server" BackColor="Black" Height="10px" 
                    ToolTip="Documentos Conciliados" Width="10px"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lbl_anteriores" runat="server" BackColor="#E6E6E6" Height="10px" 
                    ToolTip="Documentos en Circulacion, Peridos Anteriores" Width="10px"></asp:Label>
                                    </td>
        </tr>
        <tr>
            <td align="center">
                <p>
                    <b>Saldo en Banco al</b>&nbsp;&nbsp;
                    <asp:Label ID="lbl_fecha_banco" runat="server" Font-Bold="True"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_saldo_inicial" runat="server" Text="0.00"></asp:Label>
                    <asp:Label ID="lbl_saldo_anterior" runat="server" Text="0.00" Visible="False"></asp:Label>
                    <asp:Label ID="lbl_saldo_temporal" runat="server" Text="0.00" Visible="False"></asp:Label>
                </p>
            </td>
        </tr>
        <tr>
            <td align="center">
                        <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 98%">
                            <tr>
                                <td align="center" bgcolor="#D9ECFF" colspan="8">
                                    <b>DOCUMENTOS EN CONTABILIDAD</b></td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="8" height="200px" align="center">
                                    <asp:GridView ID="gv_documentos_contabilidad" runat="server" 
                                        onrowcreated="gv_documentos_contabilidad_RowCreated" BorderColor="Black" 
                                        BorderStyle="Solid" BorderWidth="1px" Width="600px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_contabilidad" runat="server" AutoPostBack="True" 
                                                        oncheckedchanged="chk_contabilidad_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="Black" ForeColor="White" />
                                        <RowStyle HorizontalAlign="Right" />
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" width="50%">
                                    <b>Total Documentos en Contabilidad</b></td>
                                <td colspan="4" width="50%">
                                    <b>Total Documentos en Circulacion</b></td>
                            </tr>
                            <tr>
                                <td width="12.5%">
                                    <b>Creditos</b></td>
                                <td bgcolor="#D9ECFF" width="12.5%">
                                    <asp:Label ID="lbl_total_creditos_contabilidad" runat="server" Text="0.00"></asp:Label>
                                </td>
                                <td width="12.5%">
                                    <b>Debitos</b></td>
                                <td bgcolor="#D9ECFF" width="12.5%">
                                    <asp:Label ID="lbl_total_debitos_contabilidad" runat="server" Text="0.00"></asp:Label>
                                </td>
                                <td width="12.5%">
                                    <b>Creditos</b></td>
                                <td bgcolor="#D9ECFF" width="12.5%">
                                    <asp:Label ID="lbl_total_creditos_circulacion" runat="server" Text="0.00"></asp:Label>
                                </td>
                                <td width="12.5%">
                                    <b>Debitos</b></td>
                                <td bgcolor="#D9ECFF" width="12.5%">
                                    <asp:Label ID="lbl_total_debitos_circulacion" runat="server">0.00</asp:Label>
                                </td>
                            </tr>
                        </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                    <tr>
                        <td align="right" width="50%">
                            <b>Saldo Final en Circulacion.:</td>
                        <td width="50%" align="right">
                            <asp:Label ID="lbl_saldo_final_circulacion" runat="server" Font-Bold="True" 
                                ForeColor="Red" Text="0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style=" border-bottom:solid 1px black; ">
                            <b>(+/-)Saldo en Contabilidad.:</b></td>
                        <td align="right" style=" border-bottom:solid 1px black; ">
                            <asp:Label ID="lbl_saldo_final_contabilidad" runat="server" 
                                Font-Bold="True" Text="0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Saldo de Banco.:</b></td>
                        <td align="right">
                            <asp:Label ID="lbl_saldo_banco" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="bt_preconciliar" runat="server" Text="Preconciliar" 
                    onclick="bt_preconciliar_Click" />
                <asp:Button ID="bt_guardar" runat="server" 
                    Text="Grabar" onclick="bt_guardar_Click"/>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table> 
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div align="center" style ="vertical-align:top;">
    <asp:Button ID="btn_reporte" runat="server" Text="Generar Reporte" 
        onclick="btn_reporte_Click" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btn_botar_conciliacion" runat="server" 
    Text="Botar Conciliacion" onclick="btn_botar_conciliacion_Click" />

    <cc1:ModalPopupExtender ID="btn_botar_conciliacion_ModalPopupExtender" 
        runat="server" 
        TargetControlID="btn_botar_conciliacion"
        PopupControlID="PNL" 
        OkControlID="ButtonOk" 
        CancelControlID="ButtonCancel"
        BackgroundCssClass="FondoAplicacion">
    </cc1:ModalPopupExtender>

    <cc1:ConfirmButtonExtender ID="btn_botar_conciliacion_ConfirmButtonExtender" runat="server" 
        TargetControlID="btn_botar_conciliacion"
        OnClientCancel="cancelClick"
        DisplayModalPopupID="btn_botar_conciliacion_ModalPopupExtender">
    </cc1:ConfirmButtonExtender>

    <asp:Panel ID="PNL" runat="server" style="display:none; width:200px; background-color:White; border-width:2px; border-color:Black; border-style:solid; padding:20px;">
        ¿Esta Seguro de querer Botar la Conciliacion?
        <br /><br />
        <div style="text-align:right;">
            <asp:Button ID="ButtonOk" runat="server" Text="Si"/>
            <input type="button" id="ButtonCancel" onclick="cancelClick();" value="No" />
        </div>
        <div>

            <asp:Panel ID="pnl_motivo" runat="server">
                <table align="center" cellpadding="0" cellspacing="0" style="width:150px;">
                    <tr>
                        <td>
                            Ingrese Motivo:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tb_motivo" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </div>
    </asp:Panel>
</div>
</asp:Content>

