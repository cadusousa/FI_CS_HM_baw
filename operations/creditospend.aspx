<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="creditospend.aspx.cs" Inherits="operations_creditospend" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<fieldset id="searchresult">
<table width="600" align="center">
<thead>
    <tr><th>Listado de solicitudes pendientes de autorizar</th></tr>
</thead>
<tbody>
<tr>
    <td>
    
        <asp:GridView ID="gv_pendaut" runat="server" AllowPaging="True" 
            onrowcommand="gv_pendaut_RowCommand" 
            onpageindexchanging="gv_pendaut_PageIndexChanging">
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Autorizar" 
                    HeaderText="Autorizar" Text="Autorizar" />
            </Columns>
        </asp:GridView>
    
    </td>
</tr>
</tbody>
</table>
</fieldset>
</div>
</asp:Content>

