<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="retencion_cliente.aspx.cs" Inherits="invoice_retencion_cliente" Title="AIMAR - BAW"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {

    }  
    </script>
<div id="box">
<fieldset id="Fieldset1">
<h3 id="adduser">RETENCIONES A CLIENTES</h3>
<table width="650">
    <tr><td>
        <table width="650" align="left">
        <tbody>
            <tr>
                <td>
                    Operacion:<asp:DropDownList ID="lb_trans" runat="server"></asp:DropDownList>
                    &nbsp;Moneda:<asp:DropDownList ID="lb_moneda" runat="server"></asp:DropDownList>
                    <asp:Label ID="lb_facid" runat="server" Visible="False"></asp:Label>
                    <br />
                    Série de documento <asp:DropDownList ID="lb_serie_factura" runat="server">
                    </asp:DropDownList><br />
                    Codigo:<asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="87px" ReadOnly="True"></asp:TextBox>
                    &nbsp;<input id="Button2" type="button" value="Buscar" onclick="javascript:window.open('pop_buscarcliente.aspx?pagina=notadebito.aspx', null, 'toolbar=no,resizable=no,status=no,width=400,height=350');" />
                    <br />Nit:<asp:TextBox ID="tb_nit" runat="server" Height="16px" Width="72px" ReadOnly="True"></asp:TextBox>
                    &nbsp;Cliente:<asp:TextBox 
                        ID="tb_clientname" runat="server" Height="16px" Width="372px" 
                        ReadOnly="True"></asp:TextBox><br />
                    Nombre del cliente:<asp:TextBox ID="tb_nombre" runat="server" 
                        Height="16px" Width="355px"></asp:TextBox>
                    <br />
                    Direccion :                     <asp:TextBox ID="tb_direccion" runat="server" Height="16px" Width="418px"></asp:TextBox>
                    <br />
                    Observaciones:
                    <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" Width="399px"></asp:TextBox>
                    <br />
                </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td>
    
        &nbsp;</td></tr>
    <tr><td>
        <table width="300" align="right">
        <thead>
            <tr>
                <th align="left">Totales</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Sub-Total:"></asp:Label>
                    &nbsp;<asp:TextBox ID="tb_subtotal" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label7" runat="server" Text="Impuesto:"></asp:Label>
                    &nbsp;<asp:TextBox ID="tb_impuesto" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label8" runat="server" Text="Total:"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tb_total" runat="server"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
        <%--<input id="bt_agregar" type="button" value="Agregar rubro" 
            onclick="javascript:window.open('additem.aspx', null, 'toolbar=no,resizable=no,status=no,width=400,height=300');"" />--%></td></tr>
    <tr><td>
        <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" 
                  onclick="bt_Enviar_Click" />&nbsp;
            <input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
            <asp:Button ID="bt_imprimir" runat="server" Enabled="False" 
                onclick="bt_imprimir_Click" Text="Imprimir" />
          </div>
    </td></tr>
    </table>
        
    </fieldset>
</div>
</asp:Content>

