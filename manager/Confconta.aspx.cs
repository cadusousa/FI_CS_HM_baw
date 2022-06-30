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

public partial class manager_Confconta : System.Web.UI.Page
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
            foreach (UsuarioBean user in arr)
            {
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
            /* obtengo el listado de contas */
            ArrayList tcon_arr = (ArrayList)DB.GetTipoContas();
            item = null;
            int t = 0;
            foreach (RE_GenericBean contas in tcon_arr)
            {
                item = new ListItem(contas.strC1, contas.intC1.ToString());
                chk_contapersonal.Items.Add(item);
                chk_contapersonal.Items[t].Selected = false;
                t++;
            }
            /* obtengo el listado de contas */
            ArrayList tconpais_arr = (ArrayList)DB.GetTipoContas();
            item = null;
            int t1 = 0;
            foreach (RE_GenericBean contas in tconpais_arr)
            {
                item = new ListItem(contas.strC1, contas.intC1.ToString());
                chk_contapais.Items.Add(item);
                chk_contapais.Items[t1].Selected = false;
                t1++;
            }
            lb_pais_SelectedIndexChanged(sender, e);
            lb_usuarios_SelectedIndexChanged(sender, e);
        }



    }
    protected void lb_pais_SelectedIndexChanged(object sender, EventArgs e)
    {
        int paiID = int.Parse(lb_pais.SelectedValue);
        string id = lb_usuarios.SelectedValue;
        int t = 0;
        int perfID = 0;
        ArrayList sucbyuser = (ArrayList)DB.getContaPais(paiID);
        for (int a = 0; a < chk_contapais.Items.Count; a++)
        {
            chk_contapais.Items[a].Selected = false;
        }
        for (int a = 0; a < chk_contapais.Items.Count; a++)
        {
            chk_contapais.Items[a].Selected = false;
            perfID = int.Parse(chk_contapais.Items[a].Value);
            foreach (RE_GenericBean rgb in sucbyuser)
            {
                t++;
                if (t == 1)
                {
                    if (rgb.intC1 == 1)
                    {
                        chk_contapais.Items[a].Selected = true;
                    }
                }
                if (t == 2)
                {
                    if (rgb.intC2 == 1)
                    {
                        chk_contapais.Items[a].Selected = true;
                    }

                }
            }
        }
        t = 0;
        ArrayList sucbyuser1 = (ArrayList)DB.getContaUsuario(id, paiID);
        for (int a = 0; a < chk_contapersonal.Items.Count; a++)
        {
            chk_contapersonal.Items[a].Selected = false;
        }
        for (int a = 0; a < chk_contapersonal.Items.Count; a++)
        {
            chk_contapersonal.Items[a].Selected = false;
            perfID = int.Parse(chk_contapersonal.Items[a].Value);
            foreach (RE_GenericBean rgb in sucbyuser1)
            {
                t++;
                if (t == 1)
                {
                    if (rgb.intC1 == 1)
                    {
                        chk_contapersonal.Items[a].Selected = true;
                    }
                }
                if (t == 2)
                {
                    if (rgb.intC2 == 1)
                    {
                        chk_contapersonal.Items[a].Selected = true;
                    }

                }
            }
        }
    }
    protected void lb_usuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        int paiID = int.Parse(lb_pais.SelectedValue);
        string id = lb_usuarios.SelectedValue;
        int t = 0;
        int perfID = 0;
        ArrayList sucbyuser = (ArrayList)DB.getContaUsuario(id, paiID);
        for (int a = 0; a < chk_contapersonal.Items.Count; a++)
        {
            chk_contapersonal.Items[a].Selected = false;
        }
        for (int a = 0; a < chk_contapersonal.Items.Count; a++)
        {
            chk_contapersonal.Items[a].Selected = false;
            perfID = int.Parse(chk_contapersonal.Items[a].Value);
            foreach (RE_GenericBean rgb in sucbyuser)
            {
                t++;
                if (t == 1)
                {
                    if (rgb.intC1 == 1)
                    {
                        chk_contapersonal.Items[a].Selected = true;
                    }
                }
                if (t == 2)
                {
                    if (rgb.intC2 == 1)
                    {
                        chk_contapersonal.Items[a].Selected = true;
                    }
 
                }
            }
        }

        t = 0;
        ArrayList sucbyuser1 = (ArrayList)DB.getContaPais(paiID);
        for (int a = 0; a < chk_contapais.Items.Count; a++)
        {
            chk_contapais.Items[a].Selected = false;
        }
        for (int a = 0; a < chk_contapais.Items.Count; a++)
        {
            chk_contapais.Items[a].Selected = false;
            perfID = int.Parse(chk_contapais.Items[a].Value);
            foreach (RE_GenericBean rgb in sucbyuser1)
            {
                t++;
                if (t == 1)
                {
                    if (rgb.intC1 == 1)
                    {
                        chk_contapais.Items[a].Selected = true;
                    }
                }
                if (t == 2)
                {
                    if (rgb.intC2 == 1)
                    {
                        chk_contapais.Items[a].Selected = true;
                    }

                }
            }
        }
    }
    protected void bt_enviar_Click(object sender, EventArgs e)
    {    
        string id = lb_usuarios.SelectedValue;
        int paiID = int.Parse(lb_pais.SelectedValue);
        /********************CONTABILIDAD PARA PAISES************/
        ArrayList arr_paises = new ArrayList();
        for (int a = 0; a < chk_contapais.Items.Count; a++)
        {
            if (chk_contapais.Items[a].Selected == true)
            {
                arr_paises.Add(chk_contapais.Items[a].Value);
            }
        }
        /********************CONTABILIDAD PARA USUARIOS************/
        ArrayList arr_usuarios = new ArrayList();
        for (int a = 0; a < chk_contapersonal.Items.Count; a++)
        {
            if (chk_contapersonal.Items[a].Selected == true)
            {
                arr_usuarios.Add(chk_contapersonal.Items[a].Value);
            }
        }
        DB.insertContaUsuario(arr_usuarios, id, paiID);
        DB.insertContaPais(arr_paises,  paiID);
    }
}