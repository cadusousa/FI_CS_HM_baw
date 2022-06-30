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

public partial class manager_deleterubro : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int rub_id = 0;
        if ((Request.QueryString["id"] != null) && (!Request.QueryString["id"].Equals("0")) && (!Request.QueryString["id"].Equals("")))
        {
            rub_id = int.Parse(Request.QueryString["id"].ToString());
            int result = DB.deleteRubro(rub_id);
            if (result == 0)
            {
                Session["msg"] = "Error<br /><br /> Existió un error al tratar de eliminar los datos que deseaba, por favor intente de nuevo.";
                Session["url"] = "searchrubro.aspx";
                Response.Redirect("message.aspx");
            }
            else if (result == -100)
            {
                Session["msg"] = "Error<br /><br />Existió un error al tratar de eliminar los datos, por favor intente de nuevo.";
                Session["url"] = "searchrubro.aspx";
                Response.Redirect("message.aspx");
            }
            Response.Redirect("searchrubro.aspx");
        }
        else
        {
            Session["msg"] = "Error<br /><br /> Debe indicar que campo desea eliminar, por favor intente de nuevo.";
            Session["url"] = "searchrubro.aspx";
            Response.Redirect("message.aspx");
        }

    }
}
