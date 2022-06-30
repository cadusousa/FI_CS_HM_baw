<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="configurar_automatizacion_servicios.aspx.cs" Inherits="manager_configurar_automatizacion_servicios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div style="border: 1px solid #EEEEEE; color: #CC3300; background-color: #EEEEEE;"><h2>Automatizacion de Servicios</h2></div>
<div align="center" style="border: 1px solid #EEEEEE">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
        <tr>
            <td align="center" valign="bottom">
                <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" Font-Bold="True" 
                    onmenuitemclick="Menu1_MenuItemClick" Font-Size="Small">
                    <Items>
                        <asp:MenuItem Text="Inicio" Value="3"></asp:MenuItem>
                        <asp:MenuItem Text="Ingresar Automatizacion" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="Configurar Automatizacion" Value="2"></asp:MenuItem>
                    </Items>
                </asp:Menu>
            </td>
        </tr>
        <tr>
            <td>
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                    <p><strong>Modulo para configurar la Automatizacion de Servicios.</strong></p>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                    <div align="center">
                        <table align="center" cellpadding="0" cellspacing="0" style="width: 80%">
                            <tr>
                                <td style="height: 25px">
                                    <strong>Pais donde se Automatizaran los Servicios</strong></td>
                                <td style="height: 25px">
                                    <asp:DropDownList ID="drp_paises" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="drp_paises_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px; font-weight: 700;">
                                    Sucursal donde se Automatizaran los Servicios</td>
                                <td style="height: 25px">
                                    <asp:DropDownList ID="drp_sucursal_automatizar" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Tipo de Servicio a Automatizar</strong></td>
                                <td>
                                    <asp:DropDownList ID="drp_tipos_servicio" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btn_guardar_automatizacion" runat="server" Text="Guardar" 
                                        onclick="btn_guardar_automatizacion_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    </asp:View>
                    <asp:View ID="View3" runat="server">
                    <div align="center">

                        <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                            <tr>
                                <td style="height: 26px">
                                    <strong>Seleccione Automatizacion de Servicios</strong></td>
                                <td style="height: 26px">
                                    <asp:DropDownList ID="drp_automatizacion_servicios" runat="server" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="drp_automatizacion_servicios_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Panel ID="Panel1" runat="server" Visible="False">
                                        <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                            <tr>
                                                <td colspan="2" height="25">
                                                    <strong>Agregar nuevas Transacciones</strong></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Tipo de Documento</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_tipo_documento" runat="server" AutoPostBack="True" 
                                                        onselectedindexchanged="drp_tipo_documento_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Pais Destino</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_pais_destino" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Contabilidad Origen</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_contabilidad_origen" runat="server" 
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="drp_contabilidad_origen_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                        <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                                        <asp:ListItem Value="2">Financiera</asp:ListItem>
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
                                                    Contabilidad Destino</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_contabilidad_destino" runat="server" 
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="drp_contabilidad_destino_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                        <asp:ListItem Value="1">Fiscal</asp:ListItem>
                                                        <asp:ListItem Value="2">Financiera</asp:ListItem>
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
                                                        <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                        <asp:ListItem Value="1">Facturacion</asp:ListItem>
                                                        <asp:ListItem Value="2">Operaciones</asp:ListItem>
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
                                                    Tipo de Persona</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_tipo_persona" runat="server">
                                                        <asp:ListItem Value="0">Seleccione...</asp:ListItem>
                                                        <asp:ListItem Value="2">Agente</asp:ListItem>
                                                        <asp:ListItem Value="3">Cliente</asp:ListItem>
                                                        <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                                        <asp:ListItem Value="5">Naviera</asp:ListItem>
                                                        <asp:ListItem Value="6">Linea Aerea</asp:ListItem>
                                                        <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    ID Persona</td>
                                                <td>
                                                    <asp:TextBox ID="tb_id_persona" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Tipo de Servicio</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_tipo_servicio" runat="server" AutoPostBack="True" 
                                                        onselectedindexchanged="drp_tipo_servicio_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Rubro</td>
                                                <td>
                                                    <asp:DropDownList ID="drp_rubro" runat="server">
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
                                                <td>
                                                    Cobra IVA</td>
                                                <td>
                                                    <asp:RadioButtonList ID="rbl_cobra_iva" runat="server" 
                                                        RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="True">Si</asp:ListItem>
                                                        <asp:ListItem Value="False">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Monto</td>
                                                <td>
                                                    <asp:TextBox ID="tb_monto" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="height: 17px">
                                    <asp:Button ID="btn_agregar_transaccion" runat="server" 
                                        onclick="btn_agregar_transaccion_Click" Text="Agregar" />
                                    <asp:Button ID="btn_nuevo" runat="server" onclick="btn_nuevo_Click" 
                                        Text="Nuevo" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="Panel2" runat="server" Visible="False">
                                    <div align="center">
                                        <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                            <tr>
                                                <td>
                                                    <strong>Transacciones Asociadas</strong></td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="height: 17px">
                                                    <asp:Panel ID="Panel3" runat="server" ScrollBars="Horizontal" Width="500px">
                                                        <asp:GridView ID="gv_transacciones_asociadas" runat="server" BackColor="White" 
                                                            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                                                            ForeColor="Black" GridLines="Vertical" 
                                                            onrowdatabound="gv_transacciones_asociadas_RowDataBound" 
                                                            onrowdeleting="gv_transacciones_asociadas_RowDeleting">
                                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                                            <Columns>
                                                                <asp:CommandField ButtonType="Image" DeleteImageUrl="~/img/icons/delete.png" 
                                                                    ShowDeleteButton="True" />
                                                            </Columns>
                                                            <FooterStyle BackColor="#CCCCCC" />
                                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                            <SortedAscendingHeaderStyle BackColor="#808080" />
                                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                            <SortedDescendingHeaderStyle BackColor="#383838" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;</td>
                            </tr>
                        </table>

                    </div>
                    </asp:View>
                    <asp:View ID="View4" runat="server">
                    <div>

                        <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                            <tr>
                                <td align="Left">
                                    <strong>Automatizaciones Ingresadas</strong></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gv_automatizaciones" runat="server" BackColor="White" 
                                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                                        ForeColor="Black" GridLines="Vertical" 
                                        onrowdatabound="gv_automatizaciones_RowDataBound" 
                                        onrowdeleting="gv_automatizaciones_RowDeleting">
                                        <AlternatingRowStyle BackColor="#CCCCCC" />
                                        <Columns>
                                            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/img/icons/delete.png" 
                                                ShowDeleteButton="True" />
                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" />
                                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#808080" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#383838" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>

                    </div>
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
    </table>

</div>
</asp:Content>

