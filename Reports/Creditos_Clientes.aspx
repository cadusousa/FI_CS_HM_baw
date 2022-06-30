<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="Creditos_Clientes.aspx.cs" Inherits="Reports_Creditos_Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">REPORTE CREDITO CLIENTES</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">
    function mpecancela_busqueda_rubro() {

    }
    function mpeCuentaOnCancel() {

    }
    function mpeSeleccionOnCancel() {

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
                <td>Pais:</td>
                <td>
                    <asp:DropDownList ID="lb_pais" runat="server" Enabled="false">
                    </asp:DropDownList> 
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
    <asp:Button ID="btn_generar" runat="server" Text="Generar Reporte" 
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

