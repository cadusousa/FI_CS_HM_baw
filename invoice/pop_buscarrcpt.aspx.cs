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

public partial class invoice_pop_buscarrcpt2 : System.Web.UI.Page
{
    UsuarioBean user = null;
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
        if ((user.SucursalID == 15) || (user.SucursalID == 20) || (user.SucursalID == 24) || (user.SucursalID == 26) || (user.SucursalID == 47) || (user.SucursalID == 41))
        {
            arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, 2, user,0);//2 porque es el tipo de documento para Recibos
        }
        else
        {
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 2, user,0);//2 porque es el tipo de documento para Recibos
        }
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_factura.Items.Add(item);
        }
    }
    protected void bt_search_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        string cliente = tb_nombrecliente.Text.Trim().ToUpper();
        string nit = tb_nit.Text.Trim().ToUpper();
        string serie = lb_serie_factura.SelectedValue;
        string corr_fact = tb_corr_factura.Text.Trim();
        string recibo = tb_recibo.Text.Trim().ToUpper();
        string hbl = tb_hbl.Text.Trim().ToUpper();
        string mbl = tb_mbl.Text.Trim().ToUpper();
        string contenedor = tb_contenedor.Text.Trim().ToUpper();
        string routing = tb_routing.Text.Trim().ToUpper();
        string codigo_cliente = tb_codigo_cliente.Text.Trim();

        string criterio = "";
        if (cliente != null && !cliente.Equals(""))
        {
            criterio += " and tre_id in (select tfr_tre_id from tbl_factura_abono where tfr_tfa_id in (select tfa_id from tbl_facturacion where tfa_pai_id=" + user.PaisID + " and tfa_nombre ilike '%" + cliente + "%'))";
        }
        if (nit != null && !nit.Equals(""))
        {
            criterio += " and tre_id in (select tfr_tre_id from tbl_factura_abono where tfr_tfa_id in (select tfa_id from tbl_facturacion where tfa_pai_id=" + user.PaisID + " and tfa_nit ilike '%" + nit + "%'))";
        }
        if (corr_fact != null && !corr_fact.Equals(""))
        {
            criterio += " and tre_id in (select tfr_tre_id from tbl_factura_abono where tfr_tfa_id in (select tfa_id from tbl_facturacion where tfa_pai_id=" + user.PaisID + " and tfa_correlativo ilike '%" + corr_fact + "%'))";
        }
        if ((recibo != null) && (!recibo.Equals("")))
        {
            criterio += " and tre_correlativo ilike '%" + recibo.Trim() + "%'";
        }
        if ((hbl != null) && (!hbl.Equals("")))
        {
            criterio += " and tre_hbl ilike '%" + hbl + "%'";
        }
        if (mbl != null && !mbl.Equals(""))
        {
            criterio += " and tre_mbl ilike '%" + mbl + "%'";
        }
        if ((routing != null) && (!routing.Equals("")))
        {
            criterio += " and tre_routing ilike '%" + routing + "%'";
        }
        if ((contenedor != null) && (!contenedor.Equals("")))
        {
            criterio += " and tre_contenedor ilike '%" + contenedor + "%'";
        }
        if (!lb_ted_id.SelectedValue.Equals("0"))
        {
            criterio += " and tre_ted_id=" + lb_ted_id.SelectedValue;
        }
        if (chk_pendiente.Checked)
        {
            criterio += " and  ((tre_monto-(select sum(tfr_abono) from tbl_factura_abono where tfr_tre_id=tre_id))>0 or tre_id not in (select distinct tfr_tre_id from tbl_factura_abono))";
        }
        if ((codigo_cliente != null) && (!codigo_cliente.Equals("")))
        {
            criterio += " and tre_cli_id =" + codigo_cliente.Trim() + " ";
        }
        criterio += " and tre_tcon_id=" + user.contaID;
        ArrayList arr = (ArrayList)DB.getRCPT(criterio, user.PaisID, serie);

        DataTable dt = new DataTable("rcpt");
        dt.Columns.Add("ID");
        dt.Columns.Add("Serie");
        dt.Columns.Add("Correlativo");
        dt.Columns.Add("Fecha");
        dt.Columns.Add("Monto");
        dt.Columns.Add("Contador");
        dt.Columns.Add("Cliente");
        dt.Columns.Add("Nombre");
        dt.Columns.Add("Observaciones");
        foreach (RE_GenericBean rgb in arr)
        {
            object[] objArr = { rgb.intC1.ToString(), rgb.strC3, rgb.strC4, rgb.strC1, rgb.douC1.ToString("#,#.00#;(#,#.00#)"), rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8 };
            dt.Rows.Add(objArr);
        }
        ViewState["dt"] = dt;
        dgw1.DataSource = dt;
        dgw1.DataBind();
    }

    protected void dgw1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Seleccionar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("showrcpt.aspx?rcptID=" + dgw1.Rows[index].Cells[1].Text + "");
        }
    }
    protected void dgw1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw1.DataSource = dt1;
        dgw1.PageIndex = e.NewPageIndex;
        dgw1.DataBind();
    }
    protected void dgw1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count == 1)
        { }
        else
        {
            e.Row.Cells[1].Visible = false;
        }
    }
}
