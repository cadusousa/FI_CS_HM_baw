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

public partial class Reports_SubReporte_BS : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ArrayList Datos = null;
    string fecha_inicial = "";
    string fecha_final = "";
    string cue_nombre = "";
    string cue_id = "";
    string consMoneda = "";
    int contaID = 0;
    int monID = 0;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            if (Session["usuario"] == null)
            {
                Response.Redirect("../default.aspx");
            }
            user = (UsuarioBean)Session["usuario"];
            LibroDiarioDS ds = new LibroDiarioDS();
            #region Backup 2012-03-27
            //fecha_inicial = Request.QueryString["fecha_inicial"].ToString();
            //fecha_final = Request.QueryString["fecha_final"].ToString();
            //cue_id = Request.QueryString["cue_id"].ToString();
            //contaID = int.Parse(Request.QueryString["contaID"].ToString());
            //monID = int.Parse(Request.QueryString["monID"].ToString());
            #endregion
            string QUERY_STRING = Request.ServerVariables["QUERY_STRING"].ToString();
            QUERY_STRING = Page.Server.UrlDecode(QUERY_STRING);
            string[] Variables = QUERY_STRING.Split('?');
            fecha_inicial = Variables[0].ToString();
            fecha_final = Variables[1].ToString();
            cue_id = Variables[2].ToString();
            contaID = int.Parse(Variables[3].ToString());
            monID = int.Parse(Variables[4].ToString());

            int num = Variables.Count();
            if (num < 6)
            {
                consMoneda = "";
            }
            else
            {
                consMoneda = Variables[5].ToString();
            }
            //consMoneda = Variables[5].ToString();
            RE_GenericBean Cuenta_Bean = DB.getCtabyCtaID(cue_id);


            if (!IsPostBack)
            {

                #region Registrar Log de Reportes
                RE_GenericBean Bean_Log = new RE_GenericBean();
                Bean_Log.intC1 = user.PaisID;
                Bean_Log.strC1 = "SUBREPORTE BALANCE GENERAL";
                Bean_Log.strC2 = user.ID;
                Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
                string mensaje_log = "";
                mensaje_log += "Contabilidad.: " + contaID + " Moneda.: " + monID + " ,";
                mensaje_log += "Cuenta ID.: " + cue_id + " ,";
                mensaje_log += "Fecha Inicial.: " + fecha_inicial + " Fecha Final.: " + fecha_final + " ,";
                Bean_Log.strC4 = mensaje_log;
                DB.Insertar_Log_Reportes(Bean_Log);
                #endregion

                if (Cuenta_Bean.intC2 == 3)//Nivel 3
                {
                    arr = (ArrayList)DB.getPartidasByCuentaContable(cue_id, user, fecha_inicial, fecha_final, contaID, monID, consMoneda);
                }
                else if (Cuenta_Bean.intC2 == 4)//Nivel 4
                {
                    //arr = (ArrayList)DB.getPartidasByCuentaContableForER(cue_id, user, fecha_inicial, fecha_final, contaID, monID, consMoneda);
                    arr = (ArrayList)DB.getPartidasByCuentaContableForERhhmm(cue_id, user, fecha_inicial, fecha_final, contaID, monID, consMoneda);                
                }

                int i = 0;
                DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "MOVIMIENTO CONTABLE " + Cuenta_Bean.strC2, ""); //pais, sistema, doc_id, titulo, edicion

                Npgsql.NpgsqlConnection conn;
                conn = DB.OpenConnection();

                foreach (RE_GenericBean Partidas in arr)
                {
                    cue_nombre= Partidas.strC3;

                    Partidas.strC7 = Partidas.intC1.ToString(); // "Pnd.";
                    Partidas.strC8 = Partidas.intC2.ToString(); // "Pnd.";
                    Partidas.strC9 = "Pnd.";
                    Partidas.strC10 = "Pnd.";
                    Partidas.strC11 = "Pnd.";

                    Datos = (ArrayList)DB.getSerieCorrAndObsByDochhmm(Partidas.intC1, Partidas.intC2, conn);
                    if (Datos != null)
                    {
                        if (Datos.Count > 0)
                        {
                            Partidas.strC7 = Datos[0].ToString();
                            Partidas.strC8 = Datos[1].ToString();
                            Partidas.strC9 = Datos[2].ToString();
                            Partidas.strC10 = Datos[3].ToString();
                            Partidas.strC11 = Datos[4].ToString();
                        }
                    }

                    i++;
                    object[] ObjArr = { Partidas.strC1, Partidas.strC7, Partidas.strC8, Partidas.strC9, Partidas.decC1, Partidas.decC2, Partidas.strC2.Substring(0, 10), Partidas.strC5, Partidas.strC10, Partidas.strC6, Partidas.strC11, "", "", "", "", "", "", i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["SubReporte_BS"].Rows.Add(ObjArr);

                    /*
                    Datos = (ArrayList)DB.getSerieCorrAndObsByDoc(Partidas.intC1, Partidas.intC2);
                    if (Datos != null)
                    {
                        if (Datos.Count > 0)
                        {
                            i++;
                            object[] ObjArr = { Partidas.strC1, Datos[0].ToString(), Datos[1].ToString(), Datos[2].ToString(), Partidas.decC1, Partidas.decC2, Partidas.strC2.Substring(0, 10), Partidas.strC5, Datos[3].ToString(), Partidas.strC6, Datos[4].ToString(), "", "", "", "", "", "", i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                            ds.Tables["SubReporte_BS"].Rows.Add(ObjArr);
                        }
                        else
                        {
                            i++;
                            object[] ObjArr2 = { Partidas.strC1, Partidas.intC1.ToString(), "Pnd.", Partidas.intC2.ToString(), Partidas.decC1, Partidas.decC2, Partidas.strC2.Substring(0, 10), Partidas.strC5, "Pnd.", Partidas.strC6, "Pnd.", "", "", "", "", "", "", i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                            ds.Tables["SubReporte_BS"].Rows.Add(ObjArr2);
                        }
                    }
                    else
                    {
                        i++;
                        object[] ObjArr2 = { Partidas.strC1, "Pnd.", "Pnd.", "Pnd.", Partidas.decC1, Partidas.decC2, Partidas.strC2.Substring(0, 10), Partidas.strC5, "Pnd.", Partidas.strC6, "Pnd.", "", "", "", "", "", "", i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["SubReporte_BS"].Rows.Add(ObjArr2);
                    }
                    */
                    

                }

                conn.Close();

                if (i == 0)
                {                                
                    object[] ObjArr = { "", "", "", "", 0, 0, "", "", "", "", "", "", "", "", "", "", "", Params.logo, Params.titulo, Params.edicion };
                    ds.Tables["SubReporte_BS"].Rows.Add(ObjArr);
                }

                ViewState["SubReporte_BS"] = ds;//Definir ViewState
                lbl_cue_nombre.Text = Cuenta_Bean.strC2;//Definir ViewState para mantener el nombre de la cuenta
                /*
                #region Construir Crystal Report
                rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rpt.Load(Server.MapPath("~/CR_SubReporte_ER.rpt"));
                rpt.SetDataSource(ds.Tables["SubReporte_BS"]);
                if (user.contaID == 1)
                {
                    rpt.SetParameterValue("contabilidad", "FISCAL");
                }
                else if (user.contaID == 2)
                {
                    rpt.SetParameterValue("contabilidad", "FINANCIERA");
                }
                else if (user.contaID == 3)
                {
                    rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
                }
                rpt.SetParameterValue("fecha_inicial", fecha_inicial);
                rpt.SetParameterValue("fecha_final", fecha_final);
                rpt.SetParameterValue("pais", user.pais.Nombre);
                rpt.SetParameterValue("cue_nombre",Cuenta_Bean.strC2);
                rpt.SetParameterValue("cue_id",cue_id);
                rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
                CrystalReportViewer1.ReportSource = rpt;
                CrystalReportViewer1.DataBind();
                #endregion
               */
            }
            else
            {
                ds = (LibroDiarioDS)ViewState["SubReporte_BS"];
                cue_nombre = lbl_cue_nombre.Text;
                
                /*            #region Construir Crystal Report
                            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                            rpt.Load(Server.MapPath("~/CR_SubReporte_ER.rpt"));
                            rpt.SetDataSource(ds.Tables["SubReporte_BS"]);
                            if (user.contaID == 1)
                            {
                                rpt.SetParameterValue("contabilidad", "FISCAL");
                            }
                            else if (user.contaID == 2)
                            {
                                rpt.SetParameterValue("contabilidad", "FINANCIERA");
                            }
                            else if (user.contaID == 3)
                            {
                                rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
                            }
                            rpt.SetParameterValue("fecha_inicial", fecha_inicial);
                            rpt.SetParameterValue("fecha_final", fecha_final);
                            rpt.SetParameterValue("pais", user.pais.Nombre);
                            rpt.SetParameterValue("cue_nombre", cue_nombre);
                            rpt.SetParameterValue("cue_id", cue_id);
                            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
                            CrystalReportViewer1.ReportSource = rpt;
                            CrystalReportViewer1.DataBind();
                            #endregion
                 */
            }


 /*
            string filename = "C:/BAW_REPORTS/BG_SubReporte.xls";
            DB.GetHtmlData(ds.Tables["SubReporte_BS"], "", 0, "", filename, "");
            Response.Write("<a href='" + "file" + ":///" + filename + "'>Abrir Reporte</a>");
*/


            
            #region Construir Crystal Report
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_SubReporte_ER.rpt"));
            rpt.SetDataSource(ds.Tables["SubReporte_BS"]);
            if (user.contaID == 1)
            {
                rpt.SetParameterValue("contabilidad", "FISCAL");
            }
            else if (user.contaID == 2)
            {
                rpt.SetParameterValue("contabilidad", "FINANCIERA");
            }
            else if (user.contaID == 3)
            {
                rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
            }
            rpt.SetParameterValue("fecha_inicial", fecha_inicial);
            rpt.SetParameterValue("fecha_final", fecha_final);
            rpt.SetParameterValue("pais", user.pais.Nombre);
            rpt.SetParameterValue("cue_nombre", cue_nombre);
            rpt.SetParameterValue("cue_id", cue_id);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
            #endregion


        }
        catch (Exception ex) {
            Response.Write(ex.Message);        
        }
    }
    private void Page_Unload(object sender, EventArgs e)
    {
        try {

            #region Clear Report Objects
            if (rpt != null)
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }
            #endregion

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
}
