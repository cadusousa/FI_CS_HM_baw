<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="searchRubro.aspx.cs" Inherits="manager_searchRubro" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
    <h3 id="adduser">BÚSQUEDA DE RUBROS</h3>
        <fieldset id="personal">
            <legend>Criterio de búsqueda</legend>
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
    <% if ((rubro_arr!=null) && (rubro_arr.Count>0)){ %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Nombre</th>
                    <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean rubean = null;
                for (int i = 0; i < rubro_arr.Count; i++) {
                    rubean = (RE_GenericBean)rubro_arr[i];
            %>
	            <tr>
                    <td><%=rubean.strC1%></td>
                    <td align="center"><a href="addrubro.aspx?id=<%=rubean.intC1 %>"><img src="../img/icons/page_white_edit.png" title="Editar"/></a><a href="deleterubro.aspx?id=<%=rubean.intC1 %>"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
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

