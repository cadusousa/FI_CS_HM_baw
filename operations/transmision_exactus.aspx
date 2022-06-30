<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="transmision_exactus.aspx.cs" Inherits="operations_transmision_exactus" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">



<link rel="stylesheet" type="text/css" href="../css/theme.css" />
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<style type="text/css">
#myBar {
    width: 10%;
    height: 15px;
    background-color: #4CAF50;
    text-align: center; /* To center it horizontally (if you want) */
    line-height: 15px; /* To center it vertically */
    color: white;
    font-weight: bold;
    display: none;
}

th 
{
    background : #333; color: White; font-size:8px; text-align:center;
    }
     
td 
{
    background : #777; color: white; font-size:8px; text-align:center;
    }
    
.th1 
{
    background : #333; color: White; font-size:12px; text-align:center;
    }
     
.td1 
{
    background : #666; color: white; font-size:12px; text-align:center;
    }
</style>



    <script language="javascript">

        function move() {
            document.getElementById('filtros').style.display = "none";
            if (document.getElementById('rep_pro'))
                document.getElementById('rep_pro').style.display = "none";
            document.getElementById('forma').style.display = "none";
            document.getElementById('myBar').style.display = "block";
            var elem = document.getElementById("myBar");
            var width = 10;
            var id = setInterval(frame, 65);
            function frame() {
                if (width >= 100) {
                    clearInterval(id);
                } else {
                    width++;
                    elem.style.width = width + '%';
                    elem.innerHTML = width * 1 + '%';
                }
            }
        }
        
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

        var t = window.location.href.split('#')[0];
        t = window.location.href.split('?')[0];
    </script>

    <table width=100%>
    <tr>
    <td width=50%> <%=(reporte == 0 ? "<h2>TRANSMISION EXACTUS</h2>" : "<input type=button onclick=\"window.location = t\" value=\"TRANSMISION EXACTUS\" style='width:100%;cursor:pointer;color:gray;' />")%></td>
    <td width=50%> <%=(reporte == 1 ? "<h2>PROVISIONES SIN FACTURA</h2>" : "<input type=button onclick=\"move();window.location = t + '?reporte=1';\" value=\"PROVISIONES SIN FACTURA\" style='width:100%;cursor:pointer;color:gray;' />")%></td>
    </tr>
    </table>
 

   
	<div id="myProgress">
      <div id="myBar">10%</div>
    </div>
        



    <asp:Panel ID="Panel1" runat="server">
  
    <asp:ScriptManager ID="ScriptManager1" runat="server" />


            <table width="100%" align="center" style="border:0px;" id=filtros>
            <tr>
                <td>Fecha Inicial:</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" name="tb_fechainicial" runat="server" Height="10px" size=10 ReadOnly="true"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fechainicial_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                        TargetControlID="tb_fechainicial">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server"   
                        Enabled="True" TargetControlID="tb_fechainicial" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                </td>
                <td>&nbsp;&nbsp;</td>
                <td>Fecha Final:</td>
                <td>
                    <asp:TextBox ID="tb_fechafinal" name="tb_fechafinal" runat="server" Height="10px" size=10 ReadOnly="true"></asp:TextBox>
                     <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" 
                        Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                        TargetControlID="tb_fechafinal">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server"   
                        Enabled="True" TargetControlID="tb_fechafinal" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                </td>
               <td>&nbsp;&nbsp;</td>
               <td colspan="1" align="center">
                    <asp:Button ID="btn_generar" runat="server"  Text="Generar" OnClientClick="move();"                        />
                </td>
               <td>&nbsp;&nbsp;</td>
 
                <td>
                <% if (reporte == 0) { %>

                    <input type=button onclick="move();window.location = t" value="Refresh" />
                
                 <% } %>
                </td>

               <td>&nbsp;&nbsp;</td>



               <td><font color=black style="font-size:8px;font-weight:normal"><%=DateTime.Now%></font></td>
                <td><font color=black style="font-size:8px;font-weight:normal">Total Registros  : <%=t%></font></td>
 




           </tr>
            </table>


        <%=html%>



    <input type=text id=flag name=flag value="" style="display:none"/>
    <iframe id=iframe_php name=iframe_php onload="if (document.getElementById('flag').value != '') { document.getElementById('flag').value = ''; window.parent.location.reload(); }" style="display:none"></iframe>


    </asp:Panel>

   

</asp:Content>

