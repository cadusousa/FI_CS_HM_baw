using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_configurar_intercompany_administrativo : System.Web.UI.Page
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
        Obtener_Listado_Configuraciones_Intercompanys();
        Obtener_Listado_Configuraciones_Intercompanys_Editar();
    }
    protected void Obtengo_listas()
    {
        arr = (ArrayList)DB.getPaises("");
        item = new ListItem("Seleccione...", "0");
        drp_empresa_origen.Items.Clear();
        drp_moneda_origen.Items.Clear();
        drp_moneda_destino.Items.Clear();
        drp_empresa_origen.Items.Add(item);
        drp_moneda_origen.Items.Add(item);
        drp_moneda_destino.Items.Add(item);
        foreach (PaisBean pais in arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_empresa_origen.Items.Add(item);
        }
        drp_empresa_origen.SelectedIndex = 0;
        arr = null;
        arr = (ArrayList)DB.Get_Intercompanys("");
        item = new ListItem("Seleccione...", "0");
        drp_intercompanys.Items.Clear();
        drp_intercompanys.Items.Add(item);
        foreach (RE_GenericBean Intercompany in arr)
        {
            item = new ListItem(Intercompany.strC5, Intercompany.intC1.ToString());
            drp_intercompanys.Items.Add(item);
        }
        drp_intercompanys.SelectedIndex = 0;

        arr = null;
        arr = (ArrayList)DB.GetDocumentosBySYS_TipoTeferencia();
        drp_documento_destino.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_documento_destino.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_documento_destino.Items.Add(item);
        }
        drp_documento_destino.SelectedIndex = 0;

        item = new ListItem("Seleccione...", "0");
        drp_sucursal_destino.Items.Clear();
        drp_moneda_impresion.Items.Clear();
        drp_sucursal_destino.Items.Add(item);
        drp_moneda_impresion.Items.Add(item);
        drp_sucursal_destino.SelectedIndex = 0;
        drp_moneda_impresion.SelectedIndex = 0;
    }
    protected void drp_contabilidad_origen_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue != "0")
        {
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(int.Parse(drp_empresa_origen.SelectedValue), int.Parse(drp_contabilidad_origen.SelectedValue));
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
        else
        {
            WebMsgBox.Show("Debe seleccionar la Empresa Origen");
            drp_contabilidad_origen.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_contabilidad_destino_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Empresa Origen");
            drp_contabilidad_destino.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Origen");
            drp_contabilidad_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_contabilidad_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_contabilidad_destino.SelectedIndex = 0;
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Tipo de Operacion");
            drp_contabilidad_destino.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar El Intercompany");
            drp_contabilidad_destino.SelectedIndex = 0;
            return;
        }
        arr = null;
        RE_GenericBean Intercompany_Bean = DB.Get_Intercompany_Data(int.Parse(drp_intercompanys.SelectedValue));
        arr = (ArrayList)DB.getMonedasbyPais(Intercompany_Bean.intC3, int.Parse(drp_contabilidad_destino.SelectedValue));
        drp_moneda_destino.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_moneda_destino.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_moneda_destino.Items.Add(item);
        }
        drp_moneda_destino.SelectedIndex = 0;

        ArrayList Arr_Sucursales = (ArrayList)DB.getSucursales(" and  suc_pai_id=" + Intercompany_Bean.intC3 + " and suc_nombre='SISTEMAS' ");
        if (Arr_Sucursales.Count == 0)
        {
            #region Crear Sucursal
            SucursalBean sucbean = new SucursalBean();
            sucbean.Nombre = "SISTEMAS";
            sucbean.paisID = Intercompany_Bean.intC3;
            int result = DB.InsertUpdateSucursal(sucbean);
            #endregion
        }

        drp_sucursal_destino.Items.Clear();
        arr = null;
        arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + Intercompany_Bean.intC3 + " and suc_nombre ilike '%SISTEMAS%' ");
        item = new ListItem("Seleccione...", "0");
        drp_sucursal_destino.Items.Add(item);
        foreach (SucursalBean Bean in arr)
        {
            item = new ListItem(Bean.Nombre, Bean.ID.ToString());
            drp_sucursal_destino.Items.Add(item);
        }
        drp_sucursal_destino.SelectedIndex = 0;


        arr = null;
        arr = (ArrayList)DB.getMonedasbyCriterio(" and tpm_pai_id=" + Intercompany_Bean.intC3 + " ");
        drp_moneda_impresion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_moneda_impresion.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC2, rgb.strC1);
            drp_moneda_impresion.Items.Add(item);
        }
        drp_moneda_impresion.SelectedIndex = 0;

    }
    protected void drp_moneda_origen_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Empresa Origen");
            drp_moneda_origen.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Origen");
            drp_moneda_origen.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_operacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Empresa Origen");
            drp_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Origen");
            drp_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_operacion.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_intercompanys_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Empresa Origen");
            drp_intercompanys.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Origen");
            drp_intercompanys.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_intercompanys.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_intercompanys.SelectedIndex = 0;
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Tipo de Operacion");
            drp_intercompanys.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_moneda_destino_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Empresa Origen");
            drp_moneda_destino.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Origen");
            drp_moneda_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_moneda_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_moneda_destino.SelectedIndex = 0;
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Tipo de Operacion");
            drp_moneda_destino.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar El Intercompany");
            drp_moneda_destino.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Destino");
            drp_moneda_destino.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_documento_destino_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Empresa Origen");
            drp_documento_destino.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Origen");
            drp_documento_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_documento_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_documento_destino.SelectedIndex = 0;
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Tipo de Operacion");
            drp_documento_destino.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar El Intercompany");
            drp_documento_destino.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Destino");
            drp_documento_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Destino");
            drp_documento_destino.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_sucursal_destino_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Empresa Origen");
            drp_sucursal_destino.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Origen");
            drp_sucursal_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_sucursal_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_sucursal_destino.SelectedIndex = 0;
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Tipo de Operacion");
            drp_sucursal_destino.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar El Intercompany");
            drp_sucursal_destino.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Destino");
            drp_sucursal_destino.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Destino");
            drp_sucursal_destino.SelectedIndex = 0;
            return;
        }
        if (drp_documento_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Documento Destino");
            drp_sucursal_destino.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_moneda_impresion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Empresa Origen");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Origen");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Tipo de Operacion");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar El Intercompany");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Destino");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
        if (drp_moneda_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Destino");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
        if (drp_documento_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Documento Destino");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
        if (drp_sucursal_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Sucursal Destino");
            drp_moneda_impresion.SelectedIndex = 0;
            return;
        }
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        if (drp_empresa_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Empresa Origen");
            return;
        }
        if (drp_contabilidad_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Origen");
            return;
        }
        if (drp_moneda_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Origen");
            return;
        }
        if (drp_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Tipo de Operacion");
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar El Intercompany");
            return;
        }
        if (drp_contabilidad_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Contabilidad Destino");
            return;
        }
        if (drp_moneda_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Moneda Destino");
            return;
        }
        if (drp_documento_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Documento Destino");
            return;
        }
        if (drp_sucursal_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar Sucursal Destino");
            return;
        }
        if (drp_moneda_impresion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Moneda de Impresion");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.intC1 = int.Parse(drp_empresa_origen.SelectedValue.ToString());
        Bean.intC2 = int.Parse(drp_contabilidad_origen.SelectedValue.ToString());
        Bean.intC3 = int.Parse(drp_moneda_origen.SelectedValue.ToString());
        Bean.intC4 = int.Parse(drp_operacion.SelectedValue.ToString());
        Bean.intC5 = int.Parse(drp_intercompanys.SelectedValue.ToString());
        Bean.intC6 = int.Parse(drp_contabilidad_destino.SelectedValue.ToString());
        Bean.intC7 = int.Parse(drp_moneda_destino.SelectedValue.ToString());
        Bean.intC8 = int.Parse(drp_documento_destino.SelectedValue.ToString());
        Bean.intC9 = int.Parse(drp_sucursal_destino.SelectedValue.ToString());
        Bean.intC10 = 0;
        Bean.intC11 = int.Parse(drp_moneda_impresion.SelectedValue.ToString());
        int result = 0;
        string sql = " and tia_pais_origen=" + Bean.intC1.ToString() + " and tia_contabilidad_origen=" + Bean.intC2.ToString() + " and tia_moneda_origen=" + Bean.intC3.ToString() + " and tia_tipo_operacion=" + Bean.intC4.ToString() + " and tia_id_intercompany=" + Bean.intC5.ToString() + " ";
        result = Contabilizacion_Automatica_CAD.Validar_Existencia_Intercompany_Administrativo(user, sql);
        if (result > 0) 
        {
            WebMsgBox.Show("Configuracion Existente");
            return;
        }
        result = 0;
        result = Contabilizacion_Automatica_CAD.Insertar_Configuracion_Intercompany_Administrativo(user, Bean);
        if ((result == 0) || (result == -100))
        {
            WebMsgBox.Show("Existio un error al Tratar de Guardar la Configuracion");
            return;
        }
        else
        {
            WebMsgBox.Show("La Configuracion fue guardada Exitosamente");
            Limpiar();
            return;
        }
    }
    protected void Limpiar()
    {
        drp_empresa_origen.SelectedIndex = 0;
        drp_contabilidad_origen.SelectedIndex = 0;
        item = new ListItem("Seleccione...", "0");
        drp_moneda_origen.Items.Clear();
        drp_moneda_origen.Items.Add(item);
        drp_moneda_origen.SelectedIndex = 0;
        drp_operacion.SelectedIndex = 0;
        drp_intercompanys.SelectedIndex = 0;
        drp_contabilidad_destino.SelectedIndex = 0;
        drp_moneda_destino.Items.Clear();
        drp_moneda_destino.Items.Add(item);
        drp_moneda_destino.SelectedIndex = 0;
        drp_documento_destino.SelectedIndex = 0;
        drp_sucursal_destino.Items.Clear();
        drp_sucursal_destino.Items.Add(item);
        drp_sucursal_destino.SelectedIndex = 0;
        drp_moneda_impresion.Items.Clear();
        drp_moneda_impresion.Items.Add(item);
        drp_moneda_impresion.SelectedIndex = 0;
    }
    public static DataTable Obtener_Configuracion_Itercomapnys(UsuarioBean user, string sql)
    {
        int correlativo = 1;
        DataTable dt = new DataTable();
        dt.Columns.Add("NO");
        dt.Columns.Add("ID");
        dt.Columns.Add("PAIS.O");
        dt.Columns.Add("CONTA.O");
        dt.Columns.Add("MONEDA.O");
        dt.Columns.Add("OPERACION");
        dt.Columns.Add("INTERCOMPANY");
        dt.Columns.Add("CONTA.D");
        dt.Columns.Add("MONEDA.D");
        dt.Columns.Add("DOCUMENTO.D");
        dt.Columns.Add("SUCURSAL.D");
        dt.Columns.Add("IMPRESION");
        ArrayList Arr = Contabilizacion_Automatica_CAD.Obtener_Configuraciones_Intercompany_Administrativo(user, sql);
        if (Arr != null)
        {
            foreach (RE_GenericBean Bean in Arr)
            {
                object[] Obj_Arr = { correlativo.ToString(), Bean.strC1, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC7, Bean.strC8, Bean.strC9, Bean.strC10, Bean.strC12 };
                correlativo++;
                dt.Rows.Add(Obj_Arr);
            }
        }
        return dt;
    }
    protected void Obtener_Listado_Configuraciones_Intercompanys()
    {
        gv_configuraciones.DataSource = Obtener_Configuracion_Itercomapnys(user, "");
        gv_configuraciones.DataBind();
    }
    protected void Obtener_Listado_Configuraciones_Intercompanys_Editar()
    {
        gv_editar_configuraciones.DataSource = Obtener_Configuracion_Itercomapnys(user, "");
        gv_editar_configuraciones.DataBind();
    }
    protected void gv_editar_configuraciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = e.RowIndex;
        int id_configuracion = 0;
        id_configuracion = int.Parse(gv_editar_configuraciones.Rows[index].Cells[2].Text.ToString());
        int resultado = 0;
        resultado = Contabilizacion_Automatica_CAD.Eliminar_Configuracion_Intercompany_Administrativo(user, id_configuracion);
        if ((resultado == 0) || (resultado == -100))
        {
            WebMsgBox.Show("Existio un error al Tratar de Eliminar la Configuracion");
            return;
        }
        else
        {
            WebMsgBox.Show("Configuracion Eliminada Exitosamente");
            Obtener_Listado_Configuraciones_Intercompanys();
            Obtener_Listado_Configuraciones_Intercompanys_Editar();
            Limpiar();
        }
    }
}