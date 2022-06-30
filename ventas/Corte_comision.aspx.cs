using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class ventas_Corte_comision : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    int bandera = 0;
    ListItem item = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        else
        {
            user = (UsuarioBean)Session["usuario"];
        }
        ArrayList Arr_Perfile = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        foreach (PerfilesBean Perfiles in Arr_Perfile)
        {
            if ((Perfiles.ID == 26))
            {
                bandera = 1;
            }
        }
        if (bandera == 0)
        {
            Response.Redirect("../default.aspx");
        }
        if (!Page.IsPostBack)
        {
            item = new ListItem("Seleccione...", "0");
            ddl_vendedores.Items.Add(item);
            item = null;
            Obtengo_vendedores();
        }

    }
    protected void btn_generar_Click(object sender, EventArgs e)
    {
        if (ddl_vendedores.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar un Vendedor");
            return;
        }
        if (tb_fechainicial.Text == "")
        {
            WebMsgBox.Show("Debe seleccionar una fecha de inicio");
            return;
        }
        if (tb_fechafinal.Text == "")
        {
            WebMsgBox.Show("Debe seleccionar una fecha de fin");
            return;
        }
        

        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('reports.aspx?monID=" + user.Moneda + "&f_ini=" + tb_fechainicial.Text + "&f_fin=" + tb_fechafinal.Text + "&reptype=1&ted=" + ddl_estado.SelectedValue.ToString() + "&vendedor="+ ddl_vendedores.SelectedValue.ToString() +"','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);

    }
    public void Obtengo_vendedores()
    {
        string criterio = " and tipo_usuario=1 and pais='" + user.pais.ISO + "' and pw_activo <> 0 ";
        ArrayList arr = (ArrayList)DB.getProveedor_cajachica(criterio);
        foreach (RE_GenericBean vendedor in arr)
        {
            item = new ListItem(vendedor.strC2, vendedor.intC1.ToString());
            ddl_vendedores.Items.Add(item);
        }

    }
}