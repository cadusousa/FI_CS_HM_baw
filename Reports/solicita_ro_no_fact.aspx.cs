using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_solicita_ro_no_fact : System.Web.UI.Page
{

    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];

    }
    protected void btn_generar_Click(object sender, EventArgs e)
    {
        if (tb_fechainicial.Text == "")
        {
            WebMsgBox.Show("Debe Ingresar la Fecha Inicial");
            return;
        }
        if (tb_fechafinal.Text == "")
        {
            WebMsgBox.Show("Debe Ingresar la Fecha Final");
            return;
        }

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "REPORTE DE ROUTING NO FACTURADOS";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Fecha Inicial.: " + tb_fechainicial.Text + " Fecha Final.: " + tb_fechafinal.Text + " ,";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        string carga_ruteada = "si";
        string cargas_fhc = "si";
        string maritimo_fhc = "si";
        string aereo_fhc = "si";
        string terrestre_fhc = "si";
        string wms = "si";
        string seguros = "si";
        string aduanas = "si";
        if (chk_carga_ruteada.Checked == true) { carga_ruteada = "si"; } else { carga_ruteada = "no"; }
        if (chk_cargas_cif.Checked == true){ cargas_fhc = "si"; } else { cargas_fhc = "no"; }
        if (chk_maritimo.Checked == true) { maritimo_fhc = "si"; } else { maritimo_fhc = "no"; }
        if (chk_aereo.Checked == true) { aereo_fhc = "si"; } else { aereo_fhc = "no"; }
        if (chk_terrestre.Checked == true) { terrestre_fhc = "si"; } else { terrestre_fhc = "no"; }
        if (chk_wms.Checked == true) { wms = "si"; } else { wms = "no"; }
        if (chk_seguros.Checked == true) { seguros = "si"; } else { seguros = "no"; }
        if (chk_aduanas.Checked == true) { aduanas = "si"; } else { aduanas = "no"; }




        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('reports.aspx?monID=" + user.Moneda + "&f_ini=" + tb_fechainicial.Text + "&f_fin=" + tb_fechafinal.Text + "&reptype=9&cargas_fhc=" + cargas_fhc + "&maritimo=" + maritimo_fhc + "&aereo=" + aereo_fhc + "&terrestre=" + terrestre_fhc + "&wms=" + wms + "&seguros=" + seguros + "&aduanas=" + aduanas + "&carga_ruteada=" + carga_ruteada + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
}