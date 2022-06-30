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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Net;
using System.Text;

public partial class Reports_EstadoCuentaCliente : System.Web.UI.Page
{
    UsuarioBean user = null;
    private DataTable dt;
    private LibroDiarioDS ds = null;
    string fechaini = "", fechafin = "", clienteNombre = "";
    string simbolomoneda = "";
    string tipoPersona = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null) {
            Response.Redirect("../default.aspx");
        }
        #region Definir Fechas
        if (!IsPostBack)
        {
            DateTime Fecha = DateTime.Now;
            tb_fechaini.Text = "01/01/" + Fecha.Year.ToString();
            tb_fechafin.Text = Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString();
        }
        #endregion
        user = (UsuarioBean)Session["usuario"];
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad, user.PaisID);
    }

    private void obtengo_listas(int tipoconta, int paiID)
    {
        System.Web.UI.WebControls.ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(paiID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new System.Web.UI.WebControls.ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }

            //arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
            //foreach (RE_GenericBean rgb in arr)
            //{
            //    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            //    lb_contabilidad.Items.Add(item);
            //}

            //************************RESTRICCION DE CONTABILIDAD USUARIOS****************************//
            lb_contabilidad.Items.Clear();
            int bandera = 0; // verifica si el usuario tiener contabilidad consolidada.
            int fiscal = 0;
            int financiera = 0;
            item = null;
            ArrayList arruser = (ArrayList)DB.getContaUsuario(user.ID, user.PaisID);
            ArrayList arrbloqueo = (ArrayList)DB.getContaPais(user.PaisID);
            foreach (RE_GenericBean rgbp in arrbloqueo)
            {
                if (rgbp.intC1 == 1)
                {
                    fiscal = 1; //desbloqueo fiscal
                }

                if (rgbp.intC2 == 1)
                {
                    financiera = 1; // desbloqueo financiera.
                }
            }
            foreach (RE_GenericBean rgb in arruser)
            {
                if ((rgb.intC1 == 1) && (fiscal == 1))
                {
                    bandera++;
                    item = new System.Web.UI.WebControls.ListItem("FISCAL", "1");
                    if (user.contaID == 1)
                    {
                        lb_contabilidad.Items.Add(item);
                    }
                }

                if ((rgb.intC2 == 1) && (financiera == 1))
                {
                    bandera++;
                    item = new System.Web.UI.WebControls.ListItem("FINANCIERA", "2");
                    if (user.contaID == 2)
                    {
                        lb_contabilidad.Items.Add(item);
                    }
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
            if (user.contaID == 2)
            {
                lb_consolidar.Visible = false;
                chk_consolidar.Visible = false;
                chk_consolidar.Checked = false;
            }

            //*********************************FIN RESTRICCION********************************************//

        }
    }
    
    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        if (lb_tipopersona.SelectedValue.Equals("3"))
        { // cliente
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(a.nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and a.id_cliente=" + tb_codigo.Text; else where += "a.id_cliente=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and a.codigo_tributario='" + tb_nitb.Text + "'"; else where += "a.codigo_tributario='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getClientes(where, user, "REPORTES"); //Proveedor
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, "REPORTES"); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit a un agente");
                return;
            }

            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(where, "REPORTES"); //Agente
            dt = (DataTable)Utility.fillGridView("Agente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una naviera");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_naviera=" + tb_codigo.Text; else where += "id_naviera=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getNavieras(where, "REPORTES"); //Naviera
            dt = (DataTable)Utility.fillGridView("Naviera", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una línea aerea");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(name)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and carrier_id=" + tb_codigo.Text; else where += " and carrier_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getCarriers(where); //Lineas aereas
            dt = (DataTable)Utility.fillGridView("LineasAereas", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit usuario de caja chica");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo.Text; else where += " and id_usuario=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(nombre_comercial)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_intercompany=" + tb_codigo.Text; else where += "and id_intercompany=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += " and  nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.Get_Intercompanys(where); //INtercompany
            dt = (DataTable)Utility.fillGridView("Intercompany", Arr);
        }

        ViewState["proveedordt"] = dt;
        gv_proveedor.DataSource = dt;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
    protected void gv_proveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["proveedordt"];
        gv_proveedor.DataSource = dt1;
        gv_proveedor.PageIndex = e.NewPageIndex;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
    protected void gv_proveedor_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_proveedor.SelectedRow;
        if (lb_tipopersona.SelectedValue.Equals("3")) //proveedor
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("4")) //proveedor
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompany
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
        }
    }

    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
            ArrayList Arr = null;
            string where = "";
            //Arr = (ArrayList)DB.getAgente(where);
            //dt = (DataTable)Utility.fillGridView("Agente", Arr);
            //ViewState["proveedordt"] = dt;
            //gv_proveedor.DataSource = dt;
            //gv_proveedor.DataBind();
        }
    }

    private void obtengo_datos_cliente()
    {
        int solopendiente = 0;
        string consolidaMoneda = "";
        if (chk_pendliq.Checked) solopendiente = 1;
        solopendiente = solopendiente;
        if (chk_consolidar.Checked == true)
        {
            consolidaMoneda = "si";
        }
        else
        {
            consolidaMoneda = "no";
        }
        string consMon = consolidaMoneda;
        //fechaini = "", fechafin = "", clienteNombre = "";
        double clienteID = double.Parse(tb_agenteID.Text);
        int tpi = int.Parse(lb_tipopersona.SelectedValue);

        if (tb_fechaini.Text != null && !tb_fechaini.Text.Equals(""))
            fechaini = tb_fechaini.Text;
        if (tb_fechafin.Text != null && !tb_fechafin.Text.Equals(""))
            fechafin = tb_fechafin.Text;
        if (tb_agentenombre.Text != null && !tb_agentenombre.Text.Equals(""))
            clienteNombre = tb_agentenombre.Text;
        int monID = int.Parse(lb_moneda.SelectedValue);
        int contaID = int.Parse(lb_contabilidad.SelectedValue);

        #region Formatear Fechas
        //Fecha Inicio
        int fe_dia = int.Parse(fechaini.Substring(3, 2));
        int fe_mes = int.Parse(fechaini.Substring(0, 2));
        int fe_anio = int.Parse(fechaini.Substring(6, 4));
        fechaini = fe_anio.ToString() + "-";
        if (fe_mes < 10)
        {
            fechaini += "0" + fe_mes.ToString() + "-";
        }
        else
        {
            fechaini += fe_mes.ToString() + "-";
        }
        if (fe_dia < 10)
        {
            fechaini += "0" + fe_dia.ToString();
        }
        else
        {
            fechaini += fe_dia.ToString();
        }
        fechaini = fechaini + " 00:00:00";
        //Fecha Fin
        fe_dia = int.Parse(fechafin.Substring(3, 2));
        fe_mes = int.Parse(fechafin.Substring(0, 2));
        fe_anio = int.Parse(fechafin.Substring(6, 4));
        fechafin = fe_anio.ToString() + "-";
        if (fe_mes < 10)
        {
            fechafin += "0" + fe_mes.ToString() + "-";
        }
        else
        {
            fechafin += fe_mes.ToString() + "-";
        }
        if (fe_dia < 10)
        {
            fechafin += "0" + fe_dia.ToString();
        }
        else
        {
            fechafin += fe_dia.ToString();
        }
        fechafin = fechafin + " 23:59:59";
        #endregion
        simbolomoneda = Utility.TraducirMonedaInt(monID);
        string where = "", where_1 = "", where2 = "", where2_1 = "", where3 = "", where3_1 = "", where4 = "", where4_1 = "", where4_2 = "", where5 = "", where5_1 = "", where5_2 = "", where5_3 = "", where5_4 = "", where5_5 = "", where6 = "", where6_1 = "", where7 = "", where7_1 = "", where8 = "", where8_1 = "", where5_6 = "", where8_2 = "", where7_2 = "", where9 = "", where9_1 = "", where9_2 = "", where10_1 = "";
        string where_cons = "", where_1_cons = "", where2_cons = "", where2_1_cons = "", where3_cons = "", where3_1_cons = "", where4_cons = "", where4_1_cons = "", where4_2_cons = "", where5_cons = "", where5_1_cons = "", where5_2_cons = "", where5_3_cons = "", where5_4_cons = "", where5_5_cons = "", where6_cons = "", where6_1_cons = "", where7_cons = "", where7_1_cons = "", where8_cons = "", where8_1_cons = "", where5_6_cons = "", where8_2_cons = "", where7_2_cons = "", where9_cons = "", where9_1_cons = "", where9_2_cons = "", where10_1_cons = "";
        string tipoPersona = "";

        #region Parametros estado cuenta cliente
        //Facturas
        where = " and tfa_conta_id=" + contaID + " and tfa_moneda=" + monID + " and tfa_ted_id<>3 and tfa_tpi_id=" + tpi + " ";
        if (fechaini != null && !fechaini.Equals("")) where += " and tfa_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where += " and tfa_fecha_emision<='" + fechafin + "'";
        where += " and tfa_pai_id=" + user.PaisID + " ";
        //facturas anuladas
        where_1 = " and a.tfa_conta_id=" + contaID + " and a.tfa_moneda=" + monID + " and a.tfa_ted_id=3 and tfa_tpi_id=" + tpi + " ";
        if (fechaini != null && !fechaini.Equals("")) where_1 += " and a.tfa_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where_1 += " and a.tfa_fecha_emision<='" + fechafin + "'";
        where_1 += " and a.tfa_pai_id=" + user.PaisID + " ";

        //Notas Debito
        where2 = " and tnd_tcon_id=" + contaID + " and tnd_moneda=" + monID + " and tnd_tpi_id=" + tpi + " and tnd_ted_id<>3";
        if (fechaini != null && !fechaini.Equals("")) where2 += " and tnd_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where2 += " and tnd_fecha_emision<='" + fechafin + "'";
        where2 += " and tnd_pai_id=" + user.PaisID + " ";
        //Notas Credito
        // anuladas Nota Debito
        //Notas Debito
        where2_1 = " and a.tnd_tcon_id=" + contaID + " and a.tnd_moneda=" + monID + " and a.tnd_tpi_id=" + tpi + " and a.tnd_ted_id=3";
        if (fechaini != null && !fechaini.Equals("")) where2_1 += " and a.tnd_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where2_1 += " and a.tnd_fecha_emision<='" + fechafin + "'";
        where2_1 += " and a.tnd_pai_id=" + user.PaisID + " ";

        //where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " ";
        where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " and tnc_ted_id<>3 ";
        //if (solopendiente == 1) where3 += " and tnc_ted_id in (1,2)"; else where3 += " and tnc_ted_id in (1,2,4)";
        if (fechaini != null && !fechaini.Equals("")) where3 += " and tnc_fecha>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where3 += " and tnc_fecha<='" + fechafin + "'";
        where3 += " and tnc_pai_id=" + user.PaisID + " ";
        // anuladas nc
        //where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " ";
        where3_1 = " and a.tnc_tcon_id=" + contaID + " and a.tnc_mon_id=" + monID + " and a.tnc_tpi_id=" + tpi + " and a.tnc_ted_id=3 ";
        //if (solopendiente == 1) where3 += " and tnc_ted_id in (1,2)"; else where3 += " and tnc_ted_id in (1,2,4)";
        if (fechaini != null && !fechaini.Equals("")) where3_1 += " and a.tnc_fecha>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where3_1 += " and a.tnc_fecha<='" + fechafin + "'";
        where3_1 += " and a.tnc_pai_id=" + user.PaisID + " ";
        //Recibos
        //where4 = " and tre_id=" + contaID + " and tre_moneda_id=" + monID + " and tre_tpi_id=" + tpi + " ";
        //where4 = " and tre_tcon_id=" + contaID + " and tre_moneda_id=" + monID + " ";
        where4 = " and tre_tcon_id=" + contaID + " and tre_moneda_id=" + monID + " and tre_ted_id<>3 ";
        //if (solopendiente == 1) where4 += " and tre_ted_id in (0)"; else where4 += " and tre_ted_id in (0)";
        if (fechaini != null && !fechaini.Equals("")) where4 += " and tre_fecha>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where4 += " and tre_fecha<='" + fechafin + "'";
        where4 += " and tre_pai_id=" + user.PaisID + " ";
        //Recibos anulados
        where4_1 = " and a.tre_tcon_id=" + contaID + " and a.tre_moneda_id=" + monID + " and a.tre_ted_id=3 ";
        //if (solopendiente == 1) where4 += " and tre_ted_id in (0)"; else where4 += " and tre_ted_id in (0)";
        if (fechaini != null && !fechaini.Equals("")) where4_1 += " and a.tre_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where4_1 += " and a.tre_fecha_emision<='" + fechafin + "'";
        where4_1 += " and a.tre_pai_id=" + user.PaisID + " ";

        //Cheques y Transferencias
        where5 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=4 ";
        if (fechaini != null && !fechaini.Equals("")) where5 += " and tcg_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where5 += " and tcg_fecha_emision<='" + fechafin + "'";
        where5 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " ";

        //Cheques y Transferencias anuladas
        where5_1 = " and a.tcg_tcon_id=" + contaID + " and a.tcg_tpi_id=" + tpi + " and a.tcg_ted_id=3 ";
        if (fechaini != null && !fechaini.Equals("")) where5_1 += " and a.tcg_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where5_1 += " and a.tcg_fecha_emision<='" + fechafin + "'";
        where5_1 += " and a.tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " ";

        if (user.PaisID == 1 && contaID == 1 && consMon == "si")
        {
            //Facturas
            where_cons = " and tfa_conta_id=" + contaID + " and tfa_moneda<>" + monID + " and tfa_ted_id<>3 and tfa_tpi_id=" + tpi + " ";
            if (fechaini != null && !fechaini.Equals("")) where_cons += " and tfa_fecha_emision>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where_cons += " and tfa_fecha_emision<='" + fechafin + "'";
            where_cons += " and tfa_pai_id=" + user.PaisID + " ";
            //facturas anuladas
            where_1_cons = " and a.tfa_conta_id=" + contaID + " and a.tfa_moneda<>" + monID + " and a.tfa_ted_id=3 and tfa_tpi_id=" + tpi + " ";
            if (fechaini != null && !fechaini.Equals("")) where_1_cons += " and a.tfa_fecha_emision>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where_1_cons += " and a.tfa_fecha_emision<='" + fechafin + "'";
            where_1_cons += " and a.tfa_pai_id=" + user.PaisID + " ";

            //Notas Debito
            where2_cons = " and tnd_tcon_id=" + contaID + " and tnd_moneda<>" + monID + " and tnd_tpi_id=" + tpi + " and tnd_ted_id<>3";
            if (fechaini != null && !fechaini.Equals("")) where2_cons += " and tnd_fecha_emision>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where2_cons += " and tnd_fecha_emision<='" + fechafin + "'";
            where2_cons += " and tnd_pai_id=" + user.PaisID + " ";
            //Notas Credito
            // anuladas Nota Debito
            //Notas Debito
            where2_1_cons = " and a.tnd_tcon_id=" + contaID + " and a.tnd_moneda<>" + monID + " and a.tnd_tpi_id=" + tpi + " and a.tnd_ted_id=3";
            if (fechaini != null && !fechaini.Equals("")) where2_1_cons += " and a.tnd_fecha_emision>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where2_1_cons += " and a.tnd_fecha_emision<='" + fechafin + "'";
            where2_1_cons += " and a.tnd_pai_id=" + user.PaisID + " ";


            where3_cons = " and tnc_tcon_id=" + contaID + " and tnc_mon_id<>" + monID + " and tnc_tpi_id=" + tpi + " and tnc_ted_id<>3 ";
            if (fechaini != null && !fechaini.Equals("")) where3_cons += " and tnc_fecha>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where3_cons += " and tnc_fecha<='" + fechafin + "'";
            where3_cons += " and tnc_pai_id=" + user.PaisID + " ";
            // anuladas nc
            where3_1_cons = " and a.tnc_tcon_id=" + contaID + " and a.tnc_mon_id<>" + monID + " and a.tnc_tpi_id=" + tpi + " and a.tnc_ted_id=3 ";
            if (fechaini != null && !fechaini.Equals("")) where3_1_cons += " and a.tnc_fecha>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where3_1_cons += " and a.tnc_fecha<='" + fechafin + "'";
            where3_1_cons += " and a.tnc_pai_id=" + user.PaisID + " ";
            //Recibos
            where4_cons = " and tre_tcon_id=" + contaID + " and tre_moneda_id<>" + monID + " and tre_ted_id<>3 ";
            if (fechaini != null && !fechaini.Equals("")) where4_cons += " and tre_fecha>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where4_cons += " and tre_fecha<='" + fechafin + "'";
            where4_cons += " and tre_pai_id=" + user.PaisID + " ";
            //Recibos anulados
            where4_1_cons = " and a.tre_tcon_id=" + contaID + " and a.tre_moneda_id<>" + monID + " and a.tre_ted_id=3 ";
            if (fechaini != null && !fechaini.Equals("")) where4_1_cons += " and a.tre_fecha_emision>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where4_1_cons += " and a.tre_fecha_emision<='" + fechafin + "'";
            where4_1_cons += " and a.tre_pai_id=" + user.PaisID + " ";

            //Cheques y Transferencias
            where5_cons = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=4 ";
            if (fechaini != null && !fechaini.Equals("")) where5_cons += " and tcg_fecha_emision>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where5_cons += " and tcg_fecha_emision<='" + fechafin + "'";
            where5_cons += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id<>" + monID + " ";

            //Cheques y Transferencias anuladas
            where5_1_cons = " and a.tcg_tcon_id=" + contaID + " and a.tcg_tpi_id=" + tpi + " and a.tcg_ted_id=3 ";
            if (fechaini != null && !fechaini.Equals("")) where5_1_cons += " and a.tcg_fecha_emision>='" + fechaini + "'";
            if (fechafin != null && !fechafin.Equals("")) where5_1_cons += " and a.tcg_fecha_emision<='" + fechafin + "'";
            where5_1_cons += " and a.tcg_pai_id=" + user.PaisID + " and tcg_ttm_id<>" + monID + " ";
        }
        #endregion

        ds = DB.getEstadoCuentaCliente_detallado(clienteID, where, where2, where3, where4, where5, fechafin, monID, solopendiente, where4_1, where_1, where2_1, where3_1, where5_1,
                                                 where_cons, where2_cons, where3_cons, where4_cons, where5_cons, where4_1_cons, where_1_cons, where2_1_cons, where3_1_cons, where5_1_cons, consMon, user.PaisID, contaID);

    }

    private void obtengo_datos_proveedor()
    {
        /*int solopendiente = int.Parse(Request.QueryString["solopendiente"].ToString());
        string consMon = Request.QueryString["consolida_moneda"].ToString();
        string fechaini = "", fechafin = "", clienteNombre = "";
        double clienteID = double.Parse(Request.QueryString["cliID"].ToString());
        int tpi = int.Parse(Request.QueryString["tpi"].ToString());*/


        int solopendiente = 0;
        string consolidaMoneda = "";
        if (chk_pendliq.Checked) solopendiente = 1;
        //solopendiente = solopendiente;
        if (chk_consolidar.Checked == true)
        {
            consolidaMoneda = "si";
        }
        else
        {
            consolidaMoneda = "no";
        }
        string consMon = consolidaMoneda;
        //fechaini = "", fechafin = "", clienteNombre = "";
        double clienteID = double.Parse(tb_agenteID.Text);
        int tpi = int.Parse(lb_tipopersona.SelectedValue);

        /*if (Request.QueryString["fechaini"] != null && !Request.QueryString["fechaini"].ToString().Equals(""))
            fechaini = Request.QueryString["fechaini"].ToString();
        if (Request.QueryString["fechafin"] != null && !Request.QueryString["fechafin"].ToString().Equals(""))
            fechafin = Request.QueryString["fechafin"].ToString();
        if (Request.QueryString["clientenombre"] != null && !Request.QueryString["clientenombre"].ToString().Equals(""))
            clienteNombre = Request.QueryString["clientenombre"].ToString();
        int monID = int.Parse(Request.QueryString["moneda"].ToString());
        int contaID = int.Parse(Request.QueryString["conta"].ToString());*/


        if (tb_fechaini.Text != null && !tb_fechaini.Text.Equals(""))
            fechaini = tb_fechaini.Text;
        if (tb_fechafin.Text != null && !tb_fechafin.Text.Equals(""))
            fechafin = tb_fechafin.Text;
        if (tb_agentenombre.Text != null && !tb_agentenombre.Text.Equals(""))
            clienteNombre = tb_agentenombre.Text;
        int monID = int.Parse(lb_moneda.SelectedValue);
        int contaID = int.Parse(lb_contabilidad.SelectedValue);

        #region Formatear Fechas
        //Fecha Inicio
        int fe_dia = int.Parse(fechaini.Substring(3, 2));
        int fe_mes = int.Parse(fechaini.Substring(0, 2));
        int fe_anio = int.Parse(fechaini.Substring(6, 4));
        fechaini = fe_anio.ToString() + "-";
        if (fe_mes < 10)
        {
            fechaini += "0" + fe_mes.ToString() + "-";
        }
        else
        {
            fechaini += fe_mes.ToString() + "-";
        }
        if (fe_dia < 10)
        {
            fechaini += "0" + fe_dia.ToString();
        }
        else
        {
            fechaini += fe_dia.ToString();
        }
        fechaini = fechaini + " 00:00:00";
        //Fecha Fin
        fe_dia = int.Parse(fechafin.Substring(3, 2));
        fe_mes = int.Parse(fechafin.Substring(0, 2));
        fe_anio = int.Parse(fechafin.Substring(6, 4));
        fechafin = fe_anio.ToString() + "-";
        if (fe_mes < 10)
        {
            fechafin += "0" + fe_mes.ToString() + "-";
        }
        else
        {
            fechafin += fe_mes.ToString() + "-";
        }
        if (fe_dia < 10)
        {
            fechafin += "0" + fe_dia.ToString();
        }
        else
        {
            fechafin += fe_dia.ToString();
        }
        fechafin = fechafin + " 23:59:59";
        #endregion
        string simbolomoneda = Utility.TraducirMonedaInt(monID);
        string where = "", where_1 = "", where2 = "", where2_1 = "", where3 = "", where3_1 = "", where4 = "", where4_1 = "", where4_2 = "", where5 = "", where5_1 = "", where5_2 = "", where5_3 = "", where5_4 = "", where5_5 = "", where6 = "", where6_1 = "", where7 = "", where7_1 = "", where8 = "", where8_1 = "", where5_6 = "", where8_2 = "", where7_2 = "", where9 = "", where9_1 = "", where9_2 = "", where10_1 = "";
        string where_cons = "", where_1_cons = "", where2_cons = "", where2_1_cons = "", where3_cons = "", where3_1_cons = "", where4_cons = "", where4_1_cons = "", where4_2_cons = "", where5_cons = "", where5_1_cons = "", where5_2_cons = "", where5_3_cons = "", where5_4_cons = "", where5_5_cons = "", where6_cons = "", where6_1_cons = "", where7_cons = "", where7_1_cons = "", where8_cons = "", where8_1_cons = "", where5_6_cons = "", where8_2_cons = "", where7_2_cons = "", where9_cons = "", where9_1_cons = "", where9_2_cons = "", where10_1_cons = "";
        

        if (tpi == 3) tipoPersona = "Cliente";
        else if (tpi == 4) tipoPersona = "Proveedor";
        else if (tpi == 2) tipoPersona = "Agente";
        else if (tpi == 5) tipoPersona = "Naviera";
        else if (tpi == 6) tipoPersona = "Lineas Aereas";
        else if (tpi == 10) tipoPersona = "Intercompany";


        //Cortes
        where = " and tcp_conta_id=" + contaID + " and tcp_mon_id=" + monID + " and tcp_tpi_id=" + tpi + " and tcp_ted_id<>3";
        if (fechaini != null && !fechaini.Equals("")) where += " and tcp_fecha_creacion>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where += " and tcp_fecha_creacion<='" + fechafin + "'";
        where += " and tcp_pai_id=" + user.PaisID + " ";
        //Notas Debito
        where2 = " and tnd_tcon_id=" + contaID + " and tnd_moneda=" + monID + " and tnd_tpi_id=" + tpi + " and tnd_ted_id<>3";
        if (fechaini != null && !fechaini.Equals("")) where2 += " and tnd_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where2 += " and tnd_fecha_emision<='" + fechafin + "'";
        where2 += " and tnd_pai_id=" + user.PaisID + " ";
        //Notas Credito
        where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " and tnc_ted_id<>3 ";
        if (fechaini != null && !fechaini.Equals("")) where3 += " and tnc_fecha>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where3 += " and tnc_fecha<='" + fechafin + "'";
        where3 += " and tnc_pai_id=" + user.PaisID + " ";
        //Recibos de Soas
        where4 = " and trc_tcon_id=" + contaID + " and trc_moneda_id=" + monID + " and trc_tpi_id=" + tpi + " and trc_ted_id<>3 ";
        if (fechaini != null && !fechaini.Equals("")) where4 += " and trc_fecha>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where4 += " and trc_fecha<='" + fechafin + "'";
        where4 += " and trc_pai_id=" + user.PaisID + " ";
        //Cheques y Transferencias
        where5 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=4 ";
        if (fechaini != null && !fechaini.Equals("")) where5 += " and tcg_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where5 += " and tcg_fecha_emision<='" + fechafin + "'";
        where5 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " ";
        //-provisiones sin corte
        where6 = " and to_char(tpr_fecha_acepta,'yyyy-mm-dd')>='" + fechaini + "' and to_char(tpr_fecha_acepta,'yyyy-mm-dd')<='" + fechafin + "' and tpr_mon_id=" + monID + " and tpr_tcon_id=" + contaID + " ";
        where6 += " and tpr_pai_id=" + user.PaisID + " and tpr_ted_id not in (1,3) ";
        where6 += " and tpr_prov_id not in (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tpr_prov_id and a.tcp_mon_id =" + monID + "  ";
        where6 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "' ";
        where6 += " and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where6 += " and tpr_tpi_id=" + tpi + " group by tpr_prov_id, tpr_correlativo, ";
        where6 += " tpr_serie, tpr_fecha_acepta,tpr_contenedor,tpr_hbl, tpr_valor,poliza, usuario having (tpr_valor-coalesce((select sum(abono) from (  ";
        where6 += "  select sum(b.tcg_valor) as abono    from tbl_cheques_generados b, tbl_cheque_corte c, tbl_corte_proveedor_detalle d   ";
        where6 += " where b.tcg_id=c.corte_tcg_id and c.corte_tcp_id=d.tcpd_tcp_id and d.tcpd_docto_id=tpr_prov_id    ";
        where6 += " and to_char(b.tcg_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and b.tcg_ted_id<>3    and d.tcpd_str_id in (5,10)   union all ";
        where6 += " select sum(d.trp_monto) as abono    from tbl_retencion_provision d    where d.trp_tpr_prov_id=tpr_prov_id    ";
        where6 += " and to_char(d.trp_fecha_generacion,'yyyy-mm-dd')<='" + fechafin + "' and d.trp_ted_id<>3 ) abonos),0))>0 and tpr_prov_id not in (       ";
        where6 += " select tpr_prov_id from aux_prov_id where tpr_proveedor_id=" + clienteID + " and tpr_tpi_id=" + tpi + " and tpr_pai_id=" + user.PaisID + " and tpr_mon_id=" + monID + " and ";
        where6 += " tpr_tcon_id=" + contaID + " and  tcg_fecha_emision<='" + fechafin + "'   ) ";
        //provisioens sin corte anuladas
        //-provisiones sin corte
        where6_1 = " and to_char(tpr_fecha_acepta,'yyyy-mm-dd')>='" + fechaini + "' and to_char(tpr_fecha_acepta,'yyyy-mm-dd')<='" + fechafin + "' and tpr_mon_id=" + monID + " and tpr_tcon_id=" + contaID + " ";
        where6_1 += " and tpr_pai_id=" + user.PaisID + " and tpr_ted_id =3 ";
        where6_1 += " and tpr_prov_id not in (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tpr_prov_id and a.tcp_mon_id =" + monID + "  ";
        where6_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "' ";
        where6_1 += " and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where6_1 += " and tpr_tpi_id=" + tpi + " group by tpr_prov_id, tpr_correlativo, ";
        where6_1 += " tpr_serie, tpr_fecha_acepta,tpr_contenedor,tpr_hbl, tpr_valor,poliza, usuario having (tpr_valor-coalesce((select sum(abono) from (  ";
        where6_1 += "  select sum(b.tcg_valor) as abono    from tbl_cheques_generados b, tbl_cheque_corte c, tbl_corte_proveedor_detalle d   ";
        where6_1 += " where b.tcg_id=c.corte_tcg_id and c.corte_tcp_id=d.tcpd_tcp_id and d.tcpd_docto_id=tpr_prov_id    ";
        where6_1 += " and to_char(b.tcg_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and b.tcg_ted_id<>3    and d.tcpd_str_id in (5,10)   union all ";
        where6_1 += " select sum(d.trp_monto) as abono    from tbl_retencion_provision d    where d.trp_tpr_prov_id=tpr_prov_id    ";
        where6_1 += " and to_char(d.trp_fecha_generacion,'yyyy-mm-dd')<='" + fechafin + "' and d.trp_ted_id<>3 ) abonos),0))>0 and tpr_prov_id not in (       ";
        where6_1 += " select tpr_prov_id from aux_prov_id where tpr_proveedor_id=" + clienteID + " and tpr_tpi_id=" + tpi + " and tpr_pai_id=" + user.PaisID + " and tpr_mon_id=" + monID + " and ";
        where6_1 += " tpr_tcon_id=" + contaID + " and  tcg_fecha_emision<='" + fechafin + "'   ) ";

        //notas debito
        where7 = "  and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnd_pai_id=" + user.PaisID + "  and a.tnd_moneda=" + monID + " and a.tnd_tcon_id=" + contaID + " and a.tnd_ted_id<>3 and a.tnd_id not in  ";
        where7 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnd_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=4   ";
        where7 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where7 += "  and a.tnd_tpi_id=" + tpi + " and tnd_id not in   ( select tnd_id from aux_nd_id where   tnd_cli_id=" + clienteID + " and   tnd_tpi_id=" + tpi + " and tnd_pai_id=" + user.PaisID + " and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnd_contenedor,hbl,poliza, usuario ";

        //anuladas notas debito

        where7_1 = "  and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnd_pai_id=" + user.PaisID + "  and a.tnd_moneda=" + monID + " and a.tnd_tcon_id=" + contaID + " and a.tnd_ted_id=3 and a.tnd_id not in  ";
        where7_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnd_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=4   ";
        where7_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where7_1 += "  and a.tnd_tpi_id=" + tpi + " and tnd_id not in   ( select tnd_id from aux_nd_id where   tnd_cli_id=" + clienteID + " and   tnd_tpi_id=" + tpi + " and tnd_pai_id=" + user.PaisID + " and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnd_contenedor,hbl,poliza,usuario ";

        where7_2 = "  and  to_char(tfr_fecha_abono,'yyyy-mm-dd')>'" + fechafin + "'  and tnd_pai_id=" + user.PaisID + "  and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + " and tnd_ted_id=3 and tnd_fecha_emision <='" + fechafin + "'  and tnd_id not in  ";
        where7_2 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnd_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=4   ";
        where7_2 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where7_2 += "  and tnd_tpi_id=" + tpi + " and tnd_id not in   ( select tnd_id from aux_nd_id where   tnd_cli_id=" + clienteID + " and   tnd_tpi_id=" + tpi + " and tnd_pai_id=" + user.PaisID + " and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )    ";

        //notas credito
        where8 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + user.PaisID + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id<>3 and a.tnc_id not in  ";
        where8 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id in (12,31)   ";
        where8 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where8 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + user.PaisID + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnc_contenedor,hbl,poliza,usuario ";

        //nc directa
        where8_2 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + user.PaisID + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id<>3 and a.tnc_id not in  ";
        where8_2 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=12   ";
        where8_2 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where8_2 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + user.PaisID + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by a.tnc_id ";

        //anuladas notas credito pv
        where8_1 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + user.PaisID + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id=3 and a.tnc_id not in  ";
        where8_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id in (12,31)   ";
        where8_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where8_1 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + user.PaisID + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnc_contenedor,hbl,poliza,usuario,a.tnc_id ";

        //anuladas cheques
        where5_1 = " and    a.tcg_ted_id =3  and a.tcg_tcon_id =" + contaID + " and a.tcg_pai_id =" + user.PaisID + " and a.tcg_ttm_id =" + monID + "  and a.tcg_tpi_id=" + tpi + "  and a.tcg_fecha_emision >='" + fechaini + "'  and  a.tcg_fecha_emision <='" + fechafin + "'  group by tcg_cuenta,tcg_numero ,tcg_fecha_emision,tcg_valor,poliza,usuario ";
        //recibos soas anulados
        where4_1 = "  and a.trc_ted_id = 3  and a.trc_tcon_id =" + contaID + " and a.trc_pai_id =" + user.PaisID + " and  a.trc_moneda_id =" + monID + "  and a.trc_tpi_id=" + tpi + " and a.trc_fecha_emision>='" + fechaini + "' and a.trc_fecha_emision<='" + fechafin + "'   group by trc_serie,trc_correlativo,trc_fecha_emision,trc_contenedor,trc_hbl,poliza,usuario ";
        //recibos a favor proveedor
        where4_2 = "  and a.trc_ted_id <> 3  and a.trc_tcon_id =" + contaID + " and a.trc_pai_id =" + user.PaisID + " and  a.trc_moneda_id =" + monID + "  and a.trc_tpi_id=" + tpi + " and a.trc_fecha_emision>='" + fechaini + "' and a.trc_fecha_emision<='" + fechafin + "'   group by trc_serie,trc_correlativo,trc_fecha_emision,trc_contenedor,trc_hbl,poliza,usuario ";
        where4_2 += " having  coalesce(sum(cast(trc_monto as numeric) - coalesce((select sum(tcr_abono) from (select sum(tcr_abono) as tcr_abono  from tbl_corte_abono where tcr_tre_id=trc_id and tcr_sysref_id = 22 and to_char(tcr_fecha_abono,'yyyy-mm-dd')<='" + fechafin + " 23:59:59')abono),0)))  > 0 ";
        //Cheques liquidaciones
        where5_2 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id<>3 ";
        if (fechaini != null && !fechaini.Equals("")) where5_2 += " and tcg_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where5_2 += " and tcg_fecha_emision<='" + fechafin + "'";
        where5_2 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is null ";
        where5_3 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=14 ";
        if (fechaini != null && !fechaini.Equals("")) where5_3 += " and tcg_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where5_3 += " and tcg_fecha_emision<='" + fechafin + "'";
        where5_3 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is null ";
        //transferencia liquidaciones
        where5_4 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id<>3 ";
        if (fechaini != null && !fechaini.Equals("")) where5_4 += " and tcg_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where5_4 += " and tcg_fecha_emision<='" + fechafin + "'";
        where5_4 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is not null ";
        where5_5 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=14 ";
        if (fechaini != null && !fechaini.Equals("")) where5_5 += " and tcg_fecha_emision>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where5_5 += " and tcg_fecha_emision<='" + fechafin + "'";
        where5_5 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is not null ";
        //depositos liquidaciones
        where5_6 = " and c.tli_ted_id <>3    and tli_conta_id=" + contaID + " and tli_tpi_id=" + tpi + " and tmb_ted_id<>3   and tli_pai_id=" + user.PaisID + " and tmb_mon_id=" + monID + " ";
        if (fechaini != null && !fechaini.Equals("")) where5_6 += " and tmb_fecha>='" + fechaini + "'";
        if (fechafin != null && !fechafin.Equals("")) where5_6 += " and tmb_fecha<='" + fechafin + "'";


        //Facturas intercompnay
        where9 = "  and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tfa_pai_id=" + user.PaisID + "  and a.tfa_moneda=" + monID + " and a.tfa_conta_id=" + contaID + " and a.tfa_ted_id<>3 and a.tfa_id not in  ";
        where9 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tfa_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=1   ";
        where9 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where9 += "  and a.tfa_tpi_id=" + tpi + " and tfa_id not in   ( select tfa_id from aux_fa_id where   tfa_cli_id=" + clienteID + " and   tfa_tpi_id=" + tpi + " and tfa_pai_id=" + user.PaisID + " and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tfa_contenedor,hbl,poliza, usuario ";

        //anuladas facturas intercompany

        where9_1 = "  and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tfa_pai_id=" + user.PaisID + "  and a.tfa_moneda=" + monID + " and a.tfa_conta_id=" + contaID + " and a.tfa_ted_id=3 and a.tfa_id not in  ";
        where9_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tfa_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=1   ";
        where9_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where9_1 += "  and a.tfa_tpi_id=" + tpi + " and tfa_id not in   ( select tfa_id from aux_fa_id where   tfa_cli_id=" + clienteID + " and   tfa_tpi_id=" + tpi + " and tfa_pai_id=" + user.PaisID + " and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tfa_contenedor,hbl,poliza,usuario ";

        where9_2 = "  and  to_char(tfr_fecha_abono,'yyyy-mm-dd')>'" + fechafin + "'  and tfa_pai_id=" + user.PaisID + "  and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + " and tfa_ted_id=3 and tfa_fecha_emision <='" + fechafin + "'  and tfa_id not in  ";
        where9_2 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tfa_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=1   ";
        where9_2 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where9_2 += "  and tfa_tpi_id=" + tpi + " and tfa_id not in   ( select tfa_id from aux_fa_id where   tfa_cli_id=" + clienteID + " and   tfa_tpi_id=" + tpi + " and tfa_pai_id=" + user.PaisID + " and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + "  and fecha<='" + fechafin + "'    )    ";

        //anuladas notas credito pv
        where10_1 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + user.PaisID + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id=3 and a.tnc_id not in  ";
        where10_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id in (3)   ";
        where10_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
        where10_1 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + user.PaisID + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )  and tnc_ttr_id=3   group by serie,correlavito,fecha2,tnc_contenedor,hbl,poliza,usuario,a.tnc_id ";



        ds = DB.getEstadoCuentaProveedor(clienteID, where, where2, where3, where4, where5, fechafin, monID, solopendiente, tpi, where6, where7, where6_1, where7_1, where8, where8_1, where5_1, where4_1, where4_2, where5_2, where5_3, where5_4, where5_5, where5_6, where8_2, where7_2, where9, where9_1, where9_2, where10_1, user.PaisID);

    }

    protected void bt_generar_pdf_Click(object sender, EventArgs e)
    {
        if (tb_agenteID.Text.Trim().Equals("0") || (tb_agenteID.Text.Trim().Equals("") || (tb_agentenombre.Text.Trim().Equals(""))))
        {
            WebMsgBox.Show("Ingrese el nombre para Generar el Reporte");
            return;
        }
        if (tb_fechaini.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha inicial");
            return;
        }
        if (tb_fechafin.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha final");
            return;
        }

        string Imagenpath = "";

        if (user.pais.Grupo_Empresas == 2)
        {
            Imagenpath = user.pais.Imagepath;
        }
        else if (user.PaisID == 11)
        {
            Imagenpath = user.pais.Imagepath;
        }
        else if (user.PaisID == 12)
        {
            Imagenpath = user.pais.Imagepath;
        }
        else if (user.PaisID == 13)
        {
            Imagenpath = user.pais.Imagepath;
        }
        else if (user.PaisID == 30)
        {
            Imagenpath = user.pais.Imagepath;
        }
        else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
        {
            Imagenpath = "~/img/aimar.jpg";
        }
        else
        {
            if (user.contaID == 1)
            {
                Imagenpath = "~/img/aimar.jpg";
            }
            else
            {
                Imagenpath = "~/img/aimar_en.jpg";
            }
        }

        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "ESTADO DE CUENTA", ""); //pais, sistema, doc_id, titulo, edicion
        MemoryStream ms = new MemoryStream(Params.logo);

        if (lb_tipopersona.SelectedValue == "3")
        {

            #region estdo cuenta cliente
            obtengo_datos_cliente();

            Document doc = new Document(PageSize.LEGAL.Rotate());

            // Indicamos donde vamos a guardar el documento
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/EstadosCuenta/EstadoCuentaCliente_" + tb_agenteID.Text + ".pdf"), FileMode.Create));

            //iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(Server.MapPath(Imagenpath));
            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(ms);
            
            imagen.BorderWidth = 0;
            imagen.Alignment = Element.ALIGN_LEFT;
            float percentage = 0.0f;
            percentage = 150 / imagen.Width;
            imagen.ScalePercent(percentage * 100);

            //doc.AddTitle("Estado de Cuenta");
            //doc.AddCreator("Aimar Group");
            doc.AddTitle(Params.titulo); // + " " + lb_tipopersona.SelectedValue);
            doc.AddCreator(Params.firma);

            doc.Open();
            doc.Add(imagen);

            Font _standardFont = new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK);
            Font _standardFontHeader = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
            Font SubTitleFont = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK);
            Font TitleFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.BLACK);
            Font WhiteFont = new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.WHITE);

            //doc.Add(new Phrase("ESTADO DE CUENTA", TitleFont));
            doc.Add(new Phrase(Params.titulo, TitleFont));

            doc.Add(Chunk.NEWLINE);

            doc.Add(new Phrase("Cliente:   ", SubTitleFont));
            string sql_clientes = "";
            string clienteNombre = "";
            sql_clientes = " a.id_cliente=" + tb_agenteID.Text;
            ArrayList Arr_Cliente = (ArrayList)DB.getClientes(sql_clientes, user, "REPORTES");
            foreach (RE_GenericBean Bean_Cliente in Arr_Cliente)
            {
                clienteNombre = Bean_Cliente.strC1;
            }
            doc.Add(new Phrase(tb_agenteID.Text, _standardFontHeader));
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Phrase("Nombre:   ", SubTitleFont));
            doc.Add(new Phrase(clienteNombre, _standardFontHeader));
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Phrase("Impresion de saldo pendiente de pago          ", _standardFontHeader));
            doc.Add(new Phrase("Desde:   ", SubTitleFont));
            doc.Add(new Phrase(tb_fechaini.Text + "   ", _standardFontHeader));
            doc.Add(new Phrase("Hasta:   ", SubTitleFont));
            doc.Add(new Phrase(tb_fechafin.Text + "   ", _standardFontHeader));
            doc.Add(new Phrase("Valores en:   ", SubTitleFont));
            doc.Add(new Phrase(simbolomoneda, _standardFontHeader));




            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblEstadoCuentaTitulos = new PdfPTable(16);
            tblEstadoCuentaTitulos.WidthPercentage = 100;
            PdfPTable tblEstadoCuentaFacturas = new PdfPTable(16);
            tblEstadoCuentaFacturas.WidthPercentage = 100;
            PdfPTable tblEstadoCuentaNotasDebito = new PdfPTable(16);
            tblEstadoCuentaNotasDebito.WidthPercentage = 100;
            PdfPTable tblEstadoCuentaRecibos = new PdfPTable(16);
            tblEstadoCuentaRecibos.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell TIPO = new PdfPCell(new Phrase("TIPO", WhiteFont));
            TIPO.BorderWidth = 0;
            TIPO.BackgroundColor = BaseColor.BLACK;
            TIPO.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell SERIE_FACT = new PdfPCell(new Phrase("SERIE", WhiteFont));
            SERIE_FACT.BorderWidth = 0;
            SERIE_FACT.BackgroundColor = BaseColor.BLACK;
            SERIE_FACT.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell CORR_FACT = new PdfPCell(new Phrase("CORR.", WhiteFont));
            CORR_FACT.BorderWidth = 0;
            CORR_FACT.BackgroundColor = BaseColor.BLACK;
            CORR_FACT.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell FECHA_FACT = new PdfPCell(new Phrase("FECHA", WhiteFont));
            FECHA_FACT.BorderWidth = 0;
            FECHA_FACT.BackgroundColor = BaseColor.BLACK;
            FECHA_FACT.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell FECHA_PAGO = new PdfPCell(new Phrase("VENCE", WhiteFont));
            FECHA_PAGO.BorderWidth = 0;
            FECHA_PAGO.BackgroundColor = BaseColor.BLACK;
            FECHA_PAGO.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell TOTAL_FACT = new PdfPCell(new Phrase("TOTAL", WhiteFont));
            TOTAL_FACT.BorderWidth = 0;
            TOTAL_FACT.BackgroundColor = BaseColor.BLACK;
            TOTAL_FACT.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell SERIE_REC = new PdfPCell(new Phrase("SERIE", WhiteFont));
            SERIE_REC.BorderWidth = 0;
            SERIE_REC.BackgroundColor = BaseColor.BLACK;
            SERIE_REC.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell CORR_REC = new PdfPCell(new Phrase("CORR.", WhiteFont));
            CORR_REC.BorderWidth = 0;
            CORR_REC.BackgroundColor = BaseColor.BLACK;
            CORR_REC.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell FECHA_REC = new PdfPCell(new Phrase("FECHA", WhiteFont));
            FECHA_REC.BorderWidth = 0;
            FECHA_REC.BackgroundColor = BaseColor.BLACK;
            FECHA_REC.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell ABONO_REC = new PdfPCell(new Phrase("ABONO", WhiteFont));
            ABONO_REC.BorderWidth = 0;
            ABONO_REC.BackgroundColor = BaseColor.BLACK;
            ABONO_REC.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell SALDO = new PdfPCell(new Phrase("SALDO", WhiteFont));
            SALDO.BorderWidth = 0;
            SALDO.BackgroundColor = BaseColor.BLACK;
            SALDO.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell CONTENEDOR = new PdfPCell(new Phrase("CONTENEDOR", WhiteFont));
            CONTENEDOR.BorderWidth = 0;
            CONTENEDOR.BackgroundColor = BaseColor.BLACK;
            CONTENEDOR.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell HBL = new PdfPCell(new Phrase("HBL", WhiteFont));
            HBL.BorderWidth = 0;
            HBL.BackgroundColor = BaseColor.BLACK;
            HBL.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell SERVICIO = new PdfPCell(new Phrase("SERVICIO", WhiteFont));
            SERVICIO.BorderWidth = 0;
            SERVICIO.BackgroundColor = BaseColor.BLACK;
            SERVICIO.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell POLIZA = new PdfPCell(new Phrase("POLIZA", WhiteFont));
            POLIZA.BorderWidth = 0;
            POLIZA.BackgroundColor = BaseColor.BLACK;
            POLIZA.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell USUARIO = new PdfPCell(new Phrase("USUARIO", WhiteFont));
            USUARIO.BorderWidth = 0;
            USUARIO.BackgroundColor = BaseColor.BLACK;
            USUARIO.BorderWidthBottom = 0.75f;

            tblEstadoCuentaTitulos.AddCell(TIPO);
            tblEstadoCuentaTitulos.AddCell(SERIE_FACT);
            tblEstadoCuentaTitulos.AddCell(CORR_FACT);
            tblEstadoCuentaTitulos.AddCell(FECHA_FACT);
            tblEstadoCuentaTitulos.AddCell(FECHA_PAGO);
            tblEstadoCuentaTitulos.AddCell(TOTAL_FACT);
            tblEstadoCuentaTitulos.AddCell(SERIE_REC);
            tblEstadoCuentaTitulos.AddCell(CORR_REC);
            tblEstadoCuentaTitulos.AddCell(FECHA_REC);
            tblEstadoCuentaTitulos.AddCell(ABONO_REC);
            tblEstadoCuentaTitulos.AddCell(SALDO);
            tblEstadoCuentaTitulos.AddCell(CONTENEDOR);
            tblEstadoCuentaTitulos.AddCell(HBL);
            tblEstadoCuentaTitulos.AddCell(SERVICIO);
            tblEstadoCuentaTitulos.AddCell(POLIZA);
            tblEstadoCuentaTitulos.AddCell(USUARIO);

            decimal total_facturas = 0;
            decimal total_notas_debito = 0;
            decimal total_recibos = 0;
            decimal total_abono_facturas = 0;
            decimal total_abono_notas_debito = 0;
            decimal total_abono_recibos = 0;
            decimal total_saldo_facturas = 0;
            decimal total_saldo_notas_debito = 0;
            decimal total_saldo_recibos = 0;
            decimal total_saldo_cliente = 0;
            int ver_header_facturas = 0;
            int ver_header_notas_debito = 0;
            int ver_header_recibos = 0;
            int id_factura = 0;
            int id_factura2 = 0;
            int id_nd = 0;
            int id_nd2 = 0;
            int id_recibo = 0;
            int id_recibo2 = 0;
            decimal saldo_facturas = 0;
            decimal saldo_notas_debito = 0;
            decimal saldo_recibos = 0;
            int alternar = 1;
            BaseColor color;

            foreach (DataRow dr in ds.Tables["estadocuenta_detallado_tbl"].Rows)
            {
                if (alternar == 2)
                {
                    color = BaseColor.LIGHT_GRAY;
                    alternar = 1;
                }
                else
                {
                    color = BaseColor.WHITE;
                    alternar = 2;
                }
                if (dr["tipo"].ToString() == "F")
                {
                    #region datos facuras
                    if (ver_header_facturas == 0)
                    {
                        #region header facturas
                        TIPO = new PdfPCell(new Phrase("F", _standardFont));
                        TIPO.BorderWidth = 0;
                        TIPO.BackgroundColor = BaseColor.GRAY;
                        SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_FACT.BorderWidth = 0;
                        SERIE_FACT.BackgroundColor = BaseColor.GRAY;
                        CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                        CORR_FACT.BorderWidth = 0;
                        CORR_FACT.BackgroundColor = BaseColor.GRAY;
                        FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_FACT.BorderWidth = 0;
                        FECHA_FACT.BackgroundColor = BaseColor.GRAY;

                        FECHA_PAGO = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_PAGO.BorderWidth = 0;
                        FECHA_PAGO.BackgroundColor = BaseColor.GRAY;

                        TOTAL_FACT = new PdfPCell(new Phrase("", _standardFont));
                        TOTAL_FACT.BorderWidth = 0;
                        TOTAL_FACT.BackgroundColor = BaseColor.GRAY;
                        SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_REC.BorderWidth = 0;
                        SERIE_REC.BackgroundColor = BaseColor.GRAY;
                        CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                        CORR_REC.BorderWidth = 0;
                        CORR_REC.BackgroundColor = BaseColor.GRAY;
                        FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_REC.BorderWidth = 0;
                        FECHA_REC.BackgroundColor = BaseColor.GRAY;
                        ABONO_REC = new PdfPCell(new Phrase("", _standardFont));
                        ABONO_REC.BorderWidth = 0;
                        ABONO_REC.BackgroundColor = BaseColor.GRAY;
                        SALDO = new PdfPCell(new Phrase("", _standardFont));
                        SALDO.BorderWidth = 0;
                        SALDO.BackgroundColor = BaseColor.GRAY;
                        CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                        CONTENEDOR.BorderWidth = 0;
                        CONTENEDOR.BackgroundColor = BaseColor.GRAY;
                        HBL = new PdfPCell(new Phrase("", _standardFont));
                        HBL.BorderWidth = 0;
                        HBL.BackgroundColor = BaseColor.GRAY;
                        SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                        SERVICIO.BorderWidth = 0;
                        SERVICIO.BackgroundColor = BaseColor.GRAY;
                        POLIZA = new PdfPCell(new Phrase("", _standardFont));
                        POLIZA.BorderWidth = 0;
                        POLIZA.BackgroundColor = BaseColor.GRAY;
                        USUARIO = new PdfPCell(new Phrase("", _standardFont));
                        USUARIO.BorderWidth = 0;
                        USUARIO.BackgroundColor = BaseColor.GRAY;
                        tblEstadoCuentaFacturas.AddCell(TIPO);
                        tblEstadoCuentaFacturas.AddCell(SERIE_FACT);
                        tblEstadoCuentaFacturas.AddCell(CORR_FACT);
                        tblEstadoCuentaFacturas.AddCell(FECHA_FACT);
                        tblEstadoCuentaFacturas.AddCell(FECHA_PAGO);
                        tblEstadoCuentaFacturas.AddCell(TOTAL_FACT);
                        tblEstadoCuentaFacturas.AddCell(SERIE_REC);
                        tblEstadoCuentaFacturas.AddCell(CORR_REC);
                        tblEstadoCuentaFacturas.AddCell(FECHA_REC);
                        tblEstadoCuentaFacturas.AddCell(ABONO_REC);
                        tblEstadoCuentaFacturas.AddCell(SALDO);
                        tblEstadoCuentaFacturas.AddCell(CONTENEDOR);
                        tblEstadoCuentaFacturas.AddCell(HBL);
                        tblEstadoCuentaFacturas.AddCell(SERVICIO);
                        tblEstadoCuentaFacturas.AddCell(POLIZA);
                        tblEstadoCuentaFacturas.AddCell(USUARIO);
                        #endregion
                    }

                    id_factura = int.Parse(dr["id_fact"].ToString());

                    if (ver_header_facturas > 0)
                    {
                        if (id_factura != id_factura2)
                        {
                            saldo_facturas += total_saldo_facturas;
                            total_saldo_facturas = 0;
                        }
                    }

                    TIPO = new PdfPCell(new Phrase(dr["tipo"].ToString(), _standardFont));
                    TIPO.BorderWidth = 0;
                    TIPO.BackgroundColor = color;
                    SERIE_FACT = new PdfPCell(new Phrase(dr["serie_fact"].ToString(), _standardFont));
                    SERIE_FACT.BorderWidth = 0;
                    SERIE_FACT.BackgroundColor = color;
                    CORR_FACT = new PdfPCell(new Phrase(dr["corr_fact"].ToString(), _standardFont));
                    CORR_FACT.BorderWidth = 0;
                    CORR_FACT.BackgroundColor = color;
                    FECHA_FACT = new PdfPCell(new Phrase(dr["fecha_fact"].ToString(), _standardFont));
                    FECHA_FACT.BorderWidth = 0;
                    FECHA_FACT.BackgroundColor = color;
                    FECHA_PAGO = new PdfPCell(new Phrase(dr["fecha_pago"].ToString(), _standardFont));
                    FECHA_PAGO.BorderWidth = 0;
                    FECHA_PAGO.BackgroundColor = color;
                    TOTAL_FACT = new PdfPCell(new Phrase(decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00"), _standardFont));
                    TOTAL_FACT.BorderWidth = 0;
                    TOTAL_FACT.BackgroundColor = color;
                    SERIE_REC = new PdfPCell(new Phrase(dr["serie_rcpt"].ToString(), _standardFont));
                    SERIE_REC.BorderWidth = 0;
                    SERIE_REC.BackgroundColor = color;
                    CORR_REC = new PdfPCell(new Phrase(dr["corr_rcpt"].ToString(), _standardFont));
                    CORR_REC.BorderWidth = 0;
                    CORR_REC.BackgroundColor = color;
                    FECHA_REC = new PdfPCell(new Phrase(dr["fecha_rcpt"].ToString(), _standardFont));
                    FECHA_REC.BorderWidth = 0;
                    FECHA_REC.BackgroundColor = color;
                    ABONO_REC = new PdfPCell(new Phrase(decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00"), _standardFont));
                    ABONO_REC.BorderWidth = 0;
                    ABONO_REC.BackgroundColor = color;
                    SALDO = new PdfPCell(new Phrase(decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00"), _standardFont));
                    SALDO.BorderWidth = 0;
                    SALDO.BackgroundColor = color;
                    CONTENEDOR = new PdfPCell(new Phrase(dr["contenedor"].ToString(), _standardFont));
                    CONTENEDOR.BorderWidth = 0;
                    CONTENEDOR.BackgroundColor = color;
                    HBL = new PdfPCell(new Phrase(dr["hbl"].ToString(), _standardFont));
                    HBL.BorderWidth = 0;
                    HBL.BackgroundColor = color;
                    SERVICIO = new PdfPCell(new Phrase(dr["servicio"].ToString(), _standardFont));
                    SERVICIO.BorderWidth = 0;
                    SERVICIO.BackgroundColor = color;
                    POLIZA = new PdfPCell(new Phrase(dr["poliza"].ToString(), _standardFont));
                    POLIZA.BorderWidth = 0;
                    POLIZA.BackgroundColor = color;
                    USUARIO = new PdfPCell(new Phrase(dr["usuario"].ToString(), _standardFont));
                    USUARIO.BorderWidth = 0;
                    USUARIO.BackgroundColor = color;
                    tblEstadoCuentaFacturas.AddCell(TIPO);
                    tblEstadoCuentaFacturas.AddCell(SERIE_FACT);
                    tblEstadoCuentaFacturas.AddCell(CORR_FACT);
                    tblEstadoCuentaFacturas.AddCell(FECHA_FACT);
                    tblEstadoCuentaFacturas.AddCell(FECHA_PAGO);
                    tblEstadoCuentaFacturas.AddCell(TOTAL_FACT);
                    tblEstadoCuentaFacturas.AddCell(SERIE_REC);
                    tblEstadoCuentaFacturas.AddCell(CORR_REC);
                    tblEstadoCuentaFacturas.AddCell(FECHA_REC);
                    tblEstadoCuentaFacturas.AddCell(ABONO_REC);
                    tblEstadoCuentaFacturas.AddCell(SALDO);
                    tblEstadoCuentaFacturas.AddCell(CONTENEDOR);
                    tblEstadoCuentaFacturas.AddCell(HBL);
                    tblEstadoCuentaFacturas.AddCell(SERVICIO);
                    tblEstadoCuentaFacturas.AddCell(POLIZA);
                    tblEstadoCuentaFacturas.AddCell(USUARIO);

                    id_factura2 = int.Parse(dr["id_fact"].ToString());

                    ver_header_facturas++;
                    total_facturas += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_facturas += decimal.Parse(dr["abono_rcpt"].ToString());
                    total_saldo_facturas = decimal.Parse(dr["saldo"].ToString());
                    #endregion
                }
                if (dr["tipo"].ToString() == "ND")
                {
                    #region datos notas debito
                    if (ver_header_notas_debito == 0)
                    {
                        #region header notas debito
                        TIPO = new PdfPCell(new Phrase("ND", _standardFont));
                        TIPO.BorderWidth = 0;
                        TIPO.BackgroundColor = BaseColor.GRAY;
                        SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_FACT.BorderWidth = 0;
                        SERIE_FACT.BackgroundColor = BaseColor.GRAY;
                        CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                        CORR_FACT.BorderWidth = 0;
                        CORR_FACT.BackgroundColor = BaseColor.GRAY;
                        FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_FACT.BorderWidth = 0;
                        FECHA_FACT.BackgroundColor = BaseColor.GRAY;
                        FECHA_PAGO = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_PAGO.BorderWidth = 0;
                        FECHA_PAGO.BackgroundColor = BaseColor.GRAY;
                        TOTAL_FACT = new PdfPCell(new Phrase("", _standardFont));
                        TOTAL_FACT.BorderWidth = 0;
                        TOTAL_FACT.BackgroundColor = BaseColor.GRAY;
                        SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_REC.BorderWidth = 0;
                        SERIE_REC.BackgroundColor = BaseColor.GRAY;
                        CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                        CORR_REC.BorderWidth = 0;
                        CORR_REC.BackgroundColor = BaseColor.GRAY;
                        FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_REC.BorderWidth = 0;
                        FECHA_REC.BackgroundColor = BaseColor.GRAY;
                        ABONO_REC = new PdfPCell(new Phrase("", _standardFont));
                        ABONO_REC.BorderWidth = 0;
                        ABONO_REC.BackgroundColor = BaseColor.GRAY;
                        SALDO = new PdfPCell(new Phrase("", _standardFont));
                        SALDO.BorderWidth = 0;
                        SALDO.BackgroundColor = BaseColor.GRAY;
                        CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                        CONTENEDOR.BorderWidth = 0;
                        CONTENEDOR.BackgroundColor = BaseColor.GRAY;
                        HBL = new PdfPCell(new Phrase("", _standardFont));
                        HBL.BorderWidth = 0;
                        HBL.BackgroundColor = BaseColor.GRAY;
                        SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                        SERVICIO.BorderWidth = 0;
                        SERVICIO.BackgroundColor = BaseColor.GRAY;
                        POLIZA = new PdfPCell(new Phrase("", _standardFont));
                        POLIZA.BorderWidth = 0;
                        POLIZA.BackgroundColor = BaseColor.GRAY;
                        USUARIO = new PdfPCell(new Phrase("", _standardFont));
                        USUARIO.BorderWidth = 0;
                        USUARIO.BackgroundColor = BaseColor.GRAY;
                        tblEstadoCuentaNotasDebito.AddCell(TIPO);
                        tblEstadoCuentaNotasDebito.AddCell(SERIE_FACT);
                        tblEstadoCuentaNotasDebito.AddCell(CORR_FACT);
                        tblEstadoCuentaNotasDebito.AddCell(FECHA_FACT);
                        tblEstadoCuentaNotasDebito.AddCell(FECHA_PAGO);
                        tblEstadoCuentaNotasDebito.AddCell(TOTAL_FACT);
                        tblEstadoCuentaNotasDebito.AddCell(SERIE_REC);
                        tblEstadoCuentaNotasDebito.AddCell(CORR_REC);
                        tblEstadoCuentaNotasDebito.AddCell(FECHA_REC);
                        tblEstadoCuentaNotasDebito.AddCell(ABONO_REC);
                        tblEstadoCuentaNotasDebito.AddCell(SALDO);
                        tblEstadoCuentaNotasDebito.AddCell(CONTENEDOR);
                        tblEstadoCuentaNotasDebito.AddCell(HBL);
                        tblEstadoCuentaNotasDebito.AddCell(SERVICIO);
                        tblEstadoCuentaNotasDebito.AddCell(POLIZA);
                        tblEstadoCuentaNotasDebito.AddCell(USUARIO);
                        #endregion
                    }

                    id_nd = int.Parse(dr["id_fact"].ToString());

                    if (ver_header_notas_debito > 0)
                    {
                        if (id_nd != id_nd2)
                        {
                            saldo_notas_debito += total_saldo_notas_debito;
                            total_saldo_notas_debito = 0;
                        }
                    }

                    TIPO = new PdfPCell(new Phrase(dr["tipo"].ToString(), _standardFont));
                    TIPO.BorderWidth = 0;
                    TIPO.BackgroundColor = color;
                    SERIE_FACT = new PdfPCell(new Phrase(dr["serie_fact"].ToString(), _standardFont));
                    SERIE_FACT.BorderWidth = 0;
                    SERIE_FACT.BackgroundColor = color;
                    CORR_FACT = new PdfPCell(new Phrase(dr["corr_fact"].ToString(), _standardFont));
                    CORR_FACT.BorderWidth = 0;
                    CORR_FACT.BackgroundColor = color;
                    FECHA_FACT = new PdfPCell(new Phrase(dr["fecha_fact"].ToString(), _standardFont));
                    FECHA_FACT.BorderWidth = 0;
                    FECHA_FACT.BackgroundColor = color;

                    FECHA_PAGO = new PdfPCell(new Phrase(dr["fecha_pago"].ToString(), _standardFont));
                    FECHA_PAGO.BorderWidth = 0;
                    FECHA_PAGO.BackgroundColor = color;

                    TOTAL_FACT = new PdfPCell(new Phrase(decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00"), _standardFont));
                    TOTAL_FACT.BorderWidth = 0;
                    TOTAL_FACT.BackgroundColor = color;
                    SERIE_REC = new PdfPCell(new Phrase(dr["serie_rcpt"].ToString(), _standardFont));
                    SERIE_REC.BorderWidth = 0;
                    SERIE_REC.BackgroundColor = color;
                    CORR_REC = new PdfPCell(new Phrase(dr["corr_rcpt"].ToString(), _standardFont));
                    CORR_REC.BorderWidth = 0;
                    CORR_REC.BackgroundColor = color;
                    FECHA_REC = new PdfPCell(new Phrase(dr["fecha_rcpt"].ToString(), _standardFont));
                    FECHA_REC.BorderWidth = 0;
                    FECHA_REC.BackgroundColor = color;
                    ABONO_REC = new PdfPCell(new Phrase(decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00"), _standardFont));
                    ABONO_REC.BorderWidth = 0;
                    ABONO_REC.BackgroundColor = color;
                    SALDO = new PdfPCell(new Phrase(decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00"), _standardFont));
                    SALDO.BorderWidth = 0;
                    SALDO.BackgroundColor = color;
                    CONTENEDOR = new PdfPCell(new Phrase(dr["contenedor"].ToString(), _standardFont));
                    CONTENEDOR.BorderWidth = 0;
                    CONTENEDOR.BackgroundColor = color;
                    HBL = new PdfPCell(new Phrase(dr["hbl"].ToString(), _standardFont));
                    HBL.BorderWidth = 0;
                    HBL.BackgroundColor = color;
                    SERVICIO = new PdfPCell(new Phrase(dr["servicio"].ToString(), _standardFont));
                    SERVICIO.BorderWidth = 0;
                    SERVICIO.BackgroundColor = color;
                    POLIZA = new PdfPCell(new Phrase(dr["poliza"].ToString(), _standardFont));
                    POLIZA.BorderWidth = 0;
                    POLIZA.BackgroundColor = color;
                    USUARIO = new PdfPCell(new Phrase(dr["usuario"].ToString(), _standardFont));
                    USUARIO.BorderWidth = 0;
                    USUARIO.BackgroundColor = color;
                    tblEstadoCuentaNotasDebito.AddCell(TIPO);
                    tblEstadoCuentaNotasDebito.AddCell(SERIE_FACT);
                    tblEstadoCuentaNotasDebito.AddCell(CORR_FACT);
                    tblEstadoCuentaNotasDebito.AddCell(FECHA_FACT);
                    tblEstadoCuentaNotasDebito.AddCell(FECHA_PAGO);
                    tblEstadoCuentaNotasDebito.AddCell(TOTAL_FACT);
                    tblEstadoCuentaNotasDebito.AddCell(SERIE_REC);
                    tblEstadoCuentaNotasDebito.AddCell(CORR_REC);
                    tblEstadoCuentaNotasDebito.AddCell(FECHA_REC);
                    tblEstadoCuentaNotasDebito.AddCell(ABONO_REC);
                    tblEstadoCuentaNotasDebito.AddCell(SALDO);
                    tblEstadoCuentaNotasDebito.AddCell(CONTENEDOR);
                    tblEstadoCuentaNotasDebito.AddCell(HBL);
                    tblEstadoCuentaNotasDebito.AddCell(SERVICIO);
                    tblEstadoCuentaNotasDebito.AddCell(POLIZA);
                    tblEstadoCuentaNotasDebito.AddCell(USUARIO);

                    id_nd2 = int.Parse(dr["id_fact"].ToString());

                    ver_header_notas_debito++;
                    total_notas_debito += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_notas_debito += decimal.Parse(dr["abono_rcpt"].ToString());
                    total_saldo_notas_debito = decimal.Parse(dr["saldo"].ToString());
                    #endregion
                }
                if (dr["tipo"].ToString() == "R")
                {
                    #region datos recibo
                    if (ver_header_recibos == 0)
                    {
                        #region header recibos
                        TIPO = new PdfPCell(new Phrase("R", _standardFont));
                        TIPO.BorderWidth = 0;
                        TIPO.BackgroundColor = BaseColor.GRAY;
                        SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_FACT.BorderWidth = 0;
                        SERIE_FACT.BackgroundColor = BaseColor.GRAY;
                        CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                        CORR_FACT.BorderWidth = 0;
                        CORR_FACT.BackgroundColor = BaseColor.GRAY;
                        FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_FACT.BorderWidth = 0;
                        FECHA_FACT.BackgroundColor = BaseColor.GRAY;
                        FECHA_PAGO = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_PAGO.BorderWidth = 0;
                        FECHA_PAGO.BackgroundColor = BaseColor.GRAY;
                        TOTAL_FACT = new PdfPCell(new Phrase("", _standardFont));
                        TOTAL_FACT.BorderWidth = 0;
                        TOTAL_FACT.BackgroundColor = BaseColor.GRAY;
                        SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_REC.BorderWidth = 0;
                        SERIE_REC.BackgroundColor = BaseColor.GRAY;
                        CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                        CORR_REC.BorderWidth = 0;
                        CORR_REC.BackgroundColor = BaseColor.GRAY;
                        FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_REC.BorderWidth = 0;
                        FECHA_REC.BackgroundColor = BaseColor.GRAY;
                        ABONO_REC = new PdfPCell(new Phrase("", _standardFont));
                        ABONO_REC.BorderWidth = 0;
                        ABONO_REC.BackgroundColor = BaseColor.GRAY;
                        SALDO = new PdfPCell(new Phrase("", _standardFont));
                        SALDO.BorderWidth = 0;
                        SALDO.BackgroundColor = BaseColor.GRAY;
                        CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                        CONTENEDOR.BorderWidth = 0;
                        CONTENEDOR.BackgroundColor = BaseColor.GRAY;
                        HBL = new PdfPCell(new Phrase("", _standardFont));
                        HBL.BorderWidth = 0;
                        HBL.BackgroundColor = BaseColor.GRAY;
                        SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                        SERVICIO.BorderWidth = 0;
                        SERVICIO.BackgroundColor = BaseColor.GRAY;
                        POLIZA = new PdfPCell(new Phrase("", _standardFont));
                        POLIZA.BorderWidth = 0;
                        POLIZA.BackgroundColor = BaseColor.GRAY;
                        USUARIO = new PdfPCell(new Phrase("", _standardFont));
                        USUARIO.BorderWidth = 0;
                        USUARIO.BackgroundColor = BaseColor.GRAY;
                        tblEstadoCuentaRecibos.AddCell(TIPO);
                        tblEstadoCuentaRecibos.AddCell(SERIE_FACT);
                        tblEstadoCuentaRecibos.AddCell(CORR_FACT);
                        tblEstadoCuentaRecibos.AddCell(FECHA_FACT);
                        tblEstadoCuentaRecibos.AddCell(FECHA_PAGO);
                        tblEstadoCuentaRecibos.AddCell(TOTAL_FACT);
                        tblEstadoCuentaRecibos.AddCell(SERIE_REC);
                        tblEstadoCuentaRecibos.AddCell(CORR_REC);
                        tblEstadoCuentaRecibos.AddCell(FECHA_REC);
                        tblEstadoCuentaRecibos.AddCell(ABONO_REC);
                        tblEstadoCuentaRecibos.AddCell(SALDO);
                        tblEstadoCuentaRecibos.AddCell(CONTENEDOR);
                        tblEstadoCuentaRecibos.AddCell(HBL);
                        tblEstadoCuentaRecibos.AddCell(SERVICIO);
                        tblEstadoCuentaRecibos.AddCell(POLIZA);
                        tblEstadoCuentaRecibos.AddCell(USUARIO);
                        #endregion
                    }
                    id_recibo = int.Parse(dr["id_fact"].ToString());

                    if (ver_header_recibos > 0)
                    {
                        if (id_recibo != id_recibo2)
                        {
                            saldo_recibos += total_saldo_recibos;
                            total_saldo_recibos = 0;
                        }
                    }
                    TIPO = new PdfPCell(new Phrase(dr["tipo"].ToString(), _standardFont));
                    TIPO.BorderWidth = 0;
                    TIPO.BackgroundColor = color;
                    SERIE_FACT = new PdfPCell(new Phrase(dr["serie_fact"].ToString(), _standardFont));
                    SERIE_FACT.BorderWidth = 0;
                    SERIE_FACT.BackgroundColor = color;
                    CORR_FACT = new PdfPCell(new Phrase(dr["corr_fact"].ToString(), _standardFont));
                    CORR_FACT.BorderWidth = 0;
                    CORR_FACT.BackgroundColor = color;
                    FECHA_FACT = new PdfPCell(new Phrase(dr["fecha_fact"].ToString(), _standardFont));
                    FECHA_FACT.BorderWidth = 0;
                    FECHA_FACT.BackgroundColor = color;
                    FECHA_PAGO = new PdfPCell(new Phrase(dr["fecha_pago"].ToString(), _standardFont));
                    FECHA_PAGO.BorderWidth = 0;
                    FECHA_PAGO.BackgroundColor = color;
                    TOTAL_FACT = new PdfPCell(new Phrase(decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00"), _standardFont));
                    TOTAL_FACT.BorderWidth = 0;
                    TOTAL_FACT.BackgroundColor = color;
                    SERIE_REC = new PdfPCell(new Phrase(dr["serie_rcpt"].ToString(), _standardFont));
                    SERIE_REC.BorderWidth = 0;
                    SERIE_REC.BackgroundColor = color;
                    CORR_REC = new PdfPCell(new Phrase(dr["corr_rcpt"].ToString(), _standardFont));
                    CORR_REC.BorderWidth = 0;
                    CORR_REC.BackgroundColor = color;
                    FECHA_REC = new PdfPCell(new Phrase(dr["fecha_rcpt"].ToString(), _standardFont));
                    FECHA_REC.BorderWidth = 0;
                    FECHA_REC.BackgroundColor = color;
                    ABONO_REC = new PdfPCell(new Phrase(decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00"), _standardFont));
                    ABONO_REC.BorderWidth = 0;
                    ABONO_REC.BackgroundColor = color;
                    SALDO = new PdfPCell(new Phrase(decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00"), _standardFont));
                    SALDO.BorderWidth = 0;
                    SALDO.BackgroundColor = color;
                    CONTENEDOR = new PdfPCell(new Phrase(dr["contenedor"].ToString(), _standardFont));
                    CONTENEDOR.BorderWidth = 0;
                    CONTENEDOR.BackgroundColor = color;
                    HBL = new PdfPCell(new Phrase(dr["hbl"].ToString(), _standardFont));
                    HBL.BorderWidth = 0;
                    HBL.BackgroundColor = color;
                    SERVICIO = new PdfPCell(new Phrase(dr["servicio"].ToString(), _standardFont));
                    SERVICIO.BorderWidth = 0;
                    SERVICIO.BackgroundColor = color;
                    POLIZA = new PdfPCell(new Phrase(dr["poliza"].ToString(), _standardFont));
                    POLIZA.BorderWidth = 0;
                    POLIZA.BackgroundColor = color;
                    USUARIO = new PdfPCell(new Phrase(dr["usuario"].ToString(), _standardFont));
                    USUARIO.BorderWidth = 0;
                    USUARIO.BackgroundColor = color;
                    tblEstadoCuentaRecibos.AddCell(TIPO);
                    tblEstadoCuentaRecibos.AddCell(SERIE_FACT);
                    tblEstadoCuentaRecibos.AddCell(CORR_FACT);
                    tblEstadoCuentaRecibos.AddCell(FECHA_FACT);
                    tblEstadoCuentaRecibos.AddCell(FECHA_PAGO);
                    tblEstadoCuentaRecibos.AddCell(TOTAL_FACT);
                    tblEstadoCuentaRecibos.AddCell(SERIE_REC);
                    tblEstadoCuentaRecibos.AddCell(CORR_REC);
                    tblEstadoCuentaRecibos.AddCell(FECHA_REC);
                    tblEstadoCuentaRecibos.AddCell(ABONO_REC);
                    tblEstadoCuentaRecibos.AddCell(SALDO);
                    tblEstadoCuentaRecibos.AddCell(CONTENEDOR);
                    tblEstadoCuentaRecibos.AddCell(HBL);
                    tblEstadoCuentaRecibos.AddCell(SERVICIO);
                    tblEstadoCuentaRecibos.AddCell(POLIZA);
                    tblEstadoCuentaRecibos.AddCell(USUARIO);

                    id_recibo2 = int.Parse(dr["id_fact"].ToString());

                    ver_header_recibos++;
                    total_recibos += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_recibos += decimal.Parse(dr["abono_rcpt"].ToString());
                    total_saldo_recibos = decimal.Parse(dr["saldo"].ToString());
                    #endregion
                }
            }

            if (ver_header_facturas > 0)
            {
                saldo_facturas += total_saldo_facturas;
                #region totales facturas
                TIPO = new PdfPCell(new Phrase("F", _standardFont));
                TIPO.BorderWidth = 0;
                TIPO.BackgroundColor = BaseColor.WHITE;
                SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                SERIE_FACT.BorderWidth = 0;
                SERIE_FACT.BackgroundColor = BaseColor.WHITE;
                CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                CORR_FACT.BorderWidth = 0;
                CORR_FACT.BackgroundColor = BaseColor.WHITE;
                FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                FECHA_FACT.BorderWidth = 0;
                FECHA_FACT.BackgroundColor = BaseColor.WHITE;
                FECHA_PAGO = new PdfPCell(new Phrase("", _standardFont));
                FECHA_PAGO.BorderWidth = 0;
                FECHA_PAGO.BackgroundColor = BaseColor.WHITE;
                TOTAL_FACT = new PdfPCell(new Phrase(total_facturas.ToString("#,#.00"), _standardFont));
                TOTAL_FACT.BorderWidth = 0;
                TOTAL_FACT.BackgroundColor = BaseColor.WHITE;
                SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                SERIE_REC.BorderWidth = 0;
                SERIE_REC.BackgroundColor = BaseColor.WHITE;
                CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                CORR_REC.BorderWidth = 0;
                CORR_REC.BackgroundColor = BaseColor.WHITE;
                FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                FECHA_REC.BorderWidth = 0;
                FECHA_REC.BackgroundColor = BaseColor.WHITE;
                ABONO_REC = new PdfPCell(new Phrase(total_abono_facturas.ToString("#,#.00"), _standardFont));
                ABONO_REC.BorderWidth = 0;
                ABONO_REC.BackgroundColor = BaseColor.WHITE;
                SALDO = new PdfPCell(new Phrase(saldo_facturas.ToString("#,#.00"), _standardFont));
                SALDO.BorderWidth = 0;
                SALDO.BackgroundColor = BaseColor.WHITE;
                CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                CONTENEDOR.BorderWidth = 0;
                CONTENEDOR.BackgroundColor = BaseColor.WHITE;
                HBL = new PdfPCell(new Phrase("", _standardFont));
                HBL.BorderWidth = 0;
                HBL.BackgroundColor = BaseColor.WHITE;
                SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                SERVICIO.BorderWidth = 0;
                SERVICIO.BackgroundColor = BaseColor.WHITE;
                POLIZA = new PdfPCell(new Phrase("", _standardFont));
                POLIZA.BorderWidth = 0;
                POLIZA.BackgroundColor = BaseColor.WHITE;
                USUARIO = new PdfPCell(new Phrase("", _standardFont));
                USUARIO.BorderWidth = 0;
                USUARIO.BackgroundColor = BaseColor.WHITE;
                tblEstadoCuentaFacturas.AddCell(TIPO);
                tblEstadoCuentaFacturas.AddCell(SERIE_FACT);
                tblEstadoCuentaFacturas.AddCell(CORR_FACT);
                tblEstadoCuentaFacturas.AddCell(FECHA_FACT);
                tblEstadoCuentaFacturas.AddCell(FECHA_PAGO);
                tblEstadoCuentaFacturas.AddCell(TOTAL_FACT);
                tblEstadoCuentaFacturas.AddCell(SERIE_REC);
                tblEstadoCuentaFacturas.AddCell(CORR_REC);
                tblEstadoCuentaFacturas.AddCell(FECHA_REC);
                tblEstadoCuentaFacturas.AddCell(ABONO_REC);
                tblEstadoCuentaFacturas.AddCell(SALDO);
                tblEstadoCuentaFacturas.AddCell(CONTENEDOR);
                tblEstadoCuentaFacturas.AddCell(HBL);
                tblEstadoCuentaFacturas.AddCell(SERVICIO);
                tblEstadoCuentaFacturas.AddCell(POLIZA);
                tblEstadoCuentaFacturas.AddCell(USUARIO);
                #endregion
            }
            if (ver_header_notas_debito > 0)
            {
                saldo_notas_debito += total_saldo_notas_debito;
                #region totales notas debito
                TIPO = new PdfPCell(new Phrase("ND", _standardFont));
                TIPO.BorderWidth = 0;
                TIPO.BackgroundColor = BaseColor.WHITE;
                SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                SERIE_FACT.BorderWidth = 0;
                SERIE_FACT.BackgroundColor = BaseColor.WHITE;
                CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                CORR_FACT.BorderWidth = 0;
                CORR_FACT.BackgroundColor = BaseColor.WHITE;
                FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                FECHA_FACT.BorderWidth = 0;
                FECHA_FACT.BackgroundColor = BaseColor.WHITE;
                FECHA_PAGO = new PdfPCell(new Phrase("", _standardFont));
                FECHA_PAGO.BorderWidth = 0;
                FECHA_PAGO.BackgroundColor = BaseColor.WHITE;
                TOTAL_FACT = new PdfPCell(new Phrase(total_notas_debito.ToString("#,#.00"), _standardFont));
                TOTAL_FACT.BorderWidth = 0;
                TOTAL_FACT.BackgroundColor = BaseColor.WHITE;
                SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                SERIE_REC.BorderWidth = 0;
                SERIE_REC.BackgroundColor = BaseColor.WHITE;
                CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                CORR_REC.BorderWidth = 0;
                CORR_REC.BackgroundColor = BaseColor.WHITE;
                FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                FECHA_REC.BorderWidth = 0;
                FECHA_REC.BackgroundColor = BaseColor.WHITE;
                ABONO_REC = new PdfPCell(new Phrase(total_abono_notas_debito.ToString("#,#.00"), _standardFont));
                ABONO_REC.BorderWidth = 0;
                ABONO_REC.BackgroundColor = BaseColor.WHITE;
                SALDO = new PdfPCell(new Phrase(saldo_notas_debito.ToString("#,#.00"), _standardFont));
                SALDO.BorderWidth = 0;
                SALDO.BackgroundColor = BaseColor.WHITE;
                CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                CONTENEDOR.BorderWidth = 0;
                CONTENEDOR.BackgroundColor = BaseColor.WHITE;
                HBL = new PdfPCell(new Phrase("", _standardFont));
                HBL.BorderWidth = 0;
                HBL.BackgroundColor = BaseColor.WHITE;
                SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                SERVICIO.BorderWidth = 0;
                SERVICIO.BackgroundColor = BaseColor.WHITE;
                POLIZA = new PdfPCell(new Phrase("", _standardFont));
                POLIZA.BorderWidth = 0;
                POLIZA.BackgroundColor = BaseColor.WHITE;
                USUARIO = new PdfPCell(new Phrase("", _standardFont));
                USUARIO.BorderWidth = 0;
                USUARIO.BackgroundColor = BaseColor.WHITE;
                tblEstadoCuentaNotasDebito.AddCell(TIPO);
                tblEstadoCuentaNotasDebito.AddCell(SERIE_FACT);
                tblEstadoCuentaNotasDebito.AddCell(CORR_FACT);
                tblEstadoCuentaNotasDebito.AddCell(FECHA_FACT);
                tblEstadoCuentaNotasDebito.AddCell(FECHA_PAGO);
                tblEstadoCuentaNotasDebito.AddCell(TOTAL_FACT);
                tblEstadoCuentaNotasDebito.AddCell(SERIE_REC);
                tblEstadoCuentaNotasDebito.AddCell(CORR_REC);
                tblEstadoCuentaNotasDebito.AddCell(FECHA_REC);
                tblEstadoCuentaNotasDebito.AddCell(ABONO_REC);
                tblEstadoCuentaNotasDebito.AddCell(SALDO);
                tblEstadoCuentaNotasDebito.AddCell(CONTENEDOR);
                tblEstadoCuentaNotasDebito.AddCell(HBL);
                tblEstadoCuentaNotasDebito.AddCell(SERVICIO);
                tblEstadoCuentaNotasDebito.AddCell(POLIZA);
                tblEstadoCuentaNotasDebito.AddCell(USUARIO);
                #endregion
            }
            if (ver_header_recibos > 0)
            {
                saldo_recibos += total_saldo_recibos;
                #region totales recibos
                TIPO = new PdfPCell(new Phrase("R", _standardFont));
                TIPO.BorderWidth = 0;
                TIPO.BackgroundColor = BaseColor.WHITE;
                SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                SERIE_FACT.BorderWidth = 0;
                SERIE_FACT.BackgroundColor = BaseColor.WHITE;
                CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                CORR_FACT.BorderWidth = 0;
                CORR_FACT.BackgroundColor = BaseColor.WHITE;
                FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                FECHA_FACT.BorderWidth = 0;
                FECHA_FACT.BackgroundColor = BaseColor.WHITE;
                FECHA_PAGO = new PdfPCell(new Phrase("", _standardFont));
                FECHA_PAGO.BorderWidth = 0;
                FECHA_PAGO.BackgroundColor = BaseColor.WHITE;
                TOTAL_FACT = new PdfPCell(new Phrase(total_recibos.ToString("#,#.00"), _standardFont));
                TOTAL_FACT.BorderWidth = 0;
                TOTAL_FACT.BackgroundColor = BaseColor.WHITE;
                SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                SERIE_REC.BorderWidth = 0;
                SERIE_REC.BackgroundColor = BaseColor.WHITE;
                CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                CORR_REC.BorderWidth = 0;
                CORR_REC.BackgroundColor = BaseColor.WHITE;
                FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                FECHA_REC.BorderWidth = 0;
                FECHA_REC.BackgroundColor = BaseColor.WHITE;
                ABONO_REC = new PdfPCell(new Phrase(total_abono_recibos.ToString("#,#.00"), _standardFont));
                ABONO_REC.BorderWidth = 0;
                ABONO_REC.BackgroundColor = BaseColor.WHITE;
                SALDO = new PdfPCell(new Phrase(saldo_recibos.ToString("#,#.00"), _standardFont));
                SALDO.BorderWidth = 0;
                SALDO.BackgroundColor = BaseColor.WHITE;
                CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                CONTENEDOR.BorderWidth = 0;
                CONTENEDOR.BackgroundColor = BaseColor.WHITE;
                HBL = new PdfPCell(new Phrase("", _standardFont));
                HBL.BorderWidth = 0;
                HBL.BackgroundColor = BaseColor.WHITE;
                SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                SERVICIO.BorderWidth = 0;
                SERVICIO.BackgroundColor = BaseColor.WHITE;
                POLIZA = new PdfPCell(new Phrase("", _standardFont));
                POLIZA.BorderWidth = 0;
                POLIZA.BackgroundColor = BaseColor.WHITE;
                USUARIO = new PdfPCell(new Phrase("", _standardFont));
                USUARIO.BorderWidth = 0;
                USUARIO.BackgroundColor = BaseColor.WHITE;
                tblEstadoCuentaRecibos.AddCell(TIPO);
                tblEstadoCuentaRecibos.AddCell(SERIE_FACT);
                tblEstadoCuentaRecibos.AddCell(CORR_FACT);
                tblEstadoCuentaRecibos.AddCell(FECHA_FACT);
                tblEstadoCuentaRecibos.AddCell(FECHA_PAGO);
                tblEstadoCuentaRecibos.AddCell(TOTAL_FACT);
                tblEstadoCuentaRecibos.AddCell(SERIE_REC);
                tblEstadoCuentaRecibos.AddCell(CORR_REC);
                tblEstadoCuentaRecibos.AddCell(FECHA_REC);
                tblEstadoCuentaRecibos.AddCell(ABONO_REC);
                tblEstadoCuentaRecibos.AddCell(SALDO);
                tblEstadoCuentaRecibos.AddCell(CONTENEDOR);
                tblEstadoCuentaRecibos.AddCell(HBL);
                tblEstadoCuentaRecibos.AddCell(SERVICIO);
                tblEstadoCuentaRecibos.AddCell(POLIZA);
                tblEstadoCuentaRecibos.AddCell(USUARIO);
                #endregion
            }

            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            doc.Add(tblEstadoCuentaTitulos);
            doc.Add(tblEstadoCuentaFacturas);
            doc.Add(tblEstadoCuentaNotasDebito);
            doc.Add(tblEstadoCuentaRecibos);

            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            total_saldo_cliente = saldo_facturas + saldo_notas_debito + saldo_recibos;

            doc.Add(new Phrase("SALDO    ", TitleFont));
            doc.Add(new Phrase(total_saldo_cliente.ToString("#,#.00"), TitleFont));

            doc.Close();
            writer.Close();



            ////Descargar el archivo

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=EstadoCuentaCliente_" + tb_agenteID.Text + ".pdf");
            Response.TransmitFile(Server.MapPath("~/EstadosCuenta/EstadoCuentaCliente_" + tb_agenteID.Text + ".pdf"));
            Response.End();
            #endregion

        }
        else
        {
            ///ESTADO DE CUENTA PARA LOS PROVEEDORES AGENTES NAVIERAS LINEAS AEREAS
            #region estado cuenta proveedor
            obtengo_datos_proveedor();

            Document doc = new Document(PageSize.LEGAL.Rotate());

            // Indicamos donde vamos a guardar el documento
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/EstadosCuenta/EstadoCuentaProveedor_" + tb_agenteID.Text + ".pdf"), FileMode.Create));

            //iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(Server.MapPath(Imagenpath));
            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(ms);

            imagen.BorderWidth = 0;
            imagen.Alignment = Element.ALIGN_LEFT;
            float percentage = 0.0f;
            percentage = 150 / imagen.Width;
            imagen.ScalePercent(percentage * 100);

            //doc.AddTitle("Estado de Cuenta");
            //doc.AddCreator("Aimar Group");
            doc.AddTitle(Params.titulo); // + " " + lb_tipopersona.SelectedValue);
            doc.AddCreator(Params.firma);

            doc.Open();
            doc.Add(imagen);

            Font _standardFont = new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK);
            Font _standardFontHeader = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
            Font SubTitleFont = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK);
            Font TitleFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.BLACK);
            Font WhiteFont = new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.WHITE);

            doc.Add(new Phrase("ESTADO DE CUENTA", TitleFont));
            doc.Add(Chunk.NEWLINE);

            doc.Add(new Phrase(tipoPersona+":   ", SubTitleFont));
            doc.Add(new Phrase(tb_agenteID.Text, _standardFontHeader));
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Phrase("Nombre:   ", SubTitleFont));
            doc.Add(new Phrase(tb_agentenombre.Text, _standardFontHeader));
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Phrase("Impresion de saldo pendiente de pago          ", _standardFontHeader));
            doc.Add(new Phrase("Desde:   ", SubTitleFont));
            doc.Add(new Phrase(tb_fechaini.Text + "   ", _standardFontHeader));
            doc.Add(new Phrase("Hasta:   ", SubTitleFont));
            doc.Add(new Phrase(tb_fechafin.Text + "   ", _standardFontHeader));
            doc.Add(new Phrase("Valores en:   ", SubTitleFont));
            doc.Add(new Phrase(simbolomoneda, _standardFontHeader));

            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblEstadoCuentaTitulos = new PdfPTable(16);
            tblEstadoCuentaTitulos.WidthPercentage = 100;
            PdfPTable tblEstadoCuentaCortes = new PdfPTable(16);
            tblEstadoCuentaCortes.WidthPercentage = 100;
            PdfPTable tblEstadoCuentaSincorte = new PdfPTable(16);
            tblEstadoCuentaSincorte.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell TIPO = new PdfPCell(new Phrase("TIPO", WhiteFont));
            TIPO.BorderWidth = 0;
            TIPO.BackgroundColor = BaseColor.BLACK;
            TIPO.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell SERIE_FACT = new PdfPCell(new Phrase("SERIE", WhiteFont));
            SERIE_FACT.BorderWidth = 0;
            SERIE_FACT.BackgroundColor = BaseColor.BLACK;
            SERIE_FACT.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell CORR_FACT = new PdfPCell(new Phrase("CORR.", WhiteFont));
            CORR_FACT.BorderWidth = 0;
            CORR_FACT.BackgroundColor = BaseColor.BLACK;
            CORR_FACT.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell FECHA_FACT = new PdfPCell(new Phrase("FECHA", WhiteFont));
            FECHA_FACT.BorderWidth = 0;
            FECHA_FACT.BackgroundColor = BaseColor.BLACK;
            FECHA_FACT.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell TOTAL_FACT = new PdfPCell(new Phrase("TOTAL", WhiteFont));
            TOTAL_FACT.BorderWidth = 0;
            TOTAL_FACT.BackgroundColor = BaseColor.BLACK;
            TOTAL_FACT.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell SERIE_REC = new PdfPCell(new Phrase("SERIE", WhiteFont));
            SERIE_REC.BorderWidth = 0;
            SERIE_REC.BackgroundColor = BaseColor.BLACK;
            SERIE_REC.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell CORR_REC = new PdfPCell(new Phrase("CORR.", WhiteFont));
            CORR_REC.BorderWidth = 0;
            CORR_REC.BackgroundColor = BaseColor.BLACK;
            CORR_REC.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell FECHA_REC = new PdfPCell(new Phrase("FECHA", WhiteFont));
            FECHA_REC.BorderWidth = 0;
            FECHA_REC.BackgroundColor = BaseColor.BLACK;
            FECHA_REC.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell VALOR = new PdfPCell(new Phrase("VALOR", WhiteFont));
            VALOR.BorderWidth = 0;
            VALOR.BackgroundColor = BaseColor.BLACK;
            VALOR.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell ABONO_REC = new PdfPCell(new Phrase("ABONO", WhiteFont));
            ABONO_REC.BorderWidth = 0;
            ABONO_REC.BackgroundColor = BaseColor.BLACK;
            ABONO_REC.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell SALDO = new PdfPCell(new Phrase("SALDO", WhiteFont));
            SALDO.BorderWidth = 0;
            SALDO.BackgroundColor = BaseColor.BLACK;
            SALDO.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell CONTENEDOR = new PdfPCell(new Phrase("CONTENEDOR", WhiteFont));
            CONTENEDOR.BorderWidth = 0;
            CONTENEDOR.BackgroundColor = BaseColor.BLACK;
            CONTENEDOR.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell HBL = new PdfPCell(new Phrase("HBL", WhiteFont));
            HBL.BorderWidth = 0;
            HBL.BackgroundColor = BaseColor.BLACK;
            HBL.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell SERVICIO = new PdfPCell(new Phrase("SERVICIO", WhiteFont));
            SERVICIO.BorderWidth = 0;
            SERVICIO.BackgroundColor = BaseColor.BLACK;
            SERVICIO.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell POLIZA = new PdfPCell(new Phrase("POLIZA", WhiteFont));
            POLIZA.BorderWidth = 0;
            POLIZA.BackgroundColor = BaseColor.BLACK;
            POLIZA.BorderWidthBottom = 0.75f;
            // Configuramos el título de las columnas de la tabla
            PdfPCell USUARIO = new PdfPCell(new Phrase("USUARIO", WhiteFont));
            USUARIO.BorderWidth = 0;
            USUARIO.BackgroundColor = BaseColor.BLACK;
            USUARIO.BorderWidthBottom = 0.75f;

            tblEstadoCuentaTitulos.AddCell(TIPO);
            tblEstadoCuentaTitulos.AddCell(SERIE_FACT);
            tblEstadoCuentaTitulos.AddCell(CORR_FACT);
            tblEstadoCuentaTitulos.AddCell(FECHA_FACT);
            tblEstadoCuentaTitulos.AddCell(TOTAL_FACT);
            tblEstadoCuentaTitulos.AddCell(SERIE_REC);
            tblEstadoCuentaTitulos.AddCell(CORR_REC);
            tblEstadoCuentaTitulos.AddCell(FECHA_REC);
            tblEstadoCuentaTitulos.AddCell(VALOR);
            tblEstadoCuentaTitulos.AddCell(ABONO_REC);
            tblEstadoCuentaTitulos.AddCell(SALDO);
            tblEstadoCuentaTitulos.AddCell(CONTENEDOR);
            tblEstadoCuentaTitulos.AddCell(HBL);
            tblEstadoCuentaTitulos.AddCell(SERVICIO);
            tblEstadoCuentaTitulos.AddCell(POLIZA);
            tblEstadoCuentaTitulos.AddCell(USUARIO);

            decimal total_cortes = 0;
            decimal total_valor_cortes = 0;
            decimal total_abono_cortes = 0;
            decimal total_sincorte = 0;
            decimal total_valor_sincortes = 0;
            decimal total_abono_sincorte = 0;
            decimal total_abonos_proveedor = 0;
            decimal total_saldo_proveedor = 0;
            int ver_header_sincorte = 0;
            int ver_header_cortes = 0;
            int alternar = 1;
            BaseColor color;
            int id_corte = 0;
            int id_corte2 = 0;

            foreach (DataRow dr in ds.Tables["estadocuenta_detallado_tbl"].Rows)
            {
                if (alternar == 2)
                {
                    color = BaseColor.LIGHT_GRAY;
                    alternar = 1;
                }
                else
                {
                    color = BaseColor.WHITE;
                    alternar = 2;
                }

                if (dr["tipo"].ToString() == "CT")
                {
                    if (ver_header_cortes == 0)
                    {
                        #region header cortes
                        TIPO = new PdfPCell(new Phrase("CT", _standardFont));
                        TIPO.BorderWidth = 0;
                        TIPO.BackgroundColor = BaseColor.GRAY;
                        SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_FACT.BorderWidth = 0;
                        SERIE_FACT.BackgroundColor = BaseColor.GRAY;
                        CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                        CORR_FACT.BorderWidth = 0;
                        CORR_FACT.BackgroundColor = BaseColor.GRAY;
                        FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_FACT.BorderWidth = 0;
                        FECHA_FACT.BackgroundColor = BaseColor.GRAY;
                        TOTAL_FACT = new PdfPCell(new Phrase("", _standardFont));
                        TOTAL_FACT.BorderWidth = 0;
                        TOTAL_FACT.BackgroundColor = BaseColor.GRAY;
                        SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_REC.BorderWidth = 0;
                        SERIE_REC.BackgroundColor = BaseColor.GRAY;
                        CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                        CORR_REC.BorderWidth = 0;
                        CORR_REC.BackgroundColor = BaseColor.GRAY;
                        FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_REC.BorderWidth = 0;
                        FECHA_REC.BackgroundColor = BaseColor.GRAY;
                        VALOR = new PdfPCell(new Phrase("", _standardFont));
                        VALOR.BorderWidth = 0;
                        VALOR.BackgroundColor = BaseColor.GRAY;
                        ABONO_REC = new PdfPCell(new Phrase("", _standardFont));
                        ABONO_REC.BorderWidth = 0;
                        ABONO_REC.BackgroundColor = BaseColor.GRAY;
                        SALDO = new PdfPCell(new Phrase("", _standardFont));
                        SALDO.BorderWidth = 0;
                        SALDO.BackgroundColor = BaseColor.GRAY;
                        CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                        CONTENEDOR.BorderWidth = 0;
                        CONTENEDOR.BackgroundColor = BaseColor.GRAY;
                        HBL = new PdfPCell(new Phrase("", _standardFont));
                        HBL.BorderWidth = 0;
                        HBL.BackgroundColor = BaseColor.GRAY;
                        SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                        SERVICIO.BorderWidth = 0;
                        SERVICIO.BackgroundColor = BaseColor.GRAY;
                        POLIZA = new PdfPCell(new Phrase("", _standardFont));
                        POLIZA.BorderWidth = 0;
                        POLIZA.BackgroundColor = BaseColor.GRAY;
                        USUARIO = new PdfPCell(new Phrase("", _standardFont));
                        USUARIO.BorderWidth = 0;
                        USUARIO.BackgroundColor = BaseColor.GRAY;
                        tblEstadoCuentaCortes.AddCell(TIPO);
                        tblEstadoCuentaCortes.AddCell(SERIE_FACT);
                        tblEstadoCuentaCortes.AddCell(CORR_FACT);
                        tblEstadoCuentaCortes.AddCell(FECHA_FACT);
                        tblEstadoCuentaCortes.AddCell(TOTAL_FACT);
                        tblEstadoCuentaCortes.AddCell(SERIE_REC);
                        tblEstadoCuentaCortes.AddCell(CORR_REC);
                        tblEstadoCuentaCortes.AddCell(FECHA_REC);
                        tblEstadoCuentaCortes.AddCell(VALOR);
                        tblEstadoCuentaCortes.AddCell(ABONO_REC);
                        tblEstadoCuentaCortes.AddCell(SALDO);
                        tblEstadoCuentaCortes.AddCell(CONTENEDOR);
                        tblEstadoCuentaCortes.AddCell(HBL);
                        tblEstadoCuentaCortes.AddCell(SERVICIO);
                        tblEstadoCuentaCortes.AddCell(POLIZA);
                        tblEstadoCuentaCortes.AddCell(USUARIO);
                        #endregion
                    }
                    id_corte = int.Parse(dr["id_fact"].ToString());

                    if (ver_header_cortes > 0)
                    {
                        if (id_corte != id_corte2)
                        {
                            #region totales subtotales cortes
                            TIPO = new PdfPCell(new Phrase("", _standardFont));
                            TIPO.BorderWidth = 0;
                            TIPO.BackgroundColor = BaseColor.WHITE;
                            SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                            SERIE_FACT.BorderWidth = 0;
                            SERIE_FACT.BackgroundColor = BaseColor.WHITE;
                            CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                            CORR_FACT.BorderWidth = 0;
                            CORR_FACT.BackgroundColor = BaseColor.WHITE;
                            FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                            FECHA_FACT.BorderWidth = 0;
                            FECHA_FACT.BackgroundColor = BaseColor.WHITE;
                            TOTAL_FACT = new PdfPCell(new Phrase(total_cortes.ToString("#,#.00"), _standardFont));
                            TOTAL_FACT.BorderWidth = 0;
                            TOTAL_FACT.BackgroundColor = BaseColor.WHITE;
                            SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                            SERIE_REC.BorderWidth = 0;
                            SERIE_REC.BackgroundColor = BaseColor.WHITE;
                            CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                            CORR_REC.BorderWidth = 0;
                            CORR_REC.BackgroundColor = BaseColor.WHITE;
                            FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                            FECHA_REC.BorderWidth = 0;
                            FECHA_REC.BackgroundColor = BaseColor.WHITE;
                            VALOR = new PdfPCell(new Phrase("", _standardFont));
                            VALOR.BorderWidth = 0;
                            VALOR.BackgroundColor = BaseColor.WHITE;
                            ABONO_REC = new PdfPCell(new Phrase(total_abono_cortes.ToString("#,#.00"), _standardFont));
                            ABONO_REC.BorderWidth = 0;
                            ABONO_REC.BackgroundColor = BaseColor.WHITE;
                            SALDO = new PdfPCell(new Phrase("", _standardFont));
                            SALDO.BorderWidth = 0;
                            SALDO.BackgroundColor = BaseColor.WHITE;
                            CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                            CONTENEDOR.BorderWidth = 0;
                            CONTENEDOR.BackgroundColor = BaseColor.WHITE;
                            HBL = new PdfPCell(new Phrase("", _standardFont));
                            HBL.BorderWidth = 0;
                            HBL.BackgroundColor = BaseColor.WHITE;
                            SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                            SERVICIO.BorderWidth = 0;
                            SERVICIO.BackgroundColor = BaseColor.WHITE;
                            POLIZA = new PdfPCell(new Phrase("", _standardFont));
                            POLIZA.BorderWidth = 0;
                            POLIZA.BackgroundColor = BaseColor.WHITE;
                            USUARIO = new PdfPCell(new Phrase("", _standardFont));
                            USUARIO.BorderWidth = 0;
                            USUARIO.BackgroundColor = BaseColor.WHITE;
                            tblEstadoCuentaCortes.AddCell(TIPO);
                            tblEstadoCuentaCortes.AddCell(SERIE_FACT);
                            tblEstadoCuentaCortes.AddCell(CORR_FACT);
                            tblEstadoCuentaCortes.AddCell(FECHA_FACT);
                            tblEstadoCuentaCortes.AddCell(TOTAL_FACT);
                            tblEstadoCuentaCortes.AddCell(SERIE_REC);
                            tblEstadoCuentaCortes.AddCell(CORR_REC);
                            tblEstadoCuentaCortes.AddCell(FECHA_REC);
                            tblEstadoCuentaCortes.AddCell(VALOR);
                            tblEstadoCuentaCortes.AddCell(ABONO_REC);
                            tblEstadoCuentaCortes.AddCell(SALDO);
                            tblEstadoCuentaCortes.AddCell(CONTENEDOR);
                            tblEstadoCuentaCortes.AddCell(HBL);
                            tblEstadoCuentaCortes.AddCell(SERVICIO);
                            tblEstadoCuentaCortes.AddCell(POLIZA);
                            tblEstadoCuentaCortes.AddCell(USUARIO);
                            total_cortes = 0;
                            total_abono_cortes = 0;
                            #endregion
                        }
                    }

                    TIPO = new PdfPCell(new Phrase(dr["tipo"].ToString(), _standardFont));
                    TIPO.BorderWidth = 0;
                    TIPO.BackgroundColor = color;
                    SERIE_FACT = new PdfPCell(new Phrase(dr["serie_fact"].ToString(), _standardFont));
                    SERIE_FACT.BorderWidth = 0;
                    SERIE_FACT.BackgroundColor = color;
                    CORR_FACT = new PdfPCell(new Phrase(dr["corr_fact"].ToString(), _standardFont));
                    CORR_FACT.BorderWidth = 0;
                    CORR_FACT.BackgroundColor = color;
                    FECHA_FACT = new PdfPCell(new Phrase(dr["fecha_fact"].ToString(), _standardFont));
                    FECHA_FACT.BorderWidth = 0;
                    FECHA_FACT.BackgroundColor = color;
                    TOTAL_FACT = new PdfPCell(new Phrase(decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00"), _standardFont));
                    TOTAL_FACT.BorderWidth = 0;
                    TOTAL_FACT.BackgroundColor = color;
                    SERIE_REC = new PdfPCell(new Phrase(dr["serie_rcpt"].ToString(), _standardFont));
                    SERIE_REC.BorderWidth = 0;
                    SERIE_REC.BackgroundColor = color;
                    CORR_REC = new PdfPCell(new Phrase(dr["corr_rcpt"].ToString(), _standardFont));
                    CORR_REC.BorderWidth = 0;
                    CORR_REC.BackgroundColor = color;
                    FECHA_REC = new PdfPCell(new Phrase(dr["fecha_rcpt"].ToString(), _standardFont));
                    FECHA_REC.BorderWidth = 0;
                    FECHA_REC.BackgroundColor = color;
                    VALOR = new PdfPCell(new Phrase(decimal.Parse(dr["valor_fact"].ToString()).ToString("#,#.00"), _standardFont));
                    VALOR.BorderWidth = 0;
                    VALOR.BackgroundColor = color;
                    ABONO_REC = new PdfPCell(new Phrase(decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00"), _standardFont));
                    ABONO_REC.BorderWidth = 0;
                    ABONO_REC.BackgroundColor = color;
                    SALDO = new PdfPCell(new Phrase(decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00"), _standardFont));
                    SALDO.BorderWidth = 0;
                    SALDO.BackgroundColor = color;
                    CONTENEDOR = new PdfPCell(new Phrase(dr["contenedor"].ToString(), _standardFont));
                    CONTENEDOR.BorderWidth = 0;
                    CONTENEDOR.BackgroundColor = color;
                    HBL = new PdfPCell(new Phrase(dr["hbl"].ToString(), _standardFont));
                    HBL.BorderWidth = 0;
                    HBL.BackgroundColor = color;
                    SERVICIO = new PdfPCell(new Phrase(dr["servicio"].ToString(), _standardFont));
                    SERVICIO.BorderWidth = 0;
                    SERVICIO.BackgroundColor = color;
                    POLIZA = new PdfPCell(new Phrase(dr["poliza"].ToString(), _standardFont));
                    POLIZA.BorderWidth = 0;
                    POLIZA.BackgroundColor = color;
                    USUARIO = new PdfPCell(new Phrase(dr["usuario"].ToString(), _standardFont));
                    USUARIO.BorderWidth = 0;
                    USUARIO.BackgroundColor = color;
                    tblEstadoCuentaCortes.AddCell(TIPO);
                    tblEstadoCuentaCortes.AddCell(SERIE_FACT);
                    tblEstadoCuentaCortes.AddCell(CORR_FACT);
                    tblEstadoCuentaCortes.AddCell(FECHA_FACT);
                    tblEstadoCuentaCortes.AddCell(TOTAL_FACT);
                    tblEstadoCuentaCortes.AddCell(SERIE_REC);
                    tblEstadoCuentaCortes.AddCell(CORR_REC);
                    tblEstadoCuentaCortes.AddCell(FECHA_REC);
                    tblEstadoCuentaCortes.AddCell(VALOR);
                    tblEstadoCuentaCortes.AddCell(ABONO_REC);
                    tblEstadoCuentaCortes.AddCell(SALDO);
                    tblEstadoCuentaCortes.AddCell(CONTENEDOR);
                    tblEstadoCuentaCortes.AddCell(HBL);
                    tblEstadoCuentaCortes.AddCell(SERVICIO);
                    tblEstadoCuentaCortes.AddCell(POLIZA);
                    tblEstadoCuentaCortes.AddCell(USUARIO);

                    id_corte2 = int.Parse(dr["id_fact"].ToString());

                    total_cortes += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_cortes += decimal.Parse(dr["abono_rcpt"].ToString());
                    ver_header_cortes++;
                    //total_saldo_cortes += decimal.Parse(dr["saldo"].ToString());
                    total_valor_cortes += decimal.Parse(dr["valor_fact"].ToString());
                }

                if (dr["tipo"].ToString() == "SC")
                {
                    if (ver_header_sincorte == 0)
                    {
                        #region header cortes
                        TIPO = new PdfPCell(new Phrase("SC", _standardFont));
                        TIPO.BorderWidth = 0;
                        TIPO.BackgroundColor = BaseColor.GRAY;
                        SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_FACT.BorderWidth = 0;
                        SERIE_FACT.BackgroundColor = BaseColor.GRAY;
                        CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                        CORR_FACT.BorderWidth = 0;
                        CORR_FACT.BackgroundColor = BaseColor.GRAY;
                        FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_FACT.BorderWidth = 0;
                        FECHA_FACT.BackgroundColor = BaseColor.GRAY;
                        TOTAL_FACT = new PdfPCell(new Phrase("", _standardFont));
                        TOTAL_FACT.BorderWidth = 0;
                        TOTAL_FACT.BackgroundColor = BaseColor.GRAY;
                        SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                        SERIE_REC.BorderWidth = 0;
                        SERIE_REC.BackgroundColor = BaseColor.GRAY;
                        CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                        CORR_REC.BorderWidth = 0;
                        CORR_REC.BackgroundColor = BaseColor.GRAY;
                        FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                        FECHA_REC.BorderWidth = 0;
                        FECHA_REC.BackgroundColor = BaseColor.GRAY;
                        VALOR = new PdfPCell(new Phrase("", _standardFont));
                        VALOR.BorderWidth = 0;
                        VALOR.BackgroundColor = BaseColor.GRAY;
                        ABONO_REC = new PdfPCell(new Phrase("", _standardFont));
                        ABONO_REC.BorderWidth = 0;
                        ABONO_REC.BackgroundColor = BaseColor.GRAY;
                        SALDO = new PdfPCell(new Phrase("", _standardFont));
                        SALDO.BorderWidth = 0;
                        SALDO.BackgroundColor = BaseColor.GRAY;
                        CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                        CONTENEDOR.BorderWidth = 0;
                        CONTENEDOR.BackgroundColor = BaseColor.GRAY;
                        HBL = new PdfPCell(new Phrase("", _standardFont));
                        HBL.BorderWidth = 0;
                        HBL.BackgroundColor = BaseColor.GRAY;
                        SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                        SERVICIO.BorderWidth = 0;
                        SERVICIO.BackgroundColor = BaseColor.GRAY;
                        POLIZA = new PdfPCell(new Phrase("", _standardFont));
                        POLIZA.BorderWidth = 0;
                        POLIZA.BackgroundColor = BaseColor.GRAY;
                        USUARIO = new PdfPCell(new Phrase("", _standardFont));
                        USUARIO.BorderWidth = 0;
                        USUARIO.BackgroundColor = BaseColor.GRAY;

                        tblEstadoCuentaSincorte.AddCell(TIPO);
                        tblEstadoCuentaSincorte.AddCell(SERIE_FACT);
                        tblEstadoCuentaSincorte.AddCell(CORR_FACT);
                        tblEstadoCuentaSincorte.AddCell(FECHA_FACT);
                        tblEstadoCuentaSincorte.AddCell(TOTAL_FACT);
                        tblEstadoCuentaSincorte.AddCell(SERIE_REC);
                        tblEstadoCuentaSincorte.AddCell(CORR_REC);
                        tblEstadoCuentaSincorte.AddCell(FECHA_REC);
                        tblEstadoCuentaSincorte.AddCell(VALOR);
                        tblEstadoCuentaSincorte.AddCell(ABONO_REC);
                        tblEstadoCuentaSincorte.AddCell(SALDO);
                        tblEstadoCuentaSincorte.AddCell(CONTENEDOR);
                        tblEstadoCuentaSincorte.AddCell(HBL);
                        tblEstadoCuentaSincorte.AddCell(SERVICIO);
                        tblEstadoCuentaSincorte.AddCell(POLIZA);
                        tblEstadoCuentaSincorte.AddCell(USUARIO);
                        #endregion
                    }
                    TIPO = new PdfPCell(new Phrase(dr["tipo"].ToString(), _standardFont));
                    TIPO.BorderWidth = 0;
                    TIPO.BackgroundColor = color;
                    SERIE_FACT = new PdfPCell(new Phrase(dr["serie_fact"].ToString(), _standardFont));
                    SERIE_FACT.BorderWidth = 0;
                    SERIE_FACT.BackgroundColor = color;
                    CORR_FACT = new PdfPCell(new Phrase(dr["corr_fact"].ToString(), _standardFont));
                    CORR_FACT.BorderWidth = 0;
                    CORR_FACT.BackgroundColor = color;
                    FECHA_FACT = new PdfPCell(new Phrase(dr["fecha_fact"].ToString(), _standardFont));
                    FECHA_FACT.BorderWidth = 0;
                    FECHA_FACT.BackgroundColor = color;
                    TOTAL_FACT = new PdfPCell(new Phrase(decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00"), _standardFont));
                    TOTAL_FACT.BorderWidth = 0;
                    TOTAL_FACT.BackgroundColor = color;
                    SERIE_REC = new PdfPCell(new Phrase(dr["serie_rcpt"].ToString(), _standardFont));
                    SERIE_REC.BorderWidth = 0;
                    SERIE_REC.BackgroundColor = color;
                    CORR_REC = new PdfPCell(new Phrase(dr["corr_rcpt"].ToString(), _standardFont));
                    CORR_REC.BorderWidth = 0;
                    CORR_REC.BackgroundColor = color;
                    FECHA_REC = new PdfPCell(new Phrase(dr["fecha_rcpt"].ToString(), _standardFont));
                    FECHA_REC.BorderWidth = 0;
                    FECHA_REC.BackgroundColor = color;
                    VALOR = new PdfPCell(new Phrase(decimal.Parse(dr["valor_fact"].ToString()).ToString("#,#.00"), _standardFont));
                    VALOR.BorderWidth = 0;
                    VALOR.BackgroundColor = color;
                    ABONO_REC = new PdfPCell(new Phrase(decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00"), _standardFont));
                    ABONO_REC.BorderWidth = 0;
                    ABONO_REC.BackgroundColor = color;
                    SALDO = new PdfPCell(new Phrase(decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00"), _standardFont));
                    SALDO.BorderWidth = 0;
                    SALDO.BackgroundColor = color;
                    CONTENEDOR = new PdfPCell(new Phrase(dr["contenedor"].ToString(), _standardFont));
                    CONTENEDOR.BorderWidth = 0;
                    CONTENEDOR.BackgroundColor = color;
                    HBL = new PdfPCell(new Phrase(dr["hbl"].ToString(), _standardFont));
                    HBL.BorderWidth = 0;
                    HBL.BackgroundColor = color;
                    SERVICIO = new PdfPCell(new Phrase(dr["servicio"].ToString(), _standardFont));
                    SERVICIO.BorderWidth = 0;
                    SERVICIO.BackgroundColor = color;
                    POLIZA = new PdfPCell(new Phrase(dr["poliza"].ToString(), _standardFont));
                    POLIZA.BorderWidth = 0;
                    POLIZA.BackgroundColor = color;
                    USUARIO = new PdfPCell(new Phrase(dr["usuario"].ToString(), _standardFont));
                    USUARIO.BorderWidth = 0;
                    USUARIO.BackgroundColor = color;
                    tblEstadoCuentaSincorte.AddCell(TIPO);
                    tblEstadoCuentaSincorte.AddCell(SERIE_FACT);
                    tblEstadoCuentaSincorte.AddCell(CORR_FACT);
                    tblEstadoCuentaSincorte.AddCell(FECHA_FACT);
                    tblEstadoCuentaSincorte.AddCell(TOTAL_FACT);
                    tblEstadoCuentaSincorte.AddCell(SERIE_REC);
                    tblEstadoCuentaSincorte.AddCell(CORR_REC);
                    tblEstadoCuentaSincorte.AddCell(FECHA_REC);
                    tblEstadoCuentaSincorte.AddCell(VALOR);
                    tblEstadoCuentaSincorte.AddCell(ABONO_REC);
                    tblEstadoCuentaSincorte.AddCell(SALDO);
                    tblEstadoCuentaSincorte.AddCell(CONTENEDOR);
                    tblEstadoCuentaSincorte.AddCell(HBL);
                    tblEstadoCuentaSincorte.AddCell(SERVICIO);
                    tblEstadoCuentaSincorte.AddCell(POLIZA);
                    tblEstadoCuentaSincorte.AddCell(USUARIO);
                    ver_header_sincorte++;
                    total_sincorte += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_sincorte += decimal.Parse(dr["abono_rcpt"].ToString());
                    total_valor_sincortes += decimal.Parse(dr["valor_fact"].ToString());
                    //total_saldo_facturas += decimal.Parse(dr["saldo"].ToString());
                }
                total_abonos_proveedor += decimal.Parse(dr["abono_rcpt"].ToString());
            }
            if (ver_header_cortes > 0)
            {
                #region totales corte
                TIPO = new PdfPCell(new Phrase("", _standardFont));
                TIPO.BorderWidth = 0;
                TIPO.BackgroundColor = BaseColor.WHITE;
                SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                SERIE_FACT.BorderWidth = 0;
                SERIE_FACT.BackgroundColor = BaseColor.WHITE;
                CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                CORR_FACT.BorderWidth = 0;
                CORR_FACT.BackgroundColor = BaseColor.WHITE;
                FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                FECHA_FACT.BorderWidth = 0;
                FECHA_FACT.BackgroundColor = BaseColor.WHITE;
                TOTAL_FACT = new PdfPCell(new Phrase(total_cortes.ToString("#,#.00"), _standardFont));
                TOTAL_FACT.BorderWidth = 0;
                TOTAL_FACT.BackgroundColor = BaseColor.WHITE;
                SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                SERIE_REC.BorderWidth = 0;
                SERIE_REC.BackgroundColor = BaseColor.WHITE;
                CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                CORR_REC.BorderWidth = 0;
                CORR_REC.BackgroundColor = BaseColor.WHITE;
                FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                FECHA_REC.BorderWidth = 0;
                FECHA_REC.BackgroundColor = BaseColor.WHITE;
                VALOR = new PdfPCell(new Phrase("", _standardFont));
                VALOR.BorderWidth = 0;
                VALOR.BackgroundColor = BaseColor.WHITE;
                ABONO_REC = new PdfPCell(new Phrase(total_abono_cortes.ToString("#,#.00"), _standardFont));
                ABONO_REC.BorderWidth = 0;
                ABONO_REC.BackgroundColor = BaseColor.WHITE;
                SALDO = new PdfPCell(new Phrase("", _standardFont));
                SALDO.BorderWidth = 0;
                SALDO.BackgroundColor = BaseColor.WHITE;
                CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                CONTENEDOR.BorderWidth = 0;
                CONTENEDOR.BackgroundColor = BaseColor.WHITE;
                HBL = new PdfPCell(new Phrase("", _standardFont));
                HBL.BorderWidth = 0;
                HBL.BackgroundColor = BaseColor.WHITE;
                SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                SERVICIO.BorderWidth = 0;
                SERVICIO.BackgroundColor = BaseColor.WHITE;
                POLIZA = new PdfPCell(new Phrase("", _standardFont));
                POLIZA.BorderWidth = 0;
                POLIZA.BackgroundColor = BaseColor.WHITE;
                USUARIO = new PdfPCell(new Phrase("", _standardFont));
                USUARIO.BorderWidth = 0;
                USUARIO.BackgroundColor = BaseColor.WHITE;
                tblEstadoCuentaCortes.AddCell(TIPO);
                tblEstadoCuentaCortes.AddCell(SERIE_FACT);
                tblEstadoCuentaCortes.AddCell(CORR_FACT);
                tblEstadoCuentaCortes.AddCell(FECHA_FACT);
                tblEstadoCuentaCortes.AddCell(TOTAL_FACT);
                tblEstadoCuentaCortes.AddCell(SERIE_REC);
                tblEstadoCuentaCortes.AddCell(CORR_REC);
                tblEstadoCuentaCortes.AddCell(FECHA_REC);
                tblEstadoCuentaCortes.AddCell(VALOR);
                tblEstadoCuentaCortes.AddCell(ABONO_REC);
                tblEstadoCuentaCortes.AddCell(SALDO);
                tblEstadoCuentaCortes.AddCell(CONTENEDOR);
                tblEstadoCuentaCortes.AddCell(HBL);
                tblEstadoCuentaCortes.AddCell(SERVICIO);
                tblEstadoCuentaCortes.AddCell(POLIZA);
                tblEstadoCuentaCortes.AddCell(USUARIO);
                #endregion
            }
            if (ver_header_sincorte > 0)
            {
                #region totales sin corte
                TIPO = new PdfPCell(new Phrase("", _standardFont));
                TIPO.BorderWidth = 0;
                TIPO.BackgroundColor = BaseColor.WHITE;
                SERIE_FACT = new PdfPCell(new Phrase("", _standardFont));
                SERIE_FACT.BorderWidth = 0;
                SERIE_FACT.BackgroundColor = BaseColor.WHITE;
                CORR_FACT = new PdfPCell(new Phrase("", _standardFont));
                CORR_FACT.BorderWidth = 0;
                CORR_FACT.BackgroundColor = BaseColor.WHITE;
                FECHA_FACT = new PdfPCell(new Phrase("", _standardFont));
                FECHA_FACT.BorderWidth = 0;
                FECHA_FACT.BackgroundColor = BaseColor.WHITE;
                TOTAL_FACT = new PdfPCell(new Phrase(total_sincorte.ToString("#,#.00"), _standardFont));
                TOTAL_FACT.BorderWidth = 0;
                TOTAL_FACT.BackgroundColor = BaseColor.WHITE;
                SERIE_REC = new PdfPCell(new Phrase("", _standardFont));
                SERIE_REC.BorderWidth = 0;
                SERIE_REC.BackgroundColor = BaseColor.WHITE;
                CORR_REC = new PdfPCell(new Phrase("", _standardFont));
                CORR_REC.BorderWidth = 0;
                CORR_REC.BackgroundColor = BaseColor.WHITE;
                FECHA_REC = new PdfPCell(new Phrase("", _standardFont));
                FECHA_REC.BorderWidth = 0;
                FECHA_REC.BackgroundColor = BaseColor.WHITE;
                VALOR = new PdfPCell(new Phrase("", _standardFont));
                VALOR.BorderWidth = 0;
                VALOR.BackgroundColor = BaseColor.WHITE;
                ABONO_REC = new PdfPCell(new Phrase(total_abono_sincorte.ToString("#,#.00"), _standardFont));
                ABONO_REC.BorderWidth = 0;
                ABONO_REC.BackgroundColor = BaseColor.WHITE;
                SALDO = new PdfPCell(new Phrase("", _standardFont));
                SALDO.BorderWidth = 0;
                SALDO.BackgroundColor = BaseColor.WHITE;
                CONTENEDOR = new PdfPCell(new Phrase("", _standardFont));
                CONTENEDOR.BorderWidth = 0;
                CONTENEDOR.BackgroundColor = BaseColor.WHITE;
                HBL = new PdfPCell(new Phrase("", _standardFont));
                HBL.BorderWidth = 0;
                HBL.BackgroundColor = BaseColor.WHITE;
                SERVICIO = new PdfPCell(new Phrase("", _standardFont));
                SERVICIO.BorderWidth = 0;
                SERVICIO.BackgroundColor = BaseColor.WHITE;
                POLIZA = new PdfPCell(new Phrase("", _standardFont));
                POLIZA.BorderWidth = 0;
                POLIZA.BackgroundColor = BaseColor.WHITE;
                USUARIO = new PdfPCell(new Phrase("", _standardFont));
                USUARIO.BorderWidth = 0;
                USUARIO.BackgroundColor = BaseColor.WHITE;
                tblEstadoCuentaSincorte.AddCell(TIPO);
                tblEstadoCuentaSincorte.AddCell(SERIE_FACT);
                tblEstadoCuentaSincorte.AddCell(CORR_FACT);
                tblEstadoCuentaSincorte.AddCell(FECHA_FACT);
                tblEstadoCuentaSincorte.AddCell(TOTAL_FACT);
                tblEstadoCuentaSincorte.AddCell(SERIE_REC);
                tblEstadoCuentaSincorte.AddCell(CORR_REC);
                tblEstadoCuentaSincorte.AddCell(FECHA_REC);
                tblEstadoCuentaSincorte.AddCell(VALOR);
                tblEstadoCuentaSincorte.AddCell(ABONO_REC);
                tblEstadoCuentaSincorte.AddCell(SALDO);
                tblEstadoCuentaSincorte.AddCell(CONTENEDOR);
                tblEstadoCuentaSincorte.AddCell(HBL);
                tblEstadoCuentaSincorte.AddCell(SERVICIO);
                tblEstadoCuentaSincorte.AddCell(POLIZA);
                tblEstadoCuentaSincorte.AddCell(USUARIO);
                #endregion
            }


            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            doc.Add(tblEstadoCuentaTitulos);
            doc.Add(tblEstadoCuentaCortes);
            doc.Add(tblEstadoCuentaSincorte);

            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            total_saldo_proveedor = total_valor_cortes + total_valor_sincortes - total_abonos_proveedor;

            doc.Add(new Phrase("SALDO    ", TitleFont));
            doc.Add(new Phrase(total_saldo_proveedor.ToString("#,#.00"), TitleFont));

            doc.Close();
            writer.Close();



            ////Descargar el archivo

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=EstadoCuentaProveedor_" + tb_agenteID.Text + ".pdf");
            Response.TransmitFile(Server.MapPath("~/EstadosCuenta/EstadoCuentaProveedor_" + tb_agenteID.Text + ".pdf"));
            Response.End();

            /*resumen_cortes.Text = total_valor_cortes.ToString("#,#.00");
            resumen_sincortes.Text = total_valor_sincortes.ToString("#,#.00");
            resumen_abonos.Text = total_abonos_proveedor.ToString("#,#.00");
            total_saldo_proveedor = total_valor_cortes + total_valor_sincortes - total_abonos_proveedor;
            lbl_total_saldo_proveedor.Text = total_saldo_proveedor.ToString("#,#.00");*/
            #endregion
        }
    }



    protected void bt_generar_Click(object sender, EventArgs e)
    {
        int solopendiente = 0;
        string consolidaMoneda = "";
        if (tb_agenteID.Text.Trim().Equals("0") || (tb_agenteID.Text.Trim().Equals("") || (tb_agentenombre.Text.Trim().Equals(""))))
        {
            WebMsgBox.Show("Ingrese el nombre para Generar el Reporte");
            return;
        }
        if (tb_fechaini.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha inicial");
            return;
        }
        if (tb_fechafin.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha final");
            return;
        }
        if (chk_consolidar.Checked == true)
        {
            consolidaMoneda = "si";
        }
        else
        {
            consolidaMoneda = "no";
        }

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "ESTADO DE CUENTA";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Tipo .: " + lb_tipopersona.SelectedItem.Text + ", ";
        mensaje_log += "Codigo .: " + tb_agenteID.Text + ", ";
        mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " Contabilidad.: " + lb_contabilidad.SelectedItem.Text + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fechaini.Text + " Fecha Final.: " + tb_fechafin.Text + " ,";
        mensaje_log += "Solo Pendiente de Liquidar .: " + solopendiente + ", ";
        mensaje_log += "Consolidar Moneda.: " + consolidaMoneda + " ";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        if (chk_pendliq.Checked) solopendiente = 1;
        string mensaje = "<script languaje=\"JavaScript\">";
        //mensaje += "window.open('viewEstadoCuentaCliente.aspx?tpi="+lb_tipopersona.SelectedValue+"&cliID="+tb_agenteID.Text+"&solopendiente="+solopendiente+"&fechaini="+tb_fechaini.Text+"&fechafin="+tb_fechafin.Text+"&moneda="+lb_moneda.SelectedValue+"&conta="+lb_contabilidad.SelectedValue+"&clientenombre="+tb_agentenombre.Text+"&consolida_moneda="+ consolidaMoneda +"','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";      +"&pais="+user.PaisID
        if (lb_tipopersona.SelectedValue == "3")
        {
            mensaje += "window.open('EstadoCuentaClienteHtml.aspx?tipo=&tpi=" + lb_tipopersona.SelectedValue + "&cliID=" + tb_agenteID.Text + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&moneda=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&clientenombre=" + tb_agentenombre.Text + "&consolida_moneda=" + consolidaMoneda + "&pais=" + user.PaisID + "','Genera_Reporte_" + tb_agenteID.Text + "_" + lb_moneda.SelectedValue + "','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=1280','_newtab');";
        }
        else
        {
            mensaje += "window.open('EstadoCuentaProveedorHtml.aspx?tipo=&tpi=" + lb_tipopersona.SelectedValue + "&cliID=" + tb_agenteID.Text + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&moneda=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&clientenombre=" + tb_agentenombre.Text + "&consolida_moneda=" + consolidaMoneda + "&pais=" + user.PaisID + "','Genera_Reporte_" + tb_agenteID.Text + "_" + lb_moneda.SelectedValue + "','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=1280','_newtab');";
        }
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);

        //Response.Redirect("viewEstadoCuentaCliente.aspx?tpi="+lb_tipopersona.SelectedValue+"&cliID="+tb_agenteID.Text+"&solopendiente="+solopendiente+"&fechaini="+tb_fechaini.Text+"&fechafin="+tb_fechafin.Text+"&moneda="+lb_moneda.SelectedValue+"&conta="+lb_contabilidad.SelectedValue+"&clientenombre="+tb_agentenombre.Text);
    }


    protected void bt_generar_excel_Click(object sender, EventArgs e)
    {
        int solopendiente = 0;
        string consolidaMoneda = "";
        if (tb_agenteID.Text.Trim().Equals("0") || (tb_agenteID.Text.Trim().Equals("") || (tb_agentenombre.Text.Trim().Equals(""))))
        {
            WebMsgBox.Show("Ingrese el nombre para Generar el Reporte");
            return;
        }
        if (tb_fechaini.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha inicial");
            return;
        }
        if (tb_fechafin.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha final");
            return;
        }
        if (chk_consolidar.Checked == true)
        {
            consolidaMoneda = "si";
        }
        else
        {
            consolidaMoneda = "no";
        }

        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = user.PaisID;
        Bean_Log.strC1 = "ESTADO DE CUENTA";
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Tipo .: " + lb_tipopersona.SelectedItem.Text + ", ";
        mensaje_log += "Codigo .: " + tb_agenteID.Text + ", ";
        mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " Contabilidad.: " + lb_contabilidad.SelectedItem.Text + " ,";
        mensaje_log += "Fecha Inicial.: " + tb_fechaini.Text + " Fecha Final.: " + tb_fechafin.Text + " ,";
        mensaje_log += "Solo Pendiente de Liquidar .: " + solopendiente + ", ";
        mensaje_log += "Consolidar Moneda.: " + consolidaMoneda + " ";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion

        if (chk_pendliq.Checked) solopendiente = 1;
        string mensaje = "<script languaje=\"JavaScript\">";
        //mensaje += "window.open('viewEstadoCuentaCliente.aspx?tpi="+lb_tipopersona.SelectedValue+"&cliID="+tb_agenteID.Text+"&solopendiente="+solopendiente+"&fechaini="+tb_fechaini.Text+"&fechafin="+tb_fechafin.Text+"&moneda="+lb_moneda.SelectedValue+"&conta="+lb_contabilidad.SelectedValue+"&clientenombre="+tb_agentenombre.Text+"&consolida_moneda="+ consolidaMoneda +"','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";      +"&pais="+user.PaisID
        if (lb_tipopersona.SelectedValue == "3")
        {
            mensaje += "window.open('EstadoCuentaClienteHtml.aspx?tipo=EXCEL&tpi=" + lb_tipopersona.SelectedValue + "&cliID=" + tb_agenteID.Text + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&moneda=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&clientenombre=" + tb_agentenombre.Text + "&consolida_moneda=" + consolidaMoneda + "&pais=" + user.PaisID + "','Genera_Reporte_" + tb_agenteID.Text + "_" + lb_moneda.SelectedValue + "','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=200,height=200','_newtab');";
        }
        else
        {
            mensaje += "window.open('EstadoCuentaProveedorHtml.aspx?tipo=EXCEL&tpi=" + lb_tipopersona.SelectedValue + "&cliID=" + tb_agenteID.Text + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&moneda=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&clientenombre=" + tb_agentenombre.Text + "&consolida_moneda=" + consolidaMoneda + "&pais=" + user.PaisID + "','Genera_Reporte_" + tb_agenteID.Text + "_" + lb_moneda.SelectedValue + "','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=200,height=200','_newtab');";
        }
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);

        //Response.Redirect("viewEstadoCuentaCliente.aspx?tpi="+lb_tipopersona.SelectedValue+"&cliID="+tb_agenteID.Text+"&solopendiente="+solopendiente+"&fechaini="+tb_fechaini.Text+"&fechafin="+tb_fechafin.Text+"&moneda="+lb_moneda.SelectedValue+"&conta="+lb_contabilidad.SelectedValue+"&clientenombre="+tb_agentenombre.Text);
    }


    protected void bt_generar_excel_Click2(object sender, EventArgs e)
    {
        int solopendiente = 0;
        string consolidaMoneda = "";
        string content = "";
        string ruta_acceso = "";
        if (tb_agenteID.Text.Trim().Equals("0") || (tb_agenteID.Text.Trim().Equals("") || (tb_agentenombre.Text.Trim().Equals(""))))
        {
            WebMsgBox.Show("Ingrese el nombre para Generar el Reporte");
            return;
        }
        if (tb_fechaini.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha inicial");
            return;
        }
        if (tb_fechafin.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la Fecha final");
            return;
        }
        if (chk_consolidar.Checked == true)
        {
            consolidaMoneda = "si";
        }
        else
        {
            consolidaMoneda = "no";
        }
        if (chk_pendliq.Checked) solopendiente = 1;
        
        //WebClient client = new WebClient();

        ruta_acceso = "http://" + Request.Url.Authority + Request.ApplicationPath;
                   
        if (lb_tipopersona.SelectedValue == "3")
        {
            //ruta_acceso = "http://localhost:62206/BAWSERVER/Reports/EstadoCuentaClienteHtml.aspx?tpi=" + lb_tipopersona.SelectedValue + "&cliID=" + tb_agenteID.Text + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&moneda=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&clientenombre=" + tb_agentenombre.Text + "&consolida_moneda=" + consolidaMoneda + "&pais=" + user.PaisID;
            //ruta_acceso = "http://10.10.1.7:8181/Reports/EstadoCuentaClienteHtml.aspx?tpi=" + lb_tipopersona.SelectedValue + "&cliID=" + tb_agenteID.Text + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&moneda=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&clientenombre=" + tb_agentenombre.Text + "&consolida_moneda=" + consolidaMoneda + "&pais=" + user.PaisID;  2020-01-22
            //ruta_acceso = Server.MapPath("~/Reports/EstadoCuentaClienteHtml.aspx");  
            ruta_acceso = ruta_acceso + "/Reports/EstadoCuentaClienteHtml.aspx";  
        }
        else
        {
            //ruta_acceso = "http://localhost:62206/BAWSERVER/Reports/EstadoCuentaProveedorHtml.aspx?tpi=" + lb_tipopersona.SelectedValue + "&cliID=" + tb_agenteID.Text + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&moneda=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&clientenombre=" + tb_agentenombre.Text + "&consolida_moneda=" + consolidaMoneda + "&pais=" + user.PaisID;
            //ruta_acceso = "http://10.10.1.7:8181/Reports/EstadoCuentaProveedorHtml.aspx?tpi=" + lb_tipopersona.SelectedValue + "&cliID=" + tb_agenteID.Text + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&moneda=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&clientenombre=" + tb_agentenombre.Text + "&consolida_moneda=" + consolidaMoneda + "&pais=" + user.PaisID; 2020-01-22
            ruta_acceso = ruta_acceso + "/ReportsEstadoCuentaProveedorHtml.aspx";            
        }


        ruta_acceso = ruta_acceso + "?tpi=" + lb_tipopersona.SelectedValue + "&cliID=" + tb_agenteID.Text + "&solopendiente=" + solopendiente + "&fechaini=" + tb_fechaini.Text + "&fechafin=" + tb_fechafin.Text + "&moneda=" + lb_moneda.SelectedValue + "&conta=" + lb_contabilidad.SelectedValue + "&clientenombre=" + tb_agentenombre.Text + "&consolida_moneda=" + consolidaMoneda + "&pais=" + user.PaisID;


        //content = client.DownloadString(ruta_acceso);
        /*CookieContainer cookieContainer = new CookieContainer();
        cookieContainer.Add((UsuarioBean)Session["usuario"]);*/

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ruta_acceso);
        request.Timeout = 600000;
        request.ReadWriteTimeout = 600000;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //request.CookieContainer = cookieContainer;

        if (response.StatusCode == HttpStatusCode.OK)
        {
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = null;

            if (response.CharacterSet == null)
            {
                readStream = new StreamReader(receiveStream);
            }
            else
            {
                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
            }

            content = readStream.ReadToEnd();

            response.Close();
            readStream.Close();
        }


        string strFile = "EstadoCuenta_" + tb_agenteID.Text + ".xls";
        string strcontentType = "application/vnd.ms-excel";
        Response.ClearContent();
        Response.ClearHeaders();
        Response.BufferOutput = true;
        Response.ContentType = strcontentType;
        Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
        Response.Charset = "UTF-8";
        Response.ContentEncoding = Encoding.Default;
        Response.Write(content);
        Response.Flush();
        Response.Close();
        Response.End();
    }
}
