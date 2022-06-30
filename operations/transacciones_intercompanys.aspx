<%@ Page Title="" Language="C#" MasterPageFile="~/operations/manager.master" AutoEventWireup="true" CodeFile="transacciones_intercompanys.aspx.cs" Inherits="operations_transacciones_intercompanys" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" Runat="Server">
    <div id="box" align="center">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 98%">
        <tr>
            <td align="center" height="35" 
                style="font-weight: bold; font-size: medium; color: #CC0000;">
                TRANSACCIONES INTERCOMPY OPERATIVO</td>
        </tr>
        <tr>
            <td align="center">
                <asp:Panel ID="pnl_intercompany_operativo" runat="server" Height="270px" 
                    ScrollBars="Both" Width="700px">
                    <asp:GridView ID="gv_transacciones_intercompany_operativo" runat="server" 
                        CellPadding="4" Font-Size="XX-Small" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                        <SortedAscendingCellStyle BackColor="#FDF5AC" />
                        <SortedAscendingHeaderStyle BackColor="#4D0000" />
                        <SortedDescendingCellStyle BackColor="#FCF6C0" />
                        <SortedDescendingHeaderStyle BackColor="#820000" />
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="center" 
                style="font-weight: bold; font-size: medium; color: #0000FF;" height="35">
                TRANSACCIONES INTERCOMPANY ADMINISTRATIVO
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Panel ID="pnl_intercompany_administrativo" runat="server" Height="300px" 
                    ScrollBars="Both" Width="700px">
                    <asp:GridView ID="gv_transacciones_intercompany_administrativo" runat="server" 
                        CellPadding="4" Font-Size="XX-Small" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btn_actualizar" runat="server" Text="Actualizar"  
                    PostBackUrl="~/operations/transacciones_intercompanys.aspx" />
            </td>
        </tr>
        </table>
</div>
</asp:Content>

