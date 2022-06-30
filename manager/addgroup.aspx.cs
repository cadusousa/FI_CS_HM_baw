using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class manager_addgroup : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        else
        {
            user = (UsuarioBean)Session["usuario"];
        }
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        int result = 0;
        if (tb_nombre.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Ingresar el Nombre del Grupo");
            return;
        }
        else
        {
            RE_GenericBean Bean = new RE_GenericBean();
            Bean.strC1 = tb_nombre.Text.Trim();
            Bean.strC2 = tb_descripcion.Text.Trim();
            result = DB.Insertar_Grupo(user, Bean);
            if (result == -100)
            {
                WebMsgBox.Show("Existio un error al tratar de crear el grupo");
                return;
            }
            else
            {
                WebMsgBox.Show("El grupo fue creado exitosamente");
                tb_nombre.Text = "";
                tb_descripcion.Text = "";
                tb_nombre.Focus();
                return;
            }


        }
    }
}