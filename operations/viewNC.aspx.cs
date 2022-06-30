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

public partial class invoice_viewrcpt : System.Web.UI.Page
{
    int factID = 0;
    int ncID = 0;
    int ttr = 0;
    UsuarioBean user = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        if (!user.Aplicaciones.Contains("5"))
        {
            Response.Redirect("index.aspx");
        }
        int permiso = int.Parse(user.Aplicaciones["5"].ToString());
        if (!((permiso & 128) == 128))
        {
            Response.Redirect("index.aspx");
        }

        int impo_expo = 0;
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        ttr = int.Parse(Request.QueryString["tipo"].ToString().Trim());
        obtengo_listas(tipo_contabilidad, impo_expo, user.PaisID);
        if (tipo_contabilidad == 2)
        {
            lb_moneda.SelectedValue = user.Moneda.ToString();
        }
        else
        {
            #region Backup
            //lb_moneda.SelectedValue = user.pais.ID.ToString();
            #endregion
            lb_moneda.SelectedValue = user.Moneda.ToString();
        }
        if (lb_imp_exp.Items.Count > 0)
            impo_expo = int.Parse(lb_imp_exp.SelectedValue);
    }
    private void obtengo_listas(int tipoconta, int impo_expo, int paiID)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            if (ttr == 12)
            {
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='pop_notacredito.aspx'");
            }
            else if (ttr == 31)
            {
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='notacreditond.aspx'");
            }
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipo_transaccion.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(paiID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (impo_expo == rgb.intC1) { item.Selected = true; lb_imp_exp.Enabled = false; }
                lb_imp_exp.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_contribuyente");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_contribuyente.Items.Add(item);
            }
            /*
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 1, user);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
            }
            */
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, ttr, user, 1);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serieNC.Items.Add(item);
            }
            
        }
    }
    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["ncID"] != null && !Request.QueryString["ncID"].ToString().Equals(""))
            {
                ncID = int.Parse(Request.QueryString["ncID"].ToString().Trim());
                ttr = int.Parse(Request.QueryString["tipo"].ToString().Trim());
                //Obtengo los rubros de la factura
                RE_GenericBean factdata = (RE_GenericBean)DB.getNotaCreditoAgenteData(ncID);
                factID = factdata.intC1;
                tbCliCod.Text=factdata.intC3.ToString();
                tb_nombre.Text=factdata.strC3;
                tb_nit.Text=factdata.strC2;
                lb_imp_exp.SelectedValue = factdata.intC6.ToString();
                lb_moneda.SelectedValue = factdata.intC4.ToString();
                lb_tipocobro.Text = factdata.intC7.ToString();
                tb_observaciones.Text = factdata.strC7;
                tb_referencia.Text = factdata.strC27;
                tb_hbl.Text = factdata.strC9;
                tb_mbl.Text = factdata.strC10;
                tb_contenedor.Text = factdata.strC11;
                tb_routing.Text = factdata.strC12;
                tb_fecha.Text = factdata.strC5;
                tb_corrNC.Text = factdata.strC33;
                lb_serieNC.SelectedValue = factdata.strC29;
                lb_serieNC.SelectedValue = factdata.strC32;
                lb_serieNC.Enabled = false;
                tb_poliza.Text = factdata.strC34;//Poliza Aduanal
                tb_total.Text = factdata.decC3.ToString();
                tb_subtotal.Text = factdata.decC4.ToString();
                tb_impuesto.Text = factdata.decC5.ToString();
                tb_totaldolares.Text = factdata.decC6.ToString();
                dgw.DataSource = (DataTable)DB.getRubbyNC_Agente(ncID, ttr);
                dgw.DataBind();

                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(ncID.ToString()), ttr, 0);
                gv_detalle_partida.DataBind();
                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
            }
        }
    }

    protected void bt_nota_credito_virtual_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 32) == 32))
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }
        else
        {
            String csname1 = "PopupScript";
            Type cstype = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('../invoice/template_invoice.aspx?id=" + Request.QueryString["ncID"].ToString() + "&transaccion=" + Request.QueryString["tipo"].ToString() + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
}
