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

public partial class manager_permisos_serie : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ListItem item = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Contabilidad"] = null;
        if (Session["usuario"] == null)
            Response.Redirect("../default.aspx");
        user = (UsuarioBean)Session["usuario"];

        if (!Page.IsPostBack)
        {
            drp_usuario.Items.Clear();
            drp_pais.Items.Clear();
            drp_sucursal.Items.Clear();
            drp_contabilidad.Items.Clear();
            drp_tipo_documento.Items.Clear();
            drp_usuario.Items.Add("Seleccione...");
            drp_pais.Items.Add("Seleccione...");
            drp_sucursal.Items.Add("Seleccione...");
            drp_contabilidad.Items.Add("Seleccione...");
            drp_tipo_documento.Items.Add("Seleccione...");
            arr = null;
            item = null;
            arr = (ArrayList)DB.getUsuarios("");            
            foreach(UsuarioBean usuario in arr)
            {
                item = new ListItem(usuario.ID, usuario.ID);
                drp_usuario.Items.Add(item);
            }
        }

    }
    protected void drp_usuario_SelectedIndexChanged(object sender, EventArgs e)
    {
        arr = null;
        item = null;
        drp_pais.Items.Clear();
        drp_pais.Items.Add("Seleccione...");
        arr = (ArrayList)DB.getPaisesbyUser(drp_usuario.SelectedValue.ToString());
        foreach (PaisBean pais in arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_pais.Items.Add(item);
        }
        drp_pais.SelectedIndex = 0;
        drp_pais.Enabled = true;
    }
    protected void drp_pais_SelectedIndexChanged(object sender, EventArgs e)
    {
        arr = null;
        item = null;
        drp_sucursal.Items.Clear();
        drp_sucursal.Items.Add("Seleccione...");
        arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + drp_pais.SelectedValue.ToString());
        foreach (SucursalBean suc in arr)
        {
            item = new ListItem(suc.Nombre, suc.ID.ToString());
            drp_sucursal.Items.Add(item);
        }
        drp_sucursal.Enabled = true;
        drp_sucursal.SelectedIndex = 0;
    }
    protected void drp_sucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        arr = null;
        item = null;
        drp_contabilidad.Items.Clear();
        drp_contabilidad.Items.Add("Seleccione...");
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_contabilidad.Items.Add(item);
        }
        drp_contabilidad.Enabled = true;
        drp_contabilidad.SelectedIndex = 0;
    }
    protected void drp_contabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_tipo_operacion.Enabled = true;
    }
    protected void drp_tipo_operacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_tipo_documento.Enabled = true;
        drp_tipo_documento.Items.Clear();
        arr = null;
        arr = (ArrayList)DB.GetDocumentosBySYS_TipoTeferencia();
        item = new ListItem("Seleccione...");
        drp_tipo_documento.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_tipo_documento.Items.Add(item);
        }
    }
    protected void btn_visualizar_Click(object sender, EventArgs e)
    {
        if ((drp_usuario.SelectedValue.ToString() != "Seleccione...") && (drp_pais.SelectedValue.ToString() != "Seleccione...") && (drp_sucursal.SelectedValue.ToString() != "Seleccione...") && (drp_contabilidad.SelectedValue.ToString() != "Seleccione...") && (drp_tipo_operacion.SelectedValue.ToString() != "Seleccione...") && (drp_tipo_documento.SelectedValue.ToString() != "Seleccione..."))
        {
            UsuarioBean usuario = new UsuarioBean();
            usuario.PaisID = int.Parse(drp_pais.SelectedValue.ToString());
            usuario.contaID = int.Parse(drp_contabilidad.SelectedValue.ToString());
            usuario.Operacion = int.Parse(drp_tipo_operacion.SelectedValue.ToString());
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(drp_sucursal.SelectedValue.ToString()), int.Parse(drp_tipo_documento.SelectedValue.ToString()), usuario, 0);
            DataTable dt = new DataTable();
            dt.Columns.Add("Serie");
            GridView1.DataBind();
            foreach (string serie in arr)
            {
                object[] obj = { serie };
                dt.Rows.Add(obj);
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        else
        {
            GridView1.DataBind();
        }
    }
    protected void drp_tipo_documento_SelectedIndexChanged(object sender, EventArgs e)
    {
        btn_visualizar.Enabled = true;
    }
}