using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

public partial class IA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ArrayList Arr_Resultado = Contabilizacion_Automatica_CN.Generar_Contabilizacion_Automatica_Intercompany_Administrativo_Manual();
        Label1.Text = Arr_Resultado[0].ToString() + Arr_Resultado[1].ToString() + Arr_Resultado[2].ToString() + Arr_Resultado[3].ToString();
    }
}