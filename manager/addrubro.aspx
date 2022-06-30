<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addrubro.aspx.cs" Inherits="manager_addrubro" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
    <h3 id="adduser">RUBROS</h3>

        <fieldset id="personal">
            <legend>Información del rubro</legend>
            <br />
            <label>ID Rubro : </label> 
                <asp:Label ID="lb_id" runat="server" Visible="true"></asp:Label>
            <br /><br />
            <label>Nombre de la cuenta: </label> 
            <asp:TextBox ID="tb_nombre" runat="server"></asp:TextBox>
            <br /><br />
            
        </fieldset>
        <br />
      <div align="center">
        <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" 
              onclick="bt_Enviar_Click" />
      </div>
</div>
<br /><br />

<fieldset id="searchresult">
    <legend></legend>
    <!--- inicia modulo --->
    <div id="box">
        <h3>Paises asociados</h3>
    <% if ((rubropais_arr != null) && (rubropais_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Nombre del pais</th>
    	            <th>Cobra Iva</th>
    	            <th>Iva Incluido</th>
                    <th width="10%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean rubrobean=null;
                for (int i = 0; i < rubropais_arr.Count; i++) {
                    rubrobean = (RE_GenericBean)rubropais_arr[i];
            %>
	            <tr>
                    <td><%=rubrobean.strC2%></td>
                    <td><% if (rubrobean.intC3 == 1) { %>SI<% } else {%>NO<% } %></td>
                    <td><% if (rubrobean.intC4 == 1) { %>SI<% } else {%>NO<% } %></td>
                    <td align="center"><a href="delete_rubropais.aspx?rub_id=<%=rubrobean.intC1 %>&pai_id=<%=rubrobean.intC2 %> "><img src="../img/icons/page_white_delete.png" /></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="#" onclick="javascript:window.open('rubropais.aspx?id=<%=rubid %>', null, 'toolbar=no,resizable=no,status=no,width=400,height=290');">Agregar un pais</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> Este rubro no se a asignado a ningun pais,<br /> es necesario realizar este procedimiento para poder utilizarlo.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="#" onclick="javascript:window.open('rubropais.aspx?id=<%=rubid %>', null, 'toolbar=no,resizable=no,status=no,width=400,height=290');">Agregar un pais</a></td>                    
                </tr>
            </thead>
        </table>
    <% } %>
    <!--- finaliza modulo --->
    </div>
  </fieldset>
</asp:Content>

