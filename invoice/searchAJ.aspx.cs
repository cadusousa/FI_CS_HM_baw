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

public partial class invoice_searchAJ : System.Web.UI.Page
{
    UsuarioBean user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        if (!Page.IsPostBack)
        {
            ArrayList arr = null;
            ListItem item = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 18, user, 0);//1 porque es el tipo de documento para facturacion
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

        //string sql = "Select tnc_id, tnc_serie, tnc_correlativo, tnc_cli_id, tnc_fecha, tnc_monto, tnc_mon_id, tnc_hbl, tnc_mbl, tnc_contenedor, tnc_routing from tbl_nota_credito where tnc_id<>0";
        //string sql = "Select tnc_id, tnc_serie, tnc_correlativo, tfa_nombre, tnc_fecha, tnc_monto, tnc_mon_id, tnc_hbl, tnc_mbl, tnc_contenedor, tnc_routing from tbl_nota_credito,tbl_facturacion,tbl_factura_abono where tfa_id=tfr_tfa_id and tfr_sysref_id=3 and tfr_tre_id=tnc_id and tnc_id<>0";
        string sql = "";
        if ((lb_ted_id.SelectedValue.Equals("2")) || (lb_ted_id.SelectedValue.Equals("4")))
        {
            sql = "Select tnc_id, tnc_serie, tnc_correlativo, tnc_nombre, tnc_fecha, tnc_monto, tnc_mon_id, tnc_hbl, tnc_mbl, tnc_contenedor, tnc_routing, tnc_cli_id from tbl_nota_credito,tbl_facturacion,tbl_factura_abono where tfa_id=tfr_tfa_id and tfr_sysref_id=18 and tfr_tre_id=tnc_id and tnc_pai_id=" + user.PaisID;
        }
        else
        {
            sql = "Select tnc_id, tnc_serie, tnc_correlativo, tnc_nombre, tnc_fecha, tnc_monto, tnc_mon_id, tnc_hbl, tnc_mbl, tnc_contenedor, tnc_routing, tnc_cli_id  from tbl_nota_credito where tnc_pai_id=" + user.PaisID;
        }

        if (!serie.Equals("")) sql += " and tnc_serie='" + serie + "'";
        if (!correlativo.Equals("")) sql += " and tnc_correlativo=" + correlativo;
        if (!hbl.Equals("")) sql += " and tnc_hbl='" + hbl + "'";
        if (!mbl.Equals("")) sql += " and tnc_mbl='" + mbl + "'";
        if (!contenedor.Equals("")) sql += " and tnc_contenedor='" + contenedor + "'";
        if (!routing.Equals("")) sql += " and tnc_routing='" + routing + "'";
        if (!lb_ted_id.SelectedValue.Equals("0")) sql += " and tnc_ted_id=" + lb_ted_id.SelectedValue;

        if (!tbCliCod.Text.Trim().Equals(""))
        {
            sql += " and tnc_cli_id=" + tbCliCod.Text.Trim();
        }


        if ((lb_ted_id.SelectedValue.Equals("2")) || (lb_ted_id.SelectedValue.Equals("4")))
        {
            string sql2 = sql.Replace("tbl_facturacion", "tbl_nota_debito");
            sql2 = sql2.Replace("where tfa_id", "where tnd_id");
            sql += " UNION " + sql2;
        }        
        sql += " order by tnc_fecha desc,tnc_id asc ";
        
        DataTable dt = (DataTable)DB.getNC(sql);
        gv_facturas.DataSource = dt;
        gv_facturas.DataBind();
    }
    protected void gv_facturas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        Response.Redirect("viewAJ.aspx?ajID=" + gv_facturas.Rows[index].Cells[1].Text);
    }
    protected void gv_facturas_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;

    }
}
