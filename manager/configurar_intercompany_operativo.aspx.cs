using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_configurar_intercompany_operativo : System.Web.UI.Page
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
        Obtener_Configuraciones_Intercompany_Operativo();
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = int.Parse(Menu1.SelectedValue);
    }
    protected void Obtengo_listas()
    {
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
        drp_tipo_documento.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_documento.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_tipo_documento.Items.Add(item);
        }
        drp_tipo_documento.SelectedIndex = 0;

        arr = null;
        arr = (ArrayList)DB.Get_Transacciones_Intercompany_Operativo(user, "");
        drp_tipo_trasaccion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_trasaccion.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem(Bean.strC2, Bean.strC1);
            drp_tipo_trasaccion.Items.Add(item);
        }
        drp_tipo_trasaccion.SelectedIndex = 0;

        arr = null;
        drp_sistema.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sistema.Items.Add(item);
        //drp_tipo_operacion.Items.Add(item);
        arr = (ArrayList)DB.getSistemas();
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_sistema.Items.Add(item);
        }
        drp_sistema.SelectedIndex = 0;


        item = new ListItem("Seleccione...", "0");
        drp_sucursal.Items.Clear();
        drp_moneda.Items.Clear();
        drp_serie.Items.Clear();
        drp_sucursal.Items.Add(item);
        drp_moneda.Items.Add(item);
        drp_serie.Items.Add(item);
        drp_sucursal.SelectedIndex = 0;
        drp_moneda.SelectedIndex = 0;
        drp_serie.SelectedIndex = 0;
    }

    protected void btn_guardar_transaccion_Click(object sender, EventArgs e)
    {
        if (tb_transaccion.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe ingresar el Nombre de la Transaccion");
            return;
        }
        int resultado = 0;
        resultado = DB.Insertar_Transaccion_Intercompany_Operativo(user, tb_transaccion.Text.Trim());
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de guardar la Transaccion");
            return;
        }
        else
        {
            WebMsgBox.Show("La Transaccion fue guardada Exitosamente");
            tb_transaccion.Text = "";
            return;
        }
    }
    protected void drp_intercompanys_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_trasaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Operar");
            drp_intercompanys.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue != "0")
        {
            RE_GenericBean Bean = null;
            Bean = (RE_GenericBean)DB.Get_Intercompany_Data(int.Parse(drp_intercompanys.SelectedValue));
            PaisBean Pais = (PaisBean)DB.getPais(Bean.intC3);
            lbl_empresa_id.Text = Bean.intC3.ToString();
        }
    }
    protected void drp_tipo_documento_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_contabilidad.SelectedIndex = 0;
        drp_moneda.Items.Clear();
        drp_sucursal.Items.Clear();
        drp_tipo_operacion.SelectedIndex = 0;
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_moneda.Items.Add(item);
        drp_sucursal.Items.Add(item);
        drp_serie.Items.Add(item);

        if (drp_tipo_trasaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Operar");
            drp_tipo_documento.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Intercompany a Operar");
            drp_tipo_documento.SelectedIndex = 0;
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema a Configurar");
            drp_tipo_documento.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_contabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_trasaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Operar");
            drp_contabilidad.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Intercompany a Operar");
            drp_contabilidad.SelectedIndex = 0;
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema a Configurar");
            drp_contabilidad.SelectedIndex = 0;
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Documento a Operar");
            drp_contabilidad.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad.SelectedValue != "0")
        {
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(int.Parse(lbl_empresa_id.Text), int.Parse(drp_contabilidad.SelectedValue));
            drp_moneda.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_moneda.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                drp_moneda.Items.Add(item);
            }
            drp_moneda.SelectedIndex = 0;

            if (drp_tipo_documento.SelectedValue == "5")
            {
                ArrayList Arr_Sucursales = (ArrayList)DB.getSucursales(" and  suc_pai_id=" + int.Parse(lbl_empresa_id.Text) + " and suc_nombre='SISTEMAS' ");
                if (Arr_Sucursales.Count == 0)
                {
                    #region Crear Sucursal
                    SucursalBean sucbean = new SucursalBean();
                    sucbean.Nombre = "SISTEMAS";
                    sucbean.paisID = int.Parse(lbl_empresa_id.Text);
                    int result = DB.InsertUpdateSucursal(sucbean);
                    #endregion
                }

                drp_sucursal.Items.Clear();
                arr = null;
                arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + int.Parse(lbl_empresa_id.Text) + " and suc_nombre ilike '%SISTEMAS%' ");
                item = new ListItem("Seleccione...", "0");
                drp_sucursal.Items.Add(item);
                foreach (SucursalBean Bean in arr)
                {
                    item = new ListItem(Bean.Nombre, Bean.ID.ToString());
                    drp_sucursal.Items.Add(item);
                }
                drp_sucursal.SelectedIndex = 0;
            }
            else
            {
                drp_sucursal.Items.Clear();
                arr = null;
                arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + int.Parse(lbl_empresa_id.Text) + "  ");
                item = new ListItem("Seleccione...", "0");
                drp_sucursal.Items.Add(item);
                foreach (SucursalBean Bean in arr)
                {
                    item = new ListItem(Bean.Nombre, Bean.ID.ToString());
                    drp_sucursal.Items.Add(item);
                }
                drp_sucursal.SelectedIndex = 0;
            }
        }
    }
    protected void drp_moneda_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_trasaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Operar");
            drp_moneda.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Intercompany a Operar");
            drp_moneda.SelectedIndex = 0;
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema a Configurar");
            drp_moneda.SelectedIndex = 0;
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Documento a Operar");
            drp_moneda.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Contabilidad");
            drp_moneda.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_sucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_trasaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Operar");
            drp_sucursal.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Intercompany a Operar");
            drp_sucursal.SelectedIndex = 0;
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema a Configurar");
            drp_sucursal.SelectedIndex = 0;
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Documento a Operar");
            drp_sucursal.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Contabilidad");
            drp_sucursal.SelectedIndex = 0;
            return;
        }
        if (drp_moneda.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la moneda a Operar");
            drp_sucursal.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_tipo_operacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_trasaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Operar");
            drp_tipo_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Intercompany a Operar");
            drp_tipo_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema a Configurar");
            drp_tipo_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Documento a Operar");
            drp_tipo_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Contabilidad");
            drp_tipo_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_moneda.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la moneda a Operar");
            drp_tipo_operacion.SelectedIndex = 0;
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal");
            drp_tipo_operacion.SelectedIndex = 0;
            return;
        }

        if (drp_tipo_documento.SelectedValue == "5")
        {
            drp_serie.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_serie.Items.Add(item);
            if (drp_contabilidad.SelectedValue == "1")
            {
                if (drp_moneda.SelectedValue == "8")
                {
                    if ((drp_intercompanys.SelectedValue == "2") || (drp_intercompanys.SelectedValue == "9") || (drp_intercompanys.SelectedValue == "6") || (drp_intercompanys.SelectedValue == "13"))
                    {
                        item = new ListItem("PFSA", "1");
                        drp_serie.Items.Add(item);
                    }
                    else
                    {
                        item = new ListItem("PFSAD", "3");
                        drp_serie.Items.Add(item);
                    }
                }
                else
                {
                    item = new ListItem("PFSA", "1");
                    drp_serie.Items.Add(item);
                }
            }
            else if (drp_contabilidad.SelectedValue == "2")
            {
                item = new ListItem("PFNA", "2");
                drp_serie.Items.Add(item);
            }
        }
        else
        {
            string sql = "";
            sql = "  fac_suc_id=" + drp_sucursal.SelectedValue + "  and fac_tipo=" + drp_tipo_documento.SelectedValue + " and fac_conta_id=" + drp_contabilidad.SelectedValue + " and fac_operacion_id=" + drp_tipo_operacion.SelectedValue + " and fac_mon_id=" + drp_moneda.SelectedValue + " ";
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


        //BK Antes de Contabilidad Fiscal USD
        //if (drp_tipo_documento.SelectedValue == "5")
        //{
        //    drp_serie.Items.Clear();
        //    item = new ListItem("Seleccione...", "0");
        //    drp_serie.Items.Add(item);
        //    if (drp_contabilidad.SelectedValue == "1")
        //    {
        //        item = new ListItem("PFSA", "1");
        //        drp_serie.Items.Add(item);
        //    }
        //    else if (drp_contabilidad.SelectedValue == "2")
        //    {
        //        item = new ListItem("PFNA", "2");
        //        drp_serie.Items.Add(item);
        //    }
        //}
        //else
        //{
        //    string sql = "";
        //    sql = "  fac_suc_id=" + drp_sucursal.SelectedValue + "  and fac_tipo="+drp_tipo_documento.SelectedValue+" and fac_conta_id=" + drp_contabilidad.SelectedValue + " and fac_operacion_id=" + drp_tipo_operacion.SelectedValue + " ";
        //    arr = null;
        //    drp_serie.Items.Clear();
        //    item = new ListItem("Seleccione...", "0");
        //    drp_serie.Items.Add(item);
        //    arr = (ArrayList)DB.getSeriesByCriterio(user, sql);
        //    foreach (RE_GenericBean bean in arr)
        //    {
        //        item = new ListItem(bean.strC2, bean.strC2);
        //        drp_serie.Items.Add(item);
        //    }
        //    drp_serie.SelectedIndex = 0;
        //}

    }
    protected void drp_serie_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_trasaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Operar");
            drp_serie.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Intercompany a Operar");
            drp_serie.SelectedIndex = 0;
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema a Configurar");
            drp_serie.SelectedIndex = 0;
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Documento a Operar");
            drp_serie.SelectedIndex = 0;
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Contabilidad");
            drp_serie.SelectedIndex = 0;
            return;
        }
        if (drp_moneda.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la moneda a Operar");
            drp_serie.SelectedIndex = 0;
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal");
            drp_serie.SelectedIndex = 0;
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Operacion");
            drp_serie.SelectedIndex = 0;
            return;
        }
    }
    protected void btn_guardar_configuracion_Click(object sender, EventArgs e)
    {
        if (drp_tipo_trasaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Operar");
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Intercompany a Operar");
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema a Configurar");
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Documento a Operar");
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Contabilidad");
            return;
        }
        if (drp_moneda.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la moneda a Operar");
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal");
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Operacion");
            return;
        }
        if (drp_serie.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Serie");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.intC1 = int.Parse(drp_intercompanys.SelectedValue);
        Bean.intC2 = int.Parse(drp_tipo_trasaccion.SelectedValue);
        Bean.intC3 = int.Parse(drp_tipo_documento.SelectedValue);
        Bean.intC4 = int.Parse(drp_contabilidad.SelectedValue);
        Bean.intC5 = int.Parse(drp_moneda.SelectedValue);
        Bean.intC6 = int.Parse(drp_sucursal.SelectedValue);
        Bean.intC7 = int.Parse(drp_tipo_operacion.SelectedValue);
        Bean.intC8 = int.Parse(drp_sistema.SelectedValue);
        Bean.strC1 = drp_serie.SelectedItem.Text;
        int result = 0;
        string sql = " and tio_id_intercompany=" + Bean.intC1.ToString() + " and tio_tiott_id=" + Bean.intC2.ToString() + " and tio_tsis_id=" + Bean.intC8.ToString() + "";
        result = Contabilizacion_Automatica_CAD.Validar_Existencia_Intercompany_Operativo(user, sql);
        if (result > 0)
        {
            WebMsgBox.Show("Configuracion Existente");
            return;
        }
        result = 0;
        result = Contabilizacion_Automatica_CAD.Insertar_Configuracion_Intercompany_Operativo(user, Bean);
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
        drp_tipo_trasaccion.SelectedIndex = 0;
        drp_intercompanys.SelectedIndex = 0;
        drp_tipo_documento.SelectedIndex = 0;
        drp_contabilidad.SelectedIndex = 0;
        drp_tipo_operacion.SelectedIndex = 0;
        drp_sistema.SelectedIndex = 0;
        drp_moneda.Items.Clear();
        drp_sucursal.Items.Clear();
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_moneda.Items.Add(item);
        drp_sucursal.Items.Add(item);
        drp_serie.Items.Add(item);
    }
    protected void btn_limpiar_Click(object sender, EventArgs e)
    {
        Limpiar();
    }
    protected void Obtener_Configuraciones_Intercompany_Operativo()
    {
        int correlativo = 1;
        DataTable dt = new DataTable();
        dt.Columns.Add("NO");
        dt.Columns.Add("ID");
        dt.Columns.Add("INTERCOMPANY");
        dt.Columns.Add("TRANSACCION");
        dt.Columns.Add("SISTEMA");
        dt.Columns.Add("DOCUMENTO");
        dt.Columns.Add("CONTABILIDAD");
        dt.Columns.Add("MONEDA");
        dt.Columns.Add("SUCURSAL");
        dt.Columns.Add("SERIE");
        ArrayList Arr = Contabilizacion_Automatica_CAD.Obtener_Configuraciones_Intercompany_Operativo(user, "");
        if (Arr != null)
        {
            foreach (RE_GenericBean Bean in Arr)
            {
                object[] Obj_Arr = { correlativo.ToString(), Bean.intC1.ToString(), Bean.strC1, Bean.strC2, Bean.strC8, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC7 };
                correlativo++;
                dt.Rows.Add(Obj_Arr);
            }
        }
        gv_configuraciones.DataSource = dt;
        gv_configuraciones.DataBind();
        gv_configuraciones_eliminar.DataSource = dt;
        gv_configuraciones_eliminar.DataBind();
    }
    protected void gv_configuraciones_eliminar_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int indice = e.RowIndex;
        int ID = int.Parse(gv_configuraciones_eliminar.Rows[indice].Cells[2].Text.ToString());
        int resultado = Contabilizacion_Automatica_CAD.Eliminar_Configuracion_Intercompany_Operativo(user, ID);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de eliminar la configuracion");
            return;
        }
        else
        {
            Obtener_Configuraciones_Intercompany_Operativo();
            WebMsgBox.Show("Configuracion Eliminada Exitosamente");
            return;
        }
    }
    protected void drp_sistema_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipo_trasaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Operar");
            drp_sistema.SelectedIndex = 0;
            return;
        }
        if (drp_intercompanys.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Intercompany a Operar");
            drp_sistema.SelectedIndex = 0;
            return;
        }
    }
}