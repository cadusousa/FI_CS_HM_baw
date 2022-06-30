<%@ Page Title="AIMAR - BAW" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="administrar_rubros.aspx.cs" Inherits="operations_administrar_rubros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
        <fieldset id="Fieldset1">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <h3 id="adduser">ADMINISTRACION DE RUBROS</h3>

         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>
        <table width="650">
        <tr>
            <td>Servicios:</td>
            <td colspan="2">
                <asp:DropDownList ID="ddl_servicio" runat="server" AutoPostBack="true" onselectedindexchanged="ddl_tipo_servico_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Rubros:</td>
            <td>
                <asp:DropDownList ID="ddl_rubro" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btn_buscar" runat="server" Text="Buscar Configuración" 
                    onclick="btn_buscar_Click" />
             </td>
        </tr>
        <tr>
            <td>Rubro:</td>
            <td>
                <asp:TextBox ID="txt_rubro" runat="server" Text="" Width="357px"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btn_crear" runat="server" Text="Crear Rubro" 
                    onclick="btn_crear_Click" />
            </td>
        </tr>
        <tr>
        <td colspan="3">
            <asp:MultiView ID="mvRubros" runat="server">
                <asp:View ID="vRubros" runat="server">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 100%" >
                        <tr>
                            <td>
                                <h3 id="H4">COMBINACION CUENTAS CONTABLES</h3>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gv_combinacionescontables" runat="server" 
                                    AutoGenerateColumns="false" ShowHeader="true" ShowHeaderWhenEmpty="true" 
                                    OnRowEditing="gv_combinacionescontables_OnRowEditing" OnRowCancelingEdit="gv_combinacionescontables_OnRowCancelingEdit"
                                              OnRowDataBound="gv_combinacionescontables_OnRowDataBound" 
                                    OnDataBound="gv_combinacionescontables_OnDataBound" 
                                    OnRowCreated="gv_combinacionescontables_OnRowCreated" 
                                    OnRowUpdating="gv_combinacionescontables_OnRowUpdating"
                                     
                                    EmptyDataText="Aun no existen combinaciones para este rubro y servicio.">
                                    <Columns>
                                        <asp:TemplateField HeaderText = "MatrizId">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_matriz_id" runat="server" Text='<%# Eval("IdMatriz")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText = "Tipo Transaccion">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_tipo_transaccion" runat="server" Text='<%# Eval("Tipo Transaccion")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lbl_tipo_transaccion" runat="server" Text='<%# Eval("Tipo Transaccion")%>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID = "ddl_tipo_transaccion" runat = "server" Width="165px">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl_tipo_transaccion" runat="server" Text='Transaccion'></asp:Label>
                                                <asp:DropDownList ID = "ddl_tipo_transaccion" runat = "server" Width="165px">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText = "Tipo Cobro">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_tipo_cobro" runat="server" Text='<%# Eval("Tipo Cobro")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lbl_tipo_cobro" runat="server" Text='<%# Eval("Tipo Cobro")%>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID = "ddl_tipo_cobro" runat = "server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl_tipo_cobro" runat="server" Text='Tipo Cobro'></asp:Label>
                                                <asp:DropDownList ID = "ddl_tipo_cobro" runat = "server">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText = "Tipo Imp/Exp">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_tipo_impexp" runat="server" Text='<%# Eval("Tipo Imp/Exp")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lbl_tipo_impexp" runat="server" Text='<%# Eval("Tipo Imp/Exp")%>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID = "ddl_tipo_impexp" runat = "server" Width="125px">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl_tipo_impexp" runat="server" Text='Tipo Imp/Exp'></asp:Label>
                                                <asp:DropDownList ID = "ddl_tipo_impexp" runat = "server" Width="125px">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText = "Cuenta">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_cuenta" runat="server" Text='<%# Eval("Cuenta")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_cuenta" runat="server" Width="80px" Text='<%# Eval("Cuenta")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl_cuenta" runat="server" Text='Cuenta'></asp:Label>
                                                <asp:TextBox ID="txt_cuenta" Width="80px" runat="server"></asp:TextBox>
                                            </HeaderTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText = "Debe">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_debe" runat="server" Text='<%# Eval("Debe")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_debe" runat="server" Width="30px" Text='<%# Eval("Debe")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl_debe" runat="server" Text='Debe'></asp:Label>
                                                <asp:TextBox ID="txt_debe" Width="30px" runat="server"></asp:TextBox>
                                            </HeaderTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText = "Haber">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_haber" runat="server" Text='<%# Eval("Haber")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_haber" runat="server" Width="30px" Text='<%# Eval("Haber")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl_haber" runat="server" Text='Haber'></asp:Label>
                                                <asp:TextBox ID="txt_haber" Width="30px" runat="server"></asp:TextBox>
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowEditButton="True" ButtonType="Image" EditImageUrl="~/img/icons/page_white_edit.png" UpdateImageUrl="~/img/icons/save.png" CancelImageUrl="~/img/icons/delete.png" />    
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Button ID="btn_nueva_combinacion" runat="server" Text="Agregar" OnClick="btn_nueva_combinacion_Click" />
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h3 id="H1">CONFIGURACION POR PAIS</h3>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gv_rubros_pais" runat="server" AutoGenerateColumns="false" OnRowEditing="gv_rubros_pais_OnRowEditing" OnRowCancelingEdit="gv_rubros_pais_OnRowCancelingEdit"
                                              OnRowDataBound="gv_rubros_pais_OnRowDataBound" OnRowCreated="gv_rubros_pais_OnRowCreated" OnRowUpdating="gv_rubros_pais_OnRowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText = "PaisId">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_pais_id" runat="server" Text='<%# Eval("PaisId")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText = "Pais">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_pais" runat="server" Text='<%# Eval("Pais")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText = "Cobra Iva?">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_cobra_iva" runat="server" Text='<%# Eval("Cobra Iva?")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lbl_cobra_iva" runat="server" Text='<%# Eval("Cobra Iva?")%>' Visible="false"></asp:Label>
                                            <asp:DropDownList ID = "ddl_cobra_iva" runat = "server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText = "Iva Incluido?">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_iva_incluido" runat="server" Text='<%# Eval("Iva Incluido?")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lbl_iva_incluido" runat="server" Text='<%# Eval("Iva Incluido?")%>' Visible="false"></asp:Label>
                                            <asp:DropDownList ID = "ddl_iva_incluido" runat = "server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText = "Es Sujeto?">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_es_sujeto" runat="server" Text='<%# Eval("Es Sujeto?")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lbl_es_sujeto" runat="server" Text='<%# Eval("Es Sujeto?")%>' Visible="false"></asp:Label>
                                            <asp:DropDownList ID = "ddl_es_sujeto" runat = "server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowEditButton="True" ButtonType="Image" EditImageUrl="~/img/icons/page_white_edit.png" UpdateImageUrl="~/img/icons/save.png" CancelImageUrl="~/img/icons/delete.png" /> 
                                </Columns>

                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="View1" runat="server">
                </asp:View>
            </asp:MultiView>
        </td>
        </tr>
        </table>
         </ContentTemplate>
         </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>

