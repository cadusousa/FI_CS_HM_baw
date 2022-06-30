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

public partial class invoice_searchBL_rcpt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
    }
    protected void bt_search_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;
        for (int i = 0; i < ch_bl.Items.Count; i++)
        {
            item = (ListItem)ch_bl.Items[i];
            if (item.Selected)
            {
                if (item.Value.Equals("BL_MASTER"))
                {
                    lista += 1;

                }
                if (item.Value.Equals("BL_HOUSE"))
                {
                    lista += 2;
                }
                if (item.Value.Equals("ROUTING"))
                {
                    lista += 4;
                }
                if (item.Value.Equals("CONTENEDOR"))
                {
                    lista += 8;
                }
                if (item.Value.Equals("PICKING"))
                {
                    lista += 16;
                }
            }
        }
        if (lb_tipo.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio.Text, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio.Text, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio.Text, user.pais.schema, codDistribucion, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
    }

    protected void dgw1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Seleccionar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Response.Write("<script languaje='JavaScript'>window.opener.document.aspnetForm.ctl00$Contenido$tb_hbl.value='" + dgw1.Rows[index].Cells[3].Text + "'; window.opener.document.aspnetForm.ctl00$Contenido$tb_mbl.value='" + dgw1.Rows[index].Cells[2].Text + "'; window.close();</script>");
        }

    }
    protected void dgw1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw1.DataSource = dt1;
        dgw1.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        dgw1.DataBind();
    }
}
