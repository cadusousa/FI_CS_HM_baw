using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class operations_genera_comisiones : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ListItem item = null;
    DataTable dt = null;
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
            item = null;
            Obtengo_vendedores();
        }

    }
    public void Obtengo_vendedores()
    {
        string criterio = " and tipo_usuario=1 and pais='"+ user.pais.ISO  +"' and pw_activo <> 0 ";
        ArrayList arr = (ArrayList)DB.getProveedor_cajachica(criterio);
        foreach (RE_GenericBean vendedor in arr)
        {
            item = new ListItem(vendedor.strC2,vendedor.intC1.ToString());
            ddl_vendedores.Items.Add(item);
        }

    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {

    }
    protected void ddl_vendedores_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void btn_generar_comis_Click(object sender, EventArgs e)
    {
        gv_comisiones.DataBind();
        //fechas muestra

        string fechaini = tb_fechainicial.Text;
        string fechafin = tb_fechafinal.Text;

        #region Formatear Fechas
        //Fecha Inicio
        int fe_dia = int.Parse(fechaini.Substring(3, 2));
        int fe_mes = int.Parse(fechaini.Substring(0, 2));
        int fe_anio = int.Parse(fechaini.Substring(6, 4));
        fechaini = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            fechaini += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            fechaini += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            fechaini += "0" + fe_dia.ToString();
        }
        else
        {
            fechaini += fe_dia.ToString();
        }
        //Fecha Fin
        fe_dia = int.Parse(fechafin.Substring(3, 2));
        fe_mes = int.Parse(fechafin.Substring(0, 2));
        fe_anio = int.Parse(fechafin.Substring(6, 4));
        fechafin = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            fechafin += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            fechafin += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            fechafin += "0" + fe_dia.ToString();
        }
        else
        {
            fechafin += fe_dia.ToString();
        }
        #endregion

        dt = DB.GetComisiones(user, fechaini, fechafin, ddl_vendedores.SelectedItem.ToString());
        if (dt !=  null)
        {

            btn_generar_corte.Enabled = true;
        }
        else
        {
            btn_generar_corte.Enabled = false;
        }
        gv_comisiones.DataSource = dt;
        gv_comisiones.DataBind();
       
    }
    protected void btn_generar_corte_Click(object sender, EventArgs e)
    {
        RE_GenericBean result = null;
        if (ddl_vendedores.SelectedValue.ToString() == "0")
        {
            WebMsgBox.Show("Debe de elegir a un vendedor");
            return;
        }
        RE_GenericBean cortebean = new RE_GenericBean();
        RE_GenericBean rgb = null;
        ArrayList arr_comis_id = new ArrayList();


        if (gv_comisiones.Rows.Count > 0)
        {
            CheckBox chk_cortar;
            decimal total_corte=0, total_corte_eq=0;
            int total_comisiones = 0;
            decimal total_comision = 0, total_comision_eq=0;
            foreach (GridViewRow gvr in gv_comisiones.Rows)
            {
                total_comision = 0;
                total_comision_eq = 0;
                chk_cortar = (CheckBox)gvr.FindControl("CB_comision_selected");
                if (chk_cortar.Checked)
                {
                    rgb = new RE_GenericBean();
                    total_comisiones++;
                    //convertir a moneda local.
                    rgb.intC5 = int.Parse(gvr.Cells[11].Text);//moneda id
                    if (user.contaID == 1)
                    {
                        if (rgb.intC5 == 8)
                        {
                            //de dolares lo paso a moneda local
                            String hoy = DateTime.Now.ToString("yyyy-MM-dd");
                            decimal tc = DB.getTipoCambioByDay(user, hoy);
                            total_comision = Math.Round(decimal.Parse(gvr.Cells[6].Text),2);
                            total_comision = Math.Round((total_comision * tc),2);
                            rgb.decC1 = total_comision;
                            rgb.decC2 = Math.Round(decimal.Parse(gvr.Cells[6].Text),2);
                            total_comision_eq = rgb.decC2;
                        }
                        else
                        {
                            String hoy = DateTime.Now.ToString("yyyy-MM-dd");
                            decimal tc = DB.getTipoCambioByDay(user, hoy);
                            total_comision = decimal.Parse(gvr.Cells[6].Text);
                            rgb.decC1 =decimal.Parse(gvr.Cells[6].Text);
                            rgb.decC2 = Math.Round((rgb.decC1 / tc),2);
                            total_comision_eq = rgb.decC2;
                        }
                    }
                    else if (user.contaID == 2)
                    {
                        if (rgb.intC5 == 8)
                        {
                            String hoy = DateTime.Now.ToString("yyyy-MM-dd");
                            decimal tc = DB.getTipoCambioByDay(user, hoy);
                            total_comision = Math.Round(decimal.Parse(gvr.Cells[6].Text),2);
                            rgb.decC1 = Math.Round(decimal.Parse(gvr.Cells[6].Text),2);
                            rgb.decC2 = rgb.decC1 * tc;
                            total_comision_eq = rgb.decC2;
                        }
                        else
                        {
                            //de local lo paso a dolares
                            String hoy = DateTime.Now.ToString("yyyy-MM-dd");
                            decimal tc = DB.getTipoCambioByDay(user, hoy);
                            total_comision = Math.Round(decimal.Parse(gvr.Cells[6].Text),2);
                            total_comision = total_comision / tc;
                            rgb.decC1 = total_comision;
                            rgb.decC2 = Math.Round(decimal.Parse(gvr.Cells[6].Text),2);
                            total_comision_eq = rgb.decC2;
                        }
                    }
                    total_corte += total_comision;
                    total_corte_eq += total_comision_eq;
                    rgb.intC1 = total_comisiones;
                    rgb.intC2 = int.Parse(ddl_vendedores.SelectedValue.ToString()); //vendedor id
                    rgb.intC3 = int.Parse(gvr.Cells[9].Text);//agente id
                    rgb.intC4 = int.Parse(gvr.Cells[10].Text);//tipo id
                    rgb.strC1 = gvr.Cells[1].Text;//llave
                    rgb.strC2 = gvr.Cells[8].Text;//RO
                    rgb.strC3 = gvr.Cells[4].Text;//HBL
                    rgb.strC4 = gvr.Cells[12].Text; //FACTURA
    
                    rgb.intC6 = DB.InsertaComisiones(rgb, user);
                    if (rgb.intC6 == -100)
                    {
                        WebMsgBox.Show("Existio un error al momento de guardar las comisiones");
                        return;
                    }
                    arr_comis_id.Add(rgb);
                }
            }
           //WebMsgBox.Show("Total " + total_corte);
            if (total_comisiones == 0)
            {
                WebMsgBox.Show("Debe seleccionar al menos una comision");
                return;
            }

            cortebean.intC1 = int.Parse(ddl_vendedores.SelectedValue.ToString());//vendedor
            cortebean.decC1 = total_corte;//total
            cortebean.decC2 = total_corte_eq; // total equivalente
            cortebean.arr1 = arr_comis_id;//array con los id de las comisiones
            cortebean.strC1 = tbx_observacion.Text.ToString(); //observaciones
            if (arr_comis_id.Count > 0)
            {
                result = DB.GenerarCorteComision(cortebean, user);
                if (result == null)
                {
                    WebMsgBox.Show("Existio un error al momento de generar el corte");
                    return;
                }
            }
            #region agrego corte
            DataTable dtc = new DataTable();
            dtc.Columns.Add("id");
            dtc.Columns.Add("vendedor");
            dtc.Columns.Add("Serie");
            dtc.Columns.Add("Correlativo");
            dtc.Columns.Add("Total");
            dtc.Columns.Add("Fecha");
            String today = DateTime.Now.ToString("yyyy-MM-dd");
            object[] objArr = { 1, ddl_vendedores.SelectedItem.ToString(), result.strC1, result.intC1, Math.Round(total_corte,2), today };
            dtc.Rows.Add(objArr);
            gv_cortes.DataSource = dtc;
            gv_cortes.DataBind();
            #endregion
        }
        btn_generar_corte.Enabled = false;
    }
    protected void gv_comisiones_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[9].Visible = false;
        e.Row.Cells[10].Visible = false;
        e.Row.Cells[11].Visible = false;
    }
}
