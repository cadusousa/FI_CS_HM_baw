<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="listado_oc.aspx.cs" Inherits="operations_listado_oc" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<script type="text/javascript">
    function PostBackHijo() {
        var btn = document.getElementById("bt_busqueda");
        if (btn) btn.click();
    }
</script>
    <div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">LISTADO DE ORDENES DE COMPRA</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<div align="center">
<table width="700" align="center">
    <tr><td align="center">
        <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
            <tr>
                <td>
                    Serie</td>
                <td>
                    <asp:TextBox ID="tb_serie_oc" runat="server"></asp:TextBox>
<%--        <asp:DropDownList ID="lb_seriefactura" runat="server">
        </asp:DropDownList>--%>
                </td>
                <td>
                    Correlativo</td>
                <td>
        <asp:TextBox ID="tb_corr_oc" runat="server" Height="16px" Width="110px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Departamento</td>
                <td colspan="3">
                    <asp:DropDownList ID="lb_departamento_interno" runat="server" Height="23px" Width="255px">                    
                 </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Estado</td>
                <td colspan="3">
                    <asp:DropDownList 
                ID="lb_estado" runat="server">
            <asp:ListItem Value="1">Activa</asp:ListItem>
            <asp:ListItem Value="5">Autorizada</asp:ListItem>
            <asp:ListItem Value="3">Rechazada / Anulada</asp:ListItem>
            <asp:ListItem Value="8">Draft</asp:ListItem>
            <asp:ListItem Value="7">Provisionada</asp:ListItem>
            </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Período inicial</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" runat="server" Height="16px" 
            Width="115px"></asp:TextBox>
        <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fechainicial" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
                </td>
                <td>
                    Final</td>
                <td>
        <asp:TextBox ID="tb_fechafinal" runat="server" Height="16px" 
            Width="115px"></asp:TextBox>
        <cc1:CalendarExtender ID="tb_fechafinal_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="tb_fechafinal" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    Contenedor</td>
                <td colspan="3">
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" 
            Width="430px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
        <asp:Button ID="bt_busqueda" runat="server" onclick="bt_busqueda_Click" 
            Text="Listar" ClientIDMode="Static" />
        <asp:Label ID="lb_contabilidad" runat="server" Text="1" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </td></tr>
    <tr><td align="center">
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="700px">
            <asp:GridView ID="gv_ordenescompra" runat="server" 
            onrowcommand="gv_ordenescompra_RowCommand" Width="700px" 
            onrowcreated="gv_ordenescompra_RowCreated" Font-Size="X-Small">
                <Columns>
                    <asp:ButtonField ButtonType="Button" CommandName="Seleccionar" 
                    Text="Seleccionar" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </td></tr>
    </table>
</div>
</fieldset>
</div>
</asp:Content>

