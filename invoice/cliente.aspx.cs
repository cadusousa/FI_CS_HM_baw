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

public partial class invoice_cliente : System.Web.UI.Page
{
    double cliID = 0;
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        string criterio = "";
        ArrayList clientearr = null;
        RE_GenericBean clienteBean = null;
        if (!Page.IsPostBack) {
            //Obtengo el codigo del cliente
            if (Request.QueryString["cliID"] != null)
            {
                cliID = double.Parse(Request.QueryString["cliID"].Trim());
                //Obtengo los datos del cliente
                criterio = "a.id_cliente=" + cliID;
                clientearr = (ArrayList)DB.getClientes(criterio, user, "");
                if ((clientearr != null) && (clientearr.Count > 0))
                {
                    // si entro aqui es porque encontre datos del cliente
                    clienteBean = (RE_GenericBean)clientearr[0];
                    tb_clientid.Text = clienteBean.douC1.ToString();
                    tb_clientname.Text = clienteBean.strC1;
                    //Obtengo el arreglo de facturas pendientes de este cliente
                    DataSet ds = (DataSet)DB.getFacturasbyCliente(cliID, true, 1, user);//false indica que solo hala las que no estan pagadas ni anuladas
                    dgw_aplicada.DataSource = ds.Tables["fact"];
                    dgw_aplicada.DataBind();
                }
            }
        }
    }


    protected void dgw_aplicada_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Generar Nota Credito")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (dgw_aplicada.Rows[index].Cells[1].Text.Equals("54"))
                dgw_aplicada.Rows[index].Cells[1].Visible = false;
            Response.Redirect("viewinvoice.aspx?factID=" + dgw_aplicada.Rows[index].Cells[1].Text);
        }
    }
}
