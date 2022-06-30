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

public partial class search_pagar_corte : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
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

        if (user.SucursalID == 20)
        {
            arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, 22, user, 0);//22 porque es el tipo de documento para Recibos
        }
        else
        {
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 22, user, 0);//22 porque es el tipo de documento para Recibos
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
        string serie = lb_serie_factura.SelectedValue;
        string corr_fact = tb_corr_factura.Text.Trim();
        string recibo = tb_recibo.Text.Trim().ToUpper();
        int proveedorID = 0;
        int tipo_persona = int.Parse(lb_tipopersona.SelectedValue);
        if (!tb_clientid.Text.Trim().Equals("")) 
        {
            proveedorID = int.Parse(tb_clientid.Text.Trim());
        }
        string criterio = "";


        if (corr_fact != null && !corr_fact.Equals(""))
        {
            criterio += " and trc_id in (select tcr_tre_id from tbl_corte_abono where tcr_tcp_id in (select tcp_id from tbl_corte_proveedor where tcp_pai_id=" + user.PaisID + " and tcp_correlativo ilike '%" + corr_fact + "%'))";
        }
        if ((recibo != null) && (!recibo.Equals("")))
        {
            criterio += " and trc_correlativo ilike '%" + recibo.Trim() + "%'";
        }
        if (tipo_persona>0)
            
        {
            criterio += " and trc_tpi_id=" + tipo_persona;
        }
        if (proveedorID > 0)
        {
            criterio += " and trc_cli_id=" + proveedorID;
        }
        if (!lb_ted_id.SelectedValue.Equals("0"))
        {
            criterio += " and trc_ted_id=" + lb_ted_id.SelectedValue;
        }
        /*
        if (chk_pendiente.Checked)
        {
            criterio += " and ((trc_monto-(select sum(tcr_abono) from tbl_corte_abono where tcr_tre_id=trc_id))>0 or trc_id not in (select distinct tcr_tre_id from tbl_corte_abono))";
        }
         */ 
        //Fin
        criterio += " and trc_tcon_id=" + user.contaID;
        ArrayList arr = (ArrayList)DB.getRCPTCorte(criterio, user.PaisID, serie);

        DataTable dt = new DataTable("rcpt");
        dt.Columns.Add("ID");
        dt.Columns.Add("Serie");
        dt.Columns.Add("Correlativo");
        dt.Columns.Add("Fecha");
        dt.Columns.Add("Monto");
        dt.Columns.Add("Contador");
        dt.Columns.Add("Proveedor");
        dt.Columns.Add("Nombre");
        dt.Columns.Add("Observaciones");
        foreach (RE_GenericBean rgb in arr)
        {
            object[] objArr = { rgb.intC1.ToString(), rgb.strC3, rgb.strC4, rgb.strC1, rgb.douC1, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8 };
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
            //Response.Write("<script languaje='JavaScript'>window.opener.location.href='showrcpt.aspx?rcptID=" + dgw1.Rows[index].Cells[1].Text + "'; window.close();</script>");
            Response.Redirect("show_recibo_pago_corte.aspx?rcptID=" + dgw1.Rows[index].Cells[1].Text + "");
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
    protected void refresh()
    {

    }

    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        else if (lb_tipopersona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit a un agente");
                return;
            }

            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(where, ""); //Agente
            dt = (DataTable)Utility.fillGridView("Agente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una naviera");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_naviera=" + tb_codigo.Text; else where += "id_naviera=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getNavieras(where, ""); //Naviera
            dt = (DataTable)Utility.fillGridView("Naviera", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una línea aerea");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(name)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and carrier_id=" + tb_codigo.Text; else where += " and carrier_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getCarriers(where); //Lineas aereas
            dt = (DataTable)Utility.fillGridView("LineasAereas", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit usuario de caja chica");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo.Text; else where += " and id_usuario=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(nombre_comercial)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_intercompany=" + tb_codigo.Text; else where += " and id_intercompany=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += " and nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.Get_Intercompanys(where);//Intercompany
            dt = (DataTable)Utility.fillGridView("Intercompany", Arr);
        }
        ViewState["proveedordt"] = dt;
        gv_proveedor.DataSource = dt;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
    protected void gv_proveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["proveedordt"];
        gv_proveedor.DataSource = dt1;
        gv_proveedor.PageIndex = e.NewPageIndex;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
    protected void gv_proveedor_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_proveedor.SelectedRow;
        if (lb_tipopersona.SelectedValue.Equals("4")) //proveedor
        {
            tb_clientid.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_clientname.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            tb_clientid.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_clientname.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_clientid.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_clientname.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tb_clientid.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_clientname.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_clientid.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_clientname.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("0"))//Retenciones
        {
            tb_clientid.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_clientname.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        {
            tb_clientid.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_clientname.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
        }
    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];

        }
    }
    protected void lb_tipopersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_clientid.Text = "";
        tb_clientname.Text = "";
        gv_proveedor.DataBind();        
    }
}
