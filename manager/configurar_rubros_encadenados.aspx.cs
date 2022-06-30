using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_configurar_rubros_encadenados : System.Web.UI.Page
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
        drp_tipo_servicio.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_servicio.Items.Add(item);
        Arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_servicio");
        foreach (RE_GenericBean rgb in Arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_tipo_servicio.Items.Add(item);
        }
        drp_tipo_servicio.SelectedIndex = 1;
        drp_rubros.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_rubros.Items.Add(item);
        drp_tipo_servicio.SelectedIndex = 0;
        drp_rubros.SelectedIndex = 1;
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
            gv_rubros_encadenados.DataBind();
        }
    }
    protected void drp_empresa_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_sistema.SelectedIndex = 0;
        item = new ListItem("Seleccione...", "0");
        drp_tipo_operacion.Items.Clear();
        drp_tipo_operacion.Items.Add(item);
        drp_tipo_operacion.SelectedIndex = 0;
    }
    protected void drp_tipo_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_rubros.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_rubros.Items.Add(item);
        ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(drp_tipo_servicio.SelectedValue), "");
        foreach (RE_GenericBean bean in rubros)
        {
            item = new ListItem(bean.strC1 + " - " + bean.intC1.ToString(), bean.intC1.ToString());
            drp_rubros.Items.Add(item);
        }
        drp_rubros.SelectedIndex = 0;
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
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
        if (drp_tipo_servicio.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Servicio");
            return;
        }
        if (drp_rubros.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Rubro");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.intC1 = int.Parse(drp_empresa.SelectedValue);
        Bean.intC2 = int.Parse(drp_sistema.SelectedValue);
        Bean.intC3 = int.Parse(drp_tipo_operacion.SelectedValue);
        Bean.intC4 = int.Parse(drp_tipo_servicio.SelectedValue);
        Bean.intC5 = int.Parse(drp_rubros.SelectedValue);
        int resultado = 0;
        resultado = DB.Insertar_Rubros_Generar_Transaccion_Encadenada(user, Bean);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de Guardar el Rubro");
            return;
        }
        else
        {
            WebMsgBox.Show("Rubro Guardado Exitosamente");
            drp_empresa.Enabled = false;
            drp_sistema.Enabled = false;
            drp_tipo_operacion.Enabled = false;
            drp_tipo_servicio.Enabled = false;
            drp_rubros.Enabled = false;
            btn_guardar.Enabled = false;
            return;
        }
    }
    protected void btn_nuevo_Click(object sender, EventArgs e)
    {
        btn_guardar.Enabled = true;
        drp_empresa.Enabled = true;
        drp_sistema.Enabled = true;
        drp_tipo_operacion.Enabled = true;
        drp_tipo_servicio.Enabled = true;
        drp_rubros.Enabled = true;
        drp_empresa.SelectedIndex = 0;
        drp_sistema.SelectedIndex = 0;
        drp_tipo_operacion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_operacion.Items.Add(item);
        drp_tipo_operacion.SelectedIndex = 0;
        drp_tipo_servicio.SelectedIndex = 0;
        drp_rubros.Items.Clear();
        drp_rubros.Items.Add(item);
        drp_rubros.SelectedIndex = 0;
        gv_rubros_encadenados.DataBind();
    }
    protected void Get_Rubros_Encadenados()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("ID_SERVICIO");
        dt.Columns.Add("ID_RUBRO");
        dt.Columns.Add("SERVICIO");
        dt.Columns.Add("RUBRO");
        ArrayList Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Rubros_Encadenados(user, int.Parse(drp_empresa.SelectedValue), int.Parse(drp_sistema.SelectedValue), int.Parse(drp_tipo_operacion.SelectedValue));
        foreach (RE_GenericBean Bean in Arr)
        {
            object[] ObjArr = { Bean.strC1, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5 };
            dt.Rows.Add(ObjArr);
        }
        gv_rubros_encadenados.DataSource = dt;
        gv_rubros_encadenados.DataBind();
    }
    protected void drp_tipo_operacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa.SelectedValue == "0")
        {
            return;
        }
        if (drp_sistema.SelectedValue == "0")
        {
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            return;
        }
        Get_Rubros_Encadenados();
    }
}