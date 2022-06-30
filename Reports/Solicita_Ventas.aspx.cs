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
            if (bandera == 1)
            {
                lb_contabilidad.Visible = false;
                l_contabilidad.Visible = false;
            }
            else
            {
                item = new ListItem("CONSOLIDADO", "3");
                lb_contabilidad.Items.Add(item);
                lb_contabilidad.Visible = true;
                l_contabilidad.Visible = true;
            }

            //*********************************FIN RESTRICCION********************************************//
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
        string Documentos = "";
        int ban_Documentos = 0;
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
        for (int i = 0; i < CB_Documentos.Items.Count; i++)
        {
            if (CB_Documentos.Items[i].Selected == true)
            {
                Documentos += CB_Documentos.Items[i].Value + ",";
                ban_Documentos++;
            }
        }
        if (ban_Documentos == 0)
        {
            WebMsgBox.Show("Debe incluir al menos un Documento");
            return;
        }

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "LIBRO DE VENTAS";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " Contabilidad.: " + lb_contabilidad.SelectedItem.Text + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fechainicial.Text + " Fecha Final.: " + tb_fechafinal.Text + " ,";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        string consolidar = "no";
        if (chk_consolidado.Checked == true)
        {
            consolidar = "si";
        }
        



        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('Libro_Ventas.aspx?t_con=" + lb_contabilidad.SelectedValue + "&t_mon=" + lb_moneda.SelectedValue + "&serie=" + tb_serie.Text.Trim() + "&docts=" + Documentos + "&grupo=" + lb_grupo.SelectedValue + "&f_inicial=" + tb_fechainicial.Text.Trim() + "&f_final=" + tb_fechafinal.Text.Trim() + "&tipo=" + ddl_tipo.SelectedValue + "&consolidar=" + consolidar + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }

    protected void lb_contabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_contabilidad.SelectedValue == "3")
        {
            chk_consolidado.Checked = false;
            l_moneda_consolidado.Visible = false;
            chk_consolidado.Visible = false;
        }
        else
        {
            l_moneda_consolidado.Visible = true;
            chk_consolidado.Visible = true;
        }
    }
}
