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
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
        }
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean proveedor = null;

        if (!Page.IsPostBack) {
            obtengo_listas();
            lb_bancos_SelectedIndexChanged(sender, e);
        }        
    }

    protected void obtengo_listas() 
    {
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
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='"+user.ID+"' and uop_pai_id="+user.PaisID+" and uop_ttt_id=ttt_id and ttt_template='pop_transferencias_ag.aspx'");
        lb_transaccion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        lb_transaccion.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_transaccion.Items.Add(item);
        }
        //obtengo Transacciones de Anticipos
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='transferencia_anticipos.aspx'");
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
        if (lb_tipopersona.SelectedValue.Equals("4")) //proveedor
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_contacto.Text = "";
            tb_contacto.Visible = false;
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[8].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            #region Validacion Grupo Empresas
            if (int.Parse(lb_transaccion.SelectedValue) == 91)
            {
                if ((DB.Validar_Restriccion_Activa(user, 19, 29) == true))
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
        else if (lb_tipopersona.SelectedValue.Equals("3"))//Clientes
        { 
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("0"))//Retenciones
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
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
        
        if (tb_agenteID.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Es necesario especificar el codigo del proveedor");
            return;
        }
        if (tb_agenteID.Text.Trim().Equals("0"))
        {
            WebMsgBox.Show("Es necesario especificar el codigo del proveedor");
            return;
        }
        else if (lb_cuentas_bancarias.SelectedItem.Text.Equals("Seleccione..."))
        {
            WebMsgBox.Show("Debe seleccionar una cuenta bancaria");
            return;
        }
        else if (tb_chequeNo.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar el numero de transferencia");
                return;
        }
        else if (tb_fecha.Text.Equals(""))
        {
            WebMsgBox.Show("Es necesario ingresar la fecha de la Transferencia");
            return;
        }
        else if (tb_valor.Text.Equals(""))
        {
            WebMsgBox.Show("El valor de la transferencia no puede estar vacío");
            return;
        }
        else if (decimal.Parse(tb_valor.Text) == 0)
        {
            WebMsgBox.Show("El valor de la transferencia debe ser mayor a 0");
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
        if ((tranID != 89) && (tranID != 90) && (tranID != 91) && (tranID != 92) && (tranID != 93) && (tranID != 111))//Solo si no es Anticipo 
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
        rgb.Fecha_Hora = lb_fecha_hora.Text;
        rgb.intC1 = int.Parse(lb_moneda.SelectedValue);//moneda
        rgb.strC7 = tb_chequeNo.Text;//No Transferencia
        rgb.strC1 = lb_cuentas_bancarias.SelectedValue;//cuenta
        rgb.strC2 = DB.DateFormat(tb_fecha.Text);//fecha transferencia
        rgb.decC1 = valor;//valor
        rgb.decC4 = valor_equivalente;//valor equivalente
        rgb.decC2 = cargoadicional;//cargo adicional
        rgb.strC3 = tb_acreditado.Text;//nombre transferencia
        rgb.strC4 = tb_motivo.Text;// nota para el cheque
        rgb.intC3 = contaID;// tipo contabilidad
        rgb.intC4 = tranID;// tipo transaccion
        rgb.douC1 = double.Parse(tb_agenteID.Text);
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
       
        int matOpID = DB.getMatrizOperacionID(tranID, monID, paisID, contaID);
        ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);
        rgb.arr1 = ctas;
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

        //************************************************************* calculo el iva del cargo adicional, solo Panama
        if ((monID != 8) && (user.PaisID == 6))
        {
            rgb.decC3 = Math.Round(rgb.decC2 * user.pais.Impuesto, 2);
        }
        else
        {
            rgb.decC3 = 0;
        }
        ArrayList result = DB.AplicoTransferencia(rgb, user);
        //if (int.Parse(result[0].ToString()) != -1 && int.Parse(result[0].ToString()) > 0 && result != null)
        if (result != null && int.Parse(result[0].ToString()) != -1)
        {
            //WebMsgBox.Show("Se guardo correctamente la transferencia " + tb_chequeNo.Text);
            WebMsgBox.Show("La Transferencia No.: " + tb_chequeNo.Text + " fue guardada exitosamente");

            //Mostrando la Partida contable generada
            gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[2].ToString()), 19, 0);
            gv_detalle_partida.DataBind();
            gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
            
            gv_cortes.Enabled = false;
            btn_calcular.Enabled = false;
            bt_guardar.Enabled = false;
            btn_imprimir.Enabled = true;
            return;
        }
        else
        {
            WebMsgBox.Show("Existió un problema al tratar de guardar la información, por favor intente de nuevo");
            btn_imprimir.Enabled = false;
            bt_guardar.Enabled = true;
            btn_calcular.Enabled = true;
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

        ListItem item = null;
        lb_cuentas_bancarias.Items.Clear();
        lb_moneda.SelectedValue = "0";
        item = new ListItem("Seleccione...", "0");
        item.Selected = true;
        lb_cuentas_bancarias.Items.Add(item);
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
            tb_cargoadicional.Text = "";
            tb_anticipo_TextChanged(sender, e);
        }
        gv_cortes.DataBind();
        gv_retenciones.DataBind();
    }
    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('../invoice/print_cheque.aspx?ctaID=" + lb_cuentas_bancarias.SelectedValue + "&correlativo="+tb_chequeNo.Text+"','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
    protected void chk_seleccion_CheckedChanged(object sender, EventArgs e)
    {
        int ban_cortes = 0;
        CheckBox chk_cortes;
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
        string retenciones_id = "", sep = "";

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
        if (!retenciones_id.Equals(""))
        {
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

        total += cargos_adicionales;
        total_eq += cargos_adicionales_eq;
        tb_valor.Text = total.ToString("#,#.00");
        tb_valor_equivalente.Text = total_eq.ToString("#,#.00");
    }
    protected void btn_calcular_Click(object sender, EventArgs e)
    {
        if (lb_cuentas_bancarias.SelectedItem.Text.Equals("Seleccione..."))
        {
            WebMsgBox.Show("Debe seleccionar una cuenta bancaria");
            return;
        }
        if ((lb_transaccion.SelectedValue.ToString() != "89") && (lb_transaccion.SelectedValue.ToString() != "90") && (lb_transaccion.SelectedValue.ToString() != "91") && (lb_transaccion.SelectedValue.ToString() != "92") && (lb_transaccion.SelectedValue.ToString() != "93"))
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
        if ((lb_transaccion.SelectedValue == "89") || (lb_transaccion.SelectedValue.ToString() == "90") || (lb_transaccion.SelectedValue.ToString() == "91") || (lb_transaccion.SelectedValue.ToString() == "92") || (lb_transaccion.SelectedValue.ToString() == "93"))
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
    protected void lb_transaccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!lb_transaccion.SelectedItem.Text.Equals("Seleccione..."))
        {
            int tranID = int.Parse(lb_transaccion.SelectedValue);
            // proveedores
            if ((tranID == 23))
            { 
                lb_tipopersona.SelectedValue = "4";
                lb_provision_retencion.Text = "8";
            }
            // navieras
            else if (tranID == 37) { 
                lb_tipopersona.SelectedValue = "5";
                lb_provision_retencion.Text = "17";
            }
            // Lineas Aereas
            else if (tranID == 38) { 
                lb_tipopersona.SelectedValue = "6";
                lb_provision_retencion.Text = "18";
            }
            // agentes
            else if (tranID == 10) { 
                lb_tipopersona.SelectedValue = "2";
                lb_provision_retencion.Text = "15";
            }
            // clientes
            else if (tranID == 41) { 
                lb_tipopersona.SelectedValue = "3"; 
            }
            // caja chica
            else if (tranID == 39)
            {
                lb_tipopersona.SelectedValue = "8";
                lb_provision_retencion.Text = "53";
            }
            //Transferencia Anticipo Caja Chica
            else if (tranID == 89)
            {
                lb_tipopersona.SelectedValue = "8";
            }
            //Transferencia Anticipo Proveedor
            else if (tranID == 90)
            {
                lb_tipopersona.SelectedValue = "4";
            }
            //Transferencia Anticipo Agentes
            else if (tranID == 91)
            {
                lb_tipopersona.SelectedValue = "2";
            }
            //Transferencia Anticipo Navieras
            else if (tranID == 92)
            {
                lb_tipopersona.SelectedValue = "5";
            }
            //Transferencia Anticipo Lineas Aereas
            else if (tranID == 93)
            {
                lb_tipopersona.SelectedValue = "6";
            }
            //Transferencia Intercompanys
            else if ((tranID == 22))
            {
                lb_tipopersona.SelectedValue = "10";
                lb_provision_retencion.Text = "8";
            }
            //Transferencia Anticipo Intercompanys
            else if ((tranID == 111))
            {
                lb_tipopersona.SelectedValue = "10";
                lb_provision_retencion.Text = "8";
            }
            if ((tranID == 89) || (tranID == 90) || (tranID == 91) || (tranID == 92) || (tranID == 93) || (tranID == 111))
            {
                tb_anticipo.Text = "0.00";
                gv_cortes.Visible = false;
                lb_anticipo.Visible = true;
                tb_anticipo.Visible = true;
            }
            else
            {
                tb_anticipo.Text = "0.00";
                gv_cortes.Visible = true;
                lb_anticipo.Visible = false;
                tb_anticipo.Visible = false;
            }
        }
        #region Limpiar Tipo de Persona
        tb_agenteID.Text = "";
        tb_agentenombre.Text = "";
        tb_contacto.Text = "";
        tb_direccion.Text = "";
        tb_telefono.Text = "";
        tb_correoelectronico.Text = "";
        tb_acreditado.Text = "";
        tb_contacto.Text = "";
        #endregion
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
            if ((int.Parse(lb_transaccion.SelectedValue) == 89) || (int.Parse(lb_transaccion.SelectedValue) == 90) || (int.Parse(lb_transaccion.SelectedValue) == 91) || (int.Parse(lb_transaccion.SelectedValue) == 92) || (int.Parse(lb_transaccion.SelectedValue) == 93) || (int.Parse(lb_transaccion.SelectedValue) == 111))
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
            if (int.Parse(lb_transaccion.SelectedValue) == 91)
            {
                if ((DB.Validar_Restriccion_Activa(user, 19, 26) == true) && (sender == "Validar_Econocaribe"))
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
        //ArrayList Arr = null;
        //Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
        //dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
        //gv_cortes.DataSource = dt;
        //gv_cortes.DataBind();
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
            ArrayList Arr = null;
            Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
            dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
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