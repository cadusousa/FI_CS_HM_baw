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
using System.Collections.Generic;

public partial class operations_movimiento_contable : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    public string titulo = "";
    public string op = "";

 

    protected void Page_Load(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];

        if (Session["usuario"] == null)
            Response.Redirect("../default.aspx");

        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 131072) == 131072))
            Response.Redirect("index.aspx");

 


        Session["SaldosTransmision"] = null;

        LinkButton1.Visible = false;
        LinkButton2.Visible = true;

        op = Request.QueryString["op"];
    
        if (op == "1") { titulo = "Saldos Registrados"; }
        //if (op == "2") { titulo = "Detalle Integra un Saldo"; }
        if (op == "3") { titulo = "Saldos Transmitidos"; }
        if (op == "4") { titulo = "Saldos Pendientes de Enviar"; }

        
        


        if (Request.QueryString["tdi_cue_id"] != null)
        {
            tb_cuenta.Text = Request.QueryString["tdi_cue_id"];
        }

        if (Request.QueryString["tdi_fecha"] != null)
        {
            tb_fecha_cuenta.Text = Request.QueryString["tdi_fecha"];
        }

        if (Request.QueryString["fecha_saldo"] != null)
        {
            tb_fecha_saldo.Text = Request.QueryString["fecha_saldo"];
        }

        if (Request.QueryString["trans_fecha"] != null)
        {
            tb_fecha_trans.Text = Request.QueryString["trans_fecha"];
        }


        //int id = 0;
        if (Request.QueryString["id"] != null)
        {

            ShowDetails(Int32.Parse(Request.QueryString["id"]), Request.QueryString["s"]);

        }
        else {

            Label3.Text = "<a href='saldos_transmision.aspx?op=" + op + "' class='example_a' rel='nofollow noopener' >" + " LIMPIAR DATOS " + "</a>";


            //if (Request.QueryString["tipo"] != null)
            {
                //if (Request.QueryString["tipo"] == "1")
                    //ShowDetails(0);
            }
        }

        //Label4.Text = "<a href='saldos_transmision.aspx?op=" + op + "&tipo=1'>" + " CONSULTA " + "</a>";
        
        tipo_conta();


        if (!Page.IsPostBack)
        {

        }

        if (lb_contabilidad.SelectedValue != "")
        {
            Session["lb_contabilidad"] = lb_contabilidad.SelectedValue;
        }
    }



    protected void tipo_conta() {


        if (lb_contabilidad.Items.Count == 0)
        {

            //************************RESTRICCION DE CONTABILIDAD USUARIOS****************************//
            lb_contabilidad.Items.Clear();
            int bandera = 0; // verifica si el usuario tiener contabilidad consolidada.
            int fiscal = 0;
            int financiera = 0;
            ListItem item = null;
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
            if (bandera == 2)
            {
                //item = new ListItem("CONSOLIDADO", "3");
                //lb_contabilidad.Items.Add(item);
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

            if (Session["lb_contabilidad"] != null)
                lb_contabilidad.SelectedValue = Session["lb_contabilidad"].ToString();

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
            //cuentaArr = null;
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







    protected void ButtonClear_Click(object sender, EventArgs e)
    {
        /*tb_cuenta.Text = "";
        tb_fecha_cuenta.Text = "";
        tb_fecha_saldo.Text = "";
        tb_fecha_trans.Text = "";
        ButtonExcel.Visible = false;
        Label2.Text = "";
        Session["SaldosTransmision"] = null;*/
    }


    protected void ButtonExcel_Click(object sender, EventArgs e)
    {
        string filename = user.PaisID + "_" + user.ID + ".xls";

        Session["SaldosTransmision"] = Label2.Text.ToString().ToUpper().Replace("DETAILS", "").Replace("INFO", "");

        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('saldos_transmision_rep.aspx?op=" + op + "&html=" + "" + "&filename=" + filename + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }

        LinkButton1.Visible = true;
        LinkButton2.Visible = false;
    }



    protected void bt_visualizar_Click(object sender, EventArgs e)
    {             
        ShowDetails(0,"");
    }

    void ShowDetails(int id, string s) {

        tipo_conta();

        if (tb_cuenta.Text == "" && tb_fecha_saldo.Text == "" && tb_fecha_trans.Text == "" && tb_fecha_cuenta.Text == "")
        {
            WebMsgBox.Show("Debe Ingresar una Cuenta Contable ó alguna fecha");
            return;
        }

        string query2 = "", query = "", orderby = " ORDER BY tdi_fecha, tdi_pai_id, tdi_cue_id, tdi_moneda_id, tdi_tcon_id ";

        //-- CAST(tdi_pai_id AS text) as ""Pais"", -- ROW_NUMBER () OVER (ORDER BY saldo_id) as No, --     
        query = (@"SELECT                    
                    saldo_id,       
                    -- saldo_index,              
                    to_char(tdi_fecha,'DD-MM-YYYY') as ""Fecha"",
                    tdi_cue_id as ""Cuenta"", 
                    CAST(tdi_moneda_id AS text) as ""Moneda"",                                        
                    CAST(tdi_tipo_cambio AS text) as ""Tipo Cambio"", 
                    CAST(tdi_tcon_id AS text) as ""Conta"",
                    tdi_debe as ""Debe USD"", 
                    tdi_haber as ""Haber USD"" 
                    , tdi_debe_equivalente as ""Debe Local"" 
                    , tdi_haber_equivalente ""Haber Local"" 
                    -- , to_char(fecha_saldo,'DD-MM-YYYY') as ""Fecha Saldo""
                    -- ,  to_char(trans_fecha,'DD-MM-YYYY') as ""Fecha Trans"" 
                    , CAST(trans_count as smallint) as ""Count""
                    , trans_count as ""Details""
                FROM tbl_saldos WHERE ");

      if (id > 0) {

          switch (s)
          {
              case "2": //DETALLES SALDO 2
                  query = query.Replace("tbl_saldos WHERE ", "tbl_saldos2 WHERE saldo_id1 = " + id);
                  break;

              case "1":  //DETALLES 
              case "3":

                  string tbl_libro =
                      //"tbl_libro_diario";
                                    "tbl_libro_diario_correcciones";
                  query = (@" 
                  		SELECT   		                           
                            -- b.saldo_index,
                            to_char(b.tdi_fecha,'DD-MM-YYYY') AS ""Fecha"",
                            a.tdi_cue_id AS ""Cuenta"",  
                            CAST(a.tdi_moneda_id AS text) AS ""Moneda"",                             
                            CAST(a.tdi_tipo_cambio AS text) AS ""Tipo Cambio"",
                            CAST(a.tdi_tcon_id AS text) as ""Conta"",
			                tdi_haber_usd AS ""Debe USD"", 
			                tdi_haber_usd AS ""Haber USD"",
			                tdi_debe_local AS ""Debe Local""
			                , tdi_haber_local AS ""Haber Local"" 	 
                            -- , to_char(b.fecha_saldo,'DD-MM-YYYY') AS ""Fecha Saldo""
                            -- , to_char(b.trans_fecha,'DD-MM-YYYY') AS ""Fecha Trans"" 
                            , 0 as ""Info""

                        FROM " + tbl_libro + @" a
                            INNER JOIN tbl_saldos_detalles c ON a.tdi_transaction_id = c.tdi_transaction_id 
	                        INNER JOIN tbl_saldos b ON saldo_id1 = b.saldo_id 
                        WHERE saldo_id1 = 10143 
                        -- ORDER BY a.tdi_transaction_id DESC 
");
                  query = query.Replace("10143", id.ToString());

                  if (s == "3")
                  {                      
                        query = query.Replace(" saldo_id1 ", " saldo_id2 ");
                        query = query.Replace(" tbl_saldos ", " tbl_saldos2 ");
                  }

                  orderby = "";

                  break;
          }
        }

        string filter2 = "?op=" + op;
        string filter = "<tr><th colspan=2 align=right>Pais : </th><td colspan=3 align=left>" + user.PaisID + "</td></tr>";
        query2 += "tdi_pai_id = " + user.PaisID;
        //query2 += "tdi_pai_id = 5";  //TEST

        switch (op)
        {
            case "1": query2 += " AND fecha_saldo IS NOT NULL "; break;
            case "2":                break;
            case "3": query2 += " AND trans_fecha IS NOT NULL "; break;
            case "4": query2 += " AND trans_fecha IS NULL "; break;
        }



        /*
        if (s != "")
        if (lb_contabilidad.SelectedValue  != "")
        {
            filter += "<tr><th colspan=2 align=right>Tipo Contabilidad : </th><td colspan=3 align=left>" + lb_contabilidad.SelectedItem  + "</td></tr>";
            query2 += " AND tdi_tcon_id = " + lb_contabilidad.SelectedValue + "";
            filter2 += "&tdi_tcon_id=" + lb_contabilidad.SelectedValue;
        }*/

        if (tb_cuenta.Text != "")
        {
            filter += "<tr><th colspan=2 align=right>Cuenta : </th><td colspan=3 align=left>" + tb_cuenta.Text + "</td></tr>";
            query2 += " AND tdi_cue_id = '" + tb_cuenta.Text + "'";
            filter2 += "&tdi_cue_id=" + tb_cuenta.Text;
        }

        if (tb_fecha_cuenta.Text != "")
        {
            filter += "<tr><th colspan=2 align=right>Fecha de Cuenta : </th><td colspan=3 align=left>'" + DB.DateFormat(tb_fecha_cuenta.Text).ToString().Replace("/", "-") + "'</td></tr>";
            query2 += " AND to_char(tdi_fecha,'YYYY-MM-DD') = '" + DB.DateFormat(tb_fecha_cuenta.Text).ToString().Replace("/", "-") + "'";
            filter2 += "&tdi_fecha=" + tb_fecha_cuenta.Text;
        }

        if (tb_fecha_saldo.Text != "")
        {
            filter += "<tr><th colspan=2 align=right>Fecha Saldo : </th><td colspan=3 align=left>'" + DB.DateFormat(tb_fecha_saldo.Text).ToString().Replace("/", "-") + "'</td></tr>";
            query2 += " AND to_char(fecha_saldo,'YYYY-MM-DD') = '" + DB.DateFormat(tb_fecha_saldo.Text).ToString().Replace("/", "-") + "'";
            filter2 += "&fecha_saldo=" + tb_fecha_saldo.Text;
        }

        if (tb_fecha_trans.Text != "")
        {
            filter += "<tr><th colspan=2 align=right>Fecha Transmision : </th><td colspan=3 align=left>'" + DB.DateFormat(tb_fecha_trans.Text).ToString().Replace("/", "-") + "'</td></tr>";
            query2 += " AND to_char(trans_fecha,'YYYY-MM-DD') = '" + DB.DateFormat(tb_fecha_trans.Text).ToString().Replace("/", "-") + "'";
            filter2 += "&trans_fecha=" + tb_fecha_trans.Text;
        }

        //filter += "<tr><th colspan=2>Tipo : </th><td colspan=2>" + lb_tipo.SelectedValue + "</td></tr>";
        //query2 += " AND tipo = '" + lb_tipo.SelectedValue + "'";

        if (id > 0)
        {
            filter += "<tr><th colspan=2 align=right>ID : </th><td colspan=3 align=left>" + id + "</td></tr>";
            //query2 = " b.stat = " + id;
            //query2 = " saldo_id1 = " + id;
            query2 = "";

            //query2 = " AND " + query2;

            //if (s == "3")
            //query2 = query2.Replace("tdi_pai_id", "a.tdi_pai_id").Replace("tdi_tcon_id", "a.tdi_tcon_id").Replace("tdi_fecha", "a.tdi_fecha");

            
            Label3.Text = "<a href='saldos_transmision.aspx" + filter2 + "' class='example_a' rel='nofollow noopener' >" + " LIMPIAR DATOS " + "</a>" ;      
        }

        query += query2 + orderby;

        IEnumerable<Dictionary<string, object>> t = getDataArray(query, lb_tipo.SelectedValue, op, titulo, "bawbk", Server);

        if (t != null)
        {
            if (t.Count() > 0)
            {
                LinkButton1.Visible = true;
                LinkButton2.Visible = false;
            }
        }

        Label2.Text = getHtml(filter, filter2, t, lb_tipo.SelectedValue, op, titulo, id, s);
    
    }



    public static IEnumerable<Dictionary<string, object>> getDataArray(string query, string tipo, string op, string titulo, string theConnection, HttpServerUtility Server)
    {
        Npgsql.NpgsqlConnection conn = OpenConn2(theConnection, Server); //OpenConnection();
        Npgsql.NpgsqlCommand comm;
        comm = new Npgsql.NpgsqlCommand();
        comm.Connection = conn;
        Npgsql.NpgsqlDataReader reader = null;

        IEnumerable<Dictionary<string, object>> t = null;

        try
        {
            comm.CommandText = query;
            reader = comm.ExecuteReader();


            if (reader.HasRows)
                t = DB.Serialize(reader);

            reader.Close();
            comm.Parameters.Clear();
        }
        catch (Exception e)
        {
            //Response.Write(e.Message);
            string msg = e.Message;
        }

        DB.CloseObj_insert(comm, conn);

        return t;
    }

    static System.Xml.XmlElement ToXElement(System.Xml.XmlNode node)
    {
        return node as System.Xml.XmlElement; // returns null if node is not an XElement
    }

    private static string GetConnectionStringFromFile(string theConnection, HttpServerUtility Server)
    {

        System.Xml.XmlDocument xobj = new System.Xml.XmlDocument();
        System.Xml.XmlElement xelement = null;

        string xScheme = "";
        string tmpCnn = "";

        try
        {

            string tmpStr = Server.MapPath("~/Include/ConfigXML/") + "Connections.xml";

            xobj.Load(tmpStr);

            xelement = ToXElement(xobj.SelectSingleNode("connections"));

            if (theConnection == "")
                xScheme = xelement.GetAttribute("default");
            else
                xScheme = theConnection;

            xelement = ToXElement(xobj.SelectSingleNode("connections/connection[@name='" + xScheme + "']"));

            tmpCnn = xelement.GetAttribute("connectionstring");

        }
        catch (Exception ex)
        {

            string msg = ex.Message;
        }

        return tmpCnn;
    }


    public static Npgsql.NpgsqlConnection OpenConn2(string connStr, HttpServerUtility Server)
    {
        Npgsql.NpgsqlConnection conn = null;
        try
        {
            connStr = GetConnectionStringFromFile(connStr, Server);
            conn = new Npgsql.NpgsqlConnection(connStr);
            conn.Open();
        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
        }
        return conn;
    }



    public static string getHtml(string filter, string filter2, IEnumerable<Dictionary<string, object>> t, string tipo, string op, string titulo, int id, string s)
    {
        string html = "";
        string th = "";
        string td = "";
        int ide = 0;
        string a1 = "", a2 = "";
        decimal n = 0;
        string bd = "", bh = "";

        decimal[] arr = new decimal[15];

        try
        {
            //<h3>RESULTADO DE LA CONSULTA</h3>
            int i = 0, j = 0, k = 0;
            Boolean isInt, isDec;
            html = (@"
                <table border=0 align=center cellpadding=10 cellspacing=0 width=100%>
                <tr><td colspan=12 align=center><h2>" + titulo.ToUpper() + (id > 0 ? " DETALLES " + (s == "2" ? " NIVEL II" : " NIVEL III") : " NIVEL I") + @"</h2></td></tr>
                " + filter + "<tbody>");

            if (t != null)
            {
                foreach (Dictionary<string, object> row in t)
                {
                    i++;
                    j = 0;

                    bh = " style='background:#4CAF50;color:white' ";

                    //th = "<tr><th " + bh + ">No.</th>";
                    th = "<tr>";

                    if (i % 2 == 0)
                        bd = " style='background:silver;' "; //" style='background:#f2f2f2;' ";
                    else
                        bd = "";

                    //td = "<tr><td " + bd + ">" + i + "</td>";
                    td = "<tr>";

                    foreach (KeyValuePair<string, object> campo in row)
                    {

                        /*
                        switch (campo.Key) {
                        
                            case "saldo_id":
                                ide = Int32.Parse(campo.Value.ToString());
                                break;
                            
                            case "details":
                            case "Details":
                                    k = Int32.Parse(campo.Value.ToString());
                                    a1 = ""; a2 = "";
                                    switch (s)
                                    {
                                        case "":
                                            //a1 = "<a href='saldos_transmision.aspx" + filter2 + "&id=" + ide + "&s=1' title='" + k + " Regs." + ide + "'>" + campo.Key + " " + ide + "</a>";

                                            //if (k > 1)
                                            a2 = "<a href='saldos_transmision.aspx" + filter2 + "&id=" + ide + "&s=2' title='" + k + " Regs.  Key : " + ide + "'>" + campo.Key + "</a>";
                                            break;

                                        case "2":
                                            a1 = "<a href='saldos_transmision.aspx" + filter2 + "&id=" + ide + "&s=3' title='" + k + " Regs. Key : " + ide + "'>" + campo.Key + "</a>";

                                            break;

                                    }

                                    td += "<td>" + (j > 0 ? a1 + " " + a2 : "") + "</td>";
                                    j++;
                                break;
                            
                            case "info":
                            case "Info":
                                    a1 = "<a href='#'>Ver Info</a>";
                                    td += "<td>" + a1 + "</td>";
                                    //j++;
                             break;
                                              
                        }

                        */
                        if (campo.Key == "saldo_id")
                        {
                            ide = Int32.Parse(campo.Value.ToString());
                        }
                        else
                        {
                            if (campo.Key.ToString().ToUpper() == "DETAILS" || campo.Key.ToString().ToUpper() == "INFO")
                            {
                                if (campo.Key.ToString().ToUpper() == "DETAILS")
                                {
                                    k = Int32.Parse(campo.Value.ToString());
                                    a1 = ""; a2 = "";
                                    switch (s)
                                    {
                                        case "":
                                            //a1 = "<a href='saldos_transmision.aspx" + filter2 + "&id=" + ide + "&s=1' title='" + k + " Regs." + ide + "'>" + campo.Key + " " + ide + "</a>";

                                            //if (k > 1)
                                            a2 = "<a href='saldos_transmision.aspx" + filter2 + "&id=" + ide + "&s=2' title='" + k + " Regs.  Key : " + ide + "'>" + campo.Key + "</a>";
                                            break;

                                        case "2":
                                            a1 = "<a href='saldos_transmision.aspx" + filter2 + "&id=" + ide + "&s=3' title='" + k + " Regs. Key : " + ide + "'>" + campo.Key + "</a>";
                                            break;
                                    }
                                    td += "<td>" + (j > 0 ? a1 + " " + a2 : "") + "</td>";
                                }

                                if (campo.Key.ToString().ToUpper() == "INFO")
                                {
                                    a1 = "<a href='#'>" + campo.Key + "</a>";

                                    td += "<td>" + a1 + "</td>";
                                }                               
                            }
                            else
                            {
                                th += "<th  " + bh + " align=center>" + campo.Key + "</th>";
                                
                                isInt = int.TryParse(campo.Value.ToString(), out k);
                                isDec = decimal.TryParse(campo.Value.ToString(), out n);

                                if (isInt) {
                                    arr[j] += k;
                                    td += "<td " + bd + " align=right>" + k + "</td>";
                                } else 
                                if (isDec)
                                {
                                    arr[j] += n;
                                    td += "<td " + bd + " align=right>" + n.ToString("#,##0.00") + "</td>";
                                }
                                else
                                {
                                    td += "<td " + bd + ">" + campo.Value + "</td>";
                                }
                            }
                            j++;
                        }                        
                    }
                    th += "</tr>";
                    td += "</tr>";
    
                    if (i == 1) html += th;
                    html += td;
                }
            }

            if (i == 0)
            {
                html += "<tr><td colspan=12 align=center><h4>No se encontraro ninguna coincidencia con este criterio de búsqueda</h4></td></tr></tbody>";
            }
            else 
            {
                html += "</tbody><tfoot><tr>";//<th></th>";                     
                    for (j=0; j<12; j++) {
                   
                        isInt = int.TryParse(arr[j].ToString(), out k);
                        isDec = decimal.TryParse(arr[j].ToString(), out n);

                        if (isInt) {

                            if (k > 0)
                            html += "<td " + bh + " align=right>" + k + "</td>";  
                            else
                            html += "<td>" + "" + "</td>";
                        }
                        else
                        if (isDec) {
                            html += "<th " + bh + " align=right>" + n.ToString("#,##0.00") + "</th>";
                        }
                        else
                        {
                            html += "<td>" + "" + "</td>";
                        }
                    }
                    html += "</tr></tfoot>";
            }

            html += "</table>";
        }
        catch (Exception e)
        {
            //Response.Write();
            html = e.Message;
        }

        return html;
    }


}

