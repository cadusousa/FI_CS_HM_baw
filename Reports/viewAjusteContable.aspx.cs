using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Reports_viewpolizadiario : System.Web.UI.Page
{
    UsuarioBean user = null;
    string Tipo_Ajuste = "";
    string Entidad = "";
    string Nombre = "";
    int Codigo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        int docID = int.Parse(Request.QueryString["docID"].ToString());
        string fechaajustar = "", userID = "", serie = "", correlativo = "", moneda = "", pais = "", userElabora = "";
        string banco = "", cuenta = "";
        string trafico = "";
        RE_GenericBean datos = (RE_GenericBean)DB.getDataAjusteContable(docID);
        Tipo_Ajuste = datos.strC6;
        fechaajustar = datos.strC3;
        userID = datos.strC5;
        serie = datos.strC1;
        correlativo = datos.intC2.ToString();
        moneda = Utility.TraducirMonedaInt(datos.intC3);
        pais = user.pais.Nombre;
        userElabora = user.ID;
        if ((datos.intC9 >= 2) && (datos.intC9 <= 6))// Ajuste x Entidades
        {
            if (datos.intC8 == 3)//Clientes
            {
                RE_GenericBean Cliente_Bean = DB.getDataClient(Convert.ToDouble(datos.intC7));
                Nombre= Cliente_Bean.strC2;
                Codigo = Cliente_Bean.intC3;
                Entidad = "Cliente     " + Codigo + "      -     " + Nombre;
            }
            if (datos.intC8 == 4)//proveedor
            {
                RE_GenericBean Proveedor_Bean = DB.getProveedorData(datos.intC7, "");
                Nombre = Proveedor_Bean.strC2;
                Codigo = Proveedor_Bean.intC1;
                Entidad = "Proveedor     " + Codigo + "      -     " + Nombre;
            }
            else if (datos.intC8 == 2)//agente
            {
                RE_GenericBean Agente_Bean = DB.getAgenteData(datos.intC7, "");
                Nombre = Agente_Bean.strC1;
                Codigo = Agente_Bean.intC1;
                Entidad = "Agente     " + Codigo + "      -     " + Nombre;
            }
            else if (datos.intC8 == 5)//naviera
            {
                RE_GenericBean Naviera_Bean = DB.getNavieraData(datos.intC7);
                Nombre = Naviera_Bean.strC1;
                Codigo = Naviera_Bean.intC1;
                Entidad = "Naviera     " + Codigo + "      -     " + Nombre;
            }
            else if (datos.intC8 == 6)//linea aerea
            {
                RE_GenericBean Carrier_Bean = DB.getCarriersData(datos.intC7);
                Nombre = Carrier_Bean.strC1;
                Codigo = Carrier_Bean.intC1;
                Entidad = "Linea Aerea     " + Codigo + "      -     " + Nombre;
            }
        }
        if (datos.intC9 == 7)//Caja Chica
        {
            ArrayList Arr = (ArrayList)DB.getProveedor(" numero=" + datos.intC7 + " ", "");//Caja Chica
            foreach (RE_GenericBean Caja_Chica_Bean in Arr)
            {
                Nombre = Caja_Chica_Bean.strC2;
                Codigo = Caja_Chica_Bean.intC1;
                Entidad = "Caja Chica     " + Codigo + "      -     " + Nombre;
            }
        }
        if (datos.intC9 == 8)// Ajuste a Bancos
        {
            RE_GenericBean bancos = (RE_GenericBean)DB.getBanco(datos.intC6);
            banco = bancos.strC1;
            cuenta = datos.strC7;
            Entidad = "Banco .: "+banco + "     Cuenta.: " + cuenta;
        }
        if (datos.intC9==10)//Planilla
        {
            Entidad = "Numero de Poliza.:  " + datos.strC12;
        }
        if ((datos.intC9 >= 2) && (datos.intC9 <= 7))
        {
            if (datos.strC8!="")
            {
                trafico = "HBL=" + datos.strC8;
            }
            if (datos.strC9 != "")
            {
                trafico += "     MBL=" + datos.strC9;
            }
            if (datos.strC10 != "")
            {
                trafico += "     Routing=" + datos.strC10;
            }
            if (datos.strC11 != "")
            {
                trafico += "     Contenedor=" + datos.strC11;
            }
        }
        LibroDiarioDS ds = null;
        ds = DB.generaReporteAjusteContable(docID, Tipo_Ajuste, user.PaisID);

        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_AjusteContable.rpt"));
        rpt.SetDataSource(ds.Tables["polizadiario"]);
        //rpt.SetParameterValue("Tipo_Ajuste", Tipo_Ajuste);
        rpt.SetParameterValue("Trafico", trafico);
        rpt.SetParameterValue("Entidad", Entidad);
        rpt.SetParameterValue("fechaajustar", fechaajustar);
        rpt.SetParameterValue("userID", userID);
        rpt.SetParameterValue("serie", serie);
        rpt.SetParameterValue("correlativo", correlativo);
        rpt.SetParameterValue("moneda", moneda);
        rpt.SetParameterValue("pais", pais);
        rpt.SetParameterValue("userElabora", userID);
        rpt.SetParameterValue("Observaciones", datos.strC4);
        if (datos.intC4 == 1)
        {
            rpt.SetParameterValue("contabilidad", "FISCAL");
        }
        else if (datos.intC4 == 2)
        {
            rpt.SetParameterValue("contabilidad", "FINANCIERA");
        }
        else if (datos.intC4 == 3)
        {
            rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
        }
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();

    }
}
