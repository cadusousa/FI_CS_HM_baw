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
        if (Session["usuario"] == null) 
        {
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
                lb_contabilidad.Visible = true;
                l_contabilidad.Visible = true;
            }
            //*********************************FIN RESTRICCION********************************************//


        }
        Rb_Tipo_Persona_SelectedIndexChanged(sender, e);
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
        arr = (ArrayList)DB.getRetencionesList(user.PaisID);
        lb_tipo_retencion_proveedores.Items.Clear();
        item= new ListItem("Todas", "0");
        lb_tipo_retencion_proveedores.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_tipo_retencion_proveedores.Items.Add(item);
        }
    }
    protected void btn_generar_Click(object sender, EventArgs e)
    {
        string Personas = "";
        int ban_Personas = 0;
        int aux = 0;
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
        if (Rb_Tipo_Persona.SelectedValue == "4")
        {
            for (int i = 0; i < CB_Personas.Items.Count; i++)
            {
                if (CB_Personas.Items[i].Selected == true)
                {
                    ban_Personas++;
                }
            }
            if (ban_Personas == 0)
            {
                WebMsgBox.Show("Debe incluir algun Tipo de Persona");
                return;
            }
            for (int i = 0; i < CB_Personas.Items.Count; i++)
            {
                if (CB_Personas.Items[i].Selected == true)
                {
                    if (ban_Personas == 1)
                    {
                        Personas = CB_Personas.Items[i].Value;
                    }
                    else
                    {
                        if ((ban_Personas - aux) == 1)
                        {
                            Personas += CB_Personas.Items[i].Value;
                            aux++;
                        }
                        else
                        {
                            Personas += CB_Personas.Items[i].Value + ",";
                            aux++;
                        }
                    }
                }
            }
        }
        string mensaje = "<script languaje=\"JavaScript\">";

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "REPORTE DE RETENCIONES";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        if (Rb_Tipo_Persona.SelectedValue == "3")
        {
            mensaje_log += " CLIENTES, ";
        }
        else
        {
            mensaje_log += " PROVEEDORES, ";
        }
        mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " Contabilidad.: " + lb_contabilidad.SelectedItem.Text + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fechainicial.Text + " Fecha Final.: " + tb_fechafinal.Text + " ,";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        if (Rb_Tipo_Persona.SelectedValue == "3")
        {
            mensaje += "window.open('Reporte_Retenciones.aspx?t_con=" + lb_contabilidad.SelectedValue + "&t_mon=" + lb_moneda.SelectedValue + "&t_rep=" + Rb_Tipo_Persona.SelectedValue + "&t_ret=" + lb_tipo_retencion_clientes.SelectedValue + "&t_tpi=3&f_inicial=" + tb_fechainicial.Text.Trim() + "&f_final=" + tb_fechafinal.Text.Trim() + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        }

        else
        {
            mensaje += "window.open('Reporte_Retenciones.aspx?t_con=" + lb_contabilidad.SelectedValue + "&t_mon=" + lb_moneda.SelectedValue + "&t_rep=" + Rb_Tipo_Persona.SelectedValue + "&t_ret=" + lb_tipo_retencion_proveedores.SelectedValue + "&t_tpi=" + Personas + "&f_inicial=" + tb_fechainicial.Text.Trim() + "&f_final=" + tb_fechafinal.Text.Trim() + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        }
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
    protected void Rb_Tipo_Persona_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Rb_Tipo_Persona.SelectedValue == "3")
        {
            lb_tipo_retencion_clientes.Visible = true;
            lb_tipo_retencion_proveedores.Visible = false;
            lbl_personas.Visible = false;
            CB_Personas.Visible = false;
        }
        else
        {
            lb_tipo_retencion_clientes.Visible = false;
            lb_tipo_retencion_proveedores.Visible = true;
            lbl_personas.Visible = true;
            CB_Personas.Visible = true;
        }

    }
}
