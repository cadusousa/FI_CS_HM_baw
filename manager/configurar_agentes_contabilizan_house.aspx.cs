using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_configurar_agentes_contabilizan_house : System.Web.UI.Page
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
        drp_grupos.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_grupos.Items.Add(item);
        Arr = (ArrayList)DB.getGrupos();
        foreach (RE_GenericBean Bean in Arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_grupos.Items.Add(item);
        }
        drp_grupos.SelectedIndex = 0;
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
            gv_agentes_contabilizan_house.DataBind();
        }
    }
    protected void btn_nuevo_Click(object sender, EventArgs e)
    {
        drp_empresa.Enabled = true;
        drp_sistema.Enabled = true;
        drp_tipo_operacion.Enabled = true;
        drp_grupos.Enabled = true;
        drp_empresa.SelectedIndex = 0;
        drp_sistema.SelectedIndex = 0;
        drp_grupos.SelectedIndex = 0;
        drp_tipo_operacion.Items.Clear();
        item = null;
        item = new ListItem("Seleccione...", "0");
        drp_tipo_operacion.Items.Add(item);
        drp_tipo_operacion.SelectedIndex = 0;
        gv_agentes_contabilizan_house.DataBind();
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
        if (drp_grupos.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Grupo de Agentes");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.intC1 = int.Parse(drp_empresa.SelectedValue);
        Bean.intC2 = int.Parse(drp_sistema.SelectedValue);
        Bean.intC3 = int.Parse(drp_tipo_operacion.SelectedValue);
        Bean.intC4 = int.Parse(drp_grupos.SelectedValue);
        int resultado = 0;
        resultado = DB.Insertar_Agentes_Contabilizacion_House(user, Bean);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de configurar el Grupo para Contabilizar por House");
            return;
        }
        else
        {
            WebMsgBox.Show("Grupo Configurado Exitosamente");
            drp_empresa.Enabled = false;
            drp_sistema.Enabled = false;
            drp_tipo_operacion.Enabled = false;
            drp_grupos.Enabled = false;
            btn_guardar.Enabled = false;
            return;
        }
    }
    protected void Get_Agentes_Contabilizan_x_House()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID_GRUPO");
        dt.Columns.Add("GRUPO_AGENTES");
        ArrayList Arr = (ArrayList)DB.Contabilizacion_Automatica_Get_Grupos_Agentes_Contabilizan_House(user, int.Parse(drp_empresa.SelectedValue), int.Parse(drp_sistema.SelectedValue), int.Parse(drp_tipo_operacion.SelectedValue));
        foreach (RE_GenericBean Bean in Arr)
        {
            object[] ObjArr = { Bean.strC2, Bean.strC3 };
            dt.Rows.Add(ObjArr);
        }
        gv_agentes_contabilizan_house.DataSource = dt;
        gv_agentes_contabilizan_house.DataBind();
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
        Get_Agentes_Contabilizan_x_House();
    }
    protected void drp_empresa_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_sistema.SelectedIndex = 0;
        item = new ListItem("Seleccione...", "0");
        drp_tipo_operacion.Items.Clear();
        drp_tipo_operacion.Items.Add(item);
        drp_tipo_operacion.SelectedIndex = 0;
        drp_grupos.SelectedIndex = 0;
    }
}