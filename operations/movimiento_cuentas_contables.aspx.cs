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

public partial class operations_movimiento_cuentas_contables : System.Web.UI.Page
{
    UsuarioBean user = null;
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
        //Cargar_Documentos();
    }
    protected void obtengo_listas()
    {
        ArrayList arr = null;
        ListItem item = null;
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
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(lb_bancos.SelectedValue), user, 1);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_banco_cuenta.Items.Add(item);
        }
    }
    protected void lb_tipo_documento_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_serie.Text = "";
        tb_correlativo.Text = "";
        tb_referencia.Text = "";
        tb_credito_no.Text = "";
        lb_bancos_SelectedIndexChanged(sender, e);
        gv_documentos.DataBind();
        if ((lb_tipo_documento.SelectedValue == "1") || (lb_tipo_documento.SelectedValue == "2") || (lb_tipo_documento.SelectedValue == "3") || (lb_tipo_documento.SelectedValue == "4") || (lb_tipo_documento.SelectedValue == "5") || (lb_tipo_documento.SelectedValue == "15") || (lb_tipo_documento.SelectedValue == "18") || (lb_tipo_documento.SelectedValue == "24") || (lb_tipo_documento.SelectedValue == "31"))
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
            Panel3.Visible = false;
        }
        if ((lb_tipo_documento.SelectedValue == "6") || (lb_tipo_documento.SelectedValue == "19") || (lb_tipo_documento.SelectedValue == "17") || (lb_tipo_documento.SelectedValue == "28"))
        {
            Panel1.Visible = false;
            Panel2.Visible = true;
            Panel3.Visible = false;
        }
        if ((lb_tipo_documento.SelectedValue == "20") || (lb_tipo_documento.SelectedValue == "21"))
        {
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = true;
        }
        if (lb_tipo_documento.Text == "Seleccione...")
        {
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
        }
    }
    protected void Cargar_Documentos()
    {
        string fecha_inicial = "";
        string fecha_final = "";
        fecha_inicial = tb_fecha_inicial.Text.Trim();
        fecha_final = tb_fecha_final.Text.Trim();
        #region Formatear Fechas
        int fe_dia = 0;
        int fe_mes = 0;
        int fe_anio = 0;
        //Fecha Inicio
        if (fecha_inicial != "")
        {
            fe_dia = int.Parse(fecha_inicial.Substring(3, 2));
            fe_mes = int.Parse(fecha_inicial.Substring(0, 2));
            fe_anio = int.Parse(fecha_inicial.Substring(6, 4));
            fecha_inicial = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                fecha_inicial += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                fecha_inicial += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                fecha_inicial += "0" + fe_dia.ToString();
            }
            else
            {
                fecha_inicial += fe_dia.ToString();
            }
        }
        //Fecha Fin
        if (fecha_final != "")
        {
            fe_dia = int.Parse(fecha_final.Substring(3, 2));
            fe_mes = int.Parse(fecha_final.Substring(0, 2));
            fe_anio = int.Parse(fecha_final.Substring(6, 4));
            fecha_final = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                fecha_final += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                fecha_final += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                fecha_final += "0" + fe_dia.ToString();
            }
            else
            {
                fecha_final += fe_dia.ToString();
            }
        }
        #endregion
        if (lb_tipo_documento.SelectedValue == "Seleccione...")
        {
            #region Cargar Todos los Documentos
            if (tb_fecha_inicial.Text == "")
            {
                WebMsgBox.Show("Debe Ingresar la Fecha Inicial");
                return;
            }
            if (tb_fecha_final.Text == "")
            {
                WebMsgBox.Show("Debe Ingresar la Fecha Final");
                return;
            }
            DataTable DT_Documentos = DB.getMovimientoCuentasContables(user, fecha_inicial, fecha_final, 0, "", 0, 0, "", "");
            gv_documentos.DataSource = DT_Documentos;
            gv_documentos.DataBind();
            #endregion
        }
        else
        {
            if ((lb_tipo_documento.SelectedValue == "1") || (lb_tipo_documento.SelectedValue == "2") || (lb_tipo_documento.SelectedValue == "3") || (lb_tipo_documento.SelectedValue == "4") || (lb_tipo_documento.SelectedValue == "5") || (lb_tipo_documento.SelectedValue == "15") || (lb_tipo_documento.SelectedValue == "18") || (lb_tipo_documento.SelectedValue == "24")  || (lb_tipo_documento.SelectedValue == "31"))
            {
                #region Cargar Documento especifico por Serie y Correlativo
                if ((tb_serie.Text != "") && (tb_correlativo.Text != ""))
                {
                    DataTable DT_Documentos = DB.getMovimientoCuentasContables(user, fecha_inicial, fecha_final, int.Parse(lb_tipo_documento.SelectedValue), tb_serie.Text.Trim().ToUpper(), int.Parse(tb_correlativo.Text.Trim()), 0, "", "");
                    gv_documentos.DataSource = DT_Documentos;
                    gv_documentos.DataBind();
                }
                else
                {
                    if (tb_serie.Text == "")
                    {
                        WebMsgBox.Show("Debe Ingresar la Serie del Documento");
                        return;
                    }
                    if (tb_correlativo.Text == "")
                    {
                        WebMsgBox.Show("Debe Ingresar el Correlativo del Documento");
                        return;
                    }
                }
                #endregion
            }
            if ((lb_tipo_documento.SelectedValue == "6") || (lb_tipo_documento.SelectedValue == "19") || (lb_tipo_documento.SelectedValue == "17") || (lb_tipo_documento.SelectedValue == "28"))
            {
                #region Cargar Deposito - Cheque - Transferencia Especifica
                if ((lb_banco_cuenta.SelectedValue != "Seleccione..") || (tb_referencia.Text != ""))
                {
                    DataTable DT_Documentos = DB.getMovimientoCuentasContables(user, fecha_inicial, fecha_final, int.Parse(lb_tipo_documento.SelectedValue), "", 0, int.Parse(lb_bancos.SelectedValue.ToString()), tb_referencia.Text.Trim(), lb_banco_cuenta.SelectedValue);
                    gv_documentos.DataSource = DT_Documentos;
                    gv_documentos.DataBind();
                }
                else
                {
                    if (lb_banco_cuenta.SelectedValue == "Seleccione...")
                    {
                        WebMsgBox.Show("Debe Seleccionar el numero de Cuenta");
                        return;
                    }
                    if (tb_referencia.Text == "")
                    {
                        WebMsgBox.Show("Debe Ingresar el numero de Referencia");
                        return;
                    }
                }
                #endregion
            }
            if ((lb_tipo_documento.SelectedValue == "20") || (lb_tipo_documento.SelectedValue == "21"))
            {
                #region Cargar NCB Y NDB Especifica
                if (tb_credito_no.Text != "")
                {
                    DataTable DT_Documentos = DB.getMovimientoCuentasContables(user, fecha_inicial, fecha_final, int.Parse(lb_tipo_documento.SelectedValue), "", 0, 0, tb_credito_no.Text.Trim(), "");
                    gv_documentos.DataSource = DT_Documentos;
                    gv_documentos.DataBind();
                }
                else
                {
                    WebMsgBox.Show("Debe Ingresar el numero de Credito");
                    return;
                }
                #endregion
            }
        }
    }

    protected void bt_visualizar_Click(object sender, EventArgs e)
    {
        Cargar_Documentos();
        Calcular_Total();
        Marcar_Partidas_Anulacion();
    }
    protected void gv_documentos_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dataTable = gv_documentos.DataSource as DataTable;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            dataView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            gv_documentos.DataSource = dataView;
            gv_documentos.DataBind();
        }
    }
    public string SortExpression
    {
        get { return (ViewState["SortExpression"] == null ? string.Empty : ViewState["SortExpression"].ToString()); }
        set { ViewState["SortExpression"] = value; }
    }
    public string SortDirection
    {
        get { return (ViewState["SortDirection"] == null ? string.Empty : ViewState["SortDirection"].ToString()); }
        set { ViewState["SortDirection"] = value; }
    }
    private string GetSortDirection(string sortExpression)
    {
        if (SortExpression == sortExpression)
        {
            if (SortDirection == "ASC")
                SortDirection = "DESC";
            else if (SortDirection == "DESC")
                SortDirection = "ASC";
            return SortDirection;
        }
        else
        {
            SortExpression = sortExpression;
            SortDirection = "ASC";
            return SortDirection;
        }
    }
    protected void Calcular_Total()
    {
        decimal DEBE = 0;
        decimal HABER = 0;
        decimal TOTAL = 0;
        DataTable dt = new DataTable();
        dt.Columns.Add("TTR_ID");
        dt.Columns.Add("TIPO");
        dt.Columns.Add("F_DOCUMENTO");
        dt.Columns.Add("F_PARTIDA");
        dt.Columns.Add("NO_PARTIDA");
        dt.Columns.Add("CUENTA CONTABLE");
        dt.Columns.Add("CUENTA NOMBRE");
        dt.Columns.Add("DOCUMENTO");
        dt.Columns.Add("DEBE");
        dt.Columns.Add("HABER");
        dt.Columns.Add("DESCRIPCION");
        GridViewRowCollection documentos = gv_documentos.Rows;
        string T = "";
        foreach (GridViewRow row in documentos)
        {
            T = row.Cells[0].Text;
            DEBE += decimal.Parse(row.Cells[8].Text);
            HABER += decimal.Parse(row.Cells[9].Text);
            if (int.Parse(row.Cells[0].Text) > 1000)
            {
                row.ForeColor = System.Drawing.Color.Blue;
            }
            object[] objArr = { Server.HtmlDecode(row.Cells[0].Text), Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), Server.HtmlDecode(row.Cells[7].Text), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
            dt.Rows.Add(objArr);
        }
        gv_documentos.DataSource = dt;
        gv_documentos.DataBind();
        lbl_debe.Text = DEBE.ToString("#,#.00#;(#,#.00#)");
        lbl_haber.Text = HABER.ToString("#,#.00#;(#,#.00#)");
        TOTAL = DEBE - HABER;
        lbl_total.Text = TOTAL.ToString("#,#.00#;(#,#.00#)");
    }
    protected void bt_exportar_excel_Click(object sender, ImageClickEventArgs e)
    {
        if (gv_documentos.Rows.Count > 0)
        {
            #region Definir Header
            string Header = "<table align='left' cellpadding='0' cellspacing='0' " +
            "style='border: black solid 1px; width: 100%;' bgcolor='White'>" +
            "<tr>" +
                "<td align='center' bgcolor='#D9ECFF' class='style2' colspan='3' " +
                    "style='border-bottom: solid 1px black; font-weight: bold; '>" +
                    "MOVIMIENTO DE CUENTAS CONTABLES</td>" +
            "</tr>" +
            "<tr>" +
                "<td class='style1' width='100'>" +
                    "&nbsp;</td>" +
                "<td style='font-weight: bold'>" +
                    "Aimar </td>" +
                "<td align='left'>" +
                    "" + user.pais.Nombre + "</td>" +
            "</tr>" +
            "<tr>" +
                "<td class='style1'>" +
                    "&nbsp;</td>" +
                "<td style='font-weight: bold'>" +
                    "Fecha Inicial</td>" +
                "<td align='left'>" +
                    "" + tb_fecha_inicial.Text + "</td>" +
            "</tr>" +
            "<tr>" +
                "<td class='style1'>" +
                    "&nbsp;</td>" +
                "<td style='font-weight: bold' align='left'>" +
                    "Fecha Final</td>" +
                "<td align='left'>" +
                    "" + tb_fecha_final.Text + "</td>" +
            "</tr>" +
        "</table>";
            #endregion
            #region Definir Pie
            string pie = "<br><table align='center' cellpadding='0' cellspacing='0' style='width: 60%; border:solid 1px black;'>" +
                        "<tr>" +
                            "<td align='right' width='50%'>" +
                                "<b>Debe.:</b></td>" +
                            "<td align='right' width='50%'>" +
                                lbl_debe.Text +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td align='right' style=' border-bottom:solid 1px black; '>" +
                                "<b>Haber.:</b></td>" +
                            "<td align='right' style=' border-bottom:solid 1px black; '>" +
                                lbl_haber.Text +
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
            Response.AddHeader("Content-Disposition", "attachment;filename=Movimiento_Cuentas_Contables.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
    }
    protected void Marcar_Partidas_Anulacion()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TTR_ID");
        dt.Columns.Add("TIPO");
        dt.Columns.Add("F_DOCUMENTO");
        dt.Columns.Add("F_PARTIDA");
        dt.Columns.Add("PARTIDA");
        dt.Columns.Add("CUENTA");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("DOCUMENTO");
        dt.Columns.Add("DEBE");
        dt.Columns.Add("HABER");
        dt.Columns.Add("DESCRIPCION");
        GridViewRowCollection grc = gv_documentos.Rows;
        foreach (GridViewRow row in grc)
        {
            if (int.Parse(row.Cells[0].Text) > 1000)
            {
                row.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                row.ForeColor = System.Drawing.Color.Black;
            }
            object[] objArr = { Server.HtmlDecode(row.Cells[0].Text), Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), Server.HtmlDecode(row.Cells[7].Text), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
            dt.Rows.Add(objArr);
        }
        gv_documentos.DataSource = dt;
    }
    protected void gv_documentos_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[0].Visible = false;
        }
    }
}
