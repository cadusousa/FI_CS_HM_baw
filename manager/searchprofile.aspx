<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="searchprofile.aspx.cs" Inherits="manager_addprofile" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
                <br />
                <div id="box">
                	<h3 id="adduser">BUSCADOR DE PERFILES</h3>
                      <fieldset id="information">
                        <legend>Información</legend>
                        <br /><br />
                        <label>Nombre : </label> 
                          <asp:TextBox ID="tb_nombre" runat="server"></asp:TextBox>
                        <br />
                        <br />
                        
                      </fieldset>
                      <div align="center">
                      <br />
                      <br />
                      <asp:Button ID="bt_enviar" runat="server" Text="Buscar" 
                              onclick="bt_enviar_Click" />
                      </div>
                      <br />
                      <fieldset id="searchresult">
                        <legend></legend>
                        <!--- inicia modulo --->
                        <div id="box">
                            <h3>RESULTADO DE BÚSQUEDA</h3>
                        <% if ((perfil_arr!=null) && (perfil_arr.Count>0)){ %>
                             <table width="400" align="center">
						        <thead>
							        <tr>
                            	        <th>Nombre del perfil</th>
                                        <th width="20%">Acción</th>
                                    </tr>
						        </thead>
						        <tbody>
						        <%  PerfilesBean perfil = null;
                                    for (int i = 0; i < perfil_arr.Count; i++) {
                                        perfil = (PerfilesBean)perfil_arr[i];
                                %>
							        <tr>
                                        <td><%=perfil.Nombre%></td>
                                        <td align="center"><a href="addprofile.aspx?id=<%=perfil.ID %>"><img src="../img/icons/page_white_edit.png"  alt="Editar"/></a><a href="deleteprofile.aspx?id=<%=perfil.ID %>"><img src="../img/icons/page_white_delete.png"  alt="Eliminar" /></a></td>
                                    </tr>
                                 <% } %>
						        </tbody>
					        </table>
					    <% } else {%>
					        <table>
					            <thead>
					                <tr>
					                    <th> No se encontraron ninguna coincidencia con este criterio de búsqueda</th>
					                </tr>
					            </thead>
					        </table>
					    <% } %>
                        <!--- finaliza modulo --->
                        </div>
                      </fieldset>
                      <br />
                        <br />
                </div>
                
</asp:Content>

