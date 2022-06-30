<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rep_contrasenas.aspx.cs" Inherits="invoice_rep_contrasenas" %>
&nbsp;<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>BAW</title>
<script type="text/javascript">
    function refresh() {
        top.opener.document.location = top.opener.document.location;
        window.close();
    }
</script>
</head>
<body onLoad="javascript:self.focus();"><form id="form1" runat="server">
    <div>
    
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" HasToggleGroupTreeButton="False"  HyperlinkTarget="_blank"
            ToolPanelView="None" />
    
    </div>
    </form>
</body>
</html>
