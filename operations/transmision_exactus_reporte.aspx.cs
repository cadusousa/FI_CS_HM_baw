using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using System.Data;
using System.Web.Script.Serialization;

public partial class operations_transmision_exactus_reporte : System.Web.UI.Page
{

    public class exactus_sen
    {
        public int OID { get; set; }
        public int LINEAS { get; set; }
        public string FECHA { get; set; }
        public string ASIENTO { get; set; }
        public string TIPO_ASIENTO { get; set; }
        public string CUENTA_CONTABLE { get; set; }

        public int COD_COMPANY { get; set; }
        public string COD_PAIS { get; set; }

        public decimal CREDITOS_LOCAL { get; set; }
        public decimal CREDITOS_DOLAR { get; set; }
        public decimal DEBITO_LOCAL { get; set; }
        public decimal DEBITO_DOLAR { get; set; }
        public string MONEDA { get; set; }
        public decimal TIPO_CAMBIO { get; set; }

        public string DESCRIPCION { get; set; }
        public string FUENTE { get; set; }
        public string REFERENCIA { get; set; }
        public string DESCRIPCION_NIT { get; set; }
        public string NIT { get; set; }

        public decimal MONTO { get; set; }
        public decimal MONTO_SIN_IVA { get; set; }
        public decimal IVA { get; set; }

    }



    public class exactus_senad
    {
        public int OID { get; set; }
        public int LINEAS { get; set; }

        public int C { get; set; }
        public string REGS { get; set; }

        public string FECHA { get; set; }
        public string ASIENTO { get; set; }
        public string TIPO_ASIENTO { get; set; }
        public string CUENTA_CONTABLE { get; set; }

        public int COD_COMPANY { get; set; }
        public string COD_PAIS { get; set; }

        public decimal CREDITOS_LOCAL { get; set; }
        public decimal CREDITOS_DOLAR { get; set; }
        public decimal DEBITO_LOCAL { get; set; }
        public decimal DEBITO_DOLAR { get; set; }
        public string MONEDA { get; set; }
        public decimal TIPO_CAMBIO { get; set; }

        public string DESCRIPCION { get; set; }
        public string FUENTE { get; set; }
        public string REFERENCIA { get; set; }
        public string DESCRIPCION_NIT { get; set; }
        public string NIT { get; set; }

        public decimal MONTO { get; set; }
        public decimal MONTO_SIN_IVA { get; set; }
        public decimal IVA { get; set; }

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





    public object DeserializeJson<T>(string Json)
    {
        JavaScriptSerializer JavaScriptSerializer = new JavaScriptSerializer();
   
        return JavaScriptSerializer.Deserialize<T>(Json);
    }


    public string SerializeJson(object obj)
    {
        JavaScriptSerializer JavaScriptSerializer = new JavaScriptSerializer();

        return JavaScriptSerializer.Serialize(obj);
    }

    public int id = 0;
    public string tp = "", way = "", usr = "", titulo = "", fecha_libro_diario = "", date_send = "", date_response = "", msg = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            id = int.Parse(Request.QueryString["id"]);
        }
        catch (Exception ex)
        {
 
        }

        try
        {
            tp = Request.QueryString["t"];
        }
        catch (Exception ex)
        {

        }

        try
        {
            way = Request.QueryString["w"];
        }
        catch (Exception ex)
        {

        }

        try
        {
            usr = Request.QueryString["u"];
        }
        catch (Exception ex)
        {

        }



