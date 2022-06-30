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

public partial class Reports_estadoctaagente : System.Web.UI.Page
{
    UsuarioBean user = null;
    LibroDiarioDS ds = new LibroDiarioDS();
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    RE_GenericBean SOA_Bean = new RE_GenericBean();
    string nombre = "";
    string Datos_Pago = "";
    int ted_id = 0;
    int ttt = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        user = (UsuarioBean)Session["usuario"];
            int id = int.Parse(Request.QueryString["id"].ToString());
            ted_id = int.Parse(Request.QueryString["ted"].ToString());
            SOA_Bean = (RE_GenericBean)DB.getCortebyID(id, ted_id);
            #region Cargar Nombre
            if (SOA_Bean.intC5 == 4)//Proveedor y Caja Chica
            {
                RE_GenericBean Proveedor_Bean = (RE_GenericBean)DB.getProveedorData(SOA_Bean.intC4, "");
                if (Proveedor_Bean != null)
                {
                    nombre = Proveedor_Bean.strC2;
                }
            }
            else if (SOA_Bean.intC5==2)//agente
            {
                RE_GenericBean Agente_Bean = (RE_GenericBean)DB.getAgenteData(SOA_Bean.intC4, "");
                if (Agente_Bean != null)
                {
                    nombre = Agente_Bean.strC1;
                }
            }
            else if (SOA_Bean.intC5 == 5)//naviera
            {
                RE_GenericBean Naviera_Bean = (RE_GenericBean)DB.getNavieraData(SOA_Bean.intC4);
                if (Naviera_Bean != null)
                {
                    nombre = Naviera_Bean.strC1;
                }
            }
            else if (SOA_Bean.intC5 == 6)//linea aerea
            {
                RE_GenericBean Carrier_Bean = (RE_GenericBean)DB.getCarriersData(SOA_Bean.intC4);
                if (Carrier_Bean != null)
                {
                    nombre = Carrier_Bean.strC1;
                }
            }
            else if (SOA_Bean.intC5 == 8)//Caja Chica
            {
                RE_GenericBean CajaChica_Bean = (RE_GenericBean)DB.getUsuarioEmpresabyID(SOA_Bean.intC4);
                nombre = CajaChica_Bean.strC1;
            }
            else if (SOA_Bean.intC5 == 10)//Intercompany
            {
                RE_GenericBean Intercompany_Bean = (RE_GenericBean)DB.getIntercompanyData(SOA_Bean.intC4);
                if (Intercompany_Bean != null)
                {
                    nombre = Intercompany_Bean.strC1;
                }
            }
            #endregion
            ArrayList transacciones = (ArrayList)DB.getTransactions(id);//2 porque es tipopersona=agente segun tbl_tipo_persona
            ArrayList RecibosCorte = (ArrayList)DB.getReciboCorte(id);
            foreach (RE_GenericBean rgb1 in RecibosCorte)
            {
                object[] objArr = { rgb1.strC1, "", "RC - " + rgb1.strC2 + "-" + rgb1.strC3, "", 0, rgb1.decC1 };
                ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);
            }


            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "STATEMENT OF ACCOUNT", "FO-CB-12"); //pais, sistema, doc_id, titulo, edicion
            int i = 0;
            //transacciones
            RE_GenericBean operacion = null;
            double Tipo_CambioDOCUMENTO = 0;
            foreach (RE_GenericBean rgb in transacciones)
            {
                if ((rgb.intC4 == 3) || (rgb.intC4 == 12) || (rgb.intC4 == 31))
                { //NC Clientes = 3
                    //NC Proveedores = 12
                    ArrayList temp = (ArrayList)DB.getNotasCreditoData(rgb.intC1);//obtengo a partir de la referencia
                    operacion = (RE_GenericBean)temp[0];
                    RE_GenericBean viaje = (RE_GenericBean)DB.getViaje(operacion.strC16, user.pais.schema);//obtengo los datos del viaje a partir del contenedor
                    operacion.strC17 = viaje.strC1;//vapor (barco)
                    operacion.strC18 = viaje.strC2;//No de viaje
                    Tipo_CambioDOCUMENTO = Convert.ToDouble(DB.getTipoCambioNotaCredito(rgb.intC1));
                    if (operacion.intC4 == 31)
                    {
                        //object[] objArr = { operacion.strC2, operacion.strC20, "NC - " + operacion.strC1 + "-" + operacion.intC2, operacion.strC17 + " " + operacion.strC18, 0, operacion.decC1, Tipo_CambioDOCUMENTO.ToString() };
		            	i++;
                        object[] objArr = { operacion.strC2, operacion.strC20, "NC - " + operacion.strC1 + "-" + operacion.intC2, operacion.strC17 + " " + operacion.strC18, 0, operacion.decC1, operacion.strC7, operacion.strC6, operacion.strC9, Tipo_CambioDOCUMENTO.ToString(), i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);
                    }
                    else
                    {
                        //object[] objArr = { operacion.strC2, operacion.strC20, "NC - " + operacion.strC1 + "-" + operacion.intC2, operacion.strC17 + " " + operacion.strC18, operacion.decC1, 0, Tipo_CambioDOCUMENTO.ToString() };
                        i++;
                        object[] objArr = { operacion.strC2, operacion.strC20, "NC - " + operacion.strC1 + "-" + operacion.intC2, operacion.strC17 + " " + operacion.strC18, operacion.decC1, 0, operacion.strC7, operacion.strC6, operacion.strC9, Tipo_CambioDOCUMENTO.ToString(), i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);
                    }
                }
                if (rgb.intC4 == 4)
                { //ND
                    operacion = (RE_GenericBean)DB.getNotaDebitoData(rgb.intC1);
                    RE_GenericBean viaje = (RE_GenericBean)DB.getViaje(operacion.strC9, user.pais.schema);//obtengo los datos del viaje a partir del contenedor
                    operacion.strC17 = viaje.strC1;//vapor (barco)
                    operacion.strC18 = viaje.strC2;//No de viaje
                    Tipo_CambioDOCUMENTO = Convert.ToDouble(DB.getTipoCambioNotaDebito(rgb.intC1));
                    i++;
                    object[] objArr = { operacion.strC3, operacion.strC11, "ND - " + operacion.strC28 + "-" + operacion.intC6, operacion.strC17 + " " + operacion.strC18, operacion.decC1, 0, operacion.strC8, operacion.strC7, operacion.strC9, Tipo_CambioDOCUMENTO.ToString(), i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);
                }
                if ((rgb.intC4 == 5) || (rgb.intC4 == 10))
                { //provision a partir de caja=5
                    //Provision de Agente = 10
                    string where = " and b.tpr_prov_id=" + rgb.intC1;
                    ArrayList temp = (ArrayList)DB.getProvision(where);//obtengo a partir de la referencia
                    operacion = (RE_GenericBean)temp[0];
                    RE_GenericBean viaje = (RE_GenericBean)DB.getViaje(operacion.strC16, user.pais.schema);//obtengo los datos del viaje a partir del contenedor
                    operacion.strC17 = viaje.strC1;//vapor (barco)
                    operacion.strC18 = viaje.strC2;//No de viaje
                    Tipo_CambioDOCUMENTO = Convert.ToDouble(DB.getTipoCambioProv(rgb.intC1));
                    i++;
                    object[] objArr = { operacion.strC1, operacion.strC20, "PRV - " + operacion.strC21 + "-" + operacion.intC3, operacion.strC17 + " " + operacion.strC18, 0, operacion.decC1, operacion.strC13, operacion.strC14, operacion.strC16, Tipo_CambioDOCUMENTO.ToString(), i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);

                    //Retenciones
                    where = " b.trp_tpr_prov_id=" + rgb.intC1;
                    temp = (ArrayList)DB.getRetencion(where);//obtengo a partir de la referencia
                    if (temp != null)
                    {
                        if (temp.Count > 0)
                        {
                            operacion = (RE_GenericBean)temp[0];
                            i++;
                            object[] objArr2 = { operacion.strC1, " ", "RT - " + operacion.strC21 + "-" + operacion.strC6, " " + " " + " ", operacion.decC1, 0, " ", " ", " ", Tipo_CambioDOCUMENTO.ToString(), i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                            ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr2);
                        }
                    }
                }
                if (rgb.intC4 == 1)
                { //FA Intercompanys
                    operacion = (RE_GenericBean)DB.getFacturaData(rgb.intC1);
                    RE_GenericBean viaje = (RE_GenericBean)DB.getViaje(operacion.strC11, user.pais.schema);//obtengo los datos del viaje a partir del contenedor
                    operacion.strC17 = viaje.strC1;//vapor (barco)
                    operacion.strC18 = viaje.strC2;//No de viaje
                    Tipo_CambioDOCUMENTO = Convert.ToDouble(DB.getTipoCambioFactura(rgb.intC1));
                    i++;
                    object[] objArr = { operacion.strC5, operacion.strC11, "FA - " + operacion.strC28 + "-" + operacion.strC1, operacion.strC17 + " " + operacion.strC18, operacion.decC3, 0, operacion.strC9, operacion.strC10, operacion.strC11, Tipo_CambioDOCUMENTO.ToString(), i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);

                    //Abonos con NC
                    ArrayList temp = null;
                    temp = (ArrayList)DB.getNotaCreditData_By_Factura(rgb.intC1);
                    if (temp != null)
                    {
                        if (temp.Count > 0)
                        {
                            operacion = (RE_GenericBean)temp[0];
                            i++;
                            object[] objArr2 = { operacion.strC1, " ", "NC   - " + operacion.strC3 + "-" + operacion.strC4, " " + " " + " ", 0, operacion.decC1, " ", " ", " ", i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                            ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr2);
                        }
                    }
                }
                operacion = null;
            }

            if (i == 0) {
                object[] objArr2 = { "", "", "", "", 0, 0, "", "", "", Params.logo, Params.titulo, Params.edicion };
                ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr2);
            }

            Datos_Pago = DB.getDatosPagoSOA(id);
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_EstadoCtaAgente.rpt"));
            rpt.SetDataSource(ds.Tables["EstadoCtaAgente_tbl"]);
            rpt.SetParameterValue("fecha_corte", SOA_Bean.strC2);
            rpt.SetParameterValue("referencia_pago_tipo", Datos_Pago);
            RE_GenericBean Usuario_Bean = (RE_GenericBean)DB.getUsuarioEmpresa(user.ID);
            rpt.SetParameterValue("prepara", Usuario_Bean.strC1);
            rpt.SetParameterValue("refNo", SOA_Bean.strC1 + SOA_Bean.intC2.ToString());
            rpt.SetParameterValue("provName", nombre);
            rpt.SetParameterValue("country", user.pais.Nombre);
            rpt.SetParameterValue("autoriza", "");
            rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(SOA_Bean.intC3));
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
    }
    private void Page_Unload(object sender, EventArgs e)
    {
        #region Clear Report Objects
        if (rpt != null)
        {
            rpt.Close();
            rpt.Dispose();
            GC.Collect();
        }
        #endregion
    }
}
