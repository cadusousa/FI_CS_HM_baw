<%@ Page Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="movimiento_cuentas_contables.aspx.cs" Inherits="operations_movimiento_cuentas_contables" Title="AIMAR - BAW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box">
    <h3 id="adduser">MOVIMIENTO DE DOCUMENTOS</h3>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <table align="center" cellpadding="0" cellspacing="0" style="width: 99%">
        <tr>
            <td>
                <b>
                <br />
                Fecha</b>
                <b>Inicial</b>
                <asp:TextBox ID="tb_fecha_inicial" runat="server" Height="16px"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_inicial_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_inicial">
                </cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="tb_fecha_inicial_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_inicial">
                </cc1:MaskedEditExtender>
                &nbsp;&nbsp;&nbsp;&nbsp; <b>Fecha Final</b> &nbsp;<asp:TextBox 
                    ID="tb_fecha_final" runat="server" Height="16px"></asp:TextBox>
                <cc1:CalendarExtender ID="tb_fecha_final_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tb_fecha_final">
                </cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="tb_fecha_final_MaskedEditExtender" runat="server" 
                    Mask="99/99/9999" MaskType="Date"  Enabled="True" 
                    TargetControlID="tb_fecha_final">
                </cc1:MaskedEditExtender>
                <br />
                <b>Tipo Documento</b>
                                <asp:DropDownList ID="lb_tipo_documento" runat="server" 
                    AutoPostBack="True" 
                    onselectedindexchanged="lb_tipo_documento_SelectedIndexChanged">
                                    <asp:ListItem>Seleccione...</asp:ListItem>
                                    <asp:ListItem Value="1">Factura</asp:ListItem>
                                    <asp:ListItem Value="2">Recibo</asp:ListItem>
                                    <asp:ListItem Value="3">Nota Credito</asp:ListItem>
                                    <asp:ListItem Value="4">Nota Debito</asp:ListItem>
                                    <asp:ListItem Value="5">Provisiones</asp:ListItem>
                                    <asp:ListItem Value="6">Cheques</asp:ListItem>
                                    <asp:ListItem Value="19">Transferencias</asp:ListItem>
                                    <asp:ListItem Value="15">Ajuste Contable</asp:ListItem>
                                    <asp:ListItem Value="17">Deposito</asp:ListItem>
                                    <asp:ListItem Value="18">Nota Credito Ajuste Contable</asp:ListItem>
                                    <asp:ListItem Value="20">Nota Debito Bancario</asp:ListItem>
                                    <asp:ListItem Value="21">Nota Credito Bancaria</asp:ListItem>
                                    <asp:ListItem Value="24">Liquidaciones</asp:ListItem>
                                    <asp:ListItem Value="28">Deposito SOA</asp:ListItem>
                                    <asp:ListItem Value="31">Nota Credito A Nota de Debito</asp:ListItem>
                                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;<asp:Button ID="bt_visualizar" runat="server" 
                    Text="VISUALIZAR" onclick="bt_visualizar_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;<asp:ImageButton ID="bt_exportar_excel" runat="server" 
                    ImageUrl="~/img/icons/report.png" onclick="bt_exportar_excel_Click" 
                    ToolTip="Exportar a Excel" />
                <br />
                <br />
                <div align="center">

                <asp:Label ID="lbl_anulacion" runat="server" BackColor="Blue" Height="10px" 
                    Width="10px" ToolTip="Partida de Anulacion"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lbl_original" runat="server" BackColor="Black" Height="10px" 
                    ToolTip="Partida Original" Width="10px"></asp:Label>

                </div>
                                    <br />
                <asp:Panel ID="Panel1" runat="server" Visible="False">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                        <tr>
                            <td width="200">
                                <b>Serie</b></td>
                            <td>
                                <asp:TextBox ID="tb_serie" runat="server" Height="16px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Correlativo</b></td>
                            <td>
                                <asp:TextBox ID="tb_correlativo" runat="server" Height="16px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" Visible="False">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                        <tr>
                            <td width="200">
                                <b>Banco</b></td>
                            <td>
                                <asp:DropDownList ID="lb_bancos" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="lb_bancos_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Cuenta</b></td>
                            <td>
                                <asp:DropDownList ID="lb_banco_cuenta" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Numero de Referencia</b></td>
                            <td>
                                <asp:TextBox ID="tb_referencia" runat="server" Height="16px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" Visible="False">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                        <tr>
                            <td width="200">
                                <b>Credito Numero</b></td>
                            <td>
                                <asp:TextBox ID="tb_credito_no" runat="server" Height="16px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table align="center" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td align="center">
                            <asp:GridView ID="gv_documentos" runat="server" AllowSorting="True" 
                                EmptyDataText="No se encontraron Datos con los Criterios de Busqueda" 
                                onsorting="gv_documentos_Sorting" BackColor="White" Font-Size="X-Small" 
                                onrowcreated="gv_documentos_RowCreated">
                                <HeaderStyle BackColor="#D9ECFF" Font-Bold="True" ForeColor="Black" />
                            </asp:GridView>
                            </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 60%">
                        <tr>
                            <td align="right" width="50%">
                                <b>Debe.:</b></td>
                            <td align="right" width="50%">
                                <asp:Label ID="lbl_debe" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style=" border-bottom:solid 1px black; ">
                                <b>Haber.:</b></td>
                            <td align="right" style=" border-bottom:solid 1px black; ">
                                <asp:Label ID="lbl_haber" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" bgcolor="#D9ECFF">
                                <b>Total.:</b></td>
                            <td align="right" bgcolor="#D9ECFF">
                                <asp:Label ID="lbl_total" runat="server" Font-Bold="True" Text="0.00"></asp:Label>
                            </td>
                        </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />
</div>
</asp:Content>

