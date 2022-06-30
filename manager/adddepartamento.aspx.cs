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

public partial class manager_adddepartamento : System.Web.UI.Page
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
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        int result = 0;
        if (drp_paises.SelectedItem.Text == "Seleccione...")
        {
            WebMsgBox.Show("Debe seleccionar un Pais");
            return;
        }
        else if (tb_nombre.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Ingresar el Nombre del Departamento");
            return;
        }
        else
        {
            RE_GenericBean Bean = new RE_GenericBean();
            Bean.intC1 = int.Parse(drp_paises.SelectedValue.ToString());
            Bean.strC1 = tb_nombre.Text.Trim();
            Bean.strC2 = tb_descripcion.Text.Trim();
            result = DB.Insertar_Departamento(user, Bean);
            if (result == -100)
            {
                WebMsgBox.Show("Existio un error al tratar de guardar el departamento");
                return;
            }
            else 
            {
                WebMsgBox.Show("El departarmento fue creado exitosamente");
                drp_paises.SelectedIndex = 0;
                tb_nombre.Text = "";
                tb_descripcion.Text = "";
                tb_nombre.Focus();
                return;
            }


        }
    }
}