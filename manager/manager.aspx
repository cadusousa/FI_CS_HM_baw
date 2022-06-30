<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="manager.aspx.cs" Inherits="manager_manager" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div id="box">
    <fieldset id="searchresult">
        <legend></legend>
        <table width="600" align="center">
	        <thead>
	            <tr><th>MENU</th></tr>
	        </thead>
	        <tbody>
	            <tr><td align="left">

         


        <% 		
            
           int P_admin_pais = 0, P_admin_users = 0, P_nuevo_menu = 0;
        UsuarioBean user = null;

        user = (UsuarioBean)Session["usuario"];
            			   				   
        ArrayList Arr_Perfile = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);

        foreach (PerfilesBean Perfil in Arr_Perfile)
        {

            if (Perfil.ID == 46 || (Perfil.ID == 7)) { P_admin_pais = 1; P_nuevo_menu = 1; }

            if (Perfil.ID == 39 || (Perfil.ID == 7)) { P_admin_users = 1; P_nuevo_menu = 1; }
        }
        
        if (P_admin_users == 1)
        {
        %>
	                <a href="adduser.aspx">Crear usuario</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción que permite crear un nuevo usuario dentro del sistema.
	                <br />
	                <a href="searchuser.aspx">Buscar usuario</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción que permite buscar un usuario para editar o eliminarlo.
	                <br /><br />
	
	                <a href="addprofile.aspx">Crear perfil</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción que permite crear un nuevo perfil dentro del sistema para asociarlo a un usuario.
	                <br />
	                <a href="searchprofile.aspx">Buscar perfil</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción que permite buscar un perfil dentro del sistema para editar o eliminarlo.
	                <br /><br />
        <% 
                                                              
        }


        if (P_admin_users == 1)
        {
            %>
	                
	                <a href="addpais.aspx">Crear pais</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción que permite crear un nuevo pais dentro del sistema.
	                <br />
	                <a href="searchpais.aspx">Buscar pais</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción que permite buscar un pais dentro del sistema para editar o eliminarlo.
	                <br /><br />
	      <% } %>          
	                <a href="addsuc.aspx">Crear sucursal</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción que permite crear una nueva sucursal asociada a un pais dentro del sistema.
	                <br />
	                <a href="searchsuc.aspx">Buscar sucursal</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción que permite buscar una sucursal en un determinado pais para editar o eliminarlo.
	                <br /><br />	                
	                <a href="confoperaciones.aspx?opt=newinvoice.aspx">Configurar cuenta de ingreso factura/invoice</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de cuentas por de cargo para facturas/invoice.
	                <br /><br />
	                <a href="confoperaciones.aspx?opt=newrcpt.aspx">Configurar Recibos</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de recibos.
	                <br /><br />
	                <a href="confoperaciones.aspx?opt=Depositos.aspx">Configurar cuentas de Depositos</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de depositos monetarios.
	                <br /><br />	                
	                <a href="confoperaciones.aspx?opt=pop_notacredito.aspx">Configurar cuenta de cargo de Nota Credito a cliente</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de cuentas de cargo de NC.
	                <br /><br />	                
	                <a href="confoperaciones.aspx?opt=notacreditoprov.aspx">Configurar cuenta Nota Credito a Proveedor</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de cuentas de cargo de NC.
	                <br /><br />
	                <a href="confoperaciones.aspx?opt=provisiones.aspx">Configurar cuentas de egreso Provisiones</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de provisiones.
	                <br /><br />
	                <a href="confoperaciones.aspx?opt=pop_cheques.aspx">Configurar cheques a proveedores</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de los cheques a proveedores locales.
	                <br /><br />
	                <a href="confoperaciones.aspx?opt=liquidacion_cheques.aspx">Configurar liquidacion de cheques de anticipo</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion la liquidacion de cheques de anticipo.
	                <br /><br />
	                <a href="confoperaciones.aspx?opt=pop_transferencias_ag.aspx">Configurar transferencias bancarias a agentes</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de las transferencias para pago a agentes.
	                <br /><br />
	                <a href="confoperaciones.aspx?opt=retencion_proveedores.aspx">Configurar retenciones a proveedores</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de las retenciones aplicadas a los proveedores.
	                <br /><br />
	                <a href="confoperaciones.aspx?opt=pagar_corte.aspx">Configurar Recibo de Cortes</a>
	                <br />
	                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Opción permite la configuracion de los recibos para cortes (soas) de proveedores.
	                <br />
	            </td></tr>
	        </tbody>
	    </table>
    </fieldset>    
</div>
</asp:Content>

