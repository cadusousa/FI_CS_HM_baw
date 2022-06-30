<%@ Page Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="solicitaReport.aspx.cs" Inherits="Reports_solicitaReport" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">REPORTES</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
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
        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }

        }
        function call(eventElement) {
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
<table width="650">
    <tr>
        <td>
            <table width="300" align="center">
            <tr>
                <td>Fecha Inicial:</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" runat="server" Height="16px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fechainicial_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                        TargetControlID="tb_fechainicial">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server"   
                        Enabled="True" TargetControlID="tb_fechainicial" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>Fecha Final:</td>
                <td>
                    <asp:TextBox ID="tb_fechafinal" runat="server" Height="16px" ReadOnly="false"></asp:TextBox>
                     <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" 
                        Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                        TargetControlID="tb_fechafinal">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server"   
                        Enabled="True" TargetControlID="tb_fechafinal" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>Moneda:</td>
                <td>
                    <asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                    <td><asp:Label ID="l_moneda_consolidado" runat="server" Text="Consolidar moneda?"></asp:Label></td>
                    <td>
                        <asp:CheckBox ID="chk_consolidado" runat="server" />
                    </td>
                </tr>
            <tr>
                <td>
                    <asp:Label ID="lb_mbl" runat="server" Visible="False">MBL</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Visible="False"></asp:TextBox>
                    <cc1:ModalPopupExtender ID="tb_mbl_ModalPopupExtender" runat="server" TargetControlID="tb_mbl"
                        PopupControlID="pnlMBL" CancelControlID="btnCuentaCancelar2"
                        OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
                        BackgroundCssClass="FondoAplicacion" />
                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_reporte" runat="server" Text="Reporte:" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="lb_reporte" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="lb_reporte_SelectedIndexChanged" Visible="False">
                        <asp:ListItem Value="1">Libro diario</asp:ListItem>
                        <asp:ListItem Value="2">Libro Mayor</asp:ListItem>
                        <asp:ListItem Value="3">Profit</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
            </tr>


            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Formato:" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="lb_formato" runat="server" AutoPostBack="True" Visible="False">
                        <asp:ListItem Value="1">Formato 1 Original</asp:ListItem>
                        <asp:ListItem Value="2">Formato 2 TLA HTML </asp:ListItem>
                        <asp:ListItem Value="3">Formato 3 TLA EXCEL </asp:ListItem>
                        <asp:ListItem Value="4">Formato 4 TLA Crystal R. </asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
            </tr>

            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btn_generar" runat="server" Text="Generar" 
                        onclick="btn_generar_Click" />
                </td>
            </tr>
            </table>
            <!--   
            -->
        </td>
    </tr>
</table>
</fieldset>
</div>
<%--********************************************************--%>
<div>
    <asp:Panel ID="pnlMBL" runat="server" CssClass="CajaDialogo" Width="600px" Style=" display:none">
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
    </div>
    <%--********************************************************--%>

    </asp:Content>

