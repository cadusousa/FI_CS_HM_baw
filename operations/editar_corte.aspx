<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="editar_corte.aspx.cs" Inherits="operations_editar_corte" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
<h3 id="adduser">EDITAR CORTE</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeSeleccionOnCancel()
    {

    }
    </script>
<div align="center">
<table>
    <tr><td width="150px" colspan="2">
        Serie&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="tb_serie" runat="server" Height="16px" Width="72px" 
            ReadOnly="True"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Correlativo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="tb_corr" runat="server" Height="16px" Width="70px" 
            ReadOnly="True"></asp:TextBox>
        </td></tr>
    <tr><td>
        Codigo</td><td>
            <asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="lb_tipo_persona" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lb_corteID" runat="server" Text="0" Visible="False"></asp:Label>
        <asp:Label ID="lb_moneda" runat="server" Text="0" Visible="False"></asp:Label>
    </td></tr>
    <tr><td>
        Nombre</td><td>
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td>
        Contacto</td><td>
            <asp:TextBox ID="tb_contacto" runat="server" Height="16px" 
                        Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td>
        Direccion</td><td>
                    <asp:TextBox ID="tb_direccion" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td>
        Telefono</td><td>
                    <asp:TextBox ID="tb_telefono" runat="server" Height="16px" 
                Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td>
        Correo</td><td>
            <asp:TextBox ID="tb_correoelectronico" runat="server" 
                        Height="16px" Width="520px" ReadOnly="True"></asp:TextBox>
    </td></tr>
    <tr><td colspan="2"><h3 id="adduser">Cuentas por pagar</tr>
    <tr><td align="center" colspan="2">&nbsp;<asp:GridView ID="gv_detalle" 
            runat="server" Width="650px" 
            onrowcreated="gv_detalle_RowCreated">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>
    <tr><td colspan="2">
        &nbsp;</td></tr>
    <tr><td colspan="2"><h3 id="H1">Notas de Credito</h3></td></tr>
    <tr><td align="center" colspan="2">&nbsp;<asp:GridView ID="gv_notacredito" 
            runat="server" Width="650px" 
            onrowcreated="gv_notacredito_RowCreated">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>
    <tr><td colspan="2">
        &nbsp;</td></tr>
    <tr><td colspan="2"><h3 id="H2">Notas de debito</h3></td></tr>
    <tr><td align="center" colspan="2">&nbsp;<asp:GridView ID="gv_notadebito" 
            runat="server" Width="650px" 
            onrowcreated="gv_notadebito_RowCreated">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>

    <tr><td colspan="2">&nbsp;<br />&nbsp;</td></tr>
    <tr><td colspan="2"><h3 id="H4">Notas credito a notas debito</h3></td></tr>
    <tr><td align="center" colspan="2">&nbsp;<asp:GridView ID="gv_notacredito_nd" 
            runat="server" Width="650px" 
            onrowcreated="gv_notacredito_ndRowCreated">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br /></td></tr>


        <tr><td colspan="2"><h3 id="H5"><asp:Label ID="lb_titulo_fac" runat="server" 
                 Text="Facturas Intercompany" Visible="False"></asp:Label></h3></td></tr>
    
    <tr><td align="center" colspan="2">&nbsp;<br /><asp:GridView ID="gv_fact_intercompany" runat="server" Width="650px" 
            onrowcreated="gv_factintercompany_RowCreated" Visible="False">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        </td></tr>
    <tr><td colspan="2"><h3 id="H3">DETALLE DEL CORTE</h3>
        <p>Total:&nbsp;
            <asp:Label ID="lb_total" runat="server"></asp:Label>
        </p>
        <p>Total Equivalente:&nbsp;&nbsp;
            <asp:Label ID="lb_equivalente" runat="server"></asp:Label>
        </p>
        </td></tr>
<tr><td align="center" colspan="2">&nbsp;<asp:GridView ID="gv_cortes" 
        runat="server" Width="650px" 
        onrowcommand="gv_cortes_RowCommand" onrowcreated="gv_cortes_RowCreated">
        <Columns>
            <asp:TemplateField HeaderText="Seleccionar">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox2" runat="server" Checked="True" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <br />
</td></tr>

    <tr><td colspan="2">
        <asp:Button ID="bt_generar" runat="server" onclick="bt_generar_Click" 
            Text="Generar corte" />
        <asp:Label ID="lb_ctas" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lb_nc" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lb_nd" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lb_facts" runat="server" Visible="False"></asp:Label>
        </td></tr>

</table>
</div>
</div>
</asp:Content>

