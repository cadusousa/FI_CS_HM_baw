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

public partial class operations_movimiento_contable : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
            Response.Redirect("../default.aspx");

        user = (UsuarioBean)Session["usuario"];

        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 131072) == 131072))
            Response.Redirect("index.aspx");
        if (!Page.IsPostBack)
        {
            obtengo_lista();

            //************************RESTRICCION DE CONTABILIDAD USUARIOS****************************//
            lb_contabilidad.Items.Clear();
            int bandera = 0; // verifica si el usuario tiener contabilidad consolidada.
            int fiscal = 0;
            int financiera = 0;
            ListItem item = null;
            ArrayList arruser = (ArrayList)DB.getContaUsuario(user.ID, user.PaisID);
            ArrayList arrbloqueo = (ArrayList)DB.getContaPais(user.PaisID);
            foreach (RE_GenericBean rgbp in arrbloqueo)
            {
                if (rgbp.intC1 == 1)
                {
                    fiscal = 1; //desbloqueo fiscal
                }

                if (rgbp.intC2 == 1)
                {
                    financiera = 1; // desbloqueo financiera.
                }
            }
            foreach (RE_GenericBean rgb in arruser)
            {
                if ((rgb.intC1 == 1) && (fiscal == 1))
                {
                    bandera++;
                    item = new ListItem("FISCAL", "1");
                    lb_contabilidad.Items.Add(item);
                }

                if ((rgb.intC2 == 1) && (financiera == 1))
                {
                    bandera++;
                    item = new ListItem("FINANCIERA", "2");
                    lb_contabilidad.Items.Add(item);
                }
            }
            if (bandera == 2)
            {
                item = new ListItem("CONSOLIDADO", "3");
                lb_contabilidad.Items.Add(item);
            }
            if (bandera == 1)
            {
                lb_contabilidad.Visible = false;
                l_contabilidad.Visible = false;
            }
            else
            {
                lb_contabilidad.Visible = true;
                l_contabilidad.Visible = true;
            }

            //*********************************FIN RESTRICCION********************************************//


        }
    }
    protected void bt_buscar_cta_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        int clasificacion = int.Parse(lb_clasificacion.SelectedValue);
        if (clasificacion != 0) where += " and cue_clasificacion in (" + clasificacion + ")";
        if (!tb_nombre_cta.Text.Trim().Equals("") && tb_nombre_cta.Text != null) where += " and UPPER(cue_nombre) like '%" + tb_nombre_cta.Text.Trim().ToUpper() + "%'";
        if (!tb_cuenta_numero.Text.Trim().Equals("") && tb_cuenta_numero.Text != null) where += " and(cue_id) ilike '%" + tb_cuenta_numero.Text.Trim() + "%'";
        Arr = Utility.getCuentasContables("XTipoClasificacion", where);
        dt = (DataTable)Utility.fillGridView("Cuenta", Arr);
        gv_cuenta.DataSource = dt;
        gv_cuenta.DataBind();
        ViewState["gv_cuenta_dt"] = dt;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void gv_cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_cuenta.DataSource = dt;
        gv_cuenta.PageIndex = e.NewPageIndex;
        gv_cuenta.DataBind();
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void gv_cuenta_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ArrayList cuentaArr = null;
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
        tb_cuenta_nombre.Text = row.Cells[2].Text;
    }
    protected void bt_visualizar_Click(object sender, EventArgs e)
    {
        if (tb_cuenta.Text == "")
        {
            WebMsgBox.Show("Debe Ingresar una cuenta Contable");
            return;
        }
        if (tb_fecha_inicial.Text == "")
        {
            WebMsgBox.Show("Debe Ingresar la Fecha Inicial");
            return;
        }
        if (tb_fecha_final.Text == "")
        {
            WebMsgBox.Show("Debe Ingresar la Fecha Final");
            return;
        }
        string Fecha_Inicial = DB.DateFormat(tb_fecha_inicial.Text);
        string Fecha_Final = DB.DateFormat(tb_fecha_final.Text);
        DateTime inicial = DateTime.Parse(Fecha_Inicial.ToString());
        DateTime final = DateTime.Parse(Fecha_Final.ToString());
        TimeSpan dd = final.Subtract(inicial);
        double dif_en_dias = dd.TotalDays;
        //------------------------------------------------------------
        //if (dif_en_dias > 31)
        //{
        //    WebMsgBox.Show("No puede generar Movimientos mayores a 31 dias");
        //    return;
        //}

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "MOVIMIENTO DE CUENTAS CONTABLES";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Contabilidad.: " + user.contaID + " Moneda.: " + lb_moneda.SelectedItem.Text + " ,";
        mensaje_log += "Cuenta ID.: " + tb_cuenta.Text + " Cuenta Nombre.: " + tb_cuenta_nombre.Text+ " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fecha_inicial.Text + " Fecha Final.: " + tb_fecha_final.Text + " ,";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('../Reports/SubReporte_BS.aspx?fecha_inicial=" + Fecha_Inicial + "&fecha_final=" + Fecha_Final + "&cue_id=" + tb_cuenta.Text + "&contaID=" + lb_contabilidad.SelectedValue + "&monID=" + lb_moneda.SelectedValue + "&typerep=1','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
    protected void obtengo_lista()
    {
        ArrayList arr = null;
        ListItem item = null;
        user = (UsuarioBean)Session["usuario"];
        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_moneda.Items.Add(item);
        }
    }
}

