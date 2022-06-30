<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="adddepartamento.aspx.cs" Inherits="manager_adddepartamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<h3 id="adduser">CREAR DEPARTAMENTO</h3>
    <table align="center" style="width: 80%">
        <tr>
            <td>
                Pais</td>
            <td>
                <asp:DropDownList ID="drp_paises" runat="server">
                </asp:DropDownList>
                </td>
        </tr>
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
                    MaxLength="254"></asp:TextBox>
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

