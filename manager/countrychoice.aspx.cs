
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

public partial class manager_countrychoice : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Contabilidad"] = null;
        if (Session["usuario"] == null)
            Response.Redirect("../default.aspx");
        user = (UsuarioBean)Session["usuario"];

        ArrayList arr = null;
        ListItem item = null;
        if (!Page.IsPostBack) {
            arr = (ArrayList)DB.getPaisesbyUser(user.ID);
            foreach (PaisBean pais in arr) {
                item = new ListItem(pais.Nombre, pais.ID.ToString());
                lb_pais.Items.Add(item);
            }
            lb_pais.SelectedIndex=0;
            arr = null;
            arr = (ArrayList)DB.getSucursalesbyuser(user.ID, int.Parse(lb_pais.SelectedValue));
            foreach (SucursalBean suc in arr) {
                item = new ListItem(suc.Nombre, suc.ID.ToString());
                lb_sucursal.Items.Add(item);
            }
            //************************RESTRICCION DE CONTABILIDAD USUARIOS****************************//
            lb_contabilidad.Items.Clear();
            int fiscal = 0;
            int financiera = 0;
            int bandera = 0;
            item = null;
            ArrayList arruser = (ArrayList)DB.getContaUsuario(user.ID, int.Parse(lb_pais.SelectedValue));
            ArrayList arrbloqueo = (ArrayList)DB.getContaPais(int.Parse(lb_pais.SelectedValue));
            foreach (RE_GenericBean rgbp in arrbloqueo)
            {
                if (rgbp.intC1 == 1)
                {
                    fiscal = 1; //desbloqueo fiscal por pais
                }

                if (rgbp.intC2 == 1)
                {
                    financiera = 1; // desbloqueo financiera por pais
                }
            }
            foreach (RE_GenericBean rgb in arruser)
            {
                if ((rgb.intC1 == 1) && (fiscal == 1))
                {
                    bandera++;
                    item = new ListItem("FISCAL", "1");
                    lb_contabilidad.Items.Add(item);
                }

                if ((rgb.intC2 == 1) && (financiera == 1))
                {
                    bandera++;
                    item = new ListItem("FINANCIERA", "2");
                    lb_contabilidad.Items.Add(item);
                }
            }
            if (bandera == 1)
            {
                lb_contabilidad.Visible = false;
                l_contabilidad.Visible = false;
            }
            else
            {
                lb_contabilidad.Visible = true;
                l_contabilidad.Visible = true;

            }
            //*********************************FIN RESTRICCION********************************************//
            //arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
            //foreach (RE_GenericBean rgb in arr)
            //{
            //    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            //    lb_contabilidad.Items.Add(item);
            //}
        }
    }
    protected void lb_pais_SelectedIndexChanged(object sender, EventArgs e)
    {
        lb_sucursal.Items.Clear();
        ListItem item;
        ArrayList arr = null;
        user = (UsuarioBean)Session["usuario"];
        arr = (ArrayList)DB.getSucursalesbyuser(user.ID, int.Parse(lb_pais.SelectedValue));
        foreach (SucursalBean suc in arr)
        {
            item = new ListItem(suc.Nombre, suc.ID.ToString());
            lb_sucursal.Items.Add(item);
        }

        //************************RESTRICCION DE CONTABILIDAD USUARIOS****************************//
        lb_contabilidad.Items.Clear();
        int fiscal = 0;
        int financiera = 0;
        int bandera = 0;
        item = null;
        ArrayList arruser = (ArrayList)DB.getContaUsuario(user.ID, int.Parse(lb_pais.SelectedValue));
        ArrayList arrbloqueo = (ArrayList)DB.getContaPais(int.Parse(lb_pais.SelectedValue));
        foreach (RE_GenericBean rgbp in arrbloqueo)
        {
            if (rgbp.intC1 == 1)
            {
                fiscal = 1; //desbloqueo fiscal por pais
            }

            if (rgbp.intC2 == 1)
            {
                financiera = 1; // desbloqueo financiera por pais
            }
        }
        foreach (RE_GenericBean rgb in arruser)
        {
            if ((rgb.intC1 == 1) && (fiscal == 1))
            {
                bandera++;
                item = new ListItem("FISCAL", "1");
                lb_contabilidad.Items.Add(item);
            }

            if ((rgb.intC2 == 1) && (financiera == 1))
            {
                bandera++;
                item = new ListItem("FINANCIERA", "2");
                lb_contabilidad.Items.Add(item);
            }
        }
        if (bandera == 1)
        {
            lb_contabilidad.Visible = false;
            l_contabilidad.Visible = false;
        }
        else
        {
            lb_contabilidad.Visible = true;
            l_contabilidad.Visible = true;
 
        }
        //*********************************FIN RESTRICCION********************************************//

    }
    protected void bt_aceptar_Click(object sender, EventArgs e)
    {
        UsuarioBean user;
        
        if ((lb_sucursal.Items.Count==0) || (lb_pais.Items.Count==0) || (lb_contabilidad.Items.Count==0) || (lb_sucursal==null) || lb_pais==null || lb_contabilidad==null){
            WebMsgBox.Show("No se puede configurar su sesion debido a que faltan criterios");
            return;
        }
        int resultado = 0;
        int paisID = int.Parse(lb_pais.SelectedValue);
        int sucID = int.Parse(lb_sucursal.SelectedValue);
        int tipo_conta = int.Parse(lb_contabilidad.SelectedValue);
        user = (UsuarioBean)Session["usuario"];
        user.PaisID = paisID;
        user.contaID = tipo_conta;
        Session["usuario"] = user;
        user.pais = (PaisBean)DB.getPais(paisID);
        user.Moneda = DB.getMonedaByPaisConta(user);
        user.Idioma = DB.getIdiomaByPaisConta(user);
        decimal SetTipoCambio = DB.getTipoCambioHoy(paisID);

        //*************************************************************BLOQUEO DE PERIODO*****************************************
        #region bloqueo periodo
        // calculando el total de dias entre fechas..................................
        RE_GenericBean bloqueo = null;
        string des = "1900/01/01";//fecha por default para el desbloqueo.
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        DateTime hoy = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
        DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1.Substring(0, 10)); //fecha del ultimo bloqueo    
        DateTime fecha_ultimo_desbloqueo = DateTime.Parse(bloqueo.strC2); //fecha del ultimo Desbloqueo
        DateTime fecha_ultimo_dia_mes = new DateTime(hoy.Year, hoy.Month, 1).AddDays(-1);  // ultimo dia del mes anterior
        // DateTime fecha_inicio = new DateTime(fecha_ultimo_dia_mes.Year, fecha_ultimo_dia_mes.Month, 1); // primer dia del mes anterior
        DateTime fecha_penultimo_mes = new DateTime(fecha_ultimo_dia_mes.Year, fecha_ultimo_dia_mes.Month, 1).AddDays(-1); //fecha del ultimo dia del penultimo mes
        TimeSpan dd = hoy.Subtract(fecha_ultimo_dia_mes);// DIFERENCIA ENTRE LA FECHA ACTUAL Y ULTIMA DEL MES ANTERIOR.
        double dif_en_dias = dd.TotalDays;
        if (bloqueo.intC3 == 1)  //si el bloqueo para ese pais esta habilitado
        {
            if (dif_en_dias > bloqueo.intC2) // si estoy fuera del perido de bloqueo
            {
                //sino estoy fuera del periodo limite entonces bloqueo al ulitmo dia del mes anterio a menos que la fecha de desbloqueo sea igual al dia de hoy
                if ((fecha_ultimo_desbloqueo != hoy) && (fecha_ultimo_dia_mes != fecha_ultimo_bloqueo))
                {
                    resultado = DB.SetBloqueoPeriodo(user, fecha_ultimo_dia_mes.ToString("yyyy-MM-dd").Substring(0, 10), des, fecha_ultimo_bloqueo.ToString("yyyy-MM-dd").Substring(0, 10), "Sistema Automatico");
                }
            }
            // estoy dentro del periodo de gracia entonces no bloqueo 
            else
            {
                //pero si la fecha del ultimo bloqueo es menor al ultimo dia del penultimo mes y la fecha de desbloqueo es distinto a hoy
                if ((fecha_ultimo_bloqueo < fecha_penultimo_mes) && (fecha_ultimo_desbloqueo != hoy))
                {
                    resultado = DB.SetBloqueoPeriodo(user, fecha_penultimo_mes.ToString("yyyy-MM-dd").Substring(0, 10), des, fecha_ultimo_bloqueo.ToString("yyyy-MM-dd").Substring(0, 10), "Sistema Automatico");
                }
            }
        }
        #endregion
        //*****************************************************************TERMINA BLOQUEO DEL PERIODO

        if (SetTipoCambio == 0)
        {
            if ((user.pais.ISO.Equals("SV")) || (user.pais.ISO.Equals("PA")) || (user.pais.ISO.Equals("SVLTF")) || (user.pais.ISO.Equals("PALTF")) || (user.pais.ISO.Equals("WMT")))
            {
                RE_GenericBean tcbean = new RE_GenericBean();
                tcbean.intC2 = user.PaisID;
                tcbean.decC1 = 1;
                tcbean.strC1 = "System";
                decimal result = DB.InsertTipoCambio(tcbean);
                SetTipoCambio = 1;
            }
            /*else if ((user.pais.ISO.Equals("BZ")) || (user.pais.ISO.Equals("BZLTF")))  2020-11-06 Ticket#2020102931000082 — RE: ROE BANCO CENTRAL BELICE 
            {
                RE_GenericBean tcbean = new RE_GenericBean();
                tcbean.intC2 = user.PaisID;
                tcbean.decC1 = decimal.Parse("2.1");
                tcbean.strC1 = "System";
                decimal result = DB.InsertTipoCambio(tcbean);
                SetTipoCambio = decimal.Parse("2.1");
                user.pais.TipoCambio = SetTipoCambio;
            }*/
            else
            {
                string mensaje = "<script languaje='JavaScript'>";
                mensaje += "window.open('tipocambio.aspx?id=" + paisID + "&accion=0', null, 'toolbar=no,resizable=no,status=no,width=400,height=280');";
                mensaje += "</script>";
                Page.RegisterClientScriptBlock("closewindow", mensaje);
            }
        }
        user.Aplicaciones = (Hashtable)DB.getPerfilesbyUser(user.ID);
        
        user.Departamento = (ArrayList)DB.getUsuarioDepartamentoAimar(user.ID, paisID);
        user.SucursalID = sucID;
        SucursalBean Suc_Bean = DB.getSucursal(sucID);
        user.Sucursal_Es_APL = Suc_Bean.Es_APL;
        user.Estado = 1;
        user.Fecha = DateTime.Now.ToShortDateString();
        user.Operacion = 1;
        Session["configurado"] = true;
        Session["Contabilidad"] = lb_contabilidad.SelectedValue;
        Session["usuario"] = user;
        if (SetTipoCambio == 0)
        {
            return;
        }
        Response.Redirect("manager.aspx");
    }
}
