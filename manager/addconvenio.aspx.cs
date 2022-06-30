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

public partial class manager_addconvenio : System.Web.UI.Page
{
    private ArrayList tipo_convenio_arr = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        tipo_convenio_arr = (ArrayList)DB.getTipoConvenios();
        if (!Page.IsPostBack)
        {
            if ((Request.QueryString["agenteid"]==null) || (Request.QueryString["agenteid"].ToString().Equals(""))) {
                lb_msg.Text = "Debe indicar un agente para poder asociarlo a un convenio";
                return;
            }
            tb_agente_id.Text = Request.QueryString["agenteid"].ToString().Trim();
            tb_agente_id.ReadOnly = true;
            ListItem item = null;
            if ((tipo_convenio_arr != null) && (tipo_convenio_arr.Count > 0))
            {
                foreach (RE_GenericBean tipo_convenio in tipo_convenio_arr)
                {
                    item = new ListItem(tipo_convenio.strC1, tipo_convenio.intC1.ToString());
                    lb_tipo_convenio.Items.Add(item);
                }
            }
        }
    }

    protected void cmdGuardar_Click(object sender, EventArgs e)
    {
        int i = lb_tipo_convenio.SelectedIndex;
        RE_GenericBean rgbtc = (RE_GenericBean)tipo_convenio_arr[i];
        if ((rgbtc.boolC1 == true) && (int.Parse(tb_valor.Text.Trim()) > 100))
        {
            lb_msg.Text = "El valor no puede ser mayor a 100% ya que esta seleccionando un convenio tipo porcentaje";
            return;
        }
        else {
            lb_msg.Text = "";
            int result = DB.InsertUpdateAgenteConvenio(int.Parse(tb_agente_id.Text.Trim()), int.Parse(lb_origen.SelectedValue), int.Parse(lb_tipo_convenio.SelectedValue), decimal.Parse(tb_valor.Text.Trim()));
            if (result == 0)
            {
                lb_msg.Text = "Existió un problema al tratar de guardar los datos del campo, por favor intente de nuevo";
                return;
            }
            else if (result == -100)
            {
                lb_msg.Text = "Existió un problema al tratar de guardar los datos del campo, por favor intente de nuevo";
                return;
            }
            string mensaje = "<script languaje=\"JavaScript\">";
            mensaje += "window.opener.location='addagenteconvenio.aspx?id=" + tb_agente_id.Text + "';";
            mensaje += "window.close();";
            mensaje += "</script>";
            Page.RegisterClientScriptBlock("closewindow", mensaje);
        }
    }
}
