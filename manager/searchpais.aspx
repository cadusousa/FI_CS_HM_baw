<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="searchpais.aspx.cs" Inherits="manager_searchpais" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
        <h3 id="adduser">BUSQUEDA DE PAISES</h3>
	        <fieldset id="personal">
                <legend>Criterios de búsqueda</legend>
                <br />
                <label>Nombre : </label> 
                <asp:TextBox ID="tb_nombre" runat="server"></asp:TextBox>
                <br />
                <br />
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
            <h3>RESULTADO DE BÚSQUEDA DE PAISES</h3>
        <% if ((pais_arr!=null) && (pais_arr.Count>0)){ %>
             <table width="400" align="center">
	            <thead>
		            <tr>
        	            <th>Nombre</th>
                        <th width="20%">Acción</th>
                    </tr>
	            </thead>
	            <tbody>
	            <%  PaisBean pais = null;
                    for (int i = 0; i < pais_arr.Count; i++) {
                        pais = (PaisBean)pais_arr[i];
                %>
		            <tr>
                        <td><%=pais.Nombre%></td>
                        <td align="center"><a href="addpais.aspx?id=<%=pais.ID %>"><img src="../img/icons/page_white_edit.png" /></a><a href="deletepais.aspx?id=<%=pais.ID %>"><img src="../img/icons/page_white_delete.png" /></a></td>
                    </tr>
                 <% } %>
	            </tbody>
            </table>
        <% } else {%>
            <table width="400" align="center">
                <thead>
                    <tr>
                        <th align="center"> No se encontraron ninguna coincidencia con este criterio de búsqueda</th>
                    </tr>
                </thead>
            </table>
        <% } %>
        <!--- finaliza modulo --->
        </div>
      </fieldset>
</asp:Content>

