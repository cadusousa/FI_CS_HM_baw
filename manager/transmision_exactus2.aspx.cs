using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using System.Data;
//using System.Runtime.Serialization.Json;
//using System.IO;
using System.Text;

using System.Web.Script.Serialization;


public partial class manager_transmision_exactus2 : System.Web.UI.Page
{
    public string html;
    public int s = 0, r = 0, t = 0;

    public DateTime starting = DateTime.Today;

    public DateTime ending; 

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

        public int COD_PAIS { get; set; }
        public string TIPO_ASIENTO { get; set; }      
        public string CUENTA_CONTABLE { get; set; }
        public string DESCRIPCION { get; set; }
        public string REFERENCIA { get; set; }
        public string DESCRIPCION_NIT { get; set; }
        public string NIT { get; set; }
        public decimal CREDITOS_LOCAL { get; set; }
        public decimal CREDITOS_DOLAR { get; set; }
        public decimal DEBITO_LOCAL { get; set; }
        public decimal DEBITO_DOLAR { get; set; }
        public string MONEDA { get; set; }                   
        public decimal TIPO_CAMBIO { get; set; }
        public string REGS { get; set; }
            
    }   

    

    protected void Page_Load(object sender, EventArgs e)
    {

        int id = 0;
        string op = "";
        string query = "";


        try {

            id = int.Parse(Request.QueryString["id"]);

        }
        catch (Exception ex)
        {

        }

        try {

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

            try {
    
                string temp;

                temp = tb_fechainicial.Text;

                ending = DateTime.Parse(temp); //.ToString("yyyy-MM-dd");

                //starting = DateTime.ParseExact(temp, "yyyy-MM-dd", null);

                temp = tb_fechafinal.Text;

                starting = DateTime.Parse(temp); //, "yyyy-MM-dd", null);
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
                    fecha_libro_diario > (CURRENT_DATE - interval '1 month') AND estatus = 3
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

            //List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = starting; date >= ending; date = date.AddDays(-1))
            {
                //allDates.Add(date);
                rgb.strC1 = date.ToString("yyyy-MM-dd").Substring(0, 10);
                object[] objArr = { 0, 0, rgb.strC1, "", "", "", "", "", "" };
                dt.Rows.Add(objArr);
            }

            

            query = (@"SELECT id, estatus, to_char(fecha_libro_diario,'YYYY-MM-DD'), to_char(date_send,'DD-mm-yyyy HH24:MI:SS'), to_char(date_response,'DD-mm-yyyy HH24:MI:SS'), msg, usuario, json_send, json_response, tipo 
            FROM tbl_transmision_exactus 
            WHERE 
                fecha_libro_diario BETWEEN '" + ending.ToString("yyyy-MM-dd").Substring(0, 10) + "' AND '" + starting.ToString("yyyy-MM-dd").Substring(0, 10)  + @"' OR estatus <> 3
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

            string url = "http://localhost/TrasnmitionExactus.php?user=" + user.ID + "&fecha=";


            List<int> arr_estatus = new List<int>();
            List<string> arr_msg = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                arr_estatus.Add(int.Parse(dr[1].ToString()));
                arr_msg.Add(dr[1].ToString());
                if (dr[1].ToString() == "3")
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
<th class=th1 align=center title='Respuesta'>R</th>
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

                if (t < dt.Rows.Count-1)
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
                    if (arr_estatus[t + 1] == 3 || arr_msg[t+1] == "No se encontraron registros") //si el dia siguiente se respuesta
                    { 
                        f = 0;
                    }
                    
                }
                else {
                    f = 0;
                }

                if (dr[5].ToString() != "Registro de inicio")
                {

                    t++;

                    html += "<tr>";
                    html += "<td class=td1 align=center style='font-family:courier'>" + t + "</td>";

                    if (t == 1 || f == 1)
                        html += "<td class=td1 align=center  style='font-family:courier;color:silver'>" + dr[2].ToString() + "</td>";
                    else
                        html += "<td class=td1 align=center style='font-family:courier;color:blue;'>" + (dr[1].ToString() == "3" ? "" : "<a href='" + url + dr[2].ToString() + "' target=iframe_php onclick=\"document.getElementById('flag').value = '1';move();\" style='color:blue' title='Transmiti a Exactus'>" + dr[2].ToString() + "</a>") + "</td>";

                    html += "<td class=td1 align=left style='font-family:courier'>" + dias[dia] + "</td>";

                    html += "<td class=td1 align=center>" + (dr[1].ToString() == "3" ? dr[2].ToString() : "") + "</td>";  //→←↑↓

                    html += "<td class=td1 align=center>" + (int.Parse(dr[1].ToString()) >= 2 && dr[7].ToString() != "" ? "<a href='transmision_exactus2.aspx?id=" + dr[0].ToString() + "&o=S' title='Json Enviado' style='font-weight:bolder;color:blue;border:1px solid gray;background:white;'>&nbsp;" + "↑" + "&nbsp;</a>" : "") + "</td>";

                    html += "<td class=td1 align=center>" + (int.Parse(dr[1].ToString()) >= 2 && dr[8].ToString() != "" ? "<a href='transmision_exactus2.aspx?id=" + dr[0].ToString() + "&o=R' title='Respuesta' style='font-weight:bolder;color:red;border:1px solid gray;background:white;'>&nbsp;" + "↓" + "&nbsp;</a>" : "") + "</td>";

                    html += "<td class=td1 align=center>" + dr[3].ToString() + "</td>";
                    html += "<td class=td1 align=center>" + dr[4].ToString() + "</td>";
                    html += "<td class=td1 align=left>" + dr[5].ToString() + "</td>";
                    html += "<td class=td1 align=center>" + dr[6].ToString() + "</td>";
                    html += "</tr>";

                }

                if (id == int.Parse(dr[0].ToString()) && int.Parse(dr[1].ToString()) > 0)
                {
                    int c = 0;
                    string respuesta = "", htm = "", temp;

 
                    if (op == "R")
                    {
                        respuesta = dr[8].ToString();

                        htm += "<center><h1>Respuesta Exactus&nbsp;&nbsp;&nbsp;<a href='transmision_exactus2.aspx'><input type=button value=X title='cerrar detalles'></a></h1></center>";
         
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

                        htm += "<center><h1>Datos Enviados&nbsp;&nbsp;&nbsp;<a href='transmision_exactus2.aspx'><input type=button value=X title='cerrar detalles'></a></h1></center>";
         
                        htm += (@"<table valign=top border=1 class=ex_details>
<tr>
<th></th>
<th>COD_PAIS</th>
<th>TIPO_ASIENTO</th>
<th>CUENTA_CONTABLE</th>
<th>DESCRIPCION</th>
<th>REFERENCIA</th>
<th>DESCRIPCION_NIT</th>
<th>NIT</th>
<th>CREDITOS_LOCAL</th>
<th>CREDITOS_DOLAR</th>
<th>DEBITO_LOCAL</th>
<th>DEBITO_DOLAR</th>
<th>MONEDA</th>
<th>TIPO_CAMBIO</th>
<th>REGS</th>
</tr>");
                    }

          
 
                    respuesta = respuesta.Replace("[", "").Replace("]", "");

                    List<string> lineas = respuesta.Split('}').ToList();

                    char[] MyChar = { ',' };

                    foreach (string linea in lineas)
                    {
                        c++;
                        try {

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

                            } else {

                                exactus_sen result = (exactus_sen)DeserializeJson<exactus_sen>(temp);

                                htm += "<tr>";
                                htm += "<td>" + c + "</td>";
                                htm += "<td>" + result.COD_PAIS + "</td>";
                                htm += "<td>" + result.TIPO_ASIENTO + "</td>";
                                htm += "<td>" + result.CUENTA_CONTABLE + "</td>";
                                htm += "<td>" + result.DESCRIPCION + "</td>";
                                htm += "<td>" + result.REFERENCIA + "</td>";
                                htm += "<td>" + result.DESCRIPCION_NIT + "</td>";
                                htm += "<td>" + result.NIT + "</td>";
                                htm += "<td>" + result.CREDITOS_LOCAL + "</td>";
                                htm += "<td>" + result.CREDITOS_DOLAR + "</td>";
                                htm += "<td>" + result.DEBITO_LOCAL + "</td>";
                                htm += "<td>" + result.DEBITO_DOLAR + "</td>";
                                htm += "<td>" + result.MONEDA + "</td>";
                                htm += "<td>" + result.TIPO_CAMBIO + "</td>";
                                htm += "<td>" + result.REGS + "</td>";
                                htm += "</tr>"; 
                            }
                        }
                        catch (Exception ex)
                        {
                            log4net ErrLog = new log4net();
                            ErrLog.ErrorLog(ex.Message);
                            //return null;
                        }
                    }
                    htm += "</table>";



                    html += "<tr>";
                    html += "<td colspan=1>" + "" + "</td>";
                    html += "<td colspan=9 style='border:1px solid black;background:lightblue;padding:3px;'><div style=\"display:table-inline;height:200px;width:700px;overflow:scroll;\">" + htm + "</div></td>";
                    html += "</tr>";           
                
                }

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



    protected void btn_reporte_Click(object sender, EventArgs e)
    {

        string mensaje = "<script languaje=\"JavaScript\">";
   
        mensaje += "window.open('transmision_exactus2.aspx?reporte=1','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }


}