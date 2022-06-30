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
        UsuarioBean user1 = (UsuarioBean)Session["usuario"];
        if (!user1.Aplicaciones.Contains("7"))
        {
            Response.Redirect("manager.aspx");
        }
        int permiso = int.Parse(user1.Aplicaciones["7"].ToString());
        if (!((permiso & 4) == 4))
        {
            Response.Redirect("manager.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if(!Page.IsPostBack) {
            ArrayList arr = (ArrayList)DB.GetRegimenTributarioList(user.PaisID);
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr){
                item=new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tiporegimen.Items.Add(item);
            }
            item = null;
            arr = null;           
            arr = (ArrayList)DB.getRetencionesList(user.PaisID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                chklist_retencion.Items.Add(item);
            }
            //obtengo todos los tipos de retencion de este proveedor
        }
    }
    protected void bt_cancelar_Click(object sender, EventArgs e)
    {
        tb_contacto.Text = "";
        tb_correo.Text = "";
        tb_descripcion.Text = "";
        tb_direccion.Text = "";
        tb_fax.Text = "";
        tb_nit.Text = "";
        tb_nombre.Text = "";
        tb_obs.Text = "";
        tb_telefono.Text = "";
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        RE_GenericBean prov = new RE_GenericBean();
        prov.strC1 = tb_nit.Text;//nit
        prov.strC2 = tb_nombre.Text;//nombre
        prov.strC3 = tb_descripcion.Text;//descripcion
        prov.intC1 = int.Parse(lb_estado.SelectedValue);//estado
        prov.intC2 = int.Parse(rradioprovde.SelectedValue);//bienes
        prov.intC3 = int.Parse(radioclasificacion.SelectedValue);//clasificacion
        if (chk_fovial.Checked) prov.intC4 = 1;
        prov.strC4 = tb_direccion.Text;
        prov.strC5 = tb_telefono.Text;
        prov.strC6 = tb_fax.Text;
        prov.strC7 = tb_correo.Text;
        prov.strC8 = tb_contacto.Text;
        prov.strC9 = tb_obs.Text;
        prov.intC5 = int.Parse(tb_dias.Text);
        prov.intC6 = int.Parse(lb_tiporegimen.SelectedValue);
        prov.decC1 = decimal.Parse(tb_monto.Text);
        if (chk_ordencompra.Checked) prov.intC7 = 1;
        prov.strC10 = user.ID;
        prov.strC11 = tb_nit2.Text;
        prov.strC12 = rb_tipopersona.SelectedValue;
        prov.intC8 = int.Parse(tb_id.Text);
        int result=0;
        RE_GenericBean retenciones = null;
        for (int a = 0; a < chklist_retencion.Items.Count; a++) {
            if (chklist_retencion.Items[a].Selected) {
                retenciones = new RE_GenericBean();
                retenciones.intC1 = int.Parse(chklist_retencion.Items[a].Value);
                if (prov.arr1 == null) prov.arr1 = new ArrayList();
                prov.arr1.Add(retenciones);
            }
        }

        if (tb_id.Equals("0"))
            result = DB.AddProveedor(prov, user);
        else
            result=DB.UpdateProveedor(prov, user);
        
        if (result!=1){
            WebMsgBox.Show("Existio un problema al tratar de grabar la informacion, \r\n por favor comuniquese con el administrador del sistema");
        } else {
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
        ArrayList arr = (ArrayList)DB.getProveedor(" and numero=" + tb_id.Text, "");
        int regimen = DB.GetRegimenProveedor(int.Parse(tb_id.Text), user.PaisID);
        
        RE_GenericBean rgb = (RE_GenericBean)arr[0];
        tb_nit.Text=rgb.strC1;//nit
        tb_nombre.Text=rgb.strC2;//nombre
        tb_descripcion.Text=rgb.strC3;//descripcion
        lb_estado.SelectedValue=rgb.intC3.ToString();//estado
        rradioprovde.SelectedValue=rgb.intC4.ToString();//bienes
        radioclasificacion.SelectedValue=rgb.intC5.ToString();//clasificacion
        chk_fovial.Checked=rgb.intC6==1;
        tb_direccion.Text=rgb.strC5;
        tb_telefono.Text=rgb.strC7;
        tb_fax.Text=rgb.strC8;
        tb_correo.Text=rgb.strC9;
        tb_contacto.Text=rgb.strC6;
        tb_obs.Text=rgb.strC10;
        tb_dias.Text=rgb.intC9.ToString();
        if (regimen>0) lb_tiporegimen.SelectedValue = regimen.ToString();
        tb_monto.Text=rgb.decC1.ToString();
        chk_ordencompra.Checked=rgb.intC10==1;
        tb_nit2.Text = rgb.strC11;
        //obtengo el listado de retenciones asociados a este proveedor
        ArrayList RetProvList = DB.getRetencionesByProv(int.Parse(tb_id.Text), 4, user.PaisID);
        int retencionID = 0;
        for (int a = 0; a < chklist_retencion.Items.Count; a++) {
            retencionID = int.Parse(chklist_retencion.Items[a].Value);
            foreach (RE_GenericBean rgb1 in RetProvList)
            {
                if (rgb1.intC4 == retencionID)
                {
                    chklist_retencion.Items[a].Selected = true;
                    break;
                }
            }
        }
            

    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
            ArrayList Arr = null;
            string where = "";
            //Arr = Utility.getProveedor("XNitNombre", where);
            //dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
            //ViewState["proveedordt"] = dt;
            //gv_proveedor.DataSource = dt;
            //gv_proveedor.DataBind();
        }
    }
    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        

        ViewState["proveedordt"] = dt;
        gv_proveedor.DataSource = dt;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
}
