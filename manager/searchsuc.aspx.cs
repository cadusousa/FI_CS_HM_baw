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

public partial class manager_search : System.Web.UI.Page
{
    public ArrayList sucursal_arr = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }

        UsuarioBean user1 = (UsuarioBean)Session["usuario"];
        if (!user1.Aplicaciones.Contains("4"))
        {
            Response.Redirect("manager.aspx");
        }
        int permiso = int.Parse(user1.Aplicaciones["4"].ToString());
        if (!((permiso & 1) == 1) && !((permiso & 2) == 2) && !((permiso & 4) == 4) && !((permiso & 8) == 8))
        {
            Response.Redirect("manager.aspx");
        }
        ArrayList paises_arr = (ArrayList)DB.getPaises("");
        ListItem item = null;
        PaisBean paisbean = new PaisBean();
        if (!Page.IsPostBack)
        {
            item = new ListItem("Todos", "Todos");
            lb_pais.Items.Add(item);
            for (int i = 0; i < paises_arr.Count; i++)
            {
                paisbean = (PaisBean)paises_arr[i];
                item = new ListItem(paisbean.Nombre, paisbean.ID.ToString());
                lb_pais.Items.Add(item);
            }
            string pais = lb_pais.SelectedValue;
            string sql = "";
            if ((pais != null) && (!pais.Equals("Todos")))
            {
                sql += "and suc_pai_id=" + pais;
            }
            sucursal_arr = DB.getSucursales(sql);
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre.Text.Trim();
        string pais = lb_pais.SelectedValue;
        string sql = "";
        sucursal_arr = null;
        if ((nombre != null) && (!nombre.Equals(""))) {
            sql = "and suc_nombre='" + nombre + "'";
        }
        if ((pais != null) && (!pais.Equals("Todos"))) {
            sql += "and suc_pai_id=" + pais;
        }
        sucursal_arr = DB.getSucursales(sql);
    }
}
