<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="retencion.aspx.cs" Inherits="manager_retencion" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
<h3 id="adduser">RETENCIONES</h3>

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
    <table width="500" align="center">
        <tr><td align="center">
            <table width="500">
            <tr><td>Listado de retenciones: 
                <asp:DropDownList ID="lb_retenciones" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_retenciones_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="0">Seleccione</asp:ListItem>
                </asp:DropDownList>
                                                </td></tr>
            <tr>
            <td>
                Identificador:<asp:TextBox ID="tb_id"  Text="0" ReadOnly="true" runat="server" Height="16px" Width="40px" ></asp:TextBox>
                <br />Nombre:<asp:TextBox ID="tb_nombre" runat="server" Height="16px" 
                    Width="209px"></asp:TextBox>
                <br />Monto minimo:<asp:TextBox ID="tb_minimo" runat="server" Height="16px" Width="131px"></asp:TextBox>
                <br />Porcentaje: <asp:TextBox ID="tb_porcentaje" runat="server" Height="16px" 
                    Width="131px"></asp:TextBox>
                <br />
                Tipo:<asp:DropDownList ID="lb_tipo" runat="server">
                    <asp:ListItem Value="1">Aplicable a la base</asp:ListItem>
                    <asp:ListItem Value="2">Aplicable al impuesto</asp:ListItem>
                    <asp:ListItem Value="3">Aplicable al total</asp:ListItem>
                </asp:DropDownList>
                <br />
                Tipo de Retencion: <asp:DropDownList ID="lb_tipo_retencion" runat="server">
                </asp:DropDownList>
            &nbsp;</td>
            </tr>
            <tr><td align="center">
                
                <asp:Button ID="bt_guardar" runat="server" onclick="bt_guardar_Click" 
                    Text="Guardar" />
                
            </td></tr>
            </table>            
        </td></tr>
        <tr><td>
            
        </td></tr>
    </table>
</div>
</asp:Content>

