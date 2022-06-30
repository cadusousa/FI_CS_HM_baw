using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

public partial class manager_configurar_documento_electronico : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ListItem item = null;
    ListItem item_aux = null;
    int result = 0;
    int maximo = 0;
    int indice = 0;
    int nuevo_indice = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!Page.IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
            Obtengo_listas();
        }
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = int.Parse(Menu1.SelectedValue);
        Limpiar_Pantalla();
    }
    protected void Obtengo_listas()
    {
        arr = null;
        arr = (ArrayList)DB.getPaises("");
        item = new ListItem("Seleccione...", "0");
        drp_pais.Items.Add(item);
        drp_pais2.Items.Add(item);
        drp_pais3.Items.Add(item);
        drp_pais4.Items.Add(item);
        foreach (PaisBean pais in arr)
        {
            item = new ListItem(pais.Nombre, pais.ID.ToString());
            drp_pais.Items.Add(item);
            drp_pais2.Items.Add(item);
            drp_pais3.Items.Add(item);
            drp_pais4.Items.Add(item);
        }
        drp_pais.SelectedIndex = 0;
        drp_pais2.SelectedIndex = 0;
        drp_pais3.SelectedIndex = 0;
        drp_pais4.SelectedIndex = 0;
        arr = null;
        arr = (ArrayList)DB.GetDocumentosBySYS_TipoTeferencia();
        item = new ListItem("Seleccione...");
        drp_documentos.Items.Add(item);
        drp_documentos2.Items.Add(item);
        drp_documentos3.Items.Add(item);
        drp_documentos4.Items.Add(item);
        foreach (RE_GenericBean Bean in arr)
        {
            item = new ListItem(Bean.strC1, Bean.intC1.ToString());
            drp_documentos.Items.Add(item);
            drp_documentos2.Items.Add(item);
            drp_documentos3.Items.Add(item);
            drp_documentos4.Items.Add(item);
        }
        drp_documentos.SelectedIndex = 0;
        drp_documentos2.SelectedIndex = 0;
        drp_documentos3.SelectedIndex = 0;
        drp_documentos4.SelectedIndex = 0;
        item = new ListItem("Seleccione...");
        drp_nodo_hijo.Items.Add(item);
        drp_nodo_padre.Items.Add(item);
        drp_nodo_padre2.Items.Add(item);
        drp_nodo_hijo.SelectedIndex = 0;
        drp_nodo_padre.SelectedIndex = 0;
        drp_nodo_padre2.SelectedIndex = 0;
    }
    protected void btn_ingresar_nodo_Click(object sender, EventArgs e)
    {
        RE_GenericBean Bean = new RE_GenericBean();
        result = 0;
        if (drp_pais.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el pais");
            return;
        }
        else if (drp_documentos.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el tipo de documento a configurar");
            return;
        }
        else if (drp_nivel.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el nivel del nodo");
            return;
        }
        else if (tb_nombre.Text == "")
        {
            WebMsgBox.Show("Debe ingresar el nombre del Nodo");
            return;
        }
        Bean.intC1 = int.Parse(drp_pais.SelectedValue);
        Bean.intC2 = int.Parse(drp_documentos.SelectedValue);
        Bean.intC3 = int.Parse(drp_nivel.SelectedValue);
        Bean.strC1 = tb_nombre.Text.Trim();
        Bean.intC4 = int.Parse(drp_contabilidad.SelectedValue);
        Bean.strC2 = tb_texto_predeterminado.Text.Trim();
        if (chk_puede_repetirse.Checked == true)
        {
            Bean.boolC1 = true;
        }
        else
        {
            Bean.boolC1 = false;
        }
        result = DB.Insertar_Nodo(user, Bean);
        if (result == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de guardar el Nodo, porfavor intente de nuevo");
            return;
        }
        else
        {
            WebMsgBox.Show("El Nodo fue guardado exitosamente");
            tb_nombre.Text = "";
            tb_texto_predeterminado.Text = "";
            chk_puede_repetirse.Checked = false;
            return;
        }
    }
    protected void btn_nuevo_Click(object sender, EventArgs e)
    {
        Limpiar_Pantalla();
    }
    protected void btn_visualizar_Click(object sender, EventArgs e)
    {
        ArrayList arr_nodos = null;
        string sql = "";
        int nivel = 0;
        if (drp_pais3.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Pais");
            return;
        }
        else if (drp_documentos3.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Documento");
            return;
        }
        else if (drp_nivel3.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Nivel");
            return;
        }
        nivel = int.Parse(drp_nivel3.SelectedValue);
        sql = " and a.tn_pai_id=" + drp_pais3.SelectedValue + " and a.tn_nivel=" + nivel + " and a.tn_conta_id=" + drp_contabilidad3.SelectedValue + " and a.tn_ttr_id=" + drp_documentos3.SelectedValue + " ";
        arr_nodos = (ArrayList)DB.Get_Nodos(user, sql);
        drp_nodo_hijo.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_nodo_hijo.Items.Add(item);
        if (arr_nodos != null)
        {
            foreach (RE_GenericBean bean in arr_nodos)
            {
                item = new ListItem(bean.strC3, bean.intC1.ToString());
                drp_nodo_hijo.Items.Add(item);
            }
        }
        drp_nodo_hijo.SelectedIndex = 0;
        nivel = int.Parse(drp_nivel3.SelectedValue) - 1;
        sql = " and a.tn_pai_id=" + drp_pais3.SelectedValue + " and a.tn_nivel=" + nivel + " and a.tn_conta_id=" + drp_contabilidad3.SelectedValue + " and a.tn_ttr_id=" + drp_documentos3.SelectedValue + " ";
        arr_nodos = (ArrayList)DB.Get_Nodos(user, sql);
        drp_nodo_padre.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_nodo_padre.Items.Add(item);
        if (arr_nodos != null)
        {
            foreach (RE_GenericBean bean in arr_nodos)
            {
                item = new ListItem(bean.strC3, bean.intC1.ToString());
                drp_nodo_padre.Items.Add(item);
            }
        }
        drp_nodo_padre.SelectedIndex = 0;
    }
    protected void btn_asignar_Click(object sender, EventArgs e)
    {
        if (drp_nodo_hijo.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Nodo Hijo");
            return;
        }
        else if (drp_nodo_padre.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Nodo Padre");
            return;
        }
        string sql = "update tbl_nodos set tn_padre=" + drp_nodo_padre.SelectedValue + ", tn_usuario_modifica='" + user.ID + "', tn_fecha_modifica=current_date where tn_id=" + drp_nodo_hijo.SelectedValue + " ";
        int result = DB.Update_Nodos(user, sql);
        if (result == -100)
        {
            WebMsgBox.Show("Existio un error al momento tratrar de actualizar el nodo");
            return;
        }
        else
        {
            WebMsgBox.Show("El Nodo fue actualizado correctamente");
            btn_visualizar_Click(sender, e);
            return;
        }
    }
    protected void Limpiar_Pantalla()
    {
        drp_pais.SelectedIndex = 0;
        drp_documentos.SelectedIndex = 0;
        drp_nivel.SelectedIndex = 0;
        tb_nombre.Text = "";
        drp_pais2.SelectedIndex = 0;
        drp_documentos2.SelectedIndex = 0;
        drp_pais3.SelectedIndex = 0;
        drp_pais4.SelectedIndex = 0;
        drp_documentos3.SelectedIndex = 0;
        drp_documentos4.SelectedIndex = 0;
        drp_nivel3.SelectedIndex = 0;
        drp_nivel4.SelectedIndex = 0;
        drp_nodo_hijo.Items.Clear();
        drp_nodo_padre.Items.Clear();
        drp_nodo_padre2.Items.Clear();
        item = new ListItem("Seleccione...");
        drp_nodo_hijo.Items.Add(item);
        drp_nodo_padre.Items.Add(item);
        drp_nodo_padre2.Items.Add(item);
        drp_nodo_hijo.SelectedIndex = 0;
        drp_nodo_padre.SelectedIndex = 0;
        drp_nodo_padre2.SelectedIndex = 0;
        lb_nodos_hijos.Items.Clear();
        tb_texto_predeterminado.Text = "";
        trv_xml.Nodes.Clear();
    }

    protected void btn_visualizar2_Click(object sender, EventArgs e)
    {
        ArrayList arr_nodos = null;
        string sql = "";
        int nivel = 0;
        if (drp_pais4.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Pais");
            return;
        }
        else if (drp_documentos4.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Documento");
            return;
        }
        else if (drp_nivel4.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el Nivel");
            return;
        }
        nivel = int.Parse(drp_nivel4.SelectedValue);
        sql = " and a.tn_pai_id=" + drp_pais4.SelectedValue + " and a.tn_nivel=" + nivel + " and a.tn_conta_id=" + drp_contabilidad4.SelectedValue + " and a.tn_ttr_id=" + drp_documentos4.SelectedValue + " ";
        arr_nodos = (ArrayList)DB.Get_Nodos(user, sql);
        drp_nodo_padre2.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_nodo_padre2.Items.Add(item);
        if (arr_nodos != null)
        {
            foreach (RE_GenericBean bean in arr_nodos)
            {
                item = new ListItem(bean.strC3, bean.intC1.ToString());
                drp_nodo_padre2.Items.Add(item);
            }
        }
        lb_nodos_hijos.Items.Clear();
    }
    protected void drp_nodo_padre2_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr_nodos = null;
        string sql = "";
        int nivel = 0;
        lb_nodos_hijos.Items.Clear();
        if (drp_nodo_padre2.SelectedValue == "0")
        {
            return;
        }
        else
        {
            nivel = int.Parse(drp_nivel4.SelectedValue) + 1;
            sql = " and a.tn_pai_id=" + drp_pais4.SelectedValue + " and a.tn_nivel=" + nivel + " and a.tn_padre=" + drp_nodo_padre2.SelectedValue + " and a.tn_ttr_id=" + drp_documentos4.SelectedValue + " order by a.tn_posicion asc ";
            arr_nodos = (ArrayList)DB.Get_Nodos(user, sql);
            lb_nodos_hijos.Items.Clear();
            if (arr_nodos != null)
            {
                foreach (RE_GenericBean bean in arr_nodos)
                {
                    item = new ListItem(bean.strC1, bean.intC1.ToString());
                    lb_nodos_hijos.Items.Add(item);
                }
            }
        }
    }
    protected void drp_nivel4_SelectedIndexChanged(object sender, EventArgs e)
    {
        drp_nodo_padre2.Items.Clear();
        item = new ListItem("Seleccione...", "0");
        drp_nodo_padre2.Items.Add(item);
        lb_nodos_hijos.Items.Clear();
    }
    protected void btn_up_Click(object sender, EventArgs e)
    {
        if (lb_nodos_hijos.SelectedIndex > -1)
        {
            indice = lb_nodos_hijos.SelectedIndex;
            nuevo_indice = indice - 1;
            if (nuevo_indice > -1)
            {
                item_aux = new ListItem(lb_nodos_hijos.Items[nuevo_indice].Text, lb_nodos_hijos.Items[nuevo_indice].Value);
                item = new ListItem(lb_nodos_hijos.Items[indice].Text,lb_nodos_hijos.Items[indice].Value);
                lb_nodos_hijos.Items[nuevo_indice].Text = item.Text;
                lb_nodos_hijos.Items[nuevo_indice].Value = item.Value;
                lb_nodos_hijos.Items[indice].Text = item_aux.Text;
                lb_nodos_hijos.Items[indice].Value = item_aux.Value;
                lb_nodos_hijos.SelectedIndex = nuevo_indice;
                item_aux = null;
                item = null;
            }
        }
    }
    protected void btn_down_Click(object sender, EventArgs e)
    {
        if (lb_nodos_hijos.SelectedIndex > -1)
        {
            indice = lb_nodos_hijos.SelectedIndex;
            nuevo_indice = indice + 1;
            maximo = lb_nodos_hijos.Items.Count - 1;
            if (nuevo_indice <= maximo)
            {
                item_aux = new ListItem(lb_nodos_hijos.Items[nuevo_indice].Text, lb_nodos_hijos.Items[nuevo_indice].Value);
                item = new ListItem(lb_nodos_hijos.Items[indice].Text, lb_nodos_hijos.Items[indice].Value);
                lb_nodos_hijos.Items[nuevo_indice].Text = item.Text;
                lb_nodos_hijos.Items[nuevo_indice].Value = item.Value;
                lb_nodos_hijos.Items[indice].Text = item_aux.Text;
                lb_nodos_hijos.Items[indice].Value = item_aux.Value;
                lb_nodos_hijos.SelectedIndex = nuevo_indice;
                item_aux = null;
                item = null;
            }
        }
    }
    protected void btn_mover_posicion_Click(object sender, EventArgs e)
    {
        string sql = "";
        int result = 0;
        for (int i = 0; i < lb_nodos_hijos.Items.Count; i++)
        {
            sql = " update tbl_nodos set tn_posicion=" + i + ", tn_usuario_modifica='" + user.ID + "', tn_fecha_modifica=current_date where tn_id=" + lb_nodos_hijos.Items[i].Value + " ";
            result = DB.Update_Nodos(user, sql);
        }
        Limpiar_Pantalla();
    }
    protected void btn_remove_Click(object sender, EventArgs e)
    {
        if (lb_nodos_hijos.SelectedIndex > -1)
        {
            string sql = "update tbl_nodos set tn_estado=3, tn_usuario_elimina='" + user.ID + "', tn_fecha_elimina=current_date where tn_id=" + lb_nodos_hijos.Items[lb_nodos_hijos.SelectedIndex].Value + " ";
            int result = DB.Update_Nodos(user, sql);
            if (result == -100)
            {
                WebMsgBox.Show("Existio un error al momento tratrar de remover el nodo");
                return;
            }
            else
            {
                WebMsgBox.Show("El Nodo fue removido correctamente");
                Limpiar_Pantalla();
                return;
            }           
        }
    }
    protected void btn_generar_arbol_Click(object sender, EventArgs e)
    {
        ArrayList arr_nodos = null;
        TreeNode Nodo_Padre = null;
        TreeNode Nodo_Hijo = null;
        string sql = "";
        trv_xml.Nodes.Clear();
        if (drp_pais2.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el pais.");
            return;
        }
        else if (drp_documentos2.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar el tipo de documento.");
            return;
        }
        sql = " and a.tn_pai_id=" + drp_pais2.SelectedValue + " and a.tn_ttr_id=" + drp_documentos2.SelectedValue + " and a.tn_conta_id=" + drp_contabilidad2.SelectedValue + " order by a.tn_nivel, a.tn_posicion asc  ";
        arr_nodos = (ArrayList)DB.Get_Nodos(user, sql);
        foreach (RE_GenericBean Bean in arr_nodos)
        {
            if (Bean.intC3 == 1)
            {
                Nodo_Padre = new TreeNode(Bean.strC1, Bean.intC1.ToString());
                Nodo_Hijo = (TreeNode)DB.Cargar_Nodos_Hijos(user, Nodo_Padre, arr_nodos);
                if (Nodo_Hijo.ChildNodes.Count > 0)
                {
                    Nodo_Padre.ChildNodes.Add(Nodo_Hijo);
                }
                trv_xml.Nodes.Add(Nodo_Padre);
            }
        }
    }

}