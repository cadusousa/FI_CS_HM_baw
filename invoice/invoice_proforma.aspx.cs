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
    public string simboloequivalente = "";
    public string simbolomoneda = "";
    int ban = 0;
    Hashtable PublicHT = null;
    int Restriccion_Carga_Ruteada = 0;
    RE_GenericBean Provision = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
        }
        int tipo_contabilidad = 0;

        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];

        #region Habilitar TextBox por Sucursales
        int ban_sucursales = DB.getSucursalesSinDocumentos(user.SucursalID);
        if (ban_sucursales > 0)
        {
            tb_hbl.ReadOnly = false;
            tb_mbl.ReadOnly = false;
            tb_routing.ReadOnly = false;
            tb_contenedor.ReadOnly = false;
        }
        else
        {
            tb_hbl.ReadOnly = true;
            tb_mbl.ReadOnly = true;
            tb_routing.ReadOnly = true;
            tb_contenedor.ReadOnly = true;
        }
        #endregion
        if (!IsPostBack)
        {
            Validar_Restricciones("Load");
        }
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
                tb_registro_no.Text = clienteBean.strC5;//RUC
                tb_giro.Text = clienteBean.strC6;//Giro
            }
        }
        if (!Page.IsPostBack)
        {
            int impo_expo = 0;
            tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            obtengo_listas(tipo_contabilidad, impo_expo, user.PaisID);
            if (tipo_contabilidad == 2)
            {
                lb_tipo_transaccion.SelectedValue = "7";
                lb_contribuyente.SelectedValue = "1";
                lb_contribuyente.Enabled = false;
            }
            else
            {

                lb_tipo_transaccion.SelectedValue = "1";
            }
            if (user.PaisID == 14)
            {
                lb_tipo_transaccion.SelectedValue = "84";
            }
            
            if ((Request.QueryString["bl_no"] != null) && (Request.QueryString["tipo"] != null))
            {
                lb_serie_factura_SelectedIndexChanged(sender, e);
            }
            if (lb_imp_exp.Items.Count > 0)
                impo_expo = int.Parse(lb_imp_exp.SelectedValue);
            if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; simboloequivalente = "Local"; }
            if (ban == 0)
            {
                cargo_datos_BL(impo_expo);
                ban++;
            }
            //Bloqueo de Contribuyente en la Fiscal
            if (user.contaID == 1)
            {
                //lb_contribuyente.Enabled = false;
            }
        }

        tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        if (tipo_contabilidad == 2)
        {
            lb_contribuyente.SelectedValue = "1";
            lb_contribuyente.Enabled = false;
        }
        if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; simboloequivalente = "Local"; }
        gv_detalle.Columns[5].HeaderText = "Subtotal " + simbolomoneda;
        gv_detalle.Columns[6].HeaderText = "Impuesto " + simbolomoneda;
        gv_detalle.Columns[7].HeaderText = "Total " + simbolomoneda;
        gv_detalle.Columns[8].HeaderText = "Equivalente " + simboloequivalente;

        //Desplegando Panel de Datos de Mayan
        if (user.PaisID==13){
            Pnl_Mayan_Logistics.Visible = true;
        }
        if (lb_serie_factura.Items.Count > 0)
        {
            Determinar_Tipo_Serie(user.SucursalID, lb_serie_factura.SelectedItem.Text);
        }
        Definir_Tipo_Operacion();
    }
    private DataTable llenoDataTable(Hashtable ht, int requierealias, long cliID)
    {
        user = (UsuarioBean)Session["usuario"];
        int Restriccion_Carga_Ruteada_y_No_Ruteada = 0;
        bool Validar_Rubro = true;
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
        dt.Columns.Add("CARGOID");
        if (ht != null && ht.Count > 0)
        {
            PublicHT = ht;
            ICollection valueColl = ht.Values;
            Rubros rubtemp = new Rubros();
            Rubros rubBackup = new Rubros();
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            Validar_Restricciones("llenoDataTable");
            #region Validar Restricciones
            ArrayList Arr_Restricciones = (ArrayList)DB.Get_Restricciones_XPais_Tipo(user, user.contaID, 14, user.PaisID, " and a.tbrp_suc_id=" + user.SucursalID + "");//1 Porque es una Factura
            if (Arr_Restricciones.Count > 0)
            {
                foreach (RE_GenericBean Bean in Arr_Restricciones)
                {
                    if (Bean.strC2 == "5")//Facturacion de Carga Ruteada y No Ruteada
                    {
                        Restriccion_Carga_Ruteada_y_No_Ruteada = 1;
                    }
                }
            }
            #endregion
            foreach (Rubros rub in valueColl)
            {
                rubtemp = (Rubros)rub;
                rubBackup = rubtemp;
                if (requierealias == 1) rubtemp.rubroName = DB.getAliasRubro(user.PaisID, (int)rubtemp.rubroID, (int)cliID, rubtemp.rubroName);
                rubtemp = (Rubros)DB.ExistRubroPais(rub, user.PaisID);
                if (rubtemp == null)
                {
                    WebMsgBox.Show("Error, El rubro " + rub.rubroID + " no se encuentra registrado en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlo.");
                    return null;
                }
                decimal tipoCambio = user.pais.TipoCambio;
                double totalD = 0;
                if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))//si debe cobrar iva y el rubro no esta en dolares y no es excento
                {
                    #region Contribuyente
                    if (rubtemp.IvaInc == 1)
                    {
                        #region Iva Incluido
                        if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            //totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user.pais.Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user.pais.Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            //totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        }
                        #endregion
                    }
                    else
                    {
                        #region No Incluye IVA
                        if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            //totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD * (double)user.pais.Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD * (double)user.pais.Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            //totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region Excento
                    if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                    {
                        totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        rubtemp.rubroSubTot = rubtemp.rubroTot;
                    }
                    else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                    {
                        rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        rubtemp.rubroSubTot = rubtemp.rubroTot;
                        totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        //totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        //rubtemp.rubroSubTot = totalD;
                        //totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    }
                    else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                    {
                        rubtemp.rubroTot = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        rubtemp.rubroSubTot = rubtemp.rubroTot;
                        totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        //totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        //rubtemp.rubroSubTot = totalD;
                        //totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    }
                    else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                    {
                        totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        rubtemp.rubroSubTot = rubtemp.rubroTot;
                    }
                    #endregion
                }
                rubtemp.rubroCommentario = "";
                Validar_Rubro = Filtro_Rubros(rubtemp);
                if (Validar_Rubro == true)
                {
                    if (Restriccion_Carga_Ruteada_y_No_Ruteada == 1)
                    {
                        if (tb_routing.Text.Trim().Equals(""))
                        {
                            #region Cargar solo Cargas Locales No Ruteadas que no sean de Terceros
                            if ((rubBackup.rubroTipoCargo == 1) && (rubBackup.rubtoType != "TERCEROS"))//Es un Cargo Local que no se de Terceros
                            {
                                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString() };
                                dt.Rows.Add(obj);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Cargar Cualquier Tipo de Carga que no sea de Terceros
                            if (rubBackup.rubtoType != "TERCEROS")
                            {
                                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString() };
                                dt.Rows.Add(obj);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region Cargar Cualquier Tipo de Carga
                        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString() };
                        dt.Rows.Add(obj);
                        #endregion
                    }
                }
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
                if ((rgb.intC1 != 83) && (rgb.intC1 != 84))
                {
                    lb_tipo_transaccion.Items.Add(item);
                }
                else if (user.PaisID == 14)
                {
                    lb_tipo_transaccion.Items.Add(item);
                }
            }
            if (tipoconta == 2)
            {
                lb_tipo_transaccion.SelectedValue = "7";
            }
            else
            {
                lb_tipo_transaccion.SelectedValue = "1";
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
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 14, user, 1);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
            }
            arr = null;
            if (DB.Validar_Restriccion_Activa(user, 14, 24) == true)
            {
                arr = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(user.PaisID, user.contaID, 14, user.SucursalID);
            }
            else
            {
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_servicio");
            }
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_servicio.Items.Add(item);
            }
            lb_servicio.SelectedIndex = 1;
            arr = null;
            arr = (ArrayList)DB.getTipoFactura();
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipo_factura.Items.Add(item);
            }
            int Doc_ID = 0;
            if (lb_serie_factura.Items.Count==0)
            {
                WebMsgBox.Show("No existe serie definida para esta sucursal");
                return;
            }
            else
            {
                Doc_ID = DB.getDocumentoID(user.SucursalID, lb_serie_factura.SelectedItem.Text, 14, user);
            }
            RE_GenericBean fac_bean = (RE_GenericBean)DB.getFactura(Doc_ID, 14);
            lb_tipo_factura.SelectedValue = fac_bean.intC11.ToString();
            if (lb_servicio.Items.Count > 0)
            {
                ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), lb_tipo_transaccion.SelectedValue);
                RE_GenericBean rubbean = null;
                for (int a = 0; a < rubros.Count; a++)
                {
                    rubbean = (RE_GenericBean)rubros[a];
                    item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
                    lb_rubro.Items.Add(item);
                }
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
        }
    }
    protected void lb_imp_exp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ban == 0)
        {
            int impo_expo = 0;
            if (lb_imp_exp.Items.Count > 0)
                impo_expo = int.Parse(lb_imp_exp.SelectedValue);
            cargo_datos_BL(impo_expo);
            ban++;
        }
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
            if (((user.PaisID == 1) && (servID == 9)) || ((user.PaisID == 14) && (servID == 9)))
            {
                if ((rubbean.intC1 == 394) || (rubbean.intC1 == 687) || (rubbean.intC1 == 688) || (rubbean.intC1 == 689))
                {
                    lb_rubro.Items.Add(item);
                }
            }
            else
            {
                lb_rubro.Items.Add(item);
            }
        }
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (lb_servicio.Items.Count <= 0)
        {
            WebMsgBox.Show("No se existen Tipos de Servicio disponibles");
            return;
        }
        if (lb_rubro.Items.Count <= 0)
        {
            WebMsgBox.Show("No existen Rubros configurados");
            return;
        }
        if (tbCliCod.Text.Equals(""))
        {
            WebMsgBox.Show("Si desea facturar un rubro, debe seleccionar un cliente");
            return;
        }
        if (tb_monto.Text == "0.00")
        {
            WebMsgBox.Show("No se puede agregar un rubro con valor cero");
            tb_cuenta_ModalPopupExtender.Show();
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
        dt.Columns.Add("CARGOID");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9;
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
            lb9 = (Label)row.FindControl("lb_cargoid");
            tb1 = (TextBox)row.FindControl("tb_comentario");
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, tb1.Text, lb9.Text };
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

        #region Backup
        //if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))
        //{
        //    if (rubtemp.IvaInc == 1)
        //    {
        //        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
        //        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
        //    }
        //    else
        //    {
        //        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
        //        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
        //        rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
        //    }
        //}
        //else
        //{
        //    rubtemp.rubroSubTot = rubtemp.rubroTot;
        //}
        //decimal tipoCambio = user.pais.TipoCambio;
        //double totalD = rubtemp.rubroTot;

        //if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
        //{
        //    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
        //}
        //else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
        //{
        //    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto * (double)tipoCambio, 2);
        //    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot * (double)tipoCambio, 2);
        //    //rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
        //    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
        //}
        //else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
        //{
        //    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto / (double)tipoCambio, 2);
        //    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot / (double)tipoCambio, 2);
        //    //rubtemp.rubroTot = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
        //    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
        //}
        //else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
        //{
        //    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
        //}
        #endregion
        decimal tipoCambio = user.pais.TipoCambio;
        double totalD = rubtemp.rubroTot;
        if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))//si debe cobrar iva y el rubro no esta en dolares y no es excento
        {
            #region Contribuyente
            if (rubtemp.IvaInc == 1)
            {
                #region Iva Incluido
                if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                {
                    //totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user.pais.Impuesto)), 2);
                    rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user.pais.Impuesto)), 2);
                    rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    //totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                }
                #endregion
            }
            else
            {
                #region No Incluye IVA
                if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                {
                    //totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    rubtemp.rubroImpuesto = Math.Round(totalD * (double)user.pais.Impuesto, 2);
                    rubtemp.rubroSubTot = Math.Round(totalD, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    rubtemp.rubroImpuesto = Math.Round(totalD * (double)user.pais.Impuesto, 2);
                    rubtemp.rubroSubTot = Math.Round(totalD, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    //totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                }
                #endregion
            }
            #endregion
        }
        else
        {
            #region Excento
            if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
            {
                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                rubtemp.rubroSubTot = rubtemp.rubroTot;
            }
            else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
            {
                rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                rubtemp.rubroSubTot = rubtemp.rubroTot;
                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                //totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                //rubtemp.rubroSubTot = totalD;
                //totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
            }
            else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
            {
                rubtemp.rubroTot = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                rubtemp.rubroSubTot = rubtemp.rubroTot;
                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                //totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                //rubtemp.rubroSubTot = totalD;
                //totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
            }
            else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
            {
                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                rubtemp.rubroSubTot = rubtemp.rubroTot;
            }
            #endregion
        }
        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), "", "0" };//0 Indica que es un rubro agregado del lado del BAW
        dt.Rows.Add(obj);

        gv_detalle.Columns[5].HeaderText = "Subtotal " + simbolomoneda;
        gv_detalle.Columns[6].HeaderText = "Impuesto " + simbolomoneda;
        gv_detalle.Columns[7].HeaderText = "Total " + simbolomoneda;
        gv_detalle.Columns[8].HeaderText = "Equivalente " + simboloequivalente;
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
        #region Validar Sender
        if (sender is Button)
        {
            Button btn_aux = (Button)sender;
            if (btn_aux.ID == "btn_siguiente_rubro")
            {
                tb_cuenta_ModalPopupExtender.Show();
            }
        }
        #endregion
        tb_monto.Text = "0.00";
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
        gv_detalle.Columns[5].HeaderText = "Subtotal en " + simbolomoneda;
        gv_detalle.Columns[6].HeaderText = "Impuesto en " + simbolomoneda;
        gv_detalle.Columns[7].HeaderText = "Total en " + simbolomoneda;
        gv_detalle.Columns[8].HeaderText = "Equivalente en " + simboloequivalente;
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
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        //string script = "window.open('print.aspx?fac_id=" + lb_facid.Text + "&tipo=14','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        string script = "window.open('printersettings.aspx?fac_id=" + lb_facid.Text + "&tipo=14','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        #region Seteo de Parametros de Impresion
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        user.ImpresionBean.Operacion = "1";
        user.ImpresionBean.Tipo_Documento = "14";
        user.ImpresionBean.Id = lb_facid.Text;
        user.ImpresionBean.Impreso = false;
        Session["usuario"] = user;
        #endregion
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
        bt_imprimir.Enabled = false;
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
        factfinal.Fecha_Hora = lb_fecha_hora.Text;//Fecha-Hora de Insercion
        factfinal.cobroID = tipo_cobro;
        factfinal.imp_exp = imp_exp;
        factfinal.Nit = tb_nit.Text.Trim();
        factfinal.Nombre = tb_nombre.Text.Trim();
        factfinal.allIN = tb_allin.Text;
        #region Validacion Longitud del Nombre del Cliente
        if (user.PaisID == 1)
        {
            tb_nombre.Text = tb_nombre.Text.Trim();
            if (tb_nombre.Text.Length > 150)
            {
                //factfinal.Direccion = tb_direccion.Text.Substring(0, 80);
                WebMsgBox.Show("El nombre del cliente tiene mas de 150 caracteres, favor corregir en el Catalogo de Clientes");
                return;
            }
            else
            {
                factfinal.Nombre = tb_nombre.Text.Trim();
            }
        }
        else
        {
            factfinal.Nombre = tb_nombre.Text.Trim();
        }
        #endregion
        #region Validacion Longitud de Direccion
        if (user.PaisID == 1)
        {
            tb_direccion.Text = tb_direccion.Text.Trim();
            if (tb_direccion.Text.Length > 80)
            {
                WebMsgBox.Show("La direccion del cliente tiene mas de 80 caracteres, favor corregir en el Catalogo de Clientes");
                return;
            }
            else
            {
                factfinal.Direccion = tb_direccion.Text.Trim();
            }
        }
        else
        {
            factfinal.Direccion = tb_direccion.Text.Trim();
        }
        #endregion
        factfinal.ReciboAduanal = tb_reciboaduanal.Text;
        factfinal.Recibo_Agencia = tb_recibo_agencia.Text.Trim();
        factfinal.Valor_Aduanero = tb_valor_aduanero.Text.Trim();
        factfinal.Ruc = tb_registro_no.Text.Trim();
        factfinal.Giro = tb_giro.Text.Trim();
        factfinal.Tipo_Factura = int.Parse(lb_tipo_factura.SelectedValue);

        //Para Mayan se guardan el Pais y Ruta
        if (user.PaisID == 13)
        {
            factfinal.Ruta_Pais = lb_ruta_pais.SelectedValue;
            factfinal.Ruta = lb_ruta.SelectedValue;
        }
        if ((factfinal.Nit == null || factfinal.Nit.Equals("")) || (factfinal.Nombre == null || factfinal.Nombre.Equals("")))
        {
            factfinal.Nit = "C/F";
        }
        DateTime fecha_emision = DateTime.Now;
        factfinal.Fecha_Emision = fecha_emision.ToString();
        factfinal.Fecha_Emision = DB.getDateTimeNow();
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
        factfinal.Observaciones = tb_observaciones.Text.Trim();
        factfinal.Otras_Observaciones = tb_otras_observaciones.Text.Trim();
        factfinal.CliID = long.Parse(tbCliCod.Text);
        factfinal.MonedaID = moneda;
        factfinal.TedID = 1;
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
        factfinal.Serie = lb_serie_factura.SelectedValue;
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
        bool valida_cliente = Validar_Restricciones("btn_enviar_cliente");
        if (valida_cliente == true)
        {
            return;
        }
        bool valida_nit = Validar_Restricciones("send_nit");
        if (valida_nit == true)
        {
            return;
        }
        valida_cliente = Validar_Restricciones("btn_cliente_contribuyente");
        if (valida_cliente == true)
        {
            return;
        }
        valida_cliente = Validar_Restricciones("btn_cliente_excento");
        if (valida_cliente == true)
        {
            return;
        }
        valida_cliente = Validar_Restricciones("Validar_Econocaribe");
        if (valida_cliente == true)
        {
            return;
        }
        valida_cliente = Validar_Restricciones("Validar_Cliente_Agente");
        if (valida_cliente == true)
        {
            return;
        }
        valida_cliente = Validar_Restricciones("Documento_Referenciado");
        if (valida_cliente == true)
        {
            return;
        }
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
            lb4 = (Label)row.FindControl("lb_subtotal");
            lb5 = (Label)row.FindControl("lb_impuesto");
            lb6 = (Label)row.FindControl("lb_total");
            lb7 = (Label)row.FindControl("lb_totaldolares");
            lb8 = (Label)row.FindControl("lb_monedatipo");
            lb9 = (Label)row.FindControl("lb_cargoid");
            tb1 = (TextBox)row.FindControl("tb_comentario");
            rubro = new Rubros();
            rubro.rubroID = long.Parse(lb1.Text); ;
            rubro.rubroName = lb2.Text.Trim();
            rubro.rubtoType = lb3.Text;
            rubro.rubroSubTot = double.Parse(lb4.Text);
            rubro.rubroImpuesto = double.Parse(lb5.Text);
            rubro.rubroTot = double.Parse(lb6.Text);
            rubro.rubroTotD = double.Parse(lb7.Text);
            rubro.rubroCommentario = tb1.Text;
            rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
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
        #region Validacion ALL-IN
        if (tb_allin.Text.Trim() != "")
        {
            rubro = new Rubros();
            rubro.rubroID = long.Parse("0");
            rubro.rubroName = tb_allin.Text;
            rubro.rubtoType = "Servicio";
            rubro.rubroSubTot = double.Parse(tb_subtotal.Text);
            rubro.rubroImpuesto = double.Parse(tb_impuesto.Text);
            rubro.rubroTot = double.Parse(tb_total.Text);
            rubro.rubroTotD = double.Parse(tb_totaldolares.Text);
            rubro.rubroCommentario = "";
            rubro.rubroMoneda = long.Parse(lb_moneda.SelectedValue);
            rubro.rubroTypeID = 0;
            if (factfinal.RubrosArr2 == null) factfinal.RubrosArr2 = new ArrayList();
            factfinal.RubrosArr2.Add(rubro);
        }
        #endregion
        if (Bean_IVA.decC2 > 0)
        {
            factfinal.arr1.Add(Bean_IVA);
        }
        if (Bean_IVACERO.stC1 == "IVA")
        {
            factfinal.arr1.Add(Bean_IVACERO);
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
            bool res= Validar_Restricciones("btn_enviar");
            if (res == true)
            {
                return;
            }
            ArrayList result = DB.insertFacturacion_proforma(factfinal, user, tipo_contabilidad);
            if (result == null || result.Count == 0 || int.Parse(result[0].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
            {
                bt_Enviar.Enabled = true;
                WebMsgBox.Show("Error, existio un error al tratar de grabar los datos, por favor intente de nuevo.");
                return;
            }
            else
            {
                bt_imprimir.Enabled = true;
                lb_facid.Text = result[0].ToString();
                tb_correlativo.Text = result[1].ToString();
                user = (UsuarioBean)Session["usuario"];
                gv_detalle.Enabled = false;
                bt_agregar.Enabled = false;
                WebMsgBox.Show("La factura fue grabada exitosamente con el correlativo " + lb_serie_factura.SelectedValue + result[1].ToString());
                bt_Enviar.Enabled = false;
                bt_proforma_virtual.Enabled = true;
                return;
            }
        }
        else
        {
            bt_Enviar.Enabled = false;
            return;
        }
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
        dt.Columns.Add("CARGOID");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9;
        TextBox tb1;
        int indice_anular = 0;
        int ban_anular = 0;
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
            lb9 = (Label)row.FindControl("lb_cargoid");
            if (indice_anular == e.RowIndex)
            {
                if (lb9.Text != "0")
                {
                    ban_anular = 1;
                }
            }
            indice_anular++;
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, tb1.Text, lb9.Text };
            dt.Rows.Add(objArr);
        }
        #region Validar Restriccion Cargos de Trafico
        bool Valida_Restriccion = Validar_Restricciones("gv_detalle_RowDeleting");
        if (Valida_Restriccion == true)
        {
            if (ban_anular == 1)
            {
                WebMsgBox.Show("No se pueden eliminar rubros cargados a partir de un documento, si desea eliminar o modificar rubros porfavor solicitelo al personal de Trafico");
                return;
            }
            else
            {
                dt.Rows[e.RowIndex].Delete();
            }
        }
        else
        {
            dt.Rows[e.RowIndex].Delete();
        }
        #endregion
        gv_detalle.Columns[5].HeaderText = "Subtotal " + simbolomoneda;
        gv_detalle.Columns[6].HeaderText = "Impuesto " + simbolomoneda;
        gv_detalle.Columns[7].HeaderText = "Total " + simbolomoneda;
        gv_detalle.Columns[8].HeaderText = "Equivalente " + simboloequivalente;
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
        Definir_Tipo_Operacion();
    }

    protected void gv_clientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_clientes.SelectedRow;
        double cliID = 0;
        cliID = double.Parse(row.Cells[1].Text);
        string criterio = "a.id_cliente=" + cliID;
        ArrayList clientearr = (ArrayList)DB.getClientes(criterio, user, "");
        RE_GenericBean clienteBean;
        if ((clientearr != null) && (clientearr.Count > 0))
        {
            clienteBean = (RE_GenericBean)clientearr[0];
            if (clienteBean.intC1 == -100)
            {
                WebMsgBox.Show("El cliente no tiene configurado su regimen tributario, por lo tanto no se puede realizar la factura");
                return;
            }
            if ((DB.Validar_Restriccion_Activa(user, 14, 28) == true))
            {
                #region Validacion Grupo Empresas
                if ((user.SucursalID == 9) || (user.SucursalID == 71))
                {
                }
                else if (user.pais.Grupo_Empresas == 1)
                {
                    if (clienteBean.strC8 == "True")
                    {
                        WebMsgBox.Show("No se puede utilizar el Cliente.: " + Page.Server.HtmlDecode(row.Cells[1].Text) + " - " + Page.Server.HtmlDecode(row.Cells[2].Text) + " porque es un Cliente COLOADER");
                        return;
                    }
                }
                else if (user.pais.Grupo_Empresas == 2)
                {
                    if (clienteBean.strC8 == "False")
                    {
                        WebMsgBox.Show("No se puede utilizar el Cliente.: " + Page.Server.HtmlDecode(row.Cells[1].Text) + " - " + Page.Server.HtmlDecode(row.Cells[2].Text) + " porque es un Cliente DIRECTO");
                        return;
                    }
                }
                else if (user.pais.Grupo_Empresas == 3)
                {
                }
                #endregion
            }
            tbCliCod.Text = clienteBean.douC1.ToString();
            tb_razon.Text = clienteBean.strC1;
            tb_nombre.Text = clienteBean.strC2;
            tb_nit.Text = clienteBean.strC3;
            tb_direccion.Text = clienteBean.strC4;
            tb_registro_no.Text = clienteBean.strC5;//Ruc
            tb_giro.Text = clienteBean.strC6;//Giro
            lbl_correo_documento_electronico.Text = clienteBean.strC7;
            Validar_Cliente_Credito_Contado();
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            if (tipo_contabilidad == 2)
            {
                lb_contribuyente.SelectedValue = "1";
            }
            else
            {
                #region Validacion Tipo Contribuyente
                if (lb_contribuyente.SelectedValue != clienteBean.intC1.ToString())
                {
                    lb_contribuyente.SelectedValue = clienteBean.intC1.ToString();
                    Actualizar_Cobro_Impuestos();
                }
                else
                {
                    lb_contribuyente.SelectedValue = clienteBean.intC1.ToString();
                }
                #endregion
            }
            lb_requierealias.Text = clienteBean.intC2.ToString();
            bool res = Validar_Restricciones("gv_clientes_SelectedIndexChanged");
            if (res == false)
            {
                return;
            }
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
        dt.Columns.Add("Tipo");
        foreach (RE_GenericBean rgb in arr)
        {
            object[] objArr = { rgb.douC1, rgb.strC1, rgb.strC9 };
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
        criterio += "es_shipper=true ";
        if ((nombre != null) && (!nombre.Equals("")))
            if (!criterio.Equals(""))
                criterio += " and nombre_cliente like ('%" + nombre + "%')";
            else
                criterio += " nombre_cliente like ('%" + nombre + "%')";
        if ((nit != null) && (!nit.Equals("")))
            if (!criterio.Equals(""))
                criterio += " and codigo_tributario='" + nit + "'";
            else
                criterio = "codigo_tributario='" + nit + "'";
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
        gv_comodities.DataBind();
        modalcomodities.Show();
    }

    protected void lb_serie_factura_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie_factura.Items.Count > 0)
        {
            RE_GenericBean rgb = (RE_GenericBean)DB.getFacturabySerie(lb_serie_factura.SelectedValue, 1, user.SucursalID);
            #region Backup
            /*
            lb_moneda.SelectedValue = rgb.intC5.ToString();
            lb_moneda2.SelectedValue = rgb.intC5.ToString();
            */
            #endregion
            lb_moneda.SelectedValue = user.Moneda.ToString();
            lb_moneda2.SelectedValue = user.Moneda.ToString();
            lb_moneda.Enabled = false;
            if (ban == 0)
            {
                int impo_expo = 0;
                if (lb_imp_exp.Items.Count > 0)
                    impo_expo = int.Parse(lb_imp_exp.SelectedValue);
                cargo_datos_BL(impo_expo);
                ban++;
            }
            int Doc_ID = DB.getDocumentoID(user.SucursalID, lb_serie_factura.SelectedItem.Text, 14, user);
            RE_GenericBean fac_bean = (RE_GenericBean)DB.getFactura(Doc_ID, 14);
            lb_tipo_factura.SelectedValue = fac_bean.intC11.ToString();
        }
        else
        {
            WebMsgBox.Show("No hay serie definida para esta Sucursal");
        }
        //Fin
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/invoice/invoice_proforma.aspx");
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        #region Backup
        /*
        int moneda = DB.getMonedaBySerie(lb_serie_factura.SelectedValue, 1, user.SucursalID);
        lb_moneda.SelectedValue = moneda.ToString();
         */
        #endregion
        lb_moneda.SelectedValue = user.Moneda.ToString();
    }
    protected void gv_navieras_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_navieras_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_navieras.PageIndex = e.NewPageIndex;
        gv_navieras.DataBind();
        modalNavieras.Show();
    }
    protected void gv_navieras_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_navieras.SelectedRow;
        tb_naviera.Text = row.Cells[2].Text;
        ViewState["dt"] = null;
        gv_navieras.DataSource = null;
        gv_navieras.DataBind();
    }
    protected void aceptar_naviera_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre_navieras.Text.Trim().Replace(' ', '%').ToUpper();
        string codigo = tb_codigo_navieras.Text.Trim().ToUpper();
        if (nombre.Equals("") && codigo.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio para la busqueda");
            return;
        }
        string criterio = "";
        if ((nombre != null) && (!nombre.Equals("")))
            criterio += "nombre ilike ('%" + nombre + "%')";
        if (codigo != null && !codigo.Equals(""))
            if (!criterio.Equals(""))
                criterio += " and id_naviera=" + codigo;
            else
                criterio = "id_naviera=" + codigo;
        ArrayList arr = (ArrayList)DB.getNavieras(criterio, "");

        DataTable dt = new DataTable("Navieras");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        if (arr != null)
        {
            foreach (RE_GenericBean rgb in arr)
            {
                object[] objArr = { rgb.intC1, rgb.strC1 };
                dt.Rows.Add(objArr);
            }
        }
        gv_navieras.DataSource = dt;
        gv_navieras.DataBind();
        ViewState["dt"] = dt;
        modalNavieras.Show(); 
    }
    protected void gv_agentes_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_agentes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_agentes.DataSource = dt1;
        gv_agentes.PageIndex = e.NewPageIndex;
        gv_agentes.DataBind();
        modalAgentes.Show();
    }
    protected void gv_agentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_agentes.SelectedRow;
        if ((DB.Validar_Restriccion_Activa(user, 14, 29) == true))
        {
            #region Validacion Grupo Empresas
            if ((user.SucursalID == 9) || (user.SucursalID == 71))
            {
            }
            else if (user.pais.Grupo_Empresas == 1)
            {
            }
            else if (user.pais.Grupo_Empresas == 2)
            {
                if (Page.Server.HtmlDecode(row.Cells[3].Text) == "NO NEUTRAL")
                {
                    WebMsgBox.Show("No se puede utilizar el Agente.: " + Page.Server.HtmlDecode(row.Cells[1].Text) + " - " + Page.Server.HtmlDecode(row.Cells[2].Text) + ", porque es un Agente NO NEUTRAL");
                    return;
                }
            }
            else if (user.pais.Grupo_Empresas == 3)
            {
            }
            #endregion
        }
        tb_agente_nombre.Text = row.Cells[2].Text;
        tb_agenteid.Text = row.Cells[1].Text;
        ViewState["dt"] = null;
        gv_agentes.DataSource = null;
        gv_agentes.DataBind();
    }
    protected void aceptar_agentes_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre_agentes.Text.Trim().Replace(' ', '%').ToUpper();
        string codigo = tb_codigo_agentes.Text.Trim().ToUpper();
        if (nombre.Equals("") && codigo.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio para la busqueda");
            return;
        }
        string criterio = "";
        if ((nombre != null) && (!nombre.Equals("")))
            criterio += " and agente ilike ('%" + nombre + "%')";
        if (codigo != null && !codigo.Equals(""))
            if (!criterio.Equals(""))
                criterio += " and agente_id=" + codigo;
            else
                criterio = " and agente_id=" + codigo;
        ArrayList arr = (ArrayList)DB.getAgentes(criterio);

        DataTable dt = new DataTable("Agentes");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        dt.Columns.Add("Tipo");
        if (arr != null)
        {
            foreach (RE_GenericBean rgb in arr)
            {
                object[] objArr = { rgb.intC1, rgb.strC1, rgb.strC7 };
                dt.Rows.Add(objArr);
            }
        }
        gv_agentes.DataSource = dt;
        gv_agentes.DataBind();
        ViewState["dt"] = dt;
        modalAgentes.Show();
    }
    protected void gv_vendedores1_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_vendedores1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_vendedores1.DataSource = dt1;
        gv_vendedores1.PageIndex = e.NewPageIndex;
        gv_vendedores1.DataBind();
        modalVendedor1.Show();
    }
    protected void gv_vendedores1_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_vendedores1.SelectedRow;
        tb_vendedor1.Text = row.Cells[3].Text;
        ViewState["dt"] = null;
        gv_vendedores1.DataSource = null;
        gv_vendedores1.DataBind();
    }
    protected void aceptar_vendedor1_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre_vendedor1.Text.Trim().Replace(' ', '%').ToUpper();
        string codigo = tb_codigo_vendedor1.Text.Trim().ToUpper();
        if (nombre.Equals("") && codigo.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio para la busqueda");
            return;
        }
        string criterio = "";
        if ((nombre != null) && (!nombre.Equals("")))
            criterio += " and pw_gecos ilike ('%" + nombre + "%')";
        if (codigo != null && !codigo.Equals(""))
            if (!criterio.Equals(""))
                criterio += " and id_usuario=" + codigo;
            else
                criterio = "and id_usuario=" + codigo;
        ArrayList arr = (ArrayList)DB.getProveedor_cajachica(criterio);

        DataTable dt = new DataTable("Vendedores1");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        dt.Columns.Add("Usuario");
        if (arr != null)
        {
            foreach (RE_GenericBean rgb in arr)
            {
                object[] objArr = { rgb.intC1, rgb.strC1, rgb.strC2 };
                dt.Rows.Add(objArr);
            }
        }
        gv_vendedores1.DataSource = dt;
        gv_vendedores1.DataBind();
        ViewState["dt"] = dt;
        modalVendedor1.Show();
    }
    protected void gv_vendedores2_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_vendedores2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_vendedores2.DataSource = dt1;
        gv_vendedores2.PageIndex = e.NewPageIndex;
        gv_vendedores2.DataBind();
        modalVendedor2.Show();
    }
    protected void gv_vendedores2_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_vendedores2.SelectedRow;
        tb_vendedor2.Text = row.Cells[3].Text;
        ViewState["dt"] = null;
        gv_vendedores2.DataSource = null;
        gv_vendedores2.DataBind();
    }
    protected void aceptar_vendedor2_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre_vendedor2.Text.Trim().Replace(' ', '%').ToUpper();
        string codigo = tb_codigo_vendedor2.Text.Trim().ToUpper();
        if (nombre.Equals("") && codigo.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio para la busqueda");
            return;
        }
        string criterio = "";
        if ((nombre != null) && (!nombre.Equals("")))
            criterio += " and pw_gecos ilike ('%" + nombre + "%')";
        if (codigo != null && !codigo.Equals(""))
            if (!criterio.Equals(""))
                criterio += " and id_usuario=" + codigo;
            else
                criterio = "and id_usuario=" + codigo;
        ArrayList arr = (ArrayList)DB.getProveedor_cajachica(criterio);

        DataTable dt = new DataTable("Vendedores2");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        dt.Columns.Add("Usuario");
        if (arr != null)
        {
            foreach (RE_GenericBean rgb in arr)
            {
                object[] objArr = { rgb.intC1, rgb.strC1, rgb.strC2 };
                dt.Rows.Add(objArr);
            }
        }
        gv_vendedores2.DataSource = dt;
        gv_vendedores2.DataBind();
        ViewState["dt"] = dt;
        modalVendedor2.Show();
    }
    protected void aceptar_paquetes2_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre_paquetes2.Text.Trim().Replace(' ', '%').ToUpper();
        string codigo = tb_codigo_paquetes2.Text.Trim().ToUpper();
        if (nombre.Equals("") && codigo.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio para la busqueda");
            return;
        }
        string criterio = "";
        if ((nombre != null) && (!nombre.Equals("")))
            criterio += " tipo ilike ('%" + nombre + "%')";
        if (codigo != null && !codigo.Equals(""))
            if (!criterio.Equals(""))
                criterio += " and tipo_id=" + codigo;
            else
                criterio = " tipo_id=" + codigo;
        ArrayList arr = (ArrayList)DB.getPaquetes(criterio);

        DataTable dt = new DataTable("Paquetes");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        if (arr != null)
        {
            foreach (RE_GenericBean rgb in arr)
            {
                object[] objArr = { rgb.intC1, rgb.strC1 };
                dt.Rows.Add(objArr);
            }
        }
        gv_paquetes2.DataSource = dt;
        gv_paquetes2.DataBind();
        ViewState["dt"] = dt;
        modalPaquetes2.Show();
    }
    protected void gv_paquetes2_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_paquetes2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_paquetes2.DataSource = dt1;
        gv_paquetes2.PageIndex = e.NewPageIndex;
        gv_paquetes2.DataBind();
        modalPaquetes2.Show();
    }
    protected void gv_paquetes2_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_paquetes2.SelectedRow;
        tb_paquetes2.Text = row.Cells[2].Text;
        ViewState["dt"] = null;
        gv_paquetes2.DataSource = null;
        gv_paquetes2.DataBind();
    }
    protected void bt_factura_virtual_Click(object sender, EventArgs e)
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
            string script = "window.open('../invoice/template_invoice.aspx?id=" + lb_facid.Text + "&transaccion=1','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void gv_detalle_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }
    private void cargo_datos_BL(int impo_expo)
    {
        if ((Request.QueryString["bl_no"] != null) && (Request.QueryString["tipo"] != null))
        {
            FacturaBean factura = null;
            string sql, nombre;
            string bl_no = Request.QueryString["bl_no"].ToString().Trim();
            string tipo = Request.QueryString["tipo"].ToString().Trim();
            string blID = Request.QueryString["blid"].ToString();
            string idRouting = "-1";
            #region Validar Restricciones
            ArrayList Arr_Restricciones = (ArrayList)DB.Get_Restricciones_XPais_Tipo(user, user.contaID, 14, user.PaisID, " and a.tbrp_suc_id=" + user.SucursalID + "");//1 Porque es una Factura
            if (Arr_Restricciones.Count > 0)
            {
                foreach (RE_GenericBean Bean in Arr_Restricciones)
                {
                    if (Bean.strC2 == "6")//Cargar Solo Carga Ruteada de lado de Facturacion
                    {
                        Restriccion_Carga_Ruteada = 1;
                    }
                }
            }
            #endregion
            if (tipo.Equals("LCL"))
            {
                // obtengo los datos de la factura 
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataLCL(bl_no, user, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("El documento que esta tratando de facturar ya se encuentra facturado, por favor verificar");
                    return;
                }
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                agente_id = rgb.lonC6;
                id_shipper.Text = rgb.lonC3.ToString();
                id_consignee.Text = rgb.lonC4.ToString();
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
                tb_consignee.Text = rgb_cliente.strC2;
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
                tb_agente_nombre.Text = nombre;
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
                idRouting = rgb.lonC10.ToString();
                tb_routing.Text = DB.getRouting(rgb.lonC10);
                tb_vendedor1.Text = DB.GetVendedorByRouting(tb_routing.Text.Trim());
                tb_contenedor.Text = factura.Contenedor;
                lb_imp_exp.SelectedValue = rgb.strC5;
                //lb_imp_exp.Enabled = false;
                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL(factura.BLID, "L", impo_expo, user, 14, factura.CliID.ToString());
                }
            }
            else if (tipo.Equals("FCL"))
            {
                // obtengo los datos de la factura 
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataFCL(bl_no, user, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("El documento que esta tratando de facturar ya se encuentra facturado, por favor verificar");
                    return;
                }
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                id_shipper.Text = rgb.lonC3.ToString();
                id_consignee.Text = rgb.lonC4.ToString();
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
                tb_consignee.Text = rgb_cliente.strC2;
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
                tb_agente_nombre.Text = nombre;
                // obtengo datos de comodity
                sql = "select namees from commodities where commodityid =" + rgb.lonC7;
                nombre = DB.getName(sql);
                tb_peso.Text = rgb.decC1.ToString();
                tb_vol.Text = rgb.decC2.ToString();
                tb_comodity.Text = nombre.Trim();
                tb_vapor.Text = rgb.strC3;
                tb_agenteid.Text = rgb.lonC6.ToString();
                //tb_paquetes1.Text = rgb.lonC8.ToString();
                tb_paquetes1.Text = rgb.decC20.ToString("#0.00");
                tb_paquetes2.Text = (string)DB.GetNombreTipoBulto(rgb.lonC9);
                lb_contribuyente.SelectedValue = rgb_cliente.intC1.ToString();

                tb_hbl.Text = factura.HBL;
                tb_mbl.Text = factura.MBL;
                idRouting = rgb.lonC10.ToString();
                tb_routing.Text = DB.getRouting(rgb.lonC10);
                tb_vendedor1.Text = DB.GetVendedorByRouting(tb_routing.Text.Trim());
                tb_contenedor.Text = factura.Contenedor;
                lb_imp_exp.SelectedValue = rgb.strC5;
                //lb_imp_exp.Enabled = false;
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
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("El documento que esta tratando de facturar ya se encuentra facturado, por favor verificar");
                    return;
                }
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.MBL = rgb.strC1;
                factura.Contenedor = rgb.strC3;
                factura.CliID = rgb.lonC2;
                // obtengo los datos del cliente
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID);
                if (rgb_cliente == null)
                {
                    WebMsgBox.Show("El cliente con codigo "+ factura.CliID + " no se encuentra registrado para esta transaccion");
                    return;
                }
                if (rgb_cliente.intC1 == -100)
                {
                    WebMsgBox.Show("El cliente no tiene configurado su regimen tributario, por lo tanto no se puede realizar la factura");
                    return;
                }
                // obtengo los datos del user
                sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC2;
                nombre = DB.getName(sql);
                tb_vendedor2.Text = nombre.Trim();

                tb_peso.Text = rgb.decC1.ToString();
                tb_vol.Text = rgb.decC2.ToString();
                tb_paquetes1.Text = rgb.decC3.ToString();
                tb_paquetes2.Text = rgb.strC4;


                //tb_hbl.Text = bl_no;
                tb_hbl.Text = rgb.strC9;
                tb_mbl.Text = factura.MBL;
                tb_routing.Text = bl_no;
                tb_contenedor.Text = factura.Contenedor;
                lb_contribuyente.SelectedValue = rgb_cliente.intC1.ToString();
                lb_contribuyente.SelectedValue = "2";
                lb_hbl.Text = "HBL";
                lb_mbl.Text = "MBL";
                lb_routing.Text = "PICKING";

                tb_naviera.Text = rgb.strC5;
                tb_shipper.Text = rgb.strC6;
                tb_orden.Text = rgb.strC7;
                tb_dua_salida.Text = rgb.strC8;
                tb_comodity.Text = rgb.strC11;
                tb_consignee.Text = rgb.strC10;
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
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("El documento que esta tratando de facturar ya se encuentra facturado, por favor verificar");
                    return;
                }
                #region Determinar Importacion Exportacion
                int Tipo_Operacion = 0;
                string Importacion_Exportacion = "0";
                long aux = 0;
                if (Request.QueryString["opid"] != null)
                {
                    Tipo_Operacion = int.Parse(Request.QueryString["opid"].ToString());
                }
                Importacion_Exportacion = DB.Determinar_ImporExport_XSistema(user, Tipo_Operacion, Convert.ToInt32(QS_BLID)).ToString();
                if (Importacion_Exportacion == "2")
                {
                    aux = rgb.lonC4;
                    rgb.lonC4 = rgb.lonC3;
                    rgb.lonC3 = aux;
                }
                if ((Importacion_Exportacion == "1") || (Importacion_Exportacion == "2"))
                {
                    lb_imp_exp.SelectedValue = Importacion_Exportacion;
                }
                #endregion
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                agente_id = rgb.lonC6;
                id_shipper.Text = rgb.lonC3.ToString();
                id_consignee.Text = rgb.lonC4.ToString();
                int paisdest = Utility.ISOPaistoInt(rgb.strC5);//

                //if (paisdest != user.PaisID) impo_expo = 2; else impo_expo = 1;
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
                tb_consignee.Text = rgb_cliente.strC2;
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
                //sql = "select nombre from proveedores where numero=" + rgb.lonC5;
                //nombre = DB.getName(sql);
                //tb_naviera.Text = nombre.Trim();
                tb_naviera.Text = DB.Get_Provider_By_ID(Convert.ToInt32(rgb.lonC5));
                
                // obtendo datos de agente
                sql = "select agente from agentes where agente_id=" + rgb.lonC6;
                nombre = DB.getName(sql);
                tb_agente_nombre.Text = nombre;
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
                idRouting = DB.GetIDRoutingbyNumero(rgb.strC7);
                tb_routing.Text = rgb.strC7;
                tb_vendedor1.Text = DB.GetVendedorByRouting(tb_routing.Text.Trim());
                tb_contenedor.Text = factura.Contenedor;

                lb_hbl.Text = "CPH";
                lb_mbl.Text = "CP";
                lb_tipotranporte.Text = "TRANSPORTISTA:";
                lb_transporte.Text = "TRANSPORTE:";

                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyTerrestre(factura.BLID, paisdest, user, 14);//importacion=0 exportacion=1
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
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("El documento que esta tratando de facturar ya se encuentra facturado o no indico la Serie y Operacion correcta, por favor verificar");
                    return;
                }
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                agente_id = rgb.lonC6;
                id_shipper.Text = rgb.lonC3.ToString();
                id_consignee.Text = rgb.lonC4.ToString();
                int paisdest = Utility.ISOPaistoInt(rgb.strC9);//
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
                tb_consignee.Text = rgb_cliente.strC2;
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
                tb_agente_nombre.Text = nombre;
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
                idRouting = rgb.lonC8.ToString();
                tb_routing.Text = rgb.strC7;
                tb_vendedor1.Text = DB.GetVendedorByRouting(tb_routing.Text.Trim());
                tb_contenedor.Text = factura.Contenedor;
                lb_hbl.Text = "HAWB";
                lb_mbl.Text = "MAWB";
                lb_tipotranporte.Text = "Linea aerea:";
                lb_transporte.Text = "TRANSPORTE:";
                tb_agenteid.Text = rgb.lonC6.ToString();
                lb_imp_exp.SelectedValue = rgb.strC5;
                //lb_imp_exp.Enabled = false;
                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    //factura.RubrosHT = (Hashtable)DB.getRubrosbyBL_Aereo(factura.BLID, impo_expo, user);//importacion=0 exportacion=1
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL_Aereo(factura.BLID, paisdest, user, rgb, 14);//importacion=0 exportacion=1
                }
            }
            else if (tipo.Equals("RO ADUANAS"))
            {
                // obtengo los datos de la factura 
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceRO_Aduanas(user, bl_no, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un inconveniente al cargar la informacion porfavor pruebe de nuevo");
                    return;
                }
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                id_shipper.Text = rgb.lonC3.ToString();
                id_consignee.Text = rgb.lonC4.ToString();
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
                tb_consignee.Text = rgb_cliente.strC2;
                // obtengo los datos del Vendedor 1
                sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC11;
                nombre = DB.getName(sql);
                tb_vendedor1.Text = nombre.Trim();
                // obtengo los datos del Vendedor 2
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
                tb_agente_nombre.Text = nombre;
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
                idRouting = lbl_blID.Text;
                tb_routing.Text = rgb.strC5;
                tb_contenedor.Text = factura.Contenedor;
                lb_imp_exp.SelectedValue = rgb.strC6;
                tb_observaciones.Text = rgb.strC7.Trim();
                lb_imp_exp.Enabled = false;
                if (factura != null)
                {
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyRO(user, factura.BLID, 13, 1);
                }
            }
            else if (tipo.Equals("RO SEGUROS"))
            {
                // obtengo los datos de la factura 
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceRO_Seguros(user, bl_no, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un inconveniente al cargar la informacion porfavor pruebe de nuevo");
                    return;
                }
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                id_shipper.Text = rgb.lonC3.ToString();
                id_consignee.Text = rgb.lonC4.ToString();
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
                tb_consignee.Text = rgb_cliente.strC2;
                // obtengo los datos del Vendedor 1
                sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC11;
                nombre = DB.getName(sql);
                tb_vendedor1.Text = nombre.Trim();
                // obtengo los datos del Vendedor 2
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
                tb_agente_nombre.Text = nombre;
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
                idRouting = lbl_blID.Text;
                tb_routing.Text = rgb.strC5;
                tb_contenedor.Text = factura.Contenedor;
                lb_imp_exp.SelectedValue = rgb.strC6;
                tb_observaciones.Text = rgb.strC7.Trim();
                lb_imp_exp.Enabled = false;
                if (factura != null)
                {
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyRO(user, factura.BLID, 14, 14);
                }
            }
            else if (tipo.Equals("DEMORAS"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoice_Contenedor_Demoras(user, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un inconveniente al Cargar los Datos de la Demora");
                    return;
                }
                factura = new FacturaBean();
                factura.BLID = Convert.ToInt32(rgb.intC1);
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;
                factura.CliID = rgb.lonC4;
                id_shipper.Text = rgb.lonC3.ToString();
                id_consignee.Text = rgb.lonC4.ToString();
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
                tb_consignee.Text = rgb_cliente.strC2;
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
                tb_agente_nombre.Text = nombre;
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
                idRouting = rgb.lonC10.ToString();
                tb_routing.Text = DB.getRouting(rgb.lonC10);
                tb_vendedor1.Text = DB.GetVendedorByRouting(tb_routing.Text.Trim());
                tb_contenedor.Text = factura.Contenedor;
                lb_imp_exp.SelectedValue = rgb.strC5;
                if (factura != null)
                {
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyDemora(user, rgb.intC1, 14);
                }
            }
            if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; simboloequivalente = "Local"; }

            gv_detalle.Columns[5].HeaderText = "Subtotal " + simbolomoneda;
            gv_detalle.Columns[6].HeaderText = "Impuesto " + simbolomoneda;
            gv_detalle.Columns[7].HeaderText = "Total " + simbolomoneda;
            gv_detalle.Columns[8].HeaderText = "Equivalente " + simboloequivalente;

            gv_detalle.DataSource = (DataTable)llenoDataTable(factura.RubrosHT, factura.alias_rubro, factura.CliID);
            gv_detalle.DataBind();
            //if (gv_detalle.Rows.Count == 0)
            //{
            //    lbl_tipoOperacionID.Text = "10";
            //}
            string criterio = "";
            ArrayList clientearr = null;
            RE_GenericBean clienteBean = null;
            if (factura.CliID > 0)
            {
                criterio = " a.id_cliente=" + factura.CliID;
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
                    tb_registro_no.Text = clienteBean.strC5;//RUC
                    tb_giro.Text = clienteBean.strC6;//Giro
                    lbl_correo_documento_electronico.Text = clienteBean.strC7;
                    Validar_Cliente_Credito_Contado();
                }
            }

            #region Cargar Datos DUA
            if ((user.PaisID == 1) || (user.PaisID == 15))
            {
                RE_GenericBean Bean_DUA = DB.Get_Datos_Aduanas_ByRouting(user, idRouting);
                if (Bean_DUA != null)
                {
                    tb_orden.Text = Bean_DUA.strC2;
                    drp_regimen_aduanero.SelectedItem.Text = Bean_DUA.strC3;
                    tb_dua_ingreso.Text = Bean_DUA.strC4;
                }
            }
            #endregion

            //if (!Page.IsPostBack)
            //{
            //    ArrayList Arr_Sistemas = DB.Get_Detalle_Sistemas(" and b.tto_id=" + lbl_tipoOperacionID.Text + "");
            //    foreach (RE_GenericBean Bean in Arr_Sistemas)
            //    {
            //        lbl_sistema.Text = Bean.strC1;
            //        lbl_operacion.Text = Bean.strC2;
            //    }
            //    pnl_operacion.Visible = true;
            //}
            //else
            //{
            //    pnl_operacion.Visible = false;
            //}
            bool res = Validar_Restricciones("cargo_datos_BL");
            if (res == false)
            {
                return;
            }
        }
    }
    protected bool Validar_Restricciones(string sender)
    {
        bool resultado = false;
        Activar_Desactivar_Campos(true);
        Activar_Desactivar_PopUps(true);
        bool paiR = DB.Validar_Pais_Restringido(user);
        if (paiR == true)
        {
            #region CARGAR A PARTIR DE UN DOCUMENTO
            if (DB.Validar_Restriccion_Activa(user, 14, 1) == true)
            {
                if (lbl_blID.Text != "0")
                {
                    Activar_Desactivar_Campos(false);
                    Activar_Desactivar_PopUps(false);
                    modalcliente.Enabled = true;
                    tb_cuenta_ModalPopupExtender.Enabled = true;
                }
                else
                {
                    Activar_Desactivar_Campos(false);
                    Activar_Desactivar_PopUps(false);
                }
            }
            #endregion
            #region VALIDAR WHITE LIST CLIENTES
            if (DB.Validar_Restriccion_Activa(user, 14, 12) == true)
            {
                modalcliente.Enabled = true;
                lbl_whitelist.Text = DB.Validar_WhiteList(int.Parse(tbCliCod.Text), 3, user.PaisID, user.SucursalID);
                if ((IsPostBack) && (tbCliCod.Text != "0"))
                {
                    if (lbl_tipoOperacionID.Text == "10")
                    {
                        #region Facturacion Manual
                        if (lbl_whitelist.Text == "FALSE")
                        {
                            #region No esta en WhiteList
                            Activar_Desactivar_Campos(false);
                            Activar_Desactivar_PopUps(false);
                            modalcliente.Enabled = true;
                            resultado = true;
                            Limpiar();
                            WebMsgBox.Show("Al cliente con Codigo: " + tbCliCod.Text.Trim() + ", solo se puede Facturar a partir de un Documento");
                            #endregion
                        }
                        else if (lbl_whitelist.Text == "TRUE")
                        {
                            #region Esta en WhiteList
                            Activar_Desactivar_Campos(true);
                            Activar_Desactivar_PopUps(true);
                            resultado = false;
                            #endregion
                        }
                        #endregion
                    }
                    else if (lbl_tipoOperacionID.Text != "10")
                    {
                        #region Facturacion a Partir de un Documento
                        Activar_Desactivar_Campos(false);
                        Activar_Desactivar_PopUps(false);
                        modalcliente.Enabled = true;
                        tb_cuenta_ModalPopupExtender.Enabled = true;
                        resultado = false;
                        #endregion
                    }
                }
            }
            #endregion;
            #region SI SE COBRAN RUBROS DE ADUANAS INGRESAR POLIZA Y REGIMEN ADUANERO
            if ((DB.Validar_Restriccion_Activa(user, 14, 2) == true) && (sender == "btn_enviar"))
            {
                int servicioID = 0;
                resultado = false;
                foreach (GridViewRow row in gv_detalle.Rows)
                {
                    Label lb = (Label)row.FindControl("lb_tipo");
                    servicioID = Utility.TraducirServiciotoINT(lb.Text);
                    if (servicioID == 9)//ADUANAS
                    {
                        if (tb_orden.Text.Trim().Equals(""))
                        {
                            WebMsgBox.Show("Debe Ingresar la Poliza Aduanal");
                            resultado = true;
                            return resultado;
                        }
                        else if (drp_regimen_aduanero.SelectedItem.Text == "Seleccione...")
                        {
                            WebMsgBox.Show("Debe seleccionar el regimen aduanero");
                            resultado = true;
                            return resultado;
                        }
                    }
                }
            }
            #endregion
            #region NO SE PUEDEN AGREGAR RUBROS DE TERCEROS
            if ((DB.Validar_Restriccion_Activa(user, 14, 3) == true) && (sender == "btn_enviar"))
            {
                int servicioID = 0;
                resultado = false;
                foreach (GridViewRow row in gv_detalle.Rows)
                {
                    Label lb = (Label)row.FindControl("lb_tipo");
                    servicioID = Utility.TraducirServiciotoINT(lb.Text);
                    if (servicioID == 14)//TERCEROS
                    {
                        WebMsgBox.Show("No se puede Facturar un Rubro de Terceros, Los Rubros de Terceros solo se pueden aplicar en una Nota de Debito, Porfavor eliminelo(s).");
                        resultado = true;
                        return resultado;
                    }
                }
            }
            #endregion
            #region NO PERMITIR QUE SE INGRESEN RUBROS REPETIDOS
            if ((DB.Validar_Restriccion_Activa(user, 14, 4) == true) && (sender == "btn_enviar"))
            {
                if (gv_detalle.Rows.Count > 0)
                {
                    //EXTRAIGO TODOS LOS RUBROS DEL GV Y DE CADA LABEL A UNA MATRIZ
                    int cantidad_rubros_cpd = 0;
                    int cantidad_rubros = gv_detalle.Rows.Count;
                    string[,] m_rubros = new string[cantidad_rubros, 2];
                    int fila = 0;
                    resultado = false;
                    foreach (GridViewRow row in gv_detalle.Rows)
                    {
                        Label lb1 = (Label)row.FindControl("lb_codigo");
                        Label lb2 = (Label)row.FindControl("lb_tipo");
                        Label lb9 = (Label)row.FindControl("lb_cargoid");
                        m_rubros[fila, 0] = lb1.Text;
                        m_rubros[fila, 1] = lb2.Text;
                        fila++;
                        if (lb9.Text != "0")
                        {
                            cantidad_rubros_cpd++;
                        }
                    }

                    #region Si es FA CPD validar que se facture al menos un rubro del BL
                    /*
                                        if ((cantidad_rubros_cpd == 0) && (lbl_blID.Text != "0"))
                                        {
                                            WebMsgBox.Show("Debe Facturar al menos un Rubro del BL porque usted esta Facturando a Partir de un Documento");
                                            resultado = false;
                                            return resultado;
                                        }
                                         */
                    #endregion

                    //VALIDO LOS RUBROS REPETIDOS
                    for (int a = 0; a < (cantidad_rubros - 1); a++)
                    {
                        for (int b = (a + 1); b < cantidad_rubros; b++)
                        {
                            if ((m_rubros[a, 0] == m_rubros[b, 0]) && (m_rubros[a, 1] == m_rubros[b, 1]))
                            {
                                WebMsgBox.Show("No se pueden facturar rubros repetidos, Porfavor elimine los rubros repetidos");
                                resultado = true;
                                return resultado;
                            }
                        }
                    }
                }
            }
            #endregion
            #region FACTURACION DE CARGA RUTEADA Y NO RUTEADA
            if ((DB.Validar_Restriccion_Activa(user, 14, 5) == true) && (sender == "llenoDataTable"))
            {
                if (tb_routing.Text.Trim().Equals(""))
                {
                    int Cant_Cargos_Internacionales = 0;
                    int Cant_Cargos_Sin_Clasificar = 0;
                    int Cant_Cargos_Terceros = 0;
                    resultado = true;
                    if ((PublicHT != null) && (PublicHT.Count > 0))
                    {
                        ICollection HT_Rubros = PublicHT.Values;
                        Rubros rubro_temp = new Rubros();
                        foreach (Rubros rub in HT_Rubros)
                        {
                            rubro_temp = (Rubros)rub;
                            if (rubro_temp.rubroTipoCargo == -1)//Cargo sin Clasificacion
                            {
                                Cant_Cargos_Sin_Clasificar++;
                            }
                            if ((rubro_temp.rubroTipoCargo == 0) && (rubro_temp.rubtoType != "TERCEROS"))//Es un Cargo Internacional y no es de Terceros
                            {
                                Cant_Cargos_Internacionales++;
                            }
                            if ((rubro_temp.rubtoType == "TERCEROS"))//Es un Cargo de Terceros
                            {
                                Cant_Cargos_Terceros++;
                            }
                        }
                        string mensaje = "";
                        if (Cant_Cargos_Sin_Clasificar > 0)
                        {
                            mensaje = "Existen " + Cant_Cargos_Sin_Clasificar + " Cargo(s) sin clasificacion, porfavor contacte al personal de Trafico. ";
                        }
                        if (Cant_Cargos_Internacionales > 0)
                        {
                            if (mensaje == "")
                            {
                                mensaje = "Existen " + Cant_Cargos_Internacionales + " Cargo(s) Internacional(es) para el/los cual(es) debera aplicar Nota de Debito";
                            }
                            else
                            {
                                mensaje += " y Existen " + Cant_Cargos_Internacionales + " Cargo(s) Internacional(es) para el/los cual(es) debera aplicar Nota de Debito";
                            }
                        }
                        if (Cant_Cargos_Terceros > 0)
                        {
                            if (mensaje == "")
                            {
                                mensaje = "Existen " + Cant_Cargos_Terceros + " Cargo(s) por Terceros para el/los cual(es) debera aplicar Nota de Debito";
                            }
                            else
                            {
                                mensaje += " y Existen " + Cant_Cargos_Terceros + " Cargo(s) por Terceros para el/los cual(es) debera aplicar Nota de Debito";
                            }
                        }
                        if (mensaje != "")
                        {
                            WebMsgBox.Show(mensaje);
                        }
                        return resultado;
                    }
                }
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
            #region CLIENTES CONTRIBUYENTES
            if ((DB.Validar_Restriccion_Activa(user, 14, 18) == true) && (sender == "btn_cliente_contribuyente"))
            {
                resultado = false;
                int rubros_excentos = 0;
                if (gv_detalle.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gv_detalle.Rows)
                    {
                        Label lb1 = (Label)row.FindControl("lb_impuesto");
                        if (decimal.Parse(lb1.Text) == 0)
                        {
                            rubros_excentos++;
                        }
                    }
                }
                if (lb_contribuyente.SelectedValue == "2")
                {
                    if (rubros_excentos > 0)
                    {
                        WebMsgBox.Show("El cliente de la factura es contribuyente de Impuestos, por lo cual no pueden cobrarse rubros excentos de impuesto");
                        resultado = true;
                    }
                }
                return resultado;
            }
            #endregion
            #region CLIENTES EXCENTOS
            if ((DB.Validar_Restriccion_Activa(user, 14, 19) == true) && (sender == "btn_cliente_excento"))
            {
                resultado = false;
                int rubros_contribuyentes = 0;
                if (gv_detalle.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gv_detalle.Rows)
                    {
                        Label lb1 = (Label)row.FindControl("lb_impuesto");
                        if (decimal.Parse(lb1.Text) > 0)
                        {
                            rubros_contribuyentes++;
                        }
                    }
                }
                if (lb_contribuyente.SelectedValue == "1")
                {
                    if (rubros_contribuyentes > 0)
                    {
                        WebMsgBox.Show("El cliente de la factura es excento de Impuestos, por lo cual no pueden cobrarse rubros con impuesto");
                        resultado = true;
                    }
                }
                return resultado;
            }
            #endregion
            #region ELIMINACION DE RUBROS TRAFICOS
            if ((DB.Validar_Restriccion_Activa(user, 14, 20) == true) && (sender == "gv_detalle_RowDeleting"))
            {
                resultado = true;
            }
            #endregion
            #region ELIMINAR Y AGREGAR RUBROS
            if ((DB.Validar_Restriccion_Activa(user, 14, 21) == true) && (sender == "cargo_datos_BL"))
            {
                if (lbl_tipoOperacionID.Text != "10")
                {
                    gv_detalle.Columns[0].Visible = false;
                    bt_agregar.Visible = false;
                }
                //if (user.SucursalID == 11)
                //{
                //    bt_agregar.Visible = true;
                //}
                //resultado = true;
            }
            #endregion
            #region SOLO AGREGAR RUBROS
            if ((DB.Validar_Restriccion_Activa(user, 14, 23) == true) && (sender == "cargo_datos_BL"))
            {
                if (lbl_tipoOperacionID.Text != "10")
                {
                    gv_detalle.Columns[0].Visible = true;
                    bt_agregar.Visible = false;
                }
                //if (user.SucursalID == 11)
                //{
                //    gv_detalle.Columns[0].Visible = false;
                //    bt_agregar.Visible = true;
                //}
                //resultado = true;
            }
            #endregion
            #region NO EMITIR COBRO MANUAL CLIENTE ECONO
            if ((DB.Validar_Restriccion_Activa(user, 14, 25) == true) && (sender == "Validar_Econocaribe"))
            {
                resultado = false;
                if (lbl_tipoOperacionID.Text == "10")
                {
                    if (user.pais.Grupo_Empresas == 2)
                    {
                        int res = DB.Validar_Persona_Grupo_Econocaribe(user, int.Parse(tbCliCod.Text), 3);
                        if (res > 0)
                        {
                            WebMsgBox.Show("El Cliente Econocaribe no puede ser utilizado en modulo.: " + user.pais.Nombre_Sistema + ", debe utilizar el Modulo de Aimar");
                            resultado = true;
                        }
                    }
                }
                return resultado;
            }
            #endregion
            #region COBRO CLIENTE COLOADER Y AGENTE ECONO
            if ((DB.Validar_Restriccion_Activa(user, 14, 27) == true) && (sender == "Validar_Cliente_Agente"))
            {
                if ((user.pais.Grupo_Empresas == 1) && (tbCliCod.Text.Trim() != "") && (tb_agenteid.Text.Trim() != "0"))
                {
                    int v_cliID = 0;
                    int v_agenteID = 0;
                    v_cliID = int.Parse(tbCliCod.Text);
                    v_agenteID = int.Parse(tb_agenteid.Text);
                    string v_sql = "a.id_cliente=" + v_cliID;
                    ArrayList V_ClienteArr = (ArrayList)DB.getClientes(v_sql, user, "");
                    RE_GenericBean V_Cliente_Bean = (RE_GenericBean)V_ClienteArr[0];
                    int V_Agente_Econo = DB.Validar_Persona_Grupo_Econocaribe(user, v_agenteID, 2);
                    if (V_Cliente_Bean.strC8 == "False")
                    {
                        resultado = false;
                    }
                    else if ((V_Cliente_Bean.strC8 == "True") && (V_Agente_Econo > 0))
                    {
                        resultado = false;
                    }
                    else if ((V_Cliente_Bean.strC8 == "True") && (V_Agente_Econo == 0))
                    {
                        resultado = true;
                        WebMsgBox.Show("No puede realizar Operaciones a Clientes Coloaders desde este Modulo, Favor comuniquese con su Supervisor");
                        bt_Enviar.Enabled = false;
                        Cancel.Enabled = false;
                        bt_imprimir.Enabled = false;
                    }
                }
            }
            #endregion
            #region COBRO DOCUMENTOS REFERENCIADOS
            if (DB.Validar_Restriccion_Activa(user, 14, 30) == true)
            {
                resultado = false;
                if (sender == "Definir_Tipo_Operacion")
                {
                    gv_detalle.Columns[0].Visible = true;
                    bt_agregar.Visible = true;
                }
                else if (sender == "Documento_Referenciado")
                {
                    resultado = false;
                }
            }
            else
            {
                if ((lbl_tipoOperacionID.Text == "15") && (sender == "Documento_Referenciado"))
                {
                    WebMsgBox.Show("Factura Referenciada. No se pueden Cobrar Rubros agregados manualmente, los Rubros deben proceder de los Sistemas de Trafico");
                    resultado = true;
                }
            }
            #endregion
        }
        return resultado;
    }
    protected string Obtener_Tipo_Operacion()
    {
        string Operacion = "10";
        if (Request.QueryString["opid"] != null)
        {
            Operacion = Request.QueryString["opid"].ToString();
        }
        return Operacion;
    }

    protected void Limpiar()
    {
        #region Limpiar
        tb_mbl.Text = "";
        tb_hbl.Text = "";
        tb_routing.Text = "";
        tb_contenedor.Text = "";
        tb_peso.Text = "";
        tb_vol.Text = "";
        tb_vapor.Text = "";
        tb_paquetes1.Text = "";
        tb_naviera.Text = "";
        tb_agente_nombre.Text = "";
        tb_agenteid.Text = "0";
        tb_shipper.Text = "";
        tb_consignee.Text = "";
        tb_comodity.Text = "";
        tb_paquetes1.Text = "";
        tb_paquetes2.Text = "";
        tb_vendedor1.Text = "";
        tb_vendedor2.Text = "";
        tb_orden.Text = "";
        tb_dua_ingreso.Text = "";
        tb_dua_salida.Text = "";
        tb_reciboaduanal.Text = "";
        tb_recibo_agencia.Text = "";
        tb_valor_aduanero.Text = "";
        gv_detalle.DataBind();
        #endregion
    }
    protected void Determinar_Tipo_Serie(int sucID, string serie)
    {
        int Doc_ID = DB.getDocumentoID(sucID, serie, 14, user);
        lbl_serie_id.Text = Doc_ID.ToString();
        RE_GenericBean Bean_Serie = (RE_GenericBean)DB.getFactura(Doc_ID, 14);
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
    protected void btn_ver_pdf_Click(object sender, EventArgs e)
    {
        if (!DB.DownloadFEL(lb_facid.Text, "1", "PDF", Response, lbl_internal_reference.Text, user.PaisID, true, true))
            DB.OpenGFACEpdf(lbl_internal_reference.Text, Response);
    }

    protected bool Filtro_Rubros(Rubros Rubro)
    {
        bool resultado = true;
        #region Aimar Guatemala
        if (user.PaisID == 1)
        {
            if (Rubro.rubtoType == "ADUANAS")
            {
                if ((Rubro.rubroID == 212) || (Rubro.rubroID == 133) || (Rubro.rubroID == 216) || (Rubro.rubroID == 232) || (Rubro.rubroID == 63) || (Rubro.rubroID == 243) || (Rubro.rubroID == 303) || (Rubro.rubroID == 125) || (Rubro.rubroID == 123) || (Rubro.rubroID == 304) || (Rubro.rubroID == 682) || (Rubro.rubroID == 439) || (Rubro.rubroID == 69) || (Rubro.rubroID == 70) || (Rubro.rubroID == 665) || (Rubro.rubroID == 647) || (Rubro.rubroID == 646) || (Rubro.rubroID == 196) || (Rubro.rubroID == 58) || (Rubro.rubroID == 492))
                {
                    resultado = false;
                }
            }
        }
        #endregion
        #region Aproa
        else if (user.PaisID == 14)
        {
            resultado = false;
            if (Rubro.rubtoType == "ADUANAS")
            {
                if ((Rubro.rubroID == 212) || (Rubro.rubroID == 133) || (Rubro.rubroID == 216) || (Rubro.rubroID == 232) || (Rubro.rubroID == 63) || (Rubro.rubroID == 243) || (Rubro.rubroID == 303) || (Rubro.rubroID == 125) || (Rubro.rubroID == 123) || (Rubro.rubroID == 304) || (Rubro.rubroID == 682) || (Rubro.rubroID == 439) || (Rubro.rubroID == 69) || (Rubro.rubroID == 70) || (Rubro.rubroID == 665) || (Rubro.rubroID == 647) || (Rubro.rubroID == 646) || (Rubro.rubroID == 196) || (Rubro.rubroID == 58) || (Rubro.rubroID == 492) || (Rubro.rubroID == 688))
                {
                    resultado = true;
                }
            }
        }
        #endregion
        return resultado;
    }
    protected void bt_proforma_virtual_Click(object sender, EventArgs e)
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
            string script = "window.open('../invoice/template_invoice.aspx?id=" + lb_facid.Text + "&transaccion=14','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void Activar_Desactivar_Campos(bool Activar)
    {
        if (Activar == true)
        {
            #region Activar
            tb_mbl.ReadOnly = false;
            tb_hbl.ReadOnly = false;
            tb_routing.ReadOnly = false;
            tb_contenedor.ReadOnly = false;
            tb_peso.ReadOnly = false;
            tb_vol.ReadOnly = false;
            tb_vapor.ReadOnly = false;
            tb_cuenta_ModalPopupExtender.Enabled = true;
            modalPaquetes2.Enabled = true;
            tb_paquetes1.ReadOnly = false;
            lbl_restricciones.Text = "FALSE";
            #endregion
        }
        else
        {
            #region Desactivar
            tb_mbl.ReadOnly = true;
            tb_hbl.ReadOnly = true;
            tb_routing.ReadOnly = true;
            tb_contenedor.ReadOnly = true;
            tb_peso.ReadOnly = true;
            tb_vol.ReadOnly = true;
            tb_vapor.ReadOnly = true;
            tb_cuenta_ModalPopupExtender.Enabled = false;
            modalPaquetes2.Enabled = true;
            tb_paquetes1.ReadOnly = true;
            lbl_restricciones.Text = "TRUE";
            #endregion
        }

    }
    protected void Activar_Desactivar_PopUps(bool Activar)
    {
        if (Activar == true)
        {
            #region Activar
            tb_cuenta_ModalPopupExtender.Enabled = true;
            modalPaquetes2.Enabled = true;
            modalcliente.Enabled = true;
            modalAgentes.Enabled = true;
            modalNavieras.Enabled = true;
            modalcomodities.Enabled = true;
            modalconsignee.Enabled = true;
            modalVendedor1.Enabled = true;
            modalVendedor2.Enabled = true;
            #endregion
        }
        else
        {
            #region Desactivar
            tb_cuenta_ModalPopupExtender.Enabled = false;
            modalPaquetes2.Enabled = false;
            modalcliente.Enabled = false;
            modalAgentes.Enabled = false;
            modalNavieras.Enabled = false;
            modalcomodities.Enabled = false;
            modalconsignee.Enabled = false;
            modalVendedor1.Enabled = false;
            modalVendedor2.Enabled = false;
            #endregion
        }
    }
    protected void Actualizar_Cobro_Impuestos()
    {
        if (lb_contribuyente.Items.Count > 0)
        {
            if (gv_detalle.Rows.Count > 0)
            {
                #region Recalcular valores monetarios
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
                dt.Columns.Add("CARGOID");
                dt.Clear();
                Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9;
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
                    lb9 = (Label)row.FindControl("lb_cargoid");
                    tb1 = (TextBox)row.FindControl("tb_comentario");
                    Rubros rubro = new Rubros();
                    rubro.rubroID = long.Parse(lb1.Text);
                    rubro.rubroName = lb2.Text;
                    int cliID = int.Parse(tbCliCod.Text);
                    int requierealias = int.Parse(lb_requierealias.Text);
                    if (requierealias == 1) rubro.rubroName = DB.getAliasRubro(user.PaisID, rubro.rubroID, cliID, rubro.rubroName);
                    rubro.rubtoType = lb3.Text;
                    rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
                    rubro.rubroTypeID = Utility.TraducirServiciotoINT(lb3.Text);
                    rubro.rubroTot = double.Parse(lb6.Text);
                    Rubros rubtemp = (Rubros)rubro;
                    rubtemp = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
                    if (rubtemp == null)
                    {
                        WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                        return;
                    }
                    int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
                    decimal tipoCambio = user.pais.TipoCambio;
                    double totalD = rubtemp.rubroTot;
                    if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))//si debe cobrar iva y el rubro no esta en dolares y no es excento
                    {
                        #region Contribuyente
                        if (rubtemp.IvaInc == 1)
                        {
                            #region Iva Incluido
                            if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                            {
                                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                            {
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                                rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user.pais.Impuesto)), 2);
                                rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                            {
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                                rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user.pais.Impuesto)), 2);
                                rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                            {
                                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            }
                            #endregion
                        }
                        else
                        {
                            #region No Incluye IVA
                            if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                            {
                                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                            {
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                                rubtemp.rubroImpuesto = Math.Round(totalD * (double)user.pais.Impuesto, 2);
                                rubtemp.rubroSubTot = Math.Round(totalD, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                            {
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                                rubtemp.rubroImpuesto = Math.Round(totalD * (double)user.pais.Impuesto, 2);
                                rubtemp.rubroSubTot = Math.Round(totalD, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                            {
                                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region Excento
                        if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = rubtemp.rubroTot;
                        }
                        else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = rubtemp.rubroTot;
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = rubtemp.rubroTot;
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = rubtemp.rubroTot;
                        }
                        #endregion
                    }
                    object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), lb8.Text, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), tb1.Text, lb9.Text };
                    dt.Rows.Add(obj);
                }
                gv_detalle.Columns[5].HeaderText = "Subtotal " + simbolomoneda;
                gv_detalle.Columns[6].HeaderText = "Impuesto " + simbolomoneda;
                gv_detalle.Columns[7].HeaderText = "Total " + simbolomoneda;
                gv_detalle.Columns[8].HeaderText = "Equivalente " + simboloequivalente;
                gv_detalle.DataSource = dt;
                gv_detalle.DataBind();
                #endregion
            }
        }
    }
    protected void Validar_Cliente_Credito_Contado()
    {
        #region Validacion Cliente de Credito o Contado
        if (tbCliCod.Text != "0")
        {
            btnInfo.Font.Bold = true;
            RE_GenericBean Credito_Bean = (RE_GenericBean)DB.getCredito_Cliente(user.PaisID, int.Parse(tbCliCod.Text));
            if (Credito_Bean == null)
            {
                btnInfo.Text = "Cliente de Contado +";
                btnInfo.ToolTip = "De clic para ver Detalle de Credito";
                pnl_credito.Visible = true;
                lbl_tiempo_autorizado.Text = "0";
                lbl_monto_autorizado.Text = decimal.Parse("0").ToString("#,#.00#;(#,#.00#)");
            }
            else
            {
                btnInfo.Text = "Cliente de Credito +";
                btnInfo.ToolTip = "De clic para ver Detalle de Credito";
                pnl_credito.Visible = true;
                lbl_tiempo_autorizado.Text = Credito_Bean.strC5;
                lbl_monto_autorizado.Text = decimal.Parse(Credito_Bean.strC7).ToString("#,#.00#;(#,#.00#)");
            }
        }
        #endregion
    }
    protected void Definir_Tipo_Operacion()
    {
        #region Definir Tipo Operacion
        if (Request.QueryString["blid"] == null)
        {
            lbl_tipoOperacionID.Text = "10";
        }
        else
        {
            int Cantidad_Rubros_Traficos = 0;
            GridViewRowCollection gvr = gv_detalle.Rows;
            Label lbl = new Label();
            foreach (GridViewRow row in gvr)
            {
                lbl = (Label)row.FindControl("lb_cargoid");
                if (lbl.Text != "0")
                {
                    Cantidad_Rubros_Traficos++;
                }
            }
            if (Cantidad_Rubros_Traficos == 0)
            {
                lbl_tipoOperacionID.Text = "15";
                Validar_Restricciones("Definir_Tipo_Operacion");
            }
            else if (Cantidad_Rubros_Traficos > 0)
            {
                lbl_tipoOperacionID.Text = lbl_tipoOperacionID.Text;
            }
        }
        ArrayList Arr_Sistemas = DB.Get_Detalle_Sistemas(" and b.tto_id=" + lbl_tipoOperacionID.Text + "");
        foreach (RE_GenericBean Bean in Arr_Sistemas)
        {
            lbl_sistema.Text = Bean.strC1;
            lbl_operacion.Text = Bean.strC2;
        }
        pnl_operacion.Visible = true;
        #endregion
    }
}
