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
using System.Text;

public partial class operations_listado_oc : System.Web.UI.Page
{
    DataTable dt = null;
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!Page.IsPostBack) {
            obtengo_listas(user);
        }
    }

    private void obtengo_listas(UsuarioBean user)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = null;
            arr = user.Departamento;
            item = new ListItem("-Seleccione-", "0");
            lb_departamento_interno.Items.Add(item);
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_departamento_interno.Items.Add(item);
            }
            arr = null;
            //arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 7, user, 0);//1 porque es el tipo de documento para facturacion
            //foreach (string valor in arr)
            //{
            //    item = new ListItem(valor, valor);
            //    lb_seriefactura.Items.Add(item);
            //}
            if (Session["Contabilidad"] != null && !Session["Contabilidad"].ToString().Equals(""))
                lb_contabilidad.Text = Session["Contabilidad"].ToString();
        }
    }

    protected void bt_busqueda_Click(object sender, EventArgs e)
    {
        string fechaini = tb_fechainicial.Text.Trim();
        string fechafin = tb_fechafinal.Text.Trim();
        string serie = tb_serie_oc.Text.Trim();
        int corr = 0;
        int deptoID = int.Parse(lb_departamento_interno.SelectedValue);
        int estado = int.Parse(lb_estado.SelectedValue);
        string where = " toc_conta_id=" + lb_contabilidad.Text;
        if (serie != "")
        {
            where  += " and toc_serie='" + serie + "' ";
        }
        if (!tb_corr_oc.Text.Equals(""))
        {
            corr = int.Parse(tb_corr_oc.Text.ToString());
        }

        if (corr != 0)
        {
            where += " and toc_correlativo=" + corr;
        }
        if (tb_contenedor != null && !tb_contenedor.Text.Equals(""))
        {
            where += " and upper(toc_contenedor)='" + tb_contenedor.Text.ToUpper() + "'";
        }


        if (!fechaini.Equals("") && !fechafin.Equals(""))
        {
            //Ticket#2020112531000097 — ERROR EN PLATAFORMA 
            //where += " and toc_fecha between '" + fechaini + "' and '" + fechafin + "'";

            where += " and toc_fecha between '" + fechaini.Substring(6, 4) + "-" + fechaini.Substring(0, 2) + "-" + fechaini.Substring(3, 2) + "'";

            where += " and '" + fechafin.Substring(6, 4) + "-" + fechafin.Substring(0, 2) + "-" + fechafin.Substring(3, 2) + "'";

        }

        if ((serie == "") && (deptoID == 0))
        {
            WebMsgBox.Show("Debe de ingresar al menos un criterio de busqueda como: Serie ó Departamento");
            return;
        }


        dt = (DataTable)DB.getOCList(deptoID, user, estado, where);
        
        //Formatea los campos de tipo fecha
        dt.Columns["Fecha creacion"].Convert(i => DateTime.Parse(i.ToString()).ToString("dd/MM/yyyy"));
        dt.Columns["Fecha autorizacion"].Convert(i => DateTime.Parse(i.ToString()).ToString("ddMM/yyy"));

        gv_ordenescompra.DataSource = dt;
        gv_ordenescompra.DataBind();
    }

    protected void gv_ordenescompra_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandArgument != null)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "Seleccionar":
                    {
                        //Response.Redirect(string.Format("genera_oc.aspx?id={0}",gv_ordenescompra.Rows[index].Cells[1].Text));
                        var urlPage = string.Format("genera_oc.aspx?id={0}", gv_ordenescompra.Rows[index].Cells[1].Text);
                        var jsKey = "PopupScript";
                        var cs = Page.ClientScript;
                        string script = string.Format("window.open('{0}', '_blank', 'toolbar=0, status=0, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -550) + ', height=' + (window.screen.height -75))", urlPage);
                        if (!cs.IsStartupScriptRegistered(this.GetType(), jsKey))
                            cs.RegisterClientScriptBlock(this.GetType(), jsKey, script, true);
                        break;
                    }
                default:
                    break;
            }
        }
    }
    protected void gv_ordenescompra_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;

    }
}
