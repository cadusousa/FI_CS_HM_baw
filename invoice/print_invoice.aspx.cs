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
    int fac_id = 0, tipo=1;
    int xnombre_ini = 0, ynombre_ini=0;
    int xsubt_ini = 0, ysubt_ini = 0;
    int ximp_ini = 0, yimp_ini = 0;
    int xtot_ini = 0, ytot_ini = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["fac_id"] != null) {
            fac_id = int.Parse(Request.QueryString["fac_id"].ToString());
        }
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean datosfactura = (RE_GenericBean)DB.getFacturaData(fac_id);
        DataTable dt=(DataTable)DB.getRubbyFact(fac_id, 1);
        ArrayList camposfact = (ArrayList)DB.getCamposDoc(1, datosfactura.intC2, datosfactura.strC28);//parametros 1=factura (tipo doc); 2=sucursal de la factura; 3=serie de factura
        foreach (RE_GenericBean rgb in camposfact) {
            if (rgb.strC1.Trim().Equals("FECHA EMISION"))
            {
                lbfecha.Text = datosfactura.strC5;
                lbfecha.Attributes.Add("Style","left: "+rgb.intC1+"px; top: "+rgb.intC2+"px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("NOMBRE"))
            {
                lbnombre.Text = datosfactura.strC3;
                lbnombre.Attributes.Add("Style","left: "+rgb.intC1+"px; top: "+rgb.intC2+"px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("DIRECCION"))
            {
                lbdireccion.Text = datosfactura.strC4;
                lbdireccion.Attributes.Add("Style","left: "+rgb.intC1+"px; top: "+rgb.intC2+"px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("NIT"))
            {
                lbnit.Text = datosfactura.strC2;
                lbnit.Attributes.Add("Style","left: "+rgb.intC1+"px; top: "+rgb.intC2+"px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("SUB TOTAL"))
            {
                lbsubtotal.Text = datosfactura.decC1.ToString("#,#.00#;(#,#.00#)");
                lbsubtotal.Attributes.Add("Style","left: "+rgb.intC1+"px; top: "+rgb.intC2+"px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("IMPUESTO"))
            {
                lbimpuesto.Text = datosfactura.decC2.ToString("#,#.00#;(#,#.00#)");
                lbimpuesto.Attributes.Add("Style","left: "+rgb.intC1+"px; top: "+rgb.intC2+"px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("TOTAL"))
            {
                lbtotal.Text = datosfactura.decC3.ToString("#,#.00#;(#,#.00#)");
                lbtotal.Attributes.Add("Style","left: "+rgb.intC1+"px; top: "+rgb.intC2+"px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("FECHA PAGO"))
            {
                lbfechapago.Text = datosfactura.strC6;
                lbfechapago.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("OBSERVACIONES"))
            {
                lbobs.Text = datosfactura.strC7;
                lbobs.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("RAZON"))
            {
                lbrazon.Text = datosfactura.strC26;
                lbrazon.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("MONEDA"))
            {
                lbmoneda.Text = datosfactura.intC4.ToString();
                lbmoneda.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("USUARIO"))
            {
                lbusuario.Text = datosfactura.strC8;
                lbusuario.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("HBL"))
            {
                lbhbl.Text = datosfactura.strC9;
                lbhbl.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("MBL"))
            {
                lbmbl.Text = datosfactura.strC10;
                lbmbl.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("CONTENEDOR"))
            {
                lbcontenedor.Text = datosfactura.strC11;
                lbcontenedor.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("ROUTING"))
            {
                lbrouting.Text = datosfactura.strC12;
                lbrouting.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("NAVIERA"))
            {
                lbnaviera.Text = datosfactura.strC13;
                lbnaviera.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("VAPOR"))
            {
                lbvapor.Text = datosfactura.strC14;
                lbvapor.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("SHIPPER"))
            {
                lbshipper.Text = datosfactura.strC15;
                lbshipper.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("ORDEN/PO"))
            {
                lbordenpo.Text = datosfactura.strC16;
                lbordenpo.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("CONSIGNATARIO"))
            {
                lbconsignee.Text = datosfactura.strC17;
                lbconsignee.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("COMODITY"))
            {
                lbcomodity.Text = datosfactura.strC18;
                lbcomodity.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("PAQUETES"))
            {
                lbpaquetes.Text = datosfactura.strC19;
                lbpaquetes.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("PESO"))
            {
                lbpeso.Text = datosfactura.strC20;
                lbpeso.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("VOLUMEN"))
            {
                lbvolumen.Text = datosfactura.strC21;
                lbvolumen.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("DUA INGRESO"))
            {
                lbduaingreso.Text = datosfactura.strC22;
                lbduaingreso.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("DUA SALIDA"))
            {
                lbduasalida.Text = datosfactura.strC23;
                lbduasalida.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("VENDEDOR 1"))
            {
                lbvendedor1.Text = datosfactura.strC24;
                lbvendedor1.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("VENDEDOR 2"))
            {
                lbvendedor2.Text = datosfactura.strC25;
                lbvendedor2.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("REFERENCIA"))
            {
                lbreferencia.Text = datosfactura.strC27;
                lbreferencia.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("SERIE"))
            {
                lbserie.Text = datosfactura.strC28;
                lbserie.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            }
            if (rgb.strC1.Trim().Equals("RUBRO_NOMBRE"))
            {
                xnombre_ini = rgb.intC1;
                ynombre_ini = rgb.intC2;                
            }
            if (rgb.strC1.Trim().Equals("RUBRO_SUBTOTAL"))
            {
                xsubt_ini = rgb.intC1;
                ysubt_ini = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("RUBRO_IMPUESTO"))
            {
                ximp_ini = rgb.intC1;
                yimp_ini = rgb.intC2;
            }
            if (rgb.strC1.Trim().Equals("RUBRO_TOTAL"))
            {
                xtot_ini = rgb.intC1;
                ytot_ini = rgb.intC2;
            }
        }
        Label lb = null;
        foreach (DataRow dr in dt.Rows) {
            // dibujo el nombre del rubro
            lb = new Label();
            lb.Text=dr[0].ToString();
            lb.Attributes.Add("Style", "left: " + xnombre_ini + "px; top: " + ynombre_ini + "px; position: absolute;");
            ynombre_ini+=18;
            Page.Controls.Add(lb);
            //dibujo el subtotal
            lb = new Label();
            lb.Text = dr[1].ToString();
            lb.Attributes.Add("Style", "left: " + xsubt_ini + "px; top: " + ysubt_ini + "px; position: absolute;");
            ysubt_ini+=18;
            Page.Controls.Add(lb);
            //dibujo el impuesto
            lb = new Label();
            lb.Text = dr[2].ToString();
            lb.Attributes.Add("Style", "left: " + ximp_ini + "px; top: " + yimp_ini + "px; position: absolute;");
            yimp_ini += 18;
            Page.Controls.Add(lb);
            //dibujo el Total
            lb = new Label();
            lb.Text = dr[3].ToString();
            lb.Attributes.Add("Style", "left: " + xtot_ini + "px; top: " + ytot_ini + "px; position: absolute;");
            ytot_ini += 18;
            Page.Controls.Add(lb);
        }
    }
}
