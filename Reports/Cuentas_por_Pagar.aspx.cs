/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;*/

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

public partial class Reports_Cuentas_por_Pagar : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        #region Definir Fechas
        if (!IsPostBack)
        {
            DateTime Fecha = DateTime.Now;
            //  tb_fechainicial.Text = "01/01/" + Fecha.Year.ToString();
            //  tb_fechafinal.Text = Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString();
        }
        #endregion
        user = (UsuarioBean)Session["usuario"];
        /*if (Request.QueryString.Count > 0)
        {
            lb_reporte.SelectedValue = Request.QueryString["tipo"].ToString();
            if (Request.QueryString["tipo"].ToString() == "3")
            {
                lb_mbl.Visible = true;
                tb_mbl.Visible = true;
            }
            else
            {
                lb_mbl.Visible = false;
                tb_mbl.Visible = false;
            }
        }*/
        if (!Page.IsPostBack)
        {
            obtengo_lista();
        }
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

    protected void btn_generar_Click(object sender, EventArgs e)
    {
        //--------------

        
        if (tb_fechafinal.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha Final");
            return;
        }

        string fecha_final = tb_fechafinal.Text.Trim();
        #region Formatear Fechas
        

        //Fecha Fin
        int fe_dia = int.Parse(fecha_final.Substring(3, 2));
        int fe_mes = int.Parse(fecha_final.Substring(0, 2));
        int fe_anio = int.Parse(fecha_final.Substring(6, 4));
        fecha_final = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            fecha_final += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            fecha_final += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            fecha_final += "0" + fe_dia.ToString();
        }
        else
        {
            fecha_final += fe_dia.ToString();
        }
        

        #endregion


        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "REPORTE DE CUENTAS POR PAGAR";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Contabilidad.: " + user.contaID + " ,";
        mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " ,";
        mensaje_log += "Fecha Corte.: " + tb_fechafinal.Text + " ,";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion
        
        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('reports.aspx?reptype=12&fechafin=" + fecha_final + "&monID=" + lb_moneda.SelectedValue + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
}