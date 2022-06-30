<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="message.aspx.cs" Inherits="manager_message" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

<div id="box">
    <fieldset id="searchresult">
        <legend></legend>
        <table width="500" align="center">
	        <thead>
	            <tr><th>MENSAJE</th></tr>
	        </thead>
	        <tbody>
	            <tr><td align="center">
	                <% if ((Session["msg"] != null) && (!Session["msg"].Equals(""))) { %>
	                <%=Session["msg"].ToString()%>
	                <% } else { %>
	                Error<br /> Mensaje no controlado.
	                <% }%>
	                <br /><br />
	                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Aceptar" />
	            </td></tr>
	        </tbody>
	    </table>
    </fieldset>    
</div>
</asp:Content>

