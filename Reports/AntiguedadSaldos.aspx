<%@ Page Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="AntiguedadSaldos.aspx.cs" Inherits="Reports_AntiguedadSaldos" Title="AIMAR - BAW" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="searchresult">
<asp:ScriptManager ID="ScriptManager1" runat="server" >
 <Scripts>
      <asp:ScriptReference Path="~/JS/jquery-1.2.6.min.js" />
      <asp:ScriptReference Path="~/JS/jquery.blockUI.js" />
    </Scripts>
</asp:ScriptManager>
<script language="javascript" type="text/javascript">
    function mpecancela_busqueda_rubro() {

    }
    function mpeCuentaOnCancel() {

    }
    function mpeSeleccionOnCancel() {

    }
</script>
<script  type="text/javascript">
    function BloquearPantalla() {
        $.blockUI({ message: '<h1>Generando...</h1>' });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI();
        });
    }
    </script>
<table width="300" align="center">
<thead>
    <tr><th colspan="2">Reporte de antiguedad de saldos</th></tr>
</thead>
<tbody>

<tr><td>Tipo</td>
    <td>        
    <asp:DropDownList ID="tb_Tipo" runat="server">
           <asp:ListItem Value="1">Saldos Cortados</asp:ListItem>
           <asp:ListItem Value="2">Saldos Completos</asp:ListItem>
    </asp:DropDownList>
    </td>
    

    <td>Fecha Corte:</td>
    <td>
                                
                                <asp:TextBox ID="tb_fechacorte" runat="server" 
            Height="16px"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="tb_fechacorte_MaskedEditExtender" runat="server" 
                                    Enabled="True" Mask="99/99/9999" MaskType="Date" 
                                    TargetControlID="tb_fechacorte">
                                </cc1:MaskedEditExtender>
                                <cc1:CalendarExtender ID="tb_fechacorte_CalendarExtender" runat="server" 
                                    Enabled="True" Format="MM/dd/yyyy" 
            TargetControlID="tb_fechacorte">
                                </cc1:CalendarExtender>
                                
    </td>
</tr>
<tr>
    <td>Tipo Persona</td>
    <td>
        <asp:DropDownList ID="lb_tipopersona" runat="server" AutoPostBack="true" 
            onselectedindexchanged="lb_tipopersona_SelectedIndexChanged">
            <asp:ListItem Value="3">Cliente</asp:ListItem>
            <asp:ListItem Value="4">Proveedor</asp:ListItem>
            <asp:ListItem Value="2">Agente</asp:ListItem>
            <asp:ListItem Value="5">Naviera</asp:ListItem>
            <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
            <asp:ListItem Value="8">Caja Chica</asp:ListItem>
            <asp:ListItem Value="10">Intercompany</asp:ListItem>
        </asp:DropDownList>
    </td>

    <td><asp:Label ID="lbl_coloader" runat="server" Text="Coloader?"></asp:Label></td>
    <td><asp:CheckBox ID="chk_coloader" runat="server" /></td>
</tr>
<tr>
    <td>  <asp:Label ID="l_contabilidad" runat="server" Text="Tipo Contabilidad"></asp:Label></td>
    
    <td>
        <asp:DropDownList ID="lb_contabilidad" runat="server">
        </asp:DropDownList>
    </td>

    <td>Moneda</td>
    <td>
        <asp:DropDownList ID="lb_moneda" runat="server">
        </asp:DropDownList>
    </td>
</tr>
<tr>
<td>
    <asp:Label ID="lb_consolidar_moneda" runat="server" Text="Consolidar moneda?" Visible="false"></asp:Label></td>
<td>
    <asp:CheckBox ID="chk_consolidar_moneda" runat="server" Visible="false"/></td>
</tr>
<tr>
    <td>Codigo: </td>
    <td>
        <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>Nombre: </td>
    <td><asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" Width="377px"></asp:TextBox></td>
</tr>

