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
    public ArrayList camposfac_arr = null;
    public int suc_id=0;
    public int fac_id = 0;
    public int tipo = 1;
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
            ArrayList arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            ListItem itemito;
            foreach (RE_GenericBean rgb in arr)
            {
                itemito = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(itemito);
            }

            ListItem item = null;
            arr = null;
            arr = (ArrayList)DB.getRetencionesList(user.PaisID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                chklist_retencion.Items.Add(item);
            }

            ArrayList paises_arr = (ArrayList)DB.getPaises("");
            item = null;
            PaisBean paisbean = new PaisBean();

            for (int i = 0; i < paises_arr.Count; i++)
            {
                paisbean = (PaisBean)paises_arr[i];
                item = new ListItem(paisbean.Nombre, paisbean.ID.ToString());
                lb_pais.Items.Add(item);
            }

            //if (Request.QueryString["suc_id"] != null)
            //{
            //    suc_id = int.Parse(Request.QueryString["suc_id"].ToString().Trim());
            //    if (Request.QueryString["tipo"] != null) {
            //        tipo = int.Parse(Request.QueryString["tipo"].ToString().Trim());
            //    }
            //    lb_tipo_doc.SelectedValue = tipo.ToString();
            //    suc_bean = (SucursalBean)DB.getSucursal(suc_id);
            //    if (suc_bean == null)
            //    {
            //        Session["msg"] = "Error, No es posible asociar un documento sin especificar un departamento valido";
            //        Session["url"] = "searchsuc.aspx";
            //        Response.Redirect("message.aspx");
            //    }
            //    ListItem ite = new ListItem(suc_bean.Nombre.ToString(), suc_bean.ID.ToString());
            //    lb_suc.Items.Add(ite);
            //    if (Request.QueryString["id"] != null)
            //    {
            //        fac_id = int.Parse(Request.QueryString["id"].ToString().Trim());
            //        if (fac_id > 0) {
            //            RE_GenericBean fac_bean = (RE_GenericBean)DB.getFactura(fac_id, tipo);
            //            if (fac_bean == null)
            //            {
            //                Session["msg"] = "Error, No es posible obtener datos del documento especificado";
            //                Session["url"] = "addsuc.aspx?id=" + suc_id;
            //                Response.Redirect("message.aspx");
            //            }
            //            tb_facid.Text = fac_bean.intC1.ToString();
            //            tb_serie.Text = fac_bean.strC1;
            //            tb_NoInicial.Text = fac_bean.intC3.ToString();
            //            lb_moneda.SelectedValue = fac_bean.intC5.ToString();
            //            tb_size.Text=fac_bean.intC7.ToString();
            //            tb_font.Text=fac_bean.strC2;
            //            tb_interlineado.Text=fac_bean.intC6.ToString();
            //            tb_corrimiento.Text = fac_bean.intC8.ToString();
            //            tb_simbolo.Text = fac_bean.strC3;
            //        } else {
            //            tb_facid.Text = "0";
            //            tb_serie.Text = "";
            //            tb_NoInicial.Text = "0";
            //        }
            //    } else {
            //        tb_facid.Text = "0";
            //        tb_serie.Text = "";
            //        tb_NoInicial.Text = "0";
            //    }
            //    llenar_gridView();
            //} else {
            //    Session["msg"] = "Error, No es posible asociar un documento sin especificar un departamento valido";
            //    Session["url"] = "searchsuc.aspx";
            //    Response.Redirect("message.aspx");
            //}
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        RE_GenericBean fac_bean = new RE_GenericBean();
        fac_bean.intC1 = int.Parse(tb_facid.Text.Trim());
        //fac_bean.intC2 = int.Parse(lb_suc.SelectedValue);
        fac_bean.strC1 = tb_serie.Text.Trim().ToUpper();
        fac_bean.intC3 = int.Parse(tb_NoInicial.Text.Trim());
        fac_bean.intC5 = int.Parse(lb_moneda.SelectedValue);
        fac_bean.intC8 = int.Parse(lb_pais.SelectedValue);
        fac_bean.strC2 = tb_font.Text.Trim();
        if (!tb_interlineado.Text.Equals("")) fac_bean.intC6 = int.Parse(tb_interlineado.Text);
        if (!tb_size.Text.Equals("")) fac_bean.intC7 = int.Parse(tb_size.Text);
        if (!tb_simbolo.Text.Equals("")) fac_bean.strC3 = tb_simbolo.Text;
        int retencion=0;
        for (int a = 0; a < chklist_retencion.Items.Count; a++)
        {
            if (chklist_retencion.Items[a].Selected)
            {
                retencion = int.Parse(chklist_retencion.Items[a].Value);
                if (fac_bean.arr2 == null) fac_bean.arr2 = new ArrayList();
                fac_bean.arr2.Add(retencion.ToString());
            }
        }

        RE_GenericBean camposbean = null;
        TextBox t1, t2, t3;
        int x = 0, y = 0;
        Label lb;
        GridViewRow row;
        for (int i = 0; i < gv_conf_cuentas.Rows.Count; i++)
        {
            row = gv_conf_cuentas.Rows[i];
            if (row.RowType == DataControlRowType.DataRow) {
                t1 = (TextBox)row.FindControl("tb_posx");
                t2 = (TextBox)row.FindControl("tb_posy");
                t3 = (TextBox)row.FindControl("tb_notas");
                if (t1 != null && !t1.Text.Equals("")) x = int.Parse(t1.Text); else x = 0;
                if (t2 != null && !t2.Text.Equals("")) y = int.Parse(t2.Text); else y = 0;
                if (x > 0 && y > 0) { // si tiene configurada mas de alguna posicion
                    camposbean = new RE_GenericBean();
                    lb = (Label)row.FindControl("Label1");
                    camposbean.strC1 = lb.Text;//Nombre del campo
                    camposbean.intC1 = x;
                    camposbean.intC2 = y;
                    camposbean.strC2 = t3.Text;
                    if (fac_bean.arr1 == null) fac_bean.arr1 = new ArrayList();
                    fac_bean.arr1.Add(camposbean);
                }
            }
        }
        int result = DB.InsertUpdatePlantillaRetencion(fac_bean);
        if ((result == 0) || (result==-100))
        {
            WebMsgBox.Show("Error, No se pudieron grabar los datos que ingreso, por favor intente de nuevo.");
            return;
        }
        tb_facid.Text = result.ToString();
    }
    private void llenar_gridView() {
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Campo");
        dt_temp.Columns.Add("PosicionX");
        dt_temp.Columns.Add("PosicionY");
        dt_temp.Columns.Add("Notas");
        dt_temp.Clear();

        int retencion = 0;
        string campos = "CODIGO CLIENTE|NIT|NOMBRE|DIRECCION|FECHA EMISION|FECHA (D)|FECHA (M)|FECHA (A)|GIRO";
        string[] camparr = campos.Split('|');
        foreach (string campo in camparr)
        {
            object[] obj = { campo, "0", "0", "" };
            dt_temp.Rows.Add(obj);
        }
        campos = "";
        for (int a = 0; a < chklist_retencion.Items.Count; a++)
        {
            if (chklist_retencion.Items[a].Selected)
            {
                campos = "";
                retencion = int.Parse(chklist_retencion.Items[a].Value);
                campos +="NOMBRE_"+retencion;
                campos +="|MONTO_"+retencion;
                campos+="|MONTO EN  LETRAS_"+retencion;
                string[] camparr1 = campos.Split('|');
                foreach (string campo in camparr1)
                {
                    object[] obj = { campo, "0", "0", "" };
                    dt_temp.Rows.Add(obj);
                }
            }
        }
        
        gv_conf_cuentas.DataSource = dt_temp;
        gv_conf_cuentas.DataBind();
    }
    protected void gv_conf_cuentas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int facid = int.Parse(tb_facid.Text);
        if (facid>0){
            Label lb;
            TextBox t1, t2, t3;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                lb=(Label)e.Row.FindControl("Label1");
                t1 = (TextBox)e.Row.FindControl("tb_posx");
                t2 = (TextBox)e.Row.FindControl("tb_posy");
                t3 = (TextBox)e.Row.FindControl("tb_notas");
                RE_GenericBean rgb = DB.getDatosCampoPlantilla_retencion(facid, lb.Text);
                if (rgb != null) {
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
        string script = "window.open('preview_retencion.aspx?serie=" + tb_serie.Text.Trim().ToUpper() + "&pais="+lb_pais.SelectedValue+"','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
    protected void bt_agregar_Click(object sender, EventArgs e)
    {
        llenar_gridView();
    }
}
