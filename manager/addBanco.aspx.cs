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

public partial class manager_addBancos : System.Web.UI.Page
{
    RE_GenericBean banco=null;
    public ArrayList ctas_banco_arr = null;
    public int id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }
        UsuarioBean user1 = (UsuarioBean)Session["usuario"];
        if (!user1.Aplicaciones.Contains("7"))
        {
            Response.Redirect("manager.aspx");
        }
        int permiso = int.Parse(user1.Aplicaciones["7"].ToString());
        if (!((permiso & 1) == 1) && !((permiso & 2) == 2))
        {
            Response.Redirect("manager.aspx");
        }
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                id = int.Parse(Request.QueryString["id"].ToString());
                banco = (RE_GenericBean)DB.getBanco(id);
                if (banco == null)
                {
                    Session["msg"] = "Error, No se pudieron obtener los datos de este registro, por favor intente de nuevo.";
                    Session["url"] = "searchbanco.aspx";
                    Response.Redirect("message.aspx");
                }
                ArrayList paisXbanco_arr = DB.getPaisXBanco(id);
                ctas_banco_arr = (ArrayList)DB.getCuentasxBanco(id);
                tb_id.Text = banco.intC1.ToString();
                tb_nombre.Text = banco.strC1;
                tb_descripcion.Text = banco.strC2;

                ListItem item = null;
                ArrayList arr = (ArrayList)DB.getPaises("");
                foreach (PaisBean pais in arr) { 
                    item=new ListItem(pais.Nombre, pais.ID.ToString());
                    
                    for (int a = 0; a < paisXbanco_arr.Count; a++) {
                        if (pais.ID == int.Parse(paisXbanco_arr[a].ToString()))
                            item.Selected = true;
                    }
                    chkl_paises.Items.Add(item);
                }

                
            }
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        RE_GenericBean banco = new RE_GenericBean();
        banco.intC1=int.Parse(tb_id.Text);
        banco.strC1 = tb_nombre.Text;
        banco.strC2 = tb_descripcion.Text;

        for (int a = 0; a < chkl_paises.Items.Count; a++)
        {
            if (chkl_paises.Items[a].Selected == true)
            {
                if (banco.arr1==null) banco.arr1=new ArrayList();
                banco.arr1.Add(chkl_paises.Items[a].Value);
            }
        }

        int result = DB.InsertUpdateBanco(banco);
        if ((result == 0) || (result == -100))
        {
            WebMsgBox.Show("Existio un problema al tratar de grabar los datos, por favor intente de nuevo.");
            return;
        }
        Response.Redirect("searchbanco.aspx");
    }
}
