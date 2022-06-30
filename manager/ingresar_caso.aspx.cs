using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class manager_ingresar_caso : System.Web.UI.Page
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
            Get_Casos();
        }
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        if (rbl_tipo_mbl.SelectedValue == "")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de MBL.");
            return;
        }
        if (rbl_tipo_hbl.SelectedValue == "")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de HBL.");
            return;
        }
        if (rbl_es_ruteado.SelectedValue == "")
        {
            WebMsgBox.Show("Debe indicar si la carga es Ruteada o No");
            return;
        }
        if (rbl_imp_exp.SelectedValue == "")
        {
            WebMsgBox.Show("Debe seleccionar si el Caso es Import o Export");
            return;
        }
        if (tb_nombre.Text.Trim().Length == 0)
        {
            WebMsgBox.Show("Debe Ingresar el nombre o descripcion del Caso.");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.boolC1 = bool.Parse(rbl_tipo_mbl.SelectedValue);
        Bean.boolC2 = bool.Parse(rbl_tipo_hbl.SelectedValue);
        Bean.boolC3 = bool.Parse(rbl_es_ruteado.SelectedValue);
        Bean.boolC4 = bool.Parse(rbl_imp_exp.SelectedValue);
        Bean.strC1 = tb_nombre.Text.Trim();
        int resultado = DB.Contabilizacion_Automatica_Insertar_Casos(user, Bean);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de Guardar el caso.");
            return;
        }
        else
        {
            WebMsgBox.Show("El Caso fue guardado exitosamente");
            Limpiar();
            Get_Casos();
        }
    }
    protected void Limpiar()
    {
        rbl_tipo_mbl.SelectedValue = "True";
        rbl_tipo_hbl.SelectedValue = "True";
        rbl_es_ruteado.SelectedValue = "True";
        rbl_imp_exp.SelectedValue = "True";
        tb_nombre.Text = "";
    }
    protected void Get_Casos()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("MBL");
        dt.Columns.Add("HBL");
        dt.Columns.Add("ROUTING");
        dt.Columns.Add("IMP_EXP");
        ArrayList Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Casos(user);
        foreach (RE_GenericBean Bean in Arr)
        {
            string MBL = "";
            string HBL = "";
            string IMP_EXP = "";
            if (Bean.strC3 == "True")
            {
                MBL = "Prepaid";
            }
            else
            {
                MBL = "Collect";
            }
            if (Bean.strC4 == "True")
            {
                HBL = "Prepaid";
            }
            else
            {
                HBL = "Collect";
            }
            if (Bean.strC6 == "True")
            {
                IMP_EXP = "Import";
            }
            else
            {
                IMP_EXP = "Export";
            }
            object[] ObjArr = { Bean.strC1, Bean.strC2, MBL, HBL, Bean.strC5, IMP_EXP };
            dt.Rows.Add(ObjArr);
        }
        gv_casos.DataSource = dt;
        gv_casos.DataBind();
    }
    protected void gv_casos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string Id = gv_casos.Rows[e.RowIndex].Cells[1].Text;
        string sql = "update tbl_contabilizacion_automatica_casos set tcaca_estado=0 where tcaca_id=" + Id + " ";
        int resultado = DB.Contabilizacion_Automatica_Ejecutar_Update(user, sql);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de eliminar el caso.");
            return;
        }
        else
        {
            WebMsgBox.Show("Caso eliminado exitosamente.");
            Get_Casos();
            return;
        }
    }
}