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
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null) {
            Response.Redirect("../default.aspx");
        }
        #region Definir Fechas
        if (!IsPostBack)
        {
            DateTime Fecha = DateTime.Now;
            tb_fechainicial.Text = "01/01/" + Fecha.Year.ToString();
            tb_fechafinal.Text = Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString();
        }
        #endregion
        user = (UsuarioBean)Session["usuario"];
        
        if (!Page.IsPostBack) {
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
            if (bandera == 2)
            {
                item = new ListItem("CONSOLIDADO", "3");
                lb_contabilidad.Items.Add(item);
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
        l_contabilidad.Visible = false;
        lb_contabilidad.Visible = false;
        tb_hbl.Visible = false;
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

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "PROFIT DETALLADO";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " Contabilidad.: " + lb_contabilidad.SelectedItem.Text + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fechainicial.Text + " Fecha Final.: " + tb_fechafinal.Text + " ,";
        mensaje_log += "Master.: " + tb_mbl.Text + "Tipo.: " + lb_tipopersona.SelectedItem.Text + " ,";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion
        
        string mensaje = "<script languaje=\"JavaScript\">";
        //mensaje += "window.open('reports.aspx?reptype=3&FechaIni=" + tb_fechainicial.Text + "&FechaFin=" + tb_fechafinal.Text + "&MBL=" + tb_mbl.Text.Trim() + "&HBL=" + tb_hbl.Text.Trim() + "&CONTENEDOR=" + tb_contenedor.Text.Trim() + "&ROUTING=" + tb_routing.Text.Trim() + "&tipopersona=" + lb_tipopersona.SelectedValue + "&codigopersona=" + tb_agenteID.Text.Trim() + "&monID=" + lb_moneda.SelectedValue + "&tipo_conta=" + lb_contabilidad.SelectedValue + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "window.open('reports.aspx?reptype=3&FechaIni=" + tb_fechainicial.Text + "&FechaFin=" + tb_fechafinal.Text + "&MBL=" + tb_mbl.Text.Trim() + "&HBL=" + tb_hbl.Text.Trim() + "&CONTENEDOR=" + tb_contenedor.Text.Trim() + "&ROUTING=" + tb_routing.Text.Trim() + "&tipopersona=" + lb_tipopersona.SelectedValue + "&codigopersona=" + tb_agenteID.Text.Trim() + "&monID=" + lb_moneda.SelectedValue + "&tipo_conta=3','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }

    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        if (lb_tipopersona.SelectedValue.Equals("3"))
        { // cliente
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(a.nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and a.id_cliente=" + tb_codigo.Text; else where += "a.id_cliente=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and a.codigo_tributario='" + tb_nitb.Text + "'"; else where += "a.codigo_tributario='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getClientes(where, user, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit a un agente");
                return;
            }

            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += " agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(where, ""); //Agente
            dt = (DataTable)Utility.fillGridView("Agente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una naviera");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_naviera=" + tb_codigo.Text; else where += " id_naviera=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getNavieras(where ,""); //Naviera
            dt = (DataTable)Utility.fillGridView("Naviera", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una línea aerea");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(name)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and carrier_id=" + tb_codigo.Text; else where += " and carrier_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getCarriers(where); //Lineas aereas
            dt = (DataTable)Utility.fillGridView("LineasAereas", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit usuario de caja chica");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo.Text; else where += "and id_usuario=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);
        }

        ViewState["proveedordt"] = dt;
        gv_proveedor.DataSource = dt;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }

    protected void gv_proveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["proveedordt"];
        gv_proveedor.DataSource = dt1;
        gv_proveedor.PageIndex = e.NewPageIndex;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
    protected void gv_proveedor_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_proveedor.SelectedRow;
        tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
        tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
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
            //string paisISO = Utility.InttoISOPais(user.PaisID);
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio2.Text.Trim(), user.pais.ISO, user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("ADUANA"))
        {
            DataSet ds = DB.getRO_Aduanas(user, tb_criterio2.Text.Trim());
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
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
            else if (tipo.Equals("PICKING"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("AEREO"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("ADUANA"))
            {
                tb_routing.Text = dgw12.Rows[index].Cells[4].Text;
            }
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




    protected void bt_search3_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;

        if (lb_tipo3.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio3.Text.Trim(), user);
            this.dgw13.DataSource = ds.Tables["bl_list"];
            this.dgw13.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo3.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio3.Text.Trim(), user);
            this.dgw13.DataSource = ds.Tables["bl_list"];
            this.dgw13.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo3.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio3.Text.Trim(), user.pais.schema, codDistribucion, user);
            this.dgw13.DataSource = ds.Tables["bl_list"];
            this.dgw13.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo3.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio3.Text.Trim(), user.pais.schema);
            this.dgw13.DataSource = ds.Tables["bl_list"];
            this.dgw13.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo3.SelectedValue.Equals("TERRESTRE T"))
        {
            string paisISO = Utility.InttoISOPais(user.PaisID);
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio3.Text.Trim(), paisISO, user);
            this.dgw13.DataSource = ds.Tables["bl_list"];
            this.dgw13.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("ADUANA"))
        {
            DataSet ds = DB.getRO_Aduanas(user, tb_criterio2.Text.Trim());
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        tb_cuenta2_ModalPopupExtender.Show();
    }
    protected void dgw13_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tipo = lb_tipo3.SelectedValue;
        if (e.CommandName == "Seleccionar3")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (tipo.Equals("FCL") || tipo.Equals("LCL"))
            {
                tb_hbl.Text = dgw13.Rows[index].Cells[2].Text;
                tb_mbl.Text = dgw13.Rows[index].Cells[3].Text;
                tb_routing.Text = dgw13.Rows[index].Cells[4].Text;
                tb_contenedor.Text = dgw13.Rows[index].Cells[5].Text;
            }
            else if (tipo.Equals("PICKING"))
            {
                tb_hbl.Text = dgw13.Rows[index].Cells[2].Text;
                tb_mbl.Text = dgw13.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("AEREO"))
            {
                tb_hbl.Text = dgw13.Rows[index].Cells[2].Text;
                tb_mbl.Text = dgw13.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                tb_hbl.Text = dgw13.Rows[index].Cells[2].Text;
                tb_mbl.Text = dgw13.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("ADUANA"))
            {
                tb_routing.Text = dgw13.Rows[index].Cells[4].Text;
            }
        }
    }
    protected void dgw13_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw13.DataSource = dt1;
        dgw13.PageIndex = e.NewPageIndex;
        dgw13.DataBind();
        tb_cuenta2_ModalPopupExtender.Show();
    }

}
