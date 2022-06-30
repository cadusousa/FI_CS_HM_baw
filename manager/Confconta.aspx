<%@ Page Title="AIMAR - BAW"  Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="Confconta.aspx.cs" Inherits="manager_Confconta" %>

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
    <legend>Contabilidad por usuario</legend>
      <asp:CheckBoxList ID="chk_contapersonal" runat="server" RepeatColumns="2" 
            RepeatDirection="Horizontal"></asp:CheckBoxList>
      <br />
    </fieldset>
      <fieldset id="Fieldset2">
    <legend>Contabilidad por pais</legend>
      <asp:CheckBoxList ID="chk_contapais" runat="server" RepeatColumns="2" 
            RepeatDirection="Horizontal"></asp:CheckBoxList>
      <br />
    </fieldset>  
    <div align="center"> 
      <asp:Button ID="bt_enviar" runat="server" Text="Guardar" onclick="bt_enviar_Click" />&nbsp;&nbsp;
     </div>

</div>
</asp:Content>

