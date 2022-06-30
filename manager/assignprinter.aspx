<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="assignprinter.aspx.cs" Inherits="manager_addfactura" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <br />
    <div id="box">
        <h3 id="adduser">&nbsp;IMPRESORAS</h3>

	        <fieldset id="personal">
                <legend>Asignar Impresora</legend>
                <br />
                Pais:&nbsp;
                <asp:DropDownList ID="lb_pais" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_pais_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                <br />
                Usuario:
                <asp:DropDownList ID="lb_usuario" runat="server">
                </asp:DropDownList>
                <br />
                <br />
                Impresora:
                <asp:DropDownList ID="lb_impresora" runat="server">
                </asp:DropDownList>
                <br />
                <br />
                Impresora Default:
                <asp:DropDownList ID="lb_impresora_default" runat="server">
                </asp:DropDownList>
                &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" />
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

