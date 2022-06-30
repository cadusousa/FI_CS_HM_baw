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
