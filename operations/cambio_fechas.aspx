<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="cambio_fechas.aspx.cs" Inherits="operations_cambio_fechas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    &nbsp;<asp:ScriptManager ID="ScriptManager1" runat="server" />
 <div id="box">
 <h3 id="adduser">CAMBIO DE FECHAS A DOCUMENTOS</h3>
 <table align="center">
 <tr>
    <td style="height: 26px"> Tipo de documento: </td><td style="height: 26px"> 
        <asp:DropDownList ID="lb_tipo_doc" runat="server" Width="350px">
            <asp:ListItem Value="1">Factura</asp:ListItem>
            <asp:ListItem Value="2">Recibo</asp:ListItem>
            <asp:ListItem Value="3">Nota de Crédito</asp:ListItem>
            <asp:ListItem Value="4">Nota de Débito</asp:ListItem>
            <asp:ListItem Value="5">Provisiones</asp:ListItem>
            <asp:ListItem Value="6">Cheques</asp:ListItem>
            <asp:ListItem Value="9">Retencion Proveedor</asp:ListItem>
            <asp:ListItem Value="12">NC Proveedor</asp:ListItem>
            <asp:ListItem Value="31">NC a ND Proveedor</asp:ListItem>
            <asp:ListItem Value="15">Ajuste Contable</asp:ListItem>
            <asp:ListItem Value="17">Depositos</asp:ListItem>
            <asp:ListItem Value="18">NC Ajuste</asp:ListItem>
            <asp:ListItem Value="19">Transferencias</asp:ListItem>
            <asp:ListItem Value="20">ND Bancaria</asp:ListItem>
            <asp:ListItem Value="21">NC Bancaria</asp:ListItem>
            <asp:ListItem Value="22">Recibo SOA</asp:ListItem>  
            <asp:ListItem Value="23">SOAS</asp:ListItem>
            <asp:ListItem Value="24">Liquidaciones</asp:ListItem>
            <asp:ListItem Value="28">Depositos SOA</asp:ListItem>    
        </asp:DropDownList>
    </td>
 </tr>
 <tr>
    <td style="height: 26px"> Moneda:</td><td style="height: 26px"> 
       <asp:DropDownList ID="lb_moneda" runat="server">
                </asp:DropDownList>
    </td>
 </tr>
 <tr>
    <td style="height: 19px"> Serie del documento:</td><td style="height: 19px">
        <asp:TextBox ID="tb_refid" runat="server" Height="16px" Width="400px"></asp:TextBox>
        </td>
 </tr>
 <tr><td>Número de correlativo: </td><td>
        <asp:TextBox ID="tb_correlativo" runat="server" Height="16px" Width="400px"></asp:TextBox>
        </td></tr>
    <tr><td>Banco: </td><td>
         <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="350px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
          </asp:DropDownList>       
        </td></tr>
    <tr><td>Cuenta Bancaria: </td><td>     
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" Width="350px">
                </asp:DropDownList>      
        </td></tr>
        <tr><td>Codigo:</td>
        <td><asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px"></asp:TextBox>
        </td>
    </tr>
    <tr><td>Nombre : </td>
        <td><asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                Width="400px" ReadOnly="True"></asp:TextBox>
        </td>
    </tr>
    <tr><td>&nbsp;</td><td>
        &nbsp;</td></tr>
        <tr><td colspan="2" align="center">
        <asp:Button ID="bt_aceptar" runat="server" Text="Buscar" 
            onclick="bt_aceptar_Click" />
        </td></tr>
 </table>
         <asp:Label ID="Label1" runat="server" 
            Text=" * El boton de seleccion permanecera deshabilitado cuando un documento este dentro de un periodo cerrado. O sea Serie electronica " 
            Enabled="False"></asp:Label>
 <table align="center">
    <tr><td>
    <asp:GridView ID="gv_resultado" runat="server" 
            onselectedindexchanged="gv_resultado_SelectedIndexChanged" onrowcreated="gv_resultado_RowCreated"   
        >
        <Columns>
            <asp:CommandField ButtonType="Button" EditText="Update" 
                ShowCancelButton="False" ShowSelectButton="True" />
        </Columns>
     </asp:GridView>
        </td></tr>
    <tr><td><br /><br /></td></tr> 
