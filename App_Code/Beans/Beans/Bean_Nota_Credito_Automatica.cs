using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bean_Nota_Credito_Automatica
/// </summary>
public class Bean_Nota_Credito_Automatica
{
	public Bean_Nota_Credito_Automatica()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _tnc_id = 0;
    public int tnc_id
    {
        get { return _tnc_id; }
        set { _tnc_id = value; }
    }
    private string _tnc_usu_id = "";
    public string tnc_usu_id
    {
        get { return _tnc_usu_id; }
        set { _tnc_usu_id = value; }
    }
    private int _tnc_pai_id = 0;
    public int tnc_pai_id
    {
        get { return _tnc_pai_id; }
        set { _tnc_pai_id = value; }
    }
    private string _tnc_fecha = "";
    public string tnc_fecha
    {
        get { return _tnc_fecha; }
        set { _tnc_fecha = value; }
    }
    private string _tnc_hora = "";
    public string tnc_hora
    {
        get { return _tnc_hora; }
        set { _tnc_hora = value; }
    }
    private int _tnc_suc_id = 0;
    public int tnc_suc_id
    {
        get { return _tnc_suc_id; }
        set { _tnc_suc_id = value; }
    }
    private int _tnc_ted_id = 0;
    public int tnc_ted_id
    {
        get { return _tnc_ted_id; }
        set { _tnc_ted_id = value; }
    }
    private double _tnc_monto = 0;
    public double tnc_monto
    {
        get { return _tnc_monto; }
        set { _tnc_monto = value; }
    }
    private string _tnc_observaciones = "";
    public string tnc_observaciones
    {
        get { return _tnc_observaciones; }
        set { _tnc_observaciones = value; }
    }
    private double _tnc_cli_id = 0;
    public double tnc_cli_id
    {
        get { return _tnc_cli_id; }
        set { _tnc_cli_id = value; }
    }
    private double _tnc_monto_equivalente = 0;
    public double tnc_monto_equivalente
    {
        get { return _tnc_monto_equivalente; }
        set { _tnc_monto_equivalente = value; }
    }
    private int _tnc_mon_id = 0;
    public int tnc_mon_id
    {
        get { return _tnc_mon_id; }
        set { _tnc_mon_id = value; }
    }
    private string _tnc_serie = "";
    public string tnc_serie
    {
        get { return _tnc_serie; }
        set { _tnc_serie = value; }
    }
    private int _tnc_correlativo = 0;
    public int tnc_correlativo
    {
        get { return _tnc_correlativo; }
        set { _tnc_correlativo = value; }
    }
    private int _tnc_tpi_id = 0;
    public int tnc_tpi_id
    {
        get { return _tnc_tpi_id; }
        set { _tnc_tpi_id = value; }
    }
    private int _tnc_tcon_id = 0;
    public int tnc_tcon_id
    {
        get { return _tnc_tcon_id; }
        set { _tnc_tcon_id = value; }
    }
    private int _tnc_ttr_id = 0;
    public int tnc_ttr_id
    {
        get { return _tnc_ttr_id; }
        set { _tnc_ttr_id = value; }
    }
    private string _tnc_hbl = "";
    public string tnc_hbl
    {
        get { return _tnc_hbl; }
        set { _tnc_hbl = value; }
    }
    private string _tnc_mbl = "";
    public string tnc_mbl
    {
        get { return _tnc_mbl; }
        set { _tnc_mbl = value; }
    }
    private string _tnc_routing = "";
    public string tnc_routing
    {
        get { return _tnc_routing; }
        set { _tnc_routing = value; }
    }
    private string _tnc_contenedor = "";
    public string tnc_contenedor
    {
        get { return _tnc_contenedor; }
        set { _tnc_contenedor = value; }
    }
    private string _tnc_fecha_emision = "";
    public string tnc_fecha_emision
    {
        get { return _tnc_fecha_emision; }
        set { _tnc_fecha_emision = value; }
    }
    private string _tnc_nombre = "";
    public string tnc_nombre
    {
        get { return _tnc_nombre; }
        set { _tnc_nombre = value; }
    }
    private string _tnc_referencia = "";
    public string tnc_referencia
    {
        get { return _tnc_referencia; }
        set { _tnc_referencia = value; }
    }
    private string _tnc_nit = "";
    public string tnc_nit
    {
        get { return _tnc_nit; }
        set { _tnc_nit = value; }
    }
    private string _tnc_poliza = "";
    public string tnc_poliza
    {
        get { return _tnc_poliza; }
        set { _tnc_poliza = value; }
    }
    private bool _tnc_fiscal = true;
    public bool tnc_fiscal
    {
        get { return _tnc_fiscal; }
        set { _tnc_fiscal = value; }
    }
    private string _tnc_fecha_libro_compras = "";
    public string tnc_fecha_libro_compras
    {
        get { return _tnc_fecha_libro_compras; }
        set { _tnc_fecha_libro_compras = value; }
    }
    private double _tnc_montosinimpuesto = 0;
    public double tnc_montosinimpuesto
    {
        get { return _tnc_montosinimpuesto; }
        set { _tnc_montosinimpuesto = value; }
    }
    private double _tnc_impuesto = 0;
    public double tnc_impuesto
    {
        get { return _tnc_impuesto; }
        set { _tnc_impuesto = value; }
    }
    private string _tnc_esignature = "-";
    public string tnc_esignature
    {
        get { return _tnc_esignature; }
        set { _tnc_esignature = value; }
    }
    private int _tnc_fac_electronica = 0;
    public int tnc_fac_electronica
    {
        get { return _tnc_fac_electronica; }
        set { _tnc_fac_electronica = value; }
    }
    private string _tnc_internal_reference = "0";
    public string tnc_internal_reference
    {
        get { return _tnc_internal_reference; }
        set { _tnc_internal_reference = value; }
    }
    private string _tnc_guid = "0";
    public string tnc_guid
    {
        get { return _tnc_guid; }
        set { _tnc_guid = value; }
    }
    private string _tnc_correo_documento_electronico = "-";
    public string tnc_correo_documento_electronico
    {
        get { return _tnc_correo_documento_electronico; }
        set { _tnc_correo_documento_electronico = value; }
    }
    private string _tnc_referencia_correo = "-";
    public string tnc_referencia_correo
    {
        get { return _tnc_referencia_correo; }
        set { _tnc_referencia_correo = value; }
    }
    private string _tnc_innerxml = "";
    public string tnc_innerxml
    {
        get { return _tnc_innerxml; }
        set { _tnc_innerxml = value; }
    }
    private string _tnc_fecha_batch = "";
    public string tnc_fecha_batch
    {
        get { return _tnc_fecha_batch; }
        set { _tnc_fecha_batch = value; }
    }
}