using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for FacturaBean
/// </summary>
public class FacturaBean
{
    private string correlativo = "";
    public string Correlativo
    {
        get { return correlativo; }
        set { correlativo = value; }
    }
    private string nit = "";
    public string Nit
    {
        get { return nit; }
        set { nit = value; }
    }
    private string nombre = "";
    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }
    private string direccion = "";
    public string Direccion
    {
        get { return direccion; }
        set { direccion = value; }
    }
    private string fecha_emision="";
    public string Fecha_Emision
    {
        get { return fecha_emision; }
        set { fecha_emision = value; }
    }
    private string fecha_pago="";
    public string Fecha_Pago
    {
        get { return fecha_pago; }
        set { fecha_pago = value; }
    }
    private double subtot = 0;
    public double SubTot
    {
        get { return subtot; }
        set { subtot = value; }
    }
    private double impuesto = 0;
    public double Impuesto
    {
        get { return impuesto; }
        set { impuesto = value; }
    }
    private double total = 0;
    public double Total
    {
        get { return total; }
        set { total = value; }
    }
    private string observaciones = "";
    public string Observaciones
    {
        get { return observaciones; }
        set { observaciones = value; }
    }
    private int sucid=0;
    public int SucID
    {
        get { return sucid ; }
        set { sucid = value; }
    }
    private long cliID = 0;
    public long CliID
    {
        get { return cliID; }
        set { cliID = value; }
    }
    private int monedaID=0;
    public int MonedaID
    {
        get { return monedaID; }
        set { monedaID = value; }
    }
    private int ted_id=1;
    public int TedID
    {
        get { return ted_id; }
        set { ted_id = value; }
    }
    private int requierealiasrubro = 0;
    public int alias_rubro
    {
        get { return requierealiasrubro; }
        set { requierealiasrubro = value; }
    }
    private string usuid="";
    public string UsuID
    {
        get { return usuid; }
        set { usuid = value; }
    }
    private string hbl = "";
    public string HBL
    {
        get { return hbl; }
        set { hbl = value; }
    }
    private string mbl="";
    public string MBL
    {
        get { return mbl; }
        set { mbl = value; }
    }
    private string contenedor = "";
    public string Contenedor
    {
        get { return contenedor; }
        set { contenedor = value; }
    }
    private string routing="";
    public string Routing
    {
        get { return routing; }
        set { routing = value; }
    }
    private string naviera="";
    public string Naviera
    {
        get { return naviera; }
        set { naviera = value; }
    }
    private string vapor="";
    public string Vapor
    {
        get { return vapor; }
        set { vapor = value; }
    }
    private string shipper="";
    public string Shipper
    {
        get { return shipper; }
        set { shipper = value; }
    }
    private string ordenpo="";
    public string OrdenPO
    {
        get { return ordenpo; }
        set { ordenpo = value; }
    }
    private string consignee="";
    public string Consignee
    {
        get { return consignee; }
        set { consignee = value; }
    }
    private string comodity="";
    public string Comodity
    {
        get { return comodity; }
        set { comodity = value; }
    }
    private string paquetes="";
    public string Paquetes
    {
        get { return paquetes; }
        set { paquetes = value; }
    }
    private string cantpaquetes = "";
    public string cantPaquetes
    {
        get { return cantpaquetes; }
        set { cantpaquetes = value; }
    }
    private string peso="";
    public string Peso
    {
        get { return peso; }
        set { peso = value; }
    }
    private string volumen="";
    public string Volumen
    {
        get { return volumen; }
        set { volumen = value; }
    }
    private string dua_ingreso="";
    public string Dua_Ingreso
    {
        get { return dua_ingreso; }
        set { dua_ingreso = value; }
    }
    private string dua_salida="";
    public string Dua_Salida
    {
        get { return dua_salida; }
        set { dua_salida = value; }
    }
    private string vendedor1="";
    public string Vendedor1
    {
        get { return vendedor1; }
        set { vendedor1 = value; }
    }
    private string vendedor2="";
    public string Vendedor2
    {
        get { return vendedor2; }
        set { vendedor2 = value; }
    }
    private string razon = "";
    public string Razon
    {
        get { return razon; }
        set { razon = value; }
    }
    private string referencia="";
    public string Referencia
    {
        get { return referencia; }
        set { referencia = value; }
    }
    private string serie_factura = "";
    public string Serie
    {
        get { return serie_factura; }
        set { serie_factura = value; }
    }
    private long bl_id = 0;
    public long BLID
    {
        get { return bl_id; }
        set { bl_id = value; }
    }
    private double total_bl = 0;
    public double TotalBL
    {
        get { return total_bl; }
        set { total_bl = value; }
    }
    private double comix_agente = 0;
    public double ComixAg
    {
        get { return comix_agente; }
        set { comix_agente = value; }
    }
    private Hashtable rubros_ht = null;
    public Hashtable RubrosHT
    {
        get { return rubros_ht; }
        set { rubros_ht = value; }
    }
    private ArrayList rubros_arr = null;
    public ArrayList RubrosArr
    {
        get { return rubros_arr; }
        set { rubros_arr = value; }
    }
    private ArrayList rubros_arr2 = null;
    public ArrayList RubrosArr2
    {
        get { return rubros_arr2; }
        set { rubros_arr2 = value; }
    }
    private int agente_ID = 0;
    public int AgenteID
    {
        get { return agente_ID; }
        set { agente_ID = value; }
    }
    private int facturaCorr = 0;
    public int FacturaCorr
    {
        get { return facturaCorr; }
        set { facturaCorr = value; }  
    }
    private double shipper_ID;
    public double ShipperID
    {
        get { return shipper_ID; }
        set { shipper_ID = value; }
    }
    private double consignee_ID;
    public double ConsigneeID
    {
        get { return consignee_ID; }
        set { consignee_ID = value; }
    }
    private double totalDolares;
    public double TotalDol {
        get { return totalDolares; }
        set { totalDolares = value; }
    }
    private int contribuyente;
    public int Contribuyente {
        get { return contribuyente; }
        set { contribuyente = value; }
    }

    private int importacionexportacion = 0;
    public int imp_exp
    {
        get { return importacionexportacion; }
        set { importacionexportacion = value; }
    }

    private int tipodecobro = 0;
    public int cobroID
    {
        get { return tipodecobro; }
        set { tipodecobro = value; }
    }

    private string todosjuntos = "";
    public string allIN
    {
        get { return todosjuntos; }
        set { todosjuntos = value; }
    }
    private string reciboaduana = "";
    public string ReciboAduanal
    {
        get { return reciboaduana; }
        set { reciboaduana = value; }
    }
    private int tipo_persona = 0;
    public int Tipo_Persona
    {
        get { return tipo_persona; }
        set { tipo_persona = value; }
    }
    private double subtotequivalente = 0;
    public double SubTotequivalente
    {
        get { return subtotequivalente; }
        set { subtotequivalente = value; }
    }
    private double impuesto_equivalente = 0;
    public double Impuesto_equivalente
    {
        get { return impuesto_equivalente; }
        set { impuesto_equivalente = value; }
    }
    private string nombre_agente = "";
    public string Nombre_Agente
    {
        get { return nombre_agente; }
        set { nombre_agente = value; }
    }
    private string fecha_hora = "";
    public string Fecha_Hora
    {
        get { return fecha_hora; }
        set { fecha_hora = value; }
    }
    private string nombre_cliente = "";
    public string Nombre_Cliente
    {
        get { return nombre_cliente; }
        set { nombre_cliente = value; }
    }
    private string recibo_agencia = "";
    public string Recibo_Agencia
    {
        get { return recibo_agencia; }
        set { recibo_agencia = value; }
    }
    private string valor_aduanero = "";
    public string Valor_Aduanero
    {
        get { return valor_aduanero; }
        set { valor_aduanero = value; }
    }
    private string ruc = "";
    public string Ruc
    {
        get { return ruc; }
        set { ruc = value; }
    }
    private string giro = "";
    public string Giro
    {
        get { return giro; }
        set { giro = value; }
    }
    private int tipo_factura = 0;
    public int Tipo_Factura
    {
        get { return tipo_factura; }
        set { tipo_factura = value; }
    }
    private string correlativos = "";
    public string Correlativos
    {
        get { return correlativos; }
        set { correlativos = value; }
    }
    private string poliza = "";
    public string Poliza
    {
        get { return poliza; }
        set { poliza = value; }
    }
    private bool fiscal_no_fiscal = true;
    public bool Fiscal_No_Fiscal
    {
        get { return fiscal_no_fiscal; }
        set { fiscal_no_fiscal = value; }
    }
    private string fecha_libro_compras = "";
    public string Fecha_Libro_Compras
    {
        get { return fecha_libro_compras; }
        set { fecha_libro_compras = value; }
    }
    private double afecto = 0;
    public double Afecto
    {
        get { return afecto; }
        set { afecto = value; }
    }
    private double noafecto = 0;
    public double Noafecto
    {
        get { return noafecto; }
        set { noafecto = value; }
    }
    private string ruta_pais = "";
    public string Ruta_Pais
    {
        get { return ruta_pais; }
        set { ruta_pais = value; }
    }
    private string ruta = "";
    public string Ruta
    {
        get { return ruta; }
        set { ruta = value; }
    }
    private string otras_observaciones = "";
    public string Otras_Observaciones
    {
        get { return otras_observaciones; }
        set { otras_observaciones = value; }
    }
    private int regimen_aduanero = 0;
    public int Regimen_Aduanero
    {
        get { return regimen_aduanero; }
        set { regimen_aduanero = value; }
    }
    private int blid = 0;
    public int BlId
    {
        get { return blid; }
        set { blid = value; }
    }
    private int ttoid = 0;
    public int ttoID
    {
        get { return ttoid; }
        set { ttoid = value; }
    }
    private int tipo_operacion = 0;
    public int Tipo_Operacion
    {
        get { return tipo_operacion; }
        set { tipo_operacion = value; }
    }
    private int tipo_bienserv = 0;
    public int Tipo_BienServicio
    {
        get { return tipo_bienserv; }
        set { tipo_bienserv = value; }
    }
	public FacturaBean()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private string referencia_interna = "";
    public string Referencia_Interna
    {
        get { return referencia_interna; }
        set { referencia_interna = value; }
    }
    private int factura_electronica = 0;
    public int Factura_Electronica
    {
        get { return factura_electronica; }
        set { factura_electronica = value; }
    }
    private int serie_id = 0;
    public int serieID
    {
        get { return serie_id; }
        set { serie_id = value; }
    }
    private ArrayList arreglo1 = new ArrayList();
    public ArrayList arr1
    {
        get { return arreglo1; }
        set { arreglo1 = value; }
    }
    private ArrayList arreglo2= new ArrayList();
    public ArrayList arr2
    {
        get { return arreglo2; }
        set { arreglo2 = value; }
    }
    private double totalingresosng;
    public double TotalIngresosNG
    {
        get { return totalingresosng; }
        set { totalingresosng = value; }
    }
    private string correo_electronico = "";
    public string Correo_Electronico
    {
        get { return correo_electronico; }
        set { correo_electronico = value; }
    }
    private string referencia_correo = "";
    public string Referencia_Correo
    {
        get { return referencia_correo; }
        set { referencia_correo = value; }
    }
    private string no_factura_aduana = "";
    public string No_Factura_Aduana
    {
        get { return no_factura_aduana; }
        set { no_factura_aduana = value; }
    }
    private string no_embarque = "";
    public string No_Embarque
    {
        get { return no_embarque; }
        set { no_embarque = value; }
    }
    private int tttid = 0;
    public int tttID
    {
        get { return tttid; }
        set { tttid = value; }
    }
    private string usuacepta = "";
    public string Usu_Acepta
    {
        get { return usuacepta; }
        set { usuacepta = value; }
    }
    private string fecha_acepta = "";
    public string Fecha_Acepta
    {
        get { return fecha_acepta; }
        set { fecha_acepta = value; }
    }
    private int tfa_id = 0;
    public int tfa_ID
    {
        get { return tfa_id; }
        set { tfa_id = value; }
    }
    private int pai_id = 0;
    public int Pai_ID
    {
        get { return pai_id; }
        set { pai_id = value; }
    }
    private int conta_id = 0;
    public int Conta_ID
    {
        get { return conta_id; }
        set { conta_id = value; }
    }
    private int ttc_id = 0;
    public int ttc_ID
    {
        get { return ttc_id; }
        set { ttc_id = value; }
    }
    private string ip_address = "0.0.0.0";
    public string IP_Address
    {
        get { return ip_address; }
        set { ip_address = value; }
    }
    private string poliza_seguro = "";
    public string Poliza_Seguro
    {
        get { return poliza_seguro; }
        set { poliza_seguro = value; }
    }
    private int poliza_seguro_ID = 0;
    public int Poliza_Seguro_ID
    {
        get { return poliza_seguro_ID; }
        set { poliza_seguro_ID = value; }
    }
    private int factura_ref_id = 0;
    public int Factura_Ref_ID
    {
        get { return factura_ref_id; }
        set { factura_ref_id = value; }
    }
    private string factura_ref_serie = "";
    public string Factura_Ref_Serie
    {
        get { return factura_ref_serie;  }
        set { factura_ref_serie = value; }
    }
    private int factura_ref_correlativo = 0;
    public int Factura_Ref_Correlativo
    {
        get { return factura_ref_correlativo; }
        set { factura_ref_correlativo = value; }
    }

    private string factura_ref_fecha = "";
    public string Factura_Ref_Fecha
    {
        get { return factura_ref_fecha; }
        set { factura_ref_fecha = value; }
    }

    private string factura_ref_doc = "";
    public string Factura_Ref_Doc
    {
        get { return factura_ref_doc; }
        set { factura_ref_doc = value; }
    }


    private string file = "";
    public string File
    {
        get { return file; }
        set { file = value; }
    }
}

