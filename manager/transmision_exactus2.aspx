<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="transmision_exactus2.aspx.cs" Inherits="manager_transmision_exactus2" %>

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


<script type="text/javascript">
    function move() {
        document.getElementById('filtros').style.display = "none";
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

</script>


    <h3>  TRANSMISION EXACTUS &nbsp;&nbsp;&nbsp;  <font color=gray style="font-size:8px;font-weight:normal"><%=DateTime.Now%></font> &nbsp;&nbsp;&nbsp; <font color=gray style="font-size:8px;font-weight:normal">Total Registros  : <%=t%> &nbsp; Faltantes : <%=r%></font></h3>
    
 
   

	<div id="myProgress">
      <div id="myBar">10%</div>
    </div>
        
    <br />


    <asp:Panel ID="Panel1" runat="server">
  
<asp:ScriptManager ID="ScriptManager1" runat="server" />

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

            <table width="100%" align="center" style="border:0px;" id=filtros>
            <tr>
                <td>Fecha Inicial:</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" name="tb_fechainicial" runat="server" Height="16px"></asp:TextBox>
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
                    <asp:TextBox ID="tb_fechafinal" name="tb_fechafinal" runat="server" Height="16px" ReadOnly="false"></asp:TextBox>
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
                    <input type=button onclick="move();window.location = window.location.href.split('#')[0];" value="Refresh" />
                </td>

               <td>&nbsp;&nbsp;</td>
 
               <td>
                    <asp:Button ID="Button2" runat="server"  Text="Reporte" onclick="btn_reporte_Click" />
                </td>

           </tr>
            </table>



<!--
   <form action="transmision_exactus.aspx" method="post">
   <table>
        <tr>
            <td>Fecha Inicial : </td>
            <td><input type=text name=fecini id=fecini value="<%=ending.ToString("dd/MM/yyyy") %>" /></td>
            <td>Fecha Final : </td>
            <td><input type=text name=fecfin id=fecfin value="<%=starting.ToString("dd/MM/yyyy")%>" /></td>
            <td><input type=submit /></td>
            <td></td>
            <td><a href="transmision_exactus2.aspx" onclick="move();" style="font-size:10px">Refresh</a> </td>
        </tr>  
   </table>  
</form>
  -->

        <%=html%>




    <input type=text id=flag name=flag value="" style="display:none"/>
    <iframe id=iframe_php name=iframe_php onload="if (document.getElementById('flag').value != '') { document.getElementById('flag').value = ''; window.parent.location.reload(); }" style="display:none"></iframe>


    </asp:Panel>


</asp:Content>

