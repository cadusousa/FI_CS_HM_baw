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

public partial class manager_tipocambio : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            string mensaje="<script language='JavaScript'>";
            mensaje += "window.opener.href='/default.aspx';";
            mensaje += "this.close();";
            mensaje += "</script>";
            
            Page.RegisterClientScriptBlock("closewindow", mensaje);
        }


        if (Request.QueryString["accion"] != null)
        {
            TextBox1.Text = Request.QueryString["accion"];
                
        }


        user = (UsuarioBean)Session["usuario"];

        int pai_id = 0;
        tb_id.Text = "0";
        if (Request.QueryString["id"] != null)
        {
            pai_id = int.Parse(Request.QueryString["id"].ToString());
            PaisBean paisbean = (PaisBean)DB.getPais(pai_id);
            lb_pai_id.Text = paisbean.ID.ToString();
            tb_pais.Text = paisbean.Nombre;
        }
        else {
            lb_msg.Text = "Debe especificar que pais quiere crear un tipo de cambio";
            lb_msg.Visible = true;
            lb_pai_id.Text = "0";
        }

    }
    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        if (lb_pai_id.Text.Equals("0")) {
            lb_msg.Text = "Existió un problema al tratar de grabar el tipo de cambio, por favor intente de nuevo";
            return;
        }
        UsuarioBean user=null;
        RE_GenericBean tcbean = new RE_GenericBean();
        tcbean.intC1 = int.Parse(tb_id.Text.Trim());
        tcbean.intC2 = int.Parse(lb_pai_id.Text.Trim());
        tcbean.decC1 = decimal.Parse(tb_tipo_cambio.Text.Trim());
        user = (UsuarioBean)Session["usuario"];
        tcbean.strC1 = user.ID;
        tcbean.intC3 = int.Parse(TextBox1.Text.Trim());
        decimal result = 0;
        if (lb_pai_id.Text == "5")
        {
            //Insertar Tipo de Cambio en Costa Rica
            result = DB.InsertTipoCambio(tcbean);
            //Insertar Tipo de Cambio en Agente Aduanero Costa Rica
            tcbean.intC1 = int.Parse(tb_id.Text.Trim());
            tcbean.intC2 = 20;
            tcbean.decC1 = decimal.Parse(tb_tipo_cambio.Text.Trim());
            result = DB.InsertTipoCambio(tcbean);
        }
        else
        {
            result = DB.InsertTipoCambio(tcbean);
        }
        if (result < 0)
        {
            lb_msg.Text = "Existió un problema al tratar de grabar el tipo de cambio, por favor intente de nuevo";
            return;
        }
        else if (result == 1)
        {
            user.pais.TipoCambio = tcbean.decC1;
            Session["usuario"] = user;
        }
        else if (result > 0)
        {
            user.pais.TipoCambio = result;
            Session["usuario"] = user;
        }



        string mensaje = "";

        switch (tcbean.intC3) {
            case 0:
                mensaje = "<script languaje=\"JavaScript\">";
                mensaje += "window.opener.location='manager.aspx';";
                mensaje += "window.close();";
                mensaje += "</script>";
            break;

            case 1:
                mensaje = "<script languaje=\"JavaScript\">";
                mensaje += "window.location='manager.aspx';";
                mensaje += "</script>";
            break;

        }

        Page.RegisterClientScriptBlock("closewindow", mensaje);


    }
    protected void cmdCancel_Click(object sender, EventArgs e)
    {

    }
}
