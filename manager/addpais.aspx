<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addpais.aspx.cs" Inherits="manager_addpais" Title="AIMAR - BAW" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
    <h3 id="adduser">PAISES</h3>

        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <script language="javascript" type="text/javascript">   
    function OnCancel()
    {
        
    }  
    </script>
    <fieldset id="personal">
        <legend>Información del pais</legend>
        <table width="650">
        <tr><td>
            Pais ID:<asp:TextBox ID="tb_paisid" Enabled="false" runat="server" Height="16px" Width="85px"></asp:TextBox><br />
            Nombre: <asp:TextBox ID="tb_nombre" runat="server" Height="16px" Width="220px"></asp:TextBox>
            &nbsp;Impuesto %:<asp:TextBox 
                ID="tb_impuesto" runat="server" Height="16px" Width="128px"></asp:TextBox>
                <asp:TextBox ID="tbhelp" runat="server" ReadOnly="True" Height="16px" 
                Width="16px"></asp:TextBox>
                &nbsp;<asp:CheckBox ID="chk_nit" runat="server" Text="NIT Obligatorio" />
                <br />
            <br />
            Tiempo recuperacion de impuesto:<asp:TextBox ID="tb_recuperacion_iva" 
                runat="server" Height="16px" Width="40px"></asp:TextBox>
            <br />
            (para el caso de nota de credito ingresar cantidad de dias calendario)<br />
            <br />
            Dias de holgura:
            <asp:TextBox ID="tb_diasholgura" 
                runat="server" Height="16px" Width="40px"></asp:TextBox>
            <br />
            (Dias adicionales para el calculo de interes y/o mora)<br />
            <br />
            Porcentaje de interes:<asp:TextBox ID="tb_porc_interes" 
                runat="server" Height="16px" Width="70px"></asp:TextBox>
            <br />
            (Ingresar entero entre 1 y 100 CLIENTES)<br />
            <br />
            Momento&nbsp; de realizar la retencion:<asp:DropDownList ID="lb_momentoret" 
                runat="server">
                <asp:ListItem Value="1">Recepcion de Factura</asp:ListItem>
                <asp:ListItem Value="2">Generacion de pago (cheque/transferencia)</asp:ListItem>
            </asp:DropDownList>
            <br />
            (Indica en que momento se realizara la retencion)<br />
            <br />
            Listado de correos para dar aviso de limites de credito :<asp:TextBox 
                ID="tb_lista_correos" runat="server" Height="56px" TextMode="MultiLine" 
                Width="304px"></asp:TextBox>
            <br />
            Separados por coma y sin espacios(correo1@aimar.com,correo2@aimar.com,etc)<br />
            <br />
                <br />
            Cuenta asociada (Cuenta x Cobrar):<asp:TextBox ID="tb_ctaCobraID" runat="server" Height="16px" Width="85px" ></asp:TextBox>
                <asp:TextBox ID="tb_ctaCobraNombre" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Buscar" 
                onclick="Button1_Click" />
                <br />
            Cuenta "bolson" donde caeran todas las facturas<br />
            Cuenta asociada (Cuentas x Pagar):<asp:TextBox ID="tb_ctaPagaID" runat="server" Height="16px" Width="85px" ></asp:TextBox>
                <asp:TextBox ID="tb_ctaPagaNombre" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="Buscar" 
                onclick="Button2_Click" /><br />
            Cuenta asociada (Iva x Pagar):<asp:TextBox ID="tb_ivaPagaID" runat="server" Height="16px" Width="85px" ></asp:TextBox>
                <asp:TextBox ID="tb_ivaPagaNombre" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button3" runat="server" Text="Buscar" 
                onclick="Button3_Click" /><br />
            Cuenta asociada (Iva x Cobrar):<asp:TextBox ID="tb_ivaCobraID" runat="server" Height="16px" Width="85px" ></asp:TextBox>
                <asp:TextBox ID="tb_ivaCobraNombre" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button4" runat="server" Text="Buscar" 
                onclick="Button4_Click" /><br />
            Cuenta asociada recibos:<asp:TextBox ID="tb_reciboID" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_reciboNombre" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button5" runat="server" Text="Buscar" 
                onclick="Button5_Click" /><br />
            Cuenta asociada anticipo clientes:<asp:TextBox ID="tb_cta_anticipoCli_ID" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_anticipoCli_nombre" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button6" runat="server" Text="Buscar" 
                onclick="Button6_Click" /><br />
            Cuenta asociada diferencial cambiario:<asp:TextBox ID="tb_dif_cambiarioID" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_dif_cambiario_nombre" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button15" runat="server" Text="Buscar" 
                onclick="Button15_Click" /><br />
            Cuenta asociada INTERCOMPANY GT:<asp:TextBox ID="tb_cta_intercompanyID" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_intercompany_nombre" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button7" runat="server" Text="Buscar" 
                onclick="Button7_Click" /><br />
            Cuenta asociada INTERCOMPANY SV:<asp:TextBox ID="tb_cta_intercompanyID_sv" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_intercompany_sv" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button8" runat="server" Text="Buscar" 
                onclick="Button8_Click" /><br />
                Cuenta asociada INTERCOMPANY HN:<asp:TextBox ID="tb_cta_intercompanyID_hn" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_intercompany_hn" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button9" runat="server" Text="Buscar" 
                onclick="Button9_Click" /><br />
                Cuenta asociada INTERCOMPANY NI:<asp:TextBox ID="tb_cta_intercompanyID_ni" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_intercompany_ni" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button10" runat="server" Text="Buscar" 
                onclick="Button10_Click" /><br />
                Cuenta asociada INTERCOMPANY CR:<asp:TextBox ID="tb_cta_intercompanyID_cr" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_intercompany_cr" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button11" runat="server" Text="Buscar" 
                onclick="Button11_Click" /><br />
                Cuenta asociada INTERCOMPANY PA:<asp:TextBox ID="tb_cta_intercompanyID_pa" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_intercompany_pa" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button12" runat="server" Text="Buscar" 
                onclick="Button12_Click" /><br />
                Cuenta asociada INTERCOMPANY BZ:<asp:TextBox ID="tb_cta_intercompanyID_bz" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_intercompany_bz" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button13" runat="server" Text="Buscar" 
                onclick="Button13_Click" /><br />
                Cuenta asociada INTERCOMPANY GRH:<asp:TextBox ID="tb_cta_intercompanyID_grh" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_intercompany_grh" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button14" runat="server" Text="Buscar" 
                onclick="Button14_Click" /><br />
                Cuenta asociada INTERCOMPANY GRH:<asp:TextBox ID="tb_cta_intercompanyID_mayanL" runat="server" Height="16px" Width="85px"></asp:TextBox>
                <asp:TextBox ID="tb_cta_intercompany_mayanL" runat="server" Height="16px" Width="220px"></asp:TextBox>
                <asp:Button ID="Button16" runat="server" Text="Buscar" 
                onclick="Button16_Click" /><br />
        </td></tr>
        
        <tr><td align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" onclick="bt_Enviar_Click" />&nbsp;&nbsp;
            <input id="Cancel" type="button" value="Cancelar" onclick="javascript:document.location.href='searchpais.aspx';" />
        </td></tr>
        </table>
        <!--- inicia modulo --->
        <% if ((tipocambio_arr != null) && (tipocambio_arr.Count > 0)) { %>
        <table width="400" align="center">
            <thead>
            <tr><th colspan="3" align="center">Ultimos 10 tipos de cambio</th></tr>
	        <tr>
	            <th>Fecha</th>
    	        <th>Tipo de cambio</th>
    	        <th>Usuario</th>
            </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean tcbean=null;
                for (int i = 0; i < tipocambio_arr.Count; i++) {
                    tcbean = (RE_GenericBean)tipocambio_arr[i];
            %>
	            <tr>
                    <td><%=tcbean.strC1%></td>
                    <td><%=tcbean.decC1 %></td>
                    <td><%=tcbean.strC2 %></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="#" onclick="javascript:window.open('tipocambio.aspx?id=<%=pai_id %>', null, 'toolbar=no,resizable=no,status=no,width=400,height=280');">Agregar un nuevo tipo de cambio</a></td>
                </tr>
            </tbody>
        </table>
        <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de haber agregado un tipo de cambio a este pais.</th>
                </tr>
                <tr>
                    <td align="center"><a href="#" onclick="javascript:window.open('tipocambio.aspx?id=<%=pai_id %>', null, 'toolbar=no,resizable=no,status=no,width=400,height=280');">Agregar un nuevo tipo de cambio</a></td>                    
                </tr>
            </thead>
        </table>
        <% } %>
        <!--- finaliza modulo --->
        <!--- inicia modulo --->
    
        <% if ((matrizopr_arr != null) && (matrizopr_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
                <tr><th colspan="8">MATRIZ DE OPERACION PARA RUBROS</th></tr>
	            <tr>
	                <th>Transaccion</th>
	                <th>Servicio</th>
	                <th>Contribuyente</th>
	                <th>Moneda</th>
	                <th>Pago</th>
	                <th>Registro</th>
	                <th>Origen</th>
	                <th>Accion</th>
                </tr>
            </thead>
            <tbody>
            <%  
                foreach (RE_GenericBean mat_arr in matrizopr_arr)
                {                    
            %>
	            <tr>
                    <td><%=mat_arr.strC1 %></td>
                    <td><%=mat_arr.strC2 %></td>
                    <td><%=mat_arr.strC3 %></td>
                    <td><%=mat_arr.strC4 %></td>
                    <td><%=mat_arr.strC5 %></td>
                    <td><%=mat_arr.strC6 %></td>
                    <td><%=mat_arr.strC7 %></td>
                    <td><a href="matriz.aspx?pai_id=<%=pai_id %>&trans=<%=mat_arr.strC1 %>&serv=<%=mat_arr.strC2 %>&contribuyente=<%=mat_arr.strC3 %>&moneda=<%=mat_arr.strC4 %>&cobro=<%=mat_arr.strC5 %>&conta=<%=mat_arr.strC6 %>&impo=<%=mat_arr.strC7 %>"><img src="../img/icons/page_white_edit.png"  alt="Editar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="8" align="center"><a href="matriz.aspx?id=0&pai_id=<%=pai_id %>">Agregar una combinacion de cobro</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro para este pais.</th>
                </tr>
                <tr>
                    <td align="center"><a href="matriz.aspx?id=0&pai_id=<%=pai_id %>">Agregar una combinacion de cobro</a></td>
                </tr>
            </thead>
        </table>
    <% } %>

<%--******************************************************************************--%>  
        <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="500px">
        <div>
        <table>
            <caption>
                &nbsp;&nbsp;Filtrar por:
            <tr>
            <td>
                Tipo:<asp:DropDownList ID="lb_clasificacion" runat="server">
                    <asp:ListItem Value="0">Todas</asp:ListItem>
                    <asp:ListItem Selected="True" Value="1">Activo</asp:ListItem>
                    <asp:ListItem Value="2">Pasivo</asp:ListItem>
                    <asp:ListItem Value="3">Venta(Ingresos)</asp:ListItem>
                    <asp:ListItem Value="4">Gastos(Egresos)</asp:ListItem>
                    <asp:ListItem Value="5">Capital</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                Nombre:<asp:TextBox ID="tb_nombre_cta" runat="server" Height="16px" 
                    Width="268px" />
            </td>
            </tr>
            </caption>
            <tr>
            <td colspan="2">
                <asp:Button ID="bt_buscar_cta" runat="server" Text="Buscar" onclick="bt_buscar_cta_Click" />
            </td>
            </tr>
        </table>
        </div>
        <div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label1" runat="server" Text="Seleccionar Cuenta" />
        </div>
        <div>
            <asp:GridView ID="gv_cuenta" runat="server" AllowPaging="True" 
                AutoGenerateSelectButton="True" EmptyDataText="No se encontraron referencias" 
                GridLines="None" onload="gv_cuenta_Load" 
                onpageindexchanging="gv_cuenta_PageIndexChanging" 
                onselectedindexchanged="gv_cuenta_SelectedIndexChanged" Width="480px" 
                PageSize="15">
            </asp:GridView>
        </div>
        <div>
            &nbsp;&nbsp;<asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" />
        </div>
        </asp:Panel>
        
        <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="tbhelp"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="OnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
        </fieldset>
</div>

</asp:Content>

