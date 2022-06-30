<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="saldos_transmision.aspx.cs" Inherits="operations_movimiento_contable" Title="AIMAR - BAW" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {
        var stb_cuenta = document.getElementById("ctl00_Contenido_tb_cuenta");
        var stb_cta_nombre= document.getElementById("ctl00_Contenido_tb_cta_nombre");
        stb_cuenta.value = "";
        stb_cta_nombre.value="";
    }


    </script>

    <style>

.menu
/* layout main menu */
 {
    clear:both;
    float:left;
    margin-top:1em;
    padding-bottom:1em;
    width:100%;
    background-color:gimgrey;
    background-image:url('/templates/swimming/images/logo.png');
    background-repeat:no-repeat;
}
ul.menu {
    display: table;
    margin-left: auto;
    margin-right: 0em;
}
.menu li
/* horizontal menu layout */
 {
    display: inline-block;
    border: 3px solid #FFFFFF;
    border-radius: 10px;
    margin-right:30px;
    background-color: #4180dd;
}
.menu li a:hover
/* link style on-mouse-over */
 {
    color:#FFFFFF;
    text-transform: uppercase;
    text-decoration: none;
}
.menu li:hover
/* button style on-mouse-over */
 {
    border: 3px solid #FFFFFF;
    border-radius: 10px;
    background-color: orange;
}
.menu li a
/* link style */
 {
    color:#FFFFFF;
    font-size: 1em;
    text-transform: uppercase;
    text-decoration: none;
    display: inline-block;
    padding: 0.7em 1em 0.7em 1em;
}
.menu li.active a
/* active menu item style */
 {
    color:#FFFFFF;
    text-transform: uppercase;
    text-decoration: none;
}
.menu li.active
/* active menu button style */
 {
    border: 3px solid #FFFFFF;
    border-radius: 10px;
    background-color: orange;
}






.example_a {
color: #fff !important;
text-transform: uppercase;
text-decoration: none;
background: silver;
padding: 10px;
border-radius: 5px;
display: inline-block;
border: none;
transition: all 0.4s ease 0s;
}

