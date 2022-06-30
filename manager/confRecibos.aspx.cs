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

public partial class manager_confRecibos : System.Web.UI.Page
{
    DataTable dt = null;
    UsuarioBean user = null;
    string operacion = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["opt"] != null && !Request.QueryString["opt"].ToString().Equals(""))
            {
                operacion = Request.QueryString["opt"].ToString();
                // Cargo los tipos de operacion
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='" + operacion + "'");
                lb_transaccion.Items.Clear();
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_transaccion.Items.Add(item);
                }
                // Cargo los tipos de moneda
                arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
                lb_moneda.Items.Clear();
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_moneda.Items.Add(item);
                }
                //Cargo los paises
                arr = (ArrayList)DB.getPaises("");
                lb_pais.Items.Clear();
                item = new ListItem("Todos", "0");
                lb_pais.Items.Add(item);
                foreach (PaisBean pais in arr)
                {
                    item = new ListItem(pais.Nombre, pais.ID.ToString());
                    lb_pais.Items.Add(item);
                }
                //Cargo las contabilidades
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
                lb_tipo_contabilidad.Items.Clear();
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_tipo_contabilidad.Items.Add(item);
                }
            }
        }
    }

    protected void gv_cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_cuenta.DataSource = dt;
        gv_cuenta.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        gv_cuenta.DataBind();
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void gv_cuenta_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ArrayList cuentaArr = null;
            cuentaArr = Utility.getCuentasContables("XClasificacion", "1,2,3");
            dt = (DataTable)Utility.fillGridView("Cuenta", cuentaArr);
            ViewState["gv_cuenta_dt"] = dt;
            gv_cuenta.DataSource = dt;
            gv_cuenta.DataBind();
        }
        else
        {
            dt = (DataTable)ViewState["gv_cuenta_dt"];
        }
    }
    protected void gv_cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_cuenta.SelectedRow;
        tb_cuenta.Text = row.Cells[1].Text;
        tb_cta_nombre.Text = row.Cells[2].Text;
    }
    protected void bt_buscar_cta_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        int clasificacion = int.Parse(lb_clasificacion.SelectedValue);
        if (clasificacion != 0) where += " and cue_clasificacion in (" + clasificacion + ")";
        if (!tb_nombre_cta.Text.Trim().Equals("") && tb_nombre_cta.Text != null) where += " and cue_nombre like '%" + tb_nombre_cta.Text.Trim() + "%'";
        Arr = Utility.getCuentasContables("XTipoClasificacion", where);
        dt = (DataTable)Utility.fillGridView("Cuenta", Arr);
        gv_cuenta.DataSource = dt;
        gv_cuenta.DataBind();
        ViewState["gv_cuenta_dt"] = dt;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void bt_agregar_Click(object sender, EventArgs e)
    {
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Debe");
        dt_temp.Columns.Add("Haber");
        GridViewRowCollection gvr = gv_conf_cuentas.Rows;
        dt_temp.Clear();
        TextBox t1 = null;
        TextBox t2 = null;
        Label lb1 = null;
        Label lb2 = null;
        foreach (GridViewRow row in gvr)
        {
            t1 = (TextBox)row.FindControl("tb_debe");
            t2 = (TextBox)row.FindControl("tb_haber");
            lb1 = (Label)row.FindControl("Label1");
            lb2 = (Label)row.FindControl("Label2");
            object[] objArr = { lb1.Text, lb2.Text, t1.Text, t2.Text };
            dt_temp.Rows.Add(objArr);
        }
        object[] objArr1 = { tb_cuenta.Text.Trim(), tb_cta_nombre.Text.Trim(), "0", "0" };
        dt_temp.Rows.Add(objArr1);
        
        gv_conf_cuentas.DataSource = dt_temp;
        gv_conf_cuentas.DataBind();
        tb_cuenta.Text = "";
        tb_cta_nombre.Text = "";
    }
    protected void gv_conf_cuentas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Debe");
        dt_temp.Columns.Add("Haber");
        GridViewRowCollection gvr = gv_conf_cuentas.Rows;
        dt_temp.Clear();
        TextBox t1 = null;
        TextBox t2 = null;
        Label lb1 = null;
        Label lb2 = null;
        foreach (GridViewRow row in gvr)
        {
            t1 = (TextBox)row.FindControl("tb_debe");
            t2 = (TextBox)row.FindControl("tb_haber");
            lb1 = (Label)row.FindControl("Label1");
            lb2 = (Label)row.FindControl("Label2");
            object[] objArr = { lb1.Text, lb2.Text, t1.Text, t2.Text };
            dt_temp.Rows.Add(objArr);
        }
        dt_temp.Rows[e.RowIndex].Delete();
        gv_conf_cuentas.DataSource = dt_temp;
        gv_conf_cuentas.DataBind();
    }
    protected void bt_buscar_conv_Click(object sender, EventArgs e)
    {
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Debe");
        dt_temp.Columns.Add("Haber");
        int tranID = int.Parse(lb_transaccion.SelectedValue);
        int monID = int.Parse(lb_moneda.SelectedValue);
        int paisID = int.Parse(lb_pais.SelectedValue);
        int contaID = int.Parse(lb_tipo_contabilidad.SelectedValue);
        int matOpID = DB.getMatrizOperacionID(tranID, monID, paisID, contaID);
        ArrayList ctas = (ArrayList)DB.getMatrizCuenta2(matOpID);
        if (ctas != null)
        {
            foreach (RE_GenericBean rgb in ctas)
            {
                object[] objArr = { rgb.strC1, rgb.strC2, rgb.intC1, rgb.intC2 };
                dt_temp.Rows.Add(objArr);
            }
            gv_conf_cuentas.DataSource = dt_temp;
            gv_conf_cuentas.DataBind();
        }

    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        GridViewRow row;
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.intC1 = int.Parse(lb_transaccion.SelectedValue);//ID departamento
        rgb.intC2 = int.Parse(lb_moneda.SelectedValue);//ID moneda
        rgb.intC3 = int.Parse(lb_pais.SelectedValue);//Pais
        rgb.intC4 = int.Parse(lb_tipo_contabilidad.SelectedValue);//Tipo de contabilidad
        TextBox t1;
        Label lb;
        TextBox t2;
        RE_GenericBean ctasbean = null;
        for (int i = 0; i < gv_conf_cuentas.Rows.Count; i++)
        {
            row = gv_conf_cuentas.Rows[i];
            ctasbean = new RE_GenericBean();
            if (row.RowType == DataControlRowType.DataRow)
            {
                lb = (Label)row.FindControl("Label1");
                ctasbean.strC1 = lb.Text;//id de la cuenta
                lb = (Label)row.FindControl("Label2");
                ctasbean.strC2 = lb.Text;//nombre de la cuenta
                t1 = (TextBox)row.FindControl("tb_debe");
                t2 = (TextBox)row.FindControl("tb_haber");
                ctasbean.intC1 = int.Parse(t1.Text.ToString());//debe
                ctasbean.intC2 = int.Parse(t2.Text.ToString());//haber
                if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                rgb.arr1.Add(ctasbean);
            }
        }

        int result = DB.insertMatrizOperacion2(rgb, user);
        if (result < 0)
        {
            Session["factura"] = null;
            Session["msg"] = "Error, Existió un problema al momento de guardar la provision, por favor intente de nuevo.";
            Session["url"] = "index.aspx";
            Response.Redirect("message.aspx");
        }
        Response.Redirect("confRecibos.aspx?opt=pop_notacredito.aspx");

    }
    protected void gv_conf_cuentas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Page.IsPostBack)
        {
            operacion = Request.QueryString["opt"].ToString();
            if (operacion.Equals("newrcpt.aspx"))
            {
                TextBox t1 = new TextBox();
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    t1 = (TextBox)e.Row.FindControl("tb_haber");
                    //t1.Enabled = false;
                }
            }
        }
    }
}
