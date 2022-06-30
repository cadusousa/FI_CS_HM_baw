<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="modificacion_creditos.aspx.cs" Inherits="operations_creditospend" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<fieldset id="searchresult">
<table width="600" align="center">
<thead>
    <tr><th>Listado de Creditos Autorizados</th></tr>
</thead>
<tbody>
<tr>
    <td>
    
        <br />
        <table align="center" cellpadding="0" cellspacing="0" style="width: 70%">
            <tr>
                <td>
                    Nombre:</td>
                <td>
                    <asp:TextBox ID="tb_nombre" runat="server" Width="344px" 
                        Height="15px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td width="100px">
                    Codigo:</td>
                <td>
                    <asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="82px"></asp:TextBox>
						</td>
            </tr>
            <tr>
                <td width="100px">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Buscar" onclick="Button1_Click" />
                </td>
            </tr>
        </table>
        <br />
    
        <asp:GridView ID="gv_pendaut" runat="server" AllowPaging="True" 
            onrowcommand="gv_pendaut_RowCommand" 
            onpageindexchanging="gv_pendaut_PageIndexChanging">
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Modificar" 
                    HeaderText="Modificar" Text="Modificar" />
            </Columns>
        </asp:GridView>
    
    </td>
</tr>
</tbody>
</table>
</fieldset>
</div>
</asp:Content>

