<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addprofile.aspx.cs" Inherits="manager_searchprofile" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
                <br />
                <div id="box">
                	<h3 id="adduser">PERFILES</h3>

                      <fieldset id="personal">
                        <legend>Información del perfil</legend>
                        <br />
                        <label>Perfil ID : </label> 
                          <asp:TextBox ID="tb_perfilid" Enabled="false" runat="server"></asp:TextBox>
                          <br /><br />
                        <label>Nombre : </label> 
                          <asp:TextBox ID="tb_nombre" runat="server"></asp:TextBox>
                        <br />
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                              ControlToValidate="tb_nombre" 
                              ErrorMessage="El campo de nombre es requerido, por favor completar" 
                              SetFocusOnError="True"></asp:RequiredFieldValidator>
                          <br />
                        <label>Descripción : </label>
                        <asp:TextBox ID="tb_descripcion" runat="server"></asp:TextBox>
                        <br /><br />
                      </fieldset>
                      <br /><br />
                      <fieldset id="address">
                        <legend>Aplicaciones</legend>
                        <!--- inicia modulo --->
                        <div id="box">
                            <h3>Usuario:</h3>  
                            <asp:CheckBoxList ID="chkl_usuario" runat="server" RepeatColumns="4" 
                                RepeatDirection="Horizontal"></asp:CheckBoxList>
                        </div>
                        <br />
                        
                        <div id="box">
                            <h3>Perfiles</h3>  
                            <asp:CheckBoxList ID="chkl_perfiles" runat="server" RepeatColumns="4" 
                                RepeatDirection="Horizontal"></asp:CheckBoxList>
                        </div>
                        <br />
                        <div id="box">
                            <h3>Paises</h3>  
                            <asp:CheckBoxList ID="chkl_paises" runat="server" RepeatColumns="4" 
                                RepeatDirection="Horizontal"></asp:CheckBoxList>
                        </div>
                        <br />
                        <div id="box">
                            <h3>Sucursales</h3>  
                            <asp:CheckBoxList ID="chkl_sucursales" runat="server" RepeatColumns="4" 
                                RepeatDirection="Horizontal"></asp:CheckBoxList>
                        </div>
                        <br />
                        <div id="box">
                            <h3>Administración</h3>  
                            <asp:CheckBoxList ID="chkl_administracion" runat="server" RepeatColumns="4" 
                                RepeatDirection="Horizontal"></asp:CheckBoxList>
                        </div>
                        <br />
                        <div id="box">
                            <h3>Facturacion</h3>  
                            <asp:CheckBoxList ID="chkl_facturacion" runat="server" RepeatColumns="4" 
                                RepeatDirection="Horizontal"></asp:CheckBoxList>
                        </div>
                        <br />
                        <div id="box">
                            <h3>Operaciones</h3>  
                            <asp:CheckBoxList ID="chkl_operaciones" runat="server" RepeatColumns="4" 
                                RepeatDirection="Horizontal"></asp:CheckBoxList>
                        </div>
                        <br />
                        <!--- finaliza modulo --->
                      </fieldset>
                      <br />
                      <div align="center">
                      <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" onclick="bt_Enviar_Click" />
                          &nbsp;&nbsp;<input id="Cancel" type="button" value="Cancelar"  onclick="javascript:document.location.href='searchprofile.aspx';"/></div>
                </div>
</asp:Content>

