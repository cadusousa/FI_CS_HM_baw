using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

/// <summary>
/// Summary description for Bean_Factura_Automatica
/// </summary>
public class Bean_Factura_Automatica
{
	public Bean_Factura_Automatica()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _tfa_id = 0;
    public int tfa_id
    {
        get { return _tfa_id; }
        set { _tfa_id = value; }
    }
    private string _tfa_correlativo = "";
    public string tfa_correlativo
    {
        get { return _tfa_correlativo; }
        set { _tfa_correlativo = value; }
    }
    private string _tfa_nit = "";
    public string tfa_nit
    {
        get { return _tfa_nit; }
        set { _tfa_nit = value; }
    }
    private string _tfa_nombre = "";
    public string tfa_nombre
    {
        get { return _tfa_nombre; }
        set { _tfa_nombre = value; }
    }
    private string _tfa_direccion = "";
    public string tfa_direccion
    {
        get { return _tfa_direccion; }
        set { _tfa_direccion = value; }
    }
    private string _tfa_fecha_emision = "";
    public string tfa_fecha_emision
    {
        get { return _tfa_fecha_emision; }
        set { _tfa_fecha_emision = value; }
    }
    private string _tfa_fecha_pago = "";
    public string tfa_fecha_pago
    {
        get { return _tfa_fecha_pago; }
        set { _tfa_fecha_pago = value; }
    }
    private double _tfa_sub_total = 0;
    public double tfa_sub_total
    {
        get { return _tfa_sub_total; }
        set { _tfa_sub_total = value; }
    }
    private double _tfa_impuesto = 0;
    public double tfa_impuesto
    {
        get { return _tfa_impuesto; }
        set { _tfa_impuesto = value; }
    }
    private double _tfa_total = 0;
    public double tfa_total
    {
        get { return _tfa_total; }
        set { _tfa_total = value; }
    }
    private string _tfa_observacion = "";
    public string tfa_observacion
    {
        get { return _tfa_observacion; }
        set { _tfa_observacion = value; }
    }
    private int _tfa_suc_id = 0;
    public int tfa_suc_id
    {
        get { return _tfa_suc_id; }
        set { _tfa_suc_id = value; }
    }
    private int _tfa_cli_id = 0;
    public int tfa_cli_id
    {
        get { return _tfa_cli_id; }
        set { _tfa_cli_id = value; }
    }
    private int _tfa_moneda = 0;
    public int tfa_moneda
    {
        get { return _tfa_moneda; }
        set { _tfa_moneda = value; }
    }
    private int _tfa_ted_id = 0;
    public int tfa_ted_id
    {
        get { return _tfa_ted_id; }
        set { _tfa_ted_id = value; }
    }
    private string _tfa_usu_id = "";
    public string tfa_usu_id
    {
        get { return _tfa_usu_id; }
        set { _tfa_usu_id = value; }
    }
    private string _tfa_hbl = "";
    public string tfa_hbl
    {
        get { return _tfa_hbl; }
        set { _tfa_hbl = value; }
    }
    private string _tfa_mbl = "";
    public string tfa_mbl
    {
        get { return _tfa_mbl; }
        set { _tfa_mbl = value; }
    }
    private string _tfa_contenedor = "";
    public string tfa_contenedor
    {
        get { return _tfa_contenedor; }
        set { _tfa_contenedor = value; }
    }
    private string _tfa_routing = "";
    public string tfa_routing
    {
        get { return _tfa_routing; }
        set { _tfa_routing = value; }
    }
    private string _tfa_naviera = "";
    public string tfa_naviera
    {
        get { return _tfa_naviera; }
        set { _tfa_naviera = value; }
    }
    private string _tfa_vapor = "";
    public string tfa_vapor
    {
        get { return _tfa_vapor; }
        set { _tfa_vapor = value; }
    }
    private string _tfa_shipper = "";
    public string tfa_shipper
    {
        get { return _tfa_shipper; }
        set { _tfa_shipper = value; }
    }
    private string _tfa_ordenpo = "";
    public string tfa_ordenpo
    {
        get { return _tfa_ordenpo; }
        set { _tfa_ordenpo = value; }
    }
    private string _tfa_consignee = "";
    public string tfa_consignee
    {
        get { return _tfa_consignee; }
        set { _tfa_consignee = value; }
    }
    private string _tfa_comodity = "";
    public string tfa_comodity
    {
        get { return _tfa_comodity; }
        set { _tfa_comodity = value; }
    }
    private string _tfa_paquetes = "";
    public string tfa_paquetes
    {
        get { return _tfa_paquetes; }
        set { _tfa_paquetes = value; }
    }
    private string _tfa_peso = "";
    public string tfa_peso
    {
        get { return _tfa_peso; }
        set { _tfa_peso = value; }
    }
    private string _tfa_volumen = "";
    public string tfa_volumen
    {
        get { return _tfa_volumen; }
        set { _tfa_volumen = value; }
    }
    private string _tfa_dua_ingreso = "";
    public string tfa_dua_ingreso
    {
        get { return _tfa_dua_ingreso; }
        set { _tfa_dua_ingreso = value; }
    }
    private string _tfa_dua_salida = "";
    public string tfa_dua_salida
    {
        get { return _tfa_dua_salida; }
        set { _tfa_dua_salida = value; }
    }
    private string _tfa_vendedor1 = "";
    public string tfa_vendedor1
    {
        get { return _tfa_vendedor1; }
        set { _tfa_vendedor1 = value; }
    }
    private string _tfa_vendedor2 = "";
    public string tfa_vendedor2
    {
        get { return _tfa_vendedor2; }
        set { _tfa_vendedor2 = value; }
    }
    private string _tfa_razon = "";
    public string tfa_razon
    {
        get { return _tfa_razon; }
        set { _tfa_razon = value; }
    }
    private string _tfa_referencia = "";
    public string tfa_referencia
    {
        get { return _tfa_referencia; }
        set { _tfa_referencia = value; }
    }
    private string _tfa_serie = "";
    public string tfa_serie
    {
        get { return _tfa_serie; }
        set { _tfa_serie = value; }
    }
    private double _tfa_id_shipper = 0;
    public double tfa_id_shipper
    {
        get { return _tfa_id_shipper; }
        set { _tfa_id_shipper = value; }
    }
    private double _tfa_id_consignee = 0;
    public double tfa_id_consignee
    {
        get { return _tfa_id_consignee; }
        set { _tfa_id_consignee = value; }
    }
    private int _tfa_pai_id = 0;
    public int tfa_pai_id
    {
        get { return _tfa_pai_id; }
        set { _tfa_pai_id = value; }
    }
    private int _tfa_conta_id = 0;
    public int tfa_conta_id
    {
        get { return _tfa_conta_id; }
        set { _tfa_conta_id = value; }
    }
    private double _tfa_sub_total_eq = 0;
    public double tfa_sub_total_eq
    {
        get { return _tfa_sub_total_eq; }
        set { _tfa_sub_total_eq = value; }
    }
    private double _tfa_impuesto_eq = 0;
    public double tfa_impuesto_eq
    {
        get { return _tfa_impuesto_eq; }
        set { _tfa_impuesto_eq = value; }
    }
    private double _tfa_total_eq = 0;
    public double tfa_total_eq
    {
        get { return _tfa_total_eq; }
        set { _tfa_total_eq = value; }
    }
    private int _tfa_tie_id = 0;
    public int tfa_tie_id
    {
        get { return _tfa_tie_id; }
        set { _tfa_tie_id = value; }
    }
    private int _tfa_ttc_id = 0;
    public int tfa_ttc_id
    {
        get { return _tfa_ttc_id; }
        set { _tfa_ttc_id = value; }
    }
    private string _tfa_allin = "";
    public string tfa_allin
    {
        get { return _tfa_allin; }
        set { _tfa_allin = value; }
    }
    private string _tfa_reciboaduana = "";
    public string tfa_reciboaduana
    {
        get { return _tfa_reciboaduana; }
        set { _tfa_reciboaduana = value; }
    }
    private string _tfa_cant_paquetes = "";
    public string tfa_cant_paquetes
    {
        get { return _tfa_cant_paquetes; }
        set { _tfa_cant_paquetes = value; }
    }
    private int _tfa_agent_id = 0;
    public int tfa_agent_id
    {
        get { return _tfa_agent_id; }
        set { _tfa_agent_id = value; }
    }
    private string _tfa_agente = "";
    public string tfa_agente
    {
        get { return _tfa_agente; }
        set { _tfa_agente = value; }
    }
    private string _tfa_recibo_agencia = "";
    public string tfa_recibo_agencia
    {
        get { return _tfa_recibo_agencia; }
        set { _tfa_recibo_agencia = value; }
    }
    private string _tfa_valor_aduanero = "";
    public string tfa_valor_aduanero
    {
        get { return _tfa_valor_aduanero; }
        set { _tfa_valor_aduanero = value; }
    }
    private string _tfa_ruc = "";
    public string tfa_ruc
    {
        get { return _tfa_ruc; }
        set { _tfa_ruc = value; }
    }
    private string _tfa_giro = "";
    public string tfa_giro
    {
        get { return _tfa_giro; }
        set { _tfa_giro = value; }
    }
    private int _tfa_ttf_id = 1;
    public int tfa_ttf_id
    {
        get { return _tfa_ttf_id; }
        set { _tfa_ttf_id = value; }
    }
    private string _tfa_ruta_pais = "";
    public string tfa_ruta_pais
    {
        get { return _tfa_ruta_pais; }
        set { _tfa_ruta_pais = value; }
    }
    private string _tfa_ruta = "";
    public string tfa_ruta
    {
        get { return _tfa_ruta; }
        set { _tfa_ruta = value; }
    }
    private string _tfa_observacion2 = "";
    public string tfa_observacion2
    {
        get { return _tfa_observacion2; }
        set { _tfa_observacion2 = value; }
    }
    private int _tfa_tra_id = 0;
    public int tfa_tra_id
    {
        get { return _tfa_tra_id; }
        set { _tfa_tra_id = value; }
    }
    private int _tfa_blid = 0;
    public int tfa_blid
    {
        get { return _tfa_blid; }
        set { _tfa_blid = value; }
    }
    private int _tfa_tto_id = 10;
    public int tfa_tto_id
    {
        get { return _tfa_tto_id; }
        set { _tfa_tto_id = value; }
    }
    private string _tfa_esignature = "-";
    public string tfa_esignature
    {
        get { return _tfa_esignature; }
        set { _tfa_esignature = value; }
    }
    private int _tfa_fac_electronica = 0;
    public int tfa_fac_electronica
    {
        get { return _tfa_fac_electronica; }
        set { _tfa_fac_electronica = value; }
    }
    private string _tfa_internal_reference = "0";
    public string tfa_internal_reference
    {
        get { return _tfa_internal_reference; }
        set { _tfa_internal_reference = value; }
    }
    private string _tfa_guid = "0";
    public string tfa_guid
    {
        get { return _tfa_guid; }
        set { _tfa_guid = value; }
    }
    private string _tfa_correo_documento_electronico = "-";
    public string tfa_correo_documento_electronico
    {
        get { return _tfa_correo_documento_electronico; }
        set { _tfa_correo_documento_electronico = value; }
    }
    private string _tfa_referencia_correo = "-";
    public string tfa_referencia_correo
    {
        get { return _tfa_referencia_correo; }
        set { _tfa_referencia_correo = value; }
    }
    private string _tfa_innerxml = "-";
    public string tfa_innerxml
    {
        get { return _tfa_innerxml; }
        set { _tfa_innerxml = value; }
    }
    private string _tfa_fecha_batch = "";
    public string tfa_fecha_batch
    {
        get { return _tfa_fecha_batch; }
        set { _tfa_fecha_batch = value; }
    }
    private int _tfa_tti_id = 2;
    public int tfa_tti_id
    {
        get { return _tfa_tti_id; }
        set { _tfa_tti_id = value; }
    }
    private string _tfa_correlativo_electronico = "0";
    public string tfa_correlativo_electronico
    {
        get { return _tfa_correlativo_electronico; }
        set { _tfa_correlativo_electronico = value; }
    }
    private string _tfa_no_factura_aduana = "-";
    public string tfa_no_factura_aduana
    {
        get { return _tfa_no_factura_aduana; }
        set { _tfa_no_factura_aduana = value; }
    }
    private string _tfa_no_embarque = "-";
    public string tfa_no_embarque
    {
        get { return _tfa_no_embarque; }
        set { _tfa_no_embarque = value; }
    }
    private int _tfa_tpi_id = 3;
    public int tfa_tpi_id
    {
        get { return _tfa_tpi_id; }
        set { _tfa_tpi_id = value; }
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
    private int _tfa_ttt_id = 0;
    public int tfa_ttt_id
    {
        get { return _tfa_ttt_id; }
        set { _tfa_ttt_id = value; }
    }
    private string _tfa_ip_address = "0.0.0.0";
    public string tfa_ip_address
    {
        get { return _tfa_ip_address; }
        set { _tfa_ip_address = value; }
    }
    private string _tfa_poliza_seguro = "-";
    public string tfa_poliza_seguro
    {
        get { return _tfa_poliza_seguro; }
        set { _tfa_poliza_seguro = value; }
    }
}