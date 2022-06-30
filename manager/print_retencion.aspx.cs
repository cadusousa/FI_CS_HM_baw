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
using System.Drawing;
using System.Drawing.Printing;

public partial class invoice_print_invoice : System.Web.UI.Page
{
    UsuarioBean user;
    int pais=0, provID=0;
    string serie = "";
    int x_retencion_13 = 0, y_retencion_13 = 0;
    int x_retencion_5 = 0, y_retencion_5 = 0;
    int x_retencion_1 = 0, y_retencion_1 = 0;
    int x_retencion_10 = 0, y_retencion_10 = 0;
    int chequetransID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt=null;
        RE_GenericBean datosretencion=null;
        RE_GenericBean font_interlineado = null;
        RE_GenericBean rgb_cliente = null;
        RE_GenericBean datosprovision = null;
        if (Request.QueryString["serie"] != null) {
            chequetransID = int.Parse(Request.QueryString["chequetransID"].ToString());
        }
        user = (UsuarioBean)Session["usuario"];
    }
    private void Imprimir_Documento()
    {
        ImpersonationSettings settings = new ImpersonationSettings();
        UserImpersonation userImpersonation = new UserImpersonation(settings);
        try
        {
            userImpersonation.Impersonate();
            string PrinterName = "";
            try
            {
                PrintDocument pd = new PrintDocument();
                PrinterName = user.PrinterName;
                if (PrinterName == "0")
                {
                    WebMsgBox.Show("El documento que esta tratando de Imprimir no tiene ninguna impresora asignada");
                    return;
                }
                pd.PrinterSettings.PrinterName = PrinterName;
                #region Definir Tamanio del Papel
                /*
                foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                    if (Tamanio_Papel.PaperName.StartsWith("A4"))
                        pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                */
                #endregion
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                pd.Print();
            }
            catch (Exception ex)
            {
                log4net ErrLog = new log4net();
                ErrLog.ErrorLog(ex.Message);
            }
        }
        finally
        {
            userImpersonation.UndoImpersonation();
        }
    }
    private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
    {
        ArrayList retencionarr = (ArrayList)DB.getRetencionesbyChequeForPrint(chequetransID);
        foreach (RE_GenericBean rgb in retencionarr)
        {
            if (rgb.intC9 == 4)//5% Sobre Total
            { 
            }
            if (rgb.intC9 == 5)//10% Sobre la Base
            {
            }
            if (rgb.intC9 == 6)//13% Sobre la Base
            {
            }
            if (rgb.intC9 == 7)//1% Sobre la Base
            {
            }
        }
    }
}
