<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="search_pagar_corte.aspx.cs" Inherits="search_pagar_corte" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {

    } 
    function mpeSeleccionOnCancel()
    {

    } 
    </script>
<div id="box">
<tbody>
<h3 id="adduser">BUSCAR RECIBOS SOA</h3>
    <table width="450" align="left">
                                <tr>
                                    <td align="center">
                                        <table>
                            <tr><td align="left">Serie:</td><td> <asp:DropDownList ID="lb_serie_factura" 
                                    runat="server" Width="100px">
                                    </asp:DropDownList><asp:TextBox ID="tb_corr_factura" runat="server" Width="35px" Visible="false"></asp:TextBox></td></tr>
                            <tr>
                                <td align="left">
                                    No. recibo:</td>
                                <td>
                                    <asp:TextBox ID="tb_recibo" runat="server" 
                                        Width="100px" Height="16px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                        ControlToValidate="tb_recibo" ErrorMessage="Solo se permite números" 
                                        SetFocusOnError="True" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                                </td>
                        </tr>
                        <tr>
                            <td>Tipo Proveedor</td>
                            <td>
                                <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="True" onselectedindexchanged="lb_tipopersona_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Seleccione</asp:ListItem>
                                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                                    <asp:ListItem Value="2">Agente</asp:ListItem>
                                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                                    <asp:ListItem Value="10">Intercompanys</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Codigo Proveedor</td>
                            <td>
                                <asp:TextBox ID="tb_clientid" runat="server" Height="16px" Width="67px" ></asp:TextBox>
                            </td>
                        </tr>                        <tr>
                            <td>Nombre Proveedor</td>
                            <td>
                                <asp:TextBox  ID="tb_clientname" runat="server" Height="16px" Width="252px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>                        <tr>
                            <td>Estado Documento</td>
                            <td>                                
                                <asp:DropDownList ID="lb_ted_id" runat="server">
                                    <asp:ListItem Value="1">Activa</asp:ListItem>
                                    <asp:ListItem Value="2">Abonada</asp:ListItem>
                                    <asp:ListItem Value="3">Anulada</asp:ListItem>
                                    <asp:ListItem Value="4">Pagada</asp:ListItem>
                                </asp:DropDownList>                                
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="left">
                                <!--Con saldo pendiente aplicar:--></td>
                            <td>
                                <!--<asp:CheckBox ID="chk_pendiente" runat="server" />-->
                                <asp:Button ID="Button1" runat="server" Text="Buscar" onclick="bt_search_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="dgw1" runat="server" onrowcommand="dgw1_RowCommand" 
                            AllowPaging="True" 
                            EmptyDataText="No se encontraron datos con los criterios ingresados" PageSize="20" 
                            onpageindexchanging="dgw1_PageIndexChanging" 
                            onrowcreated="dgw1_RowCreated">
                        <Columns>
                            <asp:ButtonField HeaderText="Seleccionar" Text="Seleccionar" 
                                    CommandName="Seleccionar" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
            <td>
            <%--********************************************************--%>
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" Width="722px" style="display:none">
            <div><table>
                <tr><td align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:<asp:TextBox ID="tb_nitb" runat="server" Height="16px" Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td>Codigo:<asp:TextBox ID="tb_codigo" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="Button2" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table>
            
            </div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Agente" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" onload="gv_proveedor_Load" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="5">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_clientid"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <%--*******************************************************************--%>
            </td>
            </tr>
    </table>
    </tbody>
  
  </div>
  
</asp:Content>

