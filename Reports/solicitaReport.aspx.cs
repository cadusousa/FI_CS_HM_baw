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

public partial class Reports_solicitaReport : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null) {
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
        if (Request.QueryString.Count > 0)
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


            if (Request.QueryString["tipo"].ToString() == "2")
            {
                lb_formato.Visible = true;
                Label1.Visible = true;
            }
            else
            {
                lb_formato.Visible = false;
                Label1.Visible = false;
            }

        }
        if (!Page.IsPostBack) {
            obtengo_lista();
        }

        if (user.contaID == 1)
        {
            chk_consolidado.Visible = true;
            l_moneda_consolidado.Visible = true;
        }
        if (user.contaID == 2)
        {
            chk_consolidado.Visible = false;
            l_moneda_consolidado.Visible = false;
        }

    }

    protected void obtengo_lista() {
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

        string fecha_inicial = tb_fechainicial.Text.Trim();
        string fecha_final = tb_fechafinal.Text.Trim();
        string consmonfiscal = "";
        if (chk_consolidado.Checked == true)
        {
            consmonfiscal = "si";
        }
        if (chk_consolidado.Checked == false)
        {
            consmonfiscal = "no";
        }
        #region Formatear Fechas
        //Fecha Inicio
        int fe_dia = int.Parse(fecha_inicial.Substring(3, 2));
        int fe_mes = int.Parse(fecha_inicial.Substring(0, 2));
        int fe_anio = int.Parse(fecha_inicial.Substring(6, 4));
        fecha_inicial = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            fecha_inicial += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            fecha_inicial += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            fecha_inicial += "0" + fe_dia.ToString();
        }
        else
        {
            fecha_inicial += fe_dia.ToString();
        }
        
        //Fecha Fin
        fe_dia = int.Parse(fecha_final.Substring(3, 2));
        fe_mes = int.Parse(fecha_final.Substring(0, 2));
        fe_anio = int.Parse(fecha_final.Substring(6, 4));
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
        // calculando el total de dias entre fechas..................................
        //DateTime inicial = DateTime.Parse(fecha_inicial.ToString());
        //DateTime final = DateTime.Parse(fecha_final.ToString());

        DateTime inicial = DB.DThhmm(fecha_inicial.ToString());
        DateTime final = DB.DThhmm(fecha_final.ToString());

        

        TimeSpan dd = final.Subtract(inicial);
        double dif_en_dias = dd.TotalDays;
        //------------------------------------------------------------
      
        #endregion
        //if ((dif_en_dias > 31) && ((lb_reporte.SelectedValue.ToString() == "1") || (lb_reporte.SelectedValue.ToString() == "3")))
        //{
        //    WebMsgBox.Show("No puede generar libros mayores a 31 dias");
        //    return;
        //}
        if (tb_fechainicial.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha Inicial");
            return;
        }
        if (tb_fechafinal.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha Final");
            return;
        }
        if (lb_reporte.SelectedValue.ToString() == "3")
        {
            if (tb_mbl.Text.Trim().Equals(""))
            {
                WebMsgBox.Show("Debe ingresar el numero del documento");
                return;
            }
        }

        #region Registrar Log de Reportes
        string Tipo_Reporte = "";
        if (lb_reporte.SelectedValue.Equals("1"))
        {
            Tipo_Reporte = "LIBRO DIARIO";
        }
        else if (lb_reporte.SelectedValue.Equals("2"))
        {
            Tipo_Reporte = "LIBRO MAYOR";
        }
        else if (lb_reporte.SelectedValue.Equals("3"))
        {
            Tipo_Reporte = "PROFIT";
        }
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = Tipo_Reporte;
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " Contabilidad.: " + user.contaID + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fechainicial.Text + " Fecha Final.: " + tb_fechafinal.Text + " ,";
        mensaje_log += "Consolidar Moneda.: " + consmonfiscal + " ";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        string mensaje = "<script languaje=\"JavaScript\">";
        if (lb_reporte.SelectedValue.Equals("1")) {//libro diario
            mensaje += "window.open('reports.aspx?reptype=" + lb_reporte.SelectedValue + "&fechaini=" + fecha_inicial + "&fechafin=" + fecha_final + "&monID=" + lb_moneda.SelectedValue + "&consmonfiscal=" + consmonfiscal + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";    
        } else if (lb_reporte.SelectedValue.Equals("2")) { // libro mayor
            mensaje += "window.open('reports.aspx?reptype=" + lb_reporte.SelectedValue + "&fechaini=" + fecha_inicial + "&fechafin=" + fecha_final + "&monID=" + lb_moneda.SelectedValue + "&consmonfiscal=" + consmonfiscal + "&formato=" + lb_formato.SelectedValue + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        } else if (lb_reporte.SelectedValue.Equals("3")) { // Profit
            mensaje += "window.open('reports.aspx?reptype=" + lb_reporte.SelectedValue + "&fechaini=" + fecha_inicial + "&fechafin=" + fecha_final + "&monID=" + lb_moneda.SelectedValue + "&MBL=" + tb_mbl.Text.Trim() + "&consmonfiscal=" + consmonfiscal + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        }
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
    protected void lb_reporte_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_reporte.SelectedValue.ToString() == "3")
        {
            tb_mbl.Visible = true;
            lb_mbl.Visible = true;
        }
        else
        {
            tb_mbl.Visible = false;
            lb_mbl.Visible = false;
        }
    }
    protected void dgw12_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
                DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw12.DataSource = dt1;
        dgw12.PageIndex = e.NewPageIndex;
        dgw12.DataBind();
        tb_mbl_ModalPopupExtender.Show();
    }
    protected void dgw12_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tipo = lb_tipo2.SelectedValue;
        if (e.CommandName == "Seleccionar2")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (tipo.Equals("FCL") || tipo.Equals("LCL"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("ALMACENADORA"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[2].Text;
            }
            else if (tipo.Equals("AEREO"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
        }
    }
    protected void bt_search2_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;

        if (lb_tipo2.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio2.Text.Trim(), user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio2.Text.Trim(), user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio2.Text.Trim(), user.pais.schema, codDistribucion, user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio2.Text.Trim(), user.pais.schema);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("TERRESTRE T"))
        {
            string paisISO = Utility.InttoISOPais(user.PaisID);
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio2.Text.Trim(), paisISO, user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        tb_mbl_ModalPopupExtender.Show();
    }
    //protected void tb_fechafinal_TextChanged(object sender, EventArgs e)
    //{
    //    string date = tb_fechainicial.Text.ToString();
    //    string date1 = tb_fechafinal.Text.ToString();
    //    DateTime dt = Convert.ToDateTime(date);
    //    DateTime dt1 = Convert.ToDateTime(date1); 
    //    DateTime inicial = new DateTime(dt.Year, dt.Month, dt.Day);
    //    DateTime final = new DateTime(dt1.Year, dt1.Month, dt1.Day);

    //    // Difference in days, hours, and minutes.
    //    TimeSpan ts = final - inicial;
    //    // Difference in days.
    //    int differenceInDays = ts.Days;
    //}


}
