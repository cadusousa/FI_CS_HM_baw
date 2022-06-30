<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="searchsuc.aspx.cs" Inherits="manager_search" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<br />
    <div id="box">
	<h3 id="adduser">BÚSQUEDA DE SUCURSALES</h3>

	        <fieldset id="personal">
                <legend>Criterios de búsqueda</legend>
                <br />
                <label>Nombre : </label> 
                <asp:TextBox ID="tb_nombre" runat="server" Height="16px"></asp:TextBox>
                <br /><br />
                <label>Pais : </label> 
                <asp:DropDownList ID="lb_pais" runat="server"></asp:DropDownList>
                <br /><br />
            </fieldset>
           <br />            
          <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Buscar" 
                  onclick="bt_Enviar_Click" />
          </div>

    </div>
    <br />
    <br />
    <fieldset id="searchresult">
        <legend></legend>
        <!--- inicia modulo --->
        <div id="box">
            <h3>RESULTADO DE BÚSQUEDA</h3>
        <% if ((sucursal_arr!=null) && (sucursal_arr.Count>0)){ %>
             <table width="400" align="center">
	            <thead>
		            <tr>
        	            <th>Nombre</th>
                        <th width="20%">Acción</th>
                    </tr>
	            </thead>
	            <tbody>
	            <%  SucursalBean sucursal = null;
                    for (int i = 0; i < sucursal_arr.Count; i++) {
                        sucursal = (SucursalBean)sucursal_arr[i];
                %>
		            <tr>
                        <td><%=sucursal.Nombre%></td>
                        <td align="center"><a href="addsuc.aspx?id=<%=sucursal.ID %>"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <% UsuarioBean user = null;
                            user = (UsuarioBean)Session["usuario"];
                            if ((user.ID == "dennis-ariana") || (user.ID == "dreiby-giron") || (user.ID == "cesar-sanchez"))
                            {
                         %>
                        <a href="deletesuc.aspx?id=<%=sucursal.ID %>"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a>
                        <%} %>
                        </td>
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
</asp:Content>

