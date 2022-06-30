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

public partial class operations_pop_retenciontoprintlist : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];

        if (!Page.IsPostBack)
        {
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            obtengo_listas(tipo_contabilidad);
            Cargo_retenciones_list();
        }
    }

    private void obtengo_listas(int tipoconta)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("../default.aspx");
            }
            user = (UsuarioBean)Session["usuario"];

            item = null;
            arr = null;
            arr = (ArrayList)DB.getPlantillaRetencionesList(user.PaisID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                rbl_optionretencion.Items.Add(item);
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        bool existe = false;
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        CheckBox chk1;
        int plantillaID = int.Parse(rbl_optionretencion.SelectedValue);
        int provisionIDTemp = 0, retenciontipotemp = 0;
        string impresion = "";

        ArrayList impresionarr = new ArrayList();
        ArrayList tiposretbyplantillaID = (ArrayList)DB.getRetencionTipobyPlantillaID(plantillaID);

        user = (UsuarioBean)Session["usuario"];
        foreach (GridViewRow row in gv_retenciones.Rows)
        {
            existe = false;
            lb1 = (Label)row.FindControl("lb_id");
            lb2 = (Label)row.FindControl("lb_serieprov");
            lb3 = (Label)row.FindControl("lb_correlativoprov");
            lb4 = (Label)row.FindControl("lb_nombreretencion");
            lb5 = (Label)row.FindControl("lb_valor");
            lb6 = (Label)row.FindControl("lb_moneda");
            lb7 = (Label)row.FindControl("lb_rettipoid");
            lb8 = (Label)row.FindControl("lb_provisionID");
            chk1 = (CheckBox)row.FindControl("chk_seleccionar");
            if (chk1.Checked)
            {
                if (retenciontipotemp != 0)
                {
                    if (lb7.Text.Equals(retenciontipotemp.ToString().Trim()))
                    {
                        WebMsgBox.Show("Las retenciones que desea imprimir no pueden ser del mismo tipo");
                        return;
                    }
                }
                retenciontipotemp = int.Parse(lb7.Text);

                if (provisionIDTemp != 0)
                {
                    if (!lb8.Text.Equals(provisionIDTemp.ToString().Trim()))
                    {
                        WebMsgBox.Show("Las retenciones que desea imprimir en el mismo documento deben pertenecer a la misma provision");
                        return;
                    }
                }
                provisionIDTemp = int.Parse(lb8.Text);

                foreach (string tiporet in tiposretbyplantillaID)
                {
                    if (tiporet.Equals(lb7.Text))
                    {
                        impresion += lb1.Text + "|";
                        existe = true;
                        break;
                    }
                }

                if (!existe)
                {
                    WebMsgBox.Show("La retención " + lb1.Text + " no esta configurada para la plantilla que seleccionó");
                    return;
                }
            }
        }

        //Response.Redirect("print_retencion.aspx?plantillaID=" + plantillaID + "&retenciones=" + impresion + "&provID=" + provisionIDTemp);

        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('print_retencion.aspx?plantillaID=" + plantillaID + "&retenciones=" + impresion + "&provID=" + provisionIDTemp+"','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }

        //Actualizo con que plantilla y correlativo se imprimio la retencion
        RE_GenericBean font_interlineado = DB.getInterlineadoRetencion(plantillaID);
        foreach (GridViewRow row in gv_retenciones.Rows)
        {
            existe = false;
            lb1 = (Label)row.FindControl("lb_id");
            chk1 = (CheckBox)row.FindControl("chk_seleccionar");
            if (chk1.Checked)
            {
                DB.AsocioRetencionconPlantillaRet(int.Parse(lb1.Text), plantillaID, user.ID, font_interlineado.strC3, font_interlineado.intC3);
            }
        }

        //Confirmacion_ModalPopupExtender.Show();
        Cargo_retenciones_list();
        
    }

    protected void Cargo_retenciones_list() {
        try {
            int chequetransID = int.Parse(Request.QueryString["chequetransID"].ToString());
            ArrayList retencionarr = (ArrayList)DB.getRetencionesbyChequeForPrint(chequetransID);
            dt = Utility.fillGridView("retenciontoprint", retencionarr);
            gv_retenciones.DataSource = dt;
            gv_retenciones.DataBind();
        } catch (Exception e) {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
        }
    }
    protected void btn_no_Click(object sender, EventArgs e)
    {
        //user = (UsuarioBean)Session["usuario"];
        //int plantillaID = int.Parse(rbl_optionretencion.SelectedValue);
        //RE_GenericBean font_interlineado = DB.getInterlineadoRetencion(plantillaID);

        //Label lb1;
        //CheckBox chk1;
        //foreach (GridViewRow row in gv_retenciones.Rows)
        //{
        //    lb1 = (Label)row.FindControl("lb_id");
        //    chk1 = (CheckBox)row.FindControl("chk_seleccionar");
        //    if (chk1.Checked)
        //    {
        //        DB.DesAsocioRetencionconPlantillaRet(int.Parse(lb1.Text), plantillaID, user.ID, font_interlineado.strC3, font_interlineado.intC3-1);
        //    }
        //}
        //Cargo_retenciones_list();
    }
}
