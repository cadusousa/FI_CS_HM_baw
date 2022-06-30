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

public partial class operations_provisionar_BL2 : System.Web.UI.Page
{
    

    int blID = 0;
    int sisID = 0;
    int Tipo_Operacion = 0;
    string paisISO = "";
    int Conta_Id = 0;
    PaisBean Pais_Bean = new PaisBean();
    UsuarioBean user;
    
    string usuID = "";
    string[,] M_Monedas = new string[6, 2] { {"1","GTQ"}, {"3","LPS"}, {"4","NIO"}, {"5","CRC"},{"7","BZD"}, {"8","USD"}};
    string[,] M_Tipo_Persona = new string[4, 3] { { "Linea Aerea", "0", "6" }, { "Agente", "1", "2" }, { "Naviera", "2", "5" }, { "Proveedor", "3", "4" } };//Tipo-ID_Master,ID_BAW
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.Count == 5)
        {
            blID = int.Parse(Request.QueryString["blID"].ToString());
            sisID = int.Parse(Request.QueryString["sis"].ToString());
            Tipo_Operacion = int.Parse(Request.QueryString["tipo"].ToString());
            paisISO = Request.QueryString["paisID"].ToString();
            usuID = Request.QueryString["usuID"].ToString();
            Cargar_Informacion();
            if (!IsPostBack)
            {
                Cargar_Costos();
            }
        }
        
        user = (UsuarioBean)Session["usuario"];

        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "** TITULO **", ""); //pais, sistema, doc_id, titulo, edicion
        Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
        Image1.Height = 63;
        Image1.Width = 237; 
		

    }
    protected void Cargar_Informacion()
    {
        #region Cargar Informacion
        ArrayList Arr = (ArrayList)DB.getSistemas();
        foreach (RE_GenericBean Bean in Arr)
        {
            if (Bean.intC1 == sisID)
            {
                lbl_sistema.Text = Bean.strC1;
            }
        }
        Arr = (ArrayList)DB.getTipo_Operacion();
        foreach (RE_GenericBean Bean in Arr)
        {
            if (Bean.intC1 == Tipo_Operacion)
            {
                lbl_tipo_operacion.Text = Bean.strC1;
            }
        }   
        Arr = (ArrayList)DB.getPaises("");
        foreach (PaisBean Pais in Arr)
        {
            if (Pais.ISO == paisISO)
            {
                Pais_Bean = Pais;
                Pais_Bean.TipoCambio = DB.getTipoCambioHoy(Pais_Bean.ID);
            }
        }
        lbl_mbl.Text = blID.ToString();
        lbl_pais.Text = Pais_Bean.Nombre;
        #endregion
    }
    protected void Cargar_Costos()
    {
        #region Cargar Costos
        DataTable dt_costos = (DataTable)DB.Get_CostosBy_Traficos_View(sisID, Tipo_Operacion, blID, "ventas_" + Pais_Bean.ISO.ToLower());
        gv_costos.DataSource = dt_costos;
        gv_costos.DataBind();
        #endregion
    }
    protected int Traducir_Tipo_Moneda(string Moneda)
    {
        #region Traducir Moneda
        int MonID = 0;
        for (int i = 0; i < 6; i++)
        {
            if (M_Monedas[i, 1] == Moneda)
            {
                MonID = int.Parse(M_Monedas[i, 0]);
            }
        }
        return MonID;
        #endregion
    }
    protected int Traducir_Tipo_Persona(string ID)
    {
        #region Traducir Tipo de Persona
        int TPI = 0;
        for (int i = 0; i < 4; i++)
        {
            if (M_Tipo_Persona[i, 1] == ID)
            {
                TPI = int.Parse(M_Tipo_Persona[i, 2]);
            }
        }
        return TPI;
        #endregion
    }
    protected int Traducir_Imp_Exp(string Imp_Exp)
    {
        #region Traducir Importacion Exportacion
        int resultado = 0;
        if (Imp_Exp == "True")
        {
            resultado = 1;//Importacion
        }
        else
        {
            resultado = 2;//Exportacion
        }
        return resultado;
        #endregion
    }
    protected Rubros Calcular_Impuestos(long RubroID, double Monto, int ContaID, int Impuesto_Proveedor, int servicioID, int operacionID, int sistemaID, int CostoID)
    {
        #region Calcular Subtotal, Impuesto de Rubro por Pais
        Rubros Rubros_Bean = null;
        Rubros Rubro = null;
        Rubro = new Rubros();
        Rubro.rubroID = RubroID;
        Rubros_Bean = (Rubros)DB.ExistRubroPais(Rubro, Pais_Bean.ID);
        Rubros_Bean.rubroTypeID = servicioID;
        Rubros_Bean.rubroOperacion = operacionID;
        Rubros_Bean.rubroSistema = sistemaID;
        Rubros_Bean.rubroCostoID = CostoID;
        if (Rubros_Bean == null)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog("Error, El BL no tiene todos los rubros registrados en contabilidad para este pais.");
            return null;
        }
        if ((Rubros_Bean.CobIva == 1) && (ContaID != 2) && (Impuesto_Proveedor != 1))//si debe cobrar iva y el rubro no esta en dolares y no es excento
        {
            if (Rubros_Bean.IvaInc == 1)
            {
                Rubros_Bean.rubroTot = Monto;
                Rubros_Bean.rubroSubTot = Math.Round(Monto * (double)(1 / (1 + Pais_Bean.Impuesto)), 2);
                Rubros_Bean.rubroImpuesto = Math.Round(Monto - Rubros_Bean.rubroSubTot, 2);
            }
            else
            {
                Rubros_Bean.rubroImpuesto = Math.Round(Monto * (double)Pais_Bean.Impuesto, 2);
                Rubros_Bean.rubroSubTot = Math.Round(Monto, 2);
                Rubros_Bean.rubroTot = Math.Round(Rubros_Bean.rubroSubTot + Rubros_Bean.rubroImpuesto, 2);
            }
            Rubros_Bean.rubroTotD = Math.Round((double)Rubros_Bean.rubroTot / (double)Pais_Bean.TipoCambio, 2);
        }
        else
        {
            Rubros_Bean.rubroTot = Monto;
            Rubros_Bean.rubroSubTot = Monto;
            Rubros_Bean.rubroImpuesto = 0;
            Rubros_Bean.rubroTotD = Math.Round((double)Rubros_Bean.rubroTot * (double)Pais_Bean.TipoCambio, 2);
        }
        return Rubros_Bean;
        #endregion
    }
    protected int Cargar_Datos_Sucursal(int ContaID)
    {
        #region Cargar Sucursal
        int SucID = 0;
        ArrayList Arr_Sucursales = (ArrayList)DB.getSucursales(" and  suc_pai_id=" + Pais_Bean.ID + " and suc_nombre='SISTEMAS' ");
        if (Arr_Sucursales.Count == 0)
        {
            #region Crear Sucursal
            SucursalBean sucbean = new SucursalBean();
            sucbean.Nombre = "SISTEMAS";
            sucbean.paisID = Pais_Bean.ID;
            int result = DB.InsertUpdateSucursal(sucbean);
            #endregion
            #region Obtener ID de la Sucursal
            Arr_Sucursales = (ArrayList)DB.getSucursales(" and  suc_pai_id=" + Pais_Bean.ID + " and suc_nombre='SISTEMAS' ");
            foreach (SucursalBean Bean in Arr_Sucursales)
            {
                SucID = Bean.ID;
            }
            #endregion
            #region Crear Series y Secuencias
            RE_GenericBean Bean_Suc = (RE_GenericBean)DB.Get_Serie_CorrelativoBy_Traficos(SucID, Conta_Id, 0);
            if (Bean_Suc == null)
            {
                int Resultado = DB.Crear_Series_Provisiones_Automaticas(SucID, Pais_Bean, Tipo_Operacion, Conta_Id, 0);
            }
            #endregion
        }
        else
        {
            #region Obtener ID de la Sucursal
            Arr_Sucursales = (ArrayList)DB.getSucursales(" and  suc_pai_id=" + Pais_Bean.ID + " and suc_nombre='SISTEMAS' ");
            foreach (SucursalBean Bean in Arr_Sucursales)
            {
                SucID = Bean.ID;
            }
            #endregion
            #region Crear Series y Secuencias
            RE_GenericBean Bean_Suc = (RE_GenericBean)DB.Get_Serie_CorrelativoBy_Traficos(SucID, Conta_Id, 0);
            if (Bean_Suc == null)
            {
                int Resultado = DB.Crear_Series_Provisiones_Automaticas(SucID, Pais_Bean, Tipo_Operacion, Conta_Id, 0);
            }
            #endregion
        }
        return SucID;
        #endregion
    }
    protected RE_GenericBean Calcular_Totales(RE_GenericBean Provision, int Conta_Id, int ImpuestoProveedor)
    {
        #region Calcular Totales
        RE_GenericBean Resultado = new RE_GenericBean();
        double Total = 0, Total_Equivalente = 0, Impuesto = 0, Afecto = 0, Noafecto = 0;
        foreach (Rubros rubro in Provision.arr1)
        {
            if ((rubro.CobIva == 1) && (Conta_Id != 2) && (ImpuestoProveedor != 1))//si debe cobrar iva y el rubro no esta en dolares y no es excento
            {
                Afecto += rubro.rubroSubTot;
            }
            else
            {
                Noafecto += rubro.rubroSubTot;
            }
            Total += rubro.rubroTot;
            Impuesto += rubro.rubroImpuesto;

        }
        if (Conta_Id == 2)
        {
            Total_Equivalente = Math.Round((double)Total * (double)Pais_Bean.TipoCambio, 2);
        }
        else
        {
            Total_Equivalente = Math.Round((double)Total / (double)Pais_Bean.TipoCambio, 2);
        }
        Resultado.decC1 = (decimal)Total;
        Resultado.decC2 = (decimal)Afecto;
        Resultado.decC3 = (decimal)Noafecto;
        Resultado.decC4 = (decimal)Impuesto;
        Resultado.decC5 = (decimal)Total_Equivalente;
        return Resultado;
        #endregion
    }
}
