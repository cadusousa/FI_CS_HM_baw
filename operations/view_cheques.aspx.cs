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

public partial class operations_pop_cheques : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        
        if (!Page.IsPostBack) {
            obtengo_listas();
            
            if (Request.QueryString["chequeNo"] == null || Request.QueryString["chequeNo"].ToString().Trim().Equals("")) {
                WebMsgBox.Show("Debe indicar cual es el número correlativo del cheque que desea visualizar");
                return;
            }
            int chequeNo = int.Parse(Request.QueryString["chequeNo"].ToString());

            RE_GenericBean rgb_cheque = (RE_GenericBean)DB.getChequeDataforView(chequeNo);

            lb_transaccion.SelectedValue=rgb_cheque.intC2.ToString();
            lb_correlativo.Text = chequeNo.ToString();
            lb_bancos.SelectedValue=rgb_cheque.intC3.ToString();
            ListItem item = null;
            lb_cuentas_bancarias.Items.Clear();
            item = new ListItem("Seleccione...", " ");
            item.Selected = true;
            lb_cuentas_bancarias.Items.Add(item);
            ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue),user.PaisID,0);
            foreach (RE_GenericBean rgb in arrCtas)
            {
                item = new ListItem(rgb.strC1, rgb.strC1);
                lb_cuentas_bancarias.Items.Add(item);
            }
            lb_cuentas_bancarias.SelectedValue=rgb_cheque.strC2;
            lb_moneda.SelectedValue=rgb_cheque.intC4.ToString();
            tb_chequeNo.Text = rgb_cheque.intC5.ToString();
            tb_fecha.Text = rgb_cheque.strC3;
            tb_acreditado.Text=rgb_cheque.strC4;
            tb_motivo.Text = rgb_cheque.strC5;
            tb_referencia.Text = rgb_cheque.strC6;
            tb_observaciones.Text = rgb_cheque.strC7;
            tb_valor.Text=rgb_cheque.decC1.ToString();
            lb_tipopersona.SelectedValue = rgb_cheque.intC7.ToString();
            tb_agenteID.Text = rgb_cheque.intC6.ToString();
            tb_agentenombre.Text = rgb_cheque.strC8.ToString();
            int proveedorID = int.Parse(tb_agenteID.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
            int monID = int.Parse(lb_moneda.SelectedValue);
            ArrayList Arr = null;
            Arr = DB.getCortesProveedorPagados(proveedorID, proveedortype, monID, 4,chequeNo);
            foreach (RE_GenericBean rgb in Arr)
            {
                lbl_cheque_serie.Text = rgb.strC1;
                lb_correlativo.Text = rgb.intC2.ToString();
            }
            dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
            gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(rgb_cheque.intC1.ToString()), 6, 0);
            gv_detalle_partida.DataBind();
            gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

            if (rgb_cheque.intC7 == 10)
            {
                #region Asignar Nombre Intercompany y Nombre Fiscal
                RE_GenericBean Intercompany_Bean = (RE_GenericBean)DB.Get_Intercompany_Data(rgb_cheque.intC6);
                tb_agentenombre.Text = Intercompany_Bean.strC5 + "   (" + Intercompany_Bean.strC1 + ")";
                #endregion
            }
        }
    }

    protected void obtengo_listas() {
        ArrayList arr = null;
        ListItem item = null;
        lb_moneda.Items.Clear();
        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_moneda.Items.Add(item);
        }
        // obtengo las transacciones
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='"+user.ID+"' and uop_pai_id="+user.PaisID+" and uop_ttt_id=ttt_id and ttt_template='pop_cheques.aspx'");
        lb_transaccion.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_transaccion.Items.Add(item);
        }
        //obtengo Transacciones de Anticipos
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='anticipos.aspx'");
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_transaccion.Items.Add(item);
        }
        arr = (ArrayList)DB.getBancosXPais(user.PaisID,1);
        lb_bancos.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_bancos.Items.Add(item);
        }
        item = new ListItem("Seleccione...", "0");
        lb_cuentas_bancarias.Items.Add(item);
    }

    
    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();

        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('../invoice/re_print.aspx?fac_id=" + Request.QueryString["chequeNo"].ToString() + "&tipo=6&ctaID=" + lb_cuentas_bancarias.SelectedValue + "&bco="+lb_bancos.SelectedItem.Text+"&correlativo=" + tb_chequeNo.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        #region Seteo de Parametros de Impresion
        user.ImpresionBean.Operacion = "2";
        user.ImpresionBean.Tipo_Documento = "6";
        user.ImpresionBean.Id = Request.QueryString["chequeNo"].ToString();
        user.ImpresionBean.Impreso = true;
        Session["usuario"] = user;
        #endregion
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }

    }

    protected void btn_printret_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "";

        if (user.PaisID == 3 || user.PaisID == 23)
        {
            script = "window.open('../invoice/template_retencion.aspx?chequeID=" + Request.QueryString["chequeNo"].ToString() + "&sucursalID=" + user.SucursalID.ToString() + "&userID=" + user.ID + "&contaId=" + user.contaID.ToString() + "&tipo=20','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            //script = "window.open('../plantillas/impresion.aspx?id=" + lb_cuentas_bancarias.SelectedValue + "&correlativo=" + tb_chequeNo.Text + "&tipo=6','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";

        }
        else
        {
            script = "window.open('../invoice/re_print.aspx?fac_id=" + Request.QueryString["chequeNo"].ToString() + "&tipo=20&ctaID=" + lb_cuentas_bancarias.SelectedValue + "&bco=" + lb_bancos.SelectedItem.Text + "&correlativo=" + tb_chequeNo.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        }
            #region Seteo de Parametros de Impresion
        user.ImpresionBean.Operacion = "2";
        user.ImpresionBean.Tipo_Documento = "9";
        user.ImpresionBean.Id = Request.QueryString["chequeNo"].ToString();
        user.ImpresionBean.Impreso = true;
        Session["usuario"] = user;
        #endregion
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
    protected void gv_cortes_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[0].Visible = false;

    }
}
