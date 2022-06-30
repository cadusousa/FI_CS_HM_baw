<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="bancaria.aspx.cs" Inherits="operations_Ajustes" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">NOTA DEBITO BANCARIA</h3>
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
	<table width="100%">
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="center">
        
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="bt_guardar" runat="server" 
                    Text="Grabar" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>    
</div>
</asp:Content>

