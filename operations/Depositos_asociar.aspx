<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="Depositos_asociar.aspx.cs" Inherits="operations_Depositos" Title="AIMAR - BAW" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
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
<h3 id="adduser">ASOCIAR DEPOSITOS</h3>
<table width="650">
<tr><td align="center">
        <table cellpadding="0" cellspacing="0" style="width: 95%">
            <tr>
                <td>
                    Transaccion</td>
                <td colspan="3">
                <asp:DropDownList ID="lb_transaccion" runat="server" Height="22px" Width="350px">
                </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Banco</td>
                <td colspan="3">
                    <asp:DropDownList ID="lb_bancos" runat="server" Height="25px" 
                    Width="350px" AutoPostBack="True" 
                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Cuenta Bancaria</td>
                <td colspan="3">
                <asp:DropDownList ID="lb_cuentas_bancarias" runat="server" 
                    onselectedindexchanged="lb_cuentas_bancarias_SelectedIndexChanged" 
            AutoPostBack="True" Width="350px">
                </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Moneda</strong></td>
                <td>
                    <asp:DropDownList ID="lb_moneda" runat="server" Enabled="False" Width="165px" 
                        BackColor="#66CCFF" Font-Bold="True" ForeColor="Black">
                </asp:DropDownList>
                </td>
                <td>
                    Numero de Referencia</td>
                <td>
                    <asp:TextBox ID="tb_refNo" 
                    runat="server" Height="16px" Width="150px" MaxLength="25"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Fecha</td>
                <td>
                    <asp:TextBox ID="tb_fecha_deposito" 
                    runat="server" Height="16px" Width="129px" MaxLength="25" 
            ReadOnly="True"></asp:TextBox>
                </td>
                <td align="center">
                    Tipo de Cambio</td>
                <td align="center">
                    <asp:TextBox ID="tb_tipo_cambio" runat="server" Height="16px" ReadOnly="True" 
                        Width="70px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td align="center">
                    <strong>Monto a Depositar</strong></td>
                <td>
        <asp:TextBox ID="tb_monto" 
                    runat="server" Height="16px" Width="130px" ReadOnly="True" BackColor="#66CCFF" 
                        style=" text-align:right" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    Observaciones</td>
                <td colspan="3">
        <asp:TextBox ID="tb_observaciones" runat="server" Height="16px" Width="450px" 
            ReadOnly="True"></asp:TextBox>                    
                </td>
            </tr>
            <tr>
                <td>
                    Ingresador Por:</td>
                <td colspan="3">
         <asp:TextBox ID="tb_usu_creador" 
                    runat="server" Height="16px" Width="129px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center" height="45px">
        <asp:Button ID="btn_cargar" runat="server" onclick="btn_cargar_Click" 
            Text="Buscar Deposito" />
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                <h3 id="H4">Recibos Aplicados</h3>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
        <asp:GridView ID="gv_recibosaplicados" runat="server" 
            EmptyDataText="No existe ningún recibo aplicado con este deposito">
        </asp:GridView>
                </td>
            </tr>
        </table>
    </td>
</tr>
<tr><td>
        &nbsp;</td></tr>
<tr><td>
    <table width="650">
    <tr><td>
        <h3 id="H2">Busqueda por recibo</h3>
    </td></tr>
    <tr><td>
        Serie:   
        <asp:TextBox ID="tb_serie" runat="server" Height="16px" Width="99px"></asp:TextBox>
        Correlativo:
        <asp:TextBox ID="tb_correlativo" runat="server" Height="16px" Width="101px"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="tb_buscar" runat="server" onclick="tb_buscar_Click" 
            Text="Buscar" />
    </td></tr>
    </table>
</td></tr>
<tr><td>
    <table width="650">
    <tr><td>
        <h3 id="H1">Detalle de recibos</h3>
    </td></tr>
    <tr><td align="center">&nbsp;<asp:GridView ID="gv_cortes" runat="server" Width="623px" 
            onrowcreated="gv_cortes_RowCreated" AllowPaging="True" 
            onpageindexchanging="gv_cortes_PageIndexChanging" PageSize="20" 
            Font-Size="X-Small">
        <Columns>
            <asp:TemplateField HeaderText="SELECCIONE">
                <ItemTemplate>
                    <asp:CheckBox ID="chk_seleccion" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>        
    </td></tr>
    </table>
</td></tr>
<tr><td align="center">
        <table width="650">
          <thead>
              <tr>
                  <th>
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
<tr><td align="center">
                            <table align="center" cellpadding="0" cellspacing="0" 
            style="width: 90%">
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                          <asp:GridView ID="gv_detalle_partida_diferencial" runat="server" 
                              AutoGenerateColumns="False" EmptyDataText="" Width="90%" Visible="False" 
                                Font-Size="X-Small">
                              <Columns>
                                  <asp:TemplateField HeaderText="CUENTA">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cuenta0" runat="server" Text='<%# Eval("CUENTA") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="DESCRIPCION DE CUENTA">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_desc_cuenta0" runat="server" 
                                              Text='<%# Eval("DESC_CUENTA") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="CARGOS">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_cargos0" runat="server" Text='<%# Eval("CARGOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="ABONOS">
                                      <ItemTemplate>
                                          <asp:Label ID="lb_abonos0" runat="server" Text='<%# Eval("ABONOS") %>'></asp:Label>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                              </Columns>
                          </asp:GridView>
                                    </td>
                                </tr>
                            </table>
</td></tr>
<tr><td align="center">
    <asp:Button ID="btn_deposito" runat="server" Text="Realizar deposito" 
        onclick="btn_deposito_Click" /></td></tr>
</table>
</fieldset>
</div>
</asp:Content>

