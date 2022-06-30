<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="searchinvoice.aspx.cs" Inherits="invoice_searchdoc" Title="AIMAR - BAW"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

    <div id="box">
<h3 id="adduser">BUSCAR DOCUMENTO</h3>
      <asp:Label ID="Label1" runat="server" Text="TIPO DE DOCUMENTO:"></asp:Label>  
        <asp:DropDownList ID="DDLchose" runat="server" AutoPostBack="True" 
            onselectedindexchanged="DDLchose_SelectedIndexChanged" >
          <asp:ListItem Value="0">Seleccione...</asp:ListItem>
          <asp:ListItem Value="1">Factura</asp:ListItem>
         <asp:ListItem Value="4">Nota de Debito</asp:ListItem>         
        </asp:DropDownList>
        
   <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script language="javascript" type="text/javascript">   
    function mpeCuentaOnCancel()
    {

    } 
    function mpeClienteOnCancel()
    {

    } 
    </script>
<table width="700" align="center">
    <tr><td>
        <table width="650" align="center">
        <tbody>
            <tr>
                <td>Serie</td>
                <td>
                    <asp:DropDownList ID="lb_serie" runat="server" 
                        onselectedindexchanged="lb_serie_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Correlativo</td>
                <td>
                    <asp:TextBox ID="tb_correlativo" runat="server" Height="16px" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>HBL</td>
                <td>
                    <asp:TextBox ID="tb_hbl" runat="server" Height="16px" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>MBL</td>
                <td>
                    <asp:TextBox ID="tb_mbl" runat="server" Height="16px" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>CONTENEDOR</td>
                <td>
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="16px" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>ROUTING</td>
                <td>
                    <asp:TextBox ID="tb_routing" runat="server" Height="16px" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>POLIZA ADUANAL</td>
                <td>
                    <asp:TextBox ID="tb_poliza" runat="server" Height="16px" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>CERTIFICADO DE SEGUROS</td>
                <td>
                    <asp:TextBox ID="tb_poliza_seguros" runat="server" Height="16px" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Estado Documento</td>
                <td>
                    
                    <asp:DropDownList ID="lb_ted_id" runat="server" Width="150px">
                        <asp:ListItem Value="0">Todos</asp:ListItem>
                        <asp:ListItem Value="1">Activa</asp:ListItem>
                        <asp:ListItem Value="2">Abonada</asp:ListItem>
                        <asp:ListItem Value="3">Anulada</asp:ListItem>
                        <asp:ListItem Value="4">Cancelada</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
            </tr>
            <tr>
                <td>Tipo de Persona</td>
                <td>
                    
                    <asp:DropDownList ID="drp_tipo_persona" runat="server" Width="150px">
                        <asp:ListItem Value="3">Cliente</asp:ListItem>
                        <asp:ListItem Value="10">Intercompany</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
            </tr>
            <tr>
                <td>Codigo de Persona</td>
                <td>
                    
                    <asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="300px"></asp:TextBox>
                    
                    <cc1:FilteredTextBoxExtender ID="tbCliCod_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers" TargetControlID="tbCliCod">
                    </cc1:FilteredTextBoxExtender>
                    
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Panel ID="pnl_anios" runat="server" Visible="False">
                        <table align="center" cellpadding="0" cellspacing="0" 
    style="width: 50%">
                            <tr>
                                <td>
                                    Año</td>
                                <td>
                                    <asp:DropDownList ID="drp_anios" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btn_buscar" runat="server" onclick="btn_buscar_Click" 
                        Text="Buscar" />
                </td>
            </tr>
        </tbody>
        </table>
        
        <br />
        
    </td>
    </tr>
    <tr>
        <td>
            <table align="left">
            <tr>
                <td>
<asp:Panel ID="pnl_provisiones" runat="server" ScrollBars="Both" Width="700px" 
                    Height="550px">

                    <asp:GridView ID="gv_facturas" runat="server" 
                        EmptyDataText="No existen datos que cumplan con estos criterios." 
                        onrowcommand="gv_facturas_RowCommand" PageSize="30" 
                        onrowcreated="gv_facturas_RowCreated" 
                        onselectedindexchanged="gv_facturas_SelectedIndexChanged">
                        <EmptyDataRowStyle BackColor="Yellow" ForeColor="#FF3300" />
                        <Columns>
                            <asp:CommandField ButtonType="Button" SelectText="Ver Detalle" 
                                ShowSelectButton="True" />
                        </Columns>
                    </asp:GridView>
                  </asp:Panel>
                </td>
            </tr>
            </table>
        </td>
    </tr>
</table>
</div>
            
</asp:Content>

