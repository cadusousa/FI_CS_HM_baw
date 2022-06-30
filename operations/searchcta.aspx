<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="searchcta.aspx.cs" Inherits="manager_searchcta" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
    <h3 id="adduser">BÚSQUEDA DE CUENTAS CONTABLES</h3>
        <fieldset id="personal">
            <legend>Criterio de búsqueda</legend>
            <br />
            <label>Cuenta a reportar : </label> 
            <asp:TextBox ID="tb_cta_madre" runat="server"></asp:TextBox>
            <br /><br />
            <label>Nombre de la cuenta: </label> 
            <asp:TextBox ID="tb_nombre" runat="server"></asp:TextBox>
            <br /><br />
            <label>Clasificacion : </label> 
                <asp:DropDownList ID="lb_clasificacion" runat="server">
                    <asp:ListItem Selected="True">Todos</asp:ListItem>
                    <asp:ListItem Value="1">Activo</asp:ListItem>
                    <asp:ListItem Value="2">Pasivo</asp:ListItem>
                    <asp:ListItem Value="3">Patrimonio</asp:ListItem>
                  </asp:DropDownList>
            <br /><br />
            <label>Tipo : </label> 
                <asp:DropDownList ID="lb_tipo" runat="server">
                      <asp:ListItem Selected="True" Value="0">No aplica</asp:ListItem>
                      <asp:ListItem Value="1">Caja</asp:ListItem>
                      <asp:ListItem Value="2">Banco</asp:ListItem>
                  </asp:DropDownList>
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
        <h3>RESULTADO DE BÚSQUEDA LISTADO</h3>
    <% if ((cta_arr != null) && (cta_arr.Count > 0)) { %>
         <table width="600" align="center">
            <thead>
	            <tr>
    	            <th width="19%">Número de cuenta</th>
    	            <th>Nombre de cuenta</th>
    	            <th width="19%">Cuenta a reportar</th>
    	            <th width="15%">Clasificación</th>
                    <th width="10%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean ctabean=null;
                for (int i = 0; i < cta_arr.Count; i++) {
                    ctabean = (RE_GenericBean)cta_arr[i];
            %>
	            <tr>
                    <td><%=ctabean.strC1%></td>
                    <td><%=ctabean.strC2%></td>
                    <td><%=ctabean.strC3%></td>
                    <td><%=ctabean.intC2%></td>
                    <td align="center"><a href="addcta.aspx?id=<%=ctabean.intC1 %>"><img src="../img/icons/page_white_edit.png" title="Editar"/></a><a href="deletecta.aspx?id=<%=ctabean.intC1 %>"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
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

