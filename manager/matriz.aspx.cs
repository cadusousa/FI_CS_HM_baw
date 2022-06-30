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

public partial class manager_matriz : System.Web.UI.Page
{
    public ArrayList rubrocuenta_arr = null;
    public int refid = 0;
    public int pai_id = 0;
    DataTable dt = null;
    UsuarioBean user = null;

    protected void Page_Load(object sender, EventArgs e)
    {        
        string trans = "", serv = "", contribuyente = "", moneda = "", cobro = "", conta = "", impo = "";
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["pai_id"] != null)
            {
                pai_id = int.Parse(Request.QueryString["pai_id"].ToString());
            }
            if (Request.QueryString["trans"] != null) {
                trans = Request.QueryString["trans"].ToString();
            }
            if (Request.QueryString["serv"] != null) {
                serv = Request.QueryString["serv"].ToString();
            }
            if (Request.QueryString["contribuyente"] != null) {
                contribuyente = Request.QueryString["contribuyente"].ToString();
            }
            if (Request.QueryString["moneda"] != null) {
                moneda = Request.QueryString["moneda"].ToString();
            }
            if (Request.QueryString["cobro"] != null) {
                cobro = Request.QueryString["cobro"].ToString();
            }
            if (Request.QueryString["conta"] != null) {
                conta = Request.QueryString["conta"].ToString();
            }
            if (Request.QueryString["impo"] != null) {
                impo = Request.QueryString["impo"].ToString();
            }

            lb_ref_id.Text = "0";
            
