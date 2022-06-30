using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;

/// <summary>
/// Summary description for BAW_Contabilizacion_Automatica_V_1
/// </summary>
//[WebService(Namespace = "http://localhost:49201/BAWDENNIS/WebServices/BAW_Contabilizacion_Automatica_V_1.asmx/",Description="WebService para Generar Contabilizacion Automatica, Sistemas Aereo, Terrestre y Maritimo.")]
[WebService(Namespace = "http://10.10.1.7:8181/WebServices/BAW_Contabilizacion_Automatica_V_1.asmx/", Description = "WebService para Generar Contabilizacion Automatica, Sistemas Aereo, Terrestre y Maritimo.")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class BAW_Contabilizacion_Automatica_V_1 : System.Web.Services.WebService 
{
    public BAW_Contabilizacion_Automatica_V_1 () 
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public ArrayList Contabilizacion_Automatica(bool _MBL, bool _HBL, bool _Routing, bool _Imp_Exp, int _paisID, int _sisID, int _ttoID, int _blID, string _usuID, string _Requestor, string _Guid)
    {
        ArrayList resultado = new ArrayList();
        int alertaID = 0;
        string mensaje = "";
        string Autenticacion = Autenticar_Usuario(_Requestor, _Guid);
        if (Autenticacion != "1111")
        {
            alertaID = 9;
            mensaje = "Autenticacion Invalida " + Autenticacion;
            resultado.Add(alertaID);
            resultado.Add(mensaje);
            return resultado;
        }
        else if (Autenticacion == "1111")
        {
            if (Contabilizacion_Automatica_CN.Existe_Pais(_paisID.ToString()) == false)
            {
                alertaID = 4;
                mensaje = "Empresa no configurada";
                resultado.Add(alertaID);
                resultado.Add(mensaje);
                return resultado;
            }
            if (Contabilizacion_Automatica_CN.Existe_Tipo_Cambio(_paisID.ToString()) == false)
            {
                alertaID = 3;
                mensaje = "No Existe Tipo de Cambio";
                resultado.Add(alertaID);
                resultado.Add(mensaje);
                return resultado;
            }
            resultado = Contabilizacion_Automatica_CN.Generar_Contabilizacion_Automatica(_MBL, _HBL, _Routing, _Imp_Exp, _paisID, _sisID, _ttoID, _blID, _usuID);
            if (resultado != null)
            {
                alertaID = 1;
                mensaje = "";
                resultado.Add(alertaID);
                resultado.Add(mensaje);
                return resultado;
            }
            else
            {
                alertaID = 10;
                mensaje = "";
                resultado.Add(alertaID);
                resultado.Add(mensaje);
                return resultado;
            }
        }   
        return resultado;
    }
    private string Autenticar_Usuario(string _Requestor, string _Guid)
    {
        #region Autenticar Usuario
        string resultado = "9";
        char[] delimiterChar = { '-' };
        string[] Arreglo = _Requestor.Split(delimiterChar);
        if (Arreglo.Length == 3)
        {

            string parametro_1 = Arreglo[0];
            string parametro_2 = Arreglo[1];
            string parametro_3 = Arreglo[2];
            RE_GenericBean Sistema_Bean = (RE_GenericBean)DB.getSistema_By_ID(parametro_2);
            if (Sistema_Bean != null)
            {
                resultado = "";
                if (parametro_1 == Sistema_Bean.strC2)
                {
                    resultado += "1";
                }
                else
                {
                    resultado += "9";
                }
                if (parametro_2 == Sistema_Bean.intC1.ToString())
                {
                    resultado += "1";
                }
                else
                {
                    resultado += "9";
                }
                ArrayList Arr_Sistemas = DB.Get_Detalle_Sistemas(" and b.tto_tsis_id=" + parametro_2 + " and b.tto_id=" + parametro_3 + "");
                if (Arr_Sistemas != null)
                {
                    if (Arr_Sistemas.Count == 1)
                    {
                        resultado += "1";
                    }
                    else
                    {
                        resultado += "9";
                    }
                }
                else
                {
                    resultado += "9";
                }
                string cadena = DB.Invertir_Cadena(_Guid);
                if (cadena == Sistema_Bean.strC3)
                {
                    resultado += "1";
                }
                else
                {
                    resultado += "9";
                }
            }
        }
        return resultado;
        #endregion
    }
}
