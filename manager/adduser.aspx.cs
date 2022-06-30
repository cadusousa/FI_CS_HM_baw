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

public partial class users_adduser : System.Web.UI.Page
{
    string id = "";
    public UsuarioBean usuariobea = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        ListItem item = null;
        ArrayList arr = new ArrayList();
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }

        UsuarioBean user1 = (UsuarioBean)Session["usuario"];
        if (!user1.Aplicaciones.Contains("1"))
        {
            Response.Redirect("manager.aspx");
        }
        int permiso = int.Parse(user1.Aplicaciones["1"].ToString());
        if (!((permiso & 1) == 1) && !((permiso & 2) == 2) && !((permiso & 4) == 4) && !((permiso & 8) == 8))
        {
            Response.Redirect("manager.aspx");
        }

        if (!Page.IsPostBack)
        {
            ArrayList arrtemp = new ArrayList();
            //Obtengo listado de usuarios
            lb_usuarios.Items.Clear();
            arr = (ArrayList)DB.getUsuarios("");            
            item = null;
            foreach(UsuarioBean user in arr){
                //user = (UsuarioBean)DB.getUsuariosData(user);
                item = new ListItem(user.ID, user.ID);
                lb_usuarios.Items.Add(item);
                arrtemp.Add(user);
            }
            //Obtengo listado de paises
            lb_pais.Items.Clear();
            arr = (ArrayList)DB.getPaises("");
            item = null;
            foreach (PaisBean pais in arr)
            {
                //user = (UsuarioBean)DB.getUsuariosData(user);
                item = new ListItem(pais.Nombre, pais.ID.ToString());
                lb_pais.Items.Add(item);
            }
            lb_pais_SelectedIndexChanged(sender, e);
            arr = arrtemp;
            /* obtengo el listado de perfiles */
            ArrayList perfil_arr = (ArrayList)DB.getPerfiles("");
            item = null;
            PerfilesBean perfilbean = new PerfilesBean();
            for (int j = 0; j < perfil_arr.Count; j++)
            {
                perfilbean = (PerfilesBean)perfil_arr[j];
                item = new ListItem(perfilbean.Nombre, perfilbean.ID.ToString());
                chk_list.Items.Add(item);
                chk_list.Items[j].Selected = true;
            }
            /* fin obtengo el listado de perfiles */

            /* obtengo el listado de operaciones */
            ArrayList operacion_arr = (ArrayList)DB.getOperaciones();
            item = null;
            int t = 0;
            foreach (RE_GenericBean operacion in operacion_arr )
            {
                item = new ListItem(operacion.strC1, operacion.intC1.ToString());
                chk_operaciones.Items.Add(item);
                chk_operaciones.Items[t].Selected = true;
                t++;
            }
            /* fin obtengo el listado de operaciones */ 
        }
    }

    protected void bt_enviar_Click(object sender, EventArgs e)
    {
        string id = lb_usuarios.SelectedValue;
        int paiID = int.Parse(lb_pais.SelectedValue);
        ArrayList arr_sucursales = new ArrayList();
        for (int a = 0; a < chk_sucursales.Items.Count; a++)
        {
            if (chk_sucursales.Items[a].Selected == true)
            {
                arr_sucursales.Add(chk_sucursales.Items[a].Value);
            }
        }

        ArrayList arr_depto = new ArrayList();
        for (int a = 0; a < chk_deptos.Items.Count; a++)
        {
            if (chk_deptos.Items[a].Selected == true)
            {
                arr_depto.Add(chk_deptos.Items[a].Value);
            }
        }

        ArrayList arr_perfil = new ArrayList();
        for (int a = 0; a < chk_list.Items.Count; a++)
        {
            if (chk_list.Items[a].Selected == true)
            {
                arr_perfil.Add(chk_list.Items[a].Value);
            }
        }

        ArrayList arr_operacion = new ArrayList();
        for (int a = 0; a < chk_operaciones.Items.Count; a++)
        {
            if (chk_operaciones.Items[a].Selected == true)
            {
                arr_operacion.Add(chk_operaciones.Items[a].Value);
            }
        }

        UsuarioBean user1 = (UsuarioBean)Session["usuario"];


        DB.insertSucursales(arr_sucursales, id, paiID, user1);
        DB.insertDeptos(arr_depto, id, paiID, user1);
        DB.insertPerfil(arr_perfil, id, paiID, user1);
        DB.insertOperaciones(arr_operacion, id, paiID, user1);
    }

    protected void lb_pais_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        int paiID = int.Parse(lb_pais.SelectedValue);
        string id = lb_usuarios.SelectedValue;
        /* obtengo el listado de sucursales y paises */
        arr = (ArrayList)DB.getSucursales_pais(" and suc_pai_id="+paiID);
        item = null;
        chk_sucursales.Items.Clear();
        if (arr!=null && arr.Count>0 ) {
            foreach (SucursalBean suc in arr)
            {
                item = new ListItem(suc.Nombre, suc.ID.ToString());
                item.Selected = true;
                chk_sucursales.Items.Add(item);

            }
        }
        /* fin obtengo el listado de paises */
        /* obtengo el listado departamentos paises */
        arr = (ArrayList)DB.getDepartamentopor_pais(" and tda_pai_id="+paiID);
        item = null;
        chk_deptos.Items.Clear();
        if (arr != null && arr.Count > 0)
        {
            foreach (SucursalBean suc in arr)
            {
                item = new ListItem(suc.Nombre, suc.ID.ToString());
                item.Selected = true;
                chk_deptos.Items.Add(item);
            }
        }
        /* fin obtengo el listado de paises */

        usuariobea = new UsuarioBean();
        usuariobea.Perfil = (ArrayList)DB.getPerfilesxUsu(id, paiID);//envio el id del usuario para obtener sus perfiles asociados
        int perfID = 0;
        
        for (int a = 0; a < chk_list.Items.Count; a++)
        {
            chk_list.Items[a].Selected = false;
            perfID = int.Parse(chk_list.Items[a].Value);
            foreach (PerfilesBean perfil in usuariobea.Perfil)
            {
                if (perfil.ID == perfID)
                {
                    chk_list.Items[a].Selected = true;
                }
                else
                {
                    chk_list.Items[a].Selected = false;
                }
            }
        }

        ArrayList sucbyuser = (ArrayList)DB.getSucursalesbyUsu(id, paiID);
        for (int a = 0; a < chk_sucursales.Items.Count; a++)
        {
            chk_sucursales.Items[a].Selected = false;
            perfID = int.Parse(chk_sucursales.Items[a].Value);
            foreach (RE_GenericBean rgb in sucbyuser)
            {
                if (rgb.intC1 == perfID)
                {
                    chk_sucursales.Items[a].Selected = true;
                }
            }
        }

        ArrayList deptobyuser = (ArrayList)DB.getDepartamentosbyUsu(id, paiID);
        for (int a = 0; a < chk_deptos.Items.Count; a++)
        {
            chk_deptos.Items[a].Selected = false;
            perfID = int.Parse(chk_deptos.Items[a].Value);
            foreach (RE_GenericBean rgb in deptobyuser)
            {
                if (rgb.intC1 == perfID)
                {
                    chk_deptos.Items[a].Selected = true;
                }
            }
        }
    }
    protected void lb_usuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        int paiID = int.Parse(lb_pais.SelectedValue);
        string id = lb_usuarios.SelectedValue;
        /* obtengo el listado de sucursales y paises */
        arr = (ArrayList)DB.getSucursales_pais(" and suc_pai_id=" + paiID);
        item = null;
        chk_sucursales.Items.Clear();
        if (arr != null && arr.Count > 0)
        {
            foreach (SucursalBean suc in arr)
            {
                item = new ListItem(suc.Nombre, suc.ID.ToString());
                item.Selected = true;
                chk_sucursales.Items.Add(item);

            }
        }
        /* fin obtengo el listado de paises */
        /* obtengo el listado departamentos paises */
        arr = (ArrayList)DB.getDepartamentopor_pais(" and tda_pai_id=" + paiID);
        item = null;
        chk_deptos.Items.Clear();
        if (arr != null && arr.Count > 0)
        {
            foreach (SucursalBean suc in arr)
            {
                item = new ListItem(suc.Nombre, suc.ID.ToString());
                item.Selected = true;
                chk_deptos.Items.Add(item);
            }
        }
        /* fin obtengo el listado de paises */

        usuariobea = new UsuarioBean();
        usuariobea.Perfil = (ArrayList)DB.getPerfilesxUsu(id, paiID);//envio el id del usuario para obtener sus perfiles asociados
        int perfID = 0;

        for (int a = 0; a < chk_list.Items.Count; a++)
        {
            chk_list.Items[a].Selected = false;
            perfID = int.Parse(chk_list.Items[a].Value);
            foreach (PerfilesBean perfil in usuariobea.Perfil)
            {
                if (perfil.ID == perfID)
                {
                    chk_list.Items[a].Selected = true;
                }
            }
        }

        ArrayList sucbyuser = (ArrayList)DB.getSucursalesbyUsu(id, paiID);
        for (int a = 0; a < chk_sucursales.Items.Count; a++)
        {
            chk_sucursales.Items[a].Selected = false;
            perfID = int.Parse(chk_sucursales.Items[a].Value);
            foreach (RE_GenericBean rgb in sucbyuser)
            {
                if (rgb.intC1 == perfID)
                {
                    chk_sucursales.Items[a].Selected = true;
                }
            }
        }

        ArrayList deptobyuser = (ArrayList)DB.getDepartamentosbyUsu(id, paiID);
        for (int a = 0; a < chk_deptos.Items.Count; a++)
        {
            chk_deptos.Items[a].Selected = false;
            perfID = int.Parse(chk_deptos.Items[a].Value);
            foreach (RE_GenericBean rgb in deptobyuser)
            {
                if (rgb.intC1 == perfID)
                {
                    chk_deptos.Items[a].Selected = true;
                }
            }
        }

        ArrayList operacinbyuser = (ArrayList)DB.getOperacionbyUsu(id, paiID);
        for (int a = 0; a < chk_operaciones.Items.Count; a++)
        {
            chk_operaciones.Items[a].Selected = false;
            perfID = int.Parse(chk_operaciones.Items[a].Value);
            foreach (RE_GenericBean rgb in operacinbyuser)
            {
                if (rgb.intC1 == perfID)
                {
                    chk_operaciones.Items[a].Selected = true;
                }
            }
        }
    }
}
