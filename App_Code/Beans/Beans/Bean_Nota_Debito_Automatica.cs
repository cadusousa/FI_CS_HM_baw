using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

/// <summary>
/// Summary description for Bean_Nota_Debito_Automatica
/// </summary>
public class Bean_Nota_Debito_Automatica
{
	public Bean_Nota_Debito_Automatica()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _tnd_id = 0;
    public int tnd_id
    {
        get { return _tnd_id; }
        set { _tnd_id = value; }
    }
    private string _tnd_nit = "";
    public string tnd_nit
    {
        get { return _tnd_nit; }
        set { _tnd_nit = value; }
    }
    private string _tnd_nombre = "";
    public string tnd_nombre
    {
        get { return _tnd_nombre; }
        set { _tnd_nombre = value; }
    }
    private double _tnd_cli_id = 0;
    public double tnd_cli_id
    {
        get { return _tnd_cli_id; }
        set { _tnd_cli_id = value; }
    }
    private string _tnd_fecha_emision = "";
    public string tnd_fecha_emision
    {
        get { return _tnd_fecha_emision; }
        set { _tnd_fecha_emision = value; }
    }
    private double _tnd_total = 0;
    public double tnd_total
    {
        get { return _tnd_total; }
        set { _tnd_total = value; }
    }
    private int _tnd_pai_id = 0;
    public int tnd_pai_id
    {
        get { return _tnd_pai_id; }
        set { _tnd_pai_id = value; }
    }
    private int _tnd_ted_id = 0;
    public int tnd_ted_id
    {
        get { return _tnd_ted_id; }
        set { _tnd_ted_id = value; }
    }
    private string _tnd_observacion = "";
    public string tnd_observacion
    {
        get { return _tnd_observacion; }
        set { _tnd_observacion = value; }
    }
    private string _tnd_usu_id = "";
    public string tnd_usu_id
    {
        get { return _tnd_usu_id; }
        set { _tnd_usu_id = value; }
    }
    private string _tnd_direccion = "";
    public string tnd_direccion
    {
        get { return _tnd_direccion; }
        set { _tnd_direccion = value; }
    }
    private int _tnd_moneda = 0;
    public int tnd_moneda
    {
        get { return _tnd_moneda; }
        set { _tnd_moneda = value; }
    }
    private string _tnd_hbl = "";
    public string tnd_hbl
    {
        get { return _tnd_hbl; }
        set { _tnd_hbl = value; }
    }
    private string _tnd_mbl = "";
    public string tnd_mbl
    {
        get { return _tnd_mbl; }
        set { _tnd_mbl = value; }
    }
    private string _tnd_contenedor = "";
    public string tnd_contenedor
    {
        get { return _tnd_contenedor; }
        set { _tnd_contenedor = value; }
    }
    private string _tnd_routing = "";
    public string tnd_routing
    {
        get { return _tnd_routing; }
        set { _tnd_routing = value; }
    }
    private string _tnd_referencia = "";
    public string tnd_referencia
    {
        get { return _tnd_referencia; }
        set { _tnd_referencia = value; }
    }
    private string _tnd_serie = "";
    public string tnd_serie
    {
        get { return _tnd_serie; }
        set { _tnd_serie = value; }
    }
    private int _tnd_suc_id = 0;
    public int tnd_suc_id
    {
        get { return _tnd_suc_id; }
        set { _tnd_suc_id = value; }
    }
    private int _tnd_correlativo = 0;
    public int tnd_correlativo
    {
        get { return _tnd_correlativo; }
        set { _tnd_correlativo = value; }
    }
    private int _tnd_tpi_id = 0;
    public int tnd_tpi_id
    {
        get { return _tnd_tpi_id; }
        set { _tnd_tpi_id = value; }
    }
    private int _tnd_tcon_id = 0;
    public int tnd_tcon_id
    {
        get { return _tnd_tcon_id; }
        set { _tnd_tcon_id = value; }
    }
    private string _tnd_fecha_pago = "";
    public string tnd_fecha_pago
    {
        get { return _tnd_fecha_pago; }
        set { _tnd_fecha_pago = value; }
    }
    private double _tnd_sub_total = 0;
    public double tnd_sub_total
    {
        get { return _tnd_sub_total; }
        set { _tnd_sub_total = value; }
    }
    private double _tnd_impuesto = 0;
    public double tnd_impuesto
    {
        get { return _tnd_impuesto; }
        set { _tnd_impuesto = value; }
    }
    private string _tnd_naviera = "";
    public string tnd_naviera
    {
        get { return _tnd_naviera; }
        set { _tnd_naviera = value; }
    }
    private string _tnd_vapor = "";
    public string tnd_vapor
    {
        get { return _tnd_vapor; }
        set { _tnd_vapor = value; }
    }
    private string _tnd_shipper = "";
    public string tnd_shipper
    {
        get { return _tnd_shipper; }
        set { _tnd_shipper = value; }
    }
    private string _tnd_ordenpo = "";
    public string tnd_ordenpo
    {
        get { return _tnd_ordenpo; }
        set { _tnd_ordenpo = value; }
    }
    private string _tnd_consignee = "";
    public string tnd_consignee
    {
        get { return _tnd_consignee; }
        set { _tnd_consignee = value; }
    }
    private string _tnd_comodity = "";
    public string tnd_comodity
    {
        get { return _tnd_comodity; }
        set { _tnd_comodity = value; }
    }
    private string _tnd_paquetes = "";
    public string tnd_paquetes
    {
        get { return _tnd_paquetes; }
        set { _tnd_paquetes = value; }
    }
    private string _tnd_peso = "";
    public string tnd_peso
    {
        get { return _tnd_peso; }
        set { _tnd_peso = value; }
    }
    private string _tnd_dua_salida = "";
    public string tnd_dua_salida
    {
        get { return _tnd_dua_salida; }
        set { _tnd_dua_salida = value; }
    }
    private string _tnd_vendedor1 = "";
    public string tnd_vendedor1
    {
        get { return _tnd_vendedor1; }
        set { _tnd_vendedor1 = value; }
    }
    private string _tnd_vendedor2 = "";
    public string tnd_vendedor2
    {
        get { return _tnd_vendedor2; }
        set { _tnd_vendedor2 = value; }
    }
    private string _tnd_razon = "";
    public string tnd_razon
    {
        get { return _tnd_razon; }
        set { _tnd_razon = value; }
    }
    private double _tnd_id_shipper = 0;
    public double tnd_id_shipper
    {
        get { return _tnd_id_shipper; }
        set { _tnd_id_shipper = value; }
    }
    private double _tnd_id_consignee = 0;
    public double tnd_id_consignee
    {
        get { return _tnd_id_consignee; }
        set { _tnd_id_consignee = value; }
    }
    private double _tnd_sub_total_eq = 0;
    public double tnd_sub_total_eq
    {
        get { return _tnd_sub_total_eq; }
        set { _tnd_sub_total_eq = value; }
    }
    private double _tnd_impuesto_eq = 0;
    public double tnd_impuesto_eq
    {
        get { return _tnd_impuesto_eq; }
        set { _tnd_impuesto_eq = value; }
    }
    private double _tnd_total_eq = 0;
    public double tnd_total_eq
    {
        get { return _tnd_total_eq; }
        set { _tnd_total_eq = value; }
    }
    private int _tnd_tie_id = 0;
    public int tnd_tie_id
    {
        get { return _tnd_tie_id; }
        set { _tnd_tie_id = value; }
    }
    private int _tnd_ttc_id = 0;
    public int tnd_ttc_id
    {
        get { return _tnd_ttc_id; }
        set { _tnd_ttc_id = value; }
    }
    private string _tnd_allin = "";
    public string tnd_allin
    {
        get { return _tnd_allin; }
        set { _tnd_allin = value; }
    }
    private string _tnd_reciboaduana = "";
    public string tnd_reciboaduana
    {
        get { return _tnd_reciboaduana; }
        set { _tnd_reciboaduana = value; }
    }
    private string _tnd_volumen = "";
    public string tnd_volumen
    {
        get { return _tnd_volumen; }
        set { _tnd_volumen = value; }
    }
    private string _tnd_dua_ingreso = "";
    public string tnd_dua_ingreso
    {
        get { return _tnd_dua_ingreso; }
        set { _tnd_dua_ingreso = value; }
    }
    private string _tnd_cant_paquetes = "";
    public string tnd_cant_paquetes
    {
        get { return _tnd_cant_paquetes; }
        set { _tnd_cant_paquetes = value; }
    }
    private int _tnd_agente_id = 0;
    public int tnd_agente_id
    {
        get { return _tnd_agente_id; }
        set { _tnd_agente_id = value; }
    }
    private string _tnd_agente = "";
    public string tnd_agente
    {
        get { return _tnd_agente; }
        set { _tnd_agente = value; }
    }
    private bool _tnd_fiscal = true;
    public bool tnd_fiscal
    {
        get { return _tnd_fiscal; }
        set { _tnd_fiscal = value; }
    }
    private string _tnd_fecha_libro_compras = "";
    public string tnd_fecha_libro_compras
    {
        get { return _tnd_fecha_libro_compras; }
        set { _tnd_fecha_libro_compras = value; }
    }
    private int _tnd_blid = 0;
    public int tnd_blid
    {
        get { return _tnd_blid; }
        set { _tnd_blid = value; }
    }
    private int _tnd_tto_id = 11;
    public int tnd_tto_id
    {
        get { return _tnd_tto_id; }
        set { _tnd_tto_id = value; }
    }
    private int _tnd_bien_serv = 2;
    public int tnd_bien_serv
    {
        get { return _tnd_bien_serv; }
        set { _tnd_bien_serv = value; }
    }
    private string _tnd_esignature = "-";
    public string tnd_esignature
    {
        get { return _tnd_esignature; }
        set { _tnd_esignature = value; }
    }
    private int _tnd_fac_electronica = 0;
    public int tnd_fac_electronica
    {
        get { return _tnd_fac_electronica; }
        set { _tnd_fac_electronica = value; }
    }
    private string _tnd_internal_reference = "0";
    public string tnd_internal_reference
    {
        get { return _tnd_internal_reference; }
        set { _tnd_internal_reference = value; }
    }
    private string _tnd_guid = "0";
    public string tnd_guid
    {
        get { return _tnd_guid; }
        set { _tnd_guid = value; }
    }
    private string _tnd_correo_documento_electronico = "-";
    public string tnd_correo_documento_electronico
    {
        get { return _tnd_correo_documento_electronico; }
        set { _tnd_correo_documento_electronico = value; }
    }
    private string _tnd_referencia_correo = "-";
    public string tnd_referencia_correo
    {
        get { return _tnd_referencia_correo; }
        set { _tnd_referencia_correo = value; }
    }
    private string _tnd_innerxml = "";
    public string tnd_innerxml
    {
        get { return _tnd_innerxml; }
        set { _tnd_innerxml = value; }
    }
    private string _tnd_fecha_batch = "";
    public string tnd_fecha_batch
    {
        get { return _tnd_fecha_batch; }
        set { _tnd_fecha_batch = value; }
    }
    private int _tnd_tti_id = 2;
    public int tnd_tti_id
    {
        get { return _tnd_tti_id; }
        set { _tnd_tti_id = value; }
    }
    private ArrayList _Arr_Detalle_Facturacion = new ArrayList();
    public ArrayList Arr_Detalle_Facturacion
    {
        get { return _Arr_Detalle_Facturacion; }
        set { _Arr_Detalle_Facturacion = value; }
    }
    private ArrayList cuentas_abono = null;
    public ArrayList ctas_abono
    {
        get { return cuentas_abono; }
        set { cuentas_abono = value; }
    }
    private int _tnd_ttt_id = 0;
    public int tnd_ttt_id
    {
        get { return _tnd_ttt_id; }
        set { _tnd_ttt_id = value; }
    }
    private int _tnd_ttd_id = 1;
    public int tnd_ttd_id
    {
        get { return _tnd_ttd_id; }
        set { _tnd_ttd_id = value; }
    }
    private string _tnd_ip_address = "0.0.0.0";
    public string tnd_ip_address
    {
        get { return _tnd_ip_address; }
        set { _tnd_ip_address = value; }
    }
    private string _tnd_poliza_seguro = "";
    public string tnd_poliza_seguro
    {
        get { return _tnd_poliza_seguro; }
        set { _tnd_poliza_seguro = value; }
    }
}
