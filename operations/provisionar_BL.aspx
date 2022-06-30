<%@ Page Language="C#" AutoEventWireup="true" CodeFile="provisionar_BL.aspx.cs" Inherits="operations_provisionar_BL2" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AIMAR - BAW</title>
    <style type="text/css">
    .container
    {
        font-family:Arial;
        font-size:x-small;
    }
        .style1
        {
            font-size: small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style=" vertical-align:middle;" align="center" class="container">
    <asp:Panel ID="pnl_costos" runat="server" Width="70%">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 70%">
        <tr>
            <td>
                <table align="center" cellpadding="0" cellspacing="0" style="width: 50%">
                    <tr>
                        <td align="center" rowspan="5" valign="middle">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/aimar.jpg" Width="150px"  />                            
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>MBL</b></td>
                        <td width="15">
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="lbl_mbl" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>SISTEMA</b></td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="lbl_sistema" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>TIPO</b></td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="lbl_tipo_operacion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>PAIS</b></td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="lbl_pais" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <table align="center" cellpadding="0" cellspacing="0" style="width: 85%; border:solid 1px black;">
                    <tr>
                        <td align="center" bgcolor="#66CCFF" class="style1">
                            <b>COSTOS</b></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Panel ID="pnl_costos_bl" runat="server" ScrollBars="Auto" Width="600px">
                                <asp:GridView ID="gv_costos" runat="server" GridLines="Vertical" 
                                    EmptyDataText="No existen costos a Provisionar">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                            No.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="Black" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#66CCFF" />
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </asp:Panel>
    </div>
    <cc1:RoundedCornersExtender ID="pnl_costos_RoundedCornersExtender" 
        runat="server" BorderColor="Black" Enabled="True" TargetControlID="pnl_costos">
    </cc1:RoundedCornersExtender>
    </form>
</body>
</html>
