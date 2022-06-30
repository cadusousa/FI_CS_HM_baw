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

public partial class operations_print_retencion : System.Web.UI.Page
{
    UsuarioBean user;
    string linea = "";
    string retenciones = "";
    int x_retencion_13 = 0, y_retencion_13 = 0;
    int x_retencion_5 = 0, y_retencion_5 = 0;
    int x_retencion_1 = 0, y_retencion_1 = 0;
    int x_retencion_10 = 0, y_retencion_10 = 0;
    int x_provision_serie = 0, y_provision_serie = 0;
    int x_provision_correlativo = 0, y_provision_correlativo = 0;
    int x_provision_total = 0, y_provision_total = 0;
    int x_sumas = 0, y_sumas = 0;
    int x_iva = 0, y_iva = 0;
    int x_sub_total = 0, y_sub_total = 0;
    int x_total_compra = 0, y_total_compra = 0;
    int x_iva_retenido = 0, y_iva_retenido = 0;
    int x_ventas_exentas = 0, y_ventas_exentas = 0;
    int x_renta = 0, y_renta = 0;
    int x_total = 0, y_total = 0;
    int chequetransID = 0;
    string codigo, nombre, direccion, telefono, nit, ruc = "";
    int tipo = 20;
    int chequeID = 0;
    int interlineado = 0;
    decimal Sumas = 0;
    decimal Iva = 0;
    decimal Sub_Total = 0;
    decimal Total_Compra = 0;
    decimal Iva_Retenido = 0;
    decimal Ventas_Exentas = 0;
    decimal Renta = 0;
    decimal Total = 0;

    RE_GenericBean font_interlineado = null;
    Font Fuente = new Font("Courier New", 10);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        user = (UsuarioBean)Session["usuario"];
        if (Request.QueryString["chequeID"] != null)
        {
            chequeID = int.Parse(Request.QueryString["chequeID"].ToString());
            Imprimir_Documento();
        }
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


