<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="ingresar_caso.aspx.cs" Inherits="manager_ingresar_caso" %>

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
                style="background-color: #000000; color: #FFFFFF">
                <strong>Ingresar Caso</strong></td>
        </tr>
        <tr>
            <td>
                MBL</td>
            <td>
                <asp:RadioButtonList ID="rbl_tipo_mbl" runat="server">
                    <asp:ListItem Value="True">Prepaid</asp:ListItem>
                    <asp:ListItem Value="False">Collect</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="height: 18px">
                HBL</td>
            <td style="height: 18px">
                <asp:RadioButtonList ID="rbl_tipo_hbl" runat="server">
                    <asp:ListItem Value="True">Prepaid</asp:ListItem>
                    <asp:ListItem Value="False">Collect</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                Es Ruteado</td>
            <td>
                <asp:RadioButtonList ID="rbl_es_ruteado" runat="server">
                    <asp:ListItem>True</asp:ListItem>
                    <asp:ListItem>False</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="height: 25px">
                Tipo</td>
            <td style="height: 25px">
                <asp:RadioButtonList ID="rbl_imp_exp" runat="server">
                    <asp:ListItem Value="True">Import</asp:ListItem>
                    <asp:ListItem Value="False">Export</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                Nombre</td>
            <td>
                <asp:TextBox ID="tb_nombre" runat="server" Height="16px" Width="400px" 
                    MaxLength="300"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_guardar" runat="server" Text="Guardar" 
                    onclick="btn_guardar_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="background-color: #000000; color: #FFFFFF; font-weight: bold;">
                Casos</td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gv_casos" runat="server" onrowdeleting="gv_casos_RowDeleting" 
                    Font-Size="XX-Small">
                    <Columns>
                        <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
</asp:Content>

