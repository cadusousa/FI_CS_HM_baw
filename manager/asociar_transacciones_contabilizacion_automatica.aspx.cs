using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_asociar_transacciones_contabilizacion_automatica : System.Web.UI.Page
{
    //manager_configurar_contabilizacion_automatica
    //asociar_transacciones_contabilizacion_automatica
    UsuarioBean user = null;
    ArrayList Arr = null;
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
            Obtengo_Listas();
        }
    }
    protected void Obtengo_Listas()
    {
        Arr = null;
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Casos(user);
        drp_caso.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_caso.Items.Add(item);
        foreach (RE_GenericBean Bean in Arr)
        {
            item = new ListItem(Bean.strC2, Bean.strC1);
            drp_caso.Items.Add(item);
        }
        drp_caso.SelectedIndex = 0;
        Arr = null;
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Tipo_Transaccion(user);
        drp_tipo_transaccion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_transaccion.Items.Add(item);
        foreach (RE_GenericBean Bean in Arr)
        {
            item = new ListItem(Bean.strC2, Bean.strC1);
            drp_tipo_transaccion.Items.Add(item);
        }
        drp_tipo_transaccion.SelectedIndex = 0;
        Arr = null;
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Tipo_Operacion(user);
        drp_tipo_operacion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_operacion.Items.Add(item);
        foreach (RE_GenericBean Bean in Arr)
        {
            item = new ListItem(Bean.strC2, Bean.strC1);
            drp_tipo_operacion.Items.Add(item);
        }
        drp_tipo_operacion.SelectedIndex = 0;
        Arr = null;
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Transacciones(user);
        drp_transaccion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_transaccion.Items.Add(item);
        foreach (RE_GenericBean Bean in Arr)
        {
            item = new ListItem(Bean.strC5 + " - Tpi Origen.: " + Bean.strC6 + " - Tpi Destino.: " + Bean.strC7, Bean.strC1);
            drp_transaccion.Items.Add(item);
        }
        drp_transaccion.SelectedIndex = 0;
        item = new ListItem("Seleccione...", "0");
        drp_transaccion_padre.Items.Clear();
        drp_transaccion_padre.Items.Add(item);
    }
    protected void btn_ver_Click(object sender, EventArgs e)
    {
        if (drp_caso.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Caso a Configurar");
            return;
        }
        drp_caso.Enabled = false;
        Panel1.Visible = true;
        Get_Transacciones_Asociadas();
        drp_transaccion_padre.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_transaccion_padre.Items.Add(item);
        Arr = null;
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Transacciones_Asociar(int.Parse(drp_caso.SelectedValue), "");
        foreach (RE_GenericBean Bean in Arr)
        {
            item = new ListItem(Bean.strC1 + " - TTR.: " + Bean.strC2 + " - Tpi Destino.: " + Bean.strC4 + " - Transaccion.: " + Bean.strC5 + " - Operacion.: " + Bean.strC6, Bean.intC1.ToString());
            drp_transaccion_padre.Items.Add(item);
        }
    }
    protected void btn_nuevo_Click(object sender, EventArgs e)
    {
        Panel1.Visible = false;
        drp_tipo_transaccion.SelectedIndex = 0;
        drp_tipo_operacion.SelectedIndex = 0;
        drp_transaccion.SelectedIndex = 0;
        gv_transacciones_asociadas.DataBind();
        drp_caso.SelectedIndex = 0;
        drp_caso.Enabled = true;
    }
    protected void btn_agregar_Click(object sender, EventArgs e)
    {
        if (drp_caso.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Caso a Configurar");
            return;
        }
        if (drp_tipo_transaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Transaccion a Configurar");
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Operacion a Configurar");
            return;
        }
        if (drp_transaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Transaccion a Configurar");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.intC1 = int.Parse(drp_caso.SelectedValue.ToString());
        Bean.intC2 = int.Parse(drp_tipo_transaccion.SelectedValue.ToString());
        Bean.intC3 = int.Parse(drp_tipo_operacion.SelectedValue.ToString());
        Bean.intC4 = int.Parse(drp_transaccion.SelectedValue.ToString());
        Bean.intC5 = int.Parse(drp_transaccion_padre.SelectedValue);
        string sql = "select tca_id from tbl_contabilizacion_automatica where tca_tcaca_id=" + Bean.intC1.ToString() + " and tca_tcatr_id=" + Bean.intC4.ToString() + " and tca_tcato_id=" + Bean.intC3.ToString() + " and tca_estado=1";
        bool existencia = DB.Contabilizacion_Automatica_Validar_Existencia(user, sql);
        if (existencia == true)
        {
            WebMsgBox.Show("No se puede asociar la Transaccion porque ya fue previamente asociada");
            return;
        }
        int resultado = DB.Contabilizacion_Automatica_Insertar_Contabilizacion_Automatica(user, Bean);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de asociar la Transaccion al Caso");
            return;
        }
        else
        {
            WebMsgBox.Show("La Transaccion fue asociada exitosamente");
            drp_tipo_transaccion.SelectedIndex = 0;
            drp_tipo_operacion.SelectedIndex = 0;
            drp_transaccion.SelectedIndex = 0;
            Get_Transacciones_Asociadas();
            drp_transaccion_padre.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_transaccion_padre.Items.Add(item);
            Arr = null;
            Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Transacciones_Asociar(int.Parse(drp_caso.SelectedValue), "");
            foreach (RE_GenericBean Bean_Transacciones in Arr)
            {
                item = new ListItem(Bean_Transacciones.strC1 + " - TTR.: " + Bean_Transacciones.strC2 + " - Tpi Destino.: " + Bean_Transacciones.strC4 + " - Transaccion.: " + Bean_Transacciones.strC5 + " - Operacion.: " + Bean_Transacciones.strC6, Bean_Transacciones.intC1.ToString());
                drp_transaccion_padre.Items.Add(item);
            }
        }
    }
    protected void Get_Transacciones_Asociadas()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("MBL");
        dt.Columns.Add("HBL");
        dt.Columns.Add("ROUTING");
        dt.Columns.Add("IMP_EXP");
        dt.Columns.Add("TTR");
        dt.Columns.Add("TPI_ORIGEN");
        dt.Columns.Add("TPI_DESTINO");
        dt.Columns.Add("TRANSACCION");
        dt.Columns.Add("OPERACION");
        dt.Columns.Add("IDPADRE");
        Arr = null;
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Transacciones_Asociar(int.Parse(drp_caso.SelectedValue), "");
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
            object[] ObjArr = { Bean.intC1.ToString(), Bean.strC1, MBL, HBL, Bean.boolC3.ToString(), IMP_EXP, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.intC9.ToString() };
            dt.Rows.Add(ObjArr);
        }
        gv_transacciones_asociadas.DataSource = dt;
        gv_transacciones_asociadas.DataBind();
    }
    protected void gv_transacciones_asociadas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string Id = gv_transacciones_asociadas.Rows[e.RowIndex].Cells[1].Text;
        string sql = "update tbl_contabilizacion_automatica set tca_estado=0 where tca_id=" + Id + " ";
        int resultado = DB.Contabilizacion_Automatica_Ejecutar_Update(user, sql);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de eliminar la Transaccion Asociada.");
            return;
        }
        else
        {
            WebMsgBox.Show("Transaccion eliminada exitosamente.");
            Get_Transacciones_Asociadas();
            return;
        }
    }
}