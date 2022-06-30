<%@ Page Language="C#" MasterPageFile="~/Reports/manager.master" AutoEventWireup="true" CodeFile="SolicitaSOA.aspx.cs" Inherits="Reports_EstadoCuentaAgente" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<fieldset id="Fieldset1">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {
        
    }  

    function mpeSeleccionOnCancel()
    {
        
    }
    </script>
<h3 id="adduser">GENERAR SOA PARA BL&nbsp;</h3>
<table width="650">
<tr><td align="center">
    <table width=650" align="center">
    <tr><td>
        HBL:<asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="100px"></asp:TextBox>
        MBL:<asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="100px"></asp:TextBox>
        Routing:<asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="100px"></asp:TextBox>
        Contenedor:<asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="100px"></asp:TextBox>
        <br />
    </td></tr>
    <tr><td>
    
        <asp:Button ID="tb_generar" runat="server" onclick="tb_generar_Click" 
            Text="Generar" />
    
    </td></tr>
    </table>
</td></tr>
<tr><td>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" />
</td>
</tr>
<tr><td>
<%--********************************************************--%>
    <asp:Panel ID="pnlCuentas" runat="server" CssClass="CajaDialogo" Width="558px">
        <div align="center">
        <table align="center">
        <tr><td colspan="2"><div style="padding: 10px; background-color: #0033CC; color: #FFFFFF;">
            <asp:Label ID="Label1" runat="server" Text="Seleccionar Cuenta" />
        </div></td></tr>
        <tr>
            <td align="center" colspan="2">
                <asp:TextBox ID="tb_criterio" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">&nbsp;<asp:CheckBoxList ID="ch_bl" runat="server">
                    <asp:ListItem>BL_MASTER</asp:ListItem>
                    <asp:ListItem>BL_HOUSE</asp:ListItem>
                    <asp:ListItem>ROUTING</asp:ListItem>
                    <asp:ListItem>CONTENEDOR</asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;&nbsp;Tipo:  
                <asp:DropDownList ID="lb_tipo" runat="server">
                    <asp:ListItem Selected="True">LCL</asp:ListItem>
                    <asp:ListItem>FCL</asp:ListItem>
                </asp:DropDownList>
            </td>
            
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="dgw1" runat="server" onrowcommand="dgw1_RowCommand" AllowPaging="True" onpageindexchanging="dgw1_PageIndexChanging" PageSize="10">
                    <Columns>
                        <asp:ButtonField HeaderText="Seleccionar" Text="Seleccionar" 
                            CommandName="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bt_search" runat="server" Text="Buscar" 
                            onclick="bt_search_Click" />
            </td>
            <td><asp:Button ID="btnCuentaCancelar" runat="server" Text="Cancelar" /></td>
        </tr>
        </table>
        </div>
    </asp:Panel>
    
    <cc1:ModalPopupExtender ID="tb_cuenta_ModalPopupExtender" runat="server" TargetControlID="tb_hbl"
            PopupControlID="pnlCuentas" CancelControlID="btnCuentaCancelar"
            OnCancelScript="mpeCuentaOnCancel()" DropShadow="True"
            BackgroundCssClass="FondoAplicacion" />
    <%--********************************************************--%>
</td></tr>
</table>
</fieldset>
</div>
</asp:Content>

