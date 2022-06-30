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
using System.Security.Principal;

public partial class invoice_print_invoice : System.Web.UI.Page
{
    UsuarioBean user;
    int fac_id = 0, tipo = 1;
    int xnombre_ini = 0, ynombre_ini = 0;
    int xsubt_ini = 0, ysubt_ini = 0;
    int ximp_ini = 0, yimp_ini = 0;
    int xtot_ini = 0, ytot_ini = 0;
    int xcargoeq_ini = 0, ycargoeq_ini = 0;
    int xabonoeq_ini = 0, yabonoeq_ini = 0;
    string texto;
    Label lb = null;
    int interlineado = 18;
    Font Fuente = new Font("Courier New", 10);
    string ctaID = "";
    int correlativo = 0;
    int imp_id = 0;
    int DOC_ID = 0;
    RE_GenericBean FONT_Interlineado = null;
    int tcg_id = 0;
    string criterio = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        DataTable dt=new DataTable();
        dt.Columns.Add("Cuenta ID");
        dt.Columns.Add("Cuenta Nombre");
        dt.Columns.Add("Debe");
        dt.Columns.Add("Haber");
        dt.Columns.Add("Debe_Equivalente");
        dt.Columns.Add("Haber_Equivalente");
        RE_GenericBean datosfactura=null;
        RE_GenericBean font_interlineado = null;
        if (Request.QueryString["ctaID"] != null) {
            ctaID = Request.QueryString["ctaID"].ToString();
            correlativo = int.Parse(Request.QueryString["correlativo"].ToString());
        }
        if (Request.QueryString["fac_id"] != null)
        {
            tcg_id = int.Parse(Request.QueryString["fac_id"].ToString());
            criterio = " and tcg_id=" + tcg_id + " ";
        }

        user = (UsuarioBean)Session["usuario"];
        datosfactura = (RE_GenericBean)DB.getChequeData(ctaID, correlativo, criterio);
        fac_id = datosfactura.intC1;
        ArrayList cuentas = (ArrayList)DB.getCuentasbyCheque(datosfactura.intC1, datosfactura.intC4);

        foreach (RE_GenericBean aux in cuentas) {
            object[] obj = { aux.strC1, aux.strC2, aux.decC1.ToString(), aux.decC2.ToString(), aux.decC3.ToString(), aux.decC4.ToString() };
            dt.Rows.Add(obj);
        }

        font_interlineado = DB.getDataCuenta(ctaID);
        FONT_Interlineado = font_interlineado;
        string banconombre = DB.getNombreBanco(font_interlineado.intC1); 
        ArrayList camposfact = (ArrayList)DB.getCampoPlantillaCheque(ctaID);

        Label lb1 = null;

