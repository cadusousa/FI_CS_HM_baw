using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Collections;

/// <summary>
/// Summary description for BAW_Notificaciones_Clientes
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//[WebService(Namespace = "http://10.10.1.7:8181/WebServices/", Description = "Web Service para enviar estado de resultados y balance general regional por correo.")]
[WebService(Namespace = "http://10.10.1.7:8181/WebServices/BAW_Notificaciones_Clientes.asmx", Description = "Web Service para enviar reporte de clientes con saldos vencidos.")]
//[WebService(Namespace = "http://localhost:53939/BAWDREIBY/WebServices/BAW_Notificaciones_Clientes.asmx", Description = "Web Service para enviar reporte de clientes con saldos vencidos.")]

public class BAW_Notificaciones_Clientes : System.Web.Services.WebService {

    public BAW_Notificaciones_Clientes () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string Identifaca_clientes_credito() {

        int band = 0;
        int pais = 0;
        try
        {
           // int[] paises = {1, 2, 3, 4, 5, 6, 7, 9, 11, 12, 13, 15, 18, 21, 22, 23, 24, 25, 26 };
            int[] paises = { 1 };
            for (int i = 0; i < paises.Length; i++)  // vaciar carpeta.
            {
                pais = paises[i];
                ArrayList Arr_locales = new ArrayList();
                ArrayList Arr_dolares = new ArrayList();

                Arr_locales = DB.Get_clientes_con_creditos(pais, 1, 1, 1);
                Arr_dolares = DB.Get_clientes_con_creditos(pais, 1, 8, 2);

                //****************MONEDA LOCAL
                if (Arr_locales != null)
                {
                    foreach (RE_GenericBean clientes in Arr_locales)
                    {
                        band = DB.Estado_cuenta_automatico(clientes, 1, 1, 3, pais, Server);
                    }
                }
                //****************MONEDA DOLAR
                if (Arr_dolares != null)
                {
                    foreach (RE_GenericBean clientes_D in Arr_dolares)
                    {
                        band = DB.Estado_cuenta_automatico(clientes_D, 8, 2, 3, pais, Server);
                    }
                }
            }
        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
            return "error!";
        }
        return "enviado";
    }
}
