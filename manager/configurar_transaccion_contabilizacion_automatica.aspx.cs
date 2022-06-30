using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_configurar_transaccion_contabilizacion_automatica : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList Arr = null;
    ListItem item = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!IsPostBack)
        {
            Obtengo_Listas();
        }
    }
    protected void Obtengo_Listas()
    {
        Arr = (ArrayList)DB.getPaises("");
        drp_empresa.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_empresa.Items.Add(item);
        foreach (PaisBean pais in Arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_empresa.Items.Add(item);
        }
        drp_empresa.SelectedIndex = 0;
        Arr = null;
        drp_sistema.Items.Clear();
        drp_tipo_operacion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sistema.Items.Add(item);
        drp_tipo_operacion.Items.Add(item);
        Arr = (ArrayList)DB.getSistemas();
        foreach (RE_GenericBean Bean in Arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_sistema.Items.Add(item);
        }
        drp_sistema.SelectedIndex = 0;
        drp_tipo_operacion.SelectedIndex = 0;
        item = new ListItem("Seleccione...", "0");
        drp_moneda_origen.Items.Clear();
        drp_moneda_destino.Items.Clear();
        drp_sucursal.Items.Clear();
        drp_serie.Items.Clear();
        drp_moneda_origen.Items.Add(item);
        drp_moneda_destino.Items.Add(item);
        drp_sucursal.Items.Add(item);
        drp_serie.Items.Add(item);
        drp_contabilidad.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_contabilidad.Items.Add(item);
        item = new ListItem("Fiscal", "1");
        drp_contabilidad.Items.Add(item);
        item = new ListItem("Financiera", "2");
        drp_contabilidad.Items.Add(item);
        drp_operacion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_operacion.Items.Add(item);
        item = new ListItem("Facturacion", "1");
        drp_operacion.Items.Add(item);
        item = new ListItem("Operaciones", "2");
        drp_operacion.Items.Add(item);
    }
    protected void drp_sistema_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_sistema.SelectedValue != "0")
        {
            drp_tipo_operacion.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_tipo_operacion.Items.Add(item);
            Arr = null;
            Arr = (ArrayList)DB.getTipo_Operacion();
            foreach (RE_GenericBean Bean in Arr)
            {
                if (Bean.intC2.ToString() == drp_sistema.SelectedValue)
                {
                    item = new ListItem(Bean.strC1, Bean.intC1.ToString());
                    drp_tipo_operacion.Items.Add(item);
                }
            }
            drp_tipo_operacion.SelectedIndex = 0;
        }
    }
    protected void Get_Transacciones_Asociadas()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TCACOID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("MBL");
        dt.Columns.Add("HBL");
        dt.Columns.Add("RO");
        dt.Columns.Add("IMP_EXP");
        dt.Columns.Add("TTR");
        dt.Columns.Add("TPIORIGEN");
        dt.Columns.Add("TPIDESTINO");
        dt.Columns.Add("TRANSACCION");
        dt.Columns.Add("OPERACION");
        dt.Columns.Add("IDPADRE");
        dt.Columns.Add("TTRID");
        string sql = " and i.tcaco_pai_id=" + drp_empresa.SelectedValue + " and i.tcaco_sis_id=" + drp_sistema.SelectedValue + " and i.tcaco_tto_id=" + drp_tipo_operacion.SelectedValue + " ";
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Transacciones_Asociadas(sql);
        foreach (RE_GenericBean Bean in Arr)
        {
            string MBL = "";
            string HBL = "";
            string IMP_EXP = "";
            if (Bean.boolC1 == true)
            {
                MBL = "Prepaid";
            }
            else
            {
                MBL = "Collect";
            }
            if (Bean.boolC2 == true)
            {
                HBL = "Prepaid";
            }
            else
            {
                HBL = "Collect";
            }
            if (Bean.boolC4 == true)
            {
                IMP_EXP = "Import";
            }
            else
            {
                IMP_EXP = "Export";
            }
            object[] ObjArr = { Bean.intC10.ToString(), Bean.strC1, MBL, HBL, Bean.boolC3.ToString(), IMP_EXP, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.intC9.ToString(), Bean.intC4.ToString() };
            dt.Rows.Add(ObjArr);
        }
        gv_transacciones.DataSource = dt;
        gv_transacciones.DataBind();
        DropDownList drp1 = null;
        DropDownList drp2 = null;
        GridViewRowCollection gvr = gv_transacciones.Rows;
        foreach (GridViewRow row in gvr)
        {
            foreach (RE_GenericBean Bean in Arr)
            {
                if (Bean.intC1.ToString() == row.Cells[3].Text)
                {
                    drp1 = (DropDownList)row.FindControl("drp_terceros");
                    drp2 = (DropDownList)row.FindControl("drp_automatizar");
                    drp1.SelectedValue = Bean.boolC5.ToString();
                    drp2.SelectedValue = Bean.boolC6.ToString();
                }
            }
        }
    }
    protected void btn_visualizar_Click(object sender, EventArgs e)
    {
        if (drp_empresa.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Empresa");
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema");
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Operacion");
            return;
        }
        drp_empresa.Enabled = false;
        drp_sistema.Enabled = false;
        drp_tipo_operacion.Enabled = false;
        Get_Transacciones_Asociadas();
        Marcar_Transacciones();
        gv_transacciones.Visible = true;
        drp_sucursal.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sucursal.Items.Add(item);
        Arr = (ArrayList)DB.getSucursalesbyuser(user.ID, int.Parse(drp_empresa.SelectedValue));
        foreach (SucursalBean suc in Arr)
        {
            item = new ListItem(suc.Nombre, suc.ID.ToString());
            drp_sucursal.Items.Add(item);
        }
        Panel1.Visible = true;
    }
    protected void btn_nuevo_Click(object sender, EventArgs e)
    {
        Limpiar();
    }
    protected void Limpiar()
    {
        drp_empresa.Enabled = true;
        drp_sistema.Enabled = true;
        drp_tipo_operacion.Enabled = true;
        drp_tipo_operacion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_operacion.Items.Add(item);
        drp_empresa.SelectedIndex = 0;
        drp_sistema.SelectedIndex = 0;
        drp_tipo_operacion.SelectedIndex = 0;
        gv_transacciones.DataBind();
        gv_transacciones.Visible = false;
        lbl_transaccion_id.Text = "0";
        Panel1.Visible = false;
        Panel2.Visible = false;
        Panel3.Visible = false;
    }
    protected void Marcar_Transacciones()
    {
        GridViewRowCollection gvr = gv_transacciones.Rows;
        DropDownList drp1 = null;
        DropDownList drp2 = null;
        Arr = null;
        string sql = " and i.tcaco_pai_id=" + drp_empresa.SelectedValue + " and i.tcaco_sis_id=" + drp_sistema.SelectedValue + " and i.tcaco_tto_id=" + drp_tipo_operacion.SelectedValue + " ";
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Transacciones_Asociadas(sql);
        foreach (GridViewRow row in gvr)
        {
            foreach (RE_GenericBean Bean in Arr)
            {
                if (Bean.intC10.ToString() == row.Cells[3].Text)
                {
                    drp1 = (DropDownList)row.FindControl("drp_terceros");
                    drp2 = (DropDownList)row.FindControl("drp_automatizar");
                    drp1.SelectedValue = Bean.boolC5.ToString();
                    drp2.SelectedValue = Bean.boolC6.ToString();
                }
            }
        }
    }
    protected void drp_contabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_moneda_origen.Items.Clear();
        drp_moneda_destino.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_moneda_origen.Items.Add(item);
        drp_moneda_destino.Items.Add(item);
        if (drp_contabilidad.SelectedValue != "0")
        {
            Arr = null;
            Arr = (ArrayList)DB.getMonedasbyPais(int.Parse(drp_empresa.SelectedValue), 0);
            foreach (RE_GenericBean rgb in Arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                drp_moneda_origen.Items.Add(item);
                drp_moneda_destino.Items.Add(item);
            }
        }
    }
    protected void drp_operacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql = "";
        if (drp_contabilidad.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe selecionar la Contabilidad Destino");
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal");
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Operacion");
            return;
        }
        sql = "  fac_suc_id=" + drp_sucursal.SelectedValue + "  and fac_tipo=" + lbl_ttr_id.Text + " and fac_conta_id=" + drp_contabilidad.SelectedValue + " and fac_operacion_id=" + drp_operacion.SelectedValue + " ";
        Arr = null;
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_serie.Items.Add(item);
        Arr = (ArrayList)DB.getSeriesByCriterio(user, sql);
        foreach (RE_GenericBean bean in Arr)
        {
            item = new ListItem(bean.strC2, bean.strC2);
            drp_serie.Items.Add(item);
        }
        drp_serie.SelectedIndex = 0;
    }

    protected void gv_transacciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_transacciones.SelectedRow;
        DropDownList drp = (DropDownList)row.FindControl("drp_automatizar");
        if (drp.SelectedValue.ToString() == "False")
        {
            WebMsgBox.Show("No se puede configurar una transaccion que no se debe Automatizar");
            return;
        }

        drp_contabilidad.SelectedIndex = 0;
        drp_moneda_origen.SelectedIndex = 0;
        drp_moneda_destino.SelectedIndex = 0;
        drp_sucursal.SelectedIndex = 0;
        drp_operacion.SelectedIndex = 0;
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_serie.Items.Add(item);
        drp_serie.SelectedIndex = 0;
        rbl_partida.SelectedValue = "True";
        lbl_transaccion_id.Text = "0";
        lbl_ttr_id.Text = "0";
        gv_configuraciones.DataBind();

        lbl_transaccion_id.Text = row.Cells[3].Text;
        lbl_ttr_id.Text = row.Cells[15].Text;
        Get_Detalle_Configuraciones(int.Parse(lbl_transaccion_id.Text));
        Panel2.Visible = true;
        Panel3.Visible = true;

    }
    protected void btn_agregar_Click(object sender, EventArgs e)
    {
        if (lbl_transaccion_id.Text == "0")
        {
            WebMsgBox.Show("Debe seleccionar una transaccion a Configurar");
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Contabilidad Destino");
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe selecconar la Moneda Origen");
            return;
        }
        if (drp_moneda_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Moneda Destino");
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal");
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Operacion");
            return;
        }
        if (drp_serie.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Serie");
            return;
        }
        if (rbl_partida.SelectedValue == "")
        {
            WebMsgBox.Show("Debe indicar si la Transaccion debe Generar Partida");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.intC1 = int.Parse(lbl_transaccion_id.Text);
        Bean.intC2 = int.Parse(drp_contabilidad.SelectedValue);
        Bean.intC3 = int.Parse(drp_moneda_origen.SelectedValue);
        Bean.intC4 = int.Parse(drp_moneda_destino.SelectedValue);
        Bean.intC5 = int.Parse(drp_sucursal.SelectedValue);
        Bean.intC6 = int.Parse(drp_operacion.SelectedValue);
        Bean.strC1 = drp_serie.SelectedValue;
        Bean.boolC1 = bool.Parse(rbl_partida.SelectedValue);
        int resultado = DB.Contabilizacion_Automatica_Insertar_Detalle(user, Bean);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de guardar la configuracion");
            return;
        }
        else
        {
            WebMsgBox.Show("Configuracion guardada exitosamente");
            drp_contabilidad.SelectedIndex = 0;
            drp_moneda_origen.SelectedIndex = 0;
            drp_moneda_destino.SelectedIndex = 0;
            drp_sucursal.SelectedIndex = 0;
            drp_operacion.SelectedIndex = 0;
            drp_serie.SelectedIndex = 0;
            rbl_partida.SelectedValue = "True";
            lbl_transaccion_id.Text = "0";
            gv_configuraciones.DataBind();
            return;
        }
    }
    protected void Get_Detalle_Configuraciones(int TCACO_ID)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("CONTA_DESTINO");
        dt.Columns.Add("M_ORIGEN");
        dt.Columns.Add("M_DESTINO");
        dt.Columns.Add("SUCURSAL");
        dt.Columns.Add("OPERACION");
        dt.Columns.Add("SERIE");
        dt.Columns.Add("PARTIDA");
        string Operacion = "";
        ArrayList Arr_Detalles = (ArrayList)DB.Contabilizacion_Automatica_Get_Detalles_Configuracion(user, TCACO_ID);
        foreach (RE_GenericBean Bean in Arr_Detalles)
        {
            if (Bean.intC2 == 1)
            {
                Operacion = "Facturacion";
            }
            else
            {
                Operacion = "Operaciones";
            }
            object[] ObjArr = { Bean.intC1.ToString(), Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Operacion, Bean.strC6, Bean.boolC1.ToString() };
            dt.Rows.Add(ObjArr);
        }
        gv_configuraciones.DataSource = dt;
        gv_configuraciones.DataBind();
    }
    protected void gv_configuraciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string Id = gv_configuraciones.Rows[e.RowIndex].Cells[1].Text;
        string sql = "update tbl_contabilizacion_automatica_detalle set tcad_estado=0 where tcad_id=" + Id + " ";
        int resultado = DB.Contabilizacion_Automatica_Ejecutar_Update(user, sql);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de eliminar la configuracion.");
            return;
        }
        else
        {
            WebMsgBox.Show("Configuracion eliminada exitosamente.");
            Get_Detalle_Configuraciones(int.Parse(lbl_transaccion_id.Text));
            return;
        }
    }
}