        //imp_id = font_interlineado.intC9;
        Imprimir_Cheque();
        foreach (RE_GenericBean rgb in camposfact)
        {
            // dibujo el campo
            lb1 = new Label();
            lb1.Text = "";
            //if (rgb.strC1.Trim().Equals("FECHA EMISION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2;
            //if (rgb.strC1.Trim().Equals("FECHA (D)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2.Substring(0,2);
            //if (rgb.strC1.Trim().Equals("FECHA (M)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2.Substring(3, 2);
            //if (rgb.strC1.Trim().Equals("FECHA (M) LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(int.Parse(datosfactura.strC2.Substring(3, 2)));
            //if (rgb.strC1.Trim().Equals("FECHA (M) LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(DateTime.Parse(datosfactura.strC2).Month);
            //if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2.Substring(6, 4);
            //if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC2).Year.ToString();
            //if (rgb.strC1.Trim().Equals("NOMBRE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC3;
            //if (rgb.strC1.Trim().Equals("TOTAL")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC1.ToString("#,#.00#;(#,#.00#)");
            //if (rgb.strC1.Trim().Equals("NOTA CHEQUE(NO NEGOCIABLE)")) lb1.Text = rgb.strC2.Trim();
            //if (rgb.strC1.Trim().Equals("TOTAL LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5;

            //***************************************************************************************************************
            //if (rgb.strC1.Trim().Equals("FECHA EMISION VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2;
            //if (rgb.strC1.Trim().Equals("FECHA (D) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2.Substring(0,2);
            //if (rgb.strC1.Trim().Equals("FECHA (M) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2.Substring(3, 2);
            //if (rgb.strC1.Trim().Equals("FECHA (M) VOUCHER LETRAS"))
            //    if (font_interlineado.intC4 == 8)
            //    {
            //        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr_english(DateTime.Parse(datosfactura.strC2).Month);
            //    }
            //    else
            //    {
            //        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(DateTime.Parse(datosfactura.strC2).Month);
            //    }
                
                
            //    //lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(int.Parse(datosfactura.strC2.Substring(3, 2)));
            ////if (rgb.strC1.Trim().Equals("FECHA (A) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2.Substring(6, 4);
            //if (rgb.strC1.Trim().Equals("FECHA (A) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC2).Year.ToString();
            //if (rgb.strC1.Trim().Equals("TOTAL LETRAS VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5;

            //if (rgb.strC1.Trim().Equals("NOMBRE VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC3;
            //if (rgb.strC1.Trim().Equals("BANCO VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + banconombre;
            //if (rgb.strC1.Trim().Equals("CUENTA VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "cuenta voucher";
            if (rgb.strC1.Trim().Equals("CONCEPTO VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC4;
            if (rgb.strC1.Trim().Equals("TOTAL VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC1.ToString("#,#.00#;(#,#.00#)");
            if (rgb.strC1.Trim().Equals("HECHO POR VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC6;
            if (rgb.strC1.Trim().Equals("TOTAL CARGO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC1.ToString("#,#.00#;(#,#.00#)");
            if (rgb.strC1.Trim().Equals("TOTAL ABONO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC1.ToString("#,#.00#;(#,#.00#)");


            if (rgb.strC1.Trim().Equals("CUENTA BANCARIA VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + ctaID;
            if (rgb.strC1.Trim().Equals("REFERENCIA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC7;
            if (rgb.strC1.Trim().Equals("TIPO CAMBIO")) lb1.Text = rgb.strC2.Trim() + " " + user.pais.TipoCambio;
            if (rgb.strC1.Trim().Equals("FECHA IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Now.ToShortDateString();
            if (rgb.strC1.Trim().Equals("OBSERVACIONES")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC8;
            if (rgb.strC1.Trim().Equals("NO CHEQUE VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + correlativo;
            if (rgb.strC1.Trim().Equals("NO CHEQUE")) lb1.Text = rgb.strC2.Trim() + " " + correlativo;
            if (rgb.strC1.Trim().Equals("HORA")) lb1.Text = DateTime.Now.ToShortTimeString();


            if (rgb.strC1.Trim().Equals("ID CUENTA CONTABLE VOUCHER"))
            {
                xnombre_ini = rgb.intC1;
                ynombre_ini = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("NOMBRE CUENTA CONTABLE VOUCHER"))
            {
                xsubt_ini = rgb.intC1;
                ysubt_ini = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("CARGO"))
            {
                ximp_ini = rgb.intC1;
                yimp_ini = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("ABONO"))
            {
                xtot_ini = rgb.intC1;
                ytot_ini = rgb.intC2;
            }
            lb1.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb1.Font.Name = font_interlineado.strC2;
            if (font_interlineado.intC7>0) lb1.Font.Size = font_interlineado.intC7; 
            Page.Controls.Add(lb1);
        }

        foreach (DataRow dr in dt.Rows)
        {
            if (xnombre_ini > 0 && ynombre_ini > 0)
            {
                //dibujo el total
                lb = new Label();
                texto = dr[0].ToString();
                lb.Text = texto;
                lb.Attributes.Add("Style", "left: " + xnombre_ini + "px; top: " + ynombre_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                ynombre_ini += interlineado;
                Page.Controls.Add(lb);
            }
            if (xsubt_ini > 0 && ysubt_ini > 0)
            {
                //dibujo el subtotal
                lb = new Label();
                texto = dr[1].ToString();
                lb.Text = texto;
                lb.Attributes.Add("Style", "left: " + xsubt_ini + "px; top: " + ysubt_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                ysubt_ini += interlineado;
                Page.Controls.Add(lb);
            }
            if (ximp_ini > 0 && yimp_ini > 0)
            {//CARGO
                //dibujo el impuesto
                lb = new Label();
                texto = decimal.Parse(dr[2].ToString()).ToString("#,#.00");
                lb.Text = texto.ToString();
                lb.Attributes.Add("Style", "left: " + ximp_ini + "px; top: " + yimp_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                yimp_ini += interlineado;
                lb.Width = 50;
                lb.Attributes.Add("Align", "Right");
                Page.Controls.Add(lb);
            }
            if (xtot_ini > 0 && ytot_ini > 0)
            {//ABONO
                //dibujo el Total
                lb = new Label();
                texto = decimal.Parse(dr[3].ToString()).ToString("#,#.00");
                lb.Text = texto.ToString();
                lb.Attributes.Add("Style", "left: " + xtot_ini + "px; top: " + ytot_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                ytot_ini += interlineado;
                lb.Width = 50;
                lb.Attributes.Add("Align", "Right");
                Page.Controls.Add(lb);
            }
        }
    }

    #region Imprimir Cheque

    protected void Imprimir_Cheque()
    {
        //ImpersonationSettings settings = new ImpersonationSettings();
        //UserImpersonation userImpersonation = new UserImpersonation(settings);
        WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero);
        try
        {
            //userImpersonation.Impersonate();
            //Actividad que deseemos realizar con mayores permisos
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
                if (user.ImpresionBean.Operacion == "1")
                {
                    #region BACKUP
                    //if ((user.ImpresionBean.Tipo_Documento == "6") && (user.ImpresionBean.Id == fac_id.ToString()) && (user.ImpresionBean.Impreso == false))
                    //{
                    //    pd.PrinterSettings.PrinterName = PrinterName;
                    //    #region Definir Tamanio del Papel
                    //    if (user.PaisID != 2)
                    //    {
                    //        foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                    //            if (Tamanio_Papel.PaperName.StartsWith("A4"))
                    //                pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                    //    }
                    //    else
                    //    {
                    //        if ((FONT_Interlineado.intC1 == 25) || (FONT_Interlineado.intC1 == 27))
                    //        {
                    //            #region Cheques del BAC/CITI El Salvador
                    //            foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                    //                if (Tamanio_Papel.PaperName.Equals("SV-CH-CITI-BAC"))
                    //                    pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                    //            #endregion
                    //        }
                    //        else
                    //        {
                    //            #region Todos los otros Cheques El Salvador
                    //            foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                    //                if (Tamanio_Papel.PaperName.Equals("SV-CH"))
                    //                    pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                    //            #endregion
                    //        }
                    //    }
                    //    #endregion
                    //    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                    //    pd.Print();
                    //    user = (UsuarioBean)Session["usuario"];
                    //    user.ImpresionBean.Impreso = true;
                    //    Session["usuario"] = user;
                    //}
                    #endregion
                    #region Impresion
                    if ((user.ImpresionBean.Tipo_Documento == "6") &&(user.ImpresionBean.Id == fac_id.ToString()) && (user.ImpresionBean.Impreso == false))
                    {
                        pd.PrinterSettings.PrinterName = PrinterName;
                        #region Definir Tamanio del Papel
                        if (user.PaisID == 2)
                        {
                            if ((FONT_Interlineado.intC1 == 25) || (FONT_Interlineado.intC1 == 27))
                            {
                                #region Cheques del BAC/CITI El Salvador
                                foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                    if (Tamanio_Papel.PaperName.Equals("SV-CH-CITI-BAC"))
                                        pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                                #endregion
                            }
                            else
                            {
                                #region Todos los otros Cheques El Salvador
                                foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                    if (Tamanio_Papel.PaperName.Equals("SV-CH"))
                                        pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                                #endregion
                            }
                        }
                        else if (user.PaisID == 1)
                        {
                            if (((FONT_Interlineado.intC1 == 18) && (FONT_Interlineado.strC1 == "0000-98010-7532")) || ((FONT_Interlineado.intC1 == 16) && (FONT_Interlineado.strC1 == "027-018962-1")))
                            {
                                #region Cheques Banco Reformador Cuenta 0000-98010-7532
                                foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                    if (Tamanio_Papel.PaperName.Equals("GT-CH-REFOR"))
                                        pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                                #endregion
                            }
                            else
                            {
                                #region Todos los otros Cheques Guatemala
                                foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                    if (Tamanio_Papel.PaperName.Equals("A4"))
                                        pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                                #endregion
                            }
                        }
                        else
                        {
                            #region Todas las Demas Empresas
                            foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                if (Tamanio_Papel.PaperName.StartsWith("A4"))
                                    pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                            #endregion
                        }
                        #endregion
                        pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                        pd.Print();
                        user = (UsuarioBean)Session["usuario"];
                        user.ImpresionBean.Impreso = true;
                        Session["usuario"] = user;
                    }
                    #endregion
                }
                else if (user.ImpresionBean.Operacion == "2")
                {
                    #region Re-Impresion
                    pd.PrinterSettings.PrinterName = PrinterName;
                    #region Definir Tamanio del Papel

                    if (user.PaisID == 2)
                    {
                        if ((FONT_Interlineado.intC1 == 25) || (FONT_Interlineado.intC1 == 27))
                        {
                            #region Cheques del BAC/CITI El Salvador
                            foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                if (Tamanio_Papel.PaperName.Equals("SV-CH-CITI-BAC"))
                                    pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                            #endregion
                        }
                        else
                        {
                            #region Todos los otros Cheques El Salvador
                            foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                if (Tamanio_Papel.PaperName.Equals("SV-CH"))
                                    pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                            #endregion
                        }
                    }
                    else if (user.PaisID == 1)
                    {
                        if (((FONT_Interlineado.intC1 == 18) && (FONT_Interlineado.strC1 == "0000-98010-7532")) || ((FONT_Interlineado.intC1 == 16) && (FONT_Interlineado.strC1 == "027-018962-1")))
                        {
                            #region Cheques Banco Reformador Cuenta 0000-98010-7532
                            foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                if (Tamanio_Papel.PaperName.Equals("GT-CH-REFOR"))
                                    pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                            #endregion
                        }
                        else
                        {
                            #region Todos los otros Cheques Guatemala
                            foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                if (Tamanio_Papel.PaperName.Equals("A4"))
                                    pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                            #endregion
                        }
 
                    }
                    else
                    {
                        foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                            if (Tamanio_Papel.PaperName.StartsWith("A4"))
                                pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                    }
                    
                    #endregion
                    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                    pd.Print();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                WebMsgBox.Show("An error occurred while printing" + ex.ToString());
                log4net ErrLog = new log4net();
                ErrLog.ErrorLog(ex.Message);
            }
        }
        finally
        {
            wic.Undo();
        }
    }
    public DataSet oset = new DataSet();
    private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Cuenta ID");
            dt.Columns.Add("Cuenta Nombre");
            dt.Columns.Add("Debe");
            dt.Columns.Add("Haber");
            dt.Columns.Add("Debe_Equivalente");
            dt.Columns.Add("Haber_Equivalente");
            RE_GenericBean datosfactura = null;
            RE_GenericBean font_interlineado = null;
            if (Request.QueryString["fac_id"] != null)
            {
                tcg_id = int.Parse(Request.QueryString["fac_id"].ToString());
                criterio = " and tcg_id=" + tcg_id + " ";
            }
            datosfactura = (RE_GenericBean)DB.getChequeData(ctaID, correlativo, criterio);
            ArrayList cuentas = (ArrayList)DB.getCuentasbyCheque(datosfactura.intC1, datosfactura.intC4);

            foreach (RE_GenericBean aux in cuentas)
            {
                object[] obj = { aux.strC1, aux.strC2, aux.decC1.ToString(), aux.decC2.ToString(), aux.decC3.ToString(), aux.decC4.ToString() };
                dt.Rows.Add(obj);
            }

            font_interlineado = DB.getDataCuenta(ctaID);
            string banconombre = DB.getNombreBanco(font_interlineado.intC1);
            ArrayList camposfact = (ArrayList)DB.getCampoPlantillaCheque(ctaID);

            Label lb1 = null;
            foreach (RE_GenericBean rgb in camposfact)
            {
                // dibujo el campo
                lb1 = new Label();
                lb1.Text = "";
                if (rgb.strC1.Trim().Equals("FECHA EMISION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2;
                if (rgb.strC1.Trim().Equals("FECHA (D)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC2).Day.ToString();
                if (rgb.strC1.Trim().Equals("FECHA (M)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC2).Month.ToString();
                if (rgb.strC1.Trim().Equals("FECHA (M) LETRAS"))
                {
                    if (font_interlineado.intC10 == 2)//Ingles
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr_english(DateTime.Parse(datosfactura.strC2).Month);
                    }
                    else
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(DateTime.Parse(datosfactura.strC2).Month);
                    }
                }

                if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC2).Year.ToString();
                if (rgb.strC1.Trim().Equals("NOMBRE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC3;
                if (rgb.strC1.Trim().Equals("TOTAL")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC1.ToString("#,#.00#;(#,#.00#)");
                if (rgb.strC1.Trim().Equals("TOTAL_EQUIVALENTE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC2.ToString("#,#.00#;(#,#.00#)");
                if (rgb.strC1.Trim().Equals("NOTA CHEQUE(NO NEGOCIABLE)")) lb1.Text = rgb.strC2.Trim();
                if (rgb.strC1.Trim().Equals("TOTAL LETRAS"))
                {
                    Conv c = new Conv();
                    if (font_interlineado.intC10 == 2)//Ingles
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC1.ToString(), 8);
                    else
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC1.ToString(), 1);
                }
                if (rgb.strC1.Trim().Equals("TOTAL_LETRAS_EQUIVALENTE"))
                {
                    Conv c = new Conv();
                    if (font_interlineado.intC10 == 2)//Ingles
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC2.ToString(), 8);
                    else
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC2.ToString(), 1);
                }
                if (rgb.strC1.Trim().Equals("FECHA EMISION VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2;
                if (rgb.strC1.Trim().Equals("FECHA (D) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC2).Day.ToString();
                if (rgb.strC1.Trim().Equals("FECHA (M) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC2).Month.ToString();
                if (rgb.strC1.Trim().Equals("FECHA (M) VOUCHER LETRAS"))
                {
                    if (font_interlineado.intC10 == 2)//Ingles
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr_english(DateTime.Parse(datosfactura.strC2).Month);
                    }
                    else
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(DateTime.Parse(datosfactura.strC2).Month);
                    }
                }

                if (rgb.strC1.Trim().Equals("FECHA (A) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC2).Year.ToString();
                if (rgb.strC1.Trim().Equals("TOTAL LETRAS VOUCHER"))
                {
                    Conv c = new Conv();
                    if (font_interlineado.intC10 == 2)//Ingles
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC1.ToString(), 8);
                    else
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC1.ToString(), 1);
                }
                if (rgb.strC1.Trim().Equals("TOTAL_LETRAS_VOUCHER_EQUIVALENTE"))
                {
                    Conv c = new Conv();
                    if (font_interlineado.intC10 == 2)//Ingles
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC1.ToString(), 8);
                    else
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC1.ToString(), 1);
                }
                if (rgb.strC1.Trim().Equals("NOMBRE VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC3;
                if (rgb.strC1.Trim().Equals("BANCO VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + banconombre;
                if (rgb.strC1.Trim().Equals("CONCEPTO VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC4;
                if (rgb.strC1.Trim().Equals("TOTAL VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC1.ToString("#,#.00#;(#,#.00#)");
                if (rgb.strC1.Trim().Equals("TOTAL_VOUCHER_EQUIVALENTE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC2.ToString("#,#.00#;(#,#.00#)");
                if (rgb.strC1.Trim().Equals("HECHO POR VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC6;
                if (rgb.strC1.Trim().Equals("TOTAL CARGO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC1.ToString("#,#.00#;(#,#.00#)");
                if (rgb.strC1.Trim().Equals("TOTAL ABONO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC1.ToString("#,#.00#;(#,#.00#)");
                if (rgb.strC1.Trim().Equals("TOTAL_CARGO_EQUIVALENTE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC2.ToString("#,#.00#;(#,#.00#)");
                if (rgb.strC1.Trim().Equals("TOTAL_ABONO_EQUIVALENTE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC2.ToString("#,#.00#;(#,#.00#)");
                if (rgb.strC1.Trim().Equals("CUENTA BANCARIA VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + ctaID;
                if (rgb.strC1.Trim().Equals("REFERENCIA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC7;
                if (rgb.strC1.Trim().Equals("TIPO CAMBIO")) lb1.Text = rgb.strC2.Trim() + " " + user.pais.TipoCambio;
                if (rgb.strC1.Trim().Equals("FECHA IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Now.ToShortDateString();
                if (rgb.strC1.Trim().Equals("OBSERVACIONES")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC8;
                if (rgb.strC1.Trim().Equals("NO CHEQUE VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + correlativo;
                if (rgb.strC1.Trim().Equals("NO CHEQUE")) lb1.Text = rgb.strC2.Trim() + " " + correlativo;
                if (rgb.strC1.Trim().Equals("HORA")) lb1.Text = DateTime.Now.ToShortTimeString();

                if (rgb.strC1.Trim().Equals("ID CUENTA CONTABLE VOUCHER"))
                {
                    xnombre_ini = rgb.intC1;
                    ynombre_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("NOMBRE CUENTA CONTABLE VOUCHER"))
                {
                    xsubt_ini = rgb.intC1;
                    ysubt_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("CARGO"))
                {
                    ximp_ini = rgb.intC1;
                    yimp_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("ABONO"))
                {
                    xtot_ini = rgb.intC1;
                    ytot_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("CARGO_EQUIVALENTE"))
                {
                    xcargoeq_ini = rgb.intC1;
                    ycargoeq_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("ABONO_EQUIVALENTE"))
                {
                    xabonoeq_ini = rgb.intC1;
                    yabonoeq_ini = rgb.intC2;
                }
                Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                e.Graphics.DrawString(lb1.Text, Fuente, Brushes.Black, rgb.intC1, rgb.intC2);
            }

            foreach (DataRow dr in dt.Rows)
            {
                if (xnombre_ini > 0 && ynombre_ini > 0)
                {
                    //ID CUENTA CONTABLE VOUCHER
                    lb = new Label();
                    texto = dr[0].ToString();
                    lb.Text = texto;
                    e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                    ynombre_ini += interlineado;
                }
                if (xsubt_ini > 0 && ysubt_ini > 0)
                {
                    //NOMBRE CUENTA CONTABLE VOUCHER
                    lb = new Label();
                    texto = dr[1].ToString();
                    lb.Text = texto;
                    e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xsubt_ini, ysubt_ini);
                    ysubt_ini += interlineado;
                }
                if (ximp_ini > 0 && yimp_ini > 0)
                {//CARGO
                    lb = new Label();
                    texto = decimal.Parse(dr[2].ToString()).ToString("#,#.00");
                    lb.Text = texto.ToString();
                    e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, ximp_ini, yimp_ini);
                    yimp_ini += interlineado;
                }
                if (xtot_ini > 0 && ytot_ini > 0)
                {//ABONO
                    lb = new Label();
                    texto = decimal.Parse(dr[3].ToString()).ToString("#,#.00");
                    lb.Text = texto.ToString();
                    e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xtot_ini, ytot_ini);
                    ytot_ini += interlineado;
                }
                if (xcargoeq_ini > 0 && ycargoeq_ini > 0)
                {//CARGO_EQUIVALENTE
                    lb = new Label();
                    texto = decimal.Parse(dr[4].ToString()).ToString("#,#.00");
                    lb.Text = texto.ToString();
                    e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xcargoeq_ini, ycargoeq_ini);
                    ycargoeq_ini += interlineado;
                }
                if (xabonoeq_ini > 0 && yabonoeq_ini > 0)
                {//ABONO_EQUIVALENTE
                    lb = new Label();
                    texto = decimal.Parse(dr[5].ToString()).ToString("#,#.00");
                    lb.Text = texto.ToString();
                    e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xabonoeq_ini, yabonoeq_ini);
                    yabonoeq_ini += interlineado;
                }
            }
        }
        catch (Exception ex)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(ex.Message);
        }
    }

    #endregion
}
