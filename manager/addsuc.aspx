<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addsuc.aspx.cs" Inherits="manager_addsuc" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<br />
    <div id="box">
	    <h3 id="adduser">DEPARTAMENTOS</h3>

	        <fieldset id="personal">
                <legend>Información del departamento</legend>
                <br />
                <label>Departamento ID : </label> 
                <asp:TextBox ID="tb_id" Enabled="false" runat="server" Height="16px"></asp:TextBox>
                <br /><br />
                <label>Nombre : </label> 
                <asp:TextBox ID="tb_nombre" runat="server" Height="16px"></asp:TextBox>
                <br />
                <br />
                <label>Pais : </label> 
                <asp:DropDownList ID="lb_pais" runat="server"></asp:DropDownList>
                <br /><br />      
            </fieldset>
            <br />
          <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" 
                  onclick="bt_Enviar_Click" />
            &nbsp;&nbsp;<input id="Cancel" type="button" value="Cancelar" onclick="javascript:window.location.href='searchsuc.aspx'" />
          </div>

    </div>
    <br /><br />
<fieldset id="Fieldset1">
    <legend></legend>
    <!--- inicia modulo --->
    <div id="box">
        <h3>Facturas asociadas</h3>
    <% if ((fac_arr != null) && (fac_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie de la factura</th>
    	            <th>Número de la factura inicial</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < fac_arr.Count; i++) {
                    facbean = (RE_GenericBean)fac_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=1"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=1"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=1">Agregar un nuevo grupo de facturas</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna factura definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=1">Agregar un nuevo grupo de facturas</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Recibos asociadas</h3>
    <% if ((rec_arr != null) && (rec_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie del</th>
    	            <th>Número inicial del recibo</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < rec_arr.Count; i++) {
                    facbean = (RE_GenericBean)rec_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=2"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=2"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=2">Agregar un nuevo grupo de recibos</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de algun recibo definido para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=2">Agregar un nuevo grupo de recibos</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Notas de credito asociadas</h3>
    <% if ((nc_arr != null) && (nc_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie de la nota de credito </th>
    	            <th>Número de la nota de credito </th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < nc_arr.Count; i++) {
                    facbean = (RE_GenericBean)nc_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=3"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=3"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=3">Agregar un nuevo grupo de notas de credito</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna nota de credito definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=3">Agregar un nuevo grupo de notas de credito</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Notas de Debito asociadas</h3>
    <% if ((nd_arr != null) && (nd_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie de la nota de debito</th>
    	            <th>Número inicial de la nota de debito</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < nd_arr.Count; i++) {
                    facbean = (RE_GenericBean)nd_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=4"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=4"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=4">Agregar un nuevo grupo de nota de debito</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna nota de debito definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=4">Agregar un nuevo grupo de nota de debito</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Provisiones</h3>
    <% if ((pa_arr != null) && (pa_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie de la contraseña</th>
    	            <th>Número inicial de la contraseña</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < pa_arr.Count; i++) {
                    facbean = (RE_GenericBean)pa_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=5"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=5"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=5">Agregar un nuevo grupo de ordenes de compra</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna contraseña definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=5">Agregar un nuevo grupo de contraseñas</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Ordenes de Compra</h3>
    <% if ((oc_arr != null) && (oc_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie de la orden de compra</th>
    	            <th>Número inicial de la orden de compra</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < oc_arr.Count; i++) {
                    facbean = (RE_GenericBean)oc_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=7"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=7"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=7">Agregar un nuevo grupo de ordenes de compra</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna orden de compra definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=7">Agregar un nuevo grupo de ordenes de compra</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Contraseña de facturas</h3>
    <% if ((cf_arr != null) && (cf_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie de la contraseña</th>
    	            <th>Número inicial de la contraseña</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < cf_arr.Count; i++) {
                    facbean = (RE_GenericBean)cf_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=8"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=8"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=8">Agregar un nuevo grupo de ordenes de compra</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna contraseña definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=8">Agregar un nuevo grupo de contraseñas</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Retenciones</h3>
    <% if ((ret_arr != null) && (ret_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie de la contraseña</th>
    	            <th>Número inicial de la contraseña</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < ret_arr.Count; i++) {
                    facbean = (RE_GenericBean)ret_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=20"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=9"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=20">Agregar un nuevo grupo de ordenes de compra</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna contraseña definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=20">Agregar un nuevo grupo de contraseñas</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Serie de corte para estado de cuenta proveedores</h3>
    <% if ((ecp_arr != null) && (ecp_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie</th>
    	            <th>Número inicial</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < ecp_arr.Count; i++) {
                    facbean = (RE_GenericBean)ecp_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=11"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=11"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=11">Agregar un nueva serie</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna serie definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=11">Agregar un nueva serie</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Serie nota credito proveedores</h3>
    <% if ((ncp_arr != null) && (ncp_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie</th>
    	            <th>Número inicial</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < ncp_arr.Count; i++) {
                    facbean = (RE_GenericBean)ncp_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=12"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=12"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=12">Agregar un nueva serie</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna serie definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=12">Agregar un nueva serie</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
        <!--- finaliza modulo --->
    <!--- inicia modulo --->

    <div id="box">
        <h3>Serie nota credito a Nota debito Provedores</h3>
    <% if ((ncnd_arr != null) && (ncnd_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie</th>
    	            <th>Número inicial</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < ncnd_arr.Count; i++) {
                    facbean = (RE_GenericBean)ncnd_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=31"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=31"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=31">Agregar un nueva serie</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna serie definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=31">Agregar un nueva serie</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Serie proforma</h3>
    <% if ((prof_arr != null) && (prof_arr.Count > 0)) { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie</th>
    	            <th>Número inicial</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < prof_arr.Count; i++) {
                    facbean = (RE_GenericBean)prof_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=14"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=14"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=14">Agregar un nueva serie</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna serie definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=14">Agregar un nueva serie</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Ajustes Contables</h3>
    <% if ((ajcon_arr != null) && (ajcon_arr.Count > 0))
       { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie</th>
    	            <th>Número inicial</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < ajcon_arr.Count; i++)
                {
                    facbean = (RE_GenericBean)ajcon_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=15"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=15"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=15">Agregar un nueva serie</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna serie definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=15">Agregar un nueva serie</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
    <!--- inicia modulo --->
    <div id="box">
        <h3>Ajustes Contables NC</h3>
    <% if ((ajconNC_arr != null) && (ajconNC_arr.Count > 0))
       { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie</th>
    	            <th>Número inicial</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < ajconNC_arr.Count; i++)
                {
                    facbean = (RE_GenericBean)ajconNC_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=18"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=18"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=18">Agregar un nueva serie</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de alguna serie definida para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=18">Agregar un nueva serie</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->
<!--- inicia modulo --->
    <div id="box">
        <h3>Recibos de Corte asociadas</h3>
    <% if ((rec_corte_arr != null) && (rec_corte_arr.Count > 0))
       { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie del</th>
    	            <th>Número inicial del recibo</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < rec_corte_arr.Count; i++)
                {
                    facbean = (RE_GenericBean)rec_corte_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=22"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=2"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=22">Agregar un nuevo grupo de recibos</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de algun recibo definido para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=22">Agregar un nuevo grupo de recibos</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->    
 <!--- inicia modulo --->
    <div id="box">
        <h3>Liquidaciones</h3>
    <% if ((Liq_arr != null) && (Liq_arr.Count > 0))
       { %>
         <table width="400" align="center">
            <thead>
	            <tr>
    	            <th>Serie de la</th>
    	            <th>Número inicial de la Liquidacion</th>
    	            <th width="20%">Acción</th>
                </tr>
            </thead>
            <tbody>
            <%  RE_GenericBean facbean=null;
                for (int i = 0; i < Liq_arr.Count; i++)
                {
                    facbean = (RE_GenericBean)Liq_arr[i];
            %>
	            <tr>
                    <td align="center"><%=facbean.strC1 %></td>
                    <td align="center"><%=facbean.intC3 %></td>
                    <td align="center"><a href="addfactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=24"><img src="../img/icons/page_white_edit.png" title="Editar"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="deletefactura.aspx?id=<%=facbean.intC1 %>&suc_id=<%=id %>&tipo=24"><img src="../img/icons/page_white_delete.png" title="Eliminar"/></a></td>
                </tr>
             <% } %>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?id=0&suc_id=<%=id %>&tipo=24">Agregar un nuevo grupo de Liquidaciones</a></td>
                </tr>
            </tbody>
        </table>
    <% } else {%>
        <table width="400" align="center">
            <thead>
                <tr>
                    <th> No existe ningún registro de liquidaciones definido para este departamento.</th>
                </tr>
                <tr>
                    <td colspan="4" align="center"><a href="addfactura.aspx?suc_id=<%=id %>&tipo=24">Agregar un nuevo grupo de Liquidaciones</a></td>
                </tr>
            </thead>
        </table>
    <% } %>
    </div>
    <!--- finaliza modulo --->       
  </fieldset>
    <br />
    <br />
</asp:Content>

