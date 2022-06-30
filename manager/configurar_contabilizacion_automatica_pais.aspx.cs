using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_configurar_contabilizacion_automatica_pais : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList Arr = null;
    ListItem item = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!IsPostBack)
        {
            Obtengo_Listas();   
        }
    }
    protected void Obtengo_Listas()
    {
        Arr = (ArrayList)DB.getPaises("");
        drp_empresa.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_empresa.Items.Add(item);
        foreach (PaisBean pais in Arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_empresa.Items.Add(item);
        }
        drp_empresa.SelectedIndex = 0;
        Arr = null;
        drp_sistema.Items.Clear();
        drp_tipo_operacion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sistema.Items.Add(item);
        drp_tipo_operacion.Items.Add(item);
        Arr = (ArrayList)DB.getSistemas();
        foreach (RE_GenericBean Bean in Arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_sistema.Items.Add(item);
        }
        drp_sistema.SelectedIndex = 0;
        drp_tipo_operacion.SelectedIndex = 0;

        Arr = null;
        drp_sistema.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sistema.Items.Add(item);
        drp_tipo_operacion.Items.Add(item);
        Arr = (ArrayList)DB.getSistemas();
        foreach (RE_GenericBean Bean in Arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_sistema.Items.Add(item);
        }
        drp_sistema.SelectedIndex = 0;
    }
    protected void drp_sistema_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_sistema.SelectedValue != "0")
        {
            drp_tipo_operacion.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_tipo_operacion.Items.Add(item);
            Arr = null;
            Arr = (ArrayList)DB.getTipo_Operacion();
            foreach (RE_GenericBean Bean in Arr)
            {
                if (Bean.intC2.ToString() == drp_sistema.SelectedValue)
                {
                    item = new ListItem(Bean.strC1, Bean.intC1.ToString());
                    drp_tipo_operacion.Items.Add(item);
                }
            }
            drp_tipo_operacion.SelectedIndex = 0;
        }
    }
    protected void Get_Transacciones_Asociadas()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TCAID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("MBL");
        dt.Columns.Add("HBL");
        dt.Columns.Add("RO");
        dt.Columns.Add("IMP_EXP");
        dt.Columns.Add("TTR");
        dt.Columns.Add("TPIORIGEN");
        dt.Columns.Add("TPIDESTINO");
        dt.Columns.Add("TRANSACCION");
        dt.Columns.Add("OPERACION");
        dt.Columns.Add("IDPADRE");
        Arr = null;
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Transacciones_Asociar(0, "");
        foreach (RE_GenericBean Bean in Arr)
        {
            string MBL = "";
            string HBL = "";
            string IMP_EXP = "";
            if (Bean.boolC1 == true)
            {
                MBL = "Prepaid";
            }
            else
            {
                MBL = "Collect";
            }
            if (Bean.boolC2 == true)
            {
                HBL = "Prepaid";
            }
            else
            {
                HBL = "Collect";
            }
            if (Bean.boolC4 == true)
            {
                IMP_EXP = "Import";
            }
            else
            {
                IMP_EXP = "Export";
            }
            object[] ObjArr = { Bean.intC1.ToString(), Bean.strC1, MBL, HBL, Bean.boolC3.ToString(), IMP_EXP, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.intC9.ToString() };
            dt.Rows.Add(ObjArr);
        }
        gv_transacciones.DataSource = dt;
        gv_transacciones.DataBind();
    }
    protected void btn_visualizar_Click(object sender, EventArgs e)
    {
        if (drp_empresa.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Empresa");
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema");
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Operacion");
            return;
        }
        drp_empresa.Enabled = false;
        drp_sistema.Enabled = false;
        drp_tipo_operacion.Enabled = false;
        Get_Transacciones_Asociadas();
        Marcar_Transacciones();
        gv_transacciones.Visible = true;
    }
    protected void btn_nuevo_Click(object sender, EventArgs e)
    {
        Limpiar();
    }
    protected void btn_asignar_Click(object sender, EventArgs e)
    {
        Arr = null;
        Arr = new ArrayList();
        RE_GenericBean Bean = null;
        CheckBox chk=null;
        DropDownList drp_aux = null;
        GridViewRowCollection gvrc = gv_transacciones.Rows;
        if (drp_empresa.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Empresa");
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Sistema");
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Operacion");
            return;
        }
        foreach (GridViewRow row in gvrc)
        {
            chk = (CheckBox)row.FindControl("chk");
            if (chk.Checked == true)
            {
                Bean = new RE_GenericBean();
                Bean.intC1 = int.Parse(drp_empresa.SelectedValue);
                Bean.intC2 = int.Parse(drp_sistema.SelectedValue);
                Bean.intC3 = int.Parse(drp_tipo_operacion.SelectedValue);
                drp_aux = ((DropDownList)row.FindControl("drp_terceros"));
                if (drp_aux.SelectedValue == "0")
                {
                    WebMsgBox.Show("Debe Indicar si la transaccion es o no de Terceros");
                    return;
                }
                Bean.boolC1 = bool.Parse(((DropDownList)row.FindControl("drp_terceros")).SelectedValue);
                drp_aux = ((DropDownList)row.FindControl("drp_automatizar"));
                if (drp_aux.SelectedValue == "0")
                {
                    WebMsgBox.Show("Debe Indicar si la transaccion se debe automatizar");
                    return;
                }
                Bean.boolC2 = bool.Parse(((DropDownList)row.FindControl("drp_automatizar")).SelectedValue);
                Bean.intC4 = int.Parse(row.Cells[3].Text);
                Arr.Add(Bean);
            }
        }
        string sql = " and tcaco_pai_id=" + drp_empresa.SelectedValue + " and tcaco_sis_id=" + drp_sistema.SelectedValue + " and tcaco_tto_id=" + drp_tipo_operacion.SelectedValue + " ";
        int resultado = DB.Contabilizacion_Automatica_Asociar_Transacciones(user, Arr, sql);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al Tratar de Asociar las Transacciones");
            return;
        }
        else
        {
            WebMsgBox.Show("Las Transacciones fueron Asociadas exitosamente");
            Limpiar();
        }
    }
    protected void Limpiar()
    {
        drp_empresa.Enabled = true;
        drp_sistema.Enabled = true;
        drp_tipo_operacion.Enabled = true;
        drp_tipo_operacion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_operacion.Items.Add(item);
        drp_empresa.SelectedIndex = 0;
        drp_sistema.SelectedIndex = 0;
        drp_tipo_operacion.SelectedIndex = 0;
        gv_transacciones.DataBind();
        gv_transacciones.Visible = false;
    }
    protected void Marcar_Transacciones()
    {
        GridViewRowCollection gvr = gv_transacciones.Rows;
        CheckBox chk = null;
        DropDownList drp1 = null;
        DropDownList drp2 = null;
        Arr = null;
        string sql = " and i.tcaco_pai_id=" + drp_empresa.SelectedValue + " and i.tcaco_sis_id=" + drp_sistema.SelectedValue + " and i.tcaco_tto_id=" + drp_tipo_operacion.SelectedValue + " ";
        Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Transacciones_Asociadas(sql);
        foreach (GridViewRow row in gvr)
        {
            foreach (RE_GenericBean Bean in Arr)
            {
                if (Bean.intC1.ToString() == row.Cells[3].Text)
                {
                    chk = (CheckBox)row.FindControl("chk");
                    drp1 = (DropDownList)row.FindControl("drp_terceros");
                    drp2 = (DropDownList)row.FindControl("drp_automatizar");
                    drp1.SelectedValue = Bean.boolC5.ToString();
                    drp2.SelectedValue = Bean.boolC6.ToString();
                    chk.Checked = true;
                }
            }
        }
    }
}