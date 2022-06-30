using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Net.Mime;

public partial class manager_Notificacion_Automatica_Cobro : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        #region Enviar Reporte por Correo
        System.Net.Mail.MailMessage Email = new System.Net.Mail.MailMessage();
        string Server = "mail.aimargroup.com";
        string body = "";
        Email.From = new System.Net.Mail.MailAddress("soporte2@aimargroup.com");
        Email.To.Add("soporte2@aimargroup.com");
        Email.Subject = "Notificacion Automatica de Cobro Intercompany";
        body = Generar_Formato(1, 1001, 1);
        AlternateView viewprint = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);
        Email.AlternateViews.Add(viewprint);
        SmtpClient Cliente_Smtp = new SmtpClient(Server);
        Cliente_Smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
        try
        {
            Cliente_Smtp.Send(Email);
        }
        catch (Exception ex)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(ex.Message);
        }
        #endregion
    }
    protected static string Generar_Formato(int ttrID, int refID, int Tipo_Intercompany)
    {

        string Formato_Cobro = "";
        Formato_Cobro = "<html>" +
        "<body>" +
        "<table align='center' cellpadding='0' cellspacing='0'>" +
        "<tr>" +
            "<td align='center'>" +
                "<img alt=Logo' src='http://www.aimargroup.com/img/aimar.jpg' height='100' width='300' />" +
                "<br>" +
                "<br>" +
            "</td>" +
        "</tr>" +
        "<tr>" +
            "<td>" +
                "<table cellpadding='0' cellspacing='0' " +
                    "style='font-family:Verdana;font-size:12pt;' width='600px'>" +
                    "<tr>" +
                        "<td>" +
                            "Estimado Intercompany, <b>NOMBRE DEL INTERCOMPANY DESTINO (Nit. 8230749)</b></td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td style='border-bottom:1px dotted #2525E2;padding-bottom:20px;'>" +
                            "<br />" +
                            "<b>INTERCOMPANY ORIGEN (Nit. 12105562) </b>Ha emitido para usted un " +
                            "documento de cobro. A continuación podrá encontrar mas " +
                            "información.</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>" +
        "<tr>" +
            "<td>" +
                "<table cellpadding='0' cellspacing='0' " +
                    "style='font-family:Verdana;font-size:12pt;' width='600px'>" +
                    "<tr>" +
                        "<td>" +
                            "<b>Información del Cobro en la Contabilidad de:</b></td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td style='border-bottom:1px dotted #2525E2;padding-bottom:20px;'>" +
                            "<br />" +
                            "<ul>" +
                                "<li>Documento:</li>" +
                                "<li>Fecha:</li>" +
                                "<li>Serie y número:</li>" +
                                "<li>Monto:</li>" +
                                "<li>Master:</li>" +
                                "<li>House:</li>" +
                                "<li>Observaciones:</li>" +
                            "</ul>" +
                        "</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>" +
        "<tr>" +
            "<td>" +
                "<table cellpadding='0' cellspacing='0' " +
                    "style='font-family:Verdana;font-size:12pt;' width='650px'>" +
                    "<tr>" +
                        "<td>" +
                            "<b>Información del Pago en la Contabilidad de:</b></td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td style='border-bottom:1px dotted #2525E2;padding-bottom:20px;'>" +
                            "<br />" +
                            "<ul>" +
                                "<li>Documento:</li>" +
                                "<li>Intercompany:</li>" +
                                "<li>Fecha:</li>" +
                                "<li>Contabilidad:</li>" +
                                "<li>Serie y número:</li>" +
                                "<li>Monto:</li>" +
                                "<li>Master:</li>" +
                                "<li>House:</li>" +
                                "<li>Observaciones:</li>" +
                            "</ul>" +
                        "</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>" +
        "<tr>" +
            "<td>" +
                "<table cellpadding='0' cellspacing='0' " +
                    "style='font-family:Verdana;font-size:12pt;' width='600px'>" +
                    "<tr>" +
                        "<td>" +
                            "<b>Información del Cobro al Cliente por Cuenta de Terceros en la Contabilidad de:</b></td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td style='border-bottom:1px dotted #2525E2;padding-bottom:20px;'>" +
                            "<br />" +
                            "<ul>" +
                                "<li>Documento:</li>" +
                                "<li>Cliente:</li>" +
                                "<li>Fecha:</li>" +
                                "<li>Contabilidad:</li>" +
                                "<li>Serie y número:</li>" +
                                "<li>Monto:</li>" +
                                "<li>Master:</li>" +
                                "<li>House:</li>" +
                                "<li>Observaciones:</li>" +
                            "</ul>" +
                        "</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>" +
    "</table>" +
    "</body>" +
    "</html>";
        return Formato_Cobro;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        bool resultado = false;
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(1, 286612, 2);
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(1, 280816, 2);
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(4, 106881, 1);
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(1, 285997, 1);
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(4, 106797, 1);
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(1, 285994, 1);
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(1, 286188, 2);
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(4, 106660, 1);
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(4, 109326, 1);
        //resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(4, 109445, 1);
        resultado = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(1, 298762, 1);
    }
}
