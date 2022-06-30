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
using System.IO;
using System.Text;

public partial class operations_Ajustes : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null, dt1 = null;
    decimal saldo_inicial = 0;
    decimal saldo_actual = 0;
    decimal total = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
            Response.Redirect("../default.aspx");

        user = (UsuarioBean)Session["usuario"];

        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 131072) == 131072))
            Response.Redirect("index.aspx");
        if (!Page.IsPostBack)
        {
            obtengo_listas();
            lb_bancos_SelectedIndexChanged(sender, e);
        }
    }
    protected void obtengo_listas()
    {
        ArrayList arr = null;
        ListItem item = null;
        lb_moneda.Items.Clear();
        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_moneda.Items.Add(item);
        }
        arr = (ArrayList)DB.getBancosXPais(user.PaisID, 1);
        lb_bancos.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_bancos.Items.Add(item);
        }
        item = new ListItem("Seleccione...", "0");
        lb_banco_cuenta.Items.Add(item);
    }
    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        lb_banco_cuenta.Items.Clear();
        item = new ListItem("Seleccione...", " ");
        item.Selected = true;
        lb_banco_cuenta.Items.Add(item);
        ArrayList arrCtas = null;
        if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
        {
            arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(lb_bancos.SelectedValue), user, 1);
        }
        else
        {
            arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(lb_bancos.SelectedValue), user, 0);
        }
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_banco_cuenta.Items.Add(item);
        }
    }
    protected void lb_banco_cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!lb_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
        {
            string cuentaID = lb_banco_cuenta.SelectedValue;
            RE_GenericBean datoscuenta = (RE_GenericBean)DB.GetMonedaIDbyCuentaBancaria(cuentaID);
            lb_moneda.SelectedValue = datoscuenta.intC1.ToString();
            gv_documentos.DataBind();
            lbl_total.Text = "0.00";
            lbl_saldo_inicial.Text = "0.00";
            lbl_saldo_actual.Text = "0.00";
        }
    }
    protected void bt_visualizar_Click(object sender, EventArgs e)
    {
        string sScript = "";
        if (lb_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
        {
            WebMsgBox.Show("Debe seleccionar una cuenta");
            return;
        }
        if (tb_fecha_inicial.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Porfavor Ingrese la Fecha Inicial del Perido a Consultar");
            return;
        }
        if (tb_fecha_final.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Porfavor Ingrese la Fecha Final del Periodo a Consultar");
            return;
        }
        if (tb_cuenta_nombre.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Porfavor Seleccione la Cuenta Contable");
            return;
        }

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "MOVIMIENTO BANCARIO";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Contabilidad.: " + user.contaID + " Moneda.: " + lb_moneda.SelectedItem.Text + " ,";
        mensaje_log += "Banco.: " + lb_bancos.SelectedItem.Text + " Cuenta.: " + lb_banco_cuenta.SelectedItem.Text + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fecha_inicial.Text + " Fecha Final.: " + tb_fecha_final.Text + " ,";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        #region Formatear Fechas
        //Fecha Inicio
        string Fecha_inicial = "";
        int fe_dia = int.Parse(tb_fecha_inicial.Text.Trim().Substring(3, 2));
        int fe_mes = int.Parse(tb_fecha_inicial.Text.Trim().Substring(0, 2));
        int fe_anio = int.Parse(tb_fecha_inicial.Text.Trim().Substring(6, 4));
        Fecha_inicial = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            Fecha_inicial += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            Fecha_inicial += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            Fecha_inicial += "0" + fe_dia.ToString();
        }
        else
        {
            Fecha_inicial += fe_dia.ToString();
        }
        //Fecha Fin
        string Fecha_final = "";
        fe_dia = int.Parse(tb_fecha_final.Text.Substring(3, 2));
        fe_mes = int.Parse(tb_fecha_final.Text.Substring(0, 2));
        fe_anio = int.Parse(tb_fecha_final.Text.Substring(6, 4));
        Fecha_final = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            Fecha_final += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            Fecha_final += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            Fecha_final += "0" + fe_dia.ToString();
        }
        else
        {
            Fecha_final += fe_dia.ToString();
        }
        #endregion
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("TIPO");
        dt.Columns.Add("DOCUMENTO");
        dt.Columns.Add("FECHA");
        dt.Columns.Add("DESCRIPCION");
        dt.Columns.Add("DEBE");
        dt.Columns.Add("HABER");
        dt.Columns.Add("SALDO");
        ArrayList arr = null;
        arr = (ArrayList)DB.getMovimientoBancario(user, int.Parse(lb_bancos.SelectedValue), lb_banco_cuenta.SelectedValue, Fecha_inicial, Fecha_final, tb_cuenta.Text.Trim(), int.Parse(lb_moneda.SelectedValue));
        foreach (RE_GenericBean rgb in arr)
        {
            object[] objArr = { rgb.intC1, rgb.strC5, rgb.strC1, rgb.strC2, rgb.strC6, rgb.strC3, rgb.strC4, "0.00" };
            dt.Rows.Add(objArr);
        }
        gv_documentos.DataSource = dt;
        gv_documentos.DataBind();
        saldo_inicial = DB.getSaldoInicialMovimientoBancario(user, int.Parse(lb_bancos.SelectedValue), lb_banco_cuenta.SelectedValue, tb_cuenta.Text.Trim(), Fecha_inicial, int.Parse(lb_moneda.SelectedValue));
        lbl_saldo_anterior.Text = saldo_inicial.ToString();
        lbl_saldo_inicial.Text = (saldo_inicial).ToString("#,#.00#;(#,#.00#)");
        lbl_saldo_inicial2.Text = (saldo_inicial).ToString("#,#.00#;(#,#.00#)");
        Calcular_Saldos();
    }
    protected void bt_buscar_cta_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        int clasificacion = int.Parse(lb_clasificacion.SelectedValue);
        if (clasificacion != 0) where += " and cue_clasificacion in (" + clasificacion + ")";
        if (!tb_nombre_cta.Text.Trim().Equals("") && tb_nombre_cta.Text != null) where += " and UPPER(cue_nombre) like '%" + tb_nombre_cta.Text.Trim().ToUpper() + "%'";
        if (!tb_cuenta_numero.Text.Trim().Equals("") && tb_cuenta_numero.Text != null) where += " and(cue_id) ilike '%" + tb_cuenta_numero.Text.Trim() + "%'";
        Arr = Utility.getCuentasContables("XTipoClasificacion", where);
        dt = (DataTable)Utility.fillGridView("Cuenta", Arr);
        gv_cuenta.DataSource = dt;
        gv_cuenta.DataBind();
        ViewState["gv_cuenta_dt"] = dt;
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void gv_cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_cuenta.DataSource = dt;
        gv_cuenta.PageIndex = e.NewPageIndex;
        gv_cuenta.DataBind();
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void gv_cuenta_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ArrayList cuentaArr = null;
        }
        else
        {
            dt = (DataTable)ViewState["gv_cuenta_dt"];
        }
    }
    protected void gv_cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_cuenta.SelectedRow;
        tb_cuenta.Text = row.Cells[1].Text;
        tb_cuenta_nombre.Text = row.Cells[2].Text;
    }
    protected void gv_documentos_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 0)
            e.Row.Cells[0].Visible = false;
    }
    protected void Calcular_Saldos()
    {
        decimal debe = 0;
        decimal haber = 0;
        decimal DEBE = 0;
        decimal HABER = 0;
        decimal SALDO_TEMPORAL = 0;
        decimal SALDO = 0;
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("TIPO");
        dt.Columns.Add("DOCUMENTO");
        dt.Columns.Add("FECHA");
        dt.Columns.Add("DESCRIPCION");
        dt.Columns.Add("DEBE");
        dt.Columns.Add("HABER");
        dt.Columns.Add("SALDO");
        SALDO_TEMPORAL = decimal.Parse(lbl_saldo_anterior.Text);
        GridViewRowCollection documentos = gv_documentos.Rows;
        foreach (GridViewRow row in documentos)
        {
            DEBE += decimal.Parse(row.Cells[5].Text);
            HABER += decimal.Parse(row.Cells[6].Text);
            debe = 0;
            haber = 0;
            debe += decimal.Parse(row.Cells[5].Text);
            haber += decimal.Parse(row.Cells[6].Text);
            SALDO = debe - haber;
            SALDO_TEMPORAL += SALDO;
            object[] objArr = { Server.HtmlDecode(row.Cells[0].Text), Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), SALDO_TEMPORAL.ToString() };
            dt.Rows.Add(objArr);
        }
        gv_documentos.DataSource = dt;
        gv_documentos.DataBind();
        saldo_actual = DEBE - HABER;
        total = saldo_actual + decimal.Parse(lbl_saldo_anterior.Text);
        lbl_saldo_actual.Text = saldo_actual.ToString("#,#.00#;(#,#.00#)");
        lbl_total.Text = total.ToString("#,#.00#;(#,#.00#)");
    }
    protected void bt_exportar_excel_Click(object sender, ImageClickEventArgs e)
    {
        if (gv_documentos.Rows.Count > 0)
        {
            #region Definir Header
            string Header = "<table align='left' cellpadding='0' cellspacing='0' "+
            "style='border: black solid 1px; width: 100%;' bgcolor='White'>"+
            "<tr>"+
                "<td align='center' bgcolor='#D9ECFF' class='style2' colspan='3' "+
                    "style='border-bottom: solid 1px black; font-weight: bold; '>" +
                    "MOVIMIENTO DE BANCOS</td>"+
            "</tr>"+
            "<tr>"+
                "<td class='style1' width='100'>"+
                    "&nbsp;</td>"+
                "<td style='font-weight: bold'>" +
                    "Aimar </td>"+
                "<td align='left'>"+
                    ""+user.pais.Nombre+"</td>"+
            "</tr>"+
            "<tr>"+
                "<td class='style1'>"+
                    "&nbsp;</td>"+
                "<td style='font-weight: bold'>" +
                    "Banco</td>"+
                "<td>"+
                    ""+lb_bancos.SelectedItem.Text+"</td>"+
            "</tr>"+
            "<tr>"+
                "<td class='style1'>"+
                    "&nbsp;</td>"+
                "<td style='font-weight: bold'>" +
                    "Cuenta</td>"+
                "<td align='left'>" +
                    ""+lb_banco_cuenta.SelectedItem.Text+"</td>"+
            "</tr>"+
            "<tr>"+
                "<td class='style1'>"+
                    "&nbsp;</td>"+
                "<td style='font-weight: bold'>" +
                    "Fecha Inicial</td>"+
                "<td align='left'>" +
                    ""+tb_fecha_inicial.Text+"</td>"+
            "</tr>"+
            "<tr>"+
                "<td class='style1'>"+
                    "&nbsp;</td>"+
                "<td style='font-weight: bold' align='left'>"+
                    "Fecha Final</td>"+
                "<td align='left'>" +
                    ""+tb_fecha_final.Text+"</td>"+
            "</tr>"+
        "</table>";
            #endregion
            #region Definir Pie
            string pie = "<br><table align='center' cellpadding='0' cellspacing='0' style='width: 60%; border:solid 1px black;'>" +
                        "<tr>" +
                            "<td align='right' width='50%'>" +
                                "<b>Saldo Actual.:</b></td>" +
                            "<td align='right' width='50%'>" +
                                lbl_saldo_actual.Text+
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td align='right' style=' border-bottom:solid 1px black; '>" +
                                "<b>(+)Saldo Inicial.:</b></td>" +
                            "<td align='right' style=' border-bottom:solid 1px black; '>" +
                                lbl_saldo_inicial2.Text+
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td align='right' bgcolor='#D9ECFF'>" +
                                "<b>Total.:</b></td>" +
                            "<td align='right' bgcolor='#D9ECFF'>" +
                               lbl_total.Text +
                            "</td>" +
                        "</tr>" +
                    "</table>";
            #endregion
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gv_documentos.EnableViewState = false;
            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;
            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            htw.Write(Header);
            form.Controls.Add(gv_documentos);
            page.RenderControl(htw);
            htw.Write(pie);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=Movimiento_Bancos.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}
