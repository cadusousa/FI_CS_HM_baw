using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class manager_contabilizacion_seguros : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
            Obtengo_listas();
        }
        Obtener_Configuracion_Seguros();
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = int.Parse(Menu1.SelectedValue);
    }
    protected void Obtengo_listas()
    {
        arr = (ArrayList)DB.Get_Intercompanys("");
        item = new ListItem("Seleccione...", "0");
        drp_empresa_asegura.Items.Clear();
        drp_empresa_contabiliza.Items.Clear();
        drp_empresa_asegura.Items.Add(item);
        drp_empresa_contabiliza.Items.Add(item);
        foreach (RE_GenericBean Intercompany in arr)
        {
            item = new ListItem(Intercompany.strC5, Intercompany.intC3.ToString());
            drp_empresa_asegura.Items.Add(item);
            drp_empresa_contabiliza.Items.Add(item);
        }
        drp_empresa_asegura.SelectedIndex = 0;
        drp_empresa_contabiliza.SelectedIndex = 0;

        arr = null;
        arr = (ArrayList)DB.GetDocumentosBySYS_TipoTeferencia();
        drp_tipo_documento.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_tipo_documento.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            if ((Bean.intC1 == 1) || (Bean.intC1 == 4) || (Bean.intC1 == 5))
            {
                item = new ListItem(Bean.strC1, Bean.intC1.ToString());
                drp_tipo_documento.Items.Add(item);
            }
        }
        drp_tipo_documento.SelectedIndex = 0;

        arr = null;
        arr = (ArrayList)DB.Get_Tipos_Transaccion_Seguros(user, "");
        drp_transaccion.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_transaccion.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem(Bean.strC2, Bean.strC1);
            drp_transaccion.Items.Add(item);
        }
        drp_transaccion.SelectedIndex = 0;

        drp_moneda_seguro.Items.Clear();
        drp_moneda_transaccion.Items.Clear();
        drp_sucursal.Items.Clear();
        drp_serie.Items.Clear();

        item = new ListItem("Seleccione...", "0");
        drp_moneda_seguro.Items.Add(item);
        drp_moneda_transaccion.Items.Add(item);
        drp_sucursal.Items.Add(item);
        drp_serie.Items.Add(item);

        drp_moneda_seguro.SelectedIndex = 0;
        drp_moneda_transaccion.SelectedIndex = 0;
        drp_sucursal.SelectedIndex = 0;
        drp_sucursal.SelectedIndex = 0;
    }
    protected void drp_empresa_asegura_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue != "0")
        {
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(int.Parse(drp_empresa_asegura.SelectedValue), 0);
            drp_moneda_seguro.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_moneda_seguro.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                drp_moneda_seguro.Items.Add(item);
            }
            drp_moneda_seguro.SelectedIndex = 0;
        }
        else
        {
            drp_moneda_seguro.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_moneda_seguro.Items.Add(item);
        }
    }
    protected void drp_empresa_contabiliza_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_empresa_contabiliza.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            drp_empresa_contabiliza.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue != "0")
        {
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(int.Parse(drp_empresa_contabiliza.SelectedValue), 0);
            drp_moneda_transaccion.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_moneda_transaccion.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                drp_moneda_transaccion.Items.Add(item);
            }
            drp_moneda_transaccion.SelectedIndex = 0;
        }
        else
        {
            drp_moneda_seguro.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_moneda_seguro.Items.Add(item);
        }
    }
    protected void drp_moneda_seguro_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_moneda_seguro.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
    }
    protected void drp_transaccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            drp_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue == "0")
        {
            drp_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa donde se Contabiliza la Carga!!");
            return;
        }
    }
    protected void drp_tipo_documento_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_tipo_documento.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            drp_tipo_documento.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue == "0")
        {
            drp_tipo_documento.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa donde se Contabiliza la Carga!!");
            return;
        }
        if (drp_transaccion.SelectedValue == "0")
        {
            drp_tipo_documento.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Transaccion a Operar!!");
            return;
        }
    }
    protected void drp_contabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_contabilidad.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            drp_contabilidad.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue == "0")
        {
            drp_contabilidad.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa donde se Contabiliza la Carga!!");
            return;
        }
        if (drp_transaccion.SelectedValue == "0")
        {
            drp_contabilidad.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Transaccion a Operar!!");
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            drp_contabilidad.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Documento a Contabilizar!!");
            return;
        }
        if (drp_tipo_servicio.SelectedValue == "0")
        {
            drp_contabilidad.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Servicio en el cual se Contabilizaran los Ingresos o Costos!!");
            return;
        }

        drp_moneda_transaccion.SelectedValue = "0";
        drp_sucursal.SelectedIndex = 0;
        drp_tipo_operacion.SelectedValue = "0";
        item = new ListItem("Seleccione...", "0");
        drp_serie.Items.Clear();
        drp_serie.Items.Add(item);
    }
    protected void drp_moneda_transaccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_moneda_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            drp_moneda_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue == "0")
        {
            drp_moneda_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa donde se Contabiliza la Carga!!");
            return;
        }
        if (drp_transaccion.SelectedValue == "0")
        {
            drp_moneda_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Transaccion a Operar!!");
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            drp_moneda_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Documento a Contabilizar!!");
            return;
        }
        if (drp_tipo_servicio.SelectedValue == "0")
        {
            drp_moneda_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Servicio en el cual se Contabilizaran los Ingresos o Costos!!");
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            drp_moneda_transaccion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Contabilidad donde desea Contabilizar el Documento");
            return;
        }

        arr = null;
        arr = (ArrayList)DB.getSucursales(" and suc_pai_id=" + drp_empresa_contabiliza.SelectedValue + " ");
        drp_sucursal.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_sucursal.Items.Add(item);
        foreach (SucursalBean Bean in arr)
        {
            item = new ListItem(Bean.Nombre, Bean.ID.ToString());
            if ((drp_transaccion.SelectedValue == "1") || (drp_transaccion.SelectedValue == "3"))
            {
                if (Bean.Nombre == "SISTEMAS")
                {
                    drp_sucursal.Items.Add(item);
                }
            }
            else
            {
                drp_sucursal.Items.Add(item);
            }
        }
        drp_sucursal.SelectedIndex = 0;
        drp_tipo_operacion.SelectedValue = "0";
        item = new ListItem("Seleccione...", "0");
        drp_serie.Items.Clear();
        drp_serie.Items.Add(item);
    }
    protected void drp_sucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_sucursal.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            drp_sucursal.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue == "0")
        {
            drp_sucursal.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa donde se Contabiliza la Carga!!");
            return;
        }
        if (drp_transaccion.SelectedValue == "0")
        {
            drp_sucursal.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Transaccion a Operar!!");
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            drp_sucursal.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Documento a Contabilizar!!");
            return;
        }
        if (drp_tipo_servicio.SelectedValue == "0")
        {
            drp_sucursal.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Servicio en el cual se Contabilizaran los Ingresos o Costos!!");
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            drp_sucursal.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Contabilidad donde desea Contabilizar el Documento");
            return;
        }
        if (drp_moneda_transaccion.SelectedValue == "0")
        {
            drp_sucursal.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Moneda en que sera Contabilizado el Documento");
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            drp_sucursal.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Sucursal en que sera Contabilizado el Documento");
            return;
        }
    }
    protected void drp_tipo_operacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa donde se Contabiliza la Carga!!");
            return;
        }
        if (drp_transaccion.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Transaccion a Operar!!");
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Documento a Contabilizar!!");
            return;
        }
        if (drp_tipo_servicio.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Servicio en el cual se Contabilizaran los Ingresos o Costos!!");
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Contabilidad donde desea Contabilizar el Documento");
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Sucursal en que sera Contabilizado el Documento");
            return;
        }
        if (drp_moneda_transaccion.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Moneda en que sera Contabilizado el Documento");
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            drp_tipo_operacion.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Moneda en que sera Contabilizado el Documento");
            return;
        }

        if (drp_tipo_documento.SelectedValue == "5")
        {
            drp_serie.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            drp_serie.Items.Add(item);
            if (drp_contabilidad.SelectedValue == "1")
            {
                if (drp_moneda_transaccion.SelectedValue == "8")
                {
                    if ((drp_empresa_contabiliza.SelectedValue == "2") || (drp_empresa_contabiliza.SelectedValue == "26") || (drp_empresa_contabiliza.SelectedValue == "6") || (drp_empresa_contabiliza.SelectedValue == "25"))
                    {
                        item = new ListItem("PFSA", "1");
                        drp_serie.Items.Add(item);
                    }
                    else
                    {
                        item = new ListItem("PFSAD", "3");
                        drp_serie.Items.Add(item);
                    }
                }
                else
                {
                    item = new ListItem("PFSA", "1");
                    drp_serie.Items.Add(item);
                }
            }
            else if (drp_contabilidad.SelectedValue == "2")
            {
                item = new ListItem("PFNA", "2");
                drp_serie.Items.Add(item);
            }
        }
        else
        {
            string sql = "";
            sql = "  fac_suc_id=" + drp_sucursal.SelectedValue + "  and fac_tipo=" + drp_tipo_documento.SelectedValue + " and fac_conta_id=" + drp_contabilidad.SelectedValue + " and fac_operacion_id=" + drp_tipo_operacion.SelectedValue + " and fac_mon_id=" + drp_moneda_transaccion.SelectedValue + " ";
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
    }
    protected void drp_tipo_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_tipo_servicio.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            drp_tipo_servicio.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue == "0")
        {
            drp_tipo_servicio.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa donde se Contabiliza la Carga!!");
            return;
        }
        if (drp_transaccion.SelectedValue == "0")
        {
            drp_tipo_servicio.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Transaccion a Operar!!");
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            drp_tipo_servicio.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Documento a Contabilizar!!");
            return;
        }
    }
    protected void drp_serie_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Empresa donde se Contabiliza la Carga!!");
            return;
        }
        if (drp_transaccion.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Transaccion a Operar!!");
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Documento a Contabilizar!!");
            return;
        }
        if (drp_tipo_servicio.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione el Tipo de Servicio en el cual se Contabilizaran los Ingresos o Costos!!");
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Contabilidad donde desea Contabilizar el Documento");
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Sucursal en que sera Contabilizado el Documento");
            return;
        }
        if (drp_moneda_transaccion.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Moneda en que sera Contabilizado el Documento");
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Moneda en que sera Contabilizado el Documento");
            return;
        }
        if (drp_serie.SelectedValue == "0")
        {
            drp_serie.SelectedValue = "0";
            WebMsgBox.Show("Por Favor seleccione la Serie en que sera Contabilizado el Documento");
            return;
        }
    }
    protected void btn_nueva_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/manager/contabilizacion_seguros.aspx");
    }
    protected void btn_guardar_Click(object sender, EventArgs e)
    {
        if (drp_empresa_asegura.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Empresa que Asegura la Carga!!");
            return;
        }
        if (drp_moneda_seguro.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la del Seguro!!");
            return;
        }
        if (drp_empresa_contabiliza.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Empresa donde se Contabiliza la Carga!!");
            return;
        }
        if (drp_transaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Transaccion a Operar!!");
            return;
        }
        if (drp_tipo_documento.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione el Tipo de Documento a Contabilizar!!");
            return;
        }
        if (drp_tipo_servicio.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione el Tipo de Servicio en el cual se Contabilizaran los Ingresos o Costos!!");
            return;
        }
        if (drp_contabilidad.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Contabilidad donde desea Contabilizar el Documento");
            return;
        }
        if (drp_sucursal.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Sucursal en que sera Contabilizado el Documento");
            return;
        }
        if (drp_moneda_transaccion.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Moneda en que sera Contabilizado el Documento");
            return;
        }
        if (drp_tipo_operacion.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Moneda en que sera Contabilizado el Documento");
            return;
        }
        if (drp_serie.SelectedValue == "0")
        {
            WebMsgBox.Show("Por Favor seleccione la Serie en que sera Contabilizado el Documento");
            return;
        }
        RE_GenericBean Bean_Configuracion = new RE_GenericBean();
        Bean_Configuracion.intC1 = int.Parse(drp_empresa_asegura.SelectedValue);//tsc_empresa_asegura_id
        Bean_Configuracion.intC2 = int.Parse(drp_moneda_seguro.SelectedValue);//tsc_moneda_seguro
        Bean_Configuracion.intC3 = int.Parse(drp_empresa_contabiliza.SelectedValue);//tsc_empresa_contabiliza_id
        Bean_Configuracion.intC4 = int.Parse(drp_transaccion.SelectedValue);//tsc_tstt_id
        Bean_Configuracion.intC5 = int.Parse(drp_tipo_documento.SelectedValue);//tsc_ttr_id
        Bean_Configuracion.intC6 = int.Parse(drp_tipo_servicio.SelectedValue);//tsc_tts_id
        Bean_Configuracion.intC7 = int.Parse(drp_contabilidad.SelectedValue);//tsc_contabilidad_id
        Bean_Configuracion.intC8 = int.Parse(drp_moneda_transaccion.SelectedValue);//tsc_moneda_id
        Bean_Configuracion.intC9 = int.Parse(drp_sucursal.SelectedValue);//tsc_suc_id
        Bean_Configuracion.intC10 = int.Parse(drp_tipo_operacion.SelectedValue);//tsc_operacion_id
        Bean_Configuracion.strC1 = drp_serie.SelectedItem.Text;//tsc_serie
        Bean_Configuracion.strC2 = user.ID;//tsc_usuario_configuracion
        int validar_existencia = 0;
        string sql = "";
        sql = " and tsc_empresa_asegura_id=" + Bean_Configuracion.intC1.ToString() + " and tsc_moneda_seguro=" + Bean_Configuracion.intC2.ToString() + " and tsc_tstt_id=" + Bean_Configuracion.intC4.ToString() + " ";
        validar_existencia = Contabilizacion_Automatica_CAD.Validar_Existencia_Configuracion_Seguros(user, sql);
        if (validar_existencia == -100)
        {
            WebMsgBox.Show("Existio un error al momento de Tratar de Validar la Existencia de la Configuracion de Seguros.");
            return;
        }
        else if (validar_existencia == 1)
        {
            WebMsgBox.Show("Configuracion Existente");
            return;
        }
        int insert_result = 0;
        insert_result = Contabilizacion_Automatica_CAD.Insertar_Configuracion_Seguros(user, Bean_Configuracion);
        if (insert_result == -100)
        {
            WebMsgBox.Show("Existio un error al momento de Ingresar la Configuracion.");
            return;
        }
        else if (insert_result == 1)
        {
            Limpiar();
            WebMsgBox.Show("Configuracion guardada exitosamente");
            return;
        }
    }
    protected void Limpiar()
    {
        item = new ListItem("Seleccione...", "0");
        drp_empresa_asegura.SelectedValue = "0";
        drp_moneda_seguro.Items.Clear();
        drp_moneda_seguro.Items.Add(item);
        drp_empresa_contabiliza.SelectedValue = "0";
        drp_transaccion.SelectedValue = "0";
        drp_tipo_documento.SelectedValue = "0";
        drp_tipo_servicio.SelectedValue = "0";
        drp_contabilidad.SelectedValue = "0";
        drp_moneda_transaccion.Items.Clear();
        drp_moneda_transaccion.Items.Add(item);
        drp_sucursal.Items.Clear();
        drp_sucursal.Items.Add(item);
        drp_tipo_operacion.SelectedValue = "0";
        drp_serie.Items.Clear();
        drp_serie.Items.Add(item);
    }
    protected void Obtener_Configuracion_Seguros()
    {
        int correlativo = 1;
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NO");
        dt.Columns.Add("EMPRESA_ASEGURA");
        dt.Columns.Add("MONEDA");
        dt.Columns.Add("EMPRESA_CONTABILIZA");
        dt.Columns.Add("TRANSACCION");
        dt.Columns.Add("DOCUMENTO");
        dt.Columns.Add("SERVICIO");
        dt.Columns.Add("CONTA");
        dt.Columns.Add("MONEDA ");
        dt.Columns.Add("SUCURSAL");
        dt.Columns.Add("OPERACION");
        dt.Columns.Add("SERIE");
        ArrayList Arr = Contabilizacion_Automatica_CAD.Obtener_Configuraciones_Seguros("");
        if (Arr != null)
        {
            foreach (RE_GenericBean Bean in Arr)
            {
                object[] Obj_Arr = { Bean.intC1.ToString(), correlativo.ToString(), Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC7, Bean.strC8, Bean.strC9, Bean.strC10, Bean.strC11, Bean.strC12 };
                correlativo++;
                dt.Rows.Add(Obj_Arr);
            }
        }
        gv_configuraciones.DataSource = dt;
        gv_configuraciones.DataBind();
        gv_configuraciones_eliminar.DataSource = dt;
        gv_configuraciones_eliminar.DataBind();


    }
    protected void gv_configuraciones_eliminar_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int indice = e.RowIndex;
        int ID = int.Parse(gv_configuraciones_eliminar.Rows[indice].Cells[1].Text.ToString());
        int resultado = Contabilizacion_Automatica_CAD.Eliminar_Configuracion_Seguros(user, ID);
        if (resultado == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de Eliminar la configuracion");
            return;
        }
        else
        {
            Obtener_Configuracion_Seguros();
            WebMsgBox.Show("Configuracion Eliminada Exitosamente");
            return;
        }
    }
    protected void gv_configuraciones_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            //e.Row.Cells[0].Visible = false;
        }
    }
    protected void gv_configuraciones_eliminar_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            //e.Row.Cells[1].Visible = false;
        }
    }
}