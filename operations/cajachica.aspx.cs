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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
        }
        UsuarioBean user = null;
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];

        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 65536) == 65536))
            Response.Redirect("index.aspx");
        #region Cargar Usuario de Caja Chica
        if (!IsPostBack)
        {
            if (Session["usuario"] != null)
            {
                ArrayList Arr = null;
                Arr = (ArrayList)DB.getDatos_Usuario(user.ID);
                if (Arr != null)
                {
                    foreach (RE_GenericBean rgb in Arr)
                    {
                        tb_agenteID.Text = rgb.intC1.ToString();//ID
                        tb_agentenombre.Text = rgb.strC1;//Nombre
                    }
                }
            }
        }
        #endregion

        if (!Page.IsPostBack) 
        {
            ArrayList arr = null;
            ListItem item = null;

            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda2.Items.Add(item);
                lb_moneda.Items.Add(item);
            }
            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 4);
            lb_moneda.SelectedValue = moneda_inicial.ToString();
            lb_moneda2.SelectedValue = moneda_inicial.ToString();
            lb_moneda.Enabled = false;


            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(5, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serie_factura.Items.Clear();
            lb_serie_factura.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serie_factura.Items.Add(item);
            }

            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_imp_exp.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_contribuyente");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_contribuyente.Items.Add(item);
            }
            if (DB.Validar_Restriccion_Activa(user, 27, 24) == true)
            {
                arr = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(user.PaisID, user.contaID, 27, user.SucursalID);
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
            

        }

        RE_GenericBean cajachicauser = null;
            tb_agenteID.Enabled = true;
            tb_agentenombre.Enabled = true;
        #region Validar Permiso Emitir Provision de Caja Chica
            int bandera_ttt_id = 0;
            int provision_ttt_id = 53;//Provision de Caja Chica
            ListItem Temp_item = null;
            ArrayList Arr_Transacciones_Usuario = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='provisiones.aspx'");
            foreach (RE_GenericBean Bean_Transaccion in Arr_Transacciones_Usuario)
            {
                if (Bean_Transaccion.intC1 == provision_ttt_id)
                {
                    bandera_ttt_id++;
                    Temp_item = new ListItem(Bean_Transaccion.strC1, Bean_Transaccion.intC1.ToString());
                    drp_tipo_transaccion.Items.Add(Temp_item);
                }
            }
            if (bandera_ttt_id == 0)
            {
                bt_guardar.Enabled = false;
                bt_agregar.Enabled = false;
                gv_detalle.Enabled = false;
                bt_agregar.Enabled = false;
                lb_serie_factura.Enabled = false;
                lb_contribuyente.Enabled = false;
                lb_imp_exp.Enabled = false;
                mpeSeleccion.Enabled = false;
                mpeSeleccion2.Enabled = false;
                WebMsgBox.Show("Estimado usuario, usted no tiene permiso para emitir .: Provision de Caja Chica");
                return;
            }
        #endregion
    }
    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        
            
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);

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
        tb_agenteID.Text=Page.Server.HtmlDecode(row.Cells[1].Text);
        tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        tb_agentenombre.ReadOnly = true;
        //tb_contacto.Text=Page.Server.HtmlDecode(row.Cells[2].Text);
    }
    protected void gv_detalle_DataBound(object sender, EventArgs e)
    {
        decimal subtotal = 0;
        decimal impuesto = 0;
        decimal total = 0;
        decimal totalD = 0;
        double afecto = 0;
        double noafecto = 0;

        user = (UsuarioBean)Session["usuario"];
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
            //if (lb_contribuyente.SelectedValue.Equals("2"))
            //{  //significa que si es contribuyente
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
            if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))//si debe cobrar iva y el rubro no esta en dolares y no es excento
            {
                /*
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
                 */
                afecto += rubtemp.rubroSubTot;
            }
            else
            {
                //rubtemp.rubroSubTot = rubtemp.rubroTot;
                noafecto += rubtemp.rubroSubTot;
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
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("TOTALD");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
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
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb7.Text, lb4.Text, lb5.Text, lb6.Text };
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
            //string paisISO = Utility.InttoISOPais(user.PaisID);
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio.Text.Trim(), user.pais.ISO, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("RO ADUANAS"))
        {
            DataSet ds = DB.getRO_Aduanas(user, tb_criterio.Text.Trim());
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("RO SEGUROS"))
        {
            DataSet ds = DB.getRO_Seguros(user, tb_criterio.Text.Trim());
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void dgw1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Seleccionar")
        {
            int index = Convert.ToInt32(e.CommandArgument);

            if (lb_tipo.SelectedValue.Equals("RO SEGUROS") || lb_tipo.SelectedValue.Equals("RO ADUANAS"))
            {
                tb_routing.Text = dgw1.Rows[index].Cells[4].Text;
            }
            else
            {
                tb_hbl.Text = dgw1.Rows[index].Cells[2].Text;
                tb_mbl.Text = dgw1.Rows[index].Cells[3].Text;
                tb_routing.Text = dgw1.Rows[index].Cells[4].Text;
                tb_contenedor.Text = dgw1.Rows[index].Cells[5].Text;
            }
        }
    }
    protected void dgw1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw1.DataSource = dt1;
        dgw1.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        dgw1.DataBind();
        tb_cuenta_ModalPopupExtender.Show();
    }

    private DataTable llenoDataTable(Hashtable ht)
    {
        user = (UsuarioBean)Session["usuario"];
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("TOTALD");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        if (ht != null && ht.Count > 0)
        {
            ICollection valueColl = ht.Values;
            Rubros rubtemp = new Rubros();
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            foreach (Rubros rub in valueColl)
            {
                rubtemp = (Rubros)rub;
                rubtemp = (Rubros)DB.ExistRubroPais(rub, user.PaisID);
                if (rubtemp == null)
                {
                    WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                    return null;
                }
                if ((rubtemp.CobIva == 1) && (rubtemp.rubroMoneda != 1) && (tipo_contabilidad != 2)/* && (!lb_contribuyente.SelectedValue.Equals("1"))*/)//si debe cobrar iva y el rubro no esta en dolares y no es excento
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
                    rubtemp.rubroSubTot = rubtemp.rubroTot;
                }
                decimal tipoCambio = user.pais.TipoCambio;
                //if (lb_moneda.SelectedValue.Equals("8"))
                double totalD = rubtemp.rubroTot;
                if (rubtemp.rubroMoneda != 1)
                {
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto * (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot * (double)tipoCambio, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }

                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), "USD"/*rubtemp.rubroMoneda.ToString()*/, totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)") };
                dt.Rows.Add(obj);
            }
        }
        return dt;
    }

    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        FacturaBean factfinal = new FacturaBean();
        if (lb_serie_factura.Items.Count == 0)
        {
            WebMsgBox.Show("No existe serie definida para esta sucursal");
            return;
        }
        if (lb_serie_factura.SelectedValue == "0")
        {
            WebMsgBox.Show("Por favor seleccione la Serie a utilizar");
            return;
        }
        if (tb_agenteID.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar el Codigo de Usuario de Caja Chica");
            return;
        }
        if (tb_documento_serie.Text.Equals("")) {
            WebMsgBox.Show("Debe ingresar la serie del documento de cobro");
            return;
        }
        if (tb_documento_correlativo.Text.Equals("")) {
            WebMsgBox.Show("Debe ingresar el correlativo del documento de cobro");
            return;
        }
        if (tb_fechadoc.Text.Equals("")) {
            WebMsgBox.Show("Debe ingresar la fecha del documento");
            return;
        }
        if (tb_proveedorID.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar el codigo del proveedor");
            return;
        }
        if (tb_proveedorID.Text.Trim().Equals("0"))
        {
            WebMsgBox.Show("Debe ingresar el codigo del proveedor");
            return;
        }
        if (gv_detalle.Rows.Count == 0) {
            WebMsgBox.Show("Debe ingresar al menos 1 rubro");
            return;
        }
        if (bool.Parse(Rb_Documento.SelectedValue) == true)
        {
            if (tb_fecha_libro_compras.Text.Trim() == "")
            {
                WebMsgBox.Show("Debe especificar la Fecha del Libro de Compras");
                return;
            }
            factfinal.Fecha_Libro_Compras = DB.DateFormat(tb_fecha_libro_compras.Text);
            factfinal.Fiscal_No_Fiscal = bool.Parse(Rb_Documento.SelectedValue);
        }
        else
        {
            factfinal.Fecha_Libro_Compras = DB.getDateTimeNow().Substring(0, 10);
            factfinal.Fiscal_No_Fiscal = bool.Parse(Rb_Documento.SelectedValue);
        }
        user = (UsuarioBean)Session["usuario"];
        //*************************************************************BLOQUEO DE PERIODO*****************************************
        #region bloqueo periodo
        RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
        DateTime hoy = DateTime.Today; //fecha del dia de hoy
        int activo = bloqueo.intC3;
        #region formateo fechas
        DateTime fecha_doc = DateTime.Parse(DB.DateFormat(tb_fechadoc.Text.ToString().Substring(0, 10)));
        #endregion
        if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
        {
            WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0, 10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10));
            return;
        }
        #endregion
        
        int contribuyente = int.Parse(lb_contribuyente.SelectedValue);//excento contribuyente
        int moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue); //importacion exportacion
        int tipo_cobro = 1;//prepaid, collect
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());//fiscal o financiera
        int servicio = 0; //fcl, lcl, etc
        int tipopersona = 8;
        int transID = 8;//provision proveedores locales
        factfinal.Volumen = tb_documento_serie.Text;//Temporal para guardar la serie de la FactID quenos trae el agente
        factfinal.Correlativo = tb_documento_correlativo.Text.Trim();//Temporal para guardar el correlativo la FactID quenos trae el agente
        int fe_dia = int.Parse(tb_fechadoc.Text.Trim().Substring(3, 2));
        int fe_mes = int.Parse(tb_fechadoc.Text.Trim().Substring(0, 2));
        int fe_anio = int.Parse(tb_fechadoc.Text.Trim().Substring(6, 4));
        factfinal.Fecha_Emision = fe_anio.ToString() + "-" + fe_mes.ToString() + "-" + fe_dia.ToString();
        factfinal.Fecha_Hora = lb_fecha_hora.Text;
        factfinal.Fecha_Pago = factfinal.Fecha_Emision;
        factfinal.SubTot = double.Parse(tb_subtotal.Text);
        factfinal.Impuesto = double.Parse(tb_impuesto.Text);
        factfinal.Total = double.Parse(tb_total.Text);
        factfinal.Afecto = double.Parse(tb_afecto.Text);
        factfinal.Noafecto = double.Parse(tb_noafecto.Text);
        factfinal.SucID=user.SucursalID;
        factfinal.MonedaID = moneda;
        factfinal.tttID = int.Parse(drp_tipo_transaccion.SelectedValue);
        factfinal.UsuID = user.ID;
        factfinal.HBL = tb_hbl.Text;
        factfinal.MBL = tb_mbl.Text;
        factfinal.Contenedor = tb_contenedor.Text;
        factfinal.Routing = tb_routing.Text;
        factfinal.Serie = lb_serie_factura.SelectedItem.Text;
        factfinal.AgenteID = int.Parse(tb_agenteID.Text);
        factfinal.TotalDol = double.Parse(tb_totaldolares.Text);
        factfinal.FacturaCorr = tipo_cobro;//temportal para guardar el tipo de contabilidad
        factfinal.BLID = int.Parse(tb_proveedorID.Text); //Se usa este campo para guardar el ID del proveedor de caja chica
        factfinal.Nombre_Cliente = tb_agentenombre.Text;
        tb_contacto.Text = tb_proveedornombre.Text;
        factfinal.Comodity = tb_contacto.Text;
        factfinal.Observaciones = tb_observaciones.Text;
        factfinal.imp_exp = imp_exp;
        factfinal.TedID = 1;
        factfinal.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
        factfinal.Contribuyente = contribuyente;
        //bool validar_restriccion = Validar_Restricciones("bt_guardar");
        //if (validar_restriccion == true)
        //{
            factfinal.TedID = 5;
            factfinal.Usu_Acepta = user.ID;
            factfinal.Fecha_Acepta = DB.getDateTimeNow().Substring(0, 10);
        //}
        //else
        //{
        //    factfinal.TedID = 1;
        //    factfinal.Usu_Acepta = "";
        //    factfinal.Fecha_Acepta = null;
        //}
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        Rubros rubro;
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
            servicio = rubro.rubroTypeID;
            factfinal.ShipperID = servicio;//se usa este campo para identificar el servicio LCL,FCL, etc
            rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, rubro.rubroTypeID);
            if ((rubro.cta_debe == null || rubro.cta_debe.Count <= 0) && (rubro.cta_haber == null || rubro.cta_haber.Count <= 0))
            {
                WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de cobro que esta realizando, por favor pongase en contacto con el Contador");
                return;
            }

            if (factfinal.RubrosArr == null) factfinal.RubrosArr = new ArrayList();
            factfinal.RubrosArr.Add(rubro);
        }
        factfinal.MonedaID = moneda;
        if (factfinal.RubrosArr == null || factfinal.RubrosArr.Count == 0)
        {
            WebMsgBox.Show("Debe tener rubros para facturar");
            return;
        }
        

        int matOpID = DB.getMatrizOperacionID(int.Parse(drp_tipo_transaccion.SelectedValue), moneda, user.PaisID, tipo_contabilidad);
        ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID, "Abono");


        string Check_Existencia = DB.CheckExistDoc(factfinal.Fecha_Hora, 5);
        if (Check_Existencia == "0")
        {
            ArrayList result = DB.insertAgenteProvision(factfinal, user, tipo_contabilidad, tipopersona, ctas_cargo, transID);
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
                WebMsgBox.Show("La Provision de Caja Chica fue grabada y autorizada exitosamente con Serie.: " + lb_serie_factura.SelectedItem.Text + " y Correlativo.: " + result[1].ToString());
                bt_guardar.Enabled = false;
                tb_corr.Text = result[1].ToString();
                gv_detalle.Enabled = false;
                bt_agregar.Enabled = false;
                lb_contribuyente.Enabled = false;
                lb_imp_exp.Enabled = false;
                drp_tipo_transaccion.Enabled = false;
                bt_agregar_factura.Enabled = true;
            }
            else
            {
                WebMsgBox.Show("Existió un problema al grabar la información, por favor intente de nuevo");
            }
        }
        else
        {
            bt_guardar.Enabled = false;
            return;
        }
    }


    protected void bt_agregar_factura_Click(object sender, EventArgs e)
    {
        //drp_tipo_transaccion.Enabled = false;
        //lb_imp_exp.Enabled = false;
        //lb_contribuyente.Enabled = false;
        lb_fecha_hora.Text = DB.getDateTimeNow();
        tb_corr.Text = string.Empty;
        
        tb_documento_serie.Text = string.Empty;
        tb_documento_correlativo.Text = string.Empty;
        tb_fechadoc.Text = string.Empty;

        tb_hbl.Text = string.Empty;
        tb_mbl.Text = string.Empty;
        tb_routing.Text = string.Empty;
        tb_contenedor.Text = string.Empty;

        tb_fecha_libro_compras.Text = string.Empty;

        gv_detalle.Enabled = true;
        gv_detalle.DataSource = null;
        gv_detalle.DataBind();
        bt_agregar.Enabled = true;

        tb_subtotal.Text = string.Empty;
        tb_impuesto.Text = string.Empty;
        tb_total.Text = string.Empty;
        tb_totaldolares.Text = string.Empty;
        tb_noafecto.Text = string.Empty;

        bt_guardar.Enabled = true;
        bt_agregar_factura.Enabled = true;

        gv_detalle_partida.DataSource = null;
        gv_detalle_partida.DataBind();
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
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("TOTALD");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
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
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb7.Text, lb4.Text, lb5.Text, lb6.Text };
            dt.Rows.Add(objArr);
        }


        //halo el rubro y luego le calculo sus chingaderas
        Rubros rubro = new Rubros();
        rubro.rubroID = long.Parse(lb_rubro.SelectedValue);
        rubro.rubroName = lb_rubro.SelectedItem.Text;
        rubro.rubroMoneda = long.Parse(lb_moneda2.SelectedValue);

        //DEFINIR TIPO DE SERVICIO
        #region Backup
        //if (lb_servicio.SelectedItem.Text.Equals("FCL")) { rubro.rubtoType = "FCL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("LCL")) { rubro.rubtoType = "LCL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("AEREO")) { rubro.rubtoType = "AEREO"; }
        //if (lb_servicio.SelectedItem.Text.Equals("APL")) { rubro.rubtoType = "APL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("TRANSPORTE T")) { rubro.rubtoType = "TRANSPORTE T"; }
        //if (lb_servicio.SelectedItem.Text.Equals("SEGUROS")) { rubro.rubtoType = "SEGUROS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("PUERTOS")) { rubro.rubtoType = "PUERTOS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("APL LOGISTICS")) { rubro.rubtoType = "APL LOGISTICS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("ADUANAS")) { rubro.rubtoType = "ADUANAS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("ALMACENADORA")) { rubro.rubtoType = "ALMACENADORA"; }
        //if (lb_servicio.SelectedItem.Text.Equals("INSPECTOR")) { rubro.rubtoType = "INSPECTOR"; }
        //if (lb_servicio.SelectedItem.Text.Equals("PO BOX")) { rubro.rubtoType = "PO BOX"; }
        //if (lb_servicio.SelectedItem.Text.Equals("ADMINISTRATIVO")) { rubro.rubtoType = "ADMINISTRATIVO"; }
        //if (lb_servicio.SelectedItem.Text.Equals("TERCEROS")) { rubro.rubtoType = "TERCEROS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("INTERMODAL")) { rubro.rubtoType = "INTERMODAL"; }
        #endregion
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
            //rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
        }
        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
        {
            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto / (double)tipoCambio, 2);
            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot / (double)tipoCambio, 2);
            //rubtemp.rubroTot = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
        }
        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
        {
            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
        }

        string simbolomoneda = "";
        if (rubtemp.rubroMoneda == 1) simbolomoneda = "GTQ";
        if (rubtemp.rubroMoneda == 2) simbolomoneda = "SVC";
        if (rubtemp.rubroMoneda == 3) simbolomoneda = "HNL";
        if (rubtemp.rubroMoneda == 4) simbolomoneda = "NIC";
        if (rubtemp.rubroMoneda == 5) simbolomoneda = "CRC";
        if (rubtemp.rubroMoneda == 6) simbolomoneda = "PAB";
        if (rubtemp.rubroMoneda == 7) simbolomoneda = "BZD";
        if (rubtemp.rubroMoneda == 8) simbolomoneda = "USD";

        //object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)") };
        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)") };
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
        user = (UsuarioBean)Session["usuario"];
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

    protected void bt_buscar2_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        /*
        if (!tb_nitb2.Text.Trim().Equals("") && tb_nitb2.Text != null) where += " rtrim(nit) like '%" + tb_nitb2.Text.Trim() + "%'";
        if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null)
        {
            if (where == "")
            {
                where += " upper(rtrim(nombre)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            }
            else
            {
                where += " and upper(rtrim(nombre)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            }
        }
        Arr = Utility.getProveedor("XNitNombre", where);
        dt = (DataTable)Utility.fillGridView("ProveedorforOC", Arr);
        ViewState["proveedordt2"] = dt;
        gv_proveedor2.DataSource = dt;
        gv_proveedor2.DataBind();
        mpeSeleccion2.Show();
        */
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
        tb_proveedornit.Text = row.Cells[2].Text;
        tb_proveedornombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        tb_proveedorID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
        tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
        lb_contribuyente.SelectedValue = DB.getProveedorRegimen(4, tb_proveedorID.Text).ToString();
        //tb_proveedoremail.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
        //tb_proveedordireccion.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
        //tb_proveedortelefono.Text = Page.Server.HtmlDecode(row.Cells[6].Text);        
    }
    protected void gv_proveedor2_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
            ArrayList Arr = null;
            string where = "";
            //Arr = Utility.getProveedor("XNitNombre", where);
            //dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
            //ViewState["proveedordt"] = dt;
            //gv_proveedor.DataSource = dt;
            //gv_proveedor.DataBind();
        }
    }
    protected bool Validar_Restricciones(string sender)
    {
        bool resultado = false;
        bool paiR = DB.Validar_Pais_Restringido(user);
        if (paiR == true)
        {
            #region CONTABILIZAR AUTOMATICAMENTE LA PROVISION
            if ((DB.Validar_Restriccion_Activa(user, 5, 40) == true) && (sender == "bt_guardar"))
            {
                resultado = true;
            }
            #endregion
        }
        return resultado;
    }
    protected void lb_serie_factura_SelectedIndexChanged(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
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
                    Actualizar_Moneda_Rubros();
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
        dt.Columns.Add("TOTALD");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
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

            //halo el rubro y luego le calculo sus chingaderas
            Rubros rubro = new Rubros();
            rubro.rubroID = long.Parse(lb1.Text);
            rubro.rubroName = lb2.Text;
            rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
            rubro.rubtoType = lb3.Text;
            rubro.rubroTot = double.Parse(lb6.Text);
            user = (UsuarioBean)Session["usuario"];
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

            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)") };
            dt.Rows.Add(obj);
        }
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }

}
