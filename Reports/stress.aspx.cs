using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

public partial class Reports_stress : System.Web.UI.Page
{
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_generar_Click(object sender, EventArgs e)
    {

        //int cantidad = int.Parse(tb_cantidad.Text);
        int cantidad = 1;
        for (int i = 1; i <= 76; i++)
        {
            Generar_Reporte();
            lbl_mensaje.Text += "Reporte de Creditos #" + cantidad + " generado exitosamente.<br>";
            cantidad++;
        }
    }
    protected void Generar_Reporte()
    {
        UsuarioBean user_temporal = new UsuarioBean();
        user_temporal.ID = "dennis-ariana";
        user_temporal.PaisID = 1;
        PaisBean Pais_Temporal = new PaisBean();
        Pais_Temporal = DB.getPais(user_temporal.PaisID);
        user_temporal.pais = Pais_Temporal;

        LibroDiarioDS ds = new LibroDiarioDS();
        ds = (LibroDiarioDS)DB.GetReporte_CreditosClientes(user_temporal, user_temporal.pais.Nombre);
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_Creditos_Clientes.rpt"));
        rpt.SetDataSource(ds.Tables["tbl_creditos_clientes"]);
        //rpt.SetParameterValue("empresa", user_temporal.pais.Nombre);
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
        CrystalReportViewer1.Dispose();
        CrystalReportViewer1 = null;
        #endregion
    }
}