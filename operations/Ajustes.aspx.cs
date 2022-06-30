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
using System.Data.OleDb;

public partial class operations_Ajustes : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null, dt1 = null;
    DataTable dt_excel = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 131072) == 131072))
            Response.Redirect("index.aspx");
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
            Obtengo_Listas();
            lb_bancos_SelectedIndexChanged(sender, e);
            lbl_tipo_cambio.Text = user.pais.TipoCambio.ToString();
        }
    }
    protected void Obtengo_Listas()
    {
        ArrayList arr = null;
        ListItem item = null;

        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
        lb_moneda.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_moneda.Items.Add(item);
        }
        
        int moneda_inicial = 0;
        moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 15);
        lb_moneda.SelectedValue = moneda_inicial.ToString();
        lb_moneda.Enabled = false;

        arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(15, user, 1);
        item = new ListItem("Seleccione...", "0");
        lb_serie.Items.Clear();
        lb_serie.Items.Add(item);
        foreach (RE_GenericBean Bean_Serie in arr)
        {
            item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
            lb_serie.Items.Add(item);
        }


        int AjusteContable = 0; //2021-02-02

        ArrayList Arr_Perfile = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        foreach (PerfilesBean Perfiles in Arr_Perfile) //Arr_Perfiles_Creditos)
        {
            //ajustes contables       spuer user          admin
            if (Perfiles.ID == 42 || Perfiles.ID == 7 || Perfiles.ID == 8)
            {
                AjusteContable = 1;
            }
        }    

        arr = (ArrayList)DB.getAjustesTipo();
        lb_tipo_ajuste.Items.Add("Seleccione...");
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC2, rgb.strC1);
            if ((rgb.strC1 == "1") || (rgb.strC1 == "8") || (rgb.strC1 == "10"))
            {
                lb_tipo_ajuste.Items.Add(item);
            }
            else if (rgb.strC1 == "11" || rgb.strC1 == "9") //9	Ajuste Contable Intercompany        11	Ajuste Contable Externo
            {

                if (AjusteContable == 1) //si el usuario tiene perfil para realizar ajustes contables intercompany / externo
                {
                    lb_tipo_ajuste.Items.Add(item);
                }

                /*
                if ((user.ID == "dennis-ariana") || (user.ID == "robin-sanchez") || (user.ID == "cesar-sanchez"))
                {
                    lb_tipo_ajuste.Items.Add(item);
                }
                if (user.ID == "jose-cruz")
                {
                    lb_tipo_ajuste.Items.Add(item);
                }
                //2021-01-18 email
                if (user.ID == "licel-velasquez" || user.ID == "mhernandezr" || user.ID == "rene-escobar" || user.ID == "noelia-gonzalez" || user.ID == "giovanni-fonseca" || user.ID == "contabilidad.hn7")
                {
                    lb_tipo_ajuste.Items.Add(item);
                }
                */
               
            }
        }

        //BAW FISCAL USD
        arr = (ArrayList)DB.getBancos_By_PaisMonedaID(user.PaisID, int.Parse(lb_moneda.SelectedValue), 0);
        lb_bancos.Items.Clear();
        item = new ListItem("Seleccione...", " ");
        lb_bancos.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_bancos.Items.Add(item);
        }
    }
    protected void lb_tipo_ajuste_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_tipo_ajuste.SelectedValue != "Seleccione...")
        {
            int tipo = int.Parse(lb_tipo_ajuste.SelectedValue);
            switch (tipo)
            {
                case 1:
                    Pnl_Entidades.Visible = false;
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Planilla.Visible = false;
                    Pnl_Partida.Visible = true;
                    Pnl_maritimo.Visible = false;
                    Pnl_Externo.Visible = false;
                    break;
                case 2:
                    Pnl_Entidades.Visible = true;
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Planilla.Visible = false;
                    Pnl_Partida.Visible = true;
                    Pnl_maritimo.Visible = true;
                    Pnl_Externo.Visible = false;
                    Setear_Persona();
                    break;
                case 3:
                    Pnl_Entidades.Visible = true;
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Planilla.Visible = false;
                    Pnl_Partida.Visible = true;
                    Pnl_maritimo.Visible = true;
                    Pnl_Externo.Visible = false;
                    Setear_Persona();
                    break;
                case 4:
                    Pnl_Entidades.Visible = true;
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Planilla.Visible = false;
                    Pnl_Partida.Visible = true;
                    Pnl_maritimo.Visible = true;
                    Pnl_Externo.Visible = false;
                    Setear_Persona();
                    break;
                case 5:
                    Pnl_Entidades.Visible = true;
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Planilla.Visible = false;
                    Pnl_Partida.Visible = true;
                    Pnl_maritimo.Visible = true;
                    Pnl_Externo.Visible = false;
                    Setear_Persona();
                    break;
                case 6:
                    Pnl_Entidades.Visible = true;
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Planilla.Visible = false;
                    Pnl_Partida.Visible = true;
                    Pnl_maritimo.Visible = true;
                    Pnl_Externo.Visible = false;
                    Setear_Persona();
                    break;
                case 7:
                    Pnl_Caja_Chica.Visible = true;
                    Pnl_Bancos.Visible = false;
                    Pnl_Entidades.Visible = false;
                    Pnl_Planilla.Visible = false;
                    Pnl_Partida.Visible = true;
                    Pnl_maritimo.Visible = true;
                    Pnl_Externo.Visible = false;
                    break;
                case 8:
                    Pnl_Bancos.Visible = true;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Entidades.Visible = false;
                    Pnl_Planilla.Visible = false;
                    Pnl_Partida.Visible = true;
                    Pnl_maritimo.Visible = false;
                    Pnl_Externo.Visible = false;
                    break;
                case 9:
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Entidades.Visible = true;
                    Pnl_Planilla.Visible = false;
                    Pnl_maritimo.Visible = false;
                    Pnl_Externo.Visible = false;
                    Pnl_Partida.Visible = true;
                    Setear_Persona();
                    break;
                case 10:
                    Pnl_Planilla.Visible = true;
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Entidades.Visible = false;
                    Pnl_Partida.Visible = false;
                    Pnl_maritimo.Visible = false;
                    Pnl_Externo.Visible = false;
                    break;
                case 11:
                    Pnl_Externo.Visible = true;
                    Pnl_Planilla.Visible = false;
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Entidades.Visible = false;
                    Pnl_Partida.Visible = false;
                    Pnl_maritimo.Visible = false;
                    break;
                default:
                    Pnl_Planilla.Visible = false;
                    Pnl_Bancos.Visible = false;
                    Pnl_Caja_Chica.Visible = false;
                    Pnl_Entidades.Visible = false;
                    Pnl_Partida.Visible = false;
                    Pnl_maritimo.Visible = false;
                    Pnl_Externo.Visible = false;
                    break;
            }
        }
        else
        {
            Pnl_Planilla.Visible = false;
            Pnl_Bancos.Visible = false;
            Pnl_Caja_Chica.Visible = false;
            Pnl_Entidades.Visible = false;
            Pnl_Partida.Visible = false;
            Pnl_maritimo.Visible = false;
        }
        gv_conf_cuentas.DataBind();
        lbl_total.Text = "0.00";
        lbl_cargo.Text = "0.00";
        lbl_abono.Text = "0.00";
        tb_entidad_codigo.Text = "";
        tb_entidad_nombre.Text = "";
    }
    protected void btn_cargar_planilla_Click(object sender, EventArgs e)
    {
        string No_Poliza = "";
        if (tb_no_poliza.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Ingresar el Numero de Poliza");
            return;
        }
        No_Poliza = tb_no_poliza.Text.Trim();
        int resultado = DB.Existe_Planilla(No_Poliza, user);
        if (resultado > 0)
        {
            WebMsgBox.Show("Planilla existente");
            tb_no_poliza.Text = "";
            return;
        }
        ArrayList arr = null;
        arr = (ArrayList)DB.getPlanilla(user, No_Poliza);
        if (arr.Count > 0)
        {
            lbl_planilla.Text = "1";
            gv_planilla.Enabled = false;
        }
        else
        {
            lbl_planilla.Text = "0";
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Cuenta ID");
        dt.Columns.Add("Cuenta Nombre");
        dt.Columns.Add("Cargo");
        dt.Columns.Add("Abono");
        foreach (RE_GenericBean rgb in arr)
        {
            object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4 };
            dt.Rows.Add(objArr);
        }
        gv_planilla.DataSource = dt;
        gv_planilla.DataBind();
        Calcular_Totales();
    }
    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        lb_banco_cuenta.Items.Clear();
        item = new ListItem("Seleccione...");
        item.Selected = true;
        lb_banco_cuenta.Items.Add(item);
        //if (lb_bancos.SelectedValue != "Seleccione...")
        if (lb_moneda.SelectedValue.Trim() == "")
        {
            WebMsgBox.Show("Por Favor seleccione la moneda a operar");
            return;
        }
        if (!lb_bancos.SelectedValue.Trim().Equals(""))
        {
            //UsuarioBean User_Temp = new UsuarioBean();
            //User_Temp.pais = user.pais;
            //User_Temp.PaisID = user.PaisID;
            //User_Temp.contaID = user.contaID;
            //User_Temp.Moneda = int.Parse(lb_moneda.SelectedValue);
            ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(lb_bancos.SelectedValue), user, 1);
            foreach (RE_GenericBean rgb in arrCtas)
            {
                item = new ListItem(rgb.strC1, rgb.strC1);
                lb_banco_cuenta.Items.Add(item);
            }
            //User_Temp = null;
        }
    }
    protected void Setear_Persona()
    {
        if (lb_tipo_ajuste.SelectedValue == "2")
        {
            lb_tipo_persona.SelectedValue = "3"; //cliente
        }
        else if (lb_tipo_ajuste.SelectedValue == "3")
        {
            lb_tipo_persona.SelectedValue = "4";
        }
        else if (lb_tipo_ajuste.SelectedValue == "4")
        {
            lb_tipo_persona.SelectedValue = "2";
        }
        else if (lb_tipo_ajuste.SelectedValue == "5")
        {
            lb_tipo_persona.SelectedValue = "5";
        }
        else if (lb_tipo_ajuste.SelectedValue == "6")
        {
            lb_tipo_persona.SelectedValue = "6";
        }
        else if (lb_tipo_ajuste.SelectedValue == "9")
        {
            lb_tipo_persona.SelectedValue = "10"; //intercompany
        }
    }

    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        if (lb_tipo_persona.SelectedValue.Equals("3"))
        { // cliente
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(a.nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and a.id_cliente=" + tb_codigo.Text; else where += "a.id_cliente=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and a.codigo_tributario='" + tb_nitb.Text + "'"; else where += "a.codigo_tributario='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getClientes(where, user, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
        }
        else if (lb_tipo_persona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (lb_tipo_persona.SelectedValue.Equals("2"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit a un agente");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(where, ""); //Agente
            dt = (DataTable)Utility.fillGridView("Agente", Arr);
        }
        else if (lb_tipo_persona.SelectedValue.Equals("5"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una naviera");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_naviera=" + tb_codigo.Text; else where += "id_naviera=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getNavieras(where, ""); //Naviera
            dt = (DataTable)Utility.fillGridView("Naviera", Arr);
        }
        else if (lb_tipo_persona.SelectedValue.Equals("6"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una línea aerea");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(name)) ilike '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and carrier_id=" + tb_codigo.Text; else where += " and carrier_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getCarriers(where); //Lineas aereas
            dt = (DataTable)Utility.fillGridView("LineasAereas", Arr);
        }
        else if (lb_tipo_persona.SelectedValue.Equals("10"))
        {

            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
            {
                where += " and id_intercompany=" + tb_codigo.Text.Trim();
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null)
                where += " and nombre_comercial ilike ('%" + tb_nombreb.Text.Trim() + "%')";

            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                where += " and nit='" + tb_nitb.Text.Trim() + "'";

            Arr = (ArrayList)DB.Get_Intercompanys(where);

            dt = (DataTable)Utility.fillGridView("Intercompany", Arr);

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
        if (lb_tipo_persona.SelectedValue.Equals("3"))//cliente
        {
            tb_entidad_codigo.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_entidad_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        if (lb_tipo_persona.SelectedValue.Equals("4"))//proveedor
        {
            tb_entidad_codigo.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_entidad_nombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        }
        else if (lb_tipo_persona.SelectedValue.Equals("2"))//agente
        {
            tb_entidad_codigo.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_entidad_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipo_persona.SelectedValue.Equals("5"))//naviera
        {
            tb_entidad_codigo.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_entidad_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipo_persona.SelectedValue.Equals("6"))//linea aerea
        {
            tb_entidad_codigo.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_entidad_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipo_persona.SelectedValue.Equals("10"))//intercompany
        {
            tb_entidad_codigo.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_entidad_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
    }
    protected void bt_buscar_cta_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        int clasificacion = int.Parse(lb_clasificacion.SelectedValue);
        if (clasificacion != 0) where += " and cue_clasificacion in (" + clasificacion + ")";
        if (!tb_nombre_cta.Text.Trim().Equals("") && tb_nombre_cta.Text != null) where += " and UPPER(cue_nombre) like '%" + tb_nombre_cta.Text.Trim().ToUpper() + "%'";
        if (!tb_cuenta_numero.Text.Trim().Equals("") && tb_cuenta_numero.Text != null) where += " and(cue_id) ilike '%" + tb_cuenta_numero.Text.Trim() + "%'";
        Arr = Utility.getCuentasContables("XTipoClasificacion", where);
        dt = (DataTable)Utility.fillGridView("Cuenta", Arr);
        gv_cuenta.DataSource = dt;
        gv_cuenta.DataBind();
        ViewState["gv_cuenta_dt"] = dt;
        tb_cuenta_ModalPopupExtender.Show();
    }

    protected void bt_agregar_Click(object sender, EventArgs e)
    {
        if (tb_cuenta.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar una cuenta contable");
            return;
        }
        if (tb_monto.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar el monto a ajustar");
            return;
        }
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Cargo");
        dt_temp.Columns.Add("Abono");
        dt_temp.Columns.Add("Cuenta de");
        #region Bloqueo de Cuentas Contables Madres
        //bool resultado = DB.Validar_Bloqueo_Cuenta_Contable(user, tb_cuenta.Text.Trim());
        //if (resultado == true)
        //{
        //    WebMsgBox.Show("No se puede afectar una Cuenta Contable madre");
        //    return;
        //}
        #endregion
        #region Bloqueo de Cuentas de Concentracion

        int Bloqueo = 0; //2021-02-02

        ArrayList Arr_Perfile = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        foreach (PerfilesBean Perfiles in Arr_Perfile) //Arr_Perfiles_Creditos)
        {
            //Bloqueo cuentas       spuer user          admin
            if (Perfiles.ID == 43 || Perfiles.ID == 7 || Perfiles.ID == 8)
            {
                Bloqueo = 1;
            }
        }    

        //if ((user.ID != "dennis-ariana") && (user.ID != "jose-cruz"))
        if (Bloqueo == 0)
        {
            RE_GenericBean Cuenta_Bean = (RE_GenericBean)DB.getCtabyCtaID(tb_cuenta.Text.Trim());
            if ((Cuenta_Bean.intC2 >= 1) && (Cuenta_Bean.intC2 <= 2))//Nivel 1 al 3
            {
                tb_cuenta.Text = "";
                tb_cta_nombre.Text = "";
                tb_monto.Text = "";
                WebMsgBox.Show("No se puede Ajustar una cuenta contable de concentracion");
                return;
            }
        }
        #endregion

        #region Bloqueo de Cuentas especificas
        //if ((user.ID != "dennis-ariana") && (user.ID != "jose-cruz"))
        if (Bloqueo == 0)
        {
            if ((tb_cuenta.Text == "2.1.1.1.0001") || (tb_cuenta.Text == "1.1.2.2.0001"))
            {
                tb_cuenta.Text = "";
                tb_cta_nombre.Text = "";
                tb_monto.Text = "";
                WebMsgBox.Show("No puede Ajustar esta cuenta contable");
                return;
            }
        }
        #endregion
        GridViewRowCollection gvr = gv_conf_cuentas.Rows; 
        dt_temp.Clear();
        foreach (GridViewRow row in gvr)
        {
            object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text };
            dt_temp.Rows.Add(objArr);
        }
        if (lb_ctade.SelectedValue.Equals("Cargo"))
        {
            object[] objArr = { tb_cuenta.Text.Trim(), tb_cta_nombre.Text.Trim(), tb_monto.Text.Trim(), "0.00", "Cargo" };
            dt_temp.Rows.Add(objArr);
        }
        else if (lb_ctade.SelectedValue.Equals("Abono"))
        {
            object[] objArr = { tb_cuenta.Text.Trim(), tb_cta_nombre.Text.Trim(), "0.00", tb_monto.Text.Trim(), "Abono" };
            dt_temp.Rows.Add(objArr);
        }
        gv_conf_cuentas.DataSource = dt_temp;
        gv_conf_cuentas.DataBind();
        tb_cuenta.Text = "";
        tb_cta_nombre.Text = "";
        tb_monto.Text = "";
        Calcular_Totales();
    }
    protected void gv_cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_cuenta.DataSource = dt;
        gv_cuenta.PageIndex = e.NewPageIndex;
        gv_cuenta.DataBind();
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void gv_cuenta_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ArrayList cuentaArr = null;
        }
        else
        {
            dt = (DataTable)ViewState["gv_cuenta_dt"];
        }
    }
    protected void gv_cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_cuenta.SelectedRow;
        tb_cuenta.Text = row.Cells[1].Text;
        tb_cta_nombre.Text = row.Cells[2].Text;
    }

    protected void gv_conf_cuentas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Cargo");
        dt_temp.Columns.Add("Abono");
        dt_temp.Columns.Add("Cuenta de");
        GridViewRowCollection gvr = gv_conf_cuentas.Rows;
        dt_temp.Clear();
        foreach (GridViewRow row in gvr)
        {
            object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text };
            dt_temp.Rows.Add(objArr);
        }
        dt_temp.Rows[e.RowIndex].Delete();
        gv_conf_cuentas.DataSource = dt_temp;
        gv_conf_cuentas.DataBind();
        Calcular_Totales();
    }
    protected void Calcular_Totales()
    {
        decimal Total_Cargo = 0;
        decimal Total_Abono = 0;
        decimal Total = 0;
        GridViewRowCollection gvr = gv_conf_cuentas.Rows;
        foreach (GridViewRow row in gvr)
        {
            
            Total_Cargo += decimal.Parse(row.Cells[3].Text);
            Total_Abono += decimal.Parse(row.Cells[4].Text);
        }
        Total = Total_Cargo - Total_Abono;
        lbl_cargo.Text = Total_Cargo.ToString("#,#.00#;(#,#.00#)");
        lbl_abono.Text = Total_Abono.ToString("#,#.00#;(#,#.00#)");
        lbl_total.Text = Total.ToString("#,#.00#;(#,#.00#)");
        decimal Total_Cargo2 = 0;
        decimal Total_Abono2 = 0;
        decimal Total2 = 0;
        GridViewRowCollection gvr_planilla = gv_planilla.Rows;
        foreach (GridViewRow row in gvr_planilla)
        {

            Total_Cargo2 += decimal.Parse(row.Cells[2].Text);
            Total_Abono2 += decimal.Parse(row.Cells[3].Text);
        }
        Total2 = Total_Cargo2 - Total_Abono2;
        lbl_cargo2.Text = Total_Cargo2.ToString("#,#.00#;(#,#.00#)");
        lbl_abono2.Text = Total_Abono2.ToString("#,#.00#;(#,#.00#)");
        lbl_total2.Text = Total2.ToString("#,#.00#;(#,#.00#)");
        decimal Total_Cargo3 = 0;
        decimal Total_Abono3 = 0;
        decimal Total3 = 0;
        GridViewRowCollection gvr_partida = gv_partida.Rows;
        foreach (GridViewRow row in gvr_partida)
        {

            Total_Cargo3 += decimal.Parse(row.Cells[2].Text);
            Total_Abono3 += decimal.Parse(row.Cells[3].Text);
        }
        Total3 = Total_Cargo3 - Total_Abono3;
        lbl_cargo3.Text = Total_Cargo3.ToString("#,#.00#;(#,#.00#)");
        lbl_abono3.Text = Total_Abono3.ToString("#,#.00#;(#,#.00#)");
        lbl_total3.Text = Total3.ToString("#,#.00#;(#,#.00#)");
    }
    protected void Btn_guardar_Click(object sender, EventArgs e)
    {
        if (lb_serie.Items.Count == 0)
        {
            WebMsgBox.Show("No se puede generar un Ajuste sin Serie");
            return;
        }
        if (lb_serie.SelectedValue == "0")
        {
            WebMsgBox.Show("Por favor seleccione la Serie a utilizar");
            return;
        }
        if (tb_fecha.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la fecha a Ajustar");
            return;
        }

        //*************************************************************BLOQUEO DE PERIODO*****************************************
        #region bloqueo periodo
        RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
        DateTime hoy = DateTime.Today; //fecha del dia de hoy
        int activo = bloqueo.intC3;
        #region formateo fechas
        DateTime fecha_doc = DateTime.Parse(DB.DateFormat(tb_fecha.Text.ToString().Substring(0, 10)));
        #endregion
        if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
        {
            WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0, 10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10));
            return;
        }
        #endregion
        if (lb_tipo_ajuste.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar el tipo de Ajuste a realizar");
            return;
        }
        if (tb_observaciones.Text.Equals(""))
        {
            WebMsgBox.Show("Es necesario que ingrese alguna observación para completar esta operación");
            return;
        }
        #region Validaciones por Tipo de Ajuste
        int Tipo = int.Parse(lb_tipo_ajuste.SelectedValue);
        switch (Tipo)
        {
            case 2: //Ajuste Contable a Clientes
                if (tb_entidad_nombre.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe seleccionar algun Cliente");
                    return;
                }
                if (tb_mbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar MBL");
                    return;
                }
                if (tb_hbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar HBL");
                    return;
                }
                break;
            case 3  :
                if (tb_entidad_nombre.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe seleccionar algun Proveedor");
                    return;
                }
                if (tb_mbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar MBL");
                    return;
                }
                if (tb_hbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar HBL");
                    return;
                }
                break;
            case 4:
                if (tb_entidad_nombre.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe seleccionar algun Agente");
                    return;
                }
                if (tb_mbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar MBL");
                    return;
                }
                if (tb_hbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar HBL");
                    return;
                }
                break;
            case 5:
                if (tb_entidad_nombre.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe seleccionar alguna Naviera");
                    return;
                }
                if (tb_mbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar MBL");
                    return;
                }
                if (tb_hbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar HBL");
                    return;
                }
                break;
            case 6:
                if (tb_entidad_nombre.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe seleccionar alguna Linea Aerea");
                    return;
                }
                if (tb_mbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar MBL");
                    return;
                }
                if (tb_hbl.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe Ingresar HBL");
                    return;
                }
                break;
            case 7:
                if (tb_caja_chica_nombre.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe seleccionar algun usuario de Caja Chica");
                    return;
                }
                break;
            case 8:
                if (lb_bancos.SelectedValue == "Seleccione...")
                {
                    WebMsgBox.Show("Debe seleccionar el Banco a Ajustar");
                    return;
                }
                if (lb_banco_cuenta.SelectedValue == "Seleccione...")
                {
                    WebMsgBox.Show("Debe seleccionar la cuenta a Ajustar");
                    return;
                }
                break;
            case 9: //Ajuste Contable a Intercompany 
                if (tb_entidad_nombre.Text.Trim() == "")
                {
                    WebMsgBox.Show("Debe seleccionar algun Intercompany");
                    return;
                }
                break;
        }
        #endregion
        #region Validar que Exista al menos una cuenta de Cargo y una de Abono
        GridViewRowCollection gvr = gv_conf_cuentas.Rows;
        int ban_cts_cargo = 0;
        int ban_cts_abono = 0;
        foreach (GridViewRow roww in gvr)
        {
            if (roww.Cells[5].Text == "Cargo")
            {
                ban_cts_cargo++;
            }
            else if (roww.Cells[5].Text == "Abono")
            {
                ban_cts_abono++;
            }
        }
        if (ban_cts_cargo == 0)
        {
            WebMsgBox.Show("Debe ingresar al menos un Cuenta Contable de Cargo");
            return;
        }
        if (ban_cts_abono == 0)
        {
            WebMsgBox.Show("Debe ingresar al menos un Cuenta Contable de Abono");
            return;
        }
        #endregion
        decimal Total = 0;
        decimal Total_Cargo = 0;
        decimal Total_Abono = 0;
        Total_Cargo = decimal.Parse(lbl_cargo.Text);
        Total_Abono = decimal.Parse(lbl_abono.Text);
        Total = Total_Cargo - Total_Abono;
        if (Total != 0)
        {
            WebMsgBox.Show("La sumatoria de Cargo es diferente que la de Abono");
            return;
        }
        RE_GenericBean Ajuste_Bean = new RE_GenericBean();
        Tipo = int.Parse(lb_tipo_ajuste.SelectedValue);
        Ajuste_Bean.intC1 = Tipo;//Tipo de Ajuste
        Ajuste_Bean.strC1 = lb_serie.SelectedItem.Text;//Serie
        Ajuste_Bean.intC2 = int.Parse(lb_moneda.SelectedValue.ToString());//Moneda
        Ajuste_Bean.intC3 = user.contaID;//ContaID
        Ajuste_Bean.intC4 = user.PaisID;//Pais
        string Fecha= DB.DateFormat(tb_fecha.Text.Trim());
        Ajuste_Bean.strC2 = Fecha;//Fecha a Ajustar
        Ajuste_Bean.Fecha_Hora = lb_fecha_hora.Text;
        Ajuste_Bean.strC3 = tb_observaciones.Text.Trim();//Observaciones
        Ajuste_Bean.strC4 = user.ID;//Usuario ID
        Ajuste_Bean.decC1 = Total_Cargo;//Cargo
        Ajuste_Bean.decC2 = Total_Abono;//Abono
        if ((Tipo >= 2) && (Tipo <= 6) || Tipo == 9)
        {
            Ajuste_Bean.intC5 = int.Parse(lb_tipo_persona.SelectedValue.ToString());//Tipo Persona
            Ajuste_Bean.intC6 = int.Parse(tb_entidad_codigo.Text);//ID Entidad
            Ajuste_Bean.strC12 = tb_entidad_nombre.Text;
        }
        if (Tipo == 7)
        {
            Ajuste_Bean.intC5 = 8;//Tipo Persona
            Ajuste_Bean.intC6 = int.Parse(tb_codigo_caja_chica.Text);//ID Caja Chica
            Ajuste_Bean.strC12 = tb_entidad_nombre.Text;
        }
        if (Tipo == 8)
        {
            Ajuste_Bean.intC7 = int.Parse(lb_bancos.SelectedValue);//Banco ID
            Ajuste_Bean.strC5 = lb_banco_cuenta.SelectedItem.Text;//Cuenta Bancaria
        }
        Ajuste_Bean.strC6 = tb_hbl.Text.Trim();//HBL
        Ajuste_Bean.strC7 = tb_mbl.Text.Trim();//MBL
        Ajuste_Bean.strC8 = tb_routing.Text.Trim();//Routing
        Ajuste_Bean.strC9 = tb_contenedor.Text.Trim();//Contenedor
        Ajuste_Bean.strC11 = lbl_tipo_cambio.Text;
        if (Ajuste_Bean.strC11 == "0")
        {
            WebMsgBox.Show("No Existe Tipo de Cambio para la Fecha seleccionada.");
            return;
        }
        #region Cargar Partida
        GridViewRow row;
        RE_GenericBean Partida_Bean = null;
        for (int i = 0; i < gv_conf_cuentas.Rows.Count; i++)
        {
            row = gv_conf_cuentas.Rows[i];
            Partida_Bean = new RE_GenericBean();
            if (row.RowType == DataControlRowType.DataRow)
            {
                Partida_Bean.strC1 = row.Cells[1].Text;//Id de la cuenta
                Partida_Bean.strC2 = row.Cells[2].Text;//Nombre de la cuenta
                Partida_Bean.decC1 = decimal.Parse(row.Cells[3].Text);//Cargo
                Partida_Bean.decC2 = decimal.Parse(row.Cells[4].Text);//Abono
                if (Ajuste_Bean.arr1 == null) Ajuste_Bean.arr1 = new ArrayList();
                Ajuste_Bean.arr1.Add(Partida_Bean);
            }
        }
        #endregion

        string Check_Existencia = DB.CheckExistDoc(Ajuste_Bean.Fecha_Hora, 15);
        if (Check_Existencia == "0")
        {
            ArrayList result = DB.InsertAjusteContable(Ajuste_Bean, user, Ajuste_Bean.intC2, Ajuste_Bean.intC3);
            if (result != null && result.Count > 0)
            {
                WebMsgBox.Show("Se guardo exitosamente el Ajuste con el numero.: " + lb_serie.SelectedItem.Text + " - " + result[0].ToString());
                tb_correlativo.Text = result[0].ToString();
                gv_conf_cuentas.Enabled = false;
                Btn_guardar.Enabled = false;
                bt_agregar.Enabled = false;
                lb_factid.Text = result[1].ToString();
                btn_imprimir.Enabled = true;
                return;
            }
        }
        else
        {
            Btn_guardar.Enabled = false;
            return;
        }
    }
    protected void bt_buscar2_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo2.Text.Equals("") && tb_nombreb2.Text.Equals("") && tb_nitb2.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
        if (!tb_codigo2.Text.Trim().Equals("") && tb_codigo2.Text != null)
            if (!where.Equals("")) where += " and numero=" + tb_codigo2.Text; else where += "numero=" + tb_codigo2.Text;
        if (!tb_nitb2.Text.Trim().Equals("") && tb_nitb2.Text != null)
            if (!where.Equals("")) where += " and nit='" + tb_nitb2.Text + "'"; else where += "nit='" + tb_nitb2.Text + "'";
        Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
        dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        ViewState["proveedordt"] = dt;
        gv_proveedor2.DataSource = dt;
        gv_proveedor2.DataBind();
        mpeSeleccion2.Show();
    }
    protected void gv_proveedor2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["proveedordt"];
        gv_proveedor2.DataSource = dt1;
        gv_proveedor2.PageIndex = e.NewPageIndex;
        gv_proveedor2.DataBind();
        mpeSeleccion2.Show();
    }
    protected void gv_proveedor2_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_proveedor2.SelectedRow;
        tb_caja_chica_nombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        tb_codigo_caja_chica.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
    }
    protected void dgw1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw1.DataSource = dt1;
        dgw1.PageIndex = e.NewPageIndex;
        dgw1.DataBind();
        tb_hbl_ModalPopupExtender.Show();
    }
    protected void dgw1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tipo = lb_tipo.SelectedValue;
        if (e.CommandName == "Seleccionar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (tipo.Equals("FCL") || tipo.Equals("LCL"))
            {
                tb_hbl.Text = dgw1.Rows[index].Cells[2].Text;
                tb_mbl.Text = dgw1.Rows[index].Cells[3].Text;
                tb_routing.Text = dgw1.Rows[index].Cells[4].Text;
                tb_contenedor.Text = dgw1.Rows[index].Cells[5].Text;
            }
            else if (tipo.Equals("ALMACENADORA"))
            {
                tb_hbl.Text = dgw1.Rows[index].Cells[2].Text;
                tb_mbl.Text = dgw1.Rows[index].Cells[3].Text;
                tb_routing.Text = "";
                tb_contenedor.Text = "";
            }
            else if (tipo.Equals("AEREO"))
            {
                tb_hbl.Text = dgw1.Rows[index].Cells[2].Text;
                tb_mbl.Text = dgw1.Rows[index].Cells[3].Text;
                tb_routing.Text = "";
                tb_contenedor.Text = "";
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                tb_hbl.Text = dgw1.Rows[index].Cells[2].Text;
                tb_mbl.Text = dgw1.Rows[index].Cells[3].Text;
                tb_routing.Text = "";
                tb_contenedor.Text = "";
            }
        }
    }
    protected void bt_search_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;

        if (lb_tipo.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio.Text, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio.Text, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio.Text, user.pais.schema, codDistribucion, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio.Text, user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("TERRESTRE T"))
        {
            string paisISO = Utility.InttoISOPais(user.PaisID);
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio.Text, paisISO, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        tb_hbl_ModalPopupExtender.Show();
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
        }
    }
    protected void Btn_guarda_planilla_Click(object sender, EventArgs e)
    {
        if (lb_serie.Items.Count == 0)
        {
            WebMsgBox.Show("No se puede generar un Ajuste sin Serie");
            return;
        }
        if (lb_serie.SelectedValue == "0")
        {
            WebMsgBox.Show("Por favor seleccione la Serie a utilizar");
            return;
        }
        if (tb_fecha.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la fecha a Ajustar");
            return;
        }

        //*************************************************************BLOQUEO DE PERIODO*****************************************
        #region bloqueo periodo
        RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
        DateTime hoy = DateTime.Today; //fecha del dia de hoy
        int activo = bloqueo.intC3;
        #region formateo fechas
        DateTime fecha_doc = DateTime.Parse(DB.DateFormat(tb_fecha.Text.ToString().Substring(0, 10)));
        #endregion
        if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
        {
            WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0, 10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10));
            return;
        }
        #endregion
        #region Validacion Planillas en Moneda Local
        if ((user.PaisID == 2) || (user.PaisID == 4) || (user.PaisID == 6) || (user.PaisID == 9) || (user.PaisID == 24) || (user.PaisID == 25) || (user.PaisID == 26) || (user.PaisID == 12) || (user.PaisID == 23))
        {

        }
        else
        {
            if (lb_serie.Items.Count > 0)
            {
                if (lb_serie.SelectedValue != "0")
                {
                    int moneda_Serie = 0;
                    moneda_Serie = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie.SelectedValue));
                    if (moneda_Serie == 8)
                    {
                        WebMsgBox.Show("La Planilla solo puede ser cargada en Moneda Local");
                        return;
                    }
                }
            }
        }
        #endregion
        if (tb_fecha.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la fecha a Ajustar");
            return;
        }
        if (lb_tipo_ajuste.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar el tipo de Ajuste a realizar");
            return;
        }
        if (tb_observaciones.Text.Equals(""))
        {
            WebMsgBox.Show("Es necesario que ingrese alguna observación para completar esta operación");
            return;
        }
        #region Validar Planilla
        if (tb_no_poliza.Text.Trim() == "")
        {
            WebMsgBox.Show("Numero de Poliza invalido");
            return;
        }
        if (gv_planilla.Rows.Count == 0)
        {
            WebMsgBox.Show("Planilla Invalida");
            return;
        }
        #endregion
        decimal Total2 = 0;
        decimal Total_Cargo2 = 0;
        decimal Total_Abono2 = 0;
        Total_Cargo2 = decimal.Parse(lbl_cargo2.Text);
        Total_Abono2 = decimal.Parse(lbl_abono2.Text);
        Total2 = Total_Cargo2 - Total_Abono2;
        if (Total2 != 0)
        {
            WebMsgBox.Show("La sumatoria de Cargo es diferente que la de Abono");
            return;
        }
        RE_GenericBean Ajuste_Bean = new RE_GenericBean();
        int Tipo = int.Parse(lb_tipo_ajuste.SelectedValue);
        Tipo = int.Parse(lb_tipo_ajuste.SelectedValue);
        Ajuste_Bean.intC1 = Tipo;//Tipo de Ajuste
        Ajuste_Bean.strC1 = lb_serie.SelectedItem.Text;//Serie
        Ajuste_Bean.intC2 = int.Parse(lb_moneda.SelectedValue.ToString());//Moneda
        Ajuste_Bean.intC3 = user.contaID;//ContaID
        Ajuste_Bean.intC4 = user.PaisID;//Pais
        string Fecha = DB.DateFormat(tb_fecha.Text.Trim());
        Ajuste_Bean.strC2 = Fecha;//Fecha a Ajustar
        Ajuste_Bean.Fecha_Hora = lb_fecha_hora.Text;
        Ajuste_Bean.strC3 = tb_observaciones.Text.Trim();//Observaciones
        Ajuste_Bean.strC4 = user.ID;//Usuario ID
        Ajuste_Bean.decC1 = Total_Cargo2;//Cargo
        Ajuste_Bean.decC2 = Total_Abono2;//Abono
        Ajuste_Bean.strC10 = tb_no_poliza.Text.Trim().ToUpper();//No Poliza
        Ajuste_Bean.strC11 = lbl_tipo_cambio.Text;
        if (Ajuste_Bean.strC11 == "0")
        {
            WebMsgBox.Show("No Existe Tipo de Cambio para la Fecha seleccionada.");
            return;
        }
        #region Cargar Partida
        GridViewRow row;
        RE_GenericBean Partida_Bean = null;
        for (int i = 0; i < gv_planilla.Rows.Count; i++)
        {
            row = gv_planilla.Rows[i];
            Partida_Bean = new RE_GenericBean();
            if (row.RowType == DataControlRowType.DataRow)
            {
                Partida_Bean.strC1 = row.Cells[0].Text;//Id de la cuenta
                Partida_Bean.strC2 = row.Cells[1].Text;//Nombre de la cuenta
                Partida_Bean.decC1 = decimal.Parse(row.Cells[2].Text);//Cargo
                Partida_Bean.decC2 = decimal.Parse(row.Cells[3].Text);//Abono
                if (Ajuste_Bean.arr1 == null) Ajuste_Bean.arr1 = new ArrayList();
                Ajuste_Bean.arr1.Add(Partida_Bean);
            }
        }
        #endregion

        string Check_Existencia = DB.CheckExistDoc(Ajuste_Bean.Fecha_Hora, 15);
        if (Check_Existencia == "0")
        {
            ArrayList result = DB.InsertAjusteContable(Ajuste_Bean, user, Ajuste_Bean.intC2, Ajuste_Bean.intC3);
            if (result != null && result.Count > 0)
            {
                WebMsgBox.Show("Se guardo exitosamente la Planilla con el numero de Ajuste .: " + lb_serie.SelectedItem.Text + " - " + result[0].ToString());
                if (lbl_planilla.Text == "1")
                {
                    int res = DB.InsertarPlanilla(user, tb_no_poliza.Text.Trim());
                    if (res == 1)
                    {
                    }
                }
                tb_correlativo.Text = result[0].ToString();
                tb_no_poliza.Enabled = false;
                gv_planilla.Enabled = false;
                Btn_guarda_planilla.Enabled = false;
                btn_cargar_planilla.Enabled = false;
                lb_factid.Text = result[1].ToString();
                btn_imprimir_planilla.Enabled = true;
                return;
            }
        }
        else
        {
            Btn_guarda_planilla.Enabled = false;
            return;
        }
    }
    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('../reports/viewAjusteContable.aspx?docID=" + lb_factid.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
        btn_imprimir.Enabled = false;
    }

    protected void gv_cuenta_Load1(object sender, EventArgs e)
    {

    }
    protected void btn_cargar_ajuste_Click(object sender, EventArgs e)
    {
        ImpersonationSettings settings = new ImpersonationSettings();
        UserImpersonation userImpersonation = new UserImpersonation(settings);
        try
        {
            userImpersonation.Impersonate();
            if (FileUpload1.HasFile)
            {
                string filename = FileUpload1.FileName;
                FileUpload1.SaveAs(Server.MapPath(filename));
                ExportToGrid(Server.MapPath(filename));
                Calcular_Totales();
            }
            else
            {
                gv_partida.DataBind();
                lbl_cargo3.Text = "0.00";
                lbl_abono3.Text = "0.00";
                lbl_total3.Text = "0.00";
            }
        }
        finally
        {
            userImpersonation.UndoImpersonation();
        }
    }
    protected void btn_guardar2_Click(object sender, EventArgs e)
    {
        int Tipo = 0;
        if (lb_serie.Items.Count == 0)
        {
            WebMsgBox.Show("No se puede generar un Ajuste sin Serie");
            return;
        }
        if (lb_serie.SelectedValue == "0")
        {
            WebMsgBox.Show("Por favor seleccione la Serie a utilizar");
            return;
        }
        if (tb_fecha.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la fecha a Ajustar");
            return;
        }
        if (lb_tipo_ajuste.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar el tipo de Ajuste a realizar");
            return;
        }
        if (tb_observaciones.Text.Equals(""))
        {
            WebMsgBox.Show("Es necesario que ingrese alguna observación para completar esta operación");
            return;
        }
        //*************************************************************BLOQUEO DE PERIODO*****************************************
        #region bloqueo periodo
        RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
        DateTime hoy = DateTime.Today; //fecha del dia de hoy
        int activo = bloqueo.intC3;
        #region formateo fechas
        DateTime fecha_doc = DateTime.Parse(DB.DateFormat(tb_fecha.Text.ToString().Substring(0, 10)));
        #endregion
        if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
        {
            WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0, 10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10));
            return;
        }
        #endregion
        decimal Total = 0;
        decimal Total_Cargo = 0;
        decimal Total_Abono = 0;
        Total_Cargo = decimal.Parse(lbl_cargo3.Text);
        Total_Abono = decimal.Parse(lbl_abono3.Text);
        Total = Total_Cargo - Total_Abono;
        if (Total != 0)
        {
            WebMsgBox.Show("La sumatoria de Cargo es diferente que la de Abono");
            return;
        }
        RE_GenericBean Ajuste_Bean = new RE_GenericBean();
        Tipo = int.Parse(lb_tipo_ajuste.SelectedValue);
        Ajuste_Bean.intC1 = Tipo;//Tipo de Ajuste
        Ajuste_Bean.strC1 = lb_serie.SelectedItem.Text;//Serie
        Ajuste_Bean.intC2 = int.Parse(lb_moneda.SelectedValue.ToString());//Moneda
        Ajuste_Bean.intC3 = user.contaID;//ContaID
        Ajuste_Bean.intC4 = user.PaisID;//Pais
        string Fecha = DB.DateFormat(tb_fecha.Text.Trim());
        Ajuste_Bean.strC2 = Fecha;//Fecha a Ajustar
        Ajuste_Bean.Fecha_Hora = lb_fecha_hora.Text;
        Ajuste_Bean.strC3 = tb_observaciones.Text.Trim();//Observaciones
        Ajuste_Bean.strC4 = user.ID;//Usuario ID
        Ajuste_Bean.decC1 = Total_Cargo;//Cargo
        Ajuste_Bean.decC2 = Total_Abono;//Abono
        Ajuste_Bean.strC11 = lbl_tipo_cambio.Text;
        if (Ajuste_Bean.strC11 == "0")
        {
            WebMsgBox.Show("No Existe Tipo de Cambio para la Fecha seleccionada.");
            return;
        }
        #region Cargar Partida
        GridViewRow row;
        RE_GenericBean Partida_Bean = null;
        for (int i = 0; i < gv_partida.Rows.Count; i++)
        {
            row = gv_partida.Rows[i];
            Partida_Bean = new RE_GenericBean();
            if (row.RowType == DataControlRowType.DataRow)
            {
                Partida_Bean.strC1 = row.Cells[0].Text;//Id de la cuenta
                Partida_Bean.strC2 = row.Cells[1].Text;//Nombre de la cuenta
                Partida_Bean.decC1 = decimal.Parse(row.Cells[2].Text);//Cargo
                Partida_Bean.decC2 = decimal.Parse(row.Cells[3].Text);//Abono
                if (Ajuste_Bean.arr1 == null) Ajuste_Bean.arr1 = new ArrayList();
                Ajuste_Bean.arr1.Add(Partida_Bean);
            }
        }
        #endregion

        string Check_Existencia = DB.CheckExistDoc(Ajuste_Bean.Fecha_Hora, 15);
        if (Check_Existencia == "0")
        {
            ArrayList result = DB.InsertAjusteContable(Ajuste_Bean, user, Ajuste_Bean.intC2, Ajuste_Bean.intC3);
            if (result != null && result.Count > 0)
            {
                WebMsgBox.Show("Se guardo exitosamente el Ajuste con el numero.: " + lb_serie.SelectedItem.Text + " - " + result[0].ToString());
                tb_correlativo.Text = result[0].ToString();
                gv_partida.Enabled = false;
                btn_guardar2.Enabled = false;
                lb_factid.Text = result[1].ToString();
                btn_imprimir2.Enabled = true;
                lb_tipo_ajuste.Enabled = false;
                return;
            }
        }
        else
        {
            btn_guardar2.Enabled = false;
            return;
        }

    }
    void ExportToGrid(String path)
    {
        string Cue_ID = "";
        string Cargo = "";
        string Abono = "";
        string Aux = "";
        DataTable DT_Partida = new DataTable();
        DT_Partida.Columns.Add("Cuenta ID");
        DT_Partida.Columns.Add("Cuenta Nombre");
        DT_Partida.Columns.Add("Cargo");
        DT_Partida.Columns.Add("Adono");
        OleDbConnection MyConnection = null;
        DataSet DtSet = null;
        OleDbDataAdapter MyCommand = null;
        //use below connection string if your excel file .xls 2003 format
        //MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" + path + "';Extended Properties=Excel 8.0;");
        //use below connection string if your excel file .xlsx 2007 format
        MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties=Excel 12.0;");
        MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Hoja1$]", MyConnection);
        DtSet = new System.Data.DataSet();
        MyCommand.Fill(DtSet, "[Hoja1$]");
        dt_excel = DtSet.Tables[0];
        MyConnection.Close();
        if (dt_excel.Rows.Count > 0)
        {
            gv_partida.DataSource = dt_excel;
            gv_partida.DataBind();
            GridViewRowCollection gvr = gv_partida.Rows;
            foreach (GridViewRow row in gvr)
            {
                Cue_ID = row.Cells[0].Text.Replace("#", ".");
                RE_GenericBean Cuenta_Bean = (RE_GenericBean)DB.getCtabyCtaID(Cue_ID);
                Aux = Server.HtmlDecode(row.Cells[1].Text);
                if (Aux.Trim().Equals(""))
                {
                    Cargo = "0.00";
                }
                else
                {
                    Cargo = row.Cells[1].Text;
                }
                Aux = Server.HtmlDecode(row.Cells[2].Text);
                if (Aux.Trim().Equals(""))
                {
                    Abono = "0.00";
                }
                else
                {
                    Abono = Server.HtmlDecode(row.Cells[2].Text);
                }
                if (Cuenta_Bean != null)
                {
                    object[] objArr = { Cue_ID, Cuenta_Bean.strC2, Math.Round(decimal.Parse(Cargo), 2).ToString(), Math.Round(decimal.Parse(Abono), 2).ToString() };
                    DT_Partida.Rows.Add(objArr);
                }
            }
            gv_partida.DataSource = DT_Partida;
            gv_partida.DataBind();
        }
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
    protected void tb_fecha_TextChanged(object sender, EventArgs e)
    {
        if (tb_fecha.Text.Trim() != "")
        {
            lbl_tipo_cambio.Text = DB.getTipoCambioXFecha(user.PaisID, DB.DateFormat(tb_fecha.Text.Trim())).ToString();
        }
        else
        {
            lbl_tipo_cambio.Text = lbl_tipo_cambio.Text;
        }
    }
    protected void lb_banco_cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!lb_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
        {
            string cuentaID = lb_banco_cuenta.SelectedValue;
            RE_GenericBean datoscuenta = (RE_GenericBean)DB.GetMonedaIDbyCuentaBancaria(cuentaID);
            //lb_moneda.SelectedValue = datoscuenta.intC1.ToString();
            if (lb_moneda.SelectedValue != datoscuenta.intC1.ToString())
            {
                WebMsgBox.Show("Debe seleccionar una Serie con la misma moneda de la Cuenta Bancaria");
                return;
            }
        }
    }
    protected void lb_serie_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie.Items.Count > 0)
        {
            if (lb_serie.SelectedValue != "0")
            {
                int moneda_Serie = 0;
                moneda_Serie = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie.SelectedValue));
                lb_moneda.SelectedValue = moneda_Serie.ToString();//Moneda de la Transaccion
                lb_moneda.Enabled = false;

                //BAW FISCAL USD
                ArrayList arr = null;
                ListItem item = null;
                arr = (ArrayList)DB.getBancos_By_PaisMonedaID(user.PaisID, int.Parse(lb_moneda.SelectedValue), 0);
                lb_bancos.Items.Clear();
                item = new ListItem("Seleccione...", " ");
                lb_bancos.Items.Add(item);
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_bancos.Items.Add(item);
                }
                lb_banco_cuenta.Items.Clear();
                item = new ListItem("Seleccione...");
                item.Selected = true;
                lb_banco_cuenta.Items.Add(item);

            }
        }
    }
}
