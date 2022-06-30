<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addBanco.aspx.cs" Inherits="manager_addBancos" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
	    <h3 id="adduser">BANCO</h3>

	        <fieldset id="personal">
                <legend>Información del banco</legend>
                <br />
                <label>Banco ID : </label> 
                <asp:TextBox ID="tb_id" Enabled="false" runat="server" Text="0" Height="16px"></asp:TextBox>
                <br /><br />
                <label>Nombre : </label> 
                <asp:TextBox ID="tb_nombre" runat="server" Height="16px"></asp:TextBox>
                <br />
                <br />
                <label>Descripcion : </label> 
                <asp:TextBox ID="tb_descripcion" runat="server" Height="16px"></asp:TextBox>
                <br />
                <br />
                <br />
                <asp:CheckBoxList ID="chkl_paises" runat="server" RepeatColumns="4" 
                    RepeatDirection="Horizontal">
                </asp:CheckBoxList>
                <br />      
            </fieldset>
            <br />
          <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" 
                  onclick="bt_Enviar_Click" />
              &nbsp;&nbsp;<input id="Cancel" type="button" value="Cancelar" onclick="javascript:window.location.href='searchbanco.aspx'" />
          </div>

    </div>
    <br /><br />
    <fieldset id="Fieldset1">
    <legend></legend>
    <!--- inicia modulo --->
    <div id="Div1">
        <h3>Cuentas asociadas</h3>
    <% if ((ctas_banco_arr != null) && (ctas_banco_arr.Count > 0))
       { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Cuenta No.</th>
    	            <th>Pais Asociado</th>
    	            <th>Departamento</th>
    	            <th width="20%">Accion</th>
                    <th style=" display:none;"></th>
                </tr>
            </thead>
            <tbody>
            <%  
                foreach (RE_GenericBean cuenta in ctas_banco_arr) {
            %>
	            <tr>
                    <td align="center"><%=cuenta.strC1 %></td>
                    <td align="center"><%=cuenta.strC4 %></td>
                    <td align="center"><%=cuenta.strC3 %></td>
                    <td align="center"><a href="addcuentabancaria.aspx?id=<%=cuenta.strC1 %>&bancoID=<%=id %>&ctaID=<%=cuenta.intC9 %>"><img src="../img/icons/page_white_edit.png" title="Editar"/></a><a href="deletecuentabancaria.aspx?id=<%=cuenta.strC1 %>&ctaID=<%=cuenta.intC9 %>"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                    <td align="center" style=" display:none;"><%=cuenta.intC9 %></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="5" align="center"><a href="addcuentabancaria.aspx?id=0&bancoID=<%=id %>&ctaID=0">
                        Agregar una nueva cuenta bancaria</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningúna cuenta asociada a este banco.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addcuentabancaria.aspx?id=0&bancoID=<%=id %>&ctaID=0">
                        Agregar una nueva cuenta bancaria</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    </fieldset>
</asp:Content>

