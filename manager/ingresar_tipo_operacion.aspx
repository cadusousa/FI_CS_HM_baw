<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="ingresar_tipo_operacion.aspx.cs" Inherits="manager_ingresar_tipo_operacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div align="center">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 70%">
        <tr>
            <td align="right" colspan="2">
                <asp:HyperLink ID="HyperLink1" runat="server" 
                    NavigateUrl="~/manager/index_contabilizacion_automatica.aspx">Inicio</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="background-color: #000000; color: #FFFFFF; font-weight: bold;">
                Ingresar Tipo de Operacion</td>
        </tr>
        <tr>
            <td>
                Tipo de Operacion</td>
            <td>
                <asp:TextBox ID="tb_tipo_operacion" runat="server" Height="16px" 
                    MaxLength="45" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_guardar" runat="server" onclick="btn_guardar_Click" 
                    Text="Guardar" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="background-color: #000000; color: #FFFFFF; font-weight: bold;">
                Tipos de Operacion</td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gv_tipo_operacion" runat="server" 
                    onrowdeleting="gv_tipo_operacion_RowDeleting" Font-Size="XX-Small">
                    <Columns>
                        <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
</asp:Content>

