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

public partial class invoice_searchdoc : System.Web.UI.Page
{
    UsuarioBean user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        if (!Page.IsPostBack) {
            ArrayList arr = null;
            ListItem item = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 14, user,0);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie.Items.Add(item);
            }
        }
    }
    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        string serie = lb_serie.SelectedValue;
        string correlativo = tb_correlativo.Text.Trim();
        string hbl = tb_hbl.Text.Trim();
        string mbl = tb_mbl.Text.Trim();
        string contenedor = tb_contenedor.Text.Trim();
        string routing = tb_routing.Text.Trim();
        string sql = "Select tfa_id, tfa_serie, tfa_correlativo, tfa_cli_id, tfa_nit, tfa_nombre, tfa_fecha_emision, tfa_total, tfa_moneda, tfa_hbl, tfa_mbl, tfa_contenedor, tfa_routing, tfa_ordenpo,'', tfa_usu_id, tfa_ted_id from tbl_facturacion_proforma where tfa_id<>0";
        if (!serie.Equals("")) sql+=" and tfa_serie='"+serie+"'";
        if (!correlativo.Equals("")) sql+=" and tfa_correlativo='"+correlativo+"'";
        if (!hbl.Equals(""))
        {
            sql = "Select tfa_id, tfa_serie, tfa_correlativo, tfa_cli_id, tfa_nit, tfa_nombre, tfa_fecha_emision, tfa_total, tfa_moneda, tfa_hbl, tfa_mbl, tfa_contenedor, tfa_routing, tfa_ordenpo,'', tfa_usu_id from tbl_facturacion_proforma where tfa_id<>0";
            sql += " and tfa_hbl ilike '%" + hbl + "%'";
        }
        if (!mbl.Equals(""))
        {
            sql = "Select tfa_id, tfa_serie, tfa_correlativo, tfa_cli_id, tfa_nit, tfa_nombre, tfa_fecha_emision, tfa_total, tfa_moneda, tfa_hbl, tfa_mbl, tfa_contenedor, tfa_routing, tfa_ordenpo,'', tfa_usu_id from tbl_facturacion_proforma where tfa_id<>0";
            sql += " and tfa_mbl ilike '%" + mbl + "%'";
        }
        if (!contenedor.Equals(""))
        {
            sql = "Select tfa_id, tfa_serie, tfa_correlativo, tfa_cli_id, tfa_nit, tfa_nombre, tfa_fecha_emision, tfa_total, tfa_moneda, tfa_hbl, tfa_mbl, tfa_contenedor, tfa_routing, tfa_ordenpo,'', tfa_usu_id from tbl_facturacion_proforma where tfa_id<>0";
            sql += " and tfa_contenedor ilike '%" + contenedor + "%'";
        }
        if (!routing.Equals(""))
        {
            sql = "Select tfa_id, tfa_serie, tfa_correlativo, tfa_cli_id, tfa_nit, tfa_nombre, tfa_fecha_emision, tfa_total, tfa_moneda, tfa_hbl, tfa_mbl, tfa_contenedor, tfa_routing, tfa_ordenpo,'', tfa_usu_id from tbl_facturacion_proforma where tfa_id<>0";
            sql += " and tfa_routing ilike '%" + routing + "%'";
        }

        if (!lb_ted_id.SelectedValue.Equals("0"))
        {
            sql += " and tfa_ted_id=" + lb_ted_id.SelectedValue;
        }

        if (!tbCliCod.Text.Trim().Equals(""))
        {
            sql += " and tfa_cli_id=" + tbCliCod.Text.Trim();
        }

        sql += " and tfa_pai_id=" + user.PaisID + " and tfa_conta_id=" + int.Parse(Session["Contabilidad"].ToString()) + " order by tfa_fecha_emision desc,tfa_id asc ";
        DataTable dt = (DataTable)DB.getInvoices(sql);
        gv_facturas.DataSource = dt;
        gv_facturas.DataBind();
    }
    protected void gv_facturas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string action = Request.QueryString["action"].ToString();
        int index = Convert.ToInt32(e.CommandArgument);
        if (action.Equals("1"))
            Response.Redirect("viewinvoice_proforma.aspx?factID=" + gv_facturas.Rows[index].Cells[1].Text);
        else if (action.Equals("0"))
            Response.Redirect("viewinvoice_proforma.aspx?factID=" + gv_facturas.Rows[index].Cells[1].Text);
    }
    protected void gv_facturas_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;
    }
    protected void gv_facturas_RowCreated1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;

    }
}