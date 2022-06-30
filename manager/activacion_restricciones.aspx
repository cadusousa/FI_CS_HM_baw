<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="activacion_restricciones.aspx.cs" Inherits="manager_searchdepartamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3>ACTIVAR O DESACTIVAR RESTRICCIONES</h3>
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
                                onrowcreated="gv_departamentos_RowCreated" 
                                onrowcommand="gv_departamentos_RowCommand">
                    <Columns>
                        <asp:ButtonField CommandName="activar/desactivar" Text="Activar/Desactivar" />
                    </Columns>
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

