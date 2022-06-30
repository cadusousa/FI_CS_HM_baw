<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addprov.aspx.cs" Inherits="operations_Default" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">PROVEEDORES</h3>

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
                Busqueda de proveedor:<asp:TextBox ID="tb_id_busqueda" runat="server" 
                    Height="16px" Width="89px"></asp:TextBox>
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
                Descripcion:
                <asp:TextBox ID="tb_descripcion" runat="server" Height="16px" Width="317px"></asp:TextBox>
                <br />
                Status:
                <asp:DropDownList ID="lb_estado" runat="server">
                    <asp:ListItem Value="-1">Borrado</asp:ListItem>
                    <asp:ListItem Value="0">Ingreso</asp:ListItem>
                    <asp:ListItem Value="1">Verificado</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Bienes: <asp:RadioButtonList ID="rradioprovde" runat="server" 
                    RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Value="0">Servicios</asp:ListItem>
                    <asp:ListItem Value="1">Bienes</asp:ListItem>
                    <asp:ListItem Value="2">Importacion</asp:ListItem>
                    <asp:ListItem Value="3">Exportacion</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                Tipo persona: <asp:RadioButtonList ID="rb_tipopersona" runat="server" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem>Natural</asp:ListItem>
                    <asp:ListItem Value="Juridica">Jurídica</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                Clasificación:&nbsp;<asp:RadioButtonList ID="radioclasificacion" runat="server" 
                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="0">Local</asp:ListItem>
                    <asp:ListItem Value="1">Extranjero</asp:ListItem>
                </asp:RadioButtonList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chk_fovial" runat="server" Text="Fovial: " TextAlign="Left" />
                <br />
                Dirección:&nbsp;                 <asp:TextBox ID="tb_direccion" runat="server" Height="16px" Width="356px"></asp:TextBox>
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                Teléfono:&nbsp;                <asp:TextBox ID="tb_telefono" runat="server" Height="16px" Width="110px"></asp:TextBox>
                &nbsp;&nbsp; Fax:&nbsp;
                <asp:TextBox ID="tb_fax" runat="server" Height="16px" Width="110px"></asp:TextBox>
                &nbsp; Correo electrónico:&nbsp;
                <asp:TextBox ID="tb_correo" runat="server" Height="16px" Width="200px"></asp:TextBox>
                <br />
                Contacto:&nbsp;
                <asp:TextBox ID="tb_contacto" runat="server" Height="16px" Width="350px"></asp:TextBox>
                <br />
                <%--Provisiona en:&nbsp;
                <asp:TextBox ID="tb_cta_provision" runat="server" Height="16px" Width="150px"></asp:TextBox>
&nbsp;&nbsp;
                <asp:DropDownList ID="lb_ctas" runat="server" Height="22px" Width="75px">
                </asp:DropDownList>
                <br />
                Cuenta Gastos:&nbsp;
                <asp:TextBox ID="tb_cta_gasto" runat="server" Height="16px" Width="150px"></asp:TextBox>
                <br />--%>
                Observaciones:
                <asp:TextBox ID="tb_obs" runat="server" Height="16px" Width="370px"></asp:TextBox>
                <br />
                Dias:
                <asp:TextBox ID="tb_dias" runat="server" Height="16px" Width="40px">0</asp:TextBox>
                &nbsp;Tipo regimen:
                <asp:DropDownList ID="lb_tiporegimen" runat="server">
                </asp:DropDownList>
                <br />
                Monto:                 <asp:TextBox ID="tb_monto" runat="server" Height="16px" Width="80px"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chk_ordencompra" runat="server" 
                    Text="Requiere orden de compra:" TextAlign="Left" />
                <br />
                Tipo de retencion:<br />
                <br />
                <asp:CheckBoxList ID="chklist_retencion" runat="server" RepeatColumns="4" 
                    RepeatDirection="Horizontal">
                </asp:CheckBoxList>
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
                    <td>Nit:<asp:TextBox ID="tb_nitb" runat="server" Height="16px" Width="131px" /></td>
                </tr>
                <tr>
                    <td>Nombre:<asp:TextBox ID="tb_nombreb" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td>Codigo:<asp:TextBox ID="tb_codigo" runat="server" Height="16px" Width="293px" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table></div>
            <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
                <asp:Label ID="Label4" runat="server" Text="Seleccionar Proveedor" />
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