            lb_paid.Text = pai_id.ToString();
            ListItem item = null;
            ArrayList arr = null;
            item = new ListItem("Seleccione", "0");
            lb_cobro.Items.Add(item);
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_cobro");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (rgb.strC1.Equals(cobro)) { item.Selected = true; lb_cobro.Enabled = false; }
                lb_cobro.Items.Add(item);
            }
            
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
            item = new ListItem("Seleccione", "0");
            lb_conta.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (rgb.strC1.Equals(conta)) { item.Selected = true; lb_conta.Enabled = false; }
                lb_conta.Items.Add(item);
            }

            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
            item = new ListItem("Seleccione", "0");
            lb_imp_exp.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (rgb.strC1.Equals(impo)) { item.Selected = true; lb_imp_exp.Enabled = false; }
                lb_imp_exp.Items.Add(item);
            }

            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_contribuyente");
            item = new ListItem("Seleccione", "0");
            lb_contribuyente.Items.Add(item);
            foreach (RE_GenericBean rgb in arr) {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (rgb.strC1.Equals(contribuyente)) { item.Selected = true; lb_contribuyente.Enabled = false; }
                lb_contribuyente.Items.Add(item);
            }

            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            item = new ListItem("Seleccione", "0");
            lb_moneda.Items.Add(item);
            foreach (RE_GenericBean rgb in arr) {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (rgb.strC1.Equals(moneda)) { item.Selected = true; lb_moneda.Enabled = false; }
                lb_moneda.Items.Add(item);
            }

            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_servicio");
            item = new ListItem("Seleccione", "0");
            lb_servicio.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (rgb.strC1.Equals(serv)) { item.Selected = true; lb_servicio.Enabled = false; }
                lb_servicio.Items.Add(item);
            }

            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template in ('newinvoice.aspx', 'provisionagente.aspx','provisiones.aspx')");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (rgb.strC1.Equals(trans)) { item.Selected = true; lb_trans.Enabled = false; }
                lb_trans.Items.Add(item);
            }

            arr = null;
            arr = (ArrayList)DB.getAllRubrosXPais(int.Parse(lb_paid.Text));
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                //item.Selected=true;
                chk_list_rubros.Items.Add(item);
            }  
          
            // Obtengo las cuentas asociadas a esta convinacion
            RE_GenericBean rgb_2 = new RE_GenericBean();
            rgb_2.intC1 = int.Parse(lb_paid.Text);;//pais
            rgb_2.intC2 = int.Parse(lb_trans.SelectedValue);//transaccion
            rgb_2.intC3 = int.Parse(lb_servicio.SelectedValue);//servicio lcl, fcl
            rgb_2.intC4 = int.Parse(lb_contribuyente.SelectedValue);//contribuyente excento
            rgb_2.intC5 = int.Parse(lb_moneda.SelectedValue);//moneda $$ o QQ
            rgb_2.intC6 = int.Parse(lb_cobro.SelectedValue);//Tipo cobro Collect Prepaid
            rgb_2.intC7 = int.Parse(lb_conta.SelectedValue);//Tipo contabilidad
            rgb_2.intC8 = int.Parse(lb_imp_exp.SelectedValue);//Impo, Expo
            DataTable dt_cuentas_aso = (DataTable)DB.getCtabyCombinacionMatriz(rgb_2);
            gv_conf_cuentas.DataSource = dt_cuentas_aso;
            gv_conf_cuentas.DataBind();

            ArrayList arrRubrosConvinacion = (ArrayList)DB.getRubrosConvinacion(rgb_2);
            foreach (RE_GenericBean rgb in arrRubrosConvinacion)
            {
                for (int a = 0; a < chk_list_rubros.Items.Count; a++)
                {
                    if (chk_list_rubros.Items[a].Value.Equals(rgb.intC1.ToString()))
                    {
                        chk_list_rubros.Items[a].Selected = true;
                    }
                }
            }
        }
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
    protected void gv_conf_cuentas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Page.IsPostBack)
        {
            //operacion = Request.QueryString["opt"].ToString();
            //if (operacion.Equals("newrcpt.aspx"))
            //{
                TextBox t1 = new TextBox();
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    t1 = (TextBox)e.Row.FindControl("tb_haber");
                    
                }
            //}
        }
    }
    protected void bt_buscar_cta_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        int clasificacion = int.Parse(lb_clasificacion.SelectedValue);
        if (clasificacion != 0) where += " and cue_clasificacion in (" + clasificacion + ")";
        if (!tb_nombre_cta.Text.Trim().Equals("") && tb_nombre_cta.Text != null) where += " and cue_nombre like '%" + tb_nombre_cta.Text.Trim().ToUpper() + "%'";
        Arr = Utility.getCuentasContables("XTipoClasificacion", where);
        dt = (DataTable)Utility.fillGridView("Cuenta", Arr);
        gv_cuenta.DataSource = dt;
        gv_cuenta.DataBind();
        ViewState["gv_cuenta_dt"] = dt;
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
    protected void gv_cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_cuenta.DataSource = dt;
        gv_cuenta.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        gv_cuenta.DataBind();
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void gv_cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_cuenta.SelectedRow;
        tb_cuenta.Text = row.Cells[1].Text;
        tb_cta_nombre.Text = row.Cells[2].Text;
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        if (!suma100("tb_debe") || !suma100("tb_haber"))
        {
            WebMsgBox.Show("Error, el total de porcentajes debe ser 100 o bien 0");
            return;
        }
        int pai_id = int.Parse(lb_paid.Text);
        int trans = int.Parse(lb_trans.SelectedValue);
        int serv = int.Parse(lb_servicio.SelectedValue);
        int contribuyente = int.Parse(lb_contribuyente.SelectedValue);
        int moneda = int.Parse(lb_moneda.SelectedValue);
        int tipocobro = int.Parse(lb_cobro.SelectedValue);
        int conta = int.Parse(lb_conta.SelectedValue);
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue);
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.intC1 = pai_id;//pais
        rgb.intC2 = trans;//transaccion
        rgb.intC3 = serv;//servicio lcl, fcl
        rgb.intC4 = contribuyente;//contribuyente excento
        rgb.intC5 = moneda;//moneda $$ o QQ
        rgb.intC6 = tipocobro;//Tipo cobro Collect Prepaid
        rgb.intC7 = conta;//Tipo contabilidad
        rgb.intC8 = imp_exp;//Impo, Expo
        rgb.intC9 = int.Parse(lb_ref_id.Text.ToString());//Referencia para update
        //Obtengo la lista de los rubros que selecciono
        ArrayList rubrosArr = new ArrayList();
        for (int a = 0; a < chk_list_rubros.Items.Count; a++)
        {
            if (chk_list_rubros.Items[a].Selected == true)
            {
                rubrosArr.Add(chk_list_rubros.Items[a].Value);
            }
        }
        rgb.arr1 = rubrosArr;
        //Obtengo las cuentas que van asociadas
        TextBox t1;
        Label lb;
        TextBox t2;
        RE_GenericBean ctasbean = null;
        GridViewRow row;
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
                if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
                rgb.arr2.Add(ctasbean);
            }
        }

        int result = DB.InsertUpdateMatriz(rgb);

        if ((result == 0) || (result == 100))
        {
            WebMsgBox.Show("Existio un problema al momento de guardar la informacion, por favor intente de nuevo");
        }
        else {
            WebMsgBox.Show("Los datos fueron grabados exitosamente");
        }
    }
    private bool suma100(string columna) {
        bool result = false;
        GridViewRowCollection gvr = gv_conf_cuentas.Rows;
        TextBox t1 = null;
        int contador = 0;
        foreach (GridViewRow row in gvr)
        {
            t1 = (TextBox)row.FindControl(columna);
            contador += Int32.Parse(t1.Text.Trim());
        }
        if (contador == 100 || contador == 0)
            result=true;
        return result;
    }
}
