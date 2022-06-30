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

public partial class Reports_Libro_Compras : System.Web.UI.Page
{
    UsuarioBean user = null;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    char[] delimit = new char[] { ',' };
    int contaID = 0;
    int monedaID = 0;
    bool Tipo_Doc;//Fiscal o No Fiscal
    string Documentos = "";
    int tipo_Persona = 0;
    string estado_Documento = "";
    string fecha_inicial = "";
    string fecha_final = "";
    string Join_Provisiones = "";
    string Join_NC = "";
    string Join_NC2 = "";
    string Join_ND = "";
    string Join_ND2 = "";
    int grupoSAT = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        contaID = int.Parse(Request.QueryString["t_con"].ToString());
        monedaID =int.Parse(Request.QueryString["t_mon"].ToString());
        Tipo_Doc = bool.Parse(Request.QueryString["t_doc"].ToString());
        Documentos = Request.QueryString["docts"].ToString();
        tipo_Persona = int.Parse(Request.QueryString["t_tpi"].ToString());
        estado_Documento = Request.QueryString["t_ted"].ToString();
        fecha_inicial = Request.QueryString["f_inicial"].ToString();
        fecha_final = Request.QueryString["f_final"].ToString();
        grupoSAT = int.Parse(Request.QueryString["grupo"].ToString());
        ArrayList Arr_Fechas = (ArrayList)DB.Formatear_Fechas(fecha_inicial, fecha_final);
        fecha_inicial = Arr_Fechas[0].ToString();
        fecha_final = Arr_Fechas[1].ToString();
        foreach (string Tipo in Documentos.Split(delimit))
        {
            if (Tipo != "")
            {
                if (int.Parse(Tipo) == 5)//Provisiones
                {
                    if (contaID == 3)
                    {
                        Join_Provisiones = " and tpr_pai_id=" + user.PaisID + " and tpr_ted_id in (" + estado_Documento + ") " +
                    " and tpr_fecha_libro_compras>='" + fecha_inicial + "' and tpr_fecha_libro_compras<='" + fecha_final + "' and tpr_fiscal=" + Tipo_Doc + "";
                    }
                    else
                    {
                        Join_Provisiones = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " and tpr_ted_id in (" + estado_Documento + ") " +
                        " and tpr_fecha_libro_compras>='" + fecha_inicial + "' and tpr_fecha_libro_compras<='" + fecha_final + "' and tpr_fiscal=" + Tipo_Doc + "";
                    }
                    if (tipo_Persona > 0)
                    {
                        Join_Provisiones += " and tpr_tpi_id=" + tipo_Persona + "";
                    }
                    if (grupoSAT >0)
                    {
                        Join_Provisiones += " and tpr_suc_id in (select tes_suc_id from tbl_empresa_sucursal, tbl_empresa where tes_tem_id=tem_id and tem_grupo_sat='" + grupoSAT + "')";
                    }
                }
                if (int.Parse(Tipo) == 3)//Notas de Credito
                {
                    if(contaID == 3)
                    {
                        Join_NC = " tnc_pai_id=" + user.PaisID + " and tnc_ted_id in (" + estado_Documento + ") " +
                        " and tnc_fecha_libro_compras>='" + fecha_inicial + "' and tnc_fecha_libro_compras<='" + fecha_final + "' and tnc_fiscal=" + Tipo_Doc + "";
                        Join_NC2 = " and tpr_pai_id=" + user.PaisID + " ";
                    }
                    else
                    {
                        Join_NC = " tnc_pai_id=" + user.PaisID + " and tnc_tcon_id=" + contaID + " and tnc_ted_id in (" + estado_Documento + ") " +
                        " and tnc_fecha_libro_compras>='" + fecha_inicial + "' and tnc_fecha_libro_compras<='" + fecha_final + "' and tnc_fiscal=" + Tipo_Doc + "";
                        Join_NC2 = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " ";
                    }
                    if (tipo_Persona > 0)
                    {
                        Join_NC += " and tnc_tpi_id=" + tipo_Persona + "";
                    }
                    else
                    {
                        Join_NC += " and tnc_tpi_id in (2,4,5,6,8) ";
                    }
                    if (grupoSAT > 0)
                    {
                        Join_NC += " and tnc_suc_id in (select tes_suc_id from tbl_empresa_sucursal, tbl_empresa where tes_tem_id=tem_id and tem_grupo_sat='" + grupoSAT + "')";
                    }
                }
                /*if (int.Parse(Tipo) == 4)//Notas de Debito
                {
                    Join_ND = " tnd_pai_id=" + user.PaisID + " and tnd_tcon_id=" + contaID + " and tnd_ted_id in (" + estado_Documento + ") " +
                    " and tnd_fecha_libro_compras>='" + fecha_inicial + "' and tnd_fecha_libro_compras<='" + fecha_final + "' and tnd_fiscal=" + Tipo_Doc + "";
                    Join_ND2 = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " ";
                    if (tipo_Persona > 0)
                    {
                        Join_ND += " and tnd_tpi_id=" + tipo_Persona + "";
                    }
                    else
                    {
                        Join_ND += " and tnd_tpi_id in (2,4,5,6,8) ";
                    }
                    if (grupoSAT > 0)
                    {
                        Join_ND += " and tnd_suc_id in (select tes_suc_id from tbl_empresa_sucursal, tbl_empresa where tes_tem_id=tem_id and tem_grupo_sat='" + grupoSAT + "')";
                    }
                }*/
            }
        }
        LibroDiarioDS ds = null;
        if (!Page.IsPostBack)
        {
            ds = (LibroDiarioDS)DB.getLibro_Compras(Join_Provisiones, Join_ND, Join_NC, Documentos, Join_NC2, Join_ND2, monedaID, contaID, user, grupoSAT, tipo_Persona);
            ViewState["dscompras"] = ds;
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["dscompras"];
        }
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();                        
        rpt.Load(Server.MapPath("~/CR_Compras.rpt"));
        rpt.SetDataSource(ds.Tables["Compras"]);
        rpt.SetParameterValue("fechaini", fecha_inicial);
        rpt.SetParameterValue("fechafin", fecha_final);
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monedaID));
        rpt.SetParameterValue("pais", user.pais.Nombre);
        string contabilidad = "";
        if (contaID == 1){contabilidad = "FISCAL";}
        if (contaID == 2){contabilidad = "FINANCIERA";}
        if (contaID == 3) { contabilidad = "CONSOLIDADO"; }
        rpt.SetParameterValue("contabilidad", contabilidad);
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

