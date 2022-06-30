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

public partial class operations_provisionaBL : System.Web.UI.Page
{    
    UsuarioBean user = null;
    int Impuesto_Proveedor = 0;
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
        if (!((permiso & 4096) == 4096))
            Response.Redirect("index.aspx");

        if (!Page.IsPostBack) {
            ListItem item= null;
            ArrayList arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_imp_exp.Items.Add(item);
            }
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            if (tipo_contabilidad == 2)
            {
                #region Backup
                //lb_moneda.SelectedValue = "8";
                #endregion
                lb_moneda.SelectedValue = user.Moneda.ToString();
                lb_moneda.Enabled = false;
            }
            else
            {
                #region Backup
                //lb_moneda.SelectedValue = user.pais.ID.ToString();
                #endregion
                lb_moneda.SelectedValue = user.Moneda.ToString();
                lb_moneda.Enabled = false;
            }
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 5, user,1);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
            }

        }
    }
    
    protected void bt_search_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;

        if (lb_tipo.SelectedValue.Equals("LCL"))
        {
            DataSet ds = (DataSet)DB.getBL_LCL_byMBL(tb_criterio.Text, user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("FCL"))
        {
            DataSet ds = (DataSet)DB.getBL_FCL_byMBL(tb_criterio.Text, user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = (DataSet)DB.getBL_AEREO_byMBL(tb_criterio.Text, user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("TERRESTRE T"))
        {
            DataSet ds = (DataSet)DB.getBL_TERRESTRE_byMBL(tb_criterio.Text, user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }

        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void dgw1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Seleccionar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            tb_mbl.Text = dgw1.Rows[index].Cells[2].Text;
            tb_mbl_id.Text = dgw1.Rows[index].Cells[1].Text;
            tb_tipo.Text = dgw1.Rows[index].Cells[3].Text;
            obtengo_cargos_BL(int.Parse(tb_mbl_id.Text), tb_tipo.Text);

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
        tb_cuenta_ModalPopupExtender.Show();
    }

    private void obtengo_cargos_BL(int blID, string tipo) {
        DataTable dt_detalle = null;
        if ((tipo.Equals("F")) || (tipo.Equals("L")))
            dt_detalle = (DataTable)DB.getRubrosProvisionarconNombre(blID, tipo, user.pais.schema);
        else if (tipo.Equals("TERRESTRE"))
            dt_detalle = (DataTable)DB.getRubrosProvisionarconNombre_Terrestre(blID, tipo, user.pais.schema);
        else if (tipo.Equals("AEREO IMPORTACION"))
            dt_detalle = (DataTable)DB.getRubrosProvisionarconNombre_Aereo(blID, tipo, user.pais.schema, 2);
        else if (tipo.Equals("AEREO EXPORTACION"))
            dt_detalle = (DataTable)DB.getRubrosProvisionarconNombre_Aereo(blID, tipo, user.pais.schema, 1);

        gv_detalle_costos.DataSource = dt_detalle;
        gv_detalle_costos.DataBind();
    }
    protected void bt_provisionar_Click(object sender, EventArgs e)
    {
        if (gv_detalle_costos.Rows.Count > 0)
        {
            try
            {
                DataTable dt = null;
                Hashtable ht = new Hashtable();
                Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
                foreach (GridViewRow row in gv_detalle_costos.Rows)
                {
                    lb1 = (Label)row.FindControl("lb_codigo");
                    lb2 = (Label)row.FindControl("lb_rubro_id");
                    lb3 = (Label)row.FindControl("lb_rubro");
                    lb9 = (Label)row.FindControl("lb_rubrotype");
                    lb4 = (Label)row.FindControl("lb_monedatipo");
                    lb5 = (Label)row.FindControl("lb_total");
                    lb6 = (Label)row.FindControl("lb_proveedortipo");
                    lb7 = (Label)row.FindControl("lb_proveedorID");
                    lb8 = (Label)row.FindControl("lb_proveedor");
                    lb10 = (Label)row.FindControl("lb_referencia");

                    if (!ht.ContainsKey(lb7.Text + "-" + lb10.Text))
                    {
                        dt = new DataTable();
                        dt.Columns.Add("ID");
                        dt.Columns.Add("Rubro ID");
                        dt.Columns.Add("Rubro");
                        dt.Columns.Add("Tipo servicio");
                        dt.Columns.Add("Moneda");
                        dt.Columns.Add("Valor");
                        dt.Columns.Add("Tipo Proveedor");
                        dt.Columns.Add("Proveedor ID");
                        dt.Columns.Add("Proveedor");
                        dt.Columns.Add("Referencia");
                        object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb9.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, lb8.Text, lb10.Text};
                        dt.Rows.Add(objArr);
                        ht.Add(lb7.Text + "-" + lb10.Text, dt);
                    }
                    else
                    {
                        dt = (DataTable)ht[lb7.Text + "-" + lb10.Text];
                        object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb9.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, lb8.Text, lb10.Text };
                        dt.Rows.Add(objArr);
                        ht.Remove(lb7.Text + "-" + lb10.Text);
                        ht.Add(lb7.Text + "-" + lb10.Text, dt);
                    }
                }
                string serv = tb_tipo.Text;
                string Nombre_Cliente="";
                RE_GenericBean rgb = null;


                int id = 0, rubroID = 0, moneda = 0, tipoproveedor = 0, provID = 0, transaccion = 0, servicio = 0;
                double valor = 0, total = 0, equivalente = 0, impuesto = 0, afecto = 0, noafecto = 0;
                DataRow dtrow = null;
                Rubros rubbean = null;
                Rubros rub = null;
                string Check_Existencia = DB.CheckExistDoc(lb_fecha_hora.Text, 3); //3=12=Nota Credito
                if (Check_Existencia == "0")
                {
                    foreach (string key in ht.Keys)
                    {
                        rgb = new RE_GenericBean();
                        valor = 0; total = 0; equivalente = 0; impuesto = 0; afecto = 0; noafecto = 0;
                        id = 0; rubroID = 0; moneda = 0; tipoproveedor = 0; provID = 0; transaccion = 0; servicio = 0;
                        rgb.strC2 = DateTime.Today.ToShortDateString();//fecha
                        rgb.strC3 = DateTime.Today.ToShortDateString();//fechamax
                        //rgb.strC2 = DateTime.Parse(rgb.strC2).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        //rgb.strC3 = DateTime.Parse(rgb.strC3).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        rgb.intC6 = int.Parse(lb_imp_exp.SelectedValue);// importacion=1, exportacion=2
                        rgb.decC3 = 0;//noafecto
                        rgb.decC4 = 0;//iva
                        rgb.strC4 = "";//observaciones
                        rgb.intC1 = 0;//depto
                        rgb.strC10 = "";//serieoc
                        rgb.intC5 = 0;//correlativooc
                        rgb.strC16 = tb_mbl.Text;//mbl
                        transaccion = 5;
                        rgb.intC4 = int.Parse(lb_moneda.SelectedValue);
                        //************************ codigo para obtener la serie de las contraseñas de facturas para esta sucursal
                        string serie = "GEN";
                        ArrayList arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, transaccion, user, 1);//1 porque es el tipo de documento para ordenes de compra
                        if (arr != null && arr.Count > 0) serie = arr[0].ToString();
                        rgb.strC20 = serie;//serie
                        dt = (DataTable)ht[key];
                        string sservicio = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dtrow = (DataRow)dt.Rows[i];
                            rubroID = int.Parse(dtrow[1].ToString());
                            valor = double.Parse(dtrow[5].ToString());
                            id = int.Parse(dtrow[0].ToString());
                            transaccion = 5;//5=provision segun sys_tipo_referencia
                            Nombre_Cliente = dtrow[8].ToString();
                            rgb.strC1 = "";//facturaID
                            rgb.strC31 = dtrow[9].ToString();                        
                            sservicio = dtrow[3].ToString();
                            servicio = Utility.TraducirServiciotoINT(sservicio);
                            if (dtrow[4].ToString().Equals("USD")) moneda = 8;
                            else if (dtrow[4].ToString().Equals("GTQ")) moneda = 1;
                            else if (dtrow[4].ToString().Equals("SVC")) moneda = 2;
                            else if (dtrow[4].ToString().Equals("HNL")) moneda = 3;
                            else if (dtrow[4].ToString().Equals("NIC")) moneda = 4;
                            else if (dtrow[4].ToString().Equals("CRC")) moneda = 5;
                            else if (dtrow[4].ToString().Equals("PAB")) moneda = 6;
                            else if (dtrow[4].ToString().Equals("BZD")) moneda = 7;
                            if (rgb.intC4 != 8)
                            {
                                if (moneda != 8)
                                {
                                    valor = Math.Round(valor * 1, 2);
                                }
                                else
                                {
                                    valor = Math.Round((double)valor * (double)user.pais.TipoCambio, 2);
                                }
                            }
                            else
                            {
                                if (moneda == 8)
                                {
                                    valor = Math.Round(valor * 1, 2);
                                }
                                else
                                {
                                    valor = Math.Round(((double)valor / (double)user.pais.TipoCambio), 2);
                                }
                            }

                            if (rgb.intC4 != 8)
                                equivalente = Math.Round((double)valor / (double)user.pais.TipoCambio, 2);
                            else
                                equivalente = Math.Round((double)valor * (double)user.pais.TipoCambio, 2);

                            if (dtrow[6].ToString().Equals("Linea Aerea")) tipoproveedor = 6; //tradusco segun tabla tbl_tipo_persona
                            else if (dtrow[6].ToString().Equals("Agente")) tipoproveedor = 2;//tradusco segun tabla tbl_tipo_persona
                            else if (dtrow[6].ToString().Equals("Naviera")) tipoproveedor = 5;//tradusco segun tabla tbl_tipo_persona
                            else if (dtrow[6].ToString().Equals("Proveedor")) tipoproveedor = 4;//tradusco segun tabla tbl_tipo_persona
                            //Verificar si proveedor es exento
                            int ImpuestoProveedor = DB.getProveedorRegimen(tipoproveedor, dtrow[7].ToString());
                            Impuesto_Proveedor = ImpuestoProveedor;
                            rubbean = new Rubros();
                            
                            #region Calcular Subtotal, Impuesto de Rubro por Pais
                            rub = new Rubros();
                            rub.rubroID = rubroID;

                            rubbean = (Rubros)DB.ExistRubroPais(rub, user.PaisID);

                            if (rubbean == null)
                            {
                                WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                                return;
                            }

                            if ((rubbean.CobIva == 1) && (user.contaID != 2) && (ImpuestoProveedor != 1))//si debe cobrar iva y el rubro no esta en dolares y no es excento
                            {
                                if (rubbean.IvaInc == 1)
                                {
                                    rubbean.rubroTot = valor;
                                    rubbean.rubroSubTot = Math.Round(valor * (double)(1 / (1 + user.pais.Impuesto)), 2);
                                    rubbean.rubroImpuesto = Math.Round(valor - rubbean.rubroSubTot, 2);
                                }
                                else
                                {
                                    rubbean.rubroImpuesto = Math.Round(valor * (double)user.pais.Impuesto, 2);
                                    rubbean.rubroSubTot = Math.Round(valor, 2);
                                    rubbean.rubroTot = Math.Round(rubbean.rubroSubTot + rubbean.rubroImpuesto, 2);
                                }
                                afecto += rubbean.rubroSubTot;
                            }
                            else
                            {
                                rubbean.rubroTot = valor;
                                rubbean.rubroSubTot = valor;
                                rubbean.rubroImpuesto = 0;
                                noafecto += rubbean.rubroSubTot;
                            }
                            #endregion

                            rubbean.rubroTypeID = servicio;//tipo de servicio F=1, L=2
                            rubbean.rubroTotD = equivalente;//monto equivalente
                            rubbean.rubroMoneda = moneda;//id de la moneda
                            if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                            rgb.arr1.Add(rubbean);
                            total += rubbean.rubroTot;
                            impuesto += rubbean.rubroImpuesto;
                        }
                        rgb.Fecha_Hora = lb_fecha_hora.Text;
                        rgb.intC7 = tipoproveedor;
                        rgb.Tipo_Contribuyente = Impuesto_Proveedor;
                        rgb.intC9 = servicio;//servicioID
                        rgb.intC10 = transaccion;//tipo de transaccion segun sys_tipo_referencia
                        rgb.intC3 = int.Parse(dtrow[7].ToString());//proveedorID
                        
                        rgb.decC1 = (decimal)total;//valor
                        rgb.decC2 = (decimal)afecto;//afecto
                        rgb.decC3 = (decimal)noafecto;//no afecto
                        rgb.decC4 = (decimal)impuesto;//impuesto
                        
                        rgb.Nombre_Cliente = Nombre_Cliente;
                        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
                        ArrayList result = DB.recibo_factura_proveedor(rgb, user, "", tipo_contabilidad);
                        if (result == null)
                        {
                            WebMsgBox.Show("Existio un error al momento de generar la provision, porfavor intentelo de nuevo");
                            return;
                        }
                    }
                    WebMsgBox.Show("Se provision exitosamente el BL " + tb_mbl.Text);
                    tb_mbl.Text = "";
                    tb_mbl_id.Text = "";
                    tb_tipo.Text = "";
                    gv_detalle_costos.DataBind();
                }
                else
                {
                    bt_provisionar.Enabled = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                WebMsgBox.Show("Existió un problema al tratar de guardar la información, por favor intente de nuevo");
                return;
            }
        }
        else
        {
            WebMsgBox.Show("No existen Rubros para provisionar, por favor revise de nuevo");
            return;
        }
    }

    
    protected void gv_detalle_costos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("Rubro ID");
        dt.Columns.Add("Rubro");
        dt.Columns.Add("Tipo servicio");
        dt.Columns.Add("Moneda");
        dt.Columns.Add("Valor");
        dt.Columns.Add("Tipo Proveedor");
        dt.Columns.Add("Proveedor ID");
        dt.Columns.Add("Proveedor");
        dt.Columns.Add("Referencia");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
        foreach (GridViewRow row in gv_detalle_costos.Rows)
        {
            lb1 = (Label)row.FindControl("lb_codigo");
            lb2 = (Label)row.FindControl("lb_rubro_id");
            lb3 = (Label)row.FindControl("lb_rubro");
            lb9 = (Label)row.FindControl("lb_rubrotype");
            lb4 = (Label)row.FindControl("lb_monedatipo");
            lb5 = (Label)row.FindControl("lb_total");
            lb6 = (Label)row.FindControl("lb_proveedortipo");
            lb7 = (Label)row.FindControl("lb_proveedorID");
            lb8 = (Label)row.FindControl("lb_proveedor");
            lb10 = (Label)row.FindControl("lb_referencia");
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb9.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, lb8.Text, lb10.Text};
            dt.Rows.Add(objArr);
        }
        dt.Rows[e.RowIndex].Delete();
        gv_detalle_costos.DataSource = dt;
        gv_detalle_costos.DataBind();
    }
    protected void gv_detalle_costos_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
