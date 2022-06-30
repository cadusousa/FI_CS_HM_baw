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

public partial class operations_pop_cheques : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    string serie = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
        }
        user = (UsuarioBean)Session["usuario"];
    
        if (!Page.IsPostBack) {
            obtengo_listas();
            lb_bancos_SelectedIndexChanged(sender, e);
        }
        
    }

    protected void obtengo_listas() {
        ArrayList arr = null;
        ListItem item = null;
        lb_moneda.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        lb_moneda.Items.Add(item);
        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, 0);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_moneda.Items.Add(item);
        }
        // obtengo las transacciones
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='"+user.ID+"' and uop_pai_id="+user.PaisID+" and uop_ttt_id=ttt_id and ttt_template='pop_cheques.aspx'");
        lb_transaccion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        lb_transaccion.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_transaccion.Items.Add(item);
        }
        //obtengo Transacciones de Anticipos
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='anticipos.aspx'");
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_transaccion.Items.Add(item);
        }
        //BAW FISCAL USD
        if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
        {
            arr = (ArrayList)DB.getBancos_By_PaisConta(user);
        }
        else
        {
            arr = (ArrayList)DB.getBancos_By_PaisMoneda(user);
        }
        lb_bancos.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_bancos.Items.Add(item);
        }
        item = new ListItem("Seleccione...", "0");
        lb_cuentas_bancarias.Items.Add(item);
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
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo.Text; else where += "and id_usuario=" + tb_codigo.Text;
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
        else if (lb_tipopersona.SelectedValue.Equals("12"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit usuario de comisiones");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo.Text; else where += "and id_usuario=" + tb_codigo.Text;
            where += " and tipo_usuario=1 and pais='" + user.pais.ISO + "'";
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("Comisiones", Arr);
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
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[8].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            //int proveedorID = int.Parse(tb_agenteID.Text);
            //int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
            //int monID = int.Parse(lb_moneda.SelectedValue);
            ////obtengo el listado de cortes
            //ArrayList Arr = null;
            //Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            //dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            //gv_cortes.DataSource = dt;
            //gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            #region Validacion Grupo Empresas
            if (int.Parse(lb_transaccion.SelectedValue) == 75)
            {
                if ((DB.Validar_Restriccion_Activa(user, 6, 29) == true))
                {
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
                            WebMsgBox.Show("No se puede Emitir un Anticipo Al Agente.: " + Page.Server.HtmlDecode(row.Cells[1].Text) + " - " + Page.Server.HtmlDecode(row.Cells[2].Text) + ", porque es un Agente NO NEUTRAL");
                            #region Limpiar Tipo de Persona
                            tb_agenteID.Text = "";
                            tb_agentenombre.Text = "";
                            tb_direccion.Text = "";
                            tb_telefono.Text = "";
                            tb_correoelectronico.Text = "";
                            tb_acreditado.Text = "";
                            #endregion
                            return;
                        }
                    }
                    else if (user.pais.Grupo_Empresas == 3)
                    {
                    }
                }
            }
            #endregion
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            //tb_contacto.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[4].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[5].Text);

            //int proveedorID = int.Parse(tb_agenteID.Text);
            //int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
            //int monID = int.Parse(lb_moneda.SelectedValue);
            ////obtengo el listado de cortes
            //ArrayList Arr = null;
            //Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            //dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            //gv_cortes.DataSource = dt;
            //gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            //tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
            //int proveedorID = int.Parse(tb_agenteID.Text);
            //int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
            //int monID = int.Parse(lb_moneda.SelectedValue);
            ////obtengo el listado de cortes
            //ArrayList Arr = null;
            //Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            //dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            //gv_cortes.DataSource = dt;
            //gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            //tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
            //int proveedorID = int.Parse(tb_agenteID.Text);
            //int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
            //int monID = int.Parse(lb_moneda.SelectedValue);
            ////obtengo el listado de cortes
            //ArrayList Arr = null;
            //Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            //dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            //gv_cortes.DataSource = dt;
            //gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            //tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
            //int proveedorID = int.Parse(tb_agenteID.Text);
            //int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
            //int monID = int.Parse(lb_moneda.SelectedValue);
            //if (lb_transaccion.SelectedValue.ToString().Equals("42")) // si la operacion es cajachica
            //{
            //    //obtengo el listado de cortes
            //    ArrayList Arr = null;
            //    Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            //    dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            //    gv_cortes.DataSource = dt;
            //    gv_cortes.DataBind();
            //}
        }
        else if (lb_tipopersona.SelectedValue.Equals("12"))//Comisiones
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("3"))//Clientes
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            //tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
            //int proveedorID = int.Parse(tb_agenteID.Text);
            //int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
            //int monID = int.Parse(lb_moneda.SelectedValue);
            ////obtengo el listado de cortes
            //ArrayList Arr = null;
            //Arr = DB.getRCPT_pendientes(proveedorID, user);
            //dt = (DataTable)Utility.fillGridView("recibosclientes_cheques", Arr);
            //gv_cortes.DataSource = dt;
            //gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("0"))//Retenciones
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            //tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
            //int proveedorID = int.Parse(tb_agenteID.Text);
            //int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
            //int monID = int.Parse(lb_moneda.SelectedValue);
            ////obtengo el listado de cortes
            //ArrayList Arr = null;
            //Arr = DB.getRetencionbyProveedor(proveedorID, proveedortype, monID, 1);
            //dt = (DataTable)Utility.fillGridView("retencionproveedor", Arr);
            //gv_cortes.DataSource = dt;
            //gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
            //int proveedorID = int.Parse(tb_agenteID.Text);
            //int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
            //int monID = int.Parse(lb_moneda.SelectedValue);
            ////obtengo el listado de cortes
            //ArrayList Arr = null;
            //Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            //dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            //gv_cortes.DataSource = dt;
            //gv_cortes.DataBind();
        }
        tb_acreditado.Text = tb_agentenombre.Text;
        gv_retenciones.DataBind();
    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
            ArrayList Arr = null;
            string where = "";
            //Arr = (ArrayList)DB.getAgente(where);
            //dt = (DataTable)Utility.fillGridView("Agente", Arr);
            //ViewState["proveedordt"] = dt;
            //gv_proveedor.DataSource = dt;
            //gv_proveedor.DataBind();
        }
    }

    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        if (lb_transaccion.SelectedItem.Text.Equals("Seleccione..."))
        {
            WebMsgBox.Show("Seleccione la Transaccion");
            return;
        }
        decimal valor = decimal.Parse(tb_valor.Text);
        decimal valor_equivalente = decimal.Parse(tb_valor_equivalente.Text);
        decimal cargoadicional = 0;
        
        int tranID = int.Parse(lb_transaccion.SelectedValue);
        int monID = int.Parse(lb_moneda.SelectedValue);
        int paisID = user.PaisID;
        int contaID = int.Parse(Session["Contabilidad"].ToString());
        int tedID = 0;
        if (!tb_cargoadicional.Text.Equals("")) cargoadicional = decimal.Parse(tb_cargoadicional.Text);
        
        if (tb_agenteID.Text.Equals("")) 
        {
            WebMsgBox.Show("Es necesario especificar el codigo del proveedor");
            return;
        }
        else if (lb_cuentas_bancarias.SelectedItem.Text.Equals("Seleccione..."))
        {
            WebMsgBox.Show("Debe seleccionar una cuenta bancaria");
            return;
        }
        else if (tb_fecha.Text.Equals(""))
        {
            WebMsgBox.Show("Es necesario ingresar la fecha del cheque");
            return;
        }
        else if (tb_valor.Text.Equals(""))
        {
            WebMsgBox.Show("El valor del cheque no puede estar vacío");
            return;
        }
        else if (decimal.Parse(tb_valor.Text)==0)
        {
            WebMsgBox.Show("El valor del cheque debe ser mayor a 0");
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
            WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0,10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0,10));
            return;
        }
        #endregion
        if ((tranID != 43) && (tranID != 74) && (tranID != 75) && (tranID != 76) && (tranID != 77) && (tranID != 109))//Solo si no es Anticipo
        {
            int ban_cortes = 0;
            CheckBox chk_cortes;
            tedID = 4;
            if (gv_cortes.Rows.Count > 0)
            {
                foreach (GridViewRow grd_Row in gv_cortes.Rows)
                {
                    chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
                    if (chk_cortes.Checked)
                    {
                        ban_cortes++;
                    }
                }
            }
            if (ban_cortes == 0)
            {
                WebMsgBox.Show("Debe seleccionar al menos un Corte");
                gv_retenciones.DataBind();
                return;
            }
        }
        else
        {
            tedID = 14;
        }
        RE_GenericBean rgb = new RE_GenericBean();
        #region Formatear Fecha del Cheque
        //Fecha Inicio
        int fe_dia = int.Parse(tb_fecha.Text.Substring(3, 2));
         int fe_mes = int.Parse(tb_fecha.Text.Substring(0, 2));
         int fe_anio = int.Parse(tb_fecha.Text.Substring(6, 4));
        rgb.strC2 = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            rgb.strC2 += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            rgb.strC2 += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            rgb.strC2 += "0" + fe_dia.ToString();
        }
        else
        {
            rgb.strC2 += fe_dia.ToString();
        }
        #endregion
        rgb.Fecha_Hora = lb_fecha_hora.Text;
        rgb.intC1 = int.Parse(lb_moneda.SelectedValue);//moneda
        rgb.intC2 = int.Parse(tb_chequeNo.Text);//No cheque
        rgb.strC1 = lb_cuentas_bancarias.SelectedValue;//cuenta
        rgb.strC2 = rgb.strC2;//fecha cheque
        rgb.decC1 = valor;//valor
        rgb.decC4 = valor_equivalente;//valor equivalente
        rgb.decC2 = cargoadicional;//cargo adicional
        rgb.strC3 = tb_acreditado.Text;//Acreditado
        rgb.strC4 = tb_motivo.Text;// nota para el cheque
        rgb.intC3 = contaID;// tipo contabilidad
        rgb.intC4 = tranID;// tipo transaccion
        rgb.intC6 = int.Parse(tb_agenteID.Text);
        rgb.intC5 = int.Parse(lb_tipopersona.SelectedValue);
        rgb.strC5 = tb_referencia.Text;
        rgb.strC6 = tb_observaciones.Text;
        rgb.intC7 = int.Parse(lb_bancos.SelectedValue);
        rgb.intC8 = tedID;
        rgb.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
        bool valida_agente = Validar_Restricciones("Validar_Econocaribe");
        if (valida_agente == true)
        {
            return;
        }

        //Retenciones (aplica para Nicaragua)
        int trans_provision_retencion = int.Parse(lb_provision_retencion.Text.ToString());
        if (gv_retenciones.Rows.Count > 0)
        {
            Label ret_id = null;
            Label ret_monto = null;
            TextBox ret_referencia = null;
            Label ret_prov_fecha = null;
            Label ret_tcambio = null;
            Label ret_transaccion = null;
            Label ret_moneda = null;
            string cta_retencion_cargo = "";

            //Obteniendo la cuenta de Cargo para la Retencion, la misma para todas las retenciones por tipo de proveedor
            int matOpIDCargo = DB.getMatrizOperacionID(trans_provision_retencion, monID, user.PaisID, contaID);
            ArrayList ctas_ret_cargo = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpIDCargo, "Abono");
            foreach (RE_GenericBean cta_ret_cargo in ctas_ret_cargo)
            {
                cta_retencion_cargo = cta_ret_cargo.strC1;
            }

            foreach (GridViewRow row in gv_retenciones.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    ret_id = (Label)row.FindControl("lb_id");
                    ret_monto = (Label)row.FindControl("lb_monto");
                    ret_referencia = (TextBox)row.FindControl("lb_referencia");
                    ret_prov_fecha = (Label)row.FindControl("lb_prov_fecha");
                    ret_tcambio = (Label)row.FindControl("lb_tcambio");
                    ret_transaccion = (Label)row.FindControl("lb_transaccion");
                    ret_moneda = (Label)row.FindControl("lb_moneda");

                    RE_GenericBean retencion = new RE_GenericBean();
                    retencion.strC3 = cta_retencion_cargo;
                    retencion.intC1 = int.Parse(ret_id.Text.Trim());//id retencion
                    retencion.decC1 = decimal.Parse(ret_monto.Text.Trim());//monto provision
                    retencion.strC1 = ret_referencia.Text.Trim();//referencia o numero de retencion fiscal
                    retencion.strC2 = ret_prov_fecha.Text.Trim();//fecha provision para calcular equiv de retencion
                    retencion.decC2 = decimal.Parse(ret_tcambio.Text.Trim());//tipo cambio provision
                    retencion.intC2 = int.Parse(ret_transaccion.Text.Trim());//id retencion
                    retencion.intC3 = int.Parse(ret_moneda.Text.Trim());//moneda

                    //Obteniendo las cuentas de Abono para cada retencion
                    int matOpIDRetAbono = DB.getMatrizOperacionID(retencion.intC2, monID, 1, contaID);
                    if (retencion.arr1 == null) retencion.arr1 = new ArrayList();
                    retencion.arr1 = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpIDRetAbono, "Abono");
                    //Asignando la cuenta de Abono
                    foreach (RE_GenericBean cta_abono in retencion.arr1)
                    {
                        retencion.strC4 = cta_abono.strC1;
                    }
                    if (rgb.arr3 == null) rgb.arr3 = new ArrayList();
                    rgb.arr3.Add(retencion);
                }
            }
        }

        //************************************************************** obtengo los cortes que voy a matar
        if (tranID != 43)// solo si no es anticipo de clientes
        {
            GridViewRow row;
            CheckBox chk;
            for (int i = 0; i < gv_cortes.Rows.Count; i++)
            {
                row = gv_cortes.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    chk = (CheckBox)row.FindControl("chk_seleccion");
                    if (chk.Checked)
                    {
                        if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
                        rgb.arr2.Add(row.Cells[1].Text);
                    }
                }
            }
        }

        //depende de la transacción crea las cuentas contables del libro diario
        switch (tranID)
        {
            //Cheque a Proveedores
            case 9:
                {
                    var cortesStr = string.Join(",", rgb.arr2.ToArray());

                    //obtiene si existe la cuenta en el libro diario de las provisiones Facturas En transito
                    var query = string.Format(@"select distinct tdi_cue_id 
                                        from tbl_corte_proveedor_detalle inner join tbl_libro_diario l
	                                        on l.tdi_ref_id = tcpd_docto_id
	                                        and l.tdi_ttr_id = tcpd_str_id
                                        inner join tbl_cuenta c
	                                        on c.cue_id = l.tdi_cue_id
                                        where tcpd_tcp_id in({0}) and tcpd_str_id = 5
                                        and l.tdi_cue_id = '2.1.1.1.0006'", cortesStr);
                    var result = DB.ObtenerValorCampoBAW(query);
                    //valida si algun corte tiene provision tiene facturas en transito, si no tiene asigna cuentas normales en caso contrario asignar cuentas de gastos provisionados
                    if (string.IsNullOrEmpty(result))
                    {
                        int matOpID = DB.getMatrizOperacionID(tranID, monID, paisID, contaID);
                        ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        rgb.arr1 = ctas;
                    }
                    else
                    {
                        rgb.arr1 = new ArrayList() { 
                                        new RE_GenericBean { intC1 = 0, strC1 = "1.1.1.2.0000", strC2 = "Abono", intC2 = 0, strC3 = "BANCOS" }
                                      , new RE_GenericBean { intC1 = 0, strC1 = "1.1.2.8.0002", strC2 = "Cargo", intC2 = 0, strC3 = "GASTOS PROVISIONADOS" } };
                    }
                    break;
                };
            //Anticipo cheque a proveedores
            case 74:
                {
                    rgb.arr1 = new ArrayList() {
                                     new RE_GenericBean { intC1 = 0, strC1 = "1.1.1.2.0000", strC2 = "Abono", intC2 = 0, strC3 = "BANCOS" }
                                   , new RE_GenericBean { intC1 = 0, strC1 = "1.1.2.8.0001", strC2 = "Cargo", intC2 = 0, strC3 = "ANTICIPOS POR LIQUIDAR" } };
                    break;
                };
            default:
                {
                    int matOpID = DB.getMatrizOperacionID(tranID, monID, paisID, contaID);
                    ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                    rgb.arr1 = ctas;
                }
                break;
        }
        
        //************************************************************* calculo el iva del cargo adicional, solo Panama
        if ((monID != 8) && (user.PaisID == 6))
        {
            rgb.decC3 = Math.Round(rgb.decC2 * user.pais.Impuesto, 2);
        }
        else 
        {
            rgb.decC3 = 0;
        }
        string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 6);//Cheques
        if (Check_Existencia == "0")
        {
            ArrayList result = DB.AplicoCheque(rgb, user);
            if (result == null || result.Count == 0 || int.Parse(result[1].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
            {
                WebMsgBox.Show("Existió un problema al tratar de guardar la información, por favor intente de nuevo");
                btn_imprimir.Enabled = false;
                bt_guardar.Enabled = true;
                btn_calcular.Enabled = true;
                return;
            }
            else
            {
                lbl_tcg_id.Text = result[0].ToString();
                tb_chequeNo.Text = result[1].ToString();
                int retenciones_aplicadas = int.Parse(result[2].ToString());

                if (retenciones_aplicadas > 0)
                {
                    if (user.PaisID == 3 || user.PaisID == 23)
                    {
                        DB.AplicarChequeRetencion(int.Parse(result[0].ToString()), user, 6, rgb.intC1);
                    }
                }

                WebMsgBox.Show("El Cheque fue guardado exitosamente con Numero.: " + result[1].ToString());
                #region Asignar Correlativo a las Retenciones Pagadas
                RE_GenericBean ChequeBean = (RE_GenericBean)DB.getChequeData(lb_cuentas_bancarias.SelectedValue, int.Parse(tb_chequeNo.Text), "");
                ArrayList arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 20, user, 0);//1 porque es el tipo de documento para Retenciones
                foreach (string Serie in arr)
                {
                    serie = Serie;
                }
                if ((user.PaisID != 3) && (user.PaisID != 23))
                {
                    int resultado = DB.updateCorrelativoRetencion(serie, user, ChequeBean);
                }
                #endregion

                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[0].ToString()), 6, 0);
                gv_detalle_partida.DataBind();
                //gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

                gv_cortes.Enabled = false;
                lb_correlativo.Text = result.ToString();
                bt_guardar.Enabled = false;
                btn_imprimir.Enabled = true;
                btn_printret.Enabled = true;
                btn_buscar_soas.Enabled = false;
                btn_calcular.Enabled = false;
                lb_bancos.Enabled = false;
                lb_cuentas_bancarias.Enabled = false;
                tb_fecha.Enabled = false;
                return;
            }
        }
        else
        {
            bt_guardar.Enabled = false;
            return;
        }
    }
    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBox chk_cortes;
        #region Limpiar GV Cortes
        //Se agrego este procedimiento porque si se escoge un banco o una cuenta bancariaria diferente
        //Se debe recalcular el total dependiendo la moneda y el tipo de cambio
        if (gv_cortes.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_cortes.Rows)
            {
                chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
                chk_cortes.Checked = false;
            }
            tb_valor.Text = "0";
            tb_valor_equivalente.Text = "0";
        }
        #endregion
        UsuarioBean user;
        user = (UsuarioBean)Session["usuario"];
        ListItem item = null;
        lb_cuentas_bancarias.Items.Clear();
        lb_moneda.SelectedValue = "0";
        item = new ListItem("Seleccione...", "0");
        item.Selected = true;
        lb_cuentas_bancarias.Items.Add(item);
        if (lb_bancos.SelectedValue == "")
        {
            WebMsgBox.Show("Debe seleccionar una Cuenta Bancaria");
            return;
        }
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(lb_bancos.SelectedValue), user,1);
        foreach (RE_GenericBean rgb in arrCtas) {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_cuentas_bancarias.Items.Add(item);
        }
        gv_retenciones.DataBind();
        gv_cortes.DataBind();
    }

    protected void gv_detalle_RowDataBound(object sender, GridViewRowEventArgs e)
    {

            TextBox t1 = new TextBox();
            TextBox t2 = new TextBox();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                t1 = (TextBox)e.Row.FindControl("tb_cargos");
                t2 = (TextBox)e.Row.FindControl("tb_abonos");
                if (DataBinder.Eval(e.Row.DataItem, "Cuenta de").ToString() == "Cargo")
                {
                    t1.Enabled = true;
                    t2.Text = "0";
                    t2.Enabled = false;
                }
                else if (DataBinder.Eval(e.Row.DataItem, "Cuenta de").ToString() == "Abono")
                {
                    t1.Text = "0";
                    t1.Enabled = false;
                    t2.Enabled = true;
                }
                
            }
 
    }

    protected void lb_cuentas_bancarias_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBox chk_cortes;
        #region Limpiar GV Cortes
        //Se agrego este procedimiento porque si se escoge un banco o una cuenta bancariaria diferente
        //Se debe recalcular el total dependiendo la moneda y el tipo de cambio
        if (gv_cortes.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_cortes.Rows)
            {
                chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
                chk_cortes.Checked = false;
            }
            tb_valor.Text = "0";
            tb_valor_equivalente.Text = "0";
        }
        #endregion
        gv_retenciones.DataBind();
        if (!lb_cuentas_bancarias.SelectedItem.Text.Equals("Seleccione..."))
        {
            string cuentaID = lb_cuentas_bancarias.SelectedValue;
            RE_GenericBean datoscuenta = (RE_GenericBean)DB.GetMonedaIDbyCuentaBancaria(cuentaID);
            lb_moneda.SelectedValue = datoscuenta.intC1.ToString();
            tb_chequeNo.Text = datoscuenta.intC2.ToString();
            tb_chequeNo.Enabled = false;
            tb_anticipo_TextChanged(sender, e); 

        }
        gv_cortes.DataBind();
        gv_retenciones.DataBind();
    }
    protected void btn_calcular_Click(object sender, EventArgs e)
    {
        if (lb_cuentas_bancarias.SelectedItem.Text.Equals("Seleccione..."))
        {
            WebMsgBox.Show("Debe seleccionar una cuenta bancaria");
            return;
        }
        if ((lb_transaccion.SelectedValue.ToString() != "43") && (lb_transaccion.SelectedValue.ToString() != "74") && (lb_transaccion.SelectedValue.ToString() != "75") && (lb_transaccion.SelectedValue.ToString() != "76") && (lb_transaccion.SelectedValue.ToString() != "77"))
        {
            int ban_cortes = 0;
            CheckBox chk_cortes;
            if (gv_cortes.Rows.Count > 0)
            {
                foreach (GridViewRow grd_Row in gv_cortes.Rows)
                {
                    chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
                    if (chk_cortes.Checked)
                    {
                        ban_cortes++;
                    }
                }
            }
            if (ban_cortes == 0)
            {
                WebMsgBox.Show("Debe seleccionar al menos un Corte");
                tb_valor.Text = "0";
                tb_valor_equivalente.Text = "0";
                gv_retenciones.DataBind();
                return;
            }
        }
        CheckBox chk;
        GridViewRow row = null;
        RE_GenericBean rgb = null;
        int monID = int.Parse(lb_moneda.SelectedValue);
        int tipopersona = int.Parse(lb_tipopersona.SelectedValue);
        decimal cargos_adicionales = 0;
        decimal cargos_adicionales_eq = 0;
        decimal total = 0;
        decimal total_eq = 0;
        
        for (int i = 0; i < gv_cortes.Rows.Count; i++)
        {
            row = gv_cortes.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("chk_seleccion");
                if (chk.Checked)
                {
                    rgb = new RE_GenericBean();
                    rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                    rgb.decC2 = decimal.Parse(row.Cells[8].Text);
                    rgb.intC1 = Utility.TraducirMonedaStr(row.Cells[6].Text);
                    if (monID != rgb.intC1)
                    {
                        total += rgb.decC2;
                        total_eq += rgb.decC1;
                    }
                    else
                    {
                        total += rgb.decC1;
                        total_eq += rgb.decC2;
                    }
                }
            }
        }
        //Obteniendo Cargos Adicionales y Convirtiendo su equivalente
        if (!tb_cargoadicional.Text.Equals("")) cargos_adicionales = decimal.Parse(tb_cargoadicional.Text);
        
        if (monID == 8)
        {
            cargos_adicionales_eq = cargos_adicionales * user.pais.TipoCambio;
        }
        else
        {
            cargos_adicionales_eq = cargos_adicionales / user.pais.TipoCambio;
        }

        //Obteniendo el Anticipo y Convirtiendo su equivalente
        if ((int.Parse(lb_transaccion.SelectedValue) == 43) || (lb_transaccion.SelectedValue.ToString() == "74") || (lb_transaccion.SelectedValue.ToString() == "75") || (lb_transaccion.SelectedValue.ToString() == "76") || (lb_transaccion.SelectedValue.ToString() == "77"))
        {
            total = decimal.Parse(tb_anticipo.Text.ToString());
            if (monID == 8)
            {
                total_eq = total * user.pais.TipoCambio;
            }
            else
            {
                total_eq = total / user.pais.TipoCambio;
            }
        }

        total += cargos_adicionales;
        total_eq += cargos_adicionales_eq;
        tb_valor.Text = total.ToString("#,#.00");
        tb_valor_equivalente.Text = total_eq.ToString("#,#.00");
    }
    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
        string criterio = "";
        string script = "";
        criterio = " and tcg_id=" + lbl_tcg_id.Text + " ";
        RE_GenericBean ChequeBean = (RE_GenericBean)DB.getChequeData(lb_cuentas_bancarias.SelectedValue, int.Parse(tb_chequeNo.Text), criterio);
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        //Script Original
        //string script = "window.open('../invoice/printersettings.aspx?ctaID=" + lb_cuentas_bancarias.SelectedValue + "&correlativo=" + tb_chequeNo.Text + "&tipo=6','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        //if ((user.PaisID == 1) || (user.PaisID == 2) || (user.PaisID == 9) || (user.PaisID == 15) || (user.PaisID == 3) || (user.PaisID == 5) || (user.PaisID == 23) || (user.PaisID == 20) || (user.PaisID == 21) || (user.PaisID == 26))
        //{
            script = "window.open('../ImpresionDocumentos.html?tipo_cambio=" + user.pais.TipoCambio.ToString() + "&id=" + lb_cuentas_bancarias.SelectedValue + "&correlativo=" + tb_chequeNo.Text + "&bcoId=" + lb_bancos.SelectedItem + "&fac_id=" + lbl_tcg_id.Text + "&tipo=6','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
            //script = "window.open('../plantillas/impresion.aspx?id=" + lb_cuentas_bancarias.SelectedValue + "&correlativo=" + tb_chequeNo.Text + "&tipo=6','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        //}
        //else
        //{
        //    script = "window.open('../invoice/printersettings.aspx?ctaID=" + lb_cuentas_bancarias.SelectedValue + "&correlativo=" + tb_chequeNo.Text + "&tipo=6&fac_id=" + lbl_tcg_id.Text + "','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        //}
        #region Seteo de Parametros de Impresion
        user.ImpresionBean.Operacion = "1";
        user.ImpresionBean.Tipo_Documento = "6";
        user.ImpresionBean.Id = ChequeBean.intC1.ToString();
        user.ImpresionBean.Impreso = false;
        Session["usuario"] = user;
        #endregion
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }

    protected void btn_printret_Click(object sender, EventArgs e)
    {
        string criterio = "";
        string script = "";
        criterio = " and tcg_id=" + lbl_tcg_id.Text + " ";
        RE_GenericBean ChequeBean = (RE_GenericBean)DB.getChequeData(lb_cuentas_bancarias.SelectedValue, int.Parse(tb_chequeNo.Text), criterio);
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        //string script = "window.open('pop_retenciontoprintlist.aspx?chequetransID=" + lb_correlativo.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        if (user.PaisID == 3 || user.PaisID == 23)
        {
            script = "window.open('../invoice/template_retencion.aspx?chequeID=" + ChequeBean.intC1.ToString() + "&sucursalID=" + user.SucursalID.ToString() + "&userID=" + user.ID + "&contaId=" + user.contaID.ToString() + "&tipo=20','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            //script = "window.open('../plantillas/impresion.aspx?id=" + lb_cuentas_bancarias.SelectedValue + "&correlativo=" + tb_chequeNo.Text + "&tipo=6','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
            
        }
        else
        {
            script = "window.open('../ImpresionDocumentos.html?chequeID=" + ChequeBean.intC1.ToString() + "&sucursalID=" + user.SucursalID.ToString() + "&userID=" + user.ID + "&contaId=" + user.contaID.ToString() + "&tipo=20','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        //    script = "window.open('../invoice/printersettings.aspx?ctaID=" + ChequeBean.intC1.ToString() + "&tipo=20','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        }
        #region Seteo de Parametros de Impresion
        user.ImpresionBean.Operacion = "1";
        user.ImpresionBean.Tipo_Documento = "9";
        user.ImpresionBean.Id = ChequeBean.intC1.ToString();
        user.ImpresionBean.Impreso = false;
        Session["usuario"] = user;
        #endregion
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }

    protected void lb_transaccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!lb_transaccion.SelectedItem.Text.Equals("Seleccione..."))
        {
            int tranID = int.Parse(lb_transaccion.SelectedValue);
            // proveedores
            if (tranID == 9 || tranID == 124) { 
                lb_tipopersona.SelectedValue = "4";
                lb_provision_retencion.Text = "8";
            
            }
            // navieras
            else if (tranID == 19) { 
                lb_tipopersona.SelectedValue = "5";
                lb_provision_retencion.Text = "17";
            }
            // Intercompanys
            else if (tranID == 20) { 
                lb_tipopersona.SelectedValue = "10";
                lb_provision_retencion.Text = "8";
            }
            // agentes
            else if (tranID == 21) { 
                lb_tipopersona.SelectedValue = "2";
                lb_provision_retencion.Text = "15";
            }
            // Intercompanys
            else if (tranID == 22)
            {
                lb_tipopersona.SelectedValue = "10";
                lb_provision_retencion.Text = "8";
            }
            // proveedores
            else if (tranID == 16) { 
                lb_tipopersona.SelectedValue = "4";
                lb_provision_retencion.Text = "8";
            }
            // devolucion de anticipo clientes
            else if (tranID == 40) { 
                lb_tipopersona.SelectedValue = "3"; 
            }
            // caja chica
            else if (tranID == 42) { 
                lb_tipopersona.SelectedValue = "8";
                lb_provision_retencion.Text = "53";
            }
            // Linea Aerea
            else if (tranID == 47) { 
                lb_tipopersona.SelectedValue = "6";
                lb_provision_retencion.Text = "18";
            }
            // devolucion de anticipo clientes
            else if (tranID == 43) { 
                lb_tipopersona.SelectedValue = "8";
            }
            //Cheque Anticipo Proveedor
            else if (tranID == 74)
            {
                lb_tipopersona.SelectedValue = "4";
            }
            //Cheque Anticipo Agentes
            else if (tranID == 75)
            {
                lb_tipopersona.SelectedValue = "2";
            }
            //Cheque Anticipo Navieras
            else if (tranID == 76)
            {
                lb_tipopersona.SelectedValue = "5";
            }
            //Cheque Anticipo Lineas Aereas
            else if (tranID == 77)
            {
                lb_tipopersona.SelectedValue = "6";
            }
            //Cheque Anticipo Intercompanys
            else if (tranID == 109)
            {
                lb_tipopersona.SelectedValue = "10";
            }
            else if (tranID == 122)
            {
                lb_tipopersona.SelectedValue = "12";
            }
            if ((tranID == 43) || (tranID == 74) || (tranID == 75) || (tranID == 76) || (tranID == 77) || (tranID == 109))
            {
                gv_cortes.Visible = false;
                lb_anticipo.Visible = true;
                tb_anticipo.Visible = true;
            }
            else
            {
                gv_cortes.Visible = true;
                lb_anticipo.Visible = false;
                tb_anticipo.Visible = false;
            }
        }
        #region Limpiar Tipo de Persona
        tb_agenteID.Text = "";
        tb_agentenombre.Text = "";
        tb_direccion.Text = "";
        tb_telefono.Text = "";
        tb_correoelectronico.Text = "";
        tb_acreditado.Text = "";
        #endregion
    }
    protected void chk_seleccion_CheckedChanged(object sender, EventArgs e)
    {
        int ban_cortes = 0;
        CheckBox chk_cortes;
        #region Validaciones
        if (lb_cuentas_bancarias.SelectedItem.Text.Equals("Seleccione..."))
        {
            if (gv_cortes.Rows.Count > 0)
            {
                foreach (GridViewRow grd_Row in gv_cortes.Rows)
                {
                    chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
                    chk_cortes.Checked = false;
                }
            }
            WebMsgBox.Show("Debe seleccionar una cuenta bancaria");
            return;
        }
        if (gv_cortes.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_cortes.Rows)
            {
                chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
                if (chk_cortes.Checked)
                {
                    ban_cortes++;
                }
            }
        }
        #endregion
        if (ban_cortes == 0)
        {
            WebMsgBox.Show("Debe seleccionar al menos un Corte");
            tb_valor.Text = "0";
            tb_valor_equivalente.Text = "0";
            gv_retenciones.DataBind();
            return;
        }
        CheckBox chk;
        GridViewRow row = null;
        RE_GenericBean rgb = null;
        int monID = int.Parse(lb_moneda.SelectedValue);
        int tipopersona = int.Parse(lb_tipopersona.SelectedValue);
        decimal cargos_adicionales = 0;
        decimal cargos_adicionales_eq = 0;
        decimal total = 0;
        decimal total_eq = 0;
        string retenciones_id ="", sep="";
        
        for (int i = 0; i < gv_cortes.Rows.Count; i++)
        {
            row = gv_cortes.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("chk_seleccion");
                if (chk.Checked)
                {
                    rgb = new RE_GenericBean();
                    rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                    rgb.decC2 = decimal.Parse(row.Cells[8].Text);
                    rgb.intC1 = Utility.TraducirMonedaStr(row.Cells[6].Text);
                    if (monID != rgb.intC1)
                    {
                        total += rgb.decC2;
                        total_eq += rgb.decC1;
                    }
                    else
                    {
                        total += rgb.decC1;
                        total_eq += rgb.decC2; 
                    }
                    //se agrupan los cortes Nicaragua para buscar sus retenciones
                    if ((user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24))
                    {
                        retenciones_id += sep + row.Cells[1].Text.ToString();
                        sep = ",";
                    }
                }
            }
        }

        //Si hay retenciones de los paises de Nicaragua se agregan al grid para que le puedan colocar su numero de retencion fiscal
        if (!retenciones_id.Equals("")) {
            DataSet ds = (DataSet)DB.GetRetencionesByCorte(retenciones_id, user);
            gv_retenciones.DataSource = ds.Tables["ret"];
            gv_retenciones.Columns[0].Visible = false;
            gv_retenciones.Columns[6].Visible = false;
            gv_retenciones.Columns[7].Visible = false;
            gv_retenciones.Columns[8].Visible = false;
            gv_retenciones.Columns[9].Visible = false;
            gv_retenciones.DataBind();
        }

        //Obteniendo Cargos Adicionales y Convirtiendo su equivalente
        if (!tb_cargoadicional.Text.Equals("")) cargos_adicionales = decimal.Parse(tb_cargoadicional.Text);

        if (monID == 8)
        {
            cargos_adicionales_eq = cargos_adicionales * user.pais.TipoCambio;
        }
        else
        {
            cargos_adicionales_eq = cargos_adicionales / user.pais.TipoCambio;
        }

        //Obteniendo el Anticipo y Convirtiendo su equivalente
        if (int.Parse(lb_transaccion.SelectedValue) == 43)
        {
            total = decimal.Parse(tb_anticipo.Text.ToString());
            if (monID == 8)
            {
                total_eq = total * user.pais.TipoCambio;
            }
            else
            {
                total_eq = total / user.pais.TipoCambio;
            }
        }

        total += cargos_adicionales;
        total_eq += cargos_adicionales_eq;
        tb_valor.Text = total.ToString("#,#.00");
        tb_valor_equivalente.Text = total_eq.ToString("#,#.00");
    }
    protected void tb_anticipo_TextChanged(object sender, EventArgs e)
    {
        decimal cargos_adicionales = 0;
        decimal cargos_adicionales_eq = 0;
        decimal total = 0;
        decimal total_eq = 0;
        int monID = int.Parse(lb_moneda.SelectedValue);
        if (!tb_cargoadicional.Text.Equals("")) cargos_adicionales = decimal.Parse(tb_cargoadicional.Text);
        if (monID == 8)
        {
            cargos_adicionales_eq = cargos_adicionales * user.pais.TipoCambio;
        }
        else
        {
            cargos_adicionales_eq = cargos_adicionales / user.pais.TipoCambio;
        }
        //Obteniendo el Anticipo y Convirtiendo su equivalente
        if ((tb_anticipo.Text != null && !tb_anticipo.Text.Equals(""))) 
        {
            if ((int.Parse(lb_transaccion.SelectedValue) == 43) || (int.Parse(lb_transaccion.SelectedValue) == 74) || (int.Parse(lb_transaccion.SelectedValue) == 75) || (int.Parse(lb_transaccion.SelectedValue) == 76) || (int.Parse(lb_transaccion.SelectedValue) == 77) || (int.Parse(lb_transaccion.SelectedValue) == 109))
            {
                total = decimal.Parse(tb_anticipo.Text.ToString());
                if (monID == 8)
                {
                    total_eq = total * user.pais.TipoCambio;
                }
                else
                {
                    total_eq = total / user.pais.TipoCambio;
                }
                total += cargos_adicionales;
                total_eq += cargos_adicionales_eq;
                tb_valor.Text = total.ToString("#,#.00");
                tb_valor_equivalente.Text = total_eq.ToString("#,#.00");
            }
        }
    }
    protected bool Validar_Restricciones(string sender)
    {
        bool resultado = false;
        bool paiR = DB.Validar_Pais_Restringido(user);
        if (paiR == true)
        {
            #region NO EMITIR PAGO MANUAL AGENTE ECONO
            if (int.Parse(lb_transaccion.SelectedValue) == 75)
            {
                if ((DB.Validar_Restriccion_Activa(user, 6, 26) == true) && (sender == "Validar_Econocaribe"))
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
            }
            #endregion
        }
        return resultado;
    }
    protected void btn_buscar_soas_Click(object sender, EventArgs e)
    {
        if (lb_transaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Transaccion a Realizar.");
            return;
        }
        if ((tb_agenteID.Text == "0") || (tb_agentenombre.Text.Trim() == ""))
        {
            WebMsgBox.Show("Debe seleccionar el Proveedor a Utilizar");
            return;
        }
        if (lb_cuentas_bancarias.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione el Banco y Cuenta Bancaria a Utilizar");
            return;
        }
        int proveedorID = int.Parse(tb_agenteID.Text);
        int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
        int monID = int.Parse(lb_moneda.SelectedValue);
        if (lb_tipopersona.SelectedValue.Equals("4")) //proveedor
        {
            ArrayList Arr = null;
            Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            ArrayList Arr = null;
            Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            ArrayList Arr = null;
            Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            ArrayList Arr = null;
            Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            if (lb_transaccion.SelectedValue.ToString().Equals("42")) // si la operacion es cajachica
            {
                ArrayList Arr = null;
                Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
                dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
                gv_cortes.DataSource = dt;
                gv_cortes.DataBind();
            }
        }
        else if (lb_tipopersona.SelectedValue.Equals("12"))//comisiones
        {
            if (lb_transaccion.SelectedValue.ToString().Equals("122")) // si la operacion es cheque a comisiones
            {
                ArrayList Arr = null;
                user.Moneda = int.Parse(lb_moneda.SelectedValue);
                Arr = DB.ObtenerCorteComisiones(proveedorID, user);
                dt = (DataTable)Utility.fillGridView("cortecomisiones", Arr);
                gv_cortes.DataSource = dt;
                gv_cortes.DataBind();
            }
        }
        else if (lb_tipopersona.SelectedValue.Equals("3"))//Clientes
        {
            ArrayList Arr = null;
            Arr = DB.getRCPT_pendientes(proveedorID, user, monID);
            dt = (DataTable)Utility.fillGridView("recibosclientes_cheques", Arr);
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("0"))//Retenciones
        {
            ArrayList Arr = null;
            Arr = DB.getRetencionbyProveedor(proveedorID, proveedortype, monID, 1);
            dt = (DataTable)Utility.fillGridView("retencionproveedor", Arr);
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        {
            ArrayList Arr = null;
            Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
        }
    }
}
