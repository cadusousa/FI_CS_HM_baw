<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addgroup.aspx.cs" Inherits="manager_addgroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">CREAR GRUPO</h3>
    <table align="center" style="width: 80%">
        <tr>
            <td>
                Nombre</td>
            <td>
                <asp:TextBox ID="tb_nombre" runat="server" MaxLength="49"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td>
                Descripcion</td>
            <td>
                <asp:TextBox ID="tb_descripcion" runat="server" TextMode="MultiLine" 
                    MaxLength="99"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_guardar" runat="server" Text="Guardar" 
                    onclick="btn_guardar_Click" />
                </td>
        </tr>
    </table>
</div>
</asp:Content>

