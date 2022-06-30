<%@ Page Language="C#" MasterPageFile="~/manager/manager.master" AutoEventWireup="true" CodeFile="plantillaRetencion.aspx.cs" Inherits="manager_addfactura" Title="AIMAR - BAW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
<br />
    <div id="box">
        <h3 id="adduser">DOCUMENTOS</h3>

	        <fieldset id="personal">
                <legend>Información del documento</legend>
                <br />
                <label>Documento ID: </label> 
                <asp:TextBox ID="tb_facid" Enabled="false" runat="server">0</asp:TextBox>
                <br /><br />
                <label>Serie : </label> 
                <asp:TextBox ID="tb_serie" runat="server"></asp:TextBox>
                <br /><br />
                <label>Secuencia : </label>
                <asp:TextBox ID="tb_NoInicial" runat="server"></asp:TextBox>
                <br />
                <br />
                Pais:
                <asp:DropDownList ID="lb_pais" runat="server" Height="22px" Width="129px">
                </asp:DropDownList>
                <br />
                <br />
                Tipo de moneda:                 <asp:DropDownList ID="lb_moneda" runat="server" Height="22px" Width="129px">
                </asp:DropDownList>
                <br />
                <br />
                Simbolo de moneda:
                <asp:TextBox ID="tb_simbolo" runat="server"></asp:TextBox>
                <br /><br />
                Tipo Letra:
                <asp:TextBox ID="tb_font" runat="server"></asp:TextBox>
                <br /><br />
                Interlineado(para detalle de factura): 
                <asp:TextBox ID="tb_interlineado" runat="server"></asp:TextBox>
                <br /><br />
                Tamaño de letra:
                <asp:TextBox ID="tb_size" runat="server"></asp:TextBox>
                <br />
                <br />
                Tipos de retencion:<br />
                <asp:CheckBoxList ID="chklist_retencion" runat="server" RepeatColumns="4" 
                    RepeatDirection="Horizontal">
                </asp:CheckBoxList>
                <br />
                <asp:Button ID="bt_agregar" runat="server" onclick="bt_agregar_Click" 
                    Text="Agregar retención" />
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
        <h3>Campos de la factura</h3>

            
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

