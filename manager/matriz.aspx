<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="matriz.aspx.cs" Inherits="manager_matriz" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
	<h3 id="adduser">Combinaciones</h3>
	<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <table width="100%">
    <tr><td>
        <asp:Label ID="lb_msg" runat="server" Font-Bold="True" Font-Underline="True" 
            ForeColor="#FF3300" Text="Label" Visible="False"></asp:Label>
        <asp:Label ID="lb_ref_id" runat="server" Text="Label" Visible="False"></asp:Label>
        <br />
        <label for="lastname">* Transacción: </label> 
        <asp:DropDownList ID="lb_trans" runat="server">
          </asp:DropDownList>
          <asp:Label ID="lb_paid" runat="server" Visible="False"></asp:Label>
        <br />
        <%--<label for="lastname">* Tipo operación: </label> 
        <asp:DropDownList ID="lb_operacion" runat="server">
        </asp:DropDownList>
        <br /><br />--%>
        <label for="lastname">* Tipo de servicio: </label> 
        <asp:DropDownList ID="lb_servicio" runat="server">
        </asp:DropDownList>
          <input id="Button1" type="button" value="Agregar" onclick="javascript:window.open('pop_addItem.aspx?id=Servicio', null, 'toolbar=no,resizable=no,status=no,width=400,height=250');"/><br />  
        <label for="lastname">* Contribuyente: </label>
        <asp:DropDownList ID="lb_contribuyente" runat="server">
        </asp:DropDownList>
        <input id="Button2" type="button" value="Agregar" onclick="javascript:window.open('pop_addItem.aspx?id=Contribuyente', null, 'toolbar=no,resizable=no,status=no,width=400,height=250');"/>
        <br />
        <label for="lastname">* Moneda: </label>
        <asp:DropDownList ID="lb_moneda" runat="server">
        </asp:DropDownList>
        <input id="Button3" type="button" value="Agregar" onclick="javascript:window.open('pop_addItem.aspx?id=Moneda', null, 'toolbar=no,resizable=no,status=no,width=400,height=250');"/>
        <br />
        <label for="lastname">* Tipo cobro: </label>
        <asp:DropDownList ID="lb_cobro" runat="server">
        </asp:DropDownList>
        <input id="Button4" type="button" value="Agregar" onclick="javascript:window.open('pop_addItem.aspx?id=Cobro', null, 'toolbar=no,resizable=no,status=no,width=400,height=250');"/>
        <br />
        
        <label for="lastname">* Contabilidad: </label>
        <asp:DropDownList ID="lb_conta" runat="server">
        </asp:DropDownList>
        <input id="Button5" type="button" value="Agregar" onclick="javascript:window.open('pop_addItem.aspx?id=Conta', null, 'toolbar=no,resizable=no,status=no,width=400,height=250');"/>
        <br />
        
        <label for="lastname">* Importación / Exportación: </label>
        <asp:DropDownList ID="lb_imp_exp" runat="server">
        </asp:DropDownList>
        <input id="Button6" type="button" value="Agregar" onclick="javascript:window.open('pop_addItem.aspx?id=ImpExp', null, 'toolbar=no,resizable=no,status=no,width=400,height=250');"/>
          <br />
          <br />
          <asp:CheckBoxList ID="chk_list_rubros" runat="server" RepeatColumns="5">
          </asp:CheckBoxList>
    </td></tr>
    <tr><td style="background-color:#99FFCC">Cuenta:<asp:TextBox ID="tb_cuenta" runat="server" Height="16px" 
            Width="85px" ReadOnly="True"></asp:TextBox>
        Descripcion:<asp:TextBox ID="tb_cta_nombre" runat="server" Height="16px" 
            Width="220px" ReadOnly="True"></asp:TextBox>

        <div align="right">
        <asp:Button ID="bt_agregar" runat="server" Text="Agregar" 
                onclick="bt_agregar_Click" />
        </div>
        <br />
    </td></tr>
    <tr><td align="center">
    
        <asp:GridView ID="gv_conf_cuentas" runat="server" 
            onrowdeleting="gv_conf_cuentas_RowDeleting" Width="100%" 
            AutoGenerateColumns="False" onrowdatabound="gv_conf_cuentas_RowDataBound">
            <Columns>
                <asp:CommandField ButtonType="Button" DeleteText="Eliminar" 
                    HeaderText="Eliminar" ShowDeleteButton="True" ShowHeader="True" />
                <asp:TemplateField HeaderText="Cuenta No.">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Cuenta ID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre de la cuenta">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Cuenta Nombre") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Debe">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_debe" runat="server" Text='<%# Eval("Debe") %>' 
                            Height="16px" Width="119px"></asp:TextBox>
                        <br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="tb_debe" ErrorMessage="Solo se permiten números" 
                            SetFocusOnError="True" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Haber">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_haber" runat="server"  Text='<%# Eval("Haber") %>' 
                            Height="16px" Width="120px"></asp:TextBox>
                        <br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                            ControlToValidate="tb_haber" ErrorMessage="Solo se permiten números" 
                            SetFocusOnError="True" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
    </td></tr>
    <tr><td align="center">
        <asp:Button ID="bt_guardar" runat="server" onclick="bt_guardar_Click" 
            Text="Guardar" />
    </td></tr>
    </table>
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="533px">
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
                                Width="293px" />
                        </td>
                    </tr>
                </caption>
            
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="bt_buscar_cta" runat="server" onclick="bt_buscar_cta_Click" 
                        Text="Buscar" />
                </td>
            </tr>
        </table></div>
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
            &nbsp;&nbsp;
            <asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" />
        </div>
    </asp:Panel>
    
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="tb_cuenta"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
</div>
</asp:Content>

