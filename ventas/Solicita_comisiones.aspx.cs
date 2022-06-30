using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ventas_Solicita_comisiones : System.Web.UI.Page
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
        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('reports.aspx?monID=" + user.Moneda + "&f_ini=" + tb_fechainicial.Text + "&f_fin=" + tb_fechafinal.Text + "&reptype=2&ted="+ ddl_estado.SelectedValue.ToString() +" ','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);

    }
}