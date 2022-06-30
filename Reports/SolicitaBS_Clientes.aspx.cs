using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_SolicitaER_2 : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null) {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];

        if (!Page.IsPostBack)
        {
            obtengo_listas();
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

    protected void obtengo_listas()
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
        string Teds = "";
        int ban_Estados = 0;
        int ban_aux = 0;
        int ban_saldo=0;
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
        for (int j = 0; j < CB_Estados.Items.Count; j++)
        {
            if (CB_Estados.Items[j].Selected == true)
            {
                ban_Estados++;
            }
        }
        if (ban_Estados == 0)
        {
            WebMsgBox.Show("Debe seleccionar el estado de los Documentos");
            return;
        }
        for (int j = 0; j < CB_Estados.Items.Count; j++)
        {
            if (CB_Estados.Items[j].Selected == true)
            {
                ban_aux++;
                if (ban_Estados == 1)
                {
                    Teds = CB_Estados.Items[j].Value;
                }
                else if (ban_Estados > 1)
                {
                    if (ban_aux == ban_Estados)
                    {
                        Teds += CB_Estados.Items[j].Value;
                    }
                    else
                    {
                        Teds += CB_Estados.Items[j].Value + ",";
                    }
                }
            }
        }
        if (Chk_saldo.Checked==true)
        {
            ban_saldo = 1;
        }
        else
        {
            ban_saldo = 0;
        }
        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('balance_saldos_clientes.aspx?monID=" + lb_moneda.SelectedValue + "&tcon=" + lb_contabilidad.SelectedValue + "&fecha_inicial=" + tb_fechainicial.Text + "&fecha_final=" + tb_fechafinal.Text + "&ted=" + Teds + "&ban_saldo=" + ban_saldo + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
}
