<%@ Page Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="profit2.aspx.cs" Inherits="Reports_solicitaReport" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">PROFIT DETALLADO</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<script language="javascript" type="text/javascript">   
    function mpecancela_busqueda_rubro()
    {

    } 
    function mpeCuentaOnCancel()
    {

    } 
    function mpeSeleccionOnCancel()
    {

    }
    </script>
<table width="650">
    <tr>
        <td>
            <table width="300" align="center">
            <tr>
                <td>Fecha Inicial:</td>
                <td>
                    <asp:TextBox ID="tb_fechainicial" runat="server" Height="16px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fechainicial_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                        TargetControlID="tb_fechainicial">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fechainicial_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechainicial" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>Fecha Final:</td>
                <td>
                    <asp:TextBox ID="tb_fechafinal" runat="server" Height="16px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="tb_fechafinal_MaskedEditExtender" runat="server" 
                        Mask="99/99/9999" MaskType="Date" Enabled="True" 
                        TargetControlID="tb_fechafinal">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="tb_fechafinal_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="tb_fechafinal" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
            </tr>
             <tr>
                <td> <asp:Label ID="l_contabilidad" runat="server" Text="Tipo Contabilidad"></asp:Label></td>
                <td>
                        <asp:DropDownList ID="lb_contabilidad" runat="server">
                            
                        </asp:DropDownList>
                </td>
           </tr>
            <tr>
                <td>Moneda:</td>
                <td>
                    <asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lb_mbl" runat="server" Visible="true">MBL</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Visible="true"></asp:TextBox>
                    <cc1:ModalPopupExtender ID="tb_mbl_ModalPopupExtender" runat="server" TargetControlID="tb_mbl"
                        PopupControlID="pnlMBL" CancelControlID="btnCuentaCancelar2"
                        OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
                        BackgroundCssClass="FondoAplicacion" />
                    
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="Panel1" runat="server" Visible="False">
                        <asp:TextBox ID="tb_hbl" runat="server" Height="16px"></asp:TextBox>
                        <cc1:ModalPopupExtender ID="tb_cuenta2_ModalPopupExtender" 
    runat="server" TargetControlID="tb_hbl"
            PopupControlID="pnlCuentas2" CancelControlID="btnCuentaCancelar2"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    CONTENEDOR:
                </td>
                <td>
                    <asp:TextBox ID="tb_contenedor" runat="server" Enabled="false" Height="16px" Visible="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    ROUTING:
                </td>
                <td>
                    <asp:TextBox ID="tb_routing" runat="server" Enabled="false" Height="16px" Visible="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Tipo Persona:</td>
                <td>
                    <asp:DropDownList ID="lb_tipopersona" runat="server">
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Codigo :                 </td>
                <td>
                    <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" Width="115px"></asp:TextBox>
                    &nbsp;<asp:Label ID="lb_fecha_hora" runat="server" Visible="False"></asp:Label>
                    
                 </td>
            </tr>
            <tr>
                <td>
                    Nombre : 
                </td>
                <td>
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" Width="377px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btn_generar" runat="server" Text="Generar" 
                        onclick="btn_generar_Click" />
                </td>
            </tr>
            </table>
            <!--   
            -->
        </td>
    </tr>
    <tr>
    <td>
    <!-- ***************************************************************************** -->
    <asp:Panel ID="pnlProveedor" runat="server" CssClass="CajaDialogo" 
        Width="800px" style="display:none">
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
                    <td><asp:Button ID="bt_buscar" runat="server" Text="Buscar" onclick="bt_buscar_Click" /></td>
                </tr>
            </table></div>
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
        <cc1:ModalPopupExtender ID="mpeSeleccion" runat="server" TargetControlID="tb_agenteID"
            PopupControlID="pnlProveedor" CancelControlID="btnProveedorCancelar"
            OnCancelScript="mpeSeleccionOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <!-- ***************************************************************************** -->
    </td>
    </tr>
</table>
</fieldset>
</div>

<%--********************************************************--%>
    <div>
    <asp:Panel ID="pnlMBL" runat="server" CssClass="CajaDialogo" Width="600px" Style=" display:none">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label12" runat="server" Text="Seleccionar Cuenta" />
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_criterio2" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="lb_tipo2" runat="server">
                    <asp:ListItem>LCL</asp:ListItem>
                    <asp:ListItem>FCL</asp:ListItem>
                    <asp:ListItem>ALMACENADORA</asp:ListItem>
                    <asp:ListItem>AEREO</asp:ListItem>
                    <asp:ListItem>TERRESTRE T</asp:ListItem>
                    <asp:ListItem>ADUANA</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="dgw12" runat="server" AllowPaging="True" 
                    onpageindexchanging="dgw12_PageIndexChanging" onrowcommand="dgw12_RowCommand" 
                    PageSize="10">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar2" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bt_search2" runat="server" Text="Buscar" 
                            onclick="bt_search2_Click" />
            </td>
            <td><asp:Button ID="btnCuentaCancelar2" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    </div>
    <%--********************************************************--%>
    <asp:Panel ID="pnlCuentas2" runat="server" CssClass="CajaDialogo" Width="600px" style="display:none;">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label1" runat="server" Text="Seleccionar Cuenta" />
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_criterio3" runat="server" Height="16px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="lb_tipo3" runat="server">
                    <asp:ListItem>LCL</asp:ListItem>
                    <asp:ListItem>FCL</asp:ListItem>
                    <asp:ListItem>ALMACENADORA</asp:ListItem>
                    <asp:ListItem>AEREO</asp:ListItem>
                    <asp:ListItem>TERRESTRE T</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="dgw13" runat="server" AllowPaging="True" 
                    onpageindexchanging="dgw13_PageIndexChanging" onrowcommand="dgw13_RowCommand" 
                    PageSize="10">
                    <Columns>
                        <asp:ButtonField CommandName="Seleccionar3" HeaderText="Seleccionar" 
                            Text="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bt_search3" runat="server" Text="Buscar" 
                            onclick="bt_search3_Click" />
            </td>
            <td><asp:Button ID="btnCuentaCancelar3" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
<%--********************************************************--%>
    </asp:Content>

