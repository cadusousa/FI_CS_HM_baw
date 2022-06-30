<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="asociar_transacciones_contabilizacion_automatica.aspx.cs" Inherits="manager_asociar_transacciones_contabilizacion_automatica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div align="center">

    <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
        <tr>
            <td align="right" colspan="2">
                <asp:HyperLink ID="HyperLink1" runat="server" 
                    NavigateUrl="~/manager/index_contabilizacion_automatica.aspx">Inicio</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="color: #FFFFFF; font-weight: bold; background-color: #000000">
                Asociar Transacciones
                Contabilizacion Automatica</td>
        </tr>
        <tr>
            <td>
                Caso</td>
            <td>
                                            <asp:DropDownList ID="drp_caso" runat="server">
                                            </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="height: 33px">
                <asp:Button ID="btn_ver" runat="server" Text="Ver" onclick="btn_ver_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_nuevo" runat="server" onclick="btn_nuevo_Click" 
                    Text="Nuevo" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Panel ID="Panel1" runat="server" Visible="False">
                    <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 95%">
                        <tr>
                            <td align="center" 
                            style="font-weight: bold; color: #FFFFFF; background-color: #000000">
                                Asociar Transaccion</td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 98%">
                                    <tr>
                                        <td>
                                            Transaccion</td>
                                        <td>
                                            <asp:DropDownList ID="drp_transaccion" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo Operacion</td>
                                        <td>
                                            <asp:DropDownList ID="drp_tipo_operacion" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo Transaccion</td>
                                        <td>
                                            <asp:DropDownList ID="drp_tipo_transaccion" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 18px">
                                            Transaccion Padre</td>
                                        <td style="height: 18px">
                                            <asp:DropDownList ID="drp_transaccion_padre" runat="server" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btn_agregar" runat="server" Text="Agregar" 
                                                onclick="btn_agregar_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" 
                            style="font-weight: bold; color: #FFFFFF; background-color: #000000">
                                Transacciones Asociadas</td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" style="height: 18px">
                                <asp:Panel ID="Panel2" runat="server" Height="400px" ScrollBars="Both" 
                                    Width="650px">
                                    <asp:GridView ID="gv_transacciones_asociadas" runat="server" 
                                        onrowdeleting="gv_transacciones_asociadas_RowDeleting" 
                                        Font-Size="XX-Small">
                                        <Columns>
                                            <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>

</div>
</asp:Content>

