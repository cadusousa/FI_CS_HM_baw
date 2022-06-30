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
    decimal Creditos_Operados = 0;
    decimal Debitos_Operados = 0;
    decimal Creditos_Circulacion = 0;
    decimal Debitos_Circulacion = 0;
    decimal Saldo_Contabilidad = 0;
    decimal Saldo_Circulacion = 0;
    decimal Saldo_Banco = 0;
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
        string Cuenta = Request.QueryString["cta"].ToString();
        if (!Page.IsPostBack)
        {
            obtengo_listas();
            lb_bancos_SelectedIndexChanged(sender, e);
            lb_banco_cuenta_SelectedIndexChanged(sender, e);
            lb_banco_cuenta.SelectedValue = Cuenta;
            lb_banco_cuenta_SelectedIndexChanged(sender, e);
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
        string Banco_ID = Request.QueryString["id"].ToString();
        lb_bancos.SelectedValue = Banco_ID;
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
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue), user.PaisID,1);
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
        }
    }
    protected void tb_fecha_inicial_TextChanged(object sender, EventArgs e)
    {
        if (tb_fecha_inicial.Text != "")
        {
            int anio = int.Parse(tb_fecha_inicial.Text.Substring(6, 4));
            int mes = int.Parse(tb_fecha_inicial.Text.Substring(0, 2)) + 1;
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
            int Cantidad_Documentos = DB.Validar_Periodo_Arranque_Conciliacion(user, DB.DateFormat(tb_fecha_final.Text), int.Parse(lb_bancos.SelectedValue), lb_banco_cuenta.SelectedItem.Text, int.Parse(lb_moneda.SelectedValue));
            if (Cantidad_Documentos > 0)
            {
                btn_grabar.Enabled = false;
                tb_fecha_inicial.Text = "";
                tb_fecha_final.Text = "";
                WebMsgBox.Show("El Periodo de Apertura debe ser menor al de los documentos ingresados en contabilidad");
                return;
            }
            else
            {
                btn_grabar.Enabled = true;
            }
        }
    }
    protected void Calcular_Saldo_Bancario()
    {
        Creditos_Operados = decimal.Parse(tb_creditos_operados.Text);
        Debitos_Operados = decimal.Parse(tb_debitos_operados.Text);
        Creditos_Circulacion = decimal.Parse(tb_creditos_circulacion.Text);
        Debitos_Circulacion = decimal.Parse(tb_debitos_circulacion.Text);
        Saldo_Contabilidad = Creditos_Operados - Debitos_Operados;
        Saldo_Circulacion = Debitos_Circulacion - Creditos_Circulacion;
        Saldo_Banco = Saldo_Contabilidad + Saldo_Circulacion;
        tb_saldo_contabilidad.Text = Saldo_Contabilidad.ToString();
        tb_saldo_circulacion.Text = Saldo_Circulacion.ToString();
        tb_saldo_banco.Text = Saldo_Banco.ToString();
    }
    protected void btn_grabar_Click(object sender, EventArgs e)
    {
        Calcular_Saldo_Bancario();
        #region Obtener Parametros
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
        rgb.decC2 = Saldo_Circulacion;//Saldo Circulacion
        rgb.decC3 = Saldo_Contabilidad;//Saldo Contabilidad
        rgb.decC4 = Creditos_Operados;//Saldo Creditos Contabilidad
        rgb.decC5 = Debitos_Operados;//Saldo Debitos Contabilidad
        rgb.decC6 = Creditos_Circulacion;//Saldo Creditos Circulacion
        rgb.decC7 = Debitos_Circulacion;//Saldo Debitos Circulacion
        rgb.decC8 = 0;//Saldo Inicial
        rgb.boolC1 = true;
        #endregion
        ArrayList result = DB.insertConciliacion(rgb, user);
        if (result != null && result.Count > 0)
        {
            WebMsgBox.Show("La Apertura de Saldos fue realizada exitosamente");
            btn_grabar.Enabled = false;
            Response.Redirect("~/operations/conciliaciones_bancarias.aspx");
            return;
        }
        else
        {
            WebMsgBox.Show("Existio un error al Aperturar los Saldos");
            return;
        }
    }
    protected void tb_creditos_operados_TextChanged(object sender, EventArgs e)
    {
        if (tb_creditos_operados.Text.Trim().Equals(""))
        {
            tb_creditos_operados.Text = "0.00";
        }
        Calcular_Saldo_Bancario();
    }
    protected void tb_debitos_operados_TextChanged(object sender, EventArgs e)
    {
        if (tb_debitos_operados.Text.Trim().Equals(""))
        {
            tb_debitos_operados.Text = "0.00";
        }
        Calcular_Saldo_Bancario();
    }
    protected void tb_debitos_circulacion_TextChanged(object sender, EventArgs e)
    {
        if (tb_debitos_circulacion.Text.Trim().Equals(""))
        {
            tb_debitos_circulacion.Text = "0.00";
        }
        Calcular_Saldo_Bancario();
    }
    protected void tb_creditos_circulacion_TextChanged(object sender, EventArgs e)
    {
        if (tb_creditos_circulacion.Text.Trim().Equals(""))
        {
            tb_creditos_circulacion.Text = "0.00";
        }
        Calcular_Saldo_Bancario();
    }
}
