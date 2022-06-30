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
    string ctaID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt=null;
        RE_GenericBean font_interlineado = null;
        if (Request.QueryString["ctaID"] != null)
        {
            ctaID = Request.QueryString["ctaID"].ToString();
        }
        else {
            WebMsgBox.Show("Debe indicar alguna cuenta bancaria para previsualizar");
            return;
        }
        user = (UsuarioBean)Session["usuario"];
        font_interlineado=DB.getDataCuenta(ctaID);
        ArrayList camposfact = (ArrayList)DB.getCampoPlantillaCheque(ctaID);
        Label lb1 = null;
        foreach (RE_GenericBean rgb in camposfact)
        {
            // dibujo el campo
            lb1 = new Label();
            lb1.Text = "";
            if (rgb.strC1.Trim().Equals("FECHA EMISION")) lb1.Text = rgb.strC2.Trim()+" "+"01/01/1900";
            if (rgb.strC1.Trim().Equals("FECHA (D)")) lb1.Text = rgb.strC2.Trim() + " " + "01";
            if (rgb.strC1.Trim().Equals("FECHA (M)")) lb1.Text = rgb.strC2.Trim() + " " + "01";
            if (rgb.strC1.Trim().Equals("FECHA (M) LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + "Julio";
            if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + "1900";
            if (rgb.strC1.Trim().Equals("NOMBRE")) lb1.Text = rgb.strC2.Trim() + " " + "Nombre ejemplo";
            if (rgb.strC1.Trim().Equals("TOTAL")) lb1.Text = rgb.strC2.Trim() + " " + "100.00";
            if (rgb.strC1.Trim().Equals("NOTA CHEQUE(NO NEGOCIABLE)")) lb1.Text = rgb.strC2.Trim() + " " + "¡NO NEGOCIABLE!";
            if (rgb.strC1.Trim().Equals("TOTAL LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + "TOTAL LETRAS";

            if (rgb.strC1.Trim().Equals("FECHA EMISION VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "fecha voucher";
            if (rgb.strC1.Trim().Equals("FECHA (D) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "01";
            if (rgb.strC1.Trim().Equals("FECHA (M) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "01";
            if (rgb.strC1.Trim().Equals("FECHA (M) VOUCHER LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + "Julio";
            if (rgb.strC1.Trim().Equals("FECHA (A) VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "1900";
            if (rgb.strC1.Trim().Equals("TOTAL LETRAS VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "TOTAL LETRAS VOUCHER";

            if (rgb.strC1.Trim().Equals("NOMBRE VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "Nombre vocher";
            if (rgb.strC1.Trim().Equals("BANCO VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "Banco voucher";
            //if (rgb.strC1.Trim().Equals("CUENTA VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "cuenta voucher";
            if (rgb.strC1.Trim().Equals("CONCEPTO VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "concepto voucher";
            if (rgb.strC1.Trim().Equals("TOTAL VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "total voucher";
            if (rgb.strC1.Trim().Equals("HECHO POR VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "hecho por";
            if (rgb.strC1.Trim().Equals("TOTAL CARGO")) lb1.Text = rgb.strC2.Trim() + "100.00";
            if (rgb.strC1.Trim().Equals("TOTAL ABONO")) lb1.Text = rgb.strC2.Trim() + "100.00";

            if (rgb.strC1.Trim().Equals("CUENTA BANCARIA VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "000-00000-0";
            if (rgb.strC1.Trim().Equals("REFERENCIA")) lb1.Text = rgb.strC2.Trim() + " " + "referencia";
            if (rgb.strC1.Trim().Equals("TIPO CAMBIO")) lb1.Text = rgb.strC2.Trim() + " " + "8.25";
            if (rgb.strC1.Trim().Equals("FECHA IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + "01-01-2010";
            if (rgb.strC1.Trim().Equals("OBSERVACIONES")) lb1.Text = rgb.strC2.Trim() + " " + "Observaciones varias";
            if (rgb.strC1.Trim().Equals("NO CHEQUE VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "44";

            if (rgb.strC1.Trim().Equals("ID CUENTA CONTABLE VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "1.1.1.1.00000";
            if (rgb.strC1.Trim().Equals("NOMBRE CUENTA CONTABLE VOUCHER")) lb1.Text = rgb.strC2.Trim() + " " + "Bancos";
            if (rgb.strC1.Trim().Equals("ABONO")) lb1.Text = rgb.strC2.Trim() + " " + "abono";
            if (rgb.strC1.Trim().Equals("CARGO")) lb1.Text = rgb.strC2.Trim() + " " + "cargo";
            

            lb1.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            if (font_interlineado.strC2!=null && !font_interlineado.strC2.Equals("")) lb1.Font.Name = font_interlineado.strC2;
            if (font_interlineado.intC7 > 0) lb1.Font.Size = font_interlineado.intC7;
            Page.Controls.Add(lb1);

        }
    }
}
