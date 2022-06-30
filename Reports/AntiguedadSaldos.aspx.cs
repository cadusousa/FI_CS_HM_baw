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
//using System.Text;

public partial class Reports_AntiguedadSaldos : System.Web.UI.Page
{
    UsuarioBean user = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        #region Definir Fechas
        if (!IsPostBack)
        {
            DateTime Fecha = DateTime.Now;
            tb_fechacorte.Text = Fecha.Month.ToString() + "/" + Fecha.Day.ToString() + "/" + Fecha.Year.ToString();
            //************************RESTRICCION DE CONTABILIDAD USUARIOS****************************//
            lb_contabilidad.Items.Clear();
            int bandera = 0; // verifica si el usuario tiener contabilidad consolidada.
            int fiscal = 0;
            int financiera = 0;
            ListItem item = null;
            ArrayList arruser = (ArrayList)DB.getContaUsuario(user.ID, user.PaisID);
            ArrayList arrbloqueo = (ArrayList)DB.getContaPais(user.PaisID);
            foreach (RE_GenericBean rgbp in arrbloqueo)
            {
                if (rgbp.intC1 == 1)
                {
                    fiscal = 1; //desbloqueo fiscal
                }

                if (rgbp.intC2 == 1)
                {
                    financiera = 1; // desbloqueo financiera.
                }
            }
            foreach (RE_GenericBean rgb in arruser)
            {
                if ((rgb.intC1 == 1) && (fiscal == 1))
                {
                    bandera++;
                    item = new ListItem("FISCAL", "1");
                    if (user.contaID == 1)
                    {
                        lb_contabilidad.Items.Add(item);
                    }
                }

                if ((rgb.intC2 == 1) && (financiera == 1))
                {
                    bandera++;
                    item = new ListItem("FINANCIERA", "2");
                    if (user.contaID == 2)
                    {
                        lb_contabilidad.Items.Add(item);
                    }
                }
            }
            if (bandera == 1)
            {
                lb_contabilidad.Visible = false;
                l_contabilidad.Visible = false;
            }
            else
            {
                lb_contabilidad.Visible = true;
                l_contabilidad.Visible = true;
            }
            if (user.contaID == 2)
            {
                lb_consolidar_moneda.Visible = false;
                chk_consolidar_moneda.Visible = false;
            }
        }
        if (IsPostBack)
        {
            if (tb_agenteID.Text != " ")
            {
                cb_credito.Enabled = false;
            }
        }
        #endregion



