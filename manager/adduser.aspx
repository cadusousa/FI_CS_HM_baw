<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="adduser.aspx.cs" Inherits="users_adduser" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <br />
<div id="box">
    <h3 id="adduser">USUARIO</h3>

    <fieldset id="personal">
        <legend>Información personal</legend>
         <table width="700">
        <tr>
            <td>
                Usuario:&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="lb_usuarios" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="lb_usuarios_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                Pais: <asp:DropDownList ID="lb_pais" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_pais_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
            </td>
        </tr>

        </table>
    </fieldset>
    
    <fieldset id="Fieldset1">
    <legend>Sucursales</legend>
      <asp:CheckBoxList ID="chk_sucursales" runat="server" RepeatColumns="2" 
            RepeatDirection="Horizontal"></asp:CheckBoxList>
      <br />
    </fieldset>
    
    <fieldset id="Fieldset2">
    <legend>Departamentos</legend>
      <asp:CheckBoxList ID="chk_deptos" runat="server" RepeatColumns="2" 
            RepeatDirection="Horizontal"></asp:CheckBoxList>
      <br />
    </fieldset>         
            
    <fieldset id="opt">
    <legend>Perfiles</legend>
      <asp:CheckBoxList ID="chk_list" runat="server" RepeatColumns="3" 
            RepeatDirection="Horizontal"></asp:CheckBoxList>
      <br />
    </fieldset>
    
    <fieldset id="opt">
    <legend>Operaciones</legend>
      <asp:CheckBoxList ID="chk_operaciones" runat="server" RepeatColumns="3" 
            RepeatDirection="Horizontal"></asp:CheckBoxList>
      <br />
    </fieldset>
                      
    <div align="center"> 
      <asp:Button ID="bt_enviar" runat="server" Text="Guardar" onclick="bt_enviar_Click" />&nbsp;&nbsp;
      <input id="Cancel" type="button" value="Cancelar" onclick="javascript:document.location.href='searchuser.aspx';"/></div>

</div>                
</asp:Content>