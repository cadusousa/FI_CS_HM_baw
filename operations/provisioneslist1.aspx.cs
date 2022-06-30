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

public partial class operations_provisioneslist : System.Web.UI.Page
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

            ArrayList arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            if (tipo_contabilidad == 2)
            {
                #region Backup
                //lb_moneda.SelectedValue = "8";
                #endregion
                lb_moneda.SelectedValue = user.Moneda.ToString();
            }
            else
            {
                #region Backup
                //lb_moneda.SelectedValue = user.pais.ID.ToString();
                #endregion
                lb_moneda.SelectedValue = user.Moneda.ToString();
            }

            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 7, user, 0);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_seriefactura.Items.Add(item);
            }

            if (Session["provisioneslist"]!=null) {
                provListDT = (DataTable)Session["provisioneslist"];
                gv_detalle.DataSource = provListDT;
                gv_detalle.DataBind();
            }
            //**********************************************++
            if ((Request.QueryString["cliID"]!=null && !Request.QueryString["cliID"].ToString().Equals("")) && (Request.QueryString["tpi"]!=null && !Request.QueryString["tpi"].ToString().Equals("")) && (Request.QueryString["monID"]!=null && !Request.QueryString["monID"].ToString().Equals("")) && (Request.QueryString["ted"]!=null && !Request.QueryString["ted"].ToString().Equals("")) && (Request.QueryString["nombre"]!=null && !Request.QueryString["nombre"].ToString().Equals(""))) {

                tb_agentenombre.Text=Request.QueryString["nombre"].ToString();
                int cliID = int.Parse(Request.QueryString["cliID"].ToString());
                int tpi = int.Parse(Request.QueryString["tpi"].ToString());
                int monID = int.Parse(Request.QueryString["monID"].ToString());
                int ted = int.Parse(Request.QueryString["ted"].ToString());
                ArrayList provisioneslist = (ArrayList)DB.getProvisionesList(cliID, tpi, monID, ted, tb_agentenombre.Text, user);
                dt = (DataTable)Utility.fillGridView("provisioneslist", provisioneslist);
                gv_detalle.DataSource = dt;
                gv_detalle.DataBind();
            }
        }
    }

    protected void btn_buscar_Click1(object sender, EventArgs e)
    {
        int tpi = 0;
        if (lb_tipopersona.SelectedValue != "Seleccione...")
        {
            tpi = int.Parse(lb_tipopersona.SelectedValue);
        }
        int monID=int.Parse(lb_moneda.SelectedValue);
        int ted = int.Parse(lb_ted.SelectedValue);
        int cliID=0;
        ArrayList provisioneslist = null;
        if ((tb_corr_oc.Text != null && !tb_corr_oc.Text.Equals("")) || (tb_corr_provision.Text != null && !tb_corr_provision.Text.Equals("")) || (tb_mbl.Text != null && !tb_mbl.Text.Equals("")))
        {
            string where = " tpr_tcon_id=" + user.contaID + " and tpr_pai_id=" + user.PaisID + " and tpr_mon_id=" + monID;
            if (!tb_corr_oc.Text.Equals("")) where += " and tpr_serie_oc='" + lb_seriefactura.SelectedValue + "' and tpr_correlativo_oc=" + tb_corr_oc.Text.Trim();
            if (!tb_corr_provision.Text.Equals(""))
                if (!where.Equals("")) where += " and tpr_correlativo=" + tb_corr_provision.Text.Trim(); else where = "tpr_correlativo=" + tb_corr_provision.Text.Trim();
            if (!tb_mbl.Text.Equals(""))
                if (!where.Equals("")) where += " and tpr_mbl='" + tb_mbl.Text.Trim() + "'"; else where = "tpr_mbl='" + tb_mbl.Text.Trim() + "'";
            if (lb_ted.SelectedValue.Trim().ToString() != "")
                if (!where.Equals("")) where += " and tpr_ted_id='" + ted + "'"; else where = "tpr_ted_id='" + ted + "'";

            provisioneslist = (ArrayList)DB.getProvisionesListbyOC(where);
            provListDT = (DataTable)Utility.fillGridView("provisioneslist", provisioneslist);
        }
        else if ((lb_tipopersona.SelectedValue != "Seleccione...") && (tb_corr_oc.Text.Equals("")) && (tb_corr_provision.Text.Equals("")) && (tb_mbl.Text.Equals("")))
        {
            if (!tb_agenteID.Text.Equals("")) cliID = int.Parse(tb_agenteID.Text);
            string where = " tpr_tcon_id=" + user.contaID + " and tpr_pai_id=" + user.PaisID + " and tpr_mon_id=" + monID;
            where += " tpr_tpi_id=" + lb_tipopersona.SelectedValue.ToString() + " ";
            where += " and tpr_ted_id=" + lb_ted.SelectedValue.ToString() + " ";
            provisioneslist = (ArrayList)DB.getProvisionesList(cliID, tpi, monID, ted, tb_agentenombre.Text, user);
            provListDT = (DataTable)Utility.fillGridView("provisioneslist", provisioneslist);
        }
        else if (lb_tipopersona.SelectedValue == "Seleccione...")
        {
            string where = " tpr_tcon_id=" + user.contaID + " and tpr_pai_id=" + user.PaisID + " and tpr_mon_id=" + monID;
            where += " and tpr_ted_id=" + lb_ted.SelectedValue.ToString() + " ";

            provisioneslist = (ArrayList)DB.getProvisionesListbyOC(where);
            provListDT = (DataTable)Utility.fillGridView("provisioneslist", provisioneslist);
        }
        gv_detalle.DataSource = provListDT;
        gv_detalle.DataBind();
    }
    protected void gv_detalle_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = 0;
        if (e.CommandName == "Detalle")
        {
            provListDT = LlenoDataTable();
            int index = Convert.ToInt32(e.CommandArgument);
            id = int.Parse(gv_detalle.Rows[index].Cells[1].Text);
            string queriesting = "cliID=" + id + "&tpi=" + lb_tipopersona.SelectedValue + "&monID=" + lb_moneda.SelectedValue + "&ted=" + lb_ted.SelectedValue + "&nombre=" + tb_agentenombre.Text+"&index="+index;
            
            Session["provisioneslist"] = provListDT;
            Response.Redirect("provisiones.aspx?queriesting=" + queriesting + "&tipo=&provID=" + id);
        }
    }
    protected DataTable LlenoDataTable() {
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
            tb_contacto.Text = "";
            tb_contacto.Visible = false;
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[4].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        
    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
            ArrayList Arr = null;
            string where = "";
            //Arr = (ArrayList)DB.getAgente(where);
            //dt = (DataTable)Utility.fillGridView("Agente", Arr);
            //ViewState["proveedordt"] = dt;
            //gv_proveedor.DataSource = dt;
            //gv_proveedor.DataBind();
        }
    }
    protected void gv_detalle_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;

    }
    protected void lb_tipopersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_agentenombre.Text="";
        tb_agenteID.Text="";
        tb_corr_oc.Text="";
        tb_corr_provision.Text="";
        tb_mbl.Text = "";
    }
}
