<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="addcta.aspx.cs" Inherits="manager_addcta" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<br />
                <div id="box">
                	<h3 id="adduser">CUENTA CONTABLE</h3>

                      <fieldset id="personal">
                        <legend>Información </legend>
                        <br />
                        <label for="lastname">* Número de cuenta : </label> 
                        <asp:TextBox ID="tb_cta" runat="server"></asp:TextBox>
                          <asp:Label ID="lb_ref" runat="server" Visible="False"></asp:Label>
                        <br /><br />
                        <label for="lastname">* Nombre de la cuenta : </label> 
                        <asp:TextBox ID="tb_nombre" runat="server"></asp:TextBox>
                        <br /><br />
                        <label for="lastname">* Cuenta a reportar : </label> 
                        <asp:TextBox ID="tb_cta_madre" runat="server"></asp:TextBox>
                          <br /><br />
                        <label for="lastname">Nivel : </label>
                          <asp:DropDownList ID="lb_nivel" runat="server">
                              <asp:ListItem Selected="True">1</asp:ListItem>
                              <asp:ListItem>2</asp:ListItem>
                              <asp:ListItem>3</asp:ListItem>
                              <asp:ListItem>4</asp:ListItem>
                              <asp:ListItem>5</asp:ListItem>
                              <asp:ListItem>6</asp:ListItem>
                              <asp:ListItem>7</asp:ListItem>
                              <asp:ListItem>8</asp:ListItem>
                          </asp:DropDownList>
                        <br /><br />
                        <label for="lastname">Clasificación : </label> 
                        <asp:DropDownList ID="lb_clasificacion" runat="server">
                            <asp:ListItem Selected="True" Value="1">Activo</asp:ListItem>
                            <asp:ListItem Value="2">Pasivo</asp:ListItem>
                            <asp:ListItem Value="3">Venta(Ingresos)</asp:ListItem>
                            <asp:ListItem Value="4">Gastos(Egresos)</asp:ListItem>
                            <asp:ListItem Value="5">Capital</asp:ListItem>
                          </asp:DropDownList>
                        <br /><br />
                        <label for="lastname">Tipo : </label> 
                          <asp:DropDownList ID="lb_tipo" runat="server">
                              <asp:ListItem Selected="True" Value="0">No aplica</asp:ListItem>
                              <asp:ListItem Value="1">Caja</asp:ListItem>
                              <asp:ListItem Value="2">Banco</asp:ListItem>
                          </asp:DropDownList>
                        <br /><br />
                        <label for="lastname">Usuario Creador : </label> 
                        <asp:TextBox ID="tb_banco_nombre" runat="server"></asp:TextBox>
                        <br /><br />
                      </fieldset>
                      <br /><br />
                      <div align="center"> 
                          <asp:Button ID="bt_enviar" runat="server" Text="Guardar" 
                              onclick="bt_enviar_Click" />&nbsp;&nbsp;
                          <input id="Cancel" type="button" value="Cancelar" onclick="javascript:window.location.href='searchcta.aspx';" />
                      </div>
                </div>
</asp:Content>

