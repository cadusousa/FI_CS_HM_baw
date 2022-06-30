<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="asignar_grupo.aspx.cs" Inherits="manager_asignar_grupo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3>Asignar Grupos</h3>
    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
        <tr>
            <td>
                Pais</td>
            <td>
                <asp:DropDownList ID="drp_paises" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drp_paises_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Departamento</td>
            <td>
                <asp:DropDownList ID="drp_departamentos" runat="server" Enabled="False" 
                    AutoPostBack="True" 
                    onselectedindexchanged="drp_departamentos_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
                    <tr>
                        <td>
                            Grupo Asignado</td>
                        <td>
                            <asp:DropDownList ID="drp_grupo_asignado" runat="server" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Grupos</td>
                        <td>
                            <asp:DropDownList ID="drp_grupos" runat="server" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="height: 18px">
                <asp:Button ID="btn_asignar" runat="server" Text="Asignar" Enabled="False" 
                    onclick="btn_asignar_Click" />
            </td>
        </tr>
    </table>
</div>
</asp:Content>