<tr><td>Credito</td>
    <td>        
    <asp:DropDownList ID="cb_credito" runat="server">
           <asp:ListItem Value="1">Todos</asp:ListItem>
           <asp:ListItem Value="2">Credito</asp:ListItem>
           <asp:ListItem Value="3">Contado</asp:ListItem>
    </asp:DropDownList>
    </td>
    
</tr>
<tr><td>Incluir Saldos A Favor</td>
    <td>
        <asp:CheckBox ID="rb_recibos_ant" runat="server" />
    </td>

   <td>Incluir Dias de Credito</td>
    <td>
        <asp:CheckBox ID="rb_dias_credito" runat="server" />
    </td>
</tr>
<tr><td>Cobrador</td>
    <td>
        <asp:DropDownList ID="lb_cobrador" runat="server">
        </asp:DropDownList>
    </td>

<!--<tr>
    <td>Ordenar por</td>
    <td>
         <asp:DropDownList ID="lb_orderby" runat="server">
            <asp:ListItem Value="codigocliente">Codigo Cliente</asp:ListItem>
            <asp:ListItem Value="nombrecliente">Nombre Cliente</asp:ListItem>            
        </asp:DropDownList> 
    </td>
</tr> AutoPostBack="true" onselectedindexchanged="lb_sucursal_SelectedIndexChanged"-->
<!--Filtros APL: Pablo Aguilar-->

    <td>Sucursal</td>
    <td>
        <asp:CheckBoxList ID="lb_sucursal" runat="server">
        </asp:CheckBoxList>
    </td>
</tr>
<tr>
    <td>
        <asp:Label ID="lblSerieFacturas" runat="server" Text="Serie Facturas" Visible="false"></asp:Label></td>
    <td>
        <asp:DropDownList ID="lb_facturas" runat="server" Visible="false">
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td>
        <asp:Label ID="lblSerieRecibos" runat="server" Text="Serie Recibos" Visible="false"></asp:Label></td>
    <td>
        <asp:DropDownList ID="lb_recibos" runat="server"  Visible="false">
        </asp:DropDownList>
    </td>
</tr>
<!--Fin Filtros-->
<tr>
    <td colspan="3" align="center">
        <asp:Button ID="bt_generar" runat="server" onclick="bt_generar_Click" Text="Generar"  OnClientClick="javascript:BloquearPantalla();" UseSubmitBehavior="false" />
        <asp:Button ID="btn_impresion" runat="server" onclick="btn_impresion_Click" 
            Text="Generar Impresion" Visible="False" />
        <asp:Button ID="bt_generar_anti" runat="server" Text="Generar Reporte" 
            onclick="bt_generar_anti_Click" />
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
</tbody>
</table>
<table align="center">
<tr><td>    
    <asp:GridView ID="gv_antiguedadsaldos" runat="server" 
        onrowcommand="gv_antiguedadsaldos_RowCommand" Width="700px" 
        onrowdatabound="gv_antiguedadsaldos_RowDataBound" onrowcreated="gv_antiguedadsaldos_RowDataBound" 
        >
        <Columns>
            <asp:ButtonField ButtonType="Button" Text="Seleccionar" InsertVisible=true  />
        </Columns>
    </asp:GridView>    
    </td></tr>
    <tr><td>Detalle de cliente</td></tr>
    <tr><td>
        <asp:GridView ID="gv_detallecliente" runat="server" 
            ondatabound="gv_detallecliente_DataBound">
        </asp:GridView>
        <asp:Label ID="SumTotal" runat="server" Visible="False"></asp:Label>
&nbsp;<asp:Label ID="SumAbono" runat="server" Visible="False"></asp:Label>
&nbsp;<asp:Label ID="SumAfvr" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="SumSaldo" runat="server" Visible="False"></asp:Label>
        </td></tr>
        <tr><td>Nomenclatura: NCD=nota credito a Nota debito; NC= nota credito; PV=provision; CH=Cheques;TR=Transferencias; ND=nota debito; RC=recibo SOA</td></tr>
</table>
</fieldset>
<!--<a href="AntiguedadSaldos.aspx.cs">AntiguedadSaldos.aspx.cs</a>-->
</div> 

</asp:Content>

