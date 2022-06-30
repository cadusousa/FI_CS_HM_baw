<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="Cuentas_por_Pagar.aspx.cs" Inherits="Reports_Cuentas_por_Pagar" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">REPORTE CUENTAS POR PAGAR</h3>
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
                <td>Fecha Corte:</td>
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
</asp:Content>


