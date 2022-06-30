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


public partial class operations_docsxpagar : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
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
        if (!((permiso & 1024) == 1024))
            Response.Redirect("index.aspx");

        if (!Page.IsPostBack) {
            lb_fecha_hora.Text = DB.getDateTimeNow();
            ArrayList arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }

            //int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            //if (tipo_contabilidad == 2)
            //{
            //    lb_moneda.SelectedValue = user.Moneda.ToString();
            //    lb_moneda.Enabled = false;
            //}
            //else
            //{
            //    lb_moneda.SelectedValue = user.Moneda.ToString();
            //    lb_moneda.Enabled = false;

            //}
            int moneda_inicial = 0;
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 4);
            if (tipo_contabilidad == 2)
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
            }
            else
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
            }
        }
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
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
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
                if (!where.Equals("")) where += " and id_naviera=" + tb_codigo.Text; else where += "id_naviera=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getNavieras(where, ""); //Naviera
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
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo.Text; else where += " and id_usuario=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(nombre_comercial)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_intercompany=" + tb_codigo.Text; else where += " and id_intercompany=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += " and nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.Get_Intercompanys(where); //INTERCOMPANY
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
        if (lb_tipopersona.SelectedValue.Equals("4")) //proveedor
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_contacto.Text = "";
            tb_contacto.Visible = false;
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[8].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[4].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("10")) //Intercompany
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            //tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_contacto.Text = "";
            tb_contacto.Visible = false;
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
        }
        //cargo_pendliquidar_y_cortes();
        gv_detalle.DataBind();
        gv_notadebito.DataBind();
        gv_notacredito.DataBind();
        gv_notacredito_nd.DataBind();
        gv_cortes.DataBind();
        gv_fact_intercompany.DataBind();
    }

    protected void cargo_pendliquidar_y_cortes() {

        
        int proveedorID = int.Parse(tb_agenteID.Text);
        int monID = int.Parse(lb_moneda.SelectedValue);
        //obtengo el listado de facturas pendientes
        ArrayList Arr = null;
        string where = " and tpr_proveedor_id=" + tb_agenteID.Text;
        where += " and tpr_ted_id=5 and tpr_tpi_id=" + lb_tipopersona.SelectedValue;
        where += " and tpr_tcon_id="+user.contaID;
        where += " and tpr_pai_id=" + user.PaisID + "";
        where += " and tpr_mon_id=" + monID + "";
        Arr = Utility.getFactProvision("XproveedorID", where, user);
        dt = (DataTable)Utility.fillGridView("FactPendProveedor", Arr);
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();

        Arr = DB.getNotasDebito(proveedorID, int.Parse(lb_tipopersona.SelectedValue), monID, user);
        dt = (DataTable)Utility.fillGridView("NDPendProveedor", Arr);
        gv_notadebito.DataSource = dt;
        gv_notadebito.DataBind();
        
        Arr = DB.getNotasCredito(proveedorID, int.Parse(lb_tipopersona.SelectedValue), monID, user);
        dt = (DataTable)Utility.fillGridView("NCPendProveedor", Arr);
        gv_notacredito.DataSource = dt;
        gv_notacredito.DataBind();

        Arr = DB.getNotasCredito(proveedorID, int.Parse(lb_tipopersona.SelectedValue), monID, user);
        dt = (DataTable)Utility.fillGridView("NCPendProveedor", Arr);
        gv_notacredito_nd.DataSource = dt;
        gv_notacredito_nd.DataBind();

        //obtengo el listado de cortes
        Arr = null;
        Arr = DB.getCortesProveedor_All(proveedorID, int.Parse(lb_tipopersona.SelectedValue), monID, 5, user);
        dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
        gv_cortes.DataSource = dt;
        gv_cortes.DataBind();
        //obtengo las facturas de intercompany
        if (lb_tipopersona.SelectedValue == "10")
        {
            lb_titulo_fac.Visible = true;
            gv_fact_intercompany.Visible = true;
            Arr = null;
            Arr = DB.getFacturas_Intercompany(proveedorID, int.Parse(lb_tipopersona.SelectedValue), monID, user);
            dt = (DataTable)Utility.fillGridView("FAIntercompany", Arr);
            gv_fact_intercompany.DataSource = dt;
            gv_fact_intercompany.DataBind();
        }
    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
            ArrayList Arr = null;
            string where = "";
        }
    }


    protected void bt_generar_Click(object sender, EventArgs e)
    {
        decimal totalprov = 0, totalproveq=0;
        decimal totalnc = 0, totalnceq=0;
        decimal totalnd = 0, totalndeq=0;
        decimal totalnc_nd = 0, totalnc_ndeq = 0; //total nc a nd estas suman en vez de restar
        decimal totalfa = 0, totalfaeq = 0;
        decimal total = 0;
        decimal totaleq = 0;
        #region Validaciones
        if ((tb_agenteID.Text.Trim().Equals("")) || (tb_agenteID.Text.Trim().Equals("0"))||(tb_agentenombre.Text.Trim().Equals("")))
        {
            WebMsgBox.Show("Debe indicar el nombre del Proveedor");
            return;
        }
        //GV Ctas por Pagar
        int ban_cts_xpagar = 0;
        decimal total_suma = 0;
        decimal total_resta = 0;
        CheckBox chk_cta_xpagar;
        if (gv_detalle.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_detalle.Rows)
            {
                chk_cta_xpagar = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_cta_xpagar.Checked)
                {
                    ban_cts_xpagar++;
                    totalprov += decimal.Parse(grd_Row.Cells[10].Text);
                }
            }
        }
        //GV Notas de Credito
        int ban_nts_credito = 0;
        CheckBox chk_nts_credito;
        if (gv_notacredito.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_notacredito.Rows)
            {
                chk_nts_credito = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_nts_credito.Checked)
                {
                    ban_nts_credito++;
                    totalnc += decimal.Parse(grd_Row.Cells[7].Text);
                }
            }
        }
        //GV Notas de Credito a ND
        int ban_nts_credito_nd = 0;
        CheckBox chk_nts_credito_nd;
        if (gv_notacredito.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_notacredito_nd.Rows)
            {
                chk_nts_credito_nd = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_nts_credito_nd.Checked)
                {
                    ban_nts_credito_nd++;
                    totalnc_nd += decimal.Parse(grd_Row.Cells[7].Text);
                }
            }
        }
        //GV Notas de Debito
        int ban_nts_debito = 0;
        CheckBox chk_nts_debito;
        if (gv_notadebito.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_notadebito.Rows)
            {
                chk_nts_debito = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_nts_debito.Checked)
                {
                    ban_nts_debito++;
                    totalnd += decimal.Parse(grd_Row.Cells[7].Text);
                }
            }
        }
        //GV FACTURAS
        int ban_ft = 0;
        CheckBox chk_ft_;
        if (gv_fact_intercompany.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_fact_intercompany.Rows)
            {
                chk_ft_ = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_ft_.Checked)
                {
                    ban_ft++;
                    totalfa += decimal.Parse(grd_Row.Cells[9].Text);
                }
            }
        }

        total_suma = totalprov - totalnd - totalfa;
        total_resta = totalnc + totalnc_nd;
        if ((ban_cts_xpagar == 0) && (ban_nts_debito == 0) && (ban_ft == 0))
        {
            WebMsgBox.Show("Debe selecccionar al menos una cuenta por pagar, nota de debito o factura");
            return;
        }
        if (totalnc > totalprov)
        {
            WebMsgBox.Show("El valor total de Notas de Credito no debe ser mayor al valor Total de Provisiones");
            return;
        }
        if (totalnc_nd > totalnd)
        {
            WebMsgBox.Show("El valor total de Notas de Credito a Notas de Debito no debe ser mayor al valor Total de Notas de Debito");
            return;
        }


        //if (total_suma < total_resta)
        //{
        //    WebMsgBox.Show("No puede aplicar un credito mayor al debito");
        //    return;
        //}
        #endregion
        int monID = int.Parse(lb_moneda.SelectedValue);
        int provID = int.Parse(tb_agenteID.Text);
        int tipopersona = int.Parse(lb_tipopersona.SelectedValue);
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean cortebean = new RE_GenericBean();
        RE_GenericBean rgb = null;
        ArrayList arrctas = null;
        GridViewRow row=null;
        CheckBox chk;


        totalprov = 0;
        totalproveq = 0;
        totalnc = 0;
        totalnceq = 0;
        totalnd = 0;
        totalndeq = 0;
        totalnc_nd = 0;
        totalnc_ndeq = 0;
        totalfa = 0;
        totalfaeq = 0;
        total = 0;
        totaleq = 0;
        
        //*Recorro el grid de provisiones
        for (int i = 0; i < gv_detalle.Rows.Count; i++)
        {
            row = gv_detalle.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("CheckBox1");
                if (chk.Checked) {
                    rgb = new RE_GenericBean();
                    rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                    rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc
                    rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[11].Text);//Moneda
                    rgb.intC3 = provID;//proveedor                
                    rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                    rgb.intC5 = 5;//segun Sys_tipo_Referencia
                    rgb.strC2 = user.ID;//usuario
                    rgb.decC1 = decimal.Parse(row.Cells[10].Text);
                    rgb.strC3 = row.Cells[3].Text;
                    rgb.decC2 = decimal.Parse(row.Cells[13].Text);
                    if (arrctas == null) arrctas = new ArrayList();
                    arrctas.Add(rgb);
                    totalprov += rgb.decC1;
                    totalproveq += rgb.decC2;
                }
            }
        }
        row = null;
        //*Recorro el grid de Notas de credito
        for (int i = 0; i < gv_notacredito.Rows.Count; i++)
        {
            row = gv_notacredito.Rows[i];
            if (row.Cells[10].Text != "31")
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    chk = (CheckBox)row.FindControl("CheckBox1");
                    if (chk.Checked)
                    {
                        rgb = new RE_GenericBean();
                        rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                        rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc
                        rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[8].Text);//Moneda
                        rgb.intC3 = provID;//proveedor                
                        rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                        rgb.strC2 = user.ID;//usuario
                        rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                        rgb.strC3 = row.Cells[3].Text;
                        rgb.decC2 = decimal.Parse(row.Cells[9].Text);
                        rgb.intC6 = int.Parse(row.Cells[10].Text);
                        rgb.intC5 = 12; //segun Sys_tipo_referencia
                        if (arrctas == null) arrctas = new ArrayList();
                        arrctas.Add(rgb);
                        totalnc += rgb.decC1;
                        totalnceq += rgb.decC2;
                    }
                }
            }
        }
        //*Recorro el grid de Notas de credito A nd
        for (int i = 0; i < gv_notacredito_nd.Rows.Count; i++)
        {
            row = gv_notacredito_nd.Rows[i];
            if (row.Cells[10].Text != "12")
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    chk = (CheckBox)row.FindControl("CheckBox1");
                    if (chk.Checked)
                    {
                        rgb = new RE_GenericBean();
                        rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                        rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc
                        rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[8].Text);//Moneda
                        rgb.intC3 = provID;//proveedor                
                        rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                        rgb.strC2 = user.ID;//usuario
                        rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                        rgb.strC3 = row.Cells[3].Text;
                        rgb.decC2 = decimal.Parse(row.Cells[9].Text);
                        rgb.intC6 = int.Parse(row.Cells[10].Text);
                        rgb.intC5 = 31; //segun Sys_tipo_referencia
                        if (arrctas == null) arrctas = new ArrayList();
                        arrctas.Add(rgb);
                        totalnc_nd += rgb.decC1;
                        totalnc_ndeq += rgb.decC2;

                    }
                }
            }
        }
        row = null;
        //*Recorro el grid de Notas de debito
        for (int i = 0; i < gv_notadebito.Rows.Count; i++)
        {
            row = gv_notadebito.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("CheckBox1");
                if (chk.Checked)
                {
                    rgb = new RE_GenericBean();
                    rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                    rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc
                    rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[8].Text);//Moneda
                    rgb.intC3 = provID;//proveedor                
                    rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                    rgb.intC5 = 4;//segun Sys_tipo_Referencia
                    rgb.strC2 = user.ID;//usuario
                    rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                    rgb.strC3 = row.Cells[3].Text;
                    rgb.decC2 = decimal.Parse(row.Cells[9].Text);
                    if (arrctas == null) arrctas = new ArrayList();
                    arrctas.Add(rgb);
                    totalnd += rgb.decC1;
                    totalndeq += rgb.decC2;
                }
            }
        }
        //*Recorro el grid de Facturas Intercompany
        for (int i = 0; i < gv_fact_intercompany.Rows.Count; i++)
        {
            row = gv_fact_intercompany.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("CheckBox1");
                if (chk.Checked)
                {
                    rgb = new RE_GenericBean();
                    rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                    rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc
                    //rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[8].Text);//Moneda
                    rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[11].Text);//Moneda
                    rgb.intC3 = provID;//proveedor                
                    rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                    rgb.intC5 = 1;//segun Sys_tipo_Referencia
                    rgb.strC2 = user.ID;//usuario
                    //rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                    rgb.decC1 = decimal.Parse(row.Cells[9].Text);
                    rgb.strC3 = row.Cells[3].Text;
                    //rgb.decC2 = decimal.Parse(row.Cells[9].Text);
                    rgb.decC2 = decimal.Parse(row.Cells[10].Text);
                    if (arrctas == null) arrctas = new ArrayList();
                    arrctas.Add(rgb);
                    totalfa += rgb.decC1;
                    totalfaeq += rgb.decC2;
                }
            }
        }

        total = totalprov - totalnd - totalnc + totalnc_nd - totalfa;
        totaleq = totalproveq - totalndeq - totalnceq + totalnc_ndeq - totalfaeq;

        cortebean.intC1 = provID;
        cortebean.intC2 = monID;
        cortebean.intC3 = tipopersona;
        cortebean.decC1 = total;
        cortebean.decC2 = totaleq;
        cortebean.arr1 = arrctas;
        cortebean.strC1 = lb_fecha_hora.Text;
        string Check_Existencia = DB.CheckExistDoc(cortebean.strC1, 23);
        if (Check_Existencia == "0")
        {
            int result = DB.GenerateCorteProv(cortebean, user);
            if (result == -100 && result == 0)
            {
                WebMsgBox.Show("Existió un problema al momento de guardar la información, por favor intente de nuevo");
                return;
            }
            else
            {
                string serie = "";
                if (user.contaID == 1)
                {
                    if (monID == 8)
                    {
                        serie = "CFS-D";
                    }
                    else
                    {
                        serie = "CFS";
                    }
                }
                else
                {
                    serie = "CFN";
                }
                WebMsgBox.Show("El Corte fue grabado éxitosamente con Serie.: " + serie + " y Correlativo.: " + result.ToString());
                cargo_pendliquidar_y_cortes();
                gv_detalle.Enabled = false;
                gv_notacredito.Enabled = false;
                gv_notacredito_nd.Enabled = false;
                gv_notadebito.Enabled = false;
                gv_fact_intercompany.Enabled = false;
                bt_generar.Enabled = false;
                btn_buscar.Enabled = false;
                lb_tipopersona.Enabled = false;
                tb_agenteID.Enabled = false;
                lb_moneda.Enabled = false;
            }
        }
        else
        {
            gv_detalle.Enabled = false;
            gv_notacredito.Enabled = false;
            gv_notacredito_nd.Enabled = false;
            gv_notadebito.Enabled = false;
            gv_fact_intercompany.Enabled = false;
            bt_generar.Enabled = false;
            WebMsgBox.Show("El SOA ya fue generado.");
            return;
        }
    }

    
    protected void gv_cortes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int correlativo = 0, id=0;
        string serie = "";
        if (e.CommandName == "Detalle")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            id = int.Parse(gv_cortes.Rows[index].Cells[1].Text);
            correlativo = int.Parse(gv_cortes.Rows[index].Cells[3].Text);
            serie = gv_cortes.Rows[index].Cells[2].Text;

            #region Registrar Log de Reportes
            RE_GenericBean Bean_Log = new RE_GenericBean();
            Bean_Log.intC1 = user.PaisID;
            Bean_Log.strC1 = "REPORTE DE SOA";
            Bean_Log.strC2 = user.ID;
            Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
            string mensaje_log = "";
            mensaje_log += "Contabilidad.: " + user.contaID + " Moneda.: " + lb_moneda.SelectedValue + " ,";
            mensaje_log += "Serie.: " + serie + " Correlativo.: " + correlativo + " ,";
            Bean_Log.strC4 = mensaje_log;
            DB.Insertar_Log_Reportes(Bean_Log);
            #endregion

            if (lb_tipopersona.SelectedValue.Equals("8"))
            {
                string mensaje = "<script languaje=\"JavaScript\">";
                mensaje += "window.open('../reports/cajachica.aspx?id=" + id + "&serie=" + serie + "&correlativo=" + correlativo + "&user=" + tb_agentenombre.Text + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
                mensaje += "</script>";
                Page.RegisterClientScriptBlock("closewindow", mensaje);
            }
            else {
                string mensaje = "<script languaje=\"JavaScript\">";
                mensaje += "window.open('../reports/estadoctaagente.aspx?id=" + id + "&serie=" + serie + "&correlativo=" + correlativo + "&agente=" + tb_agentenombre.Text + "&cliente_id=" + tb_agenteID.Text.Trim() + "&tipopersona=" + lb_tipopersona.SelectedValue + "&id_cheque=0&ted=5&moneda=" + Utility.TraducirMonedaInt(int.Parse(lb_moneda.SelectedValue)) + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
                mensaje += "</script>";
                Page.RegisterClientScriptBlock("closewindow", mensaje);
            }
        }
    }


    protected void gv_detalle_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1) { 
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[12].Visible = false;
            e.Row.Cells[13].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[11].Visible = false;
        }

    }
    protected void gv_notacredito_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = true;
            e.Row.Cells[10].Visible = false;
        }

    }
    protected void gv_notadebito_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1) { 
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = true;
        }
    }
    protected void gv_cortes_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;
    }
    protected void lb_tipopersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Limpiar Tipo de Persona
        gv_notacredito.Visible = true;
        gv_notacredito_nd.Visible = true;
        tb_agenteID.Text = "";
        tb_agentenombre.Text = "";
        tb_direccion.Text = "";
        tb_telefono.Text = "";
        #endregion
    }
    protected void gv_notacredito_nd_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = true;
            e.Row.Cells[10].Visible = false;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GridViewRowCollection gvr = gv_notacredito.Rows;
        foreach (GridViewRow row in gvr)
        {
            if (row.Cells[10].Text == "31")
            {
                row.Visible = false;
            }
        }
        GridViewRowCollection gvra = gv_notacredito_nd.Rows;
        foreach (GridViewRow row in gvra)
        {
            if (row.Cells[10].Text == "12")
            {
                row.Visible = false;
            }
        }
    }
    protected void gv_factintercompany_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = true;
        }

    }
    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        if ((tb_agenteID.Text.Trim() != "") && (tb_agenteID.Text.Trim() != "0") && (tb_agentenombre.Text.Trim() != ""))
        {
            cargo_pendliquidar_y_cortes();
        }
    }
    protected void lb_moneda_SelectedIndexChanged(object sender, EventArgs e)
    {
        gv_detalle.DataBind();
        gv_notacredito.DataBind();
        gv_notacredito_nd.DataBind();
        gv_notadebito.DataBind();
        gv_fact_intercompany.DataBind();
        gv_cortes.DataBind();
    }
    protected void gv_detalle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = 0; i < e.Row.Cells.Count; i++)
        {
            TableCell c = e.Row.Cells[i];
            switch (i)
            {
                case 0: c.Width = new Unit(15); break;
                case 2: c.Width = new Unit("100%"); break;
                case 3: c.Width = new Unit(65); break;
                case 4: c.Width = new Unit("100%"); break;
                case 5: c.Width = new Unit("100%"); break;
                case 6: c.Width = new Unit(75); break;
                case 8: c.Width = new Unit(50); break;
                case 9: c.Width = new Unit(55); break;
                case 10: c.Width = new Unit(55); break;
                default:
                    break;
            }
        }
    }
}
