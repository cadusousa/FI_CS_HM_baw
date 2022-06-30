<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="searchdepartamento.aspx.cs" Inherits="manager_searchdepartamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3>Busqueda de Departamentos</h3>
<br />
    <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
        <tr>
            <td>
                Pais</td>
            <td>
                <asp:DropDownList ID="drp_paises" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_visualizar" runat="server" Text="Visualizar" 
                    onclick="btn_visualizar_Click" />
                <br />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                    <tr>
                        <td align="center">
                <asp:GridView ID="gv_departamentos" runat="server" 
                    EmptyDataText="No existe ningun departamento" 
                                onrowcreated="gv_departamentos_RowCreated">
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

