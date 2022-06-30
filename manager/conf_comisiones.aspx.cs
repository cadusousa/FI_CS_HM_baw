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

public partial class manager_conf_comisiones : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ListItem item = null;
    ListItem item1 = null;
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
            cargar();
        }

        if (Menu1.SelectedValue != "")
        {
            if (int.Parse(Menu1.SelectedValue) == 0)
            {
                lb_titulo.Text = Menu1.SelectedItem.Text;
                llenogrid(1);
            }
            else if (int.Parse(Menu1.SelectedValue) == 1)
            {
                lb_titulo1.Text = Menu1.SelectedItem.Text;
                llenogrid(2);
            }
            else if (int.Parse(Menu1.SelectedValue) == 2)
            {
                lb_titulo2.Text = Menu1.SelectedItem.Text;
                llenogrid(3);
            }
            else if (int.Parse(Menu1.SelectedValue) == 3)
            {
                lb_titulo4.Text = Menu1.SelectedItem.Text;
                llenogrid(7);
            }
        }
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = int.Parse(Menu1.SelectedValue);
        
        if (int.Parse(Menu1.SelectedValue) == 0)
        {
            lb_titulo.Text = Menu1.SelectedItem.Text;
            llenogrid(1);
        }
        else if (int.Parse(Menu1.SelectedValue) == 1)
        {
            lb_titulo1.Text = Menu1.SelectedItem.Text;
            llenogrid(2);
        }
        else if (int.Parse(Menu1.SelectedValue) == 2)
        {
            lb_titulo2.Text = Menu1.SelectedItem.Text;
            llenogrid(3);
        }
        else if (int.Parse(Menu1.SelectedValue) == 3)
        {
            lb_titulo4.Text = Menu1.SelectedItem.Text;
            llenogrid(7);
        }
    }
    private void cargar()
    {
        
        arr = (ArrayList)DB.getServiciosMaster();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            ddl_tipo_servico.Items.Add(item);
            ddl_servicio1.Items.Add(item);
            ddl_servicio2.Items.Add(item);
            ddl_servicio3.Items.Add(item);
        }
        item = null;
        ArrayList rubros = new ArrayList();
        ddl_tipo_rubro.Items.Clear();
        rubros = (ArrayList)DB.getRubrosXservicioMaster(int.Parse(ddl_tipo_servico.SelectedValue.ToString()));
        foreach (RE_GenericBean rgb1 in rubros)
        {
            item = new ListItem(rgb1.strC1, rgb1.intC1.ToString());
            ddl_tipo_rubro.Items.Add(item);
        }
        item = null;
        ArrayList agentes = new ArrayList();
        ddl_agente.Items.Clear();
        agentes = (ArrayList)DB.getAgentesList();
        item = new ListItem("Seleccione...", "0");
        ddl_agente.Items.Add(item);
        ddl_agente1.Items.Add(item);
        foreach (RE_GenericBean rgb2 in agentes)
        {
            item = new ListItem(rgb2.strC1,rgb2.intC1.ToString());
            ddl_agente.Items.Add(item);
            ddl_agente1.Items.Add(item);
        }
        item = null;
        ArrayList traficos = new ArrayList();
        traficos = (ArrayList)DB.getTipo_Operacion();
        item = new ListItem("Seleccione...", "0");
        ddl_tipo_trafico.Items.Add(item);
        foreach (RE_GenericBean rgb in traficos)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            ddl_tipo_trafico.Items.Add(item);
        }
        item = null;
        ArrayList regimen = new ArrayList();
        regimen = (ArrayList)DB.Get_Regimen_Aduanero_XPais(user.PaisID);
        item = new ListItem("Seleccione...", "0");
        ddl_regimen.Items.Add(item);
        foreach (RE_GenericBean rgb in regimen)
        {
            item = new ListItem(rgb.strC2, rgb.strC1);
            ddl_regimen.Items.Add(item);
        }
        
        item = null;
        ddl_tipo_tarifa.Items.Clear();
        ddl_tarifa1.Items.Clear();
        ddl_tarifa2.Items.Clear();
        ddl_tarifa3.Items.Clear();
        item = new ListItem("%","1");
        ddl_tipo_tarifa.Items.Add(item);
        ddl_tarifa1.Items.Add(item);
        ddl_tarifa2.Items.Add(item);
        ddl_tarifa3.Items.Add(item);
        item1 = new ListItem("Plana","2");
        ddl_tipo_tarifa.Items.Add(item1);
        ddl_tarifa1.Items.Add(item1);
        ddl_tarifa2.Items.Add(item1);
        ddl_tarifa3.Items.Add(item1);
        item1 = null;
        item = null;
        ddl_tipo.Items.Clear();
        ddl_tipo1.Items.Clear();
        ddl_tipo2.Items.Clear();
        ddl_tipo3.Items.Clear();
        item = new ListItem("EXPORTACION", "2");
        ddl_tipo.Items.Add(item);
        ddl_tipo1.Items.Add(item);
        ddl_tipo2.Items.Add(item);
        ddl_tipo3.Items.Add(item);
        item1 = new ListItem("IMPORTACION", "1");
        ddl_tipo.Items.Add(item1);
        ddl_tipo1.Items.Add(item1);
        ddl_tipo2.Items.Add(item1);
        ddl_tipo3.Items.Add(item1);
        item = null;
        item1 = null;
        ddl_iva.Items.Clear();
        ddl_iva1.Items.Clear();
        ddl_iva2.Items.Clear();
        item = new ListItem("Despues", "1");
        ddl_iva.Items.Add(item);
        ddl_iva1.Items.Add(item);
        ddl_iva2.Items.Add(item);
        item1 = new ListItem("Antes", "2");
        ddl_iva.Items.Add(item1);
        ddl_iva1.Items.Add(item1);
        ddl_iva2.Items.Add(item1);
    }

    private void llenogrid(int tipo)
    {
        if (tipo == 1)
        {
            GridView1.DataSourceID = null;
            DataTable dt = new DataTable();
            dt = (DataTable)DB.getMatrizComision(user.PaisID, tipo);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        else if (tipo == 2)
        {
            GridView2.DataSourceID = null;
            DataTable dt = new DataTable();
            dt = (DataTable)DB.getMatrizComision(user.PaisID, tipo);
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }
        else if ((tipo == 3) || (tipo == 4) || (tipo == 5))
        {
            GridView3.DataSourceID = null;
            DataTable dt = new DataTable();
            dt = (DataTable)DB.getMatrizComision(user.PaisID, tipo);
            GridView3.DataSource = dt;
            GridView3.DataBind();
        }
        if (tipo == 7)
        {
            GridView4.DataSourceID = null;
            DataTable dt = new DataTable();
            dt = (DataTable)DB.getMatrizComision(user.PaisID, tipo);
            GridView4.DataSource = dt;
            GridView4.DataBind();
        }
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }

    protected void GridView4_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView4.PageIndex = e.NewPageIndex;
        GridView4.DataBind();
    }

    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        if (tb_valor.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe de especificar el valor de la tarifa");
            return;
        }
        if (tb_observaciones.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe ingresar una observacion acorde a la configuracion de la tarifa.");
            return;
        }
        if (tb_observaciones.Text.Length <= 10)
        {
            WebMsgBox.Show("Debe ingresar una observacion mas especifica a la tarifa de al menos 10 caracteres.");
            return;
        }
        RE_GenericBean datos = new RE_GenericBean();
        datos.intC1 = 1; //TIPO TARIFA.
        datos.intC2 = int.Parse(ddl_tipo_servico.SelectedValue.ToString()); //tipo servicio.
        datos.intC3 = int.Parse(ddl_tipo_rubro.SelectedValue.ToString()); //tipo rubro.
        datos.intC4 = int.Parse(ddl_tipo_tarifa.SelectedValue.ToString()); //tipo tarifa.
        datos.intC5 = int.Parse(ddl_agente.SelectedValue.ToString()); //tipo agente.
        datos.intC6 = int.Parse(ddl_tipo.SelectedValue.ToString()); //tipo imp exp.
        datos.intC7 = int.Parse(ddl_iva.SelectedValue.ToString()); //despues iva o antes
        datos.decC1 = decimal.Parse(tb_valor.Text.Trim()); //valor
        datos.strC1 = tb_observaciones.Text.Trim(); //observaciones
        int resultado = DB.InsertComision(datos, user);
        if (resultado > 0)
        {
            WebMsgBox.Show("CONFIGURACION INGRESADA CON EXITO");
        }
        else
        {
            WebMsgBox.Show("EXISTIO UN ERROR AL MOMENTO DE GUARDAR");
            return;
        }
        llenogrid(1);
    }

    protected void bt_guardar4_Click(object sender, EventArgs e)
    {
        if (tb_valor3.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe de especificar el valor de la tarifa");
            return;
        }
        if (tb_observacion3.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe ingresar una observacion acorde a la configuracion de la tarifa.");
            return;
        }
        if (tb_observacion3.Text.Length <= 10)
        {
            WebMsgBox.Show("Debe ingresar una observacion mas especifica a la tarifa de al menos 10 caracteres.");
            return;
        }
        RE_GenericBean datos = new RE_GenericBean();
        datos.intC1 = 7; //TIPO TARIFA.
        datos.intC2 = int.Parse(ddl_servicio3.SelectedValue.ToString()); //tipo servicio.
        //datos.intC3 = int.Parse(ddl_tipo_rubro.SelectedValue.ToString()); //tipo rubro.
        datos.intC4 = int.Parse(ddl_tarifa3.SelectedValue.ToString()); //tipo tarifa.
        datos.intC5 = int.Parse(ddl_agente1.SelectedValue.ToString()); //tipo agente.
        datos.intC6 = int.Parse(ddl_tipo3.SelectedValue.ToString()); //tipo imp exp.
        datos.intC7 = int.Parse(ddl_iva2.SelectedValue.ToString()); //despues iva o antes
        datos.decC1 = decimal.Parse(tb_valor3.Text.Trim()); //valor
        datos.strC1 = tb_observacion3.Text.Trim(); //observaciones
        int resultado = DB.InsertComision(datos, user);
        if (resultado > 0)
        {
            WebMsgBox.Show("CONFIGURACION INGRESADA CON EXITO");
        }
        else
        {
            WebMsgBox.Show("EXISTIO UN ERROR AL MOMENTO DE GUARDAR");
            return;
        }
        llenogrid(7);
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        int id = int.Parse(GridView1.Rows[index].Cells[1].Text);
    }
    protected void GridView4_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        int id = int.Parse(GridView4.Rows[index].Cells[1].Text);
    }
    protected void ddl_tipo_servico_SelectedIndexChanged(object sender, EventArgs e)
    {
        item = null;
        ArrayList rubros = new ArrayList();
        ddl_tipo_rubro.Items.Clear();
        rubros = (ArrayList)DB.getRubrosXservicioMaster(int.Parse(ddl_tipo_servico.SelectedValue.ToString()));
        foreach (RE_GenericBean rgb1 in rubros)
        {
            item = new ListItem(rgb1.strC1, rgb1.intC1.ToString());
            ddl_tipo_rubro.Items.Add(item);
        }

    }
    protected void bt_guardar1_Click(object sender, EventArgs e)
    {
         if (tb_valor1.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe de especificar el valor de la tarifa");
            return;
        }
        if (tb_observaciones1.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe ingresar una observacion acorde a la configuracion de la tarifa.");
            return;
        }
        if (tb_observaciones1.Text.Length <= 10)
        {
            WebMsgBox.Show("Debe ingresar una observacion mas especifica a la tarifa de al menos 10 caracteres.");
            return;
        }
        RE_GenericBean datos = new RE_GenericBean();
        datos.intC1 = 2; //TIPO TARIFA.
        datos.intC2 = int.Parse(ddl_servicio1.SelectedValue.ToString()); //tipo servicio.
       // datos.intC3 = int.Parse(ddl_tipo_rubro.SelectedValue.ToString()); //tipo rubro.
        datos.intC4 = int.Parse(ddl_tarifa1.SelectedValue.ToString()); //tipo tarifa.
       // datos.intC5 = int.Parse(ddl_agente.SelectedValue.ToString()); //tipo agente.
        datos.intC6 = int.Parse(ddl_tipo1.SelectedValue.ToString()); //tipo imp exp.
        datos.intC7 = int.Parse(ddl_iva1.SelectedValue.ToString()); //despues iva o antes
        datos.decC1 = decimal.Parse(tb_valor1.Text.Trim()); //valor
        datos.strC1 = tb_observaciones1.Text.Trim(); //observaciones
        datos.decC2 = decimal.Parse(tb_sobre_venta.Text.Trim().ToString());//sobre venta %
        datos.decC3 = decimal.Parse(tb_iva.Text.Trim().ToString());//total iva %
        int resultado = DB.InsertComision(datos, user);
        if (resultado > 0)
        {
            WebMsgBox.Show("CONFIGURACION INGRESADA CON EXITO");
        }
        else
        {
            WebMsgBox.Show("EXISTIO UN ERROR AL MOMENTO DE GUARDAR");
            return;
        }
        llenogrid(2);
    }


    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        int id = int.Parse(GridView2.Rows[index].Cells[1].Text);
    }
    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        GridView2.DataBind();
    }
    protected void bt_guardar3_Click(object sender, EventArgs e)
    {
        if (tb_valor2.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe de especificar el valor de la tarifa");
            return;
        }
        if (tb_observacion2.Text.Trim() == "")
        {
            WebMsgBox.Show("Debe ingresar una observacion acorde a la configuracion de la tarifa.");
            return;
        }
        if (tb_observacion2.Text.Length <= 10)
        {
            WebMsgBox.Show("Debe ingresar una observacion mas especifica a la tarifa de al menos 10 caracteres.");
            return;
        }
        RE_GenericBean datos = new RE_GenericBean();
        if (ddl_tipo_trafico.SelectedValue != "0")
        {
            if (ddl_regimen.SelectedValue == "0")
            {
                datos.intC1 = 5; //TIPO TARIFA.
                datos.intC2 = int.Parse(ddl_servicio2.SelectedValue.ToString()); //tipo servicio.
                datos.intC3 = int.Parse(ddl_tipo_trafico.SelectedValue.ToString()); //tipo trafico.
                datos.intC4 = int.Parse(ddl_tarifa2.SelectedValue.ToString()); //tipo tarifa.
                //datos.intC5 = int.Parse(ddl_regimen.SelectedValue.ToString()); //tipo regimen
                datos.intC6 = int.Parse(ddl_tipo2.SelectedValue.ToString()); //tipo imp exp.
                datos.decC1 = decimal.Parse(tb_valor2.Text.Trim()); //valor
                datos.strC1 = tb_observacion2.Text.Trim(); //observaciones
            }
            else
            {
                datos.intC1 = 3; //TIPO TARIFA.
                datos.intC2 = int.Parse(ddl_servicio2.SelectedValue.ToString()); //tipo servicio.
                datos.intC3 = int.Parse(ddl_tipo_trafico.SelectedValue.ToString()); //tipo trafico.
                datos.intC4 = int.Parse(ddl_tarifa2.SelectedValue.ToString()); //tipo tarifa.
                datos.intC5 = int.Parse(ddl_regimen.SelectedValue.ToString()); //tipo regimen
                datos.intC6 = int.Parse(ddl_tipo2.SelectedValue.ToString()); //tipo imp exp.
                datos.decC1 = decimal.Parse(tb_valor2.Text.Trim()); //valor
                datos.strC1 = tb_observacion2.Text.Trim(); //observaciones
            }
        }
        else if (ddl_tipo_trafico.SelectedValue == "0")
        {
            if (ddl_regimen.SelectedValue == "0")
            {
                WebMsgBox.Show("Sino elige un tipo de trafico debe al menos elegir un regimen.");
                return;
            }
            datos.intC1 = 4; //TIPO TARIFA.
            datos.intC2 = int.Parse(ddl_servicio2.SelectedValue.ToString()); //tipo servicio.
            //datos.intC3 = int.Parse(ddl_tipo_trafico.SelectedValue.ToString()); //tipo trafico.
            datos.intC4 = int.Parse(ddl_tarifa2.SelectedValue.ToString()); //tipo tarifa.
            datos.intC5 = int.Parse(ddl_regimen.SelectedValue.ToString()); //tipo regimen
            datos.intC6 = int.Parse(ddl_tipo2.SelectedValue.ToString()); //tipo imp exp.
            datos.decC1 = decimal.Parse(tb_valor2.Text.Trim()); //valor
            datos.strC1 = tb_observacion2.Text.Trim(); //observaciones
        }
        
        int resultado = DB.InsertComision(datos, user);
        if (resultado > 0)
        {
            WebMsgBox.Show("CONFIGURACION INGRESADA CON EXITO");
        }
        else
        {
            WebMsgBox.Show("EXISTIO UN ERROR AL MOMENTO DE GUARDAR");
            return;
        }
        llenogrid(datos.intC1);

    }
    protected void GridView3_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        int id = int.Parse(GridView3.Rows[index].Cells[1].Text);

    }
    protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView3.PageIndex = e.NewPageIndex;
        GridView3.DataBind();

    }
}