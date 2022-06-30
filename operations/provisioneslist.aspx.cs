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

public partial class operations_provisiones_serch : System.Web.UI.Page
{
    public ArrayList Arr = null;
    public string querystring = "";
    UsuarioBean user = null;
    DataTable dt = null;
    DataTable provListDT = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];

        if (!Page.IsPostBack)
        {
            gv_detalle.DataBind();
            ArrayList arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            //if (tipo_contabilidad == 2)
            //{
            //    #region Backup
            //    //lb_moneda.SelectedValue = "8";
            //    #endregion
            //    lb_moneda.SelectedValue = user.Moneda.ToString();
            //}
            //else
            //{
            //    #region Backup
            //    //lb_moneda.SelectedValue = user.pais.ID.ToString();
            //    #endregion
            //    lb_moneda.SelectedValue = user.Moneda.ToString();
            //}

            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 7, user,0);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_seriefactura.Items.Add(item);
            }
            /*
            if (Session["provisioneslist"] != null)
            {
                provListDT = (DataTable)Session["provisioneslist"];
                gv_detalle.DataSource = provListDT;
                gv_detalle.DataBind();
            }
             */ 
            //**********************************************++
            if ((Request.QueryString["cliID"] != null && !Request.QueryString["cliID"].ToString().Equals("")) && (Request.QueryString["tpi"] != null && !Request.QueryString["tpi"].ToString().Equals("")) && (Request.QueryString["monID"] != null && !Request.QueryString["monID"].ToString().Equals("")) && (Request.QueryString["ted"] != null && !Request.QueryString["ted"].ToString().Equals("")) && (Request.QueryString["nombre"] != null && !Request.QueryString["nombre"].ToString().Equals("")))
            {

                tb_agentenombre.Text = Request.QueryString["nombre"].ToString();
                int cliID = int.Parse(Request.QueryString["cliID"].ToString());
                int tpi = int.Parse(Request.QueryString["tpi"].ToString());
                int monID = int.Parse(Request.QueryString["monID"].ToString());
                int ted = int.Parse(Request.QueryString["ted"].ToString());
                ArrayList provisioneslist = (ArrayList)DB.getProvisionesList(cliID, tpi, monID, ted, tb_agentenombre.Text, user);
                dt = (DataTable)Utility.fillGridView("provisioneslist", provisioneslist);
                gv_detalle.DataSource = dt;
                gv_detalle.DataBind();
            }

            //Definir Moneda Inicial*************************************
            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 1);
            lb_moneda.SelectedValue = moneda_inicial.ToString();


        }
    }
    protected void gv_detalle_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = 0;
        if (e.CommandName == "Detalle")
        {
            provListDT = LlenoDataTable();
            int index = Convert.ToInt32(e.CommandArgument);
            id = int.Parse(gv_detalle.Rows[index].Cells[1].Text);
            string queryString = string.Format("cliID={0}&tpi={1}&monID={2}&ted={3}&nombre={4}&index={5}", id, lb_tipopersona.SelectedValue, lb_moneda.SelectedValue, lb_ted.SelectedValue, tb_agentenombre.Text, index);
            Session["provisioneslist"] = provListDT;

            var urlPage = string.Format("provisiones.aspx?queriesting={0}&tipo=&provID={1}", queryString, id);
            var jsKey = "PopupScript";
            var cs = Page.ClientScript;
            string script = string.Format("window.open('{0}', '_blank', 'toolbar=0, status=0, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -550) + ', height=' + (window.screen.height -75))", urlPage);
            if (!cs.IsStartupScriptRegistered(this.GetType(), jsKey))
                cs.RegisterClientScriptBlock(this.GetType(), jsKey, script, true);
        }
    }
    protected DataTable LlenoDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PVNID");
        dt.Columns.Add("Proveedor");
        dt.Columns.Add("Fecha Creacion");
        dt.Columns.Add("Usuario creador");
        dt.Columns.Add("Total");
        dt.Columns.Add("Moneda");
        dt.Columns.Add("OC");
        dt.Columns.Add("MBL");
        string ordencompra = "";
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            object[] objArr = { Page.Server.HtmlDecode(row.Cells[1].Text), Page.Server.HtmlDecode(row.Cells[2].Text), Page.Server.HtmlDecode(row.Cells[3].Text), Page.Server.HtmlDecode(row.Cells[4].Text), Page.Server.HtmlDecode(row.Cells[5].Text), Page.Server.HtmlDecode(row.Cells[6].Text), Page.Server.HtmlDecode(row.Cells[7].Text), Page.Server.HtmlDecode(row.Cells[8].Text) };
            dt.Rows.Add(objArr);
        }
        return dt;
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
        if (lb_tipopersona.SelectedValue.Equals("3"))
        { // cliente
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(a.nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and a.id_cliente=" + tb_codigo.Text; else where += "a.id_cliente=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and a.codigo_tributario='" + tb_nitb.Text + "'"; else where += "a.codigo_tributario='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getClientes(where, user, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
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
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += " agente_id=" + tb_codigo.Text;
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
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
        }
        //else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        //{
        //    tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
        //    tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
        //}
    }
    protected void gv_detalle_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;

    }
    protected void lb_tipopersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_agentenombre.Text = "";
        tb_agenteID.Text = "";
        tb_corr_oc.Text = "";
        tb_corr_provision.Text = "";
        tb_mbl.Text = "";
    }
    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        ArrayList provisioneslist = null;
        int _tedID = 0;
        _tedID = int.Parse(lb_ted.SelectedValue.ToString());
        tb_serie.Text.Trim();
        tb_corr_provision.Text.Trim();
        string Where = " tpr_tcon_id=" + user.contaID + " and tpr_pai_id=" + user.PaisID + " and tpr_mon_id=" + lb_moneda.SelectedValue + " ";
        if (_tedID > 0)
        {
            Where += " and tpr_ted_id=" + lb_ted.SelectedValue + " ";
        }
        if (lb_tipopersona.SelectedValue != "Todos")
        {
            Where += " and tpr_tpi_id=" + lb_tipopersona.SelectedValue + " ";
        }
        if ((tb_agenteID.Text != null) && (tb_agenteID.Text != ""))
        {
            Where += " and tpr_proveedor_id=" + tb_agenteID.Text+" ";
        }
        if (tb_serie.Text != "")
        {
            Where += " and tpr_serie='" + tb_serie.Text + "' ";
        }
        if ((tb_corr_provision.Text != "") && ((tb_rango_inicial.Text == "") && (tb_rango_final.Text == "")))
        {
            Where += " and tpr_correlativo=" + tb_corr_provision.Text + " ";
            tb_rango_inicial.Text = "";
            tb_rango_final.Text = "";
        }
        if (tb_corr_provision.Text == "")
        {
            tb_corr_provision.Text = "";
            if (tb_rango_inicial.Text != "")
            {
                Where += " and tpr_correlativo>=" + tb_rango_inicial.Text.Trim() + " ";
            }
            if (tb_rango_final.Text != "")
            {
                Where += "  and tpr_correlativo<=" + tb_rango_final.Text.Trim() + " ";
            }
        }
        if (tb_mbl.Text != "")
        {
            Where += " and tpr_mbl='" + tb_mbl.Text.Trim() + "' ";
        }
        if (tb_corr_oc.Text != "")
        {
            Where += " and tpr_serie_oc='" + lb_seriefactura.SelectedValue + "' and tpr_correlativo_oc=" + tb_corr_oc.Text.Trim() + " ";
        }
        if (tb_fecha_inicial.Text != "")
        {
            Where += " and tpr_fecha_creacion>='" + DB.DateFormat(tb_fecha_inicial.Text) + "' ";
        }
        if (tb_fecha_final.Text != "")
        {
            Where += " and tpr_fecha_creacion<='" + DB.DateFormat(tb_fecha_final.Text) + "' ";
        }
        if (tb_serie_proveedor.Text != "")
        {
            Where += " and tpr_fact_id ilike '%" + tb_serie_proveedor.Text.Trim() + "%' ";
        }
        if (tb_correlativo_proveedor.Text != "")
        {
            Where += " and tpr_fact_corr ilike '%" + tb_correlativo_proveedor.Text.Trim() + "%' ";
        }
        if (tb_poliza_seguros.Text != "")
        {
            Where += " and tpr_poliza_seguro='" + tb_poliza_seguros.Text.Trim() + "' ";
        }
        gv_detalle.DataBind();
        provisioneslist = (ArrayList)DB.getProvisionesListbyOC(Where);
        provListDT = (DataTable)Utility.fillGridView("provisioneslist", provisioneslist);
        gv_detalle.DataSource = provListDT;
        gv_detalle.DataBind();

    }
    protected void tb_corr_provision_TextChanged(object sender, EventArgs e)
    {
        tb_rango_inicial.Text = "";
        tb_rango_final.Text = "";
    }
    protected void tb_rango_inicial_TextChanged(object sender, EventArgs e)
    {
        tb_corr_provision.Text = "";
    }
    protected void tb_rango_final_TextChanged(object sender, EventArgs e)
    {
        tb_corr_provision.Text = "";
    }
    protected void btn_limpiar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/operations/provisioneslist.aspx");
    }
}