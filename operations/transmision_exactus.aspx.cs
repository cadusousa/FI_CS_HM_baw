using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;
using Npgsql;

public partial class operations_transmision_exactus : System.Web.UI.Page
{
    public string html, query;
    public int s = 0, r = 0, t = 0, reporte = 0;

    public DateTime starting = DateTime.Today;

    public DateTime ending;

    /*

    public object DeserializeJson<T>(string Json)
    {
        JavaScriptSerializer JavaScriptSerializer = new JavaScriptSerializer();
        return JavaScriptSerializer.Deserialize<T>(Json);
    }


    public class exactus_res
    {
        public string ASIENTO { get; set; }
        public string ASIENTOEXACTUS { get; set; }
        public string COD_COMPANIA { get; set; }
        public string COD_PAIS { get; set; }
        public string CODIGO_ERROR { get; set; }
        public string ESTADO { get; set; }
        public string MENSAJE { get; set; }
    }

    public class exactus_sen
    {
        public int LINEAS { get; set; }
        public int COD_COMPANY { get; set; }
        public string COD_PAIS { get; set; }
        public string ASIENTO { get; set; }
        public string TIPO_ASIENTO { get; set; }
        public string CUENTA_CONTABLE { get; set; }
        public string DESCRIPCION { get; set; }
        public string FUENTE { get; set; }
        public string REFERENCIA { get; set; }
        public string DESCRIPCION_NIT { get; set; }
        public string NIT { get; set; }
        public decimal CREDITOS_LOCAL { get; set; }
        public decimal CREDITOS_DOLAR { get; set; }
        public decimal DEBITO_LOCAL { get; set; }
        public decimal DEBITO_DOLAR { get; set; }
        public string MONEDA { get; set; }
        public decimal TIPO_CAMBIO { get; set; }
        public int C { get; set; }
        public string REGS { get; set; }

    }
    */

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
      
        }
        else
        {        

        }


        try
        {
            reporte = int.Parse(Request.QueryString["reporte"]);
        }
        catch (Exception ex)
        {

        }


        if (reporte == 0)
        {
            transmision();
        }      
        
        if (reporte == 1)
        {
            reportes();
        }
    }

    protected void transmision()
    {

        int id = 0;
        string op = "";
  

            try
            {

                id = int.Parse(Request.QueryString["id"]);

            }
            catch (Exception ex)
            {

            }

            try
            {

                op = Request.QueryString["o"];

            }
            catch (Exception ex)
            {

            }




            List<string> dias = new List<string>();
            dias.Add("Do");
            dias.Add("Lu");
            dias.Add("Ma");
            dias.Add("Mi");
            dias.Add("Ju");
            dias.Add("Vi");
            dias.Add("Sa");


            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("stat");
            dt.Columns.Add("fecha 0");
            dt.Columns.Add("fecha 1");
            dt.Columns.Add("fecha 2");
            dt.Columns.Add("mensaje");
            dt.Columns.Add("usuario");
            dt.Columns.Add("json_send");
            dt.Columns.Add("json_return");

            try
            {

                NpgsqlConnection conn;
                NpgsqlCommand comm;
                NpgsqlDataReader reader;


                conn = DB.OpenConnection();
                comm = new NpgsqlCommand();
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;




                RE_GenericBean rgb = new RE_GenericBean();

                try
                {
                    ending = DateTime.Parse(tb_fechainicial.Text);

                    starting = DateTime.Parse(tb_fechafinal.Text); 
                }
                catch (Exception ex)
                {
                    starting = DateTime.Today;

                    tb_fechafinal.Text = starting.ToString("dd/MM/yyyy").Substring(0, 10);

                    ending = starting.AddDays(-1);

                    query = (@"SELECT id, estatus, to_char(fecha_libro_diario,'YYYY-MM-DD') 
                                FROM 
                                    tbl_transmision_exactus 
                                WHERE 
                                     estatus = 3 -- AND fecha_libro_diario > (CURRENT_DATE - interval '1 month') 
                                ORDER BY 
                                    fecha_libro_diario ASC, estatus DESC, date_send ASC
                                LIMIT 1");
                    comm.CommandText = query;
                    reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(1))
                        {
                            if (reader.GetInt32(1) == 3)
                                if (!reader.IsDBNull(2)) ending = DateTime.Parse(reader.GetString(2));
                        }
                    }
                    reader.Close();

                    tb_fechainicial.Text = ending.ToString("dd/MM/yyyy").Substring(0, 10);
                }

  
                for (DateTime date = starting; date >= ending; date = date.AddDays(-1))
                {      
                    rgb.strC1 = date.ToString("yyyy-MM-dd").Substring(0, 10);
                    object[] objArr = { 0, 0, rgb.strC1, "", "", "", "", "", "" };
                    dt.Rows.Add(objArr);
                }



                query = (@"SELECT id, estatus, to_char(fecha_libro_diario,'YYYY-MM-DD'), to_char(date_send,'DD-mm-yyyy HH24:MI:SS'), to_char(date_response,'DD-mm-yyyy HH24:MI:SS'), msg, usuario, json_send, json_response, tipo 
            FROM tbl_transmision_exactus 
            WHERE 
                fecha_libro_diario BETWEEN '" + ending.ToString("yyyy-MM-dd").Substring(0, 10) + "' AND '" + starting.ToString("yyyy-MM-dd").Substring(0, 10) + @"' OR estatus <> 3
            ORDER BY 
                fecha_libro_diario  desc, estatus asc, date_send asc");

                //Response.Write (query);
                comm.CommandText = query;
                reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!reader.IsDBNull(2))
                        {
                            if (dr[2].ToString() == reader.GetString(2))
                            {
                                if (!reader.IsDBNull(0)) dr[0] = reader.GetInt32(0);
                                if (!reader.IsDBNull(1)) dr[1] = reader.GetInt32(1);
                                if (!reader.IsDBNull(2)) dr[2] = reader.GetString(2);
                                if (!reader.IsDBNull(3)) dr[3] = reader.GetString(3);
                                if (!reader.IsDBNull(4)) dr[4] = reader.GetString(4);
                                if (!reader.IsDBNull(5)) dr[5] = reader.GetString(5);
                                if (!reader.IsDBNull(6)) dr[6] = reader.GetString(6);
                                if (!reader.IsDBNull(7)) dr[7] = reader.GetString(7);
                                if (!reader.IsDBNull(8)) dr[8] = reader.GetString(8);
                            }
                        }
                    }
                }
                DB.CloseObj(reader, comm, conn);


                UsuarioBean user;

                user = (UsuarioBean)Session["usuario"];

                //string url = "http://localhost/TrasnmitionExactus.php?user=" + user.ID + "&fecha=";

                string url = "http://10.10.1.20/combex/TrasnmitionExactus.php?user=" + user.ID + "&fecha=";

                List<int> arr_estatus = new List<int>();
                List<string> arr_msg = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {
                    arr_estatus.Add(int.Parse(dr[1].ToString()));
                    arr_msg.Add(dr[1].ToString());
                    if (dr[1].ToString() == "3") //estatus 3 envio con exito
                        s++;
                    else
                        r++;
                }



                int f = 0;

                html = "";
                html += "<div id=forma style='display:table-inline;height:320px;width:100%;overflow:auto;background:#eee;border:1px solid gray'>";
                html += (@"<table width=98% valign=top border=1 id=ex_header style='background:white'>
<tr>
<th class=th1 align=center></th>
<th class=th1 align=center>Transmitir</th>
<th></th>
<th class=th1 align=center>Fecha</th>
<th class=th1 align=center title='Json Enviado'>E</th>
<th class=th1 align=center title='Json Respuesta'>R</th>
<th class=th1 align=center>Fecha Envio</th>
<th class=th1 align=center>Fecha Respuesta</th>
<th class=th1>Mensaje</th>
<th class=th1 align=center>Usuario</th>
</tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    byte dia1 = (byte)DateTime.Parse(dr[2].ToString()).DayOfWeek;

                    int dia = int.Parse(dia1.ToString());

                    f = 1; //no transmite

                    if (t < dt.Rows.Count - 1)
                    {
                        /*
                        if (dia == 1 && t + 3 <= dt.Rows.Count-1) //lunes
                        {
                            if (arr_estatus[t + 3] == 3 && arr_msg[t+3] == "No se encontraron registros") //si el viernes se completo
                            { 
                                f = 0;
                            }
                        }

                        if (dia == 0 && t + 2 <= dt.Rows.Count - 1) //domingo
                        {
                            if (arr_estatus[t + 2] == 3 && arr_msg[t+2] == 'No se encontraron registros') //si el viernes se completo
                            {
                                f = 0;
                            }
                        }
                   
                        */
                        //cualquier dia
                        if (arr_estatus[t + 1] == 3 || arr_msg[t + 1] == "No se encontraron registros") //si el dia siguiente se respuesta
                        {
                            f = 0;
                        }

                    }
                    else
                    {
                        f = 0;
                    }

                    if (dr[5].ToString() != "Registro de inicio")
                    {

                        t++;

                        html += "<tr>";

                        html += "<td class=td1 align=center style='font-family:courier'>" + t + "</td>";

                        if (f == 1)
                            html += "<td class=td1 align=center  style='font-family:courier;color:silver'>" + dr[2].ToString() + "</td>";
                        else
                            html += "<td class=td1 align=center style='font-family:courier;color:blue;'>" + (dr[1].ToString() == "3" ? "" : "<a href='" + url + dr[2].ToString() + "' target=iframe_php onclick=\"document.getElementById('flag').value = '1';move();\" style='color:blue' title='Transmiti a Exactus'>" + dr[2].ToString() + "</a>") + "</td>";

                        html += "<td class=td1 align=left style='font-family:courier'>" + dias[dia] + "</td>";

                        html += "<td class=td1 align=center>" + (dr[1].ToString() == "3" ? dr[2].ToString() : "") + "</td>";  //→←↑↓

                        //html += "<td class=td1 align=center>" + (dr[7].ToString() != "" ? "<a href='transmision_exactus.aspx?id=" + dr[0].ToString() + "&o=S' title='Json Enviado' style='font-weight:bolder;color:blue;border:1px solid gray;background:white;'>&nbsp;" + "↑" + "&nbsp;</a>" : "") + "</td>";

                        //html += "<td class=td1 align=center>" + (dr[8].ToString() != "" ? "<a href='transmision_exactus.aspx?id=" + dr[0].ToString() + "&o=R' title='Respuesta' style='font-weight:bolder;color:red;border:1px solid gray;background:white;'>&nbsp;" + "↓" + "&nbsp;</a>" : "") + "</td>";


                        html += "<td class=td1 align=center>" + (dr[7].ToString() != "" ? "<a href='transmision_exactus_reporte.aspx?w=s&t=ht&id=" + dr[0].ToString() + "' target=_blank title='Html Enviado' style='font-weight:bolder;color:blue;border:1px solid gray;background:white;'>&nbsp;" + "↑" + "&nbsp;</a>" : "") + "</td>";

                        html += "<td class=td1 align=center>" + (dr[8].ToString() != "" ? "<a href='transmision_exactus_reporte.aspx?w=r&t=ht&id=" + dr[0].ToString() + "' target=_blank title='Html Respuesta' style='font-weight:bolder;color:red;border:1px solid gray;background:white;'>&nbsp;" + "↓" + "&nbsp;</a>" : "") + "</td>";

/*
<h1>
    Datos Enviados
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus.aspx'><input type=button value=X title='cerrar detalles'></a>
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus_reporte.aspx?w=s&t=js&id=" + id + @"' target=_blank>Json Enviado</a>
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus_reporte.aspx?w=r&t=js&id=" + id + @"' target=_blank>Json Resultado</a>
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus_reporte.aspx?w=s&t=ht&id=" + id + @"' target=_blank>Html Enviado</a>
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus_reporte.aspx?w=s&t=ex&id=" + id + @"' target=_blank>Excel Enviado</a>
</h1>
*/



                        html += "<td class=td1 align=center>" + dr[3].ToString() + "</td>";

                        html += "<td class=td1 align=center>" + dr[4].ToString() + "</td>";

                        html += "<td class=td1 align=left>" + dr[5].ToString() + "</td>";

                        html += "<td class=td1 align=center>" + dr[6].ToString() + "</td>";

                        html += "</tr>";

                    }
/*
                    if (id == int.Parse(dr[0].ToString()) && int.Parse(dr[1].ToString()) > 0)
                    {
                        int c = 0;
                        string respuesta = "", htm = "", temp;


                        if (op == "R")
                        {
                            respuesta = dr[8].ToString();

                            htm += (@"<center>

<h1>

Respuesta Exactus&nbsp;&nbsp;&nbsp;<a href='transmision_exactus.aspx'><input type=button value=X title='cerrar detalles'></a>

&nbsp;&nbsp;&nbsp;<a href='transmision_exactus.aspx'>
</h1>

</center>");

                            htm += (@"<table width=99% valign=top border=1 class=ex_details>
<tr>
<th></th>
<th>ASIENTO</th>
<th>ASIENTOEXACTUS</th>
<th>COD_COMPANIA</th>
<th>COD_PAIS</th>
<th>CODIGO_ERROR</th>
<th>ESTADO</th>
<th>MENSAJE</th>
</tr>");
                        }
                        else
                        {
                            respuesta = dr[7].ToString();

                            htm += (@"<center>
<h1>
    Datos Enviados
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus.aspx'><input type=button value=X title='cerrar detalles'></a>
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus_reporte.aspx?w=s&t=js&id=" + id + @"' target=_blank>Json Enviado</a>
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus_reporte.aspx?w=r&t=js&id=" + id + @"' target=_blank>Json Resultado</a>
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus_reporte.aspx?w=s&t=ht&id=" + id + @"' target=_blank>Html Enviado</a>
    &nbsp;&nbsp;&nbsp; <a href='transmision_exactus_reporte.aspx?w=s&t=ex&id=" + id + @"' target=_blank>Excel Enviado</a>
</h1>

</center>");

                            htm += (@"<table valign=top border=1 class=ex_details>
<tr>
<th></th>
<th>LINEAS</th>
<th>COD_COMPANY</th>
<th>COD_PAIS</th>
<th>ASIENTO</th>
<th>TIPO_ASIENTO</th>
<th>CUENTA_CONTABLE</th>
<th>DESCRIPCION</th>
<th>FUENTE</th>
<th>REFERENCIA</th>
<th>DESCRIPCION_NIT</th>
<th>NIT</th>
<th>CREDITOS_LOCAL</th>
<th>CREDITOS_DOLAR</th>
<th>DEBITO_LOCAL</th>
<th>DEBITO_DOLAR</th>
<th>MONEDA</th>
<th>TIPO_CAMBIO</th>
<th>C</th>
<th>REGS</th>
</tr>");
                        }



                        respuesta = respuesta.Replace("[", "").Replace("]", "");

                        List<string> lineas = respuesta.Split('}').ToList();

                        char[] MyChar = { ',' };

                        foreach (string linea in lineas)
                        {
                            c++;
                            try
                            {

                                temp = linea + "}";

                                if (temp.Substring(0, 1) == ",")
                                {
                                    temp = temp.TrimStart(MyChar);
                                }

                                if (op == "R")
                                {

                                    exactus_res result = (exactus_res)DeserializeJson<exactus_res>(temp);

                                    htm += "<tr>";
                                    htm += "<td>" + c + "</td>";
                                    htm += "<td>" + result.ASIENTO + "</td>";
                                    htm += "<td>" + result.ASIENTOEXACTUS + "</td>";
                                    htm += "<td>" + result.COD_COMPANIA + "</td>";
                                    htm += "<td>" + result.COD_PAIS + "</td>";
                                    htm += "<td>" + result.CODIGO_ERROR + "</td>";
                                    htm += "<td>" + result.ESTADO + "</td>";
                                    htm += "<td>" + result.MENSAJE + "</td>";
                                    htm += "</tr>";

                                }
                                else
                                {

                                    exactus_sen result = (exactus_sen)DeserializeJson<exactus_sen>(temp);

                                    htm += "<tr>";
                                    htm += "<td>" + c + "</td>";
                                    htm += "<td>" + result.LINEAS + "</td>";
                                    htm += "<td>" + result.COD_COMPANY + "</td>";
                                    htm += "<td>" + result.COD_PAIS + "</td>";
                                    htm += "<td>" + result.ASIENTO + "</td>";
                                    htm += "<td>" + result.TIPO_ASIENTO + "</td>";
                                    htm += "<td>" + result.CUENTA_CONTABLE + "</td>";
                                    htm += "<td>" + result.DESCRIPCION + "</td>";
                                    htm += "<td>" + result.FUENTE + "</td>";
                                    htm += "<td>" + result.REFERENCIA + "</td>";
                                    htm += "<td>" + result.DESCRIPCION_NIT + "</td>";
                                    htm += "<td>" + result.NIT + "</td>";
                                    htm += "<td>" + result.CREDITOS_LOCAL + "</td>";
                                    htm += "<td>" + result.CREDITOS_DOLAR + "</td>";
                                    htm += "<td>" + result.DEBITO_LOCAL + "</td>";
                                    htm += "<td>" + result.DEBITO_DOLAR + "</td>";
                                    htm += "<td>" + result.MONEDA + "</td>";
                                    htm += "<td>" + result.TIPO_CAMBIO + "</td>";
                                    htm += "<td>" + result.C + "</td>";
                                    htm += "<td>" + result.REGS + "</td>";
                                    htm += "</tr>";
                                }
                            }
                            catch (Exception ex)
                            {
                                //log4net ErrLog = new log4net();
                                //ErrLog.ErrorLog(ex.Message);
                                //return null;
                            }
                        }
                        htm += "</table>";

                        html += "<tr>";
                        html += "<td colspan=1>" + "" + "</td>";
                        html += "<td colspan=9 style='border:1px solid black;background:lightblue;padding:3px;'><div style=\"display:table-inline;height:200px;width:700px;overflow:scroll;\">" + htm + "</div></td>";
                        html += "</tr>";

                    }
*/
                }
                html += "</table></div>";
                html += "</div>";

            }
            catch (Exception ex)
            {
                log4net ErrLog = new log4net();
                ErrLog.ErrorLog(ex.Message);
                //return null;
            }

        

    }

    protected void reportes()
    {
        try
        {


            try
            {
                string temp;

                temp = tb_fechainicial.Text;

                ending = DateTime.Parse(temp);    

                temp = tb_fechafinal.Text;

                starting = DateTime.Parse(temp);
            }
            catch (Exception ex)
            {

                starting = DateTime.Today;

                tb_fechafinal.Text = starting.ToString("dd/MM/yyyy").Substring(0, 10);

                ending = starting.AddMonths(-1);

                tb_fechainicial.Text = ending.ToString("dd/MM/yyyy").Substring(0, 10);

            }


            html = "<div id=forma style='display:table-inline;height:320px;width:100%;overflow:auto;background:#eee;border:1px solid gray'>";

            html += DB.ReporteProvisiones(starting, ending, "ht");

            html += "</div>";

        }
        catch (Exception ex)
        {

        }

    }


            /*
    protected void reportes()
    {
        try
        {

            try
            {
                string temp;

                temp = tb_fechainicial.Text;

                ending = DateTime.Parse(temp); //.ToString("yyyy-MM-dd");        

                temp = tb_fechafinal.Text;

                starting = DateTime.Parse(temp); //, "yyyy-MM-dd", null);
            }
            catch (Exception ex)
            {

                starting = DateTime.Today;

                tb_fechafinal.Text = starting.ToString("dd/MM/yyyy").Substring(0, 10);

                ending = starting.AddMonths(-1);

                tb_fechainicial.Text = ending.ToString("dd/MM/yyyy").Substring(0, 10);

            }



            NpgsqlConnection conn2;
            NpgsqlCommand comm2;
            NpgsqlDataReader reader2 = null;

            conn2 = DB.OpenMasterConnection();
            comm2 = new NpgsqlCommand();
            comm2.Connection = conn2;
            comm2.CommandType = CommandType.Text;




            NpgsqlConnection conn;
            NpgsqlCommand comm;
            NpgsqlDataReader reader = null;


            conn = DB.OpenConnection();
            comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;

            html += "<div id=forma style='display:table-inline;height:320px;width:100%;overflow:auto;background:#eee;border:1px solid gray'>";

            html += (@"<table width=99% valign=top border=1 class=ex_details id=rep_pro>
<tr>
    <td colspan=11>
<H1>REPORTE PROVISIONES SIN FACTURAR&nbsp;&nbsp;<a href='transmision_exactus_reporte.aspx?id=-1&ending=" + ending.ToString("yyyy-MM-dd").Substring(0, 10) + @"&starting=" + starting.ToString("yyyy-MM-dd").Substring(0, 10) + @"' target=_blank>Export Excel</a></H1>
    </td>
</tr>");



            html += (@"<table width=99% valign=top border=1 class=ex_details id=rep_pro>
<tr>
    <th></th>
    <th>PAIS</th>
    <th>SERIE</th>
    <th>CORRELATIVO</th>
    <th>PROVEEDOR</th>
    <th>NOMBRE</th>
    <th>NIT</th>
    <th>TIPO</th>
    <th>FECHA EMISION</th>
    <th>MONEDA</th>
    <th>VALOR</th>
</tr>");

            query = (@"        
    SELECT pai_iso, tpr_serie, tpr_correlativo, tpr_proveedor_id, tpr_nombre, tpi_nombre, to_char(tpr_fecha_emision,'DD-mm-yyyy HH24:MI:SS'), substring(ttm_nombre,1,3), tpr_valor, tpr_tpi_id 
    
    FROM tbl_provisiones
    
    INNER JOIN tbl_tipo_persona ON tpi_id = tpr_tpi_id 

    INNER JOIN tbl_pais ON tpr_pai_id = pai_id 

    INNER JOIN tbl_tipo_moneda ON ttm_id = tpr_mon_id 

    WHERE tpr_ted_id = 4 AND tpr_fecha_emision BETWEEN '" + ending.ToString("yyyy-MM-dd").Substring(0, 10) + "' AND '" + starting.ToString("yyyy-MM-dd").Substring(0, 10) + @"' 
    
    ORDER BY tpr_fecha_emision DESC
");
            t = 0;
            string nit = "";
            int tpr_proveedor_id = 0, tpr_tpi_id = 0;
            //Response.Write (query);
            comm.CommandText = query;
            reader = comm.ExecuteReader();
            while (reader.Read())
            {
                tpr_proveedor_id = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                tpr_tpi_id = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);

                //nit = GetNit(tpr_proveedor_id, tpr_tpi_id);

                nit = GetNit2(tpr_proveedor_id, tpr_tpi_id, conn2, comm2, reader2);
              
                t++;
                html += "<tr>";
                html += "<td>" + t + "</td>";
                html += "<td>" + (reader.IsDBNull(0) ? "" : reader.GetString(0)) + "</td>"; //pais
                html += "<td>" + (reader.IsDBNull(1) ? "" : reader.GetString(1)) + "</td>"; //serie 
                html += "<td>" + (reader.IsDBNull(2) ? 0 : reader.GetInt32(2)) + "</td>";  //correlativo
                html += "<td>" + tpr_proveedor_id + "</td>";
                html += "<td>" + (reader.IsDBNull(4) ? "" : reader.GetString(4)) + "</td>"; //nombre
                html += "<td>" + nit + "</td>";
                html += "<td>" + (reader.IsDBNull(5) ? "" : reader.GetString(5)) + "</td>"; //tipo
                html += "<td>" + (reader.IsDBNull(6) ? "" : reader.GetString(6)) + "</td>"; //fecha
                html += "<td>" + (reader.IsDBNull(7) ? "" : reader.GetString(7)) + "</td>"; //moneda
                html += "<td>" + (reader.IsDBNull(8) ? 0 : reader.GetDecimal(8)) + "</td>";  //valor
                html += "</tr>";
            }
            DB.CloseObj(reader, comm, conn);

            DB.CloseObj(reader2, comm2, conn2);



            html += "</table>";
            html += "</div>";
        }
        catch (Exception ex)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(ex.Message);
            //return null;
        }

  }


    /*

    public static string GetNit(int id, int tp)
    {


        string nit = "";

        try
        {

            NpgsqlConnection conn2;
            NpgsqlCommand comm2;
            NpgsqlDataReader reader2;


            conn2 = DB.OpenMasterConnection();
            comm2 = new NpgsqlCommand();
            comm2.Connection = conn2;
            comm2.CommandType = CommandType.Text;

            try
            {
                conn2.Open();
            }
            catch (Exception ex)
            {
                nit = ex.Message;
                nit = "";
            }


            string query = (@"
        SELECT 
	        CASE WHEN tp = 2 THEN agentes.nit 
	        WHEN tp = 4 THEN proveedores.nit 
	        WHEN tp = 5 THEN navieras.nit 
	        WHEN tp = 6 THEN carriers.nit 
	        WHEN tp = 10 THEN intercompanys.nit 
	        ELSE '' END nit

        FROM (select " + id + " id, " + tp + @" tp) a

	        LEFT JOIN agentes ON agente_id = id and tp = 2
	        LEFT JOIN proveedores ON numero = id and tp = 4
	        LEFT JOIN navieras ON id_naviera = id and tp = 5
	        LEFT JOIN carriers ON carrier_id = id and tp = 6
	        LEFT JOIN intercompanys ON id_intercompany = id and tp = 10
        ");

            //Response.Write (query);
            comm2.CommandText = query;
            reader2 = comm2.ExecuteReader();
            if (reader2.Read())
            {
                nit = (reader2.IsDBNull(0) ? "" : reader2.GetString(0));

            }

            DB.CloseObj(reader2, comm2, conn2);



        }
        catch (Exception ex)
        {
            nit = ex.Message;
            nit = "";
        }

        return nit;
    }
    */


    /*
    public static string Base64UrlEncode(byte[] arg)
    {
        string s = Convert.ToBase64String(arg); // Regular base64 encoder
        s = s.Split('=')[0]; // Remove any trailing '='s
        s = s.Replace('+', '-'); // 62nd char of encoding
        s = s.Replace('/', '_'); // 63rd char of encoding
        return s;
    }

    public static byte[] Base64UrlDecode(string arg)
    {
        string s = arg;
        s = s.Replace('-', '+'); // 62nd char of encoding
        s = s.Replace('_', '/'); // 63rd char of encoding
        switch (s.Length % 4) // Pad with trailing '='s
        {
            case 0: break; // No pad chars in this case
            case 2: s += "=="; break; // Two pad chars
            case 3: s += "="; break; // One pad char
            default: throw new System.Exception(
              "Illegal base64url string!");
        }
        return Convert.FromBase64String(s); // Standard base64 decoder
    }
    */

    /*
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }



    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    */

}