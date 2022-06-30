using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class admin : System.Web.UI.MasterPage
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            if (Session["usuario"] != null)
            {
                user = (UsuarioBean)Session["usuario"];
            }
            lb_contabilidad.Items.Clear();
            // Cargo los tipos de contabilidad
            //ArrayList arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
            //ListItem item = null;
            //foreach (RE_GenericBean rgb in arr)
            //{
            //    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            //    lb_contabilidad.Items.Add(item);
            //}

            //************************RESTRICCION DE CONTABILIDAD USUARIOS****************************//
            int fiscal = 0;
            int financiera = 0;
            int bandera = 0;
            ListItem item = null;
            ArrayList arruser = (ArrayList)DB.getContaUsuario(user.ID, user.PaisID);
            ArrayList arrbloqueo = (ArrayList)DB.getContaPais(user.PaisID);
            foreach (RE_GenericBean rgbp in arrbloqueo)
            {
                if (rgbp.intC1 == 1)
                {
                    fiscal = 1; //desbloqueo fiscal por pais
                }

                if (rgbp.intC2 == 1)
                {
                    financiera = 1; // desbloqueo financiera por pais
                }
            }
            foreach (RE_GenericBean rgb in arruser)
            {
                if ((rgb.intC1 == 1) && (fiscal == 1))
                {
                    bandera++;
                    item = new ListItem("FISCAL", "1");
                    lb_contabilidad.Items.Add(item);
                }

                if ((rgb.intC2 == 1) && (financiera == 1))
                {
                    bandera++;
                    item = new ListItem("FINANCIERA", "2");
                    lb_contabilidad.Items.Add(item);
                }
            }
            if (bandera == 1)
            {
                lb_contabilidad.Visible = false;
                l_contabilidad.Visible = false;
            }
            else
            {
                lb_contabilidad.Visible = true;
                l_contabilidad.Visible = true;
            }
            if (bandera == 0)
            {
                Response.Redirect("../logout.aspx");
            }
            //*********************************FIN RESTRICCION********************************************//
            if (Session["Contabilidad"] != null && !Session["Contabilidad"].ToString().Equals(""))
                lb_contabilidad.SelectedValue = Session["Contabilidad"].ToString();
        }
        user = (UsuarioBean)Session["usuario"];
        lbl_tipo_cambio.Text = user.pais.TipoCambio.ToString();
    }
    protected void lb_contabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        #region Asignar el Tipo de Contabilidad a la Session
        UsuarioBean user;
        user = (UsuarioBean)Session["usuario"];
        user.contaID = int.Parse(lb_contabilidad.SelectedValue);
        user.Moneda = DB.getMonedaByPaisConta(user);
        user.Idioma = DB.getIdiomaByPaisConta(user);
        Session["usuario"] = user;
        #endregion
        Session["Contabilidad"] = lb_contabilidad.SelectedValue;
        Response.Redirect("manager.aspx");
    }
}
