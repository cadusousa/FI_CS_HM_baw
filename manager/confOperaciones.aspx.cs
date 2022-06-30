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

public partial class manager_confOperaciones : System.Web.UI.Page
{
    DataTable dt = null;
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        string operacion = "";
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
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
                //Cargo las contabilidades
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
                lb_tipo_contabilidad.Items.Clear();
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_tipo_contabilidad.Items.Add(item);
                }
                //Cargo los paises
                arr = (ArrayList)DB.getPaises("");
                lb_pais.Items.Clear();
                foreach (PaisBean pais in arr)
                {
                    item = new ListItem(pais.Nombre, pais.ID.ToString());
                    lb_pais.Items.Add(item);
                }
                int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
                if (tipo_contabilidad == 2)
                {
                    lb_moneda.SelectedValue = "8";
                    
                }
                else
                {
                    lb_moneda.SelectedValue = user.PaisID.ToString();
                    
                }
                lb_pais.SelectedValue = user.PaisID.ToString();
                lb_pais.Enabled = false;
                lb_tipo_contabilidad.SelectedValue = tipo_contabilidad.ToString();
                lb_tipo_contabilidad.Enabled = false;
                bt_buscar_cta_Click(sender, e);

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
            //cuentaArr = Utility.getCuentasContables("XClasificacion", "1,2,3");
            //dt = (DataTable)Utility.fillGridView("Cuenta", cuentaArr);
            //ViewState["gv_cuenta_dt"] = dt;
            //gv_cuenta.DataSource = dt;
            //gv_cuenta.DataBind();
        }
        else
        {
            //dt = (DataTable)ViewState["gv_cuenta_dt"];
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
        if (!tb_nombre_cta.Text.Trim().Equals("") && tb_nombre_cta.Text != null) where += " and UPPER(cue_nombre) like '%" + tb_nombre_cta.Text.Trim().ToUpper() + "%'";
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
        dt_temp.Columns.Add("Cuenta de");
        GridViewRowCollection gvr = gv_conf_cuentas.Rows;
        dt_temp.Clear();
        foreach (GridViewRow row in gvr) {
            object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text };
            dt_temp.Rows.Add(objArr);
        }
        if (lb_ctade.SelectedValue.Equals("Cargo")) {
            object[] objArr = { tb_cuenta.Text.Trim(), tb_cta_nombre.Text.Trim(), "Cargo" };
            dt_temp.Rows.Add(objArr);
        } else if (lb_ctade.SelectedValue.Equals("Abono")) {
            object[] objArr = { tb_cuenta.Text.Trim(), tb_cta_nombre.Text.Trim(), "Abono" };
            dt_temp.Rows.Add(objArr);
        }
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
        dt_temp.Columns.Add("Cuenta de");
        GridViewRowCollection gvr = gv_conf_cuentas.Rows;
        dt_temp.Clear();
        foreach (GridViewRow row in gvr)
        {
            object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text };
            dt_temp.Rows.Add(objArr);
        }
        dt_temp.Rows[e.RowIndex].Delete();
        gv_conf_cuentas.DataSource = dt_temp;
        gv_conf_cuentas.DataBind();
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        int tranID = int.Parse(lb_transaccion.SelectedValue);
        int monID = int.Parse(lb_moneda.SelectedValue);
        int paisID = int.Parse(lb_pais.SelectedValue);
        int contaID = int.Parse(lb_tipo_contabilidad.SelectedValue);
        int matOpID = DB.getMatrizOperacionID(tranID, monID, paisID, contaID);
        GridViewRow row;
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.intC1 = tranID;
        rgb.intC2 = monID;
        rgb.intC3 = paisID;
        rgb.intC4 = matOpID;
        rgb.intC5 = contaID;
        RE_GenericBean ctasbean = null;
        for (int i = 0; i < gv_conf_cuentas.Rows.Count; i++)
        {
            row = gv_conf_cuentas.Rows[i];
            ctasbean = new RE_GenericBean();
            if (row.RowType == DataControlRowType.DataRow)
            {
                ctasbean.strC1 = row.Cells[1].Text;//id de la cuenta
                ctasbean.strC2 = row.Cells[2].Text;//nombre de la cuenta
                ctasbean.strC3 = row.Cells[3].Text;//tipo cuenta
                if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                rgb.arr1.Add(ctasbean);
            }
        }
        int result = DB.InsertUpdateMatrizConfiguracion(rgb);
        if (result == 0)
        {
            Session["msg"] = "Error, No se pudieron grabar los datos que ingreso, por favor intente de nuevo.";
            Session["url"] = "newrcpt.aspx";
            Response.Redirect("message.aspx");
        }
        else if (result == -100)
        {
            Session["msg"] = "Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.";
            Session["url"] = "newrcpt.aspx";
            Response.Redirect("message.aspx");
        }

    }
    protected void bt_buscar_conv_Click(object sender, EventArgs e)
    {
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Cuenta de");
        int tranID = int.Parse(lb_transaccion.SelectedValue);
        int monID = int.Parse(lb_moneda.SelectedValue);
        int paisID = int.Parse(lb_pais.SelectedValue);
        int contaID = int.Parse(lb_tipo_contabilidad.SelectedValue);
        int matOpID = DB.getMatrizOperacionID(tranID, monID, paisID, contaID);
        ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);
        if (ctas != null)
        {
            foreach (RE_GenericBean rgb in ctas)
            {
                object[] objArr = { rgb.strC1, rgb.strC3, rgb.strC2 };
                dt_temp.Rows.Add(objArr);
            }
            gv_conf_cuentas.DataSource = dt_temp;
            gv_conf_cuentas.DataBind();
        }
    }
}