                if (user.ImpresionBean.Operacion == "1")
                {
                    #region Impresion
                    if ((user.ImpresionBean.Tipo_Documento == "9") && (user.ImpresionBean.Id == chequeID.ToString()) && (user.ImpresionBean.Impreso == false))
                    {
                        #region Definir Tamanio del Papel
                        /*
                        foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                            if (Tamanio_Papel.PaperName.StartsWith("A4"))
                            pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                        */
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
                    #region Definir Tamanio del Papel
                    /*
                    foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                        if (Tamanio_Papel.PaperName.StartsWith("A4"))
                            pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                    */
                    #endregion
                    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                    pd.Print();
                    #endregion
                }
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
        RE_GenericBean rgb_cheque = (RE_GenericBean)DB.getChequeDataforView(chequeID);
        if (rgb_cheque.intC7 == 4)//proveedor
        {
            RE_GenericBean Proveedor_Bean = DB.getProveedorData(rgb_cheque.intC6, "");
            nombre = Proveedor_Bean.strC2;
            nit = Proveedor_Bean.strC1;
            direccion = Proveedor_Bean.strC5;
            telefono = Proveedor_Bean.strC7;
            ruc = Proveedor_Bean.strC11;
        }
        else if (rgb_cheque.intC7 == 2)//agente
        {
            RE_GenericBean Agente_Bean = DB.getAgenteData(rgb_cheque.intC6, "");
            nombre = Agente_Bean.strC1;
            direccion = Agente_Bean.strC2;
            telefono = Agente_Bean.strC3;
            nit = Agente_Bean.strC6;
            ruc = Agente_Bean.strC7;
        }
        else if (rgb_cheque.intC7 == 5)//naviera
        {
            RE_GenericBean Naviera_Bean = DB.getNavieraData(rgb_cheque.intC6);
            nombre = Naviera_Bean.strC1;
            nit = Naviera_Bean.strC2;
            ruc = Naviera_Bean.strC3;
        }
        else if (rgb_cheque.intC7 == 6)//linea aerea
        {
            RE_GenericBean Carrier_Bean = DB.getCarriersData(rgb_cheque.intC6);
            nombre = Carrier_Bean.strC1;
            nit = Carrier_Bean.strC2;
            ruc = Carrier_Bean.strC3;
        }
        else if (rgb_cheque.intC7 == 8)//Caja Chica
        {
            RE_GenericBean CajaChica_Bean = (RE_GenericBean)DB.getUsuarioEmpresabyID(rgb_cheque.intC6);
            nombre = CajaChica_Bean.strC1;
            nit = CajaChica_Bean.strC2;
        }
        int doc_id = DB.getDocumentoID(user.SucursalID, "RT", 20, user);
        font_interlineado = DB.getFactura(doc_id, tipo);
        interlineado= font_interlineado.intC6;
        ArrayList camposfact = (ArrayList)DB.getCamposDoc(tipo, user.SucursalID, "RT");//parametros 1=factura (tipo doc); 2=sucursal de la factura; 3=serie de factura
        Label lb1 = null;
        Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
        foreach (RE_GenericBean rgb in camposfact)
        {
            lb1 = new Label();
            lb1.Text = "";
            if (rgb.strC1.Trim().Equals("CODIGO_CLIENTE")) lb1.Text = rgb.strC2.Trim() + rgb_cheque.intC6;
            if (rgb.strC1.Trim().Equals("NOMBRE")) lb1.Text = rgb.strC2.Trim() + " " + nombre;
            if (rgb.strC1.Trim().Equals("DIRECCION")) lb1.Text = rgb.strC2.Trim() + " " + direccion;
            if (rgb.strC1.Trim().Equals("TELEFONO")) lb1.Text = rgb.strC2.Trim() + " " + telefono;
            if (rgb.strC1.Trim().Equals("NIT")) lb1.Text = rgb.strC2.Trim() + " " + nit;
            if (rgb.strC1.Trim().Equals("RUC")) lb1.Text = rgb.strC2.Trim() + " " + ruc;
            if (rgb.strC1.Trim().Equals("VENDEDOR")) lb1.Text = rgb.strC2.Trim() + " " + "";
            if (rgb.strC1.Trim().Equals("FECHA_IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + rgb_cheque.strC3;
            if (rgb.strC1.Trim().Equals("FECHA (D)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(rgb_cheque.strC3).Day.ToString();
            if (rgb.strC1.Trim().Equals("FECHA (M)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(rgb_cheque.strC3).Month.ToString();
            if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(rgb_cheque.strC3).Year.ToString();
            if (rgb.strC1.Trim().Equals("USUARIO")) lb1.Text = rgb.strC2.Trim() + " " + user.ID.ToString();
            if (rgb.strC1.Trim().Equals("HORA")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Now.ToShortTimeString();
            if (rgb.strC1.Trim().Equals("MOTIVO")) lb1.Text = rgb.strC2.Trim() + " " + rgb_cheque.strC5;
            Conv c = new Conv();
            if (rgb_cheque.intC8 == 1)//Fiscal
            {
                if (rgb.strC1.Trim().Equals("TOTAL_LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + c.enletras(rgb_cheque.decC1.ToString(), 1);
            }
            else
            {
                if (rgb.strC1.Trim().Equals("TOTAL_LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + c.enletras(rgb_cheque.decC1.ToString(), rgb_cheque.intC4);
            }
            if (rgb.strC1.Trim().Equals("PROVISION_SERIE"))
            {
                x_provision_serie = rgb.intC1;
                y_provision_serie = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("PROVISION_CORRELATIVO"))
            {
                x_provision_correlativo = rgb.intC1;
                y_provision_correlativo = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("PROVISION_TOTAL"))
            {
                x_provision_total = rgb.intC1;
                y_provision_total = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("RT_10%_SOBRE_BASE"))
            {
                x_retencion_10 = rgb.intC1;
                y_retencion_10 = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("RT_1%_SOBRE_BASE"))
            {
                x_retencion_1 = rgb.intC1;
                y_retencion_1 = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("RT_5%_SOBRE_TOTAL"))
            {
                x_retencion_5 = rgb.intC1;
                y_retencion_5 = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("RT_13%_SOBRE_BASE"))
            {
                x_retencion_13 = rgb.intC1;
                y_retencion_13 = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("SUMAS"))
            {
                x_sumas = rgb.intC1;
                y_sumas = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("IVA"))
            {
                x_iva = rgb.intC1;
                y_iva = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("SUB_TOTAL"))
            {
                x_sub_total = rgb.intC1;
                y_sub_total = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("TOTAL_COMPRA"))
            {
                x_total_compra = rgb.intC1;
                y_total_compra = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("IVA_RETENIDO"))
            {
                x_iva_retenido = rgb.intC1;
                y_iva_retenido = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("VENTAS_EXENTAS"))
            {
                x_ventas_exentas = rgb.intC1;
                y_ventas_exentas = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("RENTA"))
            {
                x_renta = rgb.intC1;
                y_renta = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("TOTAL"))
            {
                x_total = rgb.intC1;
                y_total = rgb.intC2;
            }
            lb1.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            Page.Controls.Add(lb1);
            e.Graphics.DrawString(lb1.Text, Fuente, Brushes.Black, rgb.intC1, rgb.intC2);
        }
        linea = "";
        linea = Dibujar_Linea(115);
        lb1.Text = linea;
        e.Graphics.DrawString(linea, Fuente, Brushes.Black, 100, 360);
        lb1.Attributes.Add("Style", "left: " + 100 + "px; top: " + 360 + "px; position: absolute;");
        Page.Controls.Add(lb1);
        e.Graphics.DrawString(linea, Fuente, Brushes.Black, 100, 400);
        lb1.Attributes.Add("Style", "left: " + 100 + "px; top: " + 400 + "px; position: absolute;");
        Page.Controls.Add(lb1);
        retenciones="                    10 % sobre base      1 % sobre base      5 % sobre total      13 % sobre base ";
        e.Graphics.DrawString(retenciones, Fuente, Brushes.Black, 100, 380);
        lb1.Text = retenciones;
        lb1.Attributes.Add("Style", "left: " + 100 + "px; top: " + 380 + "px; position: absolute;");
        Page.Controls.Add(lb1);
    #region Cargar Retenciones
        ArrayList Provisiones = (ArrayList)DB.getProvisionesPagadasPorCheque(chequeID);
        foreach (RE_GenericBean rgb in Provisiones)
        {
            int bandera = 0;
            ArrayList Retenciones = (ArrayList)DB.getRetencionesDeProvision(chequeID, rgb.intC1);
            foreach (RE_GenericBean rgb_retencion in Retenciones)
            {
                if ((x_provision_serie >= 0) && (y_provision_serie >= 0)&&(bandera<1))
                {
                    //Impresion Web
                    lb1.Text = rgb_retencion.strC1;
                    lb1.Attributes.Add("Style", "left: " + x_provision_serie + "px; top: " + y_provision_serie + "px; position: absolute;");
                    Page.Controls.Add(lb1);
                    //Impresion Fisica
                    e.Graphics.DrawString(rgb_retencion.strC1, Fuente, Brushes.Black, x_provision_serie, y_provision_serie);
                    //Impresion Web
                    lb1.Text = rgb_retencion.strC2;
                    lb1.Attributes.Add("Style", "left: " + x_provision_correlativo + "px; top: " + y_provision_correlativo + "px; position: absolute;");
                    Page.Controls.Add(lb1);
                    //Impresion Fisica
                    e.Graphics.DrawString(rgb_retencion.strC2, Fuente, Brushes.Black, x_provision_correlativo, y_provision_correlativo);
                    //Impresion Web
                    lb1.Text = rgb_retencion.decC3.ToString();
                    lb1.Attributes.Add("Style", "left: " + x_provision_total + "px; top: " + y_provision_total + "px; position: absolute;");
                    Page.Controls.Add(lb1);
                    //Impresion Fisica
                    e.Graphics.DrawString(rgb_retencion.decC3.ToString(), Fuente, Brushes.Black, x_provision_total, y_provision_total);
                    Sumas += rgb_retencion.decC3;
                    Iva += rgb_retencion.decC5;
                    y_provision_serie += interlineado;
                    y_provision_correlativo += interlineado;
                    y_provision_total += interlineado;
                    bandera++;
                }
                if ((x_retencion_10 >= 0) && (y_retencion_10 >= 0))
                {
                    if ((rgb_retencion.intC2 == 5)&&(rgb_retencion.decC1>=0))//Retencion 10% sobre la Base para SV
                    {
                        //Impresion Web
                        lb1.Text = rgb_retencion.decC1.ToString();
                        lb1.Attributes.Add("Style", "left: " + x_retencion_10 + "px; top: " + y_retencion_10 + "px; position: absolute;");
                        Page.Controls.Add(lb1);
                        //Impresion Fisica
                        e.Graphics.DrawString(rgb_retencion.decC1.ToString(), Fuente, Brushes.Black, x_retencion_10, y_retencion_10);
                        y_retencion_10 += interlineado;
                        Renta += rgb_retencion.decC1;
                    }
                }
                if ((x_retencion_1 >= 0) && (y_retencion_1 >= 0))
                {
                    if ((rgb_retencion.intC2 == 7) && (rgb_retencion.decC1 >= 0))//Retencion 1% sobre la Base para SV
                    {
                        //Impresion Web
                        lb1.Text = rgb_retencion.decC1.ToString();
                        lb1.Attributes.Add("Style", "left: " + x_retencion_1 + "px; top: " + y_retencion_1 + "px; position: absolute;");
                        Page.Controls.Add(lb1);
                        //Impresion Fisica
                        e.Graphics.DrawString(rgb_retencion.decC1.ToString(), Fuente, Brushes.Black, x_retencion_1, y_retencion_1);
                        y_retencion_1 += interlineado;
                        Iva_Retenido += rgb_retencion.decC1;
                    }
                }
                if ((x_retencion_5 >= 0) && (y_retencion_5 >= 0))
                {
                    if ((rgb_retencion.intC2 == 4) && (rgb_retencion.decC1 >= 0))//Retencion 5% sobre el Total para SV
                    {
                        //Impresion Web
                        lb1.Text = rgb_retencion.decC1.ToString();
                        lb1.Attributes.Add("Style", "left: " + x_retencion_5 + "px; top: " + y_retencion_5 + "px; position: absolute;");
                        Page.Controls.Add(lb1);
                        //Impresion Fisica
                        e.Graphics.DrawString(rgb_retencion.decC1.ToString(), Fuente, Brushes.Black, x_retencion_5, y_retencion_5);
                        y_retencion_5 += interlineado;
                        Iva_Retenido += rgb_retencion.decC1;
                    }
                }
                if ((x_retencion_13 >= 0) && (y_retencion_13 >= 0))
                {
                    if ((rgb_retencion.intC2 == 6) && (rgb_retencion.decC1 >= 0))//Retencion 13% sobre la Base para SV
                    {
                        //Impresion Web
                        lb1.Text = rgb_retencion.decC1.ToString();
                        lb1.Attributes.Add("Style", "left: " + x_retencion_13 + "px; top: " + y_retencion_13 + "px; position: absolute;");
                        Page.Controls.Add(lb1);
                        //Impresion Fisica
                        e.Graphics.DrawString(rgb_retencion.decC1.ToString(), Fuente, Brushes.Black, x_retencion_13, y_retencion_13);
                        y_retencion_13 += interlineado;
                        Iva_Retenido += rgb_retencion.decC1;
                    }
                }
            }
        }
        //Calculos
        Sub_Total = Sumas + Iva;
        Total_Compra = Sub_Total;
        //Impresion Web
        lb1.Text = Sumas.ToString();
        lb1.Attributes.Add("Style", "left: " + x_sumas + "px; top: " + y_sumas + "px; position: absolute;");
        Page.Controls.Add(lb1);
        //Impresion Fisica
        e.Graphics.DrawString(Sumas.ToString(), Fuente, Brushes.Black, x_sumas, y_sumas);
        //Impresion Web
        lb1.Text = Iva.ToString();
        lb1.Attributes.Add("Style", "left: " + x_iva + "px; top: " + y_iva + "px; position: absolute;");
        Page.Controls.Add(lb1);
        //Impresion Fisica
        e.Graphics.DrawString(Iva.ToString(), Fuente, Brushes.Black, x_iva, y_iva);
        //Impresion Web
        lb1.Text = Sub_Total.ToString();
        lb1.Attributes.Add("Style", "left: " + x_sub_total + "px; top: " + y_sub_total + "px; position: absolute;");
        Page.Controls.Add(lb1);
        //Impresion Fisica
        e.Graphics.DrawString(Sub_Total.ToString(), Fuente, Brushes.Black, x_sub_total, y_sub_total);
        //Impresion Web
        lb1.Text = Total_Compra.ToString();
        lb1.Attributes.Add("Style", "left: " + x_total_compra + "px; top: " + y_total_compra + "px; position: absolute;");
        Page.Controls.Add(lb1);
        //Impresion Fisica
        e.Graphics.DrawString(Total_Compra.ToString(), Fuente, Brushes.Black, x_total_compra, y_total_compra);
        //Impresion Web
        lb1.Text = Iva_Retenido.ToString();
        lb1.Attributes.Add("Style", "left: " + x_iva_retenido + "px; top: " + y_iva_retenido + "px; position: absolute;");
        Page.Controls.Add(lb1);
        //Impresion Fisica
        e.Graphics.DrawString(Iva_Retenido.ToString(), Fuente, Brushes.Black, x_iva_retenido, y_iva_retenido);
        //Impresion Web
        lb1.Text = Ventas_Exentas.ToString();
        lb1.Attributes.Add("Style", "left: " + x_ventas_exentas + "px; top: " + y_ventas_exentas + "px; position: absolute;");
        Page.Controls.Add(lb1);
        //Impresion Fisica
        e.Graphics.DrawString(Ventas_Exentas.ToString(), Fuente, Brushes.Black, x_ventas_exentas, y_ventas_exentas);
        //Impresion Web
        lb1.Text = Renta.ToString();
        lb1.Attributes.Add("Style", "left: " + x_renta + "px; top: " + y_renta + "px; position: absolute;");
        Page.Controls.Add(lb1);
        //Impresion Fisica
        e.Graphics.DrawString(Renta.ToString(), Fuente, Brushes.Black, x_renta, y_renta);
        Total= Total_Compra-Iva_Retenido-Renta;
        //Impresion Web
        lb1.Text = Total.ToString();
        lb1.Attributes.Add("Style", "left: " + x_total + "px; top: " + y_total + "px; position: absolute;");
        Page.Controls.Add(lb1);
        //Impresion Fisica
        e.Graphics.DrawString(Total.ToString(), Fuente, Brushes.Black, x_total, y_total);
        #endregion
    }
    public string Dibujar_Linea(int Longitud)
    {
        string linea = "";
        for (int i = 0; i <= Longitud; i++)
        {
            linea += "-";
        }
        return linea;
    }
}
