using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class admin : System.Web.UI.MasterPage
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((!Page.IsPostBack) || (user == null))
        {
            if (Session["usuario"] != null)
            {
                user = (UsuarioBean)Session["usuario"];
            }
        }
        user.Idioma = DB.getIdiomaByPaisConta(user);
        Session["usuario"] = user;
    }

}
