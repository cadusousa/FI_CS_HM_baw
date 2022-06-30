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
    LibroDiarioDS ds = new LibroDiarioDS();
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
        if (!((permiso & 512) == 512))
            Response.Redirect("index.aspx");

        if (!Page.IsPostBack)
        {

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
            //    lb_moneda.SelectedValue = "8";
            //    lb_moneda.Enabled = false;
            //}
            //else
            //{
            //    lb_moneda.SelectedValue = user.pais.ID.ToString();
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

            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
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
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre_intercompany)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_intercompany=" + tb_codigo.Text; else where += " and id_intercompany=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
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
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
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
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_contacto.Text = "";
            tb_contacto.Visible = false;
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
        }
        
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
    protected void gv_cortes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int correlativo = 0;
        string serie = "";
        if (e.CommandName == "Modificar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            correlativo = int.Parse(gv_cortes.Rows[index].Cells[1].Text);
            serie = gv_cortes.Rows[index].Cells[2].Text;

            Response.Redirect("editar_corte.aspx?id="+correlativo);
        }
    }
    //protected void btn_chequ_Click(object sender, EventArgs e)
    //{
    //    decimal total = 0;
    //    string facturas = "";
    //    int proveedorID = int.Parse(tb_agenteID.Text);
    //    GridViewRow row;
    //    CheckBox chk;
    //    for (int i = 0; i < gv_cortes.Rows.Count; i++)
    //    {
    //        row = gv_cortes.Rows[i];
    //        if (row.RowType == DataControlRowType.DataRow)
    //        {
    //            chk = (CheckBox)row.FindControl("chk_seleccion");
    //            if (chk.Checked)
    //            {
    //                total += decimal.Parse(row.Cells[7].Text);
    //                facturas += ", " + row.Cells[1].Text;
    //            }

    //        }
    //    }
    //    if (!tb_cargosadicionales.Text.Equals(""))
    //        total += decimal.Parse(tb_cargosadicionales.Text);

    //    string mensaje = "<script languaje=\"JavaScript\">";
    //    mensaje += "window.open('pop_cheques.aspx?total=" + total + "&fact=" + facturas + "&provID=" + tb_agenteID.Text + "&provtype="+lb_tipopersona.SelectedValue+"','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
    //    mensaje += "</script>";
    //    Page.RegisterClientScriptBlock("closewindow", mensaje);
    //}
    //protected void btn_transferencia_Click(object sender, EventArgs e)
    //{
    //    decimal total = 0;
    //    string facturas = "";
    //    int proveedorID = int.Parse(tb_agenteID.Text);
    //    GridViewRow row;
    //    CheckBox chk;
    //    for (int i = 0; i < gv_cortes.Rows.Count; i++)
    //    {
    //        row = gv_cortes.Rows[i];
    //        if (row.RowType == DataControlRowType.DataRow)
    //        {
    //            chk = (CheckBox)row.FindControl("chk_seleccion");
    //            if (chk.Checked)
    //            {
    //                total += decimal.Parse(row.Cells[7].Text);
    //                facturas += ", " + row.Cells[1].Text;
    //            }

    //        }
    //    }
    //    if (!tb_cargosadicionales.Text.Equals(""))
    //        total += decimal.Parse(tb_cargosadicionales.Text);

    //    string mensaje = "<script languaje=\"JavaScript\">";
    //    mensaje += "window.open('pop_transferencias_ag.aspx?total=" + total + "&fact=" + facturas + "&provID=" + tb_agenteID.Text + "&provtype=" + lb_tipopersona.SelectedValue + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
    //    mensaje += "</script>";
    //    Page.RegisterClientScriptBlock("closewindow", mensaje);
    //}
    protected void gv_cortes_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;

    }
    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        if ((tb_agenteID.Text.Trim() == "") || (tb_agenteID.Text.Trim() == "0"))
        {
            WebMsgBox.Show("Debe seleccionar el Proveedor");
            return;
        }
        int proveedorID = int.Parse(tb_agenteID.Text);
        int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
        int monID = int.Parse(lb_moneda.SelectedValue);
        //obtengo el listado de cortes
        ArrayList Arr = null;
        int ted = 5;
        if (chk_pagada.Checked) ted = 4;
        Arr = DB.getCortesProveedor_All(proveedorID, proveedortype, monID, ted, user);
        dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
        gv_cortes.DataSource = dt;
        gv_cortes.DataBind();
    }
    protected void lb_moneda_SelectedIndexChanged(object sender, EventArgs e)
    {
        gv_cortes.DataBind();
    }
}
