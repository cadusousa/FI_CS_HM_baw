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

public partial class manager_deletecuentabancaria : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string cta_id = "";
        int cuentaID = 0;
        if ((Request.QueryString["ctaID"] != null) && (!Request.QueryString["ctaID"].Equals("0")) && (!Request.QueryString["ctaID"].Equals("")))
        {
            cta_id = Request.QueryString["id"].ToString();
            cuentaID = int.Parse(Request.QueryString["ctaID"].ToString());
            RE_GenericBean cuenta=DB.getDataCuenta(cta_id);
            int result = DB.deleteCuentaBancaria(cuentaID);
            if (result == 0)
            {
                Session["msg"] = "Error<br /><br /> Existió un error al tratar de eliminar los datos que deseaba, por favor intente de nuevo.";
                Session["url"] = "searchbanco.aspx";
                Response.Redirect("message.aspx");
            }
            else if (result == -100)
            {
                Session["msg"] = "Error<br /><br />Existió un error al tratar de eliminar los datos, por favor intente de nuevo.";
                Session["url"] = "searchbanco.aspx";
                Response.Redirect("message.aspx");
            }
            Response.Redirect("addbanco.aspx?id="+cuenta.intC1);
        }
        else
        {
            Session["msg"] = "Error<br /><br /> Debe indicar que campo desea eliminar, por favor intente de nuevo.";
            Session["url"] = "searchcta.aspx";
            Response.Redirect("message.aspx");
        }
    }
}