.example_a:hover {
background: #434343;
letter-spacing: 1px;
-webkit-box-shadow: 0px 5px 40px -10px rgba(0,0,0,0.57);
-moz-box-shadow: 0px 5px 40px -10px rgba(0,0,0,0.57);
box-shadow: 5px 40px -10px rgba(0,0,0,0.57);
transition: all 0.4s ease 0s;
}
    </style>


    <div id="box">

        <div>    
                <ul class="nav menu">
                <li class="item-101 <%=op == "1" ? " current active " : ""%> parent"><a href="saldos_transmision.aspx?op=1" >Saldos Registrados</a></li>
                <li class="item-102 <%=op == "3" ? " current active " : ""%>parent"><a href="saldos_transmision.aspx?op=3" >Saldos Transmitidos</a></li>
                <li class="item-103 <%=op == "4" ? " current active " : ""%>parent"><a href="saldos_transmision.aspx?op=4" >Saldos Pendientes de Enviar</a></li>
                </ul>

                <br /><br />
        </div>

        <center><h1 id="adduser"><%=titulo.ToUpper()%></h1></center>

	<table width="99%">
        <tr>
            <td>

                <asp:Label ID="l_contabilidad" runat="server" Font-Bold="True" 
                    Text="Tipo Contabilidad"></asp:Label>
                &nbsp;<asp:DropDownList ID="lb_contabilidad" runat="server">
                </asp:DropDownList>
                <br />
                <br />

                <label>Cuenta Contable : </label> 
                <asp:TextBox ID="tb_cuenta" runat="server" Height="16px" ReadOnly="True"></asp:TextBox>
                <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" 
                    BackgroundCssClass="FondoAplicacion" CancelControlID="btnCuentaCancelar" 
                    DropShadow="True" OnCancelScript="mpeCuentaOnCancel()" 
                    PopupControlID="pnlCuentas" TargetControlID="tb_cuenta" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tb_cuenta_nombre" runat="server" Height="16px" ReadOnly="True" 
                    Width="300px"></asp:TextBox>
                <br />
                <br />
                
                <label>Fecha de Cuenta : </label> 
                <asp:TextBox ID="tb_fecha_cuenta" runat="server" Height="16px"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_cuenta_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_cuenta">
                </cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="tb_fecha_cuenta_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_cuenta">
                </cc1:MaskedEditExtender>
                
                <label>Fecha de Saldo : </label> 
                <asp:TextBox ID="tb_fecha_saldo" runat="server" Height="16px"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_saldo_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_saldo">
                </cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="tb_fecha_saldo_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_saldo">
                </cc1:MaskedEditExtender>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />

                <% if (op == "3") { %>

                <label>Fecha Transmision : </label> 
                <asp:TextBox ID="tb_fecha_trans" runat="server" Height="16px"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_trans">
                </cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_trans">
                </cc1:MaskedEditExtender>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />

               <% } %>


                <% if (op == "9") { %>

                <label>Transmision : </label>
                <asp:DropDownList ID="lb_tipo" runat="server">
                <asp:ListItem Value="1" Text="Enviados"></asp:ListItem>   
                <asp:ListItem Value="2" Text="Fallidos"></asp:ListItem>              
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />

                <% } %>

                    <table width=100%>  
                       <tr>

                        <td>   
                        <asp:Label ID="Label3" runat="server" Text="" ></asp:Label>                                    
                        </td>

                        <td>
                        <div class="button_cont" align="center">
                        <asp:LinkButton ID="LinkButton2" runat="server" class="example_a" rel="nofollow noopener" onclick="bt_visualizar_Click">CONSULTAR </asp:LinkButton>                                                          
                        </div>
                        </td>

                        <td>  
                        <asp:LinkButton ID="LinkButton1" runat="server" class="example_a" rel="nofollow noopener"  OnClick="ButtonExcel_Click" Visible="false">EXCEL </asp:LinkButton>                                                    
                        </td>
                    </tr>
                </table>  

                <br />
                <br />

                
            </td>
        </tr>
    </table>   


    <fieldset id="searchresult">
        <legend></legend>

        <div id="Div1">

            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>

        </div>

    </fieldset>

    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" style=" display:none" 
                    Width="533px">
                    <div>
                        <table>
                            <caption>
                                &nbsp;&nbsp;Filtrar por:
                                <tr>
                                    <td>
                                        Tipo:</td>
                                    <td>
                                        <asp:DropDownList ID="lb_clasificacion" runat="server">
                                            <asp:ListItem Value="0">Todas</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="1">Activo</asp:ListItem>
                                            <asp:ListItem Value="2">Pasivo</asp:ListItem>
                                            <asp:ListItem Value="3">Venta(Ingresos)</asp:ListItem>
                                            <asp:ListItem Value="4">Gastos(Egresos)</asp:ListItem>
                                            <asp:ListItem Value="5">Capital</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </caption>
                            <tr>
                                <td>
                                    Nombre:</td>
                                <td>
                                    <asp:TextBox ID="tb_nombre_cta" runat="server" Height="16px" Width="293px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cuenta:</td>
                                <td>
                                    <asp:TextBox ID="tb_cuenta_numero" runat="server" Height="16px" Width="293px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="bt_buscar_cta" runat="server" onclick="bt_buscar_cta_Click" 
                                        Text="Buscar" />
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
                            onselectedindexchanged="gv_cuenta_SelectedIndexChanged" PageSize="15" 
                            Width="480px">
                        </asp:GridView>
                    </div>
                    <div>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" />
                    </div>
                </asp:Panel>
</div>
    
<script language="javascript" type="text/javascript" for="window" event="onclick">
// <![CDATA[
return window_onclick()
// ]]>
</script>
</asp:Content>

