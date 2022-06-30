<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="searchsuc.aspx.cs" Inherits="manager_search" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <fieldset id="searchresult">
        <legend></legend>
        <div id="box">
            <h3>SUCURSALES</h3>
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
                        <td align="center"><a href="addsuc.aspx?id=<%=sucursal.ID %>"><img src="../img/icons/page_white_edit.png" title="Editar"/></a></td>
                    </tr>
                 <% } %>
	            </tbody>
            </table>
        <% } else {%>
        <% } %>
        <!--- finaliza modulo --->
        </div>
      </fieldset>
</asp:Content>

