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

public partial class manager_addfactura : System.Web.UI.Page
{
    UsuarioBean user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"].ToString() == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!Page.IsPostBack)
        {
            ArrayList arr = null;
            ListItem itemito;
            ListItem item = null;
            ListItem it = null;
            arr = null;
            arr = (ArrayList)DB.getPaises("");
            foreach (PaisBean pais in arr)
            {
                item = new ListItem(pais.Nombre, pais.ID.ToString());
                lb_pais.Items.Add(item);
            }
            lb_pais.SelectedValue = user.PaisID.ToString();
            arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + lb_pais.SelectedValue);
            foreach (SucursalBean suc in arr)
            {
                item = new ListItem(suc.Nombre, suc.ID.ToString());
                lb_sucursal.Items.Add(item);
            }
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        if (tb_impresora.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Ingresar la Ruta y nombre de la Impresora");
            return;
        }
        RE_GenericBean fac_bean = new RE_GenericBean();
        fac_bean.intC1 = int.Parse(lb_pais.SelectedValue);//Pais
        fac_bean.intC2 = int.Parse(lb_sucursal.SelectedValue);//Sucursal
        fac_bean.strC1 = tb_impresora.Text.Trim();//Impresora
        int result = DB.InsertPrinter(fac_bean);
        if ((result == 0) || (result == -100))
        {
            WebMsgBox.Show("Error, No se pudo Agregar la Impresora");
            return;
        }
        else
        {
            tb_impresora.Text = "";
            WebMsgBox.Show("Impresora Agregada correctamente");
            return;
        }
    }
    protected void lb_pais_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        arr = null;
        arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + lb_pais.SelectedValue);
        lb_sucursal.Items.Clear();
        foreach (SucursalBean suc in arr)
        {
            item = new ListItem(suc.Nombre, suc.ID.ToString());
            lb_sucursal.Items.Add(item);
        }
    }
}
