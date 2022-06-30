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
using System.Text;


public partial class operations_provisionagente : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    int ban_sucursales = 0;
    public string simbolomoneda = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
            Obtengo_Listas();
        }


        #region Habilitar TextBox por Sucursales
        ban_sucursales = DB.getSucursalesSinDocumentos(user.SucursalID);
        if (ban_sucursales > 0)
        {
            tb_hbl.ReadOnly = false;
            tb_mbl.ReadOnly = false;
            tb_routing.ReadOnly = false;
            tb_contenedor.ReadOnly = false;
        }
        else
        {
            tb_hbl.ReadOnly = true;
            tb_mbl.ReadOnly = true;
            tb_routing.ReadOnly = true;
            tb_contenedor.ReadOnly = true;
        }
        #endregion
        Validar_Restricciones("Load");
        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 2048) == 2048))
            Response.Redirect("index.aspx");

        if (user.PaisID == 1)
        {
            ddl_tipo_documento.Visible = true;
            lb_tipo_docto.Visible = true;
        }


        if (!Page.IsPostBack) 
        {
            ArrayList arr = null;
            ListItem item = null;
            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(5, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serie_factura.Items.Clear();
            lb_serie_factura.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serie_factura.Items.Add(item);
            }


            if (DB.Validar_Restriccion_Activa(user, 5, 24) == true)
            {
                arr = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(user.PaisID, user.contaID, 5, user.SucursalID);
            }
            else
            {
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_servicio");
            }
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_servicio.Items.Add(item);
            }
            if (lb_servicio.Items.Count > 0)
            {
                lb_servicio.SelectedIndex = 1;
                ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), "");
                RE_GenericBean rubbean = null;
                for (int a = 0; a < rubros.Count; a++)
                {
                    rubbean = (RE_GenericBean)rubros[a];
                    item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
                    lb_rubro.Items.Add(item);
                }
            }
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda2.Items.Add(item);
                lb_moneda.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_contribuyente");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_contribuyente.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_imp_exp.Items.Add(item);
            }

            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 4);
            if (user.contaID == 2)
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
                lb_moneda2.SelectedValue = moneda_inicial.ToString();
                lb_moneda.Enabled = false;
            }
            else
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
                lb_moneda2.SelectedValue = moneda_inicial.ToString();
                lb_moneda.Enabled = false;

            }

            arr = (ArrayList)DB.getTipoDocumentoSat(user,"");
            item = new ListItem("Seleccione...", "0");
            ddl_tipo_documento.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {

                item = new ListItem(rgb.strC2 + "-" + rgb.strC1, rgb.intC1.ToString());
                ddl_tipo_documento.Items.Add(item);
            }

        }
        if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; }
        if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; }
        if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; }
        if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; }
        if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; }
        if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; }
        if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; }
        if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; }

        cargo_datos_BL();
    }
    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (drp_tipo_transaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transacion a Operar");
            return;
        }
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
            //if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(name)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(name)) ilike '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
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
            //if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
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
            Arr = (ArrayList)DB.Get_Intercompanys(where);//Intercompany
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
        if (lb_tipopersona.SelectedValue.Equals("4"))//proveedor
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_contacto.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[8].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
            tb_nit.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2"))//agente
        {
            if ((DB.Validar_Restriccion_Activa(user, 5, 29) == true))
            {
                #region Validacion Grupo Empresas
                if ((user.SucursalID == 9) || (user.SucursalID == 71))
                {
                }
                else if (user.pais.Grupo_Empresas == 1)
                {
                }
                else if (user.pais.Grupo_Empresas == 2)
                {
                    if (Page.Server.HtmlDecode(row.Cells[7].Text) == "NO NEUTRAL")
                    {
                        WebMsgBox.Show("No se puede utilizar el Agente.: " + Page.Server.HtmlDecode(row.Cells[1].Text) + " - " + Page.Server.HtmlDecode(row.Cells[2].Text) + ", porque es un Agente NO NEUTRAL");
                        return;
                    }
                }
                else if (user.pais.Grupo_Empresas == 3)
                {
                }
                #endregion
            }
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[4].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
            //tb_nit.Text = Page.Server.HtmlDecode(row.Cells[9].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//linea aerea
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_nit.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
        }
        
        lb_contribuyente.SelectedValue = DB.getProveedorRegimen(int.Parse(lb_tipopersona.SelectedValue), tb_agenteID.Text.ToString()).ToString();
        bool res = Validar_Restricciones("gv_proveedor_PageIndexChanging");
        if (res == false)
        {
            return;
        }
    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //user = (UsuarioBean)Session["usuario"];
            //ArrayList Arr = null;
            //string where = "";
            //Arr = (ArrayList)DB.getAgente(where);
            //dt = (DataTable)Utility.fillGridView("Agente", Arr);
            //ViewState["proveedordt"] = dt;
            //gv_proveedor.DataSource = dt;
            //gv_proveedor.DataBind();
        }
    }

    protected void gv_detalle_DataBound(object sender, EventArgs e)
    {
        decimal subtotal = 0;
        decimal impuesto = 0;
        decimal total = 0;
        decimal totalD = 0;
        double afecto = 0;
        double noafecto = 0;
        
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        Label lb1, lb2, lb3, lb4, lb6;

        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_subtotal");
            lb2 = (Label)row.FindControl("lb_impuesto");
            lb3 = (Label)row.FindControl("lb_total");
            lb4 = (Label)row.FindControl("lb_totaldolares");
            lb6 = (Label)row.FindControl("lb_codigo");
            subtotal += Math.Round(decimal.Parse(lb1.Text), 2);
            totalD += Math.Round(decimal.Parse(lb4.Text), 2);

            Rubros rubtemp = new Rubros();
            Rubros rub = new Rubros();
            rub.rubroID = int.Parse(lb6.Text);
            rub.rubroSubTot = double.Parse(lb1.Text);
            rub.rubroImpuesto = double.Parse(lb2.Text);
            rub.rubroTot = double.Parse(lb3.Text);
            
            rubtemp = (Rubros)DB.ExistRubroPais(rub, user.PaisID);
            if (rubtemp == null)
            {
                WebMsgBox.Show("Error, no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                return;
            }

            if (rubtemp.rubroImpuesto == 0)
            {
                noafecto += rubtemp.rubroSubTot;
            }
            else
            {
                afecto += rubtemp.rubroSubTot;
            }

            lb1.Text = rubtemp.rubroSubTot.ToString();
            lb2.Text = rubtemp.rubroImpuesto.ToString();
            lb3.Text = rubtemp.rubroTot.ToString();

            impuesto += Math.Round((decimal)rubtemp.rubroImpuesto, 2);
            total += Math.Round((decimal)rubtemp.rubroTot, 2);

        }

        tb_subtotal.Text = subtotal.ToString("#,#.00#;(#,#.00#)");
        tb_impuesto.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
        tb_totaldolares.Text = totalD.ToString("#,#.00#;(#,#.00#)");
        tb_afecto.Text = afecto.ToString("#,#.00#;(#,#.00#)");
        tb_noafecto.Text = noafecto.ToString("#,#.00#;(#,#.00#)");
    }


    protected void gv_detalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = new DataTable();
        //dt.Columns.Add("ID");
        //dt.Columns.Add("NOMBRE");
        //dt.Columns.Add("TYPE");
        //dt.Columns.Add("MONEDATYPE");
        //dt.Columns.Add("TOTALD");
        //dt.Columns.Add("SUBTOTAL");
        //dt.Columns.Add("IMPUESTO");
        //dt.Columns.Add("TOTAL");

        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Columns.Add("TOTALD");

        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_codigo");
            lb2 = (Label)row.FindControl("lb_rubro");
            lb3 = (Label)row.FindControl("lb_tipo");
            lb4 = (Label)row.FindControl("lb_subtotal");
            lb5 = (Label)row.FindControl("lb_impuesto");
            lb6 = (Label)row.FindControl("lb_total");
            lb7 = (Label)row.FindControl("lb_totaldolares");
            lb8 = (Label)row.FindControl("lb_monedatipo");
            //object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb7.Text, lb4.Text, lb5.Text, lb6.Text };
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text };
            dt.Rows.Add(objArr);
        }
        dt.Rows[e.RowIndex].Delete();
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }
    protected void bt_search_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;

        if (lb_tipo.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio.Text.Trim(), user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio.Text.Trim(), user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio.Text.Trim(), user.pais.schema, codDistribucion, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio.Text.Trim(), user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("TERRESTRE T"))
        {
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio.Text.Trim(), user.pais.ISO, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
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
    protected void dgw1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw1.DataSource = dt1;
        dgw1.PageIndex = e.NewPageIndex;
        dgw1.DataBind();
    }

    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        if (lb_serie_factura.Items.Count == 0)
        {
            WebMsgBox.Show("No existe Serie Definida para Generar Provision");
            return;
        }
        if (lb_serie_factura.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Serie a utilizar");
            return;
        }
        if (drp_tipo_transaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transacion a Operar");
            return;
        }
        if (lb_tipopersona.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Persona");
            return;
        }
        if (lb_serie_factura.Items.Count == 0)
        {
            WebMsgBox.Show("No se puede Provisionar si no existe una Serie");
            return;
        }
        if ((!tb_contenedor.Text.Trim().Equals("")))
        {
            string resultado = "";
            resultado = DB.ValidarContenedor(tb_contenedor.Text.ToUpper());
            if (resultado == "0")
            {
                WebMsgBox.Show("El numero de Contenedor no es valido");
                return;
            }
            if (resultado == "")
            {
                WebMsgBox.Show("Existio un error porque el Numero de Contenedor no es valido");
                return;
            }
        }
        if (tb_agenteID.Text.Trim().Equals("")) {
            WebMsgBox.Show("Es necesario que indique un proveedor");
            tb_agenteID.Focus();
            return;
        }
        if (tb_agenteID.Text.Trim().Equals("0"))
        {
            WebMsgBox.Show("Es necesario que indique un proveedor");
            tb_agenteID.Focus();
            return;
        }
        else if (((lb_imp_exp.SelectedItem.Text == "IMPORTACION") || (lb_imp_exp.SelectedItem.Text == "EXPORTACION")) && (lb_tipopersona.SelectedValue != "10"))
        {
            if ((lbl_tipoOperacionID.Text != "13") && (lbl_tipoOperacionID.Text != "14"))
            {
                if ((tb_hbl.Text.Trim().Equals("")) && (tb_mbl.Text.Trim().Equals("")) && (lbl_tipoOperacionID.Text != ""))
                {
                    WebMsgBox.Show("Es necesario indicar HBL o MBL");
                    return;
                }
            }
        }
        if (gv_detalle.Rows.Count == 0)
        {
                WebMsgBox.Show("Es necesario que provisione por lo menos 1 rubro");
                tb_fechadoc.Focus();
                return;
        
        }
        int contribuyente = int.Parse(lb_contribuyente.SelectedValue);//excento contribuyente
        int moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue);//importacion exportacion
        int tipo_cobro = 1;//prepaid, collect
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());//fiscal o financiera
        //Revisar con Jose Cruz
        if (tipo_contabilidad == 2) contribuyente = 1; // si es financiera contribuyente=excento
        int servicio = 0; //fcl, lcl, etc
        int tipopersona = int.Parse(lb_tipopersona.SelectedValue);
        int transID = 15;
        switch (int.Parse(drp_tipo_transaccion.SelectedValue))
        {
            case 8:
                transID = 8;
                break;
            case 53:
                transID = 8;
                break;
            case 105:
                transID = 105;
                break;
        }
        Bean_Provision_Automatica Provision_Automatica = new Bean_Provision_Automatica();
        Provision_Automatica.tpr_prov_id = 0;
        Provision_Automatica.tpr_proveedor_id = int.Parse(tb_agenteID.Text);
        Provision_Automatica.tpr_valor = double.Parse(tb_total.Text);
        Provision_Automatica.tpr_afecto = double.Parse(tb_afecto.Text);
        Provision_Automatica.tpr_noafecto = double.Parse(tb_noafecto.Text);
        Provision_Automatica.tpr_iva = double.Parse(tb_impuesto.Text);
        Provision_Automatica.tpr_observacion = tb_observacion.Text.Trim();
        Provision_Automatica.tpr_suc_id = user.SucursalID;
        Provision_Automatica.tpr_pai_id = user.PaisID;
        Provision_Automatica.tpr_usu_creacion = user.ID;
        Provision_Automatica.tpr_fecha_creacion = DB.getDateTimeNow().Substring(0, 10);
        Provision_Automatica.tpr_usu_acepta = user.ID;
        Provision_Automatica.tpr_fecha_acepta = Provision_Automatica.tpr_fecha_creacion;
        Provision_Automatica.tpr_departamento = 0;
        Provision_Automatica.tpr_ted_id = 5;
        Provision_Automatica.tpr_serie = lb_serie_factura.SelectedItem.Text;
        Provision_Automatica.tpr_serie_oc = "";
        Provision_Automatica.tpr_correlativo_oc = 0;
        Provision_Automatica.tpr_tts_id = 0;
        Provision_Automatica.tpr_hbl = tb_hbl.Text;
        Provision_Automatica.tpr_mbl = tb_mbl.Text;
        Provision_Automatica.tpr_routing = tb_routing.Text;
        Provision_Automatica.tpr_contenedor = tb_contenedor.Text;
        Provision_Automatica.tpr_tpi_id = tipopersona;
        Provision_Automatica.tpr_correlativo = 0;
        Provision_Automatica.tpr_mon_id = int.Parse(lb_moneda.SelectedValue);
        Provision_Automatica.tpr_serie_contrasena = "";
        Provision_Automatica.tpr_contrasena_correlativo = 0;
        Provision_Automatica.tpr_valor_equivalente = double.Parse(tb_totaldolares.Text);
        Provision_Automatica.tpr_ip_address = Request.ServerVariables["REMOTE_ADDR"].ToString();
        if ((tb_documento_serie.Text.Trim() != "") && (tb_documento_correlativo.Text.Trim() != "") && (tb_fechadoc.Text.Trim() != ""))
        {
            Provision_Automatica.tpr_fact_id = tb_documento_serie.Text.Trim();
            Provision_Automatica.tpr_fact_corr = tb_documento_correlativo.Text.Trim();
            Provision_Automatica.tpr_fact_fecha = DB.DateFormat(tb_fechadoc.Text.Trim());
            Provision_Automatica.tpr_fecha_maxpago = Provision_Automatica.tpr_fact_fecha;

            DateTime aux_fecha_emision = DateTime.Parse(Provision_Automatica.tpr_fecha_maxpago);
            double dias_credito = DB.getFechaMaxPago(Provision_Automatica.tpr_proveedor_id , Provision_Automatica.tpr_tpi_id, user);
            DateTime aux_fecha_pago = aux_fecha_emision.AddDays(dias_credito);
            string fecha_pago_formateada = aux_fecha_pago.ToString();
            Provision_Automatica.tpr_fecha_maxpago = DateTime.Parse(fecha_pago_formateada).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);

        }
        else if ((tb_documento_serie.Text.Trim() != "") || (tb_documento_correlativo.Text.Trim() != "") || (tb_fechadoc.Text.Trim() != ""))
        {
            WebMsgBox.Show("Debe Ingresar Todos los Campos del Documento de Proveedor");
            return;
        }
        if (Provision_Automatica.tpr_fact_fecha == "")
        {
            Provision_Automatica.tpr_fact_fecha = null;
            Provision_Automatica.tpr_fecha_maxpago = null;
        }
        Provision_Automatica.tpr_imp_exp_id = int.Parse(lb_imp_exp.SelectedValue);
        Provision_Automatica.tpr_bien_serv = 2;
        Provision_Automatica.tpr_tcon_id = user.contaID;
        Provision_Automatica.tpr_fecha_emision = lb_fecha_hora.Text;
        Provision_Automatica.tpr_nombre = tb_agentenombre.Text.Trim();
        Provision_Automatica.tpr_proveedor_cajachica_id = 0;
        Provision_Automatica.tpr_poliza = "";
        Provision_Automatica.tpr_tds_id = int.Parse(ddl_tipo_documento.SelectedValue.ToString());
        if (bool.Parse(Rb_Documento.SelectedValue) == true)
        {
            if (tb_fecha_libro_compras.Text.Trim() == "")
            {
                WebMsgBox.Show("Debe especificar la Fecha del Libro de Compras");
                return;
            }
            Provision_Automatica.tpr_fecha_libro_compras = DB.DateFormat(tb_fecha_libro_compras.Text);
            Provision_Automatica.tpr_fiscal = bool.Parse(Rb_Documento.SelectedValue);
        }
        else
        {
            Provision_Automatica.tpr_fecha_libro_compras = null;
            Provision_Automatica.tpr_fiscal = bool.Parse(Rb_Documento.SelectedValue);
        }
        Provision_Automatica.tpr_tto_id = int.Parse(lbl_tipoOperacionID.Text);
        Provision_Automatica.tpr_ruta_pais = "";
        Provision_Automatica.tpr_ruta = "";
        Provision_Automatica.tpr_blid = int.Parse(lbl_blID.Text);
        Provision_Automatica.tpr_tti_id = int.Parse(lb_contribuyente.SelectedValue);
        Provision_Automatica.tpr_toc_id = 0;
        Provision_Automatica.tpr_ttt_id = int.Parse(drp_tipo_transaccion.SelectedValue);
        bool valida_agente = Validar_Restricciones("Validar_Econocaribe");
        if (valida_agente == true)
        {
            return;
        }
        //bool validar_restriccion = Validar_Restricciones("bt_guardar");
        //if (validar_restriccion == true)
        //{
            Provision_Automatica.tpr_ted_id = 5;
            Provision_Automatica.tpr_usu_acepta = user.ID;
            Provision_Automatica.tpr_fecha_acepta = DB.getDateTimeNow().Substring(0, 10);
            Provision_Automatica.tpr_ttd_id = 3;
        //}
        //else
        //{
        //    Provision_Automatica.tpr_ted_id = 1;
        //    Provision_Automatica.tpr_usu_acepta = "";
        //    Provision_Automatica.tpr_fecha_acepta = null;
        //    Provision_Automatica.tpr_ttd_id = 1;
        //}
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        Rubros rubro;
        #region Definir Rubros
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_codigo");
            lb2 = (Label)row.FindControl("lb_rubro");
            lb3 = (Label)row.FindControl("lb_tipo");
            lb4 = (Label)row.FindControl("lb_subtotal");
            lb5 = (Label)row.FindControl("lb_impuesto");
            lb6 = (Label)row.FindControl("lb_total");
            lb7 = (Label)row.FindControl("lb_totaldolares");
            lb8 = (Label)row.FindControl("lb_monedatipo");
            rubro = new Rubros();
            rubro.rubroID = long.Parse(lb1.Text); ;
            rubro.rubroName = lb2.Text;
            rubro.rubtoType = lb3.Text;
            rubro.rubroSubTot = double.Parse(lb4.Text);
            rubro.rubroImpuesto = double.Parse(lb5.Text);
            rubro.rubroTot = double.Parse(lb6.Text);
            rubro.rubroTotD = double.Parse(lb7.Text);
            rubro.rubroTypeID = Utility.TraducirServiciotoINT(rubro.rubtoType);
            rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);


            int tttID = 0;
            tttID = transID;
            if (tttID == 105)
            {
                tttID = 15;
            }


            rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, tttID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, rubro.rubroTypeID);

            if ((rubro.cta_debe == null) || (rubro.cta_debe.Count == 0))
            {
                WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de pago que esta realizando, por favor pongase en contacto con el Contador");
                return ;
            }
            if (Provision_Automatica.arr1 == null) Provision_Automatica.arr1 = new ArrayList();
            Provision_Automatica.arr1.Add(rubro);
        }
        #endregion
        #region Definir Costos
        string tipo_operacionID = lbl_tipoOperacionID.Text;
        if (tipo_operacionID != "0")
        {
            RE_GenericBean Bean_Costos = null;
            string tipo_bl = "";
            if ((tipo_operacionID == "1") || (tipo_operacionID == "17"))
            {
                tipo_bl = "F";
            }
            else if ((tipo_operacionID == "2") || (tipo_operacionID == "18"))
            {
                tipo_bl = "L";
            }
            foreach (GridViewRow row in gv_detalle.Rows)
            {
                lb1 = (Label)row.FindControl("lb_codigo");
                lb2 = (Label)row.FindControl("lb_rubro");
                lb3 = (Label)row.FindControl("lb_tipo");
                lb4 = (Label)row.FindControl("lb_subtotal");
                lb5 = (Label)row.FindControl("lb_impuesto");
                lb6 = (Label)row.FindControl("lb_total");
                lb7 = (Label)row.FindControl("lb_totaldolares");
                lb8 = (Label)row.FindControl("lb_monedatipo");
                #region Definir Moneda
                ArrayList Arr_moneda = null;
                string _simbolomoneda = "";
                int _idmoneda_master = 0;
                if (lb8.Text == "HNL")
                {
                    _simbolomoneda = "LPS";
                }
                else if (lb8.Text == "NIC")
                {
                    _simbolomoneda = "NIO";
                }
                else
                {
                    _simbolomoneda = lb8.Text;
                }
                Arr_moneda = (ArrayList)DB.Traducir_Moneda_BAW_to_MASTER(user, _simbolomoneda);
                if (Arr_moneda != null)
                {
                    _idmoneda_master = int.Parse(Arr_moneda[0].ToString());
                    _simbolomoneda = Arr_moneda[1].ToString();
                }
                #endregion
                Bean_Costos = new RE_GenericBean();
                Bean_Costos.intC1 = 4;//Tipo Prorrateo Master
                Bean_Costos.intC2 = DB.Traducir_Tipo_Persona_BawTOMaster(tipopersona.ToString());//Id Tipo Proveedor Master
                Bean_Costos.intC3 = int.Parse(tb_agenteID.Text);//Id Proveedor
                Bean_Costos.strC1 = tb_agentenombre.Text.Trim();//Proveedor Nombre
                Bean_Costos.intC4 = int.Parse(lb1.Text);//Id_rubro
                Bean_Costos.strC2 = lb2.Text;//Rubro Nombre
                Bean_Costos.intC5 = _idmoneda_master;//Id_moneda
                Bean_Costos.strC3 = _simbolomoneda;//Simbolo Moneda
                Bean_Costos.douC1 = double.Parse(lb6.Text);//Costo
                Bean_Costos.intC6 = int.Parse(lbl_blID.Text);//Blid
                Bean_Costos.intC7 = int.Parse(lbl_tipoOperacionID.Text);//Tipo Operacion ID
                Bean_Costos.intC8 = Utility.TraducirServiciotoINT(lb3.Text);//Id Tipo Servicio
                Bean_Costos.strC4 = lb3.Text;//Tipo de Servicio
                Bean_Costos.intC9 = int.Parse(lb_contribuyente.SelectedValue);//Es Afecto
                Bean_Costos.strC5 = tipo_bl;//Tipo BL
                Bean_Costos.Tipo_Operacion = int.Parse(lbl_tipoOperacionID.Text);
                Bean_Costos.Blid = int.Parse(lbl_blID.Text);
                Bean_Costos.intC10 = 0;//documentoID
                Bean_Costos.intC11 = 5;//Provision
                Bean_Costos.strC6 = tipo_bl;
                Bean_Costos.strC7 = "";//Serie Provision
                Bean_Costos.strC8 = "";//Correlativo Provision
                Bean_Costos.intC12 = int.Parse(lb_contribuyente.SelectedValue);//Tipo de Contribuyente
                Bean_Costos.intC13 = 0;//tdf_id
                Bean_Costos.intC14 = 0;//costo_id
                if (Provision_Automatica.arr2 == null) Provision_Automatica.arr2 = new ArrayList();
                Provision_Automatica.arr2.Add(Bean_Costos);
            }
        }
        #endregion

        int matOpID = DB.getMatrizOperacionID(int.Parse(drp_tipo_transaccion.SelectedValue), moneda, user.PaisID, tipo_contabilidad);
        ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID, "Abono");
        Provision_Automatica.arr3 = ctas_cargo;


        string Check_Existencia = DB.CheckExistDoc(Provision_Automatica.tpr_fecha_emision, 5);
        if (Check_Existencia == "0")
        {
            ArrayList result = (ArrayList)DB.Insertar_Provision_Automatiza_Traficos(user, Provision_Automatica);
            if (result != null && result.Count > 0)
            {
                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[0].ToString()), 5, 0);
                gv_detalle_partida.DataBind();
                int i = 0;
                Label mi_variable;
                foreach (GridViewRow row in gv_detalle_partida.Rows)
                {
                    mi_variable = (Label)row.FindControl("lb_desc_cuenta");
                    if (mi_variable.Text.Equals("TOTAL"))
                    {
                        gv_detalle_partida.Rows[i].Font.Bold = true;
                    }
                    i++;
                }
                WebMsgBox.Show("La Provision fue grabada y autoriza exitosamente con Serie.: " + lb_serie_factura.SelectedItem.Text + " y Correlativo.: " + result[2].ToString());
                tb_corr.Text = result[2].ToString();
                bt_guardar.Enabled = false;
                gv_detalle.Enabled = false;
                bt_agregar.Enabled = false;
                lb_contribuyente.Enabled = false;
                lb_imp_exp.Enabled = false;
                drp_tipo_transaccion.Enabled = false;
                return;
            }
            else
            {
                WebMsgBox.Show("Existió un problema al grabar la información, por favor intente de nuevo");
                return;
            }
        }
        else
        {
            bt_guardar.Enabled = false;
            gv_detalle.Enabled = false;
            bt_agregar.Enabled = false;
            lb_contribuyente.Enabled = false;
            lb_imp_exp.Enabled = false;
            drp_tipo_transaccion.Enabled = false;
            WebMsgBox.Show("El Documento ya fue Guardado");
            return;
        }
    }
    protected void buscar_rubro_Click(object sender, EventArgs e)
    {
        if (lb_servicio.Items.Count <= 0)
        {
            WebMsgBox.Show("No se existen Tipos de Servicio disponibles");
            return;
        }
        if (lb_rubro.Items.Count <= 0)
        {
            WebMsgBox.Show("No existen Rubros configurados");
            return;
        }
        if (tb_monto.Text == "0.00")
        {
            WebMsgBox.Show("No se puede agregar un rubro con valor cero");
            btn_rubro_ModalPopupExtender.Show();
            return;
        }
        user = (UsuarioBean)Session["usuario"];
        DataTable dt = new DataTable();
        //dt.Columns.Add("ID");
        //dt.Columns.Add("NOMBRE");
        //dt.Columns.Add("TYPE");
        //dt.Columns.Add("MONEDATYPE");
        //dt.Columns.Add("TOTALD");
        //dt.Columns.Add("SUBTOTAL");
        //dt.Columns.Add("IMPUESTO");
        //dt.Columns.Add("TOTAL");
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Columns.Add("TOTALD");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_codigo");
            lb2 = (Label)row.FindControl("lb_rubro");
            lb3 = (Label)row.FindControl("lb_tipo");
            lb4 = (Label)row.FindControl("lb_subtotal");
            lb5 = (Label)row.FindControl("lb_impuesto");
            lb6 = (Label)row.FindControl("lb_total");
            lb7 = (Label)row.FindControl("lb_totaldolares");
            lb8 = (Label)row.FindControl("lb_monedatipo");
            //object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb7.Text, lb4.Text, lb5.Text, lb6.Text };
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text };
            dt.Rows.Add(objArr);
        }

        Rubros rubro = new Rubros();
        rubro.rubroID = long.Parse(lb_rubro.SelectedValue);
        rubro.rubroName = lb_rubro.SelectedItem.Text;
        rubro.rubroMoneda = long.Parse(lb_moneda2.SelectedValue);
        rubro.rubtoType = lb_servicio.SelectedItem.Text;

        rubro.rubroTot = double.Parse(tb_monto.Text);
        Rubros rubtemp = (Rubros)rubro;
        rubtemp = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
        if (rubtemp == null)
        {
            WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
            return;
        }
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        
        if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))
        {
            if (rubtemp.IvaInc == 1)
            {
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
            }
            else
            {
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
            }
        }
        else
        {
            rubtemp.rubroImpuesto = 0;
            rubtemp.rubroSubTot = rubtemp.rubroTot;
        }
        decimal tipoCambio = user.pais.TipoCambio;
        double totalD = rubtemp.rubroTot;
        if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
        {
            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
        }
        else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
        {
            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto * (double)tipoCambio, 2);
            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot * (double)tipoCambio, 2);
            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
        }
        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
        {
            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto / (double)tipoCambio, 2);
            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot / (double)tipoCambio, 2);
            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
        }
        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
        {
            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
        }

        //object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)") };
        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
        dt.Rows.Add(obj);
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
        #region Validar Sender
        if (sender is Button)
        {
            Button btn_aux = (Button)sender;
            if (btn_aux.ID == "btn_siguiente_rubro")
            {
                btn_rubro_ModalPopupExtender.Show();
            }
        }
        #endregion
        tb_monto.Text = "0.00";
    }
    protected void lb_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        int servID = int.Parse(lb_servicio.SelectedValue);

        ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, servID, "");
        RE_GenericBean rubbean = null;
        ListItem item = null;
        lb_rubro.Items.Clear();
        for (int a = 0; a < rubros.Count; a++)
        {
            rubbean = (RE_GenericBean)rubros[a];
            item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
            lb_rubro.Items.Add(item);
        }
        btn_rubro_ModalPopupExtender.Show();
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
    }
    protected void dgw12_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw12.DataSource = dt1;
        dgw12.PageIndex = e.NewPageIndex;
        dgw12.DataBind();
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
    protected void lb_tipopersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Limpiar Tipo de Persona
        tb_agenteID.Text = "";
        tb_agentenombre.Text = "";
        tb_contacto.Text = "";
        tb_direccion.Text = "";
        tb_telefono.Text = "";
        tb_correoelectronico.Text = "";
        tb_agenteID.Text = "0";
        #endregion
    }
    protected bool Validar_Restricciones(string sender)
    {
        bool resultado = false;
        bool paiR = DB.Validar_Pais_Restringido(user);
        if (paiR == true)
        {
            #region WhiteList Proveedores
            if (DB.Validar_Restriccion_Activa(user, 5, 13) == true)
            {
                if ((sender == "gv_proveedor_PageIndexChanging") || (sender == "Load"))
                {
                    if (lb_tipopersona.SelectedValue == "10")
                    {
                        lbl_whitelist.Text = "TRUE";
                    }
                    else
                    {
                        lbl_whitelist.Text = DB.Validar_WhiteList(int.Parse(tb_agenteID.Text), int.Parse(lb_tipopersona.SelectedValue.ToString()), user.PaisID, user.SucursalID);
                    }
                    if (lbl_whitelist.Text == "FALSE")
                    {
                        #region No esta en WhiteList
                        btn_rubro_ModalPopupExtender.Enabled = false;
                        bt_guardar.Enabled = false;
                        resultado = false;
                        if ((tb_agenteID.Text != "0") && (sender == "gv_proveedor_PageIndexChanging"))
                        {
                            Limpiar();
                            if (lb_tipopersona.SelectedValue == "4")
                            {
                                WebMsgBox.Show("Al Proveedor con Codigo: " + tb_agenteID.Text.Trim() + ", solo se puede hacer Provisiones Automaticas");
                            }
                            else if (lb_tipopersona.SelectedValue == "2")
                            {
                                WebMsgBox.Show("Al Agente con Codigo: " + tb_agenteID.Text.Trim() + ", solo se puede hacer Provisiones Automaticas");
                            }
                            else if (lb_tipopersona.SelectedValue == "5")
                            {
                                WebMsgBox.Show("A la Naviera con Codigo: " + tb_agenteID.Text.Trim() + ", solo se puede hacer Provisiones Automaticas");
                            }
                            else if (lb_tipopersona.SelectedValue == "6")
                            {
                                WebMsgBox.Show("A la Linea Aerea con Codigo: " + tb_agenteID.Text.Trim() + ", solo se puede hacer Provisiones Automaticas");
                            }
                        }
                        #endregion
                    }
                    else if (lbl_whitelist.Text == "TRUE")
                    {
                        #region Esta en WhiteList
                        btn_rubro_ModalPopupExtender.Enabled = true;
                        bt_guardar.Enabled = true;
                        resultado = true;
                        #endregion
                    }
                }
            }
            #endregion
            #region NO EMITIR PAGO MANUAL AGENTE ECONO
            if ((DB.Validar_Restriccion_Activa(user, 5, 26) == true) && (sender == "Validar_Econocaribe"))
            {
                resultado = false;
                if (lb_tipopersona.SelectedValue == "2")
                {
                    if (user.pais.Grupo_Empresas == 2)
                    {
                        int res = DB.Validar_Persona_Grupo_Econocaribe(user, int.Parse(tb_agenteID.Text), 2);
                        if (res > 0)
                        {
                            WebMsgBox.Show("El Agente Econocaribe no puede ser utilizado en modulo.: " + user.pais.Nombre_Sistema + ", debe utilizar el Modulo de Aimar");
                            resultado = true;
                        }
                    }
                }
                return resultado;
            }
            #endregion
            #region CONTABILIZAR AUTOMATICAMENTE LA PROVISION
            if ((DB.Validar_Restriccion_Activa(user, 5, 40) == true) && (sender == "bt_guardar"))
            {
                resultado = true;
            }
            #endregion
        }
        return resultado;
    }
    protected void Limpiar()
    {
        #region Limpiar
        tb_mbl.Text = "";
        tb_hbl.Text = "";
        tb_routing.Text = "";
        tb_contenedor.Text = "";
        tb_agentenombre.Text = "";
        tb_direccion.Text = "";
        tb_telefono.Text = "";
        tb_contacto.Text = "";
        tb_observacion.Text = "";
        tb_documento_serie.Text = "";
        tb_documento_correlativo.Text = "";
        tb_fechadoc.Text = "";
        tb_poliza.Text = "";
        gv_detalle.DataBind();
        #endregion
    }
    protected void drp_tipo_transaccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Limpiar Tipo de Persona
        tb_agenteID.Text = "";
        tb_agentenombre.Text = "";
        tb_contacto.Text = "";
        tb_direccion.Text = "";
        tb_telefono.Text = "";
        tb_correoelectronico.Text = "";
        tb_agenteID.Text = "0";
        #endregion
        if (!drp_tipo_transaccion.SelectedItem.Text.Equals("Seleccione..."))
        {
            int tranID = int.Parse(drp_tipo_transaccion.SelectedValue);
            if (tranID == 15) { lb_tipopersona.SelectedValue = "2"; }//Agentes
            else if (tranID == 18) { lb_tipopersona.SelectedValue = "6"; }//Linea aerea
            else if (tranID == 8) { lb_tipopersona.SelectedValue = "4"; }//Proveedores
            else if (tranID == 17) { lb_tipopersona.SelectedValue = "5"; }//Naviera
            else if (tranID == 105) { lb_tipopersona.SelectedValue = "10"; }//Intercompanys
        }
        else
        {
            lb_tipopersona.SelectedValue = "0";
        }
    }
    protected void Obtengo_Listas()
    {
        //ArrayList arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='provisiones.aspx' and ttt_id not in (13,53, 105) ");
        ArrayList arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='provisiones.aspx' and ttt_id not in (13,53, 105) ");
        ListItem item = null;
        drp_tipo_transaccion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_transaccion.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_tipo_transaccion.Items.Add(item);
        }
    }
    private void cargo_datos_BL()
    {
        #region Cargo Datos BL
        if ((Request.QueryString["bl_no"] != null) && (Request.QueryString["tipo"] != null))
        {
            string bl_no = Request.QueryString["bl_no"].ToString().Trim();
            string tipo = Request.QueryString["tipo"].ToString().Trim();
            string blID = Request.QueryString["blid"].ToString();
            string opID = Request.QueryString["opid"].ToString();
            string idRouting = "-1";
            if (tipo.Equals("LCL"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataLCL(bl_no, user, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = rgb.lonC10.ToString();
                tb_routing.Text = DB.getRouting(rgb.lonC10);
                lb_imp_exp.SelectedValue = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("FCL"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataFCL(bl_no, user, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = rgb.lonC10.ToString();
                tb_routing.Text = DB.getRouting(rgb.lonC10);
                lb_imp_exp.SelectedValue = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                double QS_BLID = 0;
                if (Request.QueryString["blid"] != null && Request.QueryString["blid"] != "")
                {
                    QS_BLID = double.Parse(Request.QueryString["blid"].ToString());
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getDataTerrestre_Costos(QS_BLID, user,0);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                idRouting = DB.GetIDRoutingbyNumero(rgb.strC7);
                tb_routing.Text = rgb.strC7;
                tb_contenedor.Text = rgb.strC4;
            }
            else if (tipo.Equals("AEREO"))
            {
                int Tipo_Guia = 0;
                if (Request.QueryString["opid"] != null)
                {
                    Tipo_Guia = int.Parse(Request.QueryString["opid"].ToString());
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataAereo(bl_no, 0, Tipo_Guia, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = "";
                idRouting = rgb.lonC8.ToString();
                tb_routing.Text = rgb.strC7;
                lb_imp_exp.SelectedValue = rgb.strC10;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("RO ADUANAS"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceRO_Aduanas(user, bl_no, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = lbl_blID.Text;
                tb_routing.Text = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("RO SEGUROS"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceRO_Seguros(user, bl_no, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = lbl_blID.Text;
                tb_routing.Text = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
        }
        Definir_Tipo_Operacion();
        #endregion
    }
    protected void Definir_Tipo_Operacion()
    {
        #region Definir Tipo Operacion
        if (lbl_operacion.Text != "0")
        {
            ArrayList Arr_Sistemas = DB.Get_Detalle_Sistemas(" and b.tto_id=" + lbl_tipoOperacionID.Text + "");
            foreach (RE_GenericBean Bean in Arr_Sistemas)
            {
                lbl_sistema.Text = Bean.strC1;
                lbl_operacion.Text = Bean.strC2;
            }
            pnl_operacion.Visible = true;
        }
        #endregion
    }
    protected void lb_serie_factura_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie_factura.Items.Count > 0)
        {
            if (lb_serie_factura.SelectedValue != "0")
            {
                //BAW FISCAL USD
                if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
                {
                    int moneda_Serie = 0;
                    moneda_Serie = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                    lb_moneda.SelectedValue = moneda_Serie.ToString();//Moneda de la Transaccion
                    lb_moneda2.SelectedValue = moneda_Serie.ToString();//Moneda del Rubro
                    lb_moneda.Enabled = false;
                    int impo_expo = 0;
                    if (lb_imp_exp.Items.Count > 0)
                    {
                        impo_expo = int.Parse(lb_imp_exp.SelectedValue);
                        Actualizar_Moneda_Rubros();
                    }
                }
            }
        }
    }
    protected void Actualizar_Moneda_Rubros()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Columns.Add("TOTALD");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_codigo");
            lb2 = (Label)row.FindControl("lb_rubro");
            lb3 = (Label)row.FindControl("lb_tipo");
            lb4 = (Label)row.FindControl("lb_subtotal");
            lb5 = (Label)row.FindControl("lb_impuesto");
            lb6 = (Label)row.FindControl("lb_total");
            lb7 = (Label)row.FindControl("lb_totaldolares");
            lb8 = (Label)row.FindControl("lb_monedatipo");

            Rubros rubro = new Rubros();
            rubro.rubroID = long.Parse(lb1.Text);
            rubro.rubroName = lb2.Text;
            rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
            rubro.rubtoType = lb3.Text;
            rubro.rubroTot = double.Parse(lb6.Text);
            Rubros rubtemp = (Rubros)rubro;
            rubtemp = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
            if (rubtemp == null)
            {
                WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                return;
            }
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());

            if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))
            {
                if (rubtemp.IvaInc == 1)
                {
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                }
                else
                {
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                }
            }
            else
            {
                rubtemp.rubroImpuesto = 0;
                rubtemp.rubroSubTot = rubtemp.rubroTot;
            }
            decimal tipoCambio = user.pais.TipoCambio;
            double totalD = rubtemp.rubroTot;
            if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
            {
                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
            }
            else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
            {
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto * (double)tipoCambio, 2);
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot * (double)tipoCambio, 2);
                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
            }
            else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
            {
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto / (double)tipoCambio, 2);
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot / (double)tipoCambio, 2);
                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
            }
            else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
            {
                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
            }
            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
            dt.Rows.Add(obj);
        }
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }
}
