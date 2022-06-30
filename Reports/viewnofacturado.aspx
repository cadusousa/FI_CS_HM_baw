<%@ Page Language="C#" AutoEventWireup="true" CodeFile="viewnofacturado.aspx.cs" Inherits="Reports_viewnofacturado" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AIMAR BAW</title>
<script type="text/javascript">
    function refresh() {
        top.opener.document.location = top.opener.document.location;
        window.close();
    }
</script>
</head>
<body onload="javascript:self.focus();">
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="True" Height="50px" Width="350px" />
    </div>
    </form>
</body>
</html>
