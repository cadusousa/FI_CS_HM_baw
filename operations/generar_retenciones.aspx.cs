using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class operations_generar_retenciones : System.Web.UI.Page
{
    DataTable dt = null;
    UsuarioBean user = null;
    int provID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 256) == 256))
            Response.Redirect("index.aspx");

        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        if (!IsPostBack)
        {
            obtengo_listas();
            Carga_Provision_Data();
        }
    }
    private void obtengo_listas()
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='provisiones.aspx'");

            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                drp_tipo_transaccion.Items.Add(item);
            }
            drp_tipo_transaccion.Enabled = true;
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 8, user, 0);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                //lb_serie_factura.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());

                lb_imp_exp.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_contribuyente");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_contribuyente.Items.Add(item);
            };
            DataSet ds = (DataSet)DB.getRetencionesDataSet(user.PaisID);
            gv_retenciones.DataSource = ds.Tables["ret"];
            gv_retenciones.DataBind();
            arr = (ArrayList)DB.getTipoDocumentoSat(user, "");
            item = new ListItem("Seleccione...", "0");
            ddl_tipo_documento.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC2 + "-" + rgb.strC1, rgb.intC1.ToString());
                ddl_tipo_documento.Items.Add(item);
            }
        }
    }
    protected void Carga_Provision_Data()
    {
        if (Request.QueryString["provID"] != null)
        {
            provID = int.Parse(Request.QueryString["provID"].ToString());
            if (!IsPostBack)
            {
                lbl_prov_id.Text = provID.ToString();
            }
            RE_GenericBean provData = (RE_GenericBean)Utility.getDetalleProvision(provID);
            tb_agenteID.Text = provData.intC3.ToString();
            lb_serie_factura.Text = provData.strC17.ToString();
            tb_corr.Text = provData.intC6.ToString();
            //Datos del Proveedor
            tb_documento_serie.Text = provData.strC3;
            tb_documento_correlativo.Text = provData.strC31;
            tb_fechadoc.Text = provData.strC4;

            lb_moneda.SelectedValue = provData.intC5.ToString();
            lb_moneda.Enabled = false;
            tb_observacion.Text = provData.strC6;
            tb_valor.Text = provData.decC1.ToString("#,#.00#;(#,#.00#)");
            tb_afecto.Text = provData.decC2.ToString("#,#.00#;(#,#.00#)");
            tb_noafecto.Text = provData.decC3.ToString("#,#.00#;(#,#.00#)");
            tb_iva.Text = provData.decC4.ToString("#,#.00#;(#,#.00#)");
            lb_imp_exp.SelectedValue = provData.intC10.ToString();
            lbl_blID.Text = provData.Blid.ToString();
            lbl_tipoOperacionID.Text = provData.Tipo_Operacion.ToString();
            tb_hbl.Text = provData.strC13;
            tb_mbl.Text = provData.strC14;
            tb_routing.Text = provData.strC15;
            tb_contenedor.Text = provData.strC16;
            //lb_serie_factura.SelectedValue = provData.strC37;
            lb_imp_exp.SelectedValue = provData.intC10.ToString();
            tb_poliza.Text = provData.strC34;//Poliza Aduanal
            ddl_tipo_documento.SelectedValue = provData.intC16.ToString();
            lb_tipopersona.SelectedValue = provData.intC11.ToString();
            tb_agenteID.Text = provData.intC3.ToString();
            if (provData.boolC1 == true)
            {
                tb_fecha_libro_compras.Text = provData.strC41;//Fecha_Libro_Compras_Formateada
            }
            if (provData.boolC1 == true)
            {
                Rb_Documento.Items[0].Selected = true;
            }
            else
            {
                Rb_Documento.Items[1].Selected = true;
            }
            RE_GenericBean proveedordata = null;
            int excento = 0;
            int proveedorID = int.Parse(tb_agenteID.Text);
            int proveedorCCHID = int.Parse(tb_agenteID.Text);
            if (provData.intC11 == 2)
            { // es un agente
                drp_tipo_transaccion.SelectedValue = "15";
                proveedordata = (RE_GenericBean)DB.getAgenteData(proveedorID, "");// obtengo los datos del agente
                excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                lb_contribuyente.SelectedValue = excento.ToString();
                if (proveedordata != null)
                {
                    tb_agentenombre.Text = proveedordata.strC1;
                    tb_nit.Text = proveedordata.strC6;
                }
                drp_tipo_transaccion.Enabled = false;
            }
            else if (provData.intC11 == 4)
            {// es un proveedor 
                drp_tipo_transaccion.SelectedValue = "8";
                proveedordata = (RE_GenericBean)DB.getProveedorData(proveedorID, ""); //obtengo los datos del proveedor
                excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                lb_contribuyente.SelectedValue = excento.ToString();
                tb_nit.Text = proveedordata.strC1;
                tb_agentenombre.Text = proveedordata.strC2;
                drp_tipo_transaccion.Enabled = false;
            }
            else if (provData.intC11 == 5)
            {// naviera
                drp_tipo_transaccion.SelectedValue = "17";
                proveedordata = (RE_GenericBean)DB.getNavieraData(proveedorID); //obtengo los datos del proveedor
                excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                lb_contribuyente.SelectedValue = excento.ToString();
                tb_nit.Text = proveedordata.strC2;
                tb_agentenombre.Text = proveedordata.strC1;
                drp_tipo_transaccion.Enabled = false;
            }
            else if (provData.intC11 == 6)
            {// Linea aerea
                drp_tipo_transaccion.SelectedValue = "18";
                proveedordata = (RE_GenericBean)DB.getCarriersData(proveedorID); //obtengo los datos del proveedor
                excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                lb_contribuyente.SelectedValue = excento.ToString();
                tb_nit.Text = proveedordata.strC2;
                tb_agentenombre.Text = proveedordata.strC1;
                drp_tipo_transaccion.Enabled = false;
            }
            else if (provData.intC11 == 8)
            {// Caja chica
                drp_tipo_transaccion.SelectedValue = "53";
                proveedordata = (RE_GenericBean)DB.getProveedorData(proveedorCCHID, ""); //obtengo los datos del proveedor de caja chica
                excento = DB.getProveedorRegimen(4, proveedorCCHID.ToString());
                lb_contribuyente.SelectedValue = excento.ToString();
                tb_nit.Text = proveedordata.strC1;
                tb_agentenombre.Text = proveedordata.strC2;
                drp_tipo_transaccion.Enabled = false;
            }
            else if (provData.intC11 == 10)
            {// Intercompany
                drp_tipo_transaccion.SelectedValue = "105";
                proveedordata = (RE_GenericBean)DB.getIntercompanyData(proveedorID); //obtengo los datos del proveedor
                excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                lb_contribuyente.SelectedValue = excento.ToString();
                tb_nit.Text = proveedordata.strC2;
                tb_agentenombre.Text = proveedordata.strC1;
                drp_tipo_transaccion.Enabled = false;
            }
            lb_contribuyente.SelectedValue = provData.Tipo_Contribuyente.ToString();
            DataTable rubdt = (DataTable)DB.getRubbyOC(provID, 5);//5=provision segun sys_tipo_referencia
            gv_detalle.DataSource = rubdt;
            gv_detalle.DataBind();
            tb_valor.Text = provData.decC1.ToString("#,#.00#;(#,#.00#)");
            tb_afecto.Text = provData.decC2.ToString("#,#.00#;(#,#.00#)");
            tb_noafecto.Text = provData.decC3.ToString("#,#.00#;(#,#.00#)");
            tb_iva.Text = provData.decC4.ToString("#,#.00#;(#,#.00#)");

            //Mostrando la Partida contable generada
            gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lbl_prov_id.Text), 5, 0);
            gv_detalle_partida.DataBind();
            int i = 0;
            Label mi_variable;
            foreach (GridViewRow row in gv_detalle_partida.Rows)
            {
                mi_variable = (Label)row.FindControl("lb_desc_cuenta");
                if (mi_variable.Text.Equals("TOTAL"))
                {
                    gv_detalle_partida.Rows[i].Font.Bold = true;
                }
                i++;
            }

        }
    }
    protected void btn_generar_retenciones_Click(object sender, EventArgs e)
    {
        RE_GenericBean Bean = new RE_GenericBean();
        RE_GenericBean retencion = null;
        CheckBox chk = null;
        TextBox tb = null;
        Label lb = null;
        Label lb2 = null;
        int cantidad_retenciones = 0;
        #region Validar si la Provision tiene Retenciones Asociadas
        cantidad_retenciones = DB.Get_Cantidad_Retenciones_X_Provision(int.Parse(lbl_prov_id.Text));
        if (cantidad_retenciones == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de determinar la cantidad de retenciones asociadas a la provision, porfavor intente mas tarde");
            return;
        }
        else if (cantidad_retenciones > 0)
        {
            WebMsgBox.Show("La Provision seleccionada tiene Retenciones Asociadas, no puede agregar mas Retenciones.");
            btn_generar_retenciones.Enabled = false;
            return;
        }
        #endregion
        #region Validar Retenciones Seleccionadas y su Referencia
        foreach (GridViewRow row in gv_retenciones.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("chk_retencion");
                if (chk.Checked)
                {
                    tb = (TextBox)row.FindControl("tb_codigo");
                    lb = (Label)row.FindControl("lb_retencion");
                    lb2 = (Label)row.FindControl("lb_nombre");

                    int id_ret = int.Parse(lb.Text);

                    if (user.PaisID == 1 && (id_ret == 119 || id_ret == 120 || id_ret == 121 || id_ret == 122 || id_ret == 123 || id_ret == 124 || id_ret == 126 || id_ret == 127 || id_ret == 128))
                    {
                        decimal monto_total = decimal.Parse(tb_valor.Text);

                        decimal monto_minimo = 0;
                        //monto_minimo = DB.Validar_Montominimo_Retencion(user, id_ret);
                        switch (id_ret)
                        {
                            case 119:
                                monto_minimo = 2500;
                                break;
                            case 120:
                                monto_minimo = 2500;
                                break;
                            case 121:
                                monto_minimo = 2500;
                                break;
                            case 122:
                                monto_minimo = 2500;
                                break;
                            case 123:
                                monto_minimo = 30000;
                                break;
                            case 124:
                                monto_minimo = 0;
                                break;
                            case 126:
                                monto_minimo = 0;
                                break;
                            case 127:
                                monto_minimo = 2500;
                                break;
                            case 128:
                                monto_minimo = 2500;
                                break;
                            default:
                                monto_minimo = 0;
                                break;
                        }

                        if (monto_total < monto_minimo)
                        {
                            WebMsgBox.Show("Para poder aplicar esta Retencion el monto total debe ser igual o mayor a " + monto_minimo.ToString());
                            return;
                        }
                    }

                    if (tb.Text.Trim() == "")
                    {
                        WebMsgBox.Show("Debe ingresar el numero de Referencia para la Retencion.: " + lb2.Text);
                        return;
                    }
                    else
                    {
                        int resultado_validacion = 0;
                        resultado_validacion = DB.Validar_Referencia_Retencion(user, int.Parse(lbl_prov_id.Text), tb.Text.Trim());
                        if (resultado_validacion > 0)
                        {
                            WebMsgBox.Show("Numero de Retencion Repetido, porfavor ingrese un numero de Referencia Valido para la Retencion.: " + lb2.Text + "");
                            return;
                        }
                    }
                    cantidad_retenciones++;
                }
            }
        }
        if (cantidad_retenciones == 0)
        {
            WebMsgBox.Show("Debe seleccionar al menos una Retencion.");
            return;
        }
        #endregion
        int matOpID = DB.getMatrizOperacionID(int.Parse(drp_tipo_transaccion.SelectedValue), int.Parse(lb_moneda.SelectedValue), user.PaisID, user.contaID);
        ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID, "Abono");

        Bean.intC1 = int.Parse(lb_moneda.SelectedValue);
        Bean.intC8 = int.Parse(lbl_prov_id.Text);
        Bean.intC9 = int.Parse(lb_tipopersona.SelectedValue);

        foreach (GridViewRow row in gv_retenciones.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("chk_retencion");
                if (chk.Checked)
                {
                    tb = (TextBox)row.FindControl("tb_codigo");
                    lb = (Label)row.FindControl("lb_retencion");
                    lb2 = (Label)row.FindControl("lb_nombre");
                    retencion = new RE_GenericBean();
                    retencion = GeneroRetencion(int.Parse(lb.Text), int.Parse(lb_moneda.SelectedValue), user.contaID, int.Parse(lbl_prov_id.Text));
                    retencion.strC1 = tb.Text.Trim();
                    Bean.douC1 += double.Parse(retencion.decC1.ToString());
                    if (Bean.arr2 == null) Bean.arr2 = new ArrayList();
                    Bean.arr2.Add(retencion);
                }
            }
        }
        int resultado = DB.Generar_Retenciones(user, Bean, ctas_cargo);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de Generar las Retenciones, porfavor intente mas tarde.");
            return;
        }
        else
        {
            //Mostrando la Partida contable generada
            gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(Bean.intC8.ToString()), 5, 0);
            gv_detalle_partida.DataBind();
            int i = 0;
            Label mi_variable;
            foreach (GridViewRow row in gv_detalle_partida.Rows)
            {
                mi_variable = (Label)row.FindControl("lb_desc_cuenta");
                if (mi_variable.Text.Equals("TOTAL"))
                {
                    gv_detalle_partida.Rows[i].Font.Bold = true;
                }
                i++;
            }
            WebMsgBox.Show("Retenciones Generadas Exitosamente");
            btn_generar_retenciones.Enabled = false;
        }
    }
    protected RE_GenericBean GeneroRetencion(int retID, int moneda, int tipo_contabilidad, int provID)
    {
        RE_GenericBean result = new RE_GenericBean();
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        decimal valor = decimal.Parse(tb_valor.Text);
        decimal noafecto = decimal.Parse(tb_noafecto.Text);
        decimal afecto = (valor - noafecto);
        decimal impuesto = Math.Round(((afecto / (user.pais.Impuesto + 1)) * user.pais.Impuesto), 2);
        //BK
        //basse = Math.Round(afecto - impuesto, 2);
        decimal basse = Math.Round(((afecto - impuesto) + noafecto), 2);
        decimal retencion = 0;
        decimal retencion_equivalente = 0;
        decimal temp = 0;

        //Si no existe valor afecto sobre el cual realizar la retencion y si solicita retencion, entonces se hace sobre el valor total
        if (basse == 0)
        {
            basse = Math.Round(valor, 2);
        }

        RE_GenericBean rgb = (RE_GenericBean)DB.getRetencionData(retID);
        //Obtengo el tipo de cambio cuando se genero la provision
        decimal tipo_cambio_prov = DB.getTipoCambioProv(provID);
        int matOpID = DB.getMatrizOperacionID(rgb.intC5, moneda, 1, tipo_contabilidad);
        if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
        rgb.arr1 = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID, "Abono");

        if (rgb.intC2 == 1)
        { //Calculo sobre la base=total-impuesto
            if (basse >= rgb.decC1)
            {
                temp = basse * rgb.decC2 / 100;
                retencion = Math.Round(temp, 2);
            }
        }
        else if (rgb.intC2 == 2)
        { //Calculo sobre el impuesto
            if (impuesto >= rgb.decC1)
            {
                temp = impuesto * rgb.decC2 / 100;
                retencion = Math.Round(temp, 2);
            }
        }
        else if (rgb.intC2 == 3)
        { //Calculo sobre el total
            if (valor >= rgb.decC1)
            {
                temp = valor * rgb.decC2 / 100;
                retencion = Math.Round(temp, 2);
            }
        }
        else if (rgb.intC2 == 4)
        { //Calculo sobre el no afecto
            if (valor >= rgb.decC1)
            {
                temp = noafecto * rgb.decC2 / 100;
                retencion = Math.Round(temp, 2);
            }
        }
        else if (rgb.intC2 == 5)
        {//Calculo con un porcentaje inicial sobre el monto limite y un segundo porcentaje sobre el excedente
            if (basse >= rgb.decC1)
            {
                if (basse <= rgb.decC5)
                {
                    temp = basse * rgb.decC3 / 100;
                    retencion = Math.Round(temp, 2);
                }
                else
                {
                    decimal excedente = basse - rgb.decC5;
                    temp = rgb.decC5 * rgb.decC3 / 100;
                    temp += excedente * rgb.decC4 / 100;
                    retencion = Math.Round(temp, 2);
                }
            }
        }
        if (lb_moneda.SelectedValue.Equals("8"))
        {
            retencion_equivalente = Math.Round(retencion * tipo_cambio_prov, 2);
        }
        else
        {
            retencion_equivalente = Math.Round(retencion / tipo_cambio_prov, 2);
        }
        //obtengo las cuentas contables
        result.intC1 = retID;
        result.decC1 = retencion;
        result.decC2 = retencion_equivalente;
        result.decC3 = tipo_cambio_prov;
        result.intC5 = rgb.intC5;
        result.arr1 = rgb.arr1;
        return result;
    }
    protected void btn_actualizar_libro_compras_Click(object sender, EventArgs e)
    {
        if (Rb_Documento.SelectedValue == "TRUE")
        {
            if (tb_fecha_libro_compras.Text == "")
            {
                WebMsgBox.Show("Debe seleccionar Fecha de Libro de Compras");
                return;
            }
        }
        int resultado = DB.Actualizar_Datos_Libro_Compras(user, int.Parse(lbl_prov_id.Text), bool.Parse(Rb_Documento.SelectedValue), tb_fecha_libro_compras.Text);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de Actualizar los datos del Libro de Compras");
            return;
        }
        else
        {
            WebMsgBox.Show("Datos actualizados exitosamente");
            btn_actualizar_libro_compras.Enabled = false;
            return;
        }
    }
    protected void Rb_Documento_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Rb_Documento.SelectedValue == "FALSE")
        {
            tb_fecha_libro_compras.Text = "";
        }
    }
}
