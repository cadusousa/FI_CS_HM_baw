using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Collections;

public partial class manager_ingresar_transaccion : System.Web.UI.Page
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
        user = (UsuarioBean)Session["usuario"];
        if (!IsPostBack)
        {
            Obtengo_Listas();
            Get_Transacciones();
        }
    }
    protected void Obtengo_Listas()
    {
        drp_transaccion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_transaccion.Items.Add(item);
        arr = (ArrayList)DB.GetDocumentosBySYS_TipoTeferencia();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_transaccion.Items.Add(item);
        }
        drp_transaccion.SelectedIndex = 0;
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        if (drp_transaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe Seleccionar el Tipo de Documento");
            return;
        }
        if (drp_tpi_origen.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe Seleccionar el Tipo de Persona Origen");
            return;
        }
        if (drp_tpi_destino.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe Seleccionar el Tipo de Persona Destino");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.intC1 = int.Parse(drp_transaccion.SelectedValue);
        Bean.intC2 = int.Parse(drp_tpi_origen.SelectedValue);
        Bean.intC3 = int.Parse(drp_tpi_destino.SelectedValue);
        string sql = "select tcatr_id from tbl_contabilizacion_automatica_transacciones where tcatr_ttr_id=" + Bean.intC1.ToString() + " and tcatr_tpi_origen_id=" + Bean.intC2.ToString() + " and tcatr_tpi_destino_id=" + Bean.intC3.ToString() + "";
        bool existencia = DB.Contabilizacion_Automatica_Validar_Existencia(user, sql);
        if (existencia == true)
        {
            WebMsgBox.Show("Transaccion existente");
            return;
        }
        int result = DB.Contabilizacion_Automatica_Insertar_Transaccion(user, Bean);
        if (result == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de Guardar la Transaccion");
            return;
        }
        else
        {
            WebMsgBox.Show("La Transaccion fue guardada exitosamente");
            Limpiar();
            Get_Transacciones();
        }
    }
    protected void Limpiar()
    {
        drp_transaccion.SelectedIndex = 0;
        drp_tpi_origen.SelectedIndex = 0;
        drp_tpi_destino.SelectedIndex = 0;
    }
    protected void Get_Transacciones()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("TIPO_DOCUMENTO");
        dt.Columns.Add("TPI_OPRIGEN");
        dt.Columns.Add("TPI_DESTINO");
        ArrayList Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Transacciones(user);
        foreach (RE_GenericBean Bean in Arr)
        {
            object[] ObjArr = { Bean.strC1, Bean.strC5, Bean.strC6, Bean.strC7 };
            dt.Rows.Add(ObjArr);
        }
        gv_transacciones.DataSource = dt;
        gv_transacciones.DataBind();
    }
    protected void gv_transacciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string Id = gv_transacciones.Rows[e.RowIndex].Cells[1].Text;
        string sql = "update tbl_contabilizacion_automatica_transacciones set tcatr_estado=0 where tcatr_id=" + Id + " ";
        int resultado = DB.Contabilizacion_Automatica_Ejecutar_Update(user, sql);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de eliminar la Transaccion.");
            return;
        }
        else
        {
            WebMsgBox.Show("Transaccion eliminada exitosamente.");
            Get_Transacciones();
            return;
        }
    }
}