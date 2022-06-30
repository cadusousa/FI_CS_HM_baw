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
using System.Drawing;

public partial class operations_Ajustes : System.Web.UI.Page
{
    UsuarioBean user = null;
    decimal total_contabilidad = 0;
    decimal saldo_inicial = 0;
    decimal Saldo_Banco = 0;
    decimal Saldo_Final_Contabilidad = 0;
    decimal Saldo_Final_Circulacion = 0;
    decimal Total_Creditos_Contabilidad = 0;
    decimal Total_Debitos_Contabilidad = 0;
    decimal Total_Creditos_Circulacion = 0;
    decimal Total_Debitos_Circulacion = 0;
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
        //ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue), user.PaisID);
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(lb_bancos.SelectedValue), user,1);
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
            gv_documentos_contabilidad.DataBind();
            lbl_saldo_banco.Text = "0.00";
            lbl_saldo_final_contabilidad.Text = "0.00";
            lbl_saldo_final_circulacion.Text = "0.00";
        }
    }
    protected void bt_visualizar_Click(object sender, EventArgs e)
    {
        string sScript = "";
        if (lb_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
        {
            sScript = "alert('Debe seleccionar una cuenta bancaria');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        if (tb_fecha_inicial.Text.Trim().Equals(""))
        {
            sScript = "alert('Porfavor seleccione el Perido a Conciliar');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        #region Validar que exista la Conciliacion de Apertura
        RE_GenericBean Conciliacion_Apertura = (RE_GenericBean)DB.get_Conciliacion_Apertura(user, int.Parse(lb_bancos.SelectedValue), lb_banco_cuenta.SelectedItem.Text, int.Parse(lb_moneda.SelectedValue));
        if (Conciliacion_Apertura.intC1 == 0)
        {
            Response.Redirect("~/operations/conciliacion_apertura.aspx?id=" + lb_bancos.SelectedValue.ToString() + "&cta=" + lb_banco_cuenta.SelectedItem.Text + "");
        }
        #endregion
        #region Declaracion de Variables y Parametros
        DataTable dt_contabilidad = new DataTable();
        dt_contabilidad.Columns.Add("ID");
        dt_contabilidad.Columns.Add("TIPO");
        dt_contabilidad.Columns.Add("DOCUMENTO");
        dt_contabilidad.Columns.Add("FECHA");
        dt_contabilidad.Columns.Add("CREDITOS");
        dt_contabilidad.Columns.Add("DEBITOS");
        dt_contabilidad.Columns.Add("SALDO");
        dt_contabilidad.Columns.Add("ESTADO");
        dt_contabilidad.Columns.Add("ECH");//Estado Conciliado CH
        dt_contabilidad.Columns.Add("CONCILIADO");
        int Anio_Consultado = DateTime.Parse(DB.DateFormat(tb_fecha_inicial.Text)).Year;
        int Mes_Consultado = DateTime.Parse(DB.DateFormat(tb_fecha_inicial.Text)).Month;
        string Fecha_Inicial = DB.DateFormat(tb_fecha_inicial.Text);
        string Fecha_Final = DB.DateFormat(tb_fecha_final.Text);
        string Query0 = "tc_pai_id=" + user.PaisID + " and tc_tba_id=" + int.Parse(lb_bancos.SelectedValue) + " and tc_tbc_cuenta_bancaria='" + lb_banco_cuenta.SelectedItem.Text + "' and tc_tbc_mon_id=" + int.Parse(lb_moneda.SelectedValue.ToString()) + " and tc_fecha_inicial='" + Fecha_Inicial + "' and tc_fecha_final='" + Fecha_Final + "' and tc_ted_id=1";
        #endregion
        int ID_CONCILIACION = 0;
        ID_CONCILIACION = DB.Validar_Periodo_Conciliado(user, Query0);
        if (ID_CONCILIACION > 0)
        {
            #region Obtener Datos de la Conciliacion
            RE_GenericBean Conciliacion_Data = (RE_GenericBean)DB.Get_Conciliacion_Data(user,Fecha_Inicial,Fecha_Final,int.Parse(lb_bancos.SelectedValue), lb_banco_cuenta.SelectedItem.Text, int.Parse(lb_moneda.SelectedValue.ToString()));
            lbl_saldo_banco.Text = Conciliacion_Data.decC1.ToString("#,#.00#;(#,#.00#)");
            lbl_saldo_final_circulacion.Text = Conciliacion_Data.decC2.ToString("#,#.00#;(#,#.00#)");
            lbl_saldo_final_contabilidad.Text = Conciliacion_Data.decC3.ToString("#,#.00#;(#,#.00#)");
            lbl_total_creditos_contabilidad.Text = Conciliacion_Data.decC4.ToString("#,#.00#;(#,#.00#)");
            lbl_total_debitos_contabilidad.Text = Conciliacion_Data.decC5.ToString("#,#.00#;(#,#.00#)");
            lbl_total_creditos_circulacion.Text = Conciliacion_Data.decC6.ToString("#,#.00#;(#,#.00#)");
            lbl_total_debitos_circulacion.Text = Conciliacion_Data.decC7.ToString("#,#.00#;(#,#.00#)");
            lbl_saldo_anterior.Text = Conciliacion_Data.decC8.ToString();
            lbl_saldo_inicial.Text = Conciliacion_Data.decC8.ToString("#,#.00#;(#,#.00#)");
            #endregion
            #region Obtener Detalle Conciliacion
            ArrayList Arr_Conciliacion = (ArrayList)DB.getDetalleConciliacion(user, ID_CONCILIACION);
            foreach (RE_GenericBean Documento in Arr_Conciliacion)
            {
                object[] objArr = { Documento.strC1, Documento.strC2, Documento.strC3, Documento.strC4, Documento.strC5, Documento.strC6, Documento.strC7, Documento.strC8, Documento.strC9, Documento.strC10 };
                dt_contabilidad.Rows.Add(objArr);
            }
            gv_documentos_contabilidad.DataSource = dt_contabilidad;
            gv_documentos_contabilidad.DataBind();
            gv_documentos_contabilidad.Visible = true;
            #endregion
            gv_documentos_contabilidad.Columns[0].Visible = false;
            bt_guardar.Enabled = false;
            bt_preconciliar.Enabled = false;
            Marcar_Documentos();
        }
        else
        {
            ArrayList arr = null;
            arr = (ArrayList)DB.getDocumentosContabilidad(user, int.Parse(lb_bancos.SelectedValue), lb_banco_cuenta.SelectedValue, Fecha_Inicial, Fecha_Final);
            foreach (RE_GenericBean rgb in arr)
            {
                #region Definir Tipo
                string TIPO = rgb.strC4;
                if (TIPO == "NCB")
                {
                    TIPO = "21";
                }
                else if (TIPO == "NDB")
                {
                    TIPO = "20";
                }
                else if (TIPO == "DP")
                {
                    TIPO = "17";
                }
                else if (TIPO == "CH")
                {
                    TIPO = "5";
                }
                else if (TIPO == "TR")
                {
                    TIPO = "19";
                }
                else if (TIPO == "DPP")
                {
                    TIPO = "22";
                }
                #endregion
                #region Cargar Documentos de Contabilidad
                string Query = "a.tc_pai_id=" + user.PaisID + " and a.tc_tba_id=" + int.Parse(lb_bancos.SelectedValue) + " and a.tc_tbc_cuenta_bancaria='" + lb_banco_cuenta.SelectedItem.Text + "' and a.tc_tbc_mon_id=" + int.Parse(lb_moneda.SelectedValue.ToString()) + " and a.tc_ted_id=1 and b.tcd_id_documento=" + rgb.intC1 + " and b.tcd_tipo_documento=" + TIPO + "";
                if (rgb.strC4 == "NCB")
                {
                    object[] objArr = { rgb.intC1, rgb.strC4, rgb.strC1, rgb.strC2, rgb.strC3, "0.00", "0.00", rgb.intC2, "0", "F" };
                    dt_contabilidad.Rows.Add(objArr);
                }
                if (rgb.strC4 == "NDB")
                {
                    object[] objArr = { rgb.intC1, rgb.strC4, rgb.strC1, rgb.strC2, "0.00", rgb.strC3, "0.00", rgb.intC2, "0", "F" };// Si aun esta en circulacion
                    dt_contabilidad.Rows.Add(objArr);
                }
                if (rgb.strC4 == "DP")
                {
                    object[] objArr = { rgb.intC1, rgb.strC4, rgb.strC1, rgb.strC2, rgb.strC3, "0.00", "0.00", rgb.intC2, "0", "F" };
                    dt_contabilidad.Rows.Add(objArr);
                }
                if (rgb.strC4 == "CH")
                {
                    object[] objArr = { rgb.intC1, rgb.strC4, rgb.strC1, rgb.strC2, "0.00", rgb.strC3, "0.00", rgb.intC2, rgb.intC3, "F" };
                    dt_contabilidad.Rows.Add(objArr);
                }
                if (rgb.strC4 == "TR")
                {
                    object[] objArr = { rgb.intC1, rgb.strC4, rgb.strC1, rgb.strC2, "0.00", rgb.strC3, "0.00", rgb.intC2, rgb.intC3, "F" };
                    dt_contabilidad.Rows.Add(objArr);
                }
                if (rgb.strC4 == "DPP")
                {
                    object[] objArr = { rgb.intC1, rgb.strC4, rgb.strC1, rgb.strC2, rgb.strC3, "0.00", "0.00", rgb.intC2, "0", "F" };
                    dt_contabilidad.Rows.Add(objArr);
                }
                #endregion
            }
            gv_documentos_contabilidad.DataSource = dt_contabilidad;
            gv_documentos_contabilidad.DataBind();
            #region Calcular Saldo Inicial
            if (Conciliacion_Apertura.intC1 == 0)// Si nunca se ha Conciliado Tomar el Saldo Inicial de la Conciliacion de Apertura
            {
                lbl_saldo_anterior.Text = Conciliacion_Apertura.decC1.ToString();
                lbl_saldo_inicial.Text = (Conciliacion_Apertura.decC1).ToString("#,#.00#;(#,#.00#)");
            }
            else//Traer el Saldo en banco de la Ultima Conciliacion
            {
                saldo_inicial = DB.getSaldoInicialBanco(user, int.Parse(lb_bancos.SelectedValue), lb_banco_cuenta.SelectedValue, int.Parse(lb_moneda.SelectedValue), DB.DateFormat(tb_fecha_inicial.Text));
                lbl_saldo_anterior.Text = saldo_inicial.ToString();
                lbl_saldo_inicial.Text = (saldo_inicial).ToString("#,#.00#;(#,#.00#)");
            }
            #endregion
            gv_documentos_contabilidad.Columns[0].Visible = true;
            bt_guardar.Enabled = true;
            Marcar_Documentos();
            Calcular_Saldos();
            Calcular_Saldo_Detalle();
            Marcar_Documentos();
            Insertar_Saldo_Inicial();
            Marcar_Documentos();
            Marcar_Documentos_Preconciliados();
            #region Validar si el Periodo Anterior ya esta Conciliado
            DateTime Fecha_Anterior = new DateTime();
            Fecha_Anterior = DateTime.Parse(Fecha_Inicial).AddDays(-1);
            int ID_CONCILIACION_ANTERIOR = 0;
            int Cantidad_Documentos = DB.Validar_Periodo_Arranque_Conciliacion(user, DB.DateFormat(tb_fecha_inicial.Text), int.Parse(lb_bancos.SelectedValue), lb_banco_cuenta.SelectedItem.Text, int.Parse(lb_moneda.SelectedValue));
            string Query2 = "";
            Query2 = "tc_pai_id=" + user.PaisID + " and tc_tba_id=" + int.Parse(lb_bancos.SelectedValue) + " and tc_tbc_cuenta_bancaria='" + lb_banco_cuenta.SelectedItem.Text + "' and tc_tbc_mon_id=" + int.Parse(lb_moneda.SelectedValue.ToString()) + " and EXTRACT(YEAR FROM tc_fecha_inicial)='" + Fecha_Anterior.Year + "' and EXTRACT(MONTH FROM tc_fecha_inicial)='" + Fecha_Anterior.Month + "' and tc_ted_id=1";
            ID_CONCILIACION_ANTERIOR = DB.Validar_Periodo_Conciliado(user, Query2);
            DateTime Fecha_Apertura = DateTime.Parse(Conciliacion_Apertura.strC1);
            DateTime Fecha_Actual = DateTime.Parse(Fecha_Inicial);
            if ((ID_CONCILIACION_ANTERIOR == 0)&&(Fecha_Actual<=Fecha_Apertura))
            {
                gv_documentos_contabilidad.Visible = false;
                gv_documentos_contabilidad.Columns[0].Visible = false;
                bt_guardar.Enabled = false;
                btn_reporte.Enabled = false;
                lbl_saldo_banco.Text = "0.00";
                lbl_saldo_final_contabilidad.Text = "0.00";
                lbl_saldo_final_circulacion.Text = "0.00";
                lbl_total_creditos_contabilidad.Text = "0.00";
                lbl_total_debitos_contabilidad.Text = "0.00";
                lbl_total_creditos_circulacion.Text = "0.00";
                lbl_total_debitos_circulacion.Text = "0.00";
            }
            else if ((ID_CONCILIACION_ANTERIOR == 0) && (Fecha_Actual > Fecha_Apertura))
            {
                gv_documentos_contabilidad.Visible = true;
                gv_documentos_contabilidad.Columns[0].Visible = false;
                bt_guardar.Enabled = false;

                btn_reporte.Enabled = false;
                sScript = "alert('No se puede conciliar este mes, porque existe un periodo anterior pendiente de conciliar');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                           UpdatePanel2.GetType(),
                                           "BAW",
                                           sScript,
                                           true);
                return;
            }
            else if ((ID_CONCILIACION_ANTERIOR > 0) && (Fecha_Actual > Fecha_Apertura))
            {
                gv_documentos_contabilidad.Visible = true;
                gv_documentos_contabilidad.Columns[0].Visible = true;
                bt_guardar.Enabled = true;
                btn_reporte.Enabled = true;
            }
            #endregion
            bt_preconciliar.Enabled = true;
        }
    }
    protected void Calcular_Saldos()
    {
        decimal CREDITOS = 0;
        decimal DEBITOS = 0;
        decimal TOTAL_CREDITOS_CIRCULACION = 0;
        decimal TOTAL_DEBITOS_CIRCULACION = 0;
        decimal TOTAL_CREDITOS_CONTABILIDAD = 0;
        decimal TOTAL_DEBITOS_CONTABILIDAD = 0;
        decimal SALDO_FINAL_CONTABILIDAD = 0;
        decimal SALDO_FINAL_CIRCULACION = 0;
        decimal SALDO_EN_BANCO = 0;
        decimal SALDO_INICIAL = 0;
        GridViewRowCollection gv_contabilidad = gv_documentos_contabilidad.Rows;
        foreach (GridViewRow row in gv_contabilidad)
        {
            if ((row.ForeColor == System.Drawing.Color.Red)&&(row.Cells[1].Text!="0"))
            {
                CREDITOS += decimal.Parse(row.Cells[5].Text);
                DEBITOS += decimal.Parse(row.Cells[6].Text);
            }
        }
        TOTAL_CREDITOS_CIRCULACION = CREDITOS;
        TOTAL_DEBITOS_CIRCULACION = DEBITOS;
        SALDO_FINAL_CIRCULACION = TOTAL_DEBITOS_CIRCULACION - TOTAL_CREDITOS_CIRCULACION;
        CREDITOS = 0;
        DEBITOS = 0;
        foreach (GridViewRow row in gv_contabilidad)
        {
            if (row.Cells[1].Text != "0")
            {
                CREDITOS += decimal.Parse(row.Cells[5].Text);
                DEBITOS += decimal.Parse(row.Cells[6].Text);
            }
        }
        TOTAL_CREDITOS_CONTABILIDAD = CREDITOS;
        TOTAL_DEBITOS_CONTABILIDAD = DEBITOS;
        SALDO_INICIAL = decimal.Parse(lbl_saldo_anterior.Text);
        SALDO_FINAL_CONTABILIDAD = TOTAL_CREDITOS_CONTABILIDAD - TOTAL_DEBITOS_CONTABILIDAD;
        SALDO_FINAL_CONTABILIDAD += SALDO_INICIAL;
        SALDO_EN_BANCO = SALDO_FINAL_CONTABILIDAD + SALDO_FINAL_CIRCULACION;
        lbl_total_creditos_circulacion.Text = (TOTAL_CREDITOS_CIRCULACION).ToString("#,#.00#;(#,#.00#)");
        lbl_total_debitos_circulacion.Text = (TOTAL_DEBITOS_CIRCULACION).ToString("#,#.00#;(#,#.00#)");
        lbl_total_creditos_contabilidad.Text = (TOTAL_CREDITOS_CONTABILIDAD).ToString("#,#.00#;(#,#.00#)");
        lbl_total_debitos_contabilidad.Text = (TOTAL_DEBITOS_CONTABILIDAD).ToString("#,#.00#;(#,#.00#)");
        lbl_saldo_final_circulacion.Text = (SALDO_FINAL_CIRCULACION).ToString("#,#.00#;(#,#.00#)");
        lbl_saldo_final_contabilidad.Text = (SALDO_FINAL_CONTABILIDAD).ToString("#,#.00#;(#,#.00#)");
        lbl_saldo_banco.Text = (SALDO_EN_BANCO).ToString("#,#.00#;(#,#.00#)");
        Saldo_Banco = SALDO_EN_BANCO;
        Saldo_Final_Contabilidad = SALDO_FINAL_CONTABILIDAD;
        Saldo_Final_Circulacion = SALDO_FINAL_CIRCULACION;
        Total_Creditos_Contabilidad = TOTAL_CREDITOS_CONTABILIDAD;
        Total_Debitos_Contabilidad = TOTAL_DEBITOS_CONTABILIDAD;
        Total_Creditos_Circulacion = TOTAL_CREDITOS_CIRCULACION;
        Total_Debitos_Circulacion = TOTAL_DEBITOS_CIRCULACION;
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        string sScript = "";
        if (lb_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
        {
            sScript = "alert('Debe seleccionar una cuenta bancaria');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        if (tb_fecha_inicial.Text.Trim().Equals(""))
        {
            sScript = "alert('Porfavor seleccione el Perido a Conciliar');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        GridViewRowCollection gv_contabilidad = gv_documentos_contabilidad.Rows;
        int Cantidad_Documentos_a_Conciliar = 0;
        foreach (GridViewRow row in gv_contabilidad)
        {
            if (row.Cells[1].Text != "0")//Si no es la Fila de Saldo Inicial
            {
                if (row.Cells[10].Text != "T")//Si no esta Conciliado
                {
                    if (row.ForeColor == System.Drawing.Color.Black)//Si fue marcado para Conciliar
                    {
                        Cantidad_Documentos_a_Conciliar++;
                    }
                }
            }
        }
        int Cantidad_Documentos = 0;
        foreach (GridViewRow row in gv_contabilidad)
        {
            if (row.Cells[1].Text != "0")//Si no es la Fila de Saldo Inicial
            {
                Cantidad_Documentos++;
            }
        }
        /*if((Cantidad_Documentos>0)&&(Cantidad_Documentos_a_Conciliar<1))
        {
            sScript = "alert('No se ha seleccionado ningun Documento para Conciliar');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }*/
        #region Obtener Parametros
        Calcular_Saldos();
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.strC1 = user.ID;//Usuario
        rgb.intC1 = user.PaisID;//Pais
        rgb.intC2 = int.Parse(lb_bancos.SelectedValue);//Banco
        rgb.strC2 = lb_banco_cuenta.SelectedItem.Text;//Cuenta
        #region Formatear Fechas
        //Fecha Inicial
        string fecha_inicial = DB.DateFormat(tb_fecha_inicial.Text);
        string fecha_final = DB.DateFormat(tb_fecha_final.Text);
        #endregion
        rgb.strC3 = fecha_inicial;//Fecha Inicial
        rgb.strC4 = fecha_final;//Fecha Final
        rgb.intC3 = int.Parse(lb_moneda.SelectedValue);//Moneda
        rgb.decC1 = Saldo_Banco;//Saldo en Banco
        rgb.decC2 = Saldo_Final_Circulacion;//Saldo Circulacion
        rgb.decC3 = Saldo_Final_Contabilidad;//Saldo Contabilidad
        rgb.decC4 = Total_Creditos_Contabilidad;//Saldo Creditos Contabilidad
        rgb.decC5 = Total_Debitos_Contabilidad;//Saldo Debitos Contabilidad
        rgb.decC6 = Total_Creditos_Circulacion;//Saldo Creditos Circulacion
        rgb.decC7 = Total_Debitos_Circulacion;//Saldo Debitos Circulacion
        rgb.decC8 = decimal.Parse(lbl_saldo_anterior.Text);//Saldo Inicial
        rgb.boolC1 = false;
        #endregion
        #region Obtener Documentos a Conciliar
        RE_GenericBean Documentos_Bean = null;
        foreach (GridViewRow row in gv_contabilidad)
        {
            if (row.Cells[1].Text != "0")//Si no es la Fila de Saldo Inicial
            {
                if (row.Cells[10].Text != "T")//Si no esta Conciliado
                {
                    if (row.ForeColor == System.Drawing.Color.Black)//Si fue marcado para Conciliar
                    {
                        Documentos_Bean = new RE_GenericBean();
                        Documentos_Bean.intC1 = int.Parse(row.Cells[1].Text);//ID
                        Documentos_Bean.strC1 = row.Cells[2].Text;//TIPO
                        if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                        rgb.arr1.Add(Documentos_Bean);
                    }
                }
            }
        }
        #endregion
        #region Obtener Conciliacion
        RE_GenericBean Conciliacion_Bean = null;
        foreach(GridViewRow row in  gv_contabilidad)
        {
            if (row.Cells[1].Text != "0")//Si no es la Fila de Saldo Inicial
            {
                Conciliacion_Bean = new RE_GenericBean();
                Conciliacion_Bean.intC1 = int.Parse(row.Cells[1].Text);//ID
                Conciliacion_Bean.strC1 = row.Cells[2].Text;//TIPO
                Conciliacion_Bean.strC2 = row.Cells[3].Text;//DOCUMENTO
                Conciliacion_Bean.strC3 = row.Cells[4].Text;//FECHA
                Conciliacion_Bean.strC4 = row.Cells[5].Text;//CREDITOS
                Conciliacion_Bean.strC5 = row.Cells[6].Text;//DEBITOS
                Conciliacion_Bean.strC6 = row.Cells[7].Text;//SALDO
                Conciliacion_Bean.strC7 = row.Cells[8].Text;//TED ID
                if ((row.Cells[10].Text != "T") && (row.ForeColor == System.Drawing.Color.Black))
                {
                    Conciliacion_Bean.strC8 = "13";//ESTADO CONCILIADO
                    Conciliacion_Bean.strC9 = "T";//CONCILIADO
                }
                else
                {
                    Conciliacion_Bean.strC8 = row.Cells[9].Text;//ESTADO CONCILIADO
                    Conciliacion_Bean.strC9 = row.Cells[10].Text;//CONCILIADO
                }
                if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
                rgb.arr2.Add(Conciliacion_Bean);
            }
        }
        #endregion
        ArrayList result = DB.insertConciliacion(rgb, user);
        if (result != null && result.Count > 0)
        {
            sScript = "alert('La Conciliacion Bancaria fue realizada exitosamente');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            bt_guardar.Enabled = false;
            bt_visualizar.Enabled = false;
            gv_documentos_contabilidad.Enabled = false;
            tb_fecha_inicial.Enabled = false;
            lb_bancos.Enabled = false;
            lb_banco_cuenta.Enabled = false;
            return;
        }
        else
        {
            sScript = "alert('Existio un error al guardar la Conciliacion Bancaria');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
    }
    protected void Calcular_Saldo_Detalle()
    {
        GridViewRowCollection gv_contabilidad = gv_documentos_contabilidad.Rows;
        DataTable dt_contabilidad = new DataTable();
        dt_contabilidad.Columns.Add("ID");
        dt_contabilidad.Columns.Add("TIPO");
        dt_contabilidad.Columns.Add("DOCUMENTO");
        dt_contabilidad.Columns.Add("FECHA");
        dt_contabilidad.Columns.Add("CREDITOS");
        dt_contabilidad.Columns.Add("DEBITOS");
        dt_contabilidad.Columns.Add("SALDO");
        dt_contabilidad.Columns.Add("ESTADO");
        dt_contabilidad.Columns.Add("ECH");//Estado Conciliado CH
        dt_contabilidad.Columns.Add("CONCILIADO");
        decimal DEBE = 0;
        decimal HABER = 0;
        decimal SALDO_INICIAL = 0;
        decimal SALDO = 0;
        string FECHA = "";
        string FECHA_INICIAL="";
        string FECHA_FINAL="";
        SALDO_INICIAL = decimal.Parse(lbl_saldo_anterior.Text);
        FECHA_INICIAL = DB.DateFormat(tb_fecha_inicial.Text);
        FECHA_FINAL = DB.DateFormat(tb_fecha_final.Text);
        DateTime F_INICIAL = DateTime.Parse(FECHA_INICIAL);
        DateTime F_FINAL= DateTime.Parse(FECHA_FINAL);
        foreach (GridViewRow row in gv_contabilidad)
        {
            FECHA = row.Cells[4].Text;
            DateTime FECHA_DOCUMENTO= DateTime.Parse(FECHA);
            DEBE = 0;
            HABER = 0;
            if ((FECHA_DOCUMENTO >= F_INICIAL) && (FECHA_DOCUMENTO <= F_FINAL))
            {
                DEBE += decimal.Parse(row.Cells[5].Text);
                HABER += decimal.Parse(row.Cells[6].Text);
                SALDO = DEBE - HABER;
                SALDO_INICIAL += SALDO;
                object[] objArr = { Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), SALDO_INICIAL.ToString(), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
                dt_contabilidad.Rows.Add(objArr);
            }
            else
            {
                DEBE += decimal.Parse(row.Cells[5].Text);
                HABER += decimal.Parse(row.Cells[6].Text);
                SALDO = DEBE - HABER;
                SALDO_INICIAL += SALDO;
                object[] objArr = { Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), SALDO_INICIAL.ToString(), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
                dt_contabilidad.Rows.Add(objArr);
            }
        }
        gv_documentos_contabilidad.DataSource = dt_contabilidad;
        gv_documentos_contabilidad.DataBind();
    }
    protected void chk_contabilidad_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRowCollection gv_contabiliadad = gv_documentos_contabilidad.Rows;
        string FECHA = "";
        string FECHA_INICIAL = "";
        string FECHA_FINAL = "";
        FECHA_INICIAL = DB.DateFormat(tb_fecha_inicial.Text);
        FECHA_FINAL = DB.DateFormat(tb_fecha_final.Text);
        DateTime F_INICIAL = DateTime.Parse(FECHA_INICIAL);
        DateTime F_FINAL = DateTime.Parse(FECHA_FINAL);
        DataTable dt_contabilidad = new DataTable();
        dt_contabilidad.Columns.Add("ID");
        dt_contabilidad.Columns.Add("TIPO");
        dt_contabilidad.Columns.Add("DOCUMENTO");
        dt_contabilidad.Columns.Add("FECHA");
        dt_contabilidad.Columns.Add("CREDITOS");
        dt_contabilidad.Columns.Add("DEBITOS");
        dt_contabilidad.Columns.Add("SALDO");
        dt_contabilidad.Columns.Add("ESTADO");
        dt_contabilidad.Columns.Add("ECH");//Estado Conciliado CH
        dt_contabilidad.Columns.Add("CONCILIADO");
        foreach (GridViewRow row in gv_contabiliadad)
        {
            if (row.Cells[1].Text != "0")
            {
                FECHA = row.Cells[4].Text;
                DateTime FECHA_DOCUMENTO = DateTime.Parse(FECHA);
                if ((FECHA_DOCUMENTO >= F_INICIAL) && (FECHA_DOCUMENTO <= F_FINAL))
                {
                }
                else
                {
                    if (row.Cells[1].Text != "0")
                    {
                        row.BackColor = System.Drawing.Color.FromName("#E6E6E6");
                    }
                }
                string ID_CONTABILIDAD = row.Cells[1].Text;
                string TIPO_CONTABILIDAD = row.Cells[2].Text;
                string ESTADO_CONTABILIDAD = row.Cells[8].Text;
                string ECH = row.Cells[9].Text;
                if (ID_CONTABILIDAD != "0")
                {
                    if (row.ForeColor != System.Drawing.Color.Black)
                    {
                        bool result = ((CheckBox)row.FindControl("chk_contabilidad")).Checked;
                        if (result)
                        {
                            row.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else if (row.ForeColor == System.Drawing.Color.Black)
                    {
                        bool result = ((CheckBox)row.FindControl("chk_contabilidad")).Checked;
                        if (result)
                        {
                            if (TIPO_CONTABILIDAD == "CH")
                            {
                                if (ECH == "13")
                                {
                                    ((CheckBox)row.FindControl("chk_contabilidad")).Checked = false;
                                    return;
                                }
                                else
                                {
                                    if (ESTADO_CONTABILIDAD == "3")
                                    {
                                        row.ForeColor = System.Drawing.Color.Blue;
                                    }
                                    else
                                    {
                                        row.ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                            }
                            else
                            {
                                if (ECH == "13")
                                {
                                    ((CheckBox)row.FindControl("chk_contabilidad")).Checked = false;
                                    return;
                                }
                                else
                                {
                                    row.ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }
                    }
                }
                object[] objArr = { Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), Server.HtmlDecode(row.Cells[7].Text), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
                dt_contabilidad.Rows.Add(objArr);
                ((CheckBox)row.FindControl("chk_contabilidad")).Checked = false;
            }
            else
            {
                ((CheckBox)row.FindControl("chk_contabilidad")).Checked = false;
            }
        }
        Calcular_Saldos();
    }
    protected void gv_documentos_contabilidad_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[10].Visible = false;
        }
    }
    protected void tb_fecha_inicial_TextChanged(object sender, EventArgs e)
    {
        int anio= int.Parse(tb_fecha_inicial.Text.Substring(6, 4));
        int mes = int.Parse(tb_fecha_inicial.Text.Substring(0, 2))+1;
        DateTime Fecha_Banco = new DateTime();
        DateTime first = new DateTime();
        DateTime last = new DateTime();
        if (mes <= 12)
        {
            first = new DateTime(anio, mes, 1).AddMonths(-1);
            last = new DateTime(anio, mes, 1).AddDays(-1);
        }
        else
        {
            anio++;
            mes = 1;
            first = new DateTime(anio, mes, 1).AddMonths(-1);
            last = new DateTime(anio, mes, 1).AddDays(-1);
        }
        tb_fecha_inicial.Text = DateTime.Parse(first.ToShortDateString()).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        tb_fecha_final.Text = DateTime.Parse(last.ToShortDateString()).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        Fecha_Banco = first;
        Fecha_Banco = Fecha_Banco.AddDays(-1);
        lbl_fecha_banco.Text = Fecha_Banco.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        gv_documentos_contabilidad.DataBind();
        lbl_saldo_inicial.Text = "0.00";
        lbl_saldo_temporal.Text = "0";
        Calcular_Saldos();
    }

    protected void Marcar_Documentos_Preconciliados()
    {
        GridViewRowCollection gv_contabilidad = gv_documentos_contabilidad.Rows;
        DataTable dt_contabilidad = new DataTable();
        dt_contabilidad.Columns.Add("ID");
        dt_contabilidad.Columns.Add("TIPO");
        dt_contabilidad.Columns.Add("DOCUMENTO");
        dt_contabilidad.Columns.Add("FECHA");
        dt_contabilidad.Columns.Add("CREDITOS");
        dt_contabilidad.Columns.Add("DEBITOS");
        dt_contabilidad.Columns.Add("SALDO");
        dt_contabilidad.Columns.Add("ESTADO");
        dt_contabilidad.Columns.Add("ECH");//Estado Conciliado CH
        dt_contabilidad.Columns.Add("CONCILIADO");
        string ESTADO = "";
        string ECH = "";
        string TIPO = "";
        string FECHA = "";
        string FECHA_INICIAL = "";
        string FECHA_FINAL = "";
        string ID = "";
        bool preconsolidado = false;
        FECHA_INICIAL = DB.DateFormat(tb_fecha_inicial.Text);
        FECHA_FINAL = DB.DateFormat(tb_fecha_final.Text);
        /*ArrayList Arr_Fechas = (ArrayList)DB.Formatear_Fechas(fecha_inicial, fecha_final);
        fecha_inicial = Arr_Fechas[0].ToString();
        fecha_final = Arr_Fechas[1].ToString();*/
        foreach (GridViewRow row in gv_contabilidad)
        {
            ID = row.Cells[1].Text;
            TIPO = row.Cells[2].Text;
            preconsolidado = DB.getDocumentosPreconciliados(int.Parse(ID), TIPO,FECHA_INICIAL,FECHA_FINAL,lb_banco_cuenta.SelectedItem.ToString());

            if (preconsolidado == true)
            {
                row.ForeColor = System.Drawing.Color.Black;
            }
            object[] objArr = { Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), Server.HtmlDecode(row.Cells[7].Text), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
            dt_contabilidad.Rows.Add(objArr);
        }
    }
    
    protected void Marcar_Documentos()
    {
        GridViewRowCollection gv_contabilidad = gv_documentos_contabilidad.Rows;
        DataTable dt_contabilidad = new DataTable();
        dt_contabilidad.Columns.Add("ID");
        dt_contabilidad.Columns.Add("TIPO");
        dt_contabilidad.Columns.Add("DOCUMENTO");
        dt_contabilidad.Columns.Add("FECHA");
        dt_contabilidad.Columns.Add("CREDITOS");
        dt_contabilidad.Columns.Add("DEBITOS");
        dt_contabilidad.Columns.Add("SALDO");
        dt_contabilidad.Columns.Add("ESTADO");
        dt_contabilidad.Columns.Add("ECH");//Estado Conciliado CH
        dt_contabilidad.Columns.Add("CONCILIADO");
        string ESTADO = "";
        string ECH = "";
        string TIPO = "";
        string FECHA = "";
        string FECHA_INICIAL = "";
        string FECHA_FINAL = "";
        string ID = "";
        FECHA_INICIAL = DB.DateFormat(tb_fecha_inicial.Text);
        FECHA_FINAL = DB.DateFormat(tb_fecha_final.Text);
        DateTime F_INICIAL = DateTime.Parse(FECHA_INICIAL);
        DateTime F_FINAL = DateTime.Parse(FECHA_FINAL);
        foreach (GridViewRow row in gv_contabilidad)
        {
            ID = row.Cells[1].Text;
            if (ID != "0")
            {
                FECHA = row.Cells[4].Text;
                DateTime FECHA_DOCUMENTO = DateTime.Parse(FECHA);
                if ((FECHA_DOCUMENTO >= F_INICIAL) && (FECHA_DOCUMENTO <= F_FINAL))
                {
                }
                else
                {
                    if (row.Cells[1].Text != "0")
                    {
                        row.BackColor = System.Drawing.Color.FromName("#E6E6E6");
                    }
                }
                ESTADO = row.Cells[8].Text;
                ECH = row.Cells[9].Text;
                TIPO = row.Cells[2].Text;
                if (ECH == "13")
                {
                    row.ForeColor = System.Drawing.Color.Black;
                }
                if ((ESTADO == "3") && (ECH == "0"))
                {
                    row.ForeColor = System.Drawing.Color.Blue;
                }
                else if ((ESTADO != "3") && (ECH != "13"))
                {
                    row.ForeColor = System.Drawing.Color.Red;
                }
                object[] objArr = { Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), Server.HtmlDecode(row.Cells[7].Text), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
                dt_contabilidad.Rows.Add(objArr);
            }
            else
            {
                row.Font.Bold = true;
                row.Font.Italic = true;
                object[] objArr = { Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), Server.HtmlDecode(row.Cells[7].Text), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
                dt_contabilidad.Rows.Add(objArr);
            }
            //tooltip de CH y trasferencias.
            if (TIPO == "CH")
            {         
                //string criterio = "and tcg_id=" + int.Parse(row.Cells[1].Text);
                RE_GenericBean dataCH = new RE_GenericBean();
                dataCH = DB.getChequeDataforView(int.Parse(row.Cells[1].Text));
                ((CheckBox)row.FindControl("chk_contabilidad")).ToolTip = "Acreditado a: " + dataCH.strC4;
            }
            else if (TIPO == "TR")
            {
                RE_GenericBean dataTR = new RE_GenericBean();
                dataTR = DB.getChequeDataforView(int.Parse(row.Cells[1].Text));
                ((CheckBox)row.FindControl("chk_contabilidad")).ToolTip = "Nombre: " + dataTR.strC4;
            }
        }
    }
    protected void Insertar_Saldo_Inicial()
    {
        GridViewRowCollection gv_contabilidad = gv_documentos_contabilidad.Rows;
        DataTable dt_contabilidad = new DataTable();
        dt_contabilidad.Columns.Add("ID");
        dt_contabilidad.Columns.Add("TIPO");
        dt_contabilidad.Columns.Add("DOCUMENTO");
        dt_contabilidad.Columns.Add("FECHA");
        dt_contabilidad.Columns.Add("CREDITOS");
        dt_contabilidad.Columns.Add("DEBITOS");
        dt_contabilidad.Columns.Add("SALDO");
        dt_contabilidad.Columns.Add("ESTADO");
        dt_contabilidad.Columns.Add("ECH");//Estado Conciliado CH
        dt_contabilidad.Columns.Add("CONCILIADO");
        object[] objArr = { "0", "", "Saldo Inicial  ", "", "", "", lbl_saldo_anterior.Text, "0", "0", "0" };
        dt_contabilidad.Rows.Add(objArr);
        foreach(GridViewRow row in gv_contabilidad)
        {
            if (row.BackColor == System.Drawing.Color.FromName("#E6E6E6"))
            {
                object[] objArr2 = { Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), Server.HtmlDecode(row.Cells[7].Text), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
                dt_contabilidad.Rows.Add(objArr2);
            }
        }
        foreach (GridViewRow row in gv_contabilidad)
        {
            if (row.BackColor != System.Drawing.Color.FromName("#E6E6E6"))
            {
                object[] objArr2 = { Server.HtmlDecode(row.Cells[1].Text), Server.HtmlDecode(row.Cells[2].Text), Server.HtmlDecode(row.Cells[3].Text), Server.HtmlDecode(row.Cells[4].Text), Server.HtmlDecode(row.Cells[5].Text), Server.HtmlDecode(row.Cells[6].Text), Server.HtmlDecode(row.Cells[7].Text), Server.HtmlDecode(row.Cells[8].Text), Server.HtmlDecode(row.Cells[9].Text), Server.HtmlDecode(row.Cells[10].Text) };
                dt_contabilidad.Rows.Add(objArr2);
            }
        }
        gv_documentos_contabilidad.DataSource = dt_contabilidad;
        gv_documentos_contabilidad.DataBind();
    }
    protected void btn_reporte_Click(object sender, EventArgs e)
    {
        if ((tb_fecha_inicial.Text != "") && (tb_fecha_final.Text != "") && (lb_banco_cuenta.SelectedItem.Text != "Seleccione..."))
        {
            string Query = "tc_pai_id=" + user.PaisID + " and tc_tba_id=" + int.Parse(lb_bancos.SelectedValue) + " and tc_tbc_cuenta_bancaria='" + lb_banco_cuenta.SelectedItem.Text + "' and tc_tbc_mon_id=" + int.Parse(lb_moneda.SelectedValue.ToString()) + " and tc_fecha_inicial='" + DB.DateFormat(tb_fecha_inicial.Text) + "' and tc_fecha_final='" + DB.DateFormat(tb_fecha_final.Text) + "' and tc_ted_id=1 and tc_conciliacion_apertura=false ";
            int ID_CONCILIACION = DB.Validar_Periodo_Conciliado(user, Query);
            if (ID_CONCILIACION > 0)
            {
                string Script = "<script languaje=\"JavaScript\">";
                Script += "window.open('../Reports/Conciliacion_Bancaria.aspx?monID=" + lb_moneda.SelectedValue + "&fecha_inicial=" + tb_fecha_inicial.Text + "&fecha_final=" + tb_fecha_final.Text + "&bancoID=" + lb_bancos.SelectedValue + "&cuenta=" + lb_banco_cuenta.SelectedItem.Text + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
                Script += "</script>";
                Page.RegisterClientScriptBlock("closewindow", Script);
            }
            else
            {
                WebMsgBox.Show("No se puede Generar el Reporte sin haber Guardado la Conciliacion");
                return;
            }
        }
    }
    protected void btn_botar_conciliacion_Click(object sender, EventArgs e)
    {
        string sScript = "";
        if (lb_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
        {
            sScript = "alert('Debe seleccionar una cuenta bancaria');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        if (tb_fecha_inicial.Text.Trim().Equals(""))
        {
            sScript = "alert('Porfavor seleccione el Perido a Conciliar');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        //pnl_motivo.Visible = true;
        //btn_botar_conciliacion_ModalPopupExtender.Show();
        if (tb_motivo.Text == "")
        {
            sScript = "alert('Debe Ingresar el Motivo de Anulacion');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            btn_botar_conciliacion_ModalPopupExtender.Show();
            return;
        }
        int resultado = 0;
        DateTime Fecha_Posterior = new DateTime();
        string Fecha_Final = DB.DateFormat(tb_fecha_final.Text);
        Fecha_Posterior = DateTime.Parse(Fecha_Final).AddDays(1);
        //string Query = "tc_pai_id=" + user.PaisID + " and tc_tba_id=" + int.Parse(lb_bancos.SelectedValue) + " and tc_tbc_cuenta_bancaria='" + lb_banco_cuenta.SelectedItem.Text + "' and tc_tbc_mon_id=" + int.Parse(lb_moneda.SelectedValue.ToString()) + " and tc_fecha_inicial='" + DB.DateFormat(tb_fecha_inicial.Text) + "' and tc_fecha_final='" + DB.DateFormat(tb_fecha_final.Text) + "' and tc_ted_id=1 and tc_conciliacion_apertura=false ";
        string Query = "tc_pai_id=" + user.PaisID + " and tc_tba_id=" + int.Parse(lb_bancos.SelectedValue) + " and tc_tbc_cuenta_bancaria='" + lb_banco_cuenta.SelectedItem.Text + "' and tc_tbc_mon_id=" + int.Parse(lb_moneda.SelectedValue.ToString()) + " and tc_fecha_inicial='" + DB.DateFormat(tb_fecha_inicial.Text) + "' and tc_fecha_final='" + DB.DateFormat(tb_fecha_final.Text) + "' and tc_ted_id=1 ";
        int ID_CONCILIACION = DB.Validar_Periodo_Conciliado(user, Query);
        Query = "tc_pai_id=" + user.PaisID + " and tc_tba_id=" + int.Parse(lb_bancos.SelectedValue) + " and tc_tbc_cuenta_bancaria='" + lb_banco_cuenta.SelectedItem.Text + "' and tc_tbc_mon_id=" + int.Parse(lb_moneda.SelectedValue.ToString()) + " and EXTRACT(YEAR FROM tc_fecha_inicial)='" + Fecha_Posterior.Year + "' and EXTRACT(MONTH FROM tc_fecha_inicial)='" + Fecha_Posterior.Month + "' and tc_ted_id=1";
        int ID_CONCILIACION_POSTERIOR = DB.Validar_Periodo_Conciliado(user, Query);
        if (ID_CONCILIACION == 0)
        {
            sScript = "alert('No se puede botar la Conciliacion de un mes que no esta Conciliado');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            tb_motivo.Text = "";
            return;
        }
        else if ((ID_CONCILIACION>0)&&(ID_CONCILIACION_POSTERIOR>0))
        {
            sScript = "alert('No se puede botar la Conciliacion porque existe una Conciliacion posterior a este periodo');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        else if ((ID_CONCILIACION > 0) && (ID_CONCILIACION_POSTERIOR == 0))
        {
            resultado = DB.Botar_Conciliacion(user, ID_CONCILIACION, tb_motivo.Text.Trim());
            if (resultado == 1)
            {
                sScript = "alert('La Conciliacion fue botada exitosamente');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                           UpdatePanel2.GetType(),
                                           "BAW",
                                           sScript,
                                           true);
                bt_visualizar.Enabled = false;
                btn_botar_conciliacion.Enabled = false;
                btn_reporte.Enabled = false;
                lb_bancos.Enabled = false;
                lb_banco_cuenta.Enabled = false;
                tb_fecha_inicial.Enabled = false;
                tb_motivo.Text = "";
                pnl_motivo.Visible = false;
                return;
            }
            else if (resultado == -100)
            {
                sScript = "alert('Existio un problema al tratar de Botar la conciliacion');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                           UpdatePanel2.GetType(),
                                           "BAW",
                                           sScript,
                                           true);
                return;
            }
        }
    }

    protected void bt_preconciliar_Click(object sender, EventArgs e)
    {
        string sScript = "";
        if (lb_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
        {
            sScript = "alert('Debe seleccionar una cuenta bancaria');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        if (tb_fecha_inicial.Text.Trim().Equals(""))
        {
            sScript = "alert('Porfavor seleccione el Perido a Preconciliar');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        GridViewRowCollection gv_contabilidad = gv_documentos_contabilidad.Rows;
        int Cantidad_Documentos_a_Conciliar = 0;
        foreach (GridViewRow row in gv_contabilidad)
        {
            if (row.Cells[1].Text != "0")//Si no es la Fila de Saldo Inicial
            {
                if (row.Cells[10].Text != "T")//Si no esta Conciliado
                {
                    if (row.ForeColor == System.Drawing.Color.Black)//Si fue marcado para Conciliar
                    {
                        Cantidad_Documentos_a_Conciliar++;
                    }
                }
            }
        }
        int Cantidad_Documentos = 0;
        foreach (GridViewRow row in gv_contabilidad)
        {
            if (row.Cells[1].Text != "0")//Si no es la Fila de Saldo Inicial
            {
                Cantidad_Documentos++;
            }
        }
        /*if((Cantidad_Documentos>0)&&(Cantidad_Documentos_a_Conciliar<1))
        {
            sScript = "alert('No se ha seleccionado ningun Documento para Conciliar');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }*/
        #region Obtener Parametros
        Calcular_Saldos();
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.strC1 = user.ID;//Usuario
        rgb.intC1 = user.PaisID;//Pais
        rgb.intC2 = int.Parse(lb_bancos.SelectedValue);//Banco
        rgb.strC2 = lb_banco_cuenta.SelectedItem.Text;//Cuenta
        #region Formatear Fechas
        //Fecha Inicial
        string fecha_inicial = DB.DateFormat(tb_fecha_inicial.Text);
        string fecha_final = DB.DateFormat(tb_fecha_final.Text);
        #endregion
        rgb.strC3 = fecha_inicial;//Fecha Inicial
        rgb.strC4 = fecha_final;//Fecha Final
        rgb.intC3 = int.Parse(lb_moneda.SelectedValue);//Moneda
        rgb.decC1 = Saldo_Banco;//Saldo en Banco
        rgb.decC2 = Saldo_Final_Circulacion;//Saldo Circulacion
        rgb.decC3 = Saldo_Final_Contabilidad;//Saldo Contabilidad
        rgb.decC4 = Total_Creditos_Contabilidad;//Saldo Creditos Contabilidad
        rgb.decC5 = Total_Debitos_Contabilidad;//Saldo Debitos Contabilidad
        rgb.decC6 = Total_Creditos_Circulacion;//Saldo Creditos Circulacion
        rgb.decC7 = Total_Debitos_Circulacion;//Saldo Debitos Circulacion
        rgb.decC8 = decimal.Parse(lbl_saldo_anterior.Text);//Saldo Inicial
        rgb.boolC1 = false;
        #endregion
        #region Obtener Documentos a Conciliar
        RE_GenericBean Documentos_Bean = null;
        foreach (GridViewRow row in gv_contabilidad)
        {
            if (row.Cells[1].Text != "0")//Si no es la Fila de Saldo Inicial
            {
                if (row.Cells[10].Text != "T")//Si no esta Conciliado
                {
                    if (row.ForeColor == System.Drawing.Color.Black)//Si fue marcado para Conciliar
                    {
                        Documentos_Bean = new RE_GenericBean();
                        Documentos_Bean.intC1 = int.Parse(row.Cells[1].Text);//ID
                        Documentos_Bean.strC1 = row.Cells[2].Text;//TIPO
                        if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                        rgb.arr1.Add(Documentos_Bean);
                    }
                }
            }
        }
        #endregion
        #region Obtener Conciliacion
        RE_GenericBean Conciliacion_Bean = null;
        foreach (GridViewRow row in gv_contabilidad)
        {
            if (row.Cells[1].Text != "0")//Si no es la Fila de Saldo Inicial
            {
                Conciliacion_Bean = new RE_GenericBean();
                Conciliacion_Bean.intC1 = int.Parse(row.Cells[1].Text);//ID
                Conciliacion_Bean.strC1 = row.Cells[2].Text;//TIPO
                Conciliacion_Bean.strC2 = row.Cells[3].Text;//DOCUMENTO
                Conciliacion_Bean.strC3 = row.Cells[4].Text;//FECHA
                Conciliacion_Bean.strC4 = row.Cells[5].Text;//CREDITOS
                Conciliacion_Bean.strC5 = row.Cells[6].Text;//DEBITOS
                Conciliacion_Bean.strC6 = row.Cells[7].Text;//SALDO
                Conciliacion_Bean.strC7 = row.Cells[8].Text;//TED ID
                if ((row.Cells[10].Text != "T") && (row.ForeColor == System.Drawing.Color.Black))
                {
                    Conciliacion_Bean.strC8 = "0";//ESTADO CONCILIADO
                    Conciliacion_Bean.strC9 = "T";//PRECONCILIADO
                }
                else
                {
                    Conciliacion_Bean.strC8 = row.Cells[9].Text;//ESTADO CONCILIADO
                    Conciliacion_Bean.strC9 = row.Cells[10].Text;//CONCILIADO
                }
                if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
                rgb.arr2.Add(Conciliacion_Bean);
            }
        }
        #endregion
        ArrayList result = DB.insertPreConciliacion(rgb, user,fecha_inicial,fecha_final,lb_banco_cuenta.SelectedValue);
        if (result != null && result.Count > 0)
        {
            sScript = "alert('La Preconciliacion Bancaria fue realizada exitosamente');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            bt_guardar.Enabled = false;
            bt_visualizar.Enabled = false;
            gv_documentos_contabilidad.Enabled = false;
            tb_fecha_inicial.Enabled = false;
            lb_bancos.Enabled = false;
            lb_banco_cuenta.Enabled = false;
            return;
        }
        else
        {
            sScript = "alert('Existio un error al guardar la Preconciliacion Bancaria');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
    }
}
