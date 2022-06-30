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

public partial class operations_editar_corte : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
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
        if (!((permiso & 1024) == 1024))
            Response.Redirect("index.aspx");

        if (!Page.IsPostBack) {
            int id = int.Parse(Request.QueryString["id"].ToString());
            RE_GenericBean datoscorte = (RE_GenericBean)DB.getCortebyID(id, 5);
            if (datoscorte == null) {
                WebMsgBox.Show("El corte que desea modificar no existe o bien ya se encuentra pagado, por favor revisar");
                return;
            }

            tb_serie.Text=datoscorte.strC1;
            tb_corr.Text=datoscorte.intC2.ToString();
            lb_corteID.Text = datoscorte.intC1.ToString();
            lb_moneda.Text = datoscorte.intC3.ToString();
            lb_tipo_persona.Text = datoscorte.intC5.ToString();
            lb_total.Text = datoscorte.decC1.ToString();
            lb_equivalente.Text = datoscorte.decC2.ToString();
            buscar_proveedor(datoscorte.intC4, datoscorte.intC5);
        }
    }
    protected void buscar_proveedor(int provID, int tipopersona)
    {
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean rgb=new RE_GenericBean();
        ArrayList Arr = null;
        string where = "";

        if (tipopersona==4) {
            where += " numero="+provID;
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            if (Arr!=null) {
                rgb=(RE_GenericBean)Arr[0];
                tb_agenteID.Text = rgb.intC1.ToString();
                tb_agentenombre.Text = rgb.strC2;
                tb_contacto.Text = rgb.strC6;
                tb_direccion.Text = rgb.strC5;
                tb_telefono.Text = rgb.strC7;
                tb_correoelectronico.Text = rgb.strC9;
            }
        } else if (tipopersona==2) {
            where += " agente_id="+provID;
            Arr = (ArrayList)DB.getAgente(where, ""); //Agente
            if (Arr!=null) {
                rgb=(RE_GenericBean)Arr[0];
                tb_agenteID.Text = rgb.intC1.ToString();
                tb_agentenombre.Text = rgb.strC1;
                tb_contacto.Text = rgb.strC5;
                tb_direccion.Text = rgb.strC2;
                tb_telefono.Text = rgb.strC3;
                tb_correoelectronico.Text = rgb.strC4;
            }
        } else if (tipopersona==5) {
            where += " id_naviera="+provID;
            Arr = (ArrayList)DB.getNavieras(where, ""); //Naviera
            if (Arr!=null) {
                rgb=(RE_GenericBean)Arr[0];
                tb_agenteID.Text = rgb.intC1.ToString();
                tb_agentenombre.Text = rgb.strC1;
                tb_contacto.Text = "";
                tb_direccion.Text = "";
                tb_telefono.Text = "";
                tb_correoelectronico.Text = "";
            }
        } else if (tipopersona==6) {
            rgb = (RE_GenericBean)DB.getCarriersData(provID); //Lineas aereas
            if (rgb != null)
            {
                tb_agenteID.Text = rgb.intC1.ToString();
                tb_agentenombre.Text = rgb.strC1;
            }
        } else if (tipopersona==8) {
            where += " and id_usuario="+provID;
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Lineas aereas
            if (Arr!=null) {
                rgb=(RE_GenericBean)Arr[0];
                tb_agenteID.Text = rgb.intC1.ToString();
                tb_agentenombre.Text = rgb.strC1;
                tb_contacto.Text = "";
                tb_direccion.Text = "";
                tb_telefono.Text = "";
                tb_correoelectronico.Text = "";
            }
        }
        else if (tipopersona==10)
        {
            rgb = (RE_GenericBean)DB.getIntercompanyData(provID); //Lineas aereas
            if (rgb != null)
            {
                tb_agenteID.Text = rgb.intC1.ToString();
                tb_agentenombre.Text = rgb.strC1;
                tb_contacto.Text = "";
                tb_contacto.Visible = false;
                tb_direccion.Text = rgb.strC4;
            }
        }
        cargo_pendliquidar_y_cortes(tipopersona);
    }

    protected void cargo_pendliquidar_y_cortes(int tipopersona) 
    {
        int proveedorID = int.Parse(tb_agenteID.Text);
        int monID = int.Parse(lb_moneda.Text);
        //obtengo el listado de facturas pendientes
        ArrayList Arr = null;
        string where = " and tpr_proveedor_id=" + tb_agenteID.Text;
        where += " and tpr_ted_id=5 and tpr_tpi_id=" + tipopersona;
        where += " and tpr_tcon_id=" + user.contaID;
        where += " and tpr_pai_id=" + user.PaisID + "";
        where += " and tpr_mon_id=" + monID + "";
        Arr = Utility.getFactProvision("XproveedorID", where, user);
        dt = (DataTable)Utility.fillGridView("FactPendProveedor", Arr);
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();

        Arr = DB.getNotasDebito(proveedorID, tipopersona, monID, user);
        dt = (DataTable)Utility.fillGridView("NDPendProveedor", Arr);
        gv_notadebito.DataSource = dt;
        gv_notadebito.DataBind();

        Arr = DB.getNotasCredito(proveedorID, tipopersona, monID, user);
        dt = (DataTable)Utility.fillGridView("NCPendProveedor", Arr);
        gv_notacredito.DataSource = dt;
        gv_notacredito.DataBind();

        Arr = DB.getNotasCredito(proveedorID, tipopersona, monID, user);
        dt = (DataTable)Utility.fillGridView("NCPendProveedor", Arr);
        gv_notacredito_nd.DataSource = dt;
        gv_notacredito_nd.DataBind();

        int corteID = int.Parse(lb_corteID.Text);
        Arr = DB.getDetalleCorte(corteID);
        dt = (DataTable)Utility.fillGridView("detallecorte", Arr);
        gv_cortes.DataSource = dt;
        gv_cortes.DataBind();

        //obtengo las facturas de intercompany
        if (lb_tipo_persona.Text == "10")
        {
            lb_titulo_fac.Visible = true;
            gv_fact_intercompany.Visible = true;
            Arr = null;
            Arr = DB.getFacturas_Intercompany(proveedorID, int.Parse(lb_tipo_persona.Text), monID, user);
            dt = (DataTable)Utility.fillGridView("FAIntercompany", Arr);
            gv_fact_intercompany.DataSource = dt;
            gv_fact_intercompany.DataBind();
        }
    }
    

    protected void bt_generar_Click(object sender, EventArgs e)
    {
        string separador_ctas = "";
        string separador_nc = "";
        string separador_nd = "";
        string sql = "";
        int monID=int.Parse(lb_moneda.Text);
        int provID = int.Parse(tb_agenteID.Text);
        int tipopersona = 1;
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean cortebean = new RE_GenericBean();
        RE_GenericBean rgb = null;
        ArrayList arrctas = null;
        GridViewRow row=null;
        CheckBox chk;
        decimal totalprov = 0, totalproveq=0;
        decimal totalnc = 0, totalnceq=0;
        decimal totalnd = 0, totalndeq=0;
        decimal totalncnd = 0, totalnc_ndeq = 0;
        decimal totalfa = 0, totalfaeq = 0;
        decimal total = 0;
        decimal totaleq = 0;
        lb_ctas.Text = "";
        lb_nc.Text = "";
        lb_nd.Text = "";
        lb_facts.Text = "";
        #region Validaciones
        if ((tb_agenteID.Text.Trim().Equals("")) || (tb_agenteID.Text.Trim().Equals("0")) || (tb_agentenombre.Text.Trim().Equals("")))
        {
            WebMsgBox.Show("Debe indicar el nombre del Proveedor");
            return;
        }
        //GV Ctas por Pagar
        int ban_cts_xpagar = 0;
        decimal total_suma = 0;
        decimal total_resta = 0;
        CheckBox chk_cta_xpagar;
        if (gv_detalle.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_detalle.Rows)
            {
                chk_cta_xpagar = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_cta_xpagar.Checked)
                {
                    ban_cts_xpagar++;
                    //total_suma += decimal.Parse(grd_Row.Cells[8].Text);
                    totalprov += decimal.Parse(grd_Row.Cells[10].Text);
                }
            }
        }
        //GV Notas de Credito
        int ban_nts_credito = 0;
        CheckBox chk_nts_credito;
        if (gv_notacredito.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_notacredito.Rows)
            {
                chk_nts_credito = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_nts_credito.Checked)
                {
                    ban_nts_credito++;
                    totalnc += decimal.Parse(grd_Row.Cells[7].Text);
                }
            }
        }
        //GV Notas de Credito a ND
        int ban_nts_credito_nd = 0;
        CheckBox chk_nts_credito_nd;
        if (gv_notacredito.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_notacredito_nd.Rows)
            {
                chk_nts_credito_nd = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_nts_credito_nd.Checked)
                {
                    ban_nts_credito_nd++;
                    totalncnd += decimal.Parse(grd_Row.Cells[7].Text);
                }
            }
        }
        //GV Notas de Debito
        int ban_nts_debito = 0;
        CheckBox chk_nts_debito;
        if (gv_notadebito.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_notadebito.Rows)
            {
                chk_nts_debito = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_nts_debito.Checked)
                {
                    ban_nts_debito++;
                    totalnd += decimal.Parse(grd_Row.Cells[7].Text);
                }
            }
        }
        //GV FACTURAS
        int ban_ft = 0;
        CheckBox chk_ft_;
        if (gv_fact_intercompany.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_fact_intercompany.Rows)
            {
                chk_ft_ = (CheckBox)grd_Row.FindControl("CheckBox1");
                if (chk_ft_.Checked)
                {
                    ban_ft++;
                    totalfa += decimal.Parse(grd_Row.Cells[9].Text);
                }
            }
        }
        lb_ctas.Text = "";
        lb_nc.Text = "";
        lb_nd.Text = "";
        //GV Cortes
        int ban_cts_proveedor = 0;
        CheckBox chk_cortes;
        if (gv_cortes.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_cortes.Rows)
            {
                chk_cortes = (CheckBox)grd_Row.FindControl("CheckBox2");
                if (chk_cortes.Checked)
                {
                    ban_cts_proveedor++;
                    if (grd_Row.Cells[8].Text == "NOTA CREDITO")
                    {
                        totalnc += decimal.Parse(grd_Row.Cells[6].Text);
                    }
                    else if (grd_Row.Cells[8].Text == "NOTA CREDITO A NOTA DEBITO")
                    {
                        totalncnd += decimal.Parse(grd_Row.Cells[6].Text);
                    }
                    else if (grd_Row.Cells[8].Text == "PROVISION")
                    {
                        totalprov += decimal.Parse(grd_Row.Cells[6].Text);
                    }
                    else if (grd_Row.Cells[8].Text == "NOTA DEBITO")
                    {
                        totalnd += decimal.Parse(grd_Row.Cells[6].Text);
                    }
                    else if (grd_Row.Cells[8].Text == "FACTURA")
                    {
                        totalfa += decimal.Parse(grd_Row.Cells[6].Text);
                    }
                }
                else
                { 
                    if ((grd_Row.Cells[8].Text == "NOTA CREDITO") || (grd_Row.Cells[8].Text == "NOTA CREDITO A NOTA DEBITO"))
                    {
                        lb_nc.Text += separador_nc + grd_Row.Cells[2].Text;
                        separador_nc = ",";
                    }else if (grd_Row.Cells[8].Text == "NOTA DEBITO")
                    {
                        lb_nd.Text += separador_nd + grd_Row.Cells[2].Text;
                        separador_nd = ",";
                    }
                    else if (grd_Row.Cells[8].Text == "PROVISION")
                    {
                        lb_ctas.Text += separador_ctas + grd_Row.Cells[2].Text;
                        separador_ctas = ",";
                    }
                    else if (grd_Row.Cells[8].Text == "FACTURA")
                    {
                        lb_facts.Text += separador_ctas + grd_Row.Cells[2].Text;
                        separador_ctas = ",";
                    }
                }
            }
        }

        total_suma = totalprov - totalnd - totalfa;
        total_resta = totalnc + totalncnd;
        if ((ban_cts_proveedor == 0) && (ban_cts_xpagar == 0) && (ban_nts_debito == 0) && (ban_ft == 0))
        {
            WebMsgBox.Show("Debe seleccionar al menos una cuenta por pagar, nota de debito o factura");
            return;
        }

        if (totalnc > totalprov)
        {
            WebMsgBox.Show("El valor total de Notas de Credito no debe ser mayor al valor Total de Provisiones");
            return;
        }
        if (totalncnd > totalnd)
        {
            WebMsgBox.Show("El valor total de Notas de Credito a Notas de Debito no debe ser mayor al valor Total de Notas de Debito");
            return;
        }

        //if (total_suma < total_resta)
        //{
        //    WebMsgBox.Show("No puede aplicar un credito mayor al debito");
        //    return;
        //}
        #endregion


        totalprov = 0;
        totalproveq = 0;
        totalnc = 0;
        totalnceq = 0;
        totalnd = 0;
        totalndeq = 0;
        totalncnd = 0;
        totalnc_ndeq = 0;
        totalfa = 0;
        totalfaeq = 0;
        total = 0;
        totaleq = 0;

        //*Recorro el grid de provisiones        
        for (int i = 0; i < gv_detalle.Rows.Count; i++)
        {
            row = gv_detalle.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("CheckBox1");
                if (chk.Checked) {
                    rgb = new RE_GenericBean();
                    rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                    if (row.Cells[2].Text == "&nbsp;")
                    {
                        rgb.strC1 = "";//No del documento factura, nota credito, nota debito, etc}
                    }
                    else
                    {
                        rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc}
                    }
                    rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[11].Text);//Moneda
                    rgb.intC3 = provID;//proveedor                
                    rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                    rgb.intC5 = 5;//segun Sys_tipo_Referencia
                    rgb.strC2 = user.ID;//usuario.T
                    rgb.strC3 = row.Cells[3].Text;
                    rgb.decC1 = decimal.Parse(row.Cells[10].Text);
                    rgb.decC2 = decimal.Parse(row.Cells[13].Text);
                    if (arrctas == null) arrctas = new ArrayList();
                    arrctas.Add(rgb);
                    totalprov += rgb.decC1;
                    totalproveq += rgb.decC2;
                }
            }
        }
        row = null;
        //*Recorro el grid de Notas de credito
        for (int i = 0; i < gv_notacredito.Rows.Count; i++)
        {
            row = gv_notacredito.Rows[i];
            if (row.Cells[10].Text != "31")
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    chk = (CheckBox)row.FindControl("CheckBox1");
                    if (chk.Checked)
                    {
                        rgb = new RE_GenericBean();
                        rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                        rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc
                        rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[8].Text);//Moneda
                        rgb.intC3 = provID;//proveedor                
                        rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                        rgb.intC5 = 12; //segun Sys_tipo_referencia
                        rgb.strC2 = user.ID;//usuario
                        rgb.strC3 = row.Cells[3].Text;
                        rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                        rgb.decC2 = decimal.Parse(row.Cells[9].Text);
                        if (arrctas == null) arrctas = new ArrayList();
                        arrctas.Add(rgb);
                        totalnc += rgb.decC1;
                        totalnceq += rgb.decC2;
                    }
                }
            }
        }
        row = null;
        //*Recorro el grid de Notas de credito a ND
        for (int i = 0; i < gv_notacredito_nd.Rows.Count; i++)
        {
            row = gv_notacredito_nd.Rows[i];
            if (row.Cells[10].Text != "12")
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    chk = (CheckBox)row.FindControl("CheckBox1");
                    if (chk.Checked)
                    {
                        rgb = new RE_GenericBean();
                        rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                        rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc
                        rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[8].Text);//Moneda
                        rgb.intC3 = provID;//proveedor                
                        rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                        rgb.intC5 = 31; //segun Sys_tipo_referencia
                        rgb.strC2 = user.ID;//usuario
                        rgb.strC3 = row.Cells[3].Text;
                        rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                        rgb.decC2 = decimal.Parse(row.Cells[9].Text);

                        if (arrctas == null) arrctas = new ArrayList();
                        arrctas.Add(rgb);
                        totalncnd += rgb.decC1;
                        totalnc_ndeq += rgb.decC2;
                    }
                }
            }
        }
        row = null;
        //*Recorro el grid de Notas de debito
        for (int i = 0; i < gv_notadebito.Rows.Count; i++)
        {
            row = gv_notadebito.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("CheckBox1");
                if (chk.Checked)
                {
                    rgb = new RE_GenericBean();
                    rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                    rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc
                    rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[8].Text);//Moneda
                    rgb.intC3 = provID;//proveedor                
                    rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                    rgb.intC5 = 4;//segun Sys_tipo_Referencia
                    rgb.strC2 = user.ID;//usuario
                    rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                    rgb.strC3 = row.Cells[3].Text;
                    rgb.decC2 = decimal.Parse(row.Cells[9].Text);

                    if (arrctas == null) arrctas = new ArrayList();
                    arrctas.Add(rgb);
                    totalnd += rgb.decC1;
                    totalndeq += rgb.decC2;
                }
            }
        }
        //*Recorro el grid de Facturas Intercompany
        for (int i = 0; i < gv_fact_intercompany.Rows.Count; i++)
        {
            row = gv_fact_intercompany.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("CheckBox1");
                if (chk.Checked)
                {
                    rgb = new RE_GenericBean();
                    rgb.intC1 = int.Parse(row.Cells[1].Text);//No de id de la transaccion
                    rgb.strC1 = row.Cells[2].Text;//No del documento factura, nota credito, nota debito, etc
                    //rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[8].Text);//Moneda
                    rgb.intC2 = Utility.TraducirMonedaStr(row.Cells[11].Text);//Moneda
                    rgb.intC3 = provID;//proveedor                
                    rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                    rgb.intC5 = 1;//segun Sys_tipo_Referencia
                    rgb.strC2 = user.ID;//usuario
                    //rgb.decC1 = decimal.Parse(row.Cells[7].Text);
                    rgb.decC1 = decimal.Parse(row.Cells[9].Text);
                    rgb.strC3 = row.Cells[3].Text;
                    //rgb.decC2 = decimal.Parse(row.Cells[9].Text);
                    rgb.decC2 = decimal.Parse(row.Cells[10].Text);
                    if (arrctas == null) arrctas = new ArrayList();
                    arrctas.Add(rgb);
                    totalfa += rgb.decC1;
                    totalfaeq += rgb.decC2;
                }
            }
        }
        //*Recorro el grid de cortes
        for (int i = 0; i < gv_cortes.Rows.Count; i++)
        {
            row = gv_cortes.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("CheckBox2");
                if (chk.Checked)
                {
                    rgb = new RE_GenericBean();
                    rgb.intC1 = int.Parse(row.Cells[2].Text);//No de id de la transaccion
                    if (row.Cells[3].Text == "&nbsp;")
                    {
                        rgb.strC1 = "";//No del documento factura, nota credito, nota debito, etc
                    }
                    else 
                    {
                        rgb.strC1 = row.Cells[3].Text;//No del documento factura, nota credito, nota debito, etc
                    }
                    rgb.intC2 = monID;//Moneda
                    rgb.intC3 = provID;//proveedor                
                    rgb.intC4 = tipopersona;//tipo de persona, proveedor, agente, naviera, linea aerea, etc
                    rgb.intC5 = Utility.TraducirOperaciontoint(row.Cells[8].Text);//segun Sys_tipo_Referencia
                    rgb.strC2 = user.ID;//usuario
                    rgb.decC1 = decimal.Parse(row.Cells[6].Text);
                    rgb.decC2 = decimal.Parse(row.Cells[7].Text);
                    rgb.strC3 = row.Cells[4].Text;//Fecha
                    if (arrctas == null) arrctas = new ArrayList();
                    arrctas.Add(rgb);

                    if (row.Cells[8].Text == "NOTA CREDITO")
                    {
                        totalnc += rgb.decC1;
                        totalnceq += rgb.decC2;
                    }
                    if (row.Cells[8].Text == "NOTA CREDITO A NOTA DEBITO")
                    {
                        totalncnd += rgb.decC1;
                        totalnc_ndeq += rgb.decC2;
                    }
                    if (row.Cells[8].Text == "NOTA DEBITO")
                    {
                        totalnd += rgb.decC1;
                        totalndeq += rgb.decC2;
                    }
                    if (row.Cells[8].Text == "PROVISION")
                    {
                        totalprov += rgb.decC1;
                        totalproveq += rgb.decC2;
                    }
                    if (row.Cells[8].Text == "FACTURA")
                    {
                        totalfa += rgb.decC1;
                        totalfaeq += rgb.decC2;
                    }
                }
            }
        }
        //decimal total = totalprov - totalnd - totalnc + totalncnd;
        //decimal totaleq = totalproveq - totalndeq -totalnceq + totalnc_ndeq;

        total = totalprov - totalnd - totalnc + totalncnd - totalfa;
        totaleq = totalproveq - totalndeq - totalnceq + totalnc_ndeq - totalfaeq;
        
        cortebean.intC1 = provID;   
        cortebean.intC2 = monID;
        cortebean.intC3 = tipopersona;
        cortebean.decC1 = total;
        cortebean.decC2 = totaleq;
        cortebean.arr1 = arrctas;

        int result = DB.UpdateCorteProv(cortebean, user, int.Parse(lb_corteID.Text));
        int res = 0;
        if (result == -100) {
            WebMsgBox.Show("Existió un problema al momento de guardar la información, por favor intente de nuevo");
            return;
        }
        #region Actualizar Estado
        if (!lb_nc.Text.Trim().Equals(""))
        {
            sql = "update tbl_nota_credito set tnc_ted_id=1 where tnc_id in (" + lb_nc.Text + ")";
            res = DB.UpdateEstadoDocumento(sql);
        }
        if (!lb_nd.Text.Trim().Equals(""))
        {
            sql = "update tbl_nota_debito set tnd_ted_id=1 where tnd_id in (" + lb_nd.Text + ")";
            res = DB.UpdateEstadoDocumento(sql);
        }
        if (!lb_ctas.Text.Trim().Equals(""))
        {
            sql = "update tbl_provisiones set tpr_ted_id=5 where tpr_prov_id in (" + lb_ctas.Text + ")";
            res = DB.UpdateEstadoDocumento(sql);
        }
        if (!lb_facts.Text.Trim().Equals(""))
        {
            sql = "update tbl_facturacion set tfa_ted_id=1 where tfa_id in (" + lb_facts.Text + ")";
            res = DB.UpdateEstadoDocumento(sql);
        }
        #endregion
        Response.Redirect("editar_corte.aspx?id="+lb_corteID.Text);
    }

   
    protected void gv_cortes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sql = "";
        if (e.CommandName == "Eliminar")
        {
            int index = Convert.ToInt32(e.CommandArgument);

            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Correlativo");
            dt.Columns.Add("Documento");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Moneda");
            dt.Columns.Add("Total");
            dt.Columns.Add("Total Eq.");
            dt.Columns.Add("Tipo Documento");
            foreach (GridViewRow row in gv_cortes.Rows)
            {
                object[] objArr = { row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text, row.Cells[6].Text, row.Cells[7].Text, row.Cells[8].Text };
                dt.Rows.Add(objArr);
            }
            #region Actualizar Estado
            int ID = 0;
            ID = int.Parse(gv_cortes.Rows[index].Cells[2].Text.ToString());
            string tipo_documento = gv_cortes.Rows[index].Cells[8].Text.ToString();
            if ((tipo_documento == "NOTA CREDITO") || (tipo_documento == "NOTA CREDITO A NOTA DEBITO"))//Nota Credito Proveedores
            {
                sql = "update tbl_nota_credito set tnc_ted_id=1 where tnc_id=" + ID + "";
            }
            if (tipo_documento == "NOTA DEBITO")//Nota Debito
            {
                sql = "update tbl_nota_debito set tnd_ted_id=1 where tnd_id=" +ID + "";
            }
            if (tipo_documento == "PROVISION")//Provisiones
            {
                sql = "update tbl_provisiones set tpr_ted_id=5 where tpr_prov_id=" + ID + "";
            }
            int res = DB.UpdateEstadoDocumento(sql);
            cargo_pendliquidar_y_cortes(int.Parse(lb_tipo_persona.Text));
            #endregion
            dt.Rows[index].Delete();
            gv_cortes.DataSource = dt;
            gv_cortes.DataBind();
        }
    }
    protected void gv_cortes_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
        }
    }
    protected void gv_notadebito_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1) {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = false;
        }

    }
    protected void gv_notacredito_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = false;
        }
    }
    protected void gv_detalle_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1) {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[12].Visible = false;
        }
    }
    protected void gv_notacredito_ndRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = false;
        }

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        GridViewRowCollection gvr = gv_notacredito.Rows;
        foreach (GridViewRow row in gvr)
        {
            if (row.Cells[10].Text == "31")
            {
                row.Visible = false;
            }
        }
        GridViewRowCollection gvra = gv_notacredito_nd.Rows;
        foreach (GridViewRow row in gvra)
        {
            if (row.Cells[10].Text == "12")
            {
                row.Visible = false;
            }
        }
    }
    protected void gv_factintercompany_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = true;
        }
    }
}
