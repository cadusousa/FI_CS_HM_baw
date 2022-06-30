<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addprinter.aspx.cs" Inherits="manager_addfactura" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <br />
    <div id="box">
        <h3 id="adduser">&nbsp;IMPRESORAS</h3>

	        <fieldset id="personal">
                <legend>Agregar Impresora</legend>
                <br />
                Pais:&nbsp;
                <asp:DropDownList ID="lb_pais" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_pais_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                <br />
                Sucursal:
                <asp:DropDownList ID="lb_sucursal" runat="server">
                </asp:DropDownList>
                <br />
                <br />
                Ruta-Nombre: 
                <asp:TextBox ID="tb_impresora" runat="server"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" 
                    Text="Formato: \\192.168.8.1\EPSON LX 3000"></asp:Label>
                <br /><br />
            </fieldset>
            <br /><br />
          <div align="center">
            <asp:Button ID="bt_Enviar" runat="server" Text="Guardar" 
                  onclick="bt_Enviar_Click" />&nbsp;
            <input id="Cancel" type="button" value="Cancel" onclick="javascript:history.go(-1);" />
          </div>

    </div>
    <br /><br />
&nbsp;
        
    </asp:Content>

