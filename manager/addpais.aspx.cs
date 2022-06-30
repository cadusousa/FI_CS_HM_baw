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

public partial class manager_addpais : System.Web.UI.Page
{
    public int pai_id = 0;
    public ArrayList tipocambio_arr = null;
    public ArrayList matrizopr_arr = null;
    DataTable dt = null;
    UsuarioBean user = null;
    int btn = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        RE_GenericBean rgb_cta=null;
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }
        UsuarioBean user1 = (UsuarioBean)Session["usuario"];
        if (!user1.Aplicaciones.Contains("3"))
        {
            Response.Redirect("manager.aspx");
        }
        int permiso = int.Parse(user1.Aplicaciones["3"].ToString());
        if (!((permiso & 1) == 1) && !((permiso & 2) == 2) && !((permiso & 4) == 4) && !((permiso & 8) == 8))
        {
            Response.Redirect("manager.aspx");
        }
        UsuarioBean usuario=(UsuarioBean)Session["usuario"];
        /*
        if (!usuario.Aplicaciones.ContainsKey(apID)) {
            Session["msg"] = "Error, No se pudieron obtener los datos de este registro, por favor intente de nuevo.";
            Session["url"] = "addpais.aspx?id" + id;
            Response.Redirect("message.aspx");
        }
        if (usuario.Aplicaciones[""])*/
        if (!Page.IsPostBack)
        {
            PaisBean pais = null;
            if (Request.QueryString["id"] != null)
            {
                pai_id = int.Parse(Request.QueryString["id"].ToString());
                pais = (PaisBean)DB.getPais(pai_id);
                if (pais == null)
                {
                    Session["msg"] = "Error, No se pudieron obtener los datos de este registro, por favor intente de nuevo.";
                    Session["url"] = "addpais.aspx?id=" + pai_id;
                    Response.Redirect("message.aspx");
                }
                tb_paisid.Text = pais.ID.ToString();
                
                tb_nombre.Text = pais.Nombre;
                tb_impuesto.Text = pais.Impuesto.ToString();
                tb_ctaCobraID.Text=pais.ctaCobra;
                chk_nit.Checked = pais.ctaNit;
                tb_recuperacion_iva.Text = pais.diasimpuesto.ToString();
                tb_diasholgura.Text = pais.diasholgura.ToString();
                tb_porc_interes.Text = pais.porcinteres.ToString();
                lb_momentoret.SelectedValue= pais.momentoretencion.ToString();
                tb_lista_correos.Text = pais.lista_correos;
                if (pais.ctaCobra != null && !pais.ctaCobra.Equals("")) {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaCobra);
                    tb_ctaCobraNombre.Text = rgb_cta.strC2;
                }
                tb_ctaPagaID.Text= pais.ctaPaga;
                if (pais.ctaPaga != null && !pais.ctaPaga.Equals("")) {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaPaga);
                    tb_ctaPagaNombre.Text = rgb_cta.strC2;
                }
                tb_reciboID.Text= pais.Recibo;
                if (pais.Recibo != null && !pais.Recibo.Equals("")) {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.Recibo);
                    tb_reciboNombre.Text = rgb_cta.strC2;
                }
                tb_ivaCobraID.Text = pais.ivaCobra;
                if (pais.ivaCobra != null && !pais.ivaCobra.Equals("")) {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ivaCobra);
                    tb_ivaCobraNombre.Text = rgb_cta.strC2;
                }
                tb_ivaPagaID.Text = pais.ivaPaga;
                if (pais.ivaPaga != null && !pais.ivaPaga.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ivaPaga);
                    tb_ivaPagaNombre.Text = rgb_cta.strC2;
                }
                tb_cta_anticipoCli_ID.Text = pais.ctaAnticipoCli;
                if (pais.ctaAnticipoCli != null && !pais.ctaAnticipoCli.Equals("")) {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaAnticipoCli);
                    tb_cta_anticipoCli_nombre.Text = rgb_cta.strC2;
                }
                tb_dif_cambiarioID.Text = pais.ctaDifCambiario;
                if (pais.ctaDifCambiario != null && !pais.ctaDifCambiario.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaDifCambiario);
                    tb_dif_cambiario_nombre.Text = rgb_cta.strC2;
                }
                tb_cta_intercompanyID.Text = pais.ctaIntercompany;
                if (pais.ctaIntercompany != null && !pais.ctaIntercompany.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaIntercompany);
                    tb_cta_intercompany_nombre.Text = rgb_cta.strC2;
                }
                tb_cta_intercompanyID_sv.Text = pais.ctaIntercompanySV;
                if (pais.ctaIntercompanySV != null && !pais.ctaIntercompanySV.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaIntercompanySV);
                    tb_cta_intercompany_sv.Text = rgb_cta.strC2;
                }
                tb_cta_intercompanyID_hn.Text = pais.ctaIntercompanyHN;
                if (pais.ctaIntercompanyHN != null && !pais.ctaIntercompanyHN.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaIntercompanyHN);
                    tb_cta_intercompany_hn.Text = rgb_cta.strC2;
                }
                tb_cta_intercompanyID_ni.Text = pais.ctaIntercompanyNI;
                if (pais.ctaIntercompanyNI != null && !pais.ctaIntercompanyNI.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaIntercompanyNI);
                    tb_cta_intercompany_ni.Text = rgb_cta.strC2;
                }
                tb_cta_intercompanyID_cr.Text = pais.ctaIntercompanyCR;
                if (pais.ctaIntercompanyCR != null && !pais.ctaIntercompanyCR.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaIntercompanyCR);
                    tb_cta_intercompany_cr.Text = rgb_cta.strC2;
                }
                tb_cta_intercompanyID_pa.Text = pais.ctaIntercompanyPA;
                if (pais.ctaIntercompanyPA != null && !pais.ctaIntercompanyPA.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaIntercompanyPA);
                    tb_cta_intercompany_pa.Text = rgb_cta.strC2;
                }
                tb_cta_intercompanyID_bz.Text = pais.ctaIntercompanyBZ;
                if (pais.ctaIntercompanyBZ != null && !pais.ctaIntercompanyBZ.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaIntercompanyBZ);
                    tb_cta_intercompany_bz.Text = rgb_cta.strC2;
                }
                tb_cta_intercompanyID_grh.Text = pais.ctaIntercompanyGRH;
                if (pais.ctaIntercompanyGRH != null && !pais.ctaIntercompanyGRH.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaIntercompanyGRH);
                    tb_cta_intercompany_grh.Text = rgb_cta.strC2;
                }
                tb_cta_intercompanyID_mayanL.Text = pais.ctaIntercompanyMayanL;
                if (pais.ctaIntercompanyMayanL != null && !pais.ctaIntercompanyMayanL.Equals(""))
                {
                    rgb_cta = (RE_GenericBean)DB.getCtabyCtaID(pais.ctaIntercompanyMayanL);
                    tb_cta_intercompany_mayanL.Text = rgb_cta.strC2;
                }
                tipocambio_arr = (ArrayList)DB.getTipoCambio(pai_id);
                matrizopr_arr = (ArrayList)DB.getMatribyPais(pai_id);
            }
            else
            {
                pai_id = 0;
                tb_paisid.Text = pai_id.ToString();
            }
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        PaisBean paisbean = new PaisBean();
        paisbean.ID = int.Parse(tb_paisid.Text.Trim());
        paisbean.Nombre = tb_nombre.Text.Trim().ToUpper();
        paisbean.Impuesto = decimal.Parse(tb_impuesto.Text.Trim());
        paisbean.ctaCobra = tb_ctaCobraID.Text;
        paisbean.ctaPaga = tb_ctaPagaID.Text;
        paisbean.Recibo = tb_reciboID.Text;
        paisbean.ctaAnticipoCli = tb_cta_anticipoCli_ID.Text;
        paisbean.ivaPaga = tb_ivaPagaID.Text;
        paisbean.ivaCobra = tb_ivaCobraID.Text;
        paisbean.ctaIntercompany = tb_cta_intercompanyID.Text;
        paisbean.ctaIntercompanySV = tb_cta_intercompanyID_sv.Text;
        paisbean.ctaIntercompanyHN = tb_cta_intercompanyID_hn.Text;
        paisbean.ctaIntercompanyNI = tb_cta_intercompanyID_ni.Text;
        paisbean.ctaIntercompanyCR=tb_cta_intercompanyID_cr.Text;
        paisbean.ctaIntercompanyPA=tb_cta_intercompanyID_pa.Text;
        paisbean.ctaIntercompanyBZ = tb_cta_intercompanyID_bz.Text;
        paisbean.ctaIntercompanyGRH=tb_cta_intercompanyID_grh.Text;
        paisbean.ctaIntercompanyMayanL = tb_cta_intercompanyID_mayanL.Text;
        paisbean.ctaNit = chk_nit.Checked;
        paisbean.ctaDifCambiario = tb_dif_cambiarioID.Text;
        if (!tb_recuperacion_iva.Text.Equals("")) paisbean.diasimpuesto = int.Parse(tb_recuperacion_iva.Text); else paisbean.diasimpuesto = 0;
        if (!tb_diasholgura.Text.Equals("")) paisbean.diasholgura = int.Parse(tb_diasholgura.Text); else paisbean.diasholgura = 0;
        if (!tb_porc_interes.Text.Equals("")) paisbean.porcinteres = decimal.Parse(tb_porc_interes.Text); else paisbean.porcinteres = 0;
        if (!lb_momentoret.SelectedValue.Equals("")) paisbean.momentoretencion = decimal.Parse(lb_momentoret.SelectedValue); else paisbean.momentoretencion = 0;
        paisbean.lista_correos = tb_lista_correos.Text;
        
        int result = DB.InsertUpdatePais(paisbean);
        if (result == 0)
        {
            Session["msg"] = "Error, No se pudieron grabar los datos que ingreso, por favor intente de nuevo.";
            Session["url"] = "addpais.aspx?id=" + paisbean.ID;
            Response.Redirect("message.aspx");
        }
        else if (result == -100)
        {
            Session["msg"] = "Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.";
            Session["url"] = "addpais.aspx?id=" + paisbean.ID;
            Response.Redirect("message.aspx");
        }
        Response.Redirect("searchpais.aspx");
    }

    //======================================================================================
    protected void gv_cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_cuenta.DataSource = dt;
        gv_cuenta.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        gv_cuenta.DataBind();
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void gv_cuenta_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ArrayList cuentaArr = null;
            cuentaArr = Utility.getCuentasContables("XClasificacion", "1,2,3,4,5");
            dt = (DataTable)Utility.fillGridView("Cuenta", cuentaArr);
            ViewState["gv_cuenta_dt"] = dt;
            gv_cuenta.DataSource = dt;
            gv_cuenta.DataBind();
        }
        else
        {
            dt = (DataTable)ViewState["gv_cuenta_dt"];
        }
    }
    protected void gv_cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_cuenta.SelectedRow;
        if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn1")) {
            tb_ctaCobraID.Text = row.Cells[1].Text;
            tb_ctaCobraNombre.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn2")) {
            tb_ctaPagaID.Text = row.Cells[1].Text;
            tb_ctaPagaNombre.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn3")) {
            tb_ivaPagaID.Text = row.Cells[1].Text;
            tb_ivaPagaNombre.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn4")) {
            tb_ivaCobraID.Text = row.Cells[1].Text;
            tb_ivaCobraNombre.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn5")) {
            tb_reciboID.Text = row.Cells[1].Text;
            tb_reciboNombre.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn6")) {
            tb_cta_anticipoCli_ID.Text = row.Cells[1].Text;
            tb_cta_anticipoCli_nombre.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn7")) {
            tb_cta_intercompanyID.Text = row.Cells[1].Text;
            tb_cta_intercompany_nombre.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn8")) {
            tb_cta_intercompanyID_sv.Text = row.Cells[1].Text;
            tb_cta_intercompany_sv.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn9")) {
            tb_cta_intercompanyID_hn.Text = row.Cells[1].Text;
            tb_cta_intercompany_hn.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn10")) {
            tb_cta_intercompanyID_ni.Text = row.Cells[1].Text;
            tb_cta_intercompany_ni.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn11")) {
            tb_cta_intercompanyID_cr.Text = row.Cells[1].Text;
            tb_cta_intercompany_cr.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn12")) {
            tb_cta_intercompanyID_pa.Text = row.Cells[1].Text;
            tb_cta_intercompany_pa.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn13")) {
            tb_cta_intercompanyID_bz.Text = row.Cells[1].Text;
            tb_cta_intercompany_bz.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn14")) {
            tb_cta_intercompanyID_grh.Text = row.Cells[1].Text;
            tb_cta_intercompany_grh.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn15")) {
            tb_dif_cambiarioID.Text = row.Cells[1].Text;
            tb_dif_cambiario_nombre.Text = row.Cells[2].Text;
        } else if (ViewState["btn"] != null && ViewState["btn"].ToString().Equals("btn16")) {
            tb_cta_intercompanyID_mayanL.Text = row.Cells[1].Text;
            tb_cta_intercompany_mayanL.Text = row.Cells[2].Text;
        }
    }

    protected void bt_buscar_cta_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        int clasificacion = int.Parse(lb_clasificacion.SelectedValue);
        if (clasificacion != 0) where += " and cue_clasificacion in (" + clasificacion + ")";
        if (!tb_nombre_cta.Text.Trim().Equals("") && tb_nombre_cta.Text != null) where += " and upper(cue_nombre) like '%" + tb_nombre_cta.Text.Trim().ToUpper() + "%'";
        Arr = Utility.getCuentasContables("XTipoClasificacion", where);
        dt = (DataTable)Utility.fillGridView("Cuenta", Arr);
        gv_cuenta.DataSource = dt;
        gv_cuenta.DataBind();
        ViewState["gv_cuenta_dt"] = dt;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn1";
        btn = 1;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn2";
        btn = 2;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn3";
        btn = 3;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn4";
        btn = 4;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn5";
        btn = 5;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn6";
        btn = 6;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn7";
        btn = 7;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button8_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn8";
        btn = 8;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button9_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn9";
        btn = 9;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button10_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn10";
        btn = 10;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button11_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn11";
        btn = 11;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button12_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn12";
        btn = 12;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button13_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn13";
        btn = 13;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button14_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn14";
        btn = 14;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button15_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn15";
        btn = 15;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button16_Click(object sender, EventArgs e)
    {
        ViewState["btn"] = "btn16";
        btn = 16;
        tb_cuenta_ModalPopupExtender.Show();
    }
}
