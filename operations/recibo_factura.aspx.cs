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

public partial class operations_recibo_factura : System.Web.UI.Page
{
    
    DataTable dt = null;
    UsuarioBean user = null;
    public string querystring = "";
    RE_GenericBean provData;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
        }
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 64) == 64))
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }

        if (user.PaisID == 1)
        {
            pnl_sat.Visible = true;
            ddl_tipo_documento.Visible = true;
            lb_tipo_docto.Visible = true;
        }

        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad);
        if ((user.SucursalID == 46) || (user.SucursalID == 55) || (user.SucursalID == 52) || (user.SucursalID == 60) || (user.SucursalID == 84) || (user.SucursalID == 82) || (user.SucursalID == 89) || (user.SucursalID == 53) || (user.SucursalID == 95) || (user.SucursalID == 54) || (user.SucursalID == 91) || (user.SucursalID == 56) || (user.SucursalID == 86) || (user.SucursalID == 72) || (user.SucursalID == 96))
        {
            pnl_provision_automatica.Visible = true;
            pnl_oc.Visible = false;
        }
        else
        {
            pnl_oc.Visible = true;
            pnl_provision_automatica.Visible = false;
        }
    }
    private void obtengo_listas(int tipoconta)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {

            arr = (ArrayList)DB.getTipoDocumentoSat(user,"");
            item = new ListItem("Seleccione...", "0");
            ddl_tipo_documento.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {

                item = new ListItem(rgb.strC2 + "-" + rgb.strC1, rgb.intC1.ToString());
                ddl_tipo_documento.Items.Add(item);
            }
            arr = null;
            if ((user.SucursalID == 15) || (user.PaisID == 5))
            {
                arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, 7, user, 0);//1 porque es el tipo de documento para facturacion
            }
            else
            {
                arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 7, user, 0);//1 porque es el tipo de documento para facturacion
            }
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 8, user, 0);//1 porque es el tipo de documento para ordenes de compra
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_contraseniaserie.Items.Add(item);
            }
            arr = null;
            if ((user.SucursalID == 15) || (user.PaisID == 5))
            {
                arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, 5, user, 0);//1 porque es el tipo de documento para facturacion
            }
            else
            {
                arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 5, user, 0);
            }
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_provision.Items.Add(item);
            }
            item = new ListItem("GEN", "GEN");
            lb_serie_provision.Items.Add(item);
            //RE_GenericBean Datos_Bean = DB.Get_Serie_CorrelativoBy_Traficos_And_Conta(user.SucursalID, user.contaID);
            ArrayList Arr_Series_Sistemas = DB.Get_Serie_CorrelativoBy_Traficos_And_Conta(user.SucursalID, user.contaID);
            foreach (RE_GenericBean Datos_Bean in Arr_Series_Sistemas)
            {
                if (Datos_Bean != null)
                {
                    item = new ListItem(Datos_Bean.strC1, Datos_Bean.strC3);
                    drp_serie_provision_automatica.Items.Add(item);
                }
            }
        }
    }
    protected void bt_aceptar_Click(object sender, EventArgs e)
    {
        if (lb_contraseniaserie.Items.Count==0)
        {
            WebMsgBox.Show("No se puede Generar Contrasena sin Serie");
            return;
        }
        if (user.pais.momentoretencion != 1) {
            //mpeImpresionRetencion.Hide();
        }

        if (user.PaisID == 1)//BAW Fiscal USD
        { 
            if (user.SucursalID == 46)//Sucursal Sistemas
            {
                RE_GenericBean rgb_contrasena = (RE_GenericBean)DB.getDocumentobySerie(lb_contraseniaserie.SelectedItem.Text, 8, user.PaisID);
                RE_GenericBean rgb_provision_sistemas = (RE_GenericBean)DB.Get_Serie_Data_Provisiones_Automaticas(user.SucursalID, user.contaID, drp_serie_provision_automatica.SelectedItem.Text);
                if (rgb_contrasena.intC5.ToString() != rgb_provision_sistemas.strC7)
                {
                    WebMsgBox.Show("La moneda de la Serie de la Provision debe ser la misma de la Serie de Contrasena de Facturas");
                    return;
                }
            }
            else//No es Sucursal Sistemas
            {
                RE_GenericBean rgb_contrasena = (RE_GenericBean)DB.getDocumentobySerie(lb_contraseniaserie.SelectedItem.Text, 8, user.PaisID);
                RE_GenericBean rgb_provision = (RE_GenericBean)DB.getDocumentobySerie(lb_serie_provision.SelectedItem.Text, 5, user.PaisID);
                if (rgb_contrasena.intC5 != rgb_provision.intC5)
                {
                    WebMsgBox.Show("La moneda de la Serie de la Provision debe ser la misma de la Serie de Contrasena de Facturas");
                    return;
                }
            }
        }

        string seriecontrasena="";
        if (tb_factura.Text.Equals("")) {
            WebMsgBox.Show("Debe ingresar el numero de la factura");
            tb_factura.Focus();
            return;
        }
        if ((tb_factura_correlativo.Text.Equals(""))||(tb_factura_correlativo.Text.Trim().Equals("")))
        {
            WebMsgBox.Show("Debe ingresar el correlativo de la factura");
            tb_factura.Focus();
            return;
        }
        #region Validar Documento de Proveedor
        #endregion
        if (tb_monto_factura.Text.Equals("")){
            WebMsgBox.Show("Debe ingresar el monto de la factura");
            tb_monto_factura.Focus();
            return;
        }
        if (tb_fecha.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar la fecha de la factura");
            tb_monto_factura.Focus();
            return;
        }
        //verifica el tipo documento SAT obligatorio para GT.  
        if ((ddl_tipo_documento.SelectedValue.ToString() == "0") && (user.PaisID == 1))
        {
            WebMsgBox.Show("Debe ingresar el tipo de Documento");
            return;
        }

        decimal total_factura = decimal.Parse(tb_monto_factura.Text);
        decimal total_oc = decimal.Parse(tb_valor.Text);
        if (total_factura != total_oc) {
            WebMsgBox.Show("El monto de la factura no coincide, favor verificar la factura, si es correcta, contactar al jefe del departamento que genero el documento para que proceda a corregirla");
            tb_monto_factura.Focus();
            return;
        }
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.intC4 = int.Parse(lb_moneda.Text);//ID moneda
        rgb.intC1 = int.Parse(lb_departamento.Text);
        rgb.intC2 = int.Parse(lb_moneda.Text);
        rgb.intC3 = int.Parse(lb_proveedorID.Text);//ID proveedor
        rgb.strC1 = tb_factura.Text;//Factura
        rgb.strC31 = tb_factura_correlativo.Text;
        rgb.Tipo_Operacion = int.Parse(lbl_tipoOperacionID.Text);
        rgb.Blid = int.Parse(lbl_blID.Text);
        rgb.intC11 = int.Parse(lbl_tpiID.Text);
        rgb.intC12 = int.Parse(lbl_proveedorID.Text);
        rgb.strC2 = DB.DateFormat(tb_fecha.Text.ToString());//Fecha factura
        rgb.strC3 = DB.DateFormat(lb_fechapago.Text.ToString());//Fecha maximo de pago de factura
        rgb.strC4 = tb_observacion.Text;//Observaciones
        rgb.strC5 = tb_hbl.Text;//BL
        rgb.Fecha_Hora = lb_fecha_hora.Text;

        //************************ codigo para obtener la serie de las contraseñas de facturas para esta sucursal
        int transaccion = 5;
        string serie = "GEN";
        //Aca obtengo la serie
        ArrayList arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, transaccion, user, 0);//1 porque es el tipo de documento para ordenes de compra
        if (arr != null && arr.Count > 0) serie = arr[0].ToString();
        rgb.strC20 = serie;//serie 
        rgb.intC10 = transaccion;//tipo de transaccion segun sys_tipo_referencia
        seriecontrasena=lb_contraseniaserie.SelectedValue;
        if ((!tb_oc_correlativo.Text.Trim().Equals("")) && (tb_provision_correlativo.Text.Trim().Equals("")) && (tb_provision_automatica_correlativo.Text.Trim().Equals("")))
        {
            rgb.strC10 = lb_serie_factura.SelectedValue;
            rgb.intC5 = int.Parse(tb_oc_correlativo.Text);
        }
        else if ((tb_oc_correlativo.Text.Trim().Equals("")) && (!tb_provision_correlativo.Text.Trim().Equals("")) && (tb_provision_automatica_correlativo.Text.Trim().Equals("")))
        {
            rgb.strC10 = lb_serie_provision.SelectedValue;
            rgb.intC5 = int.Parse(tb_provision_correlativo.Text);
        }
        else if ((tb_oc_correlativo.Text.Trim().Equals("")) && (!tb_provision_correlativo.Text.Trim().Equals("")) && (tb_provision_automatica_correlativo.Text.Trim().Equals("")))
        {
            rgb.strC10 = drp_serie_provision_automatica.SelectedItem.Text;
            rgb.intC5 = int.Parse(tb_provision_automatica_correlativo.Text);
        }
        user = (UsuarioBean)Session["usuario"];
        rgb.decC1 = decimal.Parse(tb_valor.Text);//valor
        rgb.decC3 = decimal.Parse(tb_noafecto.Text);//no afecto
        rgb.decC2 = decimal.Parse(tb_afecto.Text);//afecto
        rgb.decC4 = decimal.Parse(tb_iva.Text);//Iva
        rgb.intC9 = int.Parse(lb_servicioID.Text);//servicioID        
        rgb.strC15 = tb_hbl.Text;
        rgb.strC16 = tb_mbl.Text;
        rgb.strC17 = tb_routing.Text;
        rgb.strC18 = tb_contenedor.Text;
        rgb.strC19 = tb_observacion.Text;
        rgb.intC6 = int.Parse(lb_tieID.Text);//importacion=1, exportacion=2
        if (user.PaisID == 1)
        {
            rgb.intC15 = int.Parse(ddl_tipo_documento.SelectedValue.ToString()); //tipo documento SAT
        }
        else
        {
            rgb.intC15 = 0; //tipo documento SAT
        }

        if (!lb_moneda.Text.Equals("8")) rgb.decC5 = Math.Round((decimal)rgb.decC1 / (decimal)user.pais.TipoCambio, 2);
        else rgb.decC5 = Math.Round((decimal)rgb.decC1 * (decimal)user.pais.TipoCambio, 2);
        rgb.intC7=4;//tipo persona***************************************

        //recorro el datagrid para aramar el detalle de rubros de la orden de compra
            Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
            Rubros rubro;
            double impuesto = 0;
            double subtotal = 0;
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
                rubro = new Rubros();
                rubro.rubroID = long.Parse(lb1.Text); ;
                rubro.rubroName = lb2.Text;
                rubro.rubtoType = lb3.Text;
                rubro.rubroSubTot = double.Parse(lb4.Text);
                rubro.rubroImpuesto = double.Parse(lb5.Text);
                subtotal += rubro.rubroSubTot;
                impuesto += rubro.rubroImpuesto;
                rubro.rubroTot = double.Parse(lb6.Text);
                rubro.rubroTotD = Math.Round(double.Parse(lb7.Text) / 1, 2);
                if (rubro.rubtoType.Equals("FCL")) rubro.rubroTypeID = 1;
                if (rubro.rubtoType.Equals("LCL")) rubro.rubroTypeID = 2;
                if (rubro.rubtoType.Equals("AEREO")) rubro.rubroTypeID = 3;
                if (rubro.rubtoType.Equals("APL")) rubro.rubroTypeID = 4;
                if (rubro.rubtoType.Equals("TRANSPORTE T")) rubro.rubroTypeID = 5;
                if (rubro.rubtoType.Equals("SEGUROS")) rubro.rubroTypeID = 6;
                if (rubro.rubtoType.Equals("PUERTOS")) rubro.rubroTypeID = 7;
                if (rubro.rubtoType.Equals("APL LOGISTICS")) rubro.rubroTypeID = 8;
                if (rubro.rubtoType.Equals("ADUANAS")) rubro.rubroTypeID = 9;
                if (rubro.rubtoType.Equals("ALMACENADORA")) rubro.rubroTypeID = 10;
                if (rubro.rubtoType.Equals("INSPECTOR")) rubro.rubroTypeID = 11;

                if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                rgb.arr1.Add(rubro);
            }
        ArrayList result = null;
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        if (lb_provisionID.Text.Equals("0"))
        {
            string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 5);
            if (Check_Existencia == "0")
            {
                //Cuando el proveedor requiere OC se debe ingresar su provision hasta este momento
                //La orden de compra solo guarda el total, en este momento se determina los valores afecto/noafecto/iva
                if (impuesto == 0)
                {
                    rgb.decC3 = decimal.Parse(subtotal.ToString());//no afecto
                    rgb.decC2 = 0;//afecto
                    rgb.decC4 = 0;//Iva
                }
                else
                {
                    rgb.decC3 = 0;//no afecto
                    rgb.decC2 = decimal.Parse(subtotal.ToString());//afecto
                    rgb.decC4 = decimal.Parse(impuesto.ToString());//Iva
                }                
                //Revisar
                result = DB.recibo_factura_proveedor(rgb, user, seriecontrasena, tipo_contabilidad);
            }
            else
            {
                bt_aceptar.Enabled = false;
                return;
            }
        }
        else
        {
            var where = string.Empty;
            if (string.IsNullOrEmpty(tb_provision_correlativo.Text))
                where = string.Concat(" and tpr_serie='", drp_serie_provision_automatica.SelectedItem.Text, "' and tpr_correlativo=", int.Parse(tb_provision_automatica_correlativo.Text.Trim()).ToString(), " and tpr_pai_id=", user.PaisID.ToString());
            else
                where = string.Concat(" and tpr_serie='", lb_serie_provision.SelectedValue, "' and tpr_correlativo=", int.Parse(tb_provision_correlativo.Text.Trim()).ToString(), " and tpr_pai_id=", user.PaisID.ToString());
            
            provData = DB.getDetalleProvision(where);
            result = DB.update_recibo_factura_proveedor(rgb, user, seriecontrasena, int.Parse(lb_provisionID.Text), tipo_contabilidad, provData);
        }
        if ((result == null) || (result.Count < 1))
        {
            WebMsgBox.Show("Existió un problema al tratar de guardar la información, por favor intente de nuevo");
            return;
        }
        else {
            tb_correlativo.Text = result[1].ToString();
            WebMsgBox.Show("La información fue guardada exitosamente con el numero " +result[2].ToString() + result[1].ToString());
            bt_aceptar.Enabled = false;
            if (lb_provisionID.Text.Equals("0"))
            {
                string serie1 = lb_serie_factura.SelectedValue;
                int correlativo1 = int.Parse(tb_oc_correlativo.Text);
                int id = DB.getID_OC(serie1, correlativo1, user);
                DB.updateStatusOC(id, 7, user);
            }
            bt_imprimir.Enabled = true;
            bt_aceptar.Enabled = false;
            int provID=int.Parse(lb_proveedorID.Text);
            
        }

        
    }

    protected void GeneroRetencion(int provID, int idprovision) {
        user = (UsuarioBean)Session["usuario"];
        decimal total = decimal.Parse(tb_valor.Text);
        decimal impuesto = Math.Round(total*user.pais.Impuesto,2);
        decimal basse=Math.Round(total-impuesto,2);
        decimal retencion = 0;

        ArrayList arrRetenciones = (ArrayList)DB.getRetencionesByProv(provID, 4, user.PaisID);
        foreach (RE_GenericBean rgb in arrRetenciones) {
            if (rgb.intC6 == 1) { //Calculo sobre la base=total-impuesto
                if (basse>=rgb.decC1){
                    retencion = Math.Round(basse * (rgb.decC2 / 100), 2);
                }
            } else if (rgb.intC6 == 2) { //Calculo sobre el impuesto
                if (basse >= rgb.decC1)
                {
                    retencion = Math.Round(impuesto * (rgb.decC2 / 100), 2);
                }
            } else if (rgb.intC6 == 3) { //Calculo sobre el total
                if (basse >= rgb.decC1)
                {
                    retencion = Math.Round(total * (rgb.decC2 / 100), 2);
                }
            }
        }
        //obtengo las cuentas contables
        int monID = int.Parse(lb_moneda.Text);
        int matOpID = DB.getMatrizOperacionID(14, monID, user.PaisID, 1);//14=retenciones segun tbl_tipo_transaccion, GT 1=fiscal
        if (matOpID <= 0) {
            WebMsgBox.Show("No se puede generar la retencion ya que aun no se encuentra configurada");
            return;
        }
        ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        int result=DB.InsertRetencion(ctas, retencion, idprovision, user, monID, tipo_contabilidad);
        if (result>0)
            WebMsgBox.Show("Se a recibido correctamente la factura, su numero de contrasseña es: " + result);
    }

    protected void tb_ordencompra_TextChanged(object sender, EventArgs e)
    {
        tb_provision_correlativo.Text = "";
        tb_provision_automatica_correlativo.Text = "";
        string serie = lb_serie_factura.SelectedValue;
        int correlativo = int.Parse(tb_oc_correlativo.Text);
        string where = " and tpr_serie_oc='" + serie + "' and tpr_correlativo_oc=" + correlativo + " and tpr_pai_id=" + user.PaisID + "";
        int provisionID = 0;    
        RE_GenericBean Arr = DB.getDetalleProvision(where);
        if (Arr != null)
        {
            provisionID = Arr.intC1;
        }
        else
        {
            WebMsgBox.Show("Orden de Compra Inexistente");
            return;
        }
        lb_provisionID.Text = provisionID.ToString();
        #region Validar EstadoOC
        int OC_est = DB.getStatus_OC(serie, correlativo, user.PaisID);
        #endregion
        if (OC_est == 1)
        {
            WebMsgBox.Show("La orden de compra no ha sido autorizada por su encargado.");
            return;
        }
        else if (OC_est == 7)
        {
            WebMsgBox.Show("La orden de compra ya fue provisionada");
            return;
        }else if (OC_est == 5)
        {
            int id = DB.getID_OC(serie, correlativo,user);
            id = id;
            if (id == 0)
            {
                WebMsgBox.Show("La orden de compra ingresada ya fue provisionada o No pertenece a esta Contabilidad, favor revisar");
                return;
            }
            else if (id != 0)
            {
                DataTable rubdt = (DataTable)DB.getRubbyOC(id, 7);//7=oc segun sys_tipo_referencia
                gv_detalle.DataSource = rubdt;
                gv_detalle.DataBind();

                RE_GenericBean rgb = (RE_GenericBean)DB.getOCData(id);
                string deptoRGP = (string)DB.getDepartamentoName(rgb.intC4);
                if (rgb == null)
                {
                    WebMsgBox.Show("No existe una orden de compra activa con ese identificador o bien ya fue recibida su factura, por favor revisar.");
                    return;
                }
                if (rgb.intC11 == 7)
                {
                    WebMsgBox.Show("La orden de compra que esta tratando de recibir ya fue recibida");
                    return;
                }
                ArrayList cliente_arr = (ArrayList)DB.getProveedor(" numero=" + rgb.intC3, "");
                RE_GenericBean cliente_rgb = (RE_GenericBean)cliente_arr[0];
                int ban_prov = 0;
                for (int i = 0; i < lb_serie_provision.Items.Count; i++)
                {
                    if (lb_serie_provision.Items[i].Text == Arr.strC17)
                    {
                        ban_prov++;
                        lb_serie_provision.SelectedValue = Arr.strC17;
                        tb_provision_correlativo.Text = Arr.intC6.ToString();
                    }
                }
                if ((ban_prov == 0) && (Arr.intC13.ToString() != "0"))
                {
                    WebMsgBox.Show("Esta Provision no pertenece a su departamento");
                    return;
                }
                
                tb_departamento.Text = deptoRGP;
                tb_personaautoriza.Text = rgb.strC10;
                lb_departamento.Text = rgb.intC4.ToString();
                lb_moneda.Text = rgb.intC6.ToString();
                lbl_proveedorID.Text = rgb.intC3.ToString();
                lb_proveedorID.Text = rgb.intC3.ToString();
                lbl_tpiID.Text = Arr.intC11.ToString();
                tb_nit.Text = cliente_rgb.strC1;
                tb_nombre.Text = cliente_rgb.strC2;
                tb_contenedor.Text = rgb.strC5;
                tb_contenedor.Enabled = false;
                tb_routing.Text = rgb.strC4;
                tb_routing.Enabled = false;
                tb_hbl.Text = rgb.strC6;
                tb_hbl.Enabled = false;
                tb_mbl.Text = rgb.strC11;
                tb_mbl.Enabled = false;
                tb_routing.Text = rgb.strC4;
                tb_contenedor.Text = rgb.strC5;
                tb_valor.Text = rgb.decC1.ToString();
                lb_credito.Text = cliente_rgb.intC9.ToString();
                ddl_tipo_documento.SelectedValue = Arr.intC16.ToString();
                if (Arr.intC16 != 0)
                {
                    ddl_tipo_documento.Enabled = false;
                }
                if (!Arr.strC4.Equals(""))
                {
                    tb_fecha.Text = Arr.strC4;
                }
                if (!Arr.strC5.Equals(""))
                {
                    lb_fechapago.Text = Arr.strC5;
                }
                lb_servicioID.Text = rgb.intC9.ToString();
                lb_tieID.Text = rgb.intC10.ToString();
                tb_factura.Text = Arr.strC3;
                tb_factura_correlativo.Text = Arr.strC31;
                rb_bienserv.SelectedValue = Arr.strC36;
                ListItem item = null;
                if ((Arr.strC37 != "") && (Arr.strC38 != "0"))
                {
                    if (lb_contraseniaserie.Items.Count > 0)
                    {
                        item = new ListItem(Arr.strC37);
                        if (lb_contraseniaserie.Items.Contains(item))
                        {
                            lb_contraseniaserie.SelectedItem.Text = Arr.strC37;
                        }
                    }
                    tb_correlativo.Text = Arr.strC38;
                    tb_observacion.Text = Arr.strC39;
                    bt_aceptar.Enabled = false;
                    tb_monto_factura.Text = rgb.decC1.ToString();
                    tb_observacion.ReadOnly = true;
                    tb_fecha.ReadOnly = true;
                    lb_fechapago.ReadOnly = true;
                    tb_factura.ReadOnly = true;
                    tb_factura_correlativo.ReadOnly = true;
                    tb_monto_factura.ReadOnly = true;
                    lbl_fecha_recibo_factura.Text = Arr.strC40;
                }
                
            }
        }
    }
    protected void tb_noafecto_TextChanged(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        decimal valor = decimal.Parse(tb_valor.Text);
        decimal noafecto = decimal.Parse(tb_noafecto.Text);
        decimal afecto = (valor - noafecto);
        decimal impuesto = Math.Round(((afecto / (user.pais.Impuesto + 1)) * user.pais.Impuesto), 2);
        afecto -= impuesto;
        tb_afecto.Text = afecto.ToString();
        tb_iva.Text = impuesto.ToString();
    }

    protected void bt_imprimir_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 32) == 32))
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
            //string script = "window.open('../invoice/printersettings.aspx?fac_id=" + lb_provisionID.Text.Trim() + "&tipo=8','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1 ,top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));";
            string script = "window.open('../ImpresionDocumentos.html?fac_id=" + lb_provisionID.Text.Trim() + "&tipo=8" + "&pais=" + user.PaisID.ToString() + "&idioma=" + user.Idioma.ToString() + "&tipo_cambio=" + user.pais.TipoCambio.ToString("#.00") + "&contaID=" + user.contaID.ToString() + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));";
            #region Seteo de Parametros de Impresion
            user.ImpresionBean.Operacion = "1";
            user.ImpresionBean.Tipo_Documento = "8";
            user.ImpresionBean.Id = lb_provisionID.Text.Trim();
            user.ImpresionBean.Impreso = true;
            Session["usuario"] = user;
            #endregion
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }

    protected void gv_detalle_DataBound(object sender, EventArgs e)
    {
        decimal subtotal = 0;
        decimal impuesto = 0;
        decimal total = 0;
        decimal totalD = 0;
        Label lb1, lb2, lb3, lb4, lb5;
        Button bt1;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_subtotal");
            lb2 = (Label)row.FindControl("lb_impuesto");
            lb3 = (Label)row.FindControl("lb_total");
            lb4 = (Label)row.FindControl("lb_totaldolares");
            lb5 = (Label)row.FindControl("lb_monedatipo");
            if (lb5.Text.Equals("1")) lb5.Text = "GTQ";
            if (lb5.Text.Equals("2")) lb5.Text = "SVC";
            if (lb5.Text.Equals("3")) lb5.Text = "HNL";
            if (lb5.Text.Equals("4")) lb5.Text = "NIC";
            if (lb5.Text.Equals("5")) lb5.Text = "CRC";
            if (lb5.Text.Equals("6")) lb5.Text = "PAB";
            if (lb5.Text.Equals("7")) lb5.Text = "BZD";
            if (lb5.Text.Equals("8")) lb5.Text = "USD";

            subtotal += Math.Round(decimal.Parse(lb1.Text), 2);
            impuesto += Math.Round(decimal.Parse(lb2.Text), 2);
            total += Math.Round(decimal.Parse(lb3.Text), 2);
            totalD += Math.Round(decimal.Parse(lb4.Text), 2);
        }
        //tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
        //tb_total2.Text = total.ToString("#,#.00#;(#,#.00#)");
        //tb_totaldolares.Text = totalD.ToString("#,#.00#;(#,#.00#)");
    }
    protected void gv_detalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("TOTALD");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
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
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb7.Text, lb4.Text, lb5.Text, lb6.Text };
            dt.Rows.Add(objArr);
        }
        dt.Rows[e.RowIndex].Delete();
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }

    protected void tb_provision_correlativo_TextChanged(object sender, EventArgs e)
    {
        tb_oc_correlativo.Text = "";
        tb_provision_automatica_correlativo.Text = "";
        string serie = lb_serie_provision.SelectedValue;
        int correlativo = int.Parse(tb_provision_correlativo.Text.Trim());
        string where = " and tpr_serie='" + serie + "' and tpr_correlativo=" + correlativo+ " and tpr_pai_id=" + user.PaisID + "";
        RE_GenericBean Arr = DB.getDetalleProvision(where);
        if (Arr == null)
        {
            WebMsgBox.Show("La provisión no existe.");
            return;
        }
        else
        {
            provData = (RE_GenericBean)Utility.getDetalleProvision(Arr.intC1);
            lb_provisionID.Text = Arr.intC1.ToString();
            /*if (provData.intC12 == 1)
            {
                WebMsgBox.Show("La provision no ha sido autorizada");
                return;
            }*/
            int ban_oc = 0;
            for (int i=0; i<lb_serie_factura.Items.Count;i++)
            {
                if (lb_serie_factura.Items[i].Text == provData.strC32)
                {
                    ban_oc++;
                    lb_serie_factura.SelectedValue = provData.strC32;
                    tb_oc_correlativo.Text = provData.intC13.ToString();
                }
            }
            if ((ban_oc==0)&&(provData.intC13.ToString()!="0"))
            {
                WebMsgBox.Show("Esta Orden de compra no pertenece a su departamento");
                return;
            }
            RE_GenericBean proveedordata = null;
            string deptoRGP = (string)DB.getDepartamentoName(provData.intC2);
            if (provData.intC11 == 2)
            { // es un agente
                proveedordata = (RE_GenericBean)DB.getAgenteData(provData.intC3, "");// obtengo los datos del agente
                tb_nit.Text = "";
                tb_nombre.Text = proveedordata.strC1;
            }
            else if (provData.intC11 == 4)
            {// es un proveedor 
                proveedordata = (RE_GenericBean)DB.getProveedorData(provData.intC3, ""); //obtengo los datos del proveedor
                tb_nit.Text = proveedordata.strC1;
                tb_nombre.Text = proveedordata.strC2;
            }
            else if (provData.intC11 == 5)
            {// naviera
                proveedordata = (RE_GenericBean)DB.getNavieraData(provData.intC3); //obtengo los datos del proveedor
                tb_nit.Text = "";// proveedordata.strC1;
                tb_nombre.Text = proveedordata.strC1;
            }
            else if (provData.intC11 == 6)
            {// Linea aerea
                proveedordata = (RE_GenericBean)DB.getCarriersData(provData.intC3); //obtengo los datos del proveedor
                tb_nit.Text = "";// proveedordata.strC1;
                tb_nombre.Text = proveedordata.strC1;
            }
            else if (provData.intC11 == 8)
            {// es un proveedor Caja Chica
                proveedordata = (RE_GenericBean)DB.getProveedorData(provData.intC4, ""); //obtengo los datos del proveedor
                tb_nit.Text = proveedordata.strC1;
                tb_nombre.Text = proveedordata.strC2;
            }
            if (!provData.strC4.Equals("")) 
            { 
                tb_fecha.Text = provData.strC4;
            }
            if (!provData.strC5.Equals(""))
            {
                lb_fechapago.Text = provData.strC5;
            }
            tb_hbl.Text = provData.strC13;
            tb_mbl.Text = provData.strC14;
            tb_routing.Text = provData.strC15;
            tb_contenedor.Text = provData.strC16;
            tb_observacion.Text = provData.strC6;
            tb_departamento.Text = deptoRGP;
            tb_personaautoriza.Text = provData.strC11;
            tb_valor.Text = provData.decC1.ToString();
            tb_factura.Text = provData.strC3;
            tb_factura_correlativo.Text = provData.strC31;
            lb_credito.Text = DB.getFechaMaxPago(provData.intC3, provData.intC11, user).ToString();
            lbl_blID.Text = provData.Blid.ToString();
            lbl_tipoOperacionID.Text = provData.Tipo_Operacion.ToString();
            lbl_proveedorID.Text = Arr.intC3.ToString();
            lbl_tpiID.Text = Arr.intC11.ToString();
            if ((Arr.strC37 != "") && (Arr.strC38 != "0"))
            {
                tb_correlativo.Text = Arr.strC38;
                tb_observacion.Text = Arr.strC39;
                bt_aceptar.Enabled = false;
                tb_monto_factura.Text = provData.decC1.ToString();
                tb_observacion.ReadOnly = true;
                tb_fecha.ReadOnly = true;
                lb_fechapago.ReadOnly = true;
                tb_factura.ReadOnly = true;
                tb_factura_correlativo.ReadOnly = true;
                tb_monto_factura.ReadOnly = true;
                lbl_fecha_recibo_factura.Text = Arr.strC40;
            }
            ddl_tipo_documento.SelectedValue = provData.intC16.ToString();
            if (provData.intC16 != 0)
            {
                ddl_tipo_documento.Enabled = false;
            }

        }
    }
    protected void tb_provision_automatica_correlativo_TextChanged(object sender, EventArgs e)
    {
        tb_oc_correlativo.Text = "";
        tb_provision_correlativo.Text = "";
        string serie = drp_serie_provision_automatica.SelectedItem.Text;
        int correlativo = int.Parse(tb_provision_automatica_correlativo.Text.Trim());
        string where = " and tpr_serie='" + serie + "' and tpr_correlativo=" + correlativo + " and tpr_pai_id=" + user.PaisID + "";
        RE_GenericBean Arr = DB.getDetalleProvision(where);
        if (Arr == null)
        {
            WebMsgBox.Show("Provision Inexistente");
            return;
        }
        else
        {
            provData = (RE_GenericBean)Utility.getDetalleProvision(Arr.intC1);
            lb_provisionID.Text = Arr.intC1.ToString();
            /*if (provData.intC12 == 1)
            {
                WebMsgBox.Show("La provision no ha sido autorizada");
                return;
            }*/
            int ban_oc = 0;
            for (int i = 0; i < lb_serie_factura.Items.Count; i++)
            {
                if (lb_serie_factura.Items[i].Text == provData.strC32)
                {
                    ban_oc++;
                    lb_serie_factura.SelectedValue = provData.strC32;
                    tb_oc_correlativo.Text = provData.intC13.ToString();
                }
            }
            if ((ban_oc == 0) && (provData.intC13.ToString() != "0"))
            {
                WebMsgBox.Show("Esta Orden de compra no pertenece a su departamento");
                return;
            }
            RE_GenericBean proveedordata = null;
            string deptoRGP = (string)DB.getDepartamentoName(provData.intC2);
            if (provData.intC11 == 2)
            { // es un agente
                proveedordata = (RE_GenericBean)DB.getAgenteData(provData.intC3, "");// obtengo los datos del agente
                tb_nit.Text = "";
                tb_nombre.Text = proveedordata.strC1;
            }
            else if (provData.intC11 == 4)
            {// es un proveedor 
                proveedordata = (RE_GenericBean)DB.getProveedorData(provData.intC3, ""); //obtengo los datos del proveedor
                tb_nit.Text = proveedordata.strC1;
                tb_nombre.Text = proveedordata.strC2;
            }
            else if (provData.intC11 == 5)
            {// naviera
                proveedordata = (RE_GenericBean)DB.getNavieraData(provData.intC3); //obtengo los datos del proveedor
                tb_nit.Text = "";// proveedordata.strC1;
                tb_nombre.Text = proveedordata.strC1;
            }
            else if (provData.intC11 == 6)
            {// Linea aerea
                proveedordata = (RE_GenericBean)DB.getCarriersData(provData.intC3); //obtengo los datos del proveedor
                tb_nit.Text = "";// proveedordata.strC1;
                tb_nombre.Text = proveedordata.strC1;
            }
            else if (provData.intC11 == 8)
            {// es un proveedor Caja Chica
                proveedordata = (RE_GenericBean)DB.getProveedorData(provData.intC4, ""); //obtengo los datos del proveedor
                tb_nit.Text = proveedordata.strC1;
                tb_nombre.Text = proveedordata.strC2;
            }
            if (!provData.strC4.Equals(""))
            {
                tb_fecha.Text = provData.strC4;
            }
            if (!provData.strC5.Equals(""))
            {
                lb_fechapago.Text = provData.strC5;
            }
            tb_hbl.Text = provData.strC13;
            tb_mbl.Text = provData.strC14;
            tb_routing.Text = provData.strC15;
            tb_contenedor.Text = provData.strC16;
            tb_observacion.Text = provData.strC6;
            tb_departamento.Text = deptoRGP;
            tb_personaautoriza.Text = provData.strC11;
            tb_valor.Text = provData.decC1.ToString();
            tb_factura.Text = provData.strC3;
            tb_factura_correlativo.Text = provData.strC31;
            lb_credito.Text = DB.getFechaMaxPago(provData.intC3, provData.intC11, user).ToString();
            lbl_blID.Text = provData.Blid.ToString();
            lbl_tipoOperacionID.Text = provData.Tipo_Operacion.ToString();
            lbl_proveedorID.Text = provData.intC3.ToString();
            lbl_tpiID.Text = provData.intC11.ToString();
            if ((Arr.strC37 != "") && (Arr.strC38 != "0"))
            {
                tb_correlativo.Text = Arr.strC38;
                tb_observacion.Text = Arr.strC39;
                bt_aceptar.Enabled = false;
                tb_monto_factura.Text = provData.decC1.ToString();
                tb_observacion.ReadOnly = true;
                tb_fecha.ReadOnly = true;
                lb_fechapago.ReadOnly = true;
                tb_factura.ReadOnly = true;
                tb_factura_correlativo.ReadOnly = true;
                tb_monto_factura.ReadOnly = true;
                lbl_fecha_recibo_factura.Text = Arr.strC40;
            }
            ddl_tipo_documento.SelectedValue = provData.intC16.ToString();
            if (provData.intC16 != 0)
            {
                ddl_tipo_documento.Enabled = false;
            }
        }
    }
    protected void tb_fecha_TextChanged(object sender, EventArgs e)
    {
        if (tb_fecha.Text.Trim() != "")
        {
            string Fecha_Factura = "";
            Fecha_Factura = DB.DateFormat(tb_fecha.Text.Trim());
            DateTime aux_fecha_emision = DateTime.Parse(Fecha_Factura);
            //double dias_credito = DB.getFechaMaxPago( int.Parse(lb_proveedorID.Text),int.Parse(lbl_tpiID.Text));
            double dias_credito = DB.getFechaMaxPago(int.Parse(lbl_proveedorID.Text), int.Parse(lbl_tpiID.Text), user);
            DateTime aux_fecha_pago = aux_fecha_emision.AddDays(dias_credito);
            string fecha_pago_formateada = aux_fecha_pago.ToString();
            lb_fechapago.Text = DateTime.Parse(fecha_pago_formateada).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}