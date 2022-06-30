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

public partial class Reports_EstadoCuentaAgente : System.Web.UI.Page
{
    LibroDiarioDS ds = new LibroDiarioDS();
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];

    }
    protected void tb_generar_Click(object sender, EventArgs e)
    {

        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('viewSOA.aspx?mbl="+tb_mbl.Text+"', null, 'toolbar=no,resizable=no,status=no,width=800,height=500');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
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
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void dgw1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Seleccionar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            tb_hbl.Text = dgw1.Rows[index].Cells[2].Text;
            tb_mbl.Text = dgw1.Rows[index].Cells[3].Text;
            tb_contenedor.Text = dgw1.Rows[index].Cells[4].Text;
            tb_routing.Text = dgw1.Rows[index].Cells[5].Text;
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
        tb_cuenta_ModalPopupExtender.Show();
    }
}
