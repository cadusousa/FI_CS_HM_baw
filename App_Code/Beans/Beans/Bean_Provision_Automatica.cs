using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

/// <summary>
/// Summary description for Bean_Provision_Automatica
/// </summary>
public class Bean_Provision_Automatica
{
	public Bean_Provision_Automatica()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _tpr_prov_id = 0;
    public int tpr_prov_id
    {
        get { return _tpr_prov_id; }
        set { _tpr_prov_id = value; }
    }
    private int _tpr_proveedor_id = 0;
    public int tpr_proveedor_id
    {
        get { return _tpr_proveedor_id; }
        set { _tpr_proveedor_id = value; }
    }
    private string _tpr_fact_id = "";
    public string tpr_fact_id
    {
        get { return _tpr_fact_id; }
        set { _tpr_fact_id = value; }
    }
    private string _tpr_fact_fecha = "";
    public string tpr_fact_fecha
    {
        get { return _tpr_fact_fecha; }
        set { _tpr_fact_fecha = value; }
    }
    private string _tpr_fecha_maxpago = "";
    public string tpr_fecha_maxpago
    {
        get { return _tpr_fecha_maxpago; }
        set { _tpr_fecha_maxpago = value; }
    }
    private double _tpr_valor = 0;
    public double tpr_valor
    {
        get { return _tpr_valor; }
        set { _tpr_valor = value; }
    }
    private double _tpr_afecto = 0;
    public double tpr_afecto
    {
        get { return _tpr_afecto; }
        set { _tpr_afecto = value; }
    }
    private double _tpr_noafecto = 0;
    public double tpr_noafecto
    {
        get { return _tpr_noafecto; }
        set { _tpr_noafecto = value; }
    }
    private double _tpr_iva = 0;
    public double tpr_iva
    {
        get { return _tpr_iva; }
        set { _tpr_iva = value; }
    }
    private string _tpr_observacion = "";
    public string tpr_observacion
    {
        get { return _tpr_observacion; }
        set { _tpr_observacion = value; }
    }
    private int _tpr_suc_id = 0;
    public int tpr_suc_id
    {
        get { return _tpr_suc_id; }
        set { _tpr_suc_id = value; }
    }
    private int _tpr_pai_id = 0;
    public int tpr_pai_id
    {
        get { return _tpr_pai_id; }
        set { _tpr_pai_id = value; }
    }
    private string _tpr_usu_creacion = "";
    public string tpr_usu_creacion
    {
        get { return _tpr_usu_creacion; }
        set { _tpr_usu_creacion = value; }
    }
    private string _tpr_fecha_creacion = "";
    public string tpr_fecha_creacion
    {
        get { return _tpr_fecha_creacion; }
        set { _tpr_fecha_creacion = value; }
    }
    private string _tpr_usu_acepta = "";
    public string tpr_usu_acepta
    {
        get { return _tpr_usu_acepta; }
        set { _tpr_usu_acepta = value; }
    }
    private string _tpr_fecha_acepta = "";
    public string tpr_fecha_acepta
    {
        get { return _tpr_fecha_acepta; }
        set { _tpr_fecha_acepta = value; }
    }
    private int _tpr_departamento = 0;
    public int tpr_departamento
    {
        get { return _tpr_departamento; }
        set { _tpr_departamento = value; }
    }
    private int _tpr_ted_id = 0;
    public int tpr_ted_id
    {
        get { return _tpr_ted_id; }
        set { _tpr_ted_id = value; }
    }
    private string _tpr_serie = "";
    public string tpr_serie
    {
        get { return _tpr_serie; }
        set { _tpr_serie = value; }
    }
    private string _tpr_serie_oc = "";
    public string tpr_serie_oc
    {
        get { return _tpr_serie_oc; }
        set { _tpr_serie_oc = value; }
    }
    private int _tpr_correlativo_oc = 0;
    public int tpr_correlativo_oc
    {
        get { return _tpr_correlativo_oc; }
        set { _tpr_correlativo_oc = value; }
    }
    private int _tpr_tts_id = 0;
    public int tpr_tts_id
    {
        get { return _tpr_tts_id; }
        set { _tpr_tts_id = value; }
    }
    private string _tpr_hbl = "";
    public string tpr_hbl
    {
        get { return _tpr_hbl; }
        set { _tpr_hbl = value; }
    }
    private string _tpr_mbl = "";
    public string tpr_mbl
    {
        get { return _tpr_mbl; }
        set { _tpr_mbl = value; }
    }
    private string _tpr_routing = "";
    public string tpr_routing
    {
        get { return _tpr_routing; }
        set { _tpr_routing = value; }
    }
    private string _tpr_contenedor = "";
    public string tpr_contenedor
    {
        get { return _tpr_contenedor; }
        set { _tpr_contenedor = value; }
    }
    private int _tpr_tpi_id = 0;
    public int tpr_tpi_id
    {
        get { return _tpr_tpi_id; }
        set { _tpr_tpi_id = value; }
    }
    private int _tpr_correlativo = 0;
    public int tpr_correlativo
    {
        get { return _tpr_correlativo; }
        set { _tpr_correlativo = value; }
    }
    private int _tpr_mon_id = 0;
    public int tpr_mon_id
    {
        get { return _tpr_mon_id; }
        set { _tpr_mon_id = value; }
    }
    private string _tpr_serie_contrasena = "";
    public string tpr_serie_contrasena
    {
        get { return _tpr_serie_contrasena; }
        set { _tpr_serie_contrasena = value; }
    }
    private int _tpr_contrasena_correlativo = 0;
    public int tpr_contrasena_correlativo
    {
        get { return _tpr_contrasena_correlativo; }
        set { _tpr_contrasena_correlativo = value; }
    }
    private double _tpr_valor_equivalente = 0;
    public double tpr_valor_equivalente
    {
        get { return _tpr_valor_equivalente; }
        set { _tpr_valor_equivalente = value; }
    }
    private string _tpr_fact_corr = "";
    public string tpr_fact_corr
    {
        get { return _tpr_fact_corr; }
        set { _tpr_fact_corr = value; }
    }
    private string _tpr_proveedor_cajachica = "";
    public string tpr_proveedor_cajachica
    {
        get { return _tpr_proveedor_cajachica; }
        set { _tpr_proveedor_cajachica = value; }
    }
    private int _tpr_imp_exp_id = 0;
    public int tpr_imp_exp_id
    {
        get { return _tpr_imp_exp_id; }
        set { _tpr_imp_exp_id = value; }
    }
    private int _tpr_bien_serv = 0;
    public int tpr_bien_serv
    {
        get { return _tpr_bien_serv; }
        set { _tpr_bien_serv = value; }
    }
    private int _tpr_tcon_id = 0;
    public int tpr_tcon_id
    {
        get { return _tpr_tcon_id; }
        set { _tpr_tcon_id = value; }
    }
    private string _tpr_fecha_emision = "";
    public string tpr_fecha_emision
    {
        get { return _tpr_fecha_emision; }
        set { _tpr_fecha_emision = value; }
    }
    private string _tpr_nombre = "";
    public string tpr_nombre
    {
        get { return _tpr_nombre; }
        set { _tpr_nombre = value; }
    }
    private int _tpr_proveedor_cajachica_id = 0;
    public int tpr_proveedor_cajachica_id
    {
        get { return _tpr_proveedor_cajachica_id; }
        set { _tpr_proveedor_cajachica_id = value; }
    }
    private string _tpr_poliza = "";
    public string tpr_poliza
    {
        get { return _tpr_poliza; }
        set { _tpr_poliza = value; }
    }
    private bool _tpr_fiscal = true;
    public bool tpr_fiscal
    {
        get { return _tpr_fiscal; }
        set { _tpr_fiscal = value; }
    }
    private string _tpr_fecha_libro_compras = "";
    public string tpr_fecha_libro_compras
    {
        get { return _tpr_fecha_libro_compras; }
        set { _tpr_fecha_libro_compras = value; }
    }
    private int _tpr_tto_id = 8;
    public int tpr_tto_id
    {
        get { return _tpr_tto_id; }
        set { _tpr_tto_id = value; }
    }
    private string _tpr_ruta_pais = "";
    public string tpr_ruta_pais
    {
        get { return _tpr_ruta_pais; }
        set { _tpr_ruta_pais = value; }
    }
    private string _tpr_ruta = "";
    public string tpr_ruta
    {
        get { return _tpr_ruta; }
        set { _tpr_ruta = value; }
    }
    private int _tpr_blid = 0;
    public int tpr_blid
    {
        get { return _tpr_blid; }
        set { _tpr_blid = value; }
    }
    private int _tpr_tti_id = 0;
    public int tpr_tti_id
    {
        get { return _tpr_tti_id; }
        set { _tpr_tti_id = value; }
    }
    private string _tpr_usu_modifica_regimen = "";
    public string tpr_usu_modifica_regimen
    {
        get { return _tpr_usu_modifica_regimen; }
        set { _tpr_usu_modifica_regimen = value; }
    }
    private string _tpr_usu_anula = "";
    public string tpr_usu_anula
    {
        get { return _tpr_usu_anula; }
        set { _tpr_usu_anula = value; }
    }
    private string _tpr_fecha_anula = "";
    public string tpr_fecha_anula
    {
        get { return _tpr_fecha_anula; }
        set { _tpr_fecha_anula = value; }
    }
    private int _tpr_toc_id = 0;
    public int tpr_toc_id
    {
        get { return _tpr_toc_id; }
        set { _tpr_toc_id = value; }
    }
    private string _tpr_observacion_contrasena = "";
    public string tpr_observacion_contrasena
    {
        get { return _tpr_observacion_contrasena; }
        set { _tpr_observacion_contrasena = value; }
    }
    private string _tpr_fecha_recibo_factura = "";
    public string tpr_fecha_recibo_factura
    {
        get { return _tpr_fecha_recibo_factura; }
        set { _tpr_fecha_recibo_factura = value; }
    }
    private ArrayList _Arr_Detalle_Provision = new ArrayList();
    public ArrayList Arr_Detalle_Provision
    {
        get { return _Arr_Detalle_Provision; }
        set { _Arr_Detalle_Provision = value; }
    }
    private ArrayList _Arr_Costos_Provision = new ArrayList();
    public ArrayList Arr_Costos_Provision
    {
        get { return _Arr_Costos_Provision; }
        set { _Arr_Costos_Provision = value; }
    }
    private ArrayList arreglo1 = new ArrayList();
    public ArrayList arr1
    {
        get { return arreglo1; }
        set { arreglo1 = value; }
    }
    private ArrayList arreglo2 = new ArrayList();
    public ArrayList arr2
    {
        get { return arreglo2; }
        set { arreglo2 = value; }
    }
    private ArrayList arreglo3 = new ArrayList();
    public ArrayList arr3
    {
        get { return arreglo3; }
        set { arreglo3 = value; }
    }
    private int _tpr_ttd_id = 1;
    public int tpr_ttd_id
    {
        get { return _tpr_ttd_id; }
        set { _tpr_ttd_id = value; }
    }
    private int _tpr_ttt_id = 0;
    public int tpr_ttt_id
    {
        get { return _tpr_ttt_id; }
        set { _tpr_ttt_id = value; }
    }
    private int _tpr_tds_id = 0;
    public int tpr_tds_id
    {
        get { return _tpr_tds_id; }
        set { _tpr_tds_id = value; }
    }
    private bool _tpr_mbl_modificado = true;
    public bool tpr_mbl_modificado
    {
        get { return _tpr_mbl_modificado; }
        set { _tpr_mbl_modificado = value; }
    }
    private ArrayList cuentas_cargo = null;
    public ArrayList ctas_cargo
    {
        get { return cuentas_cargo; }
        set { cuentas_cargo = value; }
    }
    private string _tpr_ip_address = "0.0.0.0";
    public string tpr_ip_address
    {
        get { return _tpr_ip_address; }
        set { _tpr_ip_address = value; }
    }
    private string _tpr_poliza_seguro = "";
    public string tpr_poliza_seguro
    {
        get { return _tpr_poliza_seguro; }
        set { _tpr_poliza_seguro = value; }
    }
}