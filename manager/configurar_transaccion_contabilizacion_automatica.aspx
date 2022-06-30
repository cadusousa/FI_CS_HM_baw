<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="configurar_transaccion_contabilizacion_automatica.aspx.cs" Inherits="manager_configurar_transaccion_contabilizacion_automatica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div align="center">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 97%">
        <tr>
            <td align="right" colspan="2">
                <asp:HyperLink ID="HyperLink1" runat="server" 
                    NavigateUrl="~/manager/index_contabilizacion_automatica.aspx">Inicio</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" 
                style="font-weight: bold; color: #FFFFFF; background-color: #000000">
                Configurar Transacciones</td>
        </tr>
        <tr>
            <td>
                Empresa</td>
            <td>
                <asp:DropDownList ID="drp_empresa" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Sistema</td>
            <td>
                <asp:DropDownList ID="drp_sistema" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drp_sistema_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Tipo de Operacion</td>
            <td>
                <asp:DropDownList ID="drp_tipo_operacion" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btn_visualizar" runat="server" onclick="btn_visualizar_Click" 
                    Text="Visualizar" />
            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_nuevo" runat="server" onclick="btn_nuevo_Click" 
                    Text="Nuevo" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
                        <tr>
                            <td align="left" 
                                style="font-weight: bold; color: #FFFFFF; background-color: #000000">
                                Transacciones Asociadas</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" Height="250px" ScrollBars="Both" 
                                    Visible="False" Width="690px">
                                    <asp:GridView ID="gv_transacciones" 
    runat="server" Font-Size="XX-Small" 
                                    Visible="False" 
                                    
    onselectedindexchanged="gv_transacciones_SelectedIndexChanged">
                                        <Columns>
                                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                            <asp:TemplateField HeaderText="TERCEROS">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drp_terceros" runat="server" Font-Size="XX-Small" 
                                                    Enabled="False">
                                                        <asp:ListItem Value="0">-</asp:ListItem>
                                                        <asp:ListItem Value="True">True</asp:ListItem>
                                                        <asp:ListItem>False</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AUTOMATIZAR">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drp_automatizar" runat="server" Font-Size="XX-Small" 
                                                    Enabled="False">
                                                        <asp:ListItem Value="0">-</asp:ListItem>
                                                        <asp:ListItem>True</asp:ListItem>
                                                        <asp:ListItem>False</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" 
                                style="font-weight: bold; color: #FFFFFF; background-color: #000000">
                                Nueva Configuracion</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="Panel2" runat="server" Height="280px" ScrollBars="Both" 
                                    Visible="False">
                                    <table align="center" cellpadding="0" 
    cellspacing="0" style="width: 95%">
                                        <tr>
                                            <td align="left" colspan="2" style="font-weight: bold">
                                                Transaccion ID.:
                                                <asp:Label ID="lbl_transaccion_id" runat="server" Text="0"></asp:Label>
                                                <asp:Label ID="lbl_ttr_id" runat="server" Text="0" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Contabilidad Destino</td>
                                            <td>
                                                <asp:DropDownList ID="drp_contabilidad" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_contabilidad_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Moneda Origen</td>
                                            <td>
                                                <asp:DropDownList ID="drp_moneda_origen" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Moneda Destino</td>
                                            <td>
                                                <asp:DropDownList ID="drp_moneda_destino" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Sucursal</td>
                                            <td>
                                                <asp:DropDownList ID="drp_sucursal" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Operacion</td>
                                            <td>
                                                <asp:DropDownList ID="drp_operacion" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_operacion_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Serie</td>
                                            <td>
                                                <asp:DropDownList ID="drp_serie" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Genera Partida</td>
                                            <td>
                                                <asp:RadioButtonList ID="rbl_partida" runat="server">
                                                    <asp:ListItem Value="True">Si</asp:ListItem>
                                                    <asp:ListItem Value="False">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btn_agregar" runat="server" Text="Agregar" 
                                                    onclick="btn_agregar_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" 
                                style="font-weight: bold; color: #FFFFFF; background-color: #000000">
                                Configuraciones</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="Panel3" runat="server" Height="150px" ScrollBars="Both" 
                                    Visible="False">
                                    <asp:GridView ID="gv_configuraciones" 
    runat="server" Font-Size="XX-Small" onrowdeleting="gv_configuraciones_RowDeleting">
                                        <Columns>
                                            <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                &nbsp;</td>
        </tr>
    </table>
</div>
</asp:Content>


