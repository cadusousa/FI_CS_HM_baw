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

public partial class manager_searchprofile : System.Web.UI.Page
{
    public AppBean apl = new AppBean();
    public OpAppBean opapl = new OpAppBean();
    public ArrayList apl_arr = null;
    public ArrayList opapl_arr = null;
    public opciones_perfil rgb = null;
    public PerfilesBean perfil = null;
    public ArrayList apparr = null;
    public ArrayList controles = new ArrayList();
    public int dec = 0;
    int perfilid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }
        
        UsuarioBean user1 = (UsuarioBean)Session["usuario"];
        if (!user1.Aplicaciones.Contains("2"))
        {
            Response.Redirect("manager.aspx");
        }
        int permiso = int.Parse(user1.Aplicaciones["2"].ToString());
        if (!((permiso & 1) == 1) && !((permiso & 2) == 2) && !((permiso & 4) == 4) && !((permiso & 8) == 8))
        {
            Response.Redirect("manager.aspx");
        }
        if (!Page.IsPostBack)
        {
            ArrayList apparr = DB.getApp();
            CheckBoxList chkl = null;
            ListItem item = null;
            foreach (AppBean apbean in apparr)
            {
                apbean.arrOp = (ArrayList)DB.getOpXApp(apbean.appID);
                if (apbean.appNombre.Equals("USUARIOS")) { chkl = chkl_usuario; }
                if (apbean.appNombre.Equals("PERFILES")) { chkl = chkl_perfiles; }
                if (apbean.appNombre.Equals("PAISES")) { chkl = chkl_paises; }
                if (apbean.appNombre.Equals("SUCURSALES")) { chkl = chkl_sucursales; }
                if (apbean.appNombre.Equals("ADMINISTRACION")) { chkl = chkl_administracion; }
                if (apbean.appNombre.Equals("FACTURACION")) { chkl = chkl_facturacion; }
                if (apbean.appNombre.Equals("OPERACIONES")) { chkl = chkl_operaciones; }
                foreach (OpAppBean opbean in apbean.arrOp) {
                    item = new ListItem();
                    item.Text = opbean.opnombre;
                    item.Value = opbean.opID.ToString();
                    chkl.Items.Add(item);
                }
            }
            
            if (Request.QueryString["id"] != null)
            {
                perfilid = int.Parse(Request.QueryString["id"].ToString());
                perfil = (PerfilesBean)DB.getPerfil(perfilid);
                if (perfil == null)
                {
                    Session["msg"] = "Error, No se pudieron obtener los datos de este registro, por favor intente de nuevo.";
                    Session["url"] = "addprofile.aspx?id" + perfilid;
                    Response.Redirect("message.aspx");
                }
                tb_perfilid.Text = perfil.ID.ToString();
                tb_nombre.Text = perfil.Nombre;
                tb_descripcion.Text = perfil.Descripcion;
                perfil.Apparr = (ArrayList)DB.getAppByPerfil(perfil.ID);
                
                foreach (opciones_perfil opperbean in perfil.Apparr) {
                    if (opperbean.pap_aplID == 1) { chkl = chkl_usuario; }
                    if (opperbean.pap_aplID == 2) { chkl = chkl_perfiles; }
                    if (opperbean.pap_aplID == 3) { chkl = chkl_paises; }
                    if (opperbean.pap_aplID == 4) { chkl = chkl_sucursales; }
                    if (opperbean.pap_aplID == 5) { chkl = chkl_facturacion; }
                    if (opperbean.pap_aplID == 6) { chkl = chkl_operaciones; }
                    if (opperbean.pap_aplID == 7) { chkl = chkl_administracion; }
                    foreach (OpAppBean opbean in DB.getOpXApp(opperbean.pap_aplID)) {
                        if ((opperbean.pap_Dec & opbean.opdecimal) == opbean.opdecimal) {
                            chkl.Items.FindByText(opbean.opnombre).Selected = true;
                        }
                    }
                }
                
                
        
            } else {
                tb_perfilid.Text = "0";
            }
            
        }
    }

    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        PerfilesBean perfilbean = new PerfilesBean();
        perfilbean.ID = int.Parse(tb_perfilid.Text.Trim());
        perfilbean.Nombre=tb_nombre.Text.ToUpper().Trim();
        perfilbean.Descripcion=tb_descripcion.Text.Trim();
        OpAppBean opbean = null;
        ArrayList opcionarr = null;
        ArrayList aplicacionarr = new ArrayList();
        CheckBoxList chkl = new CheckBoxList();

        // Obtengo todas las aplicaciones
        ArrayList aplicaarr = DB.getApp();
        
        // ya tengo todas las aplicaciones 
        // recorro el arreglo de aplicaciones para ver uno por uno
        foreach (AppBean apbean in aplicaarr) {
            opcionarr=new ArrayList();
            if (apbean.appNombre.Equals("USUARIOS")) { chkl = chkl_usuario; }
            if (apbean.appNombre.Equals("PERFILES")) { chkl = chkl_perfiles; }
            if (apbean.appNombre.Equals("PAISES")) { chkl = chkl_paises; }
            if (apbean.appNombre.Equals("SUCURSALES")) { chkl = chkl_sucursales; }
            if (apbean.appNombre.Equals("ADMINISTRACION")) { chkl = chkl_administracion; }
            if (apbean.appNombre.Equals("FACTURACION")) { chkl = chkl_facturacion; }
            if (apbean.appNombre.Equals("OPERACIONES")) { chkl = chkl_operaciones; }
            for (int i = 0; i < chkl.Items.Count; i++ )
            {
                ListItem item = (ListItem)chkl.Items[i];
                if (item.Selected)
                {
                    opbean = new OpAppBean();
                    opbean = DB.getOpcion(int.Parse(item.Value));
                    opcionarr.Add(opbean);
                }
            }
            apbean.arrOp = opcionarr;
            if ((apbean.arrOp!=null) && (apbean.arrOp.Count>0)){
                aplicacionarr.Add(apbean);
            }
        }
        perfilbean.Apparr = aplicacionarr;
        int result = DB.InsertUpdatePerfil(perfilbean);
        if (result == 0)
        {
            Session["msg"] = "Error, No se pudieron grabar los datos que ingreso, por favor intente de nuevo.";
            Session["url"] = "addprofile.aspx?id" + perfilbean.ID;
            Response.Redirect("message.aspx");
        }
        else if (result == -100)
        {
            Session["msg"] = "Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.";
            Session["url"] = "addprofile.aspx?id" + perfilbean.ID;
            Response.Redirect("message.aspx");
        }
        Response.Redirect("searchprofile.aspx");
    }
    
}
