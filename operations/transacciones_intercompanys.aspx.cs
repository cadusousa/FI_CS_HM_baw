using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;


public partial class operations_transacciones_intercompanys : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (!IsPostBack)
        {
            Obtener_Transacciones_Enviadas_Colectar();
        }
    }
    protected void Obtener_Transacciones_Enviadas_Colectar()
    {
        int correlativo = 1;
        DataTable dt_intercompany_operativo = new DataTable();
        dt_intercompany_operativo.Columns.Add("ORIGEN");
        dt_intercompany_operativo.Columns.Add("COBRO");
        dt_intercompany_operativo.Columns.Add("SERIE");
        dt_intercompany_operativo.Columns.Add("CORR");
        dt_intercompany_operativo.Columns.Add("ESTADO");
        dt_intercompany_operativo.Columns.Add("COLECTAR EN");
        dt_intercompany_operativo.Columns.Add("PAGO");
        dt_intercompany_operativo.Columns.Add("SERIE ");
        dt_intercompany_operativo.Columns.Add("CORR ");
        dt_intercompany_operativo.Columns.Add("ESTADO ");
        dt_intercompany_operativo.Columns.Add("TERCEROS");
        dt_intercompany_operativo.Columns.Add("CLIENTE");
        dt_intercompany_operativo.Columns.Add("SERIE  ");
        dt_intercompany_operativo.Columns.Add("CORR  ");
        dt_intercompany_operativo.Columns.Add("ESTADO   ");
        dt_intercompany_operativo.Columns.Add("HBL");
        ArrayList Arr_result = Contabilizacion_Automatica_CAD.Obtener_Log_Intercompany_Operativo_X_Empresa(user.PaisID);
        foreach (RE_GenericBean Bean in Arr_result)
        {
            object[] Obj = { user.pais.Nombre_Sistema, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC2, Bean.strC7, Bean.strC8, Bean.strC9, Bean.strC10, Bean.strC11, Bean.strC16, Bean.strC12, Bean.strC13, Bean.strC14, Bean.strC15 };
            dt_intercompany_operativo.Rows.Add(Obj);
            correlativo++;
        }
        gv_transacciones_intercompany_operativo.DataSource = dt_intercompany_operativo;
        gv_transacciones_intercompany_operativo.DataBind();



        correlativo = 1;
        DataTable dt_intercompany_administrativo = new DataTable();
        dt_intercompany_administrativo.Columns.Add("ORIGEN");
        dt_intercompany_administrativo.Columns.Add("COBRO");
        dt_intercompany_administrativo.Columns.Add("SERIE");
        dt_intercompany_administrativo.Columns.Add("CORR");
        dt_intercompany_administrativo.Columns.Add("ESTADO");
        dt_intercompany_administrativo.Columns.Add("DESTINO");
        dt_intercompany_administrativo.Columns.Add("PAGO");
        dt_intercompany_administrativo.Columns.Add("SERIE ");
        dt_intercompany_administrativo.Columns.Add("CORR ");
        dt_intercompany_administrativo.Columns.Add("ESTADO ");
        dt_intercompany_administrativo.Columns.Add("HBL");
        Arr_result = null;
        Arr_result = Contabilizacion_Automatica_CAD.Obtener_Log_Intercompany_Administrativo_X_Empresa(user.PaisID);
        foreach (RE_GenericBean Bean in Arr_result)
        {
            object[] Obj = { Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.strC7, Bean.strC8, Bean.strC9, Bean.strC10, Bean.strC11, Bean.strC12 };
            dt_intercompany_administrativo.Rows.Add(Obj);
            correlativo++;
        }
        gv_transacciones_intercompany_administrativo.DataSource = dt_intercompany_administrativo;
        gv_transacciones_intercompany_administrativo.DataBind();
    }
}