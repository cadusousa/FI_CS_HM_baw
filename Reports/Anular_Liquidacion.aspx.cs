using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

public partial class Reports_Anular_Liquidacion : System.Web.UI.Page
{
    UsuarioBean user = null;
    string liquidacionID = "0";
    ArrayList Arr_Liquidacion = new ArrayList();
    ArrayList Arr_Liquidacion_Detalle = new ArrayList();
    RE_GenericBean Bean_Data_Liquidacion = null;
    string sql = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        liquidacionID = Request.QueryString["id"].ToString();
        sql = " and a.tli_id=" + liquidacionID + " and a.tli_pai_id=" + user.PaisID + " ";
        Arr_Liquidacion = (ArrayList)DB.Get_Liquidaciones(user, sql);
        foreach (RE_GenericBean Bean in Arr_Liquidacion)
        {
            Bean_Data_Liquidacion = new RE_GenericBean();
            Bean_Data_Liquidacion = Bean;
        }
        if (Arr_Liquidacion.Count > 0)
        {
            tb_serie.Text = Bean_Data_Liquidacion.strC6;
            tb_correlativo.Text = Bean_Data_Liquidacion.strC7;
            tb_banco.Text = Bean_Data_Liquidacion.strC16;
            tb_cuenta.Text = Bean_Data_Liquidacion.strC17;
            tb_codigo.Text = Bean_Data_Liquidacion.strC4;
            tb_nombre.Text = Bean_Data_Liquidacion.strC5;
            lbl_liquidacionid.Text = Bean_Data_Liquidacion.strC1;
        }
        else
        {
            WebMsgBox.Show("Liquidacion Inexistente");
            btn_aceptar.Enabled = false;
            return;
        }
    }
    protected void btn_aceptar_Click(object sender, EventArgs e)
    {
        RE_GenericBean Bean_Documentos = new RE_GenericBean();
        RE_GenericBean Bean_Aux = new RE_GenericBean();
        if (lbl_liquidacionid.Text == "0")
        {
            WebMsgBox.Show("Liquidacion Inexistente");
            btn_aceptar.Enabled = false;
            return;
        }
        if (tb_motivo.Text == "")
        {
            WebMsgBox.Show("Debe Ingresar el Motivo de Anulacion");
            btn_aceptar.Enabled = true;
            return;
        }
        liquidacionID = Request.QueryString["id"].ToString();
        Arr_Liquidacion_Detalle = (ArrayList)DB.Get_Detalle_Liquidacion(user, int.Parse(liquidacionID));
        foreach (RE_GenericBean Bean in Arr_Liquidacion_Detalle)
        {
            Bean_Documentos.arr1.Add(Bean);
        }
        Bean_Aux.strC1 = "24";
        Bean_Aux.strC2 = liquidacionID;
        Bean_Documentos.arr1.Add(Bean_Aux);
        int resultado = DB.Botar_Liquidacion(user, Bean_Documentos, tb_motivo.Text, int.Parse(liquidacionID));
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un Error al Tratar de Anular la Liquidacion");
            return;
        }
        else if (resultado == -101)
        {
            WebMsgBox.Show("No se pudo Anular la Liquidacion porque tiene Documentos Amarrados los Cuales ya fueron conciliados ");
            return;
        }
        else
        {
            WebMsgBox.Show("La Liquidacion fue Anulada Correctamente");
            btn_aceptar.Visible = false;
            btn_cancelar.Visible = false;
            btn_regresar.Visible = true;
            return;
        }
    }
    protected void btn_regresar_Click(object sender, EventArgs e)
    {
        string Script = "";
        Script = "<script>window.opener.location.reload();window.close();</script>";
        Response.Write(Script);
    }
    protected void btn_cancelar_Click(object sender, EventArgs e)
    {
        string Script = "";
        Script = "<script>window.opener.location.reload();window.close();</script>";
        Response.Write(Script);
    }
}