        //*********************************FIN RESTRICCION********************************************//
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad, user.PaisID);
    }

    private void obtengo_listas(int tipoconta, int paiID)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(paiID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }

            lb_cobrador.Items.Clear();
            arr = null;
            arr = (ArrayList)DB.getcobradores(" and id_pais=" + user.PaisID);
            item = new ListItem("Seleccione...", "0");
            lb_cobrador.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.strC1);
                lb_cobrador.Items.Add(item);
            }

            //Filtros APL: Pablo Aguilar
            lb_sucursal.Items.Clear();
            arr = null;
            arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + user.PaisID);
            /*item = new ListItem("Seleccione...", "0");
            lb_sucursal.Items.Add(item);*/
            foreach (SucursalBean suc in arr)
            {
                item = new ListItem(suc.Nombre, suc.ID.ToString());
                lb_sucursal.Items.Add(item);
            }
            //Fin Filtros
        }
    }

    protected void lb_sucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        lblSerieFacturas.Visible = true;
        lblSerieRecibos.Visible = true;
        lb_facturas.Visible = true;
        lb_facturas.Enabled = false;
        lb_recibos.Visible = true;
        lb_recibos.Enabled = false;


        //Filtros APL: Facturas
        lb_facturas.Items.Clear();
        arr = null;
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue.ToString()), 1, user, 0);//1 porque es el tipo de documento para facturacion
        /*item = new ListItem("Seleccione...", "0");
        lb_facturas.Items.Add(item);*/
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_facturas.Items.Add(item);
        }
        //Fin Filtros

        //Filtros APL: Recibos
        lb_recibos.Items.Clear();
        arr = null;
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue.ToString()), 2, user, 0);//1 porque es el tipo de documento para recibos
        /*item = new ListItem("Seleccione...", "0");
        lb_recibos.Items.Add(item);*/
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_recibos.Items.Add(item);
        }
        //Fin Filtros
    }

    protected void bt_generar_Click(object sender, EventArgs e)
    {


        /*StringBuilder sb = new StringBuilder();
        foreach (object obj in arr_sucursales) sb.Append(obj);
        string s = sb.ToString();*/

        string consolidaMoneda = "";

        if (chk_consolidar_moneda.Checked == true)
        {
            consolidaMoneda = "si";
        }
        else
        {
            consolidaMoneda = "no";
        }
        gv_antiguedadsaldos.DataBind();
        gv_detallecliente.DataBind();
        if (tb_fechacorte.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Indicar la Fecha de Corte");
            return;
        }
        else
        {
            LibroDiarioDS ds = null;
            string fecha = tb_fechacorte.Text.Substring(6, 4) + "-" + tb_fechacorte.Text.Substring(0, 2) + "-" + tb_fechacorte.Text.Substring(3, 2);
            string orderby = lb_orderby.SelectedValue;
            int monedaID = int.Parse(lb_moneda.SelectedValue);
            int contabilidadID = int.Parse(lb_contabilidad.SelectedValue);
            int tipopersona = int.Parse(lb_tipopersona.SelectedValue);
            int cliID = 0;
            int credito = 0;
            string band_anti = rb_recibos_ant.Checked.ToString();
            string incluir_dias_credito = rb_dias_credito.Checked.ToString();

            //Filtros APL 
            int sucursal = 0;
            string serie_factura = "";
            string serie_recibo = "";

            //sucursal = int.Parse(lb_sucursal.SelectedValue);
            ArrayList arr_sucursales = new ArrayList();
            for (int a = 0; a < lb_sucursal.Items.Count; a++)
            {
                if (lb_sucursal.Items[a].Selected == true)
                {
                    arr_sucursales.Add(lb_sucursal.Items[a].Value);
                }
            }

            string strSucursales = string.Join(",", arr_sucursales.ToArray());
            serie_factura = lb_facturas.SelectedValue.ToString();
            serie_recibo = lb_recibos.SelectedValue.ToString();
            //Fin filtros APL

            if (cb_credito.SelectedItem.Text.ToString() == "Credito")
            {
                credito = 1;
            }
            else if (cb_credito.SelectedItem.Text.ToString() == "Contado")
            {
                credito = 2;
            }
            else
            {
                credito = 0;
            }
            string cobrador = lb_cobrador.SelectedValue.Trim();
            if ((tb_agenteID.Text != null) && (tb_agenteID.Text.Trim()!=""))
            {
                cliID = int.Parse(tb_agenteID.Text);
            }
            string es_coloader = "no";
            if (chk_coloader.Checked == true)
            {
                es_coloader = "si";
            }


            if (int.Parse(tb_Tipo.Text) == 1) //saldos cortados
            {


                //ds = (LibroDiarioDS)DB.getSaldosVencidos(user.PaisID, monedaID, contabilidadID, fecha, tipopersona, cliID, credito, cobrador,band_anti);
                if (tipopersona != 3)
                { /* 2.Agentes. 4.Proveedores, 5.navieras, 6.lineas aereas*/
                    ds = (LibroDiarioDS)DB.getAntiguedadDeSaldosProveedor(user, user.PaisID, monedaID, contabilidadID, fecha, tipopersona, cliID, credito, cobrador, band_anti, strSucursales, serie_factura, serie_recibo, incluir_dias_credito);
                    //ds = (LibroDiarioDS)DB.getSaldosVencidos(user.PaisID, monedaID, contabilidadID, fecha, tipopersona, cliID, credito, cobrador, band_anti);
                }
                else
                { // 3.Clientes
                    ds = (LibroDiarioDS)DB.getAntiguedadDeSaldosCliente(user.PaisID, monedaID, contabilidadID, fecha, tipopersona, cliID, credito, cobrador, band_anti, strSucursales, serie_factura, serie_recibo, incluir_dias_credito, es_coloader);
                }
            }
            else //saldos completos
            {
                if (tipopersona != 3)
                { /* 2.Agentes. 4.Proveedores, 5.navieras, 6.lineas aereas*/
                    ds = (LibroDiarioDS)DB.getAntiguedadDeSaldosProveedor2(user, user.PaisID, monedaID, contabilidadID, fecha, tipopersona, cliID, credito, cobrador, band_anti, strSucursales, serie_factura, serie_recibo, incluir_dias_credito);
                }
                else
                { // 3.Clientes
                    ds = (LibroDiarioDS)DB.getAntiguedadDeSaldosCliente(user.PaisID, monedaID, contabilidadID, fecha, tipopersona, cliID, credito, cobrador, band_anti, strSucursales, serie_factura, serie_recibo, incluir_dias_credito, es_coloader);
                }
            }


            try
            {

                if (ds.Tables["tbl_antiguedad_cliente"].Rows.Count > 0)
                {
                    //Ordenando el DataSet
                    // DataView view = ds.Tables["antiguedadsaldos_tbl"].DefaultView;
                    DataView view = ds.Tables["tbl_antiguedad_cliente"].DefaultView;
                    //view.Sort = "Orden, masde75 Desc, _75dias Desc, _60dias Desc, _45dias Desc, _30dias Desc, Nombre Desc, cliID Desc";
                    view.Sort = "orden, massetenta Desc, setenta Desc, sesenta Desc, cuarenta Desc, treinta Desc, nombre Desc, cliente Desc";

                    //Sumando los totales de cada columna
                    //object sumaSaldo = ds.Tables["antiguedadsaldos_tbl"].Compute("SUM(Saldo)", null);
                    //object sumaMasde75 = ds.Tables["antiguedadsaldos_tbl"].Compute("SUM(masde75)", null);
                    //object suma75dias = ds.Tables["antiguedadsaldos_tbl"].Compute("SUM(_75dias)", null);
                    //object suma60dias = ds.Tables["antiguedadsaldos_tbl"].Compute("SUM(_60dias)", null);
                    //object suma45dias = ds.Tables["antiguedadsaldos_tbl"].Compute("SUM(_45dias)", null);
                    //object suma30dias = ds.Tables["antiguedadsaldos_tbl"].Compute("SUM(_30dias)", null);
                    //object sumaIntereses = ds.Tables["antiguedadsaldos_tbl"].Compute("SUM(Intereses)", null);

                    object sumaSaldo = ds.Tables["tbl_antiguedad_cliente"].Compute("SUM(Saldo)", null);
                    object sumaMasde75 = ds.Tables["tbl_antiguedad_cliente"].Compute("SUM(massetenta)", null);
                    object suma75dias = ds.Tables["tbl_antiguedad_cliente"].Compute("SUM(setenta)", null);
                    object suma60dias = ds.Tables["tbl_antiguedad_cliente"].Compute("SUM(sesenta)", null);
                    object suma45dias = ds.Tables["tbl_antiguedad_cliente"].Compute("SUM(cuarenta)", null);
                    object suma30dias = ds.Tables["tbl_antiguedad_cliente"].Compute("SUM(treinta)", null);
                    //object sumaIntereses = ds.Tables["tbl_antiguedad_cliente"].Compute("SUM(Intereses)", null);



                    //calculando la proporcion de cada columna respecto al total general
                    decimal total = decimal.Parse(sumaMasde75.ToString()) + decimal.Parse(suma75dias.ToString()) + decimal.Parse(suma60dias.ToString()) + decimal.Parse(suma45dias.ToString()) + decimal.Parse(suma30dias.ToString());
                    decimal porcentajeMasde75 = 0;
                    decimal porcentaje75dias = 0;
                    decimal porcentaje60dias = 0;
                    decimal porcentaje45dias = 0;
                    decimal porcentaje30dias = 0;
                    if (total != 0)
                    {
                        porcentajeMasde75 = Math.Round((decimal.Parse(sumaMasde75.ToString()) / total) * 100, 2);
                        porcentaje75dias = Math.Round((decimal.Parse(suma75dias.ToString()) / total) * 100, 2);
                        porcentaje60dias = Math.Round((decimal.Parse(suma60dias.ToString()) / total) * 100, 2);
                        porcentaje45dias = Math.Round((decimal.Parse(suma45dias.ToString()) / total) * 100, 2);
                        porcentaje30dias = Math.Round((decimal.Parse(suma30dias.ToString()) / total) * 100, 2);
                    }
                    //Agregando las filas de los totales y porcentajes
                    DataRow rowTotales = ds.Tables["tbl_antiguedad_cliente"].NewRow();
                    rowTotales["Nombre"] = "TOTALES";
                    rowTotales["Saldo"] = sumaSaldo;
                    rowTotales["massetenta"] = sumaMasde75;
                    // rowTotales["masde75"] = sumaMasde75;
                    rowTotales["setenta"] = suma75dias;
                    rowTotales["sesenta"] = suma60dias;
                    rowTotales["cuarenta"] = suma45dias;
                    rowTotales["treinta"] = suma30dias;
                    //rowTotales["Intereses"] = sumaIntereses;
                    rowTotales["orden"] = 2;

                    DataRow rowPorcentajes = ds.Tables["tbl_antiguedad_cliente"].NewRow();
                    rowPorcentajes["Nombre"] = "PORCENTAJES";
                    rowPorcentajes["massetenta"] = porcentajeMasde75;
                    rowPorcentajes["setenta"] = porcentaje75dias;
                    rowPorcentajes["sesenta"] = porcentaje60dias;
                    rowPorcentajes["cuarenta"] = porcentaje45dias;
                    rowPorcentajes["treinta"] = porcentaje30dias;
                    rowPorcentajes["orden"] = 3;

                    ds.Tables["tbl_antiguedad_cliente"].Rows.Add(rowTotales);
                    ds.Tables["tbl_antiguedad_cliente"].Rows.Add(rowPorcentajes);



                    //Cargando el DataGrid
                    gv_antiguedadsaldos.DataSource = ds.Tables["tbl_antiguedad_cliente"];
                    gv_antiguedadsaldos.DataBind();



                    //Ocultando la Columna de orden
                    /*int CantRows = gv_antiguedadsaldos.Rows[0].Cells.Count - 1;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows].Visible = false;
                    }*/
                    //Ocultando la Columna de orden
                    int CantRows2 = gv_antiguedadsaldos.Rows[0].Cells.Count - 2;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows2].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows2].Visible = false;
                    }
                    //Ocultando la Columna de orden
                    int CantRows3 = gv_antiguedadsaldos.Rows[0].Cells.Count - 3;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows3].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows3].Visible = false;
                    }
                    //Ocultando la Columna de orden
                    int CantRows4 = gv_antiguedadsaldos.Rows[0].Cells.Count - 4;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows4].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows4].Visible = false;
                    }
                    //Ocultando la Columna de orden
                    int CantRows5 = gv_antiguedadsaldos.Rows[0].Cells.Count - 5;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows5].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows5].Visible = false;
                    }
                    //Ocultando la Columna de orden
                    int CantRows6 = gv_antiguedadsaldos.Rows[0].Cells.Count - 6;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows6].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows6].Visible = false;
                    }
                    int CantRows7 = gv_antiguedadsaldos.Rows[0].Cells.Count - 7;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows7].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows7].Visible = false;
                    }
                    int CantRows8 = gv_antiguedadsaldos.Rows[0].Cells.Count - 8;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows8].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows8].Visible = false;
                    }
                    int CantRows9 = gv_antiguedadsaldos.Rows[0].Cells.Count - 9;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows9].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows9].Visible = false;
                    }
                    int CantRows10 = gv_antiguedadsaldos.Rows[0].Cells.Count - 10;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows10].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows10].Visible = false;
                    }
                    int CantRows11 = gv_antiguedadsaldos.Rows[0].Cells.Count - 11;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows11].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows11].Visible = false;
                    }

                    int CantRows12 = gv_antiguedadsaldos.Rows[0].Cells.Count - 12;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows12].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows12].Visible = false;
                    }
                    int CantRows13 = gv_antiguedadsaldos.Rows[0].Cells.Count - 13;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows13].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows13].Visible = false;
                    }

                    int CantRows18 = gv_antiguedadsaldos.Rows[0].Cells.Count - 20;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows18].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows18].Visible = false;
                    }
                    int CantRows19 = gv_antiguedadsaldos.Rows[0].Cells.Count - 21;
                    gv_antiguedadsaldos.HeaderRow.Cells[CantRows19].Visible = false;
                    for (int i = 0; i < gv_antiguedadsaldos.Rows.Count; i++)
                    {
                        gv_antiguedadsaldos.Rows[i].Cells[CantRows19].Visible = false;
                    }

                    ////Cambiando el Encabezado del DataGrid
                    gv_antiguedadsaldos.HeaderRow.Cells[1].Text = "ID";
                    gv_antiguedadsaldos.HeaderRow.Cells[2].Text = "Nombre";
                    gv_antiguedadsaldos.HeaderRow.Cells[5].Text = "Saldo";
                    gv_antiguedadsaldos.HeaderRow.Cells[6].Text = "30dias";
                    gv_antiguedadsaldos.HeaderRow.Cells[7].Text = "45dias";
                    gv_antiguedadsaldos.HeaderRow.Cells[8].Text = "60dias";
                    gv_antiguedadsaldos.HeaderRow.Cells[9].Text = "75dias";
                    gv_antiguedadsaldos.HeaderRow.Cells[10].Text = ">75dias";
                    gv_antiguedadsaldos.HeaderRow.Cells[23].Text = "Dias Credito";

                    //Resaltando el resumen total y porcentaje, y ocultando el boton seleccionar para esos totales
                    gv_antiguedadsaldos.Rows[gv_antiguedadsaldos.Rows.Count - 1].Cells[0].Text = null;
                    gv_antiguedadsaldos.Rows[gv_antiguedadsaldos.Rows.Count - 2].Cells[0].Text = null;
                    gv_antiguedadsaldos.Rows[gv_antiguedadsaldos.Rows.Count - 1].Font.Bold = true;
                    gv_antiguedadsaldos.Rows[gv_antiguedadsaldos.Rows.Count - 2].Font.Bold = true;

                }
                //Detalle del Cliente
                gv_detallecliente.DataBind();

            }
            catch (Exception ex)
            {

                string tes = ex.Message;

            }
        }
    }
    //-------------------------------------------------------------------------------------------
    protected void gv_antiguedadsaldos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string band_anti = rb_recibos_ant.Checked.ToString();
        string incluir_dias_credito = rb_dias_credito.Checked.ToString();
        int index = Convert.ToInt32(e.CommandArgument);
        int monedaID = int.Parse(lb_moneda.SelectedValue);
        int contabilidadID = int.Parse(lb_contabilidad.SelectedValue);
        int tipopersona = int.Parse(lb_tipopersona.SelectedValue);
        string fecha = tb_fechacorte.Text.Substring(6, 4) + "-" + tb_fechacorte.Text.Substring(0, 2) + "-" + tb_fechacorte.Text.Substring(3, 2);
        long cliID = long.Parse(gv_antiguedadsaldos.Rows[index].Cells[1].Text);
        ArrayList arr_sucursales = new ArrayList();
        for (int a = 0; a < lb_sucursal.Items.Count; a++)
        {
            if (lb_sucursal.Items[a].Selected == true)
            {
                arr_sucursales.Add(lb_sucursal.Items[a].Value);
            }
        }

        string strSucursales = string.Join(",", arr_sucursales.ToArray());
        if (!gv_antiguedadsaldos.Rows[index].Cells[2].Text.Equals("0.00"))
        {

            if (int.Parse(tb_Tipo.Text) == 1) //saldos cortados
            {
                DataTable dt = (DataTable)DB.getDetalleCliente(cliID, monedaID, contabilidadID, user.PaisID, fecha, tipopersona, band_anti, incluir_dias_credito, strSucursales);//obtiene todas las facturas que el cliente tiene pendiente de pago y abonos del cliente
                gv_detallecliente.DataSource = dt;

            }
            else
            {
                DataTable dt = (DataTable)DB.getDetalleCliente2(cliID, monedaID, contabilidadID, user.PaisID, fecha, tipopersona, band_anti, incluir_dias_credito, strSucursales);//obtiene todas las facturas que el cliente tiene pendiente de pago y abonos del cliente
                gv_detallecliente.DataSource = dt;

            }
        }
        gv_detallecliente.DataBind();
    }
    protected void btn_impresion_Click(object sender, EventArgs e)
    {
        if (tb_fechacorte.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Indicar la Fecha de Corte");
            return;
        }
        else
        {
            string mensaje = "<script languaje=\"JavaScript\">";
            mensaje += "window.open('reports.aspx?credito=" + cb_credito.SelectedItem.Text.ToString() + "&cobrador=" + lb_cobrador.SelectedValue + "&reptype=4&fechacorte=" + tb_fechacorte.Text + "&tipopersona=" + lb_tipopersona.SelectedValue + "&monID=" + lb_moneda.SelectedValue + "&contaID=" + lb_contabilidad.SelectedValue + "&cliID=" + tb_agenteID.Text + "&orderby=" + lb_orderby.SelectedValue + "&antrecibos=" + rb_recibos_ant.Checked.ToString() + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            mensaje += "</script>";
            Page.RegisterClientScriptBlock("closewindow", mensaje);
        }
    }


    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
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
            Arr = (ArrayList)DB.getClientes(where, user, "REPORTES"); //Proveedor
            dt = (DataTable)Utility.fillGridView("cliente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, "REPORTES"); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit a un agente");
                return;
            }

            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(where, "REPORTES"); //Agente
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
            Arr = (ArrayList)DB.getNavieras(where, "REPORTES"); //Naviera
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
        else if (lb_tipopersona.SelectedValue.Equals("10"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(nombre_comercial)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_intercompany=" + tb_codigo.Text; else where += " and id_intercompany=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.Get_Intercompanys(where); //INTERCOMPANY
            dt = (DataTable)Utility.fillGridView("Intercompany", Arr);
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
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("8"))//proveedorre cajachica
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("3"))//Clientes
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompany
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
        }
        else if (lb_tipopersona.SelectedValue.Equals("0"))//Retenciones
        {
            tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
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
    protected void gv_antiguedadsaldos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[9].Visible = false;
    }

    //--------------------------------------------------------------------------------------------------------

    protected void gv_detallecliente_DataBound(object sender, EventArgs e)
    {
        decimal total = 0, abonos = 0, saldo = 0, sumafvr = 0, tabonos = 0;
        int tipopersona = int.Parse(lb_tipopersona.SelectedValue);
        foreach (GridViewRow dr in gv_detallecliente.Rows)
        {
            total += decimal.Parse(dr.Cells[5].Text.Trim());
            tabonos += decimal.Parse(dr.Cells[6].Text.Trim());
            abonos += decimal.Parse(dr.Cells[7].Text.Trim());
            saldo += decimal.Parse(dr.Cells[9].Text.Trim());
            // -----------------------------------------------------
            sumafvr += decimal.Parse(dr.Cells[8].Text.Trim());
          
        }
        if (tipopersona != 3)
        {
            total = total + tabonos;
        }
        //------cambio dreiby---------
        if (tipopersona == 3) { abonos = tabonos; }
        SumTotal.Text = "<b>Suma Total:</b> " + total;
        SumAbono.Text = "<b>Suma Abono:</b> " + abonos;
        SumSaldo.Text = "<b>Suma Saldo:</b> " + (saldo - sumafvr);
        SumAfvr.Text = "<b>Saldo A Favor:</b> " + sumafvr;
        SumTotal.Visible = true;
        SumAbono.Visible = true;
        SumSaldo.Visible = true;
        SumAfvr.Visible = true;
    }
    protected void bt_generar_anti_Click(object sender, EventArgs e)
    {
        string consolidaMoneda = "";

        if (chk_consolidar_moneda.Checked == true)
        {
            consolidaMoneda = "si";
        }
        else
        {
            consolidaMoneda = "no";
        }

        if (tb_fechacorte.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Indicar la Fecha de Corte");
            return;
        }
        else
        {
            //Filtros APL 
            int sucursal = 0;
            string serie_factura = "";
            string serie_recibo = "";
            string strSucursales = "";

            //sucursal = int.Parse(lb_sucursal.SelectedValue);
            ArrayList arr_sucursales = new ArrayList();
            for (int a = 0; a < lb_sucursal.Items.Count; a++)
            {
                if (lb_sucursal.Items[a].Selected == true)
                {
                    arr_sucursales.Add(lb_sucursal.Items[a].Value);
                }
            }

            strSucursales = string.Join(",", arr_sucursales.ToArray());

            serie_factura = lb_facturas.SelectedValue.ToString();
            serie_recibo = lb_recibos.SelectedValue.ToString();
            string incluir_dias_credito = rb_dias_credito.Checked.ToString();
            //Fin filtros APL

            #region Registrar Log de Reportes
            RE_GenericBean Bean_Log = new RE_GenericBean();
            Bean_Log.intC1 = user.PaisID;
            Bean_Log.strC1 = "ANTIGUEDAD SALDOS";
            Bean_Log.strC2 = user.ID;
            Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
            string mensaje_log = "";
            mensaje_log += "Tipo .: " + lb_tipopersona.SelectedItem.Text + ", ";
            mensaje_log += "Codigo .: " + tb_agenteID.Text + ", ";
            mensaje_log += "Moneda.: " + lb_moneda.SelectedItem.Text + " Contabilidad.: " + lb_contabilidad.SelectedItem.Text + " ,";
            mensaje_log += "Fecha Corte.: " + tb_fechacorte.Text + " ,";
            mensaje_log += "Consolidar Moneda.: " + consolidaMoneda + " ";
            Bean_Log.strC4 = mensaje_log;
            DB.Insertar_Log_Reportes(Bean_Log);
            #endregion

            string es_coloader = "no";
            if (chk_coloader.Checked == true)
            {
                es_coloader = "si";
            }

            string mensaje = "<script languaje=\"JavaScript\">";
            mensaje += "window.open('reports.aspx?credito=" + cb_credito.SelectedItem.Text.ToString() + "&cobrador=" + lb_cobrador.SelectedValue + "&reptype=4&fechacorte=" + tb_fechacorte.Text + "&tipopersona=" + lb_tipopersona.SelectedValue + "&monID=" + lb_moneda.SelectedValue + "&contaID=" + lb_contabilidad.SelectedValue + "&cliID=" + tb_agenteID.Text + "&orderby=" + lb_orderby.SelectedValue + "&antrecibos=" + rb_recibos_ant.Checked.ToString() + "&incluir_dias_credito=" + incluir_dias_credito + "&sucursal_id=" + strSucursales + "&serie_factura=" + serie_factura + "&serie_recibo=" + serie_recibo + "&es_coloader=" + es_coloader + "&tb_Tipo=" + tb_Tipo.Text + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            mensaje += "</script>";
            Page.RegisterClientScriptBlock("closewindow", mensaje);
        }

    }
    protected void lb_tipopersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_tipopersona.SelectedValue == "3")
        {
            lbl_coloader.Visible = true;
            chk_coloader.Visible = true;
        }
        else
        {
            lbl_coloader.Visible = false;
            chk_coloader.Visible = false;
            chk_coloader.Checked = false;
        }
    }
}
