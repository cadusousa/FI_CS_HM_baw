<%@ Page Title="" Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="index_contabilizacion_automatica.aspx.cs" Inherits="manager_index_contabilizacion_automatica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<div align="center">
    <table width="400" align="center">
	            <thead>
		            <tr>
        	            <th>Contabilizacion Automatica</th>
                    </tr>
	            </thead>
	            <tbody>
		            <tr>
                        <td><a href="ingresar_caso.aspx">Ingresar Caso</a></td>
                    </tr>
                    <tr>
                        <td><a href="ingresar_tipo_transaccion.aspx">Ingresar Tipo de Transaccion</a></td>
                    </tr>
                    <tr>
                        <td><a href="ingresar_tipo_operacion.aspx">Ingresar Tipo de Operacion</a></td>
                    </tr>
                    <tr>
                        <td><a href="ingresar_transaccion.aspx">Ingresar Transacciones</a></td>
                    </tr>
                    <tr>
                        <td><a href="asociar_transacciones_contabilizacion_automatica.aspx">Asociar Transacciones Contabilizacion Automatica</a></td>
                    </tr>
                    <tr>
                        <td><a href="configurar_contabilizacion_automatica_pais.aspx">Configurar Contabilizacion Automatica por Pais</a></td>
                    </tr>
                    <tr>
                        <td><a href="configurar_transaccion_contabilizacion_automatica.aspx">Configurar Transaccion Contabilizacion Automatica</a></td>
                    </tr>
                    <tr>
                        <td><a href="configurar_agentes_contabilizan_house.aspx">Configurar Agentes Contabilizan por House</a></td>
                    </tr>
                    <tr>
                        <td><a href="configurar_rubros_encadenados.aspx">Configurar Rubros Generan Transaccion Encadenada</a></td>
                    </tr>
	            </tbody>
            </table>
</div>
</asp:Content>

