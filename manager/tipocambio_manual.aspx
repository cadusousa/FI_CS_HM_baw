<%@ Page Language="C#" MasterPageFile="~/manager/manager.master"  AutoEventWireup="true" CodeFile="tipocambio_manual.aspx.cs" Inherits="manager_tipocambio"  Title="AIMAR - BAW"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<fieldset id="searchresult">
<asp:ScriptManager ID="ScriptManager1" runat="server" >
 <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>

<script language="javascript">
   var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
   document.writeln('<link rel="stylesheet" type="text/css" href="../css/' + StyleFile + '">');
</script>



    <div id="content">
 
        <table width="350" align="left">
            <thead>
                <tr><th colspan="2">Configuración de tipo de cambio</th></tr>
            </thead>
            <tbody>

                <tr>
                    <td align="right">Pais: </td>
                    <td>
                    <asp:TextBox ID="tb_id" runat="server" Enabled="False" Visible="false"></asp:TextBox>
                     <asp:TextBox ID="TextBox1" runat="server" Enabled="False" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="tb_pais" runat="server" Enabled="False"></asp:TextBox>
                        <asp:Label ID="lb_pai_id" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>

                <% //if (int.Parse(TextBox1.Text.Trim()) == 1) { %>
<tr>
    <td align="right">Fecha: </td>
    <td>


        <asp:TextBox ID="tb_fecha" runat="server" ></asp:TextBox>
        <cc1:MaskedEditExtender ID="tb_fecha_MaskedEditExtender" runat="server" 
             Mask="99/99/9999" MaskType="Date"  Enabled="True" 
            TargetControlID="tb_fecha">
        </cc1:MaskedEditExtender>
        <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fecha" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>


    </td>
</tr>
                <% //} else { %>

                <% //} %>
                <tr>
                    <td align="right">Tipo de cambio: </td>
                    <td>  
                        <asp:TextBox ID="tb_tipo_cambio" runat="server"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td align="center" colspan="2"><b><asp:Label ID="lb_msg" runat="server" Visible="true"></asp:Label></b></td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="cmdLogin" runat="server" OnClick="cmdLogin_Click" Text="Guardar" />
                        &nbsp;&nbsp;
                        <input id="Cancel" type="button" value="Cancelar" onclick="javascript:window.close();" />
                    </td>
                </tr>
            </tbody>
        </table>
    
    </div>
</fieldset>
        </asp:Content>





