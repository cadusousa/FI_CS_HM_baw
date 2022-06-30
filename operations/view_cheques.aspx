<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="view_cheques.aspx.cs" Inherits="operations_pop_cheques" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">CHEQUE</h3>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
        <table>
        <tr>
            <td>
                Transaccion:
                <asp:DropDownList ID="lb_transaccion" runat="server" Height="22px" 
                    Width="315px" Enabled="False" BackColor="White">
                </asp:DropDownList>
                <asp:Label ID="lb_correlativo" runat="server" Text="0" Visible="False"></asp:Label>
                <asp:Label ID="lbl_cheque_serie" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lbl_cheque_correlativo" runat="server"></asp:Label>
                <br />
                Banco:                 
                <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="317px" AutoPostBack="True" BackColor="White" Enabled="False">
                </asp:DropDownList>
                &nbsp;Cta Bancaria No.:
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" 
                    AutoPostBack="True" BackColor="White" Enabled="False">
                </asp:DropDownList>
                <br />
                Moneda:                 <asp:DropDownList ID="lb_moneda" runat="server" Enabled="False" 
                    BackColor="White">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                
                Número:                 
                <asp:TextBox ID="tb_chequeNo" 
                    runat="server" Height="16px" Width="129px" ReadOnly="True"></asp:TextBox>
                &nbsp; Fecha:
                <asp:TextBox ID="tb_fecha" runat="server" Height="16px" Width="128px" 
                    ReadOnly="True"></asp:TextBox>
                <br />
                Acreditado:                 
                <asp:TextBox ID="tb_acreditado" runat="server" 
                    Height="16px" Width="457px" ReadOnly="True"></asp:TextBox>
                <br />
                Concepto:<asp:TextBox ID="tb_motivo" runat="server" Height="16px" Width="484px" 
                    ReadOnly="True"></asp:TextBox>
                <br />
                Referencia:<asp:TextBox ID="tb_referencia" runat="server" Height="16px" 
                    Width="484px" ReadOnly="True"></asp:TextBox>
                <br />
                Observaciones:<asp:TextBox ID="tb_observaciones" runat="server" Height="16px" 
                    Width="484px" ReadOnly="True"></asp:TextBox>

                <br />
                Cargos Adicionales:                 
                <asp:TextBox ID="tb_cargoadicional" 
                    runat="server" Height="16px" Width="129px" ReadOnly="True"></asp:TextBox>
                <br />
                Valor:                 <asp:TextBox ID="tb_valor" runat="server" Height="16px" Width="128px" 
                    ReadOnly="True">0</asp:TextBox>
                <br />
                <br />
                
            </td>
        </tr>
        <tr><td>
            Tipo de proveedor:        <asp:DropDownList ID="lb_tipopersona" runat="server" Enabled="False" 
                BackColor="White">
                    <asp:ListItem Value="4">Proveedor</asp:ListItem>
                    <asp:ListItem Value="2">Agente</asp:ListItem>
                    <asp:ListItem Value="5">Naviera</asp:ListItem>
                    <asp:ListItem Value="6">Lineas Aereas</asp:ListItem>
                    <asp:ListItem Value="8">Caja Chica</asp:ListItem>
                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                    <asp:ListItem Value="12">Comisiones</asp:ListItem>
                    <asp:ListItem Value="0">Retenciones</asp:ListItem>
                </asp:DropDownList>
                <br />
            Codigo :<asp:TextBox ID="tb_agenteID" runat="server" Height="16px" 
                        Width="115px" ReadOnly="True"></asp:TextBox>
                    <br />
                    Nombre: 
                    <asp:TextBox ID="tb_agentenombre" runat="server" Height="16px" 
                Width="377px" ReadOnly="True"></asp:TextBox>
                    <br />
                     
    </td></tr>
    <tr><td align="center">
            <table width="650">
                <thead>
                <tr>
                    <th colspan="6">Detalle de cheque (cortes o recibos pagados)</tr>
                </thead>
                <tr><td>
                    <asp:GridView ID="gv_cortes" runat="server" Width="623px" 
                        onrowcreated="gv_cortes_RowCreated">
                    </asp:GridView>
                </td>
                </tr>
                <tr><td>
                <table width="650">
                  <thead>
                      <tr>
                          <th colspan="5">
                              Poliza de Diario</th>
                      </tr>
                  </thead>
                  <tr>
                      <td>
                          <asp:GridView ID="gv_detalle_partida" runat="server" 
                              AutoGenerateColumns="False" EmptyDataText="" Width="90%">
                              <Columns>
                                  <asp:TemplateField HeaderText="CUENTA">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cuenta" runat="server" Text='<%# Eval("CUENTA") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="DESCRIPCION DE CUENTA">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_desc_cuenta" runat="server" Text='<%# Eval("DESC_CUENTA") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="CARGOS">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cargos" runat="server" Text='<%# Eval("CARGOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="ABONOS">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_abonos" runat="server" Text='<%# Eval("ABONOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                              </Columns>
                          </asp:GridView>
                      </td>
                  </tr>
              </table>
                </td></tr>
                </table>
        </td></tr>   
        <tr>
            <td align="center">
                <asp:Button ID="btn_imprimir" runat="server" onclick="btn_imprimir_Click" 
                    Text="Re-Imprimir" />
                <asp:Button ID="btn_printret" runat="server" onclick="btn_printret_Click" 
                    Text="Imprimir retenciones" />
            </td>
        </tr>
        </table>
      </div>
</asp:Content>
