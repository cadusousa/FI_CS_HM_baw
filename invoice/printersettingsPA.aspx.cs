using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using Npgsql;
using MySql.Data.MySqlClient;
using System.Data.Odbc;

public partial class _Default : System.Web.UI.Page
{
    UsuarioBean user;
    int fac_id = 0, tipo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        user = (UsuarioBean)Session["usuario"];
        if (Request.QueryString["fac_id"] != null)
        {
            fac_id = int.Parse(Request.QueryString["fac_id"].ToString());
            tipo = int.Parse(Request.QueryString["tipo"].ToString());
            tb_factID.Text = fac_id.ToString();
            lb_tipo.Text = tipo.ToString();
        }
        if (!IsPostBack)
        {
            PopulateInstalledPrintersCombo();
        }
    }

    protected void btn_print_Click(object sender, EventArgs e)
    {
        PrintProcess(0);
    }

    protected void btn_reprint_Click(object sender, EventArgs e)
    {
        PrintProcess(1);
    }

    private void PrintProcess(int Process)
    {
        string[] PrinterSets = lb_impresora.SelectedValue.Split(',');
        string FileName = tipo + "-" + fac_id;
        string PathFile = @"\\" + PrinterSets[0] + PrinterSets[1];
        user = (UsuarioBean)Session["usuario"];
        Session["usuario"] = user;
        int NuevoNumeroImprimir = 0;
        string S1File = "S1-" + tipo;
        string ComandoServicio = "";
        string NombreTipoDoc = "";
        StreamWriter sw;

        //Cambiando al Usuario de la maquina para poder administrar la carpeta
        ImpersonationSettings settings = new ImpersonationSettings();
        UserImpersonation userImpersonation = new UserImpersonation(settings);
        try
        {
          userImpersonation.Impersonate();

            //Se verifica si hay acceso a la carpeta para poner archivo de impresion
            if (System.IO.Directory.Exists(PathFile))
            {
                //1. Limpiando la ultima consulta del ultimo correlativo
                System.IO.File.Delete(@"\\" + PathFile + S1File + "-I.txt");
                System.IO.File.Delete(@"\\" + PathFile + S1File + "-R.txt");

                //Obteniendo el ultimo correlativo de la impresora para continuar con la secuencia
                sw = new StreamWriter(@"\\" + PathFile + S1File + "-I.txt", true);
                sw.WriteLine(fac_id);
                sw.Flush();
                sw.Close();
                int UltimoNumeroImpreso = GetResultImpresion(@"\\" + PathFile + S1File + "-R.txt");
                System.IO.File.Delete(@"\\" + PathFile + S1File + "-R.txt");

                //si devuelve un numero mayor o igual a 0, obtuvo correlativo
                if (UltimoNumeroImpreso >= 0)
                {
                    NuevoNumeroImprimir = UltimoNumeroImpreso + 1;
                }
                // si devuelve un numero menor a 0, obtuvo un error
                else if (UltimoNumeroImpreso < 0)
                {
                    WebMsgBox.Show(ShowTallyPrintMsgs(UltimoNumeroImpreso));
                    return;
                }

                //2. Obteniendo los datos de la factura
                RE_GenericBean Header = new RE_GenericBean();
                RE_GenericBean result = null;
                NpgsqlConnection conn;
                NpgsqlCommand comm;
                NpgsqlDataReader reader;
                ArrayList Rubros = new ArrayList();
                try
                {
                    conn = DB.OpenConnection();
                    comm = new NpgsqlCommand();
                    comm.Connection = conn;
                    comm.CommandType = CommandType.Text;
                    //Obteniendo el encabezado del documento
                    if (tipo == 1)
                    {
                        comm.CommandText = "select tfa_nombre, tfa_nit, cast(tfa_correlativo as numeric), '0', tfa_cli_id, tfa_serie, tfa_mbl, tfa_contenedor, tfa_shipper, tfa_peso, tfa_volumen, tfa_usu_id, tfa_observacion from tbl_facturacion where tfa_id=" + fac_id + " and tfa_pai_id=" + user.PaisID;
                        ComandoServicio = "";
                        NombreTipoDoc = "FACTURA";
                    }
                    else if (tipo == 4)
                    {
                        comm.CommandText = "select tnd_nombre, tnd_nit, tnd_correlativo, '0', tnd_cli_id, tnd_serie, tnd_mbl, tnd_contenedor, tnd_shipper, tnd_peso, tnd_volumen, tnd_usu_id, tnd_observacion from tbl_nota_debito where tnd_id=" + fac_id + " and tnd_pai_id=" + user.PaisID;
                        ComandoServicio = "`";
                        NombreTipoDoc = "ND";
                    }
                    else if (tipo == 3)
                    {
                        comm.CommandText = "select tnc_nombre, tnc_nit, tnc_correlativo, tfa_serie || '-' || tfa_correlativo, tnc_cli_id, tnc_serie, tnc_mbl, tnc_contenedor, '', '', '', tnc_usu_id, '' from tbl_nota_credito, tbl_factura_abono, tbl_facturacion where tnc_id=" + fac_id + " and tnc_pai_id=" + user.PaisID + " and tfr_tre_id=tnc_id and tfr_tfa_id=tfa_id and tfr_sysref_id=3 and tfr_tfa_sysref_id in (1,4)";
                        ComandoServicio = "d";
                        NombreTipoDoc = "NC";
                    }
                    reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0)) Header.strC1 = reader.GetString(0);//nombre
                        if (!reader.IsDBNull(1)) Header.strC2 = reader.GetString(1);//nit
                        if (!reader.IsDBNull(2)) Header.intC1 = int.Parse(reader.GetValue(2).ToString());//correlativo
                        if (!reader.IsDBNull(3)) Header.strC12 = reader.GetValue(3).ToString();//correlativo de la factura asociada, aplica para NCs
                        if (!reader.IsDBNull(4)) Header.strC3 = reader.GetValue(4).ToString();//id cliente
                        if (!reader.IsDBNull(5)) Header.strC4 = reader.GetValue(5).ToString();//serie
                        if (!reader.IsDBNull(6)) Header.strC5 = reader.GetValue(6).ToString();//mbl
                        if (!reader.IsDBNull(7)) Header.strC6 = reader.GetValue(7).ToString();//contenedor
                        if (!reader.IsDBNull(8)) Header.strC7 = reader.GetValue(8).ToString();//shipper
                        if (!reader.IsDBNull(9)) Header.strC8 = reader.GetValue(9).ToString();//peso
                        if (!reader.IsDBNull(10)) Header.strC9 = reader.GetValue(10).ToString();//volumen
                        if (!reader.IsDBNull(11)) Header.strC10 = reader.GetValue(11).ToString();//usuario
                        if (!reader.IsDBNull(12)) Header.strC11 = reader.GetValue(12).ToString();//observacion
                    }

                    //Obteniendo el detalle de la factura o nota debito
                    comm.Parameters.Clear();
                    if ((tipo == 1) || (tipo == 4))
                    {
                        comm.CommandText = "select rub_nombre, tdf_monto, tdf_montosinimpuesto, tdf_impuesto, tdf_comentarios from tbl_detalle_facturacion, tbl_rubro where rub_id=tdf_rub_id and tdf_ttr_id=" + tipo + " and tdf_tfa_id=" + fac_id;
                    }
                    //Obteniendo el detalle de la nota de credito
                    else if (tipo == 3)
                    {
                        comm.CommandText = "select rub_nombre, dnc_monto, dnc_montosinimpuesto, dnc_impuesto, '' from tbl_detalle_notacredito, tbl_rubro where rub_id=dnc_rub_id and dnc_tre_id=" + fac_id;
                    }
                    reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new RE_GenericBean();
                        if (!reader.IsDBNull(0)) result.strC1 = reader.GetString(0);//nombre
                        if (!reader.IsDBNull(1)) result.decC1 = reader.GetDecimal(1);//monto
                        if (!reader.IsDBNull(2)) result.decC2 = reader.GetDecimal(2);//monto sin impuesto
                        if (!reader.IsDBNull(3)) result.decC3 = reader.GetDecimal(3);//impuesto
                        if (!reader.IsDBNull(4)) result.strC2 = reader.GetString(4);//comentarios
                        Rubros.Add(result);
                    }
                    DB.CloseObj(reader, comm, conn);
                }
                catch (Exception f)
                {
                    log4net ErrLog = new log4net();
                    ErrLog.ErrorLog("printersettingsPA" + f.Message);
                    WebMsgBox.Show("No se lograron obtener los datos del documento para impresion");
                    return;
                }

                //3. Se genera el archivo para impresion y se coloca en la maquina remota
                if ((Header != null) && (Rubros != null))
                {
                    //3.1 Se genera la Impresion si el correlativo siguiente de la impresora es igual al de la factura
                    if (NuevoNumeroImprimir == Header.intC1)
                    {
                        //Observaciones
                        //Se eliminan dobles espacios
                        Header.strC11.Replace("  ", " ").Replace("  ", " ");
                        string[] ArrObservaciones = Header.strC11.Split(' ');
                        string Observaciones1 = "";
                        string Observaciones2 = "";
                        //solo se puede 2 lineas de 40 caracteres, por lo que se construyen cada linea hasta la informacion posible de 40 caracteres
                        for (int i = 0; i < ArrObservaciones.Length; i++)
                        {
                            if ((Observaciones1.Length + ArrObservaciones[i].Length) < 40)
                            {
                                Observaciones1 += " " + ArrObservaciones[i];
                            }
                            else if ((Observaciones2.Length + ArrObservaciones[i].Length) < 40)
                            {
                                Observaciones2 += " " + ArrObservaciones[i];
                            }
                        }

                        string ComandoTasa = "";
                        sw = new StreamWriter(@"\\" + PathFile + FileName + "-I.txt", true);
                        //Datos Generales del Cliente
                        sw.WriteLine("jS" + Header.strC1); //nombre cliente
                        sw.WriteLine("jR" + Header.strC2); //nit cliente
                        sw.WriteLine("j1CLIENTE: " + Header.strC3); //id cliente
                        sw.WriteLine("j2" + NombreTipoDoc + ": " + Header.strC4 + "-" + Header.intC1.ToString()); //serie-correlativo
                        sw.WriteLine("j3MBL: " + Header.strC5); //mbl
                        sw.WriteLine("j4CONTENEDOR: " + Header.strC6); //contenedor
                        if (Header.strC7.Length > 40)
                        {
                            sw.WriteLine("j5SHIPPER: " + Header.strC7.Substring(1, 40)); //shipper
                        }
                        else
                        {
                            sw.WriteLine("j5SHIPPER: " + Header.strC7); //shipper
                        }
                        sw.WriteLine("j6PESO: " + Header.strC8 + " VOLUMEN: " + Header.strC9); //peso y volumen
                        sw.WriteLine("j7USUARIO: " + Header.strC10); //usuario
                        sw.WriteLine("j8" + Observaciones1); //usuario
                        sw.WriteLine("j9" + Observaciones2); //usuario

                        //Si es NC o ND se debe indicar el numero de Factura al que esta asociado
                        if ((tipo == 3) || (tipo == 4))
                        {
                            sw.WriteLine("jF" + Header.strC12);
                        }

                        //Comando para el detalle (productos o servicios)
                        foreach (RE_GenericBean rub in Rubros)
                        {
                            //el precio no deben llevar puntos ni comas, se toma el subtotal
                            string PrecioRubro = rub.decC2.ToString().Replace(",", "").Replace(".", "");
                            //Se deben agregar 0 hasta completar 10 digitos para el precio del producto
                            int AgregarCeros = 10 - PrecioRubro.Length;
                            for (int i = 0; i < AgregarCeros; i++)
                            {
                                PrecioRubro = "0" + PrecioRubro;
                            }
                            //la impresora solo permite hasta 116 caracteres por descripcion de producto
                            string DescripcionProducto = rub.strC1 + rub.strC2;
                            if (DescripcionProducto.Length > 116)
                            {
                                DescripcionProducto = DescripcionProducto.Substring(1, 116);
                            }
                            if (rub.decC3 == 0) //si el impuesto=0 es exento=tasa0
                            {
                                if (tipo == 1)
                                {
                                    ComandoTasa = " "; //espacio=tasa0(0%), !=tasa1(7%, default para aimar), "=tasa2(10%), #=tasa3(15%)
                                }
                                else
                                {
                                    ComandoTasa = "0"; //0=tasa0(0%), 1=tasa1(7%, default para aimar), 2=tasa2(10%), 3=tasa3(15%)
                                }
                            }
                            else //si el impuesto<>0 no es exento para Aimar aplica tasa 7%=tasa1
                            {
                                if (tipo == 1)
                                {
                                    ComandoTasa = "!"; //espacio=tasa0(0%), !=tasa1(7%, default para aimar), "=tasa2(10%), #=tasa3(15%)
                                }
                                else
                                {
                                    ComandoTasa = "1"; //0=tasa0(0%), 1=tasa1(7%, default para aimar), 2=tasa2(10%), 3=tasa3(15%)
                                }
                            }
                            sw.WriteLine(ComandoServicio + ComandoTasa + PrecioRubro + "00001000" + DescripcionProducto);
                        }
                        //Comando para imprimir hasta que esta cargada toda la informacion
                        sw.WriteLine("3401");
                        sw.WriteLine("101");
                        sw.Flush();
                        sw.Close();

                        //Si devuelve 0 la impresion fue exitosa, sino se muestra mensaje acorde a resultado
                        int PrintResult = GetResultImpresion(@"\\" + PathFile + "Results\\" + FileName + "-R.txt");
                        if (PrintResult < 0)
                        {
                            WebMsgBox.Show(ShowTallyPrintMsgs(PrintResult));
                            return;
                        }
                        else if (PrintResult == 0) {
                            btn_print.Enabled = false; //impresion exitosa desactiva el boton
                        }
                    }
                    //3.2 Si el correlativo de la impresora es mayor al de la base de datos, lo toma como reimpresion
                    else if (NuevoNumeroImprimir > Header.intC1)
                    {
                        if (Process == 1) //es 1 cuando escogen la opcion Re-imprimir
                        {
                            sw = new StreamWriter(@"\\" + PathFile + FileName + "-RI.txt", true);
                            //Comando de Nombre y Nit del Cliente
                            string NumeroReimpresion = Header.intC1.ToString();
                            int AgregarCeros = 7 - NumeroReimpresion.Length;
                            for (int i = 0; i < AgregarCeros; i++)
                            {
                                NumeroReimpresion = "0" + NumeroReimpresion;
                            }
                            //Reimpresion de Factura
                            if (tipo == 1)
                            {
                                sw.WriteLine("RF" + NumeroReimpresion + NumeroReimpresion);
                            }
                            //Reimpresion de ND
                            else if (tipo == 4)
                            {
                                sw.WriteLine("RD" + NumeroReimpresion + NumeroReimpresion);
                            }
                            //Reimpresion de NC
                            else if ((tipo == 3) || (tipo == 12))
                            {
                                sw.WriteLine("RC" + NumeroReimpresion + NumeroReimpresion);
                            }
                            sw.Flush();
                            sw.Close();

                            //Si devuelve 0 la impresion fue exitosa, sino se muestra mensaje acorde a resultado
                            int PrintResult = GetResultImpresion(@"\\" + PathFile + "Results\\" + FileName + "-RR.txt");
                            if (PrintResult < 0)
                            {
                                WebMsgBox.Show(ShowTallyPrintMsgs(PrintResult));
                                return;
                            }
                            else if (PrintResult == 0)
                            {
                                btn_reprint.Enabled = false; //impresion exitosa desactiva el boton de reimpresion
                            }
                        }
                        else
                        {
                            WebMsgBox.Show("El correlativo que desea imprimir es menor al que sigue en la impresora, primero debe revisar si ya fue impreso, si aun lo desea imprimir debe usar la opcion 'Re-Imprimir'");
                            btn_print.Enabled = false; //se desactiva el boton imprimir
                            btn_reprint.Visible = true; //se activa el boton re-imprimir
                            return;
                        }
                    }
                    //3.3 No imprime si el correlativo del sistema es mayor en 2 o mas
                    else
                    {
                        WebMsgBox.Show("El correlativo del documento= " + Header.intC1 + " es mayor al que continua en la impresora=" + NuevoNumeroImprimir);
                        return;
                    }
                }
            }
            else
            {
                WebMsgBox.Show("No hay acceso a la impresora" + PathFile);
                return;
            }
            System.IO.File.Delete(@"\\" + PathFile + S1File + "-R.txt");
        }
        finally
            {
            userImpersonation.UndoImpersonation();
        }
    }

    private void PopulateInstalledPrintersCombo()
    {
        RE_GenericBean result = null;
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        NpgsqlDataReader reader;
        ArrayList Arr = new ArrayList();
        ListItem item = null;

        //Obteniendo la IP que solicita la impresion
        string RemoteAddr = Request.ServerVariables["REMOTE_ADDR"].ToString(); //check primero si != null, otras opciones REMOTE_HOST, HTTP_REFERER

        //Obteniendo el listado de impresoras activas para la IP que solicita la impresion
        try
        {
            conn = DB.OpenConnection();
            comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select timp_nombre, timp_ip, timp_ruta, timp_puerto from tbl_impresora where timp_ip='" + RemoteAddr + "' and timp_pai_id=" + user.PaisID;
            reader = comm.ExecuteReader();
            while (reader.Read())
            {
                result = new RE_GenericBean();
                if (!reader.IsDBNull(0)) result.strC1 = reader.GetString(0);//nombre
                if (!reader.IsDBNull(1)) result.strC2 = reader.GetString(1);//ip
                if (!reader.IsDBNull(2)) result.strC3 = reader.GetString(2);//ruta
                if (!reader.IsDBNull(3)) result.strC4 = reader.GetString(3);//puerto
                Arr.Add(result);
            }
            DB.CloseObj(reader, comm, conn);
        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog("printersettingsPA" + e.Message);
            WebMsgBox.Show("No se encontraron impresoras para su IP, favor contactar a informatica");
            return;
        }
        //Actualizando el ComboBox con las impresoras activas para la IP
        if (Arr != null)
        {
            foreach (RE_GenericBean rgb in Arr)
            {
                item = new ListItem(rgb.strC1, rgb.strC2 + "," + rgb.strC3 + "," + rgb.strC4);
                lb_impresora.Items.Add(item);
            }
        }
        else
        {
            WebMsgBox.Show("No hay impresoras asignadas a esta computadora");
            return;
        }
    }
    private int GetResultImpresion(string PathPrint)
    {
        //Esperando el archivo de respuesta de la computadora remota (maximo 1 minuto)
        int LimiteTiempoImpresion = int.Parse(DateTime.Now.ToString("HHmm"));
        int IniciaTiempoImpresion = LimiteTiempoImpresion;
        LimiteTiempoImpresion += 3; //en minutos
        int ResultImpresion = -1;
        bool GetResultado = false;
        while ((IniciaTiempoImpresion < LimiteTiempoImpresion) && (GetResultado == false))
        {
            IniciaTiempoImpresion = int.Parse(DateTime.Now.ToString("HHmm"));
            //Se verifica si ya existe el archivo con el resultado
            if (System.IO.File.Exists(PathPrint))
            {
                try
                {
                    StreamReader sr = new StreamReader(PathPrint);
                    ResultImpresion = int.Parse(sr.ReadLine());
                    sr.Dispose();
                    sr.Close();
                    GetResultado = true;
                }
                catch
                {
                    //Se agrego este try, porque al querer abrir el archivo no pueda hacerlo por permisos
                    //porque todavia esta siendo creado por el Servicio de Impresion
                }
            }
        }
        return ResultImpresion;
    }

    private string ShowTallyPrintMsgs(int PrintResult)
    {
        string PrintMsg = "";
        if (PrintResult == -1)
        {
            PrintMsg = "Se excedio el tiempo de espera para obtener correlativo de la impresora";
        }
        else if (PrintResult == -2)
        {
            PrintMsg = "No se logra comunicacion con puerto de impresora";
        }
        else if (PrintResult == -3)
        {
            PrintMsg = "No se logra chequear la impresora";
        }
        else if (PrintResult == -4)
        {
            PrintMsg = "No se logra obtener estado de la impresora, puede estar apagada";
        }
        else if (PrintResult == -5)
        {
            PrintMsg = "No logro obtener secuencia de impresion";
        }
        else if (PrintResult == -6)
        {
            PrintMsg = "Se excedio el tiempo de espera para realizar la impresion";
        }
        else if (PrintResult == -7)
        {
            PrintMsg = "No se logra obtener estado de la impresion";
        }
        else if (PrintResult == -8)
        {
            PrintMsg = "Se realizo la impresion, pero la memoria fiscal esta casi llena";
        }
        else if (PrintResult == -9)
        {
            PrintMsg = "Se realizo la impresion, pero la memoria fiscal ya esta llena";
        }
        else if (PrintResult == -10)
        {
            PrintMsg = "No se encuentra carpeta de archivos de impresion";
        }
        else if (PrintResult == -11)
        {
            PrintMsg = "No se puede abrir el archivo de Status para impresion";
        }
        else if (PrintResult < -100) //Mensajes propios de la impresora
        {
            //El codigo se vuelve positivo para poderlo separar y traducir a los mensajes indicados en los manuales de la impresora
            PrintResult = PrintResult * -1;
            int PrintStatus = int.Parse(PrintResult.ToString().Substring(0, 3)) - 100;
            int PrintError = int.Parse(PrintResult.ToString().Substring(3, 3));

            //Mensajes de Status
            if (PrintStatus == 0)
            {
                PrintMsg = "La impresora se encuentra lista";
            }
            else if (PrintStatus == 1)
            {
                PrintMsg = "La impresora se encuentra en modo de prueba y espera";
            }
            else if (PrintStatus == 2)
            {
                PrintMsg = "La impresora se encuentra en modo de prueba y emision de documentos fiscales";
            }
            else if (PrintStatus == 3)
            {
                PrintMsg = "La impresora se encuentra en modo de prueba y emision de documentos no fiscales";
            }
            else if (PrintStatus == 4)
            {
                PrintMsg = "La impresora se encuentra en modo fiscal y en espera";
            }
            else if (PrintStatus == 5)
            {
                PrintMsg = "La impresora se encuentra en modo fiscal y emision de documentos fiscales";
            }
            else if (PrintStatus == 6)
            {
                PrintMsg = "La impresora se encuentra en modo fiscal y emision de documentos no fiscales";
            }
            else if (PrintStatus == 7)
            {
                PrintMsg = "La impresora se encuentra en modo fiscal y en espera, con la memoria fiscal ya casi esta llena";
            }
            else if (PrintStatus == 8)
            {
                PrintMsg = "La impresora se encuentra en modo fiscal y emision de documentos fiscales con la memoria fiscal ya casi esta llena";
            }
            else if (PrintStatus == 9)
            {
                PrintMsg = "La impresora se encuentra en modo fiscal y emision de documentos no fiscales con la memoria fiscal ya casi esta llena";
            }
            else if (PrintStatus == 10)
            {
                PrintMsg = "La impresora se encuentra en modo fiscal y en espera, con la memoria fiscal llena";
            }
            else if (PrintStatus == 11)
            {
                PrintMsg = "La impresora se encuentra en modo fiscal y emision de documentos fiscales con la memoria fiscal llena";
            }
            else if (PrintStatus == 12)
            {
                PrintMsg = "La impresora se encuentra en modo fiscal y emision de documentos no fiscales con la memoria fiscal llena";
            }
            else
            {
                PrintMsg = "Impresora devuelve mensaje: " + PrintStatus;
            }

            //Mensajes de Error
            if (PrintError == 1)
            {
                PrintMsg += " / Problema en la entrega de papel";
            }
            else if (PrintError == 2)
            {
                PrintMsg += " / Problema de indole mecanico en la entrega de papel";
            }
            else if (PrintError == 2)
            {
                PrintMsg += " / Problema en la entrega de papel con error de indole mecanico";
            }
            else if (PrintError == 80)
            {
                PrintMsg += " / Comando o valor invalido";
            }
            else if (PrintError == 84)
            {
                PrintMsg += " / Tasa invalida";
            }
            else if (PrintError == 88)
            {
                PrintMsg += " / No hay asignadas directivas";
            }
            else if (PrintError == 92)
            {
                PrintMsg += " / Comando o valor invalido";
            }
            else if (PrintError == 96)
            {
                PrintMsg += " / Error fiscal";
            }
            else if (PrintError == 100)
            {
                PrintMsg += " / Problema de la memoria fiscal";
            }
            else if (PrintError == 108)
            {
                PrintMsg += " / Memoria fiscal llena";
            }
            else if (PrintError == 112)
            {
                PrintMsg += " / Buffer completo, debe enviar el comando de reinicio";
            }
            else if (PrintError == 128)
            {
                PrintMsg += " / Problema en la comunicacion";
            }
            else if (PrintError == 137)
            {
                PrintMsg += "/ No hay respuesta";
            }
            else if (PrintError == 144)
            {
                PrintMsg += " / Problema LRC";
            }
            else if (PrintError == 145)
            {
                PrintMsg += " / Problema interno API de impresora";
            }
            else if (PrintError == 153)
            {
                PrintMsg += "/ Problema en la apertura de archivo de impresion";
            }
            else
            {
                PrintMsg += "/ Problema: " + PrintError;
            }
        }
        else
        {
            PrintMsg = "Impresora devuelve codigo: " + PrintResult;
        }
        //return PrintMsg + " " + PrintResult.ToString();
        return PrintMsg;
    }
}
