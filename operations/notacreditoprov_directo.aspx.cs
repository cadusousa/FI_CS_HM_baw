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

public partial class operations_notacreditoprov_directo : System.Web.UI.Page
{
    UsuarioBean user;
    DataTable dt1;
    public string simboloequivalente = "";
    public string simbolomoneda = "";
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
        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 16384) == 16384))
            Response.Redirect("index.aspx");
        
        
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad);

        if (!Page.IsPostBack)
        {
            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 4);
            if (tipo_contabilidad == 2)
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
                lb_moneda2.SelectedValue = moneda_inicial.ToString();
                lb_moneda.Enabled = false;
            }
            else
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
                lb_moneda2.SelectedValue = moneda_inicial.ToString();
                lb_moneda.Enabled = false;

            }
        }

        double cliID = 0;
        ArrayList clientearr = null; //el maximo es de 1 ya que solo 1 cliente debe haber
        RE_GenericBean clienteBean = null;//tendra los datos del cliente;
        string criterio = "";
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
                tb_nombre.Text = clienteBean.strC2;

                tb_direccion.Text = clienteBean.strC4;
            }
        }   
        cargo_datos_BL();
    }
    private void obtengo_listas(int tipo_contabilidad)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='notacreditoprov.aspx'");
            lb_trans.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            lb_trans.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                if (!rgb.intC1.ToString().Equals("57")) { 
                   item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                   lb_trans.Items.Add(item);
                }                
            }
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
                lb_moneda2.Items.Add(item);
            }
            //arr = null;
            //arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 12, user, 1);//4 porque es el tipo de documento para nota debito segun sys_tipo_referencia
            //foreach (string valor in arr)
            //{
            //    item = new ListItem(valor, valor);
            //    lb_serie_factura.Items.Add(item);
            //}
            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(12, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serie_factura.Items.Clear();
            lb_serie_factura.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serie_factura.Items.Add(item);
            }

            arr = null;
            if (DB.Validar_Restriccion_Activa(user, 12, 24) == true)
            {
                arr = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(user.PaisID, user.contaID, 12, user.SucursalID);
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
            if (lb_servicio.Items.Count > 0)
            {
                lb_servicio.SelectedIndex = 1;
                ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), "");
                RE_GenericBean rubbean = null;
                for (int a = 0; a < rubros.Count; a++)
                {
                    rubbean = (RE_GenericBean)rubros[a];
                    item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
                    lb_rubro.Items.Add(item);
                }
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
            }
        }
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
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text };
            dt.Rows.Add(objArr);
        }


        //halo el rubro y luego le calculo sus chingaderas
        Rubros rubro = new Rubros();
        rubro.rubroID = long.Parse(lb_rubro.SelectedValue);
        rubro.rubroName = lb_rubro.SelectedItem.Text;
        int cliID = int.Parse(tbCliCod.Text);
        rubro.rubroMoneda = long.Parse(lb_moneda2.SelectedValue);

        //DEFINIR TIPO DE SERVICIO
        #region Backup
        //if (lb_servicio.SelectedItem.Text.Equals("FCL")) { rubro.rubtoType = "FCL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("LCL")) { rubro.rubtoType = "LCL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("AEREO")) { rubro.rubtoType = "AEREO"; }
        //if (lb_servicio.SelectedItem.Text.Equals("APL")) { rubro.rubtoType = "APL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("TRANSPORTE T")) { rubro.rubtoType = "TRANSPORTE T"; }
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
        //object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
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
        #region Backup
        //DataTable dt = new DataTable();
        //dt.Columns.Add("ID");
        //dt.Columns.Add("NOMBRE");
        //dt.Columns.Add("TYPE");
        //dt.Columns.Add("TOTAL");
        //dt.Columns.Add("TOTALD");
        //dt.Clear();
        //Label lb1, lb2, lb3, lb4, lb5, lb6, lb7;
        //foreach (GridViewRow row in gv_detalle.Rows)
        //{
        //    lb1 = (Label)row.FindControl("lb_codigo");
        //    lb2 = (Label)row.FindControl("lb_rubro");
        //    lb3 = (Label)row.FindControl("lb_tipo");
        //    lb6 = (Label)row.FindControl("lb_total");
        //    //
        //    lb7 = (Label)row.FindControl("lb_totaldolares");
        //    object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb6.Text, lb7.Text };
        //    dt.Rows.Add(objArr);
        //}


        ////halo el rubro y luego le calculo sus chingaderas
        //Rubros rubro = new Rubros();
        //rubro.rubroID = long.Parse(lb_rubro.SelectedValue);
        //rubro.rubroID = long.Parse(lb_rubro.SelectedValue);
        //rubro.rubroName = lb_rubro.SelectedItem.Text;
        //rubro.rubroMoneda = long.Parse(lb_moneda2.SelectedValue);
        //if (lb_servicio.SelectedItem.Text.Equals("FCL")) { rubro.rubtoType = "FCL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("LCL")) { rubro.rubtoType = "LCL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("AEREO")) { rubro.rubtoType = "AEREO"; }
        //if (lb_servicio.SelectedItem.Text.Equals("APL")) { rubro.rubtoType = "APL"; }
        //if (lb_servicio.SelectedItem.Text.Equals("TRANSPORTE T")) { rubro.rubtoType = "TRANSPORTE T"; }
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
        //rubro.rubroTot = double.Parse(tb_monto.Text);
        //Rubros rubtemp = (Rubros)rubro;
        //rubtemp = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
        //if (rubtemp == null)
        //{
        //    WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
        //    return;
        //}
        //if ((rubtemp.CobIva == 1) && (user.contaID != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))//si debe cobrar iva y el rubro no esta en dolares y no es excento
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
        //    rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
        //}
        //else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
        //{
        //    rubtemp.rubroTot = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
        //}
        //else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
        //{
        //    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
        //}
        ////object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), /*rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), */rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)") };
        /////
        //object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), /*simbolomoneda, rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), */ rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
        //dt.Rows.Add(obj);
        //if (user.contaID == 2)
        //{
        //    gv_detalle.Columns[4].HeaderText = "Total en Dolares";
        //    gv_detalle.Columns[5].HeaderText = "Equivalente Local";
        //}
        //else
        //{
        //    gv_detalle.Columns[4].HeaderText = "Total local";
        //    gv_detalle.Columns[5].HeaderText = "Equivalente en Dolares";
        //}
        //gv_detalle.DataSource = dt;
        //gv_detalle.DataBind();
        #endregion
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
        //Label2.Text = "Equivalente en " + simboloequivalente;
        //Label6.Text = "Sub total en " + simbolomoneda;
        //Label7.Text = "Impuesto en " + simbolomoneda;
        //Label8.Text = "Total en " + simbolomoneda;
        tb_subtotal.Text = subtotal.ToString("#,#.00#;(#,#.00#)");
        tb_impuesto.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
        tb_totaldolares.Text = totalD.ToString("#,#.00#;(#,#.00#)");
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        int transID = int.Parse(lb_trans.SelectedValue);//tipo de transaccion factura, invoice

        if (transID <= 0)
        {
            WebMsgBox.Show("Debe seleccionar una Transaccion");
            return;
        }
        
        if (lb_serie_factura.Items.Count <= 0)
        {
            WebMsgBox.Show("No se puede Guardar una Nota de Credito sin Serie");
            return;
        }
        if (lb_serie_factura.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Serie a utilizar");
            return;
        }
        int contribuyente = int.Parse(lb_contribuyente.SelectedValue);
        int moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        int tipopersona = int.Parse(lb_tipopersona.SelectedValue);//tipo de persona
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue);
        int tipo_cobro = 1;//prepaid, collect
        if (imp_exp == 1) { tipo_cobro = 1; } else { tipo_cobro = 2; } //si es 1=importacion cobro 1=collect,si es 2=exportacion cobro 2=prepaid
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());//fiscal o financiera
        if ((tipopersona == 4) || (tipopersona == 8)) { transID = 8; } else { transID = 15; }//si tipopersona=4 (proveedores) provision proveedor sino provision agente
        int servicio = 0; //fcl, lcl, etc

        if ((tbCliCod.Text.Trim().Equals("") || (tbCliCod.Text.Trim().Equals("0"))))
        {
            WebMsgBox.Show("Es necesario que indique un proveedor");
            return;
        }
        if ((gv_detalle.Rows.Count == 0) || (tb_total.Text.Trim().Equals("")))
        {
            WebMsgBox.Show("Debe ingresar al menos un rubro");
            return;
        }
        if (tb_fecha_libro_compras.Text.Trim().Equals("") && (bool.Parse(Rb_Documento.SelectedValue) == true))
        {
            WebMsgBox.Show("Debe seleccionar la Fecha del Libro de Compras");
            return;
        }
        decimal total = 0, saldo = 0, abono = 0;
        total = decimal.Parse(tb_total.Text);

        
        user = (UsuarioBean)Session["usuario"];
        ////Pendiente el tema de si es menor de 2 meses
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.Fecha_Hora = lb_fecha_hora.Text;
        rgb.intC1 = int.Parse(lb_trans.SelectedValue);//tipo transaccion
        rgb.intC2 = moneda;//tipo moneda
        rgb.intC3 = tipo_contabilidad;//tipo de contabilidad
        rgb.intC4 = 0;
        rgb.intC5 = 12;//sys tipo de transaccion
        rgb.intC6 = tipopersona;//tipo de persona
        rgb.intC7 = int.Parse(tbCliCod.Text);
        rgb.decC1 = decimal.Parse(tb_total.Text.Trim());//monto a aplicar
        rgb.strC1 = tb_observaciones.Text;//observaciones
        rgb.strC2 = lb_serie_factura.SelectedItem.Text;
        rgb.strC3 = tb_referencia.Text; //Referencia
        rgb.decC2 = decimal.Parse(tb_subtotal.Text);
        rgb.decC3 = decimal.Parse(tb_impuesto.Text);
        rgb.strC9 = tb_hbl.Text;
        rgb.strC10 = tb_mbl.Text;
        rgb.strC11 = tb_contenedor.Text;
        rgb.strC12 = tb_routing.Text;
        rgb.Fiscal_No_Fiscal = bool.Parse(Rb_Documento.SelectedValue);
        rgb.Nombre_Cliente = tb_nombre.Text;//Nombre del Cliente
        rgb.strC14 = tb_nit.Text;
        rgb.Blid = int.Parse(lbl_blID.Text);
        rgb.Tipo_Operacion = int.Parse(lbl_tipoOperacionID.Text);
        rgb.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string fecha_libro_compras = tb_fecha_libro_compras.Text;
        ArrayList Arr_Fechas = (ArrayList)DB.Formatear_Fechas(fecha_libro_compras, "");
        fecha_libro_compras = Arr_Fechas[0].ToString();
        rgb.strC13 = fecha_libro_compras;
        rgb.strC14 = tb_nit.Text;
        if (tb_documento_serie.Text != "") rgb.strC15 = tb_documento_serie.Text.Trim();
        if (tb_documento_correlativo.Text != "")  rgb.intC8 = int.Parse(tb_documento_correlativo.Text.Trim());
        if (tb_fechadoc.Text != "")
        {
            //rgb.strC16 = tb_fechadoc.Text.Trim();
            string fecha_proveeor = tb_fechadoc.Text.Trim();
            ArrayList Arr_fprove = (ArrayList)DB.Formatear_Fechas(fecha_proveeor, "");
            rgb.strC16 = Arr_fprove[0].ToString();
        }
        else
        {
            rgb.strC16 = null;
        }
        if (lb_moneda.SelectedValue.Equals("8"))
        {
            rgb.decC4 = Math.Round((rgb.decC1 * user.pais.TipoCambio), 2);
        }
        else
        {
            rgb.decC4 = Math.Round((rgb.decC1 / user.pais.TipoCambio), 2);
        }

        bool valida_agente = Validar_Restricciones("Validar_Econocaribe");
        if (valida_agente == true)
        {
            return;
        }

        MatOpBean mat = new MatOpBean();
        mat.paiID = user.PaisID;
        mat.tranID = int.Parse(lb_trans.Text);
        mat.monID = int.Parse(lb_moneda.SelectedValue);
        mat.contaID = rgb.intC3;
        mat.impexpID = int.Parse(lb_imp_exp.SelectedValue);
        mat.cobroID = tipo_cobro;
        mat.contriID = int.Parse(lb_contribuyente.SelectedValue);

        int matOpID = DB.getMatrizOperacionID(int.Parse(lb_trans.Text), mat.monID, user.PaisID, mat.contaID);
        ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion(matOpID);

        ////recorro el datagrid para aramar la factura
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        Rubros rubro;
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
            rubro.rubroName = lb2.Text.Trim();
            rubro.rubtoType = lb3.Text;
            rubro.rubroSubTot = double.Parse(lb4.Text);
            rubro.rubroImpuesto = double.Parse(lb5.Text);
            rubro.rubroTot = double.Parse(lb6.Text);
            rubro.rubroTotD = double.Parse(lb7.Text);
            rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
            rubro.rubroTypeID = Utility.TraducirServiciotoINT(lb3.Text);
            servicio = rubro.rubroTypeID;
            rubro.cta_haber = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
           // rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("haber", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            if (( rubro.cta_haber == null) || ( rubro.cta_haber.Count == 0))
            {
                WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de cobro que esta realizando, por favor pongase en contacto con el Contador");
                return;
            }
            if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
            rgb.arr1.Add(rubro);
        }

        string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 12); //3=12=Nota Credito
        if (Check_Existencia == "0")
        {

            ArrayList result = DB.InsertNotaCredito_proveedor_directo(rgb, user, tipo_contabilidad, ctas_cargo);//4 ya que en la tabla tbl_tipo_persona 4=proveedor
            if (result != null)
            {
                bt_imprimir.Enabled = true;
                lb_facid.Text = result[0].ToString();
                tb_corr.Text = result[1].ToString();
                WebMsgBox.Show("La Nota de Credito fue grabada éxitosamente con Serie.: " + lb_serie_factura.SelectedItem.Text + " y Correlativo.: " + result[1].ToString());
                bt_Enviar.Enabled = false;

                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 12, 0);
                gv_detalle_partida.DataBind();
                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                return;
            }
            else
            {
                WebMsgBox.Show("Existio un error al tratar de guardar la Nota de Credito, porfavor intente de nuevo");
                return;
            }
        }
        else
        {
            bt_Enviar.Enabled = false;
            return;
        }
    }
    protected void bt_imprimir_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('print_notadebito.aspx?fac_id=" + lb_facid.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }

    }
    protected void lb_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        int servID = int.Parse(lb_servicio.SelectedValue);
        int tipomoneda = int.Parse(lb_moneda.SelectedValue);
        ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, servID, "");
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
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text };
            dt.Rows.Add(objArr);
        }
        dt.Rows[e.RowIndex].Delete();
        gv_detalle.Columns[5].HeaderText = "Subtotal " + simbolomoneda;
        gv_detalle.Columns[6].HeaderText = "Impuesto " + simbolomoneda;
        gv_detalle.Columns[7].HeaderText = "Total " + simbolomoneda;
        gv_detalle.Columns[8].HeaderText = "Equivalente " + simboloequivalente;
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }
    protected void gv_clientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_clientes.SelectedRow;
        if (lb_tipopersona.SelectedValue.Equals("4")) //proveedor
        {
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nit.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            //tb_contacto.Text = "";
            //tb_contacto.Visible = false;
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[8].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            if ((DB.Validar_Restriccion_Activa(user, 12, 29) == true))
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
                    if (Page.Server.HtmlDecode(row.Cells[7].Text) == "NO NEUTRAL")
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
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            //tb_contacto.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[4].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            //tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            //tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        {
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_nit.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
        }

        lb_contribuyente.SelectedValue = DB.getProveedorRegimen(int.Parse(lb_tipopersona.SelectedValue), tbCliCod.Text.ToString()).ToString();
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
    protected void Button4_Click(object sender, EventArgs e)
    {
        DataTable dt = null;
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        if (lb_tipopersona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
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
            Arr = (ArrayList)DB.getAgente(where, ""); //Agente
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
            Arr = (ArrayList)DB.getNavieras(where, ""); //Naviera
            dt = (DataTable)Utility.fillGridView("Naviera", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))
        {
            //Arr = (ArrayList)DB.getAgente(where); //Lineas aereas
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una línea aerea");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(name)) ilike '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and carrier_id=" + tb_codigo.Text; else where += " and carrier_id=" + tb_codigo.Text;

            Arr = (ArrayList)DB.getCarriers(where); //Lineas aereas
            dt = (DataTable)Utility.fillGridView("LineasAereas", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(nombre_comercial)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_intercompany=" + tb_codigo.Text; else where += " and id_intercompany=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += " and nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.Get_Intercompanys(where);//Intercompany
            dt = (DataTable)Utility.fillGridView("Intercompany", Arr);
        }

        gv_clientes.DataSource = dt;
        gv_clientes.DataBind();
        ViewState["dt"] = dt;
        modalcliente.Show();
    }


    protected void bt_search_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;

        if (lb_tipo.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio.Text.Trim(), user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio.Text.Trim(), user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio.Text.Trim(), user.pais.schema, codDistribucion, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio.Text.Trim(), user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("TERRESTRE T"))
        {
            //string paisISO = Utility.InttoISOPais(user.PaisID);
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio.Text.Trim(), user.pais.ISO, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        //pnlhbl_ModalPopupExtender.Show();
    }
    protected void dgw1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tipo = lb_tipo.SelectedValue;
        if (e.CommandName == "Seleccionar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (tipo.Equals("FCL") || tipo.Equals("LCL"))
            {
                tb_hbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[2].Text);
                tb_mbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[3].Text);
                tb_routing.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[4].Text);
                tb_contenedor.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[5].Text);
            }
            else if (tipo.Equals("PICKING"))
            {
                tb_hbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[2].Text);
                tb_mbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[3].Text);
            }
            else if (tipo.Equals("AEREO"))
            {
                tb_hbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[2].Text);
                tb_mbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[3].Text);
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                tb_hbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[2].Text);
                tb_mbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[3].Text);
            }
        }
    }
    protected void dgw1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw1.DataSource = dt1;
        dgw1.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        dgw1.DataBind();
        //pnlhbl_ModalPopupExtender.Show();
    }
    protected void bt_search2_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;

        if (lb_tipo2.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio2.Text.Trim(), user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio2.Text.Trim(), user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio2.Text.Trim(), user.pais.schema, codDistribucion, user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio2.Text.Trim(), user.pais.schema);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("TERRESTRE T"))
        {
            string paisISO = Utility.InttoISOPais(user.PaisID);
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio2.Text.Trim(), paisISO, user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        //tb_cuenta2_ModalPopupExtender.Show();
    }
    protected void dgw12_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tipo = lb_tipo2.SelectedValue;
        if (e.CommandName == "Seleccionar2")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (tipo.Equals("FCL") || tipo.Equals("LCL"))
            {
                tb_mbl.Text = Server.HtmlDecode(dgw12.Rows[index].Cells[3].Text.Trim());
                tb_hbl.Text = Server.HtmlDecode(dgw12.Rows[index].Cells[2].Text.Trim());
                tb_routing.Text = Server.HtmlDecode(dgw12.Rows[index].Cells[4].Text.Trim());
                tb_contenedor.Text = Server.HtmlDecode(dgw12.Rows[index].Cells[5].Text.Trim());
            }
            else if (tipo.Equals("PICKING"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("AEREO"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
        }
    }
    protected void dgw12_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw12.DataSource = dt1;
        dgw12.PageIndex = e.NewPageIndex;
        dgw12.DataBind();
        //tb_cuenta2_ModalPopupExtender.Show();
    }
    
    protected void lb_trans_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!lb_trans.SelectedItem.Text.Equals("Seleccione..."))
        {   
            int tranID = int.Parse(lb_trans.SelectedValue);
            if (tranID == 54) { lb_tipopersona.SelectedValue = "2"; }// agentes
            else if (tranID == 57) { lb_tipopersona.SelectedValue = "8"; }// caja chica
            else if (tranID == 58) { lb_tipopersona.SelectedValue = "2"; }// agente
            else if (tranID == 56) { lb_tipopersona.SelectedValue = "6"; }// linea aerea
            else if (tranID == 25) { lb_tipopersona.SelectedValue = "4"; }// proveedores
            else if (tranID == 55) { lb_tipopersona.SelectedValue = "5"; }// naviera
            else if (tranID == 52) { lb_tipopersona.SelectedValue = "6"; }// Linea Aerea
            else if (tranID == 107) { lb_tipopersona.SelectedValue = "10"; }//Intercompanys
        }
        #region Limpiar Tipo de Persona
        tbCliCod.Text = "";
        tb_nombre.Text = "";
        tb_nit.Text = "";
        tb_direccion.Text = "";
        tb_telefono.Text = "";
        tb_correoelectronico.Text = "";
        #endregion
    }
    protected void gv_detalle_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }
    protected bool Validar_Restricciones(string sender)
    {
        bool resultado = false;
        bool paiR = DB.Validar_Pais_Restringido(user);
        if (paiR == true)
        {
            #region NO EMITIR PAGO MANUAL AGENTE ECONO
            if ((DB.Validar_Restriccion_Activa(user, 12, 26) == true) && (sender == "Validar_Econocaribe"))
            {
                resultado = false;
                if (lb_tipopersona.SelectedValue == "2")
                {
                    if (user.pais.Grupo_Empresas == 2)
                    {
                        int res = DB.Validar_Persona_Grupo_Econocaribe(user, int.Parse(tbCliCod.Text), 2);
                        if (res > 0)
                        {
                            WebMsgBox.Show("El Agente Econocaribe no puede ser utilizado en modulo.: " + user.pais.Nombre_Sistema + ", debe utilizar el Modulo de Aimar");
                            resultado = true;
                        }
                    }
                }
                return resultado;
            }
            #endregion
        }
        return resultado;
    }
    private void cargo_datos_BL()
    {
        #region Cargo Datos BL
        if ((Request.QueryString["bl_no"] != null) && (Request.QueryString["tipo"] != null))
        {
            string bl_no = Request.QueryString["bl_no"].ToString().Trim();
            string tipo = Request.QueryString["tipo"].ToString().Trim();
            string blID = Request.QueryString["blid"].ToString();
            string opID = Request.QueryString["opid"].ToString();
            string idRouting = "-1";
            if (tipo.Equals("LCL"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataLCL(bl_no, user, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = rgb.lonC10.ToString();
                tb_routing.Text = DB.getRouting(rgb.lonC10);
                lb_imp_exp.SelectedValue = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("FCL"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataFCL(bl_no, user, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = rgb.lonC10.ToString();
                tb_routing.Text = DB.getRouting(rgb.lonC10);
                lb_imp_exp.SelectedValue = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                double QS_BLID = 0;
                if (Request.QueryString["blid"] != null)
                {
                    QS_BLID = double.Parse(Request.QueryString["blid"].ToString());
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getDataTerrestre_Costos(QS_BLID, user,0);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                idRouting = DB.GetIDRoutingbyNumero(rgb.strC7);
                tb_routing.Text = rgb.strC7;
                tb_contenedor.Text = rgb.strC4;
            }
            else if (tipo.Equals("AEREO"))
            {
                int Tipo_Guia = 0;
                if (Request.QueryString["opid"] != null)
                {
                    Tipo_Guia = int.Parse(Request.QueryString["opid"].ToString());
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataAereo(bl_no, 0, Tipo_Guia, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = rgb.lonC8.ToString();
                tb_routing.Text = rgb.strC7;
                lb_imp_exp.SelectedValue = rgb.strC10;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("RO ADUANAS"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceRO_Aduanas(user, bl_no, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = lbl_blID.Text;
                tb_routing.Text = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("RO SEGUROS"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceRO_Seguros(user, bl_no, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = lbl_blID.Text;
                tb_routing.Text = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
        }
        Definir_Tipo_Operacion();
        #endregion
    }
    protected void Definir_Tipo_Operacion()
    {
        #region Definir Tipo Operacion
        if (lbl_operacion.Text != "0")
        {
            ArrayList Arr_Sistemas = DB.Get_Detalle_Sistemas(" and b.tto_id=" + lbl_tipoOperacionID.Text + "");
            foreach (RE_GenericBean Bean in Arr_Sistemas)
            {
                lbl_sistema.Text = Bean.strC1;
                lbl_operacion.Text = Bean.strC2;
            }
            pnl_operacion.Visible = true;
        }
        #endregion
    }
    protected void lb_serie_factura_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie_factura.Items.Count > 0)
        {
            if (lb_serie_factura.SelectedValue != "0")
            {
                int moneda_Serie = 0;
                moneda_Serie = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                lb_moneda.SelectedValue = moneda_Serie.ToString();//Moneda de la Transaccion
                lb_moneda2.SelectedValue = moneda_Serie.ToString();//Moneda del Rubro
                lb_moneda.Enabled = false;
                int impo_expo = 0;
                if (lb_imp_exp.Items.Count > 0)
                {
                    impo_expo = int.Parse(lb_imp_exp.SelectedValue);
                    Actualizar_Moneda_Rubros();
                }
            }
        }
    }
    protected void Actualizar_Moneda_Rubros()
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

            Rubros rubro = new Rubros();
            rubro.rubroID = long.Parse(lb1.Text);
            rubro.rubroName = lb2.Text;
            int cliID = int.Parse(tbCliCod.Text);
            rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
            rubro.rubtoType = lb3.Text;
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
            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
            dt.Rows.Add(obj);

        }

        gv_detalle.Columns[5].HeaderText = "Subtotal " + simbolomoneda;
        gv_detalle.Columns[6].HeaderText = "Impuesto " + simbolomoneda;
        gv_detalle.Columns[7].HeaderText = "Total " + simbolomoneda;
        gv_detalle.Columns[8].HeaderText = "Equivalente " + simboloequivalente;
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }
}
