<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="conf_comisiones.aspx.cs" Inherits="manager_conf_comisiones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 95%">
        <tr>
            <td align="center">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:Menu ID="Menu1" runat="server" BorderStyle="Outset" 
                    onmenuitemclick="Menu1_MenuItemClick" Orientation="Horizontal" 
                    Width="400px" Font-Bold="True">
                    <Items>
                        <asp:MenuItem Text="Servicio y Rubro" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="Servicio y Agente" Value="3"></asp:MenuItem>
                        <asp:MenuItem Text="Servicio" Value="1"></asp:MenuItem>
                         <asp:MenuItem Text="Servicio, Trafico y Regimen" Value="2"></asp:MenuItem>
                    </Items>
                </asp:Menu>
            </td>
        </tr>
        <tr>
            <td>
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View1" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%" >
                                <tr><td colspan="8" align="center" bgcolor="#66CCFF">Usted esta configurando: <asp:Label ID="lb_titulo" 
                                        runat="server" Font-Bold="True" Font-Italic="True"></asp:Label> </td></tr>
                                    <tr>
                                        <td style="width: 51px"> Servicio:</td><td style="width: 114px">
                                        <asp:DropDownList ID="ddl_tipo_servico" runat="server" Width="100px" 
                                            AutoPostBack="True" Font-Bold="False" Font-Italic="False" 
                                            onselectedindexchanged="ddl_tipo_servico_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                        <td style="width: 57px"> Rubro:</td><td style="width: 163px"><asp:DropDownList ID="ddl_tipo_rubro" runat="server"  Width="150px">
                                        </asp:DropDownList></td>
                                        <td style="width: 41px">Tarifa:</td><td style="width: 113px"><asp:DropDownList ID="ddl_tipo_tarifa" runat="server"  Width="60px">
                                        </asp:DropDownList></td>
                                        <td style="width: 29px">Valor:</td><td>
                                            <asp:TextBox ID="tb_valor" runat="server" Width="57px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Agente:</td><td><asp:DropDownList ID="ddl_agente" runat="server" Width="100px">
                                        </asp:DropDownList></td><td>Tipo:</td><td>
                                        <asp:DropDownList ID="ddl_tipo" runat="server" Width="150px">
                                        </asp:DropDownList></td><td>IVA:</td><td>
                                        <asp:DropDownList ID="ddl_iva" runat="server">
                                        </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Observaciones:</td><td colspan="7"> 
                                            <asp:TextBox ID="tb_observaciones" runat="server" Width="500px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                                AllowPaging="True" onpageindexchanging="GridView1_PageIndexChanging" 
                                                PageSize="5" onrowdeleting="GridView1_RowDeleting">
                                                <Columns>
                                                    <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                                                </Columns>
                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <RowStyle ForeColor="#000066" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                            </asp:GridView> </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8" align="center"> 
                                            <asp:Button ID="bt_guardar" runat="server" Text="Guardar" 
                                                onclick="bt_guardar_Click" /> </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                   <tr><td colspan="8" align="center" bgcolor="#66CCFF">Usted esta configurando: <asp:Label ID="lb_titulo1" 
                                        runat="server" Font-Bold="True" Font-Italic="True"></asp:Label> </td></tr>
                                        <tr>
                                           <td style="width: 51px"> Servicio:</td><td style="width: 114px">
                                        <asp:DropDownList ID="ddl_servicio1" runat="server" Width="100px" Font-Bold="False" 
                                                Font-Italic="False">
                                        </asp:DropDownList></td>
                                        <td>Tipo:</td><td>
                                        <asp:DropDownList ID="ddl_tipo1" runat="server" Width="150px">
                                        </asp:DropDownList></td>
                                        <td style="width: 41px">Tarifa:</td><td style="width: 113px"><asp:DropDownList ID="ddl_tarifa1" runat="server"  Width="60px">
                                        </asp:DropDownList></td>
                                        <td>IVA:</td><td>
                                        <asp:DropDownList ID="ddl_iva1" runat="server">
                                        </asp:DropDownList></td>
                                        <td style="width: 29px">Valor:</td><td>
                                            <asp:TextBox ID="tb_valor1" runat="server" Width="57px"></asp:TextBox>
                                        </td>
                                        </tr>
                                        <tr>
                                        <td style="width: 29px">sobre venta:</td><td>
                                            <asp:TextBox ID="tb_sobre_venta" runat="server" Width="57px"></asp:TextBox>
                                        </td>
                                        <td style="width: 29px">IVA:</td><td>
                                            <asp:TextBox ID="tb_iva" runat="server" Width="57px"></asp:TextBox>
                                        </td>
                                        </tr>
                                        <tr>
                                            <td>Observaciones:</td><td colspan="7"> 
                                            <asp:TextBox ID="tb_observaciones1" runat="server" Width="500px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                             <asp:GridView ID="GridView2" runat="server" BackColor="White" 
                                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                                AllowPaging="True" onpageindexchanging="GridView2_PageIndexChanging" 
                                                PageSize="5" onrowdeleting="GridView2_RowDeleting">
                                                <Columns>
                                                    <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                                                </Columns>
                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <RowStyle ForeColor="#000066" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                            </asp:GridView> 
                                            </td>
                                        </tr>
                                        <tr>
                                        <td colspan="8" align="center"> 
                                            <asp:Button ID="bt_guardar1" runat="server" Text="Guardar" 
                                                onclick="bt_guardar1_Click" /> </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                                   <tr><td colspan="8" align="center" bgcolor="#66CCFF">Usted esta configurando: <asp:Label ID="lb_titulo2" 
                                        runat="server" Font-Bold="True" Font-Italic="True"></asp:Label> </td></tr>
                                        <tr>    
                                            <td style="width: 51px; height: 25px;"> Servicio:</td>
                                            <td style="width: 114px; height: 25px;">
                                        <asp:DropDownList ID="ddl_servicio2" runat="server" Width="100px" Font-Bold="False" 
                                                Font-Italic="False">
                                        </asp:DropDownList></td>
                                         <td style="width: 51px; height: 25px;"> Trafico:</td>
                                            <td style="width: 114px; height: 25px;">
                                        <asp:DropDownList ID="ddl_tipo_trafico" runat="server" Width="100px" Font-Bold="False" 
                                                Font-Italic="False">
                                        </asp:DropDownList></td>
                                        <td style="width: 51px; height: 25px;"> Regimen:</td>
                                            <td style="width: 114px; height: 25px;">
                                        <asp:DropDownList ID="ddl_regimen" runat="server" Width="100px" Font-Bold="False" 
                                                Font-Italic="False">
                                        </asp:DropDownList></td>
                                        <td style="width: 29px">Valor:</td><td>
                                            <asp:TextBox ID="tb_valor2" runat="server" Width="57px"></asp:TextBox>
                                        </td>
                                        </tr>
                                        <tr>
                                            <td>Tipo:</td>  <td style="width: 114px; height: 25px;">
                                        <asp:DropDownList ID="ddl_tipo2" runat="server" Width="100px" Font-Bold="False" 
                                                Font-Italic="False">
                                        </asp:DropDownList></td>
                                        
                                         <td>Observaciones:</td><td  colspan="5"> 
                                            <asp:TextBox ID="tb_observacion2" runat="server" Width="400px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 41px">Tarifa:</td><td style="width: 113px"><asp:DropDownList ID="ddl_tarifa2" runat="server"  Width="60px">
                                        </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                             <asp:GridView ID="GridView3" runat="server" BackColor="White" 
                                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                                AllowPaging="True" onpageindexchanging="GridView3_PageIndexChanging" 
                                                PageSize="5" onrowdeleting="GridView3_RowDeleting">
                                                <Columns>
                                                    <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                                                </Columns>
                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <RowStyle ForeColor="#000066" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                            </asp:GridView> 
                                            </td>
                                             <tr>
                                        <td colspan="8" align="center"> 
                                            <asp:Button ID="bt_guardar3" runat="server" Text="Guardar" 
                                                onclick="bt_guardar3_Click" /> </td>
                                    </tr>
                                        </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View4" runat="server">
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%" >
                                <tr><td colspan="8" align="center" bgcolor="#66CCFF">Usted esta configurando: <asp:Label ID="lb_titulo4" 
                                        runat="server" Font-Bold="True" Font-Italic="True"></asp:Label> </td></tr>
                                    <tr>
                                        <td style="width: 51px"> Servicio:</td><td style="width: 114px">
                                        <asp:DropDownList ID="ddl_servicio3" runat="server" Width="100px" 
                                            AutoPostBack="True" Font-Bold="False" Font-Italic="False" 
                                            onselectedindexchanged="ddl_tipo_servico_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                        <td style="width: 57px">&nbsp;</td><td style="width: 163px">&nbsp;</td>
                                        <td style="width: 41px">Tarifa:</td><td style="width: 113px"><asp:DropDownList ID="ddl_tarifa3" runat="server"  Width="60px">
                                        </asp:DropDownList></td>
                                        <td style="width: 29px">Valor:</td><td>
                                            <asp:TextBox ID="tb_valor3" runat="server" Width="57px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Agente:</td><td><asp:DropDownList ID="ddl_agente1" runat="server" Width="100px">
                                        </asp:DropDownList></td><td>Tipo:</td><td>
                                        <asp:DropDownList ID="ddl_tipo3" runat="server" Width="150px">
                                        </asp:DropDownList></td><td>IVA:</td><td>
                                        <asp:DropDownList ID="ddl_iva2" runat="server">
                                        </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Observaciones:</td><td colspan="7"> 
                                            <asp:TextBox ID="tb_observacion3" runat="server" Width="500px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <asp:GridView ID="GridView4" runat="server" BackColor="White" 
                                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                                AllowPaging="True" onpageindexchanging="GridView4_PageIndexChanging" 
                                                PageSize="5" onrowdeleting="GridView4_RowDeleting">
                                                <Columns>
                                                    <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                                                </Columns>
                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <RowStyle ForeColor="#000066" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                            </asp:GridView> </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8" align="center"> 
                                            <asp:Button ID="bt_guardar4" runat="server" Text="Guardar" 
                                                onclick="bt_guardar4_Click" /> </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
            </td>
        </tr>
    </table>
</div>
</asp:Content>

