﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="ro_noasociados.aspx.cs" Inherits="Reports_ro_noasociados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<table width="650">
    <tr>
        <td>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <table width="300" align="center">
            <thead>
                <tr>
                <td align="center" colspan="2">
                    <b><span style="font-size: small">&nbsp;Routing No Asociados</span></b></td>
                </tr>
            </thead>
                <tr>
                <td>Fecha Inicial</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" runat="server" Height="16px"></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechainicial" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
                <tr>
                <td>Fecha Final&nbsp;</td>
                <td>
                    <asp:TextBox ID="tb_fechafinal" runat="server" Height="16px"></asp:TextBox>
                    <cc1:CalendarExtender ID="tb_fechafinal_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechafinal" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                </tr>
                <tr>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btn_generar" runat="server" Text="Generar" 
                        onclick="btn_generar_Click" />
                </td>
            </tr>

            </table>
        </td>
    </tr>
</table>
</asp:Content>

