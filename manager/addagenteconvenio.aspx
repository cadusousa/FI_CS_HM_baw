<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addagenteconvenio.aspx.cs" Inherits="manager_addagenteconvenio" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<br />
    <div id="box">
        <h3 id="adduser">AGENTES</h3>

	        <fieldset id="personal">
                <legend>Información del convenio con el agente</legend>
                                <br />
                <label>Agente: </label>
                <asp:DropDownList ID="lb_agente" runat="server" 
                    onselectedindexchanged="lb_agente_SelectedIndexChanged" 
                    AutoPostBack="True"></asp:DropDownList>
                <br /><br />
            </fieldset>
    </div>
    <br />
<fieldset id="Fieldset1">
    <legend></legend>
    <!--- inicia modulo --->
    <div id="Div1">
        <h3>Convenios sobre este agente</h3>
    <% if ((convenios_arr != null) && (convenios_arr.Count > 0))
       { %>
         <table width="500" align="center">
            <thead>
	            <tr>
	                <th width="150">Origen</th>
    	            <th width="200">Tipo de convenio</th>
    	            <th width="100">Valor</th>
    	            <th width="50">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  
                foreach (RE_GenericBean agente_convenio in convenios_arr) {
            %>
	            <tr>
                    <td><%=agente_convenio.intC2%></td>
                    <td><%=agente_convenio.strC1 %></td>
                    <td><%=agente_convenio.decC1 %></td>
                    <td><a href="deleteconvenio.aspx?id=<%=agente_convenio.intC1 %>"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="#" onclick="javascript:window.open('addconvenio.aspx?agenteid=<%=agenteID %>', null, 'toolbar=no,resizable=no,status=no,width=400,height=280');">
                        Agregar un nuevo convenio</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de haber configurado un tipo de convenio para este 
                        agente.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="#" onclick="javascript:window.open('addconvenio.aspx?agenteid=<%=agenteID %>', null, 'toolbar=no,resizable=no,status=no,width=400,height=280');">
                        Agregar un nuevo convenio</a></td>                    
                </tr>
            </thead>
        </table>
    <% } %>
    <!--- finaliza modulo --->
    </div>
  </fieldset>
    <br />
    <br />
</asp:Content>

