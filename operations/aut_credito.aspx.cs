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

public partial class operations_aut_credito : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        UsuarioBean user = null;
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        //if ((user.PaisID == 1) || (user.PaisID == 15) || (user.PaisID == 18) || (user.PaisID == 2) || (user.PaisID == 26))
        //{
            int permiso_autorizar = 0;
            ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
            foreach (PerfilesBean Perfil in Arr_Perfiles)
            {
                if ((Perfil.ID == 8) || (Perfil.ID == 29))
                {
                    permiso_autorizar++;
                }
            }
            if (permiso_autorizar == 0)
            {
                Response.Redirect("index.aspx");
            }
        //}
        //else
        //{
        //    if (!user.Aplicaciones.Contains("6"))
        //        Response.Redirect("index.aspx");
        //    int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        //    if (!((permiso & 32768) == 32768))
        //        Response.Redirect("index.aspx");
        //}
        if (!Page.IsPostBack) {
            int id = 0;
            if (Request.QueryString["id"] != null)
                id = int.Parse(Request.QueryString["id"].ToString());
            RE_GenericBean datos = (RE_GenericBean)DB.getautorizaciondata(id);
            tb_idsolicitud.Text = datos.douC2.ToString();
            tb_idcliente.Text = datos.douC1.ToString();
            tb_nombrecliente.Text = datos.strC1;
            tb_diassolicitados.Text = datos.intC2.ToString();
            tb_montosolicitado.Text = datos.decC1.ToString();
            tb_diasautorizados.Text = datos.intC2.ToString();
            tb_montoautorizado.Text = datos.decC1.ToString();
            tb_solicitado_por.Text = datos.strC2;
            tb_cobrador.Text = datos.strC3;
        }
    }

    protected void bt_aceptar_Click(object sender, EventArgs e)
    {
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        RE_GenericBean datos = new RE_GenericBean();
        datos.intC1=int.Parse(tb_idsolicitud.Text);
        datos.intC2 = int.Parse(tb_diasautorizados.Text);
        datos.decC1 = decimal.Parse(tb_montoautorizado.Text);
        datos.strC2 = user.ID;
        datos.strC3 = tb_cobrador.Text;
        if (datos.intC2 <= 0)
        {
            WebMsgBox.Show("La cantidad de dias de credito autorizados debe ser mayor a cero");
            return;
        }
        if (datos.decC1 <= 0)
        {
            WebMsgBox.Show("El monto autorizado debe ser mayor a cero");
            return;
        }
        int result=DB.autorizarcredito(datos);
        if (result == 1)
        {
            WebMsgBox.Show("La informacion se grabo con exito");
            tb_idsolicitud.Text = "";
            tb_idcliente.Text = "";
            tb_nombrecliente.Text = "";
            tb_diassolicitados.Text = "";
            tb_montosolicitado.Text = "";
            tb_diasautorizados.Text = "";
            tb_montoautorizado.Text = "";
            tb_cobrador.Text = "";
            tb_solicitado_por.Text = "";
            return;
        } else {
            WebMsgBox.Show("Existio un problema al momento de grabar la informacion, por favor intente de nuevo");
            return;
        }
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
        tb_cobrador.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
    }
    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "and id_pais=" + user.PaisID;

        if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and nombre_cobrador ilike '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";

        Arr = (ArrayList)DB.getcobradores(where); //Cobradores
        dt = (DataTable)Utility.fillGridView("Cobrador", Arr);

        ViewState["proveedordt"] = dt;
        gv_proveedor.DataSource = dt;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
}
