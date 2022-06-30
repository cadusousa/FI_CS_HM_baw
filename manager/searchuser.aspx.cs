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

public partial class manager_searchuser : System.Web.UI.Page
{
    public ArrayList usuario_arr = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }
        usuario_arr = (ArrayList)DB.getUsuarios("");
        if (!Page.IsPostBack){
            /* obtengo el listado de paises */
            ArrayList paises_arr = (ArrayList)DB.getPaises("");
            ListItem item = null;
            PaisBean paisbean = new PaisBean();
            for (int i = 0; i < paises_arr.Count; i++)
            {
                paisbean = (PaisBean)paises_arr[i];
                item = new ListItem(paisbean.Nombre, paisbean.ID.ToString());
                lb_pais.Items.Add(item);
            }
            lb_pais_SelectedIndexChanged(sender, e);
        }

    }
    protected void lb_pais_SelectedIndexChanged(object sender, EventArgs e)
    {
        lb_sucursal.Items.Clear();
        int pai_id = int.Parse(lb_pais.SelectedValue);
        ArrayList sucursal_arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + pai_id);
        SucursalBean sucursalbean = new SucursalBean();
        ListItem lb = null;
        for (int j = 0; j < sucursal_arr.Count; j++)
        {
            sucursalbean = (SucursalBean)sucursal_arr[j];
            lb = new ListItem(sucursalbean.Nombre, sucursalbean.ID.ToString());
            lb_sucursal.Items.Add(lb);
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        string sql = "";
        if ((tb_user!=null) && (!tb_user.Text.Equals(""))) { sql+=" and usu_id='"+tb_user.Text+"'"; }
        if ((tb_nombre!=null) && (!tb_nombre.Text.Equals(""))) { sql+=" and usu_nombre='"+tb_nombre.Text+"'"; }
        sql += " and usu_pais=" + lb_pais.SelectedValue;
        sql += " and usu_sucursal=" + lb_sucursal.SelectedValue;
        if (chk_estado.Checked) {
            sql += " and usu_estado=1";
        } else {
            sql += " and usu_estado=0";
        }
        usuario_arr = (ArrayList)DB.getUsuarios(sql);
    }
}
