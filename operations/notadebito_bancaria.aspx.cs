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

public partial class operations_Ajustes : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null, dt1 = null;
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
           // tb_fecha.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }
        //tb_fecha.Text = DateTime.Now.ToString("MM/dd/yyyy");
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
        Limpiar();
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
    protected void lb_banco_cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        Limpiar();
        if (!lb_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
        {
            string cuentaID = lb_banco_cuenta.SelectedValue;
            RE_GenericBean datoscuenta = (RE_GenericBean)DB.GetMonedaIDbyCuentaBancaria(cuentaID);
            lb_moneda.SelectedValue = datoscuenta.intC1.ToString();
        }
    }
    protected void bt_agregar_Click(object sender, EventArgs e)
    {
        decimal totalDebe = 0, totalHaber = 0;
        if ((DB.IsNumeric(tb_monto.Text) == true) && (decimal.Parse(tb_monto.Text.ToString()) > 0))
        {
            if (lb_banco_cuenta.SelectedItem.Text.Equals("Seleccione..."))
            {
                WebMsgBox.Show("Debe seleccionar el numero de cuenta");
                tb_cuenta.Text = "";
                tb_cuenta_nombre.Text = "";
                tb_monto.Text = "";
                return;
            }
            if (tb_cuenta.Text.Trim().Equals(""))
            {
                WebMsgBox.Show("Debe ingresar una cuenta contable");
                return;
            }
            if (tb_monto.Text.Trim().Equals(""))
            {
                WebMsgBox.Show("Debe ingresar el Monto");
                return;
            }
            #region Bloqueo de Cuentas Contables Madres
            //bool resultado = DB.Validar_Bloqueo_Cuenta_Contable(user, tb_cuenta.Text.Trim());
            //if (resultado == true)
            //{
            //    WebMsgBox.Show("No se puede afectar una Cuenta Contable madre");
            //    return;
            //}
            #endregion
            #region Bloquedo de Cuentas de Concentracion
            RE_GenericBean Cuenta_Bean = (RE_GenericBean)DB.getCtabyCtaID(tb_cuenta.Text.Trim());
            if ((Cuenta_Bean.intC2 >= 1) && (Cuenta_Bean.intC2 <= 2))//Nivel 1 al 3
            {
                tb_cuenta.Text = "";
                tb_cuenta_nombre.Text = "";
                tb_monto.Text = "";
                WebMsgBox.Show("No se puede Ajustar una cuenta contable de concentracion");
                return;
            }
            #endregion


            int Bloqueo = 0; //2021-02-02

            ArrayList Arr_Perfile = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
            foreach (PerfilesBean Perfiles in Arr_Perfile) //Arr_Perfiles_Creditos)
            {
                //Bloqueo cuentas       spuer user          admin
                if (Perfiles.ID == 43 || Perfiles.ID == 7 || Perfiles.ID == 8)
                {
                    Bloqueo = 1;
                }
            }  

            #region Bloqueo de Cuentas especificas
            //if ((user.ID != "dennis-ariana") && (user.ID != "jose-cruz") && (user.ID != "omar-contreras") && (user.ID != "rene-aguirre"))
            if (Bloqueo == 0)
            {
                if ((tb_cuenta.Text == "2.1.1.1.0001") || (tb_cuenta.Text == "1.1.2.2.0001"))
                {
                    tb_cuenta.Text = "";
                    tb_cuenta_nombre.Text = "";
                    tb_monto.Text = "";
                    WebMsgBox.Show("No puede Ajustar esta cuenta contable");
                    return;
                }
            }
            #endregion
            DataTable dt_temp = new DataTable();
            dt_temp.Columns.Add("Cuenta ID");
            dt_temp.Columns.Add("Cuenta Nombre");
            dt_temp.Columns.Add("Debe");
            dt_temp.Columns.Add("Haber");
            dt_temp.Columns.Add("Bandera");
            GridViewRowCollection gvr = gv_cuentas.Rows;
            dt_temp.Clear();
            foreach (GridViewRow row in gvr)
            {
                object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text };
                dt_temp.Rows.Add(objArr);
                totalDebe += decimal.Parse(row.Cells[3].Text);
                totalHaber += decimal.Parse(row.Cells[4].Text);
            }
            if (lb_ctade.SelectedValue.Equals("Debe"))
            {
                object[] objArr = { tb_cuenta.Text.Trim(), tb_cuenta_nombre.Text.Trim(), tb_monto.Text.Trim(), "0.00", "0" };
                dt_temp.Rows.Add(objArr);
                totalDebe += decimal.Parse(tb_monto.Text.Trim());
            }
            else if (lb_ctade.SelectedValue.Equals("Haber"))
            {
                object[] objArr = { tb_cuenta.Text.Trim(), tb_cuenta_nombre.Text.Trim(), "0.00", tb_monto.Text.Trim(), "0" };
                dt_temp.Rows.Add(objArr);
                totalHaber += decimal.Parse(tb_monto.Text.Trim());
            }
            gv_cuentas.DataSource = dt_temp;
            gv_cuentas.DataBind();
            tb_cuenta.Text = "";
            tb_cuenta_nombre.Text = "";
            tb_monto.Text = "0.00";
        }
        else
        {
            tb_monto.Text = "0.00";
        }
        Calcular_Totales();
    }
    protected void gv_cuentas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        decimal totalDebe = 0, totalHaber = 0;
        int Contador = 0;
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Debe");
        dt_temp.Columns.Add("Haber");
        dt_temp.Columns.Add("Bandera");
        GridViewRowCollection gvr = gv_cuentas.Rows;
        dt_temp.Clear();
        foreach (GridViewRow row in gvr)
        {
            object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text };
            dt_temp.Rows.Add(objArr);
            if (row.Cells[5].Text == "1")
            {
                tb_valor.Text = "0.00";
            }
            Contador++;
        }
        dt_temp.Rows[e.RowIndex].Delete();
        gv_cuentas.DataSource = dt_temp;
        gv_cuentas.DataBind();
        gvr = gv_cuentas.Rows;
        Calcular_Totales();
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        decimal totalDebe = 0, totalHaber = 0;
        if (tb_credito_no.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar el numero de Credito");
            return;
        }
        if (tb_fecha.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la fecha de la Nota de Debito");
            return;
        }
        if (tb_motivo.Text.Equals(""))
        {
            WebMsgBox.Show("Es necesario que ingrese el motivo de la Nota de Debito Bancaria");
            return;
        }
        if (gv_cuentas.Rows.Count < 2)
        {
            WebMsgBox.Show("Debe ingresar al menos una cuenta en el Debe y el Haber");
            return;
        }
        #region Validar que Exista al menos una cuenta de Cargo y una de Abono
        GridViewRowCollection gvr = gv_cuentas.Rows;
        int ban_cts_cargo = 0;
        int ban_cts_abono = 0;
        foreach (GridViewRow roww in gvr)
        {
            if (roww.Cells[3].Text != "0.00")
            {
                ban_cts_cargo++;
                totalDebe += decimal.Parse(roww.Cells[3].Text);
            }
            else
            {
                ban_cts_abono++;
                totalHaber += decimal.Parse(roww.Cells[4].Text);
            }
        }
        if (ban_cts_cargo == 0)
        {
            WebMsgBox.Show("Debe ingresar al menos una Cuenta Contable en el Debe");
            return;
        }
        if (ban_cts_abono == 0)
        {
            WebMsgBox.Show("Debe ingresar al menos una Cuenta Contable en el Haber");
            return;
        }
        if ((totalDebe > totalHaber) || (totalDebe < totalHaber))
        {
            WebMsgBox.Show("Revise su transaccion, El total del Debe no es igual al del Haber");
            return;
        }
        #endregion
        if (tb_valor.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Ingresar el valor de Nota de Debito");
            return;
        }
        else
        {
            if ((decimal.Parse(tb_valor.Text.Trim()) > decimal.Parse(tb_total_debe.Text.Trim())) || (decimal.Parse(tb_valor.Text.Trim()) < decimal.Parse(tb_total_debe.Text.Trim())))
            {
                WebMsgBox.Show("Revise el valor de la Nota de Debito porque no es igual al Total del Debe y el Haber");
                return;
            }
        }
        //*************************************************************BLOQUEO DE PERIODO*****************************************
        #region bloqueo periodo
        RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
        DateTime hoy = DateTime.Today; //fecha del dia de hoy
        int activo = bloqueo.intC3;
        #region formateo fechas
        DateTime fecha_doc = DateTime.Parse(DB.DateFormat(tb_fecha.Text.ToString().Substring(0, 10)));
        #endregion
        if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
        {
            WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0, 10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10));
            return;
        }
        if (fecha_doc > DateTime.Today)
        {
            WebMsgBox.Show("No puede ingresar fechas Adelantadas a la fecha de hoy");
            return;
        }
        #endregion
        #region Obtener Parametros
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.intC1 = int.Parse(lb_bancos.SelectedValue);//Banco ID
        rgb.strC1 = lb_banco_cuenta.SelectedValue;//No Cuenta Bancaria
        rgb.intC2 = int.Parse(lb_moneda.SelectedValue);//Moneda
        rgb.strC2 = tb_credito_no.Text.Trim();//Credito No
        rgb.douC1 = double.Parse(tb_valor.Text);//Valor
        rgb.strC3 = tb_motivo.Text.Trim();//Motivo
        rgb.strC4 = tb_descripcion.Text.Trim();//Descripcion
        rgb.intC3 = user.PaisID;//Pais ID
        rgb.strC5 = user.ID;//Usu_id
        rgb.intC4 = 1;//Estado Documento
        rgb.strC6 = DB.DateFormat(tb_fecha.Text);// Fecha de la NDB
        #endregion
        #region Obtener Cuentas
        GridViewRow row;
        RE_GenericBean ctasbean = null;
        for (int i = 0; i < gv_cuentas.Rows.Count; i++)
        {
            row = gv_cuentas.Rows[i];
            ctasbean = new RE_GenericBean();
            if (row.RowType == DataControlRowType.DataRow)
            {
                ctasbean.strC1 = row.Cells[1].Text;//id de la cuenta
                ctasbean.decC1 = decimal.Parse(row.Cells[3].Text);//Debe
                ctasbean.decC2 = decimal.Parse(row.Cells[4].Text);//Haber
                if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                rgb.arr1.Add(ctasbean);
            }
        }
        #endregion
        ArrayList result = DB.insertNDBancaria(rgb, user);
        if (result != null && result.Count > 0)
        {
            WebMsgBox.Show("Se guardo exitosamente la Nota de Debito Bancaria");
            bt_guardar.Enabled = false;
            return;
        }
        else
        {
            WebMsgBox.Show("Existio un error al guardar la Nota de Debito Bancaria");
            return;
        }
    }
    protected void Calcular_Totales()
    {
        decimal totalDebe = 0, totalHaber = 0;
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Debe");
        dt_temp.Columns.Add("Haber");
        dt_temp.Columns.Add("Bandera");
        GridViewRowCollection gvr = gv_cuentas.Rows;
        dt_temp.Clear();
        foreach (GridViewRow row in gvr)
        {
            object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text };
            dt_temp.Rows.Add(objArr);
            totalDebe += decimal.Parse(row.Cells[3].Text);
            totalHaber += decimal.Parse(row.Cells[4].Text);
        }
        tb_total_debe.Text = totalDebe.ToString("#,#.00#;(#,#.00#)");
        tb_total_haber.Text = totalHaber.ToString("#,#.00#;(#,#.00#)");
    }

    protected void btn_nueva_Click(object sender, EventArgs e)
    {
        tb_monto.Text = "0.00";
        tb_valor.Text = "0.00";
        tb_cuenta.Text = "";
        tb_cuenta_nombre.Text = "";
        tb_motivo.Text = "";
        tb_descripcion.Text = "";
        tb_credito_no.Text = "";
        tb_fecha.Text = "";
        tb_total_debe.Text = "0.00";
        tb_total_haber.Text = "0.00";
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Debe");
        dt_temp.Columns.Add("Haber");
        dt_temp.Columns.Add("Bandera");
        dt_temp.Clear();
        gv_cuentas.DataSource = dt_temp;
        gv_cuentas.DataBind();
        lb_bancos_SelectedIndexChanged(sender, e);
        bt_guardar.Enabled = true;
    }
    protected void tb_valor_TextChanged(object sender, EventArgs e)
    {
        string cuentaID = "";
        string cuentaNombre = "";
        decimal totalDebe = 0, totalHaber = 0;
        int bandera = 0;
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Debe");
        dt_temp.Columns.Add("Haber");
        dt_temp.Columns.Add("Bandera");
        GridViewRowCollection gvr = gv_cuentas.Rows;
        dt_temp.Clear();
        if (lb_banco_cuenta.SelectedValue.ToString() == " ")
        {
            btn_nueva_Click(sender, e);
            WebMsgBox.Show("Debe seleccionar una Cuenta Bancaria");
            return;
        }
        if ((lb_bancos.SelectedValue.ToString() == "27") && (lb_banco_cuenta.SelectedValue.ToString() == "173-301-000002287"))
        {
            cuentaNombre = "BANCO APL";
            cuentaID = "1.1.1.2.0001";
        }
        else
        {
            cuentaNombre = "BANCOS";
            cuentaID = "1.1.1.2.0000";
        }
        if (DB.IsNumeric(tb_valor.Text) == true)
        {

            if ((tb_valor.Text.Length < 1) || (tb_valor.Text.Trim().Equals("0")) || (tb_valor.Text.Trim().Equals("0.00")))
            {
                tb_valor.Text = "0.00";
                #region Eliminar Cuenta Contable de Bancos Automatica
                foreach (GridViewRow row in gvr)
                {
                    if (row.Cells[5].Text == "1")
                    {
                    }
                    else
                    {
                        object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text };
                        dt_temp.Rows.Add(objArr);
                    }
                }
                gv_cuentas.DataSource = dt_temp;
                gv_cuentas.DataBind();
                #endregion
            }
            else
            {
                #region Validar si Existe Cuenta Contable de Bancos Automatica
                dt_temp.Clear();
                foreach (GridViewRow row in gvr)
                {
                    if (row.Cells[5].Text == "1")
                    {
                        bandera = 1;
                        object[] objArr = { cuentaID, cuentaNombre, row.Cells[3].Text, tb_valor.Text.Trim(), row.Cells[5].Text };
                        dt_temp.Rows.Add(objArr);
                    }
                    else
                    {
                        object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text };
                        dt_temp.Rows.Add(objArr);
                    }
                }
                gv_cuentas.DataSource = dt_temp;
                gv_cuentas.DataBind();
                #endregion
                if (bandera == 0)
                {
                    #region Agregar Valor por Default en Debe
                    dt_temp.Clear();
                    foreach (GridViewRow row in gvr)
                    {
                        object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text };
                        dt_temp.Rows.Add(objArr);
                    }
                    object[] objArr_new = { cuentaID, cuentaNombre, "0.00", tb_valor.Text.Trim(), "1" };
                    dt_temp.Rows.Add(objArr_new);
                    totalHaber += decimal.Parse(tb_valor.Text.Trim());
                    gv_cuentas.DataSource = dt_temp;
                    gv_cuentas.DataBind();
                    tb_cuenta.Text = "";
                    tb_cuenta_nombre.Text = "";
                    tb_monto.Text = "0.00";
                    #endregion
                }
            }
        }
        else
        {
            #region Eliminar Cuenta Contable de Bancos Automatica
            foreach (GridViewRow row in gvr)
            {
                if (row.Cells[5].Text == "1")
                {
                }
                else
                {
                    object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text };
                    dt_temp.Rows.Add(objArr);
                }
            }
            gv_cuentas.DataSource = dt_temp;
            gv_cuentas.DataBind();
            #endregion
            tb_valor.Text = "0.00";
            return;
        }
        Calcular_Totales();
    }
    protected void gv_cuentas_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 0)
        {
            e.Row.Cells[5].Visible = false;
        }
    }
    protected void tb_monto_TextChanged(object sender, EventArgs e)
    {
        if (DB.IsNumeric(tb_monto.Text) == false)
        {
            tb_monto.Text = "0.00";
            return;
        }
    }
    protected void Limpiar()
    {
        tb_monto.Text = "0.00";
        tb_valor.Text = "0.00";
        tb_cuenta.Text = "";
        tb_cuenta_nombre.Text = "";
        tb_motivo.Text = "";
        tb_descripcion.Text = "";
        tb_credito_no.Text = "";
        tb_fecha.Text = "";
        tb_total_debe.Text = "0.00";
        tb_total_haber.Text = "0.00";
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Cuenta ID");
        dt_temp.Columns.Add("Cuenta Nombre");
        dt_temp.Columns.Add("Debe");
        dt_temp.Columns.Add("Haber");
        dt_temp.Columns.Add("Bandera");
        dt_temp.Clear();
        gv_cuentas.DataSource = dt_temp;
        gv_cuentas.DataBind();
        bt_guardar.Enabled = true;
    }
}
