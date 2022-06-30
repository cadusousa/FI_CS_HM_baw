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

public partial class manager_unificador : System.Web.UI.Page
{
    UsuarioBean user;
    DataTable dt;
    bool conPermisoTodosPaises = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
            Response.Redirect("../Default.aspx");
        else
        {
            user = (UsuarioBean)Session["usuario"];
            conPermisoTodosPaises = PermisoParaUnificarCodigoTodosPaises();
        }
    }

    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
            Response.Redirect("../Default.aspx");

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
            Arr = (ArrayList)DB.getClientes(
                    string.Concat(where, conPermisoTodosPaises ? " and a.id_estatus in(1,2) " : string.Empty), user, ""); //Clientes
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(
                string.Concat(where, conPermisoTodosPaises ? " and status in (0,1,2) " : string.Empty), ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit a un agente");
                return;
            }

            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(
                string.Concat(where, conPermisoTodosPaises ? " and activo in(true, false) " : string.Empty), ""); //Agente
            dt = (DataTable)Utility.fillGridView("Agente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una naviera");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_naviera=" + tb_codigo.Text; else where += "id_naviera=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getNavieras(
                string.Concat(where, conPermisoTodosPaises ? " and activo in(true, false) " : string.Empty), ""); //Naviera
            dt = (DataTable)Utility.fillGridView("Naviera", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una línea aerea");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(name)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and carrier_id=" + tb_codigo.Text; else where += " and carrier_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getCarriers(where); //Lineas aereas
            dt = (DataTable)Utility.fillGridView("LineasAereas", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit usuario de caja chica");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo.Text; else where += " and id_usuario=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);
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
        if (lb_tipopersona.SelectedValue.Equals("4")) //proveedor
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            int proveedorID = int.Parse(tb_agenteID.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);

        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            int proveedorID = int.Parse(tb_agenteID.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = "";
            int proveedorID = int.Parse(tb_agenteID.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);

        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = "";
            int proveedorID = int.Parse(tb_agenteID.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);

        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = "";
            int proveedorID = int.Parse(tb_agenteID.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);

        }
        else if (lb_tipopersona.SelectedValue.Equals("3"))//Clientes
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("0"))//Retenciones
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion.Text = "";
            int proveedorID = int.Parse(tb_agenteID.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);

        }
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


    protected void bt_buscar2_Click(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
            Response.Redirect("../Default.aspx");

        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";

        if (lb_tipopersona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " and upper(rtrim(nombre)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2"))
        {
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " and upper(rtrim(agente)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            Arr = (ArrayList)DB.getAgente(where,""); //Agente
            dt = (DataTable)Utility.fillGridView("Agente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))
        {
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " and upper(rtrim(nombre)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            Arr = (ArrayList)DB.getNavieras(where, ""); //Naviera
            dt = (DataTable)Utility.fillGridView("Naviera", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))
        {
            Arr = (ArrayList)DB.getAgente(where,""); //Lineas aereas
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))
        {
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " and upper(rtrim(tpcc_nombre)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Lineas aereas
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("3"))
        {
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " upper(rtrim(a.nombre_cliente)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            Arr = (ArrayList)DB.getClientes(where, user, ""); //cliente
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("0"))
        {
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " and upper(rtrim(nombre)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }

        ViewState["proveedordt"] = dt;
        gv_proveedor2.DataSource = dt;
        gv_proveedor2.DataBind();
        mpeSeleccion2.Show();
    }
    protected void gv_proveedor2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["proveedordt"];
        gv_proveedor2.DataSource = dt1;
        gv_proveedor2.PageIndex = e.NewPageIndex;
        gv_proveedor2.DataBind();
        mpeSeleccion.Show();
    }
    protected void gv_proveedor2_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_proveedor2.SelectedRow;
        if (lb_tipopersona.SelectedValue.Equals("4")) //proveedor
        {
            tb_agenteID2.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre2.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion2.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            int proveedorID = int.Parse(tb_agenteID2.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            tb_agenteID2.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre2.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion2.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            int proveedorID = int.Parse(tb_agenteID2.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);

        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_agenteID2.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre2.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion2.Text = "";
            int proveedorID = int.Parse(tb_agenteID2.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);

        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tb_agenteID2.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre2.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion2.Text = "";
            int proveedorID = int.Parse(tb_agenteID2.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);


        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_agenteID2.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre2.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion2.Text = "";
            int proveedorID = int.Parse(tb_agenteID2.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);

        }
        else if (lb_tipopersona.SelectedValue.Equals("3"))//Clientes
        {
            tb_agenteID2.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre2.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion2.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            int proveedorID = int.Parse(tb_agenteID2.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
        }
        else if (lb_tipopersona.SelectedValue.Equals("0"))//Retenciones
        {
            tb_agenteID2.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre2.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion2.Text = "";
            int proveedorID = int.Parse(tb_agenteID2.Text);
            int proveedortype = int.Parse(lb_tipopersona.SelectedValue);
        }
    }
    protected void gv_proveedor2_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            user = (UsuarioBean)Session["usuario"];
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
            Response.Redirect("../Default.aspx");

        user = (UsuarioBean)Session["usuario"];

        if (!string.IsNullOrEmpty(tb_agenteID.Text))
        {
            if (!string.IsNullOrEmpty(tb_agenteID2.Text))
            {
                int tipopersona = int.Parse(lb_tipopersona.SelectedValue);
                int origen = int.Parse(tb_agenteID.Text);
                int destino = int.Parse(tb_agenteID2.Text);
                int estado = 0;
                string Msg = "";

                //actualizo el id_vendedor en la master.
                if (lb_tipopersona.SelectedValue.Equals("3"))
                {
                    DB.UnificarVendedorMaster(origen, destino, tipopersona, user.PaisID);
                    //actualizo en WMS
                    int resultado = DB.UnificarWMS(origen, destino);
                }
                //Actualizando las Partidas
                DB.changeLibroDiario(origen, destino, tipopersona, user.PaisID, conPermisoTodosPaises);

                //Actualizando los documentos de BAW
                var msgPaisesBAW = string.Empty;
                int result = DB.changeDocsBAW(origen, destino, tipopersona, user.PaisID, conPermisoTodosPaises, out msgPaisesBAW);
                if (result == 0)
                    Msg = "No habian documentos en Contabilidad con ese codigo";

                //Actualizando los documentos en Traficos Maritimo, Aereo y Terrestre
                var msgPaisesTrafico = string.Empty;
                result = DB.changeDocsTraffic(origen, destino, tipopersona, user, conPermisoTodosPaises, out msgPaisesTrafico);

                if (result > 0)
                {
                    Msg = string.Concat("Se cambio el codigo en Contabilidad en las siguientes empresa(s):"
                                        , "\\n", msgPaisesBAW, "\\n"
                                        , "Tambien se cambio en Tráfico en las siguientes empresa(s):"
                                        , "\\n", msgPaisesTrafico, "\\n");
                }
                else
                {
                    Msg = string.Concat("Se cambio el codigo en Contabilidad en las siguientes empresa(s):"
                                        , "\\n", msgPaisesBAW, "\\n"
                                        , "No habian documentos en Trafico con ese codigo", "\\n");
                }
                if (result >= 0)
                {
                    //Desactivando el ID del Cliente/Proveedor
                    if (desactivar.Checked == true)
                    {
                        string check = DB.CheckToInactivate(origen, tipopersona, user);
                        if (check.Equals(""))
                        {
                            estado = 1; //inactivo para guardar en el log del unificador
                            if (tipopersona == 4) DB.updateStatusProveedor(origen);
                            else if (tipopersona == 2) DB.updateStatusAgente(origen);
                            else if (tipopersona == 5) DB.updateStatusNaviera(origen);
                            else if (tipopersona == 6) DB.updateStatusLineasAeres(origen);
                            else if (tipopersona == 3)
                            {
                                DB.updateStatusCliente(origen);
                                DB.ActivarCliente(destino);
                            }
                            WebMsgBox.Show(string.Concat(Msg, "Y se desactivo el código en el catálogo."));
                        }
                        else if ((check.Equals("-1")) || (check.Equals("-1000")))
                        {
                            WebMsgBox.Show(string.Concat(Msg, ", Existio un inconveniente al momento de inactivar el Codigo"));
                        }
                        else
                        {
                            WebMsgBox.Show(string.Concat(Msg, "No se pudo desactivar el codigo en el catalogo porque esta siendo utilizado en:", "\\n"
                                                        , check, ", favor de ponerse en contacto con ellos."));
                        }
                    }
                    else
                    {
                        WebMsgBox.Show(Msg);
                    }
                    //Guardando Datos del usuario que realizo la Unificacion
                    DB.SaveUnificadorLog(origen, destino, user.ID, tipopersona, user.PaisID, estado);
                    Button1.Enabled = false;
                }
                else
                    WebMsgBox.Show("Existio un problema al momento de Guardar la información, codigo: " + result + "");
            }
            else
                WebMsgBox.Show("Ingrese un código de DESTINO");
        }
        else
            WebMsgBox.Show("Ingrese un código de ORIGEN");
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
            Response.Redirect("../Default.aspx");

        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo2.Text.Equals("") && tb_nombreb2.Text.Equals("") && tb_nitb2.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        if (lb_tipopersona.SelectedValue.Equals("3"))
        { // cliente
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " upper(rtrim(a.nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo2.Text.Trim().Equals("") && tb_codigo2.Text != null)
                if (!where.Equals("")) where += " and a.id_cliente=" + tb_codigo2.Text; else where += "a.id_cliente=" + tb_codigo2.Text;
            if (!tb_nitb2.Text.Trim().Equals("") && tb_nitb2.Text != null)
                if (!where.Equals("")) where += " and a.codigo_tributario='" + tb_nitb2.Text + "'"; else where += "a.codigo_tributario='" + tb_nitb2.Text + "'";
            Arr = (ArrayList)DB.getClientes(string.Concat(where, conPermisoTodosPaises ? " and a.id_estatus in(1,2) " : string.Empty), user, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo2.Text.Trim().Equals("") && tb_codigo2.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo2.Text; else where += "numero=" + tb_codigo2.Text;
            if (!tb_nitb2.Text.Trim().Equals("") && tb_nitb2.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb2.Text + "'"; else where += "nit='" + tb_nitb2.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2"))
        {
            if (!tb_nitb2.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit a un agente");
                return;
            }

            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " upper(rtrim(agente)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo2.Text.Trim().Equals("") && tb_codigo2.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo2.Text; else where += "agente_id=" + tb_codigo2.Text;
            Arr = (ArrayList)DB.getAgente(where,""); //Agente
            dt = (DataTable)Utility.fillGridView("Agente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))
        {
            if (!tb_nitb2.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una naviera");
                return;
            }
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo2.Text.Trim().Equals("") && tb_codigo2.Text != null)
                if (!where.Equals("")) where += " and id_naviera=" + tb_codigo2.Text; else where += "id_naviera=" + tb_codigo2.Text;
            Arr = (ArrayList)DB.getNavieras(where,""); //Naviera
            dt = (DataTable)Utility.fillGridView("Naviera", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))
        {
            if (!tb_nitb2.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una línea aerea");
                return;
            }
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " and upper(rtrim(name)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo2.Text.Trim().Equals("") && tb_codigo2.Text != null)
                if (!where.Equals("")) where += " and carrier_id=" + tb_codigo2.Text; else where += " and carrier_id=" + tb_codigo2.Text;
            Arr = (ArrayList)DB.getCarriers(where); //Lineas aereas
            dt = (DataTable)Utility.fillGridView("LineasAereas", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))
        {
            if (!tb_nitb2.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit usuario de caja chica");
                return;
            }
            if (!tb_nombreb2.Text.Trim().Equals("") && tb_nombreb2.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb2.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo2.Text.Trim().Equals("") && tb_codigo2.Text != null)
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo2.Text; else where += " and id_usuario=" + tb_codigo2.Text;
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);
        }

        ViewState["proveedordt"] = dt;
        gv_proveedor2.DataSource = dt;
        gv_proveedor2.DataBind();
        mpeSeleccion2.Show();
    }

    protected void lb_tipopersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Limpiar Tipo de Persona
        tb_agenteID.Text = "";
        tb_agentenombre.Text = "";
        tb_direccion.Text = "";
        desactivar.Checked = false; 
        tb_agenteID2.Text = "";
        tb_agentenombre2.Text = "";
        tb_direccion2.Text = "";
        gv_proveedor.DataBind();
        gv_proveedor2.DataBind();
        #endregion
    }

    private bool PermisoParaUnificarCodigoTodosPaises()
    {
        var resultado = false;

        var Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        Arr_Perfiles.Cast<PerfilesBean>().ToList().ForEach(i =>
        {
            if (i.ID == 36)
            {
                lbl_informacion.Text = "Atención: usted tiene activado el permiso para unificar códigos en todos los paises";
                resultado = true;
            }
        });
        if (!resultado)
            this.lbl_informacion.Text = string.Empty;

        return resultado;
    }
}

