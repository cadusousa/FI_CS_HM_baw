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

public partial class manager_addagenteconvenio : System.Web.UI.Page
{
    public ArrayList convenios_arr = null;
    public int agenteID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        string ag = "";
        if (!Page.IsPostBack) {
            ListItem li = null;
            ArrayList agente_arr = (ArrayList)DB.getAgentesList();
            if ((agente_arr != null) && (agente_arr.Count > 0)) {
                foreach (RE_GenericBean agente in agente_arr) {
                    if (agente.strC1.Length > 60)
                        ag = agente.strC1.Substring(0, 60)+" ... ";
                    else
                        ag = agente.strC1;
                    li = new ListItem(ag, agente.intC1.ToString());
                    lb_agente.Items.Add(li);
                } 
            }
            if ((Request.QueryString["id"] != null) && (!Request.QueryString["id"].ToString().Equals("")))
            {
                agenteID = int.Parse(Request.QueryString["id"].ToString().Trim());
                lb_agente.SelectedValue = agenteID.ToString();
                
                convenios_arr = (ArrayList)DB.getAgentesConvenios(agenteID,0);
            }
        }
    }
    protected void lb_agente_SelectedIndexChanged(object sender, EventArgs e)
    {
        agenteID = int.Parse(lb_agente.SelectedValue.Trim());
        Response.Redirect("addagenteconvenio.aspx?id="+agenteID);
    }
}
