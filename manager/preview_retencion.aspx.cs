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
    int pais=0;
    string serie = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt=null;
        RE_GenericBean datosfactura=null;
        RE_GenericBean font_interlineado = null;
        if (Request.QueryString["serie"] != null) {
            pais = int.Parse(Request.QueryString["pais"].ToString());
            serie = Request.QueryString["serie"].ToString();
        }
        user = (UsuarioBean)Session["usuario"];
        int doc_id = DB.getDocumento_Retencion(serie, pais);
        font_interlineado=DB.getParametrosRetencion(doc_id);
        ArrayList camposfact = (ArrayList)DB.getCampos_Retencion(doc_id);//parametros 1=factura (tipo doc); 2=sucursal de la factura; 3=serie de factura
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
            if (rgb.strC1.Trim().Equals("CODIGO CLIENTE")) lb1.Text = rgb.strC2.Trim() + " 123456";
            if (rgb.strC1.Trim().Equals("GIRO")) lb1.Text = rgb.strC2.Trim() + " NAVIERA ";

            if (rgb.strC1.Trim().StartsWith("NOMBRE_")) lb1.Text = rgb.strC2.Trim() + "Retencion tipo X%";
            if (rgb.strC1.Trim().StartsWith("MONTO_")) lb1.Text = rgb.strC2.Trim() + "$100.00";
            if (rgb.strC1.Trim().StartsWith("MONTO EN  LETRAS_")) lb1.Text = rgb.strC2.Trim() + "CIEN EXACTOS";
            

            lb1.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            if (font_interlineado.strC2!=null && !font_interlineado.strC2.Equals("")) lb1.Font.Name = font_interlineado.strC2;
            if (font_interlineado.intC7 != null && font_interlineado.intC7 > 0) lb1.Font.Size = font_interlineado.intC7;
            Page.Controls.Add(lb1);

        }
    }
}
