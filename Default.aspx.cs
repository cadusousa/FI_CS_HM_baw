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

public partial class _Default : System.Web.UI.Page
{
    public string strContrasenaExpirada = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["usuario"] = null;
        Session["Contabilidad"] = null;
        DateTime Fecha_Hora = DateTime.Now;
        string Hora = Fecha_Hora.Hour.ToString();

	if (Request.QueryString["login_terrestre"] != null)
        {
            //localhost:61766/BAW/Default.aspx?login_terrestre=!&login_user=soporte7&bl_no=CGT-2019-22-0025-83085&blid=83085
            try
            {
                string pw_name = Request.QueryString["login_user"];
                string bl_no = Request.QueryString["bl_no"];
                string blid = Request.QueryString["blid"];
                string paisid = Request.QueryString["paisid"];

                RE_GenericBean result = DB.getUsuarioEmpresa(pw_name);
                if (result.intC1 > 0)
                {
                    UsuarioBean user = new UsuarioBean();
                    user.ID = pw_name;
                    user.Contrasena = "";
                    user.Dias_Contrasena = 90;
                    int pais_id = DB.getEmpresa(paisid);
                    //country choice
                    ArrayList arr = (ArrayList)DB.getPaisesbyUser(pw_name);
                    ListItem item = null;
                    string country = "";
                    string sucursal = "";
                    foreach (PaisBean pais in arr)
                    {
                        item = new ListItem(pais.Nombre, pais.ID.ToString());
                        if (int.Parse(pais.ID.ToString()) == pais_id)
                        {
                            country = pais.ID.ToString();
                            break;
                        }
                    }

                    if (country != "")
                    {
                        arr = null;
                        arr = (ArrayList)DB.getSucursalesbyuser(pw_name, int.Parse(country));
                        foreach (SucursalBean suc in arr)
                        {
                            item = new ListItem(suc.Nombre, suc.ID.ToString());
                            sucursal = suc.ID.ToString();
                            break;
                        }

                        user.PaisID = int.Parse(country);
                        user.SucursalID = int.Parse(sucursal);
                        user.contaID = 1;

                        user.pais = (PaisBean)DB.getPais(user.PaisID);
                        user.Moneda = DB.getMonedaByPaisConta(user);
                        user.Idioma = DB.getIdiomaByPaisConta(user);
                        decimal SetTipoCambio = DB.getTipoCambioHoy(user.PaisID);

                        user.Aplicaciones = (Hashtable)DB.getPerfilesbyUser(user.ID);
                        user.Departamento = (ArrayList)DB.getUsuarioDepartamentoAimar(user.ID, user.PaisID);

                        SucursalBean Suc_Bean = DB.getSucursal(user.SucursalID);
                        user.Sucursal_Es_APL = Suc_Bean.Es_APL;
                        user.Estado = 1;
                        user.Fecha = DateTime.Now.ToShortDateString();
                        user.Operacion = 1;
                        Session["configurado"] = true;
                        Session["Contabilidad"] = "1";
                        Session["usuario"] = user;
                        Response.Redirect("invoice/invoice.aspx?bl_no=" + bl_no + "&tipo=TERRESTRE%20T&opid=5&blid=" + blid);
                    }
                    else
                    {
                        WebMsgBox.Show("No se puede configurar su sesion debido a que faltan criterios. No tiene asignado el pais " + paisid);
                    }
                }
                else
                {
                    WebMsgBox.Show("No se puede configurar su sesion debido a que faltan criterios. Usuario no encontrado " + pw_name);
                }
            }
            catch (Exception ex)
            {
                WebMsgBox.Show(ex.Message);
            }            
        } 

    }

    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        int dias_contrasena = 0;
        string username = frmLogin.Text.Trim();
        string contrasena = frmContrasena.Text.Trim(); // Utility.cifrado(frmContrasena.Text.Trim());
        UsuarioBean usuario = (UsuarioBean)DB.ValidaCliente(username, contrasena);

        if (usuario != null)
        {
            if (usuario.Dias_Contrasena > 0)
            {
                strContrasenaExpirada = "<span style=\"color:Red; background-color:Yellow; border-color:Yellow\">La contraseña ha vencido, favor actualizarla</span><a style=\"color:Blue background-color:Yellow; border-color:Yellow\" onclick=\"window.open('http://10.10.1.20/catalogo_admin/cambio/index.php', 'Cambio Clave', 'height=434, width=300, menubar=0, resizable=0, scrollbars=0, toolbar=0')\"> AQUI</a>";
                //lblcontrasenavencida.Visible = true;
            }
            else
            {
                Session["usuario"] = usuario;
                String csname1 = "PopupScript";
                Type cstype = this.GetType();
                ClientScriptManager cs2 = Page.ClientScript;
                string script = "window.open('manager/countrychoice.aspx', 'BAW', 'toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
                if (!cs2.IsStartupScriptRegistered(cstype, csname1))
                {
                    cs2.RegisterStartupScript(cstype, csname1, script, true);
                }
            }
        }
        else
        {
            lbnotautenticate.Visible = true;
        }
        
    }
}
