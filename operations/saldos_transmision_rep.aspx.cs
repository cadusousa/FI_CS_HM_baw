using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class operations_saldos_transmision_rep : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string op = Request.QueryString["op"];
        var html = Session["SaldosTransmision"];

        //Request.QueryString["html"];

        string filename = Request.QueryString["filename"];

        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ";");
        Response.Write(html);
        Response.End();
    }
}