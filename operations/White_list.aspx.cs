using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class operations_a : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ListItem item = null;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
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
            Obtengo_Listas();
        }
    }
    protected void Obtengo_Listas()
    {
        arr = (ArrayList)DB.getPaises("");
        item = new ListItem("Seleccione...", "0");
        drp_empresas.Items.Add(item);
        drp_empresas2.Items.Add(item);
        drp_empresas3.Items.Add(item);
        drp_sucursales.Items.Add(item);
        drp_sucursales2.Items.Add(item);
        drp_sucursales3.Items.Add(item);
        foreach (PaisBean pais in arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_empresas.Items.Add(item);
            drp_empresas2.Items.Add(item);
            drp_empresas3.Items.Add(item);
        }
        drp_empresas.SelectedIndex = 0;
        drp_empresas2.SelectedIndex = 0;
        drp_empresas3.SelectedIndex = 0;
    }
    protected void drp_empresas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresas.SelectedValue != "0")
        {
            drp_sucursales.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_sucursales.Items.Add(item);
            arr = DB.getSucursales(" and suc_pai_id=" + drp_empresas.SelectedValue + "  ");
            foreach (SucursalBean bean in arr)
            {
                item = new ListItem(bean.Nombre, bean.ID.ToString());
                drp_sucursales.Items.Add(item);
            }
            drp_sucursales.SelectedIndex = 0;
            drp_tipo_persona.SelectedIndex = 0;
            tb_id.Text = "";
            gv_asignar_whitelist.DataBind();
        }
        else
        {
            drp_sucursales.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_sucursales.Items.Add(item);
            drp_sucursales.SelectedIndex = 0;
            drp_tipo_persona.SelectedIndex = 0;
            tb_id.Text = "";
            gv_asignar_whitelist.DataBind();
        }
    }
    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        string Script = "";
        if (drp_empresas.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Empresa');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_sucursales.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Sucursal');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_tipo_persona.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar el Tipo de Persona');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (tb_id.Text.Trim().Equals(""))
        {
            Script = "alert('Debe ingresar el ID a buscar');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        ArrayList Datos = (ArrayList)DB.Validar_WhiteListXSucursal(user, int.Parse(tb_id.Text), int.Parse(drp_tipo_persona.SelectedValue), int.Parse(drp_empresas.SelectedValue), int.Parse(drp_sucursales.SelectedValue));

        int c = 0;
        if (Datos != null)
        {
            c = Datos.Count;
        }
        
        if (c == 0)
        {
            Script = "alert('Existio un error al tratar de buscar los registros');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        else
        {
            #region Cargar Datos
            DataTable dt = new DataTable();
            dt.Columns.Add("WHITELIST");
            dt.Columns.Add("TIPOWHITELIST");
            dt.Columns.Add("ID");
            dt.Columns.Add("NOMBRE");
            dt.Columns.Add("SUGERIDOPOR");
            dt.Columns.Add("USUARIO");
            dt.Columns.Add("FECHA");
            dt.Columns.Add("IDWHITELIST");
            foreach (RE_GenericBean Bean in Datos)
            {
                if (Bean.strC1 == "0")
                {
                    object[] Obj = { "False", Bean.strC7, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC1 };
                    dt.Rows.Add(Obj);
                }
                else
                {
                    object[] Obj = { "True", Bean.strC7, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC1 };
                    dt.Rows.Add(Obj);
                }
            }
            gv_asignar_whitelist.DataSource = dt;
            gv_asignar_whitelist.DataBind();
            dt.Rows.Clear();
            #endregion
            #region Marcar Datos
            Label lb1, lb2;
            CheckBox chk;
            TextBox Tb1;
            DropDownList drp;
            foreach (GridViewRow row in gv_asignar_whitelist.Rows)
            {
                chk = (CheckBox)row.FindControl("chk_asignar");
                lb1 = (Label)row.FindControl("lbl_idwhitelist");
                lb2 = (Label)row.FindControl("lbl_tipo_whitelist_id");
                Tb1 = (TextBox)row.FindControl("tb_sugeridopor");
                drp = (DropDownList)row.FindControl("drp_tipo");
                #region Cargar Tipos WL
                item = new ListItem("Seleccione...", "0");
                drp.Items.Clear();
                drp.Items.Add(item);
                ArrayList Arr_Tipos = (ArrayList)DB.Get_Tipo_WhiteList();
                foreach (RE_GenericBean Bean_Tipos in Arr_Tipos)
                {
                    item = new ListItem(Bean_Tipos.strC2, Bean_Tipos.strC1);
                    drp.Items.Add(item);
                }
                drp.SelectedValue = lb2.Text;
                #endregion
                if (lb1.Text == "0")
                {
                    chk.Checked = false;
                    Tb1.ReadOnly = false;
                    drp.Enabled = true;
                }
                else
                {
                    chk.Checked = true;
                    Tb1.ReadOnly = true;
                    drp.Enabled = false;
                }
            }
            #endregion
        }
    }
    protected void drp_tipo_persona_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_id.Text = "";
        gv_asignar_whitelist.DataBind();
    }
    protected void tb_id_TextChanged(object sender, EventArgs e)
    {
        if (tb_id.Text == "")
        {
            gv_asignar_whitelist.DataBind();
        }
    }
    protected void btn_asignar_Click(object sender, EventArgs e)
    {
        string Script = "";
        if (drp_empresas.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Empresa');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_sucursales.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Sucursal');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_tipo_persona.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar el Tipo de Persona');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (tb_id.Text.Trim().Equals(""))
        {
            Script = "alert('Debe ingresar el ID a buscar');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        Label lb1, lb2;
        CheckBox chk;
        TextBox Tb1;
        DropDownList drp;
        foreach (GridViewRow row in gv_asignar_whitelist.Rows)
        {
            chk = (CheckBox)row.FindControl("chk_asignar");
            lb1 = (Label)row.FindControl("lbl_idwhitelist");
            lb2 = (Label)row.FindControl("lbl_whitelist");
            Tb1 = (TextBox)row.FindControl("tb_sugeridopor");
            drp = (DropDownList)row.FindControl("drp_tipo");
            if (chk.Checked.ToString() == lb2.Text)
            {
                return;
            }
            else
            {
                string texto = "{\"id_pais\":\"" + drp_empresas.SelectedValue + "\",\"id_cliente\":\"" + tb_id.Text.Trim() + "\",\"tipo_persona\":\"" + drp_tipo_persona.SelectedValue + "\",\"id_sucursal\":\"" + drp_sucursales.SelectedValue + "\",\"id_usuario\":\"" + user.ID + "\",\"sugerido_por\":\"" + Tb1.Text.Trim() + "\",\"id_tipo_whitelist\":\"" + drp.SelectedValue + "\",\"fecha_temporal\":\"";

                string SQL = "";
                int result = 0, c = 0;
                if ((chk.Checked == true) && (lb2.Text == "False"))
                {
                    #region Validar Sugerido Por
                    if ((Tb1.Text.Trim().Equals("")) || (Tb1.Text.Trim().Equals("-")))
                    {
                        Script = "alert('Porfavor ingrese el nombre de la persona que solicita el WhiteList');";
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                                   UpdatePanel1.GetType(),
                                                   "BAW",
                                                   Script,
                                                   true);
                        return;
                    }
                    #endregion
                    #region Validar Tipo de WhiteList
                    if (drp.SelectedValue == "0")
                    {
                        Script = "alert('Porfavor seleccione el tipo de WhiteList');";
                        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                                   UpdatePanel1.GetType(),
                                                   "BAW",
                                                   Script,
                                                   true);
                        return;
                    }
                    #endregion
                    #region Activar WL
                    if (drp.SelectedValue == "1")
                    {
                        SQL = "insert into whitelist(id_pais, id_cliente, tipo_persona, id_sucursal, id_usuario, sugerido_por, id_tipo_whitelist, fecha_temporal) values (" + drp_empresas.SelectedValue + ", " + tb_id.Text.Trim() + ", " + drp_tipo_persona.SelectedValue + ", " + drp_sucursales.SelectedValue + ", '" + user.ID + "', '" + Tb1.Text.Trim() + "', " + drp.SelectedValue + ", '2000-01-01');";
                        texto += "2000-01-01" + "\"}";
                    }
                    else if (drp.SelectedValue == "2")
                    {
                        SQL = "insert into whitelist(id_pais, id_cliente, tipo_persona, id_sucursal, id_usuario, sugerido_por, id_tipo_whitelist, fecha_temporal) values (" + drp_empresas.SelectedValue + ", " + tb_id.Text.Trim() + ", " + drp_tipo_persona.SelectedValue + ", " + drp_sucursales.SelectedValue + ", '" + user.ID + "', '" + Tb1.Text.Trim() + "', " + drp.SelectedValue + ", current_date);";
                        texto += DateTime.Now + "\"}";
                    }
                    result = DB.Actualizar_WhiteList(user, SQL);

                    c = DB.log_admin(user.ID, "", "Actualizar_WhiteList", 1, int.Parse(drp_empresas.SelectedValue), "Asignar Activar", texto); 

                    #endregion
                }
                else if ((chk.Checked == false) && (lb2.Text == "True"))
                {
                    #region Desactivar WL
                    SQL = "update whitelist set estado=0, fecha_asignacion=now() where id=" + lb1.Text + " and id_pais=" + drp_empresas.SelectedValue + " and id_cliente=" + tb_id.Text.Trim() + " and tipo_persona=" + drp_tipo_persona.SelectedValue + " and id_sucursal=" + drp_sucursales.SelectedValue + "";
                    result = DB.Actualizar_WhiteList(user, SQL);

                    c = DB.log_admin(user.ID, "", "Actualizar_WhiteList", 2, int.Parse(drp_empresas.SelectedValue), "Asignar Desactivar", texto); 

                    #endregion
                }
                if (result == -100)
                {
                    Script = "alert('Existio un error al tratar de Actualizar los registros');";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                               UpdatePanel1.GetType(),
                                               "BAW",
                                               Script,
                                               true);
                    return;
                }
                else
                {
                    Limpiar();
                    Script = "alert('WhiteList Actualizado');";
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                               UpdatePanel1.GetType(),
                                               "BAW",
                                               Script,
                                               true);
                    return;
                }
            }
        }
    }
    protected void Limpiar()
    {
        drp_empresas.SelectedIndex = 0;
        drp_empresas3.SelectedIndex = 0;
        drp_sucursales.Items.Clear();
        drp_sucursales3.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sucursales.Items.Add(item);
        drp_sucursales3.Items.Add(item);
        drp_tipo_persona.SelectedIndex = 0;
        tb_id.Text = "";
        gv_asignar_whitelist.DataBind();

        drp_tipo3.SelectedValue = "0";
        tb_sugerido_por3.Text = "";
        drp_tipo_persona3.SelectedValue = "0";
        tb_id_3.Text = "";
        gv_asignar_whitelist4.DataBind();
        gv_white_list_multiple.DataBind();
    }
    protected void drp_sucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_tipo_persona.SelectedIndex = 0;
        tb_id.Text = "";
        gv_asignar_whitelist.DataBind();
    }
    protected void drp_empresas2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresas2.SelectedValue != "0")
        {
            drp_sucursales2.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_sucursales2.Items.Add(item);
            arr = DB.getSucursales(" and suc_pai_id=" + drp_empresas2.SelectedValue + "  ");
            foreach (SucursalBean bean in arr)
            {
                item = new ListItem(bean.Nombre, bean.ID.ToString());
                drp_sucursales2.Items.Add(item);
            }
            drp_sucursales2.SelectedIndex = 0;
            drp_tipo_persona2.SelectedIndex = 0;
            //gv_whitelist.DataBind();
        }
        else
        {
            drp_sucursales2.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_sucursales2.Items.Add(item);
            drp_sucursales2.SelectedIndex = 0;
            drp_tipo_persona2.SelectedIndex = 0;
            //gv_whitelist.DataBind();
        }
    }
    protected void drp_sucursales2_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_tipo_persona2.SelectedIndex = 0;
        //gv_whitelist.DataBind();
    }
    protected void btn_visualizar_Click(object sender, EventArgs e)
    {
        string Script = "";
        if (drp_empresas2.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Empresa');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_sucursales2.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Sucursal');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                       UpdatePanel2.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        //string mensaje = "<script languaje=\"JavaScript\">";
        //mensaje += "window.open('WhiteList.aspx?eid=" + drp_empresas2.SelectedValue + "&sucid=" + drp_sucursales2.SelectedValue + "&tpiid=" + drp_tipo_persona2.SelectedValue + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        //mensaje += "</script>";
        //Page.RegisterClientScriptBlock("closewindow", mensaje);


        Script = "window.open('../Reports/WhiteList.aspx?eid=" + drp_empresas2.SelectedValue + "&sucid=" + drp_sucursales2.SelectedValue + "&tpiid=" + drp_tipo_persona2.SelectedValue + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2,
                                   UpdatePanel2.GetType(),
                                   "BAW",
                                   Script,
                                   true);
        //Response.Write("<SCRIPT language=javascript>var w=window.open('../reports/WhiteList.aspx?eid=" + drp_empresas2.SelectedValue + "&sucid=" + drp_sucursales2.SelectedValue + "&tpiid=" + drp_tipo_persona2.SelectedValue + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');</SCRIPT>");
    }
    private void Page_Unload(object sender, EventArgs e)
    {
        #region Clear Report Objects
        if (rpt != null)
        {
            rpt.Close();
            rpt.Dispose();
            GC.Collect();
        }
        #endregion
    }
    protected void drp_empresas3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresas3.SelectedValue != "0")
        {
            drp_sucursales3.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_sucursales3.Items.Add(item);
            arr = DB.getSucursales(" and suc_pai_id=" + drp_empresas3.SelectedValue + "  ");
            foreach (SucursalBean bean in arr)
            {
                item = new ListItem(bean.Nombre, bean.ID.ToString());
                drp_sucursales3.Items.Add(item);
            }
            drp_sucursales3.SelectedIndex = 0;
            drp_tipo_persona3.SelectedIndex = 0;
            tb_id_3.Text = "";
            gv_white_list_multiple.DataBind();
        }
        else
        {
            drp_sucursales3.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_sucursales3.Items.Add(item);
            drp_sucursales3.SelectedIndex = 0;
            drp_tipo_persona3.SelectedIndex = 0;
            tb_id_3.Text = "";
            gv_white_list_multiple.DataBind();
        }
    }
    protected void btn_buscar_3_Click(object sender, EventArgs e)
    {
        string Script = "";
        if (drp_empresas3.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Empresa');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_sucursales3.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Sucursal');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_tipo_persona3.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar el Tipo de Persona');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (tb_id_3.Text.Trim().Equals(""))
        {
            Script = "alert('Debe ingresar el ID a buscar');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        ArrayList Datos = (ArrayList)DB.Validar_WhiteListXSucursal(user, int.Parse(tb_id_3.Text), int.Parse(drp_tipo_persona3.SelectedValue), int.Parse(drp_empresas3.SelectedValue), int.Parse(drp_sucursales3.SelectedValue));
        if (Datos == null)
        {
            Script = "alert('Existio un error al tratar de buscar los registros');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        else
        {
            #region Cargar Datos
            DataTable dt = new DataTable();
            dt.Columns.Add("WHITELIST");
            dt.Columns.Add("TIPOWHITELIST");
            dt.Columns.Add("ID");
            dt.Columns.Add("NOMBRE");
            dt.Columns.Add("SUGERIDOPOR");
            dt.Columns.Add("USUARIO");
            dt.Columns.Add("FECHA");
            dt.Columns.Add("IDWHITELIST");
            foreach (RE_GenericBean Bean in Datos)
            {
                if (Bean.strC1 == "0")
                {
                    object[] Obj = { "False", Bean.strC7, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC1 };
                    dt.Rows.Add(Obj);
                }
                else
                {
                    object[] Obj = { "True", Bean.strC7, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC1 };
                    dt.Rows.Add(Obj);
                }
            }
            gv_asignar_whitelist4.DataSource = dt;
            gv_asignar_whitelist4.DataBind();
            dt.Rows.Clear();
            #endregion
            #region Marcar Datos
            Label lb1, lb2;
            CheckBox chk;
            TextBox Tb1;
            DropDownList drp;
            foreach (GridViewRow row in gv_asignar_whitelist4.Rows)
            {
                //chk = (CheckBox)row.FindControl("chk_asignar4");
                lb1 = (Label)row.FindControl("lbl_id4");
                lb2 = (Label)row.FindControl("lbl_tipo_whitelist_id4");
                Tb1 = (TextBox)row.FindControl("tb_sugeridopor4");
                drp = (DropDownList)row.FindControl("drp_tipo4");
                #region Cargar Tipos WL
                item = new ListItem("Seleccione...", "0");
                drp.Items.Clear();
                drp.Items.Add(item);
                ArrayList Arr_Tipos = (ArrayList)DB.Get_Tipo_WhiteList();
                foreach (RE_GenericBean Bean_Tipos in Arr_Tipos)
                {
                    item = new ListItem(Bean_Tipos.strC2, Bean_Tipos.strC1);
                    drp.Items.Add(item);
                }
                drp.SelectedValue = lb2.Text;
                #endregion
                if (lb1.Text == "0")
                {
                    //chk.Checked = false;
                    Tb1.ReadOnly = true;
                    drp.Enabled = false;
                }
                else
                {
                    //chk.Checked = true;
                    Tb1.ReadOnly = true;
                    drp.Enabled = false;
                }
            }
            #endregion
        }
    }
    protected void gv_asignar_whitelist4_SelectedIndexChanged(object sender, EventArgs e)
    {
        //int index = e.RowIndex;
        //int id_configuracion = 0;
        //id_configuracion = int.Parse(gv_editar_configuraciones.Rows[index].Cells[2].Text.ToString());
        //int resultado = 0;
        Label lb1, lb2, lb3;
        GridViewRow row = gv_asignar_whitelist4.SelectedRow;
        lb1 = (Label)row.FindControl("lbl_whitelist4");
        lb2 = (Label)row.FindControl("lbl_id4");
        lb3 = (Label)row.FindControl("lbl_nombre4");
        if (lb1.Text == "True")
        {
            string Script = "alert('El Codigo ya se encuentra en WhiteList.');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NO.");
            dt.Columns.Add("ID");
            dt.Columns.Add("NOMBRE");
            #region Obtener Codigos ya agregados
            int correlativo = 0;
            GridViewRowCollection gvrc = gv_white_list_multiple.Rows;
            foreach (GridViewRow rw in gvrc)
            {
                correlativo++;
                object[] ObjArr1 = { correlativo.ToString(), rw.Cells[2].Text, rw.Cells[3].Text };
                dt.Rows.Add(ObjArr1);
            }
            #endregion
            #region Agregar nuevo Codigo
            correlativo++;
            object[] ObjArr2 = { correlativo.ToString(), lb2.Text, lb3.Text };
            dt.Rows.Add(ObjArr2);
            gv_white_list_multiple.DataSource = dt;
            gv_white_list_multiple.DataBind();
            #endregion
            #region Limpiar
            gv_asignar_whitelist4.DataBind();
            tb_id_3.Text = "";
            tb_id_3.Focus();
            #endregion
        }
    }
    protected void gv_white_list_multiple_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = e.RowIndex;
        DataTable dt = new DataTable();
        dt.Columns.Add("NO.");
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        #region Obtener Codigos ya agregados
        int correlativo = 0;
        GridViewRowCollection gvrc = gv_white_list_multiple.Rows;
        foreach (GridViewRow rw in gvrc)
        {
            correlativo++;
            object[] ObjArr1 = { correlativo.ToString(), rw.Cells[2].Text, rw.Cells[3].Text };
            dt.Rows.Add(ObjArr1);
        }
        dt.Rows[index].Delete();
        gv_white_list_multiple.DataSource = dt;
        gv_white_list_multiple.DataBind();
        #endregion
    }
    protected void btn_limpiar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/operations/White_list.aspx");
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        string Script = "";
        #region Validaciones
        if (drp_empresas3.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Empresa');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_sucursales3.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar una Sucursal');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_tipo3.SelectedValue == "0")
        {
            Script = "alert('Porfavor seleccione el tipo de WhiteList');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if ((tb_sugerido_por3.Text.Trim().Equals("")) || (tb_sugerido_por3.Text.Trim().Equals("-")))
        {
            Script = "alert('Porfavor ingrese el nombre de la persona que solicita el WhiteList');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        if (drp_tipo_persona3.SelectedValue == "0")
        {
            Script = "alert('Debe seleccionar el Tipo de Persona');";
            ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                       UpdatePanel1.GetType(),
                                       "BAW",
                                       Script,
                                       true);
            return;
        }
        #endregion
        foreach (GridViewRow row in gv_white_list_multiple.Rows)
        {
            string empresaID = "";
            string sucursalID = "";
            string Tipo_WhiteList = "";
            string SugeridoPor = "";
            string Tipo_Persona = "";
            string personaID = "";
            string usuarioID = "";

            empresaID = drp_empresas3.SelectedValue.ToString();
            sucursalID = drp_sucursales3.SelectedValue.ToString();
            Tipo_WhiteList = drp_tipo3.SelectedValue.ToString();
            SugeridoPor = tb_sugerido_por3.Text.Trim();
            Tipo_Persona = drp_tipo_persona3.SelectedValue.ToString();
            personaID = row.Cells[2].Text.ToString();
            usuarioID = user.ID;

            string texto = "{\"id_pais\":\"" + empresaID + "\",\"id_cliente\":\"" + personaID + "\",\"tipo_persona\":\"" + Tipo_Persona + "\",\"id_sucursal\":\"" + sucursalID + "\",\"id_usuario\":\"" + usuarioID + "\",\"sugerido_por\":\"" + SugeridoPor + "\",\"id_tipo_whitelist\":\"" + Tipo_WhiteList + "\",\"fecha_temporal\":\"";

            string SQL = "";
            #region Activar WL
            if (Tipo_WhiteList == "1")
            {
                SQL = "insert into whitelist(id_pais, id_cliente, tipo_persona, id_sucursal, id_usuario, sugerido_por, id_tipo_whitelist, fecha_temporal)";
                SQL += "values (" + empresaID + ", " + personaID + ", " + Tipo_Persona + ", " + sucursalID + ", '" + usuarioID + "', '" + SugeridoPor + "', " + Tipo_WhiteList + ", '2000-01-01');";
                texto += "2000-01-01" + "\"}";
            }
            else if (Tipo_WhiteList == "2")
            {
                SQL = "insert into whitelist(id_pais, id_cliente, tipo_persona, id_sucursal, id_usuario, sugerido_por, id_tipo_whitelist, fecha_temporal)";
                SQL += "values (" + empresaID + ", " + personaID + ", " + Tipo_Persona + ", " + sucursalID + ", '" + usuarioID + "', '" + SugeridoPor + "', " + Tipo_WhiteList + ", current_date);";
                texto += DateTime.Now + "\"}";
            }
            int result = DB.Actualizar_WhiteList(user, SQL);

            int c = DB.log_admin(user.ID, "", "Actualizar_WhiteList", 1, int.Parse(empresaID), "Guardar Activar", texto); 

            #endregion
            
            if (result == -100)
            {
                Script = "alert('Existio un error al tratar de Actualizar los registros');";
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                           UpdatePanel1.GetType(),
                                           "BAW",
                                           Script,
                                           true);
                return;
            }
        }
        Limpiar();
        Script = "alert('Los codigos fueron agregados exitosamente al WhiteList');";
        ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1,
                                   UpdatePanel1.GetType(),
                                   "BAW",
                                   Script,
                                   true);
        return;
    }
}