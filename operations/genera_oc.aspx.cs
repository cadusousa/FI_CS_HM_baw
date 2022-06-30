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

public partial class operations_GeneraOC : System.Web.UI.Page
{
    DataTable dt = null;
    int estadoDoc = 0;
    UsuarioBean user = null;
    int bandera = 0; //si bloquear campos o no (default:0 esta libre, con 1 bloquea)
    public string simbolomoneda = "";

    void Page_PreInit(object sender, EventArgs e)
    {
        if ((Request.QueryString["id"] != null) && (!Request.QueryString["id"].ToString().Equals("")))
        {
            int id = int.Parse(Request.QueryString["id"].ToString());
            if (id > 0)
            {
                MasterPageFile = "~/operations/blank.master";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            lb_fecha_hora.Text = DB.getDateTimeNow();
        
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!user.Aplicaciones.Contains("6"))
        {
            Response.Redirect("index.aspx");
        }
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());

        if (!((permiso & 2) == 2) && !((permiso & 4) == 4) && !((permiso & 8) == 8) && !((permiso & 16) == 16))
        {
            Response.Redirect("index.aspx");
        }
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        if (!Page.IsPostBack)
        {
            obtengo_listas(tipo_contabilidad, user.PaisID, user);
            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 7);
            if (tipo_contabilidad == 2)
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
        }
        tb_fecha.Text = DateTime.Now.ToShortDateString();
        #region Cargar Informacion Orden de Compra
        if ((Request.QueryString["id"] != null) && (!Request.QueryString["id"].ToString().Equals("")))
        {
            int id = int.Parse(Request.QueryString["id"].ToString());
            if (id > 0)
            {
                //Quita el boton para ya no agregar rubros
                bt_agregar.Enabled = false;
                lbl_tocid.Text = id.ToString();
                RE_GenericBean rgb = (RE_GenericBean)DB.getOCData(id);
                if ((rgb.intC11 == 5) || (rgb.intC11 == 7))
                {
                    pnl_provision.Visible = true;
                    lbl_provision_serie.Text = rgb.strC14;
                    lbl_provision_correlativo.Text = rgb.strC15;
                    Bloquear_Pantalla();
                }
                if (rgb.intC11 == 5)
                {
                    WebMsgBox.Show("La orden de compra ya se encuentra autorizada, por lo tanto no puede realizar ninguna modificacion");
                    tb_cancelar.Enabled = false;
                }
                if (rgb.intC11 == 7)
                {
                    #region Mostar Partida Contable
                    gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(rgb.strC16), 5, 0);
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
                    #endregion
                    WebMsgBox.Show("La orden de compra ya se encuentra provisionada, por lo tanto no puede realizar ninguna modificacion");
                    tb_cancelar.Enabled = false;
                }
                estadoDoc = rgb.intC11;
                lb_estadodoc.Text = estadoDoc.ToString();
                DataTable rubdt = (DataTable)DB.getRubbyOC(id, 7);
                gv_detalle.DataSource = rubdt;
                gv_detalle.DataBind();
                if (rgb == null)
                {
                    WebMsgBox.Show("No existe una orden de compra con ese identificador, por favor revisar.");
                    return;
                }
                bt_grabar.Enabled = false;
                if (estadoDoc == 1 || estadoDoc == 5)
                {
                    chk_draft.Enabled = false;
                }
                if (estadoDoc == 8)
                {
                    bt_aceptar.Visible = false;
                }
                else
                {
                    ArrayList arri = new ArrayList();
                    arri = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
                    for (int a = 0; a < arri.Count; a++)
                    {
                        PerfilesBean cbean = (PerfilesBean)arri[a];
                        if (cbean.ID == 14)
                        {
                            bt_aceptar.Visible = true;
                            tb_cancelar.Visible = true;
                        }
                    }
                }
                ArrayList cliente_arr = (ArrayList)DB.getProveedor("numero=" + rgb.intC3, "");
                RE_GenericBean cliente_rgb = (RE_GenericBean)cliente_arr[0];
                lb_corr.Text = rgb.intC2.ToString();
                lb_pais.Text = rgb.intC7.ToString();
                lb_suc.Text = rgb.intC8.ToString();
                tb_proveedornombre.Text = cliente_rgb.strC2;
                tb_proveedorcontacto.Text = cliente_rgb.strC6;
                tb_proveedoremail.Text = cliente_rgb.strC9;
                lb_contribuyente.SelectedValue = rgb.intC12.ToString();
                tb_terminos.Text = rgb.strC13;
                tb_proveedorID.Text = cliente_rgb.intC1.ToString();
                tb_proveedoremail.Text = cliente_rgb.strC9;
                tb_proveedordireccion.Text = cliente_rgb.strC5;
                tb_proveedortelefono.Text = cliente_rgb.strC7;
                lb_prioridad.SelectedValue = rgb.intC5.ToString();
                lb_departamento_interno.SelectedValue = rgb.intC4.ToString();
                tb_fecha.Text = rgb.strC2;

                lb_serie_factura.Items.Clear();
                lb_serie_factura.Items.Add(rgb.strC1.ToString());
                lb_serie_factura.SelectedValue = rgb.strC1.ToString();
                lb_serie_factura.Enabled = false;

                tb_descripcion_oc.Text = rgb.strC3;
                tb_contenedor.Text = rgb.strC5;
                tb_routing.Text = rgb.strC4;
                tb_hbl.Text = rgb.strC6;
                tb_mbl.Text = rgb.strC11;
                tb_total.Text = rgb.decC1.ToString();
                lb_moneda.SelectedValue = rgb.intC6.ToString();
                tb_observaciones.Text = rgb.strC7;
                lbl_blID.Text = rgb.Blid.ToString();
                lbl_tipoOperacionID.Text = rgb.Tipo_Operacion.ToString();
                if (rgb.intC11 == 1 || rgb.intC11 == 5)
                    bt_aceptar.Enabled = true;
                if (rgb.intC11 == 8)
                {
                    chk_draft.Checked = true;
                    bt_grabar.Enabled = true;
                }
                tb_cotizacion.Text = rgb.strC12;
                lb_solicita.Text = rgb.strC9;
                lb_autoriza.Text = rgb.strC10;
                lb_imp_exp.SelectedValue = rgb.intC10.ToString();

                if (!Page.IsPostBack)
                {
                    tb_serie_proveedor.Text = rgb.strC17;
                    tb_correlativo_proveedor.Text = rgb.strC18;
                    tb_fecha_proveedor.Text = rgb.strC19;
                }
            }
        }
        #endregion
        ArrayList arr = null;
        ListItem item = null;
        //funcion bloqueadora si esta autorizada la OC y la Provision
        if (Request.QueryString["id"] != null)
        {
            bandera = DB.bloquea_OC(int.Parse(Request.QueryString["id"].ToString()));
            if (bandera == 1)
            {
                gv_detalle.Columns[0].Visible = false;
                bt_agregar.Visible = false;
                bt_aceptar.Enabled = false;
            }
            else if (bandera == 0)
            {
                gv_detalle.Columns[0].Visible = true;
                bt_agregar.Visible = true;
                bt_aceptar.Enabled = true;
            }
        }
        if ((estadoDoc == 5) || (estadoDoc == 3))
        {
            //Ya esta Autorizada la OC
            bt_grabar.Enabled = false;
            bt_aceptar.Enabled = false;
            tb_cancelar.Enabled = false;
            bt_agregar.Enabled = false;
            gv_detalle.Enabled = false;
            tb_descripcion_oc.Enabled = false;
            tb_observaciones.Enabled = false;
            tb_terminos.Enabled = false;
            tb_proveedorID.Enabled = false;
            lb_departamento_interno.Enabled = false;
            lb_prioridad.Enabled = false;
            if (estadoDoc == 3)
            {
                btn_imprimir.Enabled = false;
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

        #region Manejar Estados
        int tedID = 0;
        int tocID = 0;
        if (Request.QueryString["id"] != null)
        {
            tocID = int.Parse(Request.QueryString["id"].ToString());
        }
        tedID = int.Parse(lb_estadodoc.Text);
        if ((tocID == 0) && (tedID == 0))
        {
            bt_grabar.Visible = true;
            bt_aceptar.Visible = false;
            tb_cancelar.Visible = false;
            btn_imprimir.Visible = false;

        }
        if ((tocID > 0) && (tedID == 1))
        {
            bt_grabar.Visible = false;
            bt_aceptar.Visible = true;
            tb_cancelar.Visible = true;
            btn_imprimir.Visible = true;
            lb_departamento_interno.Enabled = false;
        }
        if ((tocID > 0) && (tedID == 8))
        {
            bt_grabar.Visible = true;
            bt_aceptar.Visible = false;
            tb_cancelar.Visible = true;
            btn_imprimir.Visible = true;
            lb_departamento_interno.Enabled = false;
        }
        if ((tocID > 0) && (tedID == 7))
        {
            bt_grabar.Visible = false;
            bt_aceptar.Visible = false;
            tb_cancelar.Visible = false;
            btn_imprimir.Visible = true;
            lb_departamento_interno.Enabled = false;
        }
        #endregion
    }
    private void obtengo_listas(int tipoconta, int paiID, UsuarioBean user)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getMonedasbyPais(paiID, user.contaID);
            item = new ListItem("Seleccione", "0");
            lb_moneda.Items.Add(item);
            lb_moneda2.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
                lb_moneda2.Items.Add(item);
            }
            
            //arr = null;
            //arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 7, user,0);//1 porque es el tipo de documento para facturacion
            //foreach (string valor in arr)
            //{
            //    item = new ListItem(valor, valor);
            //    lb_serie_factura.Items.Add(item);
            //}

            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(7, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serie_factura.Items.Clear();
            lb_serie_factura.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serie_factura.Items.Add(item);
            }

            arr = null;
            if (DB.Validar_Restriccion_Activa(user, 7, 24) == true)
            {
                arr = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(user.PaisID, user.contaID, 7, user.SucursalID);
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
            lb_servicio.SelectedIndex = 1;
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
            arr = null;
            arr = user.Departamento;
            foreach (RE_GenericBean rgb in arr) {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_departamento_interno.Items.Add(item);
            }
        }
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
        tb_proveedornombre.Text =Page.Server.HtmlDecode(row.Cells[3].Text);
        tb_proveedorID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
        tb_proveedoremail.Text=Page.Server.HtmlDecode(row.Cells[6].Text);
        tb_proveedordireccion.Text=Page.Server.HtmlDecode(row.Cells[7].Text);
        tb_proveedortelefono.Text = Page.Server.HtmlDecode(row.Cells[8].Text);
        lb_contribuyente.SelectedValue = DB.getProveedorRegimen(4, tb_proveedorID.Text.Trim()).ToString();
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
    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += " numero="+tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += " nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        

        ViewState["proveedordt"] = dt;
        gv_proveedor.DataSource = dt;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
    protected void bt_grabar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        int permiso_modificar = 0;
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        foreach (PerfilesBean Perfil in Arr_Perfiles)
        {
            if ((Perfil.ID == 15))
            {
                permiso_modificar = 1;
            }
        }

        if (permiso_modificar == 0)
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }


        if (lb_serie_factura.Items.Count == 0)
        {
            WebMsgBox.Show("No se puede Generar una Orden de Compra sin serie");
            return;
        }
        if (lb_serie_factura.SelectedValue == "0")
        {
            WebMsgBox.Show("Por favor seleccione la Serie a utilizar");
            return;
        }
        if ((tb_proveedorID.Text == "") || (tb_proveedorID.Text == "0") || (tb_proveedornombre.Text == ""))
        {
            WebMsgBox.Show("Debe especificar un proveedor");
            return;
        }
        bool Requiere_OC = DB.requiereOC(int.Parse(tb_proveedorID.Text));
        if (Requiere_OC == false)
        {
            WebMsgBox.Show("El proveedor con codigo.: " + tb_proveedorID.Text + " no se encuentra dentro del listado proveedores permitidos para realizar Orden de Compra, porfavor comuniquese con el personal de Contabilidad. ");
            return;
        }
        if (tb_descripcion_oc.Text == "")
        {
            WebMsgBox.Show("Debe ingresar la Descripcion de bienes o servicios");
            return;
        }
        if ((tb_total.Text == "") || (gv_detalle.Rows.Count == 0))
        {
            WebMsgBox.Show("Debe Indicar al menos un Rubro.");
            return;
        }
        if ((lb_imp_exp.SelectedItem.Text == "IMPORTACION") || (lb_imp_exp.Text == "EXPORTACION"))
        {
            if (tb_mbl.Text == "")
            {
                WebMsgBox.Show("Debe ingresar MBL.");
                return;
            }
        }
        if (lb_departamento_interno.Items.Count == 0)
        {
            WebMsgBox.Show("Estimado usuario usted no cuenta con ningun Departamento asignado para poder registrar Ordenes de Compra.");
            return;
        }

        RE_GenericBean Bean_OC = Capturar_Data_Orden_Compra();
        if (Bean_OC == null)
        {
            WebMsgBox.Show("Existio un error al momento de obtener la informacion de la Orden de Compra.");
            return;
        }
        int tocID = 0;
        if (Request.QueryString["id"] != null)
        {
            tocID = int.Parse(Request.QueryString["id"].ToString());
        }
        ArrayList resultado = null;
        #region Definir Estado y Transaccion
        Bean_OC.boolC1 = chk_draft.Checked;
        if ((tocID == 0) && (Bean_OC.boolC1 == true))
        {
            Bean_OC.intC6 = 8;
            string Check_Existencia = DB.CheckExistDoc(Bean_OC.strC18, 7);
            if (Check_Existencia == "0")
            {
                resultado = DB.Insertar_Orden_Compra(user, Bean_OC);
            }
            else
            {
                Bloquear_Pantalla();
                WebMsgBox.Show("La Orden de Compra ya fue Guardada.");
                return;
            }
        }
        else if ((tocID == 0) && (Bean_OC.boolC1 == false))
        {
            Bean_OC.intC6 = 1;
            string Check_Existencia = DB.CheckExistDoc(Bean_OC.strC18, 7);
            if (Check_Existencia == "0")
            {
                resultado = DB.Insertar_Orden_Compra(user, Bean_OC);
            }
            else
            {
                Bloquear_Pantalla();
                WebMsgBox.Show("La Orden de Compra ya fue Guardada.");
                return;
            }
        }
        else if ((tocID > 0))
        {
            if (Bean_OC.boolC1 == true)
            {
                Bean_OC.intC6 = 8;
                resultado = DB.Actualizar_Orden_Compra(user, Bean_OC, tocID, null);
            }
            else if (Bean_OC.boolC1 == false)
            {
                Bean_OC.intC6 = 1;
                resultado = DB.Actualizar_Orden_Compra(user, Bean_OC, tocID, null);
            }
        }
        #endregion
        if (resultado == null)
        {
            WebMsgBox.Show("Existio un error al momento de guardar la Orden de Compra, porfavor intente mas tarde.");
            return;
        }
        else
        {
            WebMsgBox.Show("La Orden de Compra Serie.: " + resultado[1].ToString() + " y Correlativo.: " + resultado[2].ToString() + " fue guardada exitosamente.");
            lb_corr.Text = resultado[2].ToString();
            Bloquear_Pantalla();
            return;
        }
    }
    protected void tb_cancelar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        int permiso_modificar = 0;
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        foreach (PerfilesBean Perfil in Arr_Perfiles)
        {
            if ((Perfil.ID == 14))
            {
                permiso_modificar = 1;
            }
        }
        if (permiso_modificar == 0)
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }

        /*int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 16) == 16))
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }*/
        if (Request.QueryString["id"] == null)
        {
            WebMsgBox.Show("No se puede Obtener la informacion de la Orden de Compra", WebMsgBox.TipoMensaje.SalirVentanaActual);
            bt_grabar.Enabled = false;
            bt_aceptar.Enabled = false;
            tb_cancelar.Enabled = false;
            btn_imprimir.Enabled = false;
            return;
        }
        int tocID = int.Parse(Request.QueryString["id"].ToString());
        int tedID = int.Parse(lb_estadodoc.Text);
        if (lb_serie_factura.Items.Count == 0)
        {
            WebMsgBox.Show("La Orden de Compra no tiene Serie asignada", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        if ((tb_proveedorID.Text == "") || (tb_proveedorID.Text == "0") || (tb_proveedornombre.Text == ""))
        {
            WebMsgBox.Show("Debe especificar un proveedor", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        if (tb_descripcion_oc.Text == "")
        {
            WebMsgBox.Show("Debe ingresar la Descripcion de bienes o servicios", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        if ((tb_total.Text == "") || (gv_detalle.Rows.Count == 0))
        {
            WebMsgBox.Show("Debe Indicar al menos un Rubro.", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        RE_GenericBean Bean_OC = Capturar_Data_Orden_Compra();
        if (Bean_OC == null)
        {
            WebMsgBox.Show("Existio un error al momento de obtener la informacion de la Orden de Compra.", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        ArrayList resultado = null;
        Bean_OC.intC6 = 3;
        int estado_tiempo_real = 0;
        estado_tiempo_real = DB.getEstadoDocumento(tocID, 7);
        if ((estado_tiempo_real == 8) || (estado_tiempo_real == 1))
        {
            resultado = DB.Actualizar_Orden_Compra(user, Bean_OC, tocID, null);
        }
        else
        {
            if (estado_tiempo_real == 5)
            {
                WebMsgBox.Show("La Orden de Compra ya fue autorizada", WebMsgBox.TipoMensaje.SalirVentanaActual);
                Bloquear_Pantalla();
                return;
            }
            else if (estado_tiempo_real == 7)
            {
                WebMsgBox.Show("La Orden de Compra ya fue provisionada", WebMsgBox.TipoMensaje.SalirVentanaActual);
                Bloquear_Pantalla();
                return;
            }
            else if (estado_tiempo_real == 3)
            {
                WebMsgBox.Show("La Orden de Compra ya fue rechazada", WebMsgBox.TipoMensaje.SalirVentanaActual);
                Bloquear_Pantalla();
                return;
            }
        }
        if (resultado == null)
        {
            WebMsgBox.Show("Existio un error al tratar de Rechazar la Orden de Compra, porfavor intente mas tarde.", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        else
        {
            WebMsgBox.Show("La Orden de Compra Serie.: " + Bean_OC.strC1 + " y Correlativo.: " + Bean_OC.intC2.ToString() + " fue rechazada exitosamente.", WebMsgBox.TipoMensaje.SalirVentanaActual);
            //lb_corr.Text = resultado.ToString();
            Bloquear_Pantalla();
            return;
        }
    }

    protected void bt_aceptar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        int permiso_modificar = 0;
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        foreach (PerfilesBean Perfil in Arr_Perfiles)
        {
            if ((Perfil.ID == 14))
            {
                permiso_modificar = 1;
            }
        }
        if (permiso_modificar == 0)
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }

        /*int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 8) == 8))
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }*/
        if (Request.QueryString["id"] == null)
        {
            WebMsgBox.Show("No se puede Obtener la informacion de la Orden de Compra", WebMsgBox.TipoMensaje.SalirVentanaActual);
            bt_grabar.Enabled = false;
            bt_aceptar.Enabled = false;
            tb_cancelar.Enabled = false;
            btn_imprimir.Enabled = false;
            return;
        }
        int tocID = int.Parse(Request.QueryString["id"].ToString());
        int tedID = int.Parse(lb_estadodoc.Text);
        if (lb_serie_factura.Items.Count == 0)
        {
            WebMsgBox.Show("No se puede Generar una Orden de Compra sin serie", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        if ((tb_proveedorID.Text == "") || (tb_proveedorID.Text == "0") || (tb_proveedornombre.Text == ""))
        {
            WebMsgBox.Show("Debe especificar un proveedor", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        bool Requiere_OC = DB.requiereOC(int.Parse(tb_proveedorID.Text));
        if (Requiere_OC == false)
        {
            WebMsgBox.Show("El proveedor con codigo.: " + tb_proveedorID.Text + " no se encuentra dentro del listado proveedores permitidos para realizar Orden de Compra, porfavor comuniquese con el personal de Contabilidad. ", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        if (tb_descripcion_oc.Text == "")
        {
            WebMsgBox.Show("Debe ingresar la Descripcion de bienes o servicios", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        if ((tb_total.Text == "") || (gv_detalle.Rows.Count == 0))
        {
            WebMsgBox.Show("Debe Indicar al menos un Rubro.", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        if ((lb_imp_exp.SelectedItem.Text == "IMPORTACION") || (lb_imp_exp.Text == "EXPORTACION"))
        {
            if (tb_mbl.Text == "")
            {
                WebMsgBox.Show("Debe ingresar MBL.", WebMsgBox.TipoMensaje.SalirVentanaActual);
                return;
            }
            //else if (tb_hbl.Text == "")
            //{
            //    WebMsgBox.Show("Debe ingresar HBL.");
            //    return;
            //}
        }
        RE_GenericBean Bean_OC = Capturar_Data_Orden_Compra();
        if (Bean_OC == null)
        {
            WebMsgBox.Show("Existio un error al momento de obtener la informacion de la Orden de Compra.", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        Bean_Provision_Automatica Provision_Automatica = null;
        Provision_Automatica = Construir_Provision(Bean_OC);
        if (Provision_Automatica == null)
        {
            //WebMsgBox.Show("Existio un error al Construir la Provision Automatica");
            return;
        }
        //bool validar_restriccion = Validar_Restricciones("bt_aceptar");
        //if (validar_restriccion == true)
        //{
        //if (!string.IsNullOrEmpty(Provision_Automatica.tpr_fact_id) &&
        //    !string.IsNullOrEmpty(Provision_Automatica.tpr_fact_corr) &&
        //    !string.IsNullOrEmpty(Provision_Automatica.tpr_fact_fecha))
        //{
            Bean_OC.intC6 = 7;
            Provision_Automatica.tpr_ted_id = 5;
            Provision_Automatica.tpr_usu_acepta = user.ID;
            Provision_Automatica.tpr_fecha_acepta = DB.getDateTimeNow().Substring(0, 10);
            Provision_Automatica.tpr_ttd_id = 3;
            Provision_Automatica.tpr_ttt_id = 8;
        //}
        //else
        //{
        //    Bean_OC.intC6 = 5;
        //    Provision_Automatica.tpr_ted_id = 1;
        //    Provision_Automatica.tpr_usu_acepta = "";
        //    Provision_Automatica.tpr_fecha_acepta = null;
        //    Provision_Automatica.tpr_ttd_id = 3;
        //    Provision_Automatica.tpr_ttt_id = 8;
        //}

        if ((Bean_OC.intC6 == 5) || (Bean_OC.intC6 == 7))
        {
            Bean_OC.strC11 = user.ID;//toc_usuario_acepta
            Bean_OC.strC16 = DB.getDateTimeNow().Substring(0, 10);//toc_fecha_autoriza
            Bean_OC.strC17 = DB.getDateTimeNow().Substring(11, 8);//toc_hora_autoriza
        }
        #region Validar Serie de Provision Automatica
        if (Provision_Automatica.tpr_serie.Trim() == "")
        {
            SucursalBean SucursalBEAN = (SucursalBean)DB.getSucursal(user.SucursalID);
            WebMsgBox.Show("No existe serie de Provisiones para Autorizar la Orden de Compra en el Departamento.: " + SucursalBEAN.Nombre + "", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        #endregion
        int estado_tiempo_real = 0;
        ArrayList resultado = null;
        #region Definir Estado y Transaccion
        if (tocID > 0)
        {
            estado_tiempo_real = DB.getEstadoDocumento(tocID, 7);
            if (estado_tiempo_real == 1)
            {
                resultado = DB.Actualizar_Orden_Compra(user, Bean_OC, tocID, Provision_Automatica);
            }
            else
            {
                if (estado_tiempo_real == 5)
                {
                    WebMsgBox.Show("La Orden de Compra ya fue autorizada", WebMsgBox.TipoMensaje.SalirVentanaActual);
                    Bloquear_Pantalla();
                    return;
                }
                else if (estado_tiempo_real == 7)
                {
                    WebMsgBox.Show("La Orden de Compra ya fue provisionada", WebMsgBox.TipoMensaje.SalirVentanaActual);
                    Bloquear_Pantalla();
                    return;
                }
                else if (estado_tiempo_real == 3)
                {
                    WebMsgBox.Show("La Orden de Compra ya fue rechazada", WebMsgBox.TipoMensaje.SalirVentanaActual);
                    Bloquear_Pantalla();
                    return;
                }
            }
        }
        #endregion
        if (resultado == null)
        {
            WebMsgBox.Show("Existio un error al momento de Autorizar la Orden de Compra, porfavor intente mas tarde.", WebMsgBox.TipoMensaje.SalirVentanaActual);
            return;
        }
        else
        {
            pnl_provision.Visible = true;
            lbl_provision_serie.Text = resultado[4].ToString();
            lbl_provision_correlativo.Text = resultado[5].ToString();
            #region Mostrar Partida Contable
            if (Provision_Automatica.tpr_ted_id == 5)
            {
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(resultado[3].ToString()), 5, 0);
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
            }
            #endregion
            lb_corr.Text = resultado[2].ToString();
            //var resultadoMsg = AutorizarAutomaticamenteProvision(resultado[1].ToString(), resultado[2].ToString(), Provision_Automatica.tpr_mon_id);

            //if (Request.QueryString["id"] != null)
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "clos", "window.close();", true);
            //else
                WebMsgBox.Show(string.Format("La Orden de Compra y provision con Serie: {0} y Correlativo: {1} fue autorizadas exitosamente.", resultado[1].ToString(), resultado[2].ToString()), WebMsgBox.TipoMensaje.SalirVentanaActual);

            Bloquear_Pantalla();
            return;
        }
    }

    private string AutorizarAutomaticamenteProvision(string serie, string ordenCompra, int idMoneda )
    {
        var where = string.Format(" tpr_tcon_id={0} and tpr_pai_id={1} and tpr_mon_id={2} and tpr_serie_oc='{3}' and tpr_correlativo_oc={4}",
                                    user.contaID, user.PaisID, idMoneda.ToString(), serie, ordenCompra);
        var resultado = (ArrayList)DB.getProvisionesListbyOC(where);
        if (resultado != null && resultado.Count > 0)
        {
            var idProvision = ((RE_GenericBean)(resultado[0])).intC1;
            var urlPage = string.Format("provisiones.aspx?queriesting={0}&tipo=&provID={1}&provisionAutomatica=1", string.Empty, idProvision);
            var jsKey = "PopupScript";
            var cs = Page.ClientScript;
            string script = string.Format("window.open('{0}', '_blank', 'toolbar=0, status=0, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -550) + ', height=' + (window.screen.height -75)); window.opener.PostBackHijo();", urlPage);
            if (!cs.IsStartupScriptRegistered(this.GetType(), jsKey))
                cs.RegisterClientScriptBlock(this.GetType(), jsKey, script, true);
            return string.Empty;
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "postBackHijo", "window.opener.PostBackHijo()", true);
            return string.Format("{2}No se puede autorizar la provision porque no existe ninguna asociada a la orden de compra {0} - {1}.", 
                                    serie, ordenCompra, System.Environment.NewLine);
        }
    }

    protected void gv_detalle_DataBound(object sender, EventArgs e)
    {
        decimal subtotal = 0;
        decimal impuesto = 0;
        decimal total = 0;
        decimal totalD = 0;
        Label lb1, lb2, lb3, lb4, lb5;
        Button bt1;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_subtotal");
            lb2 = (Label)row.FindControl("lb_impuesto");
            lb3 = (Label)row.FindControl("lb_total");
            lb4 = (Label)row.FindControl("lb_totaldolares");
            lb5 = (Label)row.FindControl("lb_monedatipo");
            if (lb5.Text.Equals("1")) lb5.Text = "GTQ";
            if (lb5.Text.Equals("2")) lb5.Text = "SVC";
            if (lb5.Text.Equals("3")) lb5.Text = "HNL";
            if (lb5.Text.Equals("4")) lb5.Text = "NIC";
            if (lb5.Text.Equals("5")) lb5.Text = "CRC";
            if (lb5.Text.Equals("6")) lb5.Text = "PAB";
            if (lb5.Text.Equals("7")) lb5.Text = "BZD";
            if (lb5.Text.Equals("8")) lb5.Text = "USD";
                
            subtotal += Math.Round(decimal.Parse(lb1.Text), 2);
            impuesto += Math.Round(decimal.Parse(lb2.Text), 2);
            total += Math.Round(decimal.Parse(lb3.Text), 2);
            totalD += Math.Round(decimal.Parse(lb4.Text), 2);
        }
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
        tb_total2.Text = total.ToString("#,#.00#;(#,#.00#)");
        tb_impuesto.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_subtotal.Text = subtotal.ToString("#,#.00#;(#,#.00#)");
        tb_totaldolares.Text = totalD.ToString("#,#.00#;(#,#.00#)");
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
        if (decimal.Parse(tb_monto.Text) > 0)
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
            //// Si la moneda que estoy agregando es diferente a la que estoy facturando
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
            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)") };
            dt.Rows.Add(obj);

            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
        }
    }
    protected void lb_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        int servID = int.Parse(lb_servicio.SelectedValue);
        int impo_expo = int.Parse(lb_imp_exp.SelectedValue);

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
    protected void lb_moneda_SelectedIndexChanged(object sender, EventArgs e)
    {
        lb_moneda2.SelectedValue = lb_moneda.SelectedValue;
        lb_moneda2.Enabled = false;
        gv_detalle.DataSource = null;
        gv_detalle.DataBind();
    }

    private int GeneroVersionOC(int id, int estado) 
    {
        int moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue);//importacion exportacion
        int tipo_cobro = 1;//prepaid, collect
        if (imp_exp == 1) { tipo_cobro = 1; } else { tipo_cobro = 2; } //si es 1=importacion cobro 1=collect,si es 2=exportacion cobro 2=prepaid
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());//fiscal o financiera
        RE_GenericBean oc_rgb = new RE_GenericBean();
        oc_rgb.strC1 = lb_serie_factura.SelectedValue;
        oc_rgb.intC1 = int.Parse(tb_proveedorID.Text);
        oc_rgb.strC2 = tb_proveedornombre.Text;
        oc_rgb.intC2 = int.Parse(lb_prioridad.SelectedValue);
        oc_rgb.strC3 = tb_descripcion_oc.Text;
        oc_rgb.strC4 = tb_routing.Text;
        oc_rgb.strC5 = tb_contenedor.Text;
        oc_rgb.strC6 = tb_hbl.Text;
        oc_rgb.strC9 = tb_mbl.Text;
        oc_rgb.decC1 = decimal.Parse(tb_total.Text);
        oc_rgb.intC3 = int.Parse(lb_moneda.SelectedValue);
        oc_rgb.strC7 = tb_observaciones.Text;
        oc_rgb.strC8 = tb_terminos.Text;
        oc_rgb.intC4 = int.Parse(lb_departamento_interno.SelectedValue);
        oc_rgb.intC5 = tipo_contabilidad;
        oc_rgb.strC10 = tb_cotizacion.Text.Trim();
        oc_rgb.intC9 = int.Parse(lb_servicio.SelectedValue);
        oc_rgb.intC10 = int.Parse(lb_imp_exp.SelectedValue);
        oc_rgb.boolC1 = chk_draft.Checked;
        oc_rgb.boolC2 = DB.requiereOC(oc_rgb.intC1);
        if (lb_moneda.SelectedValue.Equals("8")) oc_rgb.decC2 = Math.Round((oc_rgb.decC1 / (decimal)user.pais.TipoCambio), 2);
        //recorro el datagrid para aramar el detalle de rubros de la orden de compra
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
            rubro.rubroTotD = Math.Round(double.Parse(lb7.Text) / 1, 2);
            rubro.rubroTypeID = Utility.TraducirServiciotoINT(rubro.rubtoType);

            if (oc_rgb.arr1 == null) oc_rgb.arr1 = new ArrayList();
            oc_rgb.arr1.Add(rubro);
        }
        if (oc_rgb.arr1 == null || oc_rgb.arr1.Count == 0)
        {
            WebMsgBox.Show("Debe tener rubros para generar orden de compra");
            return -100;
        }

        int result = DB.updateOCNuevaVersion(id, oc_rgb, user);
        return result;
    }
    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 32) == 32))
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }
        else
        {
            //int OCID = DB.getCorrelativoDoc(user.SucursalID, 7, lb_serie_factura.SelectedItem.Text,lb_corr.Text);
            int OCID = int.Parse(Request.QueryString["id"].ToString());
            String csname1 = "PopupScript";
            Type cstype = this.GetType();
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('../invoice/template_oc.aspx?id=" + OCID + "&tipo=7','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            #region Seteo de Parametros de Impresion
            user.ImpresionBean.Operacion = "1";
            user.ImpresionBean.Tipo_Documento = "7";
            user.ImpresionBean.Id = OCID.ToString();
            user.ImpresionBean.Impreso = true;
            Session["usuario"] = user;
            #endregion
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
            btn_imprimir.Enabled = false;
        }
    }
    protected void lb_rubro_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (lb_servicio.Items.Count > 0)
            {
                int servID = int.Parse(lb_servicio.SelectedValue);
                int impo_expo = int.Parse(lb_imp_exp.SelectedValue);

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
            }
        }
    }
    private void cargo_datos_BL()
    {
        #region Cargo Datos BL
        if ((lb_estadodoc.Text != "5") && (lb_estadodoc.Text != "7"))
        {
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
                    if (Request.QueryString["blid"] != null)
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
                    tb_contenedor.Text = rgb.strC4;
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
                    //tb_observaciones.Text = rgb.strC7.Trim();
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
                    //tb_observaciones.Text = rgb.strC7.Trim();
                    lb_imp_exp.Enabled = false;
                }
            }
        }
        #endregion
    }
    protected RE_GenericBean Capturar_Data_Orden_Compra()
    {
        #region Caputar Data Orden de Compra
        RE_GenericBean Bean_OC = null;
        try
        {
            int estado = int.Parse(lb_estadodoc.Text);
            int moneda = int.Parse(lb_moneda.SelectedValue);
            int imp_exp = int.Parse(lb_imp_exp.SelectedValue);
            int tipo_cobro = 1;//prepaid, collect
            if (imp_exp == 1) { tipo_cobro = 1; } else { tipo_cobro = 2; } //si es 1=importacion cobro 1=collect,si es 2=exportacion cobro 2=prepaid
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            Bean_OC = new RE_GenericBean();
            Bean_OC.intC1 = 0;//toc_id
            Bean_OC.strC1 = lb_serie_factura.SelectedItem.Text;//toc_serie
            Bean_OC.intC2 = int.Parse(lb_corr.Text);//toc_correlativo
            Bean_OC.intC3 = int.Parse(tb_proveedorID.Text);//toc_proveedor_id
            Bean_OC.strC2 = "";//toc_fecha
            Bean_OC.intC4 = int.Parse(lb_departamento_interno.SelectedValue.ToString());//toc_departamentoid
            Bean_OC.intC5 = int.Parse(lb_prioridad.SelectedValue);//toc_prioridad
            Bean_OC.strC3 = tb_descripcion_oc.Text;//toc_bienserv//Descripción de bienes o servicios//tb_descripcion_oc
            Bean_OC.strC4 = tb_routing.Text;//toc_routing
            Bean_OC.strC5 = tb_contenedor.Text;//toc_contenedor
            Bean_OC.strC6 = tb_hbl.Text;//toc_hbl
            Bean_OC.strC7 = tb_mbl.Text;//toc_mbl
            Bean_OC.strC8 = tb_observaciones.Text;//toc_observaciones
            Bean_OC.strC9 = tb_proveedornombre.Text;//toc_nombreordencompra
            Bean_OC.intC6 = 0;//toc_ted_id
            Bean_OC.strC10 = user.ID;//toc_usuario_creador
            Bean_OC.strC11 = "";//toc_usuario_acepta
            Bean_OC.intC7 = user.PaisID;//toc_pai_id
            Bean_OC.intC8 = user.SucursalID;//toc_suc_id
            Bean_OC.intC9 = int.Parse(lb_moneda.SelectedValue);//toc_moneda_id
            Bean_OC.intC10 = user.contaID;//toc_conta_id
            Bean_OC.strC12 = user.pais.TipoCambio.ToString();//toc_tipo_cambio
            Bean_OC.strC13 = tb_cotizacion.Text.Trim();//toc_cotizacion
            Bean_OC.intC11 = int.Parse(lb_servicio.SelectedValue);//toc_tts_id
            Bean_OC.intC12 = int.Parse(lb_imp_exp.SelectedValue);//toc_tie_id
            Bean_OC.strC14 = tb_proveedordireccion.Text.Trim();//toc_direccion
            Bean_OC.strC15 = tb_terminos.Text;//toc_terminos_pago
            Bean_OC.intC13 = int.Parse(lb_contribuyente.SelectedValue.ToString());//toc_tti_id
            Bean_OC.decC1 = decimal.Parse(tb_total.Text);//toc_total
            Bean_OC.decC2 = decimal.Parse(tb_totaldolares.Text);//toc_total_equivalente
            Bean_OC.decC3 = decimal.Parse(tb_impuesto.Text);//toc_iva
            Bean_OC.decC4 = decimal.Parse(tb_subtotal.Text);//toc_subtotal
            Bean_OC.Blid = int.Parse(lbl_blID.Text);//toc_blid
            Bean_OC.Tipo_Operacion = int.Parse(lbl_tipoOperacionID.Text);//toc_tto_id
            Bean_OC.boolC2 = DB.requiereOC(Bean_OC.intC1);
            Bean_OC.Fecha_Hora = lb_fecha_hora.Text;
            Bean_OC.strC16 = null;//toc_fecha_autoriza
            Bean_OC.strC17 = null;//toc_hora_autoriza
            Bean_OC.strC18 = lb_fecha_hora.Text;
            Bean_OC.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
            #region Eliminar Apostrofes en caso de que existan
            Bean_OC.strC3 = Bean_OC.strC3.Replace("'", "");
            Bean_OC.strC8 = Bean_OC.strC8.Replace("'", "");
            Bean_OC.strC15 = Bean_OC.strC15.Replace("'", "");
            #endregion
            if ((tb_serie_proveedor.Text.Trim() != "") && (tb_correlativo_proveedor.Text.Trim() != "") && (tb_fecha_proveedor.Text.Trim() != ""))
            {

                Bean_OC.strC19 = tb_serie_proveedor.Text.Trim();
                Bean_OC.strC20 = tb_correlativo_proveedor.Text.Trim();
                Bean_OC.strC21 = DB.DateFormat(tb_fecha_proveedor.Text.Trim());
                Bean_OC.strC22 = Bean_OC.strC21;

                DateTime aux_fecha_emision = DateTime.Parse(Bean_OC.strC22);
                double dias_credito = DB.getFechaMaxPago(Bean_OC.intC3, 4, user);
                DateTime aux_fecha_pago = aux_fecha_emision.AddDays(dias_credito);
                string fecha_pago_formateada = aux_fecha_pago.ToString();
                Bean_OC.strC22 = DateTime.Parse(fecha_pago_formateada).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);

            }
            else if ((tb_serie_proveedor.Text.Trim() != "") || (tb_correlativo_proveedor.Text.Trim() != "") || (tb_fecha_proveedor.Text.Trim() != ""))
            {
                WebMsgBox.Show("Debe Ingresar Todos los Campos del Documento de Proveedor");
                return null;
            }
            if (Bean_OC.strC21 == "")
            {
                Bean_OC.strC21 = null;
                Bean_OC.strC22 = null;
            }


            int transID = 8;
            switch (transID)
            {
                case 8:
                    transID = 8;
                    break;
                case 53:
                    transID = 8;
                    break;
                default:
                    transID = 15;
                    break;
            }
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
                if (rubro.rubroImpuesto == 0)
                {
                    //No Afecto
                    Bean_OC.decC5 += Convert.ToDecimal(rubro.rubroSubTot);
                }
                else
                {
                    //Afecto
                    Bean_OC.decC6 += Convert.ToDecimal(rubro.rubroSubTot);
                }
                if (Bean_OC.arr1 == null) Bean_OC.arr1 = new ArrayList();
                Bean_OC.arr1.Add(rubro);
            }
            #endregion
        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
            return null;
        }
        return Bean_OC;
        #endregion
    }
    private Bean_Provision_Automatica Construir_Provision(RE_GenericBean Orden_Compra)
    {
        #region Construir Provision Automatica
        Bean_Provision_Automatica Provision_Automatica = null;
        int contribuyente = int.Parse(lb_contribuyente.SelectedValue);//excento contribuyente
        int moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue);//importacion exportacion
        int tipo_cobro = 1;//prepaid, collect
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());//fiscal o financiera
        if (tipo_contabilidad == 2) contribuyente = 1; // si es financiera contribuyente=excento
        int servicio = 0; //fcl, lcl, etc
        int tipopersona = 4;
        int transID = 8;
        try
        {
            Provision_Automatica = new Bean_Provision_Automatica();
            Provision_Automatica.tpr_prov_id = 0;
            Provision_Automatica.tpr_proveedor_id = Orden_Compra.intC3;

            Provision_Automatica.tpr_fact_id = Orden_Compra.strC19;
            Provision_Automatica.tpr_fact_corr = Orden_Compra.strC20;
            Provision_Automatica.tpr_fact_fecha = Orden_Compra.strC21;
            Provision_Automatica.tpr_fecha_maxpago = Orden_Compra.strC22;

            Provision_Automatica.tpr_valor = double.Parse(Orden_Compra.decC1.ToString());
            Provision_Automatica.tpr_afecto = double.Parse(Orden_Compra.decC6.ToString());
            Provision_Automatica.tpr_noafecto = double.Parse(Orden_Compra.decC5.ToString());
            Provision_Automatica.tpr_iva = double.Parse(Orden_Compra.decC3.ToString());
            Provision_Automatica.tpr_observacion = Orden_Compra.strC8;
            Provision_Automatica.tpr_suc_id = user.SucursalID;
            Provision_Automatica.tpr_pai_id = Orden_Compra.intC7;
            Provision_Automatica.tpr_usu_creacion = user.ID;
            Provision_Automatica.tpr_fecha_creacion = DB.getDateTimeNow().Substring(0, 10);
            Provision_Automatica.tpr_usu_acepta = user.ID;
            Provision_Automatica.tpr_fecha_acepta = Provision_Automatica.tpr_fecha_creacion;
            Provision_Automatica.tpr_departamento = Orden_Compra.intC4;
            Provision_Automatica.tpr_ted_id = 5;
            #region Definir Serie
            //string serie = "";
            //ArrayList arr = (ArrayList)DB.getSerieFactbySuc(Provision_Automatica.tpr_suc_id, 5, user, 1);
            //if (arr != null && arr.Count > 0) serie = arr[0].ToString();
            //if (serie == "")
            //{
            //    Provision_Automatica.tpr_serie = "";
            //}
            //else
            //{
            //    Provision_Automatica.tpr_serie = serie;
            //}
            if (user.PaisID == 4)
            {
                string serie = "";
                ArrayList arr = (ArrayList)DB.getSerieFactbySuc(Provision_Automatica.tpr_suc_id, 5, user, 1);
                if (arr != null && arr.Count > 0) serie = arr[0].ToString();
                if (serie == "")
                {
                    Provision_Automatica.tpr_serie = "";
                }
                else
                {
                    Provision_Automatica.tpr_serie = serie;
                }
            }
            else
            {
                int ban_serie = 0;
                ArrayList arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(5, user, 1);
                foreach (RE_GenericBean Bean_Serie in arr)
                {
                    if ((Bean_Serie.strC3 == lb_moneda.SelectedValue) && (ban_serie == 0))
                    {
                        ban_serie++;
                        Provision_Automatica.tpr_serie = Bean_Serie.strC1;
                    }
                }
            }
            #endregion
            Provision_Automatica.tpr_serie_oc = Orden_Compra.strC1;
            Provision_Automatica.tpr_correlativo_oc = int.Parse(lb_corr.Text);
            Provision_Automatica.tpr_tts_id = Orden_Compra.intC11;
            Provision_Automatica.tpr_hbl = Orden_Compra.strC6;
            Provision_Automatica.tpr_mbl = Orden_Compra.strC7;
            Provision_Automatica.tpr_routing = Orden_Compra.strC4;
            Provision_Automatica.tpr_contenedor = Orden_Compra.strC5;
            Provision_Automatica.tpr_tpi_id = 4;
            Provision_Automatica.tpr_correlativo = 0;
            Provision_Automatica.tpr_mon_id = Orden_Compra.intC9;
            Provision_Automatica.tpr_serie_contrasena = "";
            Provision_Automatica.tpr_contrasena_correlativo = 0;
            Provision_Automatica.tpr_valor_equivalente = double.Parse(Orden_Compra.decC2.ToString());
            Provision_Automatica.tpr_imp_exp_id = Orden_Compra.intC12;
            Provision_Automatica.tpr_bien_serv = 2;
            Provision_Automatica.tpr_fecha_emision = lb_fecha_hora.Text;
            Provision_Automatica.tpr_tcon_id = Orden_Compra.intC10;
            Provision_Automatica.tpr_nombre = Orden_Compra.strC9;
            Provision_Automatica.tpr_proveedor_cajachica_id = 0;
            Provision_Automatica.tpr_poliza = "";
            Provision_Automatica.tpr_fiscal = true;
            Provision_Automatica.tpr_fecha_libro_compras = DB.getDateTimeNow().Substring(0, 10);
            Provision_Automatica.tpr_tto_id = Orden_Compra.Tipo_Operacion;
            Provision_Automatica.tpr_ruta_pais = "";
            Provision_Automatica.tpr_ruta = "";
            Provision_Automatica.tpr_blid = Orden_Compra.Blid;
            Provision_Automatica.tpr_tti_id = Orden_Compra.intC13;
            Provision_Automatica.tpr_toc_id = int.Parse(lbl_tocid.Text);

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
                rubro.rubroID = long.Parse(lb1.Text);
                rubro.rubroName = lb2.Text;
                rubro.rubtoType = lb3.Text;
                rubro.rubroSubTot = double.Parse(lb4.Text);
                rubro.rubroImpuesto = double.Parse(lb5.Text);
                rubro.rubroTot = double.Parse(lb6.Text);
                rubro.rubroTotD = double.Parse(lb7.Text);
                rubro.rubroTypeID = Utility.TraducirServiciotoINT(rubro.rubtoType);
                rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
                rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, rubro.rubroTypeID);

                if ((rubro.cta_debe == null) || (rubro.cta_debe.Count == 0))
                {
                    WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de pago que esta realizando, por favor pongase en contacto con el Contador");
                    return null;
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
                    Bean_Costos.intC2 = 3;//Id Tipo Proveedor Master
                    Bean_Costos.intC3 = int.Parse(tb_proveedorID.Text);//Id Proveedor
                    Bean_Costos.strC1 = tb_proveedornombre.Text.Trim();//Proveedor Nombre
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
                    Bean_Costos.Blid =  int.Parse(lbl_blID.Text);
                    Bean_Costos.intC10 = 0;//documentoID
                    Bean_Costos.intC11 = 5;//Provision
                    Bean_Costos.strC6 = tipo_bl;
                    Bean_Costos.strC7 = lb_serie_factura.SelectedItem.Text;//Serie Provision
                    Bean_Costos.strC8 = lb_corr.Text;//Correlativo Provision
                    Bean_Costos.intC12 = int.Parse(lb_contribuyente.SelectedValue);//Tipo de Contribuyente
                    Bean_Costos.intC13 = 0;//tdf_id
                    Bean_Costos.intC14 = 0;//costo_id
                    //Aqui me quede antes de hacer insert de costos
                    if (Provision_Automatica.arr2 == null) Provision_Automatica.arr2 = new ArrayList();
                    Provision_Automatica.arr2.Add(Bean_Costos);
                }
            }
            #endregion
            transID = 8;
            if (!string.IsNullOrEmpty(Provision_Automatica.tpr_fact_id) &&
                    !string.IsNullOrEmpty(Provision_Automatica.tpr_fact_corr) &&
                    !string.IsNullOrEmpty(Provision_Automatica.tpr_fact_fecha))
            {
                int matOpID = DB.getMatrizOperacionID(transID, Provision_Automatica.tpr_mon_id, user.PaisID, user.contaID);
                ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID, "Abono");
                Provision_Automatica.arr3 = ctas_cargo;
            }
            else
            {
                Provision_Automatica.arr3 = new ArrayList() { { new RE_GenericBean { intC1 = 690713, strC1 = "2.1.1.1.0006", strC2 = "Abono", intC2 = 0, strC3 = "FACTURAS EN TRANSITO" } } };
            }
            
        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
            WebMsgBox.Show("Existio un error al momento de obtener la informacion de la Orden de Compra.");
            return null;
        }
        return Provision_Automatica;
        #endregion
    }
    protected void Bloquear_Pantalla()
    {
        #region Bloquear Pantalla
        bt_grabar.Enabled = false;
        bt_aceptar.Enabled = false;
        tb_cancelar.Enabled = false;
        bt_agregar.Enabled = false;
        gv_detalle.Enabled = false;
        #endregion
    }
    protected bool Validar_Restricciones(string sender)
    {
        bool resultado = false;
        bool paiR = DB.Validar_Pais_Restringido(user);
        if (paiR == true)
        {
            #region CONTABILIZAR AUTOMATICAMENTE LA PROVISION
            if ((DB.Validar_Restriccion_Activa(user, 7, 39) == true) && (sender == "bt_aceptar"))
            {
                resultado = true;
            }
            #endregion
        }
        return resultado;
    }
    protected void lb_serie_factura_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie_factura.Items.Count > 0)
        {
            if (lb_serie_factura.SelectedValue != "0")
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
    protected void Actualizar_Moneda_Rubros()
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
            //dt.Rows.Add(objArr);

            //halo el rubro y luego le calculo sus chingaderas
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
            //// Si la moneda que estoy agregando es diferente a la que estoy facturando
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
            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)") };
            dt.Rows.Add(obj);
        }
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }
    protected void gv_detalle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((Request.QueryString["id"] != null) && (!Request.QueryString["id"].ToString().Equals("")))
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button bt = (Button)e.Row.Cells[0].Controls[0];
                bt.Enabled = false;
            }
        }
    }
}

