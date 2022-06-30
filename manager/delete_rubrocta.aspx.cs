using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class manager_delete_rubrocta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int rub_id = 0;
        int pai_id = 0;
        string cue_id = "";
        if ((Request.QueryString["pai_id"] != null) && (!Request.QueryString["pai_id"].Equals("0")) && (!Request.QueryString["pai_id"].Equals("")))
        {
            pai_id = int.Parse(Request.QueryString["pai_id"].ToString());
        }
        if ((Request.QueryString["rub_id"] != null) && (!Request.QueryString["rub_id"].Equals("0")) && (!Request.QueryString["rub_id"].Equals("")))
        {
            rub_id = int.Parse(Request.QueryString["rub_id"].ToString());
        }
        if ((Request.QueryString["cue_id"] != null) && (!Request.QueryString["cue_id"].Equals("")))
        {
            cue_id = Request.QueryString["cue_id"].ToString();
            int result = DB.deleteRubroCuenta(rub_id, pai_id, cue_id);
            if (result == 0)
            {
                Session["msg"] = "Error<br /><br /> Existió un error al tratar de eliminar los datos que deseaba, por favor intente de nuevo.";
                Session["url"] = "addrubro.aspx?id=" + rub_id;
                Response.Redirect("message.aspx");
            }
            else if (result == -100)
            {
                Session["msg"] = "Error<br /><br />Existió un error al tratar de eliminar los datos, por favor intente de nuevo.";
                Session["url"] = "addrubro.aspx?id=" + rub_id;
                Response.Redirect("message.aspx");
            }
            Response.Redirect("addrubro.aspx?id=" + rub_id);
        }
        else
        {
            Session["msg"] = "Error<br /><br /> Debe indicar que campo desea eliminar, por favor intente de nuevo.";
            Session["url"] = "addrubro.aspx?id=" + rub_id;
            Response.Redirect("message.aspx");
        }

    }
}