</table>    
 <table align="center">
 <thead><tr><td colspan="2" align="center"><h3>Documento Seleccionado</h3></td> </tr></thead>
 <tbody>
    <tr>
    <td><asp:Label ID="lb_tipo" runat="server" Text="Tipo: "  Visible="False"></asp:Label></td>
    <td><asp:TextBox ID="tb_tipo" runat="server" ReadOnly="True" Visible="False"></asp:TextBox></td>
    </tr>

    <tr>
    <td><asp:Label ID="lb_id" runat="server" Text="ID: "  Visible="False"></asp:Label></td>
    <td><asp:TextBox ID="tb_id" runat="server" ReadOnly="True" Visible="False"></asp:TextBox></td>
    </tr>
    <tr>
    <td><asp:Label ID="lb_serie" runat="server" Text="Serie: "  Visible="False"></asp:Label> </td>
    <td> <asp:TextBox ID="tb_serie" runat="server" ReadOnly="True" Visible="False"></asp:TextBox></td>
    </tr>
    <tr>
    <td><asp:Label ID="lb_correlativo" runat="server" Text="Correlativo: "  Visible="False"></asp:Label></td>
    <td> <asp:TextBox ID="tb_correlat" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
    </td>
    </tr> 
    <tr>
    <td><asp:Label ID="lb_monto" runat="server" Text="Monto: "  Visible="False"></asp:Label> </td>
    <td> <asp:TextBox ID="tb_monto" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
    </td>
    </tr> 
        <tr>
        <td>
            <asp:Label ID="lb_fecha_anterior" runat="server" Text="Fecha Actual: "  Visible="False"></asp:Label>
        </td>
        <td> <asp:TextBox ID="tb_fecha_anterior" runat="server" Visible="False" 
                Enabled="False" ></asp:TextBox>
            </td>
    </tr>
    <tr>
    <td><asp:Label ID="lb_nueva_fecha" runat="server" Text="Nueva Fecha: "  Visible="False"></asp:Label></td>
    <td> <asp:TextBox ID="tb_fecha" runat="server" Visible="False" ></asp:TextBox>
        <cc1:CalendarExtender ID="tb_fecha_CalendarExtender" runat="server" Format="dd/MM/yyyy"
            TargetControlID="tb_fecha" >
        </cc1:CalendarExtender>
    </td></tr>
         <tr><td>
        <asp:Label ID="lb_motivo" runat="server" Text="Motivo: " Visible="False"></asp:Label> </td><td>
        <asp:TextBox ID="tb_motivo" runat="server" Visible="False"></asp:TextBox>
        </td></tr>
        <tr><td><asp:Label ID="lb_tipo_operacion" runat="server" Text="Tomar Operacion:" 
                Visible="False"></asp:Label></td>
        <td>
            <asp:DropDownList ID="dd_tipo_op" runat="server" Visible="False">
            <asp:ListItem Value="1">Original</asp:ListItem>
            <asp:ListItem Value="2">Anulada</asp:ListItem>
            <asp:ListItem Value="3">Ambas</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:Label ID="lb_aplica_deposito" runat="server" Text="Aplicar A Deposito Asociado: " 
                    Visible="False"></asp:Label> </td>
            <td> 
                <asp:CheckBox ID="chb_deposito" runat="server" Text="SI" Visible="False" />
             </td>
            
        </tr>
    <tr><td colspan="2" align="center">
        
        <asp:Button ID="bt_guardar" runat="server" Text="Guardar" 
            onclick="bt_guardar_Click" Enabled="False" Visible="False" />
        </td></tr>
        </tbody>
    </table>
     
     
 </div>
</asp:Content>

