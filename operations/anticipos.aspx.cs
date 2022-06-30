using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class operations_anticipos : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    decimal Total_Documentos_X_Pagar = 0;
    string Mensaje = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
            Response.Redirect("../default.aspx");

        user = (UsuarioBean)Session["usuario"];

        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 131072) == 131072))
            Response.Redirect("index.aspx");
        if (!Page.IsPostBack)
        {
            obtengo_listas();
            drp_banco_SelectedIndexChanged(sender, e);
            drp_banco2_SelectedIndexChanged(sender, e);
            drp_tipo_transaccion_SelectedIndexChanged(sender, e);
            pnl_detalle.Visible = false;
        }
        if ((lbl_total_cheques_operados.Text == "0.00") && (lbl_total_cortes_operados.Text == "0.00"))
        {
            pnl_detalle.Visible = false;
        }
        else
        {
            pnl_detalle.Visible = true;
        }
    }
    protected void obtengo_listas()
    {
        ArrayList arr = null;
        ListItem item = null;
        drp_moneda.Items.Clear();
        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_moneda.Items.Add(item);
            drp_moneda2.Items.Add(item);
        }
        arr = (ArrayList)DB.getBancosXPais(user.PaisID, 1);
        drp_banco.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_banco.Items.Add(item);
            drp_banco2.Items.Add(item);
        }
        item = new ListItem("Seleccione...", "0");
        drp_banco_cuenta.Items.Add(item);
        drp_banco_cuenta2.Items.Add(item);
        // obtengo las transacciones
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='liquidacion_anticipos.aspx'");
        drp_tipo_transaccion.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_tipo_transaccion.Items.Add(item);
        }
        //obtengo Transacciones de Anticipos
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='liquidacion_transferencias.aspx'");
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_tipo_transaccion.Items.Add(item);
        }
        arr = null;
        arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 24, user, 1);//1 porque es el tipo de documento para facturacion
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            drp_serie.Items.Add(item);
        }
    }
    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        //user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo_proveedor.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        if (drp_tipo_persona.SelectedValue.Equals("3"))
        { // cliente
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(a.nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and a.id_cliente=" + tb_codigo.Text; else where += "a.id_cliente=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and a.codigo_tributario='" + tb_nitb.Text + "'"; else where += "a.codigo_tributario='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getClientes(where, user, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
        }
        else if (drp_tipo_persona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (drp_tipo_persona.SelectedValue.Equals("2"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit a un agente");
                return;
            }

            //if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(where, ""); //Agente
            dt = (DataTable)Utility.fillGridView("Agente", Arr);
        }
        else if (drp_tipo_persona.SelectedValue.Equals("5"))
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
        else if (drp_tipo_persona.SelectedValue.Equals("6"))
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
        else if (drp_tipo_persona.SelectedValue.Equals("8"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit usuario de caja chica");
                return;
            }
            //if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo.Text; else where += " and id_usuario=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);
        }
        else if (drp_tipo_persona.SelectedValue.Equals("10"))
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
        if (drp_tipo_persona.SelectedValue.Equals("4"))//proveedor
        {
            tb_codigo_proveedor.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        }
        else if (drp_tipo_persona.SelectedValue.Equals("2"))//agente
        {
            tb_codigo_proveedor.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (drp_tipo_persona.SelectedValue.Equals("5"))//naviera
        {
            tb_codigo_proveedor.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (drp_tipo_persona.SelectedValue.Equals("6"))//linea aerea
        {
            tb_codigo_proveedor.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (drp_tipo_persona.SelectedValue.Equals("8"))//Caja Chica
        {
            tb_codigo_proveedor.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (drp_tipo_persona.SelectedValue.Equals("10"))//Intercompanys
        {
            tb_codigo_proveedor.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
        }
    }
    protected void drp_banco_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        drp_banco_cuenta.Items.Clear();
        item = new ListItem("Seleccione...", " ");
        item.Selected = true;
        drp_banco_cuenta.Items.Add(item);
        
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(drp_banco.SelectedValue), user,1);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            drp_banco_cuenta.Items.Add(item);
        }

        gv_documentos.DataBind();
        gv_cortes.DataBind();
        Calcular_Saldos();
    }
    protected void btn_visualizar_Click(object sender, EventArgs e)
    {
        int ttrID = 0;
        #region Determinar TTRID
        if (drp_tipo_transaccion.Items.Count == 0)
        {
            WebMsgBox.Show("No existen Transacciones Disponibles para poder Liquidar");
            return;
        }
        else
        {
            if ((drp_tipo_transaccion.SelectedValue == "78") || (drp_tipo_transaccion.SelectedValue == "79") || (drp_tipo_transaccion.SelectedValue == "80") || (drp_tipo_transaccion.SelectedValue == "81") || (drp_tipo_transaccion.SelectedValue == "82") || (drp_tipo_transaccion.SelectedValue == "110"))
            {
                ttrID = 6;
            }
            else if ((drp_tipo_transaccion.SelectedValue == "94") || (drp_tipo_transaccion.SelectedValue == "95") || (drp_tipo_transaccion.SelectedValue == "96") || (drp_tipo_transaccion.SelectedValue == "97") || (drp_tipo_transaccion.SelectedValue == "98") || (drp_tipo_transaccion.SelectedValue == "112"))
            {
                ttrID = 19;
            }
        }
        #endregion
        if (tb_codigo_proveedor.Text == "0")
        {
            WebMsgBox.Show("Debe Seleccionar un Proveedor");
            return;
        }
        else if (drp_banco_cuenta.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe Seleccionar una Cuenta Bancaria");
            return;
        }
        else
        {
            ArrayList arr_documentos = (ArrayList)DB.Get_Documentos_Anticipados(user, int.Parse(drp_banco.SelectedValue.ToString()), drp_banco_cuenta.SelectedValue, int.Parse(drp_moneda.SelectedValue), int.Parse(drp_tipo_persona.SelectedValue), int.Parse(tb_codigo_proveedor.Text), ttrID);
            #region Cargar Documentos Anticipados
            DataTable dt_documentos = new DataTable();
            dt_documentos.Columns.Add("ttr_id");
            dt_documentos.Columns.Add("ref_id");
            dt_documentos.Columns.Add("Tipo");
            dt_documentos.Columns.Add("Numero");
            dt_documentos.Columns.Add("Fecha");
            dt_documentos.Columns.Add("Total");
            foreach(RE_GenericBean Bean in arr_documentos)
            {
                object[] obj = { Bean.strC1, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6 };
                dt_documentos.Rows.Add(obj);
            }
            gv_documentos.DataSource = dt_documentos;
            gv_documentos.DataBind();
            #endregion
            ArrayList arr_cortes = (ArrayList)DB.Get_SOAS_Pendientes_Pago(user, int.Parse(drp_moneda.SelectedValue), int.Parse(drp_tipo_persona.SelectedValue), int.Parse(tb_codigo_proveedor.Text));
            #region Cargar Documentos Anticipados
            DataTable dt_cortes = new DataTable();
            dt_cortes.Columns.Add("ttr_id");
            dt_cortes.Columns.Add("ref_id");
            dt_cortes.Columns.Add("Tipo");
            dt_cortes.Columns.Add("Serie");
            dt_cortes.Columns.Add("Correlativo");
            dt_cortes.Columns.Add("Fecha");
            dt_cortes.Columns.Add("Total");
            foreach (RE_GenericBean Bean in arr_cortes)
            {
                object[] obj = { Bean.strC1, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC7 };
                dt_cortes.Rows.Add(obj);
            }
            gv_cortes.DataSource = dt_cortes;
            gv_cortes.DataBind();
            #endregion
            Calcular_Saldos();  
        }
        if ((lbl_total_cheques_operados.Text == "0.00") && (lbl_total_cortes_operados.Text == "0.00"))
        {
            pnl_detalle.Visible = false;
        }
        else
        {
            pnl_detalle.Visible = true;
        }
    }
    protected void Calcular_Saldos()
    {
        Label lb;
        decimal Total = 0;
        #region Calcular Total Pendientes de Liquidar
        foreach (GridViewRow row in gv_documentos.Rows)
        {
            lb = (Label)row.FindControl("lbl_total");
            Total += decimal.Parse(lb.Text);
        }
        lbl_total_cheques_operados.Text = (Total).ToString("#,#.00#;(#,#.00#)");
        #endregion
        Total = 0;
        #region Calcular Total Cortes Pendientes de Liquidar
        foreach (GridViewRow row in gv_cortes.Rows)
        {
            lb = (Label)row.FindControl("lbl_total");
            Total += decimal.Parse(lb.Text);
        }
        foreach (GridViewRow row in gv_depositos.Rows)
        {
            lb = (Label)row.FindControl("lbl_total");
            Total += decimal.Parse(lb.Text);
        }
        lbl_total_cortes_operados.Text = (Total).ToString("#,#.00#;(#,#.00#)");
        #endregion
    }
    protected void chk_asignar_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk;
        Label lb;
        decimal Total = 0;
        decimal Disponible = 0;
        foreach (GridViewRow row in gv_documentos.Rows)
        {
            chk = (CheckBox)row.FindControl("chk_asignar");
            lb = (Label)row.FindControl("lbl_total");
            if (chk.Checked == true)
            {
                Total += decimal.Parse(lb.Text);
            }
        }
        lbl_total_cheques_liquidar.Text = (Total).ToString("#,#.00#;(#,#.00#)");
        Disponible = decimal.Parse(lbl_total_cheques_operados.Text) - decimal.Parse(lbl_total_cheques_liquidar.Text);
        lbl_total_cheques_disponibles.Text = (Disponible).ToString("#,#.00#;(#,#.00#)");
    }
    protected void chk_asignar_CheckedChanged1(object sender, EventArgs e)
    {
        CheckBox chk;
        Label lb;
        decimal Total = 0;
        decimal Disponible = 0;
        foreach (GridViewRow row in gv_cortes.Rows)
        {
            chk = (CheckBox)row.FindControl("chk_asignar");
            lb = (Label)row.FindControl("lbl_total");
            if (chk.Checked == true)
            {
                Total += decimal.Parse(lb.Text);
            }
        }
        //Depositos
        foreach (GridViewRow row in gv_depositos.Rows)
        {
            chk = (CheckBox)row.FindControl("chk_asignar");
            lb = (Label)row.FindControl("lbl_total");
            if (chk.Checked == true)
            {
                Total += decimal.Parse(lb.Text);
            }
        }
        lbl_total_cortes_liquidar.Text = (Total).ToString("#,#.00#;(#,#.00#)");
        Disponible = decimal.Parse(lbl_total_cortes_operados.Text) - decimal.Parse(lbl_total_cortes_liquidar.Text);
        lbl_total_cortes_disponibles.Text = (Disponible).ToString("#,#.00#;(#,#.00#)");
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        CheckBox chk;
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
        decimal Total_Cheques_Liquidar = 0;
        decimal Total_Cortes_Liquidar = 0;
        decimal Total_Cortes_Chequeados = 0;
        Total_Cheques_Liquidar = decimal.Parse(lbl_total_cheques_liquidar.Text);
        Total_Cortes_Liquidar = decimal.Parse(lbl_total_cortes_liquidar.Text);
        if (drp_serie.Items.Count == 0)
        {
            WebMsgBox.Show("No se puede Generar una Liquidacion sin Serie");
            return;
        }
        if (tb_codigo_proveedor.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe Seleccionar un Proveedor");
            return;
        }
        if (tb_codigo_proveedor.Text.Trim() == "0")
        {
            WebMsgBox.Show("Debe Seleccionar un Proveedor");
            return;
        }
        if (drp_banco_cuenta.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe Seleccionar una Cuenta Bancaria");
            return;
        }
        if ((Total_Cheques_Liquidar > 0) && (Total_Cortes_Liquidar > 0) && (Total_Cheques_Liquidar == Total_Cortes_Liquidar))
        {
            #region Validar Cortes Chequeados
            int Cantidad_Cortes = 0;
            foreach (GridViewRow row in gv_cortes.Rows)
            {
                chk = (CheckBox)row.FindControl("chk_asignar");
                if (chk.Checked == true)
                {
                    Cantidad_Cortes++;
                }
            }
            #endregion
            #region Obtener Depositos a Liquidar
            int Cantidad_Depositos = 0;
            foreach (GridViewRow row in gv_depositos.Rows)
            {
                chk = (CheckBox)row.FindControl("chk_asignar");
                if (chk.Checked == true)
                {
                    Cantidad_Depositos++;
                }
            }
            #endregion
            #region Validar Cheques marcados
            int Cantidad_Cheques = 0;
            foreach (GridViewRow row in gv_documentos.Rows)
            {
                lb3 = (Label)row.FindControl("lbl_tipo");
                chk = (CheckBox)row.FindControl("chk_asignar");
                if ((chk.Checked == true) && (lb3.Text == "CH"))
                {
                    Cantidad_Cheques++;
                }
            }
            #endregion
            #region Validar Transferencias marcadas
            int Cantidad_Transferencias = 0;
            foreach (GridViewRow row in gv_documentos.Rows)
            {
                lb3 = (Label)row.FindControl("lbl_tipo");
                chk = (CheckBox)row.FindControl("chk_asignar");
                if ((chk.Checked == true) && (lb3.Text == "TR"))
                {
                    Cantidad_Transferencias++;
                }
            }
            #endregion
            if ((Cantidad_Cortes > 0) ||(Cantidad_Depositos > 0))
            {
                RE_GenericBean Bean = new RE_GenericBean();
                Bean.intC1 = int.Parse(tb_codigo_proveedor.Text);
                Bean.intC2 = int.Parse(drp_tipo_persona.SelectedValue);
                Bean.strC1 = drp_serie.SelectedItem.Text;
                Bean.strC3 = user.ID;
                Bean.decC1 = decimal.Parse(lbl_total_cheques_liquidar.Text);
                if (int.Parse(drp_moneda.SelectedValue.ToString()) == 8)
                {
                    Bean.decC2 = Math.Round((Bean.decC1 * user.pais.TipoCambio), 2);
                }
                else
                {
                    Bean.decC2 = Math.Round((Bean.decC1 / user.pais.TipoCambio), 2);
                }
                Bean.intC3 = user.PaisID;
                Bean.intC4 = user.contaID;
                Bean.intC5 = user.SucursalID;
                Bean.strC4 = tb_observaciones.Text;
                Bean.intC6 = int.Parse(drp_banco.SelectedValue);
                Bean.strC5 = drp_banco_cuenta.SelectedItem.Text;
                Bean.intC7 = int.Parse(drp_moneda.SelectedValue);
                Bean.intC8 = int.Parse(drp_tipo_transaccion.SelectedValue);
                Bean.strC6 = tb_nombre.Text;
                #region Obtener Cheques a Liquidar
                foreach (GridViewRow row in gv_documentos.Rows)
                {
                    chk = (CheckBox)row.FindControl("chk_asignar");
                    lb1 = (Label)row.FindControl("lbl_ttr_id");
                    lb2 = (Label)row.FindControl("lbl_ref_id");
                    lb3 = (Label)row.FindControl("lbl_tipo");
                    lb4 = (Label)row.FindControl("lbl_numero");
                    lb5 = (Label)row.FindControl("lbl_fecha");
                    lb6 = (Label)row.FindControl("lbl_total");
                    RE_GenericBean Bean_Ch = null;
                    if ((chk.Checked == true) && (lb3.Text == "CH"))
                    {
                        Bean_Ch = new RE_GenericBean();
                        Bean_Ch.intC1 = int.Parse(lb1.Text);
                        Bean_Ch.intC2 = int.Parse(lb2.Text);
                        Bean_Ch.strC1 = lb3.Text;
                        Bean_Ch.strC2 = lb4.Text;
                        Bean_Ch.strC3 = lb5.Text;
                        Bean_Ch.decC1 = decimal.Parse(lb6.Text);
                        Bean_Ch.decC2 = 0;
                        Bean.arr1.Add(Bean_Ch);
                        Bean.arr2.Add(Bean_Ch.intC2);//ID Cheques a Liquidar
                    }
                }
                #endregion
                #region Obtener Transferencias a Liquidar
                foreach (GridViewRow row in gv_documentos.Rows)
                {
                    chk = (CheckBox)row.FindControl("chk_asignar");
                    lb1 = (Label)row.FindControl("lbl_ttr_id");
                    lb2 = (Label)row.FindControl("lbl_ref_id");
                    lb3 = (Label)row.FindControl("lbl_tipo");
                    lb4 = (Label)row.FindControl("lbl_numero");
                    lb5 = (Label)row.FindControl("lbl_fecha");
                    lb6 = (Label)row.FindControl("lbl_total");
                    RE_GenericBean Bean_TR = null;
                    if ((chk.Checked == true) && (lb3.Text == "TR"))
                    {
                        Bean_TR = new RE_GenericBean();
                        Bean_TR.intC1 = int.Parse(lb1.Text);
                        Bean_TR.intC2 = int.Parse(lb2.Text);
                        Bean_TR.strC1 = lb3.Text;
                        Bean_TR.strC2 = lb4.Text;
                        Bean_TR.strC3 = lb5.Text;
                        Bean_TR.decC1 = decimal.Parse(lb6.Text);
                        Bean_TR.decC2 = 0;
                        Bean.arr1.Add(Bean_TR);
                        Bean.arr2.Add(Bean_TR.intC2);//ID Transferencias a Liquidar
                    }
                }
                #endregion
                #region Obtener Cortes a Liquidar
                foreach (GridViewRow row in gv_cortes.Rows)
                {
                    chk = (CheckBox)row.FindControl("chk_asignar");
                    lb1 = (Label)row.FindControl("lbl_ttr_id");
                    lb2 = (Label)row.FindControl("lbl_ref_id");
                    lb3 = (Label)row.FindControl("lbl_tipo");
                    lb4 = (Label)row.FindControl("lbl_serie");
                    lb5 = (Label)row.FindControl("lbl_correlativo");
                    lb6 = (Label)row.FindControl("lbl_fecha");
                    lb7 = (Label)row.FindControl("lbl_total");
                    RE_GenericBean Bean_Soas = null;
                    if (chk.Checked == true)
                    {
                        Bean_Soas = new RE_GenericBean();
                        Bean_Soas.intC1 = int.Parse(lb1.Text);
                        Bean_Soas.intC2 = int.Parse(lb2.Text);
                        Bean_Soas.strC1 = lb3.Text;
                        Bean_Soas.strC2 = lb4.Text + " - " + lb5.Text;
                        Bean_Soas.strC3 = lb6.Text;
                        Bean_Soas.decC1 = 0;
                        Bean_Soas.decC2 = decimal.Parse(lb7.Text);
                        Total_Cortes_Chequeados += decimal.Parse(lb7.Text);
                        Bean.arr1.Add(Bean_Soas);
                        Bean.arr3.Add(Bean_Soas.intC2);//ID Cortes a Liquidar
                    }
                }
                #endregion
                #region Obtener Depositos a Liquidar
                foreach (GridViewRow row in gv_depositos.Rows)
                {
                    chk = (CheckBox)row.FindControl("chk_asignar");
                    lb1 = (Label)row.FindControl("lbl_ttr_id");
                    lb2 = (Label)row.FindControl("lbl_ref_id");
                    lb3 = (Label)row.FindControl("lbl_tipo");
                    lb4 = (Label)row.FindControl("lbl_bancoid");
                    lb5 = (Label)row.FindControl("lbl_banco");
                    lb6 = (Label)row.FindControl("lbl_banco_cuenta");
                    lb7 = (Label)row.FindControl("lbl_referencia");
                    lb8 = (Label)row.FindControl("lbl_fecha");
                    lb9 = (Label)row.FindControl("lbl_total");
                    lb10 = (Label)row.FindControl("lbl_moneda");
                    RE_GenericBean Bean_Depositos = null;
                    if (chk.Checked == true)
                    {
                        Bean_Depositos = new RE_GenericBean();
                        Bean_Depositos.intC1 = int.Parse(lb1.Text);//TTR_ID
                        Bean_Depositos.intC2 = int.Parse(lb2.Text);//REF_ID
                        Bean_Depositos.strC1 = lb3.Text;//TIPO
                        Bean_Depositos.intC3 = int.Parse(lb4.Text);//BancoID
                        Bean_Depositos.strC2 = lb5.Text;//Banco
                        Bean_Depositos.strC3 = lb6.Text;//Cuenta
                        Bean_Depositos.strC4 = lb7.Text;//Referencia
                        Bean_Depositos.strC5 = lb8.Text;//Fecha
                        Bean_Depositos.decC1 = 0;
                        Bean_Depositos.decC2 = decimal.Parse(lb9.Text);//Total
                        Bean_Depositos.strC6 = tb_observaciones.Text;//Obseraciones
                        Bean_Depositos.intC4 = int.Parse(lb10.Text);//MonedaID
                        Bean_Depositos.boolC1 = true;//Ingreso
                        if (Bean_Depositos.intC4 == 8)
                        {
                            Bean_Depositos.decC3 = Math.Round((Bean_Depositos.decC2 * user.pais.TipoCambio), 2);
                        }
                        else
                        {
                            Bean_Depositos.decC3 = Math.Round((Bean_Depositos.decC2 / user.pais.TipoCambio), 2);
                        }
                        Bean_Depositos.intC5 = int.Parse(tb_codigo_proveedor.Text);//Codigo
                        Bean_Depositos.intC6 = int.Parse(drp_tipo_persona.SelectedValue);//Tipo de Persona
                        Bean_Depositos.intC7 = int.Parse(drp_tipo_transaccion.SelectedValue);//Tipo de Transaccion
                        Bean.arr4.Add(Bean_Depositos);//Depositos a Ingresar
                    }
                }
                #endregion
                Bean.decC3 = Total_Cortes_Chequeados;
                int matOpID = DB.getMatrizOperacionID(int.Parse(drp_tipo_transaccion.SelectedValue), int.Parse(drp_moneda.SelectedValue), user.PaisID, user.contaID);
                ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                RE_GenericBean Bean_Resultado = (RE_GenericBean)DB.Insertar_Liquidacion(user, Bean, ctas);
                if (Bean_Resultado == null)
                {
                    WebMsgBox.Show("Existio un Error al tratar de guardar la Liquidacion");
                    return;
                }
                else
                {
                    if (Bean_Resultado.intC3 > 0)//validar si hay retencioes
                    {
                        if (user.PaisID == 3 || user.PaisID == 23)
                        {
                            DB.AplicarChequeRetencion(Bean_Resultado.intC2, user, 24, Bean.intC7);
                            btn_imprimir_retencion.Enabled = true;
                        }
                    }
                    WebMsgBox.Show("La Liquidacion " + drp_serie.SelectedItem.Text + " - " + Bean_Resultado.intC1.ToString() + " fue grabada exitosamente");
                    tb_correlativo.Text = Bean_Resultado.intC1.ToString();
                    lbl_liquidacion_id.Text = Bean_Resultado.intC2.ToString();
                    gv_documentos.Enabled = false;
                    gv_cortes.Enabled = false;
                    gv_depositos.Enabled = false;
                    btn_visualizar.Enabled = false;
                    btn_agregar.Enabled = false;
                    btn_guardar.Enabled = false;
                }
            }
            else
            {
                
                WebMsgBox.Show("No puede Generar una Liquidacion sin liquidar al menos un Corte o un Deposito");
                return;
            }
        }
        else
        {
            if (Total_Cheques_Liquidar == 0)
            {
                WebMsgBox.Show("Debe Seleccionar al menos un Cheque");
                return;
            }
            if (Total_Cortes_Liquidar == 0)
            {
                WebMsgBox.Show("Debe Seleccionar al menos un Corte o un Deposito");
                return;
            }
            if (Total_Cheques_Liquidar != Total_Cortes_Liquidar)
            {
                WebMsgBox.Show("El Total en Cheques a Liquidar debe ser igual al Total en Cortes a Liquidar");
                return;
            }
        }
    }
    protected void drp_banco2_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        drp_banco_cuenta2.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        item.Selected = true;
        drp_banco_cuenta2.Items.Add(item);
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(drp_banco2.SelectedValue), user,1);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            drp_banco_cuenta2.Items.Add(item);
        }
    }
    protected void btn_agregar_Click(object sender, EventArgs e)
    {
        if (drp_moneda.SelectedValue != drp_moneda2.SelectedValue)
        {
            gv_depositos.DataBind();
            drp_banco2_SelectedIndexChanged(sender, e);
            tb_monto.Text = "";
            tb_fecha.Text = "";
            tb_no_referencia.Text = "";
            Calcular_Saldos();
            chk_asignar_CheckedChanged1(sender, e);
            Mensaje = "alert('No se pueden agregar Depositos con moneda diferente a la de los Cheques');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "BAW", Mensaje, true);
            return;
        }
        else
        {
            CheckBox chk;
            Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
            DataTable dt_depositos = new DataTable();
            #region Validaciones de Deposito
            if (drp_banco2.SelectedValue.Equals("0"))
            {
                Mensaje = "alert('Debe seleccionar una cuenta bancaria');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "BAW", Mensaje, true);
                return;
            }
            if (drp_banco_cuenta2.SelectedValue.Trim().Equals("0"))
            {
                Mensaje = "alert('Debe seleccionar el No. de Cuenta');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "BAW", Mensaje, true);
                return;
            }
            if (tb_no_referencia.Text.Trim() == "")
            {
                Mensaje = "alert('Debe ingresar el Numero de Referencia');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "BAW", Mensaje, true);
                return;
            }
            if (DB.existDeposito(int.Parse(drp_banco2.SelectedValue), drp_banco_cuenta2.SelectedValue, tb_no_referencia.Text.Trim()))
            {
                Mensaje = "alert('No se puede ingresar este deposito ya que fue ingresado anteriormente');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "BAW", Mensaje, true);
                return;
            }
            if (tb_fecha.Text == "")
            {
                Mensaje = "alert('Debe seleccionar la Fecha del Deposito');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "BAW", Mensaje, true);
                return;
            }
            if (tb_monto.Text == "")
            {
                Mensaje = "alert('Debe Ingresar el Monto a Depositar');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "BAW", Mensaje, true);
                return;
            }
            #endregion
            dt_depositos.Columns.Add("ttr_id");
            dt_depositos.Columns.Add("ref_id");
            dt_depositos.Columns.Add("Tipo");
            dt_depositos.Columns.Add("BancoID");
            dt_depositos.Columns.Add("Banco");
            dt_depositos.Columns.Add("Cuenta");
            dt_depositos.Columns.Add("Referencia");
            dt_depositos.Columns.Add("Fecha");
            dt_depositos.Columns.Add("Total");
            dt_depositos.Columns.Add("Moneda");
            foreach (GridViewRow row in gv_depositos.Rows)
            {
                chk = (CheckBox)row.FindControl("chk_asignar");
                lb1 = (Label)row.FindControl("lbl_ttr_id");
                lb2 = (Label)row.FindControl("lbl_ref_id");
                lb3 = (Label)row.FindControl("lbl_tipo");
                lb4 = (Label)row.FindControl("lbl_bancoid");
                lb5 = (Label)row.FindControl("lbl_banco");
                lb6 = (Label)row.FindControl("lbl_banco_cuenta");
                lb7 = (Label)row.FindControl("lbl_referencia");
                lb8 = (Label)row.FindControl("lbl_fecha");
                lb9 = (Label)row.FindControl("lbl_total");
                lb10 = (Label)row.FindControl("lbl_moneda");
                object[] ObjArr = { lb1.Text, lb2.Text, lb3.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, lb8.Text, lb9.Text, lb10.Text };
                dt_depositos.Rows.Add(ObjArr);
            }
            object[] objArr_new = { "17", "0", "DP", drp_banco2.SelectedValue, drp_banco2.SelectedItem.Text, drp_banco_cuenta2.SelectedItem.Text, tb_no_referencia.Text, DB.DateFormat(tb_fecha.Text), tb_monto.Text, drp_moneda.SelectedValue.ToString() };
            dt_depositos.Rows.Add(objArr_new);
            gv_depositos.DataSource = dt_depositos;
            gv_depositos.DataBind();
            drp_banco2_SelectedIndexChanged(sender, e);
            tb_monto.Text = "";
            tb_fecha.Text = "";
            tb_no_referencia.Text = "";
            Calcular_Saldos();
            chk_asignar_CheckedChanged1(sender, e);
        }
    }
    protected void gv_depositos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
        DataTable dt_depositos = new DataTable();
        dt_depositos.Columns.Add("ttr_id");
        dt_depositos.Columns.Add("ref_id");
        dt_depositos.Columns.Add("Tipo");
        dt_depositos.Columns.Add("BancoID");
        dt_depositos.Columns.Add("Banco");
        dt_depositos.Columns.Add("Cuenta");
        dt_depositos.Columns.Add("Referencia");
        dt_depositos.Columns.Add("Fecha");
        dt_depositos.Columns.Add("Total");
        dt_depositos.Columns.Add("Moneda");
        foreach (GridViewRow row in gv_depositos.Rows)
        {
            lb1 = (Label)row.FindControl("lbl_ttr_id");
            lb2 = (Label)row.FindControl("lbl_ref_id");
            lb3 = (Label)row.FindControl("lbl_tipo");
            lb4 = (Label)row.FindControl("lbl_bancoid");
            lb5 = (Label)row.FindControl("lbl_banco");
            lb6 = (Label)row.FindControl("lbl_banco_cuenta");
            lb7 = (Label)row.FindControl("lbl_referencia");
            lb8 = (Label)row.FindControl("lbl_fecha");
            lb9 = (Label)row.FindControl("lbl_total");
            lb10 = (Label)row.FindControl("lbl_moneda");
            object[] ObjArr = { lb1.Text, lb2.Text, lb3.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, lb8.Text, lb9.Text, lb10.Text };
            dt_depositos.Rows.Add(ObjArr);
        }
        dt_depositos.Rows[e.RowIndex].Delete();
        gv_depositos.DataSource = dt_depositos;
        gv_depositos.DataBind();
        Calcular_Saldos();
        chk_asignar_CheckedChanged1(sender, e);
    }
    protected void drp_tipo_transaccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_transaccion.SelectedValue == "78")
        {
            drp_tipo_persona.SelectedValue = "4";
        }
        else if (drp_tipo_transaccion.SelectedValue == "79")
        {
            drp_tipo_persona.SelectedValue = "2";
        }
        else if (drp_tipo_transaccion.SelectedValue == "80")
        {
            drp_tipo_persona.SelectedValue = "5";
        }
        else if (drp_tipo_transaccion.SelectedValue == "81")
        {
            drp_tipo_persona.SelectedValue = "6";
        }
        else if (drp_tipo_transaccion.SelectedValue == "82")
        {
            drp_tipo_persona.SelectedValue = "8";
        }
        else if (drp_tipo_transaccion.SelectedValue == "95")
        {
            drp_tipo_persona.SelectedValue = "4";
        }
        else if (drp_tipo_transaccion.SelectedValue == "96")
        {
            drp_tipo_persona.SelectedValue = "2";
        }
        else if (drp_tipo_transaccion.SelectedValue == "97")
        {
            drp_tipo_persona.SelectedValue = "5";
        }
        else if (drp_tipo_transaccion.SelectedValue == "98")
        {
            drp_tipo_persona.SelectedValue = "6";
        }
        else if (drp_tipo_transaccion.SelectedValue == "94")
        {
            drp_tipo_persona.SelectedValue = "8";
        }
        else if (drp_tipo_transaccion.SelectedValue == "110")
        {
            drp_tipo_persona.SelectedValue = "10";
        }
        else if (drp_tipo_transaccion.SelectedValue == "112")
        {
            drp_tipo_persona.SelectedValue = "10";
        }
        tb_codigo.Text = "";
        tb_nombre.Text = "";
        tb_codigo_proveedor.Text = "0";
        gv_documentos.DataBind();
        gv_cortes.DataBind();
        Calcular_Saldos();
    }
    protected void btn_generar_reporte_Click(object sender, EventArgs e)
    {
        if (lbl_liquidacion_id.Text != "0")
        {
            Response.Write("<SCRIPT language=javascript>var w=window.open('../reports/Liquidacion.aspx?id=" + lbl_liquidacion_id.Text + "','Liquidaciones','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');</SCRIPT>");
        }
    }
    protected void drp_banco_cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!drp_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
        {
            string cuentaID = drp_banco_cuenta.SelectedValue;
            RE_GenericBean datoscuenta = (RE_GenericBean)DB.GetMonedaIDbyCuentaBancaria(cuentaID);
            drp_moneda.SelectedValue = datoscuenta.intC1.ToString();
        }
        gv_documentos.DataBind();
        gv_cortes.DataBind();
        Calcular_Saldos();
    }
    protected void drp_banco_cuenta2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!drp_banco_cuenta2.SelectedItem.Text.Equals("Seleccione..."))
        {
            string cuentaID = drp_banco_cuenta2.SelectedValue;
            RE_GenericBean datoscuenta = (RE_GenericBean)DB.GetMonedaIDbyCuentaBancaria(cuentaID);
            drp_moneda2.SelectedValue = datoscuenta.intC1.ToString();
        }
    }
    protected void btn_imprimir_retencion_Click(object sender, EventArgs e)
    {
        string script = "";
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        //string script = "window.open('pop_retenciontoprintlist.aspx?chequetransID=" + lb_correlativo.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        if (user.PaisID == 3 || user.PaisID == 23)
        {
            script = "window.open('../invoice/template_liquidacion_retencion.aspx?liquidacionID=" + lbl_liquidacion_id.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            //script = "window.open('../plantillas/impresion.aspx?id=" + lb_cuentas_bancarias.SelectedValue + "&correlativo=" + tb_chequeNo.Text + "&tipo=6','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";

        }
        #region Seteo de Parametros de Impresion
        user.ImpresionBean.Operacion = "1";
        user.ImpresionBean.Tipo_Documento = "9";
        user.ImpresionBean.Id = lbl_liquidacion_id.Text;
        user.ImpresionBean.Impreso = false;
        Session["usuario"] = user;
        #endregion
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
}