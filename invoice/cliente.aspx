<%@ Page Language="C#" MasterPageFile="~/invoice/manager.master" AutoEventWireup="true" CodeFile="cliente.aspx.cs" Inherits="invoice_cliente" Title="AIMAR - BAW"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">

    <div id="box">
        <h3 id="adduser">Resumen de operaciones del cliente</h3>
        
        <table width="650" align="left">
        <tbody>
            <tr>
                <td>
                    Codigo:<asp:TextBox ID="tb_clientid" runat="server" Height="16px" 
                        Width="87px" ReadOnly="True"></asp:TextBox>
                    &nbsp;<input id="Button1" type="button" value="Buscar" onclick="javascript:window.open('pop_buscarcliente.aspx?pagina=cliente.aspx', null, 'toolbar=no,resizable=no,status=no,width=400,height=350');" />Cliente:<asp:TextBox 
                        ID="tb_clientname" runat="server" Height="16px" Width="372px" 
                        ReadOnly="True"></asp:TextBox><br />
                    Recibo a nombre de:<asp:TextBox ID="tb_recibonombre" runat="server" 
                        Height="16px" Width="355px"></asp:TextBox><br />                        
                </td>
            </tr>
            <tr>
                <td>
                    <h3 id="H1">Facturas de este cliente</h3>       
                    <asp:GridView ID="dgw_aplicada" runat="server" 
                        onrowcommand="dgw_aplicada_RowCommand">
                        <Columns>
                            <asp:ButtonField HeaderText="Nota Credito" Text="Generar Nota Credito" 
                                CommandName="Generar Nota Credito" />
                        </Columns>
                    </asp:GridView>
                                               
                </td>
            </tr>
            <tr>
                <td>
                        &nbsp;</td>
            </tr>
        </tbody>
        </table>
    </div>
</asp:Content>

