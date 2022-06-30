<%@ Page Language="C#" AutoEventWireup="true" CodeFile="print.aspx.cs" Inherits="invoice_print_invoice"  Title="AIMAR - BAW"%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<script type="text/javascript">
function refresh ()
{
    top.opener.document.location = top.opener.document.location;
    window.close();
}
</script>
<script language="JavaScript">
document.onkeydown = function(e) 
{ 
    if(e) 
    document.onkeypress = function(){return true;} 

    var evt = e?e:event; 
    if(evt.keyCode==116) 
    { 
    if(e) 
    document.onkeypress = function(){return false;} 
    else 
    { 
    evt.keyCode = 0; 
    evt.returnValue = false; 
    } 
    } 
} 
var message="Función Desactivada!";
function clickIE4()
{
    if (event.button==2)
    {
        alert(message);
        return false;
    }
}
function clickNS4(e)
{
    if (document.layers||document.getElementById&&!document.all)
    {
        if (e.which==2||e.which==3)
        {
            alert(message);
            return false;
        }
    }
}
if (document.layers)
{
    document.captureEvents(Event.MOUSEDOWN);
    document.onmousedown=clickNS4;
}
else if (document.all&&!document.getElementById)
{
    document.onmousedown=clickIE4;
}
document.oncontextmenu=new Function("alert(message);return false" );
 
</script>
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        
    
    </div>
    </form>
</body>
</html>
