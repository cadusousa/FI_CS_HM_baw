
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
    long agente_id = 0;
    public string simboloequivalente = "";
    public string simbolomoneda = "";
    int ban = 0;
    Hashtable PublicHT = null;
    int Restriccion_Carga_Ruteada = 0;
    RE_GenericBean Provision = null;
    int ID = 0;

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
        if (!IsPostBack)
        {
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
                if (user.PaisID != 5)
                {
                    lb_contribuyente.SelectedValue = "1";
                }
                lb_contribuyente.Enabled = false;
            }
            else
            {

                lb_tipo_transaccion.SelectedValue = "1";
            }
            
            //Definir Moneda Inicial*************************************
            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 1);
            lb_moneda.SelectedValue = moneda_inicial.ToString();
            //***********************************************************

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
                if ((user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13))
                {
                    lb_contribuyente.Enabled = true;
                }
                else
                {
                    lb_contribuyente.Enabled = false;                
                }
            }
        }

        tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        if (tipo_contabilidad == 2)
        {
            if (user.PaisID != 5)
            {
                lb_contribuyente.SelectedValue = "1";
            }
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
        if ((lb_serie_factura.Items.Count > 0) &&(lb_serie_factura.SelectedValue != "0"))
        {
            Determinar_Tipo_Serie(user.SucursalID, lb_serie_factura.SelectedItem.Text);
        }
        Definir_Tipo_Operacion();
        lb_tipo_transaccion.Enabled = false;
        lb_contribuyente.Enabled = true;

        #region Bloqueos Facturacion Electronica Costa Rica
        if ((user.PaisID == 5) || (user.PaisID == 21))
        {
            if (user.SucursalID == 117)
            {
                bt_imprimir.Visible = false;
                pnl_tipo_identificacion_cliente.Visible = true;
            }
        }
        #endregion
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
        dt.Columns.Add("LOCAL_INTERNACIONAL");
        if (ht != null && ht.Count > 0)
        {
            PublicHT = ht;
            ICollection valueColl = ht.Values;
            Rubros rubtemp = new Rubros();
            Rubros rubBackup = new Rubros();
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            Validar_Restricciones("llenoDataTable");
            #region Validar Restricciones
            ArrayList Arr_Restricciones = (ArrayList)DB.Get_Restricciones_XPais_Tipo(user, user.contaID, 1, user.PaisID, " and a.tbrp_suc_id=" + user.SucursalID + "");//1 Porque es una Factura
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
                if (((rubtemp.CobIva == 1) && (lb_contribuyente.SelectedValue.Equals("2")) && (tipo_contabilidad == 1) && (rubBackup.rubtoType != "TERCEROS")) || ((rubtemp.CobIva == 1) && (lb_contribuyente.SelectedValue.Equals("2")) && (tipo_contabilidad == 2) && (user.PaisID == 5)))
                {
                    Decimal user_pais_Impuesto = DB.user_pais_Impuesto(lb_imp_exp.SelectedValue.ToString(), user.PaisID.ToString(), user.pais.Impuesto);                    
					#region Contribuyente
                    if (rubtemp.IvaInc == 1)
                    {
                        #region Iva Incluido
                        if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user_pais_Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user_pais_Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user_pais_Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user_pais_Impuesto)), 2);
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
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user_pais_Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD * (double)user_pais_Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD * (double)user_pais_Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user_pais_Impuesto, 2);
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
                rubtemp.rubroCommentario = "";
                Validar_Rubro = Filtro_Rubros(rubtemp);
                if (Validar_Rubro == true)
                {
                    #region Traducir Tipo de Cargo
                    string Tipo_Cargo = "--";
                    if (rubBackup.rubroTipoCargo == 1)
                    {
                        Tipo_Cargo = "LOCAL";
                    }
                    else
                    {
                        Tipo_Cargo = "INTERNACIONAL";
                    }
                    #endregion
                    if (Restriccion_Carga_Ruteada_y_No_Ruteada == 1)
                    {
                        if (tb_routing.Text.Trim().Equals(""))
                        {
                            #region Cargar solo Cargas Locales No Ruteadas que no sean de Terceros
                            if ((rubBackup.rubroTipoCargo == 1) && (rubBackup.rubtoType != "TERCEROS"))//Es un Cargo Local que no se de Terceros
                            {
                                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                dt.Rows.Add(obj);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Cargar Cualquier Tipo de Carga que no sea de Terceros
                            if (rubBackup.rubtoType != "TERCEROS")
                            {
                                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                dt.Rows.Add(obj);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region Cargar Cualquier Tipo de Carga
                        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
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

            //CAMBIAR PARA MANEJAR FISCAL USD Y FISCAL LOCAL
            arr = null;
            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(1, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serie_factura.Items.Clear();
            lb_serie_factura.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {   
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serie_factura.Items.Add(item);
            }
            arr = null;
            if (DB.Validar_Restriccion_Activa(user, 1, 24) == true)
            {
                arr = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(user.PaisID, user.contaID, 1, user.SucursalID);
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
            if (lb_serie_factura.Items.Count == 0)
            {
                WebMsgBox.Show("No existe serie definida para esta sucursal");
                return;
            }
            else
            {
                Doc_ID = int.Parse(lb_serie_factura.SelectedValue.ToString());
            }
            if (Doc_ID > 0)
            {
                RE_GenericBean fac_bean = (RE_GenericBean)DB.getFactura(Doc_ID, 1);
                lb_tipo_factura.SelectedValue = fac_bean.intC11.ToString();
            }


            if (lb_servicio.Items.Count > 0)
            {
                ArrayList rubros = null;
                if (lb_tipo_transaccion.SelectedValue != "108")
                {
                    rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), lb_tipo_transaccion.SelectedValue);
                }
                else
                {
                    rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), "");
                }
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

            arr = null;
            arr = (ArrayList)DB.Obtener_Tipos_Identificacion_Tributaria();
            drp_tipo_identificacion_cliente.Items.Clear();
            item = new ListItem("No Asignada", "0");
            drp_tipo_identificacion_cliente.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                drp_tipo_identificacion_cliente.Items.Add(item);
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

        ArrayList rubros = null;
        if ((lb_tipo_transaccion.SelectedValue != "108") && (lb_tipo_transaccion.SelectedValue != "120") && (lb_tipo_transaccion.SelectedValue != "121") && (lb_tipo_transaccion.SelectedValue != "122") && (lb_tipo_transaccion.SelectedValue != "123"))
        {
            rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), lb_tipo_transaccion.SelectedValue);
        }
        else
        {
            rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), "");
        }

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
        dt.Columns.Add("LOCAL_INTERNACIONAL");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
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
            lb10 = (Label)row.FindControl("lbl_local_internacional");
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, tb1.Text, lb9.Text, lb10.Text };
            dt.Rows.Add(objArr);
        }

        if ((user.PaisID == 1) || (user.PaisID == 7) || (user.PaisID == 15))
        {
            //BAW FISCAL USD
            if (lb_moneda.SelectedValue == "8")
            {
                lb_contribuyente.SelectedValue = "1";
            }
        }


        //halo el rubro y luego le calculo sus chingaderas
        Rubros rubro = new Rubros();
        rubro.rubroID = long.Parse(lb_rubro.SelectedValue);
        rubro.rubroName = lb_rubro.SelectedItem.Text;
        int cliID = int.Parse(tbCliCod.Text);
        int requierealias = int.Parse(lb_requierealias.Text);
        if (requierealias == 1) rubro.rubroName = DB.getAliasRubro(user.PaisID, rubro.rubroID, cliID, rubro.rubroName);
        rubro.rubroMoneda = long.Parse(lb_moneda2.SelectedValue);
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

        decimal tipoCambio = user.pais.TipoCambio;
        double totalD = rubtemp.rubroTot;
        if (((rubtemp.CobIva == 1) && (lb_contribuyente.SelectedValue.Equals("2")) && (tipo_contabilidad == 1) && (rubro.rubtoType != "TERCEROS")) || ((rubtemp.CobIva == 1) && (lb_contribuyente.SelectedValue.Equals("2")) && (tipo_contabilidad == 2) && (user.PaisID == 5)))
        {
			Decimal user_pais_Impuesto = DB.user_pais_Impuesto(lb_imp_exp.SelectedValue.ToString(), user.PaisID.ToString(), user.pais.Impuesto);                    
            #region Contribuyente
            if (rubtemp.IvaInc == 1)
            {				
                #region Iva Incluido
                if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                {
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user_pais_Impuesto)), 2);
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user_pais_Impuesto)), 2);
                    rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user_pais_Impuesto)), 2);
                    rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user_pais_Impuesto)), 2);
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
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user_pais_Impuesto, 2);
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    rubtemp.rubroImpuesto = Math.Round(totalD * (double)user_pais_Impuesto, 2);
                    rubtemp.rubroSubTot = Math.Round(totalD, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    rubtemp.rubroImpuesto = Math.Round(totalD * (double)user_pais_Impuesto, 2);
                    rubtemp.rubroSubTot = Math.Round(totalD, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    //totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user_pais_Impuesto, 2);
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
        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), "", "0", "MANUAL" };//0 Indica que es un rubro agregado del lado del BAW
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

        Marcar_Cargos_Locales_Internacionales();
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
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        user.ImpresionBean.Operacion = "1";
        user.ImpresionBean.Tipo_Documento = "1";
        user.ImpresionBean.Id = lb_facid.Text;
        user.ImpresionBean.Impreso = false;
        Session["usuario"] = user;
        string script = "";
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        ClientScriptManager cs2 = Page.ClientScript;


        RE_GenericBean rgb = new RE_GenericBean();
        ID = int.Parse(lb_facid.Text.ToString());
        rgb = (RE_GenericBean)DB.getFacturaData(ID);
        string path = DB.getpathImpresion(1, rgb.intC2, rgb.strC28);

        if (((user.PaisID == 6) || (user.PaisID == 25)) && (lbl_tipo_serie.Text == "1"))
        { //En el caso de Panama utiliza la impresion especial para Impresora Fiscal Tally1125, parametro op=0=impresion
            script = "window.open('printersettingsPA.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        }
        else
        {
            #region Impresiones Electronicas
            if (((user.PaisID == 1) || (user.PaisID == 15)) && (lbl_tipo_serie.Text == "1"))
            {
                if (!DB.DownloadFEL(lb_facid.Text, "1", "PDF", Response, lbl_internal_reference.Text, user.PaisID, false, true))
                    DB.DownloadGFACE(lbl_internal_reference.Text, Response, user, "");
            }
            else
            {
		        if ((user.PaisID == 2) || (user.PaisID == 9) || (user.PaisID == 26) || (user.PaisID == 38)) // sv sv2 svltf svtla
		        {
		            //2020-06-05 script = "window.open('printersettings.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
		            script = "window.open('print.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));parent.close();";
		        }
		        else
		        {
                	script = "window.open('../ImpresionDocumentos.html?fac_id=" + lb_facid.Text + "&tipo=1&pais=" + user.PaisID.ToString() + "&idioma=" + user.Idioma.ToString() + "&tipo_cambio=" + user.pais.TipoCambio.ToString() + "&contaId=" + user.contaID.ToString() + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
		        }	
            }
            #endregion
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
        bool valida_einvoice = false;
        if (lb_serie_factura.Items.Count > 0)
        {
            if (lb_serie_factura.SelectedValue == "0")
            {
                WebMsgBox.Show("Por favor seleccione la Serie a utilizar");
                return;
            }
        }
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

                    if (DB.Validar_Restriccion_Activa(user, 1, 45) != true)
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
        }
        if (gv_detalle.Rows.Count < 1)
        {
            WebMsgBox.Show("Debe facturar por lo menos 1 rubro");
            return;
        }
        #region Validaciones Nueva Representacion Grafica de Facturas Electronicas
        if (user.PaisID == 1)//Es Aimar Guatemala
        {
            if (lbl_tipo_serie.Text == "1")// Es Factura Electronica
            {
                #region Validar Cantidad de Rubros
                if ((gv_detalle.Rows.Count > 10) && (tb_allin.Text.Trim() == ""))
                {
                    WebMsgBox.Show("Las Facturas Electronicas deben tener un Maximo de 10 Rubros, si la cantidad es mayor la Boleta de Deposito no cabria en una sola hoja, Por Favor elimine los Rubros adicionales y genere otra Factura");
                    return;
                }
                #endregion
                #region Validar Cantidad de Caracteres en Campo de Observaciones
                if (tb_observaciones.Text.Length > 250)
                {
                    WebMsgBox.Show("Las Facturas Electronicas pueden tener un Maximo de 250 Caracteres en el Campo de Observaciones, si la cantidad es mayor la Boleta de Deposito no cabria en una sola hoja, Favor abreviar la Cantidad de Caracteres a un maximo de 250");
                    return;
                }
                #endregion
            }
        }
        #endregion
        if (decimal.Parse(tb_total.Text) == 0)
        {
            WebMsgBox.Show("La factura debe ser mayor a 0");
            return;
        }
        #region Validar Observaciones de Interompanys
        if (drp_tipopersona.SelectedValue == "10")
        {
            if (tb_observaciones.Text.Trim() == "")
            {
                WebMsgBox.Show("Debe ingresar las Observaciones correspondientes para el Documento de Cobro");
                return;
            }
        }
        #endregion
        int transID = int.Parse(lb_tipo_transaccion.SelectedValue);//tipo de transaccion factura, invoice
        int contribuyente = int.Parse(lb_contribuyente.SelectedValue);//excento contribuyente
        int moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue);//importacion exportacion
        int tipo_cobro = 1;//prepaid, collect
        if (imp_exp == 1) { tipo_cobro = 1; } else { tipo_cobro = 2; } //si es 1=importacion cobro 1=collect,si es 2=exportacion cobro 2=prepaid
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());//fiscal o financiera
        int servicio = 0; //fcl, lcl, etc
        
        FacturaBean factfinal = new FacturaBean();
        factfinal.Tipo_Persona = int.Parse(drp_tipopersona.SelectedValue);
        factfinal.Fecha_Hora = lb_fecha_hora.Text;//Fecha-Hora de Insercion
        factfinal.cobroID = tipo_cobro;
        factfinal.imp_exp = imp_exp;
        factfinal.Nit = tb_nit.Text.Trim();
        #region Validaciones Facturacion Electronica Costa Rica
        if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1"))
        {
            if (drp_tipo_identificacion_cliente.SelectedValue == "0")
            {
                WebMsgBox.Show("La Factura no fue guardada, ni procesada por el Ministerio de Hacienda porque el Cliente.: " + tb_nombre.Text + " con ID.: " + tbCliCod.Text+ " no tiene Asignado Tipo de Identificacion Tributaria en el Catalogo de Clientes, por favor contacte al personal Adminsitrativo de Aimar Costa Rica para que actualicen y asigen el Tipo en el Catalago de Clientes, posterior a ello genere nuevamente la Factura.");
                return;
            }
            if ((lbl_correo_documento_electronico.Text == "") && (factfinal.Tipo_Persona == 3))
            {
                WebMsgBox.Show("La Factura no fue guardada, el Cliente.: " + tb_nombre.Text + " con ID.: " + tbCliCod.Text + " no tiene asignado correo electronico en el Catalogo de Clientes para recibir Facturas Electronicas, por favor asigne para poder guardar la Factura.");
                return;
            }
        }
        #endregion
        factfinal.Nombre = tb_nombre.Text.Trim();
        factfinal.allIN = tb_allin.Text;
        #region Validacion Longitud del Nombre del Cliente
        if ((user.PaisID == 1) || (user.PaisID == 15))
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
        if ((user.PaisID == 1) || (user.PaisID == 15))
        {
            tb_direccion.Text = tb_direccion.Text.Trim();
            if (tb_direccion.Text.Length > 160)
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
        if (factfinal.Nombre.Trim() == "")
        {
            WebMsgBox.Show("El Cliente seleccionado debe tener un Nombre valido");
            bt_Enviar.Enabled = false;
            return;
        }
        if (factfinal.Nit.Trim() == "")
        {
            WebMsgBox.Show("El Cliente seleccionado debe tener un NIT valido");
            bt_Enviar.Enabled = false;
            return;
        }
        if (factfinal.Direccion.Trim() == "")
        {
            WebMsgBox.Show("El Cliente seleccionado debe tener Direccion valida");
            bt_Enviar.Enabled = false;
            return;
        }
        if (user.PaisID == 2)
        {
            if ((factfinal.Giro.Trim() == "") && (factfinal.Tipo_Persona == 3))
            {
                WebMsgBox.Show("El Cliente seleccionado debe tener Giro");
                bt_Enviar.Enabled = false;
                return;
            }
        }   
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
        factfinal.Serie = lb_serie_factura.SelectedItem.Text;
        factfinal.AgenteID = int.Parse(tb_agenteid.Text);
        factfinal.Nombre_Agente = tb_agente_nombre.Text.Trim();
        factfinal.ShipperID = int.Parse(id_shipper.Text);
        factfinal.ConsigneeID = int.Parse(id_consignee.Text);
        factfinal.Regimen_Aduanero = int.Parse(drp_regimen_aduanero.SelectedValue.ToString());
        factfinal.BlId = int.Parse(lbl_blID.Text);
        //factfinal.Tipo_Operacion = int.Parse(lbl_tipoOperacionID.Text);
        factfinal.Tipo_Operacion = int.Parse(Obtener_Tipo_Operacion());
        factfinal.serieID = int.Parse(lbl_serie_id.Text);
        factfinal.Correo_Electronico = lbl_correo_documento_electronico.Text;
        factfinal.Referencia_Correo = tb_referencia_correo.Text.Trim();
        factfinal.Factura_Electronica = int.Parse(lbl_tipo_serie.Text);
        factfinal.Contribuyente = contribuyente;
        factfinal.No_Factura_Aduana = tb_no_factura_aduana.Text.Trim();
        factfinal.No_Embarque = tb_no_embarque.Text.Trim();
        moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        factfinal.MonedaID = moneda;
        factfinal.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
        factfinal.Poliza_Seguro = tb_poliza_seguros.Text.Trim();
        factfinal.Poliza_Seguro_ID = int.Parse(lbl_poliza_seguro_id.Text);

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
        valida_cliente = Validar_Restricciones("Poliza_Repetida");
        if (valida_cliente == true)
        {
            return;
        }
        valida_cliente = Validar_Restricciones("btn_enviar_export");
        if (valida_cliente == true)
        {
            return;
        }
        #region Validacion Intercompanys
        if (factfinal.Tipo_Persona == 10)
        {
            RE_GenericBean Intercompany_Origen = (RE_GenericBean)DB.Get_Intercompany_Data_By_Empresa(user.PaisID);
            if (Intercompany_Origen.intC1.ToString() == factfinal.CliID.ToString())
            {
                WebMsgBox.Show("No se puede Emitir Cobro a la misma Empresa");
                return;
            }
            RE_GenericBean Intercompany_Bean = (RE_GenericBean)DB.Get_Intercompany_Data(Convert.ToInt32(factfinal.CliID));
            if (Intercompany_Bean == null)
            {
                WebMsgBox.Show("Existio un error al Tratar de Obtener la Informacion del Intercompany");
                return;
            }
            int existe_configuracion = 0;
            string sql_existencia = " and tia_pais_origen=" + user.PaisID + " and tia_contabilidad_origen=" + user.contaID + " and tia_moneda_origen=" + factfinal.MonedaID + " and tia_tipo_operacion=" + 2 + " and tia_id_intercompany=" + factfinal.CliID.ToString() + " ";
            existe_configuracion = Contabilizacion_Automatica_CAD.Validar_Existencia_Intercompany_Administrativo(user, sql_existencia);
            if (existe_configuracion == -100)
            {
                WebMsgBox.Show("Existio un Error al Tratar de validar la configuracion del Intercompany");
                return;
            }
            else if (existe_configuracion > 0)
            {
                decimal Tipo_Cambio_Destino = 0;
                Tipo_Cambio_Destino = DB.getTipoCambioHoy(Intercompany_Bean.intC3);
                if (Tipo_Cambio_Destino == 0)
                {
                    PaisBean Pais_Temporal = (PaisBean)DB.getPais(Intercompany_Bean.intC3);
                    WebMsgBox.Show("No Existe Tipo de Cambio para realizar la Automatizacion en Sistema BAW - " + Pais_Temporal.Nombre_Sistema + ", favor notificar al personal de Contabilidad");
                    return;
                }
            }



        }
        #endregion
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

            int tttID = 0;
            tttID = transID;
            if (tttID == 108)
            {
                if (tipo_contabilidad == 2)
                {
                    tttID = 7;
                }
                else
                {
                    tttID = 1;
                }
            }
            rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, tttID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            rubro.cta_haber = (ArrayList)DB.getCtaContablebyRubro("haber", (int)rubro.rubroID, user.PaisID, tttID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);


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

            #region Validacion de Cuentas Contables Intercompany Destino
            if (factfinal.Tipo_Persona == 10)
            {
                int _intercompanyID = 0;
                int _idpaisORIGEN = 0;
                int _idcontaORIGEN = 0;
                int _idmonedaORIGEN = 0;
                int _idOPERACION = 0;
                int _idpaisDESTINO = 0;
                int _idcontaDESTINO = 0;
                int _idmonedaDESTINO = 0;

                int existe_configuracion = 0;
                string sql_existencia = " and tia_pais_origen=" + user.PaisID + " and tia_contabilidad_origen=" + user.contaID + " and tia_moneda_origen=" + factfinal.MonedaID + " and tia_tipo_operacion=" + 2 + " and tia_id_intercompany=" + factfinal.CliID.ToString() + " ";
                existe_configuracion = Contabilizacion_Automatica_CAD.Validar_Existencia_Intercompany_Administrativo(user, sql_existencia);
                if (existe_configuracion == -100)
                {
                    WebMsgBox.Show("Existio un Error al Tratar de validar la configuracion del Intercompany");
                    return;
                }
                else if (existe_configuracion > 0)
                {
                    RE_GenericBean _Intercompany_Origen = null;
                    _intercompanyID = int.Parse(factfinal.CliID.ToString());
                    _idpaisORIGEN = user.PaisID;
                    _idcontaORIGEN = user.contaID;
                    _idmonedaORIGEN = factfinal.MonedaID;
                    _idOPERACION = 2;
                    RE_GenericBean Bean_Configuracion_Intercompany = Contabilizacion_Automatica_CN.Obtener_Configuracion_Intercompany_Administrativo(_intercompanyID, _idpaisORIGEN, _idcontaORIGEN, _idmonedaORIGEN, _idOPERACION);
                    _idpaisDESTINO = Bean_Configuracion_Intercompany.intC1;
                    _idcontaDESTINO = Bean_Configuracion_Intercompany.intC2;
                    _idmonedaDESTINO = Bean_Configuracion_Intercompany.intC3;
                    _Intercompany_Origen = (RE_GenericBean)DB.Get_Intercompany_Data_By_Empresa(_idpaisORIGEN);

                    int _tttID_Destino = 15;
                    Rubros Rubro_Destino = new Rubros();
                    //CAMBIO SOLICITADO EL 21-06-2016 POR Jose Cruz Ticket.: 2016062104000574 
                    Rubro_Destino.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, _idpaisDESTINO, _tttID_Destino, contribuyente, _idmonedaDESTINO, imp_exp, tipo_cobro, _idcontaDESTINO, servicio);
                    //CAMBIO SOLICITADO EL 24-09-2015 POR MR Holbik
                    //Rubro_Destino.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, _idpaisDESTINO, _tttID_Destino, contribuyente, _idmonedaDESTINO, imp_exp, tipo_cobro, _idcontaDESTINO, 14);
                    if ((Rubro_Destino.cta_debe == null) || (Rubro_Destino.cta_debe.Count == 0))
                    {
                        WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la Provision en Destino , por favor pongase en contacto con el Contador");
                        bt_Enviar.Enabled = false;
                        Cancel.Enabled = false;
                        bt_imprimir.Enabled = false;
                        return;
                    }

                    int transID_Destino = 105;//Provision Intercompanys
                    int _matOpID_Destino = DB.getMatrizOperacionID(transID_Destino, _idmonedaDESTINO, _idpaisDESTINO, _idcontaDESTINO);
                    ArrayList ctas_cargo_Destino = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(_matOpID_Destino, "Abono");
                    if ((ctas_cargo_Destino == null) || (ctas_cargo_Destino.Count == 0))
                    {
                        WebMsgBox.Show("No existe configuracion contable para la Provision en Destino , por favor pongase en contacto con el Contador");
                        return;
                    }
                }
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
        #region Detalle Factura Electronica
        if ((tbCliCod.Text.Trim() != "") && (tbCliCod.Text.Trim() != "0"))
        {
            //factfinal.arr2.Add("CODIGO DE CLIENTE.: " + tbCliCod.Text.Trim());
            factfinal.arr2.Add(tbCliCod.Text.Trim());
        }
        if (factfinal.HBL.Trim() != "")
        {
            factfinal.arr2.Add("HBL.: " + factfinal.HBL);
        }
        if (factfinal.MBL.Trim() != "")
        {
            factfinal.arr2.Add("MBL.: " + factfinal.MBL);
        }
        if (factfinal.Routing.Trim() != "")
        {
            factfinal.arr2.Add("ROUTING.: " + factfinal.Routing);
        }
        if (factfinal.Contenedor.Trim() != "")
        {
            factfinal.arr2.Add("CONTENEDOR.: " + factfinal.Contenedor);
        }
        //SE COMENTA POR SOLICITUD DE ESDRAS TICKET 2015082804000649 
        //if (factfinal.Naviera.Trim() != "")
        //{
        //    factfinal.arr2.Add("NAVIERA.: " + factfinal.Naviera);
        //}
        //SE COMENTARON POR SOLICITUD DE ESDRAS - CORREO RV: RV: Modelo de factura y boleta
        //if (factfinal.Vapor.Trim() != "")
        //{
        //    factfinal.arr2.Add("VAPOR.: " + factfinal.Vapor);
        //}
        //if (factfinal.Nombre_Agente.Trim() != "")
        //{
        //    factfinal.arr2.Add("AGENTE.: " + factfinal.Nombre_Agente);
        //}
        //if (factfinal.Shipper.Trim() != "")
        //{
        //    factfinal.arr2.Add("SHIPPER.: " + factfinal.Shipper);
        //}
        if (factfinal.Paquetes.Trim()!="")
        {
            factfinal.arr2.Add("PAQUETES.: " + factfinal.cantPaquetes.ToString() + " - " + factfinal.Paquetes);
        }
        if ((factfinal.Peso.Trim() != "") || (factfinal.Volumen.Trim() != ""))
        {
            factfinal.arr2.Add("PESO.: " + factfinal.Peso + " VOLUMEN.: " + factfinal.Volumen);
        }
        if ((factfinal.Dua_Ingreso.Trim() != "") || (factfinal.Dua_Salida.Trim() != ""))
        {
            factfinal.arr2.Add("DUA INGRESO.: " + factfinal.Dua_Ingreso + " DUA SALIDA.: " + factfinal.Dua_Salida);
        }
        if ((factfinal.OrdenPO.Trim() != "") || (drp_regimen_aduanero.SelectedValue != "0"))
        {
            factfinal.arr2.Add("POLIZA.: " + factfinal.OrdenPO + "  REGIMEN ADUANERO.: " + drp_regimen_aduanero.SelectedItem.Text);
        }
        //if (drp_regimen_aduanero.SelectedValue != "0")
        //{
        //    factfinal.arr2.Add("REGIMEN ADUANERO.: " + drp_regimen_aduanero.SelectedItem.Text);
        //}
        if (factfinal.Observaciones.Trim() != "")
        {
            factfinal.arr2.Add("OBSERVACIONES.: " + factfinal.Observaciones);
        }
        #region Factura de Exportacion
        RE_GenericBean Serie_Bean = (RE_GenericBean)DB.getFactura(factfinal.serieID, 1);
        if (Serie_Bean.strC9 == "FACE72")
        {
            factfinal.arr2.Add("NOMBRE COMERCIAL.: " + factfinal.Nombre);
            factfinal.arr2.Add("IDENTIFICACION TRIBUTARIA.: " + factfinal.Nit);
        }
        #endregion
        if ((tbCliCod.Text.Trim() != "") && (tbCliCod.Text.Trim() != "0"))
        {
            factfinal.arr2.Add("CODIGO DE CLIENTE.: " + tbCliCod.Text.Trim());
        }
        
        factfinal.arr2.Add("Estimado Cliente, a partir de la fecha de emision de la factura tiene 15 dias calendario para realizar cualquier reclamo sobre la misma.");
        if (user.PaisID == 1)
        {
            factfinal.arr2.Add("AGENTE RETENEDOR DE IVA (NO RETENER IVA)");
        }
        #endregion
        if (factfinal.RubrosArr == null || factfinal.RubrosArr.Count == 0)
        {
            WebMsgBox.Show("Debe tener rubros para facturar");
            return;
        }
        int matOpID = DB.getMatrizOperacionID(transID, moneda, user.PaisID, tipo_contabilidad);
        ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID, "Cargo");
        #region Validacion Agente Aduanero
        string sql = " and tcaa_empresa_cobra=" + user.PaisID + " and tcaa_tipo_contabilidad=" + user.contaID + " ";
        if (user.pais.Pai_Agente == true)
        {
            RE_GenericBean BAA = (RE_GenericBean)DB.Get_Configuracion_Agente_Aduanero(user, sql);
            if (BAA != null)
            {
                decimal TipoCambio = DB.getTipoCambioHoy(int.Parse(BAA.strC5));
                if (TipoCambio == 0)
                {
                    WebMsgBox.Show("No existe tipo de cambio para el dia de hoy en la empresa donde se emitiran los pagos");
                    return;
                }
                else
                {
                    Provision = new RE_GenericBean();
                    Provision.intC1 = int.Parse(BAA.strC9);//tpr_proveedor_id
                    Provision.intC2 = int.Parse(BAA.strC3);//tpr_pai_id
                    PaisBean Pais_Paga = (PaisBean)DB.getPais(Provision.intC2);
                    Provision.strC1 = user.ID;//tpr_usu_creacion
                    Provision.intC3 = 5;//tpr_ted_id
                    Provision.strC2 = BAA.strC7;//tpr_serie
                    Provision.strC3 = "";//tpr_correlativo
                    Provision.intC4 = 13;//tpr_tto_id
                    Provision.strC4 = factfinal.MBL;//tpr_mbl
                    Provision.strC5 = factfinal.HBL;//tpr_hbl
                    Provision.strC6 = factfinal.Routing;//tpr_routing
                    Provision.strC7 = factfinal.Contenedor;//tpr_contenedor
                    Provision.intC5 = int.Parse(BAA.strC8);//tpr_tpi_id
                    Provision.intC6 = int.Parse(BAA.strC15);//tpr_mon_id
                    Provision.intC7 = int.Parse(BAA.strC13);//tpr_imp_exp_id
                    Provision.intC8 = user.contaID;//tpr_tcon_id
                    Provision.strC8 = DB.Get_Persona_Nombre(Provision.intC5, Provision.intC1);//tpr_nombre
                    Provision.intC9 = int.Parse(BAA.strC4);//tpr_suc_id
                    int ImpuestoProveedor = DB.getProveedorRegimen(Provision.intC5, Provision.intC1.ToString());
                    Provision.intC11 = ImpuestoProveedor;//Tipo de Contribuyente
                    Rubros Rubros_provision = (Rubros)DB.Generar_Detalle_By_Rubro(long.Parse(BAA.strC12), factfinal.Total, int.Parse(BAA.strC11), ImpuestoProveedor, int.Parse(BAA.strC3), int.Parse(BAA.strC6), Provision.intC6, Pais_Paga, TipoCambio);//Rubros de la Provision
                    int Tipo_cobro = 1;//collect
                    Rubros_provision.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", int.Parse(BAA.strC12), int.Parse(BAA.strC3), int.Parse(BAA.strC10), Provision.intC11, Provision.intC6, int.Parse(BAA.strC13), Tipo_cobro, int.Parse(BAA.strC6), int.Parse(BAA.strC11));//Cuentas Contables del Rubro
                    Provision.arr1.Add(Rubros_provision);
                    RE_GenericBean Totales = (RE_GenericBean)DB.Calcular_Totales_Provision(Provision, Provision.intC8, ImpuestoProveedor, Pais_Paga, TipoCambio);
                    Provision.decC1 = Totales.decC1;//Total
                    Provision.decC2 = Totales.decC2;//Afeto
                    Provision.decC3 = Totales.decC3;//No Afecto
                    Provision.decC4 = Totales.decC4;//Impuesto
                    Provision.decC5 = Totales.decC5;//Total Equivalente
                    Provision.decC6 = TipoCambio;
                    Provision.intC10 = factfinal.BlId;//BLID
                    Provision.strC9 = "";//tpr_fact_corr
                    Provision.boolC1 = false;//tpr_fiscal
                    Provision.intC12 = int.Parse(BAA.strC10);//tpr_imp_exp_id
                    int matOpID_Prov = DB.getMatrizOperacionID(int.Parse(BAA.strC10), Provision.intC6, Provision.intC2, Provision.intC8);
                    ArrayList ctas_abono_Prov = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID_Prov, "Abono");
                    Provision.arr2 = ctas_abono_Prov;
                    Provision.arr3.Add(Pais_Paga);
                }
            }
        }
        #endregion
        string Check_Existencia = DB.CheckExistDoc(factfinal.Fecha_Hora, 1);
        if (Check_Existencia == "0")
        {
            bool res= Validar_Restricciones("btn_enviar");
            if (res == true)
            {
                return;
            }
            ArrayList result = DB.insertFacturacion(factfinal, user, tipo_contabilidad, ctas_cargo, transID, Provision);
            if (result == null || result.Count == 0 || int.Parse(result[0].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
            {
                #region Facturacion Electronica de Costa Rica
                if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1"))
                {
                    if (result.Count == 5)
                    {
                        ArrayList Arr_Transmision_CR = (ArrayList)result[4];
                        if (Arr_Transmision_CR[0].ToString() == "0")
                        {
                            pnl_transmision_electronica.Visible = true;
                            tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString();
                            bt_Enviar.Enabled = false;
                            bt_factura_virtual.Enabled = false;
                            Cancel.Enabled = false;
                            WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                            return;
                        }
                    }
                }
                #endregion
                bt_Enviar.Enabled = true;
                WebMsgBox.Show("Error, existio un error al tratar de grabar los datos, por favor intente de nuevo.");
                return;
            }
            else
            {
                bt_agregar.Visible = false;
                gv_detalle.Enabled = false;
                valida_einvoice = Validar_Restricciones("send_einvoice");
                if (valida_einvoice == true)
                {
                    #region Transmitir EInvoice
                    if ((user.PaisID == 1) || (user.PaisID == 15))
                    {
                        #region Facturacion Electronica Guatemala
                        string Referencia_Interna = "";
                        Referencia_Interna = result[2].ToString();
                        if ((lbl_tipo_serie.Text == "1") || (lbl_tipo_serie.Text == "2"))
                        {
                            ArrayList Arr_Data = new ArrayList();
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

                            var fel = DB.DataFEL(user.PaisID, "", "");
                            if (fel.isFELdate) { 

                                string iStr = "";                         
                                try
                                {
                                    #region FEL 2019-04-23
                                    string facide = result[0].ToString().Trim();
                                    
                                    fel_101017.WebService1 proceso = new fel_101017.WebService1();
                                    user = (UsuarioBean)Session["usuario"];
                                    
                                    var resultado = proceso.Proceso_07_Completo("", "", facide, user.Email, "1", user.PaisID.ToString());                                      
                                    if (resultado[3] == "")
                                    {
                                        DB.DownloadFEL(facide, "1", "PDF", Response, resultado[5], user.PaisID, false, true); //, resultado[0]); 
                                                                                
                                        iStr = "Se Transmitio correctamente la Factura. ";
                                        iStr += "FEL Serie " + resultado[1] + " y Numero " + resultado[2] + ". ";
                                        iStr += "Interno Serie " + lb_serie_factura.SelectedItem.Text + " y Correlativo " + result[1].ToString() + ". ";

                                        setButtons("1", iStr, result, Referencia_Interna, resultado[0], resultado[1], "Firma Electronica.: " + resultado[0] + " \n" + iStr, resultado[5]);
                                        return;
                                    }
                                    else
                                    {
                                        iStr = "Existio un error durante la Transmision del Documento con el FEL y no se obtuvo firma electronica, La factura fue grabada exitosamente con la Serie.: " + lb_serie_factura.SelectedItem.Text + " y Folio.: " + result[1].ToString();
                                        setButtons("2", iStr, result, Referencia_Interna, "0", result[1].ToString(), resultado[3] + " ", "");
                                        return;
                                    }
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    iStr = "FEL (SAT) se encuentra fuera de linea y no se obtuvo firma electronica. " + ex.Message + ". La factura fue grabada exitosamente en el Sistema con la Serie.: " + lb_serie_factura.SelectedItem.Text + " y Folio.: " + result[1].ToString() + " y sera retransmitida mediante un proceso automatico en un lapso de 24 horas";
                                    setButtons("3", iStr, result, Referencia_Interna, "0", result[1].ToString(), "", "");
                                    return;
                                }

                            } else {
                                /*2019-10-11
				                #region Generar XML
				                string Query = " and tn_pai_id=" + user.PaisID + " and tn_ttr_id=1 and tn_conta_id=" + user.contaID + " order by tn_nivel, tn_posicion asc  ";
				                XmlDocument ExmlDoc = (XmlDocument)DB.Generar_Xml(user, Arr_Data, Query, 1);
				                #endregion
				                if (ExmlDoc != null)
				                {
				                    #region Guardar XML Transmitido
				                    //ExmlDoc.Save("D:\\EINVOICE\\1" + Referencia_Interna + ".xml");
				                    #endregion
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
				                        WebMsgBox.Show("El GFACE (SAT) se encuentra fuera de linea y no se obtuvo firma electronica, La factura fue grabada exitosamente en el Sistema con la Serie.: " + lb_serie_factura.SelectedItem.Text + " y Folio.: " + result[1].ToString() + " y sera retransmitida mediante un proceso automatico en un lapso de 24 horas");
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
				                        bt_imprimir.Enabled = false;
				                        lb_facid.Text = result[0].ToString();
				                        tb_correlativo.Text = result[1].ToString();
				                        user = (UsuarioBean)Session["usuario"];

				                        //Mostrando la Partida contable generada
				                        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
				                        gv_detalle_partida.DataBind();
				                        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

				                        bt_Enviar.Enabled = false;
				                        bt_factura_virtual.Enabled = true;
				                        if ((user.PaisID == 1 || user.PaisID == 15))
				                        {
				                            bt_impresion_invoice.Visible = true;
				                        }
				                        //Toque Aqui
				                        btn_ver_pdf.Visible = false;
				                        return;
				                        #endregion
				                    }
				                    else if (Tag.Response.Result == false)
				                    {
				                        #region Transmision Fallida
				                        WebMsgBox.Show("Existio un error durante la Transmision del Documento con el GFACE y no se obtuvo firma electronica, La factura fue grabada exitosamente con la Serie.: " + lb_serie_factura.SelectedItem.Text + " y Folio.: " + result[1].ToString());
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
				                        bt_imprimir.Enabled = false;
				                        lb_facid.Text = result[0].ToString();
				                        tb_correlativo.Text = result[1].ToString();
				                        user = (UsuarioBean)Session["usuario"];

				                        //Mostrando la Partida contable generada
				                        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
				                        gv_detalle_partida.DataBind();
				                        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

				                        bt_Enviar.Enabled = false;
				                        bt_factura_virtual.Enabled = true;
				                        if ((user.PaisID == 1 || user.PaisID == 15))
				                        {
				                            bt_impresion_invoice.Visible = true;
				                        }
				                        btn_ver_pdf.Visible = false;
				                        return;
				                        #endregion
				                    }
				                    else if (Tag.Response.Result == true)
				                    {
				                        #region Transmision Exitosa
				                        WebMsgBox.Show("Se Transmitio correctamente la Factura Serie " + lb_serie_factura.SelectedItem.Text + " y Folio " + result[1].ToString());
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
				                        bt_factura_virtual.Enabled = true;
				                        if ((user.PaisID == 1 || user.PaisID == 15))
				                        {
				                            bt_impresion_invoice.Visible = true;
				                        }
				                        btn_ver_pdf.Visible = false;
				                        return;
				                        #endregion
				                    }

				                    #endregion
				                }
				                else
				                {
				                    WebMsgBox.Show("Existio un error en la Generacion del XML, porfavor intente de nuevo");
				                    return;
				                }		*/		                 
                            }
                        }
                        #endregion
                    }
                    else if (user.PaisID == 5)
                    {
                        #region BK Facturacion Electronica Costa Rica
                        //if (lbl_tipo_serie.Text == "1")
                        //{
                        //    EInvoice_CR EInvoice = new EInvoice_CR();
                        //    ArrayList Arr_Transmision_CR = EInvoice.Generar_Firma_Electronica(1, int.Parse(result[0].ToString()));
                        //    if (Arr_Transmision_CR[0].ToString() == "0")
                        //    {
                        //        WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                        //        pnl_transmision_electronica.Visible = true;
                        //        tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString();
                        //    }
                        //    else if (Arr_Transmision_CR[0].ToString() == "1")
                        //    {
                        //        WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                        //        pnl_transmision_electronica.Visible = true;
                        //        tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString();
                        //        tb_correlativo.Text = Arr_Transmision_CR[2].ToString();
                        //    }
                        //    lb_facid.Text = result[0].ToString();
                        //    user = (UsuarioBean)Session["usuario"];
                        //    #region Mostrando la Partida contable
                        //    gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
                        //    gv_detalle_partida.DataBind();
                        //    gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                        //    #endregion
                        //    bt_Enviar.Enabled = false;
                        //    bt_factura_virtual.Enabled = true;
                        //    return;
                        //}
                        #endregion
                    }
                    #endregion
                }
                bt_imprimir.Enabled = true;
                lb_facid.Text = result[0].ToString();
                tb_correlativo.Text = result[1].ToString();
                user = (UsuarioBean)Session["usuario"];
                
                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
                gv_detalle_partida.DataBind();
                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

                //Intercompanys
                if (factfinal.Tipo_Persona == 10)
                {
                    tb_resultado_automatizacion.Text = result[3].ToString();
                    pnl_intercompanys.Visible = true;
                    //Notificacion Automatica de Intercompanys
                    bool resultado_notificacion = false;
                    resultado_notificacion = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(1, int.Parse(lb_facid.Text), 1);
                }

                #region Facturacion Electronica de Costa Rica
                if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1"))
                {
                        if (result.Count == 5)
                        {
                            ArrayList Arr_Transmision_CR = (ArrayList)result[4];
                            if (Arr_Transmision_CR[0].ToString() == "1")
                            {
                                pnl_transmision_electronica.Visible = true;
                                tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString().Replace("\\n", "\n");
                                tb_correlativo.Text = Arr_Transmision_CR[2].ToString();
                                lb_facid.Text = result[0].ToString();
                                #region Mostrando la Partida contable
                                user = (UsuarioBean)Session["usuario"];
                                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
                                gv_detalle_partida.DataBind();
                                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                                #endregion
                                bt_Enviar.Enabled = false;
                                bt_factura_virtual.Enabled = true;
                                WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                                return;
                            }
                        }
                }
                #endregion

                #region TTS transmision 
                //2020-11-04

                if (tb_file.Text.Trim() != "")
                {

                      //1 impo 2 expo

                    string json = (@"
[
    {
        ""Routing"":""" + factfinal.Routing + @""",
        ""File"":""" + tb_file.Text + @""",
        ""NoCliente"":""" + factfinal.CliID + @""",
        ""ConsigneeID"":""" + factfinal.ConsigneeID + @""",
        ""Consignee"":""" + factfinal.Consignee + @""",
        ""NombreCliente"":""" + factfinal.Nombre_Cliente + @""",
        ""TipoTramite"":""" + factfinal.imp_exp + @""",
        ""Embarque"":""" + factfinal.No_Embarque + @""",
        ""PO"":""" + factfinal.OrdenPO + @""", 
");
                    foreach (Rubros rub in factfinal.RubrosArr)
                    {
                        if (rub.rubroTypeID == 9)
                        {
                            json += (@"
        ""Transporte"":""" + rub.rubtoType + @""",
");
                        }
                    }

                    json += (@"
        ""Comodity"":""" + factfinal.Comodity + @""",
        ""PaisID"":""" + user.PaisID + @"""
    }
]
");
                    //http://wstlatest.grupotla.com:8080/WSAimarTest/api/AgenciaAduanal/AimarToTTS
                    //usuario: user
                    //contraseña: password

                }
                #endregion

                WebMsgBox.Show("La Factura fue grabada exitosamente con Serie.: " + lb_serie_factura.SelectedItem.Text + " y Correlativo.: " + result[1].ToString());
                bt_Enviar.Enabled = false;
                bt_factura_virtual.Enabled = true;
                if ((user.PaisID == 1 || user.PaisID == 15))
                {
                    bt_impresion_invoice.Visible = true;
                }
                factfinal = null;
                return;
            }
        }
        else
        {
            bt_Enviar.Enabled = false;
            return;
        }
    }


    protected void setButtons(string tipo, string msg, ArrayList result, string Referencia_Interna, string docguid, string serial, string result_str, string esignature)
    {

        WebMsgBox.Show(msg);

        switch (tipo)
        {
            case "1": //ok
                pnl_transmision_electronica.Visible = true;
                tb_resultado_transmision.Text = result_str; //"Firma Electronica.: " + Signature + " \n" + "Se Transmitio correctamente la Factura Serie " + Tag.Response.Identifier.Batch + " y Folio " + Tag.Response.Identifier.Serial;
                //lbl_internal_reference.Text = Referencia_Interna;
                //if (DB.isFEL("1", esignature)) //2019-10-07 se agrego parametro esignature que viene desde el ws firma
                lbl_internal_reference.Text = esignature;
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
        #region Mostrando la Partida contable
        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
        gv_detalle_partida.DataBind();
        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
        #endregion
        bt_Enviar.Enabled = false;
        bt_factura_virtual.Enabled = true;
        if ((user.PaisID == 1 || user.PaisID == 15))
        {
            bt_impresion_invoice.Visible = true;
        }
        btn_ver_pdf.Visible = false;
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
        dt.Columns.Add("LOCAL_INTERNACIONAL");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
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
            lb10 = (Label)row.FindControl("lbl_local_internacional");
            if (indice_anular == e.RowIndex)
            {
                if (lb9.Text != "0")
                {
                    ban_anular = 1;
                }
            }
            indice_anular++;
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, tb1.Text, lb9.Text, lb10.Text };
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
        Marcar_Cargos_Locales_Internacionales();
    }

    protected bool ValidarClientesBL(ClientesBL clientesPermitidos, double clienteSeleccionado)
    {
        if (Request.QueryString["tipo"] != null)
        {
            if (Request.QueryString["tipo"].ToString().Trim() == "LCL" ||
                Request.QueryString["tipo"].ToString().Trim() == "FCL" ||
                Request.QueryString["tipo"].ToString().Trim() == "TERRESTRE T" ||
                Request.QueryString["tipo"].ToString().Trim() == "AEREO")
            {
                if (clienteSeleccionado > 0 && clientesPermitidos != null)
                {
                    if (clienteSeleccionado == clientesPermitidos.IdCliente)
                        return true;
                    else if (clienteSeleccionado == clientesPermitidos.IdNotifyParty)
                        return true;
                    else if (clienteSeleccionado == clientesPermitidos.IdColoader)
                        return true;
                    else if (clienteSeleccionado == clientesPermitidos.IdShipper)
                        return true;
                    else
                    {
                        txtUsuarioCambioDeCliente.Text = string.Empty;
                        txtContraseniaCambioDeCliente.Text = string.Empty;
                        var msg = "No se puede seleccionar el cliente porque no esta asignado en el BL. ¿Desea autorizar el cambio de cliente?";
                        WebMsgBox.Show(msg, "$find(\"modalUsuarioCambioDeCliente\").show();", WebMsgBox.TipoMensaje.Confirmacion);
                        
                        return false;
                    }
                }
                else
                    return false;
            }
            else
                return true;
        }
        else
            return true;
    }

    protected void btnAceptarCambioDeCliente_Click(object sender, EventArgs e)
    {
        if (VerificarPermisoParaCambioDeCliente(txtUsuarioCambioDeCliente.Text, txtContraseniaCambioDeCliente.Text))
            gv_clientes_SelectedIndexChanged(sender, e);
        
        txtUsuarioCambioDeCliente.Text = string.Empty;
        txtContraseniaCambioDeCliente.Text = string.Empty;
        Session["IdClienteSeleccionado"] = null;
        Session["NomClienteSeleccionado"] = null; 
    }

    protected void gv_clientes_SelectedIndexChanging(object sender, EventArgs e)
    {
        Session["IdClienteSeleccionado"] = null;
        Session["NomClienteSeleccionado"] = null; 
    }

    private bool VerificarPermisoParaCambioDeCliente(string nombreUsuario, string contrasenia)
    {
        bool conPermiso = false;
        var contrasena = Utility.cifrado(contrasenia);
        var usuario = DB.ValidaCliente(nombreUsuario, contrasena);
        
        if (usuario != null)
        {
            usuario.PaisID = user.PaisID;
            var Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(usuario.ID, usuario.PaisID);

            Arr_Perfiles.Cast<PerfilesBean>().ToList().ForEach(i =>
            {
                if (i.ID == 35)
                    conPermiso = true;
            });
            if (!conPermiso)
                WebMsgBox.Show("El usuario no tiene permiso para realizar esta acción.");
        }
        else
            WebMsgBox.Show("Usuario inválido, por favor verifique su usuario y/o contraseña.");

        if (usuario == null || !conPermiso)
        {
            Session["IdClienteSeleccionado"] = null;
            Session["NomClienteSeleccionado"] = null;
        }

        return conPermiso;
    }
    
    protected void gv_clientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_tipopersona.SelectedValue == "3")
        {
            #region Clientes
            double cliID = 0;
            string cliNom = string.Empty;
            bool resultadoValidarCliente = false;

            if (Session["IdClienteSeleccionado"] == null)
            {
                GridViewRow row = gv_clientes.SelectedRow;
                cliID = double.Parse(row.Cells[1].Text);
                cliNom = row.Cells[2].Text;
                Session["IdClienteSeleccionado"] = cliID;
                Session["NomClienteSeleccionado"] = cliNom;

                if (user.SucursalID != 12)
                    resultadoValidarCliente = ValidarClientesBL((ClientesBL)Session["ClientesBillOfLading"], cliID);
                else
                {
                    resultadoValidarCliente = true;
                    int activacion = DB.ActivarCliente(int.Parse(cliID.ToString()));
                    if (activacion == -1)
                    {
                        WebMsgBox.Show("Hubo un error al activar al Cliente");
                        return;
                    }
                }
            }
            else
            {
                cliID = (double)(Session["IdClienteSeleccionado"]);
                cliNom = (string)(Session["NomClienteSeleccionado"]);
                Session["IdClienteSeleccionado"] = null;
                Session["NomClienteSeleccionado"] = null;
                resultadoValidarCliente = true;
            }

            //Valida si el cliente seleccionado se encuentra en enbarque
            if (resultadoValidarCliente)
            {
                string criterio = string.Format("a.id_cliente={0}", cliID.ToString());
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
                    if ((DB.Validar_Restriccion_Activa(user, 1, 28) == true))
                    {
                        #region Validacion Grupo Empresas
                        if ((user.SucursalID == 9) || (user.SucursalID == 71))
                        {
                        }
                        else if (user.pais.Grupo_Empresas == 1)
                        {
                            if (clienteBean.strC8 == "True")
                            {
                                WebMsgBox.Show(string.Format("No se puede utilizar el Cliente.: {0} - {1} porque es un Cliente COLOADER.", cliID.ToString(), cliNom));
                                return;
                            }
                        }
                        else if (user.pais.Grupo_Empresas == 2)
                        {
                            if (clienteBean.strC8 == "False")
                            {
                                WebMsgBox.Show(string.Format("No se puede utilizar el Cliente.: {0} - {1} porque es un Cliente DIRECTO.", cliID.ToString(), cliNom));
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
                    drp_tipo_identificacion_cliente.SelectedValue = clienteBean.strC10;
                    Validar_Cliente_Credito_Contado();
                    int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
                    if ((tipo_contabilidad == 2) && (user.PaisID != 5))
                    {
                        lb_contribuyente.SelectedValue = "1";
                    }
                    else
                    {
                        //if ((user.PaisID == 1) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 3) || (user.PaisID == 23))
                        if ((user.PaisID == 1) || (user.PaisID == 7) || (user.PaisID == 15))
                        {
                            //BAW FISCAL USD
                            if (lb_moneda.SelectedValue == "8")
                            {
                                lb_contribuyente.SelectedValue = "1";
                                Actualizar_Cobro_Impuestos();
                            }
                            else
                            {
                                #region Validar si cambio el Tipo Contribuyente
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
                        }
                        else
                        {
                            #region Validar si cambio el Tipo Contribuyente
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
                    }
                    lb_requierealias.Text = clienteBean.intC2.ToString();
                    bool res = Validar_Restricciones("gv_clientes_SelectedIndexChanged");
                    if (res == false)
                    {
                        return;
                    }
                }
            }
            else
            {
               // return;
            }
            #endregion
        }
        else if (drp_tipopersona.SelectedValue == "10")
        {
            #region Intercompanys
            GridViewRow row = gv_clientes.SelectedRow;
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_nit.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
            tb_registro_no.Text = Page.Server.HtmlDecode(row.Cells[7].Text);

            drp_tipo_identificacion_cliente.SelectedValue = "10";//Tipo de Identificacion del Extranjero

            gv_detalle.DataBind();
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            if ((tipo_contabilidad == 2) && (user.PaisID != 5))
            {
                lb_contribuyente.SelectedValue = "1";
            }
            else
            {
                lb_contribuyente.SelectedValue = DB.getProveedorRegimen(int.Parse(drp_tipopersona.SelectedValue), tbCliCod.Text.ToString()).ToString();
            }
            RE_GenericBean Bean_Intercompany_Origen = null;
            RE_GenericBean Bean_Intercompany_Destino = null;
            Bean_Intercompany_Origen = DB.Get_Intercompany_Data_By_Empresa(user.PaisID);
            Bean_Intercompany_Destino = DB.Get_Intercompany_Data(int.Parse(tbCliCod.Text));
            if (Bean_Intercompany_Origen.strC4 == Bean_Intercompany_Destino.strC4)
            {
                #region Cobro a un Intercompany en el mismo Pais - Genera Impuestos
                if (user.contaID == 1)
                {
                    #region Contabilidad Fiscal
                    if ((user.PaisID == 1) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 3) || (user.PaisID == 23) || (user.PaisID == 5) || (user.PaisID == 21))
                    {
                        #region Empresas con Fiscal USD Excenta de Impuestos
                        if (lb_moneda.SelectedValue == "8")
                        {
                            //FISCAL USD
                            lb_contribuyente.SelectedValue = "1";//Excento
                        }
                        else if (lb_moneda.SelectedValue != "8")
                        {
                            //FISCAL MONEDA LOCAL
                            lb_contribuyente.SelectedValue = "2";//Contribuyente
                        }
                        #endregion
                    }
                    else
                    {
                        #region Empresas con monda Fiscal USD o Contabilidad Fiscal USD Afecta a Impuestos
                        if (((Bean_Intercompany_Origen.intC1 == 2) && (Bean_Intercompany_Destino.intC1 == 15)) || ((Bean_Intercompany_Origen.intC1 == 9) && (Bean_Intercompany_Destino.intC1 == 15)) || ((Bean_Intercompany_Origen.intC1 == 15) && (Bean_Intercompany_Destino.intC1 == 2)) || ((Bean_Intercompany_Origen.intC1 == 15) && (Bean_Intercompany_Destino.intC1 == 9)))
                        {
                            //SI AIMAR O LATIN SV LE COBRAR A AIMAR LOGISTICS NO GENERA IMPUESTOS PORQUE ES ZONA FRANCA
                            //SI AIMAR LOGISTICS SV LE COBRA A AIMAR O LATIN SV NO SE GENERA IMPUESTOS PORQUE ESTA EN UNA ZONA FRANCA
                            lb_contribuyente.SelectedValue = "1";//Excento
                        }
                        else
                        {
                            lb_contribuyente.SelectedValue = "2";//Contribuyente
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (user.contaID == 2)
                {
                    #region Contabilidad Financiera
                    lb_contribuyente.SelectedValue = "1";//Excento
                    #endregion
                }
                #endregion
            }
            else
            {
                #region Cobro a un Intercompany en otro Pais - No Genera Impuestos
                lb_contribuyente.SelectedValue = "1";//Excento
                #endregion
            }
            #endregion
        }
        if (drp_tipopersona.SelectedValue.Equals("4"))
        {
            GridViewRow row = gv_clientes.SelectedRow;
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            tb_nit.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            if ((tipo_contabilidad == 2) && (user.PaisID != 5))
            {
                lb_contribuyente.SelectedValue = "1";
            }
            else
            {
                lb_contribuyente.SelectedValue = DB.getProveedorRegimen(int.Parse(drp_tipopersona.SelectedValue), tbCliCod.Text.ToString()).ToString();
            }
        }
        else if (drp_tipopersona.SelectedValue.Equals("2"))
        {
            GridViewRow row = gv_clientes.SelectedRow;
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_nit.Text = Page.Server.HtmlDecode(row.Cells[8].Text);
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            if ((tipo_contabilidad == 2) && (user.PaisID != 5))
            {
                lb_contribuyente.SelectedValue = "1";
            }
            else
            {
                lb_contribuyente.SelectedValue = DB.getProveedorRegimen(int.Parse(drp_tipopersona.SelectedValue), tbCliCod.Text.ToString()).ToString();
            }
        }
        else if (drp_tipopersona.SelectedValue.Equals("5"))
        {
            GridViewRow row = gv_clientes.SelectedRow;
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            if ((tipo_contabilidad == 2) && (user.PaisID != 5))
            {
                lb_contribuyente.SelectedValue = "1";
            }
            else
            {
                lb_contribuyente.SelectedValue = DB.getProveedorRegimen(int.Parse(drp_tipopersona.SelectedValue), tbCliCod.Text.ToString()).ToString();
            }
        }
        else if (drp_tipopersona.SelectedValue.Equals("6"))
        {
            GridViewRow row = gv_clientes.SelectedRow;
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            if ((tipo_contabilidad == 2) && (user.PaisID != 5))
            {
                lb_contribuyente.SelectedValue = "1";
            }
            else
            {
                lb_contribuyente.SelectedValue = DB.getProveedorRegimen(int.Parse(drp_tipopersona.SelectedValue), tbCliCod.Text.ToString()).ToString();
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
        ArrayList Arr = null;
        string where = "";
        if (drp_tipopersona.SelectedValue == "3")
        {
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

            string tipo_consulta = "";
            if (user.SucursalID == 12)
            {
                tipo_consulta = "REPORTES";
            }

            ArrayList arr = (ArrayList)DB.getClientes(criterio, user, tipo_consulta);
            
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
        }
        else if (drp_tipopersona.SelectedValue.Equals("10"))
        {
            DataTable dt_intercompanys = null;
            if (!tb_nombre_cliente.Text.Trim().Equals("") && tb_nombre_cliente.Text != null) where += " and upper(rtrim(nombre_comercial)) like '%" + tb_nombre_cliente.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_intercompany=" + tb_codigo.Text; else where += " and id_intercompany=" + tb_codigo.Text;
            if (!tb_nit_cliente.Text.Trim().Equals("") && tb_nit_cliente.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nit_cliente.Text + "'"; else where += " and nit='" + tb_nit_cliente.Text + "'";
            Arr = (ArrayList)DB.Get_Intercompanys(where);//Intercompany
            dt_intercompanys = (DataTable)Utility.fillGridView("Intercompany", Arr);
            gv_clientes.DataSource = dt_intercompanys;
            gv_clientes.DataBind();
            ViewState["dt"] = dt_intercompanys;
        }

        else if (drp_tipopersona.SelectedValue.Equals("4"))
        {
            DataTable dt_proveedores = null;
            if (!tb_nombre_cliente.Text.Trim().Equals("") && tb_nombre_cliente.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombre_cliente.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nit_cliente.Text.Trim().Equals("") && tb_nit_cliente.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nit_cliente.Text + "'"; else where += "nit='" + tb_nit_cliente.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt_proveedores = (DataTable)Utility.fillGridView("Proveedor", Arr);
            gv_clientes.DataSource = dt_proveedores;
            gv_clientes.DataBind();
            ViewState["dt"] = dt_proveedores;
        }
        else if (drp_tipopersona.SelectedValue.Equals("2"))
        {
            DataTable dt_agentes = null;
            if (!tb_nombre_cliente.Text.Trim().Equals("") && tb_nombre_cliente.Text != null) where += " upper(rtrim(agente)) like '%" + tb_nombre_cliente.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(where, ""); //Agente
            dt_agentes = (DataTable)Utility.fillGridView("Agente", Arr);
            gv_clientes.DataSource = dt_agentes;
            gv_clientes.DataBind();
            ViewState["dt"] = dt_agentes;
        }
        else if (drp_tipopersona.SelectedValue.Equals("5"))
        {
            DataTable dt_navieras = null;
            if (!tb_nombre_cliente.Text.Trim().Equals("") && tb_nombre_cliente.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombre_cliente.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_naviera=" + tb_codigo.Text; else where += "id_naviera=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getNavieras(where, ""); //Naviera
            dt_navieras = (DataTable)Utility.fillGridView("Naviera", Arr);
            gv_clientes.DataSource = dt_navieras;
            gv_clientes.DataBind();
            ViewState["dt"] = dt_navieras;
        }
        else if (drp_tipopersona.SelectedValue.Equals("6"))
        {
            DataTable dt_carriers = null;
            if (!tb_nombre_cliente.Text.Trim().Equals("") && tb_nombre_cliente.Text != null) where += " and upper(rtrim(name)) ilike '%" + tb_nombre_cliente.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and carrier_id=" + tb_codigo.Text; else where += " and carrier_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getCarriers(where); //Lineas aereas
            dt_carriers = (DataTable)Utility.fillGridView("LineasAereas", Arr);
            gv_clientes.DataSource = dt_carriers;
            gv_clientes.DataBind();
            ViewState["dt"] = dt_carriers;
        }
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
        gv_consignee.DataBind();
        modalconsignee.Show();
    }
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
        if ((lb_serie_factura.Items.Count > 0))
        {
            if (lb_serie_factura.SelectedValue != "0")
            {
                if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
                {
                    int moneda_Serie = 0;
                    moneda_Serie = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                    lb_moneda.SelectedValue = moneda_Serie.ToString();//Moneda de la Transaccion
                    lb_moneda2.SelectedValue = moneda_Serie.ToString();//Moneda del Rubro
                    lb_moneda.Enabled = false;
                    RE_GenericBean fac_bean = (RE_GenericBean)DB.getFactura(int.Parse(lb_serie_factura.SelectedValue), 1);
                    lb_tipo_factura.SelectedValue = fac_bean.intC11.ToString();
                    if (lb_imp_exp.Items.Count > 0)
                    {
                        cargo_datos_BL(int.Parse(lb_imp_exp.SelectedValue));
                        Definir_Tipo_Operacion();
                        if ((lbl_tipoOperacionID.Text == "10") || (lbl_tipoOperacionID.Text == "15"))
                        {
                            if ((user.PaisID == 1) || (user.PaisID == 7) || (user.PaisID == 15))
                            {
                                //BAW FISCAL USD
                                if (lb_moneda.SelectedValue == "8")
                                {
                                    lb_contribuyente.SelectedValue = "1";
                                }
                                else
                                {
                                    string criterio_aux = "a.id_cliente=" + tbCliCod.Text.Trim();
                                    ArrayList clientearr_aux = (ArrayList)DB.getClientes(criterio_aux, user, "");
                                    if ((clientearr_aux != null) && (clientearr_aux.Count > 0))
                                    {
                                        RE_GenericBean clienteBean_aux;
                                        clienteBean_aux = (RE_GenericBean)clientearr_aux[0];
                                        lb_contribuyente.SelectedValue = clienteBean_aux.intC1.ToString();
                                    }
                                }
                            }
                            Actualizar_Cobro_Impuestos();
                        }
                    }
                }
            }
            else
            {
                gv_detalle.DataBind();
                Definir_Tipo_Operacion();
            }
        }
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/invoice/invoice.aspx");
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        Validacion_Zona_Franca();
        Validar_Restricciones("Export");
        #region Activar uso de Intermcompanys
        if (drp_tipopersona.SelectedValue == "10")
        {
            gv_detalle.Columns[0].Visible = true;
            bt_agregar.Visible = true;
            tb_cuenta_ModalPopupExtender.Enabled = true;

            tb_mbl.ReadOnly = true;
            tb_hbl.ReadOnly = true;
            tb_routing.ReadOnly = true;
            tb_contenedor.ReadOnly = true;
            tb_peso.ReadOnly = true;
            tb_vol.ReadOnly = true;
            tb_vapor.ReadOnly = true;
            tb_paquetes1.ReadOnly = true;
            tb_paquetes2.ReadOnly = true;

            modalPaquetes2.Enabled = false;
            modalAgentes.Enabled = false;
            modalNavieras.Enabled = false;
            modalcomodities.Enabled = false;
            modalconsignee.Enabled = false;
            modalVendedor1.Enabled = false;
            modalVendedor2.Enabled = false;

            lbl_correo_documento_electronico.Text = "";

            lb_tipo_transaccion.SelectedValue = "108";

        }
        else if (drp_tipopersona.SelectedValue == "3")
        {
            if (user.contaID == 1)
            {
                lb_tipo_transaccion.SelectedValue = "1";
            }
            else
            {
                lb_tipo_transaccion.SelectedValue = "7";
            }
        }
        else if (drp_tipopersona.SelectedValue == "4")
        {
            lb_tipo_transaccion.SelectedValue = "120";
        }
        else if (drp_tipopersona.SelectedValue == "2")
        {
            lb_tipo_transaccion.SelectedValue = "121";
        }
        else if (drp_tipopersona.SelectedValue == "5")
        {
            lb_tipo_transaccion.SelectedValue = "122";
        }
        else if (drp_tipopersona.SelectedValue == "6")
        {
            lb_tipo_transaccion.SelectedValue = "123";
        }
        #endregion
        #region Validar Existencia de Serie
        if (!Page.IsPostBack)
        {
            if (lb_serie_factura.Items.Count <= 0)
            {
                WebMsgBox.Show("No hay serie definida para esta Sucursal");
                bt_Enviar.Enabled = false;
                Cancel.Enabled = false;
            }
        }
        #endregion
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
        if ((DB.Validar_Restriccion_Activa(user, 1, 29) == true))
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
            ArrayList Arr_Restricciones = (ArrayList)DB.Get_Restricciones_XPais_Tipo(user, user.contaID, 1, user.PaisID, " and a.tbrp_suc_id=" + user.SucursalID + "");//1 Porque es una Factura
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
                //Se almacenan los clientes que estan asociados al BL
                Session["ClientesBillOfLading"] = null;
                Session["ClientesBillOfLading"] = new ClientesBL
                {
                    BLid = rgb.lonC1,
                    IdCliente = rgb.lonC4,
                    IdShipper = rgb.lonC3,
                    IdNotifyParty = rgb.lonC11,
                    IdColoader = rgb.lonC12
                };
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
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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
                tb_file.Text = rgb.strC6; //2020/11/04
                if (factura != null)
                {
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL(factura.BLID, "L", impo_expo, user, 1, factura.CliID.ToString());
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
                //Se almacenan los clientes que estan asociados al BL
                Session["ClientesBillOfLading"] = null;
                Session["ClientesBillOfLading"] = new ClientesBL
                {
                    BLid = rgb.lonC1,
                    IdCliente = rgb.lonC4,
                    IdShipper = rgb.lonC3,
                    IdNotifyParty = rgb.lonC11,
                    IdColoader = rgb.lonC12
                };
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
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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
                tb_file.Text = rgb.strC6; //2020/11/04
                //lb_imp_exp.Enabled = false;
                if (factura != null)
                {
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL(factura.BLID, "F", impo_expo, user, 1, factura.CliID.ToString());
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
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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


                tb_hbl.Text = rgb.strC9;
                tb_mbl.Text = factura.MBL;
                tb_routing.Text = bl_no;
                tb_contenedor.Text = factura.Contenedor;
                lb_contribuyente.SelectedValue = rgb_cliente.intC1.ToString();
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
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyPICK(factura.BLID, "A", user, 1);//importacion=0 exportacion=1
                }
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                #region Backup Cargo Datos BL Terrestre
                //double QS_BLID = 0;
                //if (Request.QueryString["blid"] != null)
                //{
                //    QS_BLID = double.Parse(Request.QueryString["blid"].ToString());
                //}
                //RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataTerrestre(QS_BLID, user);
                //if (rgb == null || rgb.lonC1 == 0)
                //{
                //    WebMsgBox.Show("El documento que esta tratando de facturar ya se encuentra facturado, por favor verificar");
                //    return;
                //}
                //#region Determinar Importacion Exportacion
                //int Tipo_Operacion = 0;
                //string Importacion_Exportacion = "0";
                //long aux = 0;
                //if (Request.QueryString["opid"] != null)
                //{
                //    Tipo_Operacion = int.Parse(Request.QueryString["opid"].ToString());
                //}
                //Importacion_Exportacion = DB.Determinar_ImporExport_XSistema(user, Tipo_Operacion, Convert.ToInt32(QS_BLID)).ToString();
                //if (Importacion_Exportacion == "2")
                //{
                //    aux = rgb.lonC4;
                //    rgb.lonC4 = rgb.lonC3;
                //    rgb.lonC3 = aux;
                //}
                //if ((Importacion_Exportacion == "1") || (Importacion_Exportacion == "2"))
                //{
                //    lb_imp_exp.SelectedValue = Importacion_Exportacion;
                //}
                //#endregion
                //factura = new FacturaBean();
                //factura.BLID = rgb.lonC1;
                //lbl_blID.Text = factura.BLID.ToString();
                //lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                //factura.HBL = rgb.strC1;
                //factura.MBL = rgb.strC2;
                //factura.Contenedor = rgb.strC4;

                //factura.CliID = rgb.lonC4;

                //agente_id = rgb.lonC6;

                //id_shipper.Text = rgb.lonC3.ToString();
                //id_consignee.Text = rgb.lonC4.ToString();

                //int paisRegitroCP = Utility.ISOPaistoInt(rgb.strC5);//
                //RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
                //if (rgb_cliente == null)
                //{
                //    WebMsgBox.Show("El cliente con codigo " + factura.CliID + " no se encuentra registrado para esta transaccion");
                //    return;
                //}
                //if (rgb_cliente.intC1 == -100)
                //{
                //    WebMsgBox.Show("El cliente no tiene configurado su regimen tributario, por lo tanto no se puede realizar la factura");
                //    return;
                //}
                //tb_consignee.Text = rgb_cliente.strC2;

                //factura.alias_rubro = rgb.intC2;//si requiere alias los rubros
                //// obtengo los datos del Vendedor 2
                //sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC2;
                //nombre = DB.getName(sql);
                //tb_vendedor2.Text = nombre.Trim();

                //// obtengo datos del shipper
                //sql = "select nombre_cliente from clientes where id_cliente=" + rgb.lonC3;
                //nombre = DB.getName(sql);
                //tb_shipper.Text = nombre.Trim();

                //tb_naviera.Text = DB.Get_Provider_By_ID(Convert.ToInt32(rgb.lonC5));

                //// obtendo datos de agente
                //sql = "select agente from agentes where agente_id=" + rgb.lonC6;
                //nombre = DB.getName(sql);
                //tb_agente_nombre.Text = nombre;
                //// obtengo datos de comodity
                //sql = "select namees from commodities where commodityid =" + rgb.lonC7;
                //nombre = DB.getName(sql);
                //tb_peso.Text = rgb.decC1.ToString();
                //tb_vol.Text = rgb.decC2.ToString();
                //tb_comodity.Text = nombre.Trim();
                //tb_vapor.Text = rgb.strC3;
                //tb_agenteid.Text = rgb.lonC6.ToString();
                //tb_paquetes1.Text = rgb.douC1.ToString();
                //tb_paquetes2.Text = rgb.strC6;
                //lb_contribuyente.SelectedValue = rgb_cliente.intC1.ToString();
                //tb_hbl.Text = factura.HBL;
                //tb_mbl.Text = factura.MBL;
                //idRouting = DB.GetIDRoutingbyNumero(rgb.strC7);
                //tb_routing.Text = rgb.strC7;
                //tb_vendedor1.Text = DB.GetVendedorByRouting(tb_routing.Text.Trim());
                //tb_contenedor.Text = factura.Contenedor;
                //lb_hbl.Text = "CPH";
                //lb_mbl.Text = "CP";
                //lb_tipotranporte.Text = "TRANSPORTISTA:";
                //lb_transporte.Text = "TRANSPORTE:";
                //if (factura != null)
                //{
                //    factura.RubrosHT = (Hashtable)DB.getRubrosbyTerrestre(factura.BLID, paisRegitroCP, user, 1);//importacion=0 exportacion=1
                //}
                #endregion
                factura = new FacturaBean();
                double QS_BLID = 0;
                if (Request.QueryString["blid"] != null)
                {
                    QS_BLID = double.Parse(Request.QueryString["blid"].ToString());
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataTerrestre(QS_BLID, user);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un inconveniente al momento de importar los Datos de la Carta de Porte!!");
                    return;
                }
                //Se almacenan los clientes que estan asociados al BL
                Session["ClientesBillOfLading"] = null;
                Session["ClientesBillOfLading"] = new ClientesBL
                {
                    BLid = rgb.lonC1,
                    IdCliente = rgb.lonC4,
                    IdShipper = rgb.lonC3,
                    IdNotifyParty = rgb.lonC9,
                    IdColoader = rgb.lonC10
                };

                #region Determinar Importacion Exportacion
                int Tipo_Operacion = 0;
                string Importacion_Exportacion = "0";
                long aux = 0;
                if (Request.QueryString["opid"] != null)
                {
                    Tipo_Operacion = int.Parse(Obtener_Tipo_Operacion());
                }
                if (Tipo_Operacion == 6 && (user.PaisID == 4 || user.PaisID == 24))
                {
                    Importacion_Exportacion = DB.Determinar_ImporExport_XSistema(user, Tipo_Operacion, Convert.ToInt32(QS_BLID)).ToString();

                    if (Importacion_Exportacion == "0")
                        Importacion_Exportacion = DB.Determinar_ImporExport_XSistemaNicaragua(user, Tipo_Operacion, Convert.ToInt32(QS_BLID)).ToString();
                }
                else
                    Importacion_Exportacion = DB.Determinar_ImporExport_XSistema(user, Tipo_Operacion, Convert.ToInt32(QS_BLID)).ToString();

                if (Importacion_Exportacion == "0")
                {
                    bt_Enviar.Enabled = false;
                    WebMsgBox.Show("Favor verificar con el personal del Departamento Terrestre que la CP ya tenga Fecha de Arribo y corresponda a la Empresa donde se desea Facturar.");
                    return;
                }
                if ((Importacion_Exportacion == "1") || (Importacion_Exportacion == "2"))
                {
                    lb_imp_exp.SelectedValue = Importacion_Exportacion;
                }

                if (Importacion_Exportacion == "1")
                {
                    //IMPORTACION FACTURAR AL CONSIGNATARIO
                    factura.CliID = rgb.lonC4;
                }
                if (Importacion_Exportacion == "2")
                {
                    //EXPORTACION FACTURAR AL SHIPPER
                    factura.CliID = rgb.lonC3;
                }
                #endregion
               
                // obtengo datos del Shipper
                sql = "select nombre_cliente from clientes where id_cliente=" + rgb.lonC3;
                nombre = DB.getName(sql);
                tb_shipper.Text = nombre.Trim();
                id_shipper.Text = rgb.lonC3.ToString();
                
                // obtengo datos del Consignatario
                sql = "select nombre_cliente from clientes where id_cliente=" + rgb.lonC4;
                nombre = DB.getName(sql);
                tb_consignee.Text = nombre.Trim();
                id_consignee.Text = rgb.lonC4.ToString();

                
                factura.BLID = rgb.lonC1;
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                factura.Contenedor = rgb.strC4;                

                int paisRegitroCP = Utility.ISOPaistoInt(rgb.strC5);
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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
                factura.alias_rubro = rgb.intC2;//si requiere alias los rubros

                // obtengo los datos del Vendedor 2
                sql = "select pw_name from usuarios_empresas where id_usuario=" + rgb.lonC2;
                nombre = DB.getName(sql);
                tb_vendedor2.Text = nombre.Trim();

                // obtengo datos de agente
                sql = "select agente from agentes where agente_id=" + rgb.lonC6;
                nombre = DB.getName(sql);
                tb_agente_nombre.Text = nombre;
                agente_id = rgb.lonC6;

                // obtengo datos del transportista o naviera
                tb_naviera.Text = DB.Get_Provider_By_ID(Convert.ToInt32(rgb.lonC5));

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
                tb_file.Text = rgb.strC9; //2020//11//04

                if (factura != null)
                {
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyTerrestre(factura.BLID, paisRegitroCP, user, 1);//importacion=0 exportacion=1
                }
            }
            else if (tipo.Equals("AEREO"))
            {
                // obtengo los datos de la factura 
                int Tipo_Guia = 0;
                if (Request.QueryString["opid"] != null)
                {
                    Tipo_Guia = int.Parse(Obtener_Tipo_Operacion());
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataAereo(bl_no, impo_expo, Tipo_Guia, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("El documento que esta tratando de facturar ya se encuentra facturado o no indico la Serie y Operacion correcta, por favor verificar");
                    return;
                }
                //Se almacenan los clientes que estan asociados al BL
                Session["ClientesBillOfLading"] = null;
                Session["ClientesBillOfLading"] = new ClientesBL
                {
                    BLid = rgb.lonC1,
                    IdCliente = rgb.lonC4,
                    IdShipper = rgb.lonC3,
                    IdNotifyParty = rgb.lonC9,
                    IdColoader = rgb.lonC10
                };
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
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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
                lb_imp_exp.SelectedValue = rgb.strC10;
                tb_file.Text = rgb.strC11; //2020/11/04
                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL_Aereo(factura.BLID, paisdest, user, rgb, 1);//importacion=0 exportacion=1
                }
            }
            else if (tipo.Equals("AEREO BATCH"))
            {
                // obtengo los datos de la factura 
                int Tipo_Guia = 0;
                if (Request.QueryString["opid"] != null)
                {
                    Tipo_Guia = int.Parse(Obtener_Tipo_Operacion());
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataAereoBatch(bl_no, impo_expo, Tipo_Guia, blID);

                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("El documento que esta tratando de facturar ya se encuentra facturado o no indico la Serie y Operacion correcta, por favor verificar");
                    return;
                }
                //Se almacenan los clientes que estan asociados al BL
                Session["ClientesBillOfLading"] = null;
                Session["ClientesBillOfLading"] = new ClientesBL
                {
                    BLid = rgb.lonC1,
                    IdCliente = rgb.lonC4,
                    IdShipper = rgb.lonC3,
                    IdNotifyParty = rgb.lonC9,
                    IdColoader = rgb.lonC10
                };
                factura = new FacturaBean();
                factura.BLID = rgb.lonC1;
                lbl_blID.Text = factura.BLID.ToString();
                lbl_tipoOperacionID.Text = Obtener_Tipo_Operacion();
                factura.HBL = rgb.strC1;
                factura.MBL = rgb.strC2;
                //factura.Contenedor = rgb.strC4;
                factura.Contenedor = rgb.strC6; //AQUI VIENE EL TIPO DE BATCH
                factura.imp_exp = int.Parse(rgb.strC10); //AQUI VIENE EL TIPO DE OPERACION IMPORT EXPORT
                factura.CliID = rgb.lonC4;
                agente_id = rgb.lonC6;
                id_shipper.Text = rgb.lonC3.ToString();
                id_consignee.Text = rgb.lonC4.ToString();
                int paisdest = Utility.ISOPaistoInt(rgb.strC9);//
                // obtengo los datos del cliente
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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
                tb_paquetes2.Text = ""; //rgb.strC6; //AQUI VIENE EL TIPO DE BATCH
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
                lb_imp_exp.SelectedValue = rgb.strC10;
                if (factura != null)
                {
                    // obtengo los rubros a partir del bl
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL_AereoBATCH(factura.BLID, paisdest, user, rgb, Int32.Parse(rgb.strC10));//importacion=0 exportacion=1
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
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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
                tb_file.Text = rgb.strC13;
                if (user.PaisID == 11)
                {
                    tb_orden.Text = rgb.strC8;
                    tb_no_factura_aduana.Text = rgb.strC9;
                    tb_otras_observaciones.Text = "REFERENCIA INTERNA.: " + rgb.strC10 + "       -       ADUANA DESPACHO.: " + rgb.strC11 + "";
                    tb_no_embarque.Text = rgb.strC12;
                    tb_orden.ReadOnly = true;
                    tb_no_factura_aduana.ReadOnly = true;
                    tb_no_embarque.ReadOnly = true;
                }
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
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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
                tb_file.Text = rgb.strC8;
                if (factura != null)
                {
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyRO(user, factura.BLID, 14, 1);
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
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyDemora(user,rgb.intC1 , 1);
                }
            }
            else if (tipo.Equals("FCL APL"))
            {
                // obtengo los datos de la factura 
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataFCL_APL(bl_no, user, blID);
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
                RE_GenericBean rgb_cliente = (RE_GenericBean)DB.getDataClient((int)factura.CliID, user.PaisID);
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
                    factura.RubrosHT = (Hashtable)DB.getRubrosbyBL_APL(factura.BLID, "F", impo_expo, user);
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


            string criterio = "";
            ArrayList clientearr = null;
            RE_GenericBean clienteBean = null;
            drp_tipopersona.SelectedValue = "3";
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
                    drp_tipo_identificacion_cliente.SelectedValue = clienteBean.strC10;
                    Validar_Cliente_Credito_Contado();
                }
            }

            //BAW FISCAL USD
            if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
            {
                //EXT
                #region Definir Tipo de Contribuyente - Fiscal USD
                if ((user.PaisID == 1) || (user.PaisID == 7) || (user.PaisID == 15))
                {
                    if (lb_moneda.SelectedValue == "8")
                    {
                        lb_contribuyente.SelectedValue = "1";
                    }
                }
                #endregion
                gv_detalle.DataSource = (DataTable)Nuevo_Lleno_DataTable(factura.RubrosHT, factura.alias_rubro, factura.CliID);
                gv_detalle.DataBind();
            }
            else
            {
                gv_detalle.DataSource = (DataTable)llenoDataTable(factura.RubrosHT, factura.alias_rubro, factura.CliID);
                gv_detalle.DataBind();
            }

            Marcar_Cargos_Locales_Internacionales();
            
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
        int _MonedaAUX = 0;
        if (lb_moneda.SelectedValue != "")
        {
            _MonedaAUX = int.Parse(lb_moneda.SelectedValue);
        }
        if (paiR == true)
        {
            #region CARGAR A PARTIR DE UN DOCUMENTO
            //if (DB.Validar_Restriccion_Activa(user, 1, 1) == true)
            //if ((DB.Validar_Restriccion_Activa(user, 1, 1) == true) && (drp_tipopersona.SelectedValue != "10"))
            if ((DB.Validar_Restriccion_Activa(user, 1, 1) == true) && (drp_tipopersona.SelectedValue == "3"))
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
            //if (DB.Validar_Restriccion_Activa(user, 1, 12)==true)
            if ((DB.Validar_Restriccion_Activa(user, 1, 12) == true) && (drp_tipopersona.SelectedValue == "3"))
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
            if ((DB.Validar_Restriccion_Activa(user, 1, 2) == true) && (sender == "btn_enviar"))
            {
                int servicioID = 0;
                int cantidad_rubros_aduanas = 0;
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
                        cantidad_rubros_aduanas++;
                    }
                }
            }
            #endregion
            #region NO SE PUEDEN AGREGAR RUBROS DE TERCEROS
            if ((DB.Validar_Restriccion_Activa(user, 1, 3) == true) && (sender == "btn_enviar"))
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
            if ((DB.Validar_Restriccion_Activa(user, 1, 4) == true) && (sender == "btn_enviar"))
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
            if ((DB.Validar_Restriccion_Activa(user, 1, 5) == true) && (sender == "llenoDataTable"))
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
            #region FACTURACION ELECTRONICA
            if ((DB.Validar_Restriccion_Activa(user, 1, 14) == true) && (sender == "send_einvoice"))
            {
                resultado = true;
                return resultado;
            }
            #endregion
            #region COBRO UNICAMENTE A CLIENTES LOCALES
            if ((DB.Validar_Restriccion_Activa(user, 1, 15) == true) && (sender == "btn_enviar_cliente") && (drp_tipopersona.SelectedValue == "3") && (_MonedaAUX != 8))
            {
                //VALIDAR USD ACA
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
            if ((DB.Validar_Restriccion_Activa(user, 1, 16) == true) && (sender == "send_nit") && (_MonedaAUX != 8))
            {
                //VALIDAR USD ACA
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
            if ((DB.Validar_Restriccion_Activa(user, 1, 18) == true) && (sender == "btn_cliente_contribuyente"))
            {
                resultado = false;
                int rubros_excentos = 0;
                if (gv_detalle.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gv_detalle.Rows)
                    {
                        Label lb1 = (Label)row.FindControl("lb_impuesto");
                        if (decimal.Parse( lb1.Text)==0)
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
            if ((DB.Validar_Restriccion_Activa(user, 1, 19) == true) && (sender == "btn_cliente_excento"))
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
            if ((DB.Validar_Restriccion_Activa(user, 1, 20) == true) && (sender == "gv_detalle_RowDeleting"))
            {
                resultado = true;
            }
            #endregion
            #region ELIMINAR Y AGREGAR RUBROS
            //if ((DB.Validar_Restriccion_Activa(user, 1, 21) == true) && (sender == "cargo_datos_BL"))
            if ((DB.Validar_Restriccion_Activa(user, 1, 21) == true) && (sender == "cargo_datos_BL") && (drp_tipopersona.SelectedValue == "3"))
            {
                if (lbl_tipoOperacionID.Text != "10")
                {
                    gv_detalle.Columns[0].Visible = false;
                    bt_agregar.Visible = false;
                }
            }
            #endregion
            #region AGREGAR RUBROS
            //if ((DB.Validar_Restriccion_Activa(user, 1, 23) == true) && (sender == "cargo_datos_BL"))
            if ((DB.Validar_Restriccion_Activa(user, 1, 23) == true) && (sender == "cargo_datos_BL") && (drp_tipopersona.SelectedValue == "3"))
            {
                if (lbl_tipoOperacionID.Text != "10")
                {
                    gv_detalle.Columns[0].Visible = true;
                    bt_agregar.Visible = false;
                }
            }
            #endregion
            #region NO EMITIR COBRO MANUAL CLIENTE ECONO
            if ((DB.Validar_Restriccion_Activa(user, 1, 25) == true) && (sender == "Validar_Econocaribe"))
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
            if ((DB.Validar_Restriccion_Activa(user, 1, 27) == true) && (sender == "Validar_Cliente_Agente") && (drp_tipopersona.SelectedValue == "3"))
            {
                if ((user.pais.Grupo_Empresas == 1) && (tbCliCod.Text.Trim() != "") && (tb_agenteid.Text.Trim() != "0"))
                {
                    int v_cliID = 0;
                    int v_agenteID = 0;
                    v_cliID = int.Parse(tbCliCod.Text);
                    //v_cliID = 61903;//Coloader
                    //v_cliID = 1814;//Directo
                    v_agenteID = int.Parse(tb_agenteid.Text);
                    //v_agenteID = 194;//No es Econo
                    //v_agenteID = 469;//Es Econo
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
                        bt_factura_virtual.Enabled = false;
                        if ((user.PaisID == 1 || user.PaisID == 15))
                        {
                            bt_impresion_invoice.Visible = false;
                        }
                    }
                }
                
            }
            #endregion
            #region COBRO DOCUMENTOS REFERENCIADOS
            //if (DB.Validar_Restriccion_Activa(user, 1, 30) == true)
            if ((DB.Validar_Restriccion_Activa(user, 1, 30) == true) && (drp_tipopersona.SelectedValue == "3"))
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
            else if (drp_tipopersona.SelectedValue == "3")
            {
                if ((lbl_tipoOperacionID.Text == "15") && (sender == "Documento_Referenciado"))
                {
                    WebMsgBox.Show("Factura Referenciada. No se pueden Cobrar Rubros agregados manualmente, los Rubros deben proceder de los Sistemas de Trafico");
                    resultado = true;
                }
            }
            #endregion
            #region POLIZA DE ADUANAS REPETIDA
            if ((DB.Validar_Restriccion_Activa(user, 1, 31) == true) && (sender == "Poliza_Repetida"))
            {
                resultado = false;
                if (tb_orden.Text.Trim() != "")
                {
                    int Cantidad_Facturas = DB.Validar_Numero_Poliza_X_Cliente(user, tb_orden.Text.Trim(), tbCliCod.Text.Trim());
                    if (Cantidad_Facturas > 0)
                    {
                        WebMsgBox.Show("El numero de poliza.: " + tb_orden.Text.Trim() + " ya fue utilizado en otra factura al mismo cliente, por lo tanto no puede ser utilizado nuevamente.");
                        resultado = true;
                    }
                }
            }
            #endregion
            #region SERVICIOS AUXILIARES DE ADUANA
            if ((DB.Validar_Restriccion_Activa(user, 1, 32) == true) && (sender == "btn_enviar"))
            {
                int servicioID = 0;
                int rubID = 0;
                string rubroName="";
                double Base_Original = 0;
                double Base_Equivalente = 0;
                resultado = false;
                foreach (GridViewRow row in gv_detalle.Rows)
                {
                    Label lb_tts = (Label)row.FindControl("lb_tipo");
                    Label lb_rubID = (Label)row.FindControl("lb_codigo");
                    Label lb_rubro = (Label)row.FindControl("lb_rubro");
                    Label lb_base = (Label)row.FindControl("lb_subtotal");
                    servicioID = Utility.TraducirServiciotoINT(lb_tts.Text);
                    rubID =  int.Parse(lb_rubID.Text.Trim());
                    rubroName = lb_rubro.Text.Trim();
                    Base_Original = double.Parse(lb_base.Text.Trim());
                    if (lb_moneda.SelectedValue == "8")
                    {
                        Base_Equivalente = Math.Round(Base_Original * (double)user.pais.TipoCambio, 2);
                    }
                    else
                    {
                        Base_Equivalente = Math.Round(Base_Original / (double)user.pais.TipoCambio, 2);
                    }
                    if (lb_moneda.SelectedValue == "8")
                    {
                        if ((servicioID == 9) && (rubID == 739) && (Base_Equivalente < 5000))
                        {
                            WebMsgBox.Show("El monto base ingresado para el rubro " + rubroName + " no puede ser menor de 5,000 colones , favor consulte con el contador de su pais");
                            resultado = true;
                            return resultado;
                        }
                    }
                    else
                    {
                        if ((servicioID == 9) && (rubID == 739) && (Base_Original < 5000))
                        {
                            WebMsgBox.Show("El monto base ingresado para el rubro " + rubroName + " no puede ser menor de 5,000 colones , favor consulte con el contador de su pais");
                            resultado = true;
                            return resultado;
                        }
                    }
                }
            }
            #endregion
            #region COBRO CLIENTES DIRECTOS Y COLOADER CON TODOS LOS AGENTES EXCEPTO CAROTRANS, CRAFT, COLGRUPAJE, FASTFORWARD
            if ((DB.Validar_Restriccion_Activa(user, 1, 35) == true) && (sender == "Validar_Cliente_DYC_EXAG"))
            {
                if ((user.pais.Grupo_Empresas == 1) && (tbCliCod.Text.Trim() != "") && (tb_agenteid.Text.Trim() != "0"))
                {
                    int v_agenteID = 0;
                    int id_Grupo=0;
                    v_agenteID = int.Parse(tb_agenteid.Text);
                    RE_GenericBean Agente_Bean = DB.getAgenteData(v_agenteID, "");
                    id_Grupo= int.Parse(Agente_Bean.strC8);
                    if (Agente_Bean!=null)
                    {
                        id_Grupo= int.Parse(Agente_Bean.strC8);
                        if ((id_Grupo == 308) || (id_Grupo == 250) || (id_Grupo == 309) || (id_Grupo == 310))
                        {
                            resultado = true;
                            WebMsgBox.Show("No puede realizar Operaciones con el Agente.: " + tb_agente_nombre.Text.Trim() + " desde este Modulo, Favor utilice el Modulo de Latin Freight");
                            bt_Enviar.Enabled = false;
                            Cancel.Enabled = false;
                            bt_imprimir.Enabled = false;
                            bt_factura_virtual.Enabled = false;
                            return resultado;
                        }
                    }
                }
            }
            #endregion
            #region COBRO CLIENTES COLOADER UNICAMENTE CON AGENTES CAROTRANS, CRAFT, COLGRUPAJE, FASTFORWARD
            if ((DB.Validar_Restriccion_Activa(user, 1, 36) == true) && (sender == "Validar_Cliente_COL_CONEXAG") && (drp_tipopersona.SelectedValue == "3"))
            {
                if ((user.pais.Grupo_Empresas == 2) && (tbCliCod.Text.Trim() != "") && (tb_agenteid.Text.Trim() != "0"))
                {
                    int v_cliID = 0;
                    int v_agenteID = 0;
                    int id_Grupo = 0;
                    v_cliID = int.Parse(tbCliCod.Text);
                    v_agenteID = int.Parse(tb_agenteid.Text);
                    string v_sql = "a.id_cliente=" + v_cliID;
                    ArrayList V_ClienteArr = (ArrayList)DB.getClientes(v_sql, user, "");
                    RE_GenericBean V_Cliente_Bean = (RE_GenericBean)V_ClienteArr[0];
                    if (V_Cliente_Bean.strC8 == "False")
                    {
                        resultado = true;
                        WebMsgBox.Show("No puede realizar Operaciones con directos desde este Modulo, Favor utilice el Modulo de Aimar");
                        bt_Enviar.Enabled = false;
                        Cancel.Enabled = false;
                        bt_imprimir.Enabled = false;
                        bt_factura_virtual.Enabled = false;
                        return resultado;
                    }
                    RE_GenericBean Agente_Bean = DB.getAgenteData(v_agenteID, "");
                    if (Agente_Bean != null)
                    {
                        id_Grupo = int.Parse(Agente_Bean.strC8);
                        if ((id_Grupo != 308) && (id_Grupo != 250) && (id_Grupo != 309) && (id_Grupo != 310))
                        {
                            resultado = true;
                            WebMsgBox.Show("No puede realizar Operaciones con el Agente.: " + tb_agente_nombre.Text.Trim() + " desde este Modulo, Favor utilice el Modulo de Aimar");
                            bt_Enviar.Enabled = false;
                            Cancel.Enabled = false;
                            bt_imprimir.Enabled = false;
                            bt_factura_virtual.Enabled = false;
                            return resultado;
                        }
                    }
                }
            }
            #endregion
            #region COBRO CLIENTES DIRECTOS UNICAMENTE CON AGENTES CAROTRANS, CRAFT, COLGRUPAJE, FASTFORWARD
            if ((DB.Validar_Restriccion_Activa(user, 1, 37) == true) && (sender == "Validar_Cliente_COL_CONEXAG") && (drp_tipopersona.SelectedValue == "3"))
            {
                if ((user.pais.Grupo_Empresas == 2) && (tbCliCod.Text.Trim() != "") && (tb_agenteid.Text.Trim() != "0"))
                {
                    int v_cliID = 0;
                    int v_agenteID = 0;
                    int id_Grupo = 0;
                    v_cliID = int.Parse(tbCliCod.Text);
                    v_agenteID = int.Parse(tb_agenteid.Text);
                    string v_sql = "a.id_cliente=" + v_cliID;
                    ArrayList V_ClienteArr = (ArrayList)DB.getClientes(v_sql, user, "");
                    RE_GenericBean V_Cliente_Bean = (RE_GenericBean)V_ClienteArr[0];
                    if (V_Cliente_Bean.strC8 == "True")
                    {
                        resultado = true;
                        WebMsgBox.Show("No puede realizar Operaciones con coloaders desde este Modulo, Favor utilice el Modulo de Aimar");
                        bt_Enviar.Enabled = false;
                        Cancel.Enabled = false;
                        bt_imprimir.Enabled = false;
                        bt_factura_virtual.Enabled = false;
                        return resultado;
                    }
                    RE_GenericBean Agente_Bean = DB.getAgenteData(v_agenteID, "");
                    if (Agente_Bean != null)
                    {
                        id_Grupo = int.Parse(Agente_Bean.strC8);
                        if ((id_Grupo != 308) && (id_Grupo != 250) && (id_Grupo != 309) && (id_Grupo != 310))
                        {
                            resultado = true;
                            WebMsgBox.Show("No puede realizar Operaciones con el Agente.: " + tb_agente_nombre.Text.Trim() + " desde este Modulo, Favor utilice el Modulo de Aimar");
                            bt_Enviar.Enabled = false;
                            Cancel.Enabled = false;
                            bt_imprimir.Enabled = false;
                            bt_factura_virtual.Enabled = false;
                            return resultado;
                        }
                    }
                }
            }
            #endregion
            #region COBRO CLIENTES DIRECTOS Y COLOADER UNICAMENTE CON LOS AGENTES CAROTRANS, CRAFT, COLGRUPAJE, FASTFORWARD
            if ((DB.Validar_Restriccion_Activa(user, 1, 38) == true) && (sender == "Validar_Cliente_DYC_EXAG"))
            {
                if ((user.pais.Grupo_Empresas == 2) && (tbCliCod.Text.Trim() != "") && (tb_agenteid.Text.Trim() != "0"))
                {
                    int v_agenteID = 0;
                    int id_Grupo = 0;
                    v_agenteID = int.Parse(tb_agenteid.Text);
                    RE_GenericBean Agente_Bean = DB.getAgenteData(v_agenteID, "");
                    if (Agente_Bean != null)
                    {
                        id_Grupo = int.Parse(Agente_Bean.strC8);
                        if ((id_Grupo != 308) && (id_Grupo != 250) && (id_Grupo != 309) && (id_Grupo != 310))
                        {
                            resultado = true;
                            WebMsgBox.Show("No puede realizar Operaciones con el Agente.: " + tb_agente_nombre.Text.Trim() + " desde este Modulo, Favor utilice el Modulo de Aimar");
                            bt_Enviar.Enabled = false;
                            Cancel.Enabled = false;
                            bt_imprimir.Enabled = false;
                            bt_factura_virtual.Enabled = false;
                            return resultado;
                        }
                    }
                }
            }
            #endregion
            #region SUCURSAL DE EXPORTACIONES
            if ((DB.Validar_Restriccion_Activa(user, 1, 41) == true) && ((sender == "btn_enviar_export") || (sender == "Export")))
            {
                resultado = false;
                if (sender == "Export")
                {
                    lb_contribuyente.SelectedValue = "1";
                    int ban_temp = 0;
                    ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
                    foreach (PerfilesBean Perfil in Arr_Perfiles)
                    {
                        if (Perfil.ID == 28)
                        {
                            ban_temp++;
                        }
                    }
                    if (ban_temp == 0)
                    {
                        WebMsgBox.Show("Usted No tiene permisos para realizar Facturas de Exportacion");
                        bt_Enviar.Enabled = false;
                        Cancel.Enabled=false;
                        bt_imprimir.Enabled = false;
                        bt_factura_virtual.Enabled = false;
                        bt_agregar.Enabled = false;
                        gv_detalle.Enabled = false;
                        lb_contribuyente.Enabled = false;
                        lb_imp_exp.Enabled = false;
                        lb_tipo_transaccion.Enabled = false;
                        lb_serie_factura.Enabled = false;
                        drp_regimen_aduanero.Enabled = false;
                        return resultado;
                    }
                }
            }
            #endregion
            #region RUBROS DE SEGUROS :: SE DEBE INGRESAR POLIZA DE SEGUROS
            if ((DB.Validar_Restriccion_Activa(user, 1, 44) == true) && (sender == "btn_enviar"))
            {
                if (drp_tipopersona.SelectedValue == "3")
                {
                    int servicioID = 0;
                    int cantidad_rubros_seguros = 0;
                    resultado = false;
                    foreach (GridViewRow row in gv_detalle.Rows)
                    {
                        Label lb = (Label)row.FindControl("lb_tipo");
                        servicioID = Utility.TraducirServiciotoINT(lb.Text);
                        if (servicioID == 6)//SEGUROS
                        {
                            if (tb_poliza_seguros.Text.Trim().Equals(""))
                            {
                                WebMsgBox.Show("Por favor ingrese el Certificado de Seguros.");
                                resultado = true;
                                return resultado;
                            }
                            cantidad_rubros_seguros++;
                        }
                    }
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
            if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 15) || (user.PaisID == 21))
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
        //if (user.PaisID == 1)
        //{
        //    if (Rubro.rubtoType == "ADUANAS")
        //    {
        //        //if ((Rubro.rubroID == 212) || (Rubro.rubroID == 133) || (Rubro.rubroID == 216) || (Rubro.rubroID == 232) || (Rubro.rubroID == 63) || (Rubro.rubroID == 243) || (Rubro.rubroID == 303) || (Rubro.rubroID == 125) || (Rubro.rubroID == 123) || (Rubro.rubroID == 304) || (Rubro.rubroID == 682) || (Rubro.rubroID == 439) || (Rubro.rubroID == 69) || (Rubro.rubroID == 70) || (Rubro.rubroID == 665) || (Rubro.rubroID == 647) || (Rubro.rubroID == 646) || (Rubro.rubroID == 196) || (Rubro.rubroID == 58) || (Rubro.rubroID == 492))
        //        if ((Rubro.rubroID == 212) || (Rubro.rubroID == 133) || (Rubro.rubroID == 216) || (Rubro.rubroID == 232) || (Rubro.rubroID == 63) || (Rubro.rubroID == 243) || (Rubro.rubroID == 303) || (Rubro.rubroID == 125) || (Rubro.rubroID == 123) || (Rubro.rubroID == 304) || (Rubro.rubroID == 682) || (Rubro.rubroID == 439) || (Rubro.rubroID == 69) || (Rubro.rubroID == 70) || (Rubro.rubroID == 665) || (Rubro.rubroID == 647) || (Rubro.rubroID == 646) || (Rubro.rubroID == 196) || (Rubro.rubroID == 58) || (Rubro.rubroID == 492) || (Rubro.rubroID == 687))
        //        {
        //            resultado = false;
        //        }
        //    }
        //}
        #endregion
        #region Aproa
        //else if (user.PaisID == 14)
        //{
        //    resultado = false;
        //    if (Rubro.rubtoType == "ADUANAS")
        //    {
        //        //if ((Rubro.rubroID == 212) || (Rubro.rubroID == 133) || (Rubro.rubroID == 216) || (Rubro.rubroID == 232) || (Rubro.rubroID == 63) || (Rubro.rubroID == 243) || (Rubro.rubroID == 303) || (Rubro.rubroID == 125) || (Rubro.rubroID == 123) || (Rubro.rubroID == 304) || (Rubro.rubroID == 682) || (Rubro.rubroID == 439) || (Rubro.rubroID == 69) || (Rubro.rubroID == 70) || (Rubro.rubroID == 665) || (Rubro.rubroID == 647) || (Rubro.rubroID == 646) || (Rubro.rubroID == 196) || (Rubro.rubroID == 58) || (Rubro.rubroID == 492) || (Rubro.rubroID == 688))
        //        if ((Rubro.rubroID == 212) || (Rubro.rubroID == 133) || (Rubro.rubroID == 216) || (Rubro.rubroID == 232) || (Rubro.rubroID == 63) || (Rubro.rubroID == 243) || (Rubro.rubroID == 303) || (Rubro.rubroID == 125) || (Rubro.rubroID == 123) || (Rubro.rubroID == 304) || (Rubro.rubroID == 682) || (Rubro.rubroID == 439) || (Rubro.rubroID == 69) || (Rubro.rubroID == 70) || (Rubro.rubroID == 665) || (Rubro.rubroID == 647) || (Rubro.rubroID == 646) || (Rubro.rubroID == 196) || (Rubro.rubroID == 58) || (Rubro.rubroID == 492) || (Rubro.rubroID == 688) || (Rubro.rubroID == 687))
        //        {
        //            resultado = true;
        //        }
        //    }
        //}
        #endregion
        return resultado;
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
            //modalconsignee.Enabled = true;
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
            //modalconsignee.Enabled = false;
            modalVendedor1.Enabled = false;
            modalVendedor2.Enabled = false;
            #endregion
        }
    }
    protected void Validar_Cobros_Repetidos(int ttrID, ArrayList Arr)
    {
 
    }
    protected void Actualizar_Cobro_Impuestos()
    {
        if (lb_contribuyente.Items.Count > 0)
        {
            
            if (gv_detalle.Rows.Count > 0)
            {
                if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; simboloequivalente = "Local"; }
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
                dt.Columns.Add("LOCAL_INTERNACIONAL");
                dt.Clear();
                Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
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
                    lb10 = (Label)row.FindControl("lbl_local_internacional");
                    Rubros rubro = new Rubros();
                    rubro.rubroID = long.Parse(lb1.Text);
                    rubro.rubroName = lb2.Text;

                    RE_GenericBean Configuracion_Rubro = DB.Get_Configuracion_Rubro(user.PaisID, Convert.ToInt32(rubro.rubroID));

                    int cliID = int.Parse(tbCliCod.Text);
                    int requierealias = int.Parse(lb_requierealias.Text);
                    if (requierealias == 1) rubro.rubroName = DB.getAliasRubro(user.PaisID, rubro.rubroID, cliID, rubro.rubroName);
                    rubro.rubtoType = lb3.Text;
                    rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
                    rubro.rubroTypeID = Utility.TraducirServiciotoINT(lb3.Text);

                    int Tipo_Contribuyente = int.Parse(lb_contribuyente.SelectedValue);
                    string Tipo_Cargo = "";
                    Tipo_Cargo = lb10.Text;
                    #region Validar Cargos Locales e Internacionales
                    if (Tipo_Cargo == "LOCAL")
                    {
                        Tipo_Contribuyente = Tipo_Contribuyente;
                    }
                    else if (Tipo_Cargo == "MANUAL")
                    {
                        Tipo_Contribuyente = Tipo_Contribuyente;
                    }
                    else if (Tipo_Cargo == "INTERNACIONAL")
                    {
                        if (user.PaisID == 1 && lb_imp_exp.SelectedValue == "2")
                        {
                            Tipo_Contribuyente = Tipo_Contribuyente;
                        }
                        else
                        {
                            Tipo_Contribuyente = 1;//Excento: Cambio aplicado para que los Cargos Internacionales no generen impuestos
                        }
                    }
                    #endregion

                    #region Obtener el monto del Rubro en base a su configuracion
                    if (Configuracion_Rubro.intC1 == 0)
                    {
                        //Rubro Excento
                        rubro.rubroTot = double.Parse(lb4.Text);//Obtengo valor sobre la Base
                    }
                    else
                    {
                        //Rubro Afecto a Impuestos
                        if (Configuracion_Rubro.intC2 == 0)
                        {
                            //Rubro sin Impuesto Incluido
                            rubro.rubroTot = double.Parse(lb4.Text);//Obtengo valor sobre la Base
                        }
                        else if (Configuracion_Rubro.intC2 == 1)
                        {
                            //Rubro con Impuesto Incluido
                            rubro.rubroTot = double.Parse(lb6.Text);//Obtengo valor sobre la Base
                        }
                    }
                    #endregion

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
                    //if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))//si debe cobrar iva y el rubro no esta en dolares y no es excento
                    if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (Tipo_Contribuyente == 2))//si debe cobrar iva y el rubro no esta en dolares y no es excento
                    {
                        if (user.PaisID == 1 && lb_imp_exp.SelectedValue == "2")
                        {
                            rubtemp.IvaInc = 0;
                        }
						
                        Decimal user_pais_Impuesto = DB.user_pais_Impuesto(lb_imp_exp.SelectedValue.ToString(), user.PaisID.ToString(), user.pais.Impuesto);                    
                        #region Contribuyente
                        if (rubtemp.IvaInc == 1)
                        {
                            #region Iva Incluido
                            if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                            {
                                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user_pais_Impuesto)), 2);
                                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                            {
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                                rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user_pais_Impuesto)), 2);
                                rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                            {
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                                rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user_pais_Impuesto)), 2);
                                rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                            {
                                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user_pais_Impuesto)), 2);
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
                                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user_pais_Impuesto, 2);
                                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                            {
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                                rubtemp.rubroImpuesto = Math.Round(totalD * (double)user_pais_Impuesto, 2);
                                rubtemp.rubroSubTot = Math.Round(totalD, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                            {
                                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                                rubtemp.rubroImpuesto = Math.Round(totalD * (double)user_pais_Impuesto, 2);
                                rubtemp.rubroSubTot = Math.Round(totalD, 2);
                                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            }
                            else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                            {
                                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user_pais_Impuesto, 2);
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
                    object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), tb1.Text, lb9.Text, lb10.Text };
                    dt.Rows.Add(obj);
                }

                
                gv_detalle.Columns[5].HeaderText = "Subtotal " + simbolomoneda;
                gv_detalle.Columns[6].HeaderText = "Impuesto " + simbolomoneda;
                gv_detalle.Columns[7].HeaderText = "Total " + simbolomoneda;
                gv_detalle.Columns[8].HeaderText = "Equivalente " + simboloequivalente;
                gv_detalle.DataSource = dt;
                gv_detalle.DataBind();
                #endregion
                Marcar_Cargos_Locales_Internacionales();
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
    protected void Validacion_Zona_Franca()
    {
        #region COBRO EN ZONA FRANCA
        if ((DB.Validar_Restriccion_Activa(user, 1, 34) == true))
        {
            if ((lb_imp_exp.SelectedValue == "1") || (lb_imp_exp.SelectedValue == "2"))
            {
                lb_contribuyente.SelectedValue = "1";
            }
            else
            {
                ArrayList clientearr = null;
                RE_GenericBean clienteBean = null;
                if (tbCliCod.Text != "0")
                {
                    clientearr = (ArrayList)DB.getClientes(" a.id_cliente=" + tbCliCod.Text, user, "");
                    clienteBean = (RE_GenericBean)clientearr[0];
                    if ((clientearr != null) && (clientearr.Count > 0))
                    {
                        int contaID = int.Parse(Session["Contabilidad"].ToString());
                        if (contaID == 2)
                        {
                            if (user.PaisID != 5)
                            {
                                lb_contribuyente.SelectedValue = "1";
                            }
                            else
                            {
                                lb_contribuyente.SelectedValue = (clienteBean.intC1).ToString();
                            }
                        }
                        else
                        {
                            lb_contribuyente.SelectedValue = (clienteBean.intC1).ToString();
                        }
                    }
                }
            }
            Actualizar_Cobro_Impuestos();
        }
        #endregion
    }


    protected void drp_tipopersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbCliCod.Text = "0";
        tb_nombre.Text = "";
        tb_nit.Text = "";
        tb_razon.Text = "";
        tb_giro.Text = "";
        tb_direccion.Text = "";
        drp_tipo_identificacion_cliente.SelectedValue = "0";
        Label3.Text = drp_tipopersona.SelectedItem.Text.ToUpper();
        if (drp_tipopersona.SelectedValue == "10")
        {
            btn_borrar_hbl.Visible = true;
        }
        else
        {
            btn_borrar_hbl.Visible = false;
        }
    }
    protected void btn_borrar_hbl_Click(object sender, ImageClickEventArgs e)
    {
        tb_hbl.Text = "";
    }
    private DataTable Nuevo_Lleno_DataTable(Hashtable ht, int requierealias, long cliID)
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
        dt.Columns.Add("CARGOID");
        dt.Columns.Add("LOCAL_INTERNACIONAL");
        if (ht != null && ht.Count > 0)
        {
            PublicHT = ht;
            ICollection valueColl = ht.Values;
            Rubros rubtemp = new Rubros();
            Rubros rubBackup = new Rubros();
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            string Tipo_Cargo = "";
            foreach (Rubros rub in valueColl)
            {
                int Tipo_Contribuyente = int.Parse(lb_contribuyente.SelectedValue);
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

                #region Traducir Tipo Cargo
                if (lbl_tipoOperacionID.Text == "12")
                {
                    //Si es PICKING se clasifica como Cargo Local dado que en WMS no existe la clasificacion de Local e Internacional
                    rubBackup.rubroTipoCargo = 1;
                }
                if (lbl_tipoOperacionID.Text == "19")
                {
                    //Si es Demora se clasifica como Cargo Local dado que en Sistema de Demoras no existe la clasificacion de Local e Internacional
                    rubBackup.rubroTipoCargo = 1;
                }
                if (rubBackup.rubroTipoCargo == 1)
                {
                    Tipo_Cargo = "LOCAL";
                }
                else
                {
                    Tipo_Cargo = "INTERNACIONAL";
                    if (user.PaisID == 1 && lb_imp_exp.SelectedValue == "2")
                    {
                        Tipo_Contribuyente = Tipo_Contribuyente;
                    }
                    else
                    {
                        Tipo_Contribuyente = 1;//Excento: Cambio aplicado para que los Cargos Internacionales no generen impuestos -- Pablo Aguilar::siempre y cuando no sea una exportacion en Guatemala
                    }
                }

                #endregion

                if (((rubtemp.CobIva == 1) && (Tipo_Contribuyente == 2) && (tipo_contabilidad == 1) && (rubBackup.rubtoType != "TERCEROS")) || ((rubtemp.CobIva == 1) && (Tipo_Contribuyente == 2) && (tipo_contabilidad == 2) && (user.PaisID == 5)))
                {
                    if ((user.PaisID == 1 || user.PaisID == 15) && lb_imp_exp.SelectedValue == "2" && rubtemp.rubroMoneda == 8)
                    {
                        rubtemp.IvaInc = 0;
                    }
					Decimal user_pais_Impuesto = DB.user_pais_Impuesto(lb_imp_exp.SelectedValue.ToString(), user.PaisID.ToString(), user.pais.Impuesto);                    
                    #region Contribuyente
                    if (rubtemp.IvaInc == 1)
                    {
                        #region Iva Incluido
                        if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user_pais_Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user_pais_Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user_pais_Impuesto)), 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user_pais_Impuesto)), 2);
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
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user_pais_Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD * (double)user_pais_Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                        {
                            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                            rubtemp.rubroImpuesto = Math.Round(totalD * (double)user_pais_Impuesto, 2);
                            rubtemp.rubroSubTot = Math.Round(totalD, 2);
                            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        }
                        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                        {
                            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user_pais_Impuesto, 2);
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

                //Nueva Configuracion solicitada el 27-11-2015 Ticket 2015112604000431 
                string _RO = "";
                string _IMPORTACION_EXPORTACION = "";
                string _MONEDA_TRANSACCION = "";
                string _MONEDA_CARGO = "";
                string _SERVICIO = "";
                #region Capturar Variables
                _RO = tb_routing.Text.Trim();
                _IMPORTACION_EXPORTACION = lb_imp_exp.SelectedItem.Text;
                _MONEDA_TRANSACCION = lb_moneda.SelectedItem.Text.Substring(0, 3);
                _MONEDA_CARGO = Utility.TraducirMonedaInt(Convert.ToInt32(rubtemp.rubroMoneda));
                #region Convertir ISO de Moneda Nicaragua
                if ((_MONEDA_CARGO == "C$") || (_MONEDA_CARGO == "NIO"))
                {
                    _MONEDA_CARGO = "NIC";
                }
                #endregion
                _SERVICIO = rubBackup.rubtoType.ToString();
                #endregion
                #region Routing de Aduanas y Seguros
                #endregion
                if (_SERVICIO != "TERCEROS")
                {
                    if (_IMPORTACION_EXPORTACION == "IMPORTACION")
                    {
                        #region Importacion
                        if (_RO != "")
                        {
                            #region Carga RO
                            if (_MONEDA_TRANSACCION != "USD")
                            {
                                #region Cargos en Quetzales
                                if (_MONEDA_TRANSACCION == _MONEDA_CARGO)
                                {
                                    if ((user.PaisID == 4) || (user.PaisID == 24))
                                    {
                                        if (Tipo_Cargo == "LOCAL")
                                        {
                                            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                            dt.Rows.Add(obj);
                                        }
                                    }
                                    else
                                    {
                                        if ((Tipo_Cargo == "LOCAL") || (Tipo_Cargo == "INTERNACIONAL"))
                                        {
                                            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                            dt.Rows.Add(obj);
                                        }
                                    }
                                }
                                #endregion
                            }
                            if (_MONEDA_TRANSACCION == "USD")
                            {
                                #region Cargos en USD
                                if (_MONEDA_TRANSACCION == _MONEDA_CARGO)
                                {
                                    if ((user.PaisID == 4) || (user.PaisID == 24))
                                    {
                                        if (Tipo_Cargo == "LOCAL")
                                        {
                                            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                            dt.Rows.Add(obj);
                                        }
                                    }
                                    else
                                    {
                                        if ((Tipo_Cargo == "LOCAL") || (Tipo_Cargo == "INTERNACIONAL"))
                                        {
                                            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                            dt.Rows.Add(obj);
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else if (_RO == "")
                        {
                            #region Carga FHC
                            if (_MONEDA_TRANSACCION != "USD")
                            {
                                #region Cargos en Quetzales
                                if (_MONEDA_TRANSACCION == _MONEDA_CARGO)
                                {
                                    if (Tipo_Cargo == "LOCAL")
                                    {
                                        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                        dt.Rows.Add(obj);
                                    }
                                    if (Tipo_Cargo == "INTERNACIONAL")
                                    {
                                        WebMsgBox.Show("El Rubro.: " + rubtemp.rubroName + " es un Cargo Internacional con moneda Local, por favor asigne moneda USD en Trafico.");
                                        return null;
                                    }
                                }
                                #endregion
                            }
                            if (_MONEDA_TRANSACCION == "USD")
                            {
                                #region Cargos en USD
                                if (_MONEDA_TRANSACCION == _MONEDA_CARGO)
                                {
                                    if ((user.PaisID == 4) || (user.PaisID == 24))
                                    {
                                        if (Tipo_Cargo == "LOCAL")
                                        {
                                            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                            dt.Rows.Add(obj);
                                        }
                                    }
                                    else
                                    {
                                        if ((Tipo_Cargo == "LOCAL") || (Tipo_Cargo == "INTERNACIONAL"))
                                        {
                                            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                            dt.Rows.Add(obj);
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else if (_IMPORTACION_EXPORTACION == "EXPORTACION")
                    {
                        #region Exportacion
                        if (_RO != "")
                        {
                            #region Carga RO
                            if ((user.PaisID != 5) && (user.PaisID != 21) && (user.PaisID != 4) && (user.PaisID != 24))//Se modifica xq Aimar Nicaragua fue creada como USD como si fuera SV o PA - 2016060704000289 
                            {
                                #region Todas las Empresas excepto Costa Rica y Nicaragua
                                if (_MONEDA_TRANSACCION != "USD")
                                {
                                    #region Cargos en Quetzales
                                    if ((Tipo_Cargo == "LOCAL") || (Tipo_Cargo == "INTERNACIONAL"))
                                    {
                                        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                        dt.Rows.Add(obj);
                                    }
                                    #endregion
                                }
                                if (_MONEDA_TRANSACCION == "USD")
                                {
                                    #region Cargos en USD

                                    WebMsgBox.Show("El Rubro.: " + rubtemp.rubroName + ", Moneda Transaccion:USD / Moneda Cargo:" + _MONEDA_CARGO + " / Tiene RO : no tiene configuracion");
                                    return null;

                                    //NO DEBIA EXISTIR CONFIGURACION
                                    //SE MODIFICA PARA COSTA RICA EL 29-04-2016 SEGUN CONFERENCIA SR HOLBIK, JOSE CRUZ CON PERSONAL DE COSTA RICA - 2016042004000571
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region Solo Empresas de Costa Rica y Nicaragua
                                if (_MONEDA_TRANSACCION != "USD")
                                {
                                    #region Cargos en Quetzales
                                    if (_MONEDA_TRANSACCION == _MONEDA_CARGO)
                                    {
                                        if ((user.PaisID == 4) || (user.PaisID == 24))
                                        {
                                            if (Tipo_Cargo == "LOCAL")
                                            {
                                                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                                dt.Rows.Add(obj);
                                            }
                                        }
                                        else
                                        {
                                            //Costa Rica
                                            if ((Tipo_Cargo == "LOCAL") || (Tipo_Cargo == "INTERNACIONAL"))
                                            {
                                                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                                dt.Rows.Add(obj);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                if (_MONEDA_TRANSACCION == "USD")
                                {
                                    #region Cargos en USD
                                    //NO DEBIA EXISTIR CONFIGURACION
                                    //SE MODIFICA EL 29-04-2016 SEGUN CONFERENCIA SR HOLBIK, JOSE CRUZ CON PERSONAL DE COSTA RICA - TICKET 2016042004000571
                                    if (_MONEDA_TRANSACCION == _MONEDA_CARGO)
                                    {
                                        if ((user.PaisID == 4) || (user.PaisID == 24))
                                        {
                                            if (Tipo_Cargo == "LOCAL")
                                            {
                                                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                                dt.Rows.Add(obj);
                                            }
                                        }
                                        else
                                        {
                                            if ((Tipo_Cargo == "LOCAL") || (Tipo_Cargo == "INTERNACIONAL"))
                                            {
                                                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                                dt.Rows.Add(obj);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else if (_RO == "")
                        {
                            #region Carga FHC
                            if (_MONEDA_TRANSACCION != "USD")
                            {
                                #region Cargos en Quetzales
                                if ((user.PaisID == 4) || (user.PaisID == 24))
                                {
                                    if (Tipo_Cargo == "LOCAL")
                                    {
                                        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                        dt.Rows.Add(obj);
                                    }
                                }
                                else
                                {
                                    if ((Tipo_Cargo == "LOCAL") || (Tipo_Cargo == "INTERNACIONAL"))
                                    {
                                        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                        dt.Rows.Add(obj);
                                    }
                                }
                                #endregion
                            }
                            if (_MONEDA_TRANSACCION == "USD")
                            {
                                #region Cargos en USD
                                //NO HAY CONFIGURACION - NO DEBE EXISTIR ESTE TIPO DE CARGO

                                object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroCommentario, rubBackup.rubroCargoID.ToString(), Tipo_Cargo };
                                dt.Rows.Add(obj);

                                WebMsgBox.Show("El Rubro.: " + rubtemp.rubroName + ", Moneda Transaccion:USD / Moneda Cargo:" + _MONEDA_CARGO + " / No Tiene RO : no tiene configuracion");
                                return null;

                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
        }
        return dt;
    }
    protected void Marcar_Cargos_Locales_Internacionales()
    {
        #region Marcar Cargos Locales e Internacionales
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
        GridViewRowCollection gvrc = gv_detalle.Rows;
        foreach (GridViewRow row in gvrc)
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
            lb10 = (Label)row.FindControl("lbl_local_internacional");
            if (lb10.Text == "INTERNACIONAL")
            {
                lb1.ForeColor = System.Drawing.Color.Navy;
                lb1.Font.Bold = true;
                lb2.ForeColor = System.Drawing.Color.Navy;
                lb2.Font.Bold = true;
                lb3.ForeColor = System.Drawing.Color.Navy;
                lb3.Font.Bold = true;
                lb4.ForeColor = System.Drawing.Color.Navy;
                lb4.Font.Bold = true;
                lb5.ForeColor = System.Drawing.Color.Navy;
                lb5.Font.Bold = true;
                lb6.ForeColor = System.Drawing.Color.Navy;
                lb6.Font.Bold = true;
                lb7.ForeColor = System.Drawing.Color.Navy;
                lb7.Font.Bold = true;
                lb8.ForeColor = System.Drawing.Color.Navy;
                lb8.Font.Bold = true;
                lb9.ForeColor = System.Drawing.Color.Navy;
                lb9.Font.Bold = true;
                lb10.ForeColor = System.Drawing.Color.Navy;
                lb10.Font.Bold = true;
            }
        }
        #endregion
    }
    protected void bt_impresion_invoice_Click(object sender, EventArgs e)
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
            string script = "window.open('../invoice/template_invoice_en.aspx?id=" + lb_facid.Text + "&transaccion=1','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void gv_poliza_seguros_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_poliza_seguros_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_poliza_seguros.DataSource = dt1;
        gv_poliza_seguros.PageIndex = e.NewPageIndex;
        gv_poliza_seguros.DataBind();
        modal_poliza_seguros.Show();
    }
    protected void gv_poliza_seguros_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_poliza_seguros.SelectedRow;
        if ((int.Parse(lbl_tipoOperacionID.Text) >= 1) && (int.Parse(lbl_tipoOperacionID.Text) <= 7))
        {
            if (tb_hbl.Text.Trim() != row.Cells[3].Text.Trim())
            {
                WebMsgBox.Show("El House de la Factura.: " + tb_hbl.Text + " no coincide con el House Asegurado.: " + row.Cells[3].Text + ", por favor asigne el Certificado de Seguro correcto.");
                return;
            }
        }
        else if (int.Parse(lbl_tipoOperacionID.Text) == 14)
        {
            if (tb_routing.Text.Trim() != row.Cells[4].Text.Trim())
            {
                WebMsgBox.Show("El Routing de la Factura.: " + tb_hbl.Text + " no coincide con el Routing Asegurado.: " + row.Cells[4].Text + ", por favor asigne el Certificado de Seguro correcto.");
                return;
            }
        }
        tb_poliza_seguros.Text = row.Cells[2].Text;
        lbl_poliza_seguro_id.Text = row.Cells[1].Text;
        ViewState["dt"] = null;
        gv_poliza_seguros.DataSource = null;
        gv_poliza_seguros.DataBind();
    }
    protected void btn_buscar_poliza_seguro_Click(object sender, EventArgs e)
    {
        string Tipo_Busqueda = "";
        string No_Documento = "";
        string Criterio = "";
        Tipo_Busqueda = drp_tipo_documento_asegurado.SelectedValue;
        No_Documento = tb_documento_asegurado.Text.Trim();
        if (Tipo_Busqueda == "1")
        {
            Criterio = " and no_routing ilike '%" + No_Documento + "%' ";
        }
        else if (Tipo_Busqueda == "2")
        {
            Criterio = " and no_bl ilike '%" + No_Documento + "%' ";
        }
        else if (Tipo_Busqueda == "3")
        {
            Criterio = " and poliza_seguro ilike '%" + No_Documento + "%' ";
        }
        ArrayList arr = (ArrayList)DB.Get_Certificado_Seguro(Criterio, user);
        DataTable dt = new DataTable("seguros");
        dt.Columns.Add("Id");
        dt.Columns.Add("Certificado");
        dt.Columns.Add("House");
        dt.Columns.Add("Routing");
        dt.Columns.Add("Contenedor");
        foreach (RE_GenericBean rgb in arr)
        {
            object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5 };
            dt.Rows.Add(objArr);
        }
        gv_poliza_seguros.DataSource = dt;
        gv_poliza_seguros.DataBind();
        ViewState["dt"] = dt;
        modal_poliza_seguros.Show();
    }

}