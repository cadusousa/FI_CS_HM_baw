using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for BAW_Intercompany_Notification_Background
/// </summary>
[WebService(Namespace = "http://10.10.1.7:8181/WebServices/BAW_Intercompany_Notification_Background.asmx/", Description = "Web Service que reenvia las Notificaciones Automaticas de Cobro Intercompany pendientes de enviar")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class BAW_Intercompany_Notification_Background : System.Web.Services.WebService {

    public BAW_Intercompany_Notification_Background () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public bool Generate_Notification_Forwarding() 
    {
        bool resultado = true;
        resultado = Contabilizacion_Automatica_CN.Generate_Intercompany_Notification_Forwarding();
        return resultado;
    }
    
}
