using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;

/// <summary>
/// Summary description for BAW_Intercompany_Operativo
/// </summary>
[WebService(Namespace = "http://10.10.1.7:8181/WebServices/BAW_Intercompany_Operativo.asmx/", Description = "Web Service para Contabilizar Automaticamente el Intercompanys Operativo")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class BAW_Intercompany_Operativo : System.Web.Services.WebService {

    public BAW_Intercompany_Operativo () 
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 5
    }
    [WebMethod]
    public ArrayList Contabilizar_Automaticamente(int _empresaORIGENID, int _intercompanyDESTINOID, int _sisID, int _ttoID, int _blID, string _usuID)
    {
        ArrayList resultado = new ArrayList();
        resultado = Contabilizacion_Automatica_CN.Generar_Contabilizacion_Automatica_Intercompany_Operativo(_empresaORIGENID, _intercompanyDESTINOID, _sisID, _ttoID, _blID, _usuID);
        return resultado;
    }
}
