<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchBL_ND.aspx.cs" Inherits="invoice_searchBL"  Title="AIMAR - BAW"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>AIMAR - BAW</title>
<link rel="stylesheet" type="text/css" href="../css/theme4.css" />
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script language="javascript">
   var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
   document.writeln('<link rel="stylesheet" type="text/css" href="../css/' + StyleFile + '">');
</script>
<script type="text/javascript">
function refresh ()
{
    top.opener.document.location = top.opener.document.location;
    window.close();
}
</script>
</head>

<body onLoad="JavaScript:self.focus()">
    <div id="content">
        <form runat="server" id="form" method="post" name="frmCompletar">
        <table width="350" align="left">
            <thead>
                <tr><th>Búsqueda de ID</th></tr>
            </thead>
            <tbody>
                <tr>
                    <td align="center">
                        <asp:TextBox ID="tb_criterio" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="bt_search" runat="server" Text="Buscar" 
                            onclick="bt_search_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;&nbsp;Tipo:  
                        <asp:DropDownList ID="lb_tipo" runat="server">
                            <asp:ListItem>LCL</asp:ListItem>
                            <asp:ListItem>FCL</asp:ListItem>
                            <asp:ListItem>ALMACENADORA</asp:ListItem>
                            <asp:ListItem>AEREO</asp:ListItem>
                            <asp:ListItem>TERRESTRE T</asp:ListItem>
                            <asp:ListItem>RO ADUANAS</asp:ListItem>
                            <asp:ListItem>RO SEGUROS</asp:ListItem>                           
                            <asp:ListItem>FCL APL</asp:ListItem>
                            <asp:ListItem>DEMORAS</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="dgw1" runat="server" onrowcommand="dgw1_RowCommand" 
                            AllowPaging="True" onpageindexchanging="dgw1_PageIndexChanging" 
                            PageSize="10" 
                            EmptyDataText="No existen datos con este criterio, por favor verifique" 
                            onrowcreated="dgw1_RowCreated">
                            <Columns>
                                <asp:ButtonField HeaderText="Seleccionar" Text="Seleccionar" 
                                    CommandName="Seleccionar" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                
            </tbody>
        </table>
        </form>
    </div>
</body>
</html>
