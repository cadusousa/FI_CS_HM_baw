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

public partial class manager_admin_retencion : System.Web.UI.Page
{
    UsuarioBean user;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            ArrayList paises_arr = (ArrayList)DB.getPaises("");
            ListItem item = null;
            PaisBean paisbean = new PaisBean();

            for (int i = 0; i < paises_arr.Count; i++)
            {
                paisbean = (PaisBean)paises_arr[i];
                item = new ListItem(paisbean.Nombre, paisbean.ID.ToString());
                lb_pais.Items.Add(item);
            }

        }
    }
    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and rtrim(trt_nombre) ilike '%" + tb_nombreb.Text.Trim() + "%'";
        Arr = DB.getRetenciones(where);
        dt = (DataTable)Utility.fillGridView("Retencion", Arr);
        ViewState["dt"] = dt;
        dgw.DataSource = dt;
        dgw.DataBind();
        mpeSeleccion.Show();
    }
    protected void dgw_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = dgw.SelectedRow;
        tb_id.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
        tb_nombre.Text= Page.Server.HtmlDecode(row.Cells[2].Text);
        lb_tipo.SelectedValue= row.Cells[3].Text;
        tb_minimo.Text= Page.Server.HtmlDecode(row.Cells[4].Text);
        tb_porcentaje.Text= Page.Server.HtmlDecode(row.Cells[5].Text);
        lb_pais.SelectedValue = row.Cells[6].Text;
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        int result = 0;
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.intC1 = int.Parse(tb_id.Text);//id
        rgb.strC1 = tb_nombre.Text;// nombre
        rgb.intC2 = int.Parse(lb_tipo.SelectedValue);// tipo
        rgb.decC1 = decimal.Parse(tb_minimo.Text);//minimo
        //rgb.intC3 = int.Parse(tb_porcentaje.Text);//porcentaje

        rgb.decC2 = decimal.Parse(tb_porcentaje.Text);//minimo

        rgb.intC4 = int.Parse(lb_pais.SelectedValue);//paisID

        if (rgb.intC1 == 0)
            result = DB.AddRetencion(rgb, user);
        else
            result = DB.UpdateRetencion(rgb, user);

        if (result != 1)
        {
            WebMsgBox.Show("Existio un problema al tratar de grabar la informacion, \r\n por favor comuniquese con el administrador del sistema");
        }
        else
        {
            WebMsgBox.Show("Los datos se grabaron exitosamente");
        }
    }
}
