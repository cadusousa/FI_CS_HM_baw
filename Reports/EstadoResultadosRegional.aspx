<%@ Page Title="AIMAR - BAW" Language="C#" AutoEventWireup="true" CodeFile="EstadoResultadosRegional.aspx.cs" Inherits="Reports_EstadoResultadosRegional" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body onLoad="javascript:self.focus();">
    <form id="form1" runat="server">
    <div>
    
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" HyperlinkTarget="_blank" ToolPanelView="None" />
    
    </div>
    </form>
</body>
</html>
