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

public partial class operations_administrar_rubros : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr_servicios = null;
    ListItem items_servicios = null;
    ArrayList arr_rubros = null;
    ListItem items_rubros = null;
    ArrayList arr_impexp = null;
    ListItem items_impexp = null;
    ArrayList arr_tipotransaccion = null;
    ListItem items_tipotransaccion = null;
    ArrayList arr_tipocobro = null;
    ListItem items_tipocobro = null;
    string sScript = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        else
        {
            user = (UsuarioBean)Session["usuario"];
        }
        if (!Page.IsPostBack)
        {
            arr_servicios = (ArrayList)DB.getServiciosMaster();
            foreach (RE_GenericBean rgb_servicios in arr_servicios)
            {
                items_servicios = new ListItem(rgb_servicios.strC1 + "-" + rgb_servicios.intC1.ToString(), rgb_servicios.intC1.ToString());
                ddl_servicio.Items.Add(items_servicios);
            }
            items_servicios = null;
            arr_rubros = (ArrayList)DB.getRubrosXservicioMaster(int.Parse(ddl_servicio.SelectedValue.ToString()));
            foreach (RE_GenericBean rgb_rubros in arr_rubros)
            {
                items_rubros = new ListItem(rgb_rubros.strC1 + "-" + rgb_rubros.intC1.ToString(), rgb_rubros.intC1.ToString());
                ddl_rubro.Items.Add(items_rubros);
            }
            items_rubros = null;
        }
    }

    protected void ddl_tipo_servico_SelectedIndexChanged(object sender, EventArgs e)
    {
        items_rubros = null;
        ArrayList rubros = new ArrayList();
        ddl_rubro.Items.Clear();
        rubros = (ArrayList)DB.getRubrosXservicioMaster(int.Parse(ddl_servicio.SelectedValue.ToString()));
        foreach (RE_GenericBean rgb_rubros in rubros)
        {
            items_rubros = new ListItem(rgb_rubros.strC1 + "-" + rgb_rubros.intC1.ToString(), rgb_rubros.intC1.ToString());
            ddl_rubro.Items.Add(items_rubros);
        }
        /*gv_combinacionescontables.DataSource = null;
        gv_combinacionescontables.DataBind();
        gv_rubros_pais.DataSource = null;
        gv_rubros_pais.DataBind();*/
        txt_rubro.Text = "";
        mvRubros.ActiveViewIndex = 1;

    }

    protected void btn_crear_Click(object sender, EventArgs e)
    {
        string nombreRubro = txt_rubro.Text;
        int idServicio = int.Parse(ddl_servicio.SelectedValue);
        
        if (txt_rubro.Text.Trim() == "")
        {
            sScript = "alert('Debe de especificar el nombre del rubro a crear');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }

        int idRubro = DB.CreateRubros(nombreRubro, idServicio);

        if (idRubro == -100)
        {
            sScript = "alert('Existio error al crar nuevo rubro');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }

        mvRubros.ActiveViewIndex = 1;
        items_rubros = null;
        ArrayList rubros = new ArrayList();
        ddl_rubro.Items.Clear();
        rubros = (ArrayList)DB.getRubrosXservicioMaster(int.Parse(ddl_servicio.SelectedValue.ToString()));
        foreach (RE_GenericBean rgb_rubros in rubros)
        {
            items_rubros = new ListItem(rgb_rubros.strC1, rgb_rubros.intC1.ToString());
            ddl_rubro.Items.Add(items_rubros);
        }

        ddl_rubro.SelectedValue = idRubro.ToString();
        txt_rubro.Text = "";

        mvRubros.ActiveViewIndex = 0;
        cargarCombinacionesContables();
        cargarConfiguracionRubroPais();

        sScript = "alert('El Rubro "+idRubro.ToString()+"-"+ddl_rubro.SelectedItem.ToString()+" ha sido creado exitosamente.');";
        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                   UpdatePanel1.GetType(),
                                   "BAW",
                                   sScript,
                                   true);

    }

    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        mvRubros.ActiveViewIndex = 0;
        cargarCombinacionesContables();
        cargarConfiguracionRubroPais();
    }

    private void cargarCombinacionesContables()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)DB.getCombinacionesContables(int.Parse(ddl_servicio.SelectedValue), int.Parse(ddl_rubro.SelectedValue));
        gv_combinacionescontables.DataSource = dt;
        gv_combinacionescontables.DataBind();
    }

    private void cargarConfiguracionRubroPais()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)DB.getConfiguracionRubroPais(int.Parse(ddl_rubro.SelectedValue));
        gv_rubros_pais.DataSource = dt;
        gv_rubros_pais.DataBind();
    }

    protected void gv_combinacionescontables_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gv_combinacionescontables.EditIndex = e.NewEditIndex;
        cargarCombinacionesContables();
    }

    protected void gv_rubros_pais_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gv_rubros_pais.EditIndex = e.NewEditIndex;
        cargarConfiguracionRubroPais();
    }

    protected void gv_combinacionescontables_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gv_combinacionescontables.EditIndex = -1;
        cargarCombinacionesContables();
    }
    
    protected void gv_rubros_pais_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gv_rubros_pais.EditIndex = -1;
        cargarConfiguracionRubroPais();
    }

    protected void gv_combinacionescontables_OnDataBound(object sender, EventArgs e)
    {
        DropDownList ddlTipoTransaccion = gv_combinacionescontables.HeaderRow.FindControl("ddl_tipo_transaccion") as DropDownList;
        DropDownList ddlTipoCobro = gv_combinacionescontables.HeaderRow.FindControl("ddl_tipo_cobro") as DropDownList;
        DropDownList ddlTipoImpExp = gv_combinacionescontables.HeaderRow.FindControl("ddl_tipo_impexp") as DropDownList;

        arr_tipotransaccion = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion order by ttt_nombre");
        foreach (RE_GenericBean rgb in arr_tipotransaccion)
        {
            items_tipotransaccion = new ListItem(rgb.strC1+"-"+rgb.intC1.ToString(), rgb.intC1.ToString());
            ddlTipoTransaccion.Items.Add(items_tipotransaccion);

        }

        arr_tipocobro = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_cobro order by ttc_nombre");
        foreach (RE_GenericBean rgb1 in arr_tipocobro)
        {
            items_tipocobro = new ListItem(rgb1.strC1 + "-" + rgb1.intC1.ToString(), rgb1.intC1.ToString());
            ddlTipoCobro.Items.Add(items_tipocobro);

        }

        arr_impexp = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp order by tie_nombre");
        foreach (RE_GenericBean rgb2 in arr_impexp)
        {
            items_impexp = new ListItem(rgb2.strC1 + "-" + rgb2.intC1.ToString(), rgb2.intC1.ToString());
            ddlTipoImpExp.Items.Add(items_impexp);
        }
    }

    protected void gv_combinacionescontables_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_combinacionescontables.EditIndex == e.Row.RowIndex)
        {
            DropDownList ddlTipoTransaccion = (DropDownList)e.Row.FindControl("ddl_tipo_transaccion");
            DropDownList ddlTipoCobro = (DropDownList)e.Row.FindControl("ddl_tipo_cobro");
            DropDownList ddlTipoImpExp = (DropDownList)e.Row.FindControl("ddl_tipo_impexp");

            arr_tipotransaccion = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion order by ttt_nombre");
            foreach (RE_GenericBean rgb in arr_tipotransaccion)
            {
                items_tipotransaccion = new ListItem(rgb.strC1+"-"+rgb.intC1.ToString(), rgb.intC1.ToString());
                ddlTipoTransaccion.Items.Add(items_tipotransaccion);
                
            }
            ddlTipoTransaccion.Items.FindByText((e.Row.FindControl("lbl_tipo_transaccion") as Label).Text).Selected = true;

            arr_tipocobro = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_cobro order by ttc_nombre");
            foreach (RE_GenericBean rgb1 in arr_tipocobro)
            {
                items_tipocobro = new ListItem(rgb1.strC1 + "-" + rgb1.intC1.ToString(), rgb1.intC1.ToString());
                ddlTipoCobro.Items.Add(items_tipocobro);

            }
            ddlTipoCobro.Items.FindByText((e.Row.FindControl("lbl_tipo_cobro") as Label).Text).Selected = true;

            arr_impexp = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp order by tie_nombre");
            foreach (RE_GenericBean rgb2 in arr_impexp)
            {
                items_impexp = new ListItem(rgb2.strC1 + "-" + rgb2.intC1.ToString(), rgb2.intC1.ToString());
                ddlTipoImpExp.Items.Add(items_impexp);
            }
            ddlTipoImpExp.Items.FindByText((e.Row.FindControl("lbl_tipo_impexp") as Label).Text).Selected = true;
        }
    }



    protected void btn_nueva_combinacion_Click(object sender, EventArgs e)
    {
        TextBox txtCuenta = gv_combinacionescontables.HeaderRow.FindControl("txt_cuenta") as TextBox;
        TextBox txtPorDebe = gv_combinacionescontables.HeaderRow.FindControl("txt_debe") as TextBox;
        TextBox txtPorHaber = gv_combinacionescontables.HeaderRow.FindControl("txt_haber") as TextBox;

        if (txtCuenta.Text.Trim() == "")
        {
            sScript = "alert('Debe de especificar la cuenta contable');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,UpdatePanel1.GetType(),"BAW",sScript,true);
            return;
        }
        if (txtPorDebe.Text.Trim() == "")
        {
            sScript = "alert('Debe de especificar el porcentaje para el DEBE');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "BAW", sScript, true);
            return;
        }
        if (txtPorHaber.Text.Trim() == "")
        {
            sScript = "alert('Debe de especificar el porcentaje para el HABER');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, UpdatePanel1.GetType(), "BAW", sScript, true);
            return;
        }
        
        int IdServicio = int.Parse(ddl_servicio.SelectedValue);
        int IdRubro = int.Parse(ddl_rubro.SelectedValue);
        int TipoTransaccion = int.Parse(((DropDownList)gv_combinacionescontables.HeaderRow.FindControl("ddl_tipo_transaccion")).SelectedValue);
        int TipoCobro = int.Parse(((DropDownList)gv_combinacionescontables.HeaderRow.FindControl("ddl_tipo_cobro")).SelectedValue);
        int TipoImpExp = int.Parse(((DropDownList)gv_combinacionescontables.HeaderRow.FindControl("ddl_tipo_impexp")).SelectedValue);
        string CuentaContable = ((TextBox)gv_combinacionescontables.HeaderRow.FindControl("txt_cuenta")).Text;
        int PorDebe = int.Parse(((TextBox)gv_combinacionescontables.HeaderRow.FindControl("txt_debe")).Text);
        int PorHaber = int.Parse(((TextBox)gv_combinacionescontables.HeaderRow.FindControl("txt_haber")).Text);

        int CreacionCombinacionContable = DB.CreateCombinacionContable(TipoTransaccion, IdServicio, IdRubro, TipoCobro, TipoImpExp, CuentaContable, PorDebe, PorHaber);

        if (CreacionCombinacionContable == -100)
        {
            sScript = "alert('Existio error al crar combinacion contable');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }

        if (CreacionCombinacionContable == 100)
        {
            sScript = "alert('La combinacion contable ya existe');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            return;
        }
        
        txtCuenta.Text = "";
        txtPorDebe.Text = "";
        txtPorHaber.Text = "";

        mvRubros.ActiveViewIndex = 0;
        cargarCombinacionesContables();
        cargarConfiguracionRubroPais();
    }


    protected void gv_rubros_pais_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_rubros_pais.EditIndex == e.Row.RowIndex)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ddlValue");
            dt.Columns.Add("ddlItem");
            object[] obj1 = {"0","No" };
            object[] obj2 = {"1","Si" };
            dt.Rows.Add(obj1);
            dt.Rows.Add(obj2);

            DropDownList ddlCobraIva = (DropDownList)e.Row.FindControl("ddl_cobra_iva");
            ddlCobraIva.DataSource = dt;
            ddlCobraIva.DataTextField = "ddlItem";
            ddlCobraIva.DataValueField = "ddlValue";
            ddlCobraIva.DataBind();
            ddlCobraIva.Items.FindByText((e.Row.FindControl("lbl_cobra_iva") as Label).Text).Selected = true;

            DropDownList ddlIvaIncluido = (DropDownList)e.Row.FindControl("ddl_iva_incluido");
            ddlIvaIncluido.DataSource = dt;
            ddlIvaIncluido.DataTextField = "ddlItem";
            ddlIvaIncluido.DataValueField = "ddlValue";
            ddlIvaIncluido.DataBind();
            ddlIvaIncluido.Items.FindByText((e.Row.FindControl("lbl_iva_incluido") as Label).Text).Selected = true;

            DropDownList ddlEsSujeto = (DropDownList)e.Row.FindControl("ddl_es_sujeto");
            ddlEsSujeto.DataSource = dt;
            ddlEsSujeto.DataTextField = "ddlItem";
            ddlEsSujeto.DataValueField = "ddlValue";
            ddlEsSujeto.DataBind();
            ddlEsSujeto.Items.FindByText((e.Row.FindControl("lbl_es_sujeto") as Label).Text).Selected = true;
        }
    }

    protected void gv_combinacionescontables_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int MatrizId = int.Parse(((Label)gv_combinacionescontables.Rows[e.RowIndex].FindControl("lbl_matriz_id")).Text);
        int TipoTransaccion = int.Parse((gv_combinacionescontables.Rows[e.RowIndex].FindControl("ddl_tipo_transaccion") as DropDownList).SelectedItem.Value);
        int TipoCobro = int.Parse((gv_combinacionescontables.Rows[e.RowIndex].FindControl("ddl_tipo_cobro") as DropDownList).SelectedItem.Value);
        int TipoImpExp = int.Parse((gv_combinacionescontables.Rows[e.RowIndex].FindControl("ddl_tipo_impexp") as DropDownList).SelectedItem.Value);
        string CuentaContable = ((TextBox)gv_combinacionescontables.Rows[e.RowIndex].FindControl("txt_cuenta") as TextBox).Text;
        int PorDebe = int.Parse(((TextBox)gv_combinacionescontables.Rows[e.RowIndex].FindControl("txt_debe") as TextBox).Text);
        int porHaber = int.Parse(((TextBox)gv_combinacionescontables.Rows[e.RowIndex].FindControl("txt_haber") as TextBox).Text);

        int actualiza_combinacion = DB.ActualizarCombinacionContable(MatrizId, TipoTransaccion, TipoCobro, TipoImpExp, CuentaContable, PorDebe, porHaber);

        if (actualiza_combinacion == -100)
        {
            sScript = "alert('Ha ocurrido un error al actualizar la combinacion contable, favor contactar al Administrador del Sistema.');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       sScript,
                                       true);

        }

        gv_combinacionescontables.EditIndex = -1;
        cargarCombinacionesContables();
  
    }

    protected void gv_rubros_pais_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string strRubroId = ddl_rubro.SelectedValue;
        string strPaisId = ((Label)gv_rubros_pais.Rows[e.RowIndex].FindControl("lbl_pais_id")).Text; 
        string strCobraIva = (gv_rubros_pais.Rows[e.RowIndex].FindControl("ddl_cobra_iva") as DropDownList).SelectedItem.Value;
        string strIvaIncluido = (gv_rubros_pais.Rows[e.RowIndex].FindControl("ddl_iva_incluido") as DropDownList).SelectedItem.Value;
        string strEsSujeto = (gv_rubros_pais.Rows[e.RowIndex].FindControl("ddl_es_sujeto") as DropDownList).SelectedItem.Value;

        int actualiza = DB.ActualizarConfiguracionRubroPais(strRubroId, strPaisId, strCobraIva, strIvaIncluido, strEsSujeto);

        if (actualiza == -100)
        {
            sScript = "alert('Ha ocurrido un error al actualizar los datos del rubro, favor contactar al Administrador del Sistema.');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       sScript,
                                       true);
            
        }

        gv_rubros_pais.EditIndex = -1;
        cargarConfiguracionRubroPais();
    }

    protected void gv_combinacionescontables_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[0].Visible = false;
        }
    }

    protected void gv_rubros_pais_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[0].Visible = false;
        }
    }

}