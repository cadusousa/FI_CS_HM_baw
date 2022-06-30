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

public partial class operations_bloqueoperiodo : System.Web.UI.Page
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
            RE_GenericBean bloqueo = null;
            bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
            DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo   
            tb_bloqueo.Text = fecha_ultimo_bloqueo.ToString().Substring(0, 10);
            TB_pais.Text = user.pais.Nombre;
            dt = (DataTable)DB.getbloqueoactual(user.PaisID);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
    }
    protected void Bguardar_Click(object sender, EventArgs e)
    {
        RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        int result = 0;
        string fecha_bloqueo = DB.DateFormat(tb_fechacorte.Text);
        string hoy =  DateTime.Now.ToString("yyyy-MM-dd");
        DateTime fecha_anterior = DateTime.Parse(bloqueo.strC1);
        if (DateTime.Parse(fecha_bloqueo) > DateTime.Parse(hoy))
        {
            WebMsgBox.Show("No puede bloquear periodos mayores a la fecha de hoy.");
            return;
        }
        if (DateTime.Parse(fecha_bloqueo) > DateTime.Parse(bloqueo.strC1))
        {
            WebMsgBox.Show("No puede bloquear periodos mayores al periodo ya bloqueado " + bloqueo.strC1);
            return;
        }
        result = DB.SetBloqueoPeriodo(user, fecha_bloqueo,hoy,fecha_anterior.ToString("yyyy-MM-dd").Substring(0, 10),"Bloqueo Manual");
        if (result != -100)
        {
            
            WebMsgBox.Show("Periodo Bloqueado con exito");
            Bguardar.Enabled = false;
            dt = (DataTable)DB.getbloqueoactual(user.PaisID);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else
        {
            WebMsgBox.Show("Existio un error al guardar. ");
            return;
        }
    }
    protected void gv_resultado_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int result = 0, resultado = 0;
         RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        user = (UsuarioBean)Session["usuario"];
        int index = Convert.ToInt32(e.RowIndex);
        string hoy = "1900/01/01";
        DateTime fechabloqueo = DateTime.Parse(bloqueo.strC1);
        result = DB.SetBloqueoPeriodo(user, "", hoy, fechabloqueo.ToString("yyyy-MM-dd").Substring(0, 10), "Anulacion de Bloqueo Manual");
        if (result == -100)
        {
            WebMsgBox.Show("Existio un error al guardar. ");
            return;
        }
        else
        {
            dt = null;
            dt = (DataTable)DB.getbloqueoactual(user.PaisID);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();

            //*************************************************************BLOQUEO DE PERIODO*****************************************
            #region bloqueo periodo
            // calculando el total de dias entre fechas..................................
            string des = "1900/01/01";//fecha por default para el desbloqueo.
            bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
            DateTime hoy2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1.Substring(0, 10)); //fecha del ultimo bloqueo    
            DateTime fecha_ultimo_desbloqueo = DateTime.Parse(bloqueo.strC2); //fecha del ultimo Desbloqueo
            DateTime fecha_ultimo_dia_mes = new DateTime(hoy2.Year, hoy2.Month, 1).AddDays(-1);  // ultimo dia del mes anterior
            // DateTime fecha_inicio = new DateTime(fecha_ultimo_dia_mes.Year, fecha_ultimo_dia_mes.Month, 1); // primer dia del mes anterior
            DateTime fecha_penultimo_mes = new DateTime(fecha_ultimo_dia_mes.Year, fecha_ultimo_dia_mes.Month, 1).AddDays(-1); //fecha del ultimo dia del penultimo mes
            TimeSpan dd = hoy2.Subtract(fecha_ultimo_dia_mes);// DIFERENCIA ENTRE LA FECHA ACTUAL Y ULTIMA DEL MES ANTERIOR.
            double dif_en_dias = dd.TotalDays;
            if (bloqueo.intC3 == 1)  //si el bloqueo para ese pais esta habilitado
            {
                if (dif_en_dias > bloqueo.intC2) // si estoy fuera del perido de bloqueo
                {
                    //sino estoy fuera del periodo limite entonces bloqueo al ulitmo dia del mes anterio a menos que la fecha de desbloqueo sea igual al dia de hoy
                    if ((fecha_ultimo_desbloqueo != hoy2) && (fecha_ultimo_dia_mes != fecha_ultimo_bloqueo))
                    {
                        resultado = DB.SetBloqueoPeriodo(user, fecha_ultimo_dia_mes.ToString("yyyy-MM-dd").Substring(0, 10), des, fecha_ultimo_bloqueo.ToString("yyyy-MM-dd").Substring(0, 10), "Anulacion de Bloqueo Manual");
                    }
                }
                // estoy dentro del periodo de gracia entonces no bloqueo 
                else
                {
                    //pero si la fecha del ultimo bloqueo es menor al ultimo dia del penultimo mes y la fecha de desbloqueo es distinto a hoy
                    if ((fecha_ultimo_bloqueo < fecha_penultimo_mes) && (fecha_ultimo_desbloqueo != hoy2))
                    {
                        resultado = DB.SetBloqueoPeriodo(user, fecha_penultimo_mes.ToString("yyyy-MM-dd").Substring(0, 10), des, fecha_ultimo_bloqueo.ToString("yyyy-MM-dd").Substring(0, 10), "Anulacion de Bloqueo Manual");
                    }
                }
            }
            if (resultado != -100)
            {
                WebMsgBox.Show("Bloqueo de periodo Anulado con exito");
            }
        }
        #endregion
        //*****************************************************************TERMINA BLOQUEO DEL PERIODO
    }
}