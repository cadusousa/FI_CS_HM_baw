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

public partial class manager_administrar_restricciones : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
            Cargar_Restricciones();
        }
    }
    protected void Obtengo_listas()
    {
        arr = (ArrayList)DB.getPaises("");
        item = new ListItem("Seleccione...","0");
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
        arr = null;
        arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + user.PaisID + "");
        item = new ListItem("Seleccione...", "0");
        drp_sucursales.Items.Add(item);
        foreach (SucursalBean Bean in arr)
        {
            item = new ListItem(Bean.Nombre, Bean.ID.ToString());
            drp_sucursales.Items.Add(item);
        }
        drp_sucursales.SelectedIndex = 0;
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = int.Parse(Menu1.SelectedValue);
        drp_paises.SelectedIndex = 0;
        drp_contabilidad.SelectedIndex = 0;
        drp_documentos.SelectedIndex = 0;
        gv_restricciones3.Visible = false;
        Limpiar_CheckBoxs();
    }
    protected void btn_ingresar_Click(object sender, EventArgs e)
    {
        int resultado = 0;
        if (tb_nombre.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Ingresar el Nombre de la Restriccion");
            return;
        }
        else if (tb_descripcion.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Ingresar la Descripcion de la Restriccion");
            return;
        }
        else 
        {
            RE_GenericBean Bean = new RE_GenericBean();
            Bean.strC1 = tb_nombre.Text.Trim();
            Bean.strC2 = tb_descripcion.Text.Trim();
            resultado = DB.Insertar_Restriccion(user, Bean);
            if (resultado == -100)
            {
                WebMsgBox.Show("Existió un problema al tratar de guardar la información, por favor intente de nuevo");
                return;
            }
            else
            {
                WebMsgBox.Show("La Restriccion fue ingresada correctamente");
                tb_nombre.Text = "";
                tb_descripcion.Text = "";
                tb_nombre.Focus();
                MultiView1.ActiveViewIndex = 0;
                Cargar_Restricciones();
                return;
            }
        }
    }
    protected void Cargar_Restricciones()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("DESCRIPCION");
        dt.Columns.Add("ACTIVO");
        ArrayList Arr = (ArrayList)DB.Get_Restricciones();
        foreach (RE_GenericBean Bean in Arr)
        {
            object[] obj = { Bean.intC1.ToString(), Bean.strC1, Bean.strC2, Bean.strC3 };
            dt.Rows.Add(obj);
        }
        gv_restricciones.DataSource = dt;
        gv_restricciones.DataBind();
        gv_restricciones2.DataSource = dt;
        gv_restricciones2.DataBind();
        gv_restricciones3.DataSource = dt;
        gv_restricciones3.DataBind();
    }
    protected void gv_restricciones_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
        }
    }
    protected void gv_restricciones2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[0].Visible = false;
        }
    }
    protected void gv_restricciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string estado = "";
        int id = 0;
        int resultado = 0;
        if (e.CommandName == "Desactivar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            id = int.Parse(gv_restricciones.Rows[index].Cells[1].Text);
            estado = gv_restricciones.Rows[index].Cells[4].Text;
            if (id > 0)
            {
                if (estado == "True")
                {
                    estado = "False";
                }
                else
                {
                    estado = "True";
                }
                resultado = DB.Desactivar_Restriccion(user, id, estado);
                if (resultado == -100)
                {
                    WebMsgBox.Show("Existio un error al tratar de Desactivar la Restriccion");
                    return;
                }
                else
                {
                    WebMsgBox.Show("La restriccion fue modificada exitosamente");
                    tb_nombre.Text = "";
                    tb_descripcion.Text = "";
                    tb_nombre.Focus();
                    MultiView1.ActiveViewIndex = 0;
                    Cargar_Restricciones();
                    return;
                }
            }
        }
    }
    protected void gv_restricciones3_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
        }
    }
    protected void btn_visualizar_Click(object sender, EventArgs e)
    {
        int contaID = 0;
        int docID = 0;
        int paiID = 0;
        int sucID = 0;
        if (drp_paises.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un Pais");
            return;
        }
        else if (drp_documentos.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un tipo de Documeto");
            return;
        }
        else if (drp_sucursales.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal");
            return;
        }
        else
        {
            contaID = int.Parse(drp_contabilidad.SelectedValue.ToString());
            docID = int.Parse(drp_documentos.SelectedValue.ToString());
            paiID = int.Parse(drp_paises.SelectedValue.ToString());
            sucID = int.Parse(drp_sucursales.SelectedValue.ToString());
            ArrayList Arr_Restricciones = (ArrayList)DB.Get_Restricciones_XPais_Tipo(user, contaID, docID, paiID, " and a.tbrp_suc_id=" + sucID + " ");
            GridViewRowCollection gvr = gv_restricciones3.Rows;
            CheckBox chk;
            Limpiar_CheckBoxs();
            foreach (GridViewRow row in gvr)
            {
                foreach (RE_GenericBean Bean in Arr_Restricciones)
                {
                    chk = (CheckBox)row.FindControl("chk_estado");
                    if (Bean.strC2 == row.Cells[1].Text)
                    {
                        chk.Checked = true;
                    }
                }
            }
            gv_restricciones3.Visible = true;
        }
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        int contaID = 0;
        int docID = 0;
        int resultado = 0;
        int paiID = 0;
        int sucID = 0;
        if (drp_paises.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un Pais");
            return;
        }
        else if (drp_documentos.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un tipo de Documeto");
            return;
        }
        else if (drp_sucursales.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal");
            return;
        }
        else
        {
            contaID = int.Parse(drp_contabilidad.SelectedValue.ToString());
            docID = int.Parse(drp_documentos.SelectedValue.ToString());
            paiID = int.Parse(drp_paises.SelectedValue.ToString());
            sucID = int.Parse(drp_sucursales.SelectedValue.ToString());
            GridViewRowCollection gvr = gv_restricciones3.Rows;
            ArrayList Arr = new ArrayList();
            RE_GenericBean Bean = null;
            foreach (GridViewRow row in gvr)
            {
                CheckBox Box = (CheckBox)row.FindControl("chk_estado");
                if (Box.Checked == true)
                {
                    Bean = new RE_GenericBean();
                    Bean.intC1 = int.Parse(row.Cells[1].Text);
                    Bean.intC2 = paiID;
                    Bean.intC3 = docID;
                    Bean.intC4 = contaID;
                    Bean.intC5 = sucID;
                    Arr.Add(Bean);
                }
            }
            resultado = DB.Update_Restricciones(user, Arr, contaID, docID,paiID, sucID);
            if (resultado == -100)
            {
                WebMsgBox.Show("Existio un error al tratar de actualizar las restricciones");
                return;
            }
            else
            {
                WebMsgBox.Show("Restricciones actualizadas correctamente");
                Limpiar_CheckBoxs();
                drp_paises.SelectedIndex = 0;
                drp_contabilidad.SelectedIndex = 0;
                drp_documentos.SelectedIndex = 0;
                drp_sucursales.SelectedIndex = 0;
                gv_restricciones3.Visible = false;
                return;
            }
        }
    }
    protected void Limpiar_CheckBoxs()
    {
        #region Limpiar Todos los Checkbox's
        GridViewRowCollection gvr = gv_restricciones3.Rows;
        foreach (GridViewRow row in gvr)
        {
            CheckBox chk = (CheckBox)row.FindControl("chk_estado");
            chk = (CheckBox)row.FindControl("chk_estado");
            chk.Checked = false;
        }
        #endregion
    }
    protected void drp_documentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cargar_Restricciones();
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
    }
}