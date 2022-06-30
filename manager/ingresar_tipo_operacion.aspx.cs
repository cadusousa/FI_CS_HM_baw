using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class manager_ingresar_tipo_operacion : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!IsPostBack)
        {
            Get_Tipo_Operacion();
        }
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        if (tb_tipo_operacion.Text.Trim().Length == 0)
        {
            WebMsgBox.Show("Debe ingresar el Tipo de Operacion");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.strC1 = tb_tipo_operacion.Text.Trim();
        int resultado = DB.Contabilizacion_Automatica_Insertar_Tipo_Operacion(user, Bean);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de Guardar el Tipo de Operacion");
            return;
        }
        else
        {
            WebMsgBox.Show("El Tipo de Operacion fue guardado exitosamente");
            tb_tipo_operacion.Text = "";
            Get_Tipo_Operacion();
        }
    }
    protected void Get_Tipo_Operacion()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("TIPO");
        ArrayList Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Tipo_Operacion(user);
        foreach (RE_GenericBean Bean in Arr)
        {
            object[] ObjArr = { Bean.strC1, Bean.strC2 };
            dt.Rows.Add(ObjArr);
        }
        gv_tipo_operacion.DataSource = dt;
        gv_tipo_operacion.DataBind();
    }
    protected void gv_tipo_operacion_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string Id = gv_tipo_operacion.Rows[e.RowIndex].Cells[1].Text;
        string sql = "update tbl_contabilizacion_automatica_tipo_operacion set tcato_estado=0 where tcato_id=" + Id + " ";
        int resultado = DB.Contabilizacion_Automatica_Ejecutar_Update(user, sql);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de eliminar el Tipo de Operacion.");
            return;
        }
        else
        {
            WebMsgBox.Show("Tipo de Operacion eliminada exitosamente.");
            Get_Tipo_Operacion();
            return;
        }
    }
}