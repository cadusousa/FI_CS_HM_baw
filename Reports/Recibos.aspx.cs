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

public partial class Reports_EstadoCuentaCliente : System.Web.UI.Page
{
    UsuarioBean user = null;
    private DataTable dt;
    ListItem item = null;
    ArrayList arr = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null) {
            Response.Redirect("../default.aspx");
        }
        #region Definir Fechas
        if (!IsPostBack)
        {
            DateTime Fecha = DateTime.Now;
            tb_fechaini.Text = "01/01/" + Fecha.Year.ToString();
            tb_fechafin.Text = Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString();
        }
        #endregion
        user = (UsuarioBean)Session["usuario"];
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad, user.PaisID);
    }

    private void obtengo_listas(int tipoconta, int paiID)
    {
        if (!Page.IsPostBack)
        {
            lb_sucursal.Items.Clear();
            lb_moneda.Items.Clear();
            lb_contabilidad.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            lb_sucursal.Items.Add(item);
            lb_moneda.Items.Add(item);
            lb_contabilidad.Items.Add(item);
            arr = null;
            arr = (ArrayList)DB.getSucursales(" and suc_pai_id="+paiID);
            foreach (SucursalBean rgb in arr)
            {
                item = new ListItem(rgb.Nombre, rgb.ID.ToString());
                lb_sucursal.Items.Add(item);
            }
        }
    }
    
    protected void bt_generar_Click(object sender, EventArgs e)
    {
        int solopendiente = 0;
        if (lb_sucursal.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe Seleccionar una Sucursal");
            return;
        }
        if ((tb_rango_ini.Text.Trim() != "")||(tb_rango_fin.Text.Trim() != ""))
        {
            if (tb_rango_ini.Text.Trim() == "")
            {
                WebMsgBox.Show("Debe ingresar el Rango Inicial");
                return;
            }
            if (tb_rango_fin.Text.Trim() == "")
            {
                WebMsgBox.Show("Debe ingresar el Rango Final");
                return;
            }
        }
        if (tb_fechaini.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha inicial");
            return;
        }
        if (tb_fechafin.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha final");
            return;
        }

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "REPORTE DE RECIBOS";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " Contabilidad.: " + lb_contabilidad.SelectedItem.Text + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fechaini.Text + " Fecha Final.: " + tb_fechafin.Text + " ,";
        mensaje_log += "Solo pendiente de Liquidar.: " + solopendiente + " Sucursal.: " + lb_sucursal.SelectedItem.Text + " Serie.: " + lb_serie.SelectedItem.Text + "  ,";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        if (chk_pendliq.Checked) solopendiente = 1;
        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('reports.aspx?reptype=5&serie=" + lb_serie.SelectedValue + "&sucID=" + lb_sucursal.SelectedValue + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&monID=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&r_ini="+tb_rango_ini.Text.Trim()+"&r_fin="+tb_rango_fin.Text.Trim()+"','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
    protected void lb_moneda_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem it = new ListItem();
        lb_serie.Items.Clear();
        it = new ListItem("Seleccione...","0");
        lb_serie.Items.Add(it);
        ArrayList arr = (ArrayList)DB.getSerieFactbySucReport(int.Parse(lb_sucursal.SelectedValue), 2, int.Parse(lb_contabilidad.SelectedValue), int.Parse(lb_moneda.SelectedValue));
        ArrayList arr2 = (ArrayList)DB.getSerieFactbySucReport(int.Parse(lb_sucursal.SelectedValue), 22, int.Parse(lb_contabilidad.SelectedValue), int.Parse(lb_moneda.SelectedValue));//2 porque es el tipo de documento para Recibos de Cortes
        foreach (string serie in arr)
        {
            it = new ListItem(serie);
            lb_serie.Items.Add(it);
        }
        foreach (string serie in arr2)
        {
            it = new ListItem(serie);
            lb_serie.Items.Add(it);
        }
    }
    protected void lb_sucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_sucursal.SelectedValue != "0")
        {
            lb_contabilidad.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            lb_contabilidad.Items.Add(item);
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
            foreach (RE_GenericBean rgb in arr)
            {
                if (rgb.intC1 == user.contaID)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_contabilidad.Items.Add(item);
                }
            }
            item = new ListItem("Seleccione...", "0");
            lb_moneda.Items.Clear();
            lb_moneda.Items.Add(item);
            lb_serie.Items.Clear();
            lb_serie.Items.Add(item);
        }
        else
        {
            item = new ListItem("Seleccione...", "0");
            lb_moneda.Items.Clear();
            lb_moneda.Items.Add(item);
            lb_serie.Items.Clear();
            lb_serie.Items.Add(item);
            lb_contabilidad.Items.Clear();
            lb_contabilidad.Items.Add(item);
        }
    }
    protected void lb_contabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_contabilidad.SelectedValue != "0")
        {
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            lb_moneda.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            lb_moneda.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            item = new ListItem("Seleccione...", "0");
            lb_serie.Items.Clear();
            lb_serie.Items.Add(item);
        }
        else
        {
            item = new ListItem("Seleccione...", "0");
            lb_serie.Items.Clear();
            lb_serie.Items.Add(item);
            lb_moneda.Items.Clear();
            lb_moneda.Items.Add(item);
        }
    }
}
