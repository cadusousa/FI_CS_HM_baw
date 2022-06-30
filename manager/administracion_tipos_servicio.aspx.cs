using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_administracion_tipos_servicio : System.Web.UI.Page
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
        if (!Page.IsPostBack)
        {
            Obtengo_listas();
        }
    }
    protected void Obtengo_listas()
    {
        arr = (ArrayList)DB.getPaises("");
        item = new ListItem("Seleccione...", "0");
        drp_paises.Items.Add(item);
        foreach (PaisBean pais in arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_paises.Items.Add(item);
        }
        drp_paises.SelectedIndex = 0;
        arr = null;
        arr = (ArrayList)DB.GetDocumentosBySYS_TipoTeferencia();
        item = new ListItem("Seleccione...");
        drp_documentos.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_documentos.Items.Add(item);
        }
        drp_documentos.SelectedIndex = 0;
        drp_sucursales.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sucursales.Items.Add(item);
        drp_sucursales.SelectedIndex = 0;
    }
    protected void btn_visualizar_Click(object sender, EventArgs e)
    {
        int contaID = 0;
        int ttrID = 0;
        int paiID = 0;
        int sucID = 0;
        if (drp_paises.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un Pais");
            return;
        }
        else if (drp_sucursales.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar una sucursal");
            return;
        }
        else if (drp_documentos.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un tipo de Documeto");
            return;
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("TIPO SERVICIO");
        arr = null;
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_servicio");
        foreach (RE_GenericBean rgb in arr)
        {
            object[] obj = { rgb.intC1, rgb.strC1 };
            dt.Rows.Add(obj);
        }
        gv_tipos_servicio.DataSource = dt;
        gv_tipos_servicio.DataBind();
        paiID = int.Parse(drp_paises.SelectedValue.ToString());
        contaID = int.Parse(drp_contabilidad.SelectedValue.ToString());
        ttrID = int.Parse(drp_documentos.SelectedValue.ToString());
        sucID = int.Parse(drp_sucursales.SelectedValue.ToString());
        ArrayList Arr_TS = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(paiID, contaID, ttrID, sucID);
        if (Arr_TS != null)
        {
            GridViewRowCollection gvr = gv_tipos_servicio.Rows;
            CheckBox chk;
            foreach (GridViewRow row in gvr)
            {
                foreach (RE_GenericBean Bean in Arr_TS)
                {
                    chk = (CheckBox)row.FindControl("chk_estado");
                    if (Bean.intC1.ToString() == row.Cells[1].Text)
                    {
                        chk.Checked = true;
                    }
                }
            }
        }
    }
    protected void btn_asignar_Click(object sender, EventArgs e)
    {
        ArrayList Arr_TS = new ArrayList();
        int contaID = 0;
        int ttrID = 0;
        int paiID = 0;
        int sucID = 0;
        if (drp_paises.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un Pais");
            return;
        }
        else if (drp_sucursales.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar una sucursal");
            return;
        }
        else if (drp_documentos.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un tipo de Documeto");
            return;
        }
        paiID = int.Parse(drp_paises.SelectedValue.ToString());
        contaID = int.Parse(drp_contabilidad.SelectedValue.ToString());
        ttrID = int.Parse(drp_documentos.SelectedValue.ToString());
        sucID = int.Parse(drp_sucursales.SelectedValue.ToString());
        GridViewRowCollection gvrs = gv_tipos_servicio.Rows;
        CheckBox chk;
        foreach (GridViewRow row in gvrs)
        {
            chk = (CheckBox)row.FindControl("chk_estado");
            if (chk.Checked == true)
            {
                Arr_TS.Add(row.Cells[1].Text);
            }
        }
        int resultado = DB.Update_Tipos_Servicio_Permitidos(user, Arr_TS, paiID, contaID, ttrID, sucID);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al momento de tratar de actualizar los Tipos de Servicio permitidos");
            return;
        }
        else
        {
            WebMsgBox.Show("Tipos de Servicio Permitidos actualizados correctamente");
            gv_tipos_servicio.DataBind();
            drp_paises.SelectedIndex = 0;
            drp_documentos.SelectedIndex = 0;
            return;
        }
    }
    protected void drp_paises_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_paises.SelectedValue != "0")
        {
            drp_sucursales.Items.Clear();
            arr = null;
            arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + drp_paises.SelectedValue + "");
            item = new ListItem("Seleccione...", "0");
            drp_sucursales.Items.Add(item);
            foreach (SucursalBean Bean in arr)
            {
                item = new ListItem(Bean.Nombre, Bean.ID.ToString());
                drp_sucursales.Items.Add(item);
            }
            drp_sucursales.SelectedIndex = 0;
        }
        else
        {
            drp_sucursales.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_sucursales.Items.Add(item);
            drp_sucursales.SelectedIndex = 0;
        }
    }
}