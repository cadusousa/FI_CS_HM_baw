using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using Npgsql;
using MySql.Data.MySqlClient;
using System.Data.Odbc;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.IO;

public partial class operations_seguros : System.Web.UI.Page
{
    string[,] provisiones = new string[54,2];
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        

    }
    protected void Generar_Provisiones(UsuarioBean user)
    {
        user.PaisID = 1;
        user.pais.ISO = "GT";
        string correlativo = "";
        int NoCorr = 0;
        int prov_id = 0, corr_id = 0;
        int partidaNo = 0;
        string partida = "";
        int contaID = 1;
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        NpgsqlDataReader reader;
        RE_GenericBean Bean = null;
        try
        {
            conn = new NpgsqlConnection("Server=10.10.1.18;Port=5432;User Id=dbmaster;Password=aimargt;Database=aimar_baw_dennis;POOLING=True;MINPOOLSIZE=2;MAXPOOLSIZE=600");
            conn.Open();
            comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from tbl_provisiones " +
            "where tpr_serie='PSG' and tpr_correlativo in (151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,182,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,199,200,201,202,203,204) and tpr_pai_id=1";
            reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Bean = new RE_GenericBean();
                Bean.intC1 = int.Parse(reader.GetValue(0).ToString());//ID
                Bean.intC2 = int.Parse(reader.GetValue(1).ToString());//PROVEEDOR_ID
                Bean.strC1 = reader.GetValue(2).ToString();//tpr_fact_id
                Bean.strC2 = reader.GetValue(3).ToString();//tpr_fact_fecha
                Bean.strC3 = reader.GetValue(4).ToString();//tpr_fecha_maxpago
                Bean.decC1 = decimal.Parse(reader.GetValue(5).ToString());//tpr_valor
                Bean.decC2 = decimal.Parse(reader.GetValue(6).ToString());//tpr_afecto
                Bean.decC3 = decimal.Parse(reader.GetValue(7).ToString());//tpr_noafecto
                Bean.decC4 = decimal.Parse(reader.GetValue(8).ToString());//tpr_iva
                Bean.strC5 = reader.GetValue(9).ToString();//tpr_observacion
                Bean.intC3 = int.Parse(reader.GetValue(10).ToString());//tpr_suc_id
                Bean.intC4 = int.Parse(reader.GetValue(11).ToString());//tpr_pai_id
                Bean.strC6 = reader.GetValue(12).ToString();//tpr_usu_creacion
                Bean.strC7 = reader.GetValue(13).ToString();//tpr_fecha_creacion
                Bean.strC8 = reader.GetValue(14).ToString();//tpr_usu_acepta
                Bean.strC9 = reader.GetValue(15).ToString();//tpr_fecha_acepta
                Bean.intC5 = int.Parse(reader.GetValue(16).ToString());//tpr_departamento
                Bean.intC6 = int.Parse(reader.GetValue(17).ToString());//tpr_ted_id
                Bean.strC10 = reader.GetValue(18).ToString();//tpr_serie
                Bean.strC11 = reader.GetValue(19).ToString();//tpr_serie_oc
                Bean.strC12 = reader.GetValue(20).ToString();//tpr_correlativo_oc
                Bean.intC7 = int.Parse(reader.GetValue(21).ToString());//tpr_tts_id
                Bean.strC13 = reader.GetValue(22).ToString();//tpr_hbl
                Bean.strC14 = reader.GetValue(23).ToString();//tpr_mbl
                Bean.strC15 = reader.GetValue(24).ToString();//tpr_routing
                Bean.strC16 = reader.GetValue(25).ToString();//tpr_contenedor
                Bean.intC8 = int.Parse(reader.GetValue(26).ToString());//tpr_tpi_id
                Bean.intC9 = int.Parse(reader.GetValue(27).ToString());//tpr_correlativo
                Bean.intC10 = int.Parse(reader.GetValue(28).ToString());//tpr_mon_id
                Bean.strC17 = reader.GetValue(29).ToString();//tpr_serie_contrasena
                Bean.intC11 = int.Parse(reader.GetValue(30).ToString());//tpr_contrasena_correlativo
                Bean.decC5 = decimal.Parse(reader.GetValue(31).ToString());//tpr_valor_equivalente
                Bean.strC18 = reader.GetValue(32).ToString();//tpr_fact_corr
                Bean.strC19 = reader.GetValue(33).ToString();//tpr_proveedor_cajachica
                Bean.intC12 = int.Parse(reader.GetValue(34).ToString());//tpr_imp_exp_id
                Bean.intC13 = int.Parse(reader.GetValue(35).ToString());//tpr_bien_serv
                Bean.strC20 = reader.GetValue(36).ToString();//tpr_tcon_id--
                Bean.strC21 = reader.GetValue(37).ToString();//tpr_fecha_emision--
                Bean.strC22 = reader.GetValue(38).ToString();//tpr_nombre
                Bean.strC23 = reader.GetValue(39).ToString();//tpr_proveedor_cajachica_id--
                Bean.strC24 = reader.GetValue(40).ToString();//tpr_poliza
                Bean.strC25 = reader.GetValue(41).ToString();//tpr_fiscal
                Bean.strC26 = reader.GetValue(42).ToString();//tpr_fecha_libro_compras
                Bean.strC27 = reader.GetValue(43).ToString();//tpr_tto_id--
                Bean.strC28 = reader.GetValue(44).ToString();//tpr_ruta_pais
                Bean.strC29 = reader.GetValue(45).ToString();//tpr_ruta
                Bean.strC30 = reader.GetValue(46).ToString();//tpr_blid--
                Bean.strC31 = reader.GetValue(47).ToString();//tpr_tti_id--
                Bean.strC32 = reader.GetValue(48).ToString();//tpr_usu_modifica_regimen
                Bean.strC33 = reader.GetValue(49).ToString();//tpr_usu_anula
                Bean.strC34 = reader.GetValue(50).ToString();//tpr_fecha_anula
                Bean.strC35 = reader.GetValue(51).ToString();//tpr_toc_id--




                // Obtengo el ID
                comm.CommandText = "select nextval('id_provision')";
                prov_id = int.Parse(comm.ExecuteScalar().ToString());
                //halo el correlativo de la partida y genero su codigo
                comm.CommandText = "select spv_value from sys_partidas_value where spv_pai_id =@paiID and spv_tcon_id=@tconID";
                comm.Parameters.Add("@paiID", NpgsqlTypes.NpgsqlDbType.Integer).Value = user.PaisID;
                comm.Parameters.Add("@tconID", NpgsqlTypes.NpgsqlDbType.Integer).Value = contaID;
                partidaNo = int.Parse(comm.ExecuteScalar().ToString());
                comm.Parameters.Clear();
                partida = Utility.GeneroPartida(user.pais.ISO, partidaNo, contaID);
                partidaNo += 1;
                //Actualizo el correlativo de la partida
                comm.CommandText = "update sys_partidas_value set spv_value=@value where spv_pai_id=@paiID and spv_tcon_id=@tconID";
                comm.Parameters.Add("@paiID", NpgsqlTypes.NpgsqlDbType.Integer).Value = user.PaisID;
                comm.Parameters.Add("@value", NpgsqlTypes.NpgsqlDbType.Integer).Value = partidaNo;
                comm.Parameters.Add("@tconID", NpgsqlTypes.NpgsqlDbType.Integer).Value = contaID;
                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
                corr_id = DB.GetCorr(user.SucursalID, 5, "PSG");

            }

        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
        }
    }
}