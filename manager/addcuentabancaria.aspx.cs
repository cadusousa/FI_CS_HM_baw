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

public partial class manager_addfactura : System.Web.UI.Page
{
    public ArrayList campos_arr = null;
    public int bancoID=0;
    public string ctaID = "";
    int cuentaID = 0;
    public int tipo = 13;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            obtengo_listas(sender, e);

            if (Request.QueryString["bancoID"] != null)
                bancoID = int.Parse(Request.QueryString["bancoID"].ToString().Trim());

            lb_bancoID.Text = bancoID.ToString();

            if ((Request.QueryString["ctaID"] != null)&&(Request.QueryString["ctaID"].ToString() != "0"))
            {
                cuentaID = int.Parse(Request.QueryString["ctaID"].ToString().Trim());
                if (!ctaID.Equals("0"))
                {
                    RE_GenericBean cuenta = DB.getDataCuentaby_ctaID(cuentaID);

                    if (cuenta == null)
                    {
                        WebMsgBox.Show("Existio un problema al trata de obtener los datos, por favor intente de nuevo");
                        return;
                    }
                    lb_bancoID.Text = cuenta.intC1.ToString();
                    lb_tipo_doc.SelectedValue = tipo.ToString();
                    tb_chequeid.Text = cuenta.strC1;
                    lb_pais_SelectedIndexChanged(sender, e);
                    lb_pais.SelectedValue = cuenta.intC2.ToString();
                    lb_suc.SelectedValue = cuenta.intC8.ToString();
                    tb_NoInicial.Text = cuenta.intC5.ToString();
                    lb_moneda.SelectedValue = cuenta.intC4.ToString();
                    tb_font.Text = cuenta.strC2;
                    tb_interlineado.Text = cuenta.intC6.ToString();
                    tb_size.Text = cuenta.intC7.ToString();
                    lb_idiomas.SelectedValue = cuenta.intC10.ToString();
                    llenar_gridView();
                    //lb_pais_SelectedIndexChanged(sender, e);
                }
            }
        }
    }

    protected void obtengo_listas(object sender, EventArgs e)
    {
        ArrayList paises_arr = (ArrayList)DB.getPaises("");
        ListItem item = null;
        PaisBean paisbean = new PaisBean();

        for (int i = 0; i < paises_arr.Count; i++)
        {
            paisbean = (PaisBean)paises_arr[i];
            item = new ListItem(paisbean.Nombre, paisbean.ID.ToString());
            lb_pais.Items.Add(item);
        }

        ArrayList arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_moneda");
        ListItem itemito;
        foreach (RE_GenericBean rgb in arr)
        {
            itemito = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_moneda.Items.Add(itemito);
        }
        lb_pais_SelectedIndexChanged(sender, e);
        ListItem it = null;
        arr = null;
        arr = (ArrayList)DB.getIdiomas();
        it = null;
        foreach (RE_GenericBean rgb in arr)
        {
            it = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_idiomas.Items.Add(it);
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        RE_GenericBean cuenta = new RE_GenericBean();
        cuenta.strC1 = tb_chequeid.Text;//cuenta bancaria
        cuenta.intC1 = int.Parse(lb_bancoID.Text);//banco id
        cuenta.intC2 = int.Parse(lb_pais.SelectedValue);//pais id
        cuenta.intC4 = int.Parse(lb_moneda.SelectedValue);//moneda id
        cuenta.intC5 = int.Parse(tb_NoInicial.Text);//correlativo id
        cuenta.strC2 = tb_font.Text;// font
        if (!tb_interlineado.Text.Equals("")) cuenta.intC6 = int.Parse(tb_interlineado.Text);// interlineado
        if (!tb_size.Text.Equals("")) cuenta.intC7 = int.Parse(tb_size.Text);// size
        cuenta.intC8 = int.Parse(lb_suc.SelectedValue);// suc id
        cuenta.intC9 = int.Parse(lb_idiomas.SelectedValue);//Idioma ID
        cuenta.intC10 = int.Parse(Request.QueryString["ctaID"].ToString());//id de la cuenta
        RE_GenericBean camposbean = null;
        TextBox t1, t2, t3;
        int x = 0, y = 0;
        Label lb;
        GridViewRow row;
        for (int i = 0; i < gv_conf_cuentas.Rows.Count; i++)
        {
            row = gv_conf_cuentas.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                t1 = (TextBox)row.FindControl("tb_posx");
                t2 = (TextBox)row.FindControl("tb_posy");
                t3 = (TextBox)row.FindControl("tb_notas");
                if (t1 != null && !t1.Text.Equals("")) x = int.Parse(t1.Text); else x = 0;
                if (t2 != null && !t2.Text.Equals("")) y = int.Parse(t2.Text); else y = 0;
                if (x > 0 && y > 0)
                { // si tiene configurada mas de alguna posicion
                    camposbean = new RE_GenericBean();
                    lb = (Label)row.FindControl("Label1");
                    camposbean.strC1 = lb.Text;//Nombre del campo
                    camposbean.intC1 = x;
                    camposbean.intC2 = y;
                    camposbean.strC2 = t3.Text;
                    if (cuenta.arr1 == null) cuenta.arr1 = new ArrayList();
                    cuenta.arr1.Add(camposbean);
                }
            }
        }
        int existe_cuenta = 0;
        if (cuenta.intC10 == 0)
        {
            existe_cuenta = DB.Validar_Existencia_Cuenta_Bancaria(cuenta.strC1.Trim());
            if (existe_cuenta == -100)
            {
                WebMsgBox.Show("Existio un error al Validar la Cuenta Bancaria");
            }
            else if (existe_cuenta > 0)
            {
                WebMsgBox.Show("El numero de Cuenta Bancaria.: " + cuenta.strC1.Trim() + " ya existe, por favor ingrese un numero de Cuenta Valido ");
                bt_Enviar.Enabled = false;
                return;
            }
        }
        int result = DB.InsertUpdateCuentaCheque(cuenta);
        if ((result == 0) || (result == -100))
        {
            WebMsgBox.Show("Error, No se pudieron grabar los datos que ingreso, por favor intente de nuevo.");
        }
        else
        {
            Response.Redirect("~/manager/addcuentabancaria.aspx?id=" + Request.QueryString["id"].ToString() + "&bancoID=" + Request.QueryString["bancoID"].ToString() + "&ctaID=" + result + "");
        }
        
    }
    private void llenar_gridView() {
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Campo");
        dt_temp.Columns.Add("PosicionX");
        dt_temp.Columns.Add("PosicionY");
        dt_temp.Columns.Add("Notas");
        dt_temp.Clear();


        string campos = "NOMBRE|FECHA EMISION|FECHA (D)|FECHA (D)_1|FECHA (D)_2|FECHA (M)|FECHA (M)_1|FECHA (M)_2|FECHA (M) LETRAS|FECHA (A)|FECHA (A)_1|FECHA (A)_2|FECHA (A)_3|FECHA (A)_4|TOTAL|TOTAL LETRAS|NOTA CHEQUE(NO NEGOCIABLE)";
        campos += "|FECHA EMISION VOUCHER|FECHA (D) VOUCHER|FECHA (M) VOUCHER|FECHA (M) VOUCHER LETRAS|FECHA (A) VOUCHER|TOTAL LETRAS VOUCHER";
        campos += "|NOMBRE VOUCHER|BANCO VOUCHER|CONCEPTO VOUCHER|TOTAL VOUCHER|HECHO POR VOUCHER|CUENTA BANCARIA VOUCHER";
        campos += "|REFERENCIA|TIPO CAMBIO|FECHA IMPRESION|OBSERVACIONES|NO CHEQUE|NO CHEQUE VOUCHER|TOTAL CARGO|TOTAL ABONO";
        campos += "|ID CUENTA CONTABLE VOUCHER|NOMBRE CUENTA CONTABLE VOUCHER|ABONO|CARGO|HORA";
        campos += "|TOTAL_EQUIVALENTE|TOTAL_LETRAS_EQUIVALENTE|CARGO_EQUIVALENTE|ABONO_EQUIVALENTE|TOTAL_CARGO_EQUIVALENTE|TOTAL_ABONO_EQUIVALENTE|TOTAL_VOUCHER_EQUIVALENTE|TOTAL_LETRAS_VOUCHER_EQUIVALENTE";
        
        //campos += "|CUENTA VOUCHER";
        string[] camparr = campos.Split('|');
        foreach (string campo in camparr)
        {
            object[] obj = { campo, "0", "0", "" };
            dt_temp.Rows.Add(obj);
        }
        
        gv_conf_cuentas.DataSource = dt_temp;
        gv_conf_cuentas.DataBind();
    }
    protected void gv_conf_cuentas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Request.QueryString["ctaID"] != null)
        {
            cuentaID = int.Parse(Request.QueryString["ctaID"].ToString().Trim());
            Label lb;
            TextBox t1, t2, t3;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                lb = (Label)e.Row.FindControl("Label1");
                t1 = (TextBox)e.Row.FindControl("tb_posx");
                t2 = (TextBox)e.Row.FindControl("tb_posy");
                t3 = (TextBox)e.Row.FindControl("tb_notas");
                RE_GenericBean rgb = DB.getDatosCampoPlantillaChequeby_ctaID(cuentaID, lb.Text);
                if (rgb != null)
                {
                    t1.Text = rgb.intC1.ToString();
                    t2.Text = rgb.intC2.ToString();
                    t3.Text = rgb.strC2;
                }
            }
        }
    }
    protected void bt_preview_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('../invoice/preview_cheque.aspx?ctaID="+tb_chequeid.Text+"','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
    protected void lb_pais_SelectedIndexChanged(object sender, EventArgs e)
    {
        lb_suc.Items.Clear();
        int paiID = int.Parse(lb_pais.SelectedValue);
        ArrayList arr = (ArrayList)DB.getSucursales_pais(" and suc_pai_id=" + paiID);
        ListItem item = null;
        if (arr != null && arr.Count > 0)
        {
            foreach (SucursalBean suc in arr)
            {
                item = new ListItem(suc.Nombre, suc.ID.ToString());
                lb_suc.Items.Add(item);
            }
        }
    }
}