        switch (id) {
            case -1 :
                
                DateTime ending = DateTime.Now, starting = DateTime.Now;  

                try
                {
                    ending = DateTime.Parse(Request.QueryString["ending"]);
                }
                catch (Exception ex)
                {

                }
                      
                try
                {
                    starting = DateTime.Parse(Request.QueryString["starting"]);
                }
                catch (Exception ex)
                {

                }

                

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment;filename=ReporteProvisionesActivo.xls");
                Response.Write(DB.ReporteProvisiones(starting, ending, "ex"));
                Response.End();


                break;

            case 0:
                break;

            default :
                DownloadJson(id, tp, way);
            break;

        }
       
        
        
    }




    public void DownloadJson(int id, string tp, string way)
    {

        try
        {

            NpgsqlConnection conn;
            NpgsqlCommand comm;
            NpgsqlDataReader reader;

            conn = DB.OpenConnection();
            comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;

            string json_send = "", json_result = "";  //to_char(fecha_libro_diario,'YYYY-MM-DD'),

            string query = (@"SELECT id, estatus, 
                to_char(fecha_libro_diario,'DD/mm/yyyy'),
                to_char(date_send,'DD/mm/yyyy HH24:MI:SS'), 
                to_char(date_response,'DD/mm/yyyy HH24:MI:SS'),
                msg, usuario, json_send, json_response, tipo 
            FROM 
                tbl_transmision_exactus 
            WHERE id = " + id);

            //Response.Write (query);
            comm.CommandText = query;
            reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(2)) fecha_libro_diario = reader.GetString(2);
                if (!reader.IsDBNull(3)) date_send = reader.GetString(3);
                if (!reader.IsDBNull(4)) date_response = reader.GetString(4);
                if (!reader.IsDBNull(5)) msg = reader.GetString(5);
                if (!reader.IsDBNull(7)) json_send = reader.GetString(7);
                if (!reader.IsDBNull(8)) json_result = reader.GetString(8);
            }
            DB.CloseObj(reader, comm, conn);


            switch (tp) {
                case "js": //json

                    if (usr == "1")
                    {
                        if (json_send != "" && way == "s")
                            ProcesaJson<exactus_senad>(json_send);

                        if (json_result != "" && way == "r")
                            ProcesaJson<exactus_res>(json_result);
                    }
                    else 
                    {
                        if (json_send != "" && way == "s")
                            ProcesaJson<exactus_sen>(json_send);

                        if (json_result != "" && way == "r")
                            ProcesaJson<exactus_res>(json_result);
                    }


                    break;

                case "ht": //html
                case "ex": //excel

                    if (usr == "1")
                    {

                        if (json_send != "" && way == "s")
                            ProcesaHtml<exactus_senad>(json_send);

                        if (json_result != "" && way == "r")
                            ProcesaHtml<exactus_res>(json_result);

                    } 
                    else
                        {
                        if (json_send != "" && way == "s")
                            ProcesaHtml<exactus_sen>(json_send);

                        if (json_result != "" && way == "r")
                            ProcesaHtml<exactus_res>(json_result);

                    }
                break;

                    /*
                case "ex":  //excel

                    
                    if (usr == "1")
                    {

                        if (json_send != "" && way == "s")
                            ProcesaExcel<exactus_senad>(json_send);

                        if (json_result != "" && way == "r")
                            ProcesaExcel<exactus_res>(json_send);

                    } else {
                        if (json_send != "" && way == "s")
                            ProcesaExcel<exactus_sen>(json_send);

                        if (json_result != "" && way == "r")
                            ProcesaExcel<exactus_res>(json_send);
                    }
                    break;
                     * */
            }


        }
        catch (Exception ex)
        {
            //Response.Write(ex.Message);
        }

    }


   public void ProcesaJson<T>(string json)
    {
        try
        {

            var json_file = DeserializeJson<T[]>(json);

            string json_exactus = "";

            json_exactus = SerializeJson(json_file);

            Response.AddHeader("Content-disposition", "attachment; filename=" + way.ToUpper() + fecha_libro_diario + ".json");
            Response.ContentType = "application/octet-stream";

            Response.Write(json_exactus);
            Response.End();

        }
        catch (Exception ex)
        {
            //Response.Write(ex.Message);
        }

    }

    /*
    public void ProcesaJson2<T>(string json)
    {
        try
        {

            string temp = "", json_exactus = "";

            json = json.Replace("[", "").Replace("]", "");

            List<string> lineas = json.Split('}').ToList();

            char[] MyChar = { ',' };

            foreach (string linea in lineas)
            {
                try
                {
                    temp = linea + "}";

                    if (temp.Substring(0, 1) == ",")
                    {
                        temp = temp.TrimStart(MyChar);
                    }

                    temp = temp.Replace("NULL", "\"\"").Replace("\"-\"", "\"\"");
           
                    T result = (T)DeserializeJson<T>(temp); 

                    if (json_exactus != "") json_exactus += ",\n";

                    json_exactus += SerializeJson(result);

                }
                catch (Exception ex)
                {
                    temp = ex.Message;

                    //log4net ErrLog = new log4net();
                    //ErrLog.ErrorLog(ex.Message);
                    //return null;
                }
            }

            json_exactus = "[\n" + json_exactus + "\n]\n";

            Response.AddHeader("Content-disposition", "attachment; filename=" + way.ToUpper() + fecha_libro_diario + ".json");
            Response.ContentType = "application/octet-stream";

            json_exactus = json_exactus.Replace("Subproceso anulado.", "");

            Response.Write(json_exactus);
            Response.End();

        }
        catch (Exception ex)
        {
            //Response.Write(ex.Message);
        }

    }
    */


    public void ProcesaHtml<T>(string json)    
    {
        try
        {

            string html = "", usuario = "", admin = "";

            //json = json.Replace("[", "").Replace("]", "");
            //List<string> lineas = json.Split('}').ToList();
            //char[] MyChar = { ',' };

            titulo = "Datos " + (way == "r" ? "Resultado" : "Enviados") + " " + fecha_libro_diario + " " + (usr == "0" ? "" : "ADMIN") + " Mensaje : " + msg;

            html = (@"<table width=100%>
                        <tr><td colspan=25 align=center>
                            <center><h1>" + titulo + @"</h1></center>
                        </td></tr>");


            if (tp == "ht")
            {
                usuario = (@"
<table align=center style='align:center'>
<tr>
    <td colspan=2>LINKS DESCARGA</td>
</tr>

<tr>
    <td>
        <a href='transmision_exactus_reporte.aspx?u=0&w=s&t=js&id=" + id + @"' target=_blank><h2>Json Enviado</h2></a>    
    </td>
    <td>
        <a href='transmision_exactus_reporte.aspx?u=0&w=r&t=js&id=" + id + @"' target=_blank><h2>Json Resultado</h2></a>    
    </td>
</tr>

<tr>
    <td>
        <a href='transmision_exactus_reporte.aspx?u=0&w=s&t=ex&id=" + id + @"' target=_blank><h2>Excel Enviado</h2></a>    
    </td>
    <td>
        <a href='transmision_exactus_reporte.aspx?u=0&w=r&t=ex&id=" + id + @"' target=_blank><h2>Excel Resultado</h2></a>    
    </td>
</tr>

<tr>
    <td>
        <a href='transmision_exactus_reporte.aspx?u=0&w=s&t=ht&id=" + id + @"'><h2>Html Enviado</h2></a>    
    </td>
    <td>
        <a href='transmision_exactus_reporte.aspx?u=0&w=r&t=ht&id=" + id + @"'><h2>Html Resultado</h2></a>    
    </td>
</tr>
</table>");

                UsuarioBean user;

                user = (UsuarioBean)Session["usuario"];

                if (user.ID == "soporte7" && usr == "1")
                {
                    admin = usuario.Replace("u=0", "u=1").Replace("colspan=2>", "colspan=2>ADMIN ");
                }

                html += (@"                        
                    <tr>                            
                        <td colspan=12 align=center>
                            <center>
                            " + usuario + @"
                            </center>
                        </td>
                        <td colspan=13 align=center>
                            <center>
                            " + admin + @"
                            </center>
                        </td>
                    </tr>");

            }

            int c = 0;

            var json_file = DeserializeJson<T[]>(json);

            foreach (T json_row in (IEnumerable<T>)json_file)
            {
                c++;

                if (c == 1)
                {
                    html += "<tr><th></th>";
                    foreach (var prop in json_row.GetType().GetProperties())
                    {
                        html += "<th>" + prop.Name + "</th>";
                    }
                    html += "</tr>";
                }

                html += "<tr><td>" + c + "</td>";
                foreach (var prop in json_row.GetType().GetProperties())
                {
                    html += "<td>" + prop.GetValue(json_row, null) + "</td>";
                }
                html += "</tr>";

            }

/*
            foreach (string linea in lineas)
            {
                try
                {
                    c++;

                    temp = linea + "}";

                    if (temp.Substring(0, 1) == ",")
                    {
                        temp = temp.TrimStart(MyChar);
                    }

                    temp = temp.Replace("NULL", "\"\"").Replace("\"-\"", "\"\"");

                    T result = (T)DeserializeJson<T>(temp); 

                    if (c == 1)
                    {
                        html += "<tr><th></th>";
                        foreach (var prop in result.GetType().GetProperties())
                        {
                            //Console.WriteLine(prop.Name + ": " + prop.GetValue(result, null));

                            html += "<th>" + prop.Name + "</th>";
                        }
                        html += "</tr>";
                    }


                    
                    //html += "<tr>";
                    //foreach (var field in result.GetType().GetFields())
                    //{
                    //    //Console.WriteLine(field.Name + ": " + field.GetValue(result));
                    //    html += "<td>" + field.GetValue(result) + "</td>";
                    //}
                    //html += "</tr>";


                    html += "<tr><td>" + c + "</td>";
                    foreach (var prop in result.GetType().GetProperties())
                    {
                        //Console.WriteLine(prop.Name + ": " + prop.GetValue(result, null));
                        html += "<td>" + prop.GetValue(result, null) + "</td>";
                    }
                    html += "</tr>";

                }
                catch (Exception ex)
                {
                    temp = ex.Message;

                    //log4net ErrLog = new log4net();
                    //ErrLog.ErrorLog(ex.Message);
                    //return null;
                }
            }
*/
            html += "</table>";

            if (tp == "ex")
            {
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + way.ToUpper() + fecha_libro_diario + ".xls");
                Response.Write(html);
                Response.End();
            }
            else
            {
                Response.Write(html);
            }
            
        }
        catch (Exception ex)
        {
            //Response.Write(ex.Message);
        }

    }





}