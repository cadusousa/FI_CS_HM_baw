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

public partial class operations_Default : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null) Response.Redirect("../default.aspx");
        user = (UsuarioBean)Session["usuario"];
        if(!Page.IsPostBack) {
            ArrayList arr = (ArrayList)DB.GetRegimenTributarioList(user.PaisID);
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr){
                item=new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tiporegimen.Items.Add(item);
            }
            //obtengo todos los tipos de retencion de este proveedor
            //Obtengo listado de usuarios
            lb_cobrador.Items.Clear();
            arr = (ArrayList)DB.getUsuarios("");
            item = null;
            foreach (UsuarioBean usuarios in arr)
            {
                //user = (UsuarioBean)DB.getUsuariosData(user);
                item = new ListItem(usuarios.ID, usuarios.No.ToString());
                lb_cobrador.Items.Add(item);
            }

            //Obtengo listado de grupos
            lb_grupo.Items.Clear();
            arr = (ArrayList)DB.getGrupos();
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_grupo.Items.Add(item);
            }

            //Obtengo listado de clases
            lb_clase.Items.Clear();
            arr = (ArrayList)DB.getClases();
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC2, rgb.strC1);
                lb_clase.Items.Add(item);
            }

            //Obtengo listado de tipos de cliente
            lb_tipolciente.Items.Clear();
            arr = (ArrayList)DB.getTiposCliente();
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipolciente.Items.Add(item);
            }
        }
    }
    protected void bt_cancelar_Click(object sender, EventArgs e)
    {
        //tb_contacto.Text = "";
        //tb_correo.Text = "";
        //tb_nombre_facturar.Text = "";
        //tb_direccion.Text = "";
        //tb_fax.Text = "";
        //tb_nit.Text = "";
        //tb_nombre.Text = "";
        //tb_obs.Text = "";
        //tb_telefono.Text = "";
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        RE_GenericBean cliente = new RE_GenericBean();
        cliente.intC1 = int.Parse(tb_id.Text);
        cliente.strC1 = tb_nombre.Text;
        cliente.strC2 = tb_nit.Text;
        cliente.strC3 = tb_nit2.Text;
        cliente.strC4 = tb_nombre_facturar.Text;
        cliente.intC2 = int.Parse(lb_tipolciente.SelectedValue);
        cliente.intC3 = int.Parse(lb_grupo.SelectedValue);
        cliente.intC4 = int.Parse(lb_cobrador.SelectedValue);
        cliente.intC5 = int.Parse(lb_estadocliente.SelectedValue);
        cliente.boolC1=chk_consignee.Checked;
        cliente.boolC2=chk_shipper.Checked;
        cliente.strC6 = lb_clase.SelectedValue;
        cliente.strC7 = lb_pais.SelectedValue;
        cliente.strC5 = tb_obs.Text;
        cliente.intC10 = int.Parse(lb_tiporegimen.SelectedValue);

        int result = 0;
        if (tb_id.Text.Equals("0"))
            result = DB.AddCliente(cliente, user);
        else
            result = DB.UpdateCliente(cliente, user);

        if (result != 1)
        {
            WebMsgBox.Show("Existio un problema al tratar de grabar la informacion, \r\n por favor comuniquese con el administrador del sistema");
        }
        else
        {
            WebMsgBox.Show("Los datos se grabaron exitosamente");
        }
    }
    protected void bt_eliminar_Click(object sender, EventArgs e)
    {

    }

    protected void gv_proveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["proveedordt"];
        gv_proveedor.DataSource = dt1;
        gv_proveedor.PageIndex = e.NewPageIndex;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
    protected void gv_proveedor_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_proveedor.SelectedRow;
        tb_id.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
        RE_GenericBean rgb = (RE_GenericBean)DB.getDatosCliente(double.Parse(tb_id.Text));

        tb_id.Text=rgb.intC1.ToString();
        tb_nombre.Text=rgb.strC2;
        tb_nit.Text=rgb.strC1;
        tb_nit2.Text=rgb.strC8;
        tb_nombre_facturar.Text=rgb.strC3;
        lb_tipolciente.SelectedValue=rgb.intC2.ToString();
        lb_grupo.SelectedValue=rgb.intC3.ToString();
        lb_cobrador.SelectedValue=rgb.intC4.ToString();;
        lb_estadocliente.SelectedValue=rgb.intC5.ToString();
        chk_consignee.Checked=rgb.boolC1;
        chk_shipper.Checked=rgb.boolC2;
        lb_clase.SelectedValue=rgb.strC6;
        lb_pais.SelectedValue=rgb.strC7;
        tb_obs.Text=rgb.strC9;
    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
            ArrayList Arr = null;
            string where = "";
            Arr = Utility.getProveedor("XNitNombre", where);
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
            ViewState["proveedordt"] = dt;
            gv_proveedor.DataSource = dt;
            gv_proveedor.DataBind();
        }
    }
    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null) where += " and rtrim(codigo_tributario) like '%" + tb_nitb.Text.Trim().ToUpper() + "%'";
        if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) {
            if (where.Equals("")) where += " upper(rtrim(nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'"; else where += " and upper(rtrim(nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
        } 
        Arr = (ArrayList)DB.getClientes(where, user, ""); //cliente
        dt = (DataTable)Utility.fillGridView("cliente", Arr);
        ViewState["proveedordt"] = dt;
        gv_proveedor.DataSource = dt;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
}
