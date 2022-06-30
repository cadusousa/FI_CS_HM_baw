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

public partial class invoice_searchBL : System.Web.UI.Page
{
    private DataTable dt1;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["factura"] = null;
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }

    }

    protected void bt_search_Click(object sender, EventArgs e)
    {
        ListItem item=null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;
        
        if (lb_tipo.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL_invoice(lista, tb_criterio.Text.Trim(), user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        } else if (lb_tipo.SelectedValue.Equals("FCL")) {
            DataSet ds = DB.getBL_FCL_invoice(lista, tb_criterio.Text.Trim(), user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        } else if (lb_tipo.SelectedValue.Equals("ALMACENADORA")) {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio.Text.Trim(), user.pais.schema, codDistribucion, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio.Text.Trim(), user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("TERRESTRE T"))
        {
            DataSet ds = DB.getBL_TERRESTRE_Costos(lista, tb_criterio.Text.Trim(), user.pais.ISO, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("RO ADUANAS"))
        {
            DataSet ds = DB.getRO_Aduanas(user, tb_criterio.Text.Trim());
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("RO SEGUROS"))
        {
            DataSet ds = DB.getRO_Seguros(user, tb_criterio.Text.Trim());
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
            Response.Write("<script languaje='JavaScript'>window.opener.location.href='notacreditoprov_directo.aspx?bl_no=" + dgw1.Rows[index].Cells[2].Text + "&tipo=" + lb_tipo.SelectedValue + "&opid=" + dgw1.Rows[index].Cells[6].Text + "&blid=" + dgw1.Rows[index].Cells[7].Text + "'; window.close();</script>");
        }
    }
    protected void dgw1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        dgw1.DataSource = dt1;
        dgw1.PageIndex = e.NewPageIndex;
        dgw1.DataBind();
    }
    protected void dgw1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[6].Visible = false;
        }
    }
}
