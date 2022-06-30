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

public partial class invoice_searchdoc : System.Web.UI.Page
{
    UsuarioBean user;
    DataTable dt1;
    string serie = "0";//dg
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        string action = Request.QueryString["action"].ToString();
        if (action == "0")
        {
            if (!Page.IsPostBack)
            {
                Label1.Visible = false;
                DDLchose.Visible = false;
                serie = lb_serie.SelectedValue;
                ArrayList arr = null;
                ListItem item = null;
                if ((user.SucursalID == 15) || (user.SucursalID == 20) || (user.SucursalID == 24) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47) || (user.SucursalID == 71))
                {
                    arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, 1, user, 0);//1 porque es el tipo de documento para facturacion
                }
                else
                {
                    arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 1, user, 0);//1 porque es el tipo de documento para facturacion
                }
                foreach (string valor in arr)
                {
                    item = new ListItem(valor, valor);
                    lb_serie.Items.Add(item);
                }
                if (lb_serie.Items.Count == 0) { btn_buscar.Enabled = false; WebMsgBox.Show("No hay serie definida para esta Sucursal"); }
                else { btn_buscar.Enabled = true; }
                Determinar_Tipo_Serie();

                //validar anios
                DateTime fechaActual = DateTime.Today;
                int anio = fechaActual.Year;
                ListItem item2 = null;
                for (int i = anio; i > 2004; i--)
                {
                    item2 = new ListItem(i.ToString(), i.ToString());
                    drp_anios.Items.Add(item2);
                }


            }
        }
        else if (action == "1")
        {
            Label1.Visible = true;
            DDLchose.Visible = true;
            if (DDLchose.SelectedValue == "1")
            {
                Determinar_Tipo_Serie();
            }
            else
            {
                pnl_anios.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                //validar anios
                DateTime fechaActual = DateTime.Today;
                int anio = fechaActual.Year;
                ListItem item3 = null;
                for (int i = anio; i > 2004; i--)
                {
                    item3 = new ListItem(i.ToString(), i.ToString());
                    drp_anios.Items.Add(item3);
                }
            }
            //drp_tipo_persona.Enabled = false;
            //drp_tipo_persona.SelectedValue = "3";
        }
        else if (action == "2")
        {
            if (DDLchose.SelectedValue == "1")
            {
                Determinar_Tipo_Serie();
            }
            else
            {
                pnl_anios.Visible = false;
            }
            drp_tipo_persona.Enabled = false;
            drp_tipo_persona.SelectedValue = "3";

            if (!Page.IsPostBack)
            {
                //validar anios
                DateTime fechaActual = DateTime.Today;
                int anio = fechaActual.Year;
                ListItem item4 = null;
                for (int i = anio; i > 2004; i--)
                {
                    item4 = new ListItem(i.ToString(), i.ToString());
                    drp_anios.Items.Add(item4);
                }
            }
        }

        

        //--------dg-----
        #region backup

        //string tipo;
        //tipo = DDLchose.SelectedValue;
        //int ttipo;
        //ttipo = Int32.Parse(tipo.ToString());
        ////  lb_serie.Items.Clear();
        //serie = lb_serie.SelectedValue;
        //lb_serie.Items.Clear();
        ////  gv_facturas.DataBind();
        //if (tipo == "1")
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        serie = lb_serie.SelectedValue;
        //        //  lb_serie.Items.Clear();
        //        ArrayList arr = null;
        //        ListItem item = null;
        //        if ((user.SucursalID == 15) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47))
        //        {
        //            arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, ttipo, user);//1 porque es el tipo de documento para facturacion
        //        }
        //        else
        //        {
        //            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, ttipo, user);//1 porque es el tipo de documento para facturacion
        //        }
        //        foreach (string valor in arr)
        //        {
        //            item = new ListItem(valor, valor);
        //            lb_serie.Items.Add(item);
        //        }
        //        if (lb_serie.Items.Count == 0) { btn_buscar.Enabled = false; WebMsgBox.Show("No hay serie definida para esta Sucursal"); }
        //        else { btn_buscar.Enabled = true; }
        //    }
        //    if (Page.IsPostBack)
        //    {
        //        serie = lb_serie.SelectedValue;
        //        lb_serie.Items.Clear();
        //        //  gv_facturas.DataBind();
        //        ArrayList arr = null;
        //        ListItem item = null;
        //        if ((user.SucursalID == 15) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47))
        //        {
        //            arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, ttipo, user);//1 porque es el tipo de documento para facturacion
        //        }
        //        else
        //        {
        //            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, ttipo, user);//1 porque es el tipo de documento para facturacion
        //        }
        //        foreach (string valor in arr)
        //        {
        //            item = new ListItem(valor, valor);
        //            lb_serie.Items.Add(item);
        //        }
        //        if (lb_serie.Items.Count == 0) { btn_buscar.Enabled = false; WebMsgBox.Show("No hay serie definida para esta Sucursal"); }
        //        else { btn_buscar.Enabled = true; }

        //    }

        //}

        //      else if (tipo == "4")
        //      {
        //          if (!Page.IsPostBack)
        //          {
        //              serie = lb_serie.SelectedValue;
        //            //  gv_facturas.DataBind();
        //             //   lb_serie.Items.Clear();
        //              ArrayList arr = null;
        //              ListItem item = null;
        //              if ((user.SucursalID == 15) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47))
        //              {
        //                  arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, ttipo, user);//1 porque es el tipo de documento para nd
        //              }
        //              else
        //              {
        //                  arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, ttipo, user);//1 porque es el tipo de documento para facturacion

        //              }
        //              foreach (string valor in arr)
        //              {
        //                  item = new ListItem(valor, valor);
        //                  lb_serie.Items.Add(item);
        //              }
        //              if (lb_serie.Items.Count == 0) { btn_buscar.Enabled = false; WebMsgBox.Show("No hay serie definida para esta Sucursal"); }
        //              else { btn_buscar.Enabled = true; }
        //          }

        //          if (Page.IsPostBack)
        //          {
        //              serie = lb_serie.SelectedValue;
        //              lb_serie.Items.Clear();
        //              // gv_facturas.DataBind();
        //              ArrayList arr = null;
        //              ListItem item = null;
        //              if ((user.SucursalID == 15) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47))
        //              {
        //                  arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, ttipo, user);//1 porque es el tipo de documento para nd
        //              }
        //              else
        //              {
        //                  arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, ttipo, user);//1 porque es el tipo de documento para facturacion

        //              }
        //              foreach (string valor in arr)
        //              {
        //                  item = new ListItem(valor, valor);
        //                  lb_serie.Items.Add(item);
        //              }
        //              if (lb_serie.Items.Count == 0) { btn_buscar.Enabled = false; WebMsgBox.Show("No hay serie definida para esta Sucursal"); }
        //              else { btn_buscar.Enabled = true; }
        //          }
        //      }
        #endregion

    }
    protected void btn_buscar_Click(object sender, EventArgs e)
    {

        string correlativo = tb_correlativo.Text.Trim();
        string hbl = tb_hbl.Text.Trim();
        string mbl = tb_mbl.Text.Trim();
        string poliza = tb_poliza.Text.Trim();
        string contenedor = tb_contenedor.Text.Trim();
        string routing = tb_routing.Text.Trim();
        string tipo = DDLchose.SelectedValue; 
        serie = lb_serie.SelectedValue; // obtiene la serie seleccionada
        
            //----------------------dg------------ si es factura------------------------
            if (tipo == "1")
            {
                //string sql = "Select tfa_id, tfa_serie, tfa_correlativo, tfa_cli_id, tfa_nit, tfa_nombre, tfa_fecha_emision, CAST(tfa_total AS NUMERIC), tfa_moneda, tfa_hbl, tfa_mbl, tfa_contenedor, tfa_routing, tfa_ordenpo,tra_nombre, tfa_usu_id,tfa_ted_id from tbl_facturacion a left join  tbl_regimen_aduanero b on a.tfa_tra_id=b.tra_id where tfa_pai_id=" + user.PaisID;
                string sql = "Select tfa_id, tfa_serie, tfa_correlativo, tfa_cli_id, tfa_nit, tfa_nombre, tfa_fecha_emision, CAST(tfa_total AS NUMERIC), tfa_moneda, tfa_hbl, tfa_mbl, tfa_contenedor, tfa_routing, tfa_ordenpo,tra_nombre, tfa_usu_id,tfa_ted_id from tbl_facturacion a left join  tbl_regimen_aduanero b on a.tfa_tra_id=b.tra_id where tfa_tpi_id=" + drp_tipo_persona.SelectedValue + " and tfa_pai_id=" + user.PaisID;

                if (!serie.Equals("")) sql += " and tfa_serie='" + serie + "'";
                if (!correlativo.Equals("")) sql += " and tfa_correlativo='" + correlativo + "'";
                if (!hbl.Equals(""))
                {
                    sql += " and tfa_hbl ilike '%" + hbl + "%'";
                }
                if (!mbl.Equals(""))
                {
                    sql += " and tfa_mbl ilike '%" + mbl + "%'";
                }
                if (!poliza.Equals(""))
                {
                    sql += " and tfa_ordenpo ilike '%" + poliza + "%'";
                }
                if (!tb_poliza_seguros.Text.Equals(""))
                {
                    sql += " and tfa_poliza_seguro ilike '%" + tb_poliza_seguros.Text.Trim() + "%'";
                }
                if (!contenedor.Equals(""))
                {
                    sql += " and tfa_contenedor ilike '%" + contenedor + "%'";
                }

                if (!routing.Equals(""))
                {
                    sql += " and tfa_routing ilike '%" + routing + "%'";
                }
                if (!lb_ted_id.SelectedValue.Equals("0"))
                {
                    sql += " and tfa_ted_id=" + lb_ted_id.SelectedValue;
                }
                //*******************MUESTRA SOLO LAS FACTURAS DE SU SUCURSAL A MENOS QUE SEAN CREDITO Y CENTRAL QUE SI PUEDEN VER TODAS.
                if ((user.SucursalID == 15) || (user.SucursalID == 20) || (user.SucursalID == 24) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47))
                {
                    sql += " ";
                }
                else
                {
                    sql += " and tfa_suc_id =" + user.SucursalID;           
                }
                if (!tbCliCod.Text.Trim().Equals(""))
                {
                    sql += " and tfa_cli_id=" + tbCliCod.Text.Trim();
                }
                if (pnl_anios.Visible == true)
                {
                    sql += " and EXTRACT(YEAR FROM tfa_fecha_emision)=" + drp_anios.SelectedValue.ToString() + "  ";
                }
                sql += " and tfa_conta_id=" + int.Parse(Session["Contabilidad"].ToString()) + "  order by tfa_fecha_emision desc,tfa_id asc ";

                DataTable dt = (DataTable)DB.getInvoices(sql);
                gv_facturas.DataSource = dt;
                gv_facturas.DataBind();
            }
            //----------------dg--------------------si es nd------------------------------
            if (tipo == "4")
            {
                string sql = "Select tnd_id, tnd_serie, tnd_correlativo, tnd_cli_id, tnd_nit, tnd_nombre, tnd_fecha_emision, CAST(tnd_total AS NUMERIC), tnd_moneda, tnd_hbl, tnd_mbl, tnd_contenedor, tnd_routing, tnd_usu_id,tnd_ted_id from tbl_nota_debito where tnd_pai_id=" + user.PaisID;

                if (!serie.Equals("")) sql += " and tnd_serie='" + serie + "'";
                if (!correlativo.Equals("")) sql += " and tnd_correlativo='" + correlativo + "'";
                if (!hbl.Equals(""))
                {
                    sql += " and tnd_hbl ilike '%" + hbl + "%'";
                }
                if (!mbl.Equals(""))
                {
                    sql += " and tnd_mbl ilike '%" + mbl + "%'";
                }
                if (!contenedor.Equals(""))
                {
                    sql += " and tnd_contenedor ilike '%" + contenedor + "%'";
                }

                if (!routing.Equals(""))
                {
                    sql += " and tnd_routing ilike '%" + routing + "%'";
                }
                if (!lb_ted_id.SelectedValue.Equals("0"))
                {
                    sql += " and tnd_ted_id=" + lb_ted_id.SelectedValue;
                }
                //*******************MUESTRA SOLO LAS FACTURAS DE SU SUCURSAL A MENOS QUE SEAN CREDITO Y CENTRAL QUE SI PUEDEN VER TODAS.
                if ((user.SucursalID == 15) || (user.SucursalID == 20) || (user.SucursalID == 24) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47))
                {
                    sql += " ";
                }
                else
                {
                    sql += " and tnd_suc_id =" + user.SucursalID;
                }

                if (!tbCliCod.Text.Trim().Equals(""))
                {
                    sql += " and tnd_cli_id=" + tbCliCod.Text.Trim();
                }
                sql += " and tnd_tcon_id=" + int.Parse(Session["Contabilidad"].ToString()) + " order by tnd_fecha_emision desc,tnd_id asc ";


                DataTable dt = (DataTable)DB.getInvoices_nd(sql);
                gv_facturas.DataSource = dt;
                gv_facturas.DataBind();
            }
            else
            {
                if (DDLchose.Visible == false)
                {
                    //string sql = "Select tfa_id, tfa_serie, tfa_correlativo, tfa_cli_id, tfa_nit, tfa_nombre, tfa_fecha_emision, CAST(tfa_total AS NUMERIC), tfa_moneda, tfa_hbl, tfa_mbl, tfa_contenedor, tfa_routing,tfa_ordenpo,tra_nombre, tfa_usu_id,tfa_ted_id from tbl_facturacion a left join  tbl_regimen_aduanero b on a.tfa_tra_id=b.tra_id where tfa_pai_id=" + user.PaisID;
                    string sql = "Select tfa_id, tfa_serie, tfa_correlativo, tfa_cli_id, tfa_nit, tfa_nombre, tfa_fecha_emision, CAST(tfa_total AS NUMERIC), tfa_moneda, tfa_hbl, tfa_mbl, tfa_contenedor, tfa_routing,tfa_ordenpo,tra_nombre, tfa_usu_id,tfa_ted_id from tbl_facturacion a left join  tbl_regimen_aduanero b on a.tfa_tra_id=b.tra_id where tfa_tpi_id=" + drp_tipo_persona.SelectedValue + " and tfa_pai_id=" + user.PaisID;

                    if (!serie.Equals("")) sql += " and tfa_serie='" + serie + "'";
                    if (!correlativo.Equals("")) sql += " and tfa_correlativo='" + correlativo + "'";
                    if (!hbl.Equals(""))
                    {
                        sql += " and tfa_hbl ilike '%" + hbl + "%'";
                    }
                    if (!mbl.Equals(""))
                    {
                        sql += " and tfa_mbl ilike '%" + mbl + "%'";
                    }
                    if (!poliza.Equals(""))
                    {
                        sql += " and tfa_ordenpo ilike '%" + poliza + "%'";
                    }
                    if (!tb_poliza_seguros.Text.Equals(""))
                    {
                        sql += " and tfa_poliza_seguro ilike '%" + tb_poliza_seguros.Text.Trim() + "%'";
                    }
                    if (!contenedor.Equals(""))
                    {
                        sql += " and tfa_contenedor ilike '%" + contenedor + "%'";
                    }

                    if (!routing.Equals(""))
                    {
                        sql += " and tfa_routing ilike '%" + routing + "%'";
                    }
                    if (!lb_ted_id.SelectedValue.Equals("0"))
                    {
                        sql += " and tfa_ted_id=" + lb_ted_id.SelectedValue;
                    }
                    //*******************MUESTRA SOLO LAS FACTURAS DE SU SUCURSAL A MENOS QUE SEAN CREDITO Y CENTRAL QUE SI PUEDEN VER TODAS.
                    if ((user.SucursalID == 15) || (user.SucursalID == 20) || (user.SucursalID == 24) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47))
                    {
                        sql += " ";
                    }
                    else
                    {
                        sql += " and tfa_suc_id =" + user.SucursalID;
                    }

                    if (!tbCliCod.Text.Trim().Equals(""))
                    {
                        sql += " and tfa_cli_id=" + tbCliCod.Text.Trim();
                    }
                    if (pnl_anios.Visible == true)
                    {
                        sql += " and EXTRACT(YEAR FROM tfa_fecha_emision)=" + drp_anios.SelectedValue.ToString() + "  ";
                    }
                    sql += " and tfa_conta_id=" + int.Parse(Session["Contabilidad"].ToString()) + "    order by tfa_fecha_emision desc,tfa_id asc ";
                    DataTable dt = (DataTable)DB.getInvoices(sql);
                    gv_facturas.DataSource = dt;
                    gv_facturas.DataBind();
                }
            
        }
       
    }
    protected void gv_facturas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string action = Request.QueryString["action"].ToString();
        string tipo = DDLchose.SelectedValue;
        string t;
        int index = Convert.ToInt32(e.CommandArgument);
        t = gv_facturas.Rows[index].Cells[1].Text.ToString();
        if (action.Equals("1"))

            //----------------------------------------dg---------------------------------
            if (tipo == "4")
            {
                #region Validaciones Notas de Debito Electronicas de Costa Rica
                if ((user.PaisID == 5) || (user.PaisID == 21))
                {
                    int _ID_ND = 0; ;
                    _ID_ND = int.Parse(gv_facturas.Rows[index].Cells[1].Text);
                    RE_GenericBean Data_Nota_Debito_Temporal = (RE_GenericBean)DB.getNotaDebitoData(_ID_ND);
                    if (Data_Nota_Debito_Temporal.strC50 == "1")
                    {
                        //Serie Electronica
                        if ((Data_Nota_Debito_Temporal.strC1 == "0") || (Data_Nota_Debito_Temporal.strC49 == "-"))
                        {
                            WebMsgBox.Show("No es posible aplicar Nota de Credito a la Nota de Debito Electronica seleccionada, dado que la misma no ha sido Firmada por GTI (Proveedor de Facturas Electronicas) ni tiene un correlativo asignado , por favor intente mas tarde y valide que los Datos del Cliente fueron ingresados correctamente en el Catalogo de Clientes.");
                            return;
                        }
                    }
                }
                #endregion
                Response.Redirect("view_invoice_nd.aspx?ndID=" + gv_facturas.Rows[index].Cells[1].Text);
            }
            else
            {
                #region Validaciones Facturacion Electronica de Costa Rica
                if ((user.PaisID == 5) || (user.PaisID == 21))
                {
                    int _ID_FA = 0;
                    _ID_FA = int.Parse(gv_facturas.Rows[index].Cells[1].Text);
                    RE_GenericBean Data_Factura_Temporal = (RE_GenericBean)DB.getFacturaData(_ID_FA);
                    if (Data_Factura_Temporal.strC50 == "1")
                    {
                        //Serie Electronica
                        if ((Data_Factura_Temporal.strC1 == "0") || (Data_Factura_Temporal.strC49 == "-"))
                        {
                            WebMsgBox.Show("No es posible aplicar Nota de Credito a la Factura Electronica seleccionada, dado que la misma no ha sido Firmada por GTI (Proveedor de Facturas Electronicas) ni tiene un correlativo asignado , por favor intente mas tarde y valide que los Datos del Cliente fueron ingresados correctamente en el Catalogo de Clientes.");
                            return;
                        }
                    }
                }
                #endregion
                Response.Redirect("view_invoice.aspx?factID=" + gv_facturas.Rows[index].Cells[1].Text);
            }
        else if (action.Equals("0"))
            Response.Redirect("verdetallefactura.aspx?factID=" + gv_facturas.Rows[index].Cells[1].Text);
        else if (action.Equals("2"))
            if (tipo == "4")
            {

                Response.Redirect("NC_ajustes_nd.aspx?ndID=" + gv_facturas.Rows[index].Cells[1].Text);
            }
            else
            {
                Response.Redirect("NC_ajustes.aspx?factID=" + gv_facturas.Rows[index].Cells[1].Text);
            }
    }
    protected void gv_facturas_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[1].Visible = false;

    }

    protected void DDLchose_SelectedIndexChanged(object sender, EventArgs e)
    {
        string tipo;
        tipo = DDLchose.SelectedValue;
        int ttipo;
        ttipo = Int32.Parse(tipo.ToString());
        serie = lb_serie.SelectedValue;
        lb_serie.Items.Clear();
        if (tipo != "0")
        {
            if (tipo == "1")
            {
                serie = lb_serie.SelectedValue;
                string action = Request.QueryString["action"].ToString();
                if ((action == "0") || (action == "1"))
                {
                    drp_tipo_persona.Enabled = true;
                    drp_tipo_persona.SelectedValue = "3";
                }
                ArrayList arr = null;
                ListItem item = null;
                if ((user.SucursalID == 15) || (user.SucursalID == 20) || (user.SucursalID == 24) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47))
                {
                    arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, ttipo, user, 0);//1 porque es el tipo de documento para facturacion
                }
                else
                {
                    arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, ttipo, user, 0);//1 porque es el tipo de documento para facturacion
                }
                foreach (string valor in arr)
                {
                    item = new ListItem(valor, valor);
                    lb_serie.Items.Add(item);
                }
                if (lb_serie.Items.Count == 0) { btn_buscar.Enabled = false; WebMsgBox.Show("No hay serie definida para esta Sucursal"); }
                else { btn_buscar.Enabled = true; }
            }

            else if (tipo == "4")
            {
                serie = lb_serie.SelectedValue;
                drp_tipo_persona.Enabled = false;
                drp_tipo_persona.SelectedValue = "3";
                ArrayList arr = null;
                ListItem item = null;
                if ((user.SucursalID == 15) || (user.SucursalID == 20) || (user.SucursalID == 24) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47))
                {
                    arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, ttipo, user, 0);//1 porque es el tipo de documento para nd
                }
                else
                {
                    arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, ttipo, user, 0 );//1 porque es el tipo de documento para facturacion

                }
                foreach (string valor in arr)
                {
                    item = new ListItem(valor, valor);
                    lb_serie.Items.Add(item);
                }
                if (lb_serie.Items.Count == 0) { btn_buscar.Enabled = false; WebMsgBox.Show("No hay serie definida para esta Sucursal"); }
                else { btn_buscar.Enabled = true; }
            }
        }
    }
    protected void gv_facturas_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void lb_serie_SelectedIndexChanged(object sender, EventArgs e)
    {
        Determinar_Tipo_Serie();
    }
    protected void Determinar_Tipo_Serie()
    {
        if (user.PaisID == 1)
        {
            if (lb_serie.Items.Count > 0)
            {
                int Doc_ID = DB.getDocumentoID(user.SucursalID, lb_serie.Text, 1, user);
                RE_GenericBean Bean_Serie = (RE_GenericBean)DB.getFactura(Doc_ID, 1);
                if (Bean_Serie == null)
                {
                    pnl_anios.Visible = false;
                    return;
                }
                else
                {
                    if (Bean_Serie.intC14 == 1)
                    {
                        pnl_anios.Visible = true;
                    }
                    else
                    {
                        pnl_anios.Visible = false;
                    }
                }
            }
            else
            {
                pnl_anios.Visible = false;
            }
        }
        else
        {
            pnl_anios.Visible = false;
        }
    }
}