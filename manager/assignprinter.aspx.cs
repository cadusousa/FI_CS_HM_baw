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
            ListItem item = null;
            arr = null;
            arr = (ArrayList)DB.getPaises("");
            foreach (PaisBean pais in arr)
            {
                item = new ListItem(pais.Nombre, pais.ID.ToString());
                lb_pais.Items.Add(item);
            }
            lb_pais.SelectedValue = user.PaisID.ToString();
            arr = (ArrayList)DB.getUsuarios("");
            item = null;
            foreach (UsuarioBean usuario in arr)
            {
                item = new ListItem(usuario.ID, usuario.ID);
                lb_usuario.Items.Add(item);
            }
            string criterio = " where imp_pai_id=" + lb_pais.SelectedValue;
            arr = (ArrayList)DB.Get_Impresoras(criterio);
            item = null;
            lb_impresora.Items.Clear();
            lb_impresora_default.Items.Clear();
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_impresora.Items.Add(item);
                lb_impresora_default.Items.Add(item);
            }
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        
        RE_GenericBean fac_bean = new RE_GenericBean();
        fac_bean.intC1 = int.Parse(lb_impresora.SelectedValue);//Impresora ID
        fac_bean.strC1 = lb_usuario.SelectedValue;//Usuario ID
        if (CheckBox1.Checked == true)
        {
            fac_bean.intC2 = int.Parse(lb_impresora_default.SelectedValue);//Impresora ID Default
        }
        else
        {
            fac_bean.intC2 = 0;
        }
        int result = DB.AssingPrinter(fac_bean);
        if ((result == 0) || (result == -100))
        {
            WebMsgBox.Show("Error, No se pudo Asignar la Impresora");
            return;
        }
        else
        {
            WebMsgBox.Show("Impresora Asignada correctamente");
            return;
        }
    }
    protected void lb_pais_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        string criterio = " where imp_pai_id=" + lb_pais.SelectedValue;
        arr = (ArrayList)DB.Get_Impresoras(criterio);
        item = null;
        lb_impresora.Items.Clear();
        lb_impresora_default.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_impresora.Items.Add(item);
            lb_impresora_default.Items.Add(item);
        }
    }
}
