<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="Anular_Liquidacion.aspx.cs" Inherits="Reports_Anular_Liquidacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box" align]="center">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
        <tr>
            <td align="center" bgcolor="#EEEEEE" colspan="4" height="30px" 
                style="height: 19px">
                <strong>ANULAR LIQUIDACION</strong></td>
        </tr>
        <tr>
            <td>
                Serie</td>
            <td>
                <asp:TextBox ID="tb_serie" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
            <td>
                Correlativo</td>
            <td>
                <asp:TextBox ID="tb_correlativo" runat="server" ReadOnly="True"></asp:TextBox>
                <asp:Label ID="lbl_liquidacionid" runat="server" Text="0" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Banco</td>
            <td>
                <asp:TextBox ID="tb_banco" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
            <td>
                Cuenta</td>
            <td>
                <asp:TextBox ID="tb_cuenta" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 18px">
                Codigo</td>
            <td style="height: 18px" colspan="3">
                <asp:TextBox ID="tb_codigo" runat="server" ReadOnly="True" Width="50px"></asp:TextBox>
&nbsp;
                Nombre&nbsp;
                <asp:TextBox ID="tb_nombre" runat="server" ReadOnly="True" Width="250px"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td colspan="4">
                Motivo&nbsp;&nbsp;
                <asp:TextBox ID="tb_motivo" runat="server" Width="370px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Button ID="btn_aceptar" runat="server" Text="Si" 
                    onclick="btn_aceptar_Click" Width="50px" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_cancelar" runat="server" Text="No" Width="50px" 
                    onclick="btn_cancelar_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Button ID="btn_regresar" runat="server" onclick="btn_regresar_Click" 
                    Text="Regresar" Visible="False" />
            </td>
        </tr>
    </table>
    <br />
</div>
</asp:Content>


