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

public partial class manager_searchdepartamento : System.Web.UI.Page
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
        arr = (ArrayList)DB.getPaisesbyUser(user.ID);
        item = new ListItem("Seleccione...");
        drp_paises.Items.Add(item);
        foreach (PaisBean pais in arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_paises.Items.Add(item);
        }
        drp_paises.SelectedIndex = 0;
    }
    protected void btn_visualizar_Click(object sender, EventArgs e)
    {
        ArrayList Arr_Departamentos = new ArrayList();
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("DESCRIPCION");
        dt.Columns.Add("PAIS_ID");
        dt.Columns.Add("RESTRINGIDO");
        int paiID=0;
        if (drp_paises.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un pais");
            return;
        }
        else
        {
            paiID= int.Parse(drp_paises.SelectedValue.ToString());
            Arr_Departamentos = (ArrayList)DB.Get_Departamentos(user, paiID);
            foreach (RE_GenericBean Bean in Arr_Departamentos)
            {
                object[] obj = { Bean.strC1, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5 };
                dt.Rows.Add(obj);
            }
            gv_departamentos.DataSource = dt;
            gv_departamentos.DataBind();
        }
    }
    protected void gv_departamentos_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
        }
    }
    protected void gv_departamentos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string estado = "";
        int id = 0;
        int resultado = 0;
        if (e.CommandName == "activar/desactivar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            id = int.Parse(gv_departamentos.Rows[index].Cells[1].Text);
            estado = gv_departamentos.Rows[index].Cells[5].Text;
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
                resultado = DB.Desactivar_Restriccion_XPais(user, id, estado);
                if (resultado == -100)
                {
                    WebMsgBox.Show("Existio un error al tratar de Desactivar la Restriccion al Departamento");
                    return;
                }
                else
                {
                    if (estado == "True")
                    {
                        WebMsgBox.Show("El Departamento fue restringido exitosamente");
                    }
                    else
                    {
                        WebMsgBox.Show("La restriccion fue desactivada exitosamente");
                    }
                    drp_paises.SelectedIndex = 0;
                    gv_departamentos.DataBind();
                    return;
                }
            }
        }
    }
}