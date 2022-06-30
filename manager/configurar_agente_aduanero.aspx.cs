using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
public partial class manager_configurar_agente_aduanero : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ListItem item = null;
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
            Obtengo_listas();
        }
    }
    protected void Obtengo_listas()
    {
        arr = (ArrayList)DB.getPaises("");
        item = new ListItem("Seleccione...", "0");
        drp_empresa_cobra.Items.Add(item);
        foreach (PaisBean pais in arr)
        {
            if (pais.Pai_Agente == true)
            {
                item = new ListItem(pais.Nombre, pais.ID.ToString());
                drp_empresa_cobra.Items.Add(item);
            }
        }
        drp_empresa_cobra.SelectedIndex = 0;
        arr = null;
        arr = (ArrayList)DB.getPaises("");
        item = new ListItem("Seleccione...", "0");
        drp_empresa_paga.Items.Add(item);
        foreach (PaisBean pais in arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_empresa_paga.Items.Add(item);
        }
        drp_empresa_paga.SelectedIndex = 0;
        drp_sucursal.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sucursal.Items.Add(item);
        drp_sucursal.SelectedIndex = 0;
        drp_tipo_persona.SelectedIndex = 0;
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_serie.Items.Add(item);
        drp_serie.SelectedIndex = 0;
        arr = null;
        drp_tipo_servicio.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_servicio.Items.Add(item);
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_servicio");
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_tipo_servicio.Items.Add(item);
        }
        drp_tipo_servicio.SelectedIndex = 1;
        drp_rubros.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_servicio.Items.Add(item);
        ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(drp_tipo_servicio.SelectedValue), "");
        RE_GenericBean rubbean = null;
        for (int a = 0; a < rubros.Count; a++)
        {
            rubbean = (RE_GenericBean)rubros[a];
            item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
            drp_rubros.Items.Add(item);
        }
        drp_rubros.SelectedIndex = 1;
        arr = null;
        drp_tipo_transaccion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_transaccion.Items.Add(item);
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion order by ttt_id asc ");
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_tipo_transaccion.Items.Add(item);
        }
        drp_tipo_transaccion.SelectedIndex = 0;
        arr = null;
        drp_empresa_tipo_cambio.Items.Clear();
        arr = (ArrayList)DB.getPaises("");
        item = new ListItem("Seleccione...", "0");
        drp_empresa_tipo_cambio.Items.Add(item);
        foreach (PaisBean pais in arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_empresa_tipo_cambio.Items.Add(item);
        }
        drp_empresa_tipo_cambio.SelectedIndex = 0;
        arr = null;
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
        drp_imp_exp_id.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_imp_exp_id.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_imp_exp_id.Items.Add(item);
        }
        drp_imp_exp_id.SelectedIndex = 0;
        drp_moneda.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_moneda.Items.Add(item);
        drp_moneda.SelectedIndex = 0;
    }
    protected void drp_operacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql="";
        sql = "  fac_suc_id=" + drp_sucursal.SelectedValue + "  and fac_tipo=5 and fac_conta_id=" + drp_contabilidad.SelectedValue + " and fac_operacion_id=" + drp_operacion.SelectedValue + " ";
        arr = null;
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_serie.Items.Add(item);
        arr = (ArrayList)DB.getSeriesByCriterio(user, sql);
        foreach (RE_GenericBean bean in arr)
        {
            item = new ListItem(bean.strC2, bean.strC2);
            drp_serie.Items.Add(item);
        }
        drp_serie.SelectedIndex = 0;
    }
    protected void drp_sucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_serie.Items.Add(item);
        drp_serie.SelectedIndex = 0;
        drp_operacion.SelectedIndex = 0;
    }
    protected void drp_tipo_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_rubros.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_rubros.Items.Add(item);
        ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(drp_tipo_servicio.SelectedValue), "");
        foreach (RE_GenericBean bean in rubros)
        {
            item = new ListItem(bean.strC1, bean.intC1.ToString());
            drp_rubros.Items.Add(item);
        }
        drp_rubros.SelectedIndex = 0;
    }
    protected void btn_grabar_Click(object sender, EventArgs e)
    {
        if (drp_empresa_cobra.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Empresa que realiara el cobro.");
            return;
        }
        else if (drp_empresa_paga.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Empresa que realiara el pago.");
            return;
        }
        else if (drp_empresa_tipo_cambio.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la empresa de la cual se debe obtener el tipo de cambio");
            return;
        }
        else if (drp_sucursal.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Sucursal donde se emitira la Provision");
            return;
        }
        else if (drp_serie.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la serie en la que se emitira la Provision");
            return;
        }
        else if (drp_tipo_persona.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe el tipo de persona a la cual se emitira el documento de pago");
            return;
        }
        else if (tb_codigo_persona.Text == "")
        {
            WebMsgBox.Show("Debe ingresar el codigo de persona a quien se emitira el documento de pago");
            return;
        }
        else if (drp_tipo_transaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el tipo de transaccion de pago");
            return;
        }
        else if (drp_tipo_servicio.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el tipo de servicio de pago");
            return;
        }
        else if (drp_rubros.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar rubro de pago");
            return;
        }
        else if (drp_imp_exp_id.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Tipo de Operacion");
            return;
        }
        else if (drp_moneda.SelectedValue=="0")
        {
            WebMsgBox.Show("Debe seleccionar la moneda de Pago.");
            return;
        }
        RE_GenericBean Bean = new RE_GenericBean();
        Bean.intC1 = int.Parse(drp_empresa_cobra.SelectedValue);
        Bean.intC2 = int.Parse(drp_empresa_paga.SelectedValue);
        Bean.intC3 = int.Parse(drp_sucursal.SelectedValue);
        Bean.intC4 = int.Parse(drp_empresa_tipo_cambio.SelectedValue);
        Bean.intC5 = int.Parse(drp_contabilidad.SelectedValue);
        Bean.strC1 = drp_serie.SelectedItem.Text;
        Bean.intC6 = int.Parse(drp_tipo_persona.SelectedValue);
        Bean.intC7 = int.Parse(tb_codigo_persona.Text);
        Bean.intC8 = int.Parse(drp_tipo_transaccion.SelectedValue);
        Bean.intC9 = int.Parse(drp_tipo_servicio.SelectedValue);
        Bean.intC10 = int.Parse(drp_rubros.SelectedValue);
        Bean.intC11 = int.Parse(lbl_configuracion_id.Text);
        Bean.intC12 = int.Parse(drp_imp_exp_id.SelectedValue);
        Bean.intC13 = int.Parse(drp_operacion.SelectedValue);
        Bean.intC14 = int.Parse(drp_moneda.SelectedValue);
        int result = 0;
        if (Bean.intC11 == 0)
        {
            result = DB.Insertar_Configuracion_Agente(user, Bean);
        }
        else
        {
            result = DB.Modificar_Configuracion_Agente(user, Bean);
        }
        if (result == -100)
        {
            WebMsgBox.Show("Existio un error al momento de guargar la configuracion porfavor intente de nuevo");
            return;
        }
        else
        {
            WebMsgBox.Show("La configuracion fue guardada exitosamente");
            btn_grabar.Enabled = false;
            return;
        }
    }
    protected void drp_contabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        arr = null;
        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, int.Parse(drp_contabilidad.SelectedValue));
        drp_moneda.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_moneda.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            drp_moneda.Items.Add(item);
        }
        drp_moneda.SelectedIndex = 0;
        arr = null;
        drp_sucursal.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sucursal.Items.Add(item);
        arr = DB.getSucursales(" and suc_pai_id=" + drp_empresa_paga.SelectedValue + "  ");
        foreach (SucursalBean bean in arr)
        {
            item = new ListItem(bean.Nombre, bean.ID.ToString());
            drp_sucursal.Items.Add(item);
        }
        drp_sucursal.SelectedIndex = 0;
        drp_serie.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_serie.Items.Add(item);
        drp_serie.SelectedIndex = 0;
        drp_operacion.SelectedIndex = 0;
        //Cargar Configuracion
        RE_GenericBean Bean = null;
        if (drp_empresa_cobra.SelectedValue != "0")
        {
            Bean = (RE_GenericBean)DB.Get_Configuracion_Agente_Aduanero(user, " and tcaa_empresa_cobra=" + int.Parse(drp_empresa_cobra.SelectedValue) + " and tcaa_empresa_paga=" + drp_empresa_paga.SelectedValue + " and tcaa_tipo_contabilidad=" + int.Parse(drp_contabilidad.SelectedValue) + " ");
            if (Bean != null)
            {
                drp_sucursal.SelectedValue = Bean.strC4;
                drp_empresa_tipo_cambio.SelectedValue = Bean.strC5;
                drp_operacion.SelectedValue = "2";
                drp_operacion_SelectedIndexChanged(sender, e);
                drp_serie.SelectedValue = Bean.strC7;
                drp_tipo_persona.SelectedValue = Bean.strC8;
                tb_codigo_persona.Text = Bean.strC9;
                drp_tipo_transaccion.SelectedValue = Bean.strC10;
                drp_tipo_servicio.SelectedValue = Bean.strC11;
                drp_tipo_servicio_SelectedIndexChanged(sender, e);
                drp_rubros.SelectedValue = Bean.strC12;
                lbl_configuracion_id.Text = Bean.strC1;
                drp_empresa_cobra.Enabled = false;
                drp_empresa_paga.Enabled = false;
                drp_contabilidad.Enabled = false;
                drp_moneda.Enabled = false;
                drp_imp_exp_id.SelectedValue = Bean.strC13;
                drp_operacion.SelectedValue = Bean.strC14;
                drp_moneda.SelectedValue = Bean.strC15;
            }
        }
    }
    protected void btn_limpiar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/manager/configurar_agente_aduanero.aspx");
    }
}
