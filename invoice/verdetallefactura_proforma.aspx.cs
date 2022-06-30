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
    UsuarioBean user = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        int impo_expo = 0;
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad, impo_expo, user.PaisID);
        if (tipo_contabilidad == 2)
        {
            lb_moneda.SelectedValue = "8";
        }
        else
        {
            lb_moneda.SelectedValue = user.pais.ID.ToString();
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
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='pop_notacredito.aspx'");
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
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 1, user, 0);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
            }
            
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["factID"] != null && !Request.QueryString["factID"].ToString().Equals(""))
            {
                factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                //Obtengo los rubros de la factura
                RE_GenericBean factdata = (RE_GenericBean)DB.getFacturaData(factID);
                tb_agente_nombre.Text = factdata.strC41;//Agente
                tbCliCod.Text=factdata.intC3.ToString();
                tb_nombre.Text=factdata.strC3;
                tb_nit.Text=factdata.strC2;
                tb_razon.Text = factdata.strC26;
                //lb_imp_exp.SelectedValue = factdata;
                lb_moneda.SelectedValue = factdata.intC4.ToString();
                tb_direccion.Text = factdata.strC4;
                tb_observaciones.Text = factdata.strC7;
                tb_referencia.Text = factdata.strC27;
                tb_naviera.Text = factdata.strC13;
                tb_vapor.Text = factdata.strC14;
                tb_contenedor.Text = factdata.strC11;
                tb_bl.Text = factdata.strC9;
                tb_shipper.Text = factdata.strC15;
                tb_orden.Text = factdata.strC16;
                tb_consignee.Text = factdata.strC17;
                tb_comodity.Text = factdata.strC18;
                tb_paquetes2.Text = factdata.strC19;
                tb_peso.Text = factdata.strC20;
                tb_vol.Text = factdata.strC21;
                tb_dua_ingreso.Text = factdata.strC22;
                tb_dua_salida.Text = factdata.strC23;
                tb_vendedor1.Text = factdata.strC24;
                tb_vendedor2.Text = factdata.strC25;
                tb_total.Text = factdata.decC3.ToString();
                dgw.DataSource = (DataTable)DB.getRubbyFact(factID,1);
                dgw.DataBind();
            }
        }
    }
    protected void btn_factura_Click(object sender, EventArgs e)
    {
        
    }
}
