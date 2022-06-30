<%@ Page Language="C#" AutoEventWireup="true" CodeFile="balance_general.aspx.cs" Inherits="Reports_balance"  Title="AIMAR - BAW"%>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BAW</title>
</head>
<body onLoad="javascript:self.focus();">
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="lbl_pasivo_capital" runat="server" Visible="False"></asp:Label>
    
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" HyperlinkTarget="_blank" ToolPanelView="None" />
    
        <asp:Label ID="lb_resultado_ejercicio" runat="server" Visible="False"></asp:Label>
    
    </div>
    </form>
</body>
</html>
