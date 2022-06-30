using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using System.Data;

public partial class manager_transmision_exactus : System.Web.UI.Page
{

    public string html;

    protected void Page_Load(object sender, EventArgs e)
    {
        html = "<table width=700 valign=top border=1><tr><td>id</td><td>Estado</td><td>Fecha Envio</td><td>Fecha Respuesta</td></tr>";

        string query = (@"
            select  max(id), max(estatus), to_char(date_send,'YYYY-MM-DD'), MAX(to_char(date_response,'YYYY-MM-DD')) 
            -- json_send, json_response, tipo, estatus, msg, usuario

            from tbl_transmision_exactus

            group by to_char(date_send,'YYYY-MM-DD') -- , to_char(date_response,'YYYY-MM-DD') -- , tipo, estatus, msg, usuario 

            order by to_char(date_send,'YYYY-MM-DD') desc -- , estatus desc");

        //RE_GenericBean result = new RE_GenericBean();
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        NpgsqlDataReader reader;
        try
        {
            conn = DB.OpenConnection();
            comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = query;                        
            //comm.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = id;
            reader = comm.ExecuteReader();
            while (reader.Read())
            {
                html += "<tr>";
                if (!reader.IsDBNull(0)) html += "<td>" + reader.GetInt32(0) + "</td>";
                if (!reader.IsDBNull(1)) html += "<td>" + reader.GetInt32(1) + "</td>";
                if (!reader.IsDBNull(2)) html += "<td>" + reader.GetString(2) + "</td>";
                if (!reader.IsDBNull(3)) html += "<td>" + reader.GetString(3) + "</td>";
                html += "</tr>";
            }
            DB.CloseObj(reader, comm, conn);

            html += "</html>";
        }
        catch (Exception ex)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(ex.Message);
            //return null;
        }


    }
}