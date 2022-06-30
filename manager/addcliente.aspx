<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addcliente.aspx.cs" Inherits="operations_Default" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">CLIENTES</h3>

        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {
        
    }  

    function mpeSeleccionOnCancel()
    {
        
    }
    function mpecancela_busqueda_rubro() {}
    </script>
        <table width="95%" align="center">
            <tr><td>
                Busqueda de proveedor:<asp:TextBox ID="tb_id_busqueda" runat="server" Height="16px" Width="384px"></asp:TextBox>
            </td></tr>
            <tr><td>
                Identificador interno:<asp:TextBox ID="tb_id"  Text="0" ReadOnly runat="server" Height="16px" Width="40px" ></asp:TextBox>
                Nombre:<asp:TextBox ID="tb_nombre" runat="server" Height="16px" Width="384px"></asp:TextBox>
                <br />
                Nit:
                <asp:TextBox ID="tb_nit" runat="server" Height="16px" Width="131px"></asp:TextBox>
                &nbsp;&nbsp; Nit2:
                <asp:TextBox ID="tb_nit2" runat="server" Height="16px" Width="131px"></asp:TextBox>
                <br />
                Nombre Facturar:
                <asp:TextBox ID="tb_nombre_facturar" runat="server" Height="16px" Width="317px"></asp:TextBox>
                <br />
                Tipo Cliente
                <asp:DropDownList ID="lb_tipolciente" runat="server">
                    <asp:ListItem Value="1">Tipo 1</asp:ListItem>
                </asp:DropDownList>
&nbsp; Grupo:
                <asp:DropDownList ID="lb_grupo" runat="server">
                    <asp:ListItem Value="1">Grupo 1</asp:ListItem>
                </asp:DropDownList>
                <br />
                Cobrador
                <asp:DropDownList ID="lb_cobrador" runat="server">
                </asp:DropDownList>
&nbsp; Estado cliente:
                <asp:DropDownList ID="lb_estadocliente" runat="server">
                    <asp:ListItem Value="1">Activo</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:CheckBox ID="chk_consignee" runat="server" Text="Es consignee" 
                    TextAlign="Left" />
&nbsp;&nbsp;
                <asp:CheckBox ID="chk_shipper" runat="server" Text="Es shipper" 
                    TextAlign="Left" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Clase:
                <asp:DropDownList ID="lb_clase" runat="server">
                    <asp:ListItem Value="1">Clase1</asp:ListItem>
                </asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp; Pais:
                <asp:DropDownList ID="lb_pais" runat="server">
                    <asp:ListItem Value="1">GT</asp:ListItem>
                </asp:DropDownList>
                <%--Provisiona en:&nbsp;
                <asp:TextBox ID="tb_cta_provision" runat="server" Height="16px" Width="150px"></asp:TextBox>
&nbsp;&nbsp;
                <asp:DropDownList ID="lb_ctas" runat="server" Height="22px" Width="75px">
                </asp:DropDownList>
                <br />
                Cuenta Gastos:&nbsp;
                <asp:TextBox ID="tb_cta_gasto" runat="server" Height="16px" Width="150px"></asp:TextBox>
                <br />--%><br />
                Observaciones:
                <asp:TextBox ID="tb_obs" runat="server" Height="16px" Width="370px"></asp:TextBox>
                <br />
                Tipo regimen:
                <asp:DropDownList ID="lb_tiporegimen" runat="server">
                </asp:DropDownList>
                <br />
                <br />
                </td></tr>
            <tr>
            <td>
            <!-- ***************************************************************************** -->
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" 
        Width="722px">
            <div><table>
                <tr><td colspan="2" align="center">Filtrar por</td></tr>
                <tr>
                    <td>Nit:<asp:TextBox ID="tb_nitb" runat="server" Height="16px" Width="131px" /></td><td>
                    Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Cliente" />
            </div>
            <div>
                <asp:GridView ID="gv_proveedor" runat="server" AllowPaging="True" 
                    AutoGenerateSelectButton="True" onload="gv_proveedor_Load" 
                    onpageindexchanging="gv_proveedor_PageIndexChanging" 
                    onselectedindexchanged="gv_proveedor_SelectedIndexChanged" PageSize="1">
                </asp:GridView>
            </div>
            <div>
                &nbsp;&nbsp;
                <asp:Button ID="btnProveedorCancelar" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_id_busqueda"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <!-- ***************************************************************************** -->
            </td>
            </tr>    
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="bt_eliminar" runat="server" Text="Eliminar" 
                                    onclick="bt_eliminar_Click" />
                            </td>
                            <td>
                                <asp:Button ID="bt_guardar" runat="server" Text="Grabar" 
                                    onclick="bt_guardar_Click" />
                            </td>
                            <td>
                                <asp:Button ID="bt_cancelar" runat="server" Text="Cancelar" 
                                    onclick="bt_cancelar_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
</div>
</asp:Content>

