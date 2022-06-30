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

public partial class operations_cheques_search : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean proveedor = null;

        if (!Page.IsPostBack)
        {
            obtengo_listas();
            lb_bancos_SelectedIndexChanged(sender, e);
        }
    }
    protected void obtengo_listas()
    {
        ArrayList arr = null;
        ListItem item = null;
        
        arr = (ArrayList)DB.getBancosXPais(user.PaisID,1);
        lb_bancos.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        lb_bancos.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_bancos.Items.Add(item);
        }
        item = new ListItem("Seleccione...", "0");
        lb_cuentas_bancarias.Items.Add(item);
    }

    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        lb_cuentas_bancarias.Items.Clear();
        item = new ListItem("Seleccione...", " ");
        item.Selected = true;
        lb_cuentas_bancarias.Items.Add(item);
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue),user.PaisID,0);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_cuentas_bancarias.Items.Add(item);
        }
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

            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(where ,""); //Agente
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
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(nombre_intercompany)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
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
            tb_agenteID.Text = "";
            tb_agentenombre.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("3"))//Clientes
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("0"))//Retenciones
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
    }
    protected void gv_facturas_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        string where = " and tcg_tcon_id=" + user.contaID + " and tcg_pai_id=" + user.PaisID + "";
        if (!lb_bancos.SelectedValue.Trim().Equals("0") && !lb_bancos.SelectedValue.Trim().Equals(""))
        {
            where += " and tba_id=" + lb_bancos.SelectedValue;
        }
        if (!lb_cuentas_bancarias.SelectedValue.Trim().Equals("0") && !lb_cuentas_bancarias.SelectedValue.Trim().Equals(""))
        {
            where += " and tcg_cuenta='"+lb_cuentas_bancarias.SelectedValue+"'";
        }
        if (!tb_chequeNo.Text.Trim().Equals("")) {
            where += " and tcg_numero="+tb_chequeNo.Text;
        }
        if (!lb_ted_id.SelectedValue.Equals("0"))
        {
            where += " and tcg_ted_id=" + lb_ted_id.SelectedValue;
        }
        if ((!lb_tipopersona.SelectedValue.Equals("3")) && (!lb_tipopersona.SelectedValue.Equals("0")))
        {
            //where += " and tpr_tpi_id=" + lb_tipopersona.SelectedValue;
            where += " and tcg_tpi_id=" + lb_tipopersona.SelectedValue;
        }
        
        if (!lb_tipopersona.SelectedValue.Equals("0"))
        {
            if (!tb_agenteID.Text.Trim().Equals(""))
            { 
                if (lb_tipopersona.SelectedValue.Equals("3"))
                {
                    where += " and tre_cli_id=" + tb_agenteID.Text.Trim();
                }
                else
                {
                    where += " and tpr_proveedor_id=" + tb_agenteID.Text.Trim();
                }
            }
        }
        if (!tb_fechainicial.Text.Trim().Equals(""))
        {
            if (tb_fechafinal.Text.Trim().Equals(""))
            {
                WebMsgBox.Show("Debe de especificar una fecha final");
                return;
            }
            #region Formatear Fechas
            string fecha_ini = tb_fechainicial.Text.Trim();
            string fecha_fin = tb_fechafinal.Text.Trim();
            //Fecha Inicio
            int fe_dia = int.Parse(fecha_ini.Substring(3, 2));
            int fe_mes = int.Parse(fecha_ini.Substring(0, 2));
            int fe_anio = int.Parse(fecha_ini.Substring(6, 4));
            fecha_ini = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                fecha_ini += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                fecha_ini += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                fecha_ini += "0" + fe_dia.ToString();
            }
            else
            {
                fecha_ini += fe_dia.ToString();
            }
            //Fecha Fin
            fe_dia = int.Parse(fecha_fin.Substring(3, 2));
            fe_mes = int.Parse(fecha_fin.Substring(0, 2));
            fe_anio = int.Parse(fecha_fin.Substring(6, 4));
            fecha_fin = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                fecha_fin += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                fecha_fin += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                fecha_fin += "0" + fe_dia.ToString();
            }
            else
            {
                fecha_fin += fe_dia.ToString();
            }
            #endregion
            where += " and tcg_fecha >=" + "'" + fecha_ini + "'";
            where += " and tcg_fecha <=" + "'" + fecha_fin + "'";
        }
        ArrayList Arr = null;
        Arr = DB.getChequesGenerados(where, int.Parse(lb_tipopersona.SelectedValue.ToString()));
        dt = (DataTable)Utility.fillGridView("chequesgenerados", Arr);
        gv_cheques.DataSource = dt;
        gv_cheques.DataBind();

    }
    protected void gv_cheques_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Label lb1 = null;
        GridViewRow row=null;
        if (e.CommandName == "Select")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            row=gv_cheques.Rows[index];
            lb1 = (Label)row.FindControl("lb_chequeid");
            Response.Redirect("view_cheques.aspx?chequeNo="+lb1.Text);
        }

    }
    protected void gv_cheques_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;

    }
    protected void lb_tipopersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_agenteID.Text = "";
        tb_agentenombre.Text = "";
    }
}
