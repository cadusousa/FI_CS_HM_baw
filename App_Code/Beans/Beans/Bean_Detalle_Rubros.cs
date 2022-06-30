using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

/// <summary>
/// Summary description for Bean_Detalle_Rubros
/// </summary>
public class Bean_Detalle_Rubros
{
	public Bean_Detalle_Rubros()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _tdf_id = 0;
    public int tdf_id
    {
        get { return _tdf_id; }
        set { _tdf_id = value; }
    }
    private int _tdf_rub_id = 0;
    public int tdf_rub_id
    {
        get { return _tdf_rub_id; }
        set { _tdf_rub_id = value; }
    }
    private double _tdf_montosinimpuesto = 0;
    public double tdf_montosinimpuesto
    {
        get { return _tdf_montosinimpuesto; }
        set { _tdf_montosinimpuesto = value; }
    }
    private double _tdf_impuesto = 0;
    public double tdf_impuesto
    {
        get { return _tdf_impuesto; }
        set { _tdf_impuesto = value; }
    }
    private double _tdf_monto = 0;
    public double tdf_monto
    {
        get { return _tdf_monto; }
        set { _tdf_monto = value; }
    }
    private int _tdf_tfa_id = 0;
    public int tdf_tfa_id
    {
        get { return _tdf_tfa_id; }
        set { _tdf_tfa_id = value; }
    }
    private int _tdf_ttr_id = 0;
    public int tdf_ttr_id
    {
        get { return _tdf_ttr_id; }
        set { _tdf_ttr_id = value; }
    }
    private int _tdf_tts_id = 0;
    public int tdf_tts_id
    {
        get { return _tdf_tts_id; }
        set { _tdf_tts_id = value; }
    }
    private double _tdf_total_equivalente = 0;
    public double tdf_total_equivalente
    {
        get { return _tdf_total_equivalente; }
        set { _tdf_total_equivalente = value; }
    }
    private int _tdf_ttm_id = 0;
    public int tdf_ttm_id
    {
        get { return _tdf_ttm_id; }
        set { _tdf_ttm_id = value; }
    }
    private string _tdf_comentarios = "";
    public string tdf_comentarios
    {
        get { return _tdf_comentarios; }
        set { _tdf_comentarios = value; }
    }
    private int _tdf_cargo_id = 0;
    public int tdf_cargo_id
    {
        get { return _tdf_cargo_id; }
        set { _tdf_cargo_id = value; }
    }
    private int _tdf_costo_id = 0;
    public int tdf_costo_id
    {
        get { return _tdf_costo_id; }
        set { _tdf_costo_id = value; }
    }
    private double _tdf_montosinimpuesto_equivalente = 0;
    public double tdf_montosinimpuesto_equivalente
    {
        get { return _tdf_montosinimpuesto_equivalente; }
        set { _tdf_montosinimpuesto_equivalente = value; }
    }
    private double _tdf_impuesto_equivalente = 0;
    public double tdf_impuesto_equivalente
    {
        get { return _tdf_impuesto_equivalente; }
        set { _tdf_impuesto_equivalente = value; }
    }
    private ArrayList cuentas_debe = null;
    public ArrayList cta_debe
    {
        get { return cuentas_debe; }
        set { cuentas_debe = value; }
    }
    private ArrayList cuentas_haber = null;
    public ArrayList cta_haber
    {
        get { return cuentas_haber; }
        set { cuentas_haber = value; }
    }
}