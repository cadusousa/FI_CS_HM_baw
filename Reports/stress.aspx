<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stress.aspx.cs" Inherits="Reports_stress" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 80%;
        }
        .style2
        {
            width: 70%;
        }
        .style3
        {
            height: 19px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table align="center" cellpadding="0" cellspacing="0" class="style1">
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnl_detalle" runat="server" Height="100px" ScrollBars="Vertical">
                        <table align="center" cellpadding="0" cellspacing="0" class="style2">
                            <tr>
                                <td>
                                    </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" class="style3" colspan="2">
                                    <asp:Button ID="btn_generar" runat="server" onclick="btn_generar_Click" 
                                        Text="Generar" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lbl_mensaje" runat="server" Text="--"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnl_reporte" runat="server" Height="400px" ScrollBars="Both" 
                        Width="750px">
                        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
                            AutoDataBind="true" HasToggleGroupTreeButton="False" 
    ToolPanelView="None" Width="350px" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
