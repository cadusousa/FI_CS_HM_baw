using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class operations_pago_comision : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ListItem item = null;
    DataTable dt = null;
    DataTable dt1 = new DataTable();
    RE_GenericBean rgb = null;
    int total_cortes = 0, total_cheques;
    decimal total_corte = 0, total_cheque=0;
    int bandera = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        else
        {
            user = (UsuarioBean)Session["usuario"];
        }
        ArrayList Arr_Perfile = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        foreach (PerfilesBean Perfiles in Arr_Perfile)
        {
            if ((Perfiles.ID == 26))
            {
                bandera = 1;
            }
        }
        if (bandera == 0)
        {
            Response.Redirect("../default.aspx");
        }

        if (!Page.IsPostBack)
        {
            item = new ListItem("Seleccione...", "0");
            ddl_vendedores.Items.Add(item);
            ddl_bancos.Items.Add(item);
            ddl_cuenta.Items.Add(item);
            item = null;
            Obtengo_vendedores();
        }
    }
    public void Obtengo_vendedores()
    {
        string criterio = " and tipo_usuario=1 and pais='" + user.pais.ISO + "' and pw_activo <> 0 ";
        ArrayList arr = (ArrayList)DB.getProveedor_cajachica(criterio);
        foreach (RE_GenericBean vendedor in arr)
        {
            item = new ListItem(vendedor.strC2, vendedor.intC1.ToString());
            ddl_vendedores.Items.Add(item);
        }
        arr = (ArrayList)DB.getBancos_comision(user);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            ddl_bancos.Items.Add(item);
        }
     
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if  ((tb_chequeNo.Text == "") || (tbx_monto.Text == "") || (tb_acreditado.Text == "") || (tb_motivo.Text == "") || (tb_observaciones.Text == "") )
        {
            WebMsgBox.Show("Debe de llenar todos los campos del cheque.");
            return;
        }
        dt1.Columns.Add("Numero");
        dt1.Columns.Add("Banco");
        dt1.Columns.Add("Cuenta");
        dt1.Columns.Add("Monto");
        dt1.Columns.Add("Acreditado");
        dt1.Columns.Add("motivo");
        dt1.Columns.Add("fecha");
        dt1.Columns.Add("descripcion");

        String hoy = DateTime.Now.ToString("dd-MM-yyyy");
        Object[] abjArr = { tb_chequeNo.Text.ToString().Trim(), ddl_bancos.SelectedValue.ToString(), ddl_cuenta.SelectedValue.ToString(), tbx_monto.Text.ToString().Trim(),tb_acreditado.Text.ToString().Trim(), tb_motivo.Text.ToString().Trim(),hoy, tb_observaciones.Text.ToString().Trim()};
        dt1.Rows.Add(abjArr);
        gv_cheques.DataSource = dt1;
        gv_cheques.DataBind();
    }
    protected void ddl_vendedores_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_acreditado.Text = ddl_vendedores.SelectedItem.ToString();
        gv_cortes.DataBind();
        dt = DB.ObtengoCorteComision(int.Parse(ddl_vendedores.SelectedValue.ToString()), user);
        if (dt == null)
        {
            lbl_notifica.Visible = true;
            lbl_notifica.Text = "Este vendedor no tiene cortes comision generados";
            return;
        }
        gv_cortes.DataSource = dt;
        gv_cortes.DataBind();
        if (gv_cortes.Rows.Count > 0)
        {
            btn_agregar_chk.Enabled = true;
        }


    }
    protected void btn_pagar_Click(object sender, EventArgs e)
    {

        RE_GenericBean rgb = null;
        ArrayList arr_cortes_id = new ArrayList();
        ArrayList arr_cheques_id = new ArrayList();
        if (gv_cortes.Rows.Count == 0)
        {
            WebMsgBox.Show("Deben existir cortes para generar un pago");
            return;
        }
        if (gv_cheques.Rows.Count == 0)
        {
            WebMsgBox.Show("Se debe agregar un cheque antes de realizar un pago");
            return;
        }
        //sumando total cortes
        if (gv_cortes.Rows.Count > 0)
        {
            CheckBox chk_cortes;
            total_cortes = 0;
            foreach (GridViewRow gvr in gv_cortes.Rows)
            {
                rgb = new RE_GenericBean();

                chk_cortes = (CheckBox)gvr.FindControl("chb_cortes");
                if (chk_cortes.Checked)
                {
                    total_cortes++;
                    total_corte += decimal.Parse(gvr.Cells[6].Text);//total_corte
                    rgb.intC1 = int.Parse(gvr.Cells[1].Text);//id corte
                    rgb.decC1 = decimal.Parse(gvr.Cells[6].Text); //total por cada corte
                    arr_cortes_id.Add(rgb);
                }
            }
        }
        if (gv_cheques.Rows.Count > 0)
        {
            total_cheques = 0;
            CheckBox chk_cheques;
            foreach (GridViewRow gvcr in gv_cheques.Rows)
            {
                rgb = new RE_GenericBean();
                chk_cheques = (CheckBox)gvcr.FindControl("chb_cheques");
                if (chk_cheques.Checked)
                {
                    total_cheques++;
                    total_cheque += decimal.Parse(gvcr.Cells[4].Text);//total cheques
                    rgb.intC1 = int.Parse(gvcr.Cells[1].Text); //numero
                    rgb.strC1 = gvcr.Cells[2].Text; //banco
                    rgb.strC2 = gvcr.Cells[3].Text; //cuenta
                    rgb.decC1 = decimal.Parse(gvcr.Cells[4].Text); //monto
                    rgb.strC3 = gvcr.Cells[5].Text; //acreditado
                    rgb.strC4 = gvcr.Cells[6].Text; //motivo
                    rgb.strC5 = gvcr.Cells[8].Text; //descripcion
                    rgb.intC2 = int.Parse(ddl_vendedores.SelectedValue.ToString()); //vendedor id
                    String hoy = DateTime.Now.ToString("yyyy-MM-dd");
                    decimal tc = DB.getTipoCambioByDay(user, hoy);
                    decimal total_eq = decimal.Parse(gvcr.Cells[4].Text);
                    if (user.Moneda == 8)
                    {
                        rgb.decC2 = rgb.decC1 * tc;
                    }
                    else if (user.Moneda != 8)
                    {
                        rgb.decC2 = rgb.decC1 / tc;
                    }
                    arr_cheques_id.Add(rgb);
                }
            }
        }
        if (total_cheque == 0)
        {
            WebMsgBox.Show("No se ha seleccionado ningun cheque");
            return;
        }
        if (total_cortes == 0) 
        {
            WebMsgBox.Show("No se ha seleccionado ningun corte");
            return;
        }
        decimal saldo = total_corte - total_cheque;
        if (saldo == 0)
        {

        }
        else 
        {
            WebMsgBox.Show("El cheque " + total_cheque + " no mata la suma total de los cortes " + total_corte + ", por lo que queda un saldo pendiente de " + saldo + " ");
            return;
        }
       // btn_pagar.Enabled = false;
        int resultado = DB.PagoCorteComision(user, arr_cheques_id, arr_cortes_id);
        if (resultado > 0)
        {
            WebMsgBox.Show("Pago Realizado");
        }
        else
        {
            WebMsgBox.Show("Existio un error al momento de realizar el pago");
        }
        btn_pagar.Enabled = false;
        btn_agregar_chk.Enabled = false;   
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        gv_cheques.DataBind();
    }
    protected void ddl_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_cuenta.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        ddl_cuenta.Items.Add(item);
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_comisiones(int.Parse(ddl_bancos.SelectedValue), user);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            ddl_cuenta.Items.Add(item);
        }
    }
}