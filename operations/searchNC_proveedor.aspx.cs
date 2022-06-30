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
        if (!Page.IsPostBack) 
        {
            Obtengo_Listas();
        }
    }
    protected void Obtengo_Listas()
    {
        ArrayList arr = null;
        ListItem item = null;
        ListItem item_tipo = null;
        int id = 1;
        arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 12, user, 0);//4 porque es el tipo de documento para nota debito segun sys_tipo_referencia
        foreach (string valor in arr)
        {
            //item = new ListItem(valor, "12");
            item = new ListItem(valor, id.ToString());
            item_tipo = new ListItem("12", id.ToString());
            lb_serie.Items.Add(item);
            drp_tipo_serie.Items.Add(item_tipo);
            id++;
        }
        arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 31, user, 0);//4 porque es el tipo de documento para nota debito segun sys_tipo_referencia
        foreach (string valor in arr)
        {
            //item = new ListItem(valor, "31");
            item = new ListItem(valor, id.ToString());
            item_tipo = new ListItem("31", id.ToString());
            lb_serie.Items.Add(item);
            drp_tipo_serie.Items.Add(item_tipo);
            id++;   
        }
    }
    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        #region Obtener Tipo de Nota de Credito
        string value = lb_serie.SelectedValue.ToString();
        string tipo = "";
        foreach (ListItem it in drp_tipo_serie.Items)
        {
            if (value == it.Value)
            {
                tipo = it.Text;
            }
        }
        #endregion
        string serie = lb_serie.SelectedItem.ToString();
        string correlativo = tb_correlativo.Text.Trim();
        string hbl = tb_hbl.Text.Trim();
        string mbl = tb_mbl.Text.Trim();
        string contenedor = tb_contenedor.Text.Trim();
        string routing = tb_routing.Text.Trim();
        string sql = "";
        sql = "Select tnc_id, tnc_serie, tnc_correlativo, tnc_nombre , tnc_fecha, tnc_monto, tnc_mon_id, tnc_hbl, tnc_mbl, tnc_contenedor, tnc_routing, tnc_cli_id, tnc_ttr_id  from tbl_nota_credito where tnc_pai_id=" + user.PaisID;
        sql += " and tnc_ttr_id =" + int.Parse(tipo) + " ";
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
        if (lb_tipopersona.SelectedValue != "0")
        {
            sql += " and tnc_tpi_id=" + lb_tipopersona.SelectedValue.ToString() + " ";
        }
        sql += " order by tnc_fecha desc,tnc_id asc ";
        DataTable dt = (DataTable)DB.getNC(sql);
        gv_facturas.DataSource = dt;
        gv_facturas.DataBind();
    }
    protected void gv_facturas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        Response.Redirect("viewNC.aspx?ncID=" + gv_facturas.Rows[index].Cells[1].Text + "&tipo=" + gv_facturas.Rows[index].Cells[13].Text);
        //Response.Redirect("viewNC.aspx?ncID=" + gv_facturas.Rows[index].Cells[1].Text + "&tipo=" + lb_serie.SelectedValue);
    }
    protected void gv_facturas_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[13].Visible = false;
        }

    }
}
