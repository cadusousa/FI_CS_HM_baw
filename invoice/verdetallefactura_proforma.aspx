<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="verdetallefactura_proforma.aspx.cs" Inherits="invoice_viewrcpt" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
    <fieldset id="Fieldset1">
    <h3 id="adduser">FACTURACION</h3>

    <table width="650">
    <tr><td>
        <table width="650" align="left">
        <tbody>
            <tr>
                <td>
                    Tipo de operacion:<asp:DropDownList ID="lb_tipo_transaccion" runat="server">
                    </asp:DropDownList>
                    &nbsp;Moneda a facturar<asp:DropDownList ID="lb_moneda" runat="server">
                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br />
                    Série de factura: <asp:DropDownList ID="lb_serie_factura" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="tb_agenteid" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:TextBox ID="tb_comix_agente" runat="server" Visible="False" Width="16px" Text="0"></asp:TextBox>
                    <asp:Label ID="lb_facid" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:Label ID="lb_mbl" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lb_routing" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="id_shipper" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="id_consignee" runat="server" Visible="False" Text="0"></asp:Label>
                    <br />
                    &nbsp;Codigo:<asp:TextBox ID="tbCliCod" runat="server" Height="16px" 
                        Width="82px" ReadOnly="True"></asp:TextBox>
                    <br />
                    Nombre:<asp:TextBox ID="tb_nombre" runat="server" ReadOnly="True" Width="344px" 
                        Height="15px"></asp:TextBox>
                    &nbsp;&nbsp; Nit:&nbsp;
                    <asp:TextBox ID="tb_nit" runat="server" ReadOnly="True" Height="15px" 
                        Width="80px"></asp:TextBox>
                    <br />
                    Razon:  <asp:TextBox ID="tb_razon" runat="server" Height="15px" Width="242px" 
                        ReadOnly="True"></asp:TextBox>
                    &nbsp;<asp:DropDownList ID="lb_imp_exp" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="lb_contribuyente" runat="server">
                    </asp:DropDownList>
                    <br />
                    Dir: <asp:TextBox ID="tb_direccion" runat="server" Height="15px" Width="396px" 
                        ReadOnly="True"></asp:TextBox>
                    <br />
                    Notas: <asp:TextBox ID="tb_observaciones" runat="server" Height="15px" 
                        Width="378px"></asp:TextBox>
                    &nbsp;Ref #:&nbsp;
                    <asp:TextBox ID="tb_referencia" runat="server" Height="15px" Width="128px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                                        MBL/GUIA:
                    <asp:TextBox ID="tb_bl" runat="server" ReadOnly="True" Height="15px" 
                        Width="85px"></asp:TextBox>
                                        <br />
                                        Routing:
                    <asp:TextBox ID="tb_orden" runat="server" Height="15px" Width="100px"></asp:TextBox>
                                        &nbsp; Contenedor:
                    <asp:TextBox ID="tb_contenedor" runat="server" Height="15px" Width="85px"></asp:TextBox>
                                        <br />
                                        &nbsp; Naviera:<asp:TextBox ID="tb_naviera" runat="server" Height="15px" Width="85px"></asp:TextBox>
                                        &nbsp; Vapor:&nbsp;
                    <asp:TextBox ID="tb_vapor" runat="server" Height="15px" Width="85px"></asp:TextBox>
                                        &nbsp;&nbsp;Agente:
                                        <asp:TextBox ID="tb_agente_nombre" runat="server" Height="16px" Width="102px" 
                                            ReadOnly="True"></asp:TextBox>
                    <br />
                                        &nbsp;Shipper: 
                                        <asp:TextBox ID="tb_shipper" runat="server" Height="15px" 
                        Width="180px" ReadOnly="True"></asp:TextBox>
                                        &nbsp;&nbsp;Consignee:
                    <asp:TextBox ID="tb_consignee" runat="server" Height="15px" Width="100px" ReadOnly="True"></asp:TextBox>
                    <br />
                                        &nbsp;Comodity:                     <asp:TextBox ID="tb_comodity" runat="server" Height="15px" Width="215px"></asp:TextBox>
                                        &nbsp;Paquetes:
                    <asp:TextBox ID="tb_paquetes1" runat="server" Height="15px" Width="35px"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="tb_paquetes2" runat="server" Height="15px" Width="80px"></asp:TextBox>
                                        &nbsp;Peso:
                    <asp:TextBox ID="tb_peso" runat="server" Height="15px" Width="40px"></asp:TextBox>
                                        &nbsp;Vol:<asp:TextBox ID="tb_vol" runat="server" Height="15px" Width="40px"></asp:TextBox>
                                        &nbsp;<br />
                                        &nbsp;Dua Ingreso:                     <asp:TextBox ID="tb_dua_ingreso" runat="server" Height="15px" Width="100px"></asp:TextBox>
                                        &nbsp;Dua Salida:
                    <asp:TextBox ID="tb_dua_salida" runat="server" Height="15px" Width="100px"></asp:TextBox>
                                        &nbsp;Vendedor:
                    <asp:TextBox ID="tb_vendedor1" runat="server" Height="15px" Width="65px"></asp:TextBox>
                                        &nbsp;<asp:TextBox ID="tb_vendedor2" runat="server" Width="90px" Height="15px"></asp:TextBox>
                </td>
            </tr>
        </tbody>
        </table>
    </td></tr>
    <tr><td>
    <br /><br />
        <table width="650">
        <thead>
            <tr>
                <th colspan="5">Detalle de factura</th>
             </tr>
        </thead>
        <tbody>
            <tr><td>
                <asp:GridView ID="dgw" runat="server" PageSize="30" Width="638px">
                    </asp:GridView>
            </td></tr>
        </tbody>
        </table>
    </td></tr>
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
                    <asp:Label ID="Label2" runat="server" Text="Total Dolares:"></asp:Label>
                    &nbsp;<asp:TextBox ID="tb_totaldolares" runat="server"></asp:TextBox>
                    <br />
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
        </td></tr>
    </table>
       

        <br /><br />
          <div align="center">
              <asp:Button ID="btn_factura" runat="server" onclick="btn_factura_Click" 
                  Text="Convertir a factura" />
              &nbsp;<input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
          </div>
</fieldset>
</div>

</asp:Content>

