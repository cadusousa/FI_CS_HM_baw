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
using System.Xml;
using System.IO;

public partial class invoice_invoice : System.Web.UI.Page
{
    UsuarioBean user;
    DataTable dt1;
    long agente_id = 0;// Codigo del agente
    public string simboloequivalente = "USD";
    public string simbolomoneda = "GTQ";
    RE_GenericBean Provision = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        lb_fecha_hora.Text = DB.getDateTimeNow();
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        if (!user.Aplicaciones.Contains("5"))
        {
            Response.Redirect("index.aspx");
        }
        int permiso = int.Parse(user.Aplicaciones["5"].ToString());
        if (!((permiso & 1) == 1) && !((permiso & 2) == 2) && !((permiso & 4) == 4) && !((permiso & 8) == 8))
        {
            Response.Redirect("index.aspx");
        }
        //Obtengo el codigo del cliente
        double cliID = 0;
        string criterio = "";
        ArrayList clientearr = null;
        RE_GenericBean clienteBean = null;
        if (Request.QueryString["cliID"] != null)
        {
            cliID = double.Parse(Request.QueryString["cliID"].Trim());
            //Obtengo los datos del cliente
            criterio = "id_cliente=" + cliID;
            clientearr = (ArrayList)DB.getClientes(criterio, user, "");
            if ((clientearr != null) && (clientearr.Count > 0))
            {
                // si entro aqui es porque encontre datos del cliente
                clienteBean = (RE_GenericBean)clientearr[0];
                tbCliCod.Text = clienteBean.douC1.ToString();
                tb_razon.Text = clienteBean.strC1;
                tb_nombre.Text = clienteBean.strC2;
                tb_nit.Text = clienteBean.strC3;
                tb_direccion.Text = clienteBean.strC4;
                lb_contribuyente.SelectedValue = (clienteBean.intC1).ToString();
                lb_requierealias.Text = clienteBean.intC2.ToString();
            }
        }
        if (!Page.IsPostBack)
        {
            int impo_expo = 0;
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            obtengo_listas(tipo_contabilidad, impo_expo, user.PaisID);
            if (tipo_contabilidad == 2)
            {
                lb_moneda.SelectedValue = user.Moneda.ToString();
                lb_tipo_transaccion.SelectedValue = "7";
                lb_tipo_transaccion.Enabled = false;
            }
            else
            {
                #region Backup
                //lb_moneda.SelectedValue = user.pais.ID.ToString();
                #endregion
                lb_moneda.SelectedValue = user.Moneda.ToString();
                lb_tipo_transaccion.SelectedValue = "1";
                lb_tipo_transaccion.Enabled = false;
            }
            if (lb_imp_exp.Items.Count > 0)
                impo_expo = int.Parse(lb_imp_exp.SelectedValue);

            cargo_datos_BL(impo_expo);
        }
        if (lb_serie_factura2.Items.Count > 0)
        {
            Determinar_Tipo_Serie(user.SucursalID, lb_serie_factura2.SelectedItem.Text);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        int factID = 0;
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["factID"] != null && !Request.QueryString["factID"].ToString().Equals(""))
            {
                factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                //Obtengo los rubros de la factura
                RE_GenericBean factdata = (RE_GenericBean)DB.getFacturaData_proforma(factID);
                tb_agente_nombre.Text = factdata.strC41;//Agente
                tb_corr_proforma.Text = factdata.strC1;
                tb_nit.Text=factdata.strC2;
                tb_nombre.Text=factdata.strC3;
                tb_allin.Text=factdata.strC30;
                tb_direccion.Text=factdata.strC4;
                tb_reciboaduanal.Text=factdata.strC31;
                tb_subtotal.Text=factdata.decC1.ToString();
                tb_impuesto.Text=factdata.decC2.ToString();
                tb_total.Text=factdata.decC3.ToString();
                tb_observaciones.Text=factdata.strC7;
                tbCliCod.Text=factdata.intC3.ToString();
                tb_hbl.Text=factdata.strC9;
                tb_mbl.Text=factdata.strC10;
                tb_contenedor.Text=factdata.strC11;
                tb_routing.Text=factdata.strC12;
                tb_naviera.Text=factdata.strC13;
                tb_vapor.Text = factdata.strC14;
                tb_shipper.Text = factdata.strC15;
                tb_orden.Text = factdata.strC16;
                tb_consignee.Text = factdata.strC17;
                tb_comodity.Text = factdata.strC18;
                tb_paquetes1.Text = factdata.strC32;
                tb_paquetes2.Text = factdata.strC19;
                tb_peso.Text=factdata.strC20;
                tb_vol.Text=factdata.strC21;
                tb_dua_ingreso.Text=factdata.strC22;
                tb_dua_salida.Text = factdata.strC23;
                tb_vendedor1.Text=factdata.strC24;
                tb_vendedor2.Text=factdata.strC25;
                tb_razon.Text = factdata.strC26;

                gv_detalle.Columns[5].HeaderText = "Subtotal en " + simbolomoneda;
                gv_detalle.Columns[6].HeaderText = "Impuesto en " + simbolomoneda;
                gv_detalle.Columns[7].HeaderText = "Total en " + simbolomoneda;
                gv_detalle.Columns[8].HeaderText = "Equivalente en " + simboloequivalente;
                gv_detalle.DataSource = (DataTable)DB.getRubbyFact_proforma(factID, 14);
                gv_detalle.DataBind();
                if (factdata.intC5 != 1)
                {
                    bt_Enviar.Enabled = false;
                }
                tb_otras_observaciones.Text = factdata.strC47;
                tb_recibo_agencia.Text = factdata.strC42;
                tb_valor_aduanero.Text = factdata.strC43;
                drp_regimen_aduanero.SelectedValue = factdata.intC10.ToString();
                lbl_blID.Text = factdata.intC11.ToString();
                lbl_tipoOperacionID.Text = factdata.intC12.ToString();
                lbl_correo_documento_electronico.Text = factdata.strC48;
                tb_referencia_correo.Text = factdata.strC49;
                tb_otras_observaciones.Text = factdata.strC47;
                RE_GenericBean Data_Cliente = DB.getDataClient(Convert.ToDouble(tbCliCod.Text));
                lb_contribuyente.SelectedValue = Data_Cliente.intC1.ToString();
                id_shipper.Text = factdata.intC14.ToString();
                id_consignee.Text = factdata.intC15.ToString();
                tb_agenteid.Text = factdata.intC13.ToString();
                lb_contribuyente.SelectedValue = factdata.strC50;
            }
        }
    }

    private void cargo_datos_BL(int impo_expo)
    {
        if ((Request.QueryString["bl_no"] != null) && (Request.QueryString["tipo"] != null))
        {
            FacturaBean factura = null;
            string sql, nombre;
            string bl_no = Request.QueryString["bl_no"].ToString();
            string tipo = Request.QueryString["tipo"].ToString();
            string blID = Request.QueryString["blid"].ToString();
            if (tipo.Equals("LCL"))
            {
                // obtengo los datos de la factura 
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataLCL(bl_no, user, blID);
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                agente_id = rgb.lonC6;
                // obtengo los datos del cliente
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID);
                if (rgb_cliente == null)
                {
                    WebMsgBox.Show("El cliente con codigo " + factura.CliID + " no se encuentra registrado para esta transaccion");
                    return;
                }
                if (rgb_cliente.intC1 == -100)
                {
                    WebMsgBox.Show("El cliente no tiene configurado su regimen tributario, por lo tanto no se puede realizar la factura");
                    return;
                }
                factura.Nombre = rgb_cliente.strC3;
                factura.Nit = rgb_cliente.strC1;
                factura.Razon = rgb_cliente.strC2;
                factura.Direccion = rgb_cliente.strC4;
                factura.Contribuyente = rgb_cliente.intC1;
                factura.alias_rubro = rgb.intC2;//si requiere alias los rubros
                // obtengo los datos del user
                sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC2;
                nombre = DB.getName(sql);
                tb_vendedor2.Text = nombre.Trim();
                // obtengo datos del shipper
                sql = "select nombre_cliente from clientes where id_cliente=" + rgb.lonC3;
                nombre = DB.getName(sql);
                tb_shipper.Text = nombre.Trim();
                //obtengo datos de naviera
                sql = "select nombre from navieras where activo=true and id_naviera=" + rgb.lonC5;
                nombre = DB.getName(sql);
                tb_naviera.Text = nombre.Trim();
                // obtendo datos de agente
                sql = "select agente from agentes where agente_id=" + rgb.lonC6;
                nombre = DB.getName(sql);
                //tb_vendedor2.Text = nombre.Trim();
                // obtengo datos de comodity
                sql = "select namees from commodities where commodityid =" + rgb.lonC7;
                nombre = DB.getName(sql);
                tb_peso.Text = rgb.decC1.ToString();
                tb_vol.Text = rgb.decC2.ToString();
                tb_comodity.Text = nombre.Trim();
                tb_vapor.Text = rgb.strC3;
                tb_agenteid.Text = rgb.lonC6.ToString();
                tb_paquetes1.Text = rgb.lonC8.ToString();
                tb_paquetes2.Text = (string)DB.GetNombreTipoBulto(rgb.lonC9);
                lb_contribuyente.SelectedValue = rgb_cliente.intC1.ToString();
                tb_hbl.Text = factura.HBL;
                tb_mbl.Text = factura.MBL;
                tb_routing.Text = factura.Routing;
                tb_contenedor.Text = factura.Contenedor;
                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL(factura.BLID, "L", impo_expo, user, 14, factura.CliID.ToString());//importacion=0 exportacion=1
                }
            }
            else if (tipo.Equals("FCL"))
            {
                // obtengo los datos de la factura 
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataFCL(bl_no, user, blID);
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;

                agente_id = rgb.lonC6;
                // obtengo los datos del cliente
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID);
                if (rgb_cliente == null)
                {
                    WebMsgBox.Show("El cliente con codigo " + factura.CliID + " no se encuentra registrado para esta transaccion");
                    return;
                }
                if (rgb_cliente.intC1 == -100)
                {
                    WebMsgBox.Show("El cliente no tiene configurado su regimen tributario, por lo tanto no se puede realizar la factura");
                    return;
                }
                factura.Nombre = rgb_cliente.strC3;
                factura.Nit = rgb_cliente.strC1;
                factura.Razon = rgb_cliente.strC2;
                factura.Direccion = rgb_cliente.strC4;
                factura.Contribuyente = rgb_cliente.intC1;
                factura.alias_rubro = rgb.intC2;//si requiere alias los rubros
                // obtengo los datos del user
                sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC2;
                nombre = DB.getName(sql);
                tb_vendedor2.Text = nombre.Trim();
                // obtengo datos del shipper
                sql = "select nombre_cliente from clientes where id_cliente=" + rgb.lonC3;
                nombre = DB.getName(sql);
                tb_shipper.Text = nombre.Trim();
                //obtengo datos de naviera
                sql = "select nombre from navieras where activo=true and id_naviera=" + rgb.lonC5;
                nombre = DB.getName(sql);
                tb_naviera.Text = nombre.Trim();
                // obtendo datos de agente
                sql = "select agente from agentes where agente_id=" + rgb.lonC6;
                nombre = DB.getName(sql);
                //tb_vendedor2.Text = nombre.Trim();
                // obtengo datos de comodity
                sql = "select namees from commodities where commodityid =" + rgb.lonC7;
                nombre = DB.getName(sql);
                tb_peso.Text = rgb.decC1.ToString();
                tb_vol.Text = rgb.decC2.ToString();
                tb_comodity.Text = nombre.Trim();
                tb_vapor.Text = rgb.strC3;
                tb_agenteid.Text = rgb.lonC6.ToString();
                tb_paquetes1.Text = rgb.lonC8.ToString();
                tb_paquetes2.Text = (string)DB.GetNombreTipoBulto(rgb.lonC9);
                lb_contribuyente.SelectedValue = rgb_cliente.intC1.ToString();

                tb_hbl.Text = factura.HBL;
                tb_mbl.Text = factura.MBL;
                tb_routing.Text = factura.Routing;
                tb_contenedor.Text = factura.Contenedor;

                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL(factura.BLID, "F", impo_expo, user, 14, factura.CliID.ToString());
                }
            }
            else if (tipo.Equals("ALMACENADORA"))
            {
                // obtengo los datos de la factura 
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataALMACENADORA(int.Parse(bl_no), user);
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                factura.MBL = rgb.strC1;
                factura.Contenedor = rgb.strC3;
                factura.CliID = rgb.lonC2;
                // obtengo los datos del cliente
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID);
                if (rgb_cliente == null)
                {
                    WebMsgBox.Show("El cliente con codigo " + factura.CliID + " no se encuentra registrado para esta transaccion");
                    return;
                }
                if (rgb_cliente.intC1 == -100)
                {
                    WebMsgBox.Show("El cliente no tiene configurado su regimen tributario, por lo tanto no se puede realizar la factura");
                    return;
                }
                factura.Nombre = rgb_cliente.strC3;
                factura.Nit = rgb_cliente.strC1;
                factura.Razon = rgb_cliente.strC2;
                factura.Direccion = rgb_cliente.strC4;
                factura.Contribuyente = rgb_cliente.intC1;
                factura.alias_rubro = rgb.intC2;//si requiere alias los rubros
                // obtengo los datos del user
                sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC2;
                nombre = DB.getName(sql);
                tb_vendedor2.Text = nombre.Trim();

                tb_peso.Text = rgb.decC1.ToString();
                tb_vol.Text = rgb.decC2.ToString();
                tb_paquetes1.Text = rgb.decC3.ToString();
                tb_paquetes2.Text = rgb.strC4;


                //tb_bl.Text = factura.HBL;
                tb_mbl.Text = factura.MBL;
                //tb_routing.Text = factura.Routing;
                tb_contenedor.Text = factura.Contenedor;
                lb_contribuyente.SelectedValue = rgb_cliente.intC1.ToString();

                lb_hbl.Text = "PICKING";
                lb_mbl.Text = "POLIZA";

                lb_contenedor.Visible = false;
                tb_contenedor.Visible = false;
                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyPICK(factura.BLID, "A", user, 14);
                }
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                double QS_BLID = 0;
                if (Request.QueryString["blid"] != null)
                {
                    QS_BLID = double.Parse(Request.QueryString["blid"].ToString());
                }
                // obtengo los datos de la factura 
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataTerrestre(QS_BLID, user);
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                agente_id = rgb.lonC6;
                int paisdest = Utility.ISOPaistoInt(rgb.strC5);

                if (paisdest != user.PaisID) impo_expo = 2; else impo_expo = 1;
                // obtengo los datos del cliente
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID);
                if (rgb_cliente == null)
                {
                    WebMsgBox.Show("El cliente con codigo " + factura.CliID + " no se encuentra registrado para esta transaccion");
                    return;
                }
                if (rgb_cliente.intC1 == -100)
                {
                    WebMsgBox.Show("El cliente no tiene configurado su regimen tributario, por lo tanto no se puede realizar la factura");
                    return;
                }
                factura.Nombre = rgb_cliente.strC3;
                factura.Nit = rgb_cliente.strC1;
                factura.Razon = rgb_cliente.strC2;
                factura.Direccion = rgb_cliente.strC4;
                factura.Contribuyente = rgb_cliente.intC1;
                factura.alias_rubro = rgb.intC2;//si requiere alias los rubros
                // obtengo los datos del user
                sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC2;
                nombre = DB.getName(sql);
                tb_vendedor2.Text = nombre.Trim();
                // obtengo datos del shipper
                sql = "select nombre_cliente from clientes where id_cliente=" + rgb.lonC3;
                nombre = DB.getName(sql);
                tb_shipper.Text = nombre.Trim();
                //obtengo datos de transportista
                sql = "select nombre from proveedores where numero=" + rgb.lonC5;
                nombre = DB.getName(sql);
                tb_naviera.Text = nombre.Trim();
                // obtendo datos de agente
                sql = "select agente from agentes where agente_id=" + rgb.lonC6;
                nombre = DB.getName(sql);
                //tb_vendedor2.Text = nombre.Trim();
                // obtengo datos de comodity
                sql = "select namees from commodities where commodityid =" + rgb.lonC7;
                nombre = DB.getName(sql);
                tb_peso.Text = rgb.decC1.ToString();
                tb_vol.Text = rgb.decC2.ToString();
                tb_comodity.Text = nombre.Trim();
                tb_vapor.Text = rgb.strC3;
                tb_agenteid.Text = rgb.lonC6.ToString();
                tb_paquetes1.Text = rgb.douC1.ToString();
                tb_paquetes2.Text = rgb.strC6;
                lb_contribuyente.SelectedValue = rgb_cliente.intC1.ToString();
                tb_hbl.Text = factura.HBL;
                tb_mbl.Text = factura.MBL;
                tb_routing.Text = factura.Routing;
                tb_contenedor.Text = factura.Contenedor;

                lb_hbl.Text = "CPH";
                lb_mbl.Text = "CP";
                lb_tipotranporte.Text = "TRANSPORTISTA:";
                lb_transporte.Text = "TRANSPORTE:";

                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyTerrestre(factura.BLID, impo_expo, user, 14);//importacion=0 exportacion=1
                }
            }
            else if (tipo.Equals("AEREO"))
            {
                // obtengo los datos de la factura 
                int Tipo_Guia = 0;
                if (Request.QueryString["opid"] != null)
                {
                    Tipo_Guia = int.Parse(Request.QueryString["opid"].ToString());
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataAereo(bl_no, impo_expo, Tipo_Guia, blID);
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                agente_id = rgb.lonC6;
                // obtengo los datos del cliente
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID);
                if (rgb_cliente == null)
                {
                    WebMsgBox.Show("El cliente con codigo " + factura.CliID + " no se encuentra registrado para esta transaccion");
                    return;
                }
                if (rgb_cliente.intC1 == -100)
                {
                    WebMsgBox.Show("El cliente no tiene configurado su regimen tributario, por lo tanto no se puede realizar la factura");
                    return;
                }
                factura.Nombre = rgb_cliente.strC3;
                factura.Nit = rgb_cliente.strC1;
                factura.Razon = rgb_cliente.strC2;
                factura.Direccion = rgb_cliente.strC4;
                factura.Contribuyente = rgb_cliente.intC1;
                factura.alias_rubro = rgb.intC2;//si requiere alias los rubros

                // obtengo los datos del user
                sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC2;
                nombre = DB.getName(sql);
                tb_vendedor2.Text = nombre.Trim();
                // obtengo datos del shipper
                sql = "select nombre_cliente from clientes where id_cliente=" + rgb.lonC3;
                nombre = DB.getName(sql);
                tb_shipper.Text = nombre.Trim();
                //obtengo datos de naviera
                sql = "select name from carriers where carrier_id=" + rgb.lonC5;
                nombre = DB.getName(sql);
                tb_naviera.Text = nombre.Trim();
                // obtendo datos de agente
                sql = "select agente from agentes where agente_id=" + rgb.lonC6;
                nombre = DB.getName(sql);
                //tb_vendedor2.Text = nombre.Trim();
                // obtengo datos de comodity
                sql = "select namees from commodities where commodityid =" + rgb.lonC7;
                nombre = DB.getName(sql);
                tb_peso.Text = rgb.decC1.ToString();
                tb_vol.Text = rgb.decC2.ToString();
                tb_comodity.Text = nombre.Trim();
                tb_vapor.Text = rgb.strC3;
                tb_paquetes1.Text = rgb.strC5;
                tb_paquetes2.Text = rgb.strC6;
                lb_contribuyente.SelectedValue = rgb_cliente.intC1.ToString();

                tb_hbl.Text = factura.HBL;
                tb_mbl.Text = factura.MBL;
                tb_routing.Text = factura.Routing;
                tb_contenedor.Text = factura.Contenedor;

                lb_hbl.Text = "HAWB";
                lb_mbl.Text = "MAWB";
                lb_tipotranporte.Text = "Linea aerea:";
                lb_transporte.Text = "TRANSPORTE:";

                tb_agenteid.Text = rgb.lonC6.ToString();
                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL_Aereo(factura.BLID, impo_expo, user, rgb, 14);//importacion=0 exportacion=1
                }
            }
            gv_detalle.Columns[5].HeaderText = "Subtotal en " + simbolomoneda;
            gv_detalle.Columns[6].HeaderText = "Impuesto en " + simbolomoneda;
            gv_detalle.Columns[7].HeaderText = "Total en " + simbolomoneda;
            gv_detalle.Columns[8].HeaderText = "Equivalente en " + simboloequivalente;
            gv_detalle.DataSource = (DataTable)llenoDataTable(factura.RubrosHT, factura.alias_rubro, factura.CliID);
            gv_detalle.DataBind();
            tb_nit.Text = factura.Nit;
            tb_nombre.Text = factura.Nombre;
            tb_razon.Text = factura.Razon;
            lb_contribuyente.SelectedValue = factura.Contribuyente.ToString();
            tb_direccion.Text = factura.Direccion;
            tbCliCod.Text = factura.CliID.ToString();
        }
    }

    private DataTable llenoDataTable(Hashtable ht, int requierealias, long cliID)
    {
        user = (UsuarioBean)Session["usuario"];
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Columns.Add("TOTALD");
        dt.Columns.Add("COMENTARIO");
        if (ht != null && ht.Count > 0)
        {
            ICollection valueColl = ht.Values;
            Rubros rubtemp = new Rubros();
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            foreach (Rubros rub in valueColl)
            {
                if ((rub.rubtoType.Equals("LCL") || rub.rubtoType.Equals("FCL")) && rub.rubroMoneda == 1)
                {
                    if (user.PaisID == 1) rub.rubroMoneda = 1;
                    if (user.PaisID == 2) rub.rubroMoneda = 2;
                    if (user.PaisID == 3) rub.rubroMoneda = 3;
                    if (user.PaisID == 4) rub.rubroMoneda = 4;
                    if (user.PaisID == 5) rub.rubroMoneda = 5;
                    if (user.PaisID == 6) rub.rubroMoneda = 6;
                    if (user.PaisID == 7) rub.rubroMoneda = 7;
                }
                rubtemp = (Rubros)rub;

                if (requierealias == 1) rubtemp.rubroName = DB.getAliasRubro(user.PaisID, (int)rubtemp.rubroID, (int)cliID, rubtemp.rubroName);
                rubtemp = (Rubros)DB.ExistRubroPais(rub, user.PaisID);
                if (rubtemp == null)
                {
                    WebMsgBox.Show("Error, El rubro " + rub.rubroID + " no se encuentra registrado en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlo.");
                    //WebMsgBox.Show("Error, El rubro  no se encuentra registrado en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlo.");
                    return null;
                }
                if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))//si debe cobrar iva y el rubro no esta en dolares y no es excento
                {
                    if (rubtemp.IvaInc == 1)
                    {
                        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                    }
                    else
                    {
                        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                        rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    }
                }
                else
                {
                    rubtemp.rubroSubTot = rubtemp.rubroTot;
                }
                decimal tipoCambio = user.pais.TipoCambio;
                double totalD = rubtemp.rubroTot;
                string simbolomoneda = "";
                if (rubtemp.rubroMoneda == 8)
                {
                    totalD = (double)Math.Round(((decimal)rubtemp.rubroTot / tipoCambio), 2);
                    simbolomoneda = "USD";
                }

                if (rubtemp.rubroMoneda != 8)
                {
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto * (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot * (double)tipoCambio, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    if (lb_moneda.SelectedValue.Equals("1")) simbolomoneda = "GTQ";
                    if (lb_moneda.SelectedValue.Equals("2")) simbolomoneda = "SVC";
                    if (lb_moneda.SelectedValue.Equals("3")) simbolomoneda = "HNL";
                    if (lb_moneda.SelectedValue.Equals("4")) simbolomoneda = "NIC";
                    if (lb_moneda.SelectedValue.Equals("5")) simbolomoneda = "CRC";
                    if (lb_moneda.SelectedValue.Equals("6")) simbolomoneda = "PAB";
                    if (lb_moneda.SelectedValue.Equals("7")) simbolomoneda = "BZD";
                }

                rubtemp.rubroCommentario = "";
                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario };
                dt.Rows.Add(obj);
            }
        }
        return dt;
    }
    private void obtengo_listas(int tipoconta, int impo_expo, int paiID)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='newinvoice.aspx'");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipo_transaccion.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(paiID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
                lb_moneda2.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (impo_expo == rgb.intC1) { item.Selected = true; lb_imp_exp.Enabled = false; }
                lb_imp_exp.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_contribuyente");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_contribuyente.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 14, user,0);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
            }
            arr = null;
            if (user.PaisID == 1)
            {
                arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 1, user, 1);//1 porque es el tipo de documento para facturacion
            }
            else
            {
                arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 1, user, 0);//1 porque es el tipo de documento para facturacion
            }
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura2.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_servicio");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_servicio.Items.Add(item);
            }
            lb_servicio.SelectedIndex = 1;
            ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), lb_tipo_transaccion.SelectedValue);
            RE_GenericBean rubbean = null;
            for (int a = 0; a < rubros.Count; a++)
            {
                rubbean = (RE_GenericBean)rubros[a];
                item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
                lb_rubro.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.Get_Regimen_Aduanero_XPais(user.PaisID);
            drp_regimen_aduanero.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_regimen_aduanero.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC2, rgb.strC1);
                drp_regimen_aduanero.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getTipoFactura();
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipo_factura.Items.Add(item);
            }
        }
    }
    protected void lb_imp_exp_SelectedIndexChanged(object sender, EventArgs e)
    {
        int impo_expo = 0;
        if (lb_imp_exp.Items.Count > 0)
            impo_expo = int.Parse(lb_imp_exp.SelectedValue);
        cargo_datos_BL(impo_expo);
    }
    protected void lb_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        int servID = int.Parse(lb_servicio.SelectedValue);
        int impo_expo = int.Parse(lb_imp_exp.SelectedValue);
        int tipocontri = int.Parse(lb_contribuyente.SelectedValue);
        int tipomoneda = int.Parse(lb_moneda.SelectedValue);
        ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, servID, lb_tipo_transaccion.SelectedValue);
        RE_GenericBean rubbean = null;
        ListItem item = null;
        lb_rubro.Items.Clear();
        for (int a = 0; a < rubros.Count; a++)
        {
            rubbean = (RE_GenericBean)rubros[a];
            item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
            lb_rubro.Items.Add(item);
        }
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (tbCliCod.Text.Equals(""))
        {
            WebMsgBox.Show("Si desea facturar un rubro, debe seleccionar un cliente");
            return;
        }
        user = (UsuarioBean)Session["usuario"];
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Columns.Add("TOTALD");
        dt.Columns.Add("COMENTARIO");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        TextBox tb1;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_codigo");
            lb2 = (Label)row.FindControl("lb_rubro");
            lb3 = (Label)row.FindControl("lb_tipo");
            lb4 = (Label)row.FindControl("lb_subtotal");
            lb5 = (Label)row.FindControl("lb_impuesto");
            lb6 = (Label)row.FindControl("lb_total");
            lb7 = (Label)row.FindControl("lb_totaldolares");
            lb8 = (Label)row.FindControl("lb_monedatipo");
            tb1 = (TextBox)row.FindControl("tb_comentario");
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, tb1.Text };
            dt.Rows.Add(objArr);
        }


        //halo el rubro y luego le calculo sus chingaderas
        Rubros rubro = new Rubros();
        rubro.rubroID = long.Parse(lb_rubro.SelectedValue);
        rubro.rubroName = lb_rubro.SelectedItem.Text;
        int cliID = int.Parse(tbCliCod.Text);
        int requierealias = int.Parse(lb_requierealias.Text);
        if (requierealias == 1) rubro.rubroName = DB.getAliasRubro(user.PaisID, rubro.rubroID, cliID, rubro.rubroName);
        rubro.rubroMoneda = long.Parse(lb_moneda2.SelectedValue);
        
        //DEFINIR TIPO DE SERVICIO
        #region Backup
        //if (lb_servicio.SelectedItem.Text.Equals("FCL")) { rubro.rubtoType = "FCL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("LCL")) { rubro.rubtoType = "LCL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("AEREO")) { rubro.rubtoType = "AEREO"; }
        //if (lb_servicio.SelectedItem.Text.Equals("APL")) { rubro.rubtoType = "APL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("TRANSPORTE LOCAL")) { rubro.rubtoType = "TRANSPORTE LOCAL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("SEGUROS")) { rubro.rubtoType = "SEGUROS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("PUERTOS")) { rubro.rubtoType = "PUERTOS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("APL LOGISTICS")) { rubro.rubtoType = "APL LOGISTICS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("ADUANAS")) { rubro.rubtoType = "ADUANAS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("ALMACENADORA")) { rubro.rubtoType = "ALMACENADORA"; }
        //if (lb_servicio.SelectedItem.Text.Equals("INSPECTOR")) { rubro.rubtoType = "INSPECTOR"; }
        //if (lb_servicio.SelectedItem.Text.Equals("PO BOX")) { rubro.rubtoType = "PO BOX"; }
        //if (lb_servicio.SelectedItem.Text.Equals("ADMINISTRATIVO")) { rubro.rubtoType = "ADMINISTRATIVO"; }
        //if (lb_servicio.SelectedItem.Text.Equals("TERCEROS")) { rubro.rubtoType = "TERCEROS"; }
        //if (lb_servicio.SelectedItem.Text.Equals("INTERMODAL")) { rubro.rubtoType = "INTERMODAL"; }
        #endregion
        rubro.rubtoType = lb_servicio.SelectedItem.Text;

        rubro.rubroTot = double.Parse(tb_monto.Text);
        Rubros rubtemp = (Rubros)rubro;
        rubtemp = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
        if (rubtemp == null)
        {
            WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
            return;
        }
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        //// Si la moneda que estoy agregando es diferente a la que estoy facturando
        if (!lb_moneda.SelectedValue.Equals("8"))
        {
            if (!lb_moneda2.SelectedValue.Equals("8"))
            {
                //rubro.rubroTot = Math.Round((double.Parse(tb_monto.Text) / (double)user.pais.TipoCambio), 2);
                rubtemp.rubroTot = Math.Round(double.Parse(tb_monto.Text), 2);
            }
            else
            {
                rubtemp.rubroTot = Math.Round((double.Parse(tb_monto.Text) * (double)user.pais.TipoCambio), 2);
            }
        }
        else
        {
            if (lb_moneda2.SelectedValue.Equals("8"))
            {
                rubtemp.rubroTot = double.Parse(tb_monto.Text);
            }
            else
            {
                rubtemp.rubroTot = Math.Round((double.Parse(tb_monto.Text) / (double)user.pais.TipoCambio), 2);
            }
        }

        if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))
        {
            if (rubtemp.IvaInc == 1)
            {
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
            }
            else
            {
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
            }
        }
        else
        {
            rubtemp.rubroSubTot = rubtemp.rubroTot;
        }
        decimal tipoCambio = user.pais.TipoCambio;
        double totalD = rubtemp.rubroTot;

        if (!lb_moneda.SelectedValue.Equals("8"))
        {
            totalD = (double)Math.Round(((decimal)rubtemp.rubroTot / tipoCambio), 2);
        }

        string simbolomoneda = "";
        if (rubtemp.rubroMoneda == 1) simbolomoneda = "GTQ";
        if (rubtemp.rubroMoneda == 2) simbolomoneda = "SVC";
        if (rubtemp.rubroMoneda == 3) simbolomoneda = "HNL";
        if (rubtemp.rubroMoneda == 4) simbolomoneda = "NIC";
        if (rubtemp.rubroMoneda == 5) simbolomoneda = "CRC";
        if (rubtemp.rubroMoneda == 6) simbolomoneda = "PAB";
        if (rubtemp.rubroMoneda == 7) simbolomoneda = "BZD";
        if (rubtemp.rubroMoneda == 8) simbolomoneda = "USD";

        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
        dt.Rows.Add(obj);

        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();

    }
    protected void gv_detalle_DataBound(object sender, EventArgs e)
    {
        decimal subtotal = 0;
        decimal impuesto = 0;
        decimal total = 0;
        decimal totalD = 0;
        Label lb1, lb2, lb3, lb4;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_subtotal");
            lb2 = (Label)row.FindControl("lb_impuesto");
            lb3 = (Label)row.FindControl("lb_total");
            lb4 = (Label)row.FindControl("lb_totaldolares");
            subtotal += Math.Round(decimal.Parse(lb1.Text), 2);
            impuesto += Math.Round(decimal.Parse(lb2.Text), 2);
            total += Math.Round(decimal.Parse(lb3.Text), 2);
            totalD += Math.Round(decimal.Parse(lb4.Text), 2);
        }
        Label2.Text = "Equivalente en " + simboloequivalente;
        Label6.Text = "Sub total en " + simbolomoneda;
        Label7.Text = "Impuesto en " + simbolomoneda;
        Label8.Text = "Total en " + simbolomoneda;
        tb_subtotal.Text = subtotal.ToString("#,#.00#;(#,#.00#)");
        tb_impuesto.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
        tb_totaldolares.Text = totalD.ToString("#,#.00#;(#,#.00#)");
    }
    protected void bt_imprimir_Click(object sender, EventArgs e)
    {
        #region Backup
        //String csname1 = "PopupScript";
        //Type cstype = this.GetType();
        //// Get a ClientScriptManager reference from the Page class.
        //ClientScriptManager cs2 = Page.ClientScript;
        //string script = "window.open('print.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        
        //if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        //{
        //    cs2.RegisterStartupScript(cstype, csname1, script, true);
        //}
        //bt_imprimir.Enabled = false;
        #endregion
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        user.ImpresionBean.Operacion = "1";
        user.ImpresionBean.Tipo_Documento = "1";
        user.ImpresionBean.Id = lb_facid.Text;
        user.ImpresionBean.Impreso = false;
        Session["usuario"] = user;
        string script = "";
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        //Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        if (user.PaisID != 6) //Si no es Panama, realiza impresion estandar
        {
            //Toque Aqui
            #region Impresiones Electronicas
            if ((user.PaisID == 1) && (lbl_tipo_serie.Text == "1"))
            {
                if (!DB.DownloadFEL(lb_facid.Text, "1", "PDF", Response, lbl_internal_reference.Text, user.PaisID, true, true)) 
                    DB.OpenGFACEpdf(lbl_internal_reference.Text, Response);
            }
            else
            {
                script = "window.open('printersettings.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
            }
            #endregion
            //script = "window.open('printersettings.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        }
        else if ((user.PaisID == 6) && (lbl_tipo_serie.Text == "1"))
        { //En el caso de Panama utiliza la impresion especial para Impresora Fiscal Tally1125, parametro op=0=impresion
            script = "window.open('printersettingsPA.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        }
        else
        {
            script = "window.open('printersettings.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        }

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
        if (lbl_tipo_serie.Text != "1")
        {
            bt_imprimir.Enabled = false;
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        if (tbCliCod.Text.Equals("") || (tbCliCod.Text.Equals("0")))
        {
            WebMsgBox.Show("Debe ingresar el codigo del cliente");
            return;
        }
        if (lb_imp_exp.SelectedValue != "3")
        {
            if ((!tb_contenedor.Text.Trim().Equals("")) && (tb_contenedor.Text.Trim().Length > 3))
            {

                if ((Request.QueryString["tipo"] == "LCL") || (Request.QueryString["tipo"] == "FCL") || (Request.QueryString["tipo"] == null))
                {
                    string resultado = "";
                    resultado = DB.ValidarContenedor(tb_contenedor.Text.ToUpper());
                    if (resultado == "0")
                    {
                        WebMsgBox.Show("El numero de Contenedor no es valido");
                        return;
                    }
                    if (resultado == "")
                    {
                        WebMsgBox.Show("Existio un error porque el Numero de Contenedor no es valido");
                        return;
                    }
                }
            }
        }
        if (gv_detalle.Rows.Count < 1)
        {
            WebMsgBox.Show("Debe facturar por lo menos 1 rubro");
            return;
        }
        if (decimal.Parse(tb_total.Text) == 0)
        {
            WebMsgBox.Show("La factura debe ser mayor a 0");
            return;
        }
        int transID = int.Parse(lb_tipo_transaccion.SelectedValue);//tipo de transaccion factura, invoice
        int contribuyente = int.Parse(lb_contribuyente.SelectedValue);//excento contribuyente
        int moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue);//importacion exportacion
        int tipo_cobro = 1;//prepaid, collect
        if (imp_exp == 1) { tipo_cobro = 1; } else { tipo_cobro = 2; } //si es 1=importacion cobro 1=collect,si es 2=exportacion cobro 2=prepaid
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());//fiscal o financiera
        int servicio = 0; //fcl, lcl, etc

        FacturaBean factfinal = new FacturaBean();
        factfinal.Fecha_Hora = lb_fecha_hora.Text;
        factfinal.cobroID = tipo_cobro;
        factfinal.imp_exp = imp_exp;
        factfinal.Nit = tb_nit.Text;
        factfinal.Nombre = tb_nombre.Text;
        factfinal.allIN = tb_allin.Text;
        factfinal.Direccion = tb_direccion.Text;
        factfinal.ReciboAduanal = tb_reciboaduanal.Text;
        factfinal.Recibo_Agencia = tb_recibo_agencia.Text.Trim();
        factfinal.Valor_Aduanero = tb_valor_aduanero.Text.Trim();
        factfinal.Tipo_Factura = int.Parse(lb_tipo_factura.SelectedValue);
        if ((factfinal.Nit == null || factfinal.Nit.Equals("")) || (factfinal.Nombre == null || factfinal.Nombre.Equals("")))
        {
            factfinal.Nit = "C/F";
        }
        factfinal.Fecha_Emision = DateTime.Now.ToString();
        factfinal.SubTot = double.Parse(tb_subtotal.Text);
        factfinal.Impuesto = double.Parse(tb_impuesto.Text);
        if ((lb_moneda.SelectedValue == "8"))
        {

            factfinal.SubTotequivalente = Math.Round(factfinal.SubTot * (double)user.pais.TipoCambio, 2);
            factfinal.Impuesto_equivalente = Math.Round(factfinal.Impuesto * (double)user.pais.TipoCambio, 2);
        }
        else
        {
            factfinal.SubTotequivalente = Math.Round(factfinal.SubTot / (double)user.pais.TipoCambio, 2);
            factfinal.Impuesto_equivalente = Math.Round(factfinal.Impuesto / (double)user.pais.TipoCambio, 2);
        }
        factfinal.Total = double.Parse(tb_total.Text);
        factfinal.TotalDol = double.Parse(tb_totaldolares.Text);
        factfinal.Observaciones = tb_observaciones.Text;
        factfinal.Otras_Observaciones = tb_otras_observaciones.Text;
        factfinal.CliID = long.Parse(tbCliCod.Text);
        factfinal.MonedaID = moneda;
        factfinal.TedID = 1;//activo=1 tbl estado documento
        factfinal.UsuID = user.ID;
        factfinal.HBL = tb_hbl.Text;
        factfinal.MBL = tb_mbl.Text;
        factfinal.Contenedor = tb_contenedor.Text;
        factfinal.Routing = tb_routing.Text;
        factfinal.Naviera = tb_naviera.Text;
        factfinal.Vapor = tb_vapor.Text;
        factfinal.Shipper = tb_shipper.Text;
        factfinal.OrdenPO = tb_orden.Text;
        factfinal.Consignee = tb_consignee.Text;
        factfinal.Comodity = tb_comodity.Text;
        factfinal.Paquetes = tb_paquetes2.Text;
        factfinal.cantPaquetes = tb_paquetes1.Text;
        factfinal.Peso = tb_peso.Text;
        factfinal.Volumen = tb_vol.Text;
        factfinal.Dua_Ingreso = tb_dua_ingreso.Text;
        factfinal.Dua_Salida = tb_dua_salida.Text;
        factfinal.Vendedor1 = tb_vendedor1.Text;
        factfinal.Vendedor2 = tb_vendedor2.Text;
        factfinal.Razon = tb_razon.Text;
        factfinal.Referencia = "";
        factfinal.Serie = lb_serie_factura2.SelectedValue;
        factfinal.AgenteID = int.Parse(tb_agenteid.Text);
        factfinal.Nombre_Agente = tb_agente_nombre.Text.Trim();
        factfinal.ShipperID = int.Parse(id_shipper.Text);
        factfinal.ConsigneeID = int.Parse(id_consignee.Text);
        factfinal.Regimen_Aduanero = int.Parse(drp_regimen_aduanero.SelectedValue.ToString());
        factfinal.BlId = int.Parse(lbl_blID.Text);
        factfinal.Tipo_Operacion = int.Parse(lbl_tipoOperacionID.Text);
        factfinal.serieID = int.Parse(lbl_serie_id.Text);
        factfinal.Correo_Electronico = lbl_correo_documento_electronico.Text;
        factfinal.Referencia_Correo = tb_referencia_correo.Text.Trim();
        factfinal.Factura_Electronica = int.Parse(lbl_tipo_serie.Text);
        factfinal.Contribuyente = contribuyente;
        //recorro el datagrid para aramar la factura
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9;
        TextBox tb1;
        Rubros rubro;
        XML_Bean Bean_IVA = new XML_Bean();
        XML_Bean Bean_IVACERO = new XML_Bean();
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_codigo");
            lb2 = (Label)row.FindControl("lb_rubro");
            lb3 = (Label)row.FindControl("lb_tipo");
            lb4 = (Label)row.FindControl("lb_monedatipo");
            lb5 = (Label)row.FindControl("lb_subtotal");
            lb6 = (Label)row.FindControl("lb_impuesto");
            lb7 = (Label)row.FindControl("lb_total");
            lb8 = (Label)row.FindControl("lb_totaldolares");
            lb9 = (Label)row.FindControl("lb_cargoid");
            tb1 = (TextBox)row.FindControl("tb_comentario");
            rubro = new Rubros();
            rubro.rubroID = long.Parse(lb1.Text); ;
            rubro.rubroName = lb2.Text.Trim();
            rubro.rubtoType = lb3.Text;
            rubro.rubroSubTot = double.Parse(lb5.Text);
            rubro.rubroImpuesto = double.Parse(lb6.Text);
            rubro.rubroTot = double.Parse(lb7.Text);
            rubro.rubroTotD = double.Parse(lb8.Text);
            rubro.rubroCommentario = tb1.Text;
            rubro.rubroMoneda = Utility.TraducirMonedaStr(lb4.Text);
            rubro.rubroTypeID = Utility.TraducirServiciotoINT(lb3.Text);
            servicio = rubro.rubroTypeID;
            rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            rubro.cta_haber = (ArrayList)DB.getCtaContablebyRubro("haber", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            rubro.rubroCargoID = double.Parse(lb9.Text);
            if ((rubro.cta_debe == null && rubro.cta_haber == null) || (rubro.cta_debe.Count == 0 && rubro.cta_haber.Count == 0))
            {
                WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de cobro que esta realizando, por favor pongase en contacto con el Contador");
                return;
            }

            if (factfinal.RubrosArr == null) factfinal.RubrosArr = new ArrayList();
            factfinal.RubrosArr.Add(rubro);
            #region Tasas de Impuesto
            if (rubro.rubroImpuesto > 0)
            {
                Bean_IVA.stC1 = "IVA";
                Bean_IVA.decC1 += decimal.Parse(rubro.rubroSubTot.ToString());//Base
                Bean_IVA.stC2 = "12";//Tasa
                Bean_IVA.decC2 += decimal.Parse(rubro.rubroImpuesto.ToString());//Monto
                factfinal.TotalIngresosNG += rubro.rubroSubTot;
            }
            else if (rubro.rubroImpuesto == 0)
            {
                Bean_IVACERO.stC1 = "IVA";
                Bean_IVACERO.decC1 += decimal.Parse(rubro.rubroSubTot.ToString());//Base
                Bean_IVACERO.stC2 = "0";//Tasa
                Bean_IVACERO.decC2 += decimal.Parse(rubro.rubroImpuesto.ToString());//Monto
            }
            #endregion
        }
        moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        factfinal.MonedaID = moneda;
        if (factfinal.RubrosArr == null || factfinal.RubrosArr.Count == 0)
        {
            WebMsgBox.Show("Debe tener rubros para facturar");
            return;
        }
        int matOpID = DB.getMatrizOperacionID(transID, moneda, user.PaisID, tipo_contabilidad);
        ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID, "Cargo");
        string Check_Existencia = DB.CheckExistDoc(factfinal.Fecha_Hora, 1);
        if (Check_Existencia == "0")
        {
            ArrayList result = DB.insertFacturacion(factfinal, user, tipo_contabilidad, ctas_cargo, transID, Provision);

            if (result == null || result.Count == 0 || int.Parse(result[0].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
            {
                WebMsgBox.Show("Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.");
                return;
            }
            else
            {
                int resultado_proforma = DB.Actualizar_Estado_Proforma(int.Parse(Request.QueryString["factID"].ToString().Trim()), 4);
                bool valida_einvoice = Validar_Restricciones("send_einvoice");
                if (valida_einvoice == true)
                {
                    #region Transmitir EInvoice
                    string Referencia_Interna = "";
                    Referencia_Interna = result[2].ToString();
                    if ((lbl_tipo_serie.Text == "1") || (lbl_tipo_serie.Text == "2"))
                    {
                        ArrayList Arr_Data = new ArrayList();
                        /*if (DB.isFELDate(user.PaisID)) { //2019-07-29
                        
		                    #region FEL 2019-04-25
		                    try
		                    {
		                        string facide = result[0].ToString().Trim();
		                        //2019-04-23
		                        //http://10.10.1.7:9191/WebService1.asmx
		                        fel_101017.WebService1 proceso = new fel_101017.WebService1();
		                        user = (UsuarioBean)Session["usuario"];
		                        var resultado = proceso.Proceso_07_Completo("", "", facide, user.ID, "1", "", 1); //lb_serie_factura.SelectedItem.Text, result[1].ToString()
		                        if (resultado[3] == "")
		                        {
		                            //ExmlDoc.InnerXml = DB.Base64String_String(Tag.ResponseData.ResponseData1);  //string Signature = resultado[0]; //DB.Get_Signature(user, ExmlDoc);
		                            string iStr = "Se Transmitio correctamente la Factura Serie " + resultado[1] + " y Folio " + resultado[2];
		                            //setButtons("1", "Se Transmitio correctamente la Factura Serie " + lb_serie_factura.SelectedItem.Text + " y Folio " + result[1].ToString(), result, Signature, Referencia_Interna, resultado[0], resultado[1], "Firma Electronica.: " + Signature + " \n" + "Se Transmitio correctamente la Factura Serie " + resultado[1] + " y Folio " + resultado[2]);
		                            setButtons("1", iStr, result, resultado[0], Referencia_Interna, resultado[0], resultado[1], "Firma Electronica.: " + resultado[0] + " \n" + iStr);
		                            return;
		                        }
		                        else
		                        {
		                            setButtons("2", "Existio un error durante la Transmision del Documento con el GFACE y no se obtuvo firma electronica, La factura fue grabada exitosamente con la Serie.: " + lb_serie_factura.SelectedItem.Text + " y Folio.: " + result[1].ToString(), result, "-", Referencia_Interna, "0", result[1].ToString(), resultado[3] + " ");
		                            return;
		                        }
		                        //resultado[3] = "Key : " + resultado[0] + " Serie : " + resultado[1] + " Correlativo :" + resultado[2];
		                    }
		                    catch (Exception ex)
		                    {
		                        setButtons("3", "FEL (SAT) se encuentra fuera de linea y no se obtuvo firma electronica. " + ex.Message + ". La factura fue grabada exitosamente en el Sistema con la Serie.: " + lb_serie_factura.SelectedItem.Text + " y Folio.: " + result[1].ToString() + " y sera retransmitida mediante un proceso automatico en un lapso de 24 horas", result, "-", Referencia_Interna, "0", result[1].ToString(), "");
		                        return;
		                    }
		                    #endregion
                    } else {*/
                        #region Definir Referencia Interna
                        if (Referencia_Interna == "0")
                        {
                            WebMsgBox.Show("Existio un error al momento de definir la referencia Interna, porfavor intente de nuevo");
                            return;
                        }
                        factfinal.Referencia_Interna = Referencia_Interna;
                        factfinal.Correlativo = result[1].ToString();
                        Arr_Data.Add(factfinal);
                        Arr_Data.Add(Referencia_Interna);
                        #endregion
                        #region Generar XML
                        XmlDocument ExmlDoc = (XmlDocument)DB.Generar_XMLNativo(1, int.Parse(result[0].ToString()));
                        #endregion
                        if (ExmlDoc != null)
                        {
                            #region Conectar a GFACE
                            int intentos_transmision = 0;
                            GFACEWEBSERVICE.TransactionTag Tag = new GFACEWEBSERVICE.TransactionTag();
                            do
                            {
                                Tag = (GFACEWEBSERVICE.TransactionTag)DB.Transmitir_Documento_Electronico(user, ExmlDoc, Referencia_Interna);
                                intentos_transmision += 1;
                            } while (((Tag == null)) && (intentos_transmision <= 3));
                            if (Tag == null)
                            {
                                #region Transmision Fallida
                                WebMsgBox.Show("El GFACE (SAT) se encuentra fuera de linea y no se obtuvo firma electronica, La factura fue grabada exitosamente en el Sistema con la Serie.: " + lb_serie_factura2.SelectedValue + " y Folio.: " + result[1].ToString() + " y sera retransmitida mediante un proceso automatico en un lapso de 24 horas");
                                #region Registrar Referencia Interna
                                int result_signature = 0;
                                ArrayList EArr = new ArrayList();
                                EArr.Add(1);
                                EArr.Add("-");
                                EArr.Add(int.Parse(lbl_tipo_serie.Text));
                                EArr.Add(int.Parse(result[0].ToString()));
                                EArr.Add(Referencia_Interna);
                                EArr.Add("0");
                                EArr.Add(result[1].ToString());
                                EArr.Add(ExmlDoc);
                                result_signature = DB.Update_Signature(user, EArr);
                                #endregion
                                //Toque Aqui
                                bt_imprimir.Enabled = false;
                                lb_facid.Text = result[0].ToString();
                                tb_correlativo.Text = result[1].ToString();
                                user = (UsuarioBean)Session["usuario"];

                                //Mostrando la Partida contable generada
                                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
                                gv_detalle_partida.DataBind();
                                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

                                bt_Enviar.Enabled = false;
                                bt_proforma_virtual.Enabled = true;
                                //Toque Aqui
                                //btn_ver_pdf.Visible = false;
                                return;
                                #endregion
                            }
                            else if (Tag.Response.Result == false)
                            {
                                #region Transmision Fallida
                                WebMsgBox.Show("Existio un error durante la Transmision del Documento con el GFACE y no se obtuvo firma electronica, La factura fue grabada exitosamente con la Serie.: " + lb_serie_factura2.SelectedValue + " y Folio.: " + result[1].ToString());
                                tb_resultado_transmision.Text = Tag.Response.Hint + " " + Tag.Response.Description + " " + Tag.Response.Data + " ";
                                pnl_transmision_electronica.Visible = true;
                                #region Registrar Referencia Interna
                                int result_signature = 0;
                                ArrayList EArr = new ArrayList();
                                EArr.Add(1);
                                EArr.Add("-");
                                EArr.Add(int.Parse(lbl_tipo_serie.Text));
                                EArr.Add(int.Parse(result[0].ToString()));
                                EArr.Add(Referencia_Interna);
                                EArr.Add("0");
                                EArr.Add(result[1].ToString());
                                EArr.Add(ExmlDoc);
                                result_signature = DB.Update_Signature(user, EArr);
                                #endregion
                                //Toque Aqui
                                bt_imprimir.Enabled = false;
                                lb_facid.Text = result[0].ToString();
                                tb_correlativo.Text = result[1].ToString();
                                user = (UsuarioBean)Session["usuario"];

                                //Mostrando la Partida contable generada
                                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
                                gv_detalle_partida.DataBind();
                                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

                                bt_Enviar.Enabled = false;
                                bt_proforma_virtual.Enabled = true;
                                //Toque Aqui
                                //btn_ver_pdf.Visible = false;
                                return;
                                #endregion
                            }
                            else if (Tag.Response.Result == true)
                            {
                                #region Transmision Exitosa
                                WebMsgBox.Show("Se Transmitio correctamente la Factura Serie " + lb_serie_factura2.SelectedValue + " y Folio " + result[1].ToString());
                                #region Registrar Firma Electronica
                                string Signature = "";
                                int result_signature = 0;
                                ArrayList EArr = new ArrayList();
                                ExmlDoc.InnerXml = DB.Base64String_String(Tag.ResponseData.ResponseData1);
                                Signature = DB.Get_Signature(user, ExmlDoc);
                                EArr.Add(1);
                                EArr.Add(Signature);
                                EArr.Add(int.Parse(lbl_tipo_serie.Text));
                                EArr.Add(int.Parse(result[0].ToString()));
                                EArr.Add(Referencia_Interna);
                                EArr.Add(Tag.Response.Identifier.DocumentGUID);
                                EArr.Add(Tag.Response.Identifier.Serial);
                                EArr.Add(null);
                                result_signature = DB.Update_Signature(user, EArr);
                                pnl_transmision_electronica.Visible = true;
                                tb_resultado_transmision.Text = "Firma Electronica.: " + Signature + " \n" + "Se Transmitio correctamente la Factura Serie " + Tag.Response.Identifier.Batch + " y Folio " + Tag.Response.Identifier.Serial;
                                lbl_internal_reference.Text = Referencia_Interna;
                                #endregion
                                bt_imprimir.Enabled = true;
                                lb_facid.Text = result[0].ToString();
                                tb_correlativo.Text = result[1].ToString();
                                user = (UsuarioBean)Session["usuario"];
                                #region Mostrando la Partida contable
                                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
                                gv_detalle_partida.DataBind();
                                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                                #endregion
                                bt_Enviar.Enabled = false;
                                bt_proforma_virtual.Enabled = true;
                                //Toque Aqui
                                //btn_ver_pdf.Visible = false;
                                return;
                                #endregion
                            }

                            #endregion
                        }
                        else
                        {
                            WebMsgBox.Show("Existio un error en la Generacion del XML, porfavor intente de nuevo");
                            return;
                        }                        
                        }
                    //}
                    #endregion
                }
                bt_imprimir.Enabled = true;
                bt_Enviar.Enabled = false;
                lb_facid.Text = result[0].ToString();
                tb_correlativo.Text = result[1].ToString();

                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
                gv_detalle_partida.DataBind();
                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;


                WebMsgBox.Show("La factura fue grabada exitosamente con el correlativo " + lb_serie_factura2.SelectedValue + result[1].ToString());
                return;
            }
        }
        else
        {
            bt_Enviar.Enabled = false;
            return;
        }
    }



    protected void setButtons(string tipo, string msg, ArrayList result, string Signature, string Referencia_Interna, string docguid, string serial, string result_str)
    {
        WebMsgBox.Show(msg);

        /*#region Registrar Firma Electronica
        int result_signature = 0;
        ArrayList EArr = new ArrayList();
        EArr.Add(1);
        EArr.Add(Signature);
        EArr.Add(int.Parse(lbl_tipo_serie.Text));
        EArr.Add(int.Parse(result[0].ToString()));
        EArr.Add(Referencia_Interna);
        EArr.Add(docguid);  // EArr.Add(Tag.Response.Identifier.DocumentGUID);
        EArr.Add(serial);  //EArr.Add(Tag.Response.Identifier.Serial);
        EArr.Add(null);
        result_signature = DB.Update_Signature(user, EArr);
        #endregion*/

        switch (tipo)
        {
            case "1": //ok
                pnl_transmision_electronica.Visible = true;
                tb_resultado_transmision.Text = result_str; //"Firma Electronica.: " + Signature + " \n" + "Se Transmitio correctamente la Factura Serie " + Tag.Response.Identifier.Batch + " y Folio " + Tag.Response.Identifier.Serial;
                lbl_internal_reference.Text = Referencia_Interna;
                bt_imprimir.Enabled = true;
                break;

            case "2": //error
                pnl_transmision_electronica.Visible = true;
                tb_resultado_transmision.Text = result_str; //Tag.Response.Hint + " " + Tag.Response.Description + " " + Tag.Response.Data + " ";
                bt_imprimir.Enabled = false;
                break;

            case "3": //no connect
                bt_imprimir.Enabled = false;
                break;

        }
        
        lb_facid.Text = result[0].ToString();
        tb_correlativo.Text = result[1].ToString();
        user = (UsuarioBean)Session["usuario"];
        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
        gv_detalle_partida.DataBind();
        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
        bt_Enviar.Enabled = false;
        bt_proforma_virtual.Enabled = true;
        //btn_ver_pdf.Visible = false;
    }


    protected void gv_detalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Columns.Add("TOTALD");
        dt.Columns.Add("COMENTARIO");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        TextBox tb1;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_codigo");
            lb2 = (Label)row.FindControl("lb_rubro");
            lb3 = (Label)row.FindControl("lb_tipo");
            lb4 = (Label)row.FindControl("lb_subtotal");
            lb5 = (Label)row.FindControl("lb_impuesto");
            lb6 = (Label)row.FindControl("lb_total");
            lb7 = (Label)row.FindControl("lb_totaldolares");
            lb8 = (Label)row.FindControl("lb_monedatipo");
            tb1 = (TextBox)row.FindControl("tb_comentario");
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, tb1.Text };
            dt.Rows.Add(objArr);
        }
        dt.Rows[e.RowIndex].Delete();
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }

    protected void gv_clientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_clientes.SelectedRow;
        double cliID = 0;

        cliID = double.Parse(row.Cells[1].Text);
        //Obtengo los datos del cliente
        string criterio = "a.id_cliente=" + cliID;
        ArrayList clientearr = (ArrayList)DB.getClientes(criterio, user, "");
        RE_GenericBean clienteBean;
        if ((clientearr != null) && (clientearr.Count > 0))
        {
            // si entro aqui es porque encontre datos del cliente

            clienteBean = (RE_GenericBean)clientearr[0];
            if (clienteBean.intC1 == -100)
            {
                WebMsgBox.Show("El cliente no tiene configurado su regimen tributario, por lo tanto no se puede realizar la factura");
                return;
            }
            tbCliCod.Text = clienteBean.douC1.ToString();
            tb_razon.Text = clienteBean.strC1;
            tb_nombre.Text = clienteBean.strC2;
            tb_nit.Text = clienteBean.strC3;
            tb_direccion.Text = clienteBean.strC4;
            lb_contribuyente.SelectedValue = clienteBean.intC1.ToString();
            lb_requierealias.Text = clienteBean.intC2.ToString();
        }
        ViewState["dt"] = null;
        gv_clientes.DataSource = null;
        gv_clientes.DataBind();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        string codigo = tb_codigo.Text.Trim();
        string nombre = tb_nombre_cliente.Text.Trim().Replace(' ', '%').ToUpper();
        string nit = tb_nit_cliente.Text.Trim().ToUpper();
        string criterio = "";
        if ((codigo.Equals("")) && (nombre.Equals("")) && (nit.Equals("")))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de busqueda");
            return;
        }
        if ((codigo != null) && (!codigo.Equals("")))
        {
            criterio += "a.id_cliente=" + codigo;
        }
        if ((nombre != null) && (!nombre.Equals("")))
            if (!criterio.Equals(""))
                criterio += " and nombre_cliente like ('%" + nombre + "%')";
            else
                criterio += "nombre_cliente like ('%" + nombre + "%')";
        if ((nit != null) && (!nit.Equals("")))
            if (!criterio.Equals(""))
                criterio += " and codigo_tributario='" + nit + "'";
            else
                criterio = "codigo_tributario='" + nit + "'";
        ArrayList arr = (ArrayList)DB.getClientes(criterio, user, "");
        if (arr == null || arr.Count == 0)
        {
            WebMsgBox.Show("No se encuentran clientes con estos criterios de busqueda");
            return;
        }
        DataTable dt = new DataTable("cliente");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        foreach (RE_GenericBean rgb in arr)
        {
            object[] objArr = { rgb.douC1, rgb.strC1 };
            dt.Rows.Add(objArr);
        }
        gv_clientes.DataSource = dt;
        gv_clientes.DataBind();
        ViewState["dt"] = dt;
        modalcliente.Show();
    }

    protected void gv_clientes_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_clientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_clientes.DataSource = dt1;
        gv_clientes.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        gv_clientes.DataBind();
        modalcliente.Show();
    }
    protected void aceptar_shipperbtn_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre_shipper.Text.Trim().Replace(' ', '%').ToUpper();
        string nit = tb_nit_shipper.Text.Trim().ToUpper();
        if (nombre.Equals("") && nit.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio para la busqueda");
            return;
        }
        string criterio = null;
        criterio += "es_shipper=true and ";
        if ((nombre != null) && (!nombre.Equals("")))
            criterio += "nombre_cliente like ('%" + nombre + "%')";
        if ((nit != null) && (!nit.Equals("")))
            if (!criterio.Equals(""))
                criterio += " and codigo tributario='" + nit + "'";
            else
                criterio = "codigo tributario='" + nit + "'";
        ArrayList arr = (ArrayList)DB.getClientes(criterio, user, "");

        DataTable dt = new DataTable("cliente");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        foreach (RE_GenericBean rgb in arr)
        {
            object[] objArr = { rgb.douC1, rgb.strC1 };
            dt.Rows.Add(objArr);
        }
        gv_shipper.DataSource = dt;
        gv_shipper.DataBind();
        ViewState["dt"] = dt;
        modalshipper.Show();
    }

    protected void gv_shipper_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_shipper.SelectedRow;
        id_shipper.Text = row.Cells[1].Text;
        tb_shipper.Text = row.Cells[2].Text;
        ViewState["dt"] = null;
        gv_shipper.DataSource = null;
        gv_shipper.DataBind();
    }

    protected void gv_shipper_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_shipper_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_shipper.DataSource = dt1;
        gv_shipper.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        gv_shipper.DataBind();
        modalshipper.Show();
    }

    protected void aceptar_consigneebtn_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre_consignee.Text.Trim().Replace(' ', '%').ToUpper();
        string nit = tb_nit_consignee.Text.Trim().ToUpper();
        if (nombre.Equals("") && nit.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio para la busqueda");
            return;
        }
        string criterio = null;
        criterio += "es_consigneer=true and ";
        if ((nombre != null) && (!nombre.Equals("")))
            criterio += "nombre_cliente like ('%" + nombre + "%')";
        if ((nit != null) && (!nit.Equals("")))
            if (!criterio.Equals(""))
                criterio += " and codigo tributario='" + nit + "'";
            else
                criterio = "codigo tributario='" + nit + "'";
        ArrayList arr = (ArrayList)DB.getClientes(criterio, user, "");

        DataTable dt = new DataTable("cliente");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        if (arr != null)
        {
            foreach (RE_GenericBean rgb in arr)
            {
                object[] objArr = { rgb.douC1, rgb.strC1 };
                dt.Rows.Add(objArr);
            }
        }
        gv_consignee.DataSource = dt;
        gv_consignee.DataBind();
        ViewState["dt"] = dt;
        modalconsignee.Show();
    }
    protected void gv_consignee_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_consignee_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_consignee.SelectedRow;
        id_consignee.Text = row.Cells[1].Text;
        tb_consignee.Text = row.Cells[2].Text;
        ViewState["dt"] = null;
        gv_consignee.DataSource = null;
        gv_consignee.DataBind();
    }
    protected void gv_consignee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_consignee.DataSource = dt1;
        gv_consignee.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        gv_consignee.DataBind();
        modalconsignee.Show();
    }
    //**********************************************************************
    protected void aceptar_comoditiesbtn_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre_comoditie.Text.Trim().Replace(' ', '%').ToUpper();
        string codigo = tb_codigo_comoditie.Text.Trim().ToUpper();
        if (nombre.Equals("") && codigo.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio para la busqueda");
            return;
        }
        string criterio = null;
        if ((nombre != null) && (!nombre.Equals("")))
            criterio += "namees like ('%" + nombre + "%')";
        if (codigo != null && !codigo.Equals(""))
            if (!criterio.Equals(""))
                criterio += " and commodityid=" + codigo;
            else
                criterio = "commodityid=" + codigo;
        ArrayList arr = (ArrayList)DB.getComodities(criterio);

        DataTable dt = new DataTable("cliente");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        if (arr != null)
        {
            foreach (RE_GenericBean rgb in arr)
            {
                object[] objArr = { rgb.douC1, rgb.strC1 };
                dt.Rows.Add(objArr);
            }
        }
        gv_comodities.DataSource = dt;
        gv_comodities.DataBind();
        ViewState["dt"] = dt;
        modalcomodities.Show();
    }
    protected void gv_comodities_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_comodities_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_comodities.SelectedRow;
        id_comodities.Text = row.Cells[1].Text;
        tb_comodity.Text = row.Cells[2].Text;
        ViewState["dt"] = null;
        gv_comodities.DataSource = null;
        gv_comodities.DataBind();
    }
    protected void gv_comodities_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_comodities.DataSource = dt1;
        gv_comodities.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        gv_comodities.DataBind();
        modalcomodities.Show();
    }
    protected void bt_reimprimir_proforma_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();

        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('re_print.aspx?fac_id=" + Request.QueryString["factID"].ToString() + "&s=" + lb_serie_factura.SelectedItem.Text + "&c=" + tb_corr_proforma.Text.Trim() + "&tipo=14','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        #region Seteo de Parametros de Impresion
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        user.ImpresionBean.Operacion = "2";
        user.ImpresionBean.Tipo_Documento = "14";
        user.ImpresionBean.Id = Request.QueryString["factID"].ToString();
        user.ImpresionBean.Impreso = true;
        Session["usuario"] = user;
        #endregion
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
    protected void bt_proforma_virtual_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        int b_permiso = 0;
        foreach (PerfilesBean Bean in Arr_Perfiles)
        {
            if ((Bean.ID == 8) || (Bean.ID == 9) || (Bean.ID == 10) || (Bean.ID == 12) || (Bean.ID == 18))
            {
                b_permiso = 1;
            }
        }
        if (b_permiso == 0)
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }
        else
        {
            String csname1 = "PopupScript";
            Type cstype = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('../invoice/template_invoice.aspx?id=" + Request.QueryString["factID"].ToString() + "&transaccion=14','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void Determinar_Tipo_Serie(int sucID, string serie)
    {
        int Doc_ID = DB.getDocumentoID(sucID, serie, 1, user);
        lbl_serie_id.Text = Doc_ID.ToString();
        RE_GenericBean Bean_Serie = (RE_GenericBean)DB.getFactura(Doc_ID, 1);
        if (Bean_Serie == null)
        {
            WebMsgBox.Show("Existio un error tratando de determinar el Tipo de Documento, por lo cual no fue transmitido");
            return;
        }
        else
        {
            lbl_tipo_serie.Text = Bean_Serie.intC14.ToString();
            if (user.PaisID == 1)
            {
                pnl_documento_electronico.Visible = true;
                lbl_tipo_serie_caption.Font.Bold = true;
                lbl_correo_documento_electronico.Font.Bold = true;
                if (lbl_tipo_serie.Text == "0")
                {
                    lbl_tipo_serie_caption.Text = "Serie Estandard";
                }
                else if (lbl_tipo_serie.Text == "1")
                {
                    lbl_tipo_serie_caption.Text = "Serie Electronica";
                }
                else if (lbl_tipo_serie.Text == "2")
                {
                    lbl_tipo_serie_caption.Text = "Serie en Copia";
                }
            }
        }
    }
    protected void lb_serie_factura2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie_factura2.Items.Count > 0)
        {
            Determinar_Tipo_Serie(user.SucursalID, lb_serie_factura2.SelectedItem.Text);
        }
    }
    protected bool Validar_Restricciones(string sender)
    {
        bool resultado = false;
        bool paiR = DB.Validar_Pais_Restringido(user);
        if (paiR == true)
        {
            #region FACTURACION ELECTRONICA
            if ((DB.Validar_Restriccion_Activa(user, 14, 14) == true) && (sender == "send_einvoice"))
            {
                resultado = true;
                return resultado;
            }
            #endregion
            #region COBRO UNICAMENTE A CLIENTES LOCALES
            if ((DB.Validar_Restriccion_Activa(user, 14, 15) == true) && (sender == "btn_enviar_cliente"))
            {
                double cliID = 0;
                cliID = double.Parse(tbCliCod.Text.Trim().ToString());
                RE_GenericBean Cliente_Bean = DB.getDataClient(cliID);
                if (Cliente_Bean.strC7 == user.pais.ISO)
                {
                    resultado = false;
                }
                else
                {
                    WebMsgBox.Show("No se puede emitir Cobro a clientes Internacionales");
                    bt_Enviar.Enabled = true;
                    resultado = true;
                }
                return resultado;
            }
            #endregion
            #region VALIDACION DE NIT
            if ((DB.Validar_Restriccion_Activa(user, 14, 16) == true) && (sender == "send_nit"))
            {
                resultado = DB.ValidarNIT(tb_nit.Text, user.pais.ISO);
                if (resultado == false)
                {
                    WebMsgBox.Show("Nit invalido, porfavor solicite la correcion en el Catalogo de Clientes");
                    bt_Enviar.Enabled = false;
                    tb_cuenta_ModalPopupExtender.Enabled = false;
                    modalcliente.Enabled = false;
                    gv_detalle.Enabled = false;
                    resultado = true;
                }
                else
                {
                    resultado = false;
                }
                return resultado;
            }
            #endregion
        }
        return resultado;
    }
}
