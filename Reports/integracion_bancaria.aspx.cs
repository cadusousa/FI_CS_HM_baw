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

public partial class Reports_integracion_bancaria : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        if (Session["usuario"] == null)
            Response.Redirect("../default.aspx");

        user = (UsuarioBean)Session["usuario"];

        if (!Page.IsPostBack)
        {
            
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }

            item = null;
             //************************RESTRICCION DE CONTABILIDAD USUARIOS****************************//
            lb_contabilidad.Items.Clear();
            int bandera = 0; // verifica si el usuario tiener contabilidad consolidada.
            int fiscal = 0;
            int financiera = 0;
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

            item = null;
            arr = (ArrayList)DB.getBancosXPais(user.PaisID, 1);
            lb_bancos.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            lb_bancos.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_bancos.Items.Add(item);
            }
            item = new ListItem("Seleccione...", "0");
            lb_cuentas_bancarias.Items.Add(item);
        }

    }
    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
          
            ListItem item = null;
            UsuarioBean user;
            user = (UsuarioBean)Session["usuario"];
            lb_cuentas_bancarias.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            item.Selected = true;
            lb_cuentas_bancarias.Items.Add(item);
            ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue), user.PaisID,0);
            foreach (RE_GenericBean rgb in arrCtas)
            {
                item = new ListItem(rgb.strC1, rgb.strC1);
                lb_cuentas_bancarias.Items.Add(item);
            }
            btn_seleccionar_banco.Enabled = false;
      
    }
    protected void btn_generar_Click(object sender, EventArgs e)
    {

        if (tb_fechainicial.Text.Equals(""))
        {
            WebMsgBox.Show(" Debe de ingresar una fecha de inicio");
            return;
        }
        if (tb_fechafinal.Text.Equals(""))
        {
            WebMsgBox.Show(" Debe de ingresar una fecha final");
            return;
        }

        //Validacion para tipo de cambio en fecha
        user = (UsuarioBean)Session["usuario"];
        string fechafin = tb_fechafinal.Text;

        int fe_dia = int.Parse(fechafin.Substring(3, 2));
        int fe_mes = int.Parse(fechafin.Substring(0, 2));
        int fe_anio = int.Parse(fechafin.Substring(6, 4));
        fechafin = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            fechafin += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            fechafin += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            fechafin += "0" + fe_dia.ToString();
        }
        else
        {
            fechafin += fe_dia.ToString();
        }

        string tc_ultimo = DB.getTipoCambioByDay(user, fechafin).ToString();
        if (tc_ultimo == "0")
        {
            WebMsgBox.Show("No existe tipo de cambio para la Fecha Final seleccionada");
            return;
        }

        // Fecha fin 

        int banco = 0;
        string cuenta = "";
        string cuentas = "";
        string bancos = "";
        GridViewRow row = null;
        if (gv_bancos.Rows.Count > 0)
        {
            for (int i = 0; i < gv_bancos.Rows.Count; i++)
            {
                row = gv_bancos.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if ((bancos != "") && (cuentas != ""))
                    {
                        bancos = row.Cells[0].Text.ToString() + "," + bancos;//id banco
                        cuentas = row.Cells[1].Text.ToString() + "," + cuentas;//cuenta
                    }
                    else
                    {
                        bancos = row.Cells[0].Text.ToString();//id banco
                        cuentas =  row.Cells[1].Text.ToString();//cuenta
                    }
                }
            }
        }
        else
        {
            if ((lb_bancos.SelectedValue.ToString() != ""))
            {
                banco = int.Parse(lb_bancos.SelectedValue.ToString());
                cuenta = lb_cuentas_bancarias.SelectedItem.ToString();
            }
            else
            {
                WebMsgBox.Show("Debe elegir un criterio de busqueda.");
                return;
            }
        }

        string fecha_ini = tb_fechainicial.Text.Trim();
        string fecha_fin = tb_fechafinal.Text.Trim();

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "INTEGRACION BANCARIA";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " Contabilidad.: " + lb_contabilidad.SelectedItem.Text + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fechainicial.Text + " Fecha Final.: " + tb_fechafinal.Text + " ,";
        mensaje_log += "Bancos.: " + banco + ", Cuentas.: " + cuenta + " ";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('reports.aspx?reptype=11&banco=" + banco + "&cuenta=" + cuenta + "&fechaini=" + fecha_ini + "&fechafin=" + fecha_fin + "&monID=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&bancos=" + bancos + "&cuentas=" + cuentas + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);

    }

    protected void bt_buscar_Click(object sender, EventArgs e)
    {
    }

    protected void btn_seleccionar_banco_Click(object sender, EventArgs e)
    {

        lb_bancos.Items.Clear();
        lb_cuentas_bancarias.Items.Clear();

        lb_bancos.Enabled = false;
        lb_cuentas_bancarias.Enabled = false;

        ArrayList arr = null;
        ListItem item = null;
        item = null;
        arr = (ArrayList)DB.getBancosXPais(user.PaisID, 1);
        ddl_bancos_panel.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        ddl_bancos_panel.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            ddl_bancos_panel.Items.Add(item);
        }
        //item = new ListItem("Seleccione...", "0");
        //lb_cuentas_bancarias.Items.Add(item);
        mpeSeleccion.Show();
    }
    protected void ddl_bancos_panel_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user;

        DataTable dt = new DataTable();
        dt.Columns.Add("ID_banco");
        dt.Columns.Add("Cuenta");
        dt.Columns.Add("Banco");
        user = (UsuarioBean)Session["usuario"];
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(ddl_bancos_panel.SelectedValue), user.PaisID, 0);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            //item = new ListItem(rgb.strC1, rgb.strC1);
            //lb_cuentas_bancarias.Items.Add(item);
            object[] objArr = { rgb.intC1, rgb.strC1, ddl_bancos_panel.SelectedItem.ToString() };
            dt.Rows.Add(objArr);
        }
        gv_cuentas.DataSource = dt;
        gv_cuentas.DataBind();
        mpeSeleccion.Show();
    }
    protected void bt_agregar_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID_banco");
        dt.Columns.Add("Cuenta");
        dt.Columns.Add("Banco");
        GridViewRow row = null;
        RE_GenericBean rgb = null;
        CheckBox chk;
        //*Recorro el grid de cuentas       
        for (int i = 0; i < gv_cuentas.Rows.Count; i++)
        {
            row = gv_cuentas.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("CheckBox1");
                if (chk.Checked)
                {
                    rgb = new RE_GenericBean();
                    rgb.intC1 = int.Parse(row.Cells[1].Text);//id banco
                    rgb.strC1 = row.Cells[2].Text.ToString();//cuenta
                    rgb.strC2 = row.Cells[3].Text.ToString();//banco
                    object[] objArr = { rgb.intC1, rgb.strC1, ddl_bancos_panel.SelectedItem.ToString() };
                    dt.Rows.Add(objArr);
                }
            }
        }
        for (int i = 0; i < gv_bancos.Rows.Count; i++)
        {
            row = gv_bancos.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                rgb = new RE_GenericBean();
                rgb.intC1 = int.Parse(row.Cells[0].Text);//id banco
                rgb.strC1 = row.Cells[1].Text.ToString();//cuenta
                rgb.strC2 = row.Cells[2].Text.ToString();//banco
                object[] objArr = { rgb.intC1, rgb.strC1, rgb.strC2 };
                dt.Rows.Add(objArr);
            }
        }
        gv_bancos.DataSource = dt;
        gv_bancos.DataBind();
        gv_bancos.Visible = true;
        Label2.Visible = true;


        int CantRows = gv_bancos.Rows[0].Cells.Count - 3;
        gv_bancos.HeaderRow.Cells[CantRows].Visible = false;
        for (int i = 0; i < gv_bancos.Rows.Count; i++)
        {
            gv_bancos.Rows[i].Cells[CantRows].Visible = false;
        }
        mpeSeleccion.Show();
    }
}