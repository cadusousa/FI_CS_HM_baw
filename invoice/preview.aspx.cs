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

public partial class invoice_print_invoice : System.Web.UI.Page
{
    UsuarioBean user;
    int suc_id = 0, tipo=1;
    string serie = "";
    int xnombre_ini = 0, ynombre_ini=0;
    int xsubt_ini = 0, ysubt_ini = 0;
    int ximp_ini = 0, yimp_ini = 0;
    int xtot_ini = 0, ytot_ini = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt=null;
        RE_GenericBean datosfactura=null;
        RE_GenericBean font_interlineado = null;
        if (Request.QueryString["suc_id"] != null) {
            suc_id = int.Parse(Request.QueryString["suc_id"].ToString());
            tipo = int.Parse(Request.QueryString["tipo"].ToString());
            serie = Request.QueryString["serie"].ToString();
        }
        user = (UsuarioBean)Session["usuario"];
        int doc_id=DB.getDocumentoID(suc_id, serie, tipo, user);
        font_interlineado=DB.getFactura(doc_id, tipo);
        ArrayList camposfact = (ArrayList)DB.getCamposDoc(tipo, suc_id, serie);//parametros 1=factura (tipo doc); 2=sucursal de la factura; 3=serie de factura
        Label lb1 = null;
        foreach (RE_GenericBean rgb in camposfact)
        {
            // dibujo el campo
            lb1 = new Label();
            lb1.Text = "";
            if (rgb.strC1.Trim().Equals("FECHA EMISION")) lb1.Text = rgb.strC2.Trim()+" "+"01/01/1900";
            if (rgb.strC1.Trim().Equals("FECHA (D)")) lb1.Text = rgb.strC2.Trim() + " " + "01";
            if (rgb.strC1.Trim().Equals("FECHA (M)")) lb1.Text = rgb.strC2.Trim() + " " + "01";
            if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + "1900";
            if (rgb.strC1.Trim().Equals("NOMBRE")) lb1.Text = rgb.strC2.Trim() + " " + "Nombre ejemplo";
            if (rgb.strC1.Trim().Equals("DIRECCION")) lb1.Text = rgb.strC2.Trim() + " " + "Direccion Ejemplo";
            if (rgb.strC1.Trim().Equals("NIT")) lb1.Text = rgb.strC2.Trim() + " " + "123456-9";
            if (rgb.strC1.Trim().Equals("SUB TOTAL")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + "89.29";
            if (rgb.strC1.Trim().Equals("IMPUESTO")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + "10.71";
            if (rgb.strC1.Trim().Equals("TOTAL")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + "100.00";
            if (rgb.strC1.Trim().Equals("FECHA PAGO")) lb1.Text = rgb.strC2.Trim() + " " + "01/01/2000";
            if (rgb.strC1.Trim().Equals("OBSERVACIONES")) lb1.Text = rgb.strC2.Trim() + " " + "Observaciones ejemplo";
            if (rgb.strC1.Trim().Equals("RAZON")) lb1.Text = rgb.strC2.Trim() + " " + "Razon ejemplo";
            if (rgb.strC1.Trim().Equals("MONEDA")) lb1.Text = rgb.strC2.Trim() + " " + "Mon ej.";
            if (rgb.strC1.Trim().Equals("USUARIO")) lb1.Text = rgb.strC2.Trim() + " " + "user ejemplo";
            if (rgb.strC1.Trim().Equals("HBL")) lb1.Text = rgb.strC2.Trim() + " " + "hbl ejemplo";
            if (rgb.strC1.Trim().Equals("MBL")) lb1.Text = rgb.strC2.Trim() + " " + "mbl ejemplo";
            if (rgb.strC1.Trim().Equals("CONTENEDOR")) lb1.Text = rgb.strC2.Trim() + " " + "contenedor ejemplo";
            if (rgb.strC1.Trim().Equals("ROUTING")) lb1.Text = rgb.strC2.Trim() + " " + "routing ejemplo";
            if (rgb.strC1.Trim().Equals("NAVIERA")) lb1.Text = rgb.strC2.Trim() + " " + "naviera ejemplo";
            if (rgb.strC1.Trim().Equals("VAPOR")) lb1.Text = rgb.strC2.Trim() + " " + "vapor ejemplo";
            if (rgb.strC1.Trim().Equals("SHIPPER")) lb1.Text = rgb.strC2.Trim() + " " + "shipper ejemplo";
            if (rgb.strC1.Trim().Equals("POLIZA ADUANAL")) lb1.Text = rgb.strC2.Trim() + " " + "oren ejemplo";
            if (rgb.strC1.Trim().Equals("CONSIGNATARIO")) lb1.Text = rgb.strC2.Trim() + " " + "consignatario ej.";
            if (rgb.strC1.Trim().Equals("COMODITY")) lb1.Text = rgb.strC2.Trim() + " " + "comodity ej.";
            if (rgb.strC1.Trim().Equals("PAQUETES")) lb1.Text = rgb.strC2.Trim() + " " + "paquetes ej.";
            if (rgb.strC1.Trim().Equals("PESO")) lb1.Text = rgb.strC2.Trim() + " " + "peso ej.";
            if (rgb.strC1.Trim().Equals("VOLUMEN")) lb1.Text = rgb.strC2.Trim() + " " + "vol ej.";
            if (rgb.strC1.Trim().Equals("DUA INGRESO")) lb1.Text = rgb.strC2.Trim() + " " + "dua ingreso ej";
            if (rgb.strC1.Trim().Equals("DUA SALIDA")) lb1.Text = rgb.strC2.Trim() + " " + "dua salida ej.";
            if (rgb.strC1.Trim().Equals("VENDEDOR 1")) lb1.Text = rgb.strC2.Trim() + " " + "vendedor 1 ej.";
            if (rgb.strC1.Trim().Equals("VENDEDOR 2")) lb1.Text = rgb.strC2.Trim() + " " + "vendedor 2 ej.";
            if (rgb.strC1.Trim().Equals("REFERENCIA")) lb1.Text = rgb.strC2.Trim() + " " + "referencia";
            if (rgb.strC1.Trim().Equals("SERIE")) lb1.Text = rgb.strC2.Trim() + " " + "serie";
            if (rgb.strC1.Trim().Equals("TOTAL LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + "total letras";
            if (rgb.strC1.Trim().Equals("DOC. REFERENCIA (CHEQUE/DEPOSITO)")) lb1.Text = rgb.strC2.Trim() + " " + "No boleta";
            if (rgb.strC1.Trim().Equals("BANCO")) lb1.Text = rgb.strC2.Trim() + " " + "Banco prueba";
            if (rgb.strC1.Trim().Equals("NOTA DOCUMENTO")) lb1.Text = rgb.strC2.Trim();
            if (rgb.strC1.Trim().Equals("FECHA IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Now.ToShortDateString();
            if (rgb.strC1.Trim().Equals("HORA IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Now.ToShortTimeString();
            if (rgb.strC1.Trim().Equals("MARCA CONTADO")) lb1.Text = rgb.strC2.Trim() + " X";
            if (rgb.strC1.Trim().Equals("MARCA CREDITO")) lb1.Text = rgb.strC2.Trim() + " X";
            if (rgb.strC1.Trim().Equals("CODIGO CLIENTE")) lb1.Text = rgb.strC2.Trim() + " 123456";
            if (rgb.strC1.Trim().Equals("SERIE FACTURA")) lb1.Text = rgb.strC2.Trim() + " F ";
            if (rgb.strC1.Trim().Equals("CORRELATIVO FACTURA")) lb1.Text = rgb.strC2.Trim() + " 123";
            if (rgb.strC1.Trim().Equals("RECIBO ADUANA")) lb1.Text = rgb.strC2.Trim() + " ejemplo recibo aduana";
            if (rgb.strC1.Trim().Equals("ENCABEZADO")) lb1.Text = rgb.strC2.Trim() + " Ejemplo de encabezado";
            if ((tipo == 1 || tipo == 4))
            {
                if (rgb.strC1.Trim().Equals("RUBRO_NOMBRE"))
                {
                    xnombre_ini = rgb.intC1+font_interlineado.intC8;
                    ynombre_ini = rgb.intC2;
                    lb1.Text = rgb.strC2.Trim() + " " + "Rubro prueba";
                }
                if (rgb.strC1.Trim().Equals("RUBRO_SUBTOTAL"))
                {
                    xsubt_ini = rgb.intC1;
                    ysubt_ini = rgb.intC2;
                    lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + "90.00";
                }
                if (rgb.strC1.Trim().Equals("RUBRO_IMPUESTO"))
                {
                    ximp_ini = rgb.intC1;
                    yimp_ini = rgb.intC2;
                    lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + "10.00";
                }
                if (rgb.strC1.Trim().Equals("RUBRO_TOTAL"))
                {
                    xtot_ini = rgb.intC1;
                    ytot_ini = rgb.intC2;
                    lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + "100.00";
                }

                //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&66
                if (font_interlineado.intC8 != 0)
                {
                    if (rgb.strC1.Trim().Equals("RUBRO_TOTAL"))
                    {
                        xtot_ini = rgb.intC1 + font_interlineado.intC8;
                        ytot_ini = rgb.intC2;
                        lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + "100.00(excento)";
                    }
                }
            }
            lb1.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            if (font_interlineado.strC2!=null && !font_interlineado.strC2.Equals("")) lb1.Font.Name = font_interlineado.strC2;
            if (font_interlineado.intC7 != null && font_interlineado.intC7 > 0) lb1.Font.Size = font_interlineado.intC7;
            Page.Controls.Add(lb1);

        }
        //************************************************************************
        if ((tipo == 1 || tipo == 4) && (dt != null && dt.Rows.Count > 0))
        {
            int interlineado = 18;
            if (font_interlineado.intC6 != 0) interlineado = font_interlineado.intC6;
            Label lb = null;
            foreach (DataRow dr in dt.Rows)
            {
                // dibujo el nombre del rubro
                lb = new Label();
                lb.Text = dr[0].ToString();
                lb.Attributes.Add("Style", "left: " + xnombre_ini + "px; top: " + ynombre_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 != null && font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                ynombre_ini += interlineado;
                Page.Controls.Add(lb);
                //dibujo el subtotal
                lb = new Label();
                lb.Text = dr[1].ToString();
                lb.Attributes.Add("Style", "left: " + xsubt_ini + "px; top: " + ysubt_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 != null && font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                ysubt_ini += interlineado;
                Page.Controls.Add(lb);
                //dibujo el impuesto
                lb = new Label();
                lb.Text = dr[2].ToString();
                lb.Attributes.Add("Style", "left: " + ximp_ini + "px; top: " + yimp_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 != null && font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                yimp_ini += interlineado;
                Page.Controls.Add(lb);
                //dibujo el Total
                lb = new Label();
                lb.Text = dr[3].ToString();
                lb.Attributes.Add("Style", "left: " + xtot_ini + "px; top: " + ytot_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 != null && font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                ytot_ini += interlineado;
                Page.Controls.Add(lb);
            }
        }
    }
}
