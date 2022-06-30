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

public partial class invoice_re_print : System.Web.UI.Page
{
    UsuarioBean user = null;
    int fac_id = 0, tipo = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        if (Request.QueryString["fac_id"] != null)
        {
            fac_id = int.Parse(Request.QueryString["fac_id"].ToString());
            tipo = int.Parse(Request.QueryString["tipo"].ToString());
            tb_factID.Text = fac_id.ToString();
            lb_tipo.Text = tipo.ToString();
        }
        if (Request.QueryString["s"] != null)//Serie
        {lb_serie.Text = Request.QueryString["s"].ToString();}
        if (Request.QueryString["c"] != null)//Correlativo
        { lb_correlativo.Text = Request.QueryString["c"].ToString(); }
        if (tipo == 6)
        {
            Label2.Visible = false;
            lb_serie.Visible = false;
            Label3.Text = "";
            lb_correlativo.Text = "Banco: " + Request.QueryString["bco"].ToString() + "<br>" + "&nbsp;&nbsp;Cuenta: " + Request.QueryString["ctaID"].ToString() + "<br>" + "&nbsp;&nbsp;Numero: " + Request.QueryString["correlativo"].ToString();
        }
    }
    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
        
            user = (UsuarioBean)Session["usuario"];
            DB.insertReimpresionLog(int.Parse(tb_factID.Text), int.Parse(lb_tipo.Text), tb_motivo.Text, user);
            
                String csname1 = "PopupScript";
                Type cstype = this.GetType();
                // Get a ClientScriptManager reference from the Page class.
                ClientScriptManager cs2 = Page.ClientScript;
                string script = "";
                if (Request.QueryString["tipo"].ToString()!="6")//Factura
                {
                    script = "window.open('print.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
                }
                else if (Request.QueryString["tipo"].Equals("6"))//Cheque
                {
                    script = "window.open('../invoice/print_cheque.aspx?ctaID=" + Request.QueryString["ctaID"].ToString() + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
                }
                if (!cs2.IsStartupScriptRegistered(cstype, csname1))
                {
                    cs2.RegisterStartupScript(cstype, csname1, script, true);
                }
                btn_imprimir.Enabled = false;
            
    }
}
