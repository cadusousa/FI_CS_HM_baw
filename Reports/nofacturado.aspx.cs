using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_nofacturado : System.Web.UI.Page
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
        Bean_Log.strC1 = "REPORTE DE CARGOS NO FACTURADOS";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Contabilidad.: " + user.contaID + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fechainicial.Text + " Fecha Final.: " + tb_fechafinal.Text + " ,";
        mensaje_log += "Maritimo=" + chb_maritimo.Checked + "  Aereo=" + chb_aereo.Checked + "  Terrestre=" + chb_terrestre.Checked + "  WMS=" + chb_wms.Checked + "  Seguros=" + chb_seguros.Checked + " ,";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

            string mensaje = "<script languaje=\"JavaScript\">";
            mensaje += "window.open('viewnofacturado.aspx?f_ini=" + tb_fechainicial.Text + "&f_fin=" + tb_fechafinal.Text + "&maritimo=" + chb_maritimo.Checked + "&aereo=" + chb_aereo.Checked + "&terrestre=" + chb_terrestre.Checked + "&wms=" + chb_wms.Checked + "&seguros=" + chb_seguros.Checked + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            mensaje += "</script>";
            Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
}