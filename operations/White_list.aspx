<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="White_list.aspx.cs" Inherits="operations_a" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <table align="center" cellpadding="0" cellspacing="0" 
        style=" width:100%;  border: 1px solid #E6E6E6; margin: 0px; color: black;">
        <tr>
            <td bgcolor="WhiteSmoke" height="25">
            <h3 id="adduser" style="color: #A43708">ADMINISTRACION DE WHITELIST<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
            <br />
                <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                    Width="700px" BackColor="White" BorderStyle="None" 
                    Font-Bold="False" Font-Names="Arial,Tahoma,Verdana" Font-Size="10pt" 
                    ScrollBars="Auto">
                    <ajaxToolkit:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                        <HeaderTemplate>
                            ASIGNAR WHITELIST
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%; color:Black;">
                                    <tr>
                                        <td>
                                            Empresa</td>
                                        <td>
                                            <asp:DropDownList ID="drp_empresas" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_empresas_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Sucursal</td>
                                        <td>
                                            <asp:DropDownList ID="drp_sucursales" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_sucursales_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo de Persona</td>
                                        <td>
                                            <asp:DropDownList ID="drp_tipo_persona" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="drp_tipo_persona_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                <asp:ListItem Value="3">Cliente</asp:ListItem>
                                                <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                                <asp:ListItem Value="2">Agente</asp:ListItem>
                                                <asp:ListItem Value="5">Naviera</asp:ListItem>
                                                <asp:ListItem Value="6">Linea Aerea</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            ID</td>
                                        <td>
                                            <asp:TextBox ID="tb_id" runat="server" AutoPostBack="True" 
                                                ontextchanged="tb_id_TextChanged"></asp:TextBox>
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="tb_id_TextBoxWatermarkExtender" 
                                                runat="server" Enabled="True" TargetControlID="tb_id" 
                                                WatermarkCssClass="watermark" WatermarkText="Ingrese el ID...">
                                            </ajaxToolkit:TextBoxWatermarkExtender>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="tb_id_FilteredTextBoxExtender" 
                                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="tb_id">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btn_buscar" runat="server" Text="Buscar" 
                                                onclick="btn_buscar_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:GridView ID="gv_asignar_whitelist" runat="server" 
                                                AutoGenerateColumns="False" 
                                                EmptyDataText="......" BorderColor="Black" 
                                                BorderStyle="Solid" BorderWidth="1px" GridLines="None" Width="650px" 
                                                Font-Size="8pt">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chk_asignar" runat="server" 
                                                                ToolTip="Asignar/Desasignar WhiteList" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="WhiteList">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_whitelist" runat="server" Text='<%# Eval("WHITELIST") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TipoWhiteList" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_tipo_whitelist_id" runat="server" Text='<%# Eval("TIPOWHITELIST") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tipo">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="drp_tipo" runat="server">
                                                                <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nombre">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_nombre" runat="server" Text='<%# Eval("NOMBRE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sugerido por">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="tb_sugeridopor" runat="server" 
                                                                Text='<%# Eval("SUGERIDOPOR") %>' MaxLength="90"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle BackColor="#66CCFF" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asignado por">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_usuario" runat="server" Text='<%# Eval("USUARIO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fecha">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_fecha" runat="server" Text='<%# Eval("FECHA") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IDWhitelist" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_idwhitelist" runat="server" Text='<%# Eval("IDWHITELIST") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="Black" ForeColor="White" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btn_asignar" runat="server" Text="Asignar" 
                                                onclick="btn_asignar_Click" />
                                        </td>
                                    </tr>
                                </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2">
                        <HeaderTemplate>
                            REPORTE DE WHITELIST'S
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%; color:Black">
                            <tr>
                                <td>
                                    Empresa</td>
                                <td>
                                    <asp:DropDownList ID="drp_empresas2" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="drp_empresas2_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 27px">
                                    Sucursal</td>
                                <td style="height: 27px">
                                    <asp:DropDownList ID="drp_sucursales2" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="drp_sucursales2_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tipo de Persona</td>
                                <td>
                                    <asp:DropDownList ID="drp_tipo_persona2" runat="server">
                                        <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                        <asp:ListItem Value="3">Cliente</asp:ListItem>
                                        <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                        <asp:ListItem Value="2">Agente</asp:ListItem>
                                        <asp:ListItem Value="5">Naviera</asp:ListItem>
                                        <asp:ListItem Value="6">Linea Aerea</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btn_visualizar" runat="server" onclick="btn_visualizar_Click" 
                                        Text="Visualizar" />
                                </td>
                            </tr>
                        </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" HeaderText="TabPanel3" ID="TabPanel3">
                        <HeaderTemplate>
                            WHITELIST MULTIPLE
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <table align="center" cellpadding="0" cellspacing="0" 
                                        style="width: 90%; color:Black;">
                                        <tr>
                                            <td>
                                                Empresa</td>
                                            <td>
                                                <asp:DropDownList ID="drp_empresas3" runat="server" AutoPostBack="True" 
                                                    onselectedindexchanged="drp_empresas3_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Sucursal</td>
                                            <td>
                                                <asp:DropDownList ID="drp_sucursales3" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Tipo WhiteList</td>
                                            <td>
                                                <asp:DropDownList ID="drp_tipo3" runat="server" AutoPostBack="True" 
                                                    onselectedindexchanged="drp_tipo_persona_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                    <asp:ListItem Value="1">Permanente</asp:ListItem>
                                                    <asp:ListItem Value="2">Temporal</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Sugerido Por</td>
                                            <td>
                                                <asp:TextBox ID="tb_sugerido_por3" runat="server" Height="16px" Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Tipo de Persona</td>
                                            <td>
                                                <asp:DropDownList ID="drp_tipo_persona3" runat="server" AutoPostBack="True" 
                                                    onselectedindexchanged="drp_tipo_persona_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                                                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                                    <asp:ListItem Value="2">Agente</asp:ListItem>
                                                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                                                    <asp:ListItem Value="6">Linea Aerea</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                ID</td>
                                            <td>
                                                <asp:TextBox ID="tb_id_3" runat="server" AutoPostBack="True" Height="16px"></asp:TextBox>
                                                <ajaxToolkit:TextBoxWatermarkExtender ID="tb_id_3_TextBoxWatermarkExtender" 
                                                    runat="server" Enabled="True" TargetControlID="tb_id_3" 
                                                    WatermarkCssClass="watermark" WatermarkText="Ingrese el ID...">
                                                </ajaxToolkit:TextBoxWatermarkExtender>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="tb_id_3_FilteredTextBoxExtender" 
                                                    runat="server" Enabled="True" FilterType="Numbers" TargetControlID="tb_id_3">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btn_buscar_3" runat="server" Text="Buscar" 
                                                    onclick="btn_buscar_3_Click" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btn_limpiar" runat="server" onclick="btn_limpiar_Click" 
                                                    Text="Limpiar" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:GridView ID="gv_asignar_whitelist4" runat="server" 
                                                    AutoGenerateColumns="False" BorderColor="Black" BorderStyle="Solid" 
                                                    BorderWidth="1px" EmptyDataText="......" Font-Size="8pt" GridLines="None" 
                                                    onselectedindexchanged="gv_asignar_whitelist4_SelectedIndexChanged" 
                                                    Width="650px">
                                                    <Columns>
                                                        <asp:CommandField ButtonType="Button" SelectText="Agregar" 
                                                            ShowSelectButton="True" />
                                                        <asp:TemplateField HeaderText="WhiteList">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_whitelist4" runat="server" Text='<%# Eval("WHITELIST") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="TipoWhiteList" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_tipo_whitelist_id4" runat="server" 
                                                                    Text='<%# Eval("TIPOWHITELIST") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tipo">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="drp_tipo4" runat="server">
                                                                    <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_id4" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nombre">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_nombre4" runat="server" Text='<%# Eval("NOMBRE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sugerido por">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="tb_sugeridopor4" runat="server" MaxLength="90" 
                                                                    Text='<%# Eval("SUGERIDOPOR") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle BackColor="#66CCFF" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asignado por">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_usuario4" runat="server" Text='<%# Eval("USUARIO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fecha">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_fecha4" runat="server" Text='<%# Eval("FECHA") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="IDWhitelist" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_idwhitelist4" runat="server" 
                                                                    Text='<%# Eval("IDWHITELIST") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="Black" ForeColor="White" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" 
                                                style="text-decoration: underline; font-weight: bold;">
                                                Listado de Personas a Agregar al WhiteList</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="gv_white_list_multiple" runat="server" Font-Size="8pt" 
                                                    onrowdeleting="gv_white_list_multiple_RowDeleting">
                                                    <Columns>
                                                        <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                                                            ShowDeleteButton="True" />
                                                    </Columns>
                                                    <HeaderStyle BackColor="Black" ForeColor="White" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                &nbsp;<asp:Button ID="btn_guardar" runat="server" Text="Guardar" 
                                                    onclick="btn_guardar_Click" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
                <br />
            </td>
        </tr>
    </table>
 </asp:Content>

