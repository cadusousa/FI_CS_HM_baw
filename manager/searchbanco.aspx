<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="searchbanco.aspx.cs" Inherits="manager_search" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<br />
    <div id="box">
	<h3 id="adduser">BÚSQUEDA DE BANCOS</h3>

	        <fieldset id="personal">
                <legend>Criterios de búsqueda</legend>
                <br />
                <label>Nombre : </label> 
                <asp:TextBox ID="tb_nombre" runat="server"></asp:TextBox>
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
        <% if ((banco_arr!=null) && (banco_arr.Count>0)){ %>
             <table width="400" align="center">
	            <thead>
		            <tr>
        	            <th>Nombre</th>
                        <th width="20%">Acción</th>
                    </tr>
	            </thead>
	            <tbody>
	            <%  
                    foreach (RE_GenericBean banco in banco_arr) {
                %>
		            <tr>
                        <td><%=banco.strC1%></td>
                        <td align="center"><a href="addbanco.aspx?id=<%=banco.intC1 %>"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletebanco.aspx?id=<%=banco.intC1 %>"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
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

