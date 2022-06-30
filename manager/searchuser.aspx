<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="searchuser.aspx.cs" Inherits="manager_searchuser" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<br />
<div id="box">
	<h3 id="adduser">USUARIO BUSCADOR</h3>
          <fieldset id="personal">
            <legend>Información personal</legend>
            <br />
            <label for="lastname">Usuario : </label> 
            <asp:TextBox ID="tb_user" runat="server"></asp:TextBox>
            <br /><br />
            
            <label for="lastname">Nombre : </label> 
            <asp:TextBox ID="tb_nombre" runat="server"></asp:TextBox>
            <br /><br />
            <label for="lastname">Pais : </label> 
            <asp:DropDownList ID="lb_pais" runat="server" onselectedindexchanged="lb_pais_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
            <br /><br />
            <label for="lastname">Sucursal : </label> 
            <asp:DropDownList ID="lb_sucursal" runat="server"></asp:DropDownList>
            <br /><br />
            <label for="lastname">Estado (Activo/Inactivo) : </label> 
              <asp:CheckBox ID="chk_estado" Checked="true" runat="server" />
            <br /><br />
          </fieldset>
          <br />
          <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Buscar" 
                  onclick="bt_Enviar_Click" />
          </div>
          <br />
    </div>
          
          <br />
          <fieldset id="searchresult">
        <legend></legend>
        <!--- inicia modulo --->
        <div id="box">
            <h3>RESULTADO DE BÚSQUEDA</h3>
        <% if ((usuario_arr!=null) && (usuario_arr.Count>0)){ %>
             <table width="700" align="center">
	            <thead>
		            <tr>
        	            <th width="50">Usuario</th>
        	            <th width="230">Nombre</th>
        	            <th width="135">Pais</th>
        	            <th width="185">Sucursal</th>
        	            <th width="50">Estado</th>
                        <th width="50">Acción</th>
                    </tr>
	            </thead>
	            <tbody>
	            <%  UsuarioBean usuario = null;
                    PaisBean pais = null;
                    SucursalBean sucursal = null;
                    for (int i = 0; i < usuario_arr.Count; i++) {
                        usuario = (UsuarioBean)usuario_arr[i];
                        pais = (PaisBean)DB.getPais(usuario.PaisID);
                        sucursal = (SucursalBean)DB.getSucursal(usuario.SucursalID);
                %>
		            <tr>
                        <td><%=usuario.ID%></td>
                        <td><%=usuario.Nombre%></td>
                        <td><%=pais.Nombre%></td>
                        <td><%=sucursal.Nombre%></td>
                        <td><%=usuario.Estado%></td>
                        <td align="center"><a href="adduser.aspx?id=<%=usuario.ID %>"><img src="../img/icons/user_edit.png" title="Editar" width="16" height="16" /></a><a href="deleteuser.aspx?id=<%=usuario.ID %>"><img src="../img/icons/user_delete.png" title="Eliminar" width="16" height="16" /></a></td
                    </tr>
                 <% } %>
	            </tbody>
            </table>
        <% } else {%>
            <table width="500" align="center">
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
    </asp:Content>

