<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="searchgroup.aspx.cs" Inherits="manager_serchgroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3>Listado de Grupos</h3>
<br />
    <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
        <tr>
            <td align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                    <tr>
                        <td align="center">
                <asp:GridView ID="gv_grupos" runat="server" 
                    EmptyDataText="No existe ningun departamento" onrowcreated="gv_grupos_RowCreated">
                </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        </table>
<br />
</div>
</asp:Content>

