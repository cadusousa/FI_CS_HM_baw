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
public partial class manager_asignar_grupo : System.Web.UI.Page
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
        item = new ListItem("Seleccione...");
        drp_departamentos.Items.Add(item);
        drp_grupo_asignado.Items.Add(item);
        drp_grupos.Items.Add(item);
    }
    protected void Cargar_Departamentos()
    {
        int paidID = 0;
        if (drp_paises.SelectedItem.Text != "Seleccione...")
        {
            paidID = int.Parse(drp_paises.SelectedValue.ToString());
            drp_departamentos.Items.Clear();
            drp_departamentos.Items.Add("Seleccione...");
            ArrayList Arr_Departamentos = (ArrayList)DB.Get_Departamentos(user, paidID);
            foreach (RE_GenericBean Bean in Arr_Departamentos)
            {
                item = new ListItem(Bean.strC2, Bean.strC1);
                drp_departamentos.Items.Add(item);
            }
            drp_departamentos.Enabled = true;
        }
        else
        {
            drp_departamentos.Enabled = false;
            drp_departamentos.SelectedIndex = 0;
            drp_grupo_asignado.Enabled = false;
            drp_grupos.Enabled = false;
            drp_grupos.SelectedIndex = 0;
            drp_grupo_asignado.SelectedIndex = 0;
        }
    }
    protected void Cargar_Grupos()
    {
        if (drp_departamentos.SelectedItem.Text != "Seleccione...")
        {
            ArrayList Arr_Grupos = new ArrayList();
            Arr_Grupos = (ArrayList)DB.Get_Grupos();
            drp_grupos.Items.Clear();
            drp_grupos.Items.Add("Seleccione...");
            foreach (RE_GenericBean Bean in Arr_Grupos)
            {
                item = new ListItem(Bean.strC2, Bean.strC1);
                drp_grupos.Items.Add(item);
            }
            drp_grupos.Enabled = true;
            drp_grupo_asignado.Enabled = true;
        }
        else
        {
            drp_grupo_asignado.Enabled = false;
            drp_grupos.Enabled = false;
            drp_grupos.SelectedIndex = 0;
            drp_grupo_asignado.SelectedIndex = 0;
        }
    }
    protected void drp_paises_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cargar_Departamentos();
    }
    protected void drp_departamentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cargar_Grupos();
        Cargar_Grupo_Asignado();
        btn_asignar.Enabled = true;
    }
    protected void Cargar_Grupo_Asignado()
    {
        int departamentoID = 0;
        if (drp_departamentos.SelectedItem.Text != "Seleccione...")
        {
            departamentoID = int.Parse(drp_departamentos.SelectedValue.ToString());
            ArrayList Arr_grupo = new ArrayList();
            Arr_grupo = (ArrayList)DB.Get_Grupos_XDepartamento(departamentoID);
            drp_grupo_asignado.Items.Clear();
            drp_grupo_asignado.Items.Add("Seleccione...");
            foreach (RE_GenericBean Bean in Arr_grupo)
            {
                if (Bean.strC1 != "0")
                {
                    item = new ListItem(Bean.strC2, Bean.strC1);
                    drp_grupo_asignado.Items.Add(item);
                }
            }
        }
    }
    protected void btn_asignar_Click(object sender, EventArgs e)
    {
        int resultado = 0;
        if ((drp_grupos.SelectedItem.Text != "Seleccione...") && (drp_departamentos.SelectedItem.Text != "Seleccione..."))
        {
            int departamentoID = 0;
            int grupoID = 0;
            departamentoID = int.Parse(drp_departamentos.SelectedValue.ToString());
            grupoID = int.Parse(drp_grupos.SelectedValue.ToString());
            resultado = DB.Actualizar_Departamento_Grupo(user, departamentoID, grupoID);
            if (resultado == -100)
            {
                WebMsgBox.Show("Existio un error al tratar de Asociar el Grupo");
                return;
            }
            else
            {
                WebMsgBox.Show("El grupo fue asociado exitosamente");
                drp_departamentos.Enabled = false;
                drp_departamentos.SelectedIndex = 0;
                drp_grupo_asignado.Enabled = false;
                drp_grupos.Enabled = false;
                drp_grupos.SelectedIndex = 0;
                drp_grupo_asignado.SelectedIndex = 0;
                return;
            }
        }
    }
}