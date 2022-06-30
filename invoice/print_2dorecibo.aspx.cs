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

public partial class invoice_print_2dorecibo : System.Web.UI.Page
{
    public string nombre, recibo;
    public decimal totalrecibo, totalpagado, saldo;
    public ArrayList facturas = new ArrayList();
    public string listado_facturas = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["rcptID"] != null)
        {
            int rcpt = int.Parse(Request.QueryString["rcptID"].ToString().Trim());
            RE_GenericBean rgb = (RE_GenericBean)DB.getRcptData(rcpt); //obtengo los datos del recibo
            rgb.decC1 = DB.getTotalAbonadoXRcpt(rcpt, 2);//obtengo todo lo abonado por el recibo tipo2 porque es recibo
            // Obtengo el arreglo de facturas que el recibo mato o aplico
            DataTable dt = (DataTable)DB.getRcptFacturas(rcpt);
            RE_GenericBean datos = null;
            RE_GenericBean cliente = (RE_GenericBean)DB.getDataClient(rgb.douC1);
            nombre = cliente.strC2;
            recibo=rgb.strC1;
            totalrecibo = Math.Round(rgb.decC3, 2);
            totalpagado = Math.Round(rgb.decC1, 2);
            saldo = Math.Round(totalrecibo - totalpagado, 2);
            DataRow row = null;
            #region Dibujar Facturas
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt.Rows[i];
                listado_facturas +="<tr>";
                listado_facturas +="<td>Factura No "+ row[4].ToString() + "" + row[5].ToString()+"</td>";
                listado_facturas += "<td align='right'>" + row[6].ToString()+ "</td>";
                listado_facturas +="</tr>";
                datos = new RE_GenericBean();
                datos.strC1 = row[1].ToString();
                datos.decC1 = decimal.Parse(row[6].ToString());
                facturas.Add(datos);
            }
            #endregion
            #region Dibujar ND
            DataTable dt_nd = (DataTable)DB.getRcptNotaDebito(rcpt);
            for ( int i=0; i < dt_nd.Rows.Count; i ++ )
            {
                row = dt_nd.Rows[i];
                listado_facturas += "<tr>";
                listado_facturas += "<td>Nota Debito No " + row[4].ToString() + "" + row[5].ToString() + "</td>";
                listado_facturas += "<td align='right'>" + row[6].ToString() + "</td>";
                listado_facturas += "</tr>";
                datos = new RE_GenericBean();
                datos.strC1 = row[1].ToString();
                datos.decC1 = decimal.Parse(row[6].ToString());
                facturas.Add(datos);
            }
            #endregion
        }
    }
}
