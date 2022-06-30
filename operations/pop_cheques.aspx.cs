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

public partial class operations_pop_cheques : System.Web.UI.Page
{
    DataTable dt = null;
    UsuarioBean user = null;

    public string querystring = "";
    ArrayList arrCtas = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        int tipoproveedor = 0;
        tb_valor.Text = Request.QueryString["total"].ToString();
        lb_facturas.Text = Request.QueryString["fact"].ToString();
        lb_proveedorID.Text = Request.QueryString["provID"].ToString();
        tipoproveedor = int.Parse(Request.QueryString["provtype"].ToString());
        tb_motivo.Text = "Pago de cortes" + lb_facturas.Text;
        tb_valor.ReadOnly = true;
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean proveedor=null;
        if (tipoproveedor==4) {
            proveedor=(RE_GenericBean)DB.getProveedorData(int.Parse(lb_proveedorID.Text), "");
            tb_acreditado.Text = proveedor.strC2;
        }
        if (!Page.IsPostBack) {
            obtengo_listas();
            cargo_cuentas();
        }
        lb_bancos_SelectedIndexChanged(sender, e);
    }

    protected void cargo_cuentas() {
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Cuenta de");
        int tranID = int.Parse(lb_transaccion.SelectedValue);
        int monID = int.Parse(lb_moneda.SelectedValue);
        int paisID = user.PaisID;
        int contaID = int.Parse(Session["Contabilidad"].ToString());
        int matOpID = DB.getMatrizOperacionID(tranID, monID, paisID, contaID);
        ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);
        if (ctas != null)
        {
            foreach (RE_GenericBean rgb in ctas)
            {
                object[] objArr = { rgb.strC1, rgb.strC3, rgb.strC2 };
                dt_temp.Rows.Add(objArr);
            }
            gv_detalle.DataSource = dt_temp;
            gv_detalle.DataBind();
        }
    }
    protected void obtengo_listas() {
        ArrayList arr = null;
        ListItem item = null;
        lb_moneda.Items.Clear();
        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_moneda.Items.Add(item);
        }
        // obtengo las transacciones
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='pop_cheques.aspx'");
        lb_transaccion.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_transaccion.Items.Add(item);
        }
        arr = (ArrayList)DB.getBancosXPais(user.PaisID,1);
        lb_bancos.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_bancos.Items.Add(item);
        }
        item = new ListItem("Seleccione...", "0");
        lb_cuentas_bancarias.Items.Add(item);
    }
    
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        
        decimal valor=decimal.Parse(tb_valor.Text);
        string facturas = lb_facturas.Text;
        
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.intC1 = int.Parse(lb_moneda.SelectedValue);
        rgb.intC2 = int.Parse(tb_chequeNo.Text);
        rgb.strC2 = tb_fecha.Text;
        rgb.decC1 = valor;
        rgb.strC3 = tb_acreditado.Text;
        rgb.strC4 = tb_motivo.Text;

        TextBox t1;
        Label lb;
        TextBox t2;
        GridViewRow row;
        RE_GenericBean ctasbean = null;
        for (int i = 0; i < gv_detalle.Rows.Count; i++)
        {
            row = gv_detalle.Rows[i];
            ctasbean = new RE_GenericBean();
            if (row.RowType == DataControlRowType.DataRow)
            {
                lb = (Label)row.FindControl("Label1");
                ctasbean.strC1 = lb.Text;//id de la cuenta
                lb = (Label)row.FindControl("Label2");
                ctasbean.strC2 = lb.Text;//nombre de la cuenta
                lb = (Label)row.FindControl("Label3");
                ctasbean.strC3 = lb.Text;//tipo cuenta
                t1 = (TextBox)row.FindControl("tb_cargos");
                t2 = (TextBox)row.FindControl("tb_abonos");
                ctasbean.decC1 = decimal.Parse(t1.Text.ToString());//cargo
                ctasbean.decC2 = decimal.Parse(t2.Text.ToString());//abono
                if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
                rgb.arr2.Add(ctasbean);
            }
        }

        string[] arr = facturas.Split(',');
        ArrayList temp = new ArrayList();
        for (int i = 1; i < arr.Count(); i++) {
            temp.Add(arr[i].ToString());
        }
        rgb.arr1 = temp;
        user = (UsuarioBean)Session["usuario"];
        DB.AplicoCheque(rgb, user);
        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.opener.location.href='index.aspx';";
        mensaje += "this.close();";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);


    }
    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        lb_cuentas_bancarias.Items.Clear();
        arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue),user.PaisID,1);
        foreach (RE_GenericBean rgb in arrCtas) {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_cuentas_bancarias.Items.Add(item);
        } 
    }

    protected void gv_detalle_RowDataBound(object sender, GridViewRowEventArgs e)
    {

            TextBox t1 = new TextBox();
            TextBox t2 = new TextBox();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                t1 = (TextBox)e.Row.FindControl("tb_cargos");
                t2 = (TextBox)e.Row.FindControl("tb_abonos");
                if (DataBinder.Eval(e.Row.DataItem, "Cuenta de").ToString() == "Cargo")
                {
                    t1.Enabled = true;
                    t2.Text = "0";
                    t2.Enabled = false;
                }
                else if (DataBinder.Eval(e.Row.DataItem, "Cuenta de").ToString() == "Abono")
                {
                    t1.Text = "0";
                    t1.Enabled = false;
                    t2.Enabled = true;
                }
                //if (Request.QueryString["provID"] != null)
                //{
                //    int provID = int.Parse(Request.QueryString["provID"].ToString());
                //    ArrayList arr = null; //(ArrayList)DB.getCtabyProvision(provID);
                //    if (arr != null)
                //    {
                //        foreach (RE_GenericBean tgb in arr)
                //        {
                //            if (DataBinder.Eval(e.Row.DataItem, "Cuenta ID").ToString() == tgb.strC1)
                //            {
                //                t1.Text = tgb.decC1.ToString();
                //                t2.Text = tgb.decC2.ToString();
                //            }
                //        }
                //    }
                //}
            }
 
    }
    protected void lb_moneda_SelectedIndexChanged(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Cuenta de");
        int tranID = int.Parse(lb_transaccion.SelectedValue);
        int monID = int.Parse(lb_moneda.SelectedValue);
        int paisID = user.PaisID;
        int contaID = int.Parse(Session["Contabilidad"].ToString());
        int matOpID = DB.getMatrizOperacionID(tranID, monID, paisID, contaID);
        ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);
        if (ctas != null)
        {
            foreach (RE_GenericBean rgb in ctas)
            {
                object[] objArr = { rgb.strC1, rgb.strC3, rgb.strC2 };
                dt_temp.Rows.Add(objArr);
            }
            gv_detalle.DataSource = dt_temp;
            gv_detalle.DataBind();
        }
    }
    protected void lb_cuentas_bancarias_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string cuentaID = lb_cuentas_bancarias.SelectedValue;
        //int monID = DB.GetMonedaIDbyCuentaBancaria(cuentaID);
        //lb_moneda.SelectedValue = monID.ToString();
        //lb_moneda_SelectedIndexChanged(sender, e);
    }
}
