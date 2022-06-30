<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="addcuentabancaria.aspx.cs" Inherits="manager_addfactura" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <br />
    <div id="box">
        <h3 id="adduser">CUENTAS BANCARIAS</h3>

	        <fieldset id="personal">
                <legend>Información del documento</legend>
                <br /><br />
                <label>Tipo de documento :</label> 
                <asp:DropDownList ID="lb_tipo_doc" runat="server">
                    <asp:ListItem Value="13">Cheque</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lb_bancoID" runat="server" Text="0" Visible="False"></asp:Label>
                <br /><br />
                <label>Numero de cuenta: </label> 
                <asp:TextBox ID="tb_chequeid" runat="server" Height="16px"></asp:TextBox>
                <br /><br />
                <label>Pais :</label> 
                <asp:DropDownList ID="lb_pais" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="lb_pais_SelectedIndexChanged">
                </asp:DropDownList>
                <br /><br />
                <label>Departamento :</label> 
                <asp:DropDownList ID="lb_suc" runat="server">
                </asp:DropDownList>
                <br /><br />
                <label>Secuencia : </label>
                <asp:TextBox ID="tb_NoInicial" runat="server" Height="16px"></asp:TextBox>
                <br /><br />
                Tipo de moneda:                 <asp:DropDownList ID="lb_moneda" runat="server" Height="22px" Width="129px">
                </asp:DropDownList>
                <br /><br />
                Tipo Letra:
                <asp:TextBox ID="tb_font" runat="server" Height="16px"></asp:TextBox>
                <br /><br />
                Interlineado(para detalle de cheque): 
                <asp:TextBox ID="tb_interlineado" runat="server" Height="16px"></asp:TextBox>
                <br /><br />
                Tamaño de letra:                 <asp:TextBox ID="tb_size" runat="server" 
                    Height="16px"></asp:TextBox>
                <br />
                <br />
                Idioma:&nbsp;
                <asp:DropDownList ID="lb_idiomas" runat="server">
                </asp:DropDownList>
                <br />
                <br />
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
<fieldset id="Fieldset1">
    <legend></legend>
    <!--- inicia modulo --->
    <div id="box">
        <h3>Campos del cheque </h3>

            
        <asp:GridView ID="gv_conf_cuentas" runat="server" Width="400px" 
            AutoGenerateColumns="False" onrowdatabound="gv_conf_cuentas_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Campo">
                <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Campo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Posicion X">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_posx" runat="server" Height="16px" 
                            Text='<%# Eval("PosicionX") %>' Width="120px"></asp:TextBox>
                        <br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="tb_posx" ErrorMessage="Solo se permiten números" 
                            SetFocusOnError="True" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </ItemTemplate>
                    <ItemStyle Width="75px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Posicion Y">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_posy" runat="server" Text='<%# Eval("PosicionY") %>' 
                            Height="16px" Width="120px"></asp:TextBox>
                        <br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                            ControlToValidate="tb_posy" ErrorMessage="Solo se permiten números" 
                            SetFocusOnError="True" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </ItemTemplate>
                    <ItemStyle Width="75px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Notas">
                    <ItemTemplate>
                        <asp:TextBox ID="tb_notas" runat="server" Text='<%# Eval("Notas") %>' 
                            Height="16px" Width="120px"></asp:TextBox>
                        <br />
                    </ItemTemplate>
                    <ItemStyle Width="75px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
        <table width="400" align="center">
        <tr><td align="center"> 
            <asp:Button ID="bt_preview" runat="server" onclick="bt_preview_Click" 
                Text="Previsualizar" />
            </td></tr>
        </table>

    <!--- finaliza modulo --->
    </div>
  </fieldset>
        
    </asp:Content>

