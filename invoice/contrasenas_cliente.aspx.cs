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

public partial class invoice_contrasenas_cliente : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
    }

    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        if (tb_agenteID.Text.Trim() == "" || tb_agentenombre.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe indicar el nombre del Cliente");
            return;
        }
        
        user = (UsuarioBean)Session["usuario"];
        int clienteID = int.Parse(tb_agenteID.Text);
        //obtengo el listado de facturas pendientes
        ArrayList Arr = null;
        Arr = DB.getFacturasContrasena(clienteID, int.Parse(lb_tipopersona.SelectedValue), user);
        dt = (DataTable)Utility.fillGridView("DocumentosContrasena", Arr);
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
        
    }

    protected void bt_generar_Click(object sender, EventArgs e)
    {
        if (tb_nocontrasena.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe indicar el número de contraseña");
            return;
        }
        if (tb_fecha_pago.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe indicar la fecha de pago.");
            return;
        }

        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean cortebean = new RE_GenericBean();
        RE_GenericBean rgb = null;
        ArrayList arrctas = null;
        GridViewRow row = null;
        CheckBox chk;

        //*Recorro el grid de facturas
        for (int i = 0; i < gv_detalle.Rows.Count; i++)
        {
            row = gv_detalle.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("CheckBox1");
                if (chk.Checked)
                {
                    rgb = new RE_GenericBean();
                    rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la factura
                    rgb.strC3 = row.Cells[2].Text;//Tipo de Documento
                    rgb.strC1 = row.Cells[3].Text;//No del documento factura, nota credito, nota debito, etc
                    rgb.strC2 = row.Cells[4].Text;//Fecha del documento
                    rgb.decC1 = decimal.Parse(row.Cells[8].Text);//valor documento
                    rgb.decC2 = decimal.Parse(row.Cells[10].Text);//volor equivalente documento
                    rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[9].Text);//Moneda
                    if (arrctas == null) arrctas = new ArrayList();
                    arrctas.Add(rgb);
                }
            }
        }
        row = null;

        cortebean.strC1 = tb_nocontrasena.Text;//No contrasena
        cortebean.strC2 = tb_fecha_pago.Text;//Fecha de pago
        cortebean.intC1 = int.Parse(tb_agenteID.Text);//Codigo Cliente
        cortebean.intC2 = int.Parse(lb_tipopersona.SelectedValue);//Tipo Persona
        cortebean.strC3 = tb_observaciones.Text;//Observaciones
        cortebean.arr1 = arrctas;


        int result = DB.GenerarContrasenaCliente(cortebean, user);
        if (result == -100 || result == 0)
        {
            WebMsgBox.Show("Existió un problema al momento de guardar la información, por favor intente de nuevo");
            return;
        }
        else
        {
           
            WebMsgBox.Show("La contraseña fue generada éxitosamente");
            //cargo_pendliquidar_y_cortes();
            gv_detalle.Enabled = false;
            bt_generar.Enabled = false;
            btn_buscar.Enabled = false;
            lb_tipopersona.Enabled = false;
            tb_agenteID.Enabled = false;
        }

    }

    protected void gv_detalle_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
        }

    }

    protected void gv_cortes_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;
    }

    protected void gv_cortes_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        if (lb_tipopersona.SelectedValue.Equals("3"))
        { // cliente
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(a.nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and a.id_cliente=" + tb_codigo.Text; else where += "a.id_cliente=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and a.codigo_tributario='" + tb_nitb.Text + "'"; else where += "a.codigo_tributario='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getClientes(where, user, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
        }
        

        ViewState["proveedordt"] = dt;
        gv_proveedor.DataSource = dt;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }

    protected void gv_proveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["proveedordt"];
        gv_proveedor.DataSource = dt1;
        gv_proveedor.PageIndex = e.NewPageIndex;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }

    protected void gv_proveedor_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_proveedor.SelectedRow;
        if (lb_tipopersona.SelectedValue.Equals("3")) //Cliente
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        }
        
        //cargo_pendliquidar_y_cortes();
        gv_detalle.DataBind();
    }

    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
            ArrayList Arr = null;
            string where = "";
        }
    }
    
}