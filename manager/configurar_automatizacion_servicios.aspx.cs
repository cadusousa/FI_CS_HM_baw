using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_configurar_automatizacion_servicios : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ListItem item = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        else
        {
            user = (UsuarioBean)Session["usuario"];
        }
        if (!IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
            Obtengo_listas();
        }
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = int.Parse(Menu1.SelectedValue);
        Obtengo_listas();
        Panel2.Visible = false;
        gv_transacciones_asociadas.DataBind();
    }
    protected void Obtengo_listas()
    {
        arr = (ArrayList)DB.getPaises("");
        item = new ListItem("Seleccione...", "0");
        drp_paises.Items.Clear();
        drp_pais_destino.Items.Clear();
        drp_paises.Items.Add(item);
        drp_pais_destino.Items.Add(item);
        foreach (PaisBean pais in arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_paises.Items.Add(item);
            drp_pais_destino.Items.Add(item);
        }
        drp_paises.SelectedIndex = 0;
        drp_pais_destino.SelectedIndex = 0;
        arr = null;
        item = new ListItem("Seleccione...", "0");
        drp_tipos_servicio.Items.Clear();
        drp_tipo_servicio.Items.Clear();
        drp_tipos_servicio.Items.Add(item);
        drp_tipo_servicio.Items.Add(item);
        drp_sucursal_automatizar.Items.Clear();
        drp_sucursal_automatizar.Items.Add(item);
        drp_sucursal_automatizar.SelectedIndex = 0;
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_servicio");
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_tipos_servicio.Items.Add(item);
            drp_tipo_servicio.Items.Add(item);
        }
        drp_tipos_servicio.SelectedIndex = 0;
        drp_tipo_servicio.SelectedIndex = 0;
        arr = null;
        arr = DB.Get_Automatizaciones_Servicios(user, "");
        item = new ListItem("Seleccione...", "0");
        drp_automatizacion_servicios.Items.Clear();
        drp_automatizacion_servicios.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem("Empresa.: " + Bean.strC2 + " - Sucursal.: " + Bean.strC7 + " - Servicio.: " + Bean.strC3, Bean.strC1);
            drp_automatizacion_servicios.Items.Add(item);
        }
        drp_automatizacion_servicios.SelectedIndex = 0;
        drp_automatizacion_servicios.Enabled = true;
        Panel1.Visible = false;
        arr = null;
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
        drp_tipo_operacion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_operacion.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_tipo_operacion.Items.Add(item);
        }
        drp_tipo_operacion.SelectedIndex = 0;
        arr = null;
        arr = (ArrayList)DB.GetDocumentosBySYS_TipoTeferencia();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_documento.Items.Clear();
        drp_tipo_documento.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_tipo_documento.Items.Add(item);
        }
        drp_tipo_documento.SelectedIndex = 0;
        item = new ListItem("Seleccione...", "0");
        drp_sucursal.Items.Clear();
        drp_sucursal.Items.Add(item);
        drp_serie.Items.Clear();
        drp_serie.Items.Add(item);
        drp_rubro.Items.Clear();
        drp_rubro.Items.Add(item);
        drp_moneda_origen.Items.Clear();
        drp_moneda_origen.Items.Add(item);
        drp_moneda_destino.Items.Clear();
        drp_moneda_destino.Items.Add(item);
        drp_sucursal.SelectedIndex = 0;
        drp_serie.SelectedIndex = 0;
        drp_rubro.SelectedIndex = 0;
        Cargar_Automatizaciones();

    }
    protected void btn_guardar_automatizacion_Click(object sender, EventArgs e)
    {
        if (drp_paises.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Pais a Automatizar");
            return;
        }
        if (drp_sucursal_automatizar.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal a Automatizar");
            return;
        }
        if (drp_tipos_servicio.SelectedValue == "0")
        {   
            WebMsgBox.Show("Debe seleccionar el Tipo de Servicio a Automatizar");
            return;
        }
        RE_GenericBean bean = new RE_GenericBean();
        bean.intC1 = int.Parse(drp_paises.SelectedValue);
        bean.intC2 = int.Parse(drp_tipos_servicio.SelectedValue);
        bean.intC3 = int.Parse(drp_sucursal_automatizar.SelectedValue);
        int resultado = DB.Validar_Automatizacion_Servicios_Existente(user, bean);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de Validar si la Automatizacion de Servicios ya existe");
            return;
        }
        else if (resultado > 0)
        {
            WebMsgBox.Show("Automatizacion de Servicios Existente");
            return;
        }
        else
        {
            resultado = 0;
            resultado = DB.Insertar_Automatizacion_Servicios(user, bean);
            if ((resultado == -100) && (resultado < 1))
            {
                WebMsgBox.Show("Existio un error al Tratar de Guardar la informacion");
                return;
            }
            else
            {
                WebMsgBox.Show("Automatizacion de Servicios guardada correctamente");
                drp_paises.SelectedIndex = 0;
                drp_tipos_servicio.SelectedIndex = 0;
                drp_sucursal_automatizar.SelectedIndex = 0;
                return;
            }
        }
    }
    protected void drp_automatizacion_servicios_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_automatizacion_servicios.SelectedValue != "0")
        {
            Panel1.Visible = true;
            drp_automatizacion_servicios.Enabled = false;
            Get_Transacciones_Asociadas(int.Parse(drp_automatizacion_servicios.SelectedValue));
        }
        else
        {
            Panel1.Visible = false;
            drp_automatizacion_servicios.Enabled = true;
        }
    }
    protected void drp_contabilidad_destino_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_pais_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Pais Destino");
            drp_contabilidad_destino.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Contabilidad de Origen");
            drp_contabilidad_destino.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_destino.SelectedValue != "0")
        {
            arr = null;
            drp_sucursal.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_sucursal.Items.Add(item);
            arr = DB.getSucursales(" and suc_pai_id=" + drp_pais_destino.SelectedValue + "  ");
            foreach (SucursalBean bean in arr)
            {
                item = new ListItem(bean.Nombre, bean.ID.ToString());
                drp_sucursal.Items.Add(item);
            }
            drp_sucursal.SelectedIndex = 0;

            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(int.Parse(drp_pais_destino.SelectedValue), int.Parse(drp_contabilidad_destino.SelectedValue));
            drp_moneda_destino.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_moneda_destino.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                drp_moneda_destino.Items.Add(item);
            }
            drp_moneda_destino.SelectedIndex = 0;
        }
    }
    protected void drp_operacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_documento.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Documento");
            drp_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal");
            drp_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Operacion");
            return;
        }
        string sql = "";
        sql = "  fac_suc_id=" + drp_sucursal.SelectedValue + "  and fac_tipo=" + drp_tipo_documento.SelectedValue + " and fac_conta_id=" + drp_contabilidad_destino.SelectedValue + " and fac_operacion_id=" + drp_operacion.SelectedValue + " ";
        arr = null;
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_serie.Items.Add(item);
        arr = (ArrayList)DB.getSeriesByCriterio(user, sql);
        foreach (RE_GenericBean bean in arr)
        {
            item = new ListItem(bean.strC2, bean.strC2);
            drp_serie.Items.Add(item);
        }
        drp_serie.SelectedIndex = 0;
    }
    protected void drp_tipo_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_servicio.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Servicio");
            return;
        }
        drp_rubro.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_rubro.Items.Add(item);
        ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(drp_tipo_servicio.SelectedValue), "");
        foreach (RE_GenericBean bean in rubros)
        {
            item = new ListItem(bean.strC1, bean.intC1.ToString());
            drp_rubro.Items.Add(item);
        }
        drp_rubro.SelectedIndex = 0;
    }
    protected void btn_agregar_transaccion_Click(object sender, EventArgs e)
    {
        if (drp_tipo_documento.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Documento");
            return;
        }
        if (drp_pais_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Pais Destino donde se generara la Transaccion");
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Contabilidad de Origen");
            return;
        }
        if (drp_contabilidad_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Contabilidad Destino");
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal donde se realizara la Transaccion");
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
        if (drp_tipo_persona.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Persona");
            return;
        }
        if (tb_id_persona.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe ingresar el ID de la Persona");
            return;
        }
        if (drp_tipo_servicio.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Servicio");
            return;
        }
        if (drp_rubro.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Rubro");
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0") 
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Operacion");
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Moneda Origen");
            return;
        }
        if (drp_moneda_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Moneda Destino");
            return;
        }
        if ((tb_monto.Text.Trim() == "") || (tb_monto.Text.Trim() == "0"))
        {
            WebMsgBox.Show("Debe un Monto valido para esta Operacion");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.intC1 = int.Parse(drp_automatizacion_servicios.SelectedValue.ToString());
        Bean.intC2 = int.Parse(drp_tipo_documento.SelectedValue.ToString());
        Bean.intC3 = int.Parse(drp_pais_destino.SelectedValue.ToString());
        Bean.intC4 = int.Parse(drp_contabilidad_origen.SelectedValue.ToString());
        Bean.intC5 = int.Parse(drp_contabilidad_destino.SelectedValue.ToString());
        Bean.intC6 = int.Parse(drp_sucursal.SelectedValue.ToString());
        Bean.intC7 = int.Parse(drp_operacion.SelectedValue.ToString());
        Bean.strC1 = drp_serie.SelectedValue.ToString();
        Bean.intC8 = int.Parse(drp_tipo_persona.SelectedValue.ToString());
        Bean.intC9 = int.Parse(tb_id_persona.Text.Trim());
        Bean.intC10 = int.Parse(drp_tipo_servicio.SelectedValue.ToString());
        Bean.intC11 = int.Parse(drp_rubro.SelectedValue.ToString());
        Bean.intC12 = int.Parse(drp_tipo_operacion.SelectedValue.ToString());
        Bean.intC13 = int.Parse(drp_moneda_origen.SelectedValue.ToString());
        Bean.intC14 = int.Parse(drp_moneda_destino.SelectedValue.ToString());
        Bean.boolC1 = bool.Parse(rbl_cobra_iva.SelectedValue);
        Bean.decC1 = decimal.Parse(tb_monto.Text.Trim());
        int resultado = DB.Validar_Automatizacion_Servicios_Detalle_Existente(user, Bean);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de Validar si la Transaccion ya existe");
            return;
        }
        else if (resultado > 0)
        {
            WebMsgBox.Show("Transaccion Existente");
            return;
        }
        else
        {
            resultado = DB.Insertar_Detalle_Automatizacion_Servicios(user, Bean);
            if (resultado == -100)
            {
                WebMsgBox.Show("Existio un error al Tratar de Agregar la Transaccion");
                return;
            }
            else
            {
                WebMsgBox.Show("La transaccion ha sido Agregada Exitosamente");
                Limpiar_Ingreso_Transacciones();
                gv_transacciones_asociadas.DataBind();
                Get_Transacciones_Asociadas(int.Parse(drp_automatizacion_servicios.SelectedValue));
                return;
            }
        }
    }
    protected void Limpiar_Ingreso_Transacciones()
    {
        drp_tipo_documento.SelectedIndex = 0;
        drp_paises.SelectedIndex = 0;
        drp_pais_destino.SelectedIndex = 0;
        drp_contabilidad_origen.SelectedIndex = 0;
        drp_contabilidad_destino.SelectedIndex = 0;
        drp_tipos_servicio.SelectedIndex = 0;
        drp_tipo_servicio.SelectedIndex = 0;
        drp_tipo_operacion.SelectedIndex = 0;
        drp_operacion.SelectedIndex = 0;
        item = new ListItem("Seleccione...", "0");
        drp_sucursal.Items.Clear();
        drp_sucursal.Items.Add(item);
        drp_serie.Items.Clear();
        drp_serie.Items.Add(item);
        drp_rubro.Items.Clear();
        drp_rubro.Items.Add(item);
        drp_moneda_origen.Items.Clear();
        drp_moneda_origen.Items.Add(item);
        drp_moneda_destino.Items.Clear();
        drp_moneda_destino.Items.Add(item);
        drp_sucursal.SelectedIndex = 0;
        drp_serie.SelectedIndex = 0;
        drp_rubro.SelectedIndex = 0;
        drp_tipo_persona.SelectedIndex = 0;
        tb_id_persona.Text = "";
        drp_moneda_origen.SelectedIndex = 0;
        drp_moneda_destino.SelectedIndex = 0;
        tb_monto.Text = "";
    }
    protected void btn_nuevo_Click(object sender, EventArgs e)
    {
        drp_automatizacion_servicios.Enabled = true;
        drp_automatizacion_servicios.SelectedIndex = 0;
        Panel1.Visible = false;
        Limpiar_Ingreso_Transacciones();
        Panel2.Visible = false;
        gv_transacciones_asociadas.DataBind();
    }
    protected void Get_Transacciones_Asociadas(int ID)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("IDD");
        dt.Columns.Add("DOCUMENTO");
        dt.Columns.Add("PAIS");
        dt.Columns.Add("CONTA_ORIGEN");
        dt.Columns.Add("CONTA_DESTINO");
        dt.Columns.Add("SUCURSAL");
        dt.Columns.Add("OPERACION");
        dt.Columns.Add("SERIE");
        dt.Columns.Add("TPI");
        dt.Columns.Add("ID_PERSONA");
        dt.Columns.Add("TTS_ID");
        dt.Columns.Add("SERVICIO");
        dt.Columns.Add("RUB_ID");
        dt.Columns.Add("RUBRO");
        dt.Columns.Add("IMP_EXP");
        dt.Columns.Add("MONEDA_ORIGEN");
        dt.Columns.Add("MONEDA_DESTINO");
        dt.Columns.Add("COBRA_IVA");
        dt.Columns.Add("MONTO");
        ArrayList Arr = DB.Get_Transacciones_Automatizacion_Servicios(user, ID, "");
        if (Arr == null)
        {
            WebMsgBox.Show("Existio un error al Tratar de Cargar las Transacciones Asociadas");
            return;
        }
        if (Arr.Count > 0)
        {
            foreach (RE_GenericBean Bean in Arr)
            {
                object[] ObjArr = { Bean.strC1, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC7, Bean.strC8, Bean.strC9, Bean.strC10, Bean.strC11, Bean.strC12, Bean.strC13, Bean.strC14, Bean.strC15, Bean.strC16, Bean.strC17, Bean.strC18, Bean.strC19, Bean.strC20 };
                dt.Rows.Add(ObjArr);
            }
            gv_transacciones_asociadas.DataSource = dt;
            gv_transacciones_asociadas.DataBind();
            Panel2.Visible = true;
        }
    }
    protected void gv_transacciones_asociadas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int Transaccion_ID = int.Parse(gv_transacciones_asociadas.Rows[e.RowIndex].Cells[2].Text);
        int resultado = DB.Update_Transacciones_Automatizacion_Servicios(user, Transaccion_ID);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de Eliminar la Transaccion");
            return;
        }
        else
        {
            WebMsgBox.Show("Transaccion Eliminada Correctamente");
            gv_transacciones_asociadas.DataBind();
            Get_Transacciones_Asociadas(int.Parse(drp_automatizacion_servicios.SelectedValue));
            return;
        }
    }
    protected void gv_transacciones_asociadas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            foreach (Image img in e.Row.Cells[0].Controls.OfType<Image>())
            {
                if (img.ImageUrl == "~/img/icons/delete.png")
                {
                    img.Attributes["onclick"] = "if(!confirm('Esta seguro de querer Eliminar esta Transaccion.:  ?')){ return false; };";
                }
            }
        }
    }
    protected void Cargar_Automatizaciones()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("PAIS");
        dt.Columns.Add("TIPO_SERVICIO");
        dt.Columns.Add("SUCURSAL");
        arr = null;
        arr = DB.Get_Automatizaciones_Servicios(user, "");
        foreach (RE_GenericBean Bean in arr)
        {
            object[] ObjArr = { Bean.strC1, Bean.strC2, Bean.strC3, Bean.strC7 };
            dt.Rows.Add(ObjArr);
        }
        gv_automatizaciones.DataSource = dt;
        gv_automatizaciones.DataBind();
    }
    protected void gv_automatizaciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int Automatizacion_ID = int.Parse(gv_automatizaciones.Rows[e.RowIndex].Cells[1].Text);
        int resultado = DB.Update_Automatizacion_Servicios(user, Automatizacion_ID);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de Eliminar la Automatizacion de Servicios");
            return;
        }
        else
        {
            WebMsgBox.Show("Automatizacion Eliminada Correctamente");
            gv_automatizaciones.DataBind();
            Obtengo_listas();
            MultiView1.ActiveViewIndex = 3;
            return;
        }
    }
    protected void gv_automatizaciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            foreach (Image img in e.Row.Cells[0].Controls.OfType<Image>())
            {
                if (img.ImageUrl == "~/img/icons/delete.png")
                {
                    img.Attributes["onclick"] = "if(!confirm('Esta seguro de querer Eliminar esta Automatizacion.:  ?')){ return false; };";
                }
            }
        }
    }
    protected void drp_tipo_documento_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_pais_destino.SelectedIndex = 0;
        drp_contabilidad_origen.SelectedIndex = 0;
        drp_contabilidad_destino.SelectedIndex = 0;
        drp_operacion.SelectedIndex = 0;
        drp_sucursal.Items.Clear();
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sucursal.Items.Add(item);
        drp_serie.Items.Add(item);
    }
    protected void drp_contabilidad_origen_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_pais_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Pais Destino");
            return;
        }
        if (drp_contabilidad_origen.SelectedValue != "0")
        {
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(int.Parse(drp_pais_destino.SelectedValue), int.Parse(drp_contabilidad_origen.SelectedValue));
            drp_moneda_origen.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_moneda_origen.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                drp_moneda_origen.Items.Add(item);
            }
            drp_moneda_origen.SelectedIndex = 0;
        }
    }
    protected void drp_paises_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_paises.SelectedValue.ToString() != "")
        {
            arr = null;
            drp_sucursal_automatizar.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_sucursal_automatizar.Items.Add(item);
            arr = DB.getSucursales(" and suc_pai_id=" + drp_paises.SelectedValue + "  ");
            foreach (SucursalBean bean in arr)
            {
                item = new ListItem(bean.Nombre, bean.ID.ToString());
                drp_sucursal_automatizar.Items.Add(item);
            }
            drp_sucursal_automatizar.SelectedIndex = 0;
        }
    }
}